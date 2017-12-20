Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Partial Public Class WorkBookItemsList
    Inherits PageBase
    Implements IWKitemsList


#Region "VIew"
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _Presenter As WKItemsListPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _AvailableStatus As List(Of TranslatedItem(Of Integer))
    Private _AvailableEditing As List(Of TranslatedItem(Of Integer))
    Public ReadOnly Property AvailableStatus() As List(Of TranslatedItem(Of Integer))
        Get
            If IsNothing(_AvailableStatus) Then
                _AvailableStatus = Me.CurrentPresenter.GetAvailableStatus
            End If
            Return _AvailableStatus
        End Get
    End Property
    Public ReadOnly Property AvailableEditing(ByVal oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem) As List(Of TranslatedItem(Of Integer))
        Get
            Dim oItemEditing As EditingPermission = oItem.Editing
            Dim oEditingAvailables As List(Of TranslatedItem(Of Integer))
            If IsNothing(_AvailableEditing) Then
                _AvailableEditing = Me.CurrentPresenter.GetEditingValues
            End If

            oEditingAvailables = (From o In _AvailableEditing Where ((o.Id And oItemEditing) > 0) Select o).ToList
            If (From o In oEditingAvailables Select o.Id = oItemEditing).Count = 0 Then
                oEditingAvailables.Add(New TranslatedItem(Of Integer) With {.Id = oItemEditing, .Translation = Me.GetEditingTranslation(oItemEditing)})
            End If
            Dim oOwnerTranslated As TranslatedItem(Of Integer) = (From t In oEditingAvailables Where t.Id = EditingSettings.OnlyAuthor Select t).FirstOrDefault
            If Not IsNothing(oOwnerTranslated) Then
                If oItem.CreatedBy.Id = Me.CurrentContext.UserContext.CurrentUserID Then
                    oOwnerTranslated.Translation = Me.GetEditingTranslationOwner(True, EditingSettings.OnlyAuthor)
                Else
                    oOwnerTranslated.Translation = Me.GetEditingTranslationOwner(False, EditingSettings.OnlyAuthor)
                    oOwnerTranslated.Translation = String.Format(oOwnerTranslated.Translation, GetDisplayName(oItem.CreatedBy))
                End If
            End If
            Return oEditingAvailables.OrderBy(Function(c) c.Translation).ToList
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
    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IWKitemsList.CommunitiesPermission
        Get
            '      Dim oTest = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) Where sb.CommunityID = 1 _
            '                                   Select sb).FirstOrDefault

            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If

            '    Dim p As WorkBookCommunityPermission = (From sb In _CommunitiesPermission Where sb.ID = 1 _
            '                                 Select sb).FirstOrDefault
            Return _CommunitiesPermission
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IWKitemsList.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
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
    Public ReadOnly Property CurrentPresenter() As WKItemsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WKItemsListPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Protected Function BackGroundItem(ByVal oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem) As String
        If oItem.isDeleted Then
            Return "ROW_Disabilitate_Small"
        Else
            Return ""
        End If
    End Function
    Public Property DisplayOrderAscending() As Boolean Implements IWKitemsList.DisplayOrderAscending
        Get
            Return (Me.RBLorderby.SelectedIndex = 0)
        End Get
        Set(ByVal value As Boolean)
            Me.RBLorderby.SelectedIndex = IIf(value, 0, 1)
        End Set
    End Property
    Public ReadOnly Property PreloadedWorkBookID() As System.Guid Implements IWKitemsList.PreloadedWorkBookID
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
    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IWKitemsList.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property

    Public Property AllowChangeApprovation() As Boolean Implements IWKitemsList.AllowChangeApprovation
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowChangeApprovation")) Then
                Me.ViewState("AllowChangeApprovation") = False
            End If
            Return CBool(Me.ViewState("AllowChangeApprovation"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowChangeApprovation") = value
            Me.LNBsaveStatus.Visible = value
        End Set
    End Property
    Public Property AllowItemsSelection() As Boolean Implements IWKitemsList.AllowItemsSelection
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowItemsSelection")) Then
                Me.ViewState("AllowItemsSelection") = False
            End If
            Return CBool(Me.ViewState("AllowItemsSelection"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowItemsSelection") = value
        End Set
    End Property
    Public Property AllowMultipleDelete() As Boolean Implements IWKitemsList.AllowMultipleDelete
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowMultipleDelete")) Then
                Me.ViewState("AllowMultipleDelete") = False
            End If
            Return CBool(Me.ViewState("AllowMultipleDelete"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowMultipleDelete") = value
            Me.LNBdeleteItems.Visible = value
        End Set
    End Property
    Public Property AllowPrint() As Boolean Implements IWKitemsList.AllowPrint
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowPrint")) Then
                Me.ViewState("AllowPrint") = False
            End If
            Return CBool(Me.ViewState("AllowPrint"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowPrint") = value
            Me.HYPprintItems.Visible = value
        End Set
    End Property
    Public Property AllowAddItem() As Boolean Implements IWKitemsList.AllowAddItem
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowAddItem")) Then
                Me.ViewState("AllowAddItem") = False
            End If
            Return CBool(Me.ViewState("AllowAddItem"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowAddItem") = value
            Me.HYPaddItem.Visible = value
        End Set
    End Property
    Public Property SelectedItems() As List(Of System.Guid) Implements IWKitemsList.SelectedItems
        Get
            Dim oList As New List(Of System.Guid)
            For Each oRow As RepeaterItem In Me.RPTitemsDetails.Items
                Dim oCheck As HtmlInputCheckBox
                oCheck = oRow.FindControl("CBXselected")
                If Not IsNothing(oCheck) AndAlso oCheck.Visible AndAlso oCheck.Checked Then
                    oList.Add(New System.Guid(oCheck.Value))
                End If
            Next
            Return oList
        End Get
        Set(ByVal value As List(Of System.Guid))
            Dim TotalItems As Integer = value.Count
            For Each oRow As RepeaterItem In Me.RPTitemsDetails.Items
                Dim oCheck As HtmlInputCheckBox
                oCheck = oRow.FindControl("CBXselected")
                If Not IsNothing(oCheck) Then
                    If Not oCheck.Visible OrElse TotalItems = 0 OrElse Not value.Contains(New System.Guid(oCheck.Value)) Then
                        oCheck.Checked = False
                    Else
                        oCheck.Checked = True
                    End If
                End If
            Next
        End Set
    End Property
    Public Property WorkBookModuleID() As Integer Implements IWKitemsList.WorkBookModuleID
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
    Public Property WorkBookCommunityID() As Integer Implements IWKitemsList.WorkBookCommunityID
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

#Region "Inherits"
    Public ReadOnly Property ScormImage() As String
        Get
            Return Me.BaseUrl & "images/scorm/visualizza.png"
        End Get
    End Property
    Public ReadOnly Property VideoCastImage() As String
        Get
            Return Me.BaseUrl & "images/scorm/visualizza.png"
        End Get
    End Property
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

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookItemsList", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBorderby_t)
            .setLabel(Me.LBtitle)
            .setLinkButton(Me.LNBdeleteItems, True, True, , True)
            .setLinkButton(Me.LNBsaveStatus, True, True)
            .setHyperLink(Me.HYPaddItem, True, True)
            .setHyperLink(Me.HYPbackToItemsList, True, True)
            .setHyperLink(Me.HYPgoToWorkbooksList, True, True)
            .setHyperLink(Me.HYPprintItems, True, True)
            .setRadioButtonList(Me.RBLorderby, 0)
            .setRadioButtonList(Me.RBLorderby, 1)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Function GetItemsStatus() As List(Of dtoItemStatusEditing) Implements IWKitemsList.GetItemsStatusEditing
        Dim oList As New List(Of dtoItemStatusEditing)
        For Each oRow As RepeaterItem In Me.RPTitemsDetails.Items
            '       Dim oCheck As HtmlInputCheckBox
            Dim oLiteral As Literal = oRow.FindControl("LTitemID")
            Dim oDDLstatus, oDDLediting As DropDownList
            '   oCheck = oRow.FindControl("CBXselected")

            If Not IsNothing(oLiteral) Then
                oDDLstatus = oRow.FindControl("DDLstatus")
                oDDLediting = oRow.FindControl("DDLediting")

                'If Not IsNothing(oCheck) Then ' AndAlso oCheck.Checked Then
                Dim oDto As New dtoItemStatusEditing
                oDto.ItemId = New System.Guid(oLiteral.Text)
                If Not IsNothing(oDDLstatus) AndAlso oDDLstatus.SelectedIndex > -1 AndAlso oDDLstatus.Visible Then
                    oDto.StatusId = oDDLstatus.SelectedValue
                Else
                    oDto.StatusId = -1
                End If
                If Not IsNothing(oDDLediting) AndAlso oDDLediting.SelectedIndex > -1 Then
                    oDto.Editing = oDDLediting.SelectedValue
                Else
                    oDto.Editing = EditingPermission.None
                End If
                oList.Add(oDto)
            End If
        Next
        Return oList
    End Function

#Region "Notification / Action"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub
    'Public Sub SendActionAdd(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid) Implements IWKitemsList.SendActionAdd

    'End Sub
    Public Sub SendActionDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid) Implements IWKitemsList.SendActionDelete
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.DeleteItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, WorkBookItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    'Public Sub SendActionEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid) Implements IWKitemsList.SendActionEdit
    '    Me.PageUtility.AddAction(Services_WorkBook.ActionType.UndeleteItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, ItemID.ToString), InteractionType.UserWithLearningObject)
    'End Sub
    Public Sub SendActionList(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid) Implements IWKitemsList.SendActionList
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.ListWorkBookItems, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBook, WorkBookID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub SendActionVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid) Implements IWKitemsList.SendActionVirtualDelete
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.DeleteItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, WorkBookItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub SendActionVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid) Implements IWKitemsList.SendActionVirtualUnDelete
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.UndeleteItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, WorkBookItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub NotifyDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWKitemsList.NotifyDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemDelete(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub NotifyEdit(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWKitemsList.NotifyEdit
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemEdit(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub NotifyVirtualDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWKitemsList.NotifyVirtualDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemVirtualDelete(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub NotifyVirtualUnDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IWKitemsList.NotifyVirtualUnDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemVirtualUnDelete(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
#End Region

    Public Sub LoadItems(ByVal oList As List(Of dtoWorkBookItem)) Implements IWKitemsList.LoadItems
        Me.MLVitems.SetActiveView(Me.VIWitems)
        Me.RPTitemsDetails.DataSource = oList
        Me.RPTitemsDetails.DataBind()
    End Sub

    Private Sub RPTitemsDetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTitemsDetails.ItemCommand
        Dim ItemID As System.Guid
        Try
            ItemID = New System.Guid(e.CommandArgument.ToString)
            If e.CommandName = "edit" Then
                Me.PageUtility.RedirectToUrl("Generici/WorkBookItem.aspx?ItemID=" & ItemID.ToString & "&View=" & Me.PreviousWorkBookView.ToString)
            ElseIf e.CommandName = "confirmdelete" Then
                Me.CurrentPresenter.DeleteItem(ItemID, Me.PageUtility.BaseUserRepositoryPath)
            ElseIf e.CommandName = "virtualdelete" Then
                Me.CurrentPresenter.VirtualDeleteItem(ItemID)
            ElseIf e.CommandName = "undelete" Then
                Me.CurrentPresenter.VirtualUnDeleteItem(ItemID)
            ElseIf e.CommandName = "managementfiles" Then
                Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & Me.PreviousWorkBookView.ToString)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RPTitemsDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitemsDetails.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim odtoWorkBookItem As lm.Comol.Modules.Base.DomainModel.dtoWorkBookItem = TryCast(e.Item.DataItem, lm.Comol.Modules.Base.DomainModel.dtoWorkBookItem)
            If Not IsNothing(odtoWorkBookItem) Then
                Dim oDiv As HtmlControls.HtmlControl
                Dim oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem = odtoWorkBookItem.Item
                Dim oPermission As WorkBookItemPermission = odtoWorkBookItem.Permission


                Dim oLiteral As Literal
                oLiteral = e.Item.FindControl("LTitemHeader")
                If Not IsNothing(oLiteral) Then
                    If IsNothing(oItem.CreatedBy) Then
                        oLiteral.Text = ""
                    ElseIf IsNothing(oItem.ModifiedBy) Then
                        If oItem.CreatedOn.Equals(New Date) Then
                            oLiteral.Text = String.Format(Me.Resource.getValue("createdHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), " // ", " // ")
                        Else
                            oLiteral.Text = String.Format(Me.Resource.getValue("createdHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), GetDateToString(oItem.CreatedOn), GetTimeToString(oItem.CreatedOn))
                        End If
                    ElseIf oItem.isDeleted Then
                        oLiteral.Text = String.Format(Me.Resource.getValue("deletedHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), GetDateToString(oItem.ModifiedOn), GetTimeToString(oItem.ModifiedOn), GetDisplayName(oItem.ModifiedBy))
                    ElseIf oItem.ModifiedBy Is oItem.CreatedBy Then
                        If oItem.CreatedOn = oItem.ModifiedOn Then
                            oLiteral.Text = String.Format(Me.Resource.getValue("createdHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), GetDateToString(oItem.StartDate), GetTimeToString(oItem.CreatedOn))
                        Else
                            oLiteral.Text = String.Format(Me.Resource.getValue("selfchangedHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), GetDateToString(oItem.ModifiedOn), GetTimeToString(oItem.ModifiedOn))
                        End If
                    Else
                        oLiteral.Text = String.Format(Me.Resource.getValue("changedHeader"), GetDateToString(oItem.StartDate), GetDisplayName(oItem.CreatedBy), GetDateToString(oItem.ModifiedOn), GetTimeToString(oItem.ModifiedOn), GetDisplayName(oItem.ModifiedBy))
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
                Dim oLabel As Label

                oDiv = e.Item.FindControl("DIVnote")
                oLiteral = e.Item.FindControl("LTnote")
                oLabel = e.Item.FindControl("LBnote_t")
                If Not IsNothing(oDiv) AndAlso Not String.IsNullOrEmpty(oItem.Note) Then 'AndAlso Not IsNothing(oLiteral) AndAlso Not IsNothing(oLabel)
                    Me.Resource.setLabel(oLabel)
                    oLiteral.Text = oItem.Note ' IIf(oPermission.ViewPersonalNote, oItem.Note, "")
                    'If oItem.Note = "" Then

                    'Else
                    oDiv.Style("Display") = "block"
                    'End If
                Else
                    oDiv.Style("Display") = "none"
                End If

                oLabel = e.Item.FindControl("LBtitolo_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If
                oLabel = e.Item.FindControl("LBprogramma_t")
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
                            oRepeater.DataSource = Me.CurrentPresenter.GetItemFiles(oItem.Id, odtoWorkBookItem.Permission)
                            oRepeater.DataBind()

                            If oRepeater.Items.Count = 0 Then
                                oDiv.Style("Display") = "none"
                            End If
                        End If
                    End If
                End If

                Dim isItemEditable As Boolean = odtoWorkBookItem.Permission.Write
                Dim isItemDeletable As Boolean = odtoWorkBookItem.Permission.Delete


                Dim oHyperlink As HyperLink
                oHyperlink = e.Item.FindControl("HYPitemFiles")
                If Not IsNothing(oHyperlink) Then
                    oHyperlink.Visible = isItemEditable
                    oHyperlink.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemManagementFile.aspx?ItemID=" & oItem.Id.ToString & "&View=" & Me.PreviousWorkBookView.ToString
                    Me.Resource.setHyperLink(oHyperlink, True, True)
                End If

                oHyperlink = e.Item.FindControl("HYPedit")
                If Not IsNothing(oHyperlink) Then
                    oHyperlink.Visible = isItemEditable
                    oHyperlink.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItem.aspx?WorkbookID=" & oItem.WorkBookOwner.Id.ToString & "&ItemID=" & oItem.Id.ToString & "&View=" & Me.PreviousWorkBookView.ToString
                    Me.Resource.setHyperLink(oHyperlink, True, True)
                    oHyperlink.Text = String.Format(oHyperlink.Text, Me.BaseUrl & "images/m.gif", oHyperlink.ToolTip)
                End If

                Dim oImageButton As ImageButton
                oImageButton = e.Item.FindControl("IMGvirtualDelete")
                If Not IsNothing(oImageButton) Then
                    oImageButton.Visible = isItemDeletable AndAlso Not oItem.isDeleted
                    oImageButton.CommandName = "virtualdelete"
                    Me.Resource.setImageButton(oImageButton, False, True, True, True)
                    oImageButton.ImageUrl = Me.BaseUrl & "images/x.gif"
                End If

                oImageButton = e.Item.FindControl("IMGundelete")
                If Not IsNothing(oImageButton) Then
                    Me.Resource.setImageButton(oImageButton, False, True, True)
                    oImageButton.ImageUrl = Me.BaseUrl & "images/grid/ripristina.gif"
                    oImageButton.Visible = oItem.isDeleted
                End If

                oImageButton = e.Item.FindControl("IMGdelete")
                If Not IsNothing(oImageButton) Then
                    oImageButton.Visible = isItemDeletable AndAlso oItem.isDeleted
                    Me.Resource.setImageButton(oImageButton, False, True, True, True)
                    oImageButton.ImageUrl = Me.BaseUrl & "images/grid/eliminato1.gif"
                End If
                Dim oCheck As HtmlInputCheckBox
                oCheck = e.Item.FindControl("CBXselected")
                If Not IsNothing(oCheck) Then
                    oCheck.Visible = isItemDeletable AndAlso Me.AllowItemsSelection
                End If

                ' EDITING !


                oDiv = e.Item.FindControl("DIVadminPanel")
                Dim oLabelStatusItemTitle As Label = e.Item.FindControl("LBstatusItem_t")
                Dim oLabelStatusItem As Label = e.Item.FindControl("LBstatusItem")
                Dim oLabelEditingTitle As Label = e.Item.FindControl("LBediting_t")
                Dim oLabelEditing As Label = e.Item.FindControl("LBediting")
                Dim oLabelDraft As Label = e.Item.FindControl("LBdraft")
                Dim oDDLstatus As DropDownList = e.Item.FindControl("DDLstatus")
                Dim oDDLediting As DropDownList = e.Item.FindControl("DDLediting")

                If Not IsNothing(oDiv) Then
                    'If Not oPermission.ChangeApprovation OrElse oItem.MetaInfo.isDeleted Then
                    oLabelStatusItem.Visible = oItem.isDeleted
                    oDDLstatus.Visible = Not oItem.isDeleted
                    'oLabelEditing.Visible = Not oItem.MetaInfo.isDeleted
                    'oDDLediting.Visible = Not oItem.MetaInfo.isDeleted
                    'oLabelEditingTitle.Visible = Not oItem.MetaInfo.isDeleted
                    oLabelStatusItemTitle.Visible = True
                    Me.Resource.setLabel(oLabelEditingTitle)
                    Me.Resource.setLabel(oLabelStatusItemTitle)
                    Me.Resource.setLabel(oLabelDraft)

                    oLabelStatusItem.Text = odtoWorkBookItem.StatusTranslated
                    If oItem.ApprovedOn.HasValue AndAlso oItem.ApprovedOn <> oItem.CreatedOn Then
                        oLabelStatusItem.Text &= " - " & GetDisplayName(oItem.ApprovedBy) & " (" & GetDateTimeString(oItem.ApprovedOn) & "). "
                    End If

                    oLabelEditing.Visible = False
                    oDDLediting.Visible = False
                    oLabelEditingTitle.Visible = False

                    oDDLediting.DataSource = Me.AvailableEditing(oItem)
                    oDDLediting.DataTextField = "Translation"
                    oDDLediting.DataValueField = "Id"
                    oDDLediting.DataBind()

                    oDDLediting.SelectedValue = oItem.Editing

                    If oItem.Editing = EditingSettings.OnlyAuthor Then
                        If oItem.CreatedBy.Id = Me.CurrentContext.UserContext.CurrentUserID Then
                            oLabelEditing.Text = Me.GetEditingTranslationOwner(True, EditingSettings.OnlyAuthor)
                        Else
                            oLabelEditing.Text = Me.GetEditingTranslationOwner(False, EditingSettings.OnlyAuthor)
                            oLabelEditing.Text = String.Format(oLabelEditing.Text, GetDisplayName(oItem.CreatedBy))
                        End If
                    Else
                        oLabelEditing.Text = Me.GetEditingTranslation(oItem.Editing)
                    End If

                    If oItem.isDeleted Then
                        'oDiv.Style("Display") = "none"
                        oLabelDraft.Visible = oItem.isDraft
                        oLabelStatusItem.Visible = True
                        oLabelEditing.Visible = Not oItem.isDraft
                        oLabelEditingTitle.Visible = Not oItem.isDraft
                    Else
                        oDiv.Style("Display") = "block"
                        Me.Resource.setLabel(oLabelEditingTitle)
                        oDDLstatus.Items.Clear()
                        oDDLstatus.Items.AddRange((From s In Me.AvailableStatus Select New ListItem(s.Translation, s.Id)).ToArray())

                        oDDLstatus.SelectedValue = odtoWorkBookItem.StatusId
                        oDDLstatus.Visible = odtoWorkBookItem.Permission.ChangeApprovation 'AndAlso (oDDLstatus.SelectedValue = odtoWorkBookItem.StatusId)
                        oLabelStatusItem.Visible = Not odtoWorkBookItem.Permission.ChangeApprovation

                        oLabelDraft.Visible = oItem.isDraft
                        If oItem.isDraft Then
                            oLabelDraft.Visible = oItem.isDraft
                        Else
                            oLabelEditing.Visible = Not odtoWorkBookItem.Permission.ChangeEditing
                            oDDLediting.Visible = odtoWorkBookItem.Permission.ChangeEditing
                            oLabelEditingTitle.Visible = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub RPTitemFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oFileItem As dtoWorkBookFile = TryCast(e.Item.DataItem, dtoWorkBookFile)
            If Not IsNothing(oFileItem) Then
                Try
                    'Dim cssLink As String = "ROW_ItemLink_Small"
                    'Dim cssRiga As String = "ROW_TD_Small"
                    'Try
                    '    If oFileItem.FromItemDeleted Then
                    '        cssLink = "ROW_Disabilitate_Small"
                    '    Else
                    '        cssLink = "ROW_Normal_Small"
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    Dim oLBnomeFile, oLBdimensione As Label
                    Dim oHYPfile, oHYPdownload As HyperLink

                    oLBnomeFile = e.Item.FindControl("LBnomeFile")
                    oHYPdownload = e.Item.FindControl("HYPdownload")
                    oHYPfile = e.Item.FindControl("HYPfile")
                    oLBdimensione = e.Item.FindControl("LBdimensione")

                    Dim NomeFile As String = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oFileItem.Extension) & "'>&nbsp;" & oFileItem.Name
                    Dim quote As String = """"

                    oLBnomeFile.Text = NomeFile
                    oHYPfile.Text = NomeFile

                    Me.Resource.setHyperLink(oHYPdownload, True, True)
                    If oFileItem.isCommunityFile Then
                        'oHYPdownload.NavigateUrl = PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oFileItem.CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro)
                        'oHYPfile.NavigateUrl = PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oFileItem.CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro)

                        'oLBnomeFile.Visible = (oFileItem.Permission.Play AndAlso Not oFileItem.Permission.Download)
                        'oHYPfile.Visible = Not (oFileItem.Permission.Play AndAlso Not oFileItem.Permission.Download)
                        'oHYPdownload.Visible = False
                        oHYPdownload.NavigateUrl = "File.repository?FileID=" & oFileItem.CommunityFileID.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode & "&ModuleID=" & Me.WorkBookModuleID.ToString & "&ItemID=" & oFileItem.ItemOwner.ToString   'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro) 
                        oHYPfile.NavigateUrl = "File.repository?FileID=" & oFileItem.CommunityFileID.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & Me.LinguaCode & "&ModuleID=" & Me.WorkBookModuleID.ToString & "&ItemID=" & oFileItem.ItemOwner.ToString  'PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & oCommunityFile.Id, UtilityLibrary.SecretKeyUtil.EncType.Altro)

                        oLBnomeFile.Visible = Not oFileItem.Permission.Download ' (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        oHYPfile.Visible = oFileItem.Permission.Download 'Not (oDto.Permission.Play AndAlso Not oDto.Permission.Download)
                        oHYPdownload.Visible = False

                        If oFileItem.Permission.Play Then
                            Dim oHYPcontenutoAttivo As HyperLink
                            Dim oIMBcontenutoAttivo As ImageButton

                            oIMBcontenutoAttivo = e.Item.FindControl("IMBcontenutoAttivo")
                            oHYPcontenutoAttivo = e.Item.FindControl("HYPcontenutoAttivo")
                            oHYPcontenutoAttivo.Visible = True
                            oIMBcontenutoAttivo.Visible = True
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
                        End If
                    Else
                        oHYPfile.Visible = True
                        oHYPdownload.Visible = False
                        oLBnomeFile.Visible = False

                        oHYPfile.NavigateUrl = "File.repository?InternalFileID=" & oFileItem.InternalFileID.ToString ' Me.BaseUrl & "FileStore/" & oFileItem.InternalFileID.ToString & "/" & oFileItem.Name
                    End If
                    If oFileItem.Size = 0 Then
                        oLBdimensione.Text = "&nbsp;"
                    Else
                        Dim FileSize As Long = 0
                        If oFileItem.isCommunityFile Then
                            FileSize = oFileItem.Size
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
                        Else
                            FileSize = oFileItem.Size / 1024
                            If FileSize < 1024 Then
                                oLBdimensione.Text = " (" & Math.Round(FileSize) & " kb) "
                            ElseIf FileSize >= 1024 Then
                                oLBdimensione.Text = " (" & Math.Round(FileSize / 1024, 2) & " mb) "
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            End If
        End If

    End Sub
    Public Sub NoPermissionToViewItems() Implements IWKitemsList.NoPermissionToViewItems
        Me.BindNoPermessi()
    End Sub
    Public Sub NoWorkBookItemWithThisID() Implements IWKitemsList.NoWorkBookItemWithThisID
        Me.MLVitems.SetActiveView(Me.VIWerrors)
        LBerrors.Text = Me.Resource.getValue("NoWorkBookItemWithThisID")
    End Sub
    Public Sub NoWorkBookWithThisID() Implements IWKitemsList.NoWorkBookWithThisID
        Me.MLVitems.SetActiveView(Me.VIWerrors)
        LBerrors.Text = Me.Resource.getValue("NoWorkBookWithThisID")
    End Sub


#Region "Navigation"
    Public Sub ReturnToWorkBookManagement(ByVal oView As WorkBookTypeFilter) Implements IWKitemsList.ReturnToWorkBookManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookList.aspx?View=" & oView.ToString)
    End Sub
    Public Sub SetAddItemUrl(ByVal WorkBookID As System.Guid) Implements IWKitemsList.SetAddItemUrl
        Me.HYPaddItem.Visible = Not (WorkBookID = System.Guid.Empty)
        Me.HYPaddItem.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItem.aspx?WorkBookID=" & WorkBookID.ToString & "&Action=Add" & "&View=" & Me.PreviousWorkBookView.ToString
    End Sub
    Public Sub SetItemsListUrl(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IWKitemsList.SetItemsListUrl
        Me.HYPbackToItemsList.Visible = Not (WorkBookID = System.Guid.Empty)
        Me.HYPbackToItemsList.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & Me.PreviousWorkBookView.ToString
    End Sub
    Public Sub SetPrintItemUrl(ByVal WorkBookID As System.Guid, ByVal Ascending As Boolean) Implements IWKitemsList.SetPrintItemUrl
        Me.HYPprintItems.Visible = Not (WorkBookID = System.Guid.Empty)
        Me.HYPprintItems.NavigateUrl = Me.BaseUrl & "Generici/PrintWorkBook.aspx?WorkBookID=" & WorkBookID.ToString & "&Ascending=" & Ascending.ToString
    End Sub
    Public Sub SetWorkBookManagementItemUrl(ByVal oView As WorkBookTypeFilter) Implements IWKitemsList.SetWorkBookManagementItemUrl
        Me.HYPgoToWorkbooksList.Visible = True
        Me.HYPgoToWorkbooksList.NavigateUrl = Me.BaseUrl & "Generici/WorkBookList.aspx?View=" & Me.PreviousWorkBookView.ToString
    End Sub
#End Region

    Private Sub LNBsaveStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveStatus.Click
        Me.CurrentPresenter.SaveItemsStatus()
    End Sub

    Private Sub LNBdeleteItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeleteItems.Click
        Me.CurrentPresenter.DeleteItems(Me.PageUtility.BaseUserRepositoryPath)
    End Sub

    Private Sub RBLorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLorderby.SelectedIndexChanged
        Me.CurrentPresenter.ChangeOrderBy()
    End Sub

    Public ReadOnly Property PreloadedAscending() As Boolean Implements lm.Comol.Modules.Base.Presentation.IWKitemsList.PreloadedAscending
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
    Public ReadOnly Property GetEditingTranslation(ByVal Permissions As Integer) As String Implements IWKitemsList.GetEditingTranslation
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString)
        End Get
    End Property
    Public ReadOnly Property GetEditingTranslationOwner(ByVal isOwner As Boolean, ByVal Permissions As Integer) As String Implements IWKitemsList.GetEditingTranslationOwner
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString & "." & isOwner.ToString)
        End Get
    End Property
    Public Sub SetRedirectToItemList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter, ByVal Ascending As Boolean) Implements IWKitemsList.SetRedirectToItemList
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString & "&Ascending=" & Ascending.ToString)
    End Sub


    Private Function GetDateTimeString(ByVal datetime As DateTime?)
        If datetime.HasValue Then
            Return GetDateToString(datetime) & " " & GetTimeToString(datetime)
        Else
            Return "//"
        End If
    End Function
    Private Function GetDateToString(ByVal datetime As DateTime?)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return "//"
        End If
    End Function
    Private Function GetTimeToString(ByVal datetime As DateTime?)
        If datetime.HasValue Then
            Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
        Else
            Return "//"
        End If
    End Function
    Private Function GetDisplayName(ByVal person As Person)
        If IsNothing(person) Then
            Return ""
        Else
            Return person.SurnameAndName
        End If
    End Function
End Class