using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditCallAvailability : IViewEditCall
    {
        Int32 MaxDisplayUsers { get; }
        Boolean ForPortal { get; set; }
        Boolean IsPublic { get; set; }
        Boolean isSkinSelectorVisible { get; }
        String UnknownUserTranslation { get; }
        String UnknownCommunityTranslation { get; }
        //List<KeyValuePair<Int32,String>> AvailableUsers { get; set; }


        void LoadAvailableAssignments(List<CallAssignmentType> items);
        void LoadAssignments(List<dtoCallAssignment> assignments);
        void LoadCommunityAssignment(dtoCallCommunityAssignment assignment, String communityName, Int32 idCommunity, List<Int32> availableRoles);
        void DisplayCommunityToAdd(Int32 idProfile, Boolean forAdministration, Dictionary<Int32,long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability);
        Int32 IdEditingCommunity { get; set; }
        void DisplayAddUsersFromCall(CallForPaperType typeToLoad, Boolean fromPortal, List<Int32> fromCommunities, List<Int32> removeUsers);
        void DisplayAddUsersFromCommunity(Int32 idCommunity,List<Int32> removeUsers);
        void DisplayAddUsersFromCommunity(List<Int32> idCommunities, List<Int32> removeUsers);
        void DisplayAddUsersFromPortal(List<Int32> removeUsers);

        Dictionary<Int32, String> GetTranslatedRoles();
        Dictionary<Int32, String> GetTranslatedProfileTypes();
        List<Int32> SelectedIdUsers { get; set; }
        List<dtoSelectItem<Int32>> GetCurrentSelectedUsers();


        void HideSkinsInfo();
        void LoadSkinInfo(long idCall,String fullyQualifiedName,ModuleCallForPaper.ObjectType itemType, Int32 idModule, Int32 idCommunity, Boolean allowAdd, Boolean allowEdit, Boolean loadAll);
        void LoadSkinInfo(long idCall, String fullyQualifiedName, ModuleRequestForMembership.ObjectType itemType, Int32 idModule, Int32 idCommunity, Boolean allowAdd, Boolean allowEdit, Boolean loadAll);
        //void LoadPersonAssignments(List<dtoCallPersonAssignment> items);
        
       
        List<Int32> SelectedRoles();
        List<Int32> SelectedProfileTypes();
        void DisplayNoAvailability();
        void DisplayNoAssignments();
        void DisplayAvailabilitySaved();
        void DisplaySaveErrors(Boolean display);
        void DisplayCommunityAssignmentAdded(String name);
        void DisplayCommunityAssignmentsAdded();
        void DisplayCommunityAssignmentDeleted(String name);
        void DisplayCommunityAssignmentSaved(String name);
        void DisplayUserAssignmentsAdded();
        void DisplayPublicUrl(CallForPaperType type, string callUrl);
        void DisplayPublicUrls(CallForPaperType type, string callUrl, string collectorUrl);
        void HidePublicUrl();
        //void GoToUrl(String url);
    }
}