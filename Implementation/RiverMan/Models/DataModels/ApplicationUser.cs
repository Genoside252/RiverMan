using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverMan.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.DateTime), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime LastLogin { get; set; }

        public virtual ICollection<UserSubscription> Subscriptions { get; set; }
    }
}
