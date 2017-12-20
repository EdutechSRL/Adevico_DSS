
namespace lm.Comol.Core.DomainModel
{
	public interface iCoreItemPermission
	{
		bool AllowDelete { get; set; }
		bool AllowVirtualDelete { get; set; }
		bool AllowUnDelete { get; set; }
		bool AllowEdit { get; set; }
		bool AllowAddFiles { get; set; }
		bool AllowView { get; set; }
		bool AllowViewFiles { get; set; }
		bool AllowViewStatistics { get; set; }
		bool AllowFilePublish { get; set; }
	}
}