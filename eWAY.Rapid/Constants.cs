namespace eWAY.Rapid {
    /// <summary>
    /// Contains system constants.
    /// </summary>
    public static class SystemConstants {
        public const string INVALID_ERROR_CODE_MESSAGE = "";
        public const string DEFAULT_LANGUAGE_CODE = "en";
        public const string API_VERSION = "31";
    }

    /// <summary>
    /// Contains URLs for Rapid Endpoints.
    /// </summary>
    public static class GlobalEndpoints {
        public const string PRODUCTION = "https://api.ewaypayments.com";
        public const string SANDBOX = "https://api.sandbox.ewaypayments.com";
    }

    /// <summary>
    /// Contains system error codes for the eWAY Rapid library.
    /// </summary>
    public static class RapidSystemErrorCode {
        public const string INVALID_ENDPOINT_ERROR = "S9990";
        public const string INVALID_CREDENTIAL_ERROR = "S9991";
        public const string COMMUNICATION_ERROR = "S9992";
        public const string AUTHENTICATION_ERROR = "S9993";
        public const string INTERNAL_SDK_ERROR = "S9995";
        public const string NOT_FOUND = "HTTP404";
    }
}
