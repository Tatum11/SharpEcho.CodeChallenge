using System;

namespace SharpEcho.CodeChallenge.Api.Match.Entities
{
    public class Match
    {
        public long Id { get; set; }
        public long HomeTeamId { get; set; }
        public long VisitorTeamId { get; set; }
        public long WinnerId { get; set; }
        public DateTime Matchdate { get; set; }
    }
}
