using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoFolderSize
    {
        public virtual long IdFolder { get; set; }
        public virtual FolderType FolderType { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual long Size {get;set;}
        public virtual long FreeSpace { get { return (Quota!=null && Quota.AvailableSize >= Size) ? Quota.AvailableSize - Size : 0; } }
        public virtual long OverSize { get { return (Quota==null) ? 0 : ((Quota != null && Quota.AvailableSize >= Size) ? 0 : Size - Quota.AvailableSize); } }

        public virtual dtoContainerQuota Quota { get; set; }
        public virtual List<FolderSizeItem> Items  {get;set;}
        public virtual Boolean UploadAvailable { get; set; }
        public virtual String Name { get; set; }
        public String IdentifierPath { get; set; }
        public dtoFolderSize(){
            Items = new List<FolderSizeItem>();
        }

        public String GetSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(Size, decimals);
        }
        public String GetFreeSpace(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(FreeSpace, decimals);
        }
        public String GetOverSize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(OverSize, decimals);
        }
        public virtual List<FolderSizeItem> GetForProgressBar()
        {
            return (Items == null) ? new List<FolderSizeItem>() : Items.Where(i => (FolderType == Domain.FolderType.recycleBin || i.Type != FolderSizeItemType.folder) && i.Type != FolderSizeItemType.overflow && i.Type != FolderSizeItemType.link).ToList();
        }

        public void SetPercentage()
        {
            long size = (Quota.AvailableSize > Size) ? Quota.AvailableSize : Size;
            if (size <= 0)
                size = Items.Select(i => i.Size).Sum();
            if (size > 0 && Items.Any(i => i.Type != FolderSizeItemType.overflow && i.Size > 0))
            {
                Items.Where(i=>i.Type != FolderSizeItemType.overflow).ToList().ForEach(i=> i.SetPercentage(size));
                if (Items.Where(i => i.Type != FolderSizeItemType.overflow).Select(i => i.IntPercentage).Sum() != 100)
                {
                    FolderSizeItemType type = Items.Where(i=> i.Type != FolderSizeItemType.overflow && i.Size>0).LastOrDefault().Type;
                    if (Items.Where(i => i.Type != FolderSizeItemType.overflow).Count() == 1)
                        Items.Where(i => i.Type != FolderSizeItemType.overflow).FirstOrDefault().SetPercentage(100, 100);
                    else
                        Items.Where(i => i.Type != FolderSizeItemType.overflow).LastOrDefault().SetPercentage(100 - Items.Where(i => i.Type != FolderSizeItemType.overflow && i.Type != type).Select(i => i.IntPercentage).Sum(), 100 - Items.Where(i => i.Type != FolderSizeItemType.overflow && i.Type != type).Select(i => i.Percentage).Sum());
                }
            }
        }


    }

    [Serializable]
    public class FolderSizeItem
    {
        private Decimal percentage;
        private Int32 intPercentage;
        public virtual Decimal Percentage { get {return percentage;}}
        public virtual Int32 IntPercentage { get {return intPercentage;}}
        
        public virtual long Number {get;set;}
        public virtual long Size {get;set;}
        public virtual FolderSizeItemType Type {get;set;}
        public String DisplaySize(Int32 decimals = 2)
        {
            return FolderSizeItem.FormatBytes(Size, decimals);
        }
        public static String FormatBytes(long bytes, Int32 decimals = 2)
        {
            if (bytes <= 0) return "0 Byte";
            Int32 k = 1000;
            var sizes = new List<String>() { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            Int32 i = (Int32)Math.Floor(Math.Log(bytes) / Math.Log(k));
            string size = (bytes / Math.Pow(k, i)).ToString("N" + decimals);
            if (size.EndsWith(",00"))
                size = size.Remove(size.Length - 3);
            return  size + ' ' + sizes[i];
        }

        public void SetPercentage(long totalSize){
            if (totalSize<=0){
                intPercentage=0;
                percentage=0;
            }
            else{
                Double p = ((double)Size / (double)totalSize) * 100;
                intPercentage =  Convert.ToInt32(Math.Floor(p));
                if (Size > 0 && intPercentage == 0)
                    intPercentage = 1;
                percentage = Convert.ToDecimal(p);
            }
        }
        public void SetPercentage(Int32 iPercentage, decimal dPercentage)
        {
            intPercentage = iPercentage;
            percentage = dPercentage;
        }
        public String ToString()
        {
            return Type.ToString() + " Number: " + Number.ToString() + " Size: " + DisplaySize();
        }
    }

    [Serializable]
    public enum FolderSizeItemType : int
    {
        file = 0,
        folder = 1,
        link = 2,
        version = 3,
        deleted = 4,
        freespace = 5,
        overflow = 6,
        unavailableItems = 7,
        deletedonsubfolders =8,
        onchildrenfolders = 9,
        fullSize = 10
    }
}