﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class User : IdentityUser, IUpdateTrackable
    {
        [PersonalData]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        [PersonalData]
        public required Gender Gender { get; set; }

        [PersonalData]
        public required DateTime DateOfBirth { get; set; }

        [PersonalData]
        [StringLength(maximumLength: 12, MinimumLength = 12)]
        [Column(TypeName = "char(12)")]
        public required string IdCardNumber { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        public string? Description { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>().ForAllMembers(
                    o => o.Condition((src, dest, srcMember) => srcMember != null)
                );
        }
    }
}
