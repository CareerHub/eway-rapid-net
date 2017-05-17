using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Internals.Request;
using eWAY.Rapid.Internals.Response;
using eWAY.Rapid.Internals.Services;
using eWAY.Rapid.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace eWAY.Rapid.Internals {
    internal class RapidClient : IRapidClient {
        private readonly IRapidService _rapidService;
        private readonly IMappingService _mappingService;

        public RapidClient(IRapidService rapidService) {
            _rapidService = rapidService;
            _mappingService = new MappingService();
        }

        private async Task<CreateTransactionResponse> CreateInternalAsync(PaymentMethod paymentMethod, Transaction transaction) {
            switch(paymentMethod) {
                case PaymentMethod.Direct:
                    return await CreateTransactionAsync<DirectPaymentRequest, DirectPaymentResponse>(_rapidService.DirectPaymentAsync, transaction);
                case PaymentMethod.TransparentRedirect:
                    return await CreateTransactionAsync<CreateAccessCodeRequest, CreateAccessCodeResponse>(_rapidService.CreateAccessCodeAsync, transaction);
                case PaymentMethod.ResponsiveShared:
                    return await CreateTransactionAsync<CreateAccessCodeSharedRequest, CreateAccessCodeSharedResponse>(_rapidService.CreateAccessCodeSharedAsync, transaction);
                case PaymentMethod.Authorisation:
                    return await CreateTransactionAsync<DirectAuthorisationRequest, DirectAuthorisationResponse>(_rapidService.DirectAuthorisationAsync, transaction);
                case PaymentMethod.Wallet:
                    return transaction.Capture
                        ? await CreateTransactionAsync<DirectPaymentRequest, DirectPaymentResponse>(_rapidService.DirectPaymentAsync, transaction)
                        : await CreateTransactionAsync<DirectAuthorisationRequest, DirectAuthorisationResponse>(_rapidService.DirectAuthorisationAsync, transaction);
            }
            throw new NotSupportedException("Invalid PaymentMethod");
        }

        private async Task<CreateCustomerResponse> CreateInternalAsync(PaymentMethod paymentMethod, Customer customer) {
            switch(paymentMethod) {
                case PaymentMethod.Direct:
                    return await CreateCustomerAsync<DirectPaymentRequest, DirectPaymentResponse>(_rapidService.DirectPaymentAsync, customer);
                case PaymentMethod.TransparentRedirect:
                    return await CreateCustomerAsync<CreateAccessCodeRequest, CreateAccessCodeResponse>(_rapidService.CreateAccessCodeAsync, customer);
                case PaymentMethod.ResponsiveShared:
                    return await CreateCustomerAsync<CreateAccessCodeSharedRequest, CreateAccessCodeSharedResponse>(_rapidService.CreateAccessCodeSharedAsync, customer);
            }
            throw new NotSupportedException("Invalid PaymentMethod");
        }

        private async Task<CreateCustomerResponse> UpdateInternalAsync(PaymentMethod paymentMethod, Customer customer) {
            switch(paymentMethod) {
                case PaymentMethod.Direct:
                    return await CreateCustomerAsync<DirectPaymentRequest, DirectPaymentResponse>(_rapidService.UpdateCustomerDirectPaymentAsync, customer);
                case PaymentMethod.TransparentRedirect:
                    return await CreateCustomerAsync<CreateAccessCodeRequest, CreateAccessCodeResponse>(_rapidService.UpdateCustomerCreateAccessCodeAsync, customer);
                case PaymentMethod.ResponsiveShared:
                    return await CreateCustomerAsync<CreateAccessCodeSharedRequest, CreateAccessCodeSharedResponse>(_rapidService.UpdateCustomerCreateAccessCodeSharedAsync, customer);
            }
            throw new NotSupportedException("Invalid PaymentMethod");
        }


        public Task<CreateTransactionResponse> CreateAsync(PaymentMethod paymentMethod, Transaction transaction) {
            return CreateInternalAsync(paymentMethod, transaction);
        }

        public Task<CreateCustomerResponse> CreateAsync(PaymentMethod paymentMethod, Customer customer) {
            return CreateInternalAsync(paymentMethod, customer);
        }

        public Task<CreateCustomerResponse> UpdateCustomerAsync(PaymentMethod paymentMethod, Customer customer) {
            return UpdateInternalAsync(paymentMethod, customer);
        }

        private async Task<CreateTransactionResponse> CreateTransactionAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> invoker, Transaction transaction) {
            var request = _mappingService.Map<Transaction, TRequest>(transaction);
            var response = await invoker(request);
            return _mappingService.Map<TResponse, CreateTransactionResponse>(response);
        }

        private async Task<CreateCustomerResponse> CreateCustomerAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> invoker, Customer customer) {
            var request = _mappingService.Map<Customer, TRequest>(customer);
            var response = await invoker(request);
            return _mappingService.Map<TResponse, CreateCustomerResponse>(response);
        }

        public async Task<QueryTransactionResponse> QueryTransactionAsync(TransactionFilter filter) {
            if(!filter.IsValid) {
                return new QueryTransactionResponse() {
                    Errors = new List<string>(new[] { RapidSystemErrorCode.INTERNAL_SDK_ERROR })
                };
            }

            var response = new TransactionSearchResponse();
            if(filter.IsValidTransactionID) {
                response = await _rapidService.QueryTransactionAsync(filter.TransactionID);
            } else if(filter.IsValidAccessCode) {
                response = await _rapidService.QueryTransactionAsync(filter.AccessCode);
            } else if(filter.IsValidInvoiceRef) {
                response = await _rapidService.QueryInvoiceRefAsync(filter.InvoiceReference);
            } else if(filter.IsValidInvoiceNum) {
                response = await _rapidService.QueryInvoiceNumberAsync(filter.InvoiceNumber);
            }

            return _mappingService.Map<TransactionSearchResponse, QueryTransactionResponse>(response);
        }

        public Task<QueryTransactionResponse> QueryTransactionAsync(int transactionId) {
            return QueryTransactionAsync(Convert.ToInt64(transactionId));
        }

        public async Task<QueryTransactionResponse> QueryTransactionAsync(long transactionId) {
            var response = await  _rapidService.QueryTransactionAsync(transactionId);
            return _mappingService.Map<TransactionSearchResponse, QueryTransactionResponse>(response);
        }

        public async Task<QueryTransactionResponse> QueryTransactionAsync(string accessCode) {
            var response = await  _rapidService.QueryTransactionAsync(accessCode);
            return _mappingService.Map<TransactionSearchResponse, QueryTransactionResponse>(response);
        }
        public async Task<QueryTransactionResponse> QueryInvoiceNumberAsync(string invoiceNumber) {
            var response = await _rapidService.QueryInvoiceNumberAsync(invoiceNumber);
            return _mappingService.Map<TransactionSearchResponse, QueryTransactionResponse>(response);
        }
        public async Task<QueryTransactionResponse> QueryInvoiceRefAsync(string invoiceRef) {
            var response = await _rapidService.QueryInvoiceRefAsync(invoiceRef);
            return _mappingService.Map<TransactionSearchResponse, QueryTransactionResponse>(response);
        }

        public async Task<QueryCustomerResponse> QueryCustomerAsync(long tokenCustomerId) {
            var request = new DirectCustomerSearchRequest() { TokenCustomerID = tokenCustomerId.ToString() };
            var response = await _rapidService.DirectCustomerSearchAsync(request);
            return _mappingService.Map<DirectCustomerSearchResponse, QueryCustomerResponse>(response);
        }

        public async Task<RefundResponse> RefundAsync(Refund refund) {
            var request = _mappingService.Map<Refund, DirectRefundRequest>(refund);
            var response = await _rapidService.DirectRefundAsync(request);
            return _mappingService.Map<DirectRefundResponse, RefundResponse>(response);
        }

        public async Task<CapturePaymentResponse> CapturePaymentAsync(CapturePaymentRequest captureRequest) {
            var request = _mappingService.Map<CapturePaymentRequest, DirectCapturePaymentRequest>(captureRequest);
            var response = await _rapidService.CapturePaymentAsync(request);
            return _mappingService.Map<DirectCapturePaymentResponse, CapturePaymentResponse>(response);
        }

        public async Task<CancelAuthorisationResponse> CancelAuthorisationAsync(CancelAuthorisationRequest cancelRequest) {
            var request = _mappingService.Map<CancelAuthorisationRequest, DirectCancelAuthorisationRequest>(cancelRequest);
            var response = await _rapidService.CancelAuthorisationAsync(request);
            return _mappingService.Map<DirectCancelAuthorisationResponse, CancelAuthorisationResponse>(response);
        }

        public async Task<SettlementSearchResponse> SettlementSearchAsync(SettlementSearchRequest settlementSearchRequest) {
            var query = new QueryBuilder();
            var properties = settlementSearchRequest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in properties) {
                var value = prop.GetValue(settlementSearchRequest, null);
                if(value != null && !String.IsNullOrWhiteSpace(value.ToString())) {
                    if((!prop.Name.Equals("Page") && !prop.Name.Equals("PageSize")) || !value.Equals(0)) {
                        query.Add(prop.Name, value.ToString());
                    }
                }
            }

            var response = await _rapidService.SettlementSearchAsync(query.ToString());
            return _mappingService.Map<DirectSettlementSearchResponse, SettlementSearchResponse>(response);
        }
    }
}
