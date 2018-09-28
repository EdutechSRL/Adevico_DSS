using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ActivityForGantt
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pID {get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pName{get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pStart {get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pEnd {get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pClass {get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pLink {get;set;}

        /// <summary>
        /// Milestone
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pMile {get;set;}
        /// <summary>
        /// Resources
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pRes {get;set;}
        /// <summary>
        /// Completion
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pComp {get;set;}
        /// <summary>
        ///  indicates whether this is a group task(parent) - Numeric; 1 = group task, 0 = not group task
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pGroup {get;set;}
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pParent {get;set;}
        /// <summary>
        ///  pOpen  (required) indicates whether a group task is open when chart is first drawn. Value must be set for all items but is only used by group tasks. Numeric, 1 = open, 0 = closed
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pOpen {get;set;}

        /// <summary>
        /// pDepend (optional) comma separated list of id's this task is dependent on. A line will be drawn from each listed task to this item
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pDepend {get;set;}

        /// <summary>
        /// (optional) caption that will be added after task bar if CaptionType set to "Caption"
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pCaption {get;set;}

        /// <summary>
        /// Detailed task information that will be displayed in tool tip for this task
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pNote {get;set;}

        public ActivityForGantt() { }


        public ActivityForGantt(ProjectResource resource, PmActivity activity, ProjectVisibility visibility,  String formatDatePattern, Dictionary<GanttCssClass, String> cssClass, String defaultActivityName,String urlBase = "")
        {
            pID = activity.Id.ToString();

            if (activity.Depth == 0)
            {
                pName = (visibility == ProjectVisibility.Full ) ? activity.Name : activity.WBSindex + " - " + defaultActivityName;
                pParent = "0";
            }
            else
            {
                //string FinalWBS = "";
                //string s = oTask.WBS;
                //string[] words = s.Split('.');
                ////foreach (string w in words) 
                //for (int i = 0; i < words.Length; i++)
                //{
                //    if (words[i].Length == 1)
                //    {
                //        FinalWBS += "0" + words[i];
                //    }
                //    else
                //    {
                //        FinalWBS += words[i];
                //    }

                //    if (i != words.Length - 1)
                //    {
                //        FinalWBS += ".";
                //    }

                //}
                //pName = FinalWBS + " " + oTask.Name;
                pName = activity.Name;
                pParent = (activity.Parent != null) ? activity.Parent.Id.ToString() : "0";
            }
           
            if (activity.isMilestone && !activity.IsSummary )
                pClass = cssClass[GanttCssClass.Milestone];
            else if (activity.IsSummary)
                pClass = cssClass[GanttCssClass.Summary];
            else if (activity.isCritical)
                pClass = cssClass[GanttCssClass.Critical];
            else
                pClass = cssClass[GanttCssClass.None];
            if (activity.Predecessors != null && activity.Predecessors.Any())
            {
                if (activity.Predecessors.Where(p => p.Type != PmActivityLinkType.FinishStart).Any())
                {
                    pDepend = String.Join(",", activity.Predecessors.Where(p => p.Target != null).Select(p => GetDependency(p)).ToList());
                }
                else
                    pDepend = String.Join(",", activity.Predecessors.Where(p => p.Target != null).Select(p => p.Target.Id).ToList());
            }
            else
                pDepend = "";
            if (activity.CurrentAssignments.Where(a => a.Resource != null).Any())
                pRes = String.Join(",", activity.CurrentAssignments.Where(a => a.Resource != null).Select(a => a.Resource.GetLongName()).ToList());
            else
                pRes = "";
            pStart = activity.EarlyStartDate.Value.ToString(formatDatePattern);
            if (activity.EarlyFinishDate.HasValue)
                pEnd = activity.EarlyFinishDate.Value.ToString(formatDatePattern);
            else
                pEnd = "";
            pMile = (activity.isMilestone) ? "1" : "0";
            pComp = activity.Completeness.ToString();
            pGroup = (activity.IsSummary) ? "1" : "0";
            pNote = activity.Description;
            pCaption = activity.Name;
            pLink = "";
            if (activity.IsSummary && activity.IsCompleted)
                pOpen = "0";
            else if (activity.IsSummary && activity.Depth>4)
                pOpen = "0";   
           // pLink = BaseUrl + "TaskList/TaskDetail.aspx?CurrentTaskID=" + oTask.ID + "&CurrentViewType=Read";
        }
        private String GetDependency(PmActivityLink link) {
            if (link == null)
                return "";
            else {
                switch (link.Type) { 
                    case PmActivityLinkType.FinishStart:
                        return link.Target.Id.ToString();
                    case PmActivityLinkType.FinishFinish:
                        return link.Target.Id.ToString()+"FF";
                    case PmActivityLinkType.StartFinish:
                        return link.Target.Id.ToString() + "SF";
                    case PmActivityLinkType.StartStart:
                        return link.Target.Id.ToString() + "SS";
                }
            }
            return "";
        }
    }
}