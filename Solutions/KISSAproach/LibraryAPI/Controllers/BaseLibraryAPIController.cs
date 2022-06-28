using FluentValidation.Results;
using LibraryAPI.DataAccess;
using LibraryAPI.Validators.Common;
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

        public ObjectResult ValidateRequest<TContract>(object request, CancellationToken cancellationToken = default)
            where TContract : class
        {
            var validator = serviceProvider.GetService(typeof(IRequestValidator<TContract>));

            ValidationResult vResult = new();
            Task.Run(async () =>
            {
                vResult = await ((IRequestValidator<TContract>)validator!)
                    .Validate((TContract)request, new LibraryContext(cancellationToken));
            }, cancellationToken)
                .Wait(cancellationToken);

            if (!vResult.IsValid)
            {
                return StatusCode(
                    (int)HttpStatusCode.UnprocessableEntity,
                    vResult.Errors.Select(e => e.ErrorCode).ToList());
            }

            return null;
        }
    }
}