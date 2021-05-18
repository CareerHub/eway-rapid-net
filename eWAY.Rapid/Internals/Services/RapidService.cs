using eWAY.Rapid.Internals.Enums;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eWAY.Rapid.Internals.Services {
    /// <summary>
    /// Internal Client class that does the invocation of Rapid native API call
    /// </summary>
    internal class RapidService : IRapidService {
        private readonly Uri _rapidEndpoint;
        private readonly string _apiKey;
        private readonly string _password;

        private const string ACCESS_CODES = "AccessCodes";
        private const string ACCESS_CODE_RESULT = "AccessCode/{0}";
        private const string CANCEL_AUTHORISATION = "CancelAuthorisation";
        private const string DIRECT_PAYMENT = "Transaction";
        private const string ACCESS_CODES_SHARED = "AccessCodesShared";
        private const string CAPTURE_PAYMENT = "CapturePayment";
        private const string REFUND_PAYMENT = "Transaction/{0}/Refund";
        private const string QUERY_TRANSACTION = "Transaction/{0}";
        private const string QUERY_CUSTOMER = "Customer/{0}";
        private const string TRANSACTION_FILTER_INVOICE_NUMBER = "Transaction/InvoiceNumber/{0}";
        private const string TRANSACTION_FILTER_INVOICE_REF = "Transaction/InvoiceRef/{0}";
        private const string SETTLEMENT_SEARCH = "Search/Settlement";

        public RapidService(string apiKey, string password, string endpoint) {
            if(string.IsNullOrWhiteSpace(apiKey)) {
                throw new ArgumentException("required", nameof(apiKey));
            }
            if(string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("required", nameof(password));
            }
            if(string.IsNullOrEmpty(endpoint)) {
                throw new ArgumentException($"required", nameof(endpoint));
            }
            if(!Uri.IsWellFormedUriString(endpoint, UriKind.Absolute)) {
                throw new ArgumentException($"invalid url: '{endpoint}'", nameof(endpoint));
            }

            _apiKey = apiKey;
            _password = password;
            _rapidEndpoint = new Uri(endpoint);
        }

        public Task<DirectCancelAuthorisationResponse> CancelAuthorisationAsync(DirectCancelAuthorisationRequest request) {
            return JsonPostAsync<DirectCancelAuthorisationRequest, DirectCancelAuthorisationResponse>(request, CANCEL_AUTHORISATION);
        }

        public Task<DirectCapturePaymentResponse> CapturePaymentAsync(DirectCapturePaymentRequest request) {
            return JsonPostAsync<DirectCapturePaymentRequest, DirectCapturePaymentResponse>(request, CAPTURE_PAYMENT);
        }

        public Task<CreateAccessCodeResponse> CreateAccessCodeAsync(CreateAccessCodeRequest request) {
            return JsonPostAsync<CreateAccessCodeRequest, CreateAccessCodeResponse>(request, ACCESS_CODES);
        }

        public Task<CreateAccessCodeResponse> UpdateCustomerCreateAccessCodeAsync(CreateAccessCodeRequest request) {
            request.Method = Method.UpdateTokenCustomer;
            return JsonPostAsync<CreateAccessCodeRequest, CreateAccessCodeResponse>(request, ACCESS_CODES);
        }

        public Task<CreateAccessCodeSharedResponse> CreateAccessCodeSharedAsync(CreateAccessCodeSharedRequest request) {
            return JsonPostAsync<CreateAccessCodeSharedRequest, CreateAccessCodeSharedResponse>(request, ACCESS_CODES_SHARED);
        }

        public Task<CreateAccessCodeSharedResponse> UpdateCustomerCreateAccessCodeSharedAsync(CreateAccessCodeSharedRequest request) {
            request.Method = Method.UpdateTokenCustomer;
            return JsonPostAsync<CreateAccessCodeSharedRequest, CreateAccessCodeSharedResponse>(request, ACCESS_CODES_SHARED);
        }

        public Task<GetAccessCodeResultResponse> GetAccessCodeResultAsync(GetAccessCodeResultRequest request) {
            return JsonGetAsync<GetAccessCodeResultResponse>(string.Format(ACCESS_CODE_RESULT, request.AccessCode));
        }

        public Task<DirectPaymentResponse> DirectPaymentAsync(DirectPaymentRequest request) {
            return JsonPostAsync<DirectPaymentRequest, DirectPaymentResponse>(request, DIRECT_PAYMENT);
        }

        public Task<DirectPaymentResponse> UpdateCustomerDirectPaymentAsync(DirectPaymentRequest request) {
            request.Method = Method.UpdateTokenCustomer;
            return JsonPostAsync<DirectPaymentRequest, DirectPaymentResponse>(request, DIRECT_PAYMENT);
        }

        public Task<DirectAuthorisationResponse> DirectAuthorisationAsync(DirectAuthorisationRequest request) {
            return JsonPostAsync<DirectAuthorisationRequest, DirectAuthorisationResponse>(request, DIRECT_PAYMENT);
        }

        public Task<DirectCustomerResponse> DirectCustomerCreateAsync(DirectCustomerRequest request) {
            return JsonPostAsync<DirectCustomerRequest, DirectCustomerResponse>(request, DIRECT_PAYMENT);
        }

        public Task<DirectCustomerSearchResponse> DirectCustomerSearchAsync(DirectCustomerSearchRequest request) {
            return JsonGetAsync<DirectCustomerSearchResponse>(string.Format(QUERY_CUSTOMER, request.TokenCustomerID));
        }

        public Task<DirectRefundResponse> DirectRefundAsync(DirectRefundRequest request) {
            return JsonPostAsync<DirectRefundRequest, DirectRefundResponse>(request, string.Format(REFUND_PAYMENT, request.Refund.TransactionID));
        }

        public Task<TransactionSearchResponse> QueryTransactionAsync(long transactionID) {
            var method = string.Format(QUERY_TRANSACTION, transactionID);
            return JsonGetAsync<TransactionSearchResponse>(method);
        }

        public Task<TransactionSearchResponse> QueryTransactionAsync(string accessCode) {
            var method = string.Format(QUERY_TRANSACTION, accessCode);
            return JsonGetAsync<TransactionSearchResponse>(method);
        }

        public Task<TransactionSearchResponse> QueryInvoiceRefAsync(string invoiceRef) {
            var method = string.Format(TRANSACTION_FILTER_INVOICE_REF, invoiceRef);
            return JsonGetAsync<TransactionSearchResponse>(method);
        }

        public Task<TransactionSearchResponse> QueryInvoiceNumberAsync(string invoiceNumber) {
            var method = string.Format(TRANSACTION_FILTER_INVOICE_NUMBER, invoiceNumber);
            return JsonGetAsync<TransactionSearchResponse>(method);
        }

        public Task<DirectSettlementSearchResponse> SettlementSearchAsync(string query) {
            var method = SETTLEMENT_SEARCH + query;
            return JsonGetAsync<DirectSettlementSearchResponse>(method);
        }

        public async Task<TResponse> JsonPostAsync<TRequest, TResponse>(TRequest request, string method)
            where TRequest : class
            where TResponse : BaseResponse, new() {

            var jsonString = JsonSerializer.Serialize(request, new JsonSerializerOptions {
                IgnoreNullValues = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false) }
            });

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            using(var client = GetClient()) {
                var response = await client.PostAsync(method, content);

                return await DeserializeResponse<TResponse>(response);
            }
        }

        public async Task<TResponse> JsonGetAsync<TResponse>(string method)
            where TResponse : BaseResponse, new() {
            using(var client = GetClient()) {
                var response = await client.GetAsync(method);
                return await DeserializeResponse<TResponse>(response);
            }
        }

        private async Task<TResponse> DeserializeResponse<TResponse>(HttpResponseMessage response) where TResponse : BaseResponse, new() {
            if(!response.IsSuccessStatusCode) {
                switch(response.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        return new TResponse() { Errors = RapidSystemErrorCode.AUTHENTICATION_ERROR };
                    case HttpStatusCode.NotFound:
                        return new TResponse() { Errors = RapidSystemErrorCode.NOT_FOUND };
                    default:
                        return new TResponse() { Errors = RapidSystemErrorCode.COMMUNICATION_ERROR };
                }
            } else {
                var content = await response.Content.ReadAsStringAsync();

                if(String.IsNullOrEmpty(content)) {
                    return new TResponse() { Errors = RapidSystemErrorCode.COMMUNICATION_ERROR };
                } else {
                    return JsonSerializer.Deserialize<TResponse>(content);
                }
            }
        }

        private HttpClient GetClient() {
            var handler = new HttpClientHandler {
                SslProtocols = SslProtocols.Tls12
            };

            var client = new HttpClient(handler) {
                BaseAddress = _rapidEndpoint
            };

            var headers = client.DefaultRequestHeaders;

            headers.UserAgent.Add(new ProductInfoHeaderValue("eWAY-SDK-DotNetCore", "1.0.0"));

            headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(_apiKey + ":" + _password)));

            headers.Add("X-EWAY-APIVERSION", SystemConstants.API_VERSION);
            
            return client;
        }
    }
}
