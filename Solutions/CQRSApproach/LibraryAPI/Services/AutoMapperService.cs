using AutoMapper;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.Domain.Authors;
using LibraryAPI.Domain.Books;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Services
{
    public static class AutoMapperService
    {
        public static IMapper Initialize(ILoggerFactory loggerFactory)
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>()
                    .ForMember(x => x.Name, y => y.MapFrom(z => $"{z.FirstName}{(z.FirstName == null ? "" : " ")}{z.LastName}"));
                cfg.CreateMap<Book, BookDetailsDto>()
                    .ForMember(x => x.CurrentStatus, y => y.MapFrom(z => z.CurrentStatus.Status))
                    .ForMember(x => x.IsPolish, y => y.MapFrom(z => z.Language != null && (z.Language == "polski" || z.Language == "Polski")));
            }, loggerFactory)
            .CreateMapper();
    }
}