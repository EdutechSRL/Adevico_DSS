using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{

    //public class ProjectTask : DomainBaseObjectMetaInfo<long> 
    //{
    //    //public virtual long ID { get; set; }

        
       
    //    #region "Project"
    //        public virtual IList<ProjectResource> ProjectResources { get; set; }
    //        public virtual Boolean isPersonal { get; set; }
    //        public virtual Boolean isArchived { get; set; }
    //        public virtual Boolean isPortal { get; set; }
    //        public virtual Boolean ConfirmCompletion { get; set; }
            
    //    #endregion

    //    #region "Common"
    //        public virtual IList<ProjectTask> TaskChildren { get; set; }
    //        public virtual Community Community { get; set; }
    //        public virtual String Name { get; set; }
    //        public virtual String Description { get; set; }
    //        public virtual String Notes { get; set; }
    //        public virtual int Completeness { get; set; }
    //        public virtual TaskStatus Status { get; set; }
    //        public virtual ProjectVisibility Visibility { get; set; }
    //    #endregion

    //    #region "Task"
    //        public virtual ProjectTask Project { get; set; }
    //        public virtual ProjectTask TaskParent { get; set; }
    //    #endregion
    //    #region "CPM"

    //    #endregion
 
    //    /// <summary>
    //    /// DA CONTROLLARE
    //    /// </summary>
    //    public virtual TaskPriority Priority { get; set; }
    //    public virtual DateTime? StartDate { get; set; }
    //    public virtual DateTime? EndDate { get; set; }
    //    public virtual DateTime? Deadline { get; set; }      
    //    public virtual bool isMilestone { get; set; }
    //    public virtual int Level { get; set; }
    //    public virtual int TaskWBSindex { get; set; }
    //    public virtual String TaskWBSstring { get; set; }       
    //    public virtual TaskConstraint ConstraintType { get; set; }
    //    public virtual DateTime? ConstraintDate { get; set; }

    //    public virtual String WBS
    //    {
    //        get
    //        {
    //            return TaskWBSstring + TaskWBSindex;
    //        }
    //    }

    //    public virtual TimeSpan? Duration 
    //    {
    //        get
    //        {
    //            if (EndDate!=null && StartDate!=null)
    //            {
    //                DateTime enddate = (DateTime)EndDate;
    //                DateTime startdate = (DateTime)StartDate;
    //                return enddate.Subtract(startdate);                 
    //            }

    //            return TimeSpan.Zero;   
                 
    //        }
    //    }

    //    public ProjectTask()
    //    {

    //    }
    //}
}
