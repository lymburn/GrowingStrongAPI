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
using System;

namespace GrowingStrongAPI.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        IUserService userService;
        Mock<IUserRepository> mockUserRepository;
        Mock<IFoodEntryRepository> mockFoodEntryRepository;
        Mock<IAuthenticationHelper> mockAuthenticationHelper;
        Mock<ILogger<IUserService>> mockLogger;
        Mock<IMapper> mockMapper;
        Mock<IJwtHelper> mockJwtHelper;
        Mock<IUserProfileCalculator> mockUserProfileCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockFoodEntryRepository = new Mock<IFoodEntryRepository>();
            mockAuthenticationHelper = new Mock<IAuthenticationHelper>();
            mockLogger = new Mock<ILogger<IUserService>>();
            mockMapper = new Mock<IMapper>();
            mockJwtHelper = new Mock<IJwtHelper>();
            mockUserProfileCalculator = new Mock<IUserProfileCalculator>();

            userService = new UserService(mockUserRepository.Object,
                                          mockFoodEntryRepository.Object,
                                          mockMapper.Object,
                                          mockLogger.Object,
                                          mockAuthenticationHelper.Object,
                                          mockJwtHelper.Object,
                                          mockUserProfileCalculator.Object);
        }

        [TestMethod]
        public void TestAuthenticateInvalidCredentials()
        {
            string email = "";
            string password = "Password1";

            AuthenticateUserResponse response = userService.Authenticate(email, password);
            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.BAD_REQUEST);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);

            email = null;
            response = userService.Authenticate(email, password);
            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.BAD_REQUEST);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);

            email = "test123@gmail.com";
            password = "";
            response = userService.Authenticate(email, password);
            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.BAD_REQUEST);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);

            password = null;
            response = userService.Authenticate(email, password);
            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.BAD_REQUEST);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);
        }

        [TestMethod]
        public void TestAuthenticateFailToRetrieveUserByEmail()
        {
            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns<IEnumerable<User>>(null);

            userService = new UserService(mockUserRepository.Object,
                                                           mockFoodEntryRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object,
                                                           mockJwtHelper.Object,
                                                           mockUserProfileCalculator.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            AuthenticateUserResponse response = userService.Authenticate(email, password);

            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.UNAUTHORIZED);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);
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
                                                           mockFoodEntryRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object,
                                                           mockJwtHelper.Object,
                                                           mockUserProfileCalculator.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            AuthenticateUserResponse response = userService.Authenticate(email, password);

            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.UNAUTHORIZED);
            Assert.AreEqual(response.ResponseStatus.Message, Constants.AuthenticateUserMessages.InvalidCredentials);
        }

        [TestMethod]
        public void TestAuthenticateInvalidPasswordHashLength()
        {
            User user = new User();
            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns(user);

            ArgumentException exception = new ArgumentException(Constants.AuthenticationHelperExceptions.InvalidPasswordHashLength);
            mockAuthenticationHelper.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(),
                                                                     It.IsAny<byte[]>(),
                                                                     It.IsAny<byte[]>()))
                                                                    .Throws(exception);

            userService = new UserService(mockUserRepository.Object,
                                                           mockFoodEntryRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object,
                                                           mockJwtHelper.Object,
                                                           mockUserProfileCalculator.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            AuthenticateUserResponse response = userService.Authenticate(email, password);

            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.INTERNAL_SERVER_ERROR);
            Assert.IsTrue(response.ResponseStatus.Message.Contains(exception.Message));
        }

        [TestMethod]
        public void TestAuthenticateInvalidPasswordSaltLength()
        {
            User user = new User();
            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns(user);

            ArgumentException exception = new ArgumentException(Constants.AuthenticationHelperExceptions.InvalidPasswordSaltLength);
            mockAuthenticationHelper.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(),
                                                                     It.IsAny<byte[]>(),
                                                                     It.IsAny<byte[]>()))
                                                                    .Throws(exception);

            userService = new UserService(mockUserRepository.Object,
                                                          mockFoodEntryRepository.Object,
                                                          mockMapper.Object,
                                                          mockLogger.Object,
                                                          mockAuthenticationHelper.Object,
                                                          mockJwtHelper.Object,
                                                          mockUserProfileCalculator.Object);

            string email = "test123@gmail.com";
            string password = "Password1";
            AuthenticateUserResponse response = userService.Authenticate(email, password);

            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.INTERNAL_SERVER_ERROR);
            Assert.IsTrue(response.ResponseStatus.Message.Contains(exception.Message));
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
            expectedUserDto.UserId = 1;

            mockUserRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                              .Returns(user);

            mockAuthenticationHelper.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(),
                                                                     It.IsAny<byte[]>(),
                                                                     It.IsAny<byte[]>()))
                                                                    .Returns(true);

            mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(expectedUserDto);

            mockJwtHelper.Setup(m => m.GenerateJWT(It.IsAny<int>(), It.IsAny<string>())).Returns("Token");

            userService = new UserService(mockUserRepository.Object,
                                                           mockFoodEntryRepository.Object,
                                                           mockMapper.Object,
                                                           mockLogger.Object,
                                                           mockAuthenticationHelper.Object,
                                                           mockJwtHelper.Object,
                                                           mockUserProfileCalculator.Object);


            AuthenticateUserResponse response = userService.Authenticate(email, password);

            Assert.AreEqual(response.ResponseStatus.Status, ResponseStatusCode.OK);

            Assert.AreEqual(response.UserDto.EmailAddress, expectedUserDto.EmailAddress);
            Assert.AreEqual(response.UserDto.UserId, expectedUserDto.UserId);
        }
    }
}
