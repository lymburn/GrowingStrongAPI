using Moq;
using GrowingStrongAPI.Services;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Collections.Generic;

namespace GrowingStrongAPI.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        IUserService userService;
        Mock<IUserRepository> mockUserRepository;
        Mock<IAuthenticationHelper> mockAuthenticationHelper;
        Mock<ILogger<IUserService>> mockLogger;
        Mock<IMapper> mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockAuthenticationHelper = new Mock<IAuthenticationHelper>();
            mockLogger = new Mock<ILogger<IUserService>>();
            mockMapper = new Mock<IMapper>();

            userService = new UserService(mockUserRepository.Object,
                                          mockMapper.Object,
                                          mockLogger.Object,
                                          mockAuthenticationHelper.Object);
        }

        [TestMethod]
        public void TestAuthenticateInvalidCredentials()
        {
            string email = "";
            string password = "Password1";

            UserDto userDto = userService.Authenticate(email, password);
            Assert.IsNull(userDto);

            email = null;
            userDto = userService.Authenticate(email, password);
            Assert.IsNull(userDto);

            email = "test123@gmail.com";
            password = "";

            userDto = userService.Authenticate(email, password);
            Assert.IsNull(userDto);

            password = null;
            userDto = userService.Authenticate(email, password);
            Assert.IsNull(userDto);
        }

        [TestMethod]
        public void TestAuthenticateFailToRetrieveUserByEmail()
        {
            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns<IEnumerable<User>>(null);

            userService = new UserService(mockUserRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            UserDto userDto = userService.Authenticate(email, password);

            Assert.IsNull(userDto);
        }

        [TestMethod]
        public void TestAuthenticateFailToVerifyPasswordHash()
        {
            User user = new User();
            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns(user);

            mockAuthenticationHelper.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(),
                                                                     It.IsAny<byte[]>(),
                                                                     It.IsAny<byte[]>()))
                                                                    .Returns(false);

            userService = new UserService(mockUserRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            UserDto userDto = userService.Authenticate(email, password);

            Assert.IsNull(userDto);
        }

        [TestMethod]
        public void TestAuthenticateSuccess()
        {
            string email = "test123@gmail.com";
            string password = "Password1";
            User user = new User();
            UserDto expectedUserDto = new UserDto();
            user.EmailAddress = email;
            expectedUserDto.EmailAddress = email;

            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns(user);

            mockAuthenticationHelper.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(),
                                                                     It.IsAny<byte[]>(),
                                                                     It.IsAny<byte[]>()))
                                                                    .Returns(true);

            mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(expectedUserDto);

            userService = new UserService(mockUserRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object);


            UserDto userDto = userService.Authenticate(email, password);

            Assert.IsNotNull(userDto);
            Assert.AreEqual(expectedUserDto.EmailAddress, userDto.EmailAddress);
        }
    }
}
