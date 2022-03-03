using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordStats.Models;

namespace DiscordStats_Tests
{
    public class FakeData
    {
        public static readonly List<Server> Servers = new List<Server>
        {
            new Server{Id = "789317480325316640", ServerPk = 1, Name = "input/output server", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5},
            new Server{Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", ApproximateMemberCount=5},
            new Server{Id = "151516415641361", ServerPk = 3, Name = "fake server", Owner = "false", Icon = "", HasBot = "false", ApproximateMemberCount=23532}
        };


        public static readonly List<ServerPartial> ServersfromPartial = new List<ServerPartial>
        {
            new ServerPartial{Id = "789317480325316640", Name = "input/output server", Approximate_Member_Count=5},
            new ServerPartial{Id = "928010025958510632",  Name = "Advanced Innovations", Approximate_Member_Count=5},
            new ServerPartial{Id = "151516415641361", Name = "fake server", Approximate_Member_Count=23532}

        };
    }
}


