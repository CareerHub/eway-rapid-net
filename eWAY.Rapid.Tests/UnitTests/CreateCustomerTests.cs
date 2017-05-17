using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Internals;
using eWAY.Rapid.Internals.Enums;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;
using eWAY.Rapid.Internals.Services;
using eWAY.Rapid.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eWAY.Rapid.Tests.UnitTests {
    [TestClass]
    public class CreateCustomerTests : SdkTestBase {
        [TestMethod]
        public async Task CreateCustomer_Direct_InvokeDirectPayment_MethodCreateTokenCustomer() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            DirectPaymentRequest assertRequest = null;
            //Arrange
            var customer = TestUtil.CreateCustomer();
            mockRapidApiClient.Setup(x => x.DirectPaymentAsync(It.IsAny<DirectPaymentRequest>()))
                .Callback<DirectPaymentRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new DirectPaymentResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Direct, customer);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.CreateTokenCustomer);
        }
        [TestMethod]
        public async Task CreateCustomer_Direct_InvokeSecureFields_MethodCreateTokenCustomer() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            DirectPaymentRequest assertRequest = null;
            //Arrange
            var customer = TestUtil.CreateCustomer();
            customer.SecuredCardData = "44DD7jYYyRgaQnVibOAsYbbFIYmSXbS6hmTxosAhG6CK1biw=";
            mockRapidApiClient.Setup(x => x.DirectPaymentAsync(It.IsAny<DirectPaymentRequest>()))
                .Callback<DirectPaymentRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new DirectPaymentResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Direct, customer);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.CreateTokenCustomer);
            Assert.AreEqual(assertRequest.SecuredCardData, customer.SecuredCardData);
        }

        [TestMethod]
        public async Task CreateCustomer_TransparentRedirect_InvokeCreateAccessCode_MethodCreateTokenCustomer() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeRequest assertRequest = null;
            //Arrange
            var customer = TestUtil.CreateCustomer();
            mockRapidApiClient.Setup(x => x.CreateAccessCodeAsync(It.IsAny<CreateAccessCodeRequest>()))
                .Callback<CreateAccessCodeRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.TransparentRedirect, customer);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.CreateTokenCustomer);
        }
        [TestMethod]
        public async Task CreateCustomer_ResponsiveShared_InvokeCreateAccessCodeShared_MethodCreateTokenCustomer() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeSharedRequest assertRequest = null;
            //Arrange
            var customer = TestUtil.CreateCustomer();
            mockRapidApiClient.Setup(x => x.CreateAccessCodeSharedAsync(It.IsAny<CreateAccessCodeSharedRequest>()))
                .Callback<CreateAccessCodeSharedRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeSharedResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.ResponsiveShared, customer);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.CreateTokenCustomer);
        }
    }
}
