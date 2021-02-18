using Doctors.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Doctors.Parsers
{
    class DoctorImageDownloadDirector
    {
        private DataBase _dataBase;

        public DoctorImageDownloadDirector(DataBase database)
        {
            _dataBase = database;
        }

        public static bool SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            Console.WriteLine("URL:" + imageUrl);
            Console.WriteLine("LOC:" + filename);
            

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; 
            bitmap = new Bitmap(stream);

            bool success = false;
            if (bitmap != null)
            {
                bitmap.Save(filename, format);
                success = true;
            }

            stream.Flush();
            stream.Close();
            client.Dispose();

            return success;
        }

        public readonly string LocalImageRoot = "Pictures";

        public void Parse(Doctor doctor)
        {
            Console.WriteLine("Doctor download: " + doctor.Id);
            try
            {
                var filename = "d-" + doctor.Id.ToString() + ".jpeg";
                var fullFileName = LocalImageRoot + "\\" + filename;

                
                if (SaveImage("https:" + doctor.ImageUrl, fullFileName, ImageFormat.Jpeg))
                {
                    doctor.LocalImageFile = filename;
                    _dataBase.Save(doctor);
                    Console.WriteLine("File: " + filename);
                }
                else
                {
                    Console.WriteLine("File not saved");
                }
            }
            catch (Exception e)
            {
                if (doctor.ImageUrl== "https://assets.doctolib.fr/images/default_doctor_avatar.png")
                {
                    doctor.LocalImageFile = "Default.png";
                    _dataBase.Save(doctor);
                    Console.WriteLine("File: Default");
                }
                Console.WriteLine(e.Message);
            }


        }

        public bool Execute()
        {
            List<Doctor> doctors;

            doctors = _dataBase.GetAllDoctorsWithoutLocalImage();

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
