using System;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
	[CLSCompliant(true), Serializable()]
	public class dtoExternalModuleInitializer
	{
        public Boolean OnModalPage { get; set; }
        public Boolean RefreshContainerPage { get; set; }
        public String OpenLinkCssClass { get; set; }
        public Boolean SaveLinkStatistics { get; set; }
        public ModuleLink Link { get; set; }
        public List<dtoPlaceHolder> PlaceHolders{ get; set; }
		public dtoExternalModuleInitializer()
		{
            GenerateDto(null, new List<dtoPlaceHolder>());
		}
        public dtoExternalModuleInitializer(ModuleLink link) : this(link, new List<dtoPlaceHolder>())
        {
        }
        public dtoExternalModuleInitializer(ModuleLink link, List<dtoPlaceHolder>  placeHolders)
        {
            GenerateDto(link,placeHolders);
        }
        private void GenerateDto(ModuleLink link) {
            GenerateDto(link, new List<dtoPlaceHolder>());
        }
        private void GenerateDto(ModuleLink link ,List<dtoPlaceHolder> placeHolders)
        {
            RefreshContainerPage = true;
            SaveLinkStatistics = true;
            Link = link;
            PlaceHolders = placeHolders;
        }
	}
}