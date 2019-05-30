using Microsoft.Azure.CognitiveServices.FormRecognizer;
using Microsoft.Azure.CognitiveServices.FormRecognizer.Models;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

//namespace Microsoft.Azure.CognitiveServices.Vision.FormRecognizer.Tests
namespace FormRecognizerSDK.Tests
{
    public class ReceiptTests : BaseTests
    {       
        static private ReadReceiptResult GetRecognitionResultWithPolling(IFormRecognizerClient client, string operationLocation)
        {
            string operationId = operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);

            for (int remainingTries = 10; remainingTries > 0; remainingTries--)
            {
                ReadReceiptResult result = client.GetReadReceiptResultAsync(operationId).Result;

                Assert.True(result.Status != TextOperationStatusCodes.Failed);

                if (result.Status == TextOperationStatusCodes.Succeeded)
                {
                    return result;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            return null;
        }

        static private string ExtractElementsString(IList<ElementReference> elements, ReadReceiptResult result)
        {
            return String.Join(" ", elements.Select(e => e.ResolveWord(result).Text));
        }

        [Fact]
        public void BatchReadReceiptTest()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                HttpMockServer.Initialize(this.GetType().FullName, "BatchReadReceiptTest");

                string imageUrl = GetTestImageUrl("receipt.jpg");

                using (IFormRecognizerClient client = GetFormRecognizerClient(HttpMockServer.CreateInstance()))
                {
                    BatchReadReceiptHeaders headers = client.BatchReadReceiptAsync(imageUrl).Result;
                    Assert.NotNull(headers.OperationLocation);

                    ReadReceiptResult readReceiptResult = GetRecognitionResultWithPolling(client, headers.OperationLocation);
                    Assert.NotNull(readReceiptResult);
                    Assert.Equal(TextOperationStatusCodes.Succeeded, readReceiptResult.Status);
                    Assert.NotNull(readReceiptResult.UnderstandingResults);
                    Assert.Equal(1, readReceiptResult.UnderstandingResults.Count);

                    var result = readReceiptResult.UnderstandingResults[0];
                    Assert.NotNull(result);
                    Assert.Equal(1, result.Pages?.First());
                    Assert.Equal(8, result.Fields?.Count);

                    // Verify MerchantName
                    StringValue merchantName = result.Fields["MerchantName"] as StringValue;
                    Assert.NotNull(merchantName);
                    Assert.Equal("Serafina Osteria & Enoteca", merchantName.Value);
                    Assert.Equal("Serafina Osteria & Enoteca", merchantName.Text);
                    Assert.Equal("Serafina Osteria & Enoteca", ExtractElementsString(merchantName.Elements, readReceiptResult));

                    // Verify TransactionDate
                    StringValue transactionDate = result.Fields["TransactionDate"] as StringValue;
                    Assert.NotNull(transactionDate);
                    Assert.Equal("2018-06-06", transactionDate.Value);
                    Assert.Equal("06/06/18", transactionDate.Text);
                    Assert.Equal("06/06/18", ExtractElementsString(transactionDate.Elements, readReceiptResult));

                    // Verify Total
                    NumberValue total = result.Fields["Total"] as NumberValue;
                    Assert.NotNull(total);
                    Assert.Equal(463.61, total.Value);
                    Assert.Equal("$463.61", total.Text);
                    Assert.Equal("$463.61", ExtractElementsString(total.Elements, readReceiptResult));

                    // Test serialization
                    var expectedJson = File.ReadAllText(GetTestImagePath("receipt.json"));
                    var beforeJson = JsonConvert.SerializeObject(readReceiptResult, client.SerializationSettings);
                    Assert.Equal(expectedJson, beforeJson);

                    // Test deserialization
                    var readReceiptResult2 = JsonConvert.DeserializeObject<ReadReceiptResult>(beforeJson, client.DeserializationSettings);
                    var afterJson = JsonConvert.SerializeObject(readReceiptResult2, client.SerializationSettings);
                    Assert.Equal(beforeJson, afterJson);
                }
            }
        }

        [Fact]
        public void BatchReadReceiptInStreamTest()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                HttpMockServer.Initialize(this.GetType().FullName, "BatchReadReceiptInStreamTest");

                using (IFormRecognizerClient client = GetFormRecognizerClient(HttpMockServer.CreateInstance()))
                using (FileStream stream = new FileStream(GetTestImagePath("receipt.jpg"), FileMode.Open))
                {
                    BatchReadReceiptInStreamHeaders headers = client.BatchReadReceiptInStreamAsync(stream).Result;
                    Assert.NotNull(headers.OperationLocation);

                    ReadReceiptResult readReceiptResult = GetRecognitionResultWithPolling(client, headers.OperationLocation);
                    Assert.NotNull(readReceiptResult);
                    Assert.Equal(TextOperationStatusCodes.Succeeded, readReceiptResult.Status);
                    Assert.NotNull(readReceiptResult.UnderstandingResults);
                    Assert.Equal(1, readReceiptResult.UnderstandingResults.Count);

                    var result = readReceiptResult.UnderstandingResults[0];
                    Assert.NotNull(result);
                    Assert.Equal(1, result.Pages?.First());
                    Assert.Equal(8, result.Fields?.Count);

                    // Verify MerchantName
                    StringValue merchantName = result.Fields["MerchantName"] as StringValue;
                    Assert.NotNull(merchantName);
                    Assert.Equal("Serafina Osteria & Enoteca", merchantName.Value);
                    Assert.Equal("Serafina Osteria & Enoteca", merchantName.Text);
                    Assert.Equal("Serafina Osteria & Enoteca", ExtractElementsString(merchantName.Elements, readReceiptResult));

                    // Verify TransactionDate
                    StringValue transactionDate = result.Fields["TransactionDate"] as StringValue;
                    Assert.NotNull(transactionDate);
                    Assert.Equal("2018-06-06", transactionDate.Value);
                    Assert.Equal("06/06/18", transactionDate.Text);
                    Assert.Equal("06/06/18", ExtractElementsString(transactionDate.Elements, readReceiptResult));

                    // Verify Total
                    NumberValue total = result.Fields["Total"] as NumberValue;
                    Assert.NotNull(total);
                    Assert.Equal(463.61, total.Value);
                    Assert.Equal("$463.61", total.Text);
                    Assert.Equal("$463.61", ExtractElementsString(total.Elements, readReceiptResult));
                }
            }
        }
    }
}
