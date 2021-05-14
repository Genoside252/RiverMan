using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiverMan.Models
{
    public class UserSubscription
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SubscriptionId { get; set; }
        public virtual SubscriptionService Subscription { get; set; }

    }
}
