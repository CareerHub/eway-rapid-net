﻿using System.Collections.Generic;
using eWAY.Rapid.Enums;

namespace eWAY.Rapid.Models
{
    /// <summary>
    /// The details of a transaction that will be processed either via the responsive shared page, 
    /// by transparent redirect or by Direct.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// What type of transaction this is (Purchase, MOTO,etc)
        /// </summary>
        public TransactionTypes TransactionType { get; set; }
        /// <summary>
        /// Set to true to create a regular transaction with immediate capture (default).
        /// Set to false to create an Authorisation transaction that can be used in a subsequent transaction.
        /// </summary>
        public bool Capture { get; set; }
        /// <summary>
        /// Set to true to create a token when the payment is processed using Transparent Redirect or Shared Page
        /// Set to false to process a standard transaction (default)
        /// </summary>
        public bool SaveCustomer { get; set; }
        /// <summary>
        /// Customer details (name address token etc)
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// (optional) Shipping Address, name etc for the product ordered with this transaction
        /// </summary>
        public ShippingDetails ShippingDetails { get; set; }
        /// <summary>
        /// Payment details (amount, currency and invoice information)
        /// </summary>
        public PaymentDetails PaymentDetails { get; set; }
        /// <summary>
        /// (optional) Invoice Line Items for the purchase
        /// </summary>
        public List<LineItem> LineItems { get; set; }
        /// <summary>
        /// (optional) General Options for the transaction
        /// </summary>
        public List<string> Options { get; set; }
        /// <summary>
        /// (optional) Used to supply an identifier for the device sending the transaction.
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// (optional) Used by shopping carts/ partners.
        /// </summary>
        public string PartnerID { get; set; }
        /// <summary>
        /// (optional) This field has been deprecated, please use SecuredCardData instead
        /// </summary>
        public string ThirdPartyWalletID { get; set; }
        /// <summary>
        /// (optional) Card data ID, used for Secure Fields, Visa Checkout, AMEX Express Checkout and Android Pay
        /// </summary>
        public string SecuredCardData { get; set; }
        /// <summary>
        /// (optional) Used with a PaymentType of Authorisation. This specifies the original authorisation that the funds are to be captured from.
        /// </summary>
        public int AuthTransactionID { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a CardSource of TransparentRedirect, or ResponsiveShared This field specifies the URL on the 
        /// merchant's site that the RapidAPI will redirect the cardholder's browser to after processing the transaction.
        /// </summary>
        public string RedirectURL { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. This field specifies the URL on the merchant's 
        /// site that the responsive page redirect the cardholder to if they choose to cancel the transaction.
        /// </summary>
        public string CancelURL { get; set; }
        /// <summary>
        /// (optional) The URL used for the integrating PayPal Checkout
        /// </summary>
        public string CheckoutURL { get; set; }
        /// <summary>
        /// (optional) Flag to set if the PayPal Checkout should be used for this
        /// </summary>
        public bool CheckoutPayment { get; set; }
        /// <summary>
        /// The customer’s IP address
        /// </summary>
        public string CustomerIP { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Sets whether customers can edit fields on the Responsive
        /// Shared Page
        /// </summary>
        public bool CustomerReadOnly { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Language code determines the language that the shared page 
        /// will be displayed in. One of: EN (English, default), ES (Spanish)
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Set the theme of the Responsive Shared Page from 12 available 
        /// themes: Bootstrap, BootstrapAmelia, BootstrapCerulean, BootstrapCosmo, BootstrapCyborg, BootstrapFlatly, BootstrapJournal, 
        /// BootstrapReadable, BootstrapSimplex, BootstrapSlate, BootstrapSpacelab, BootstrapUnited
        /// </summary>
        public string CustomView { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Set whether the customer’s phone number should be confirmed 
        /// using Beagle Verify
        /// </summary>
        public bool VerifyCustomerPhone { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Set whether the customer’s email should be confirmed using 
        /// Beagle Verify
        /// </summary>
        public bool VerifyCustomerEmail { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. Short text description to be placed under the logo on the shared page.
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// (optional) Used by transactions with a card source of ResponsiveShared. The URL of the merchant’s logo to display on the shared page. 
        /// This must start with https://. 
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// (v40 query response only) The date and time the transaction took place
        /// </summary>
        public string TransactionDateTime { get; set; }
        /// <summary>
        /// (v40 query response only) The fraud action that occurred if any. One of NotChallenged, Allow, Review, PreAuth, Processed, Approved, Block
        /// </summary>
        public string FraudAction { get; set; }
        /// <summary>
        /// (v40 query response only) True if funds were captured in the transaction.
        /// </summary>
        public bool? TransactionCaptured { get; set; }
        /// <summary>
        /// (v40 query response only) The ISO 4217 numeric currency code (e.g. AUD = 036)
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// (v40 query response only) Reserved for future use
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// (v40 query response only)  The maximum amount that could be refunded from this transaction
        /// </summary>
        public int? MaxRefund { get; set; }
        /// <summary>
        /// (v40 query response only) Contains the original transaction ID if the queried transaction is a refund
        /// </summary>
        public int? OriginalTransactionId { get; set; }

        /// <summary>
        /// Initializes a new instance of the Transaction class
        /// </summary>
        public Transaction()
        {
            // Default to capture
            Capture = true;
        }
    }
}
