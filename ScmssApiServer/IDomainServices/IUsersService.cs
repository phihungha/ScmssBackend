﻿using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        Task<UserDto> CreateAsync(UserCreateDto dto);

        FileUploadInfoDto GenerateProfileImageUploadUrl();

        Task<UserDto?> GetAsync(string id);

        Task<IList<UserDto>> GetManyAsync(SimpleQueryDto dto);

        Task<UserDto> UpdateAsync(string id, UserInputDto dto);
    }
}
