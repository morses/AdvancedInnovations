using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.ViewModel

{
    public class AddMemberToPickedServerVM
    {
        //public string? infoOfProcessOfBeingAdded;

        public string? infoOfProcessOfBeingAdded(string response)
        {
            var responseList = response.Split(",");
            string outPutOfResponse= "";
            foreach (var item in responseList)
            {
                if(item.Contains("is_pending"))
                {
                    var isPendingResult = item.Split(":");
                    if(item.ToString().Contains("false"))
                    {
                        outPutOfResponse = $"You've already joined. From discord: { responseList[4]}";
                    }
                    else
                    {
                        outPutOfResponse = "Joining is pending.";
                    }
                }
            }
            return outPutOfResponse;
        }

    }
}


