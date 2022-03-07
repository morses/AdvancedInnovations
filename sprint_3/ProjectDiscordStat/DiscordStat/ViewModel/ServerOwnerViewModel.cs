using DiscordStats.ViewModel;

namespace DiscordStats.ViewModels
{
    public class ServerOwnerViewModel
    {
        public string Id { get; set; }

        public int ServerPk { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public string Icon { get; set; }

        public string HasBot { get; set; }

        public int? Approximate_Member_Count { get; set; }

        public string Owner_Id { get; set; }

        public string Verification_Level { get; set; }

        public string Description { get; set; }

        public string Premium_Tier { get; set; }

        public string Approximate_Presence_Count { get; set; }
        public List<GuildUsers> users { get; set; }
    }
}