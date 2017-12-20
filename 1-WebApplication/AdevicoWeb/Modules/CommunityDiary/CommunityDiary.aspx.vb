Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
Imports lm.Comol.Core.BaseModules.Domain

Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation

Partial Public Class CommunityDiary
    Inherits PageBase
    Implements IViewCommunityDiary

#Region "Generics"
    Private _Presenter As CommunityDiaryPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CommunityDiaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityDiaryPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Protected ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property DisplayOrderAscending() As Boolean Implements IViewCommunityDiary.DisplayOrderAscending
        Get
            Return (Me.RBLorderby.SelectedIndex = 0)
        End Get
        Set(ByVal value As Boolean)
            Me.RBLorderby.SelectedIndex = IIf(value, 0, 1)
        End Set
    End Property
    Private ReadOnly Property PreloadIdCommunity() As Integer Implements IViewCommunityDiary.PreloadIdCommunity
        Get
            Dim idCommunity As Integer = 0
            Integer.TryParse(Request.QueryString("CommunityID"), idCommunity)
            If idCommunity = 0 Then
                Integer.TryParse(Request.QueryString("idCommunity"), idCommunity)
            End If
            Return idCommunity
        End Get
    End Property
    Private Property AllowItemsSelection() As Boolean Implements IViewCommunityDiary.AllowItemsSelection
        Get
            Return ViewStateOrDefault("AllowItemsSelection", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowItemsSelection") = value
        End Set
    End Property
    Private Property AllowMultipleDelete() As Boolean Implements IViewCommunityDiary.AllowMultipleDelete
        Get
            Return ViewStateOrDefault("AllowMultipleDelete", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowMultipleDelete") = value
            Me.LNBdeleteSelectedItems.Visible = value
        End Set
    End Property
    Private Property AllowPrint() As Boolean Implements IViewCommunityDiary.AllowPrint
        Get
            Return ViewStateOrDefault("AllowPrint", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowPrint") = value
            Me.HYPprintItems.Visible = value
        End Set
    End Property
    Private Property AllowAddItem() As Boolean Implements IViewCommunityDiary.AllowAddItem
        Get
            Return ViewStateOrDefault("AllowAddItem", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowAddItem") = value
            Me.HYPaddItem.Visible = value
        End Set
    End Property
    Private Property AllowImport As Boolean Implements lm.Comol.Core.BaseModules.CommunityDiary.Presentation.IViewCommunityDiary.AllowImport
        Get
            Return ViewStateOrDefault("AllowImport", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowImport") = value
            Me.HYPimport.Visible = value
        End Set
    End Property
    Private Property AllowDeleteDiary As Boolean Implements IViewCommunityDiary.AllowDeleteDiary
        Get
            Return ViewStateOrDefault("AllowDeleteDiary", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowDeleteDiary") = value
            Me.LNBdeleteItems.Visible = value

            LNBdeleteItems.CommandArgument = Me.IdCommunityDiary
            LNBdeleteItems.CommandName = "confirmDeleteDiary"
            LNBdeleteItems.DialogClass = "deleteDiary"
        End Set
    End Property
    Private Property SelectedItems() As List(Of Long) Implements IViewCommunityDiary.SelectedItems
        Get
            Dim oList As New List(Of Long)
            For Each oRow As RepeaterItem In Me.RPTitemsDetails.Items
                Dim oCheck As HtmlInputCheckBox
                oCheck = oRow.FindControl("CBXselected")
                If Not IsNothing(oCheck) AndAlso oCheck.Visible AndAlso oCheck.Checked Then
                    oList.Add(oCheck.Value)
                End If
            Next
            Return oList
        End Get
        Set(ByVal value As List(Of Long))
            Dim TotalItems As Integer = value.Count
            For Each oRow As RepeaterItem In Me.RPTitemsDetails.Items
                Dim oCheck As HtmlInputCheckBox
                oCheck = oRow.FindControl("CBXselected")
                If Not IsNothing(oCheck) Then
                    If Not oCheck.Visible OrElse TotalItems = 0 OrElse Not value.Contains(oCheck.Value) Then
                        oCheck.Checked = False
                    Else
                        oCheck.Checked = True
                    End If
                End If
            Next
        End Set
    End Property
    Private Property IdModuleCommunityDiary() As Integer Implements IViewCommunityDiary.IdModuleCommunityDiary
        Get
            Return ViewStateOrDefault("IdModuleCommunityDiary", -1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdModuleCommunityDiary") = value
        End Set
    End Property
    Private Property IdModuleRepository() As Integer Implements IViewCommunityDiary.IdModuleRepository
        Get
            Return ViewStateOrDefault("IdModuleRepository", -1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdModuleRepository") = value
        End Set
    End Property

    Private Property IdCommunityDiary() As Integer Implements IViewCommunityDiary.IdCommunityDiary
        Get
            Return ViewStateOrDefault("IdCommunityDiary", -1)
        End Get
        Set(ByVal value As Integer)
            ViewState("IdCommunityDiary") = value
        End Set
    End Property
    Public ReadOnly Property BaseFolder As String Implements IViewCommunityDiary.BaseFolder
        Get
            Return Resource.getValue("BaseFolder")
        End Get
    End Property

    Private _BaseUrlNoSSL As String
    Private Overloads ReadOnly Property BaseUrlNoSSL() As String
        Get
            If _BaseUrlNoSSL = "" Then
                _BaseUrlNoSSL = Me.ApplicationUrlBase()
                If Not _BaseUrlNoSSL.EndsWith("/") Then
                    _BaseUrlNoSSL &= "/"
                End If
            End If
            Return _BaseUrlNoSSL
        End Get
    End Property
    Private ReadOnly Property PreloadAscending() As Boolean Implements IViewCommunityDiary.PreloadAscending
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("Ascending")) Then
                Return True
            Else
                Try
                    Return CBool(Me.Request.QueryString("Ascending"))
                Catch ex As Exception
                    Return True
                End Try
            End If
        End Get
    End Property
    Private ReadOnly Property UnknownUserTranslation As String Implements IViewCommunityDiary.UnknownUserTranslation
        Get
            Return Resource.getValue("UnknownUserTranslation")
        End Get
    End Property
    Private Property RepositoryIdentifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IViewCommunityDiary.RepositoryIdentifier
        Get
            Dim result As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier = Nothing
            If Not IsNothing(ViewState("RepositoryIdentifier")) Then
                Try
                    result = DirectCast(ViewState("RepositoryIdentifier"), lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
                Catch ex As Exception

                End Try
            End If
            Return result
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "Internal"
    Protected Function BackGroundItem(ByVal oItem As dtoDiaryItem, ByVal Type As ListItemType) As String
        If Not oItem.EventItem.IsVisible Then
            Return "ROW_Disabilitate_Small_Editor"
        ElseIf Type = ListItemType.AlternatingItem Then
            Return "ROW_Alternate_Small_Editor"
        Else
            Return "ROW_Normal_Small_Editor"
        End If
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView(UnknownUserTranslation)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityDiary", "Modules", "CommunityDiary")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBorderby_t)
            .setLabel(Me.LBtitle)
            .setLabel(LBnoLesson)
            .setLinkButton(Me.LNBdeleteItems, True, True)
            .setHyperLink(Me.HYPaddItem, True, True)
            .setHyperLink(Me.HYPprintItems, True, True)
            .setHyperLink(HYPimport, True, True)
            .setRadioButtonList(Me.RBLorderby, 0)
            .setRadioButtonList(Me.RBLorderby, 1)
            .setLinkButton(LNBdeleteSelectedItems, False, True)

            Me.DLGclearItems.DialogTitle = Me.Resource.getValue("DLGclearItemsTitle")
            Me.DLGclearItems.DialogText = Me.Resource.getValue("DLGclearItemsText")
            Me.DLGclearItems.InitializeControl(New List(Of String), -1)

            Me.DLGremoveItems.DialogTitle = Me.Resource.getValue("DLGremoveItemsTitle")
            Me.DLGremoveItems.DialogText = Me.Resource.getValue("DLGremoveItemsText")
            Me.DLGremoveItems.InitializeControl(New List(Of String), -1)

            Me.DLGremoveItem.DialogTitle = Me.Resource.getValue("DLGremoveItemTitle")
            Me.DLGremoveItem.DialogText = Me.Resource.getValue("DLGremoveItemText")
            Me.DLGremoveItem.InitializeControl(New List(Of String), -1)

            Me.DLGmoduleFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGmoduleFileItemVisibilityTitle")
            Me.DLGmoduleFileItemVisibility.DialogText = Me.Resource.getValue("DLGmoduleFileItemVisibilityText")


            Dim options As New List(Of String)
            options.Add(.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
            Me.DLGmoduleFileItemVisibility.InitializeControl(options, 0, True)
            options = New List(Of String)
            options.Add(.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
            options.Add(.getValue("ModuleItemFileVisibilityStatus.Visible"))
            Me.DLGrepositoryFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGrepositoryFileItemVisibilityTitle")
            Me.DLGrepositoryFileItemVisibility.DialogText = Me.Resource.getValue("DLGrepositoryFileItemVisibilityText")
            Me.DLGrepositoryFileItemVisibility.InitializeControl(options, 0, True)


            LNBdeleteSelectedItems.DialogClass = "removeItems"
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Messages"
    Private Sub ShowUnkownCommunityDiary(ByVal idCommunity As Integer, ByVal idModule As Integer) Implements IViewCommunityDiary.ShowUnkownCommunityDiary
        MLVitems.SetActiveView(Me.VIWerrors)
        CTRLerrorMessages.InitializeControl(Resource.getValue("NoCommunityDiaryWithThisID"), Helpers.MessageType.error)
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleCommunityDiary.ActionType.UnkownDiary, Me.PageUtility.CreateObjectsList(IdModuleCommunityDiary, ModuleCommunityDiary.ObjectType.Diary, idCommunity), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub HideItemsForNoPermission(ByVal idCommunity As Integer, ByVal idModule As Integer) Implements IViewCommunityDiary.HideItemsForNoPermission
        MLVitems.SetActiveView(Me.VIWerrors)
        CTRLerrorMessages.InitializeControl(Resource.getValue("nopermission"), Helpers.MessageType.error)
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleCommunityDiary.ActionType.NoPermission, Me.PageUtility.CreateObjectsList(idModule, ModuleCommunityDiary.ObjectType.Diary, idCommunity), InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub DisplaySessionTimeout(idCommunity As Integer, Optional ByVal idItem As Long = 0) Implements IViewCommunityDiary.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.CommunityDiary(idCommunity, idItem)
        webPost.Redirect(dto)
    End Sub
    Private Function GetPortalNameTranslation() As String Implements IViewCommunityDiary.GetPortalNameTranslation
        Return Resource.getValue("PortalHome")
    End Function
    Private Sub SetTitleName(ByVal CommunityName As String) Implements IViewCommunityDiary.SetTitleName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleCommunity"), CommunityName, CommunityName)
    End Sub
    Private Sub LoadItems(ByVal oList As List(Of dtoDiaryItem), ByVal IdCommunity As Integer, ByVal IdModule As Integer) Implements IViewCommunityDiary.LoadItems
        If oList.Count = 0 Then
            Me.MLVitems.SetActiveView(Me.VIWnoLesson)
        Else
            MLVitems.SetActiveView(Me.VIWitems)
            _DisplayManagerTable = oList.Any(Function(i) i.Permission.AllowEdit OrElse i.Permission.AllowAddFiles)
            RPTitemsDetails.DataSource = oList
            RPTitemsDetails.DataBind()
         
        End If
        If HYPaddItem.Visible Then
            MyBase.SetFocus(Me.HYPaddItem)
        ElseIf Me.HYPprintItems.Visible Then
            MyBase.SetFocus(Me.HYPprintItems)
        End If

    End Sub
    Private Sub SetAddItemUrl(ByVal idCommunity As Integer) Implements IViewCommunityDiary.SetAddItemUrl
        HYPaddItem.Visible = Not (idCommunity = -1)
        HYPaddItem.NavigateUrl = Me.BaseUrl & "Modules/CommunityDiary/DiaryItem.aspx?idCommunity=" & idCommunity.ToString & "&InsertMode=True&idItem=0"
    End Sub
    Private Sub InitializeAttachmentsControl(actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Implements IViewCommunityDiary.InitializeAttachmentsControl
        If Not IsNothing(actions) Then
            CTRLattachmentsHeader.Visible = True
            CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))
        Else
            CTRLattachmentsHeader.Visible = False
        End If
    End Sub

#End Region

#Region "Internal"

#Region "Url"
    Private Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.liteRepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                url = item.Url
                If Not String.IsNullOrWhiteSpace(url) Then
                    If Not (url.ToLower().StartsWith("http://") OrElse url.ToLower().StartsWith("https://")) Then
                        url = "http://" & url
                    End If
                End If
            Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia, lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.PlayForRepository(item, setBackUrl, backUrl)
            Case Else
                url = lm.Comol.Core.FileRepository.Domain.RootObject.Download(PageUtility.ApplicationUrlBase(), item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
        End Select


        Return url
    End Function
    Private Function CleanBackUrl(backUrl As String) As String
        backUrl = Replace(backUrl, ApplicationUrlBase(True), "")
        backUrl = Replace(backUrl, ApplicationUrlBase(False), "")
        Return backUrl
    End Function
    Private Function SanitizeFolderUrl(folderUrl As String) As String
        If Not folderUrl.StartsWith(ApplicationUrlBase(True)) AndAlso Not folderUrl.StartsWith(ApplicationUrlBase(False)) Then
            folderUrl = ApplicationUrlBase & folderUrl
        End If
        Return folderUrl
    End Function
#End Region

#Region "Management"
    Private Sub RBLorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLorderby.SelectedIndexChanged
        Me.CurrentPresenter.ChangeOrderBy(IdCommunityDiary, UnknownUserTranslation)
    End Sub


#Region "Lesson"
    Private Sub RPTitemsDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitemsDetails.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoItem As dtoDiaryItem = TryCast(e.Item.DataItem, dtoDiaryItem)
            If Not IsNothing(oDtoItem) Then
                Dim oDiv As HtmlControls.HtmlControl
                Dim oItem As CommunityEventItem = oDtoItem.EventItem
                Dim oPermission As CoreItemPermission = oDtoItem.Permission

                Dim oLabel As Label
                Dim oLiteral As Literal
                oLabel = e.Item.FindControl("LBitemHeader")
                If Not IsNothing(oLabel) Then
                    If oItem.ShowDateInfo Then
                        If oItem.StartDate.Day = oItem.EndDate.Day AndAlso oItem.StartDate.Month = oItem.EndDate.Month AndAlso oItem.StartDate.Year = oItem.EndDate.Year Then
                            oLabel.Text = oItem.StartDate.Date.ToString("ddd dd MMMM yyyy", Resource.CultureInfo.DateTimeFormat)
                            oLabel.Text &= " " & FormatDateTime(oItem.StartDate, DateFormat.ShortTime)
                            oLabel.Text &= " - " & FormatDateTime(oItem.EndDate, DateFormat.ShortTime)
                        Else
                            oLabel.Text = oItem.StartDate.Date.ToString("dddd dd MMMM", Resource.CultureInfo.DateTimeFormat)
                            oLabel.Text &= " " & oItem.EndDate.Date.ToString("dddd dd MMMM", Resource.CultureInfo.DateTimeFormat)
                        End If
                        If oItem.MinutesDuration > 0 Then
                            oLabel.ToolTip = Resource.getValue("duration") & " " & Me.GetDurationString(oItem.MinutesDuration)
                        End If
                    ElseIf oItem.MinutesDuration > 0 Then
                        oLabel.Text = Resource.getValue("duration") & " " & Me.GetDurationString(oItem.MinutesDuration)
                        oLabel.ToolTip = oLabel.Text
                    End If
                End If


                oLabel = e.Item.FindControl("LBlez")
                If Not IsNothing(oLabel) Then
                    If oItem.ShowDateInfo Then
                        Me.Resource.setLabel(oLabel)
                        If oLabel.Text <> "" Then
                            oLabel.Text = String.Format(oLabel.Text, oDtoItem.LessonNumber)
                        End If
                    Else
                        oLabel.Text = ""
                    End If
                End If
                oLabel = e.Item.FindControl("LBtitle_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBplace_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBdescription_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBnote_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBmateriale_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oDiv = e.Item.FindControl("DIVtitle")
                If Not IsNothing(oDiv) Then
                    If oItem.Title = "" Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                    End If
                End If
                oDiv = e.Item.FindControl("DIVduration")
                If Not IsNothing(oDiv) Then
                    If oItem.MinutesDuration = 0 Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                        oLabel = e.Item.FindControl("LBduration_t")
                        Resource.setLabel(oLabel)
                        oLabel = e.Item.FindControl("LBduration")
                        oLabel.Text = Me.GetDurationString(oItem.MinutesDuration)
                    End If
                End If
                oDiv = e.Item.FindControl("DIVplace")
                If Not IsNothing(oDiv) Then
                    If oItem.Place = "" Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                    End If
                End If



                'LTnote" runat="server"></asp:Literal>
                oDiv = e.Item.FindControl("DIVtext")
                If Not IsNothing(oDiv) AndAlso Not String.IsNullOrEmpty(oDtoItem.Description) Then
                    If oDtoItem.Description.Trim = "" OrElse oDtoItem.Description.Trim = vbCrLf OrElse oDtoItem.Description.Trim = "<br>" OrElse oDtoItem.Description.Trim = "</br>" Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                        oLiteral = e.Item.FindControl("LTdescription")
                        oLiteral.Text = SmartTagsAvailable.TagAll(oDtoItem.Description)
                    End If
                Else
                    oDiv.Style("Display") = "none"
                End If
                oDiv = e.Item.FindControl("DIVnote")
                If Not IsNothing(oDiv) AndAlso Not String.IsNullOrEmpty(oItem.Note) Then 'AndAlso Not IsNothing(oLiteral) AndAlso Not IsNothing(oLabel)
                    If oItem.Note.Trim = "" OrElse oItem.Note.Trim = vbCrLf OrElse oItem.Note.Trim = "<br>" OrElse oItem.Note.Trim = "</br>" Then
                        oDiv.Style("Display") = "none"
                    Else
                        Me.Resource.setLabel(oLabel) 'almeno apparentemente, quando si arriva qui e' gia' stata impostata /sempre/, da verificare!
                        oDiv.Style("Display") = "block"
                        oLiteral = e.Item.FindControl("LTnote")
                        oLiteral.Text = SmartTagsAvailable.TagAll(oItem.Note)
                    End If
                Else
                    oDiv.Style("Display") = "none"
                End If
                oDiv = e.Item.FindControl("DIVlink")
                If Not IsNothing(oDiv) Then
                    If oItem.Link = "" Then
                        oDiv.Style("Display") = "none"
                    Else
                        Dim oQuote As String = """"
                        oDiv.Style("Display") = "block"
                        oLabel = e.Item.FindControl("LBlinkDisplay")
                        If oItem.Link.Contains(";") OrElse oItem.Link.Contains(",") Then
                            For Each link As String In IIf(oItem.Link.Contains(";"), oItem.Link.Split(";"), oItem.Link.Split(","))
                                If link <> "" Then
                                    link = link.Trim()
                                    If Not link.StartsWith("http://") AndAlso Not link.StartsWith("https://") Then
                                        link = "http://" & link
                                    End If
                                    oLabel.Text &= "<a class=" & oQuote & "ROW_ItemLink_Small" & oQuote & " href=" & oQuote & link & oQuote & " target=" & oQuote & "_blank" & oQuote & " >" & link & "</a>&nbsp;"
                                End If
                            Next
                        Else
                            Dim link As String = oItem.Link
                            If Not link.StartsWith("http://") Then
                                link = "http://" & link
                            End If
                            oLabel.Text = "<a class=" & oQuote & "ROW_ItemLink_Small" & oQuote & " href=" & oQuote & link & oQuote & " target=" & oQuote & "_blank" & oQuote & " >" & oItem.Link & "</a>"
                        End If
                    End If
                End If


                oDiv = e.Item.FindControl("DIVmateriale")
                'NASCOSTO TEMPORANEO
                If Not IsNothing(oDiv) Then
                    oLabel = e.Item.FindControl("LBmateriale_t")
                    If IsNothing(oDtoItem.Attachments) OrElse oDtoItem.Attachments.Count = 0 Then
                        oDiv.Visible = False
                    Else
                        _DisplayVisibilityColumn = oDtoItem.Attachments.Any(Function(i) i.Permissions.Edit OrElse i.Permissions.EditVisibility OrElse i.Permissions.EditRepositoryVisibility)
                        _DisplayActionsColumn = oDtoItem.Attachments.Any(Function(i) i.Permissions.Edit OrElse i.Permissions.Download)

                        Resource.setLabel(oLabel)
                        oDiv.Visible = True
                        Dim oRepeater As System.Web.UI.WebControls.Repeater = e.Item.FindControl("RPTitemFiles")
                        If Not IsNothing(oRepeater) Then
                            AddHandler oRepeater.ItemDataBound, AddressOf RPTitemFiles_ItemDataBound
                            '            ''   AddHandler oRepeater.ItemCommand, AddressOf RPTitemFiles_ItemCommand
                            oRepeater.DataSource = oDtoItem.Attachments
                            oRepeater.DataBind()

                        End If
                    End If
                End If

                Dim isItemEditable As Boolean = oPermission.AllowEdit
                Dim isItemDeletable As Boolean = oPermission.AllowDelete

                Dim oCommands As UC_ModuleAttachmentInlineCommands = e.Item.FindControl("CTRLcommands")
                If Not IsNothing(oCommands) Then
                    oCommands.Visible = isItemDeletable AndAlso oDtoItem.UploadActions.Any
                    If oDtoItem.UploadActions.Any Then
                        oCommands.InitializeControlForPostback(oDtoItem.UploadActions, oDtoItem.DefaultUploadAction)
                    End If
                End If

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPedit")
                If Not IsNothing(oHyperlink) Then
                    oHyperlink.Visible = isItemEditable
                    oHyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditDiaryItem(oDtoItem.CommunityId, oItem.Id)
                    Me.Resource.setHyperLink(oHyperlink, True, True)
                    oHyperlink.Text = "&nbsp;"
                End If

                Dim oImageButton As ImageButton
                Dim oDialogDelete As MyUC.DialogLinkButton = e.Item.FindControl("LNBdeleteItem")
                Me.Resource.setLinkButton(oDialogDelete, False, True)
                oDialogDelete.Text = "&nbsp;" 'String.Format(oDialogDelete.Text, Me.BaseUrl & "images/x.gif", oDialogDelete.ToolTip)
                oDialogDelete.Visible = isItemDeletable 'AndAlso oItem.isDeleted
                oDialogDelete.CommandArgument = oDtoItem.Id
                oDialogDelete.DialogClass = "removeItem"

                Dim oCheckBox As CheckBox
                oCheckBox = e.Item.FindControl("CBXitem")
                'If Not IsNothing(oCheck) Then
                oCheckBox.Visible = isItemDeletable AndAlso Me.AllowItemsSelection
                'End If
            End If
        End If
    End Sub
    Private Sub RPTitemsDetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTitemsDetails.ItemCommand
        Dim idEventItem As Long = 0
        Dim idEvent As Long = 0
        Dim lessonNumber As Integer = 0
        Long.TryParse(DirectCast(e.Item.FindControl("LTitemID"), Literal).Text, idEventItem)
        Long.TryParse(DirectCast(e.Item.FindControl("LTidEvent"), Literal).Text, idEvent)
        Integer.TryParse(DirectCast(e.Item.FindControl("LTlessonNumber"), Literal).Text, lessonNumber)
        If idEventItem > 0 AndAlso e.CommandSource.GetType() Is GetType(LinkButton) Then
            Dim uploadItem As String = Replace(DirectCast(e.CommandSource, LinkButton).ID, "LNB", "")
            Dim action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions).GetByString(uploadItem, Repository.RepositoryAttachmentUploadActions.none)
            Select Case action
                Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity
                    CTRLlinkFromCommunity.Visible = True
                    CTRLlinkFromCommunity.InitializeControl(idEvent, idEventItem, lessonNumber, action, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & action.ToString))

                    Master.SetOpenDialogOnPostbackByCssClass("dlg" & action.ToString)
                Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                    CTRLinternalUpload.Visible = True
                    CTRLinternalUpload.InitializeControl(idEvent, idEventItem, lessonNumber, action, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & action.ToString))

                    Master.SetOpenDialogOnPostbackByCssClass("dlg" & action.ToString)
                Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                    CTRLcommunityUpload.Visible = True
                    CTRLcommunityUpload.InitializeControl(idEvent, idEventItem, lessonNumber, action, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & action.ToString))

                    Master.SetOpenDialogOnPostbackByCssClass("dlg" & action.ToString)
                Case Else
                    CTRLinternalUpload.Visible = False
                    CTRLcommunityUpload.Visible = False
                    CTRLlinkFromCommunity.Visible = False
                    Master.ClearOpenedDialogOnPostback()
            End Select
        End If
    End Sub

#Region "Attachments"
    Protected Sub RPTitemFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim attachmentItem As lm.Comol.Core.Events.Domain.dtoAttachmentItem = TryCast(e.Item.DataItem, lm.Comol.Core.Events.Domain.dtoAttachmentItem)

            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = attachmentItem.Attachment.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            Dim actions As List(Of dtoModuleActionControl)
            'initializer.OnModalPage
            '  initializer.OpenLinkCssClass

            Dim requiredActions As lm.Comol.Core.ModuleLinks.DisplayActionMode = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            If attachmentItem.Permissions.Edit Then
                requiredActions = requiredActions Or lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode
            Else
                requiredActions = requiredActions Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            End If
            actions = renderItem.InitializeRemoteControl(initializer, StandardActionType.Play, requiredActions)

            Dim isReadyToPlay As Boolean = (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.available)
            Dim isReadyToManage As Boolean = isReadyToPlay OrElse (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.waitingsettings)
            Dim oHyperlink As HyperLink

            '<asp:HyperLink ID="HYPdownload" runat="server" Text="D" CssClass="icon download" Visible="false" />
            '                                                <asp:HyperLink ID="HYPstats" runat="server" Text="S" CssClass="icon stats" Visible="false" />
            '                                                <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="M" CssClass="icon editmetadata" Visible="false" />
            If attachmentItem.Permissions.Download AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.DownloadItem).Any Then
                oHyperlink = e.Item.FindControl("HYPdownload")
                oHyperlink.Visible = True
                oHyperlink.ToolTip = Resource.getValue("Download.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.DownloadItem).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If

            If isReadyToPlay AndAlso attachmentItem.Permissions.ViewOtherStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Any Then
                oHyperlink = e.Item.FindControl("HYPstats")
                oHyperlink.Visible = True
                oHyperlink.ToolTip = Resource.getValue("statistic.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
            ElseIf isReadyToPlay AndAlso attachmentItem.Permissions.ViewMyStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Any Then
                oHyperlink = e.Item.FindControl("HYPstats")
                oHyperlink.ToolTip = Resource.getValue("statistic.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If
            If isReadyToManage AndAlso attachmentItem.Permissions.SetMetadata AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Any Then
                oHyperlink = e.Item.FindControl("HYPeditMetadata")
                oHyperlink.ToolTip = Resource.getValue("settings.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If
            Dim oLiteral As Literal = Nothing
            Dim oCell As HtmlControls.HtmlTableCell = e.Item.FindControl("TDvisibilityLesson")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("TDvisibilityRepository")
            oCell.Visible = _DisplayVisibilityColumn
            If _DisplayVisibilityColumn Then
                oLiteral = e.Item.FindControl("LTvisibleForItem")
                If attachmentItem.Attachment.IsVisible Then
                    oLiteral.Text = Replace(LTtemplateVisibile.Text, "#visibility#", Resource.getValue("Attachment.Visible"))
                Else
                    oLiteral.Text = Replace(LTtemplateNotVisibile.Text, "#visibility#", Resource.getValue("Attachment.NotVisible"))
                End If
                oLiteral = e.Item.FindControl("LTvisibleForRepository")
                If IsNothing(attachmentItem.Attachment.File) OrElse attachmentItem.Attachment.File.IsInternal Then
                    oLiteral.Text = Replace(LTtemplateIgnore.Text, "#visibility#", "//")
                ElseIf attachmentItem.Attachment.File.IsVisible Then
                    oLiteral.Text = Replace(LTtemplateVisibile.Text, "#visibility#", Resource.getValue("Attachment.Visible"))
                Else
                    oLiteral.Text = Replace(LTtemplateNotVisibile.Text, "#visibility#", Resource.getValue("Attachment.NotVisible"))
                End If
            End If

            oCell = e.Item.FindControl("TDactions")
            oCell.Visible = _DisplayActionsColumn


            If Not IsNothing(attachmentItem.Attachment.File) AndAlso (attachmentItem.Permissions.EditRepositoryVisibility OrElse attachmentItem.Permissions.EditVisibility) Then
                Dim oDialogHide As MyUC.DialogLinkButton = e.Item.FindControl("LNBhide")
                Me.Resource.setLinkButton(oDialogHide, True, True)
                Dim fileStatus As ModuleItemFileVisibilityStatus = GetVisibilityStatus(attachmentItem.Attachment.File, attachmentItem.Attachment.IsVisible)

                oDialogHide.CommandArgument = attachmentItem.Attachment.Id.ToString & "," & attachmentItem.Attachment.IdEventItem.ToString
                Dim selectedVisibility As New List(Of Integer)
                If attachmentItem.Attachment.File.IsInternal OrElse Not attachmentItem.Permissions.EditRepositoryVisibility Then
                    If attachmentItem.Attachment.IsVisible Then
                        selectedVisibility.Add(0)
                    End If
                    oDialogHide.Visible = attachmentItem.Permissions.EditVisibility
                    oDialogHide.CommandName = "editModuleVisibility"
                    oDialogHide.InitializeMultiSelectControlByClass("moduleItemVisibility", selectedVisibility)
                Else
                    If attachmentItem.Attachment.IsVisible Then
                        selectedVisibility.Add(0)
                    End If
                    If attachmentItem.Attachment.File.IsVisible Then
                        selectedVisibility.Add(1)
                    End If
                    oDialogHide.Visible = attachmentItem.Permissions.EditRepositoryVisibility
                    oDialogHide.CommandName = "editCommunityVisibility"
                    oDialogHide.InitializeMultiSelectControlByClass("repositoryItemVisibility", selectedVisibility)
                End If
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTvisibilityLessonHeader")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTvisibilityRepositoryHeader")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTattachmentactionsHeader")
            Resource.setLiteral(oLiteral)

            Dim oCell As HtmlControls.HtmlTableCell = e.Item.FindControl("THvisibilityLesson")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("THvisibilityRepository")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("THactions")
            oCell.Visible = _DisplayActionsColumn
        End If
    End Sub
    Private Function GetVisibilityStatus(ByVal item As lm.Comol.Core.FileRepository.Domain.RepositoryItemObject, ByVal isVisible As Boolean) As ModuleItemFileVisibilityStatus
        Dim iResponse As ModuleItemFileVisibilityStatus
        If item.IsInternal Then
            iResponse = IIf(isVisible, ModuleItemFileVisibilityStatus.VisibleForModule, ModuleItemFileVisibilityStatus.HiddenForModule)
        Else
            If isVisible AndAlso item.IsVisible Then
                iResponse = (ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity)
            ElseIf Not isVisible AndAlso Not item.IsVisible Then
                iResponse = (ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity)
            ElseIf isVisible AndAlso Not item.IsVisible Then
                iResponse = ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity
            ElseIf Not isVisible Then
                iResponse = ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity
            End If
        End If
        Return iResponse
    End Function
    Private Sub DLGrepositoryFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As System.Collections.Generic.IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGrepositoryFileItemVisibility.ButtonPressedMulti
        Dim idEventItem As Long = 0
        Dim idItemFile As Long = 0
        Long.TryParse(CommandArgument.Split(",")(1), idEventItem)
        Long.TryParse(CommandArgument.Split(",")(0), idItemFile)

        Dim iResponse As ModuleItemFileVisibilityStatus
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
        Else
            CurrentPresenter.EditFileItemVisibility(idEventItem, idItemFile, IdCommunityDiary, dialogResults.Contains(0), dialogResults.Contains(1), UnknownUserTranslation)
        End If
    End Sub
    Private Sub DLGmoduleFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGmoduleFileItemVisibility.ButtonPressedMulti
        Dim idEventItem As Long = 0
        Dim idItemFile As Long = 0
        Long.TryParse(CommandArgument.Split(",")(1), idEventItem)
        Long.TryParse(CommandArgument.Split(",")(0), idItemFile)
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
        Else
            CurrentPresenter.EditFileItemVisibility(idEventItem, idItemFile, IdCommunityDiary, dialogResults.Contains(0), False, UnknownUserTranslation)
        End If
    End Sub
    Public Function GetAttachmentsCssClass() As String
        If _DisplayManagerTable Then
            Return " asmanager"
        Else
            Return " asuser"
        End If
    End Function
#End Region
    Private Function GetSelectedItems() As List(Of Long)
        Dim IdItems As New List(Of Long)
        For Each Row As RepeaterItem In (From r As RepeaterItem In RPTitemsDetails.Items Where TypeOf r.FindControl("CBXitem") Is CheckBox AndAlso r.FindControl("CBXitem").Visible Select r).ToList
            Dim oLiteral As Literal = Row.FindControl("LTitemID")
            Dim oCheck As CheckBox = Row.FindControl("CBXitem")
            If oCheck.Checked Then
                IdItems.Add(CLng(oLiteral.Text))
            End If
        Next
        Return IdItems
    End Function
    Private _DisplayManagerTable As Boolean
    Private _DisplayVisibilityColumn As Boolean
    Private _DisplayActionsColumn As Boolean
#End Region

#Region "Add Controls"
    Private Sub CTRLaddUrls_ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.ItemsAdded, CTRLinternalUpload.ItemsAdded, CTRLlinkFromCommunity.ItemsAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsAdded.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.success)
        CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLaddUrls_ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.ItemsNotAdded, CTRLinternalUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsNotAdded.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.error)
        CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLadd_NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.NoFilesToAdd, CTRLinternalUpload.NoFilesToAdd, CTRLlinkFromCommunity.NoFilesToAdd
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("NoFilesToAdd.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.alert)
        CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLadd_WorkingSessionExpired() Handles CTRLcommunityUpload.WorkingSessionExpired, CTRLinternalUpload.WorkingSessionExpired, CTRLlinkFromCommunity.WorkingSessionExpired
        Master.ClearOpenedDialogOnPostback()
        Me.DisplaySessionTimeout(IdCommunityDiary)
    End Sub
    Private Sub CTRLadd_EventItemNotFound() Handles CTRLcommunityUpload.EventItemNotFound, CTRLinternalUpload.EventItemNotFound, CTRLlinkFromCommunity.EventItemNotFound
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayEventItemNotFound.Attachments"), Helpers.MessageType.error)
        CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
    End Sub
#End Region

#Region "User Dialog"
    Private Sub DLGclearItems_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGclearItems.ButtonPressed
        If CommandName = "confirmDeleteDiary" Then
            CurrentPresenter.DeleteCommunityDiary(IdCommunityDiary, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath, UnknownUserTranslation)
        End If
    End Sub
    Private Sub DLGremoveItems_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGremoveItems.ButtonPressed
        If CommandName = "confirmDeleteSelectItems" Then
            CurrentPresenter.DeleteSelectedItems(GetSelectedItems(), IdCommunityDiary, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath, UnknownUserTranslation)
        End If
    End Sub
    Private Sub DLGremoveItem_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGremoveItem.ButtonPressed
        If CommandName = "confirmdeleteitem" Then
            CurrentPresenter.DeleteItem(CLng(CommandArgument), IdCommunityDiary, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath, UnknownUserTranslation)
        End If
    End Sub
#End Region


#End Region
#End Region


#Region "Management items"
    Protected Sub RPTitemFiles_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        If e.CommandName = "editvisibility" Then
            'Me.CurrentPresenter.UpdateFileItemVisibility(CLng(e.CommandArgument))
        End If
    End Sub

    Private Function GetDurationString(ByVal minutesDuration As Integer) As String
        Dim durationDate As New TimeSpan(0, minutesDuration, 0)
        Dim TotalHours As Integer
        If durationDate.TotalHours > Integer.MaxValue Then
            TotalHours = Integer.MaxValue
        Else
            TotalHours = Convert.ToInt32(durationDate.TotalHours)
        End If
        Dim Hours As Integer = durationDate.Hours
        Dim Minutes As Integer = durationDate.Minutes

        If TotalHours = 0 AndAlso Minutes > 0 Then
            Return durationDate.Minutes.ToString & " " & Resource.getValue("durationMinutes")
        ElseIf TotalHours = Hours Then
            Return durationDate.Hours.ToString & IIf(Hours = 1, Resource.getValue("durationHour"), Resource.getValue("durationHours")) & IIf(Minutes = 0, ".", " " & durationDate.Minutes.ToString & Resource.getValue("durationMinutes"))
        ElseIf Not TotalHours.Equals(0) Then
            Return durationDate.TotalHours.ToString & IIf(TotalHours = 1, Resource.getValue("durationHour"), Resource.getValue("durationHours")) & IIf(Minutes = 0, ".", "  " & durationDate.Minutes.ToString & Resource.getValue("durationMinutes"))
        End If
        Return ""
    End Function
#End Region





    Private Sub Page_PreInit1(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub


   
End Class