using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("QuoteDetails")]
    [PrimaryKey("QuoteDetailID")]
    [Sort("QuoteDetailID", "ASC")]
    public class QuoteDetail
    {

        public long QuoteDetailID { get; set; }
        public string TotalCrudePerkg { get; set; }
        public string TotalMFGOH { get; set; }
        public string TotalADMOH { get; set; }
        public string TotalPackingCost { get; set; }
        public string TotalFreightCost { get; set; }
        public string TotalForwardingCost { get; set; }
        public string TotalInterest { get; set; }
        public string TotalProfit { get; set; }
        public string TotalNett { get; set; }
        public string USDRate { get; set; }
        public string TotalNettInUSD { get; set; }
        public string Discount { get; set; }
        public string DiscountedPrice { get; set; }
        public string Commission { get; set; }
        public string FinalPrice { get; set; }


        
        public long QuoteID { get; set; }
        

        [Ignore]
        public string EncryptedQuoteDetailID
        {
            get { return Crypto.Encrypt(Convert.ToString(QuoteDetailID)); }
        }

        [Ignore]
        public string EncryptedQuoteID
        {
            get { return Crypto.Encrypt(Convert.ToString(QuoteID)); }
        }

      
    }
}
