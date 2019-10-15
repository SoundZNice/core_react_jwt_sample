using System;
using AutoMapper;
using Core3.Application.Interfaces.Mapping;
using Core3.Domain.Entities;

namespace Core3.Application.Models.Notes
{
    public class NoteViewModel : IHaveCustomMapping
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Note, NoteViewModel>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(n => n.Id))
                .ForMember(dto => dto.Text, opt => opt.MapFrom(n => n.Text))
                .ForMember(dto => dto.DateCreated, opt => opt.MapFrom(n => n.DateCreated))
                .ForMember(dto => dto.DateModified, opt => opt.MapFrom(n => n.DateModified));
        }
    }
}
