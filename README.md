eWAY Rapid .NET Standard 2.0 Library
========
![Build Status](https://ci.appveyor.com/api/projects/status/github/visualeyes/eway-rapid-netstandard?branch=master&svg=true) 
[![Nuget Version](https://img.shields.io/nuget/v/eway-rapid-netstandard.svg)](https://www.nuget.org/packages/eway-rapid-netstandard/) 
[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE.md)

A .NET Standard 2.0 library to integrate with eWAY's Rapid Payment API.

Sign up with eWAY at:
 - Australia:    https://www.eway.com.au/
 - New Zealand:  https://eway.io/nz/
 - Hong Kong:    https://eway.io/hk/
 - Malaysia:     https://eway.io/my/
 - Singapore:    https://eway.io/sg/

For testing, get a free eWAY Partner account: https://www.eway.com.au/developers
 
## Usage

See the [eWAY Rapid API Reference](https://eway.io/api-v3/) for usage details.

A simple Direct payment example:

```csharp
using eWAY.Rapid;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Models;

string APIKEY = "Rapid API Key";
string PASSWORD = "Rapid API Password";
string ENDPOINT = "Sandbox";

IRapidClient ewayClient = RapidClientFactory.NewRapidClient(APIKEY, PASSWORD, ENDPOINT);

Transaction transaction = new Transaction(){
    Customer = new Customer() { 
        CardDetails = new CardDetails()
        {
            Name = "John Smith",
            Number = "4444333322221111",
            ExpiryMonth = "11",
            ExpiryYear = "22",
            CVN = "123" 
        } 
    },
    PaymentDetails = new PaymentDetails()
    {
        TotalAmount = 1000
    },
    TransactionType = TransactionTypes.Purchase
};

CreateTransactionResponse response = await ewayClient.CreateAsync(PaymentMethod.Direct, transaction);

if (response.TransactionStatus != null && response.TransactionStatus.Status == true)
{
    int transactionID = response.TransactionStatus.TransactionID;
}
```

## License

The MIT License (MIT). Please see [License File](LICENSE.md) for more information.