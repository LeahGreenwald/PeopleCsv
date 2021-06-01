using CsvHelper;
using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CsvUploadController : ControllerBase
    {
        private readonly string _connectionString;
        public CsvUploadController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("upload")]
        public void Upload(string Base64File)
        {
            int commaIndex = Base64File.IndexOf(',');
            string base64 = Base64File.Substring(commaIndex + 1);

            byte[] fileData = Convert.FromBase64String(base64);


            List<Person> people = GetFromCsv(fileData);

            var repo = new PeopleRepository(_connectionString);
            repo.AddPeople(people);
        }
        static List<Person> GetFromCsv(byte[] csvBytes)
        {
            using var memoryStream = new MemoryStream(csvBytes);
            using var reader = new StreamReader(memoryStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Person>().ToList();
        }

        [HttpGet]
        [Route("getallpeople")]
        public List<Person> GetAllPeople()
        {
            var repo = new PeopleRepository(_connectionString);
            return repo.GetAllPeople();
        }

        [HttpPost]
        [Route("deleteall")]
        public void DeleteAll()
        {
            var repo = new PeopleRepository(_connectionString);
            repo.DeleteAll();
        }

        [Route("generatecsv")]
        public IActionResult GenerateCsv(int amount)
        {
            var peopleList = GetRandomPeople(amount);
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
        static  List<Person> GetRandomPeople(int count)
        {
            var people = new List<Person>();

            for (int i = 1; i <= count; i++)
            {
                people.Add(new Person
                {
                    Id = 0,
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Email = Faker.Internet.Email(),
                    Address = Faker.Address.StreetAddress(),
                    Age = Faker.RandomNumber.Next()
                });
            }
            return people;
        }

    }
}
