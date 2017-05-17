using Microsoft.Extensions.Configuration;

namespace eWAY.Rapid.Tests.IntegrationTests {
    public abstract class SdkTestBase {
        protected IRapidClient CreateRapidApiClient() {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("Api")
                .Get<RapidClientConfig>();

            return RapidClientFactory.NewRapidClient(config);
        }
    }
}
