using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Doctors.Models;

namespace Doctors.Parsers
{
    class DoctorPageParser
    {
        private HtmlNode document;

        public DoctorPageParser(HtmlNode document) => this.document = document;

        public Doctor GetDoctor()
        {
            var result = new Doctor();
                        
            var script = document.QuerySelector("script[type=\"application/ld+json\"]");
            var json = JsonDocument.Parse(script.InnerText);

            result.Name = json.RootElement.GetProperty("name").ToString();
            result.Description = json.RootElement.GetProperty("description").ToString();
            result.Url = json.RootElement.GetProperty("url").ToString();

            JsonElement el = new JsonElement();
            json.RootElement.TryGetProperty("telephone", out el);
            result.Phone = el.ToString();
            result.PostalCode = json.RootElement.GetProperty("address").GetProperty("postalCode").ToString();
            result.City = json.RootElement.GetProperty("address").GetProperty("addressLocality").ToString();
            result.Address = json.RootElement.GetProperty("address").GetProperty("streetAddress").ToString();
            result.ImageUrl = json.RootElement.GetProperty("image").ToString();

            var array = json.RootElement.GetProperty("availableService").EnumerateArray();
            while (array.MoveNext())
            {
               var service = new Service();
                service.Title = array.Current.GetProperty("name").ToString();
                result.Services.Add(service);
            }

            return result;
        }
    }
}
