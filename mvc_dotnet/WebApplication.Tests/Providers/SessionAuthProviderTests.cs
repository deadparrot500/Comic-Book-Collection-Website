using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Tests.Providers
{
    [TestClass]
    public class SessionAuthProviderTests
    {
        Mock<IHttpContextAccessor> mockAccessor;
        Mock<ISession> mockSession;
        Mock<IUserDAL> mockUserDal;

        [TestInitialize]
        public void Initialize()
        {
            // Mocking helps us set up all of the properties to access session in our test
            // w/out really requiring session (tightly-coupled code is bad)
            mockAccessor = new Mock<IHttpContextAccessor>();
            mockSession = new Mock<ISession>();
            mockUserDal = new Mock<IUserDAL>();

            var mockContext = new Mock<HttpContext>();                                    
            mockContext.SetupGet(m => m.Session).Returns(mockSession.Object);

            mockAccessor.SetupGet(m => m.HttpContext).Returns(mockContext.Object);
        }

        [TestMethod]
        public void IsLoggedIn_Should_ReturnFalseIfSessionIsEmpty()
        {
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            Assert.IsFalse(provider.IsLoggedIn);
        }

        [TestMethod]
        public void IsLoggedIn_Should_ReturnTrueIfUserInSession()
        {
            AddUserToSession("test");
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            Assert.IsTrue(provider.IsLoggedIn);
        }

        [TestMethod]
        public void GetCurrentUser_Should_ReturnsNullIfNotLoggedIn()
        {
            // Arrange DAL to always return null ensuring that user does not exist           
            mockUserDal.Setup(m => m.GetUser(It.IsAny<string>())).Returns<User>(null);

            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            Assert.IsNull(provider.GetCurrentUser());            
        }

        [TestMethod]
        public void GetCurrentUser_Should_ReturnsUserIfLoggedIn()
        {
            // Arrange DAL to have user named "test"
            AddUserToDAL("test");
            AddUserToSession("test");
            
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Call our method to test
            var user = provider.GetCurrentUser();

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("test", user.Username);
        }

        [TestMethod]
        public void SignIn_Should_ReturnTrueIfMatch()
        {
            // Arrange the DAL to have a user                                    
            AddUserToDAL("test");                       
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Call our method to test
            bool success = provider.SignIn("test", "password123");

            // Assert
            Assert.IsTrue(success);
            // Asserts Set was called in session
            mockSession.Verify(m => m.Set(SessionAuthProvider.SessionKey, It.IsAny<byte[]>()));
        }

        [DataTestMethod]
        [DataRow("test", "password1234", "Password should not have a match")]
        [DataRow("test1234", "password123", "Username should not exist")]
        public void SignIn_Should_ReturnFalse(string username, string password, string error)
        {
            // Create a user in the DAL
            AddUserToDAL("test");
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Call our method with different credentials
            bool success = provider.SignIn(username, password);

            Assert.IsFalse(success, error);
        }

        [TestMethod]
        public void LogOff_Should_ClearTheSession()
        {
            // Arrange
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Act
            provider.LogOff();

            // Asserts that Clear was called
            mockSession.Verify(m => m.Clear());
        }
                
        [TestMethod]
        public void ChangePassword_Should_ReturnFalseIfNotLoggedIn()
        {
            // Arrange
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Act
            var result = provider.ChangePassword("password", "password1234");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangePassword_Should_ReturnFalseIfCredentialsDontMatch()
        {
            // Arrange
            AddUserToDAL("test");
            AddUserToSession("test");
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Act
            var result = provider.ChangePassword("wrongpassword", "newpassword");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangePassword_Should_UpdateIfCredentialsMatch()
        {
            // Arrange
            AddUserToDAL("test");
            AddUserToSession("test");
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Act
            var result = provider.ChangePassword("password123", "newpassword");

            // Assert
            Assert.IsTrue(result);
            mockUserDal.Verify(m => m.UpdateUser(It.IsAny<User>()));
        }

        [TestMethod]
        public void Register_Should_SaveUserToDatabase()
        {
            // Arrange
            var provider = new SessionAuthProvider(mockAccessor.Object, mockUserDal.Object);

            // Act
            provider.Register("test", "password123", "user");

            // Assert
            mockUserDal.Verify(m => m.CreateUser(It.IsAny<User>()));
            mockSession.Verify(m => m.Set(SessionAuthProvider.SessionKey, It.IsAny<byte[]>()));
        }
        
        #region Private Methods
        private void AddUserToDAL(string username)
        {
            var user = new User { Username = username, Password = "OZpnzNCj1mcK3lvPKxhh89ikT0w=", Salt = "rsD3TaOu0XQ=" };            
            mockUserDal.Setup(m => m.GetUser(username)).Returns(user);              
        }

        private void AddUserToSession(string username)
        {
            byte[] data = Encoding.UTF8.GetBytes(username);
            mockSession.Setup(m => m.TryGetValue(SessionAuthProvider.SessionKey, out data)).Returns(true);
        }
        #endregion
    }
}
