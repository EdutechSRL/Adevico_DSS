using System;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
    [CLSCompliant(true), Serializable()]
    public class dtoObjectRenderInitializer
    {
        public Boolean SetPreviousPage { get; set; }
        public Boolean SaveObjectStatistics { get; set; }
        public Boolean SaveOwnerStatistics { get; set; }
        public Boolean ForceOnModalPage { get; set; }
        public Boolean SetOnModalPageByItem { get; set; }
        public Boolean RefreshContainerPage { get; set; }
        public liteModuleLink Link { get; set; }
        public List<dtoPlaceHolder> PlaceHolders { get; set; }
        public dtoObjectRenderInitializer()
        {
            PlaceHolders = new List<dtoPlaceHolder>();
            ForceOnModalPage = false;
            SetOnModalPageByItem = true;
            RefreshContainerPage = true;
            SaveObjectStatistics = true;
            SaveOwnerStatistics = false;
        }
    }
}