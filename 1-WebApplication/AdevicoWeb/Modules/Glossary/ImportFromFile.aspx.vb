Imports System.IO
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.File
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
Imports RootObject = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject

Public Class ImportFromFile
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

    Private _Presenter As ImportFromFilePresenter

    Private ReadOnly Property CurrentPresenter() As ImportFromFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ImportFromFilePresenter(PageUtility.CurrentContext, Me)
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

    Public Property CurrentKey As String
        Get
            Return ViewStateOrDefault("CurrentKey", String.Empty)
        End Get
        Set(value As String)
            ViewState("CurrentKey") = value
        End Set
    End Property


#End Region

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBImportGlossaryFromFile, False, False, False, False)
            .setLiteral(LTglossaryImportFromFile_s)
            .setLabel(LBglossaryImportFromFileDescription_s)
            .setLiteral(LTImportFromFileOptions)
            .setLiteral(LTuploadFile_s)
            .setLiteral(LTuploadFileText_s)
            .setLinkButton(LNBImportGlossaries, False, False, False, False)
            .setHyperLink(HYPcloseModal, False, False, False, False)
            .setLabel(LBuploadedfile_s)
            .setHyperLink(HYPdelete, False, False, False, False)
            .setHyperLink(HYPchange, False, False, False, False)
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
        Dim titleString = Resource.getValue("ImportExportGlossary")
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        Master.DisplayTitleRow = False

        LTpageTitle_t.Text = titleString
        CTRCommunityGlossaryTerms.BindDati(idCommunity)
        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(idCommunity)
        HYPback.Visible = idCommunity > 0
        RBImportOptions.Items.Clear()

        Dim rbAllPublished = New ListItem(Resource.getValue("AllPublished"), "1")
        rbAllPublished.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllPublished)

        Dim rbAllDraft = New ListItem(Resource.getValue("AllDraft"), "2")
        rbAllDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllDraft)

        Dim rbAlreadyExistAsDraft = New ListItem(Resource.getValue("DraftIfAlreadyExist"), "3")
        rbAlreadyExistAsDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAlreadyExistAsDraft)

        RBImportOptions.SelectedIndex = 0

        ShowMessage(String.Empty, MessageType.none)
    End Sub

    Public Function GetSelectedTermIds() As Dictionary(Of Int32, List(Of Int64))
        Dim result As New Dictionary(Of Int32, List(Of Int64))

        For Each itemGlossary As RepeaterItem In CTRCommunityGlossaryTerms.GetRPTCommunites.Items
            Dim ucGlossaryImportTerms As UC_GlossaryImportTerms = itemGlossary.FindControl("UCglossaryTerms")
            If Not IsNothing(ucGlossaryImportTerms) Then

                Dim dic As Dictionary(Of Int32, List(Of Int64)) = ucGlossaryImportTerms.GetSelectedTermIdsToCommunity(False)
                For Each item As Int32 In dic.Keys
                    If Not result.ContainsKey(item) Then
                        result.Add(item, New List(Of Long)())
                    End If
                    result(item).AddRange(dic(item))
                Next

            End If
        Next
        Return result
    End Function

    Public Sub ExportGlossaries(ByVal content As String, ByVal fileName As String) Implements IViewImportExport.ExportGlossaries
    End Sub

    
    ''' <summary>
    '''     Metodo che esegue import dei glossari-termini selezionati nella comunità corrente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LNBConfirmImport_Click(sender As Object, e As EventArgs) Handles LNBImportGlossaryFromFile.Click
        Dim filenamePath As String = CurrentPresenter.GetFullUserPath(LBselectedFileName.Text)
        If (File.Exists(filenamePath)) Then
            Dim idList As Dictionary(Of Int32, List(Of Int64)) = GetSelectedTermIds()

            If CurrentPresenter.ImportGlossaries(filenamePath, idList, RBImportOptions.SelectedIndex) Then
                ShowMessage("ImportExport.SuccessImport", MessageType.success)
            Else
                ShowMessage("ImportExport.ErrorImport", MessageType.error)
            End If
        Else
            ShowMessage("ImportExport.ErrorImport", MessageType.error)
        End If
    End Sub

#Region "Uploader"

    Private Sub LNBImportGlossaries_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LNBImportGlossaries.Click
        ShowMessage(String.Empty, MessageType.none)
        LBselectedFileName.Text = FUPimportFile.FileName
        PNLUploadInfo.Visible = False
        LNBImportGlossaryFromFile.Visible = False
        Dim filenamePath As String = CurrentPresenter.GetFullUserPath(FUPimportFile.FileName)
        Create.UploadFile(FUPimportFile.PostedFile, filenamePath)
        If (File.Exists(filenamePath)) Then
            CTRCommunityGlossaryTerms.LoadViewDataFromFile(filenamePath, True)
            PNLUploadInfo.Visible = True
            LNBImportGlossaryFromFile.Visible = True
        Else
            ShowMessage("ImportExport.ErrorImport", MessageType.error)
        End If
    End Sub


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


#Region "Community Selector"

    Protected Overrides Function OnBubbleEvent(ByVal source As Object, ByVal args As EventArgs) As Boolean
        If args.GetType() = GetType(CustomArgs) Then
            Dim arg As CustomArgs = args
            CurrentKey = arg.Key
            CurrentPresenter.AddAllCommunityClick(PageUtility.CurrentUser.ID)
        End If
    End Function

    'Private Sub CTRLglossaryShare_AddCommunityClicked() Handles LNBselectCommunities.Click
    '    CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    'End Sub

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
        'CurrentPresenter.AddNewCommunity(idCommunities)
        If idCommunities.Count > 0 Then
            For Each itemGlossary As RepeaterItem In CTRCommunityGlossaryTerms.GetRPTCommunites.Items
                Dim ucGlossaryImportTerms As UC_GlossaryImportTerms = itemGlossary.FindControl("UCglossaryTerms")
                If Not IsNothing(ucGlossaryImportTerms) Then
                    ucGlossaryImportTerms.SetMappedCommunity(CurrentKey, idCommunities(0), CurrentPresenter.GetCommunityName(idCommunities(0)))
                End If
            Next
        End If
    End Sub

#End Region
End Class