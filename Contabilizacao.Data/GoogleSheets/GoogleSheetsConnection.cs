using Contabilizacao.Data.Interfaces;
using Contabilizacao.Domain;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Data.GoogleSheets
{
    public class GoogleSheetsConnection
    {
        //new List<string> { "CB", "Nome", "Preço", "Peso", "QuantidadeT1", "QuantidadeT2", "QuantidadeT3", "QuantidadeT4" }
        private ContabilizacaoContext _context;
        private SheetsService _sheetsService1;
        private SheetsService _sheetsService2;
        private SheetsService _sheetsService3;
        private List<SheetsService> _sheetsServiceList = new List<SheetsService>();
        private Tuple<int, int> _baseRange;
        private string _spreadsheetId;

        public GoogleSheetsConnection(IConfiguration configuration, ContabilizacaoContext context)
        {
            _context = context;
            _spreadsheetId = configuration.GetSection("SpreadsheetId").Get<string>()!;

            GoogleSheetsServiceAccountCredentials credentials1 = configuration.GetSection("acc1").Get<GoogleSheetsServiceAccountCredentials>();
            GoogleSheetsServiceAccountCredentials credentials2 = configuration.GetSection("acc2").Get<GoogleSheetsServiceAccountCredentials>();
            GoogleSheetsServiceAccountCredentials credentials3 = configuration.GetSection("acc3").Get<GoogleSheetsServiceAccountCredentials>();

            var xCred1 = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(credentials1.client_email)
            {
                Scopes = new[] {
                SheetsService.Scope.Spreadsheets
            }
            }.FromPrivateKey(credentials1.private_key));

            var xCred2 = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(credentials2.client_email)
            {
                Scopes = new[] {
                SheetsService.Scope.Spreadsheets
            }
            }.FromPrivateKey(credentials1.private_key));



            var xCred3 = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(credentials3.client_email)
            {
                Scopes = new[] {
                SheetsService.Scope.Spreadsheets
            }
            }.FromPrivateKey(credentials1.private_key));


            _sheetsService1 = new SheetsService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = xCred1,
                }
            );
            _sheetsService2 = new SheetsService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = xCred2,
                }
            );
            _sheetsService3 = new SheetsService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = xCred3,
                }
            );

            _sheetsServiceList.Add(_sheetsService1);
            _sheetsServiceList.Add(_sheetsService2);
            _sheetsServiceList.Add(_sheetsService3);


            _baseRange = new Tuple<int, int>(200, 15);
        }
        
        public void UpdateSpreadsheet()
        {
            var products = _context.Products.ToList();
            var supermarkets = _context.Supermarkets.ToList();

            Dictionary<string, List<Product>> productsDictionary = new Dictionary<string, List<Product>>();
            //List<List<object>> updatedProductSpreadsheet = new List<List<object>>();

            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>>();

            foreach (var product in products)
            {
                if (!productsDictionary.Keys.Contains(product.Code))
                {
                    productsDictionary.Add(product.Code, new List<Product>() { product });
                    continue;
                }

                productsDictionary[product.Code].Add(product);
            }

            int numberOfRows = 0;
            foreach (var code in productsDictionary.Keys)
            {

                string cb = code;
                string name = productsDictionary[code][0].Name;
                double price = 0;
                double weight = 0;
                int amountFirstShift = 0;
                int amountSecondShift = 0;
                int amountThirdShift = 0;
                int amountFourthShift = 0;

                

                foreach(var product in productsDictionary[code])
                {
                    amountFirstShift += product.FirstShiftAmount;
                    amountSecondShift += product.SecondShiftAmount;
                    amountThirdShift += product.ThirdShiftAmount;
                    amountFourthShift += product.ForthShiftAmount;
                    price += product.Price;
                    weight += product.WeightOrVolume;
                }

                List<object> row = new List<object>{cb, name, price, weight, amountFirstShift, amountSecondShift, amountThirdShift, amountFourthShift };
                //updatedProductSpreadsheet.Add(row);
                valueRange.Values.Add(row);
                numberOfRows += 1;
            }

            var range = SpreadsheetRangeHelper.GetUpdateRequestRange(1, numberOfRows, 1, 8, "Produtos");
            MakeUpdateRequest(valueRange, range);

        }

        private ValueRange MakeGetRequest(string page)
        {
            ValueRange? response = null;

            foreach (var service in _sheetsServiceList)
            {
                try
                {
                    response = service.Spreadsheets.Values.Get(_spreadsheetId,
                    SpreadsheetRangeHelper.GetReadRequestRange(1, _baseRange.Item1, 1, _baseRange.Item2, page)).Execute();

                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (response == null)
                throw new Exception("Muitas requests!");

            return response;
        }

        private void MakeAppendRequest(ValueRange valuerange, string range)
        {

            AppendValuesResponse? response = null;

            foreach (var service in _sheetsServiceList)
            {
                try
                {
                    var appendRequest = service.Spreadsheets.Values.Append(valuerange, _spreadsheetId, range);
                    appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

                    response = appendRequest.Execute();

                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (response == null)
                throw new Exception("Muitas requests!");
        }

        private void MakeUpdateRequest(ValueRange valuerange, string range)
        {
            //var updateRequest = _sheetsService1.Spreadsheets.Values.Update(valuerange, spreadsheetId, range);
            //updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            //try
            //{
            //    updateRequest.Execute();
            //}
            //catch
            //{
            //    Thread.Sleep(60000);
            //    updateRequest.Execute();
            //}

            UpdateValuesResponse? response = null;

            foreach (var service in _sheetsServiceList)
            {
                try
                {
                    var updateRequest = service.Spreadsheets.Values.Update(valuerange, _spreadsheetId, range);
                    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

                    response = updateRequest.Execute();

                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (response == null)
                throw new Exception("Muitas requests!");

        }
    }
}
