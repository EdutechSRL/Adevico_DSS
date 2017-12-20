
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ModuleCommunityPermission<T>
	{
		private T _Module;

		private int _ID;
		public T Permissions {
			get { return _Module; }
			set { _Module = value; }
		}
		public int ID {
			get { return _ID; }
			set { _ID = value; }
		}
	}
}