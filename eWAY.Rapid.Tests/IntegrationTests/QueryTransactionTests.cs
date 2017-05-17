using System;
using System.Linq;
using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.IntegrationTests {
    [TestClass]
    public class QueryTransactionTests : SdkTestBase {

        [TestMethod]
        public async Task QueryTransaction_ByTransactionId_Test() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();

            //Act
            var response = await client.CreateAsync(PaymentMethod.Direct, transaction);
            TestUtil.AssertNoErrors(response);

            var filter = new TransactionFilter() { TransactionID = response.TransactionStatus.TransactionID };

            var queryResponse = await client.QueryTransactionAsync(filter);
            TestUtil.AssertNoErrors(queryResponse);

            var queryResponse2 = await client.QueryTransactionAsync(response.TransactionStatus.TransactionID);
            TestUtil.AssertNoErrors(queryResponse2);

            //Assert
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse.TransactionStatus.TransactionID);
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse2.TransactionStatus.TransactionID);
            Assert.AreEqual(response.TransactionStatus.Total, queryResponse2.TransactionStatus.Total);
        }

        [TestMethod]
        public async Task QueryTransaction_ByAccessCode_Test() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();

            //Act
            var response = await client.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            var filter = new TransactionFilter() { AccessCode = response.AccessCode };
            var queryResponse = await client.QueryTransactionAsync(filter);
            var queryResponse2 = await client.QueryTransactionAsync(response.AccessCode);
            //Assert
            Assert.IsNotNull(queryResponse);
            Assert.IsNotNull(queryResponse2);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse.Transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse2.Transaction.Customer);
        }

        [TestMethod]
        public async Task QueryTransaction_ByInvoiceRef_Test() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();
            var r = new Random();
            var randomInvoiceRef = r.Next(100000, 999999);
            transaction.PaymentDetails.InvoiceReference = randomInvoiceRef.ToString();
            //Act
            var response = await client.CreateAsync(PaymentMethod.Direct, transaction);
            var filter = new TransactionFilter() {
                InvoiceReference = response.Transaction.PaymentDetails.InvoiceReference
            };
            var queryResponse = await client.QueryTransactionAsync(filter);
            var queryResponse2 = await client.QueryInvoiceRefAsync(response.Transaction.PaymentDetails.InvoiceReference);
            //Assert
            Assert.IsNotNull(queryResponse);
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse.TransactionStatus.TransactionID);
            Assert.IsNotNull(queryResponse2);
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse2.TransactionStatus.TransactionID);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse.Transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse2.Transaction.Customer);
        }
        [TestMethod]
        public async Task QueryTransaction_ByInvoiceNumber_Test() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();
            var r = new Random();
            var randomInvoiceNumber = r.Next(10000, 99999);
            transaction.PaymentDetails.InvoiceNumber = "Inv " + randomInvoiceNumber;
            //Act
            var response = await client.CreateAsync(PaymentMethod.Direct, transaction);
            var filter = new TransactionFilter() {
                InvoiceNumber = response.Transaction.PaymentDetails.InvoiceNumber
            };
            var queryResponse = await client.QueryTransactionAsync(filter);
            var queryResponse2 = await client.QueryInvoiceNumberAsync(response.Transaction.PaymentDetails.InvoiceNumber);
            //Assert
            Assert.IsNotNull(queryResponse);
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse.TransactionStatus.TransactionID);
            Assert.IsNotNull(queryResponse2);
            Assert.AreEqual(response.TransactionStatus.TransactionID, queryResponse2.TransactionStatus.TransactionID);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse.Transaction.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(response.Transaction.Customer,
                queryResponse2.Transaction.Customer);
        }

        [TestMethod]
        public async Task QueryTransaction_InvalidInputData_ReturnVariousErrors() {
            var client = CreateRapidApiClient();
            //Arrange
            var filter = new TransactionFilter() {
                TransactionID = -1
            };
            //Act
            var queryByIdResponse = await client.QueryTransactionAsync(filter);
            //Assert
            Assert.IsNotNull(queryByIdResponse.Errors);
            Assert.AreEqual(queryByIdResponse.Errors.FirstOrDefault(), "S9995");

            //Arrange
            filter = new TransactionFilter() {
                AccessCode = "leRandomAccessCode"
            };
            //Act
            var queryByAccessCodeResponse = await client.QueryTransactionAsync(filter);
            //Assert
            Assert.IsNull(queryByAccessCodeResponse.Transaction);

            //Arrange
            filter = new TransactionFilter() {
                InvoiceNumber = "leRandomInvoiceNumber"
            };
            //Act
            var queryByInvoiceNumberResponse = await client.QueryTransactionAsync(filter);
            //Assert
            Assert.IsNotNull(queryByInvoiceNumberResponse.Errors);
            Assert.AreEqual(queryByInvoiceNumberResponse.Errors.FirstOrDefault(), "V6171");

            //Arrange
            filter = new TransactionFilter() {
                InvoiceReference = "leRandomInvoiceReference"
            };
            //Act
            var queryByInvoiceRefResponse = await client.QueryTransactionAsync(filter);
            //Assert
            Assert.IsNotNull(queryByInvoiceRefResponse.Errors);
            Assert.AreEqual(queryByInvoiceRefResponse.Errors.FirstOrDefault(), "V6171");
        }
    }
}
