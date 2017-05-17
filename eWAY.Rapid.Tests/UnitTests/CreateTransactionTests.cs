using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Internals;
using eWAY.Rapid.Internals.Enums;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;
using eWAY.Rapid.Internals.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eWAY.Rapid.Tests.UnitTests {
    [TestClass]
    public class CreateTransactionTests {
        [TestMethod]
        public async Task CreateTransaction_Direct_CaptureTrue_InvokeDirectPayment() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            mockRapidApiClient.Setup(x => x.DirectPaymentAsync(It.IsAny<DirectPaymentRequest>()))
                .Returns(Task.FromResult(new DirectPaymentResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Direct, transaction);
            //Assert
            mockRapidApiClient.Verify();
        }

        [TestMethod]
        public async Task CreateTransaction_ResponsiveShared_CaptureTrue_TokenNo_InvokeCreateAccessCodeShared() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            mockRapidApiClient.Setup(x => x.CreateAccessCodeSharedAsync(It.IsAny<CreateAccessCodeSharedRequest>()))
                .Returns(Task.FromResult(new CreateAccessCodeSharedResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.ResponsiveShared, transaction);
            //Assert
            mockRapidApiClient.Verify();
        }

        [TestMethod]
        public async Task CreateTransaction_ResponsiveShared_CaptureTrue_TokenYes_InvokeCreateAccessCodeShared_TokenPayment() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeSharedRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(true, "123123123");
            mockRapidApiClient.Setup(x => x.CreateAccessCodeSharedAsync(It.IsAny<CreateAccessCodeSharedRequest>()))
                .Callback<CreateAccessCodeSharedRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeSharedResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.ResponsiveShared, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.TokenPayment);
        }
        [TestMethod]
        public async Task CreateTransaction_TransparentRedirect_CaptureTrue_TokenNo_InvokeCreateAccessCode() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            mockRapidApiClient.Setup(x => x.CreateAccessCodeAsync(It.IsAny<CreateAccessCodeRequest>()))
                .Returns(Task.FromResult(new CreateAccessCodeResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            mockRapidApiClient.Verify();
        }
        [TestMethod]
        public async Task CreateTransaction_TransparentRedirect_CaptureTrue_TokenYes_InvokeCreateAccessCode_TokenPayment() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(true, "123123123");
            mockRapidApiClient.Setup(x => x.CreateAccessCodeAsync(It.IsAny<CreateAccessCodeRequest>()))
                .Callback<CreateAccessCodeRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.TokenPayment);
        }
        [TestMethod]
        public async Task CreateTransaction_TransparentRedirect_CreateTokenTrue_InvokeCreateAccessCode_TokenPayment() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(true, null, true);
            mockRapidApiClient.Setup(x => x.CreateAccessCodeAsync(It.IsAny<CreateAccessCodeRequest>()))
                .Callback<CreateAccessCodeRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(Method.TokenPayment, assertRequest.Method);
        }

        [TestMethod]
        public async Task CreateTransaction_Authorisation_InvokeDirectAuthorisation() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            //Arrange
            var transaction = TestUtil.CreateTransaction(false);
            mockRapidApiClient.Setup(x => x.DirectAuthorisationAsync(It.IsAny<DirectAuthorisationRequest>()))
                .Returns(Task.FromResult(new DirectAuthorisationResponse()))
                .Verifiable();

            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Authorisation, transaction);
            //Assert
            mockRapidApiClient.Verify();
        }
        [TestMethod]
        public async Task CreateTransaction_Direct_CaptureFalse_InvokeDirectPayment_MethodAuthorise() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            DirectPaymentRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(false);
            mockRapidApiClient.Setup(x => x.DirectPaymentAsync(It.IsAny<DirectPaymentRequest>()))
                .Callback<DirectPaymentRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new DirectPaymentResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Direct, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.Authorise);
        }
        [TestMethod]
        public async Task CreateTransaction_ResponsiveShared_CaptureFalse_InvokeCreateAccessCodeShared_MethodAuthorise() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeSharedRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(false);
            mockRapidApiClient.Setup(x => x.CreateAccessCodeSharedAsync(It.IsAny<CreateAccessCodeSharedRequest>()))
                .Callback<CreateAccessCodeSharedRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeSharedResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.ResponsiveShared, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.Authorise);
        }
        [TestMethod]
        public async Task CreateTransaction_TransparentRedirect_CaptureFalse_InvokeCreateAccessCode_MethodAuthorise() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            CreateAccessCodeRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(false);
            mockRapidApiClient.Setup(x => x.CreateAccessCodeAsync(It.IsAny<CreateAccessCodeRequest>()))
                .Callback<CreateAccessCodeRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new CreateAccessCodeResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.TransparentRedirect, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.Authorise);
        }

        [TestMethod]
        public async Task CreateTransaction_Wallet_CaptureTrue_InvokeDirectPayment_MethodProcessPayment() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            DirectPaymentRequest assertRequest = null;
            //Arrange
            var transaction = TestUtil.CreateTransaction(true);
            transaction.SecuredCardData = "123123123";
            mockRapidApiClient.Setup(x => x.DirectPaymentAsync(It.IsAny<DirectPaymentRequest>()))
                .Callback<DirectPaymentRequest>(i => assertRequest = i)
                .Returns(Task.FromResult(new DirectPaymentResponse()))
                .Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Wallet, transaction);
            //Assert
            mockRapidApiClient.Verify();
            Assert.IsNotNull(assertRequest);
            Assert.AreEqual(assertRequest.Method, Method.ProcessPayment);
        }

        [TestMethod]
        public async Task CreateTransaction_Wallet_CaptureFalse_InvokeDirectAuthorisation() {
            var mockRapidApiClient = new Mock<IRapidService>();
            var rapidSdkClient = new RapidClient(mockRapidApiClient.Object);
            //Arrange
            var transaction = TestUtil.CreateTransaction(false);
            transaction.SecuredCardData = "123123123";
            mockRapidApiClient.Setup(x => x.DirectAuthorisationAsync(It.IsAny<DirectAuthorisationRequest>())).Returns(Task.FromResult(new DirectAuthorisationResponse())).Verifiable();
            //Act
            await rapidSdkClient.CreateAsync(PaymentMethod.Wallet, transaction);
            //Assert
            mockRapidApiClient.Verify();
        }
    }
}
