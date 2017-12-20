
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Presentation
{
    public interface IViewModuleItemUserSelector : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long UsersCount { get; set; }
        Boolean AllowByAnonymous { get; set; }
        Boolean ShowOtherColumns { get; set; }
        Boolean ShowMailColumns { get; set; }
        Boolean ShowPreview { get; set; }
        Boolean HasPersonToSelect { get; }
        Boolean HasPermissionToSelectUsers { get; }
        Boolean isInitialized { get; set; }
        Boolean EnableControl { get; set; }
        OrderContact CurrentOrder { get; set; }
        Boolean OrderAscending { get; set; }
        List<int> SelectedCommunitiesId { get; }
        int PageSize { get; set; }
        List<FilterParameter> FiltersAvailable { get; set; }
        System.Web.UI.WebControls.ListSelectionMode SelectionMode { get; set; }
        void LoadSelectedFilters( List<FilterParameter> filters);
        void LoadUsers(List<dtoMemberContact> users);
        void LoadPreviewSelectedUsers(List<dtoMemberContact> users);
        Boolean SelectedContacts(ref List<int> selectedId, ref List<int> removedId);

       // List<dtoMemberContact> GetContactsFromService(
        void InitializeNoPermission(int idCommunity);
        void InitializeView(String moduleCode, int objectId, int objectTypeId, IList<int> selectedUsersId,IList<int> exceptUsersId, int idCommunity);
        void InitializeView(String moduleCode, int objectId, int objectTypeId, IList<int> selectedUsersId, IList<int> exceptUsersId, List<int> idCommunities);
        void UnselectAll();
        

    //     ReadOnly Person Name() As String
    //ReadOnly Person Surname() As String
    //ReadOnly Person RegistrationCode() As String
    //ReadOnly Person MailAddress() As String
    //ReadOnly Person Login() As String
    //ReadOnly Person SelectedRoleId() As Integer
    //ReadOnly Person PreviewUserList_isVisible() As Boolean
    //ReadOnly Person CurrentUserId() As Integer

    //Person OrderBy() As String
    //Person Direction() As Comol.Entity.sortDirection
    //Person GridPageSize() As Integer
    //Person GridCurrentPage() As Integer
    //Person GridMaxPage() As Integer


    //Sub SetRegistrationCodeVisibility(ByRef isVisible As Boolean)
    //Sub init(ByRef oRoleList As List(Of Role))

    //Function GetUsers() As List(Of BaseElement)
    //Sub BindRolesByCommunities(ByRef oRoleList As List(Of Role))
    //Sub BindPreview(ByRef oUserList As List(Of MemberContact))
    //Sub BindSearchResult(ByRef oUserList As List(Of MemberContact))

    //Person SelectedCommunitiesId() As List(Of Integer)


    }
}