
using System;
using lm.Comol.Core.DomainModel.Repository;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class BaseCommunityFile: DomainObject<long>
    {
        public virtual Boolean IsInternal {get;set;}
        public virtual Community CommunityOwner {get;set;}
        public virtual Person Owner {get;set;}
        public virtual String Name {get;set;}
        public virtual String Extension {get;set;}
        public virtual String Description {get;set;}
        public virtual DateTime CreatedOn {get;set;}
        public virtual long Size {get;set;}
        public virtual String ContentType {get;set;}
        public virtual Boolean isVirtual {get;set;}
        public virtual Boolean isFile {get;set;}
        public virtual Boolean isSCORM {get;set;}
        public virtual Boolean isVideocast {get;set;}
        public virtual Boolean isDeleted {get;set;}
        public virtual Boolean isPersonal {get;set;}
        public virtual Boolean isVisible {get;set;}
        public virtual long CloneID {get;set;}
        public virtual long FolderId {get;set;}
        public virtual long Downloads {get;set;}
        public virtual String FilePath { get; set; }
        public virtual Person ModifiedBy {get;set;}
        public virtual int FileCategoryID {get;set;}
        public virtual DateTime? ModifiedOn {get;set;}
        public virtual System.Guid CloneUniqueID {get;set;}
        public virtual System.Guid UniqueID {get;set;}
        public virtual Boolean IsDownloadable {get;set;}
        public virtual String DisplayName {
            get{
                return Name + Extension;
            }
            set{}
        }
        public virtual FileRepositoryType Discriminator { get; set; }
        public virtual Boolean AllowUpload { get; set; }

        public virtual RepositoryItemType RepositoryItemType { get; set; }

        


        public BaseCommunityFile(){
            isPersonal = false;
            isDeleted = false;
            isVideocast = false;
            isSCORM = false;
            isVisible = true;
            IsDownloadable = true;
            CreatedOn = DateTime.Now;
            UniqueID = System.Guid.NewGuid();
            CloneUniqueID = System.Guid.Empty;
            Description = "";
            Downloads = 0;
            FilePath = "";
            AllowUpload = false;
            RepositoryItemType = RepositoryItemType.FileStandard;
        }
    }
}