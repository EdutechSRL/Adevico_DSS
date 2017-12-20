

using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoModuleUploadedFile
	{
		public ModuleActionLink Link  {get;set;}
		public ModuleInternalFile File  {get;set;}
		public ItemRepositoryStatus Status  {get;set;}
        public string SavedFilePath { get; set; }

		public dtoModuleUploadedFile()
		{
		}
		public dtoModuleUploadedFile(ModuleInternalFile pFile, ItemRepositoryStatus pStatus)
		{
			this.File = pFile;
			this.Status = pStatus;
		}
	}
}