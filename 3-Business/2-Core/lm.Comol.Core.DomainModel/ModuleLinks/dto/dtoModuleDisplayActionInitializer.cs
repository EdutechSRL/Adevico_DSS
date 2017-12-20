using System;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
	[CLSCompliant(true), Serializable()]
	public class dtoModuleDisplayActionInitializer
	{
        public Boolean SaveLinkStatistics { get; set; }
        public Boolean OnModalPage { get; set; }
        public Boolean RefreshContainerPage { get; set; }
        public String OpenLinkCssClass { get; set; }
        public ModuleLink Link { get; set; }
        public DisplayActionMode Display { get; set; }
        public String ContainerCSS { get; set; }
        public List<dtoPlaceHolder> PlaceHolders{ get; set; }
        public lm.Comol.Core.DomainModel.Helpers.IconSize IconSize { get; set; }
		public dtoModuleDisplayActionInitializer()
		{
            Display = DisplayActionMode.defaultAction;
            ContainerCSS = "";
            PlaceHolders = new List<dtoPlaceHolder>();
            IconSize = DomainModel.Helpers.IconSize.Medium;
            OnModalPage = false;
            OpenLinkCssClass = "";
            RefreshContainerPage = true;
            SaveLinkStatistics = true;
		}

        public static dtoModuleDisplayActionInitializer Create(dtoExternalModuleInitializer dto, DisplayActionMode display,String containerCSS,lm.Comol.Core.DomainModel.Helpers.IconSize size){
            dtoModuleDisplayActionInitializer item = new dtoModuleDisplayActionInitializer();
            item.ContainerCSS = containerCSS;
            item.Display = display;
            item.Link = dto.Link;
            item.PlaceHolders = dto.PlaceHolders;
            item.IconSize = size;
            item.RefreshContainerPage = dto.RefreshContainerPage;
            item.OpenLinkCssClass = dto.OpenLinkCssClass;
            item.OnModalPage = dto.OnModalPage;
            item.SaveLinkStatistics = dto.SaveLinkStatistics;
            return item;
        }

        public dtoExternalModuleInitializer ConvertTo()
        {
            dtoExternalModuleInitializer item = new dtoExternalModuleInitializer();
            item.Link = this.Link;
            item.PlaceHolders = this.PlaceHolders;
            item.RefreshContainerPage = this.RefreshContainerPage;
            item.OpenLinkCssClass = this.OpenLinkCssClass;
            item.OnModalPage = this.OnModalPage;
            item.SaveLinkStatistics = this.SaveLinkStatistics;
            return item;
        }
	}
}