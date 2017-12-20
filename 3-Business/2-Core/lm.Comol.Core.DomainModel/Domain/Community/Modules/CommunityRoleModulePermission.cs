using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityRoleModulePermission : DomainObject<int>, iCommunityRoleModulePermission
	{
        public virtual Community Community { get; set; }
        public virtual Role Role { get; set; }
        public virtual ModuleDefinition Service { get; set; }
        public virtual string PermissionString { get; set; }
		public virtual long PermissionInt {
			get {
                if (!string.IsNullOrEmpty(PermissionString))
                {
                    char[] charArray = PermissionString.ToCharArray();
                    Array.Reverse(charArray);
                    return Convert.ToInt64(new string(charArray), 2);
				} else {
					return 0;
				}

			}
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik, @toddanglin
//Facebook: facebook.com/telerik
//=======================================================
