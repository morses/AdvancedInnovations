using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordStats.Models;
using DiscordStats.ViewModels;

namespace DiscordStats_Tests
{
    public class FakeData
    {
        public static readonly List<ServerOwnerViewModel> ServersVM = new List<ServerOwnerViewModel>
        {
            new ServerOwnerViewModel{Id = "789317480325316640", ServerPk = 1, Name = "input/output server", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", Approximate_Member_Count=5},
            new ServerOwnerViewModel{Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", Approximate_Member_Count=5},
            new ServerOwnerViewModel{Id = "151516415641361", ServerPk = 3, Name = "fake server", Owner = "false", Icon = "", HasBot = "false", Approximate_Member_Count=23532}
        };

        public static readonly List<Server> Servers = new List<Server>
        {

            //new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount="null", Privacy="true",OnForum="null",Message="null"},
            new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500},
            new Server{Id = "928010025958510632", ServerPk = 2, Name = "Second Most Member Count", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true",Privacy="true", ApproximateMemberCount=400},
            new Server{Id = "151516415641361", ServerPk = 3, Name = "Third Most Member Count", Owner = "false", Icon = "", HasBot = "false",Privacy="true", ApproximateMemberCount=300},
            new Server{Id = "78931748032531134", ServerPk = 4, Name = "Fourth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true",Privacy="true", ApproximateMemberCount=200},
            new Server{Id = "928010025958513251", ServerPk = 5, Name = "Fifth Most Member Count", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true",Privacy="true", ApproximateMemberCount=100},
            new Server{Id = "151516415641235", ServerPk = 6, Name = "fake server", Owner = "false", Icon = "", HasBot = "false",Privacy="true", ApproximateMemberCount=23}

        };


        //public static readonly List<ServerPartial> ServersfromPartial = new List<ServerPartial>
        //{
        //    new ServerPartial{Id = "789317480325316640", Name = "input/output server", Approximate_Member_Count=5},
        //    new ServerPartial{Id = "928010025958510632",  Name = "Advanced Innovations", Approximate_Member_Count=5},
        //    new ServerPartial{Id = "151516415641361", Name = "fake server", Approximate_Member_Count=23532}

        //};
    }
}


