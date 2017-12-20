Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2.UCServices


Partial Public Class UC_WorkBookItemFiles
    Inherits BaseControlSession
    Implements IviewWorkBookItemFileList
    Public Event UpdateToParent()

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
#Region "View property"
    Private _PageUtility As OLDpageUtility
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As WorkBookItemFilePresenter
    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Public Property ShowManagementButtons() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItemFileList.ShowManagementButtons
        Get
            If String.IsNullOrEmpty(Me.ViewState("ShowManagementButtons")) Then
                Me.ViewState("ShowManagementButtons") = False
            End If
            Return CBool(Me.ViewState("ShowManagementButtons"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowManagementButtons") = value
            Me.GDVWorkBookItemFiles.Columns(1).Visible = value
            Me.GDVWorkBookItemFiles.Columns(2).Visible = value
        End Set
    End Property
    Private Property ItemID() As System.Guid Implements IviewWorkBookItemFileList.ItemID
        Get
            If TypeOf Me.ViewState("ItemID") Is System.Guid Then
                Try
                    Return Me.ViewState("ItemID") 'New System.Guid(Me.ViewState("ItemID").ToString)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
        Set(ByVal value As System.Guid)
            Me.ViewState("ItemID") = value
        End Set
    End Property
    Private ReadOnly Property CurrentPresenter() As WorkBookItemFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WorkBookItemFilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Private ReadOnly Property ScormImage() As String
        Get
            Return Me.BaseUrl & "images/scorm/visualizza.png"
        End Get
    End Property
    Public ReadOnly Property VideoCastImage() As String
        Get
            Return Me.BaseUrl & "images/scorm/visualizza.png"
        End Get
    End Property

    Private Property CommunityRepositoryPermissions() As ModuleCommunityRepository Implements IviewWorkBookItemFileList.CommunityRepositoryPermissions
        Get
            Dim oPermission As ModuleCommunityRepository = TryCast(Me.ViewState("CommunityRepositoryPermissions"), ModuleCommunityRepository)
            If IsNothing(oPermission) Then
                oPermission = New ModuleCommunityRepository
                Me.ViewState("CommunityRepositoryPermissions") = oPermission
            End If
            Return oPermission
        End Get
        Set(ByVal value As ModuleCommunityRepository)
            Me.ViewState("CommunityRepositoryPermissions") = value
        End Set
    End Property

    Private Property ItemPermissions() As WorkBookItemPermission Implements IviewWorkBookItemFileList.ItemPermissions
        Get
            Dim oPermission As WorkBookItemPermission = TryCast(Me.ViewState("ItemPermissions"), WorkBookItemPermission)
            If IsNothing(oPermission) Then
                oPermission = New WorkBookItemPermission
                Me.ViewState("ItemPermissions") = oPermission
            End If
            Return oPermission
        End Get
        Set(ByVal value As WorkBookItemPermission)
            Me.ViewState("ItemPermissions") = value
        End Set
    End Property

    Public Property AutoUpdate() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItemFileList.AutoUpdate
        Get
            Return CBool(Me.ViewState("AutoUpdate"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AutoUpdate") = value
        End Set
    End Property

    Private ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
    Private Property AllowPublish() As Boolean
        Get
            If TypeOf Me.ViewState("AllowPublish") Is Boolean Then
                Return CBool(Me.ViewState("AllowPublish"))
            End If
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowPublish") = value
        End Set
    End Property
    Public Property WorkBookModuleID() As Integer Implements IviewWorkBookItemFileList.WorkBookModuleID
        Get
            If TypeOf Me.ViewState("WorkBookModuleID") Is Integer Then
                Return CInt(Me.ViewState("WorkBookModuleID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("WorkBookModuleID") = value
        End Set
    End Property
    Public Property WorkBookCommunityID() As Integer Implements IviewWorkBookItemFileList.WorkBookCommunityID
        Get
            If TypeOf Me.ViewState("WorkBookCommunityID") Is Integer Then
                Return CInt(Me.ViewState("WorkBookCommunityID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("WorkBookCommunityID") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal AutoUpdate As Boolean, ByVal ShowButtons As Boolean, ByVal OnlyVisibleFiles As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository, ByVal pAllowPublish As Boolean)
        If Me.Page.IsPostBack = False Then
            Me.SetCultureSettings()
            Me.SetInternazionalizzazione()
        End If
        Me.AutoUpdate = AutoUpdate
        Me.ShowManagementButtons = ShowButtons
        Me.AllowPublish = pAllowPublish
        Me.CurrentPresenter.InitView(ItemID, CommunityID, OnlyVisibleFiles, oPermission, oModule)
    End Sub

#Region "Inherited Method"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("WorkBookItemManagementFile", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHeaderGridView(Me.GDVWorkBookItemFiles, 1, "E", True)
            .setHeaderGridView(Me.GDVWorkBookItemFiles, 2, "EXPORT", True)
            .setHeaderGridView(Me.GDVWorkBookItemFiles, 4, "CreatedOn", True)
            .setHeaderGridView(Me.GDVWorkBookItemFiles, 5, "CreatedBy", True)
            .setHeaderGridView(Me.GDVWorkBookItemFiles, 6, "C", True)
        End With
    End Sub
#End Region

    Public Sub LoadFiles(ByVal oList As List(Of dtoWorkBookFile)) Implements IviewWorkBookItemFileList.LoadFiles
        Me.GDVWorkBookItemFiles.Visible = (oList.Count > 0)
        Me.GDVWorkBookItemFiles.DataSource = oList
        Me.GDVWorkBookItemFiles.DataBind()
    End Sub

    Private Sub GDVWorkBookItemFiles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVWorkBookItemFiles.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oFileItem As dtoWorkBookFile = TryCast(e.Row.DataItem, dtoWorkBookFile)

            If Not IsNothing(oFileItem) Then
                Dim cssLink As String = "ROW_ItemLink_Small"
                Dim cssRiga As String = "ROW_TD_Small"
                Try
                    If oFileItem.isDeleted Then
                        e.Row.CssClass = "ROW_Disabilitate_Small"
                    ElseIf e.Row.RowType = ListItemType.AlternatingItem Then
                        e.Row.CssClass = "ROW_Alternate_Small"
                    Else
                        e.Row.CssClass = "ROW_Normal_Small"
                    End If
                Catch ex As Exception
                    If e.Row.RowType = ListItemType.AlternatingItem Then
                        e.Row.CssClass = "ROW_Alternate_Small"
                    End If
                End Try

                ' CELL 2
                Dim oLNBunlink, oLNBvirtualDelete, oLNBundelete As LinkButton

                oLNBunlink = e.Row.FindControl("LNBunlink")
                oLNBvirtualDelete = e.Row.FindControl("LNBvirtualDelete")
                oLNBundelete = e.Row.FindControl("LNBundelete")


                Me.Resource.setLinkButton(oLNBvirtualDelete, True, True)
                oLNBvirtualDelete.Visible = Not oFileItem.isCommunityFile AndAlso Not oFileItem.isDeleted
                oLNBundelete.Visible = Not oFileItem.isCommunityFile AndAlso oFileItem.isDeleted
                oLNBunlink.Visible = oFileItem.isCommunityFile

                Me.Resource.setLinkButton(oLNBunlink, True, True)
                oLNBunlink.Text = String.Format(oLNBunlink.Text, Me.BaseUrl & "images/grid/Unlink.gif", oLNBunlink.ToolTip)

                Me.Resource.setLinkButton(oLNBvirtualDelete, True, True)
                oLNBvirtualDelete.Text = String.Format(oLNBvirtualDelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBvirtualDelete.ToolTip)

                Me.Resource.setLinkButton(oLNBundelete, True, True)
                oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLNBundelete.ToolTip)

                oLNBvirtualDelete.CommandArgument = oFileItem.ID.ToString
                oLNBunlink.CommandArgument = oFileItem.ID.ToString
                oLNBundelete.CommandArgument = oFileItem.ID.ToString


                ' CELL 3
                Dim oPublish As HyperLink
                oPublish = e.Row.FindControl("HYPpublishItem")
                Me.Resource.setHyperLink(oPublish, True, True)
                oPublish.Text = String.Format(oPublish.Text, Me.BaseUrl & "images/grid/SendToCommunityRepository.jpg", oPublish.ToolTip)
                oPublish.Visible = Me.AllowPublish AndAlso Not oFileItem.isCommunityFile AndAlso Not oFileItem.isDeleted 'AndAlso oFileItem.AllowPublish
                oPublish.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemPublishFile.aspx?ItemID=" & oFileItem.ItemOwner.ToString & "&FileID=" & oFileItem.ID.ToString & "&View=" & Me.PreviousWorkBookView.ToString

                'CELL 4
                Try
                    Dim oLBnomeFile, oLBdimensione As Label
                    Dim oHYPfile, oHYPdownload As HyperLink

                    oLBnomeFile = e.Row.FindControl("LBnomeFile")
                    oHYPdownload = e.Row.FindControl("HYPdownload")
                    oHYPfile = e.Row.FindControl("HYPfile")
                    oLBdimensione = e.Row.FindControl("LBdimensione")

                    Dim NomeFile As String = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oFileItem.Extension) & "'>&nbsp;" & oFileItem.Name
                    Dim quote As String = """"

                    oLBnomeFile.Text = NomeFile
                    oHYPfile.Text = NomeFile

                    oLBnomeFile.CssClass = cssRiga
                    oHYPfile.CssClass = cssLink
                    oHYPdownload.CssClass = cssLink
                    oLBdimensione.CssClass = cssRiga

                    Me.Resource.setHyperLink(oHYPdownload, True, True)

                    If oFileItem.isCommunityFile Then
                        'oHYPdownload.NavigateUrl = PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oFileItem.CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro)
                        'oHYPfile.NavigateUrl = PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oFileItem.CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro)

                        oHYPdownload.NavigateUrl = "File.repository?FileID=" & oFileItem.CommunityFileID.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode & "&ModuleID=" & Me.WorkBookModuleID.ToString & "&ItemID=" & Me.ItemID.ToString   'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro) 
                        oHYPfile.NavigateUrl = "File.repository?FileID=" & oFileItem.CommunityFileID.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode & "&ModuleID=" & Me.WorkBookModuleID.ToString & "&ItemID=" & Me.ItemID.ToString  'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro)

                        oLBnomeFile.Visible = Not oFileItem.Permission.Download ' (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        oHYPfile.Visible = oFileItem.Permission.Download 'Not (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        oHYPdownload.Visible = False


                        'oLBnomeFile.Visible = (oFileItem.Permission.Play AndAlso Not oFileItem.Permission.Download)
                        'oHYPfile.Visible = Not (oFileItem.Permission.Play AndAlso Not oFileItem.Permission.Download)
                        'oHYPdownload.Visible = False

                        If oFileItem.Permission.Play Then
                            Dim oHYPcontenutoAttivo As HyperLink
                            Dim oIMBcontenutoAttivo As ImageButton

                            oIMBcontenutoAttivo = e.Row.FindControl("IMBcontenutoAttivo")
                            oHYPcontenutoAttivo = e.Row.FindControl("HYPcontenutoAttivo")
                            oHYPcontenutoAttivo.Visible = True
                            oIMBcontenutoAttivo.Visible = True
                            oIMBcontenutoAttivo.CssClass = cssLink
                            oIMBcontenutoAttivo.CssClass = cssLink
                            If oFileItem.isSCORM Then
                                oIMBcontenutoAttivo.Visible = False
                                oHYPcontenutoAttivo.Visible = False
                            ElseIf oFileItem.isVideocast Then
                                oIMBcontenutoAttivo.ImageUrl = Me.VideoCastImage
                                oIMBcontenutoAttivo.CommandName = "videocast"
                                oHYPcontenutoAttivo.NavigateUrl = Me.EncryptedUrl("generici/Materiale_PlayVideocast.aspx", "FileID=" & oFileItem.CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro)
                                MyBase.Resource.setHyperLink(oHYPcontenutoAttivo, "videocast", True, True)
                                MyBase.Resource.setImageButton_To_Value(oIMBcontenutoAttivo, False, "videocast", True, True)
                            End If

                            Try
                                Dim oHYPstatistics As HyperLink
                                oHYPstatistics = e.Row.FindControl("HYPstatistics")

                                MyBase.Resource.setHyperLink(oHYPstatistics, True, True)
                                oHYPstatistics.Text = String.Format(oHYPstatistics.Text, Me.BaseUrl & "images/grid/statistic.gif", oHYPstatistics.ToolTip)
                                oHYPstatistics.Visible = True
                                Dim DestinationUrl As String = Request.Url.LocalPath
                                If Me.BaseUrl <> "/" Then
                                    DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
                                End If

                                DestinationUrl = Server.UrlEncode(DestinationUrl & Request.Url.Query)
                                If Me.CommunityRepositoryPermissions.Administration Then
                                    oHYPstatistics.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormStatisticheMain.aspx?FileID=" & oFileItem.CommunityFileID.ToString & "&BackUrl=" & DestinationUrl
                                ElseIf Me.CommunityRepositoryPermissions.ListFiles Then

                                    oHYPstatistics.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormStatisticheUtente.aspx?FileID=" & oFileItem.CommunityFileID.ToString & "&BackUrl=" & DestinationUrl
                                Else
                                    oHYPstatistics.NavigateUrl = ""
                                    oHYPstatistics.Visible = False
                                End If
                                oHYPstatistics.CssClass = cssLink

                            Catch ex As Exception

                            End Try

                        End If
                    Else
                        oHYPfile.Visible = True
                        oHYPdownload.Visible = False
                        oLBnomeFile.Visible = False
                        oHYPfile.NavigateUrl = "File.repository?InternalFileID=" & oFileItem.InternalFileID.ToString
                    End If
                    If oFileItem.Size = 0 Then
                        oLBdimensione.Text = "&nbsp;"
                    Else
                        Dim FileSize As Long = oFileItem.Size
                        If FileSize = 0 Then
                            oLBdimensione.Text = "&nbsp;"
                        Else
                            If FileSize < 1024 Then
                                oLBdimensione.Text = " ( 1 kb) "
                            Else
                                FileSize = FileSize / 1024
                                If FileSize < 1024 Then
                                    oLBdimensione.Text = " (" & Math.Round(FileSize) & " kb) "
                                ElseIf FileSize >= 1024 Then
                                    oLBdimensione.Text = " (" & Math.Round(FileSize / 1024, 2) & " mb) "
                                End If
                            End If
                        End If
                        'If oFileItem.isCommunityFile Then
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

                        'Else
                        '    FileSize = oFileItem.Size / 1024
                        '    If FileSize < 1024 Then
                        '        oLBdimensione.Text = " (" & Math.Round(FileSize) & " kb) "
                        '    ElseIf FileSize >= 1024 Then
                        '        oLBdimensione.Text = " (" & Math.Round(FileSize / 1024, 2) & " mb) "
                        '    End If
                        'End If
                    End If
                Catch ex As Exception
                End Try

                'CELL 5
                Dim oLBauthor, oLBdata As Label
                oLBauthor = e.Row.FindControl("LBauthor")
                oLBdata = e.Row.FindControl("LBdata")

                Try
                    oLBauthor.CssClass = cssRiga
                    If oFileItem.isCommunityFile Then
                        oLBauthor.Text = String.Format(Me.Resource.getValue("CommunityAuthor"), oFileItem.CreatedBy.Name)
                    Else
                        oLBauthor.Text = oFileItem.CreatedBy.SurnameAndName
                    End If
                    oLBdata.CssClass = cssRiga
                    oLBdata.Text = oFileItem.ModifiedOn.ToString("dd/MM/yy") & " " & FormatDateTime(oFileItem.ModifiedOn, DateFormat.ShortTime)
                Catch ex As Exception
                    oLBauthor.Text = oFileItem.CreatedBy.SurnameAndName
                End Try
            End If
        End If
    End Sub
    Private Sub GDVdiaryFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GDVWorkBookItemFiles.RowCommand
        Dim FileID As System.Guid
        Try
            FileID = New System.Guid(e.CommandArgument.ToString)
            Select Case e.CommandName
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDelete(FileID)
                Case "undelete"
                    Me.CurrentPresenter.VirtualUndelete(FileID)
                Case "confirmDelete"
                    Me.CurrentPresenter.Delete(FileID, Me.PageUtility.BaseUserRepositoryPath)
                Case "unlink"
                    Me.CurrentPresenter.UnlinkCommunityFile(FileID)
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RequireUpdate() Implements IviewWorkBookItemFileList.RequireUpdate
        RaiseEvent UpdateToParent()
    End Sub

    Public Property AllowOnlyVisibleFiles() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItemFileList.AllowOnlyVisibleFiles
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowOnlyVisibleFiles")) Then
                Me.ViewState("AllowOnlyVisibleFiles") = True
            End If
            Return CBool(Me.ViewState("AllowOnlyVisibleFiles"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowOnlyVisibleFiles") = value
        End Set
    End Property


End Class