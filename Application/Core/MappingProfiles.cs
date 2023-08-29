using API.DTOs;
using Application.Books;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, Book>();
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>()
                .ForMember(
                    b => b.Authors, 
                    o =>o.MapFrom(
                        s => s.BookAuthors.Select(e => e.Author)));
        }
    }
}