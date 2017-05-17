using eWAY.Rapid.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.UnitTests {
    [TestClass]
    public class UserDisplayMessageTests : SdkTestBase {

        [TestMethod]
        public void ReturnValidErrorMessage() {
            //Arrange
            var testMessage = "Invalid TransactionType, account not certified for eCome only MOTO or Recurring available";
            //Act
            var message = RapidClientFactory.UserDisplayMessage("V6010", "en");
            //Assert
            Assert.AreEqual(message, testMessage);
        }

        [TestMethod]
        public void ReturnInvalidErrorMessage() {
            //Arrange
            var testMessage = SystemConstants.INVALID_ERROR_CODE_MESSAGE;
            //Act
            var message = RapidClientFactory.UserDisplayMessage("blahblah", "en");
            //Assert
            Assert.AreEqual(message, testMessage);
        }

        [TestMethod]
        public void ReturnDefaultEnglishLanguage() {
            //Arrange
            var testMessage = "Invalid TransactionType, account not certified for eCome only MOTO or Recurring available";
            //Act
            var message1 = RapidClientFactory.UserDisplayMessage("V6010", "de");
            var message2 = RapidClientFactory.UserDisplayMessage("V6010", "blahblah");
            //Assert
            Assert.AreEqual(message1, testMessage);
            Assert.AreEqual(message2, testMessage);
        }
    }
}
