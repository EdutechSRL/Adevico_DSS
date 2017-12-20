using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class TransferToNewItemTable
    {
        public virtual long Id {get; set;}
        public virtual long IdFolder {get; set;}
        public virtual Guid UniqueId {get; set;}
        public virtual long IdPerson { get; set; }
        public virtual long IdCommunity { get; set; }
        public virtual String Name {get; set;}
        public virtual String Extension {get; set;}
        public virtual String Description {get; set;}
        public virtual String ContentType {get; set;}
        public virtual long IdPlayer { get; set; }
        public virtual long Size {get; set;}
        public virtual long VersionsSize { get; set; }
        public virtual long DeletedSize { get; set; }
        public virtual long Downloaded {get; set;}
        public virtual Boolean IsFile {get; set;}
        public virtual ItemType Type {get; set;}
        public virtual long IdVersion {get; set;}
        public virtual Guid UniqueIdVersion {get; set;}
        public virtual ItemAvailability Availability { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual DisplayMode DisplayMode { get; set; }
        public virtual String Thumbnail { get; set; }
        public virtual String Url { get; set; }
        public virtual Boolean AutoThumbnail { get; set; }
        public virtual long PreviewTime { get; set; }
        public virtual long Time { get; set; }
        public virtual long Number {get; set;}
        public virtual String Tags {get; set;}
        public virtual long CloneOf {get; set;}
        public virtual Guid CloneOfUniqueId { get; set; }


         public virtual Boolean HasVersions {get; set;}
         public virtual Boolean IsVisible {get; set;}
         public virtual Boolean IsDownloadable {get; set;}
        public virtual Boolean IsInternal {get; set;}
        public virtual Boolean IsVirtual {get; set;}
        public virtual Boolean IsPersonal { get; set; }

        
        public virtual long DisplayOrder { get; set; }

        public virtual ItemModuleSettings Module { get; set; }

	 public virtual Int32 _Deleted {get; set;}
        public virtual DateTime _CreatedOn {get; set;}
        public virtual Int32 _CreatedBy {get; set;}
        public virtual DateTime _ModifiedOn {get; set;}
        public virtual Int32 _ModifiedBy {get; set;}
        public virtual String _CreatedProxyIPaddress {get; set;}
        public virtual String _CreatedIPaddress {get; set;}
        public virtual String _ModifiedIPaddress {get; set;}
        public virtual String _ModifiedProxyIPaddress { get; set; }

        public virtual Boolean AllowUpload { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }

        public TransferToNewItemTable()
        {
            Repository = new RepositoryIdentifier();
        }


        public virtual TransferVersion CreateFirstVersion()
        {
            TransferVersion version = new TransferVersion();
            version.Availability = Availability;
            version._CreatedOn = _CreatedOn;
            version._CreatedBy = _CreatedBy;
            version._ModifiedBy = _ModifiedBy;
            version._ModifiedOn = _ModifiedOn;
            version._CreatedProxyIPaddress = _CreatedProxyIPaddress;
            version._CreatedIPaddress = _CreatedIPaddress;
            version._ModifiedIPaddress = _ModifiedIPaddress;
            version._ModifiedProxyIPaddress = _ModifiedProxyIPaddress;
            version.IdItem = Id;
            version.UniqueIdItem = UniqueId;
            version.UniqueIdVersion = UniqueId;
            version._Deleted = 0;
            version.DisplayMode = (int)DisplayMode;
            version.Downloaded = Downloaded;
            version.IdCommunity = (int)IdCommunity;
            version.IdPlayer = IdPlayer;
            version.IsActive = true;
            version.Number = 0;
            version.IdPerson = (int)IdPerson;
            version.PreviewTime = PreviewTime;
            version.Size = Size;
            version.Status = Status;
            version.Thumbnail = Thumbnail;
            version.AutoThumbnail = AutoThumbnail;
            version.Time = Time;
            version.ItemType = (int)Type;
            version.UniqueIdItem = UniqueId;
            version.UniqueIdVersion = UniqueId;
            version.Name = Name;
            version.Description = Description;
            version.Url = Url;
            version.Extension = Extension;
            version.ContentType = ContentType;
            version.Repository = Repository;
            return version;
        }
    }



    [Serializable]
    public class NewliteModuleLink{
          public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
        public virtual long Id { get; set; }
        public virtual Int32 IdAction { get; set; }
        public virtual long Permission { get; set; }
        public virtual Boolean AutoEvaluable { get; set; }
        public virtual Boolean NotifyExecution { get; set; }
        public virtual String SourceModule { get; set; }
        public virtual String DestinationModule { get; set; }
        public virtual String FullName { get; set; }
        public virtual Int32 IdObjectype { get; set; }
        public virtual long IdObject { get; set; }
        public virtual long IdObjectVersion { get; set; }
    }
}