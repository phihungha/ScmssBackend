﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : CustomControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserDto>>> Get()
        {
            IList<User> users = await _usersService.GetUsersAsync();
            IList<UserDto> userDtos = users.Select(ToUserGetDto).ToList();
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetId(string id)
        {
            User? user = await _usersService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserDto userDto = ToUserGetDto(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] UserCreateDto body)
        {
            try
            {
                User newUser = await _usersService.CreateUserAsync(body);
                UserDto newUserDto = ToUserGetDto(newUser);
                return Ok(newUserDto);
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<User>> Update(string id, [FromBody] UserUpdateDto body)
        {
            try
            {
                User updatedUser = await _usersService.UpdateUserAsync(id, body);
                UserDto updatedUserDto = ToUserGetDto(updatedUser);
                return Ok(updatedUserDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            try
            {
                await _usersService.DeleteUserAsync(id);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/profileImageUploadUrl")]
        public string GetProfileImageUploadUrl(string id)
        {
            return _usersService.GetProfileImageUploadUrl(id);
        }

        // TODO: Use automapping later
        private UserDto ToUserGetDto(User user)
        {
            Position position = user.Position;
            var positionDto = new PositionDto
            {
                Id = position.Id,
                Name = position.Name,
                Description = position.Description,
                IsActive = position.IsActive,
                CreatedTime = position.CreatedTime,
                UpdatedTime = position.UpdatedTime,
            };

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                IdCardNumber = user.IdCardNumber,
                Address = user.Address,
                Description = user.Description,
                Position = positionDto,
                IsActive = user.IsActive,
                CreatedTime = user.CreatedTime,
                UpdatedTime = user.UpdatedTime
            };
        }
    }
}