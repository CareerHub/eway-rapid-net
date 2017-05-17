using System.Linq;
using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.IntegrationTests {
    [TestClass]
    public class CreateTransactionTests : SdkTestBase {
        [TestMethod]
        public async Task Transaction_CreateTransactionDirect_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();

            //Act
            var response = await client.CreateAsync(PaymentMethod.Direct, transaction);

            //Assert
            TestUtil.AssertNoErrors(response);
            Assert.IsNotNull(response.Transaction);
            Assert.IsNotNull(response.TransactionStatus);
            Assert.IsNotNull(response.TransactionStatus.Status);
            Assert.IsTrue(response.TransactionStatus.Status.Value);
            Assert.IsTrue(response.TransactionStatus.TransactionID > 0);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyCardDetailsAreEqual(response.Transaction.Customer,
                transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAllFieldsAreEqual(response.Transaction.Customer,
                transaction.Customer);
        }

        [TestMethod]
        public async Task Transaction_CreateTransactionTransparentRedirect_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();

            //Act
            var response = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);

            //Assert
            TestUtil.AssertNoErrors(response);
            Assert.IsNotNull(response.AccessCode);
            Assert.IsNotNull(response.FormActionUrl);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAllFieldsAreEqual(response.Transaction.Customer,
                transaction.Customer);
        }

        [TestMethod]
        public async Task Transaction_CreateTokenTransactionTransparentRedirect_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();
            transaction.SaveCustomer = true;

            //Act
            var response = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);

            //Assert
            TestUtil.AssertNoErrors(response);
            Assert.IsNotNull(response.AccessCode);
            Assert.IsNotNull(response.FormActionUrl);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAllFieldsAreEqual(response.Transaction.Customer,
                transaction.Customer);
        }

        [TestMethod]
        public async Task Transaction_CreateTransactionResponsiveShared_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();

            // Responsive Shared Fields
            transaction.LogoUrl = "https://mysite.com/images/logo4eway.jpg";
            transaction.HeaderText = "My Site Header Text";
            transaction.CustomerReadOnly = true;
            transaction.CustomView = "bootstrap";
            transaction.VerifyCustomerEmail = false;
            transaction.VerifyCustomerPhone = false;

            //Act
            var response = await client.CreateAsync(PaymentMethod.ResponsiveShared, transaction);

            //Assert
            TestUtil.AssertNoErrors(response);
            Assert.IsNotNull(response.AccessCode);
            Assert.IsNotNull(response.FormActionUrl);
            Assert.IsNotNull(response.SharedPaymentUrl);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAllFieldsAreEqual(response.Transaction.Customer,
                transaction.Customer);
        }

        [TestMethod]
        public async Task Transaction_CreateTransactionDirect_InvalidInputData_ReturnVariousErrors() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            transaction.Customer.CardDetails.Number = "-1";
            //Act
            var response1 = await client.CreateAsync(PaymentMethod.Direct, transaction);
            //Assert
            Assert.IsNotNull(response1.Errors);
            Assert.AreEqual(response1.Errors.FirstOrDefault(), "V6110");
            //Arrange
            transaction = TestUtil.CreateTransaction(true);
            transaction.PaymentDetails.TotalAmount = -1;
            //Act
            var response2 = await client.CreateAsync(PaymentMethod.Direct, transaction);
            //Assert
            Assert.IsNotNull(response2.Errors);
            Assert.AreEqual(response2.Errors.FirstOrDefault(), "V6011");
        }

        [TestMethod]
        public async Task Transaction_CreateTransactionTransparentRedirect_InvalidInputData_ReturnVariousErrors() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            transaction.PaymentDetails.TotalAmount = 0;
            //Act
            var response1 = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            Assert.IsNotNull(response1.Errors);
            Assert.AreEqual(response1.Errors.FirstOrDefault(), "V6011");
            //Arrange
            transaction = TestUtil.CreateTransaction(true);
            transaction.RedirectURL = "anInvalidRedirectUrl";
            //Act
            var response2 = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            Assert.IsNotNull(response2.Errors);
            Assert.AreEqual(response2.Errors.FirstOrDefault(), "V6059");
        }

        [TestMethod]
        public async Task Transaction_CreateTransactionResponsiveShared_InvalidInputData_ReturnVariousErrors() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            transaction.PaymentDetails.TotalAmount = 0;
            //Act
            var response1 = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            Assert.IsNotNull(response1.Errors);
            Assert.AreEqual(response1.Errors.FirstOrDefault(), "V6011");
            //Arrange
            transaction = TestUtil.CreateTransaction(true);
            transaction.RedirectURL = "anInvalidRedirectUrl";
            //Act
            var response2 = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            Assert.IsNotNull(response2.Errors);
            Assert.AreEqual(response2.Errors.FirstOrDefault(), "V6059");
        }
    }
}
