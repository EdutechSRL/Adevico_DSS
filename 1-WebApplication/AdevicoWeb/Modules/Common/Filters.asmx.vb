Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Xml
Imports System.ComponentModel
Imports lm.Comol.Core.BaseModules.Dashboard.Business
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<System.Web.Script.Services.ScriptService> _
Public Class FiltersWebService
    Inherits System.Web.Services.WebService

    Private _service As lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities
    Private _serviceTags As lm.Comol.Core.BaseModules.Tags.Business.ServiceTags
    Private _serviceTiles As lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles
    Private _Context As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_Context) Then
                _Context = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _Context
        End Get
    End Property
    Private ReadOnly Property Service As lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities
        Get
            If IsNothing(_service) Then
                _service = New lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(CurrentContext)
            End If
            Return _service
        End Get
    End Property
    Private ReadOnly Property ServiceTags As lm.Comol.Core.BaseModules.Tags.Business.ServiceTags
        Get
            If IsNothing(_serviceTags) Then
                _serviceTags = New lm.Comol.Core.BaseModules.Tags.Business.ServiceTags(CurrentContext)
            End If
            Return _serviceTags
        End Get
    End Property
    Private ReadOnly Property ServiceTiles As lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles
        Get
            If IsNothing(_serviceTiles) Then
                _serviceTiles = New lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles(CurrentContext)
            End If
            Return _serviceTiles
        End Get
    End Property

    Private _Resource As New Dictionary(Of String, ResourceManager)
    Protected Function GetResource(modulecode As String) As ResourceManager
        If _Resource.ContainsKey(modulecode) AndAlso Not IsNothing(_Resource(modulecode)) Then
            Return _Resource(modulecode)
        Else
            Select Case modulecode
                Case lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode, lm.Comol.Core.Dashboard.Domain.ModuleDashboard.UniqueCode
                    Dim dResource As New ResourceManager
                    dResource.UserLanguages = CurrentContext.UserContext.Language.Code
                    dResource.ResourcesName = "pg_Dashboard"
                    dResource.Folder_Level1 = "Modules"
                    dResource.Folder_Level2 = "Dashboard"
                    dResource.setCulture()
                    _Resource(modulecode) = dResource
                    Return dResource

                Case Else
                    Return New ResourceManager
            End Select
        End If
    End Function

    Private Property TransactionFilters As Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter))
        Get
            Return SessionOrDefault("TransactionFilters", New Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter)))
        End Get
        Set(value As Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter)))
            Session("TransactionFilters") = value
        End Set
    End Property
    Private Property TransactionFiltersTranslated As Dictionary(Of String, Boolean)
        Get
            Return SessionOrDefault("TransactionFiltersTranslated", New Dictionary(Of String, Boolean))
        End Get
        Set(value As Dictionary(Of String, Boolean))
            Session("TransactionFiltersTranslated") = value
        End Set
    End Property
    Public Function SessionOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (Session(Key) Is Nothing) Then
            Session(Key) = DefaultValue
            Return DefaultValue
        Else
            Return Session(Key)
        End If
    End Function

    Private Sub GenerateException(dto As List(Of lm.Comol.Core.DomainModel.Filters.Filter), Optional inDialog As Boolean = True, Optional closeDialog As Boolean = False)
        Dim err As New lm.Comol.Core.DomainModel.Domain.JsonError(Of List(Of lm.Comol.Core.DomainModel.Filters.Filter)) With {.ReturnObject = dto}
        err.Message = GetResource(lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode).getValue("TagException.ErrorMessage." & lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout.ToString)
        err.Level = lm.Comol.Core.DomainModel.Helpers.MessageType.error.ToString
        err.MessageDialog = inDialog
        err.CloseDialog = closeDialog
        Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)
    End Sub
    <WebMethod(True)> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetFilters(search As Dictionary(Of String, String), transactionid As String, filtermodule As String, filtermodulescope As String, filteridLanguage As String, requiredpermissions As String, unloadcommunitites As String, availabilitystring As String, onlyfromOrganizations As String) As List(Of lm.Comol.Core.DomainModel.Filters.Filter)
        If CurrentContext.UserContext.isAnonymous Then
            Dim fItems As New List(Of lm.Comol.Core.DomainModel.Filters.Filter)

            If TransactionFilters.ContainsKey(transactionid & "_" & filtermodule & "_" & filtermodulescope) Then
                fItems = TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope)
            End If
            GenerateException(fItems, False)
        Else
            Dim filters As New List(Of lm.Comol.Core.DomainModel.Filters.Filter)
            Dim helper As New FilterHelpers(CurrentContext.UserContext.Language)
            If TransactionFilters.ContainsKey(transactionid & "_" & filtermodule & "_" & filtermodulescope) Then
                'filters = TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope)
                'If Not TransactionFiltersTranslated.ContainsKey(transactionid & "_" & filtermodule & "_" & filtermodulescope) OrElse Not TransactionFiltersTranslated(transactionid & "_" & filtermodule & "_" & filtermodulescope) Then
                '    helper.AnalyzeFiltersForTranslation(filters, filtermodule, filtermodulescope)
                '    TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope) = filters
                '    TransactionFiltersTranslated(transactionid & "_" & filtermodule & "_" & filtermodulescope) = True
                'End If
                Return TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope)
            End If
            Dim idCommunity As Integer = 0

            Select Case filtermodule
                Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.UniqueCode
                    Select Case filtermodulescope
                        Case "Tile"
                            If String.IsNullOrEmpty(filteridLanguage) Then
                                filteridLanguage = "-1"
                            End If
                            Dim forPortal As Boolean = False
                            If search.ContainsKey(UrlKeyTiles.forPortal.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.forPortal.ToString)) Then
                                Boolean.TryParse(search(UrlKeyTiles.forPortal.ToString), forPortal)
                            End If
                            Dim recycleBin As Boolean = False
                            If search.ContainsKey(UrlKeyTiles.recycle.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.recycle.ToString)) Then
                                Boolean.TryParse(search(UrlKeyTiles.recycle.ToString), recycleBin)
                            End If
                            Dim type As lm.Comol.Core.Dashboard.Domain.DashboardType = lm.Comol.Core.Dashboard.Domain.DashboardType.Portal
                            If search.ContainsKey(UrlKeyTiles.type.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.type.ToString)) Then
                                type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Dashboard.Domain.DashboardType).GetByString(search(UrlKeyTiles.type.ToString), lm.Comol.Core.Dashboard.Domain.DashboardType.Portal)
                            End If
                            Dim idTilesCommunity As Integer = 0
                            If search.ContainsKey(UrlKeyTiles.idCommunity.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.idCommunity.ToString)) Then
                                Integer.TryParse(search(UrlKeyTiles.idCommunity.ToString), idTilesCommunity)
                            End If
                            If idTilesCommunity = 0 AndAlso type = lm.Comol.Core.Dashboard.Domain.DashboardType.Community Then
                                idTilesCommunity = CurrentContext.UserContext.CurrentCommunityID
                            End If


                            idCommunity = CurrentContext.UserContext.CurrentCommunityID

                            Filters = ServiceTiles.GetDefaultFilters(type, CurrentContext.UserContext.CurrentUserID, idCommunity, filteridLanguage, idTilesCommunity, recycleBin).OrderBy(Function(f) f.DisplayOrder).ToList()
                        Case "MultipleTagAssignments"
                            Filters = Service.GetDefaultFiltersForAssignments(CurrentContext.UserContext.CurrentUserID, "", -1, Nothing, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All).OrderBy(Function(f) f.DisplayOrder).ToList()
                        Case Else
                            Dim searchBy As String = ""
                            If search.ContainsKey(UrlKey.t.ToString) Then
                                searchBy = search(UrlKey.t.ToString)
                                If Not String.IsNullOrEmpty(searchBy) Then
                                    searchBy = Server.UrlDecode(searchBy)
                                End If
                            End If

                            Dim idCommunityType As Integer = -1
                            If search.ContainsKey(UrlKey.idType.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idType.ToString)) AndAlso IsNumeric(search(UrlKey.idType.ToString)) Then
                                idCommunityType = CInt(search(UrlKey.idType.ToString))
                            ElseIf search.ContainsKey(UrlKey.idType.ToString.ToLower) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idType.ToString.ToLower)) AndAlso IsNumeric(search(UrlKey.idType.ToString.ToLower)) Then
                                idCommunityType = CInt(search(UrlKey.idType.ToString.ToLower))
                            End If
                            Dim idTile As Long = -1
                            If search.ContainsKey(UrlKey.idTile.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idTile.ToString)) AndAlso IsNumeric(search(UrlKey.idTile.ToString)) Then
                                idTile = CLng(search(UrlKey.idTile.ToString))
                            ElseIf search.ContainsKey(UrlKey.idTile.ToString.ToLower) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idTile.ToString.ToLower)) AndAlso IsNumeric(search(UrlKey.idTile.ToString.ToLower)) Then
                                idTile = CLng(search(UrlKey.idTile.ToString.ToLower))
                            End If
                            Dim availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
                            If filtermodulescope = "subscribe" Then
                                availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.NotSubscribed
                            ElseIf search.ContainsKey(UrlKey.subscribe.ToString) Then
                                If CBool(search(UrlKey.subscribe.ToString)) Then
                                    availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.NotSubscribed
                                End If
                            End If

                            Filters = Service.GetDefaultFilters(CurrentContext.UserContext.CurrentUserID, searchBy, idCommunityType, idTile, Nothing, Nothing, availability).OrderBy(Function(f) f.DisplayOrder).ToList()
                    End Select

                Case lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode
                    Dim forOrganization As Boolean = False
                    If search.ContainsKey(UrlKeyTags.forOrganization.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTags.forOrganization.ToString)) Then
                        Boolean.TryParse(search(UrlKeyTags.forOrganization.ToString), forOrganization)
                    End If

                    If forOrganization Then
                        idCommunity = CurrentContext.UserContext.CurrentCommunityID
                    End If
                    Dim recycleBin As Boolean = False
                    If search.ContainsKey(UrlKeyTags.recycle.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTags.recycle.ToString)) Then
                        Boolean.TryParse(search(UrlKeyTags.recycle.ToString), recycleBin)
                    End If
                    If String.IsNullOrEmpty(filteridLanguage) Then
                        filteridLanguage = "-1"
                    End If

                    Select Case filtermodulescope
                        Case "Community"
                            Filters = ServiceTags.GetDefaultFilters(lm.Comol.Core.Tag.Domain.TagType.Community, CurrentContext.UserContext.CurrentUserID, idCommunity, CInt(filteridLanguage), forOrganization, recycleBin).OrderBy(Function(f) f.DisplayOrder).ToList()
                    End Select

            End Select

            helper.AnalyzeFiltersForTranslation(Filters, filtermodule, filtermodulescope)
            TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope) = Filters
            Return Filters
        End If
    End Function

    <WebMethod(True)> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SetFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), url As String, search As Dictionary(Of String, String), transactionid As String, filtermodule As String, filtermodulescope As String, filteridLanguage As String, requiredpermissions As String) As List(Of lm.Comol.Core.DomainModel.Filters.Filter)
        If Not CurrentContext.UserContext.isAnonymous Then
            TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope) = filters
        End If
        Return filters
    End Function


    <WebMethod(True)> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SetFiltersChange(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), filter As lm.Comol.Core.DomainModel.Filters.Filter, reason As String, url As String, search As Dictionary(Of String, String), transactionid As String, filtermodule As String, filtermodulescope As String, filteridLanguage As String) As List(Of lm.Comol.Core.DomainModel.Filters.Filter)
        SetFiltersChange(filters, filter, reason, url, search, transactionid, filtermodule, filtermodulescope, filteridLanguage, "", "", "", "")
    End Function
    <WebMethod(True)> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SetFiltersChange(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), filter As lm.Comol.Core.DomainModel.Filters.Filter, reason As String, url As String, search As Dictionary(Of String, String), transactionid As String, filtermodule As String, filtermodulescope As String, filteridLanguage As String, requiredpermissions As String, unloadcommunitites As String, availabilitystring As String, onlyfromOrganizations As String) As List(Of lm.Comol.Core.DomainModel.Filters.Filter)
        If CurrentContext.UserContext.isAnonymous Then
            GenerateException(filters, False)
        Else
            Dim idCommunity As Integer = 0
            Dim changedFilters As List(Of lm.Comol.Core.DomainModel.Filters.Filter) = New List(Of lm.Comol.Core.DomainModel.Filters.Filter)
            Select Case filtermodule
                Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.UniqueCode
                    Select Case filtermodulescope
                        Case "Tile"
                            If String.IsNullOrEmpty(filteridLanguage) Then
                                filteridLanguage = "-1"
                            End If
                            Dim forPortal As Boolean = False
                            If search.ContainsKey(UrlKeyTiles.forPortal.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.forPortal.ToString)) Then
                                Boolean.TryParse(search(UrlKeyTiles.forPortal.ToString), forPortal)
                            End If
                            Dim recycleBin As Boolean = False
                            If search.ContainsKey(UrlKeyTiles.recycle.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.recycle.ToString)) Then
                                Boolean.TryParse(search(UrlKeyTiles.recycle.ToString), recycleBin)
                            End If
                            Dim type As lm.Comol.Core.Dashboard.Domain.DashboardType = lm.Comol.Core.Dashboard.Domain.DashboardType.Portal
                            If search.ContainsKey(UrlKeyTiles.type.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.type.ToString)) Then
                                type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Dashboard.Domain.DashboardType).GetByString(search(UrlKeyTiles.type.ToString), lm.Comol.Core.Dashboard.Domain.DashboardType.Portal)
                            End If
                            Dim idTilesCommunity As Integer = 0
                            If search.ContainsKey(UrlKeyTiles.idCommunity.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTiles.idCommunity.ToString)) Then
                                Integer.TryParse(search(UrlKeyTiles.idCommunity.ToString), idTilesCommunity)
                            End If
                            If idTilesCommunity = 0 AndAlso type = lm.Comol.Core.Dashboard.Domain.DashboardType.Community Then
                                idTilesCommunity = CurrentContext.UserContext.CurrentCommunityID
                            End If
                            idCommunity = CurrentContext.UserContext.CurrentCommunityID
                            changedFilters = ServiceTiles.ChangeFilters(type, CurrentContext.UserContext.CurrentUserID, filters, filter, idTilesCommunity, filteridLanguage, recycleBin).OrderBy(Function(f) f.DisplayOrder).ToList()
                        Case "MultipleTagAssignments"
                            changedFilters = Service.ChangeFilters(True, CurrentContext.UserContext.CurrentUserID, filters, filter, -1, -1, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All).OrderBy(Function(f) f.DisplayOrder).ToList()

                        Case Else
                            Dim searchBy As String = ""
                            If search.ContainsKey(UrlKey.t.ToString) Then
                                searchBy = search(UrlKey.t.ToString)
                                If Not String.IsNullOrEmpty(searchBy) Then
                                    searchBy = Server.UrlDecode(searchBy)
                                End If
                            End If

                            Dim idCommunityType As Integer = -1
                            If search.ContainsKey(UrlKey.idType.ToString) Then
                                idCommunityType = CInt(search(UrlKey.idType.ToString))
                            ElseIf search.ContainsKey(UrlKey.idType.ToString.ToLower) Then
                                idCommunityType = CInt(search(UrlKey.idType.ToString.ToLower))
                            End If
                            Dim idTile As Long = -1
                            If search.ContainsKey(UrlKey.idTile.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idTile.ToString)) AndAlso IsNumeric(search(UrlKey.idTile.ToString)) Then
                                idTile = CLng(search(UrlKey.idTile.ToString))
                            ElseIf search.ContainsKey(UrlKey.idTile.ToString.ToLower) AndAlso Not String.IsNullOrEmpty(search(UrlKey.idTile.ToString.ToLower)) AndAlso IsNumeric(search(UrlKey.idTile.ToString.ToLower)) Then
                                idTile = CLng(search(UrlKey.idTile.ToString.ToLower))
                            End If
                            Dim availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
                            If filtermodulescope = "subscribe" Then
                                availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.NotSubscribed
                            ElseIf search.ContainsKey(UrlKey.subscribe.ToString) Then
                                If CBool(search(UrlKey.subscribe.ToString)) Then
                                    availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.NotSubscribed
                                End If
                            End If
                            If Not String.IsNullOrWhiteSpace(availabilitystring) Then
                                availability = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability).GetByString("availabilitystring", availability)
                            End If
                            changedFilters = Service.ChangeFilters(False, CurrentContext.UserContext.CurrentUserID, filters, filter, idCommunityType, idTile, availability, GetListFromString(unloadcommunitites), GetListFromString(onlyfromOrganizations), GetRequiredPermissions(requiredpermissions)).OrderBy(Function(f) f.DisplayOrder).ToList()
                    End Select
                Case lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode
                    Dim forOrganization As Boolean = False
                    If search.ContainsKey(UrlKeyTags.forOrganization.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTags.forOrganization.ToString)) Then
                        Boolean.TryParse(search(UrlKeyTags.forOrganization.ToString), forOrganization)
                    End If

                    If forOrganization Then
                        idCommunity = CurrentContext.UserContext.CurrentCommunityID
                    End If
                    Dim recycleBin As Boolean = False
                    If search.ContainsKey(UrlKeyTags.recycle.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTags.recycle.ToString)) Then
                        Boolean.TryParse(search(UrlKeyTags.recycle.ToString), recycleBin)
                    End If
                    If String.IsNullOrEmpty(filteridLanguage) Then
                        filteridLanguage = "-1"
                    End If

                    Select Case filtermodulescope
                        Case "Community"
                            changedFilters = ServiceTags.ChangeFilters(lm.Comol.Core.Tag.Domain.TagType.Community, CurrentContext.UserContext.CurrentUserID, filters, filter, idCommunity, CInt(filteridLanguage), forOrganization, recycleBin).OrderBy(Function(f) f.DisplayOrder).ToList()
                    End Select
                Case Else
                    Return filters
            End Select
            Dim helper As New FilterHelpers(CurrentContext.UserContext.Language)
            helper.AnalyzeFiltersForTranslation(changedFilters, filtermodule, filtermodulescope)
            TransactionFilters(transactionid & "_" & filtermodule & "_" & filtermodulescope) = changedFilters
            Return changedFilters
        End If
    End Function

    Private Function GetRequiredPermissions(requiredpermissions As String) As Dictionary(Of Integer, Long)
        If String.IsNullOrWhiteSpace(requiredpermissions) Then
            Return Nothing
        Else
            requiredpermissions.Split(",").ToDictionary(Function(s) s.Split("-")(0), Function(s) s.Split("-")(1))
        End If
    End Function
    Private Function GetListFromString(unloadCommunitites As String) As List(Of Integer)
        Try
            If String.IsNullOrWhiteSpace(unloadCommunitites) Then
                Return New List(Of Integer)
            Else
                unloadCommunitites.Split(",").Select(Function(s) CInt(s)).Distinct().ToList()
            End If
        Catch ex As Exception
            Return New List(Of Integer)
        End Try
    End Function
    Private Enum UrlKey
        idType = 0
        s = 1
        t = 2
        subscribe = 3
        idTile = 4
    End Enum
    Private Enum UrlKeyTags
        recycle = 0
        forOrganization = 1
    End Enum
    Private Enum UrlKeyTiles
        recycle = 0
        forPortal = 1
        idCommunity = 2
        type = 3

    End Enum
