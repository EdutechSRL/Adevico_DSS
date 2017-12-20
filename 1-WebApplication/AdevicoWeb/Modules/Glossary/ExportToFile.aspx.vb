Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
Imports RootObject = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject

Public Class ExportToFile
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

    Private _Presenter As ExportToFilePresenter

    Private ReadOnly Property CurrentPresenter() As ExportToFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ExportToFilePresenter(PageUtility.CurrentContext, Me)
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
        With Resource
            .setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBExportGlossary, False, False, False, False)
            .setLinkButton(LNBselectCommunities, False, False, False, False)
            .setLiteral(LTglossaryExportToFile_s)
            .setLabel(LBglossaryExportToFileDescription_s)
        End With
        CTRLcommunitySelectorHeader.ModalTitle = Me.Resource.getValue("selectCommunities")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

    Public Overrides Sub BindDati()
        Master.DisplayTitleRow = False
        CurrentPresenter.InitView()
        Dim temp = GlossaryPath
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer) Implements IViewImportExport.LoadViewData
        Dim titleString = Resource.getValue("ImportExportGlossary")
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString


        LTpageTitle_t.Text = titleString
        CTRCommunityGlossaryTerms.BindDati(idCommunity)

        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(idCommunity)
        HYPback.Visible = idCommunity > 0

        ShowMessage(String.Empty, MessageType.none)
    End Sub

#Region "Community Selector"

    Private Sub CTRLglossaryShare_AddCommunityClicked() Handles LNBselectCommunities.Click
        ShowMessage(String.Empty, MessageType.none)
        CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    End Sub

    Public Sub DisplayCommunityToAdd(forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability) Implements IViewImportExport.DisplayCommunityToAdd
        ShowMessage(String.Empty, MessageType.none)
        CTRLcommunity.Visible = True
        Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
        If forAdministration Then
            CTRLcommunity.InitializeAdministrationControl(PageUtility.CurrentUser.ID, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLcommunity.InitializeControlByModules(PageUtility.CurrentUser.ID, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub

    Public Sub ShowCommunity(ByVal idCommunities As List(Of Integer)) Implements IViewImportExport.ShowCommunity
        ShowMessage(String.Empty, MessageType.none)
        CTRCommunityGlossaryTerms.LoadViewData(idCommunities)
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
        ShowMessage(String.Empty, MessageType.none)
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
        ShowMessage(String.Empty, MessageType.none)
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddNewCommunity(idCommunities)

        LNBExportGlossary.Visible = idCommunities.Count > 0
    End Sub

#End Region

    Public Function GetSelectedTermIds(ByVal idCom As Integer) As IEnumerable(Of Long)
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
        Dim idCom As Integer = 0
        Dim list = GetSelectedTermIds(idCom)
        If (list.count = 0) Then
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