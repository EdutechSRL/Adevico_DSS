using System.Runtime.Serialization;
using System;
using lm.Comol.Core.Communities;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
	[Serializable(), CLSCompliant(true)]
    public class dtoCommunityNode : dtoBaseCommunityNode
	{
        public Int32 IdOrganization { get; set; }
        public String Name { get; set; }
        public String FirstLetter { get {
            if (String.IsNullOrEmpty(Name))
                return "";
            else
                return Name[0].ToString().ToLower();
        } }
        public String ToolTip { get; set; }
        public Boolean isPrimary { get; set; }
        public Int32 IdCommunityType { get; set; }
        public Int32 IdResponsible { get; set; }

        public String CourseCode { get; set; }
        public Int32 Year { get; set; }
        public Int32 IdDegreeType { get; set; }
        public Int32 IdCourseTime { get; set; }

        public Boolean ConfirmSubscription { get; set; }
        public Boolean isClosedByAdministrator { get; set; }

        
        public dtoCommunityNodeType Type {get; set;}
        public CommunityStatus Status {get; set;}
        public Boolean Selected { get; set; }
        public Boolean AccessAvailable { get; set; }
    }
}