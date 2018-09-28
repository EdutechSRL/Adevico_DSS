using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Modules.ScormStat;
using lm.Comol.Core.BaseModules.FileStatistics.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.Scorm.Business
{
    public class ServiceFileStatistics :CoreServices, iLinkedService
    {
        private iApplicationContext _Context;

        #region iLinkedService Members
        //Effettivamente utilizzati
        public dtoEvaluation EvaluateModuleLink (ModuleLink link, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //if (Link.SourceItem.ObjectTypeID == ??)

            Int64 FileId = link.SourceItem.ObjectLongID;

            IList<Int64> FilesIds = new List<Int64>();
            FilesIds.Add(FileId);

            IList<int> UsersIds = new List<int>();
            UsersIds.Add(idUser);

            IList<ColFile> ColFiles = this.ColScormManager.GetStatFileUser(FilesIds, UsersIds);


            dtoEvaluation EvalML = new dtoEvaluation();

            if (ColFiles.Count > 0)
            {
                ColFile File = ColFiles[0];
                //Stat.Status
                switch (File.UserStats[0].Status)
                {
                    case lm.Comol.Modules.ScormStat.Icodeon.StatusCode.Completed:
                        EvalML.isStarted = true;
                        EvalML.isCompleted = true;
                        EvalML.isPassed = true;
                        break;

                    case lm.Comol.Modules.ScormStat.Icodeon.StatusCode.Started:
                        EvalML.isStarted = true;
                        EvalML.isCompleted = false;
                        EvalML.isPassed = false;
                        break;

                    default:
                        EvalML.isStarted = false;
                        EvalML.isCompleted = false;
                        EvalML.isPassed = false;
                        break;
                }

                EvalML.Completion = (short)((File.UserStats[0].NumActivity / File.Package.ActivityCount) * 100);
            }

            return EvalML;
        }
        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //if (Link.SourceItem.ObjectTypeID == ??)


            IList<Int64> FilesIds = new List<Int64>();
            foreach (ModuleLink link in links)
            {
                FilesIds.Add(link.SourceItem.ObjectLongID);
            }

            IList<int> UsersIds = new List<int>();
            UsersIds.Add(idUser);

            IList<ColFile> ColFiles = this.ColScormManager.GetStatFileUser(FilesIds, UsersIds);

            List<dtoItemEvaluation<long>> EvalMLs = new List<dtoItemEvaluation<long>>();

            if (ColFiles.Count > 0)
            {

                foreach (ColFile File in ColFiles)
                {
                    dtoItemEvaluation<long> EvalML = new dtoItemEvaluation<long>();

                    switch (File.UserStats[0].Status)
                    {
                        case lm.Comol.Modules.ScormStat.Icodeon.StatusCode.Completed:
                            EvalML.isStarted = true;
                            EvalML.isCompleted = true;
                            EvalML.isPassed = true;
                            break;

                        case lm.Comol.Modules.ScormStat.Icodeon.StatusCode.Started:
                            EvalML.isStarted = true;
                            EvalML.isCompleted = false;
                            EvalML.isPassed = false;
                            break;

                        default:
                            EvalML.isStarted = false;
                            EvalML.isCompleted = false;
                            EvalML.isPassed = false;
                            break;
                    }

                    EvalML.Completion = (short)((File.UserStats[0].NumActivity / File.Package.ActivityCount) * 100);
                    EvalMLs.Add(EvalML);
                }

            }
            return EvalMLs;
        }
        public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, bool alreadyCompleted, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }

        //Non utilizzati... ??
        public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
            return true;
        }
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return new List<StandardActionType>();
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
            return true;
        }
        public void PhisicalDeleteCommunity(int idCommunity, int idUser, string baseFilePath, string baseThumbnailPath)
        {

        }
        public void PhisicalDeleteCommunity(string baseFileRepositoryPath, int idCommunity, int idUser)
        {
            //throw new NotImplementedException();
        }

        public void PhisicalDeleteRepositoryItem(long idFileItem, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, Int32 objectTypeId, Dictionary<Int32, string> translations, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }
        #endregion

        #region Stat Management
        public IList<ColFile> GetFileStat(IList<StatFileTreeLeaf> StatTreeLeafs, IList<Int32> UsersIds, Int32 CurrentUserId)
        {
            List<long> AllPermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                         where (((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type))
                                         select STL.Id).ToList<long>();

            List<long> SinglePermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                            where STL.Type == StatTreeLeafType.Personal
                                            select STL.Id).ToList<long>();

            List<StatFileTreeLeaf> NoPermSFTLs = (from StatFileTreeLeaf STL in StatTreeLeafs
                                                  where STL.Type == StatTreeLeafType.None
                                                  select STL).ToList();

            List<ColFile> NPFile = new List<ColFile>();

            foreach (StatFileTreeLeaf SFTL in NoPermSFTLs)
            {
                ColFile file = new ColFile();
                file.Id = SFTL.Id;
                file.Guid = Guid.Empty;
                file.Name = SFTL.Name;
                file.Permission = 0;
                file.Extension = SFTL.Extension;

                NPFile.Add(file);
            }

            //Seleziono QUI i GUID,
            //perchè così posso avere SOLO i GUID dei file SCORM e non tutti i guid dei file che non mi interessano...
            List<Guid> AllGuids = (from StatFileTreeLeaf STL in StatTreeLeafs
                                   where (STL.isScorm &&
                                      ((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type) || (StatTreeLeafType.Personal == STL.Type))
                                   select STL.UniqueID).ToList<Guid>();

            String guidstr = "";

            AllGuids.ForEach(delegate(Guid g)
            { guidstr += " ### " + g.ToString(); });

            List<Guid> SingleGuids = (from StatFileTreeLeaf STL in StatTreeLeafs
                                   where (STL.isScorm &&
                                      (StatTreeLeafType.Personal == STL.Type))
                                   select STL.UniqueID).ToList<Guid>();

            IDictionary<String, Int32> DownInfo = GetDonwloadStat(UsersIds, AllPermFileIds);

            return this.ColScormManager.GetStatGenericFile(AllPermFileIds, SinglePermFileIds, NPFile, AllGuids, SingleGuids, UsersIds, CurrentUserId, DownInfo);

        }

        public IDictionary<String, Int32> GetDonwloadStat(IList<Int32> UsersIds, IList<Int64> FilesIds)
        {
            //Qui ho TUTTI i DOWNLOADINFO...
            //dovrei raggrupparli e contarli
            //List<FileDownloadInfo> AllPermFDIs = 
            //    (from f in Manager.GetIQ<FileDownloadInfo>()
            //     where FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
            //     select f)
            //     .ToList();


            //var x = (from f in Manager.GetIQ<FileDownloadInfo>()
            //         where FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
            //         group f by () into di
            //         select new { Key = di, NumDownload = di.Count() })
            //         ;

            
            var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)))
                     select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() })
                     ;

            //var xl = x.ToList();

            var z = from f in x
                    group f by f.Key into di
                    select new { MyKey = di.Key, NumDown = di.Count() };

            //var y = z.ToList();


            return z.ToDictionary(k => k.MyKey, k => k.NumDown);
            //return null;
        }

        public IDictionary<String, Int32> GetDonwloadStat(Int32 UserId, IList<Int64> FilesIds)
        {
            var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && (UserId == f.Downloader.Id)))
                     select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() })
                     ;

            //var xl = x.ToList();

            var z = from f in x
                    group f by f.Key into di
                    select new { MyKey = di.Key, NumDown = di.Count() };

            //var y = z.ToList();


            return z.ToDictionary(k => k.MyKey, k => k.NumDown);
        }
        //public IList<ColFile> GetFileStat_OLD(IList<StatFileTreeLeaf> StatTreeLeafs, IList<Int32> UsersIds, Int32 CurrentUserId)
        //{
            

        //    //Recupero i dati sui file dagli ID appena recuperati

        //    //INFO SUI DOWNLOAD:

        

        //    //IList<FileDownloadInfo> SinglePermFDIs = (from f in Manager.GetIQ<FileDownloadInfo>()
        //    //                                          where SinglePermFileIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
        //    //                                          select f).ToList();

        //    //IList<FileDownloadInfo> NoPermFDIs = (from f in Manager.GetIQ<FileDownloadInfo>()
        //    //                                          where NoPermFileIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
        //    //                                          select f).ToList();

        //    //Definizione di alcune "variabili interne":
        //    //Permessi per tutti gli utenti
        //    IList<ColFile> AllPermFiles = new List<ColFile>();
        //    IList<Guid> AllPermGuids = new List<Guid>();

        //    //Permessi solo per l'utente corrente
        //    IList<ColFile> SinglePermFiles = new List<ColFile>();
        //    IList<Guid> SinglePermGuids = new List<Guid>();

        //    IList<Int32> SingleUserId = new List<Int32>();
        //    if (CurrentUserId != null || CurrentUserId != 0)
        //    {
        //        SingleUserId.Add(CurrentUserId);
        //    }

        //    //No permessi
        //    IList<ColFile> NoPermFiles = new List<ColFile>();
            

        //    //"Trasformo" le info raccolte da COMOL, nel DTO per le statistiche SCORM
        //    //foreach (FileDownloadInfo fdi in AllPermFDIs)
        //    //{
        //    //    AllPermFiles.Add(FileDownloadInfo_to_ColFile(fdi,2));
        //    //    if (fdi.File.isSCORM)
        //    //    {
        //    //        AllPermGuids.Add(fdi.UniqueID);
        //    //    }
        //    //}

        //    //foreach (FileDownloadInfo fdi in SinglePermFDIs)
        //    //{
        //    //    SinglePermFiles.Add(FileDownloadInfo_to_ColFile(fdi,1));
        //    //    if (fdi.File.isSCORM)
        //    //    {
        //    //        SinglePermGuids.Add(fdi.UniqueID);
        //    //    }
        //    //}

        //    //foreach (FileDownloadInfo fdi in NoPermFDIs)
        //    //{
        //    //    ColFile file = new ColFile();
        //    //    file.Id = fdi.File.Id;
        //    //    file.Guid = Guid.Empty;
        //    //    file.Name = fdi.File.DisplayName;
        //    //    file.Permission = 0;

        //    //    NoPermFiles.Add(file);
        //    //}

        //    //Recupero le statistiche per i file di cui ho tutti i permessi
        //    IList<ColFile> AllFiles = this.ColScormManager.GetStatGenericFile(AllPermFiles, AllPermGuids, UsersIds);

        //    //Recupero le statistiche per i file di cui posso visualizzare solo l'utente corrente
        //    IList<ColFile> SingleFiles = new List<ColFile>();

        //    if (SingleUserId.Count >= 0)
        //    {
        //        SingleFiles = this.ColScormManager.GetStatGenericFile(SinglePermFiles, SinglePermGuids, SingleUserId);
        //    }

        //    //I file di cui non ho permessi, non vado a raccogliere nulla...

        //    //Ritorno la UNION delle 3 precedenti, ordinata per nome file...
        //    return (from ColFile file 
        //                in AllFiles.Union(SingleFiles.Union(NoPermFiles))
        //                orderby file.Name select file).ToList();
        //}

        private ColFile FileDownloadInfo_to_ColFile(FileDownloadInfo fdi, long Permission)
        {
            ColFile file = new ColFile();
            file.Id = fdi.Id;
            file.Guid = fdi.UniqueID;
            file.Name = fdi.File.DisplayName;
            file.Permission = Permission;   //fdi.Link.Permission;
            
            //file.Package
            //file.PlayCount
            //file.TotActivity
            file.UploadDate = fdi.File.CreatedOn;

            ColPerson person = new ColPerson();
            person.Id = fdi.File.Owner.Id;
            person.Name = fdi.File.Owner.Name;
            person.SecondName = fdi.File.Owner.Surname;

            file.UploadedBy = person;

            return file;

        }

        public IList<ColPerson> GetUsersStat(IList<StatFileTreeLeaf> StatTreeLeafs, IList<Int32> UsersIds, Int32 CurrentUserId)
        {
            List<long> AllPermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                         where (((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type))
                                         select STL.Id).ToList<long>();

            List<long> SinglePermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                            where STL.Type == StatTreeLeafType.Personal
                                            select STL.Id).ToList<long>();

            
            List<StatFileTreeLeaf> NoPermSFTLs = (from StatFileTreeLeaf STL in StatTreeLeafs
                                        where STL.Type == StatTreeLeafType.None
                                        select STL).ToList();

            List<ColFile> NPFile = new List<ColFile>();

            foreach (StatFileTreeLeaf SFTL in NoPermSFTLs)
            {
                ColFile file = new ColFile();
                file.Id = SFTL.Id;
                file.Guid = Guid.Empty;
                file.Name = SFTL.Name;
                file.Permission = 0;
                file.Extension = SFTL.Extension;

                NPFile.Add(file);
            }

            List<Guid> AllGuids = (from StatFileTreeLeaf STL in StatTreeLeafs
                                   where (STL.isScorm &&
                                      ((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type) || (StatTreeLeafType.Personal == STL.Type))
                                   select STL.UniqueID).ToList<Guid>();

            IDictionary<String, Int32> DownInfo = GetDonwloadStat(UsersIds, AllPermFileIds);
            
            return this.ColScormManager.GetStatGenericUser(
                AllPermFileIds, 
                SinglePermFileIds, 
                NPFile, 
                AllGuids, 
                UsersIds, 
                CurrentUserId,
                DownInfo);

        }
        //public IList<ColPerson> GetUsersStat_Old(IList<StatFileTreeLeaf> StatTreeLeafs, IList<Int32> UsersIds, Int32 CurrentUserId)
        //{
        //    ////SOLO PER TEST
        //    List<StatFileTreeLeaf> TestLeafsAdm = new List<StatFileTreeLeaf>();
        //    //List<StatFileTreeLeaf> TestLeafsNoAdm = new List<StatFileTreeLeaf>();

        //    String TestStr = "";

        //    foreach (StatFileTreeLeaf STF in StatTreeLeafs)
        //    {
        //        TestLeafsAdm.Add(STF);
        //        TestStr += "  " + STF.Id;
        //        //if ( == STF.Type)
        //        //{
        //        //    TestLeafsAdm.Add(STF);
        //        //}
        //        //else
        //        //{
        //        //    TestLeafsNoAdm.Add(STF);
        //        //}
                
        //    }

        //    TestStr += "/n/r -- Altri ID -- /n/r  ";

        //    //Recupero gli ID dei file divisi per "permessi": tutti, solo corrente, nessuno

        //    //Tolto questo:
        //    //STL.Selected == true && 
        //    List<long> AllPermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
        //                                  where (((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type))
        //                                  select STL.Id).ToList<long>();

        //    foreach (long Id in AllPermFileIds)
        //    {
        //        TestStr += "++ " + Id;
        //    }


        //    List<long> SinglePermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
        //                                     where STL.Type == StatTreeLeafType.Personal
        //                                     select STL.Id).ToList<long>();

        //    List<long> NoPermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
        //                                 where STL.Type == StatTreeLeafType.None
        //                                 select STL.Id).ToList<long>();

        //    //Recupero i dati sui file dagli ID appena recuperati
        //    //Tolto: 
        //    // && UsersIds.Contains(f.Downloader.Id)
        //    List<FileDownloadInfo> AllPermFDIs = (from f in Manager.GetAll<FileDownloadInfo>(f => AllPermFileIds.Contains(f.File.Id))
        //                                           select f).ToList();

        //    List<FileDownloadInfo> SinglePermFDIs = (from f in Manager.GetIQ<FileDownloadInfo>()
        //                                              where SinglePermFileIds.Contains(f.File.Id)
        //                                              select f).ToList();

        //    List<FileDownloadInfo> NoPermFDIs = (from f in Manager.GetIQ<FileDownloadInfo>()
        //                                          where NoPermFileIds.Contains(f.File.Id)
        //                                          select f).ToList();




        //    //Definisco parametri interni che verranno passati "sotto"
        //    List<ColFile> AllFiles = new List<ColFile>();
        //    //List<Guid> AllGuids = new List<Guid>();

        //    //IList<Int32> SingleUserId = new List<Int32>();
        //    //if (CurrentUserId != null || CurrentUserId != 0)
        //    //{
        //    //    SingleUserId.Add(CurrentUserId);
        //    //}


        //    //"Trasformo" le lista di oggetti COMOL in oggetti "scorm", impostando i permessi:
        //    // 2: ALL
        //    // 1: SOLO corrente
        //    // 0: NO PERMESSI
        //    foreach (FileDownloadInfo fdi in AllPermFDIs)
        //    {
        //        AllFiles.Add(FileDownloadInfo_to_ColFile(fdi, 2));
        //        //if (fdi.File.isSCORM)
        //        //{
        //        //    AllGuids.Add(fdi.File.UniqueID);
        //        //}
        //    }

        //    foreach (FileDownloadInfo fdi in SinglePermFDIs)
        //    {
        //        AllFiles.Add(FileDownloadInfo_to_ColFile(fdi, 1));
        //        //if (fdi.File.isSCORM)
        //        //{
        //        //    AllGuids.Add(fdi.File.UniqueID);
        //        //}
        //    }



        //    //La funzione sotto è stata rivista.
        //    //  Recupera comunque TUTTI i dati per gli Utenti/File selezionati, 
        //    //  ma vengono considerati e calcolati SOLO in base ai permessi selezionati...
            

        //    return Persons;
        //}
        #endregion

        #region initClass
        public ServiceFileStatistics() { }
            public ServiceFileStatistics(iApplicationContext oContext)
            {
                throw new NullReferenceException("No nHibernate session!!!");
                this.ColScormManager = new lm.Comol.Modules.ScormStat.Manager.Col_Manager(null, null);
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
                //_Context = oContext;
                //this.UC = oContext.UserContext;
            }
            public ServiceFileStatistics(iApplicationContext oContext, NHibernate.ISession ComolSession, NHibernate.ISession IcodeonSession) {
                //RIVEDERE!!!!
                this.ColScormManager = new lm.Comol.Modules.ScormStat.Manager.Col_Manager(ComolSession, IcodeonSession);

                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);

                this.UC = oContext.UserContext;
            }
            //public ServiceFileStatistics(iDataContext oDC)
            //{
            //    this.Manager = new BaseModuleManager(oDC);
            //    _Context = new ApplicationContext();
            //    _Context.DataContext = oDC;
            //    this.UC = null;
            //}

        #endregion
        
        private lm.Comol.Modules.ScormStat.Manager.Col_Manager ColScormManager { get; set; }

        #region iLinkedService Members

        /// <summary>
        /// Mirco: Che è sta roba?!?!?!?!??!--> STATISTICHE INDIPENDENTI DALLA SORGENTE , DA MOSTRARE A VIDEO IN UN ALBERO DOVE OGNI PADRE RAPPRESENTA L'OGGETTO PROPRIETARIO DEL FILE....
        /// </summary>
        /// <param name="IdCommunity"></param>
        /// <param name="UserID"></param>
        /// <param name="objectId"></param>
        /// <param name="objectTypeId"></param>
        /// <returns></returns>
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(int IdCommunity, int UserID, long objectId, int objectTypeId)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }

        #endregion

        #region Detailed Stats
        public ColPerson GetDetailScorm(Int32 UserId, Int64 FileId)
        {
            return ColScormManager.GetStatDetailed(UserId, FileId);
        }

        public dtoFileDetail GetDetailFile(IList<Int32> UsersId, Int64 FileId, IDictionary<String, String> translateServiceCode)
        {
            dtoFileDetail FileInfo = new dtoFileDetail();
            
            // 1. Col_Manager -> ColBaseCommunityFile (per le info sul file).
            //      La proprietà IsInternal mi dice se appartiene ad un servizio o alla comunità.
            BaseCommunityFile File = Manager.Get<BaseCommunityFile>(FileId);


            FileInfo.FileName = File.Name;
            
            if (File.IsInternal)
            {
                FileInfo.ComService = "Service";
                FileInfo.Path = "";
            } else {
                FileInfo.ComService = File.CommunityOwner.Name;
                FileInfo.Path = File.FilePath;
            }

            FileInfo.Size = File.Size;
            FileInfo.LoadedBy = File.Owner.SurnameAndName;
            FileInfo.LoadedOn = File.CreatedOn;
            FileInfo.Downloads = File.Downloads;

            FileInfo.isInternal = File.IsInternal;

            
            // 2. da filelink recupero le info sui download (riferiti ad un singolo file)
            //

            //var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)))
            //         select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() });

            IList<Person> Users = Manager.GetAll<Person>(p => (UsersId.Contains(p.Id))).ToList();
            FileInfo.DownDetails = new List<dtoUserDownInfo>();

            Users.ForEach(delegate(Person prsn)
            {
                dtoUserDownInfo UDI = new dtoUserDownInfo();
                UDI.downBy = prsn.SurnameAndName;
                UDI.downOnList = GetdtoDownInfos(prsn.Id, FileId, translateServiceCode);
                if (UDI.downOnList == null)
                {
                    UDI.downOnList = new List<dtoDownInfo>();
                }
                UDI.TotalDownload = UDI.downOnList.Count(); //Eventualmente metterlo "fuori" o toglierlo del tutto...
                FileInfo.DownDetails.Add(UDI);
            });

            

            // 3. Prendere dalla View un iDictionary(String, String) -> Dictionary(Service.Code, ServiceTranslateName) 
            //      per tradurre i nomi dei servizi.
            //

            return FileInfo;
        }

        private IList<dtoDownInfo> GetdtoDownInfos(Int32 UserId, Int64 FileId, IDictionary<String, String> ServTranslation)
        {
            //IList<dtoDownInfo> dtoDIs = 
              return (from FileDownloadInfo f in (Manager.GetAll<FileDownloadInfo>(fdi => ((fdi.File.Id == FileId) && (fdi.Downloader.Id == UserId))))
                     select new dtoDownInfo{ downDate = f.CreatedOn, downService = ServTranslation[f.ServiceCode] }).ToList<dtoDownInfo>();
                                            
            //    new List<dtoDownInfo>();
            //IList<FileDownloadInfo> FDIs = ;
            
            //foreach (FileDownloadInfo fdi in FDIs)
            //{
            //    dtoDownInfo dDI = new dtoDownInfo();
            //    dDI.downDate = fdi.CreatedOn;
            //    dDI.downService = ServTranslation[fdi.ServiceCode];
            //    dtoDIs.Add(dDI);
            //}
            
            //return dtoDIs;
        }

        #endregion

        public String GetXMLStringExport(
            IList<StatFileTreeLeaf> StatTreeLeafs, 
            IList<Int32> LearnersIds, 
            Int32 CurrentLearner,
            Modules.ScormStat.ExcelExport.ExportInternationalization Localization)
        {
            List<long> AllPermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                         where (((StatTreeLeafType.Advanced | StatTreeLeafType.Personal) == STL.Type) || (StatTreeLeafType.Advanced == STL.Type))
                                         select STL.Id).ToList<long>();

            List<long> SinglePermFileIds = (from StatFileTreeLeaf STL in StatTreeLeafs
                                            where STL.Type == StatTreeLeafType.Personal
                                            select STL.Id).ToList<long>();

            //List<long> NoPermSFTLs = (from StatFileTreeLeaf STL in StatTreeLeafs
            //                                      where STL.Type == StatTreeLeafType.None
            //                                      select STL).ToList();

            //List<ColFile> NPFile = new List<ColFile>();

            //foreach (StatFileTreeLeaf SFTL in NoPermSFTLs)
            //{
            //    ColFile file = new ColFile();
            //    file.Id = SFTL.Id;
            //    file.Guid = Guid.Empty;
            //    file.Name = SFTL.Name;
            //    file.Permission = 0;
            //    file.Extension = SFTL.Extension;
            //    NPFile.Add(file);
            //}

            return ColScormManager.GetStatExportGeneric(AllPermFileIds, SinglePermFileIds, LearnersIds, CurrentLearner, Localization);
        }

        
    }

}
