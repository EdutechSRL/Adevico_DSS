using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class FileStatisticsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        //private ServiceFileStatistics _Service;

        //private NHibernate.ISessionFactory _IcoFactory { get; set; }
        //string _IcoMappingPath { get; set; }

        

        //private BaseModuleManager _OldManager;
        //private BaseModuleManager OldManager
        //{
        //    get
        //    {
        //        if (_OldManager == null)
        //            _OldManager = new BaseModuleManager(AppContext);
        //        return _OldManager;
        //    }
        //}
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewFileStatistics View
        {
            get { return (IViewFileStatistics)base.View; }
        }

         public FileStatisticsPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
         public FileStatisticsPresenter(iApplicationContext oContext, IViewFileStatistics view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

         //public FileStatisticsPresenter(iApplicationContext oContext, IViewFileStatistics view, String IcoMappingPath)
         //    : base(oContext, view)
         //{
         //    //this._IcoMappingPath = IcoMappingPath; //Eventualmente sostituire con l'ISession...
         //    this.CurrentManager = new BaseModuleManager(oContext);

         //}

        //ref NHibernate.ISession ColSession, ref NHibernate.ISession IcoSession

         //private void InitPresenter()
         //{
         //    //Inizializzazioni varie...
         //}
#region Navigazione

        public void FirstAccess()
        {
            /// 
            ///     OCCHIO Mirco, se questa è l'inizializzazione della pagina qui devi definire i current..
            ///     altrimenti con il bind dati che richiama sempre e comunque le cose che trova via url
            ///     uno sgamato potrebbe comunque fregarti in uno dei vari giri che fai passando
            ///     avanti o indietro visto che sia per l'inizalizzazione che per la navigazione
            ///     chiami sempre i bind dati sia per file che per utenti
            ///     di conseguenza io ho aggiunto questa parte perchè via url passo solo l'id del modulo,
            ///     all'inizializzazione dall'id trovo il code e lo memorizzo nel viewstate
            ///     della pagina. poi agli inizializzatori dei selettori passi sto valore memorizzato 
            ///     e non quello caricato da url
            /// 
            ///     @Francesco:
            ///     <Mirco>
            ///     Certo, ma questo riguarda SOLO le logiche di navigazione (avanti-indietro, salti, etc...)
            ///     La parte di "controllo" l'avevo parzialmente saltata,
            ///     anche perchè abbiamo comunque un secondo controllo a livello di UC di visualizzazione.
            ///     Anche modificando i parametri via URL, se l'utente corrente NON HA permessi di selezionare
            ///     file, nel selettore NON  HA FILE, se anche seleziona file ed utenti, ma non puo' vedere
            ///     statistiche, NON vengono mostrate.
            ///     
            ///     E' più carino per l'utente essere "bloccato" subito, ma qui abbiamo il casino
            ///     del calcolo dei permessi iniziali.
            ///     


            View.CurrentModuleOwnerCode = CurrentManager.GetModuleCode(View.PreloadedModuleOwnerId); 

            int idCommunity = View.PreloadedCommunityId;
            if (idCommunity==0)
                idCommunity= UserContext.CurrentCommunityID;

            View.CurrentObjectCommunityId= idCommunity;
            View.CurrentLinkId = View.PreloadedLinkId;
            View.CurrentObjectId=View.PreloadedObjectId;
            View.CurrentObjectTypeId= View.PreloadedObjectTypeId;
            ModuleObject remoteObject = null; // CONTINUO DOMANI MATTINA QUI= ModuleObject.CreateLongObject(View.CurrentObjectId, View.CurrentObjectTypeId,View.CurrentModuleOwnerCode
           // StatTreeLeafType permission=  View.GetModuleLinkPermission(idCommunity,View.CurrentLinkId,remoteObject,null,UserContext.CurrentUserID);
            

            //  <Mirco> Modificata la IF sottostante...
            //  Nella VIEW al limite controllo permessi (quello che ti dicevo ieri)
            //  che mi modifica uno qualunque dei parametri sottostanti.
            if ((View.PreloadedView != FileStatPages.NoPermission) 
                && (View.CurrentStartView != FileStatPages.NoPermission )
                && (View.CurrentPage != FileStatPages.NoPermission))
            {
                //FileStatPages PageToGo;

                if (!View.PreloadedAdminView)   //Utente <- Modificare IsForAdmin....
                {
                    if ((View.CurrentStartView == FileStatPages.Start) || (View.CurrentStartView == FileStatPages.SelectUsers))
                    {
                        View.CurrentStartView = FileStatPages.SelectFiles;
                        //this.BindStatisticsFiles();
                        this.View.CurrentIsPathByUser = true;
                        View.CurrentStartView = FileStatPages.SelectFiles;
                    }
                    else
                    {
                        View.CurrentStartView = View.PreloadedView;
                    }
                    View.CurrentIsPathByUser = true;
                }
                else
                {
                    //Amministratore...
                    View.CurrentStartView = View.PreloadedView;

                    switch (View.CurrentStartView)
                    { 
                        case FileStatPages.SelectUsers:
                            this.BindSelectionUsers();
                            View.CurrentIsPathByUser = false;
                            break;
                        case FileStatPages.SelectFiles:
                            this.BindSelectionFiles();
                            View.CurrentIsPathByUser = true;
                            break;
                        case FileStatPages.StatsUsers:
                            this.BindStatisticsUsers();
                            View.CurrentIsPathByUser = true;
                            break;
                        case FileStatPages.StatsFiles:
                            this.BindStatisticsFiles();
                            View.CurrentIsPathByUser = false;
                            break;
                    }

                }


                //= FileStatPages.Start

                Boolean IsStatistics = (View.CurrentStartView == FileStatPages.StatsFiles || View.CurrentStartView == FileStatPages.StatsUsers);
                View.GoToPage(View.CurrentStartView
                , (View.CurrentStartView != FileStatPages.Start) && View.CurrentStartView != FileStatPages.StatsFiles && View.CurrentStartView != FileStatPages.StatsUsers
                , false
                , IsStatistics
                , IsStatistics && this.ShowExport());
            }
            else // NON SI HANNO I PERMESSI...
            {
                View.GoToPage(FileStatPages.NoPermission, false, false, false, false);
            }
        }

         public void NextPage()
         {
             FileStatPages CurrentPage = View.CurrentPage;

             switch(CurrentPage)
                 {
                 case FileStatPages.NoPermission:
                         View.GoToPage(FileStatPages.NoPermission, false, false, false, false);
                     break;

                 //I pulsanti di ByUsers e ByFiles in "Start" settano il percorso e lanciano il NEXT del presenter
                 case FileStatPages.Start:
                        if (View.CurrentIsPathByUser)
                        {
                            BindSelectionUsers();
                        View.GoToPage(FileStatPages.SelectUsers, true, true, false, false);
                        }
                        else
                        {
                            BindSelectionFiles();
                            View.GoToPage(FileStatPages.SelectFiles, true, true, false, false);
                        }
                        break;

                 case FileStatPages.SelectUsers:
                        if (View.CurrentIsPathByUser)
                        {
                            BindSelectionFiles();
                            View.GoToPage(FileStatPages.SelectFiles, true, true,false, false);
                        }
                        else {
                            BindStatisticsFiles();
                            View.GoToPage(FileStatPages.StatsFiles, false, true, true, ShowExport());
                        }
                        break;

                 case FileStatPages.SelectFiles:
                        if (View.PreloadedAdminView)
                        {
                            if (View.CurrentIsPathByUser)
                            {
                                BindStatisticsUsers();
                                View.GoToPage(FileStatPages.StatsUsers, false, true, true, ShowExport());
                            }
                            else
                            {
                                BindSelectionUsers();
                                View.GoToPage(FileStatPages.SelectUsers, true, true, false, false);
                            }
                        }
                        else {
                            BindStatisticsUsers();
                            View.GoToPage(FileStatPages.StatsUsers, false, true, true, ShowExport());
                        }
                        break;

                 //case FileStatPages.StatsFiles:
                 //       break;
                 //case FileStatPages.StatsUsers:
                 //       break;
                 //case FileStatPages.DetailFile:
                 //       break;
                 //case FileStatPages.DetailScorm:
                 //       break;
                 default:
                     break;
                 
                 }
         }

         public void BackPage()
         {
             FileStatPages CurrentPage = View.CurrentPage;

             switch (CurrentPage)
             {
                 case FileStatPages.NoPermission:
                     View.GoToPage(FileStatPages.NoPermission, false, false, false, false);
                     break;
                 //case FileStatPages.NoPermission:
                 //    break;
                 //case FileStatPages.Start:
                 //    break;
                 case FileStatPages.SelectUsers:
                     if (View.CurrentIsPathByUser)
                     {
                         if (View.CurrentStartView == FileStatPages.Start)
                         {
                             View.GoToPage(FileStatPages.Start,false,false,false,false);
                         }
                     }
                     else 
                     {
                         View.GoToPage(FileStatPages.SelectFiles, true, (View.CurrentStartView == FileStatPages.Start), false, false);
                     }
                     break;

                 case FileStatPages.SelectFiles:
                     if (View.PreloadedAdminView)
                     {
                         if (View.CurrentIsPathByUser)
                         {
                             if (View.CurrentStartView == FileStatPages.Start || View.CurrentStartView == FileStatPages.SelectUsers)
                             {
                                 View.GoToPage(FileStatPages.SelectUsers
                                     , true
                                     , (View.CurrentStartView == FileStatPages.Start)
                                     , false
                                     , false);
                             }
                         }
                         else
                         {
                             if (View.CurrentStartView == FileStatPages.Start)
                             {
                                 View.GoToPage(FileStatPages.Start, false, false, false, false);
                             }
                         }
                     }
                     break;
                 case FileStatPages.StatsFiles:
                     
                     if (View.PreloadedAdminView)
                     {
                         if (View.CurrentIsPathByUser)
                         {
                             //View.GoToPage(FileStatPages.StatsUsers, 
                             //    false, 
                             //    (View.CurrentStartView != FileStatPages.StatsUsers && View.CurrentStartView != FileStatPages.StatsFiles), 
                             //    true, 
                             //    this.ShowExport());
                             View.GoToPage(FileStatPages.SelectFiles,
                                 true,
                                 (View.CurrentStartView == FileStatPages.Start),
                                 true,
                                 this.ShowExport());
                         }
                         else
                         {
                             if (View.CurrentStartView != FileStatPages.StatsFiles)
                             {
                                 this.View.GoToPage(FileStatPages.SelectUsers,
                                     true,
                                     (this.View.CurrentStartView == FileStatPages.Start || View.CurrentStartView == FileStatPages.SelectFiles),
                                     false,
                                     false);
                             }
                         }
                     }
                     else 
                     {
                         if (View.CurrentStartView != FileStatPages.StatsFiles)
                         {
                             this.View.GoToPage(FileStatPages.SelectFiles,
                                      true,
                                      false,
                                      false,
                                      false);
                         }
                     }
                     break;
                 case FileStatPages.StatsUsers:
                     if (View.CurrentIsPathByUser)
                     {
                         if (View.CurrentStartView != FileStatPages.StatsUsers)
                         {
                             View.GoToPage(FileStatPages.SelectFiles,
                                 true,
                                 (View.CurrentStartView == FileStatPages.Start || View.CurrentStartView == FileStatPages.SelectUsers),
                                 false,
                                 false);
                         }
                     }
                     else
                     {
                         //View.GoToPage(FileStatPages.StatsFiles,
                         //    false,
                         //    (View.CurrentStartView != FileStatPages.StatsFiles) && (View.CurrentStartView != FileStatPages.StatsUsers),
                         //    true,
                         //    this.ShowExport());
                         View.GoToPage(FileStatPages.SelectUsers,
                             true,
                             (View.CurrentStartView == FileStatPages.Start), 
                             true,
                             this.ShowExport());
                         //.StatsFiles) && (View.CurrentStartView != FileStatPages.StatsUsers),
                     }
                    break;

                 case FileStatPages.DetailFile:
                     if (View.CurrentIsStatUser)
                     {
                         View.GoToPage(FileStatPages.StatsUsers,
                             false,
                             (View.CurrentStartView != FileStatPages.StatsFiles) && (View.CurrentStartView != FileStatPages.StatsUsers),
                             true,
                             this.ShowExport());
                     }
                     else {
                         View.GoToPage(FileStatPages.StatsFiles,
                             false,
                             (View.CurrentStartView != FileStatPages.StatsFiles) && (View.CurrentStartView != FileStatPages.StatsUsers),
                             true,
                             this.ShowExport());
                     }
                     break;
                 case FileStatPages.DetailScorm:
                     if (View.CurrentIsStatUser)
                         {
                             View.GoToPage(FileStatPages.StatsUsers,
                                 false,
                                 (View.CurrentStartView != FileStatPages.StatsUsers),
                                 true,
                                 this.ShowExport());
                         }
                         else {
                             View.GoToPage(FileStatPages.StatsFiles,
                                 false,
                                 (View.CurrentStartView != FileStatPages.StatsUsers),
                                 true,
                                 this.ShowExport());
                         }
                     break;
                 default:
                     break;
             }
         
         }

         //private void GoToPage(FileStatPages Page)
         //{ 
         //    //QUI DENTRO avviene ANCHE il "bind" dei vari UC...
         
         //}

         public void SwitchStatus()
         {
             if (View.CurrentPage == FileStatPages.NoPermission || View.CurrentStartView == FileStatPages.NoPermission)
             {
                 View.GoToPage(FileStatPages.NoPermission, false, false, false, false);

             }
             else
             {

                 Boolean ShowBack = ((View.CurrentStartView == FileStatPages.Start) ||
                            (View.CurrentStartView == FileStatPages.SelectFiles) ||
                            (View.CurrentStartView == FileStatPages.SelectUsers));// ||
                 //(View.CurrentStartView == FileStatPages.StatsUsers) && (View.CurrentPage == FileStatPages.StatsUsers) ||
                 //(View.CurrentStartView == FileStatPages.StatsFiles) && (View.CurrentPage == FileStatPages.StatsFiles));


                 if (View.CurrentPage == FileStatPages.StatsFiles)
                 {
                     this.BindStatisticsUsers();

                     View.GoToPage(FileStatPages.StatsUsers
                         , false
                         , ShowBack
                         , true
                         , this.ShowExport());
                 }
                 else if (View.CurrentPage == FileStatPages.StatsUsers)
                 {
                     this.BindStatisticsFiles();

                     View.GoToPage(FileStatPages.StatsFiles
                         , false
                         , ShowBack
                         , true
                         , this.ShowExport());
                 }
             }
         }

         public void GoToDetailFile(Int32 UserId, Int64 FileId, Int32 Perm)
         {
             this.BindDetailFiles(UserId, FileId, Perm);
             View.GoToPage(FileStatPages.DetailFile, false, true, false, false);
         }

         public void GoToDetailSCORM(Int32 UserId, Int64 FileId, Int32 Perm)
         {
             this.BindDetailSCORM(UserId, FileId, Perm);
             View.GoToPage(FileStatPages.DetailScorm, false, true, false, false);
         }



#endregion

    #region BindControlli
        void BindSelectionFiles() 
        {

            IList<long> FileIds = new List<long>();

            if(View.PreloadedFileId > 0)
            {
                FileIds.Add(View.PreloadedFileId);
            }

            String ModuleOwnerCode = View.CurrentModuleOwnerCode;
            if (ModuleOwnerCode == "")
            {
                ModuleOwnerCode = "SRVMATER";
            }

            this.View.InitFileList(
                ModuleOwnerCode
                , View.PreloadedObjectId
                , View.PreloadedObjectTypeId
                , FileIds
                , this.AppContext.UserContext.CurrentCommunityID);
        }
        void BindSelectionUsers() {

            IList<int> UsersIds = new List<int>();

            if (View.PreloadedUserId > 0)
            {
                UsersIds.Add(View.PreloadedUserId);
            }

            this.View.InitUserList(
                View.CurrentModuleOwnerCode
                , View.PreloadedObjectId
                , View.PreloadedObjectTypeId
                , UsersIds
                , this.AppContext.UserContext.CurrentCommunityID);
        
        }

        void BindStatisticsFiles() {

            Int32 CurrentUserId = this.UserContext.CurrentUserID;
            IList<Int32> CurrentSelectedUserId =View.CurrentUsers;

            //Tengo l'utente corrente SOLO SE si comincia dai file o dalle statistiche...
            //Implementare la STESSA cosa per i FILE

            //  --> Aggiungere TUTTE le logiche anche all bind statistiche FILE <--

            if ((View.CurrentStartView == FileStatPages.SelectFiles) ||
                (View.CurrentStartView == FileStatPages.StatsFiles) ||
                (View.CurrentStartView == FileStatPages.StatsUsers))
            {
                CurrentSelectedUserId.Add(CurrentUserId);
            }
            else
            {
                if (!this.View.CurrentUsers.Contains(CurrentUserId))
                {
                    CurrentUserId = 0;
                }
            }

            //this.View.InitUserList(
            //    View.CurrentModuleOwnerCode
            //    , View.PreloadedObjectId
            //    , View.PreloadedObjectTypeId
            //    , UsersIds
            //    , this.AppContext.UserContext.CurrentCommunityID);




            //Int32 CurrentUserId = this.UserContext.CurrentUserID;

            //// Il CurrentUserId mi serve per identificare QUALE sia l'utente corrente.
            //// Viene impostato a 0 SE NON E' nei selezionati.
            //// NOTA: SE l'utente mi viene passato dal link, mi finisce come UNICO valore negli Utenti Selezionati.
            ////       In questo modo viene automaticamente controllato che l'utente abbia i permessi corretti 
            ////       per l'ID impostato via URL.

            //if(!this.View.CurrentUsers.Contains(CurrentUserId))
            //{
            //    CurrentUserId = 0;
            //}
            View.InitFileStat(View.CurrentFiles(), CurrentSelectedUserId, CurrentUserId);

        } 
        void BindStatisticsUsers() {
            Int32 CurrentUserId = this.UserContext.CurrentUserID;
            IList<Int32> CurrentSelectedUserId = View.CurrentUsers; //new List<int>();

            //Tengo l'utente corrente SOLO SE si comincia dai file o dalle statistiche...
            //Implementare la STESSA cosa per i FILE
            
            //  --> Aggiungere TUTTE le logiche anche all bind statistiche FILE <--

            if ((View.CurrentStartView == FileStatPages.SelectFiles) ||
                (View.CurrentStartView == FileStatPages.StatsFiles) ||
                (View.CurrentStartView == FileStatPages.StatsUsers))
            {
                CurrentSelectedUserId.Add(CurrentUserId);
            }
            else {
                if (!this.View.CurrentUsers.Contains(CurrentUserId))
                {
                    CurrentUserId = 0;
                }
            }

            View.InitUsersStat(View.CurrentFiles(), CurrentSelectedUserId, CurrentUserId);
        }

        private void BindDetailFiles(Int32 UserId, Int64 FileId, Int32 Perm)
        {
            this.View.InitFileDetail(UserId, FileId, Perm);
        }
        private void BindDetailSCORM(Int32 UserId, Int64 FileId, Int32 Perm)
        {

            this.View.InitScormDetail(UserId, FileId, Perm);
        }
    #endregion

        Boolean ShowExport()
        {
            //BASED ON PERMISSION....
            return true;
        }


        public void SendReport(Boolean isPdf, String CommunityName)
        {
            Int32 CurrentUserId = this.UserContext.CurrentUserID;
            IList<Int32> CurrentSelectedUserId = View.CurrentUsers;
            
            //Tengo l'utente corrente SOLO SE si comincia dai file o dalle statistiche...
            //Implementare la STESSA cosa per i FILE

            //  --> Aggiungere TUTTE le logiche anche all bind statistiche FILE <--

            if ((View.CurrentStartView == FileStatPages.SelectFiles) ||
                (View.CurrentStartView == FileStatPages.StatsFiles) ||
                (View.CurrentStartView == FileStatPages.StatsUsers))
            {
                CurrentSelectedUserId.Add(CurrentUserId);
            }
            else
            {
                if (!this.View.CurrentUsers.Contains(CurrentUserId))
                {
                    CurrentUserId = 0;
                }
            }



            if (View.CurrentPage == FileStatPages.StatsFiles)
            {
                View.SendReportFile(
                    View.CurrentFiles(), 
                    CurrentSelectedUserId, 
                    CurrentUserId, 
                    isPdf, 
                    CommunityName);
            }
            else if (View.CurrentPage == FileStatPages.StatsUsers)
            {
                View.SendReportUser(
                    View.CurrentFiles(),
                    CurrentSelectedUserId,
                    CurrentUserId,
                    isPdf,
                    CommunityName);
            }
        }

        public String GetXMLExportString(
            iApplicationContext oContext, 
            ref NHibernate.ISession IcoSession, 
            Modules.ScormStat.ExcelExport.ExportInternationalization Localization)
        {
            IList<Int32> LearnersIds = this.View.CurrentUsers;
            Int32 CurrentUserId = oContext.UserContext.CurrentUserID;

            if ((View.CurrentStartView == FileStatPages.SelectFiles) ||
                (View.CurrentStartView == FileStatPages.StatsFiles) ||
                (View.CurrentStartView == FileStatPages.StatsUsers))
            {
                LearnersIds.Add(CurrentUserId);
            }
            //else
            //{
            //    if (!this.View.CurrentUsers.Contains(CurrentUserId))
            //    {
            //        CurrentUserId = 0;
            //        CurrentSelectedUserId = View.CurrentUsers;
            //    }
            //}
            

            ServiceFileStatistics oService = new ServiceFileStatistics(oContext, oContext.DataContext.GetCurrentSession(), IcoSession);
            return oService.GetXMLStringExport(this.View.CurrentFiles(), LearnersIds, CurrentUserId, Localization);
        }

    }
}
