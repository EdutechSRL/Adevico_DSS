using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewModuleTextAction : IGenericModuleDisplayAction
    {
        lm.Comol.Core.DomainModel.ContentView PreLoadedContentView {get;}
        String DestinationUrl {get;}
        String GetBaseUrl();
        Boolean InsideOtherModule {get;set;}
        Boolean RefreshContainer { get; set; }
        String ItemIdentifier {get;set;}
        Int32 ForUserId {get;set;}

        String GetBaseUrl(Boolean useSSL);
        void DisplayUnknownAction();
        void DisplayEmptyAction();
        void DisplayEmptyActions();
        void DisplayActions(List<dtoModuleActionControl> actions);
        void DisplayPlaceHolders(List<dtoPlaceHolder> items);
        void DisplayItem(string actionText);
        void DisplayItem(long idItem, string actionText);
        //event EventHandler<RefreshContainerArgs> RefreshContainerEvent;

    }
}