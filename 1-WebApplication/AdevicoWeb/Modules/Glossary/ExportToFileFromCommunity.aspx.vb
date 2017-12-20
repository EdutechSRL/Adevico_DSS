Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
Imports RootObject = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject

Public Class ExportToFileFromCommunity
    Inherits GLpageBase
    Implements IViewImportExport

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

#Region "Context"

    Private _Presenter As ExportToFileCommunityPresenter

    Private ReadOnly Property CurrentPresenter() As ExportToFileCommunityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ExportToFileCommunityPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Private _glossaryPath As String

    Private ReadOnly Property GlossaryPath As String Implements IViewImportExport.GlossaryPath
        Get
            If String.IsNullOrEmpty(_glossaryPath) Then
                If Not String.IsNullOrEmpty(SystemSettings.File.Glossary.DrivePath) Then
                    _glossaryPath = SystemSettings.File.Glossary.DrivePath
                Else
                    _glossaryPath = Server.MapPath(String.Format("{0}{1}", PageUtility.BaseUrl, SystemSettings.File.Glossary.VirtualPath))
                End If
            End If
            Return _glossaryPath
        End Get
    End Property

#End Region

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBExportGlossary, False, False, False, False)
            .setLiteral(LTglossaryExportToFile_s)
            .setLabel(LBglossaryExportToFileDescription_s)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

    Public Overrides Sub BindDati()
        CurrentPresenter.InitView()
        Master.DisplayTitleRow = False
        Dim temp = GlossaryPath
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer) Implements IViewImportExport.LoadViewData
        Dim titleString = Resource.getValue("ExportToFileFromCommunityTitle")
        Master.DisplayTitleRow = False
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
        ShowMessage(String.Empty, MessageType.none)

        CTRCommunityGlossaryTerms.BindDati(idCommunity)
        Dim list As New List(Of Int32)
        list.Add(idCommunity)
        CTRCommunityGlossaryTerms.LoadViewData(list)
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddNewCommunity(list)
        LNBExportGlossary.Visible = list.Count > 0
        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(idCommunity)
    End Sub

    Public Sub DisplayCommunityToAdd(ByVal forAdministration As Boolean, ByVal requiredPermissions As Dictionary(Of Integer, Long), ByVal unloadIdCommunities As List(Of Integer), ByVal availability As CommunityAvailability) Implements IViewImportExport.DisplayCommunityToAdd
    End Sub

    Public Sub ShowCommunity(ByVal idCommunities As List(Of Integer)) Implements IViewImportExport.ShowCommunity
    End Sub


#Region "Community Selector"

    'Private Sub CTRLglossaryShare_AddCommunityClicked() Handles LNBselectCommunities.Click
    '    CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    'End Sub

    'Public Sub DisplayCommunityToAdd(forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability) Implements IViewImportExport.DisplayCommunityToAdd
    '    CTRLcommunity.Visible = True
    '    Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
    '    If forAdministration Then
    '        CTRLcommunity.InitializeAdministrationControl(PageUtility.CurrentUser.ID, unloadIdCommunities, availability, New List(Of Integer))
    '    Else
    '        CTRLcommunity.InitializeControlByModules(PageUtility.CurrentUser.ID, requiredPermissions, unloadIdCommunities, availability)
    '    End If
    'End Sub

    'Public Sub ShowCommunity(ByVal idCommunities As List(Of Integer)) Implements IViewImportExport.ShowCommunity
    '    CTRCommunityGlossaryTerms.LoadViewData(idCommunities)
    'End Sub

    'Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
    '    CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    'End Sub

    'Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
    '    Master.SetOpenDialogOnPostbackByCssClass("")
    '    CurrentPresenter.AddNewCommunity(idCommunities)

    '    LNBExportGlossary.Visible = idCommunities.Count > 0
    'End Sub

#End Region

    Public Function GetSelectedTermIds() As IEnumerable(Of Long)
        ShowMessage(String.Empty, MessageType.none)
        Dim result As New List(Of Int64)

        For Each itemGlossary As RepeaterItem In CTRCommunityGlossaryTerms.GetRPTCommunites.Items
            Dim ucGlossaryImportTerms As UC_GlossaryImportTerms = itemGlossary.FindControl("UCglossaryTerms")
            If Not IsNothing(ucGlossaryImportTerms) Then
                result.AddRange(ucGlossaryImportTerms.GetSelectedTermIds(True))
            End If
        Next
        result = result.Distinct().ToList()
        Return result
    End Function

    Private Sub LNBExportGlossary_Click(sender As Object, e As EventArgs) Handles LNBExportGlossary.Click
        ShowMessage(String.Empty, MessageType.none)
        Dim list As IEnumerable(Of Long) = GetSelectedTermIds()
        If (list.Count() = 0) Then
            ShowMessage("ImportExport.NothingToExport", MessageType.error)
            Return
        End If

        If Not CurrentPresenter.ExportGlossaries(list) Then
            ShowMessage("ImportExport.ErrorExport", MessageType.error)
        End If
    End Sub

    Public Sub ExportGlossaries(ByVal content As String, ByVal fileName As String) Implements IViewImportExport.ExportGlossaries
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
        Response.ContentType = "application/xml"
        Response.Write(content)
        Response.End()
    End Sub

#Region "Uploader"

    Public Sub ShowMessage(ByVal messageKey As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(messageKey)) Then
            DVmessages.Attributes.Add("class", LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            DVmessages.Attributes.Add("class", LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(Resource.getValue(messageKey), type)
        End If
    End Sub

#End Region
End Class