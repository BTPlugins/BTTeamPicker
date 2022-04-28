using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BTTeamPicker.Modules
{
    public class Teams
    {
        [XmlAttribute]
        public string Tag { get; set; }
        [XmlAttribute]
        public string Group { get; set; }
    }
}
