Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Linq
Public Class MultimediaFileSettings
    Inherits PageBase
    Implements IViewMultimediaFileSettings

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _Presenter As MultimediaFileSettingsPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As MultimediaFileSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MultimediaFileSettingsPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public WriteOnly Property AllowSetDefaultDocument As Boolean Implements IViewMultimediaFileSettings.AllowSetDefaultDocument
        Set(value As Boolean)
            Me.BTNsetDefault.Visible = value
        End Set
    End Property
    Public Property BackUrl As String Implements IViewMultimediaFileSettings.BackUrl
        Get
            Return ViewStateOrDefault("BackUrl", "")
        End Get
        Set(value As String)
            If (value = "" AndAlso Not IsNothing(Request.UrlReferrer)) Then
                value = Request.UrlReferrer.ToString()
            ElseIf value <> "" Then
                Me.HYPback.NavigateUrl = BaseUrl & value
            End If

            ViewState("BackUrl") = value
            Me.HYPback.Visible = Not String.IsNullOrEmpty(value)

        End Set
    End Property
    Public Property IdFile As Long Implements IViewMultimediaFileSettings.IdFile
        Get
            Return ViewStateOrDefault("IdFile", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdFile") = value
        End Set
    End Property
    Public Property IdLink As Long Implements IViewMultimediaFileSettings.IdLink
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedBackUrl As String Implements IViewMultimediaFileSettings.PreloadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Public ReadOnly Property PreloadedIdFile As Long Implements IViewMultimediaFileSettings.PreloadedIdFile
        Get
            If IsNumeric(Request.QueryString("FileID")) Then
                Return CLng(Request.QueryString("FileID"))
            Else : Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdLink As Long Implements IViewMultimediaFileSettings.PreloadedIdLink
        Get
            If IsNumeric(Request.QueryString("LinkID")) Then
                Return CLng(Request.QueryString("LinkID"))
            Else : Return 0
            End If
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
    Public ReadOnly Property BaseUrl() As String
        Get
            Return Me.PageUtility.ApplicationUrlBase(True)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Implementati "
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = True
            DirectCast(Me.Master, AjaxPortal).ServiceNopermission = Resource.getValue("ServiceSettings_NoPermission")
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = True
            '    DirectCast(Me.Master, Registrazione).ServiceNopermission = Resource.getValue("ServiceSettings_NoPermission")
        ElseIf TypeOf Me.Master Is AjaxPopup Then
            DirectCast(Me.Master, AjaxPopup).ShowNoPermission = True
            DirectCast(Me.Master, AjaxPopup).ServiceNopermission = Resource.getValue("ServiceSettings_NoPermission")
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MultimediaFileLoader", "Modules", "Repository")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPback, True, True)
            .setButton(BTNsetDefault, True, , , True)
            .setLiteral(LTmultimediaInfo)
            If TypeOf Me.Master Is AjaxPortal Then
                DirectCast(Me.Master, AjaxPortal).ServiceTitle = Resource.getValue("serviceSettings_Title")
                'ElseIf TypeOf Me.Master Is Registrazione Then
                '    DirectCast(Me.Master, Registrazione).ServiceNopermission = Resource.getValue("serviceSettings_Title")
            ElseIf TypeOf Me.Master Is AjaxPopup Then
                DirectCast(Me.Master, AjaxPopup).ServiceNopermission = Resource.getValue("serviceSettings_Title")
            End If
            .setLiteral(LTinfo)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Permessi"

    Public Function GetModuleLinkPermission(IdCommunity As Integer, IdUser As Integer, link As ModuleLink) As RepositoryItemPermission Implements IViewMultimediaFileSettings.GetModuleLinkPermission
        Dim permission As New RepositoryItemPermission

        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            Dim moduleUserString As New System.Collections.Generic.Dictionary(Of String, String)()

            Dim actions As List(Of StandardActionType) = oSender.GetAllowedStandardActionForExternal(link.SourceItem, link.DestinationItem, IdUser, GetExternalUsersLong(), moduleUserString)

            permission.EditSettings = actions.Contains(StandardActionType.EditMetadata)
            permission.ViewBaseStatistics = actions.Contains(StandardActionType.ViewPersonalStatistics)
            permission.ViewAdvancedStatistics = actions.Contains(StandardActionType.ViewAdvancedStatistics)
            permission.Play = actions.Contains(StandardActionType.Play)

            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        End Try
        Return permission
        Return permission
    End Function

    Private Function ActionType(type As Repository.RepositoryItemType) As CoreModuleRepository.ActionType
        Select Case type
            Case RepositoryItemType.FileStandard
                Return CoreModuleRepository.ActionType.DownloadFile
            Case RepositoryItemType.Folder
                Return CoreModuleRepository.ActionType.ViewFolder
            Case RepositoryItemType.None
                Return CoreModuleRepository.ActionType.None
            Case Else
                Return CoreModuleRepository.ActionType.PlayFile
        End Select

    End Function

#End Region

#Region "Errori"
    Public Sub LoadFileNoPermission() Implements IViewMultimediaFileSettings.LoadFileNoPermission
        Me.RDTmultimediaFiles.Visible = False
        Me.LTmultimediaInfo.Text = Resource.getValue("LoadFileNoPermission")
        Me.LTinfo.Visible = False
    End Sub

    Public Sub LoadFileNotExist() Implements IViewMultimediaFileSettings.LoadFileNotExist
        Me.RDTmultimediaFiles.Visible = False
        Me.LTmultimediaInfo.Text = Resource.getValue("LoadFileNoPermission")
        Me.LTinfo.Visible = False
    End Sub

    Public Sub LoadFileWithoutIndex(file As BaseCommunityFile) Implements IViewMultimediaFileSettings.LoadFileWithoutIndex
        Me.LTinfo.Visible = False
        Me.RDTmultimediaFiles.Visible = False
        Me.LTmultimediaInfo.Visible = True
        Me.LTmultimediaInfo.Text = String.Format(Resource.getValue("LoadFileWithoutIndex"), file.DisplayName, Me.BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(file.Extension), GetFileSize(file.Size))
    End Sub

    Public Sub LoadInvalidFileType(file As BaseCommunityFile) Implements IViewMultimediaFileSettings.LoadInvalidFileType
        Me.RDTmultimediaFiles.Visible = False
        Me.LTinfo.Visible = False
        Me.LTmultimediaInfo.Visible = True
        Me.LTmultimediaInfo.Text = String.Format(Resource.getValue("LoadInvalidFileType"), file.DisplayName, Me.BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(file.Extension), GetFileSize(file.Size))
    End Sub
    Private Function GetFileSize(ByVal size As Long) As String
        Dim sizeTranslated As String = ""
        If size = 0 Then
            sizeTranslated = "&nbsp;"
        Else
            If size < 1024 Then
                sizeTranslated = "1 kb"
            Else
                size = size / 1024
                If size < 1024 Then
                    sizeTranslated = Math.Round(size) & " kb"
                ElseIf size >= 1024 Then
                    sizeTranslated = Math.Round(size / 1024, 2) & " mb"
                End If
            End If
        End If
        Return sizeTranslated
    End Function
#End Region

#Region "Tree"
    Private Sub LoadTree(file As BaseCommunityFile, paths As IList(Of String), selectedPath As String) Implements IViewMultimediaFileSettings.LoadTree
        Dim root As Telerik.Web.UI.RadTreeNode = New Telerik.Web.UI.RadTreeNode()
        Me.LTinfo.Visible = True
        Me.RDTmultimediaFiles.Visible = True
        Me.LTmultimediaInfo.Visible = False

        root.Value = ""
        root.Text = file.Name
        root.Checkable = False
        root.Category = 0
        Dim node As Telerik.Web.UI.RadTreeNode = root
        For Each filePath As String In paths
            node = root
            For Each item As String In filePath.Split("\")
                node = AddNode(node, item)
            Next
        Next
        Me.RDTmultimediaFiles.Nodes.Clear()
        Me.RDTmultimediaFiles.Nodes.Add(root)

        selectedPath = Replace(file.Name & "/" & selectedPath, "\", "/")
        Dim selectedNode As Telerik.Web.UI.RadTreeNode = (From n In Me.RDTmultimediaFiles.GetAllNodes Where n.Nodes.Count = 0 AndAlso n.FullPath = selectedPath Select n).FirstOrDefault
        If Not IsNothing(selectedNode) Then
            selectedNode.Selected = True
            selectedNode.ExpandParentNodes()
        End If

        Dim folders As List(Of Telerik.Web.UI.RadTreeNode) = (From n In Me.RDTmultimediaFiles.GetAllNodes Where n.Nodes.Count > 0 Select n).ToList()
        Dim files As List(Of Telerik.Web.UI.RadTreeNode) = (From n In Me.RDTmultimediaFiles.GetAllNodes Where n.Nodes.Count = 0 Select n).ToList()

        For Each n As Telerik.Web.UI.RadTreeNode In folders
            n.Text = "<img src=""" & Me.BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & """ > " & n.Text
        Next

        For Each nn As Telerik.Web.UI.RadTreeNode In files
            Dim extension As String = "." & StrReverse((From n As String In StrReverse(nn.Text).Split(".").ToList Select n).FirstOrDefault)
            nn.Text = "<img src=""" & Me.BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(extension) & """ > " & nn.Text
        Next
       
    End Sub

    Private Function AddNode(node As Telerik.Web.UI.RadTreeNode, key As String) As Telerik.Web.UI.RadTreeNode
        Dim item As Telerik.Web.UI.RadTreeNode = node.Nodes.FindNodeByText(key)

        If (Not IsNothing(item)) Then
            Return item
        Else
            item = New Telerik.Web.UI.RadTreeNode(key)
            item.Value = node.Value & "\" & key
            node.Nodes.Add(item)
            node.Checkable = False
            Return item
        End If
    End Function
#End Region

    Public Sub SendToSessionExpiredPage(IdCommunity As Integer, languageCode As String) Implements IViewMultimediaFileSettings.SendToSessionExpiredPage
        'Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
        'oRemotePost.Url = PageUtility.BaseUrl & "Modules/Common/CommonSessionExpired.aspx?PreservePreviousUrl=True"
        'oRemotePost.Add("ForUserID", IdUser.ToString)
        'oRemotePost.Add("Language", languageCode)
        'oRemotePost.Add("CommunityID", IdCommunity)
        'oRemotePost.Add("ForDownload", "False")
        'oRemotePost.Add("LinkID", IdLink)
        'oRemotePost.Post()

        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        If PageUtility.BaseUrl = "/" Then
            dto.DestinationUrl = Request.RawUrl.Replace("//", "/")
        Else
            dto.DestinationUrl = Request.RawUrl.Replace(PageUtility.BaseUrl, "")
        End If

        If IdCommunity > 0 Then
            dto.IdCommunity = IdCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub BTNsetDefault_Click(sender As Object, e As System.EventArgs) Handles BTNsetDefault.Click
        If Not IsNothing(Me.RDTmultimediaFiles.SelectedNode) Then
            Me.CurrentPresenter.SetDefaultDocument(Me.RDTmultimediaFiles.SelectedNode.Value)
        End If
    End Sub
End Class