using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class VetUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int OfficeId { get; set; }
        [ForeignKey("OfficeId")]
        public virtual Office Office { get; set; }
        
        public ICollection<Appointment> Appointments { get; set; }
    }
}
