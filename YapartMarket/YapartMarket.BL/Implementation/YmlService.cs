using Azure.Storage.Blobs;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;

namespace YapartMarket.BL.Implementation
{
    public class YmlService : YmlServiceBase
    {
        readonly ConnectionSettings connectionSettings;
        readonly StorageAccountSettings storageAccount;
        public YmlService(StorageAccountSettings storageAccount, ConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            this.storageAccount = storageAccount ?? throw new ArgumentNullException(nameof(storageAccount));
        }

        public override async Task ProcessCreateFileAsync(CancellationToken cancellationToken)
        {
            var blobServiceClient = new BlobServiceClient(storageAccount.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(storageAccount.Container);
            var blobClient = containerClient.GetBlobClient(storageAccount.ReadFileName);
            var xmlSerializer = new XmlSerializer(typeof(YmlCatalog));
            YmlCatalog? ymlCatalog = null;
            using (var memoryStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;
                using (var streamReader = new StreamReader(memoryStream))
                {
                    ymlCatalog = (YmlCatalog)xmlSerializer.Deserialize(streamReader)!;
                }
            }
            var productsSku = ymlCatalog?.Shop!.Offers!.Offer!.Select(x => x.VendorCode).ToList();
            var products = new Dictionary<string, int>();
            using (var connection = new SqlConnection(connectionSettings.SQLServerConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var take = 2000;
                var skip = 0;
                do
                {
                    var takeSkus = productsSku?.Skip(skip).Take(take);
                    skip += take;
                    if (!takeSkus!.Any())
                        break;
                    var tmp = await connection.QueryAsync<(string sku, int count)>("select sku, count from products where sku IN @skus", new { skus = takeSkus });
                    foreach (var item in tmp)
                    {
                        products.Add(item.sku, item.count);
                    }
                } while (true);
            }
            var offers = ymlCatalog!.Shop!.Offers!.Offer!.Where(x => products.Any(t => x.VendorCode?.ToLower() == t.Key.ToLower())).ToList();
            foreach (var offer in offers)
            {
                var product = products.FirstOrDefault(x => x.Key?.ToLower() == offer.VendorCode?.ToLower());
                offer.Outlets!.Outlet!.Instock = product.Value;
                offer.Outlets.Outlet.Id = 1;
            }
            var yapartYmlCatalog = new YmlCatalog()
            {
                Date = DateTime.Now,
                Shop = new Shop()
                {
                    Name = "Yapart",
                    Company = "Yapart",
                    Currencies = new Currencies() 
                    {
                        Currency = new List<Currency>() { new Currency() { Id = "RUR", Rate = 1 } }
                    },
                    Categories = ymlCatalog?.Shop.Categories,
                    Offers = new Offers()
                    {
                        Offer = offers
                    }
                }
            };
            var blobClientTest = containerClient.GetBlobClient(storageAccount.FileName);
            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, yapartYmlCatalog);
                memoryStream.Position = 0;
                await blobClientTest.UploadAsync(memoryStream, true, cancellationToken);
            }
        }
    }
}
