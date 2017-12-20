using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class ModuleRequestForMembership
    {
        public const String UniqueCode = "SRVRFM";
        public virtual Boolean ViewBaseForPapers { get; set; }
        public virtual Boolean AddSubmission { get; set; }
        public virtual Boolean CreateBaseForPaper { get; set; }
        public virtual Boolean EditBaseForPaper { get; set; }
        public virtual Boolean ManageBaseForPapers { get; set; }
        public virtual Boolean DeleteOwnBaseForPaper { get; set; }
        public virtual Boolean Administration { get; set; }
        public ModuleRequestForMembership()
        {
            ViewBaseForPapers = true;
        }
        public static ModuleRequestForMembership CreatePortalmodule(int UserTypeID)
        {
            ModuleRequestForMembership module = new ModuleRequestForMembership();
            module.ViewBaseForPapers = true;
            module.AddSubmission = true;//(UserTypeID != (int)UserTypeStandard.Guest);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreateBaseForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditBaseForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManageBaseForPapers = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.DeleteOwnBaseForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            return module;
        }

        public ModuleRequestForMembership(long permission)
        {
            ViewBaseForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListRequests, permission);
            AddSubmission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddSubmission, permission);
            CreateBaseForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddRequest, permission);
            EditBaseForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditRequest, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            ManageBaseForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageRequests, permission);
            DeleteOwnBaseForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteRequest, permission);
        }
        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule()
        {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = ManageBaseForPapers || Administration || CreateBaseForPaper;
            m.Administration = ManageBaseForPapers || Administration;
            m.Clone = ManageBaseForPapers || Administration;
            m.DeleteMyTemplates = ManageBaseForPapers || Administration || CreateBaseForPaper;
            m.DeleteOtherTemplates = ManageBaseForPapers || Administration;
            m.Edit = ManageBaseForPapers || Administration;
            m.List = ManageBaseForPapers || Administration;
            m.SendMessage = ManageBaseForPapers || Administration || CreateBaseForPaper;
            m.ManageModulePermission = ManageBaseForPapers || Administration || CreateBaseForPaper;
            return m;
        }

        [Flags, Serializable]
        public enum Base2Permission
        {
            ListRequests = 1,
            AddSubmission = 2,
            EditRequest = 4,
            DeleteRequest = 8,
            ManageRequests = 16,
            Admin = 64,
            AddRequest = 8192
        }
        [Serializable]
        public enum ActionType 
        {
            None = 87000,
            NoPermission = 87001,
            GenericError = 87002,
            List = 87003,
            Manage = 87004,
            EditRequest = 87008,
            DeleteRequest = 87009,
            AddSubmission = 87010,
            EditSubmission = 87011,
            ConfirmSubmission = 87012,
            DeleteSubmission = 87013,
            ViewSubmission = 87015,
            ViewUnknowSubmission = 87016,
            ViewRequestSubmission = 87017,
            ViewUnknowRequest = 87018,
            ViewPreviewRequest = 87019,
            DownloadSubmittedFile = 87020,
            DownloadRequestFile = 87021,
            AcceptSubmission = 87039,
            RejectSubmission = 87040,
            StartSubmission = 87041,
            VirtualDeleteSubmission = 87042,
            VirtualDeleteSubmittedFile = 87043,
            VirtualDeleteRequest = 87044,
            VirtualUndeleteRequest = 87045,
            StartRequestCreation = 87046,
            StartRequestEdit = 87047,
            CreateRequest = 87048,
            VirtualDeleteRequestField = 87049,
            VirtualUndeleteRequestField = 87050,
            VirtualDeleteRequestSection = 87051,
            VirtualUndeleteRequestSection = 87052,
            AddFieldToRequest = 87053,
            AddSectionToRequest = 87054,
            SaveRequestField = 87055,
            SaveRequestSection = 87056,
            EditAssignedSubmissions = 87057,
            ViewUnassignedEvaluation = 87058,
            AddCallSettings = 87059,
            SaveCallSettings = 87060,
            ViewCallAvailability = 87061,
            SaveCallAvailability = 87062,
            ViewCallMessages = 87063,
            SaveCallMessages = 87064,
            EditSubmittersType = 87065,
            SaveSubmittersType = 87066,
            EditAttachments = 87067,
            SaveAttachments = 87068,
            AddAttachments = 87069,
            DownloadCallFile = 87070,
            VirtualDeleteAttachment = 87071,
            VirtualDeleteSubmitterType = 87072,
            LoadCallEditor = 87073,
            SaveCallSections = 87074,
            VirtualDeleteFieldOption = 87075,
            AddFieldOption = 87076,
            ViewSubmittersTemplate = 87077,
            EditSubmitterTemplate = 87078,
            AddSubmitterTemplate = 87079,
            VirtualDeleteSubmitterTemplate = 87080,
            SaveSubmittersTemplate = 87081,
            EditManagerTemplate = 87082,
            SaveManagerTemplate = 87083,
            ViewRequestedFiles = 87084,
            AddRequestedFile = 87085,
            EditRequestedFile = 87086,
            VirtualDeleteRequestedFile = 87087,
            ViewRevision = 87088,
            SaveRevision = 87089,
            CompleteRevision = 87090,
            EditFieldsAssociation = 87091,
            SaveFieldsAssociation = 87092,
            LoadRevisionsList = 87093,
            LoadSubmissionsList = 87094,
            CloneRequest = 87095,
            SetAsDefaultOption = 87096,
            EditDisclaimerType = 87097,
            UploadFileToUnknownCall = 87098,
            AttachmentsNotAddedFiles = 87099,
            AttachmentsAddedFiles = 87100,

            VirtualUndeleteSubmission = 87120,
        }
        [Serializable]
        public enum ObjectType
        {
            None = 0,
            RequestForMembership = 100,
            FieldsSection = 2,
            FieldDefinition = 3,
            FieldValue = 4,
            RequestedFile = 5,
            SubmittedFile = 6,
            SubmitterType = 7,
            UserSubmission = 8,
            AttachmentFile = 9,
            Criterion = 10,
            Evaluator = 11,
            Evaluation = 12,
            Revision = 13
        }
    }
}