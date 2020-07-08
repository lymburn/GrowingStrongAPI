using System;
using GrowingStrongAPI.Helpers.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrowingStrongAPI.Tests.Helpers.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void TestToSnakeCaseEmptyOrNull()
        {
            string str = "";
            str = str.ToSnakeCase();

            Assert.AreEqual(str, "");

            str = null;
            str = str.ToSnakeCase();

            Assert.IsNull(str);
        }

        [TestMethod]
        public void TestToSnakeCase()
        {
            string str = "NonSnakeCase";
            string snakeStr = "non_snake_case";
            str = str.ToSnakeCase();

            Assert.AreEqual(str, snakeStr);

        }
    }
}
