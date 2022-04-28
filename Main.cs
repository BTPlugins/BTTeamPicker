using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTeamPicker.Modules;
using Rocket.Core;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API.Collections;
using SDG.Unturned;
using UnityEngine;
using System.Collections;

namespace BTTeamPicker
{
    public class Main : RocketPlugin<TeamPickerConfiguration>
    {
        public static Main Instance;
        protected override void Load()
        {
            Instance = this;
            Logger.Log("#############################################", ConsoleColor.Yellow);
            Logger.Log("###           BTTeamPicker Loaded         ###", ConsoleColor.Yellow);
            Logger.Log("###   Plugin Created By blazethrower320   ###", ConsoleColor.Yellow);
            Logger.Log("###            Join my Discord:           ###", ConsoleColor.Yellow);
            Logger.Log("###     https://discord.gg/YsaXwBSTSm     ###", ConsoleColor.Yellow);
            Logger.Log("#############################################", ConsoleColor.Yellow);
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if(Main.Instance.Configuration.Instance.RemoveGroupOnLeave == true)
            {
                var Teams = Main.Instance.Configuration.Instance.TeamPicker;
                if(Teams == null)
                {
                    return;
                }
                foreach(Teams Team in Teams)
                {
                    var TeamGroup = R.Permissions.GetGroup(Team.Group);
                    if(TeamGroup == null)
                    {
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        return;
                    }
                    if (TeamGroup.Members.Contains(player.CSteamID.m_SteamID.ToString()))
                    {
                        R.Permissions.RemovePlayerFromGroup(Team.Group, player);
                        if(Main.Instance.Configuration.Instance.DebugMode == true)
                        {
                            Logger.Log(player.CharacterName + " has been removed from " + Team.Group.ToString() + " Group (Player Left)");
                        }
                    }
                }
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            if (Main.Instance.Configuration.Instance.RequireTeamToPlay == true)
            {
                int count = 0;
                var Teams = Main.Instance.Configuration.Instance.TeamPicker;
                if (Teams == null)
                {
                    return;
                }
                foreach (Teams Team in Teams)
                {
                    var TeamGroup = R.Permissions.GetGroup(Team.Group);
                    if (TeamGroup == null)
                    {
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        return;
                    }
                    if (TeamGroup.Members.Contains(player.CSteamID.m_SteamID.ToString()))
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    // Not in any Group
                    player.Player.movement.sendPluginSpeedMultiplier(0);
                    StartCoroutine(MustJoinTeam(player));
                }
                else
                {
                    return;
                }
            }
        }
        public IEnumerator MustJoinTeam(UnturnedPlayer player)
        {
            while(Main.Instance.Configuration.Instance.RequireTeamToPlay)
            {
                int count = 0;
                var Teams = Main.Instance.Configuration.Instance.TeamPicker;
                if (Teams == null)
                {
                    yield break;
                }
                foreach (Teams Team in Teams)
                {
                    var TeamGroup = R.Permissions.GetGroup(Team.Group);
                    if (TeamGroup == null)
                    {
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        Logger.LogWarning("ERROR: Invalid Group: " + TeamGroup.ToString());
                        yield break;
                    }
                    if (TeamGroup.Members.Contains(player.CSteamID.m_SteamID.ToString()))
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    ChatManager.say(player.CSteamID, Main.Instance.Translate("Must_JoinTeam").Replace("{{", "<").Replace("}}", ">"), Color.red, true);
                    ChatManager.say(player.CSteamID, "<color=#3E65FF>/Teams <color=#F3F3F3>- List of Available Teams</color>", Color.red, true);
                    yield return new WaitForSeconds(10f);
                }
                else
                {
                    yield break;
                }
            }
        }
        public override TranslationList DefaultTranslations => new TranslationList
        {
            { 
                "ProperUsage", "{{color=#FF0000}}[TeamPicker] {{/color}}{{color=#F3F3F3}}Proper Usage |{{/color}} {{color=#3E65FF}}{0}{{/color}}" 
            }, // {0} = Usage
            { 
                "Must_JoinTeam", "{{color=#FF0000}}[TeamPicker] {{/color}} {{color=#F3F3F3}}You must join a Team to play!{{/color}} {{color=#3E65FF}}/Team Join <TeamTag>{{/color}}"
            },
            { 
                "MissingPermission", "{{color=#FF0000}}[TeamPicker] {{/color}}{{color=#F3F3F3}}Missing Permission:{{/color}}{{color=#3E65FF}}{0}{{/color}} "
            },
            { 
                "AlreadyInTeam", "{{color=#FF0000}}[TeamPicker] {{/color}}{{color=#F3F3F3}}Please leave your current team to join another!{{/color}}" 
            },
            { 
                "JoinedTeam", "{{color=#FF0000}}[TeamPicker] {{/color}}{{color=#F3F3F3}}You have successfully Joined{{/color}} {{color=#3E65FF}}{0}{{/color}}{{color=#F3F3F3}}!{{/color}}" 
            },
            { 
                "LeaveTeam", "{{color=#FF0000}}[TeamPicker] {{/color}}{{color=#F3F3F3}}You have successfully Left{{/color}} {{color=#3E65FF}}{0}{{/color}}{{color=#F3F3F3}}!{{/color}}" 
            },
            {
                "Teams_Display", "{color=#F3F3F3}Team:{/color} {color=#3E65FF}{0}{/color}"
            },

        };

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }
    }
}
