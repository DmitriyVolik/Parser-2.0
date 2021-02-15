using Doctors.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctors.Parsers
{
    class ListParserDirector
    {
        private readonly string _baseUrl;
        private int _currentPage = 1;
        private DataBase _dataBase;
        public ListParserDirector(string baseUrl, DataBase dataBase)
        {
            _baseUrl = baseUrl;
            _dataBase = dataBase;
        }

        private string GetPageUrl(int page)
        {
            return _baseUrl + "?page=" + page.ToString();
        }

        static HtmlDocument getHTML(string URL)
        {
            HtmlWeb web = new HtmlWeb();

            return web.Load(URL);
        }

        public void Parse()
        {
            while (_currentPage < 100)
            {
                Console.WriteLine("Page: " + _currentPage);
                var html = getHTML(GetPageUrl(_currentPage));

                var parser = new ListPageParser(html.DocumentNode);
                var doctors = parser.GetDoctors();

                foreach (var item in doctors)
                {
                    Console.WriteLine("Doctor URL: " + item.Url);
                    _dataBase.Save(item);
                }

                _currentPage++;
            }
            
        }
    }
}
