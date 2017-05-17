using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.IntegrationTests {
    [TestClass]
    public class UpdateCustomerTests : SdkTestBase {
        [TestMethod]
        public async Task Customer_UpdateCustomerDirect_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var customer = TestUtil.CreateCustomer();
            var createResponse = await client.CreateAsync(PaymentMethod.Direct, customer);
            TestUtil.AssertNoErrors(createResponse);

            customer.TokenCustomerID = createResponse.Customer.TokenCustomerID;
            //Act
            var updateResponse = await client.UpdateCustomerAsync(PaymentMethod.Direct, customer);

            //Assert
            TestUtil.AssertNoErrors(updateResponse);
            Assert.AreEqual(createResponse.Customer.TokenCustomerID, updateResponse.Customer.TokenCustomerID);
        }

        [TestMethod]
        public async Task Customer_UpdateCustomerTransparentRedirect_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var customer = TestUtil.CreateCustomer();
            var createResponse = await client.CreateAsync(PaymentMethod.Direct, customer);
            TestUtil.AssertNoErrors(createResponse);

            customer.TokenCustomerID = createResponse.Customer.TokenCustomerID;
            //Act
            var updateResponse = await client.UpdateCustomerAsync(PaymentMethod.TransparentRedirect, customer);

            //Assert
            TestUtil.AssertNoErrors(updateResponse);
            Assert.AreEqual(createResponse.Customer.TokenCustomerID, updateResponse.Customer.TokenCustomerID);
        }

        [TestMethod]
        public async Task Customer_UpdateCustomerResponsiveShared_ReturnValidData() {
            var client = CreateRapidApiClient();
            //Arrange
            var customer = TestUtil.CreateCustomer();
            var createResponse = await client.CreateAsync(PaymentMethod.Direct, customer);
            TestUtil.AssertNoErrors(createResponse);

            customer.TokenCustomerID = createResponse.Customer.TokenCustomerID;
            //Act
            var updateResponse = await client.UpdateCustomerAsync(PaymentMethod.ResponsiveShared, customer);

            //Assert
            TestUtil.AssertNoErrors(updateResponse);
            Assert.AreEqual(createResponse.Customer.TokenCustomerID, updateResponse.Customer.TokenCustomerID);
        }
    }
}
