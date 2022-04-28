using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BTTeamPicker.Modules;
using Rocket.Core;
using Logger = Rocket.Core.Logging.Logger;
using System.Collections;

namespace BTTeamPicker.Commands
{
    public class TeamsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "Teams";

        public string Help => "";

        public string Syntax => "Teams";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "BTTeamPicker.DisplayTeams" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var Teams = Main.Instance.Configuration.Instance.TeamPicker;
            ChatManager.say(player.CSteamID, "{{color=#F3F3F3}}Available Teams{{/color}}".Replace("{{", "<").Replace("}}", ">"), Color.red, true);
            foreach (Teams Team in Teams)
            {
                ChatManager.say(player.CSteamID, Main.Instance.Translate("Teams_Display", Team.Tag).Replace("{", "<").Replace("}", ">"), Color.red, true);
            }
        }
    }
}
