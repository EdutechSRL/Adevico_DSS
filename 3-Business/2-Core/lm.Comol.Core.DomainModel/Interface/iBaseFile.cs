using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iBaseFile : lm.Comol.Core.DomainModel.iDomainObject<Guid>
	{

		string Name { get; set; }
		string DisplayName { get; }
		string Extension { get; set; }
		string Description { get; set; }
		string FileSystemName { get; }
		long Size { get; set; }
		string ContentType { get; set; }
		iMetaData MetaInfo { get; set; }
		iPerson Owner { get; set; }
		long HardLink { get; set; }
		double SizeKB { get; }
		double SizeMB { get; }
		bool IsScormFile { get; set; }
		bool IsDownloadable { get; set; }
	}
}