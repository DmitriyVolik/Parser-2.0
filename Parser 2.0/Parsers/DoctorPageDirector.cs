using Doctors.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Doctors.Parsers
{
    class DoctorPageDirector
    {
        private DataBase _dataBase;

        public DoctorPageDirector(DataBase database)
        {
            _dataBase = database;


        }
        static HtmlDocument getHTML(string URL)
        {
            WebClient wc = new WebClient();
            //wc.Proxy = new WebProxy(host, port);
            var page = wc.DownloadString(URL);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            return doc;
        }

        public void Parse(Doctor doctor)
        {
            Console.WriteLine("Get Doctor: " + doctor.Id);

            var html = getHTML("https://www.doctolib.de" + doctor.Url);
            var parser = new DoctorPageParser(html.DocumentNode);
            var siteDoctor = parser.GetDoctor();

            doctor.Phone = siteDoctor.Phone;
            doctor.ImageUrl = siteDoctor.ImageUrl;
            doctor.Description = siteDoctor.Description;
            doctor.Services = siteDoctor.Services;

            _dataBase.Save(doctor);
        }

        public bool Execute(bool all = false)
        {
            List<Doctor> doctors;
            if (all)
            {
                doctors = _dataBase.GetAllDoctorsWithoutImage();
            }
            else
            {
                doctors = _dataBase.getNotCollectedDoctors();
            }

            if (doctors.Count == 0) return false;

            foreach (var item in doctors)
            {
                Parse(item);
                Thread.Sleep(10000);
            }
            return true;
        }
    }
}
