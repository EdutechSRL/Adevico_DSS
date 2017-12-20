
using lm.Comol.Core.DomainModel;
using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoUploadedFile
	{
		public ModuleActionLink Link {get;set;}
		public CommunityFile File {get;set;}
		public ItemRepositoryStatus Status  {get;set;}
        public string SavedFilePath { get; set; }

		public dtoUploadedFile()
		{
		}
		public dtoUploadedFile(CommunityFile pFile, ItemRepositoryStatus pStatus)
		{
			this.File = pFile;
			this.Status = pStatus;
		}
	}
}