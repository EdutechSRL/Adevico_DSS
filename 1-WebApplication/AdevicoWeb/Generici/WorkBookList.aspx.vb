Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Partial Public Class WorkBookList
    Inherits PageBase
    Implements IWKworkBookList


#Region "View Property"
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _Presenter As WKworkbookListPresenter
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
    Public ReadOnly Property AvailableEditing(ByVal oItemEditing As EditingPermission) As List(Of TranslatedItem(Of Integer))
        Get
            Dim oEditingAvailables As List(Of TranslatedItem(Of Integer))
            If IsNothing(_AvailableEditing) Then
                _AvailableEditing = Me.CurrentPresenter.GetEditingValues
            End If

            oEditingAvailables = (From o In _AvailableEditing Where ((o.Id And oItemEditing) > 0) Select o).ToList
            If (From o In oEditingAvailables Select o.Id = oItemEditing).Count = 0 Then
                oEditingAvailables.Add(New TranslatedItem(Of Integer) With {.Id = oItemEditing, .Translation = Me.GetEditingTranslation(oItemEditing)})
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
    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IWKworkBookList.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As WKworkbookListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WKworkbookListPresenter(Me.CurrentContext, Me)
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

    Public WriteOnly Property AllowCreateWorkBook() As IWKworkBookList.AllowCreation Implements IWKworkBookList.AllowCreateWorkBook
        Set(ByVal value As IWKworkBookList.AllowCreation)
            Me.HYPaddWorkBook.Visible = Not (value = IWKworkBookList.AllowCreation.None)
            If Not (value = IWKworkBookList.AllowCreation.None) Then
                Me.HYPaddWorkBook.NavigateUrl = Me.BaseUrl & "Generici/WorkBookAdd.aspx?View=" & Me.CurrentView.ToString & IIf(value = IWKworkBookList.AllowCreation.Current, "", "&CreateForOther=true")
            End If
        End Set
    End Property

    Public Property AllowChangeStatusEditing() As Boolean Implements IWKworkBookList.AllowChangeStatusEditing
        Get
            Return Me.LNBsaveStatus.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.LNBsaveStatus.Visible = value
        End Set
    End Property
    Public Property CurrentCommunityFilter() As lm.Comol.Modules.Base.DomainModel.WorkBookCommunityFilter Implements IWKworkBookList.CurrentCommunityFilter
        Get
            Return Me.DDLfilterBy.SelectedValue
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.DomainModel.WorkBookCommunityFilter)
            Try
                Me.DDLfilterBy.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property CurrentOrder() As lm.Comol.Modules.Base.DomainModel.WorkBookOrder Implements IWKworkBookList.CurrentOrder
        Get
            If Me.RBLorderBy.SelectedIndex > -1 Then
                Return Me.RBLorderBy.SelectedValue
            Else
                Return WorkBookOrder.ChangedOn
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.DomainModel.WorkBookOrder)
            Try
                Me.RBLorderBy.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase Implements IWKworkBookList.CurrentPager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Public Property CurrentPageSize() As Integer Implements IWKworkBookList.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(Me.DDLpage.Items.FindByValue(value)) Then
                Me.DDLpage.SelectedValue = value
            End If
        End Set
    End Property
    Public Property CurrentView() As WorkBookTypeFilter Implements IWKworkBookList.CurrentView
        Get
            If Me.TBSdiario.SelectedTab Is Nothing Then
                Return WorkBookTypeFilter.None
            Else
                Return Me.TBSdiario.SelectedTab.Value
            End If
        End Get
        Set(ByVal value As WorkBookTypeFilter)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSdiario.FindTabByValue(value, True)
            If Not IsNothing(oTab) Then
                Me.TBSdiario.SelectedIndex = oTab.Index
            End If
        End Set
    End Property
    Public ReadOnly Property PreLoadedCommunityFilter() As WorkBookCommunityFilter Implements IWKworkBookList.PreLoadedCommunityFilter
        Get
            If IsNothing(Request.QueryString("CommunityFilter")) Then
                Return WorkBookCommunityFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookCommunityFilter).GetByString(Request.QueryString("CommunityFilter"), WorkBookCommunityFilter.None)
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedOrder() As WorkBookOrder Implements IWKworkBookList.PreLoadedOrder
        Get
            If IsNothing(Request.QueryString("Order")) Then
                Return WorkBookOrder.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookOrder).GetByString(Request.QueryString("Order"), WorkBookOrder.None)
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedPageIndex() As Integer Implements IWKworkBookList.PreLoadedPageIndex
        Get
            If IsNumeric(Request.QueryString("Page")) Then
                Return CInt(Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IWKworkBookList.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedView() As WorkBookTypeFilter Implements IWKworkBookList.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer Implements IWKworkBookList.DefaultPageSize
        Get
            Return 25
        End Get
    End Property

    Public ReadOnly Property GetAllCommunitiesName() As String Implements IWKworkBookList.GetAllCommunitiesName
        Get
            Return Me.Resource.getValue("GetAllCommunitiesName")
        End Get
    End Property

    Public ReadOnly Property GetPortalName() As String Implements IWKworkBookList.GetPortalName
        Get
            Return Me.Resource.getValue("GetPortalName")
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Function BackGroundItem(ByVal oItem As NEWdtoWorkbooks) As String
        If oItem.isDeleted Then
            Return "ROW_Disabilitate_Small"
        Else
            Return ""
        End If
    End Function
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.CurrentPager
    End Sub


#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
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
        MyBase.SetCulture("pg_WorkBooksList", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBfilterBy)
            .setLabel(Me.LBorderBy)
            Dim TextValue As String = ""
            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSdiario.Tabs
                TextValue = Me.Resource.getValue("WorkBookTypeFilter." & oTab.Value)
                If TextValue <> "" Then
                    oTab.Text = TextValue
                    oTab.ToolTip = Me.Resource.getValue("WorkBookTypeFilter." & oTab.Value)
                End If
            Next
            For Each oListItem As ListItem In Me.RBLorderBy.Items
                .setRadioButtonList(Me.RBLorderBy, oListItem.Value)
            Next
            For Each oListItem As ListItem In Me.DDLfilterBy.Items
                .setDropDownList(Me.DDLfilterBy, oListItem.Value)
            Next
            .setLabel(Me.LBpagesize)
            .setLinkButton(LNBsaveStatus, True, True)
            .setHyperLink(HYPaddWorkBook, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub
#End Region


    Public Sub LoadAvailableFilters(ByVal oList As List(Of WorkBookCommunityFilter)) Implements IWKworkBookList.LoadAvailableFilters
        Dim oTranslated As New List(Of TranslatedItem(Of Integer))

        For Each oFilter As WorkBookCommunityFilter In oList
            oTranslated.Add(New TranslatedItem(Of Integer) With {.Id = CInt(oFilter), .Translation = Me.Resource.getValue("DDLfilterBy." & CInt(oFilter))})
        Next

        Me.DDLfilterBy.DataSource = oTranslated.OrderBy(Function(t) t.Translation).ToList
        Me.DDLfilterBy.DataTextField = "Translation"
        Me.DDLfilterBy.DataValueField = "Id"
        Me.DDLfilterBy.DataBind()

    End Sub

    Public Sub LoadAvailableOrderBy(ByVal oList As List(Of WorkBookOrder)) Implements IWKworkBookList.LoadAvailableOrderBy
        Dim oTranslated As New List(Of TranslatedItem(Of Integer))

        For Each oFilter As WorkBookOrder In oList
            oTranslated.Add(New TranslatedItem(Of Integer) With {.Id = CInt(oFilter), .Translation = Me.Resource.getValue("RBLorderBy." & CInt(oFilter))})
        Next

        Me.RBLorderBy.DataSource = oTranslated.OrderBy(Function(t) t.Translation).ToList
        Me.RBLorderBy.DataTextField = "Translation"
        Me.RBLorderBy.DataValueField = "Id"
        Me.RBLorderBy.DataBind()
    End Sub

    Public Sub LoadWorkBooks(ByVal oList As List(Of dtoWorkbooks)) Implements IWKworkBookList.LoadWorkBooks
        Me.RPTcommunities.DataSource = oList
        Me.RPTcommunities.DataBind()
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.List, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Public Sub LoadAvailableView(ByVal oList As List(Of dtoWorkBookListView)) Implements IWKworkBookList.LoadAvailableView

        For Each oTab As Telerik.Web.UI.RadTab In Me.TBSdiario.Tabs
            oTab.Visible = (From o In oList Select o.TypeFilter).ToList.Contains(oTab.Value)
            If oTab.Visible Then
                oTab.NavigateUrl = Me.GetBaseListUrl((From o In oList Where o.TypeFilter = oTab.Value Select o.Context).FirstOrDefault, True)
            End If
        Next
    End Sub

#Region "Notification / Action"
    Public Sub NoPermissionToAccessPage() Implements IWKworkBookList.NoPermissionToAccessPage
        Me.BindNoPermessi()
    End Sub

    Public Sub NotifyCommunityVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyCommunityVirtualDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyCommunityWorkBookVirtualDelete(CommunityID, WorkBookID, Name, CreatorName, Authors)
    End Sub

    Public Sub NotifyCommunityVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyCommunityVirtualUnDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyCommunityWorkBookVirtualUnDelete(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.CurrentView.ToString)
    End Sub

    Public Sub NotifyPersonalVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyPersonalVirtualDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyPersonalWorkBookVirtualDelete(CommunityID, WorkBookID, Name, CreatorName, Authors)
    End Sub

    Public Sub NotifyPersonalVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyPersonalVirtualUnDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyPersonalWorkBookVirtualUnDelete(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.CurrentView.ToString)
    End Sub

    Public Sub NotifyCommunityDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyCommunityDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyCommunityWorkBookDelete(CommunityID, WorkBookID, Name, CreatorName, Authors)
    End Sub

    Public Sub NotifyPersonalDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer)) Implements IWKworkBookList.NotifyPersonalDelete
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyPersonalWorkBookDelete(CommunityID, WorkBookID, Name, CreatorName, Authors)
    End Sub
