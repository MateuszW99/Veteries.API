using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
