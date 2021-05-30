using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Faker;

namespace ReactCsvPeople.Data
{
    public class PeopleRepository
    {
        private readonly string _connectionString;
        public PeopleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Person> GetAllPeople ()
        {
            var ctx = new PeopleContext(_connectionString);
            return ctx.People.ToList();
        }
        public void AddPeople (List<Person> people)
        {
            var ctx = new PeopleContext(_connectionString);
            ctx.People.AddRange(people);
            ctx.SaveChanges();
        }
        public void DeleteAll()
        {
            var ctx = new PeopleContext(_connectionString);
            ctx.Database.ExecuteSqlRaw("delete from People");
            ctx.SaveChanges();
        }
        public List<Person> GetRandomPeople (int count)
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
