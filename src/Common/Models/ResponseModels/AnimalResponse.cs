using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class AnimalResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public List<AppointmentResponse> Appointments { get; set; }
    }
}
