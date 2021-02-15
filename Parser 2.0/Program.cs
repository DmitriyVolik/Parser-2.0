using Doctors.Models;
using Doctors.Parsers;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;

namespace Doctors
{

    public static class Helper
    {
        public static string MD5(this string s)
        {
            using (var provider = System.Security.Cryptography.MD5.Create())
            {
                StringBuilder builder = new StringBuilder();

                foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
                    builder.Append(b.ToString("x2").ToLower());

                return builder.ToString();
            }
        }
    }


    class Program
    {
        static HtmlDocument getHTML(string URL)
        {
            HtmlWeb web = new HtmlWeb();

            return web.Load(URL);
        }


        //void jfjfjf()
        //{https://www.doctolib.de/allgemeinmedizin/pritzwalk/sara-flassig
        //    //var html = getHTML("https://www.doctolib.de/allgemeinmedizin/deutschland?page=2");
        //    //var html = getHTML("https://www.doctolib.de/allgemeinmedizin/aichach-friedberg/verena-berger?insurance_sector=public");

        //    //var sw = new StreamWriter("list.html");

        //    //sw.Write(html.DocumentNode.InnerHtml);
        //    //sw.Close();



        //    var sr = new StreamReader("list.html");
        //    string htmlText = sr.ReadToEnd();
        //    sr.Close();

        //    var html = new HtmlDocument();
        //    html.LoadHtml(htmlText);

        //    var parser = new DoctorPageParser(html.DocumentNode);

        //    var doctor = parser.GetDoctor();

        //    Console.WriteLine(doctor.Name);
        //    Console.WriteLine(doctor.Phone);
        //    Console.WriteLine(doctor.Description);
        //    Console.WriteLine("---SERVICES--");
        //    foreach (var item in doctor.Services)
        //    {
        //        Console.WriteLine('\t' + item.Title);
        //    }
        //    Console.WriteLine("================");

        //    database.Save(doctor);

        //    /*
        //    var html = new HtmlDocument();
        //    html.LoadHtml(htmlText);

        //    var parser = new ListPageParser(html.DocumentNode);

        //    var doctors = parser.GetDoctors();

        //    foreach (var item in doctors)
        //    {
        //        Console.WriteLine(item.Url);
        //        Console.WriteLine(item.Name);
        //        Console.WriteLine("================");
        //    }
        //    */

        //    //Console.WriteLine(parser.GetJSON());


        //}

        static async void LoadListAsync(DataBase database)
        {
            var director = new ListParserDirector(
                "https://www.doctolib.de/allgemeinmedizin/deutschland", database);

            await Task<bool>.Run(() => director.Parse());
        }



        static async void LoadDoctorsAsync(DataBase database)
        {
            var director = new DoctorPageDirector(database);
            bool hasNext;
            do
            {
                hasNext = await Task<bool>.Run(() => director.Execute());
                Thread.Sleep(4000);
            } while (hasNext);
        }

        static void Main(string[] args)
        {
            var database = new DataBase(@"Data Source=.\SQLEXPRESS;Initial Catalog=Doctors;Integrated Security=True");
            //LoadListAsync(database);
            
            var list = database.FindAllServices();

            foreach (var item in list)
            {
                Console.WriteLine(item.Title);
                //var ss = database.Find(item);
                //foreach (var s in ss)
                //{
                //    Console.WriteLine(s.Title);
                //}
                //break;
            }

            //LoadDoctorsAsync(database);

            //while (true)
            //{
                
            //    Console.WriteLine(".");
            //    Thread.Sleep(1000);
            //}
        }
    }
}
