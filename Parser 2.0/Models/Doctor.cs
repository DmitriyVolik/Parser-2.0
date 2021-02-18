using System;
using System.Collections.Generic;
using System.Text;

namespace Doctors.Models
{
    public class Doctor
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public string LocalImageFile { get; set; }

        public List<Service> Services = new List<Service>();
    }
}
