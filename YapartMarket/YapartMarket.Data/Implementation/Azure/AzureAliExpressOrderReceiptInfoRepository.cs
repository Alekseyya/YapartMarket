﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderReceiptInfoRepository : AzureGenericRepository<AliExpressOrderReceiptInfo>, IAzureAliExpressOrderReceiptInfoRepository
    {
        private readonly ILogger<AzureAliExpressOrderReceiptInfoRepository> _logger;
        private readonly string _tableName;
        private readonly string _connectionString;

        public AzureAliExpressOrderReceiptInfoRepository(ILogger<AzureAliExpressOrderReceiptInfoRepository> logger, string tableName, string connectionString) : base(tableName, connectionString)
        {
            _logger = logger;
            _tableName = tableName;
            _connectionString = connectionString;
        }
        public async Task InsertAsync(IEnumerable<AliExpressOrderReceiptInfo> collection)
        {
            var insertSql = new AliExpressOrderReceiptInfo().InsertString(_tableName);
            var listOrdersReceipt = collection.Select(x => new
            {
                order_id = x.OrderId,
                country_name = x.CountryName,
                mobile_no = x.Mobile,
                contact_person = x.ContractPerson,
                phone_country = x.PhoneCountry,
                phone_area = x.PhoneArea,
                province = x.Province,
                address = x.Address,
                phone_number = x.PhoneNumber,
                fax_number = x.FaxNumber,
                detail_address = x.StreetDetailedAddress,
                city = x.City,
                country = x.Country,
                address2 = x.Address2,
                fax_country = x.FaxCountry,
                zip = x.PostCode,
                fax_area = x.FaxArea,
                localized_address = x.LocalizedAddress
            });
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(insertSql, listOrdersReceipt);
            }
        }

        public async Task InsertAsync(AliExpressOrderReceiptInfo aliExpressOrderReceiptInfo)
        {
            var insertSql = new AliExpressOrderReceiptInfo().InsertString(_tableName);
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(insertSql, new
                    {
                        order_id = aliExpressOrderReceiptInfo.OrderId,
                        country_name = aliExpressOrderReceiptInfo.CountryName,
                        mobile_no = aliExpressOrderReceiptInfo.Mobile,
                        contact_person = aliExpressOrderReceiptInfo.ContractPerson,
                        phone_country = aliExpressOrderReceiptInfo.PhoneCountry,
                        phone_area = aliExpressOrderReceiptInfo.PhoneArea,
                        province = aliExpressOrderReceiptInfo.Province,
                        address = aliExpressOrderReceiptInfo.Address,
                        phone_number = aliExpressOrderReceiptInfo.PhoneNumber,
                        fax_number = aliExpressOrderReceiptInfo.FaxNumber,
                        detail_address = aliExpressOrderReceiptInfo.StreetDetailedAddress,
                        city = aliExpressOrderReceiptInfo.City,
                        country = aliExpressOrderReceiptInfo.Country,
                        address2 = aliExpressOrderReceiptInfo.Address2,
                        fax_country = aliExpressOrderReceiptInfo.FaxCountry,
                        zip = aliExpressOrderReceiptInfo.PostCode,
                        fax_area = aliExpressOrderReceiptInfo.FaxArea,
                        localized_address = aliExpressOrderReceiptInfo.LocalizedAddress
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"OrderId {aliExpressOrderReceiptInfo.OrderId} error");
                _logger.LogWarning(e.Message);
                throw;
            }
            
        }
    }
}
