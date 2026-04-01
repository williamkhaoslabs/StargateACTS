using Microsoft.EntityFrameworkCore;

namespace StargateAPI.Business.Data;

public class StargateContext(DbContextOptions options) : DbContext (options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<AstronautDetail> AstronautDetails { get; set; }
    public DbSet<AstronautDuty> AstronautDuties { get; set; }
    public DbSet<ProcessLog> ProcessLogs { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);
        SeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(
            new Person { Id = 13, Name = "Jessica Meir" },
            new Person { Id = 14, Name = "Victor Glover" },
            new Person { Id = 15, Name = "Sunita Williams" },
            new Person { Id = 16, Name = "Michael Collins" },
            new Person { Id = 17, Name = "Eileen Collins" },
            new Person { Id = 18, Name = "Guion Bluford" },
            new Person { Id = 19, Name = "Anne McClain" },
            new Person { Id = 20, Name = "Jasmin Moghbeli" }
        );

        modelBuilder.Entity<AstronautDetail>().HasData(
            new AstronautDetail
            {
                Id = 2,
                PersonId = 13,
                CurrentRank = "Mission Specialist",
                CurrentDutyTitle = "Artemis Support Crew",
                CareerStartDate = new DateTime(2013, 6, 17, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 3,
                PersonId = 14,
                CurrentRank = "Commander",
                CurrentDutyTitle = "Lunar Gateway Pilot",
                CareerStartDate = new DateTime(2013, 6, 17, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 4,
                PersonId = 15,
                CurrentRank = "Captain",
                CurrentDutyTitle = "ISS Commander",
                CareerStartDate = new DateTime(1998, 6, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 5,
                PersonId = 16,
                CurrentRank = "Major General",
                CurrentDutyTitle = "RETIRED",
                CareerStartDate = new DateTime(1963, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                CareerEndDate = new DateTime(1981, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 6,
                PersonId = 17,
                CurrentRank = "Colonel",
                CurrentDutyTitle = "RETIRED",
                CareerStartDate = new DateTime(1990, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                CareerEndDate = new DateTime(2006, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 7,
                PersonId = 18,
                CurrentRank = "Colonel",
                CurrentDutyTitle = "RETIRED",
                CareerStartDate = new DateTime(1978, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                CareerEndDate = new DateTime(1993, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 8,
                PersonId = 19,
                CurrentRank = "Lieutenant Colonel",
                CurrentDutyTitle = "Flight Engineer",
                CareerStartDate = new DateTime(2013, 6, 17, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDetail
            {
                Id = 9,
                PersonId = 20,
                CurrentRank = "Major",
                CurrentDutyTitle = "Mission Commander",
                CareerStartDate = new DateTime(2017, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<AstronautDuty>().HasData(
            new AstronautDuty
            {
                Id = 2,
                PersonId = 13,
                Rank = "Mission Specialist",
                DutyTitle = "Artemis Support Crew",
                DutyStartDate = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 3,
                PersonId = 14,
                Rank = "Commander",
                DutyTitle = "Lunar Gateway Pilot",
                DutyStartDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 4,
                PersonId = 15,
                Rank = "Captain",
                DutyTitle = "ISS Commander",
                DutyStartDate = new DateTime(2012, 7, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 5,
                PersonId = 16,
                Rank = "Major General",
                DutyTitle = "RETIRED",
                DutyStartDate = new DateTime(1981, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 6,
                PersonId = 17,
                Rank = "Colonel",
                DutyTitle = "RETIRED",
                DutyStartDate = new DateTime(2006, 5, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 7,
                PersonId = 18,
                Rank = "Colonel",
                DutyTitle = "RETIRED",
                DutyStartDate = new DateTime(1993, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 8,
                PersonId = 19,
                Rank = "Lieutenant Colonel",
                DutyTitle = "Flight Engineer",
                DutyStartDate = new DateTime(2021, 4, 24, 0, 0, 0, DateTimeKind.Utc)
            },
            new AstronautDuty
            {
                Id = 9,
                PersonId = 20,
                Rank = "Major",
                DutyTitle = "Mission Commander",
                DutyStartDate = new DateTime(2023, 8, 26, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
