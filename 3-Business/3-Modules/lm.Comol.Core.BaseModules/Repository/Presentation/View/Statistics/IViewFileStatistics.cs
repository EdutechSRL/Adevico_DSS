using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewFileStatistics : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        

        String PreloadedBackUrl { get; }
        int PreloadedModuleOwnerId { get; }
        int PreloadedObjectTypeId { get; }
        int PreloadedUserId { get; }
        int PreloadedCommunityId { get; }
        long PreloadedObjectId { get; }
        long PreloadedFileId { get; }
        long PreloadedLinkId { get; }
        Boolean PreloadedAdminView { get; }
        FileStatPages PreloadedView { get; } //Necessario il SET, poiché il presenter controlla che non ci siano "errori" ed eventualmente li corregge...
        FileStatPages CurrentStartView { get; set; } //Necessario il SET, poiché il presenter controlla che non ci siano "errori" ed eventualmente li corregge...

        int CurrentObjectCommunityId { get; set; }
        String CurrentModuleOwnerCode { get; set; }
        int CurrentObjectTypeId { get; set; }
        long CurrentObjectId { get; set; }
        long CurrentLinkId { get; set; }
        Boolean CurrentIsStatUser { get; }

        // Boolean HasSelfPermissionView(int IdCommunity,long LinkID, ModuleObject sourceObject, ModuleObject linkedObject,int UserID);

        //Saranno passati i valori necessari agli UserControl di selezione per inizializzarsi, 
        //compresi eventuali valori preselezionati, anche se finora è previsto UN solo file selezionato...


        StatTreeLeafType GetModuleLinkPermission(int IdCommunity, long LinkID, ModuleObject sourceObject, ModuleObject linkedObject, int UserID);
        void InitFileList(String moduleCode, long objectId, int objectTypeId, IList<long> FilesIds, int idCommunity);   //PRESELECT FilesIds
        void InitUserList(String moduleCode, long objectId, int objectTypeId, IList<int> UsersIds, int idCommunity);    //PRESELECT UsersIds
        IList<StatFileTreeLeaf> CurrentFiles();

        IList<int> CurrentUsers { get; }
        //    IList<Int64> CurrentFiles { get; set; }

        //Per aggiornare i dati nei controlli che visualizzano le statistiche...
        void InitFileStat(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId);
        void InitUsersStat(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId);
        void InitFileDetail(Int32 UserId, Int64 FileId, Int32 Perm);
        void InitScormDetail(Int32 UserId, Int64 FileId, Int32 Perm);

        void SendReportFile(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId, Boolean isPdf, String CommunityName);
        void SendReportUser(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId, Boolean isPdf, String CommunityName);
                           
        //Navigazione
        Boolean CurrentIsPathByUser { get; set; } //True = ByUser, False = ByFile
        FileStatPages CurrentPage { get; }
        void GoToPage(FileStatPages DestinationPage, Boolean ShowNext, Boolean ShowBack, Boolean ShowShowBy, Boolean ShowExport);

        NHibernate.ISession IcoSession { get; }
    }
}
