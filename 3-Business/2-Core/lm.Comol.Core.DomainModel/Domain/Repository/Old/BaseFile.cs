
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class BaseFile : DomainObject<System.Guid>, iBaseFile
	{
        private iMetaData _MetaData;
		public virtual string Name{ get; set; }
		public virtual string DisplayName {
			get { return this.Name + this.Extension; }
		}
		public virtual string Extension { get; set; }
		public virtual string Description { get; set; }
		public virtual string FileSystemName {
			get { return base.Id.ToString() + ".stored"; }
		}
		public virtual long Size{ get; set; }
		public virtual string ContentType{ get; set; }
		public virtual iMetaData MetaInfo {
			get {
				if ((this._MetaData == null)) {
					this._MetaData = new MetaData();
				}
				return this._MetaData;
			}
			set { this._MetaData = value; }
		}
		public virtual iPerson Owner{ get; set; }
		public virtual long HardLink { get; set; }
		public virtual double SizeKB {
			get { return Math.Round((double)Size / 1024, 2); }
		}

		public virtual double SizeMB {
            get { return Math.Round((double)Size / (1024 * 1024), 2); }
		}
		public virtual bool IsDownloadable{ get; set; }
        public virtual bool IsScormFile { get; set; }

		public BaseFile()
		{
			this.IsScormFile = false;
			this.IsDownloadable = false;
		}
	}
}