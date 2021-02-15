using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Doctors.Models;

namespace Doctors.Parsers
{
    class ListPageParser
    {
        private HtmlNode document;

        public ListPageParser(HtmlNode document) => this.document = document;

        public List<Doctor> GetDoctors()
        {
            var result = new List<Doctor>();
            var scripts = document.QuerySelectorAll("script[type=\"application/ld+json\"]");

            foreach (var item in scripts)
            {
                var json = JsonDocument.Parse(item.InnerText);
               

                try
                {

                    var array = json.RootElement.EnumerateArray();

                    while (array.MoveNext())
                    {

                        var doctor = new Doctor();
                        doctor.Name = array.Current.GetProperty("name").ToString();
                        doctor.Url = array.Current.GetProperty("url").ToString();
                        doctor.PostalCode = array.Current.GetProperty("address").GetProperty("postalCode").ToString();
                        doctor.City = array.Current.GetProperty("address").GetProperty("addressLocality").ToString();
                        doctor.Address = array.Current.GetProperty("address").GetProperty("streetAddress").ToString();
                        result.Add(doctor);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }
    }
}
