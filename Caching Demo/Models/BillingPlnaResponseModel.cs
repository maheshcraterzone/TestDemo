using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caching_Demo.Models
{
    public class BillingPlanResponseModel
    {
        public int AppId { get; set; }
        public int PlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int ValidityDays { get; set; }
    }
}