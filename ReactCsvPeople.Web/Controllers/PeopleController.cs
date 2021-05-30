using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactCsvPeople.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactCsvPeople.Web.Controllers
{
    public class PeopleController : Controller
    {
        private readonly string _connectionString;
        public PeopleController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult GenerateCsv(int amount)
        {
            var repo = new PeopleRepository(_connectionString);
            var peopleList = repo.GetRandomPeople(amount);
            var csv = GetCsv(peopleList);
            var bytes = Encoding.UTF8.GetBytes(csv);

            return File(bytes, "APPLICATION/octet-stream", "PeopleCsv.csv");
        }
        static string GetCsv(List<Person> ppl)
        {
            var builder = new StringBuilder();
            var stringWriter = new StringWriter(builder);

            using var csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);
            csv.WriteRecords(ppl);

            return builder.ToString();
        }
    }
 }
