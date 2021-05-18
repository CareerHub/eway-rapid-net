using System.Globalization;
using System.Reflection;
using System.Resources;
using eWAY.Rapid.Internals;
using eWAY.Rapid.Internals.Services;

namespace eWAY.Rapid {
    /// <summary>
    /// Factory class to create an instance of IRapidSdkClient
    /// </summary>
    public static class RapidClientFactory {
        /// <summary>
        /// Static method to create a new Rapid SDK Client configured to communicate with eWAY's Rapid API
        /// </summary>
        /// <param name="apiKey">Rapid API Key</param>
        /// <param name="password">Password for the API Key</param>
        /// <param name="rapidEndpoint">Possible values ("Production", "Sandbox", or a URL) Production and sandbox will default to the Global Rapid API Endpoints.</param>
        /// <returns>Native class/object that can be used to create and access business objects such as customers and transactions. </returns>
        public static IRapidClient NewRapidClient(string apiKey, string password, string rapidEndpoint) {
            return new RapidClient(new RapidService(apiKey, password, rapidEndpoint));
        }

        public static IRapidClient NewRapidClient(RapidClientConfig config) {
            return new RapidClient(new RapidService(config.ApiKey, config.Password, config.RapidEndpoint));
        }

        /// <summary>
        /// This static/utility method will provide a message suitable for display to a user corresponding to a given Rapid Code/language.
        /// </summary>
        /// <param name="errorCode">Rapid API Error Code e.g. "V6023" </param>
        /// <param name="language">Language Code, e.g. "EN" (default) or "ES"</param>
        /// <returns>String with a description for the given code in the specified language</returns>
        public static string UserDisplayMessage(string errorCode, string language = SystemConstants.DEFAULT_LANGUAGE_CODE) {
            ResourceManager rm = new ResourceManager("eWAY.Rapid.Resources.ErrorMessages", typeof(RapidClientFactory).GetTypeInfo().Assembly);

            try {
                var cultureInfo = new CultureInfo(language);
                return rm.GetString(errorCode, cultureInfo);

            } catch(CultureNotFoundException) {
                var cultureInfo = new CultureInfo(SystemConstants.DEFAULT_LANGUAGE_CODE);
                return rm.GetString(errorCode, cultureInfo);

            } catch(MissingManifestResourceException) {
                var cultureInfo = new CultureInfo(SystemConstants.DEFAULT_LANGUAGE_CODE);
                try {
                    return rm.GetString(errorCode, cultureInfo);
                } catch(MissingManifestResourceException) {
                    return SystemConstants.INVALID_ERROR_CODE_MESSAGE;
                }
            }
        }
    }
}
