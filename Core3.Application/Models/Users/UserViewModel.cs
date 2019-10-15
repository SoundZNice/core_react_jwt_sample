using System;
using AutoMapper;
using Core3.Application.Interfaces.Mapping;
using Core3.Domain.Entities;

namespace Core3.Application.Models.Users
{
    public class UserViewModel : IHaveCustomMapping
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        public string SerialNumber { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<User, UserViewModel>();
        }
    }
}
