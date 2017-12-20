using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewEditPermissions : IViewBaseEdit 
    {
        Int32 IdEditingCommunity { get; set; }
        Int32 MaxDisplayUsers { get; }
        Boolean ForPortal { get; set; }
        
        //Boolean IsPublic { get; set; }

        String UnknownUserTranslation { get; }
        String UnknownCommunityTranslation { get; }
        List<Int32> SelectedRoles();
        List<Int32> SelectedProfileTypes();
        List<dtoProfileTypeAssignment> GetPortalPermissions();
        List<dtoRoleAssignment> GetRolePermissions();
        List<dtoTemplateAssignment> GetPermissions();
        List<dtoPersonAssignment> GetPersonPermissions();
        

        Dictionary<Int32, String> GetTranslatedRoles();
        Dictionary<Int32, String> GetTranslatedProfileTypes();
        Dictionary<String, String> GetTranslatedModules();
        List<Int32> SelectedIdUsers { get; set; }
        List<dtoSelectItem<Int32>> GetCurrentSelectedUsers();


        void DisplayPortalPermissionsSaved();
        void DisplayPortalPermissionsErrorSaving();
        //void DisplayTemplatePermissionDeleted();
        //void DisplayTemplatePermissionsDeleted();
        //void DisplayTemplatePermissionErrorDeleting();
        //void DisplayTemplatePermissionsErrorDeleting();

        void LoadAvailableAssignments(List<PermissionType> items);
        void LoadAssignments(List<dtoTemplateAssignment> assignments);
        ////List<KeyValuePair<Int32,String>> AvailableUsers { get; set; }

        void LoadCommunityAssignment(dtoCommunityAssignment assignment, String communityName, Int32 idCommunity, List<Int32> availableRoles);
        //void DisplayCommunityToAdd(Int32 idProfile, Boolean forAdministration, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability);

        void InitializeCommunityToAdd(Int32 idProfile, Boolean forAdministration, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability);

        void DisplayAddUsersFromCommunity(Int32 idCommunity, List<Int32> removeUsers);
        void DisplayAddUsersFromCommunity(List<Int32> idCommunities, List<Int32> removeUsers);
        void DisplayAddUsersFromPortal(List<Int32> removeUsers);

       
        ////void DisplayNoAvailability();
        void DisplayNoAssignments();
        ////void DisplayAvailabilitySaved();
        ////void DisplaySaveErrors(Boolean display);
        void DisplayUserAssignmentsSaved();
        void DisplayCommunityAssignmentAdded(String name);
        void DisplayCommunityAssignmentsAdded();
        void DisplayCommunityAssignmentDeleted(String name);
        void DisplayCommunityAssignmentErrorSaving();
        void DisplayCommunityAssignmentSaved(String name);
        void DisplayCommunityDeletingError();
        void DisplayCommunityAddingError();
        void DisplayUserAssignmentsAdded();
        void DisplayUserAddingError();
        void DisplayUserAssigmentsSavingError();
    }
}