using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
