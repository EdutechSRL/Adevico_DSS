using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ModuleGuidInternalFile : ModuleInternalFile
	{

		private System.Guid _ObjectID;
		public virtual System.Guid ObjectID {
			get { return _ObjectID; }
			set { _ObjectID = value; }
		}


		public ModuleGuidInternalFile()
		{
		}

		public ModuleGuidInternalFile(ModuleInternalFile oFile)
		{
			this.IsInternal = oFile.IsInternal;
			this.CommunityOwner = oFile.CommunityOwner;
			this.Owner = oFile.Owner;
			this.Name = oFile.Name;
			this.Extension = oFile.Extension;
			this.Description = oFile.Description;
			this.CreatedOn = oFile.CreatedOn;
			this.Size = oFile.Size;
			this.ContentType = oFile.ContentType;
			this.isVirtual = oFile.isVirtual;
			this.isFile = oFile.isFile;
			this.isSCORM = oFile.isSCORM;
			this.isVideocast = isVideocast;
			this.isDeleted = oFile.isDeleted;
			this.isPersonal = oFile.isPersonal;
			this.isVisible = oFile.isVisible;
			this.CloneID = oFile.CloneID;
			this.FolderId = oFile.FolderId;
			this.Downloads = oFile.Downloads;
			this.FilePath = oFile.FilePath;
			this.ModifiedBy = oFile.ModifiedBy;
			this.ModifiedOn = oFile.ModifiedOn;
			this.FileCategoryID = oFile.FileCategoryID;
			this.UniqueID = oFile.UniqueID;
			this.CloneUniqueID = oFile.CloneUniqueID;
			this.IsDownloadable = oFile.IsDownloadable;
			this.Discriminator = oFile.Discriminator;


			this.ServiceOwner = oFile.ServiceOwner;
			this.ObjectTypeID = oFile.ObjectTypeID;
			this.ObjectOwner = oFile.ObjectOwner;
            this.RepositoryItemType = oFile.RepositoryItemType;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik, @toddanglin
//Facebook: facebook.com/telerik
//=======================================================
