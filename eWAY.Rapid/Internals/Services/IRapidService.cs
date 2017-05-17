using System.Threading.Tasks;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;

namespace eWAY.Rapid.Internals.Services {
    internal interface IRapidService {
        Task<DirectCancelAuthorisationResponse> CancelAuthorisationAsync(DirectCancelAuthorisationRequest request);
        Task<DirectCapturePaymentResponse> CapturePaymentAsync(DirectCapturePaymentRequest request);
        Task<CreateAccessCodeResponse> CreateAccessCodeAsync(CreateAccessCodeRequest request);
        Task<CreateAccessCodeResponse> UpdateCustomerCreateAccessCodeAsync(CreateAccessCodeRequest request);
        Task<CreateAccessCodeSharedResponse> CreateAccessCodeSharedAsync(CreateAccessCodeSharedRequest request);
        Task<CreateAccessCodeSharedResponse> UpdateCustomerCreateAccessCodeSharedAsync(CreateAccessCodeSharedRequest request);
        Task<GetAccessCodeResultResponse> GetAccessCodeResultAsync(GetAccessCodeResultRequest request);
        Task<DirectPaymentResponse> DirectPaymentAsync(DirectPaymentRequest request);
        Task<DirectPaymentResponse> UpdateCustomerDirectPaymentAsync(DirectPaymentRequest request);
        Task<DirectAuthorisationResponse> DirectAuthorisationAsync(DirectAuthorisationRequest request);
        Task<DirectCustomerResponse> DirectCustomerCreateAsync(DirectCustomerRequest request);
        Task<DirectRefundResponse> DirectRefundAsync(DirectRefundRequest request);
        Task<DirectCustomerSearchResponse> DirectCustomerSearchAsync(DirectCustomerSearchRequest request);
        Task<TransactionSearchResponse> QueryTransactionAsync(long transactionID);
        Task<TransactionSearchResponse> QueryTransactionAsync(string accessCode);
        Task<TransactionSearchResponse> QueryInvoiceRefAsync(string invoiceRef);
        Task<TransactionSearchResponse> QueryInvoiceNumberAsync(string invoiceNumber);
        Task<DirectSettlementSearchResponse> SettlementSearchAsync(string request);
    }
}