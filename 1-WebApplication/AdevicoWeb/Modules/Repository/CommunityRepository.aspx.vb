Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Core.BaseModules.Repository.Domain

Partial Public Class CommunityRepository
    Inherits PageBase
    Implements IViewExploreCommunityRepository


#Region "View"
    ' Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _Presenter As CRexplorePresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _BaseUrlNoSSL As String

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IViewExploreCommunityRepository.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault

            'Dim list = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
            '    Where sb.CommunityID = CommunityID _
            '    Select New With {.Antico = New ModuleCommunityRepository(New Services_File(sb.PermissionString)), .Nuovo = New ModuleCommunityRepository(sb.PermissionString)}).ToList


            If IsNothing(oModule) Then
                oModule = New ModuleCommunityRepository
            End If
        End If

        Return oModule
    End Function
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
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


    Public ReadOnly Property CurrentPresenter() As CRexplorePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRexplorePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Protected Function BackGroundItem(ByVal oType As ListItemType, ByVal isVisible As Boolean) As String
        If isVisible Then
            Select Case oType
                Case ListItemType.Item
                    Return "ROW_Normal_Small"
                Case ListItemType.AlternatingItem
                    Return "ROW_Alternate_Small"
                Case Else
                    Return ""
            End Select
        Else
            Return "ROW_Disabilitate_Small"
        End If

        Return ""
    End Function

    Public WriteOnly Property AllowUploadFile(ByVal FolderID As Long, ByVal CommunityID As Integer) As Boolean Implements IViewExploreCommunityRepository.AllowUploadFile
        Set(ByVal value As Boolean)
            Me.HYPupload.Visible = value
            If value Then
                Me.HYPupload.NavigateUrl = Me.BaseUrl & RootObject.RepositoryUploadFile(FolderID, CommunityID, Me.CurrentView.ToString, RepositoryPage.DownLoadPage.ToString, PreLoadedContentView)
                ' Me.BaseUrl & "Modules/Repository/CommunityRepositoryUpload.aspx?Create=" & ItemRepositoryToCreate.File.ToString & "&FolderID=" & FolderID.ToString & "&PreviousView=" & Me.CurrentView.ToString & "&CommunityID=" & CommunityID & "&PreviousPage=" & RepositoryPage.DownLoadPage.ToString
            End If
        End Set
    End Property
    'Public WriteOnly Property AllowAdvancedManagement(ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean Implements IViewExploreCommunityRepository.AllowAdvancedManagement
    '    Set(ByVal value As Boolean)
    '        Me.HYPadvancedManagement.Visible = value
    '        If value Then
    '            Me.HYPadvancedManagement.NavigateUrl = Me.BaseUrl '& "Modules/Repository/WorkBookAdd.aspx?View=" & Me.CurrentView.ToString & IIf(value = IWKworkBookList.AllowCreation.Current, "", "&CreateForOther=true")
    '        End If
    '    End Set
    'End Property
    Public WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean Implements IViewExploreCommunityRepository.AllowManagement
        Set(ByVal value As Boolean)
            Me.HYPmanagement.Visible = value
            If value Then
                Me.HYPmanagement.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, Me.CurrentView.ToString, PageIndex, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&PreviousView=" & Me.CurrentView.ToString & "&Page=" & PageIndex & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property
    Public ReadOnly Property PreLoadedPageIndex() As Integer Implements IViewExploreCommunityRepository.PreLoadedPageIndex
        Get
            If IsNumeric(Request.QueryString("Page")) Then
                Return CInt(Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedFolder() As Long Implements IViewExploreCommunityRepository.PreLoadedFolder
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IViewExploreCommunityRepository.PreLoadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository Implements lm.Comol.Modules.Base.Presentation.IViewExploreCommunityRepository.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IViewExploreCommunityRepository.ViewRepository.FolderList
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("View"), IViewExploreCommunityRepository.ViewRepository.FolderList)
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer Implements IViewExploreCommunityRepository.DefaultPageSize
        Get
            Return 20
        End Get
    End Property
    Public Property CurrentPageSize() As Integer Implements IViewExploreCommunityRepository.CurrentPageSize
        Get
            Return 20
        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewExploreCommunityRepository.CurrentPager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.PGgridTop.Pager = value
            Me.PGgridBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.PGgridTop.Visible = Me.PGgridBottom.Visible
            '  Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Public Property Ascending() As Boolean Implements IViewExploreCommunityRepository.Ascending
        Get
            If TypeOf Me.ViewState("Ascending") Is Boolean Then
                Return CBool(Me.ViewState("Ascending"))
            Else
                Me.ViewState("Ascending") = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("Ascending") = value
        End Set
    End Property
    Public Property OrderBy() As CommunityFileOrder Implements IViewExploreCommunityRepository.OrderBy
        Get
            If TypeOf Me.ViewState("OrderBy") Is CommunityFileOrder Then
                Return Me.ViewState("OrderBy")
            Else
                Me.ViewState("OrderBy") = CommunityFileOrder.Name
                Return CommunityFileOrder.Name
            End If
        End Get
        Set(ByVal value As CommunityFileOrder)
            Me.ViewState("OrderBy") = value
        End Set
    End Property
    Public Property CurrentView() As IViewExploreCommunityRepository.ViewRepository Implements IViewExploreCommunityRepository.CurrentView
        Get
            If Me.DVfolders.Visible = True Then
                Return IViewExploreCommunityRepository.ViewRepository.FolderList
            Else
                Return IViewExploreCommunityRepository.ViewRepository.FileList
            End If
        End Get
        Set(ByVal value As IViewExploreCommunityRepository.ViewRepository)
            If value = IViewExploreCommunityRepository.ViewRepository.FolderList Then
                Me.DVfolders.Visible = True
                Me.DVseparator.Visible = True
                Me.DVfiles.Attributes.Add("class", "FolderFiles")
            Else
                Me.DVfolders.Visible = False
                Me.DVseparator.Visible = False
                Me.DVfiles.Attributes.Add("class", "FilesList")
            End If
        End Set
    End Property
    Private ReadOnly Property Portalname() As String Implements IViewExploreCommunityRepository.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Public WriteOnly Property TitleCommunity() As String Implements IViewExploreCommunityRepository.TitleCommunity
        Set(ByVal value As String)
            Dim CommunityName As String = value
            If Len(CommunityName) > 62 Then
                CommunityName = Left(value, 32) & " ... " & Right(value, 20)
            End If
            Me.Master.ServiceTitle = String.Format(Me.Resource.getValue("serviceManagementTitleCommunityName"), CommunityName)
            Me.Master.ServiceTitleToolTip = String.Format(Me.Resource.getValue("serviceManagementTitleCommunityName"), value)
        End Set
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IViewExploreCommunityRepository.RepositoryCommunityID
        Get
            If IsNumeric(Me.ViewState("RepositoryCommunityID")) Then
                Return Me.ViewState("RepositoryCommunityID")
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Public Property RepositoryModuleID() As Integer Implements IViewExploreCommunityRepository.RepositoryModuleID
        Get
            If IsNumeric(Me.ViewState("RepositoryModuleID")) Then
                Return Me.ViewState("RepositoryModuleID")
            Else
                Me.ViewState("RepositoryModuleID") = -1
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryModuleID") = value
        End Set
    End Property
    Public Property ShowDescription As Boolean Implements IViewExploreCommunityRepository.ShowDescription
        Get
            Return ViewStateOrDefault("ShowDescription", False)
        End Get
        Set(value As Boolean)
            Me.CBXshowDescriptionTop.Checked = value
            Me.CBXshowDescriptionBottom.Checked = value
            Me.UDPfiles.Update()
            ViewState("ShowDescription") = value
        End Set
    End Property
