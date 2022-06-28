using LibraryAPI.CQRS;
using LibraryAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    public class BaseLibraryAPIController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public BaseLibraryAPIController(IServiceProvider serviceProvider) : base()
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> HandleQueryAsync<TContract, TResult>(object query, CancellationToken cancellationToken = default)
            where TContract : class
        {
            var validator = serviceProvider.GetService(typeof(ICQValidator<TContract>));
            var vResult = await ((ICQValidator<TContract>)validator!).Validate((TContract)query, new LibraryContext(cancellationToken));
            if (!vResult.IsValid)
            {
                return StatusCode(
                    (int)HttpStatusCode.UnprocessableEntity,
                    vResult.Errors.Select(e => e.ErrorCode).ToList());
            }

            var handler = serviceProvider.GetService(typeof(IQueryHandler<TContract, TResult>));
            var result = await ((IQueryHandler<TContract, TResult>)handler!)
                .ExecuteAsync((TContract)query, new LibraryContext(cancellationToken));

            return Ok(result);
        }

        public async Task<IActionResult> HandleCommandAsync<TContract>(object command, CancellationToken cancellationToken = default)
            where TContract : class
        {
            var validator = serviceProvider.GetService(typeof(ICQValidator<TContract>));
            var vResult = await ((ICQValidator<TContract>)validator!).Validate((TContract)command, new LibraryContext(cancellationToken));
            if (!vResult.IsValid)
            {
                return StatusCode(
                    (int)HttpStatusCode.UnprocessableEntity,
                    vResult.Errors.Select(e => e.ErrorCode).ToList());
            }

            var handler = serviceProvider.GetService(typeof(ICommandHandler<TContract>));
            await ((ICommandHandler<TContract>)handler!).ExecuteAsync((TContract)command, new LibraryContext(cancellationToken));

            return Ok();
        }
    }
}