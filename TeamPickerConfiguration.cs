using BTTeamPicker.Modules;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BTTeamPicker
{
    public class TeamPickerConfiguration : IRocketPluginConfiguration
    {
        public bool RequireTeamToPlay { get; set; }
        public bool RemoveGroupOnLeave { get; set; }
        [XmlArrayItem(ElementName = "Team")]
        public List<Teams> TeamPicker { get; set; }
        public bool DebugMode { get; set; }
        public bool KillOnTeamUpdate { get; set; }
        public void LoadDefaults()
        {
            RequireTeamToPlay = true;
            RemoveGroupOnLeave = false;
            KillOnTeamUpdate = true;
            TeamPicker = new List<Teams>()
            {
                new Teams()
                {
                    Tag = "USA",
                    Group = "TeamUSA"
                },
                new Teams()
                {
                    Tag = "Germany",
                    Group = "TeamGermany"
                }
            };
            DebugMode = true;
        }
    }
}
