using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoViewSettings 
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual String Name { get; set; }
        public virtual DashboardType Type { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual Boolean ForAll { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual Boolean FullWidth { get; set; }
        public virtual dtoContainerSettings Container { get; set; }
        public virtual List<dtoPageSettings> Pages { get; set; }
        public dtoViewSettings() {
            Pages = new List<dtoPageSettings>();
            Container = new dtoContainerSettings();
        }
        public dtoViewSettings(liteDashboardSettings s)
            : this()
        {
            Id = s.Id;
            Deleted = s.Deleted;
            Name = s.Name;
            Type = s.Type;
            Status = s.Status;
            ForAll = s.ForAll;
            Active = s.Active;
            FullWidth = s.FullWidth;
            if (s.Pages.Any())
                Pages = s.Pages.Select(p =>
                            new dtoPageSettings()
                            {
                                Id = p.Id,
                                AutoUpdateLayout = p.AutoUpdateLayout,
                                DisplayAsTile = p.DisplayAsTile,
                                Deleted = p.Deleted,
                                Type = p.Type,
                                MaxItems = p.MaxItems,
                                MaxMoreItems = p.MaxMoreItems,
                                PlainLayout = p.PlainLayout,
                                TileLayout = p.TileLayout,
                                MiniTileDisplayItems = p.MiniTileDisplayItems,
                                Noticeboard = p.Noticeboard,
                                Range = p.Range,
                                More = p.More,
                                TileRedirectOn = p.TileRedirectOn,
                                ExpandOrganizationList = p.ExpandOrganizationList,
                             }).ToList();
            if (s.Container != null)
            {
                Container.AvailableGroupBy= s.Container.AvailableGroupBy.ToList();
                Container.AvailableOrderBy= s.Container.AvailableOrderBy.ToList();
                Container.AvailableViews = s.Container.AvailableViews.ToList();
                Container.Default= s.Container.Default;
            }
               
        }
    }
} 