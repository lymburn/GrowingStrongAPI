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

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> users = _userService.GetAll().AsList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            UserDto user = _userService.GetById(id);
            return Ok(user);
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

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegistrationModel registrationModel)
        {
            User user = _mapper.Map<User>(registrationModel);

            CreateUserResponse createResponse = _userService.Create(user, registrationModel.Password);

            if (createResponse.ResponseStatus.HasError())
            {
                return StatusCode(createResponse.ResponseStatus.Status, createResponse.ResponseStatus.Message);
            }

            AuthenticateUserResponse authenticateResponse = _userService.Authenticate(user.EmailAddress, registrationModel.Password);

            if (authenticateResponse.ResponseStatus.HasError())
            {
                return StatusCode(authenticateResponse.ResponseStatus.Status, authenticateResponse.ResponseStatus.Message);
            }

            return Ok(new
            {
                User = createResponse.userDto,
                authenticateResponse.Token
            });
        }
    }
}