#End Region

    Public ReadOnly Property GetEditingTranslation(ByVal Permissions As Integer) As String Implements IWKworkBookList.GetEditingTranslation
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString)
        End Get
    End Property


    Private Sub RPTcommunities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoWorkbooks = TryCast(e.Item.DataItem, dtoWorkbooks)
            If Not IsNothing(oDto) Then
                Dim oDiv As HtmlControls.HtmlControl

                Dim oLiteral As Literal
                oLiteral = e.Item.FindControl("LTcommunityName")

                If Not IsNothing(oLiteral) Then
                    If oDto.CommunityID > 0 Then
                        oLiteral.Text = oDto.CommunityName
                    ElseIf oDto.CommunityID = 0 AndAlso oDto.isPortal Then
                        oLiteral.Text = Me.Resource.getValue("GetPortalName")
                    Else
                        oLiteral.Text = Me.Resource.getValue("GetAllCommunitiesName")
                    End If
                End If

                Dim oRepeater As System.Web.UI.WebControls.Repeater = e.Item.FindControl("RPTworkBooks")
                If Not IsNothing(oRepeater) Then
                    AddHandler oRepeater.ItemDataBound, AddressOf RPTworkBooks_ItemDataBound
                    AddHandler oRepeater.ItemCommand, AddressOf RPTworkBooks_ItemCommand
                    oRepeater.DataSource = oDto.Items
                    oRepeater.DataBind()
                End If

            End If
        End If
    End Sub
    Protected Friend Sub RPTworkBooks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label
            oLabel = e.Item.FindControl("LBaction")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBedit")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBworkbook")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBlastEdit_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoWorkbook As NEWdtoWorkbooks = TryCast(e.Item.DataItem, NEWdtoWorkbooks)
            If Not IsNothing(oDtoWorkbook) Then
                Dim oDiv As HtmlControls.HtmlControl
                Dim oPermission As WorkBookPermission = oDtoWorkbook.Permission

                Dim isEditable As Boolean = oPermission.EditWorkBook
                Dim isDeletable As Boolean = oPermission.DeleteWorkBook

                Dim oLNBvirtualDelete, oLNBundelete, oLNBdelete As LinkButton
                oLNBvirtualDelete = e.Item.FindControl("LNBvirtualDelete")
                oLNBundelete = e.Item.FindControl("LNBundelete")
                oLNBdelete = e.Item.FindControl("LNBdelete")

                If Not IsNothing(oLNBvirtualDelete) Then
                    oLNBvirtualDelete.Visible = isEditable AndAlso Not oDtoWorkbook.isDeleted
                    '  oLNBvirtualDelete.CommandName = "virtualdelete"
                    Me.Resource.setLinkButton(oLNBvirtualDelete, True, True, , True)
                    oLNBvirtualDelete.Text = String.Format(oLNBvirtualDelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBvirtualDelete.ToolTip)
                End If

                If Not IsNothing(oLNBundelete) Then
                    Me.Resource.setLinkButton(oLNBundelete, True, True)
                    oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLNBundelete.ToolTip)
                    oLNBundelete.Visible = oDtoWorkbook.isDeleted AndAlso oPermission.UndeleteWorkBook
                End If

                If Not IsNothing(oLNBdelete) Then
                    oLNBdelete.Visible = isDeletable AndAlso oDtoWorkbook.isDeleted
                    Me.Resource.setLinkButton(oLNBdelete, True, True, , True)
                    oLNBdelete.Text = String.Format(oLNBdelete.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLNBdelete.ToolTip)
                End If
                oLNBundelete.CommandArgument = oDtoWorkbook.ID.ToString
                oLNBvirtualDelete.CommandArgument = oLNBundelete.CommandArgument
                oLNBdelete.CommandArgument = oLNBundelete.CommandArgument

                Dim oHYPedit As HyperLink = e.Item.FindControl("HYPedit")
                If Not IsNothing(oHYPedit) Then
                    Me.Resource.setHyperLink(oHYPedit, True, True)
                    oHYPedit.Text = String.Format(oHYPedit.Text, Me.BaseUrl & "Images/grid/modifica.gif", oHYPedit.ToolTip)
                    oHYPedit.Visible = isEditable AndAlso oDtoWorkbook.isDeleted = False
                    oHYPedit.NavigateUrl = Me.BaseUrl & "generici/WorkBookEdit.aspx?WorkBookID=" & oDtoWorkbook.ID.ToString & "&View=" & Me.CurrentView.ToString
                End If

                ' Dati workbook
                Dim oLiteral As Literal = e.Item.FindControl("LTworkbookID")
                oLiteral.Text = oDtoWorkbook.ID.ToString
                oLiteral = e.Item.FindControl("LTtitle")

                Dim oHYPtitle As HyperLink = e.Item.FindControl("HYPtitle")

                oHYPtitle.Visible = False
                oLiteral.Visible = False
                Dim Title As String = ""
                If oPermission.AddItems OrElse oPermission.ReadWorkBook OrElse oPermission.ChangeApprovation Then
                    oHYPtitle.Visible = True
                    oHYPtitle.Text = oDtoWorkbook.Title
                    oHYPtitle.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & oDtoWorkbook.ID.ToString & "&View=" & Me.CurrentView.ToString
                Else
                    oLiteral.Text = oDtoWorkbook.Title
                    oLiteral.Visible = True
                End If

                Dim oLabel As Label
                oLabel = e.Item.FindControl("LBlastEdit")
                If Not IsNothing(oLabel) Then
                    Dim DefaultTime As String = "<span title='{0}'>{1}</span>"
                    Dim Modified As String = ""
                    Dim TimeTitle As String = ""
                    If IsNothing(oDtoWorkbook.ModifiedBy) Then
                        Modified = String.Format(Me.Resource.getValue("modifiedon"), oDtoWorkbook.CreatedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.CreatedBy.SurnameAndName)
                    Else
                        Modified = String.Format(Me.Resource.getValue("modifiedon"), oDtoWorkbook.ModifiedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.ModifiedBy.SurnameAndName)
                    End If
                    If IsNothing(oDtoWorkbook.ModifiedBy) Then
                        TimeTitle = String.Format(Me.Resource.getValue("createdHeader"), oDtoWorkbook.CreatedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.CreatedOn.Value.ToString("hh:ss"), oDtoWorkbook.CreatedBy.SurnameAndName)
                    ElseIf oDtoWorkbook.isDeleted Then
                        TimeTitle = String.Format(Me.Resource.getValue("deletedHeader"), oDtoWorkbook.ModifiedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.ModifiedOn.Value.ToString("hh:ss"), oDtoWorkbook.ModifiedBy.SurnameAndName)
                    ElseIf oDtoWorkbook.ModifiedBy Is oDtoWorkbook.CreatedBy Then
                        If oDtoWorkbook.CreatedOn = oDtoWorkbook.ModifiedOn Then
                            TimeTitle = String.Format(Me.Resource.getValue("createdHeader"), oDtoWorkbook.CreatedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.CreatedOn.Value.ToString("hh:ss"), oDtoWorkbook.CreatedBy.SurnameAndName)
                        Else
                            TimeTitle = String.Format(Me.Resource.getValue("selfchangedHeader"), oDtoWorkbook.ModifiedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.ModifiedOn.Value.ToString("hh:ss"), oDtoWorkbook.ModifiedBy.SurnameAndName)
                        End If
                    Else
                        TimeTitle = String.Format(Me.Resource.getValue("changedHeader"), oDtoWorkbook.ModifiedOn.Value.ToString("dd/MM/yy"), oDtoWorkbook.ModifiedOn.Value.ToString("hh:ss"), oDtoWorkbook.ModifiedBy.SurnameAndName)
                    End If

                    oLabel.Text = String.Format(DefaultTime, TimeTitle, Modified)
                End If

                oLabel = e.Item.FindControl("LBauthors")
                If Not IsNothing(oLabel) Then
                    Dim Autori As String = ""
                    For Each oAuthor As Person In (From o In oDtoWorkbook.Authors Order By o.SurnameAndName Select o).ToList
                        Autori &= ", " & oAuthor.SurnameAndName
                    Next
                    oLabel.Text = String.Format(Me.Resource.getValue("Autori"), Autori, "<span title='" & Me.Resource.getValue("Owner") & "'>" & oDtoWorkbook.Owner.SurnameAndName & "</span>", Autori)
                End If

                oLabel = e.Item.FindControl("LBtitle_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
                End If

                oDiv = e.Item.FindControl("DVcommunity")
                oDiv.Visible = Not (Me.CurrentOrder = WorkBookOrder.Community)
                If Me.CurrentOrder <> WorkBookOrder.Community Then
                    oLabel = e.Item.FindControl("LBcommunityOwner_t")
                    If Not IsNothing(oLabel) Then
                        Me.Resource.setLabel(oLabel)
                    End If
                    oLabel = e.Item.FindControl("LBcommunityOwner")
                    oLabel.Text = oDtoWorkbook.CommunityName
                End If

                oLabel = e.Item.FindControl("LBauthors_t")
                If Not IsNothing(oLabel) Then
                    Me.Resource.setLabel(oLabel)
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
                    oLabelStatusItem.Visible = oDtoWorkbook.isDeleted
                    oDDLstatus.Visible = Not oDtoWorkbook.isDeleted
                    'oLabelEditing.Visible = Not oItem.MetaInfo.isDeleted
                    'oDDLediting.Visible = Not oItem.MetaInfo.isDeleted
                    'oLabelEditingTitle.Visible = Not oItem.MetaInfo.isDeleted
                    oLabelStatusItemTitle.Visible = True
                    Me.Resource.setLabel(oLabelEditingTitle)
                    Me.Resource.setLabel(oLabelStatusItemTitle)
                    Me.Resource.setLabel(oLabelDraft)

                    oLabelStatusItem.Text = oDtoWorkbook.StatusTranslated
                    If oDtoWorkbook.ApprovedOn.HasValue AndAlso oDtoWorkbook.ApprovedOn <> oDtoWorkbook.CreatedOn Then
                        oLabelStatusItem.Text &= " - " & oDtoWorkbook.ApprovedBy.SurnameAndName & " (" & CDate(oDtoWorkbook.ApprovedOn).ToString("dd/MM/yy - hh:mm") & "). "
                    End If

                    oLabelEditing.Visible = False
                    oDDLediting.Visible = False
                    oLabelEditingTitle.Visible = False
                    oDDLediting.DataSource = (From oEditing In oDtoWorkbook.AvailableEditingValues Select New TranslatedItem(Of Integer) With {.Id = CInt(oEditing), .Translation = GetEditingTranslation(oEditing)}).ToList
                    oDDLediting.DataTextField = "Translation"
                    oDDLediting.DataValueField = "Id"
                    oDDLediting.DataBind()

                    oDDLediting.SelectedValue = oDtoWorkbook.Editing
                    oLabelEditing.Text = GetEditingTranslation(oDtoWorkbook.Editing)


                    If oDtoWorkbook.isDeleted Then
                        'oDiv.Style("Display") = "none"
                        oLabelDraft.Visible = oDtoWorkbook.isDraft
                        oLabelStatusItem.Visible = True
                        oLabelEditing.Visible = Not oDtoWorkbook.isDraft
                        oLabelEditingTitle.Visible = Not oDtoWorkbook.isDraft
                    Else
                        oDiv.Style("Display") = "block"
                        Me.Resource.setLabel(oLabelEditingTitle)
                        oDDLstatus.Items.Clear()
                        oDDLstatus.DataSource = oDtoWorkbook.AvailableStatusValues
                        oDDLstatus.DataTextField = "Translation"
                        oDDLstatus.DataValueField = "Id"
                        oDDLstatus.DataBind()

                        oDDLstatus.SelectedValue = oDtoWorkbook.StatusId
                        oDDLstatus.Visible = oDtoWorkbook.Permission.ChangeApprovation 'AndAlso (oDDLstatus.SelectedValue = odtoWorkBookItem.StatusId)
                        oLabelStatusItem.Visible = Not oDtoWorkbook.Permission.ChangeApprovation

                        oLabelDraft.Visible = oDtoWorkbook.isDraft
                        If oDtoWorkbook.isDraft Then
                            oLabelDraft.Visible = oDtoWorkbook.isDraft
                        Else
                            oLabelEditing.Visible = Not oDtoWorkbook.Permission.ChangeEditing
                            oDDLediting.Visible = oDtoWorkbook.Permission.ChangeEditing
                            oLabelEditingTitle.Visible = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub RPTworkBooks_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim WorkBookID As System.Guid
        Dim Reload As Boolean = True
        Try
            WorkBookID = New System.Guid(e.CommandArgument.ToString)
            If e.CommandName = "confirmDelete" Then
                Me.CurrentPresenter.DeleteWorkBook(WorkBookID, Me.PageUtility.BaseUserRepositoryPath)
            ElseIf e.CommandName = "virtualdelete" Then
                Me.CurrentPresenter.VirtualDeleteWorkBook(WorkBookID)
            ElseIf e.CommandName = "undelete" Then
                Me.CurrentPresenter.UnDeleteWorkBook(WorkBookID)
            Else
                Me.CurrentPresenter.LoadWorkbooks()
            End If
        Catch ex As Exception
            Me.CurrentPresenter.LoadWorkbooks()
        End Try
    End Sub
    Public Sub ShowErrorView() Implements IWKworkBookList.ShowErrorView
        Me.MLVworkbooks.SetActiveView(VIWerrors)
    End Sub

    Public Sub ShowListView() Implements IWKworkBookList.ShowListView
        Me.MLVworkbooks.SetActiveView(VIWlist)
    End Sub

    Public Sub NavigationUrl(ByVal oContext As WorkBookContext) Implements IWKworkBookList.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseListUrl(oContext) & "&Page={0}"
    End Sub
    Private Function GetBaseListUrl(ByVal oContext As WorkBookContext, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"
        If oContext.View <> WorkBookTypeFilter.None Then
            url &= "&View=" & oContext.View.ToString
        End If
        If oContext.Order <> WorkBookOrder.None Then
            url &= "&Order=" & oContext.Order.ToString
        End If
        If oContext.CommunityFilter <> WorkBookCommunityFilter.None Then
            url &= "&CommunityFilter=" & oContext.CommunityFilter.ToString
        End If
        url &= "&PageSize=" & oContext.PageSize

        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        url = "Generici/WorkBookList.aspx" & url
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Private Sub DDLfilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfilterBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadWorkbooks()
    End Sub

    Private Sub DDLpage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.LoadWorkbooks()
    End Sub

    Private Sub RBLorderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLorderBy.SelectedIndexChanged
        Me.CurrentPresenter.LoadWorkbooks()
    End Sub

    Public Function GetItemsStatusEditing() As List(Of lm.Comol.Modules.Base.DomainModel.dtoItemStatusEditing) Implements lm.Comol.Modules.Base.Presentation.IWKworkBookList.GetItemsStatusEditing
        Dim oList As New List(Of dtoItemStatusEditing)

        For Each oCommunityRow As RepeaterItem In Me.RPTcommunities.Items
            Dim oRepeater As Repeater = oCommunityRow.FindControl("RPTworkBooks")
            If Not IsNothing(oRepeater) Then

                For Each oRow As RepeaterItem In oRepeater.Items
                    Dim oLiteral As Literal = oRow.FindControl("LTworkbookID")
                    Dim oDDLstatus, oDDLediting As DropDownList

                    If Not IsNothing(oLiteral) Then
                        oDDLstatus = oRow.FindControl("DDLstatus")
                        oDDLediting = oRow.FindControl("DDLediting")

                        'If Not IsNothing(oCheck) Then ' AndAlso oCheck.Checked Then
                        Dim oDto As New dtoItemStatusEditing
                        oDto.ItemId = New System.Guid(oLiteral.Text)
                        If Not IsNothing(oDDLstatus) AndAlso oDDLstatus.SelectedIndex > -1 Then
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
            End If
        Next
        Return oList
    End Function
    Private Sub LNBsaveStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveStatus.Click
        Me.CurrentPresenter.UpdateWorkbooks()
    End Sub
End Class