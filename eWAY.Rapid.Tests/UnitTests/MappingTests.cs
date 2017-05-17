using AutoMapper;
using eWAY.Rapid.Internals.Mappings;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;
using eWAY.Rapid.Internals.Services;
using eWAY.Rapid.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseResponse = eWAY.Rapid.Internals.Response.BaseResponse;

namespace eWAY.Rapid.Tests.UnitTests {
    [TestClass]
    public class MappingTests {
        /*
        [TestMethod]
        public void CustomMapProfile_IsValid() {
            new MapperConfiguration(c => c.AddProfile<CustomMapProfile>()).AssertConfigurationIsValid();
        }
        [TestMethod]
        public void EntitiesMapProfile_IsValid() {
            new MapperConfiguration(c => c.AddProfile<EntitiesMapProfile>()).AssertConfigurationIsValid();
        }
        [TestMethod]
        public void RequestMapProfile_IsValid() {
            new MapperConfiguration(c => c.AddProfile<RequestMapProfile>()).AssertConfigurationIsValid();
        }
        [TestMethod]
        public void ResponseMapProfile_IsValid() {
            new MapperConfiguration(c => c.AddProfile<ResponseMapProfile>()).AssertConfigurationIsValid();
        }
        */
        [TestMethod]
        public void Transaction_To_DirectPaymentRequest_Test() {
            var source = TestUtil.CreateTransaction();
            var dest = new MappingService().Map<Transaction, DirectPaymentRequest>(source);
            Assert.AreEqual(source.CustomerIP, dest.CustomerIP);
        }

        [TestMethod]
        public void ErrorMapping_NoError_Test() {
            var source = new BaseResponse();
            var dest = new MappingService().Map<BaseResponse, Models.BaseResponse>(source);
            Assert.IsNull(dest.Errors);
        }

        [TestMethod]
        public void ErrorMapping_Test() {
            var source = new BaseResponse() {
                Errors = "D4401,D4403,D4404"
            };

            var dest = new MappingService().Map<BaseResponse, Models.BaseResponse>(source);
            Assert.AreEqual(dest.Errors.Count, 3);
            Assert.AreEqual(dest.Errors[0], "D4401");
            Assert.AreEqual(dest.Errors[1], "D4403");
            Assert.AreEqual(dest.Errors[2], "D4404");
        }

        [TestMethod]
        public void ErrorMapping_Inheritance_Test() {
            var source = TestUtil.CreateDirectPaymentResponse();
            source.Errors = "D4401,D4403,D4404";

            var dest = new MappingService().Map<DirectPaymentResponse, CreateTransactionResponse>(source);
            Assert.AreEqual(dest.Errors.Count, 3);
            Assert.AreEqual(dest.Errors[0], "D4401");
            Assert.AreEqual(dest.Errors[1], "D4403");
            Assert.AreEqual(dest.Errors[2], "D4404");
        }


        [TestMethod]
        public void DirectPaymentResponse_To_CreateTransactionResponse_Test() {
            var source = TestUtil.CreateDirectPaymentResponse();
            var dest = new MappingService().Map<DirectPaymentResponse, CreateTransactionResponse>(source);
            Assert.AreEqual(source.TransactionStatus, dest.TransactionStatus.Status);
        }
    }
}
