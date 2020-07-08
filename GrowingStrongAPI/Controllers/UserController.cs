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
using GrowingStrongAPI.Helpers;

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

            if (response.ResponseStatus.Message.Equals(Constants.AuthenticateUserMessages.InvalidCredentials))
            {
                return Unauthorized(Constants.AuthenticateUserMessages.InvalidCredentials);
            }
            else if (response.ResponseStatus.Message.Equals(Constants.AuthenticateUserMessages.FailedToGenerateJWT))
            {
                return StatusCode(500,Constants.AuthenticateUserMessages.FailedToGenerateJWT);
            }
            else if (response.ResponseStatus.Message.Equals(Constants.AuthenticateUserMessages.InvalidPasswordHashOrSaltLength))
            {
                return StatusCode(500, Constants.AuthenticateUserMessages.InvalidPasswordHashOrSaltLength);
            }
            else
            {
                return Ok(new
                {
                    response.Token
                });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> users = _userService.GetAll().AsList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            UserDto user = _userService.GetById(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegistrationModel registrationModel)
        {
            User user = _mapper.Map<User>(registrationModel);

            CreateUserResponse response = _userService.Create(user, registrationModel.Password);

            if (response.ResponseStatus.Message.Equals(Constants.CreateUserMessages.NullOrEmptyCredentials))
            {
                return BadRequest(Constants.CreateUserMessages.NullOrEmptyCredentials);
            }
            else if (response.ResponseStatus.Message.Equals(Constants.CreateUserMessages.UserAlreadyExists))
            {
                return StatusCode(409, Constants.CreateUserMessages.UserAlreadyExists);
            }
            else if (response.ResponseStatus.Message.Equals(Constants.CreateUserMessages.FailedToCreatePasswordHash))
            {
                return StatusCode(500, Constants.CreateUserMessages.FailedToCreatePasswordHash);
            }
            else if (response.ResponseStatus.Message.Equals(Constants.CreateUserMessages.FailedToCreateUser))
            {
                return StatusCode(500, Constants.CreateUserMessages.FailedToCreateUser);
            }
            else
            {
                return Ok(new
                {
                    User = response.userDto
                });
            } 
        }
    }
}
