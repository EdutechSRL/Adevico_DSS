using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class LazyCommunityModulePermission : DomainObject<int>
	{
        public virtual int CommunityID { get; set; }
        public virtual int ModuleID { get; set; }
        public virtual int RoleID { get; set; }
        public virtual string Permission { get; set; }

        public virtual long PermissionLong
        {
            get
            {
                if (!string.IsNullOrEmpty(Permission))
                {
                    char[] charArray = Permission.ToCharArray();
                    Array.Reverse(charArray);
                    return Convert.ToInt64(new string(charArray), 2);
                }
                else
                {
                    return 0;
                }

            }
        }

		public LazyCommunityModulePermission()
		{
		}
	}
}