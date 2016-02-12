using System;
using System.Collections.Generic;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Models.Entity;

namespace AsiaticIndustries.Core.Models.ViewModel
{

    #region Dashboard

    public class DashboardModel
    {
        public DashboardModel()
        { 
            CustomerList = new List<CustomerViewModel>();
            QuoteList = new List<QuoteViewModel>(); 
            USDMasterList = new List<CurrentUsdRateViewModel>();
            SampleList = new List<ProductSampleViewModel>();
        }

        public long TotalActiveUsers { get; set; }
        public long TotalActiveCustomer { get; set; }
        public long TotalActiveQuote { get; set; }
        public long TotalActiveProductSample { get; set; }

        public List<CustomerViewModel> CustomerList { get; set; }
        public List<QuoteViewModel> QuoteList { get; set; }
        public List<CurrentUsdRateViewModel> USDMasterList { get; set; }
        public List<ProductSampleViewModel> SampleList { get; set; }
    }

    #endregion Dashboard

}
