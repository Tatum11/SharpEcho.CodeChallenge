using Microsoft.AspNetCore.Mvc;
using SharpEcho.CodeChallenge.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEcho.CodeChallenge.Api.Match.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : GenericController<Entities.Match>
    {
        public MatchesController(IRepository repository) : base(repository)
        {
        }

        [HttpGet("GetMatchHistoryBetweenTeams")]
        public ActionResult<List<Entities.Match>> GetMatchHistoryBetweenTeams(string FirstTeam, string SecondTeam)
        {
            var firstTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = FirstTeam });
            var secondTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = SecondTeam });
            if (firstTeamId.Count() == 0 || secondTeamId.Count() == 0)
            {
                return NotFound();

            }
            else
            {
                var result = Repository.Query<Entities.Match>("SELECT * FROM Match WHERE  HomeTeamId in(@firstTeam,@secondTeam)" +
                    "AND VisitorTeamId in(@firstTeam, @secondTeam)", new { firstTeam = firstTeamId.First().Id, secondTeam = secondTeamId.First().Id });

                if (result != null && result.Count() > 0)
                {
                    return result.ToList();

                }
                else
                {
                    return NotFound();
                }
            }
          
            
        }

        [HttpGet("GetOverallResultsBetweenTeams")]
        public ActionResult<List<OverallMatchResults>> GetOverallResultsBetweenTeams(string FirstTeam, string SecondTeam)
        {
            var firstTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = FirstTeam });
            var secondTeamId = Repository.Query<Entities.Match>("SELECT Id FROM Team WHERE Name = @Name", new { Name = SecondTeam });

            if (firstTeamId.Count() == 0 || secondTeamId.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                var results = Repository.Query<Entities.Match>("SELECT * FROM Match WHERE  HomeTeamId in(@firstTeam,@secondTeam)" +
                 "AND VisitorTeamId in(@firstTeam, @secondTeam)", new { firstTeam = firstTeamId.First().Id, secondTeam = secondTeamId.First().Id });



                var Teams = new List<OverallMatchResults>();
                Teams.Add(new OverallMatchResults() { Team = FirstTeam, Wins = 0, Loses = 0 });
                Teams.Add(new OverallMatchResults() { Team = SecondTeam, Wins = 0, Loses = 0 });

                var a = Teams.Find(x => x.Team == FirstTeam);
                var b = Teams.Find(x => x.Team == SecondTeam);

                foreach (var winningTeam in results.GroupBy(x => x.WinnerId))
                {
                    if (winningTeam.First().WinnerId == firstTeamId.First().Id)
                    {
                        a.Wins = winningTeam.Count();
                        a.Loses = results.Count() - winningTeam.Count();
                    }
                    else
                    {
                        b.Wins = winningTeam.Count();
                        b.Loses = results.Count() - winningTeam.Count();
                    }
                }

                return Teams.ToList();
            }

        }


    }
    public class OverallMatchResults
    {
        public string Team { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }

    }
}
