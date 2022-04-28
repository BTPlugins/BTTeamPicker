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
    public class Team : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "Team";

        public string Help => "Team Function";

        public string Syntax => "Team <Join | Leave> <Tag>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "BTTeamPicker.Team" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if(command.Length < 2)
            {
                ChatManager.say(player.CSteamID, Main.Instance.Translate("ProperUsage", "/Team <Join | Leave> <Tag>").Replace("{", "<").Replace("}", ">"), Color.red, true);
                return;
            }
            string TeamOption = command[0].ToLower();
            string TeamTag = string.Join(" ", command.Skip(1)).ToLower();
            var Teams = Main.Instance.Configuration.Instance.TeamPicker;
            if (Teams == null)
            {
                return;
            }
            // TODO - Add a Message if the Team Tag they put does not Exists. 
            // TODO - Add another command /Teams - Displays all Aviaible Teams
            foreach (Teams Team in Teams)
            {
                if(TeamTag == Team.Tag.ToLower())
                {
                    var Group = R.Permissions.GetGroup(Team.Group);
                    if (Group == null)
                    {
                        Logger.LogWarning("ERROR: Invalid Group: " + Group.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + Group.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + Group.ToString());
                        return;
                    }
                    if (TeamOption == "join" || TeamOption == "j")
                    {
                        if (player.HasPermission("BTTeamPicker.Team.Join") || player.HasPermission("BTTeamPicker.Team"))
                        {
                            foreach (string IDS in Group.Members)
                            {
                                if (IDS == player.CSteamID.ToString())
                                {
                                    ChatManager.say(player.CSteamID, Main.Instance.Translate("AlreadyInTeam").Replace("{{", "<").Replace("}}", ">"), Color.red, true);
                                    return;
                                }
                            }
                            R.Permissions.AddPlayerToGroup(Team.Group, player);
                            ChatManager.say(player.CSteamID, Main.Instance.Translate("JoinedTeam", TeamTag).Replace("{", "<").Replace("}", ">"), Color.red, true);
                            player.Player.movement.sendPluginSpeedMultiplier(1);
                            if (Main.Instance.Configuration.Instance.KillOnTeamUpdate == true)
                            {
                                DamageTool.damage(player.Player, EDeathCause.KILL, ELimb.SPINE, player.CSteamID, Vector3.down, 105, 1, out _, false, false);
                            }
                            return;
                        }
                        else
                        {
                            ChatManager.say(player.CSteamID, Main.Instance.Translate("MissingPermission", "BTTeamPicker.Team.Join").Replace("{", "<").Replace("}", ">"), Color.red, true);
                            return;
                        }
                    }
                    else if (TeamOption == "leave" || TeamOption == "l")
                    {
                        if (player.HasPermission("BTTeamPicker.Team.Leave") || player.HasPermission("BTTeamPicker.Team"))
                        {
                            if (Group.Members.Contains(player.CSteamID.m_SteamID.ToString()))
                            {
                                // They are in the group
                                R.Permissions.RemovePlayerFromGroup(Team.Group, player);
                                ChatManager.say(player.CSteamID, Main.Instance.Translate("LeaveTeam", TeamTag).Replace("{", "<").Replace("}", ">"), Color.red, true);
                                player.Player.movement.sendPluginSpeedMultiplier(0);
                                if (Main.Instance.Configuration.Instance.KillOnTeamUpdate == true)
                                {
                                    DamageTool.damage(player.Player, EDeathCause.KILL, ELimb.SPINE, player.CSteamID, Vector3.down, 105, 1, out _, false, false);
                                }
                                Main.Instance.StartCoroutine(Main.Instance.MustJoinTeam(player));
                                // FIX - Above does not start the Countdown
                            }
                        }
                        else
                        {
                            ChatManager.say(player.CSteamID, Main.Instance.Translate("MissingPermission", "BTTeamPicker.Team.Leave").Replace("{", "<").Replace("}", ">"), Color.red, true);
                            return;
                        }
                    }
                    else
                    {
                        ChatManager.say(player.CSteamID, Main.Instance.Translate("ProperUsage", "/Team <Join | Leave> <Tag>").Replace("{", "<").Replace("}", ">"), Color.red, true);
                        return;
                    }
                }
            }
        }
    }
}
