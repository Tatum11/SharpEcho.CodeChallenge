using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpEcho.CodeChallenge.Api.Match.Controllers;
using SharpEcho.CodeChallenge.Data;
using System;
using System.Linq;

namespace SharpEcho.CodeChallenge.Api.Match.Tests
{
    [TestClass]
    public class MatchsUnitTest
    {
        IRepository Repository = new GenericRepository(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("SharpEcho"));

        [TestMethod]
        public void GetTeamByName_ShouldReturnCorrectTeam()
        {
            var controller = new MatchesController(Repository);

            var homeTeam = "Dallas Cowboys";
            var visitorTeam = "Atlanta Falcons";

            Repository.Query<Entities.Match>("Delete FROM Match", null);


            var firstTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = homeTeam });
            var secondTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = visitorTeam });


            for (int i = 0; i < 28; i++)
            {
                var match = new Entities.Match();
                if (i < 17)
                {
                     match = new Entities.Match
                    {
                        HomeTeamId = firstTeamId.First().Id,
                        VisitorTeamId = secondTeamId.First().Id,
                        WinnerId = firstTeamId.First().Id,
                        Matchdate = DateTime.Now
                    };
                }
                else
                {
                     match = new Entities.Match
                    {
                        HomeTeamId = firstTeamId.First().Id,
                        VisitorTeamId = secondTeamId.First().Id,
                        WinnerId = secondTeamId.First().Id,
                        Matchdate = DateTime.Now
                    };
                }

                controller.Post(match);
            }

            var result = controller.GetOverallResultsBetweenTeams(homeTeam, visitorTeam);
            var cowboys = result.Value.Find(x => x.Team == "Dallas Cowboys");


            Assert.IsTrue(cowboys.Wins == 17);
        }

        //[TestMethod]
        //public void GetTeamByName_ShouldNotReturnTeam()
        //{
        //    var controller = new TeamsController(Repository);

        //    var team = new Entities.Team
        //    {
        //        Name = Guid.NewGuid().ToString()
        //    };

        //    var result = controller.GetTeamByName(team.Name).Value;

        //    Assert.IsNull(result);
        //}
    }


}