#End Region

#Region ""
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.CurrentPager
        Me.PGgridTop.Pager = Me.CurrentPager
    End Sub

#Region ""
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
            Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
            Me.CurrentPresenter.InitView()
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
        MyBase.SetCulture("pg_CommunityRepository", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBfolderTitle)
            .setHyperLink(Me.HYPadvancedManagement, True, True)
            .setHyperLink(Me.HYPmanagement, True, True)
            .setHyperLink(Me.HYPupload, True, True)
            .setCheckBox(CBXshowDescriptionTop)
            .setCheckBox(CBXshowDescriptionBottom)
            .setHyperLink(HYPlist, True, True)
            .setHyperLink(HYPfolders, True, True)
            Me.HYPlist.Text = String.Format(Me.HYPlist.Text, Me.BaseUrl & "images/list.jpg", Me.HYPlist.ToolTip)
            Me.HYPfolders.Text = String.Format(Me.HYPfolders.Text, Me.BaseUrl & "images/tree.jpg", Me.HYPfolders.ToolTip)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
    Public Sub NoPermission(ByVal CommunityID As Integer) Implements IViewExploreCommunityRepository.NoPermission
        Me.PageUtility.AddActionToModule(Me.RepositoryCommunityID, Me.RepositoryModuleID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.BindNoPermessi()
    End Sub
#End Region

    Public Sub LoadFolder(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IViewExploreCommunityRepository.LoadFolder
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, SelectedFolderID, ShowHiddenItems, AdminPurpose, -1, CurrentView, PreLoadedContentView)
        Me.CurrentPresenter.UpdatePathSelector(SelectedFolderID, Me.CTRLCommunityFolder.SelectedFolderPath)
        Me.CurrentPresenter.RetrieveFolderContent(Me.CTRLCommunityFolder.SelectedFolder)
        Me.UDPpath.Update()
    End Sub

    Private Sub CTRLCommunityFolder_AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLCommunityFolder.AjaxFolderSelected
        Dim Args As UC_SelectCommunityFolder.FolderArgs = DirectCast(e, UC_SelectCommunityFolder.FolderArgs)
        Me.CurrentPresenter.RetrieveFolderContent(Args.FolderID)
        Me.CurrentPresenter.UpdatePathSelector(Args.FolderID, Me.CTRLCommunityFolder.SelectedFolderPath)
        Me.UDPpath.Update()
    End Sub
    Public Sub LoadFolderContent(ByVal oList As List(Of dtoCommunityItemRepository)) Implements IViewExploreCommunityRepository.LoadFolderContent
        Me.RPTfile.DataSource = oList
        Me.RPTfile.DataBind()

        CBXshowDescriptionBottom.Visible = oList.Where(Function(f) Not IsNothing(f.File) AndAlso Not String.IsNullOrEmpty(f.File.Description)).Any()
        CBXshowDescriptionTop.Visible = oList.Where(Function(f) Not IsNothing(f.File) AndAlso Not String.IsNullOrEmpty(f.File.Description)).Any()

        Me.UDPfiles.Update()
        Me.PageUtility.AddActionToModule(Me.RepositoryCommunityID, Me.RepositoryModuleID, Services_File.ActionType.ListForDownload, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Private Sub RPTfile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfile.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBname_t")

            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBdinfo_t")
            Me.Resource.setLabel(oLabel)

            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBsortByName")
            oLinkbutton.CommandArgument = IIf(Me.OrderBy = CommunityFileOrder.Name, Not Me.Ascending, True)
            Me.Resource.setLinkButton(oLinkbutton, True, True)
            If oLinkbutton.CommandArgument Then
                oLinkbutton.Text = String.Format(oLinkbutton.Text, Me.BaseUrl & "images/dg/up.gif", oLinkbutton.ToolTip)

            Else
                oLinkbutton.Text = String.Format(oLinkbutton.Text, Me.BaseUrl & "images/dg/down.gif", oLinkbutton.ToolTip)
            End If
            oLinkbutton = e.Item.FindControl("LNBsortByDate")
            oLinkbutton.CommandArgument = IIf(Me.OrderBy = CommunityFileOrder.DateUpload, Not Me.Ascending, False)
            Me.Resource.setLinkButton(oLinkbutton, True, True)

            If oLinkbutton.CommandArgument Then
                oLinkbutton.Text = String.Format(oLinkbutton.Text, Me.BaseUrl & "images/dg/up.gif", oLinkbutton.ToolTip)
            Else
                oLinkbutton.Text = String.Format(oLinkbutton.Text, Me.BaseUrl & "images/dg/down.gif", oLinkbutton.ToolTip)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoCommunityItemRepository = DirectCast(e.Item.DataItem, dtoCommunityItemRepository)
            If Not IsNothing(oDto) Then
                Try
                    Dim oCommunityFile As CommunityFile = oDto.File

                    Dim oControl As UC_DisplayRepositoryItem = e.Item.FindControl("CTRLdisplayItem")

                    'oDiv.Visible = Not oDto.Virtual AndAlso (Me.CBXshowDescriptionBottom.Checked AndAlso oCommunityFile.Description <> "")
                    Dim displayDescription As ItemDescriptionDisplayMode = IIf(Not oDto.Virtual AndAlso (ShowDescription AndAlso oCommunityFile.Description <> ""), ItemDescriptionDisplayMode.Show, ItemDescriptionDisplayMode.None)
                    Dim commands As ItemAvailableCommand = ItemAvailableCommand.None
                    Dim displayItem As New dtoDisplayItemRepository() With {.AvailableForAll = oDto.AvailableForAll, .DisplayName = oDto.DisplayName, .File = oDto.File, .Permission = oDto.Permission, .Virtual = oDto.Virtual}
                    Dim currentUrl As String = ""

                    If oDto.File.isFile Then
                        currentUrl = RootObject.RepositoryList(IIf(oDto.Virtual, oDto.File.FolderId, oDto.File.Id), RepositoryCommunityID, Me.CurrentView.ToString, Me.CurrentPager.PageIndex, PreLoadedContentView)
                        commands = (ItemAvailableCommand.Play Or ItemAvailableCommand.Download Or ItemAvailableCommand.Settings Or ItemAvailableCommand.Statistics)
                        oControl.InitializeControl(displayItem, currentUrl, Me.RepositoryModuleID, Me.RepositoryCommunityID, CoreModuleRepository.ActionType.AjaxUpdate, Repository.ItemDisplayView.multilineFull, Repository.ItemDisplayMode.inline, displayDescription, commands)
                    Else
                        currentUrl = RootObject.RepositoryList(oDto.File.Id, RepositoryCommunityID, Me.CurrentView.ToString, 0, PreLoadedContentView)
                        oControl.InitializeControlForFolder(displayItem, currentUrl, Me.RepositoryModuleID, Me.RepositoryCommunityID, Repository.ItemDisplayView.multilineFull, Repository.ItemDisplayMode.inline, displayDescription, commands)
                    End If

                    'Dim oLBnomeFile, oLBdimensione As Label
                    'Dim oHYPfile, oHYPdownload As HyperLink
                    'Dim oLiteral As Literal = e.Item.FindControl("LTfileImage")
                    'oLBnomeFile = e.Item.FindControl("LBnomeFile")
                    'oHYPdownload = e.Item.FindControl("HYPdownload")
                    'oHYPfile = e.Item.FindControl("HYPfile")
                    'oLBdimensione = e.Item.FindControl("LBdimensione")

                    'If oCommunityFile.isFile Then
                    '    oLiteral.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oCommunityFile.Extension) & "'>"
                    'Else
                    '    oLiteral.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "'>"
                    'End If

                    'Dim quote As String = """"

                    'oLBnomeFile.Text = oDto.DisplayName
                    'oHYPfile.Text = oDto.DisplayName
                    'Me.Resource.setHyperLink(oHYPdownload, True, True)

                    If oCommunityFile.isFile Then
                        ''oCommunityFile.Name &
                        'oHYPdownload.NavigateUrl = "File.repository?FileID=" & oCommunityFile.Id.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode   'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro) 
                        'oHYPfile.NavigateUrl = "File.repository?FileID=" & oCommunityFile.Id.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode  'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro)

                        'oLBnomeFile.Visible = Not oDto.Permission.Download ' (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        'oHYPfile.Visible = oDto.Permission.Download 'Not (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        'oHYPdownload.Visible = False

                        'If oDto.Permission.Play Then
                        '    Dim oHYPcontenutoAttivo As HyperLink
                        '    Dim oIMBcontenutoAttivo As ImageButton

                        '    oIMBcontenutoAttivo = e.Item.FindControl("IMBcontenutoAttivo")
                        '    oHYPcontenutoAttivo = e.Item.FindControl("HYPcontenutoAttivo")
                        '    oHYPcontenutoAttivo.Visible = True
                        '    oIMBcontenutoAttivo.Visible = True
                        '    'oIMBcontenutoAttivo.CssClass = cssLink
                        '    ' oIMBcontenutoAttivo.CssClass = cssLink
                        '    If oCommunityFile.isSCORM Then
                        '        Dim UrlScorm As String = Me.BaseUrlNoSSL & Me.PageUtility.SystemSettings.Icodeon.GetScormRepositoryLink(oCommunityFile.Id, oCommunityFile.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.RepositoryModuleID, Me.RepositoryCommunityID, Services_File.ActionType.AjaxUpdate, Me.LinguaCode)

                        '        oIMBcontenutoAttivo.ImageUrl = Me.ScormImage
                        '        oIMBcontenutoAttivo.CommandName = "scorm"
                        '        oIMBcontenutoAttivo.OnClientClick = "window.open('" & UrlScorm & "');return false;"
                        '        MyBase.Resource.setImageButton_To_Value(oIMBcontenutoAttivo, False, "scorm", True, True)

                        '        oIMBcontenutoAttivo.OnClientClick = ""

                        '        'oHYPcontenutoAttivo.NavigateUrl = Me.EncryptedUrl("generici/Materiale_VisualizzaScorm.aspx", "ScormUniqueID=" & e.Item.DataItem("FLDS_ID"), SecretKeyUtil.EncType.Altro)
                        '        oHYPcontenutoAttivo.Target = "_blank"
                        '        oHYPcontenutoAttivo.NavigateUrl = UrlScorm
                        '        MyBase.Resource.setHyperLink(oHYPcontenutoAttivo, "scorm", True, True)

                        '    ElseIf oCommunityFile.isVideocast Then
                        '        oIMBcontenutoAttivo.ImageUrl = Me.VideoCastImage
                        '        oIMBcontenutoAttivo.CommandName = "videocast"
                        '        oHYPcontenutoAttivo.NavigateUrl = Me.EncryptedUrl("generici/Materiale_PlayVideocast.aspx", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro)
                        '        MyBase.Resource.setHyperLink(oHYPcontenutoAttivo, "videocast", True, True)
                        '        MyBase.Resource.setImageButton_To_Value(oIMBcontenutoAttivo, False, "videocast", True, True)
                        '    End If

                        '    Try
                        '        Dim oHYPstatistics As HyperLink
                        '        oHYPstatistics = e.Item.FindControl("HYPstatistics")

                        '        MyBase.Resource.setHyperLink(oHYPstatistics, True, True)
                        '        ' oHYPstatistics.ImageUrl = Me.BaseUrl & "images/grid/statistic.jpg"

                        '        oHYPstatistics.Text = String.Format(oHYPstatistics.Text, Me.BaseUrl & "images/grid/statistic.gif", oHYPstatistics.ToolTip)
                        '        oHYPstatistics.Visible = True
                        '        Dim DestinationUrl As String = Request.Url.LocalPath
                        '        If Me.BaseUrl <> "/" Then
                        '            DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
                        '        End If


                        '        DestinationUrl = Server.HtmlEncode(DestinationUrl & Request.Url.Query)
                        '        If oDto.Permission.ViewAdvancedStatistics Then
                        '            oHYPstatistics.NavigateUrl = Me.BaseUrl & RootObject.ManagementScormStatistics(oCommunityFile.Id, DestinationUrl, PreLoadedContentView)
                        '            '"Modules/Scorm/ScormStatisticheMain.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
                        '        ElseIf oDto.Permission.ViewBaseStatistics Then
                        '            oHYPstatistics.NavigateUrl = Me.BaseUrl & RootObject.UserScormStatistics(oCommunityFile.Id, DestinationUrl, PreLoadedContentView)
                        '            '"Modules/Scorm/ScormStatisticheUtente.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
                        '        Else
                        '            oHYPstatistics.NavigateUrl = ""
                        '            oHYPstatistics.Visible = False
                        '        End If
                        '        ' oHYPstatistics.CssClass = cssLink

                        '    Catch ex As Exception

                        '    End Try
                        'End If
                        'Dim FileSize As Long = oCommunityFile.Size
                        'If FileSize = 0 Then
                        '    oLBdimensione.Text = "&nbsp;"
                        'Else
                        '    If FileSize < 1024 Then
                        '        oLBdimensione.Text = " ( 1 kb) "
                        '    Else
                        '        FileSize = FileSize / 1024
                        '        If FileSize < 1024 Then
                        '            oLBdimensione.Text = " (" & Math.Round(FileSize) & " kb) "
                        '        ElseIf FileSize >= 1024 Then
                        '            oLBdimensione.Text = " (" & Math.Round(FileSize / 1024, 2) & " mb) "
                        '        End If
                        '    End If
                        'End If
                    Else
                        'oLBnomeFile.Visible = False
                        'Dim oLNBfolder As LinkButton = e.Item.FindControl("LNBfolder")
                        'oLNBfolder.Visible = True
                        'oLNBfolder.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "'>" & oDto.DisplayName
                        'oLNBfolder.CommandArgument = oCommunityFile.Id
                    End If
                    'Dim oDiv As HtmlGenericControl = e.Item.FindControl("DVdescription")
                    'oDiv.Visible = Not oDto.Virtual AndAlso (Me.CBXshowDescriptionBottom.Checked AndAlso oCommunityFile.Description <> "")
                    'If oDiv.Visible Then
                    '    Dim oLabel As Label = e.Item.FindControl("LBdescrizioneFile")
                    '    oLabel.Text = oCommunityFile.Description
                    'End If

                    'CELL 5
                    Dim oLBdata As Label
                    oLBdata = e.Item.FindControl("LBdata")
                    If oDto.Virtual Then
                        oLBdata.Text = " "
                    Else
                        Try
                            oLBdata.Text = oCommunityFile.CreatedOn.ToString("dd/MM/yy HH:mm")
                            Dim Author As String = ""
                            If Not IsNothing(oCommunityFile.Owner) Then
                                Author = oCommunityFile.Owner.SurnameAndName
                            End If
                            oLBdata.ToolTip = String.Format(Me.Resource.getValue("uploaded." & oCommunityFile.isFile), Author, oCommunityFile.CreatedOn.ToString("HH:mm"), oCommunityFile.CreatedOn.ToString("dd/MM/yy"))
                        Catch ex As Exception
                        End Try
                    End If


                    Dim oLinkButton As LinkButton = e.Item.FindControl("LNBinfo")
                    '  If oCommunityFile.isFile Then
                    Me.Resource.setLinkButton(oLinkButton, True, True)
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/proprieta.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDto.File.Id
                    oLinkButton.Visible = Not oDto.Virtual
                    ' Else
                    '   oLinkButton.Visible = False
                    '  End If
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub
    Private Sub RPTfile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTfile.ItemCommand
        If e.CommandName = "updatefoldercontent" OrElse e.CommandName = "sortName" OrElse e.CommandName = "sortDate" Then
            If e.CommandName = "updatefoldercontent" Then
                Dim FileId As Long = CLng(e.CommandArgument)
                Me.CTRLCommunityFolder.SelectedFolder = FileId
                Me.UDPfolders.Update()
            End If
            If e.CommandName = "sortName" Then
                Me.OrderBy = CommunityFileOrder.Name
                Me.Ascending = CBool(e.CommandArgument)
            ElseIf e.CommandName = "sortDate" Then
                Me.OrderBy = CommunityFileOrder.DateUpload
                Me.Ascending = CBool(e.CommandArgument)
            End If
            If e.CommandName = "updatefoldercontent" Then
                Me.CurrentPresenter.UpdatePathSelector(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLCommunityFolder.SelectedFolderPath)
                Me.UDPpath.Update()
            End If
            Me.CurrentPresenter.RetrieveFolderContent(Me.CTRLCommunityFolder.SelectedFolder)
        ElseIf e.CommandName = "info" Then
            Dim FileId As Long = CLng(e.CommandArgument)
            Me.CTRLfileDetail.InitializeControl(FileId)
            Me.OpenDialog("detailFile", Me.UDPfiles)
            Me.UDPdetails.Update()
            'Me.OpenDialog("detailFile")
            'Me.UDPfiles.Update()
        End If
    End Sub
    Public Sub UpdatePathSelector(ByVal oList As List(Of FilterElement), ByVal CommunityID As Integer) Implements IViewExploreCommunityRepository.UpdatePathSelector
        Dim UrlFormat As String = RootObject.RepositoryListFormatUrl(CommunityID, CurrentView.ToString, PreLoadedContentView) ' "{0}Modules/Repository/CommunityRepository.aspx?Page=0&FolderID={1}&CommunityID=" & CommunityID

        Me.CTRLPathSelector.InitializeControl(oList, UrlFormat)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}');", dialogId)
        'ScriptManager.RegisterOnSubmitStatement(UDPdetails, Me.GetType, Me.UniqueID, script, True)
        'ScriptManager.RegisterStartupScript(UDPdetails, UDPdetails.GetType, UDPdetails.UniqueID, script, True)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String, control As UpdatePanel)

        Dim script As String = "$(document).ready(function () {showDialog('" & dialogId & "');});"

        ''Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterStartupScript(control, Me.GetType, Guid.NewGuid().ToString(), script, True)
    End Sub
#Region "Action"

#End Region

    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected, PGgridBottom.OnPageSelected
        Me.CurrentPresenter.RetrieveFolderContent(Me.CTRLCommunityFolder.SelectedFolder)
    End Sub

    Private Sub CBXshowDescriptionTop_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXshowDescriptionTop.CheckedChanged
        Me.CurrentPresenter.SaveDescriptionSettings(Me.CBXshowDescriptionTop.Checked, Me.CTRLCommunityFolder.SelectedFolder)
    End Sub
    Private Sub CBXshowDescriptionBottom_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXshowDescriptionBottom.CheckedChanged
        Me.CurrentPresenter.SaveDescriptionSettings(Me.CBXshowDescriptionBottom.Checked, Me.CTRLCommunityFolder.SelectedFolder)
    End Sub

    Private Sub CTRLPathSelector_AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLPathSelector.AjaxFolderSelected
        Dim Args As UC_RepositoryFolderPathSelector.FolderArgs = DirectCast(e, UC_RepositoryFolderPathSelector.FolderArgs)
        Me.CurrentPresenter.RetrieveFolderContent(Args.FolderID)

        If Not Me.CurrentPager Is Nothing Then
            Me.CurrentPager.PageIndex = 0
        End If
        Me.CTRLCommunityFolder.SelectedFolder = Args.FolderID
        Me.CurrentPresenter.UpdatePathSelector(Args.FolderID, Me.CTRLCommunityFolder.SelectedFolderPath)
        Me.UDPfolders.Update()
    End Sub
    Private Sub CTRLCommunityFolder_FolderSelected(idFolder As Long) Handles CTRLCommunityFolder.FolderSelected
        PageUtility.RedirectToUrl(RootObject.RepositoryList(idFolder, RepositoryCommunityID, CurrentView.ToString, 0, PreLoadedContentView))
    End Sub
    Private Sub DefineUrlSelector(ByVal FolderID As Long, ByVal CommunityID As Integer) Implements IViewExploreCommunityRepository.DefineUrlSelector
        Me.HYPlist.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, IViewExploreCommunityRepository.ViewRepository.FileList.ToString, PreLoadedContentView)
        Me.HYPfolders.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, IViewExploreCommunityRepository.ViewRepository.FolderList.ToString, PreLoadedContentView)

        'Me.HYPlist.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepository.aspx?CommunityID=" & CommunityID.ToString & "&FolderID=" & FolderID.ToString & "&Page=0&View=" & IViewExploreCommunityRepository.ViewRepository.FileList.ToString
        ' Me.HYPfolders.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepository.aspx?CommunityID=" & CommunityID.ToString & "&FolderID=" & FolderID.ToString & "&Page=0&View=" & IViewExploreCommunityRepository.ViewRepository.FolderList.ToString
        Me.UDPpath.Update()
    End Sub

    Public Sub LoadRepositoryAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewExploreCommunityRepository.LoadRepositoryAction
        Me.PageUtility.AddActionToModule(Me.RepositoryCommunityID, Me.RepositoryModuleID, Services_File.ActionType.ListForDownload, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
    Private Sub UpdateAction()
        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            TMsession.Enabled = False
        Else
            Me.PageUtility.AddActionToModule(Me.RepositoryCommunityID, Me.RepositoryModuleID, Services_File.ActionType.AjaxUpdate, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        End If
    End Sub

    Private Sub TMsession_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMsession.Tick
        UpdateAction()
        If Me.CurrentContext.UserContext.isAnonymous Then
            Me.TMsession.Enabled = False
        End If
    End Sub
    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class