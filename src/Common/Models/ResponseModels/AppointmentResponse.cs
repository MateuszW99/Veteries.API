using System;

namespace Models.ResponseModels
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public AnimalResponse Animal { get; set; }
    }
}
