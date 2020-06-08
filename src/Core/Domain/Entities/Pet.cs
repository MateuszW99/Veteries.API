using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")] 
        public virtual ApplicationUser Owner { get; set; }
    }
}
