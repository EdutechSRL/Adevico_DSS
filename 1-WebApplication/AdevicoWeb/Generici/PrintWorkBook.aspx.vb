Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices

Partial Public Class PrintWorkBook
	Inherits PageBasePopUp
    Implements IviewPrintWorkBook

#Region "Private"
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _PageUtility As OLDpageUtility
    Private _Presenter As lm.Comol.Modules.Base.Presentation.PrintWorkBookPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_WorkBook
    Private _BaseUrl As String
#End Region

#Region "Public"
    Public ReadOnly Property PreloadedWorkBookID() As System.Guid Implements IviewPrintWorkBook.PreloadedWorkBookID
        Get
            Dim UrlID As String = Request.QueryString("WorkBookID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public Overrides ReadOnly Property AutoCloseWindow() As Boolean
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.PrintWorkBookPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.PrintWorkBookPresenter(Me.CurrentContext, Me)
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
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IviewPrintWorkBook.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IviewPrintWorkBook.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                 Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        End If
      
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function
    Public ReadOnly Property DisplayOrderAscending() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.DisplayOrderAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return True
            Else
                Try
                    Return CBool(Request.QueryString("Ascending"))
                Catch ex As Exception
                    Return True
                End Try
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub LoadItems(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Modules.Base.DomainModel.dtoWorkBookItem)) Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.LoadItems
        Me.DIVpermessi.Style("display") = "none"
        Me.BTNprintItems.Enabled = oList.Count > 0
        RPTitemsDetails.DataSource = oList
        Me.RPTitemsDetails.DataBind()
    End Sub

    'Public Sub ShowError(ByVal ErrorString As String) Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.ShowError
    '    If ErrorString <> "" Then
    '        Me.LBNopermessi.Text = ErrorString
    '    Else
    '        Me.Resource.setLabel(Me.LBNopermessi)
    '    End If
    '    Me.DIVpermessi.Style("display") = "block"
    'End Sub


    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitPrintItems()
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookItemsList", "Generici")
    End Sub


    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(Me.BTNclose, True)
            .setButton(Me.BTNprintItems, True)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Private Sub RPTitemsDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitemsDetails.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim odtoWorkBookItem As lm.Comol.Modules.Base.DomainModel.dtoWorkBookItem = TryCast(e.Item.DataItem, lm.Comol.Modules.Base.DomainModel.dtoWorkBookItem)
            If Not IsNothing(odtoWorkBookItem) Then
                Dim oDiv As HtmlControls.HtmlControl
                Dim oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem = odtoWorkBookItem.Item

                Dim oLiteral As Literal
                oLiteral = e.Item.FindControl("LTitemHeader")
                If Not IsNothing(oLiteral) Then
                    If IsNothing(oItem.ModifiedBy) Then
                        oLiteral.Text = String.Format(Me.Resource.getValue("createdHeader"), oItem.StartDate.ToString("dd/MM/yy"), oItem.CreatedBy.SurnameAndName, oItem.CreatedOn.Value.ToString("dd/MM/yy"), oItem.CreatedOn.Value.ToString("hh:ss"))
                    ElseIf oItem.isDeleted Then
                        oLiteral.Text = String.Format(Me.Resource.getValue("deletedHeader"), oItem.StartDate.ToString("dd/MM/yy"), oItem.CreatedBy.SurnameAndName, oItem.ModifiedOn.Value.ToString("dd/MM/yy"), oItem.ModifiedOn.Value.ToString("hh:ss"), oItem.ModifiedBy.SurnameAndName)
                    ElseIf oItem.ModifiedBy Is oItem.CreatedBy Then
                        If oItem.CreatedOn = oItem.ModifiedOn Then
                            oLiteral.Text = String.Format(Me.Resource.getValue("createdHeader"), oItem.StartDate.ToString("dd/MM/yy"), oItem.CreatedBy.SurnameAndName, oItem.CreatedOn.Value.ToString("dd/MM/yy"), oItem.CreatedOn.Value.ToString("hh:ss"))
                        Else
                            oLiteral.Text = String.Format(Me.Resource.getValue("selfchangedHeader"), oItem.StartDate.ToString("dd/MM/yy"), oItem.CreatedBy.SurnameAndName, oItem.CreatedOn.Value.ToString("dd/MM/yy"), oItem.CreatedOn.Value.ToString("hh:ss"))
                        End If
                    Else
                        oLiteral.Text = String.Format(Me.Resource.getValue("changedHeader"), oItem.StartDate.ToString("dd/MM/yy"), oItem.CreatedBy.SurnameAndName, oItem.ModifiedOn.Value.ToString("dd/MM/yy"), oItem.ModifiedOn.Value.ToString("hh:ss"), oItem.ModifiedBy.SurnameAndName)
                    End If
                End If

                oDiv = e.Item.FindControl("DIVtitolo")
                If Not IsNothing(oDiv) Then
                    If oItem.Title = "" Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                    End If
                End If
                oDiv = e.Item.FindControl("DIVtext")
                If Not IsNothing(oDiv) Then
                    If oItem.Body = "" Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                    End If
                End If
                oDiv = e.Item.FindControl("DIVnote")
                If Not IsNothing(oDiv) Then
                    'If oItem.Note = "" OrElse Not Me.isOwner Then
                    oDiv.Style("Display") = "none"
                    'Else
                    '	oDiv.Style("Display") = "block"
                    'End If
                End If
                Dim oLabel As Label
                oLabel = e.Item.FindControl("LBtitolo_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBprogramma_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBnote_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If

                oDiv = e.Item.FindControl("DIVmateriale")
                If Not IsNothing(oDiv) Then
                    oLabel = e.Item.FindControl("LBmateriale_t")
                    If Not IsNothing(oLabel) Then
                        Me.Resource.setLabel(oLabel)
                    End If
                    If IsNothing(oItem.Files) OrElse oItem.Files.Count = 0 Then
                        oDiv.Style("Display") = "none"
                    Else
                        oDiv.Style("Display") = "block"
                        Dim oRepeater As System.Web.UI.WebControls.Repeater = e.Item.FindControl("RPTitemFiles")
                        If Not IsNothing(oRepeater) Then
                            AddHandler oRepeater.ItemDataBound, AddressOf RPTitemFiles_ItemDataBound
                            oRepeater.DataSource = Me.CurrentPresenter.GetItemFiles(oItem.ID, odtoWorkBookItem.Permission)
                            oRepeater.DataBind()

                            If oRepeater.Items.Count = 0 Then
                                oDiv.Style("Display") = "none"
                            End If
                        End If
                    End If
                End If

                Dim oLink As LinkButton
                Dim isItemEditable As Boolean = odtoWorkBookItem.Permission.Write
                Dim isItemDeletable As Boolean = odtoWorkBookItem.Permission.Delete
                oLink = e.Item.FindControl("LKBMateriale")
                If Not IsNothing(oLink) Then
                    oLink.Visible = isItemEditable
                    Me.Resource.setLinkButton(oLink, True, True)
                End If

                oDiv = e.Item.FindControl("DIVadminPanel")
                If Not IsNothing(oDiv) Then
                    oDiv.Style("Display") = "block"

                    oLabel = e.Item.FindControl("LBstatusItem_t")
                    If Not IsNothing(oLabel) Then
                        Me.Resource.setLabel(oLabel)
                    End If
                    oLabel = e.Item.FindControl("LBstatusItem")
                    oLabel.Text = odtoWorkBookItem.StatusTranslated
                    If oItem.ApprovedOn.HasValue AndAlso oItem.ApprovedOn <> oItem.CreatedOn Then
                        oLabel.Text &= " - " & oItem.ApprovedBy.SurnameAndName & " (" & CDate(oItem.ApprovedOn).ToString("dd/MM/yy - hh:mm") & "). "
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub RPTitemFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oFileItem As dtoWorkBookFile = TryCast(e.Item.DataItem, dtoWorkBookFile)
            If Not IsNothing(oFileItem) Then
                Dim oLabel, oLabelDimension As Label
                oLabel = e.Item.FindControl("LBnomeFile")
                oLabelDimension = e.Item.FindControl("LBdimensione")
                Dim NomeFile As String = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oFileItem.Extension) & "'>&nbsp;" & oFileItem.Name

                oLabel.Text = NomeFile
                If oFileItem.Size = 0 Then
                    oLabelDimension.Text = "&nbsp;"
                Else
                    Dim FileSize As Long = 0
                    If oFileItem.isCommunityFile Then
                        If oFileItem.Size < 1000 Then : oLabelDimension.Text = " (" & oFileItem.Size & " kb) "
                        ElseIf oFileItem.Size >= 1000 Then
                            '   oLBdimensione.Text = " (" & oFileItem.Size / 1000 & " mb) "
                            oLabelDimension.Text = " (" & Math.Round(oFileItem.Size / 1000, 2) & " mb) "
                        End If
                    Else
                        FileSize = oFileItem.Size / 1024
                        If FileSize < 1024 Then
                            oLabelDimension.Text = " (" & Math.Round(FileSize) & " kb) "
                        ElseIf FileSize >= 1024 Then
                            oLabelDimension.Text = " (" & Math.Round(FileSize / 1024, 2) & " mb) "
                        End If
                    End If
                End If
                oLabel.Visible = True
            End If
        End If
    End Sub

    Public Sub NoPermissionToViewItems() Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.NoPermissionToViewItems
        Me.LBNopermessi.Text = Me.Resource.getValue("nopermission")
        Me.DIVpermessi.Style("display") = "block"
    End Sub
    Public Sub NoWorkBookItemWithThisID() Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.NoWorkBookItemWithThisID
        Me.LBNopermessi.Text = Me.Resource.getValue("NoWorkBookWithThisID")
        Me.DIVpermessi.Style("display") = "block"
    End Sub
    Public Sub NoWorkBookWithThisID() Implements lm.Comol.Modules.Base.Presentation.IviewPrintWorkBook.NoWorkBookWithThisID
        Me.LBNopermessi.Text = Me.Resource.getValue("NoWorkBookItemWithThisID")
        Me.DIVpermessi.Style("display") = "block"
    End Sub

 
End Class