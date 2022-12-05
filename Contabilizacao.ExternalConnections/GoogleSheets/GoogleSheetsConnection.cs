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

namespace Contabilizacao.ExternalConnections.GoogleSheets
{
    public class GoogleSheetsConnection: IExternalConnection
    {
        //new List<string> { "CB", "Nome", "Preço", "Peso", "QuantidadeT1", "QuantidadeT2", "QuantidadeT3", "QuantidadeT4" }
        private SheetsService _sheetsService1;
        private SheetsService _sheetsService2;
        private SheetsService _sheetsService3;
        private List<SheetsService> _sheetsServiceList = new List<SheetsService>();
        private Tuple<int, int> _baseRange;
        private string _spreadsheetId;

        public GoogleSheetsConnection(IConfiguration configuration)
        {
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

        public List<Product> GetProducts()
        {
            var page = "Produtos";

            var response = MakeGetRequest(page);

           // List<string> columnsFromSpreadsheet = response.Values[0].Select(o => o.ToString()).ToList();

            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

            //var columns = new List<string> { "CB", "Nome", "Preço", "Peso", "QuantidadeT1", "QuantidadeT2", "QuantidadeT3", "QuantidadeT4" };

            //foreach (var c in columns)
            //{
            //    dic.Add(c, new List<string>());
            //}

            List<Product> result = new List<Product>();

            for(int i = 1; i < response.Values.Count; i++)
            {

                result.Add(new Product
                {
                    Id = i,
                    Code = response.Values[i][0].ToString()!,
                    Name = response.Values[i][1].ToString(),
                    Price = Convert.ToDouble(response.Values[i][2].ToString()),
                    Weight = Convert.ToDouble(response.Values[i][3].ToString()),
                    AmountPerShift = new List<int> 
                    { 
                        Convert.ToInt32(response.Values[i][4].ToString()),
                        Convert.ToInt32(response.Values[i][5].ToString()),
                        Convert.ToInt32(response.Values[i][6].ToString()),
                        Convert.ToInt32(response.Values[i][7].ToString())
                    }
                    
                });

            }

            //foreach (List<object> row in response.Values.Skip(1))
            //{
            //    List<string> data = new List<string>();

            //    for (int i = 0; i < row.Count; i++)
            //    {
            //        if (dic.Keys.Contains(columnsFromSpreadsheet[i]))
            //            dic[columnsFromSpreadsheet[i]].Add(row[i].ToString());
            //    }
            //}

            return result;
        }

        public void AddProduct(Product product)
        {
            var page = "Produtos";
            List<object> list = new List<object>
            {
                product.Code,
                product.Name,
                product.Price,
                product.Weight,
                product.AmountPerShift[0],
                product.AmountPerShift[1],
                product.AmountPerShift[2],
                product.AmountPerShift[3]
            };

            //foreach (string value in valuesToAppend)
            //{
            //    list.Add(value);
            //}

            var range = SpreadsheetRangeHelper.GetAppendRequestRange(1, list.Count, page);
            var valuerange = new ValueRange();
            valuerange.Values = new List<IList<object>>() { list };

            MakeAppendRequest(valuerange, range);
        }

        public void UpdateProduct(Product product)
        {
            var page = "Produtos";
           // List<object> list = new List<object>();

            //foreach (string value in valuesToUpdate)
            //{
            //    list.Add(value);
            //}

            List<object> list = new List<object>
            {
                product.Code,
                product.Name,
                product.Price,
                product.Weight,
                product.AmountPerShift[0],
                product.AmountPerShift[1],
                product.AmountPerShift[2],
                product.AmountPerShift[3]
            };

            var range = SpreadsheetRangeHelper.GetUpdateRequestRange(product.Id + 1, 1,  list.Count, page);
            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>>() { list };

            MakeUpdateRequest(valueRange, range);
        }

        public void AddProductToSupermarket(Product product, string supermarketName)
        {
            var page = supermarketName;
            //List<object> list = new List<object>();

            //foreach (string value in valuesToAppend)
            //{
            //    list.Add(value);
            //}

            List<object> list = new List<object>
            {
                product.Code,
                product.Name,
                product.Price,
                product.Weight,
                product.AmountPerShift[0],
                product.AmountPerShift[1],
                product.AmountPerShift[2],
                product.AmountPerShift[3]
            };

            var range = SpreadsheetRangeHelper.GetAppendRequestRange(1, list.Count, page);
            var valuerange = new ValueRange();
            valuerange.Values = new List<IList<object>>() { list };

            MakeAppendRequest(valuerange, range);
        }

        public void UpdateProductToSupermarket(Product product, string supermarketName)
        {
            var page = supermarketName;
            // List<object> list = new List<object>();

            //foreach (string value in valuesToUpdate)
            //{
            //    list.Add(value);
            //}

            List<object> list = new List<object>
            {
                product.Code,
                product.Name,
                product.Price,
                product.Weight,
                product.AmountPerShift[0],
                product.AmountPerShift[1],
                product.AmountPerShift[2],
                product.AmountPerShift[3]
            };

            var range = SpreadsheetRangeHelper.GetUpdateRequestRange(product.Id + 1, 1, list.Count, page);
            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>>() { list };

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