End Class


'[WebMethod(true)]
'[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
'public List<Filter> GetFilters()
'{
'    List<Filter> filters = new List<Filter>();

'    Filter f = new Filter() { Name = "Filter1", Label = "Label 1", Value = "Text", FilterType = FilterType.Text, AutoUpdate = true };

'    filters.Add(f);

'    f = new Filter(new FilterListItem(0, "", true)) { Name = "Filter2", Label = "Label 2", FilterType = FilterType.Checkbox, AutoUpdate = true };

'    filters.Add(f);

'    f = new Filter(
'            new FilterListItem(1, "Option 1"),
'            new FilterListItem(2, "Option 2", true),
'            new FilterListItem(3, "Option 1")
'        ) { Name = "Filter3", Label = "Label 3", FilterType = FilterType.Checkbox, AutoUpdate = true };

'    filters.Add(f);

'    f = new Filter(
'            new FilterListItem(1, "Option 1"),
'            new FilterListItem(2, "Option 2"),
'            new FilterListItem(3, "Option 3")
'        ) { Name = "Filter4", Label = "Label 4", FilterType = FilterType.Radio, Selected = 1, AutoUpdate = true };

'    filters.Add(f);

'    f = new Filter(
'            new FilterListItem(1, "Opt 1"),
'            new FilterListItem(2, "Opt 2"),
'            new FilterListItem(3, "Opt 3")
'        ) { Name = "Filter5", Label = "Label 5", Value = "Text", FilterType = FilterType.TextSelect, Selected = 1, AutoUpdate = true };

'    filters.Add(f);

'    f = new Filter(
'            new FilterListItem(1, "Opt 1"),
'            new FilterListItem(2, "Opt 2"),
'            new FilterListItem(3, "Opt 3")
'        ) { Name = "Filter6", Label = "Label 6", FilterType = FilterType.Select, Selected = 1, AutoUpdate = false };

'    filters.Add(f);

'    return filters;
'}


