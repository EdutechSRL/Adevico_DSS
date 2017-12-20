using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CoreItemPermission : iCoreItemPermission
	{
        public bool AllowAddFiles { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowUnDelete { get; set; }
        public bool AllowViewStatistics { get; set; }
        public bool AllowVirtualDelete { get; set; }
        public bool AllowFilePublish { get; set; }
        public bool AllowViewFiles { get; set; }
        public bool AllowView { get; set; }
	}
}