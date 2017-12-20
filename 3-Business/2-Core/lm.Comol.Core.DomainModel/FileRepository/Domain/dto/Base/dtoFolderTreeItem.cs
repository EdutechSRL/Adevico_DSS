using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoFolderTreeItem
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public virtual List<dtoFolderTreeItem> Children { get; set; }
        public virtual long Size { get; set; }
        public virtual long VersionsSize { get; set; }
        public virtual long DeletedSize { get; set; }
        public virtual long FullSize { get { return Size + VersionsSize + DeletedSize; } }

        public virtual long FreeSize { get { return (Quota.AvailableSize >= FullSize) ? Quota.AvailableSize - FullSize : 0; } }
        public virtual long ExtendedFreeSize { get { return ((Quota.MaxAvailableSize >= FullSize) && Quota.HasAllowedSpace()) ? Quota.MaxAvailableSize - FullSize : FreeSize; } }
        public virtual Boolean UploadAvailable { get; set; }
        public virtual Boolean MoveIntoAvailable { get { return Quota.HasAllowedSpace(); } }
        public virtual Boolean OverrideQuota { get; set; }

        public virtual dtoContainerQuota Quota { get; set; }

        public Boolean HasChildren { get { return Children==null || !Children.Any();}}
        public Boolean IsCurrent { get; set; }
        public Boolean IsInCurrentPath { get; set; }
        public Boolean IsHome { get; set; }
        public Boolean HasRepositorySettings { get; set; }

        public String GetSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(Size, decimals);
        }
        public String GetFullSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(FullSize, decimals);
        }
        public String GetMaxSize(Int32 decimals = 2)
        {
            if (FreeSize > 0 || !Quota.HasAllowedSpace())
                return FolderSizeItem.FormatBytes(Quota.AvailableSize, decimals);
            else
                return FolderSizeItem.FormatBytes(Quota.MaxAvailableSize, decimals);
        }
        public String GetPossibleOverSize(long size, Int32 decimals = 2)
        {
            long newSize = FullSize + size;
            if (FreeSize > 0 || !Quota.HasAllowedSpace())
                return FolderSizeItem.FormatBytes((Quota.AvailableSize >= newSize) ? 0 : newSize - Quota.AvailableSize, decimals);
            else
                return FolderSizeItem.FormatBytes((Quota.MaxAvailableSize >= newSize) ? 0 : newSize - Quota.MaxAvailableSize, decimals);
        }
        public Boolean ValidateSpace(long size, Boolean forUpload)
        {
            Boolean result = false;
            if (forUpload)
            {
                result = ValidateSpace(size, false);
                if (!UploadAvailable || !result)
                    return false;
                else if (size <= Quota.MaxUploadFileSize)
                    return UploadAvailable;
                else
                {
                    switch (Quota.UploadOverflow)
                    {
                        case OverflowAction.Allow:
                        case OverflowAction.AllowWithWarning:
                            return size <= ExtendedFreeSize;
                    }
                }

            }
            else
            {
                if (Quota.MaxAvailableSize == -1)
                    return MoveIntoAvailable;
                else if (FreeSize - size > 0 || (OverrideQuota && ExtendedFreeSize >0))
                    return MoveIntoAvailable;
                else
                {
                    switch (Quota.Overflow)
                    {
                        case OverflowAction.Allow:
                        case OverflowAction.AllowWithWarning:
                            return MoveIntoAvailable;
                        default:
                            return false;
                    }
                }
            }
            return result;
        }
        public Boolean ValidateUploadSize(long size)
        {
            Boolean result = false;
            if (size <= Quota.MaxUploadFileSize)
                return UploadAvailable;
            else
            {
                switch (Quota.UploadOverflow)
                {
                    case OverflowAction.Allow:
                    case OverflowAction.AllowWithWarning:
                        return size <= ExtendedFreeSize;
                }
            }
            return result;
        }
        public dtoFolderTreeItem()
        {
            Children = new List<dtoFolderTreeItem>();
            HasRepositorySettings = true;
        }
        public String ToString()
        {
            return Id.ToString() + " " + Name + "(" + GetSize() + ")";
        }
        public List<dtoFolderTreeItem> GetAll()
        {
            List<dtoFolderTreeItem> items = new List<dtoFolderTreeItem>();
            items.Add(this);
            if (Children.Any())
                items.AddRange(Children.SelectMany(c => c.GetAll()).ToList());
            return items;
        }
    }
}