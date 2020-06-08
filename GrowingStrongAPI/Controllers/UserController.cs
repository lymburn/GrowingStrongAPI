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
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public UserController(IUserService userService,
                              IMapper mapper,
                              IOptions<AppSettings> appSettings,
                              ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            UserDto user = _userService.Authenticate(authenticateModel.EmailAddress, authenticateModel.Password);

            if (user is null)
            {
                return BadRequest("Invalid username or password");
            }

            string tokenString = JwtHelper.GenerateJWT(user.Id, _appSettings.JWTSecret);

            if (string.IsNullOrEmpty(tokenString))
            {
                return StatusCode(500, "Unable to generate JWT string");
            }

            return Ok(new
            {
                user,
                Token = tokenString
            });
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

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegistrationModel registrationModel)
        {
            User user = _mapper.Map<User>(registrationModel);

            _userService.Create(user, registrationModel.Password);

            return Ok();  
        }
    }
}
