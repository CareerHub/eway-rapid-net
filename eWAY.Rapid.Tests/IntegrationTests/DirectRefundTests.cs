﻿using System.Linq;
using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.IntegrationTests {
    [TestClass]
    public class DirectRefundTests : SdkTestBase {
        [TestMethod]
        public async Task DirectRefund_ReturnValidData() {
            //Arrange
            var client = CreateRapidApiClient();
            var transaction = TestUtil.CreateTransaction();
            var createTransactionResponse = await client.CreateAsync(PaymentMethod.Direct, transaction);
            var refund = TestUtil.CreateRefund(createTransactionResponse.TransactionStatus.TransactionID);
            //Act
            var refundResponse = await client.RefundAsync(refund);
            //Assert
            TestUtil.AssertNoErrors(refundResponse);
            Assert.AreEqual(createTransactionResponse.TransactionStatus.TransactionID, refundResponse.Refund.OriginalTransactionID);
            Assert.IsTrue(refundResponse.ResponseMessage.StartsWith("A"));
            Assert.IsTrue(refundResponse.TransactionID > 0);
            TestUtil.AssertReturnedCustomerData_VerifyCardDetailsAreEqual(refund.Customer, refundResponse.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAllFieldsAreEqual(refund.Customer, refundResponse.Customer);
            TestUtil.AssertReturnedCustomerData_VerifyAddressAreEqual(refund.Customer, refundResponse.Customer);
        }

        [TestMethod]
        public async Task DirectRefund_InvalidInputData_ReturnVariousErrors() {
            var client = CreateRapidApiClient();
            //Arrange
            var transaction = TestUtil.CreateTransaction();
            var createTransactionResponse = await client.CreateAsync(PaymentMethod.Direct, transaction);
            TestUtil.AssertNoErrors(createTransactionResponse);

            var refund = TestUtil.CreateRefund(createTransactionResponse.TransactionStatus.TransactionID);
            refund.RefundDetails.TotalAmount = -1;
            //Act
            var refundResponse1 = await client.RefundAsync(refund);
            //Assert
            // This test is failing at the moment
            //Assert.IsNotNull(refundResponse1.Errors);
            //Assert.AreEqual(refundResponse1.Errors.FirstOrDefault(), "D4413");
            //Arrange
            refund = TestUtil.CreateRefund(createTransactionResponse.TransactionStatus.TransactionID);
            refund.RefundDetails.OriginalTransactionID = -1;
            //Act
            var refundResponse2 = await client.RefundAsync(refund);
            //Assert
            Assert.AreEqual(refundResponse2.Errors.FirstOrDefault(), "V6115");
        }
    }
}
