using System;
using GrowingStrongAPI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrowingStrongAPI.Tests.Helpers
{
    [TestClass]
    public class AuthenticationHelperTests
    {
        IAuthenticationHelper authenticationHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            authenticationHelper = new AuthenticationHelper();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatePasswordHashNullPassword()
        {
            byte[] passwordHash, passwordSalt;
            string nullPassword = null;
            authenticationHelper.CreatePasswordHash(nullPassword, out passwordHash, out passwordSalt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePasswordHashEmptyPassword()
        {
            byte[] passwordHash, passwordSalt;
            string emptyPassword = "";
            authenticationHelper.CreatePasswordHash(emptyPassword, out passwordHash, out passwordSalt);
        }

        [TestMethod]
        public void TestCreatePasswordHashSuccessful()
        {
            byte[] passwordHash, passwordSalt;
            string password = "Password1";
            authenticationHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            Assert.IsNotNull(passwordHash);
            Assert.IsNotNull(passwordSalt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestVerifyPasswordHashNullPassword()
        {
            byte[] passwordHash = new byte[0];
            byte[] passwordSalt = new byte[0];
            string nullPassword = null;
            authenticationHelper.VerifyPasswordHash(nullPassword, passwordHash, passwordSalt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestVerifyPasswordHashEmptyPassword()
        {
            byte[] passwordHash = new byte[0];
            byte[] passwordSalt = new byte[0];
            string emptyPassword = "";
            authenticationHelper.VerifyPasswordHash(emptyPassword, passwordHash, passwordSalt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestVerifyPasswordInvalidHashLength()
        {
            byte[] passwordHash = new byte[100];
            byte[] passwordSalt = new byte[128];
            string password = "Password1";
            authenticationHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestVerifyPasswordInvalidSaltLength()
        {
            byte[] passwordHash = new byte[64];
            byte[] passwordSalt = new byte[100];
            string password = "Password1";
            authenticationHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
        }

        [TestMethod]
        public void TestVerifyPasswordFalse()
        {
            byte[] passwordHash = new byte[64];
            byte[] passwordSalt = new byte[128];
            string password = "Password1";
            bool matches = authenticationHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

            Assert.IsFalse(matches);
        }
    }
}
