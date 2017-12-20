
using System.Runtime.Serialization;
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable(), Flags(), DataContract()]
	public enum StandardActionType
	{
		[EnumMemberAttribute()]
		None = 0,
		[EnumMemberAttribute()]
		Play = 1,
		[EnumMemberAttribute()]
		Edit = 2,
		[EnumMemberAttribute()]
		VirtualDelete = 4,
		[EnumMemberAttribute()]
		Delete = 8,
		[EnumMemberAttribute()]
		VirtualUndelete = 16,
		[EnumMemberAttribute()]
		Admin = 32,
		[EnumMemberAttribute()]
		Create = 64,
		[EnumMemberAttribute()]
		ViewPersonalStatistics = 128,
		[EnumMemberAttribute()]
		ViewAdvancedStatistics = 256,
		[EnumMemberAttribute()]
		EditPermission = 512,
		[EnumMemberAttribute()]
		EditMetadata = 1024,
		[EnumMemberAttribute()]
		ViewUserStatistics = 2048,
		[EnumMemberAttribute()]
		ViewDescription = 4096,
        [EnumMemberAttribute()]
        DownloadItem = 8192,
        [EnumMemberAttribute()]
        ViewAdministrationCharts = 16384,
        [EnumMemberAttribute()]
        ViewUserCharts = 32768,
        [EnumMemberAttribute()]
        ModuleStatistics = 65536,
        [EnumMemberAttribute()]
        ViewPreview = 131072,
	}
}