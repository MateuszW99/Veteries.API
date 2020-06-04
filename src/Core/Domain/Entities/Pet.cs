using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<string> AppointmentsHistory { get; set; }

        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")] 
        public virtual ApplicationUser Owner { get; set; }
    }
}
