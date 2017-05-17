using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Models;

namespace eWAY.Rapid {
    /// <summary>
    /// Public interface to  create/query transactions and customers
    /// </summary>
    public interface IRapidClient {
        /// <summary>
        /// This Method is used to create a transaction for the merchant in their eWAY account
        /// </summary>
        /// <param name="paymentMethod">
        /// Describes where the card details will be coming from for this transaction (Direct, Responsive Shared, 
        /// Transparent Redirect etc).
        /// </param>
        /// <param name="transaction">Request containing the transaction details</param>
        /// <returns>CreateTransactionResponse</returns>
        Task<CreateTransactionResponse> CreateAsync(PaymentMethod paymentMethod, Transaction transaction);

        /// <summary>
        /// This Method is used to create a token customer for the merchant in their eWAY account. 
        /// </summary>
        /// <param name="paymentMethod">Describes where the card details will be coming from that will be saved with the new token customer
        /// (Direct, Responsive Shared, Transparent Redirect etc).</param>
        /// <param name="customer">Request containing the Customer details</param>
        /// <returns>CreateCustomerResponse</returns>
        /// <remarks>
        /// Like the CreateTransaction, a PaymentMethod is specified which determines what method will be used to capture the card that will 
        /// be saved with the customer. Depending on the PaymentMethod the customer may be created immediately, or it may be pending (waiting 
        /// for Card Details to be supplied by the Responsive Shared Page, or Transparent Redirect). 
        /// The SDK will use the PaymentMethod parameter to determine what type of transaction to create rather than attempting to determine 
        /// the method to use implicitly (e.g. from the presence of CardDetails in a Customer).
        /// </remarks>
        Task<CreateCustomerResponse> CreateAsync(PaymentMethod paymentMethod, Customer customer);

        /// <summary>
        /// This Method is used to update a token customer for the merchant in their eWAY account. 
        /// </summary>
        /// <param name="paymentMethod">Describes where the card details will be coming from that will be saved with the new token customer
        /// (Direct, Responsive Shared, Transparent Redirect etc).</param>
        /// <param name="customer">Request containing the Customer details</param>
        /// <returns>CreateCustomerResponse</returns>
        Task<CreateCustomerResponse> UpdateCustomerAsync(PaymentMethod paymentMethod, Customer customer);

        /// <summary>
        /// This method is used to return the details of a Token Customer. This includes masked Card information for displaying in a UI to a user.
        /// </summary>
        /// <param name="tokenCustomerId">ID returned in the original create request.</param>
        /// <returns>the details of a Token Customer</returns>
        Task<QueryCustomerResponse> QueryCustomerAsync(long tokenCustomerId);

        /// <summary>
        /// This method is used to determine the status of a transaction. 
        /// </summary>
        /// <param name="filter">Filter definition for searching by other fields (e.g. invoice ID).</param>
        /// <returns>QueryTransactionResponse</returns>
        /// <remarks>
        /// However, it's also of use in situations where anti-fraud rules have triggered a transaction hold. 
        /// Once the transaction has been reviewed then the status will change, and in some cases the transaction ID as well. 
        /// So this method can be used by automated business processes to determine the state of a transaction that might 
        /// be under review.
        /// </remarks>
        Task<QueryTransactionResponse> QueryTransactionAsync(TransactionFilter filter);
        /// <summary>
        /// Gets transaction information given an int eWAY transaction ID
        /// (wrapper for the version which uses long)
        /// </summary>
        /// <param name="transactionId">eWAY Transaction ID for the transaction</param>
        /// <returns>QueryTransactionResponse</returns>
        Task<QueryTransactionResponse> QueryTransactionAsync(int transactionId);
        /// <summary>
        /// Gets transaction information given a long eWAY transaction ID
        /// </summary>
        /// <param name="transactionId">eWAY Transaction ID for the transaction</param>
        /// <returns>QueryTransactionResponse</returns>
        Task<QueryTransactionResponse> QueryTransactionAsync(long transactionId);
        /// <summary>
        /// Gets transaction information given an access code
        /// </summary>
        /// <param name="accessCode">Access code for the transaction to query</param>
        /// <returns>QueryTransactionResponse</returns>
        Task<QueryTransactionResponse> QueryTransactionAsync(string accessCode);
        /// <summary>
        /// Gets transaction information given an invoice number
        /// </summary>
        /// <param name="invoiceNumber">Merchant’s Invoice Number for the transaction</param>
        /// <returns>QueryTransactionResponse</returns>
        Task<QueryTransactionResponse> QueryInvoiceNumberAsync(string invoiceNumber);
        /// <summary>
        /// Gets transaction information given an invoice reference
        /// </summary>
        /// <param name="invoiceRef">The merchant's invoice reference</param>
        /// <returns>QueryTransactionResponse</returns>
        Task<QueryTransactionResponse> QueryInvoiceRefAsync(string invoiceRef);

        /// <summary>
        /// Refunds all or part of a previous transaction
        /// </summary>
        /// <param name="refund">Contains the details of the Refund</param>
        /// <returns>RefundResponse</returns>
        Task<RefundResponse> RefundAsync(Refund refund);

        /// <summary>
        /// Complete an authorised transaction with a Capture request
        /// </summary>
        /// <param name="captureRequest">Contains the details of the Payment</param>
        /// <returns>CapturePaymentResponse</returns>
        Task<CapturePaymentResponse> CapturePaymentAsync(CapturePaymentRequest captureRequest);

        /// <summary>
        /// Cancel an authorised transaction with a Cancel request
        /// </summary>
        /// <param name="cancelRequest">Contains the TransactionId of which needs to be cancelled</param>
        /// <returns>CancelAuthorisationResponse</returns>
        Task<CancelAuthorisationResponse> CancelAuthorisationAsync(CancelAuthorisationRequest cancelRequest);

        /// <summary>
        /// Perform a search of settlements with a given filter
        /// </summary>
        /// <param name="settlementSearchRequest">Contains the filter to search settlements by</param>
        /// <returns>SettlementSearchResponse</returns>
        Task<SettlementSearchResponse> SettlementSearchAsync(SettlementSearchRequest settlementSearchRequest);
    }
}
