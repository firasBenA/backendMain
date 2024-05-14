using System.Collections.Generic;
using System.Threading.Tasks;
using Braintree;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly IBraintreeGateway _braintreeGateway;

        public CardRepository(IBraintreeGateway braintreeGateway)
        {
            _braintreeGateway = braintreeGateway;
        }

        public async Task<bool> ValidateCard(CardDetails cardDetails)
        {
            var request = new TransactionRequest
            {
                Amount = 1.00m, // This can be set to any amount; we are just doing a validation
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = cardDetails.CardNumber,
                    ExpirationMonth = cardDetails.ExpiryMonth.ToString(), // Convert to string
                    ExpirationYear = cardDetails.ExpiryYear.ToString(), // Convert to string
                    CVV = cardDetails.Cvv,
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = false // We are not actually processing a payment, just verifying the card
                }
            };

            var result = await _braintreeGateway.Transaction.SaleAsync(request);

            return result.IsSuccess();
        }
    }
}
