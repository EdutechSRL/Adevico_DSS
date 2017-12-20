using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewGenericFieldsMatcher : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean isValid { get; }
        List<ExternalColumnComparer<String, Int32>> SourceFields { get; }
        List<DestinationItem<Int32>> DestinationFields { get; set; }
        //List<DestinationItem<Int32>> RequiredFields { get; set; }

        void InitializeControl(List<ExternalColumnComparer<String, Int32>> columns, List<DestinationItem<Int32>> fields);
        //void InitializeControl(List<ExternalColumnComparer<String, Int32>> columns, List<DestinationItem<Int32>> aFields, List<DestinationItem<Int32>> rFields);
        void LoadItems(List<ExternalColumnComparer<String, Int32>> source);
        void DisplaySessionTimeout();
    }
}