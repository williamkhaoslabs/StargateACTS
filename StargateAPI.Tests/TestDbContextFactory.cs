using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;
    public static class TestDbContextFactory
    {
        public static StargateContext Create(string? dbName = null)
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new StargateContext(options) { SeedData = false };
            context.Database.EnsureCreated();
            return context;
        }
        
        public static Person SeedPerson(StargateContext context, string name)
        {
            var person = new Person { Name = name };
            context.People.Add(person);
            context.SaveChanges();
            return person;
        }
        
        public static (Person Person, AstronautDetail Detail, AstronautDuty Duty) SeedPersonWithDuty(
            StargateContext context,
            string name,
            string rank,
            string dutyTitle,
            DateTime startDate)
        {
            var person = new Person { Name = name };
            context.People.Add(person);
            context.SaveChanges();

            var detail = new AstronautDetail
            {
                PersonId = person.Id,
                CurrentRank = rank,
                CurrentDutyTitle = dutyTitle,
                CareerStartDate = startDate.Date
            };
            context.AstronautDetails.Add(detail);

            var duty = new AstronautDuty
            {
                PersonId = person.Id,
                Rank = rank,
                DutyTitle = dutyTitle,
                DutyStartDate = startDate.Date,
                DutyEndDate = null
            };
            context.AstronautDuties.Add(duty);
            context.SaveChanges();

            return (person, detail, duty);
        }
    }
