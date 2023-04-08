using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockBotFunction
{
    public static class StockManager
    {
        public static async Task<double?> GetClosePriceOfStock(string stock)
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

        private static async Task<string> GetAsync(string uri)
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

        private static Stream GenerateStreamFromString(string s)
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
                    var closePriceStr = parser.ReadFields()[CLOSE_FIELD];
                    closePrice = (double)Convert.ToDouble(closePriceStr);

                }
            }

            return closePrice;
        }
    }
}
