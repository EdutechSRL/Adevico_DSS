using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBaseCallList<T> : IViewBase
    {
        CallStandardAction PreloadAction { get; }
        CallStatusForSubmitters PreloadView { get; }
        int PreloadIdCommunity { get; }
        String Portalname { get; }
        int IdCallCommunity { get; set; }
       

        FilterCallVisibility CurrentFilter { get; set; }
        CallStatusForSubmitters CurrentView { get; set; }
        CallStandardAction CurrentAction { get; set; }
        PagerBase Pager { get; set; }

        Boolean AllowManage { get; set; }
        Boolean AllowView { get; set; }
        Boolean AllowSubmmissions { get; set; }

        void SetContainerName(int idCommunity, String name);
        void SetActionUrl(CallStandardAction action, String url);
        void LoadAvailableView(int idCommunity, List<CallStatusForSubmitters> views);
        void LoadAvailableFilters(List<FilterCallVisibility> filters, FilterCallVisibility current );
        void LoadCalls(List<T> items);
        void SendUserActionList(int idCommunity, int idModule);
        void CloneSkinAssociation(int idUser,Int32 idModule, Int32 idCommunity, long idOldModuleItem, long idNewModuleItem, Int32 idItemType, String fullyQualifiedName);
        void RemoveSkinAssociation(int idUser, Int32 idModule, Int32 idCommunity, long idCall,Int32 idItemType, String fullyQualifiedName);
        void DisplayUnableToDeleteCall();
        void DisplayUnableToDeleteUnknownCall();
        void GotoUrl(String url);
    }
}