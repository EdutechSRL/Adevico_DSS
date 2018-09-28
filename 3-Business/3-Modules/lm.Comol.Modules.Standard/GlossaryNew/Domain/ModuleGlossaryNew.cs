using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class ModuleGlossaryNew
    {
        [Serializable]
        public enum ActionType
        {
            None = 86000,
            NoPermission = 86001,
            GenericError = 86002,
            ViewListByLetter = 86003,
            ViewListByIndex = 86004,
            StartAddTerm = 86008,
            AddTerm = 86009,
            StartEditTerm = 86010,
            SaveTerm = 86011,
            VirtualDeleteTerm = 86012,
            VirtualUndeleteTerm = 86013,
            StartAddGlossary = 86015,
            AddGlossary = 86016,
            StartEditGlossary = 86017,
            SaveGlossary = 86018,
            VirtualDeleteGlossary = 86019,
            VirtualUndeleteGlossary = 86020,
            PhisicalDeleteGlossary = 86021,
            EditPaging = 86022,
            StartManageSort = 86023,
            SaveManageSort = 86024,
            DeleteTerm = 86025,
            RecoverTerm = 86026,
            StartEditGlossaryShare = 86027,
            SaveGlossaryShare = 86028,
            ChangeGlossaryState = 86029,
            ChangeGlossaryVisibility = 86030
        }

        [Flags, Serializable]
        public enum Base2Permission
        {
            ListItems = 1,
            AddItem = 2,
            EditItem = 4,
            DeleteItem = 8,
            ManageGroup = 16,
            Admin = 64,
            // Nuovi Permessi
            AddGlossary = 8192, //Send
            //ViewGlossary = 256,     //Receive
            //EditGlossary = 512, //Synchronize
            //DeleteGlossary = 1024, //Browse
            ViewStat = 2048 //Print
        }

        [Serializable, CLSCompliant(true)]
        public enum InteractionType
        {
            None = 1,

            /// <summary>
            ///     Interaction between users
            /// </summary>
            UserWithUser = 2,

            /// <summary>
            ///     Interaction between user and community administrator
            /// </summary>
            UserWithCommunityAdministrator = 3,

            /// <summary>
            ///     Interaction between user and LearingObjects
            /// </summary>
            UserWithLearningObject = 4,

            /// <summary>
            ///     Interaction generic
            /// </summary>
            Generic = 5,

            /// <summary>
            ///     Interaction betweeen core
            /// </summary>
            SystemToSystem = 6,

            /// <summary>
            ///     Interaction from core to user
            /// </summary>
            SystemToUser = 7,

            /// <summary>
            ///     Interaction from core to module
            /// </summary>
            SystemToModule = 8,

            /// <summary>
            ///     Interaction from module to core
            /// </summary>
            ModuleToSystem = 9,

            /// <summary>
            ///     Interaction between modules
            /// </summary>
            ModuleToModule = 10
        }

        [Serializable]
        public enum ObjectType
        {
            None = 0,
            Term = 1,
            Glossary = 2,
            File = 3
        }

        public const String UniqueCode = "SRVGLS";

        public ModuleGlossaryNew()
        {
            ViewTerm = false;
        }

        public ModuleGlossaryNew(long permission)
        {
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            ManageGlossary = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageGroup, permission);
            ViewTerm = Administration || ManageGlossary || PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListItems, permission);
            AddTerm = Administration || ManageGlossary || PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddItem, permission);
            EditTerm = Administration || ManageGlossary || PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditItem, permission);
            DeleteTerm = Administration || ManageGlossary || PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteItem, permission);
           
            

            EditGlossary = Administration|| ManageGlossary; //|| PermissionHelper.CheckPermissionSoft((long) Base2Permission.EditGlossary, permission);
            DeleteGlossary = Administration; // || PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteGlossary, permission);
            ViewStat = false; // PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewStat, permission);
        }

        public virtual Boolean ViewTerm { get; set; }
        public virtual Boolean AddTerm { get; set; }
        public virtual Boolean EditTerm { get; set; }
        public virtual Boolean DeleteTerm { get; set; }
        public virtual Boolean ManageGlossary { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean EditGlossary { get; set; }
        public virtual Boolean DeleteGlossary { get; set; }
        public virtual Boolean ViewStat { get; set; }
        public virtual Boolean AddGlossary { get; set; }

        public static ModuleGlossaryNew CreatePortalmodule(int UserTypeID)
        {
            var module = new ModuleGlossaryNew();
            module.ViewTerm = (UserTypeID != (int) UserTypeStandard.Guest);
            module.AddTerm = (UserTypeID == (int) UserTypeStandard.SysAdmin ||
                              UserTypeID == (int) UserTypeStandard.Administrator);
            module.EditTerm = (UserTypeID == (int) UserTypeStandard.SysAdmin ||
                               UserTypeID == (int) UserTypeStandard.Administrator);
            module.DeleteTerm = (UserTypeID == (int) UserTypeStandard.SysAdmin ||
                                 UserTypeID == (int) UserTypeStandard.Administrator);
            module.ManageGlossary = (UserTypeID == (int) UserTypeStandard.SysAdmin ||
                                     UserTypeID == (int) UserTypeStandard.Administrator);
            module.Administration = (UserTypeID == (int) UserTypeStandard.SysAdmin ||
                                     UserTypeID == (int) UserTypeStandard.Administrator);
            return module;
        }
    }
}