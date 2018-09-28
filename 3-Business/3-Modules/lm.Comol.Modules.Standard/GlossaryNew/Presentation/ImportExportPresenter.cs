using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.File;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class ImportExportPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.IdCommunity = idCommunity;
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    View.LoadViewData(View.IdCommunity);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddAllCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = new List<int>();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.EditGlossary || module.Administration || module.ManageGlossary)
            {
                Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
            }
            else
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public String GetFullUserPath(String filename)
        {
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            if (String.IsNullOrWhiteSpace(filename))
                filename = "glossary.xml";

            return String.Format("{0}\\{1}", currentPath, filename);
        }

        public Boolean ExportGlossaries(IEnumerable<long> selectedTerms)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            const string filename = "export";
            var completePath = String.Format("{0}\\{1}.xml", currentPath, filename);
            var index = 0;
            while (!Delete.File(completePath))
            {
                completePath = String.Format("{0}\\{1}_{2}.xml", currentPath, filename, index);
                index++;
            }

            if (Delete.File(completePath))
            {
                var item = Service.RetriveGlossaryData(selectedTerms);
                var serializer = new XmlRepository<GlossaryContainer>();
                serializer.Serialize(completePath, item);
                if (File.Exists(completePath))
                {
                    var fileName = string.Format("export_{0}.xml", DateTime.Now.ToString("yyyyMMddHmmss"));
                    View.ExportGlossaries(File.ReadAllText(completePath), fileName);
                    Delete.File(completePath);
                    return true;
                }
            }
            return false;
        }

        public bool ImportGlossaries(string fileName, List<long> idTermList, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.EditGlossary || module.Administration || module.ManageGlossary)
            {
                var item = new XmlRepository<GlossaryContainer>();
                var result = Service.ImportGlossaryData(View.IdCommunity, item.Deserialize(fileName), idTermList, selectedIndex);
                Delete.File(fileName);
                return result;
            }
            View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
            View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());

            return false;
        }

        public bool ImportGlossaries(string fileName, Dictionary<Int32, List<long>> idTermDictionary, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.EditGlossary || module.Administration || module.ManageGlossary)
            {
                var item = new XmlRepository<GlossaryContainer>();
                var gitem = item.Deserialize(fileName);
                var idTermList = new List<long>();


                foreach (var value in idTermDictionary.Values)
                    idTermList.AddRange(value);

                foreach (var glossarySerialized in gitem.Glossaries)
                {
                    var glossaryTerm = gitem.Terms.FirstOrDefault(f => f.IdGlossary == glossarySerialized.Id);
                    if (glossaryTerm != null)
                        foreach (var keyValue in idTermDictionary.Where(keyValue => keyValue.Value.Contains(glossaryTerm.Id)))
                            glossarySerialized.IdCommunity = keyValue.Key;
                }

                var result = Service.ImportGlossaryData(-1, gitem, idTermList, selectedIndex);
                Delete.File(fileName);
                return result;
            }
            View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
            View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            return false;
        }

        public string GetCommunityName(int id)
        {
            return CurrentManager.GetCommunityName(id);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImportExport View
        {
            get { return (IViewImportExport)base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ImportExportPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ImportExportPresenter(iApplicationContext oContext, IViewImportExport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }

    public class ImportFromFilePresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    View.LoadViewData(View.IdCommunity);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddAllCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = new List<int>();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
        }

        public String GetFullUserPath(String filename)
        {
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            if (String.IsNullOrWhiteSpace(filename))
                filename = "glossary.xml";

            return String.Format("{0}\\{1}", currentPath, filename);
        }

        public Boolean ExportGlossaries(IEnumerable<long> selectedTerms)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            const string filename = "export";
            var completePath = String.Format("{0}\\{1}.xml", currentPath, filename);
            var index = 0;
            while (!Delete.File(completePath))
            {
                completePath = String.Format("{0}\\{1}_{2}.xml", currentPath, filename, index);
                index++;
            }

            if (Delete.File(completePath))
            {
                var item = Service.RetriveGlossaryData(selectedTerms);
                var serializer = new XmlRepository<GlossaryContainer>();
                serializer.Serialize(completePath, item);
                if (File.Exists(completePath))
                {
                    var fileName = string.Format("export_{0}.xml", DateTime.Now.ToString("yyyyMMddHmmss"));
                    View.ExportGlossaries(File.ReadAllText(completePath), fileName);
                    Delete.File(completePath);
                    return true;
                }
            }
            return false;
        }

        public bool ImportGlossaries(string fileName, List<long> idTermList, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var result = Service.ImportGlossaryData(View.IdCommunity, item.Deserialize(fileName), idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public bool ImportGlossaries(string fileName, Dictionary<Int32, List<long>> idTermDictionary, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var gitem = item.Deserialize(fileName);
            var idTermList = new List<long>();


            foreach (var value in idTermDictionary.Values)
                idTermList.AddRange(value);

            foreach (var glossarySerialized in gitem.Glossaries)
            {
                var glossaryTerm = gitem.Terms.FirstOrDefault(f => f.IdGlossary == glossarySerialized.Id);
                if (glossaryTerm != null)
                    foreach (var keyValue in idTermDictionary.Where(keyValue => keyValue.Value.Contains(glossaryTerm.Id)))
                        glossarySerialized.IdCommunity = keyValue.Key;
            }

            var result = Service.ImportGlossaryData(-1, gitem, idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public string GetCommunityName(int id)
        {
            return CurrentManager.GetCommunityName(id);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImportExport View
        {
            get { return (IViewImportExport)base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ImportFromFilePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ImportFromFilePresenter(iApplicationContext oContext, IViewImportExport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }

    public class ExportToFilePresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    View.LoadViewData(View.IdCommunity);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddAllCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = new List<int>();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
        }

        public String GetFullUserPath(String filename)
        {
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            if (String.IsNullOrWhiteSpace(filename))
                filename = "glossary.xml";

            return String.Format("{0}\\{1}", currentPath, filename);
        }

        public Boolean ExportGlossaries(IEnumerable<long> selectedTerms)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            const string filename = "export";
            var completePath = String.Format("{0}\\{1}.xml", currentPath, filename);
            var index = 0;
            while (!Delete.File(completePath))
            {
                completePath = String.Format("{0}\\{1}_{2}.xml", currentPath, filename, index);
                index++;
            }

            if (Delete.File(completePath))
            {
                var item = Service.RetriveGlossaryData(selectedTerms);
                var serializer = new XmlRepository<GlossaryContainer>();
                serializer.Serialize(completePath, item);
                if (File.Exists(completePath))
                {
                    var fileName = string.Format("export_{0}.xml", DateTime.Now.ToString("yyyyMMddHmmss"));
                    View.ExportGlossaries(File.ReadAllText(completePath), fileName);
                    Delete.File(completePath);
                    return true;
                }
            }
            return false;
        }

        public bool ImportGlossaries(string fileName, List<long> idTermList, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var result = Service.ImportGlossaryData(View.IdCommunity, item.Deserialize(fileName), idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public bool ImportGlossaries(string fileName, Dictionary<Int32, List<long>> idTermDictionary, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var gitem = item.Deserialize(fileName);
            var idTermList = new List<long>();


            foreach (var value in idTermDictionary.Values)
                idTermList.AddRange(value);

            foreach (var glossarySerialized in gitem.Glossaries)
            {
                var glossaryTerm = gitem.Terms.FirstOrDefault(f => f.IdGlossary == glossarySerialized.Id);
                if (glossaryTerm != null)
                    foreach (var keyValue in idTermDictionary.Where(keyValue => keyValue.Value.Contains(glossaryTerm.Id)))
                        glossarySerialized.IdCommunity = keyValue.Key;
            }

            var result = Service.ImportGlossaryData(-1, gitem, idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public string GetCommunityName(int id)
        {
            return CurrentManager.GetCommunityName(id);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImportExport View
        {
            get { return (IViewImportExport)base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ExportToFilePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ExportToFilePresenter(iApplicationContext oContext, IViewImportExport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }

    public class ImportFromFileCommunityPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null || idCommunity <= 0)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    View.LoadViewData(View.IdCommunity);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddAllCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = new List<int>();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
        }

        public String GetFullUserPath(String filename)
        {
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            if (String.IsNullOrWhiteSpace(filename))
                filename = "glossary.xml";

            return String.Format("{0}\\{1}", currentPath, filename);
        }

        public Boolean ExportGlossaries(IEnumerable<long> selectedTerms)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            const string filename = "export";
            var completePath = String.Format("{0}\\{1}.xml", currentPath, filename);
            var index = 0;
            while (!Delete.File(completePath))
            {
                completePath = String.Format("{0}\\{1}_{2}.xml", currentPath, filename, index);
                index++;
            }

            if (Delete.File(completePath))
            {
                var item = Service.RetriveGlossaryData(selectedTerms);
                var serializer = new XmlRepository<GlossaryContainer>();
                serializer.Serialize(completePath, item);
                if (File.Exists(completePath))
                {
                    var fileName = string.Format("export_{0}.xml", DateTime.Now.ToString("yyyyMMddHmmss"));
                    View.ExportGlossaries(File.ReadAllText(completePath), fileName);
                    Delete.File(completePath);
                    return true;
                }
            }
            return false;
        }

        public bool ImportGlossaries(string fileName, List<long> idTermList, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var result = Service.ImportGlossaryData(View.IdCommunity, item.Deserialize(fileName), idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public bool ImportGlossaries(string fileName, Dictionary<Int32, List<long>> idTermDictionary, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var gitem = item.Deserialize(fileName);
            var idTermList = new List<long>();


            foreach (var value in idTermDictionary.Values)
                idTermList.AddRange(value);

            foreach (var glossarySerialized in gitem.Glossaries)
            {
                var glossaryTerm = gitem.Terms.FirstOrDefault(f => f.IdGlossary == glossarySerialized.Id);
                if (glossaryTerm != null)
                    foreach (var keyValue in idTermDictionary.Where(keyValue => keyValue.Value.Contains(glossaryTerm.Id)))
                        glossarySerialized.IdCommunity = keyValue.Key;
            }

            var result = Service.ImportGlossaryData(-1, gitem, idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public string GetCommunityName(int id)
        {
            return CurrentManager.GetCommunityName(id);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImportExport View
        {
            get { return (IViewImportExport)base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ImportFromFileCommunityPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ImportFromFileCommunityPresenter(iApplicationContext oContext, IViewImportExport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }

    public class ExportToFileCommunityPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null || idCommunity <= 0)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(idCommunity, litePerson);
                if (module.EditGlossary || module.Administration || module.ManageGlossary)
                {
                    View.LoadViewData(View.IdCommunity);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void AddCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = Service.GetIdCommunityWithoutGlossaries();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddAllCommunityClick(int integer)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            var forAdmin = UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator || UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;
            var availability = forAdmin ? CommunityAvailability.All : CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary));

            var unloadIdCommunities = new List<int>();
            View.DisplayCommunityToAdd(forAdmin, rPermissions, unloadIdCommunities, availability);
        }

        public void AddNewCommunity(List<int> idCommunities)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.ShowCommunity(idCommunities);
        }

        public void ImportTerms(List<long> listId)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            Service.ImportTerms(View.IdCommunity, View.IdGlossary, listId, -1);
        }

        public String GetFullUserPath(String filename)
        {
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            if (String.IsNullOrWhiteSpace(filename))
                filename = "glossary.xml";

            return String.Format("{0}\\{1}", currentPath, filename);
        }

        public Boolean ExportGlossaries(IEnumerable<long> selectedTerms)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var currentPath = String.Format("{0}{1}", View.GlossaryPath, UserContext.CurrentUserID);

            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            const string filename = "export";
            var completePath = String.Format("{0}\\{1}.xml", currentPath, filename);
            var index = 0;
            while (!Delete.File(completePath))
            {
                completePath = String.Format("{0}\\{1}_{2}.xml", currentPath, filename, index);
                index++;
            }

            if (Delete.File(completePath))
            {
                var item = Service.RetriveGlossaryData(selectedTerms);
                var serializer = new XmlRepository<GlossaryContainer>();
                serializer.Serialize(completePath, item);
                if (File.Exists(completePath))
                {
                    var fileName = string.Format("export_{0}.xml", DateTime.Now.ToString("yyyyMMddHmmss"));
                    View.ExportGlossaries(File.ReadAllText(completePath), fileName);
                    Delete.File(completePath);
                    return true;
                }
            }
            return false;
        }

        public bool ImportGlossaries(string fileName, List<long> idTermList, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var result = Service.ImportGlossaryData(View.IdCommunity, item.Deserialize(fileName), idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public bool ImportGlossaries(string fileName, Dictionary<Int32, List<long>> idTermDictionary, int selectedIndex)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return false;
            }
            var item = new XmlRepository<GlossaryContainer>();
            var gitem = item.Deserialize(fileName);
            var idTermList = new List<long>();


            foreach (var value in idTermDictionary.Values)
                idTermList.AddRange(value);

            foreach (var glossarySerialized in gitem.Glossaries)
            {
                var glossaryTerm = gitem.Terms.FirstOrDefault(f => f.IdGlossary == glossarySerialized.Id);
                if (glossaryTerm != null)
                    foreach (var keyValue in idTermDictionary.Where(keyValue => keyValue.Value.Contains(glossaryTerm.Id)))
                        glossarySerialized.IdCommunity = keyValue.Key;
            }

            var result = Service.ImportGlossaryData(-1, gitem, idTermList, selectedIndex);
            Delete.File(fileName);
            return result;
        }

        public string GetCommunityName(int id)
        {
            return CurrentManager.GetCommunityName(id);
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewImportExport View
        {
            get { return (IViewImportExport)base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public ExportToFileCommunityPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public ExportToFileCommunityPresenter(iApplicationContext oContext, IViewImportExport view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}