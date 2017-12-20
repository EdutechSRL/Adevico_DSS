
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	public class UserContext : iUserContext
	{

        private int _CurrentUserID;
        //private int _CurrentCommunityID;
        //private iLanguage _UserLanguage;
        private iPerson _CurrentUser;
        //private System.Guid _WorkSessionID;
        //private bool _isAnonymous;
        //private int _UserTypeID;

        public virtual int WorkingCommunityID { get; set; }
        public virtual int CurrentCommunityID { get; set; }
        public virtual int CurrentCommunityOrganizationID { get; set; }
        public virtual int UserDefaultOrganizationId { get; set; }
        public virtual iLanguage Language { get; set; }

		public virtual iPerson CurrentUser {
			get { return _CurrentUser; }
			set {
				_CurrentUser = value;
				if ((value != null)) {
					_CurrentUserID = value.Id;
				}
			}
		}
        public virtual IList<int> RolesID { get; set; }
        public virtual System.Guid WorkSessionID { get; set; }
		public bool isSameCommunity {
			get { return this.CurrentCommunityID.Equals(this.WorkingCommunityID); }
		}
        public virtual bool isAnonymous { get; set; }
        public virtual int UserTypeID { get; set; }
		public virtual int CurrentUserID {
			get { return _CurrentUserID; }
		}
        public string IpAddress { get; set; }
        public string ProxyIpAddress { get; set; }

		public UserContext()
		{
			this.RolesID = new List<int>();
		}


      
    }
}