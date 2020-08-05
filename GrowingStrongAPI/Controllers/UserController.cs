using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using AutoMapper;
using Dapper;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Services;
using GrowingStrongAPI.Helpers.Extensions;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly ILogger _logger;

        public UserController(IUserService userService,
                              IMapper mapper,
                              ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            AuthenticateUserResponse response = _userService.Authenticate(authenticateModel.EmailAddress, authenticateModel.Password);

            if (!response.ResponseStatus.HasError())
            {
                return Ok(new
                {
                    User = response.UserDto,
                    response.Token
                });
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            GetUserByIdResponse response = _userService.GetUserById(id);

            if (!response.ResponseStatus.HasError())
            {
                return Ok(new
                {
                    User = response.UserDto
                }); ;
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpGet("{id}/foodEntries")]
        public IActionResult GetUserFoodEntries(int id)
        {
            GetUserFoodEntriesResponse response = _userService.GetUserFoodEntries(id);

            if (!response.ResponseStatus.HasError())
            {
                return Ok(response.FoodEntryDtos);
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserDetails(int id, [FromBody] UserDetailsUpdateModel updateModel)
        {
            updateModel.UserId = id;

            UpdateUserDetailsResponse response = _userService.UpdateUserDetails(updateModel);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpPut("{id}/profile")]
        public IActionResult UpdateUserProfile(int id, [FromBody] UserProfile profile)
        {
            profile.UserId = id;

            UpdateUserProfileResponse response = _userService.UpdateUserProfile(profile);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpPut("{id}/targets")]
        public IActionResult UpdateUserTargets(int id, [FromBody] UserTargets targets)
        {
            targets.UserId = id;

            UpdateUserTargetsResponse response = _userService.UpdateUserTargets(targets);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegistrationModel registrationModel)
        {
            RegisterUserResponse registerResponse = _userService.Register(registrationModel);

            if (registerResponse.ResponseStatus.HasError())
            {
                return StatusCode(registerResponse.ResponseStatus.Status, registerResponse.ResponseStatus.Message);
            }

            AuthenticateUserResponse authenticateResponse = _userService.Authenticate(registrationModel.EmailAddress,
                                                                                      registrationModel.Password);

            if (authenticateResponse.ResponseStatus.HasError())
            {
                return StatusCode(authenticateResponse.ResponseStatus.Status, authenticateResponse.ResponseStatus.Message);
            }

            return Ok(new
            {
                User = registerResponse.userDto,
                authenticateResponse.Token
            });;
        }
    }
}
