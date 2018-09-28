using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
	[Serializable()]
    public static class TemplatePlaceHolders 
	{

        public static Dictionary<PlaceHoldersType, String> PlaceHolders(List<PlaceHoldersType> removePlaceHolders = null) {
            return lm.Comol.Core.TemplateMessages.Domain.BaseTemplatePlaceHolders<PlaceHoldersType>.PlaceHolders(removePlaceHolders);
        }
	    public static Boolean HasUserValues(List<String> subjects, List<String> body)
		{
			return HasUserValues(String.Join(" ", subjects), String.Join(" ", body));
		}
		public static Boolean HasUserValues(string subject, String body)
		{
			return HasUserValues(subject) || HasUserValues(body);
		}
		public static Boolean HasUserValues(string content)
		{
            return lm.Comol.Core.TemplateMessages.Domain.BaseTemplatePlaceHolders<PlaceHoldersType>.HasUserValues(content, GetUserValues());
		}
        private static List<PlaceHoldersType> GetUserValues() {
            return new List<PlaceHoldersType>() { PlaceHoldersType.ManagerName, PlaceHoldersType.ResourceName };
        }
	}



	/// <summary>
    /// Placeholder ProjectManagement
	/// </summary>
	[Serializable()]
	public enum PlaceHoldersType
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,
		/// <summary>
		/// Project Name 
		/// </summary>
		ProjectName = 1,
        /// <summary>
        /// Project communiy name
        /// </summary>
        ProjectCommunity = 2,
        /// <summary>
        /// Project Dashboard Url
        /// </summary>
        ProjectDashboardUrl = 3,
        /// <summary>
        /// Projects List Url
        /// </summary>
        ProjectsListUrl = 4,
        /// <summary>
        /// Tasks Url
        /// </summary>
        TasksListUrl = 5,
        /// <summary>
        /// Expiring Tasks Url
        /// </summary>
        ExpiringTasksUrl = 6,
        /// <summary>
        /// Late Tasks Url
        /// </summary>
        LateTasksUrl = 7,
        /// <summary>
        /// To do Tasks Url
        /// </summary>
        ToDoTasksUrl = 8,
        /// <summary>
        /// Resource name
        /// </summary>
        ResourceName = 9,
        /// <summary>
        /// Manager name
        /// </summary>
        ManagerName = 10,
        /// <summary>
        /// Number of tasks
        /// </summary>
        ToDoTasks = 11,
        /// <summary>
        /// Number of late tasks
        /// </summary>
        LateTasks = 12,
        /// <summary>
        /// Number of Expiring tasks
        /// </summary>
        ExpiringTasks = 13,
        /// <summary>
        /// Name of task
        /// </summary>
        TaskName = 14
	}
}