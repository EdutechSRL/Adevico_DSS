using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoContainerQuota
    {
        private long _AvailableSize;
        private long _MaxAvailableSize;
        private long _MaxUploadFileSize;
        private OverflowAction _Overflow;
        private OverflowAction _UploadOverflow;
        private Boolean _IsRepository;
        public virtual long UsedSize { get; set; }
        public virtual long AvailableSize { get { return _AvailableSize; } }
        public virtual long FreeSpace { get { return (AvailableSize >= UsedSize) ? AvailableSize - UsedSize : 0; } }
        public virtual long MaxAvailableSize { get { return (_MaxAvailableSize > DiskSize && DiskSize != -1) ? DiskSize : _MaxAvailableSize; } }
        public virtual long MaxUploadFileSize { get { return _MaxUploadFileSize; } }

        public virtual long ExtendedFreeSize { get { return ((MaxAvailableSize >= UsedSize) && HasAllowedSpace()) ? MaxAvailableSize - UsedSize : FreeSpace; } }
        public virtual long OverSize { get { return ((AvailableSize < UsedSize)) ? UsedSize - AvailableSize : 0; } }

        public virtual long DiskSize { get; set; }
        public virtual Boolean AllowOverrideQuota { get; set; }
        public virtual Boolean IsRepository { get { return _IsRepository; } }
        public OverflowAction Overflow { get { return _Overflow; } }
        public OverflowAction UploadOverflow { get { return _UploadOverflow; } }
        public dtoContainerQuota()
        {
            _Overflow = OverflowAction.None;
            DiskSize = -1;
            _MaxAvailableSize = 0;
            _AvailableSize = 0;
            UsedSize = 0;
            _MaxUploadFileSize = 0;
            _UploadOverflow = OverflowAction.None;
        }
        public void Initialize(DiskSettings settings,Boolean overrideQuota, long usedSize = 0)
        {
            AllowOverrideQuota = overrideQuota;
            UsedSize = usedSize;
            _Overflow = settings.RepositoryOverflow;
            _AvailableSize = settings.AdditionalSpace + settings.AvailableSpace;
            _MaxAvailableSize = settings.MaxSpace;
            if (_MaxAvailableSize > DiskSize && DiskSize != -1)
                _MaxAvailableSize = DiskSize;
            
            _IsRepository = true;
            switch (settings.RepositoryOverflow)
            {
                case OverflowAction.Allow:
                case OverflowAction.AllowWithWarning:
                    break;
                default:
                    if (!overrideQuota)
                        _MaxAvailableSize = _AvailableSize;
                    break;
            }
            _MaxUploadFileSize = settings.MaxUploadFileSize;
            _UploadOverflow = settings.UploadOverflow;
            switch (settings.UploadOverflow)
            {
                case OverflowAction.Allow:
                case OverflowAction.AllowWithWarning:
                    break;
                default:
                    if (_MaxUploadFileSize > AvailableSize)
                        _MaxUploadFileSize = AvailableSize;
                    break;
            }
            if (_MaxUploadFileSize > MaxAvailableSize)
                _MaxUploadFileSize = MaxAvailableSize;
            
        }
        public void InitializeFromFather(dtoContainerQuota fatherQuota, long usedSize = 0)
        {
            _Overflow = fatherQuota.Overflow;
            _UploadOverflow = fatherQuota.UploadOverflow;
            DiskSize = fatherQuota.DiskSize;
            UsedSize = usedSize;

            _AvailableSize = fatherQuota.AvailableSize - fatherQuota.UsedSize + usedSize;
            if (_AvailableSize < 0)
                _AvailableSize = 0;
            if (fatherQuota.IsRepository)
                _MaxAvailableSize = (fatherQuota.AllowOverrideQuota) ? fatherQuota.MaxAvailableSize : fatherQuota.AvailableSize;
            else
                _MaxAvailableSize = fatherQuota.MaxAvailableSize;

            _MaxUploadFileSize = fatherQuota.MaxUploadFileSize;
        }

        public Boolean HasAllowedSpace()
        {
            switch (Overflow)
            {
                case OverflowAction.Allow:
                    return FreeSpace > 0 || MaxAvailableSize > UsedSize || DiskSize > UsedSize;
                case OverflowAction.AllowWithWarning:
                    return FreeSpace > 0 || MaxAvailableSize > UsedSize || DiskSize > UsedSize;
                case OverflowAction.None:
                    return FreeSpace > 0;
                case OverflowAction.NotAllow:
                    return FreeSpace > 0;
            }
            return false;
        }

        public static dtoContainerQuota Create(dtoContainerQuota fatherQuota, long usedSize = 0)
        {
            dtoContainerQuota dto = new dtoContainerQuota();
            dto.InitializeFromFather(fatherQuota, usedSize);
            return dto;
        }

        public String GetAvailableSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(AvailableSize, decimals);
        }
        public String GetPercentageSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes((long)0, decimals); 
        }
        public String GetMaxAvailableSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(MaxAvailableSize, decimals);
        }
        public String GetFreeSpace(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(FreeSpace, decimals);
        }
        public String GetFolderMaxUploadFileSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(MaxUploadFileSize, decimals);
        }
        public String GetOverSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(OverSize, decimals);
        }

    }
}