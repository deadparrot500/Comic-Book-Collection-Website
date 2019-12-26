using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Providers.Tests
{
    [TestClass]
    public class HashProviderTests
    {
        [TestMethod]        
        public void HashProvider_ReturnsHashedPassword()
        {
            HashProvider hashProvider = new HashProvider();

            var hashedPassword = hashProvider.HashPassword("password123");

            Assert.IsNotNull(hashedPassword.Salt);
            Assert.AreNotEqual(hashedPassword.Password, "password123");
        }

        [DataTestMethod]
        [DataRow("password123", "rsD3TaOu0XQ=", "OZpnzNCj1mcK3lvPKxhh89ikT0w=")]
        public void HashProvider_ReturnsPasswordMatch(string password, string salt, string hashedPassword)
        {
            HashProvider hashProvider = new HashProvider();

            Assert.IsTrue(hashProvider.VerifyPasswordMatch(hashedPassword, password, salt));            
        }
    }
}
