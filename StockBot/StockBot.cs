using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

namespace StockBot
{
    public static class StockBot
    { 
        [Function("stockbot")]
        public async static Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get" , Route = "stockbot/{stock}")] HttpRequestData req, string stock,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");
            double? closePrice = await GetClosePriceOfStock(stock);

            string messageToBroker = $"APPL.US quote is ${closePrice} per share";
            logger.LogInformation(messageToBroker);



            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to Azure Functions!");

            return response;
        }


        private static async Task<double?> GetClosePriceOfStock(string stock)
        {
            var stockUri = GetStockMarketDataUri(stock);
            var responseStock = await GetAsync(stockUri);
            var stream = GenerateStreamFromString(responseStock);
            double? closePrice = GetClosePriceFromStream(stream);
            return closePrice;
        }

       
        private static string GetStockMarketDataUri(string stock)
        {
            var stockMarketData = $"https://stooq.com/q/l/?s={stock}&f=sd2t2ohlcv&h&e=csv";
            return stockMarketData;
        }

        public static async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static double? GetClosePriceFromStream(Stream stream)
        {
            double? closePrice = null;
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] fields = parser.ReadFields();
                int CLOSE_FIELD = 6;
                while (!parser.EndOfData)
                {
                    //Process row
                    foreach (string field in fields)
                    {
                        var closePriceStr = parser.ReadFields()[CLOSE_FIELD];
                        closePrice = (double)Convert.ToDouble(closePriceStr);
                    }
                }
            }

            return closePrice;
        }

    }
}
