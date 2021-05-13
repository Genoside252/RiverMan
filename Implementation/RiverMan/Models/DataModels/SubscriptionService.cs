using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiverMan.Models
{
    public class SubscriptionService
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }

        public virtual ServiceType ServiceType { get; set; }

        public string ImageURI { get; set; }
    }
}
