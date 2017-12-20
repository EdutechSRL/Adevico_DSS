using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class litePageSettings: lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable, IDisposable 
    {
        public virtual DashboardViewType Type { get; set; }
        /// <summary>
        /// Max items visible on page, <1 all available
        /// </summary>
        public virtual Int32 MaxItems { get; set; }
        public virtual Int32 MaxMoreItems { get; set; }
        public virtual PlainLayout PlainLayout { get; set; }
        public virtual TileLayout TileLayout { get; set; }
        public virtual Int32 MiniTileDisplayItems { get; set; }
        public virtual Boolean AutoUpdateLayout { get; set; }
        public virtual DisplayNoticeboard Noticeboard { get; set; }
        public virtual RangeSettings Range { get; set; }
        public virtual DisplayMoreItems More { get; set; }
        public virtual DashboardViewType TileRedirectOn { get; set; }
        public virtual Boolean ExpandOrganizationList { get; set; }
        public virtual Boolean DisplayAsTile { get; set; }
        public litePageSettings()
        {
            More = DisplayMoreItems.AsLink;
            TileRedirectOn = DashboardViewType.Combined;
            Range = new RangeSettings();
            DisplayAsTile = false;
        }

        public virtual object Clone()
        {
            litePageSettings item = new litePageSettings();
            item.Type = Type;
            item.MaxItems = MaxItems;
            item.MaxMoreItems = MaxMoreItems;
            item.MiniTileDisplayItems = MiniTileDisplayItems;
            item.AutoUpdateLayout = AutoUpdateLayout;
            item.More = More;
            item.TileRedirectOn = TileRedirectOn;
            item.Range = (RangeSettings)Range.Clone();
            item.Deleted = Deleted;
            item.Id = Id;
            item.PlainLayout = PlainLayout;
            item.TileLayout = TileLayout;
            item.DisplayAsTile = DisplayAsTile;
            item.ExpandOrganizationList = ExpandOrganizationList;
            return item;
        }

        public void Dispose()
        {
            
        }
    }
}
