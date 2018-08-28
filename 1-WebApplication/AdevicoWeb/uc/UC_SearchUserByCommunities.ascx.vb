Partial Public Class UC_SearchUserByCommunities
    Inherits BaseControlSession
    Implements IviewSearchUser

#Region "TEMP"
    Private _ProfileService As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
    Private ReadOnly Property ProfileService() As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
        Get
            If IsNothing(_ProfileService) Then
                _ProfileService = New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(PageUtility.CurrentContext)
            End If
            Return _ProfileService
        End Get
    End Property

#End Region
    Public Event AjaxEventUpdate()

    Private _Presenter As PresenterSearchUserByCommunities
    Private Property _SelectedCommunitiesId() As List(Of Integer)
        Get
            If ViewState("_SelectedCommunitiesId") Is Nothing Then
                Return New List(Of Integer)
            Else
                Return ViewState("_SelectedCommunitiesId")
            End If
        End Get
        Set(ByVal value As List(Of Integer))
            ViewState("_SelectedCommunitiesId") = value
        End Set
    End Property
    Private Property _oRoleList() As List(Of Role)
        Get
            If ViewState("oRoleList") Is Nothing Then
                Return New List(Of Role)
            Else
                Return ViewState("oRoleList")
            End If
        End Get
        Set(ByVal value As List(Of Role))
            ViewState("oRoleList") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindDati()
    End Sub

    Private ReadOnly Property CurrentLanguage() As Comol.Entity.Lingua Implements PresentationLayer.IviewSearchUser.CurrentLanguage
        Get
            Return PageUtility.UserSessionLanguage
        End Get
    End Property

    Protected Friend ReadOnly Property CurrentPresenter() As PresentationLayer.PresenterSearchUserByCommunities Implements PresentationLayer.IviewSearchUser.CurrentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PresenterSearchUserByCommunities(Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property AjaxEnabled() As Boolean
        Get
            If TypeOf Me.ViewState("AjaxEnabled") Is Boolean Then
                Return Me.ViewState("AjaxEnabled")
            Else
                Me.ViewState("AjaxEnabled") = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxEnabled") = value
        End Set
    End Property
#Region "Grid Property"
    Private Property Direction() As Comol.Entity.sortDirection Implements PresentationLayer.IviewSearchUser.Direction
        Get
            Try
                Return DirectCast(Me.ViewState("Direction"), Comol.Entity.sortDirection)
            Catch ex As Exception
                Return Comol.Entity.sortDirection.Ascending
            End Try
        End Get
        Set(ByVal value As Comol.Entity.sortDirection)
            Me.ViewState("Direction") = value
        End Set
    End Property
    Private Property OrderBy() As String Implements PresentationLayer.IviewSearchUser.OrderBy
        Get
            Return Me.ViewState("SortExpression")
        End Get
        Set(ByVal value As String)
            Me.ViewState("SortExpression") = value
        End Set
    End Property
    Private Property ShowMail() As Boolean Implements PresentationLayer.IviewSearchUser.showMail
        Get
            Return Me.ViewState("ShowMail")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowMail") = value
        End Set
    End Property
    Private Property GridCurrentPage() As Integer Implements PresentationLayer.IviewSearchUser.GridCurrentPage
        Get
            Return Me.ViewState("GridCurrentPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("GridCurrentPage") = value
            Me.GRVuser.PageIndex = value - 1
        End Set
    End Property
    Private Property GridMaxPage() As Integer Implements PresentationLayer.IviewSearchUser.GridMaxPage
        Get
            Return Me.ViewState("GridMaxPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("GridMaxPage") = value
        End Set
    End Property
    Private Property GridPageSize() As Integer Implements PresentationLayer.IviewSearchUser.GridPageSize
        Get
            Return Me.GRVuser.PageSize
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                Me.GRVuser.AllowPaging = False
            Else
                Me.GRVuser.AllowPaging = True
                Me.GRVuser.PageSize = value
            End If
        End Set
    End Property
#End Region

    Protected Sub LNBuser_OnClientClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim userId As Integer
        Dim LNBuser As New LinkButton
        LNBuser = DirectCast(sender, LinkButton)
        userId = Integer.Parse(LNBuser.CommandArgument)
        CurrentPresenter.RemoveUserFromPreview(userId)
        'CurrentPresenter.BindPreview()
    End Sub
    Protected Sub IMBuser_OnClientClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim userId As Integer
        Dim IMBuser As New ImageButton
        IMBuser = DirectCast(sender, ImageButton)
        userId = Integer.Parse(IMBuser.CommandArgument)
        CurrentPresenter.RemoveUserFromPreview(userId)
        'CurrentPresenter.BindPreview()
    End Sub
    Private Sub BindPreview(ByRef oUserList As System.Collections.Generic.List(Of Comol.Entity.MemberContact)) Implements PresentationLayer.IviewSearchUser.BindPreview
        If oUserList.Count > 0 Then
            RPTuserListPreview.DataSource = oUserList
            RPTuserListPreview.DataBind()
            LBnoRecordPreview.Visible = False
            RPTuserListPreview.Visible = True
        Else
            LBnoRecordPreview.Visible = True
            RPTuserListPreview.Visible = False
        End If
        If CBshowPreview.Checked And Not LBnoRecordPreview.Visible Then
            DIVpreview.Style("display") = "block"
        Else
            DIVpreview.Style("display") = "none"
        End If
        If Me.SelectionMode = ListSelectionMode.Multiple Then
            Me.CurrentPresenter.Search(False)
        End If
    End Sub

    Private Sub BindRolesByCommunities(ByRef oRoleList As System.Collections.Generic.List(Of Comol.Entity.Role)) Implements PresentationLayer.IviewSearchUser.BindRolesByCommunities
        Dim counter As Integer = 0
        DDLrole.Items.Clear()

        For Each oRole As Role In oRoleList
            DDLrole.Items.Add(oRole.Name)
            DDLrole.Items(counter).Text = oRole.Name
            DDLrole.Items(counter).Value = oRole.ID
            counter += 1
        Next
        _oRoleList = oRoleList
    End Sub

    Public Property SelectionMode() As System.Web.UI.WebControls.ListSelectionMode Implements PresentationLayer.IviewSearchUser.SelectionMode
        Get
            Dim oSelection As ListSelectionMode
            Try
                If String.IsNullOrEmpty(Me.ViewState("SelectionMode")) Then
                    oSelection = ListSelectionMode.Single
                Else
                    oSelection = DirectCast(Me.ViewState("SelectionMode"), ListSelectionMode)
                End If
            Catch ex As Exception
                oSelection = ListSelectionMode.Single
            End Try
            Return oSelection
        End Get
        Set(ByVal value As ListSelectionMode)
            Me.ViewState("SelectionMode") = value
            If value = ListSelectionMode.Single Then
                Me.GRVuser.Columns(1).Visible = True
                Me.GRVuser.Columns(2).Visible = False
            Else
                Me.GRVuser.Columns(1).Visible = False
                Me.GRVuser.Columns(2).Visible = True
            End If
        End Set
    End Property

    Public Overrides Sub BindDati()
        SetControlsVisibility()
    End Sub

    Private Sub SetControlsVisibility()
        BTNupdatePreview.Visible = CBshowPreview.Checked
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_SearchUserByCommunities", "UC")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(Me.BTNsearch, True)
            .setButton(Me.BTNupdatePreview, True)

            .setLabel(LBsearchTitle)
            .setLabel(LBname)
            .setLabel(LBsurname)
            .setLabel(LBrole)
            .setLabel(LBuserPerPage)
            .setLabel(LBnoRecord)
            .setLabel(LBnoRecordPreview)

            .setCheckBox(CBshowPreview)

            .setHeaderGridView(Me.GRVuser, 3, "Name", True)
            .setHeaderGridView(Me.GRVuser, 4, "Surname", True)
            .setHeaderGridView(Me.GRVuser, 6, "Roles", True)

        End With
    End Sub

#Region "readonly property"

    Private ReadOnly Property Login() As String Implements PresentationLayer.IviewSearchUser.Login
        Get
            Return TXBlogin.Text
        End Get
    End Property

    Private ReadOnly Property MailAddress() As String Implements PresentationLayer.IviewSearchUser.MailAddress
        Get
            Return TXBmail.Text
        End Get
    End Property

    Private ReadOnly Property Name() As String Implements PresentationLayer.IviewSearchUser.Name
        Get
            Return TXBname.Text
        End Get
    End Property

    Private ReadOnly Property PreviewUserList_isVisible() As Boolean Implements PresentationLayer.IviewSearchUser.PreviewUserList_isVisible
        Get
            Return CBshowPreview.Checked
        End Get
    End Property


    Private ReadOnly Property Surname() As String Implements PresentationLayer.IviewSearchUser.Surname
        Get
            Return TXBsurname.Text
        End Get
    End Property

    Private ReadOnly Property SelectedRoleId() As Integer Implements PresentationLayer.IviewSearchUser.SelectedRoleId
        Get
            Return DDLrole.SelectedItem.Value()
        End Get
    End Property

    Private ReadOnly Property CurrentUserId1() As Integer Implements PresentationLayer.IviewSearchUser.CurrentUserId
        Get
            'Altrimenti allo scadere della sessione da errore...
            Try
                Return Me.PageUtility.CurrentContext.UserContext.CurrentUserID
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

#End Region

   
    Private Function GetUsers() As System.Collections.Generic.List(Of Comol.Entity.BaseElement) Implements PresentationLayer.IviewSearchUser.GetUsers
        Dim oUserList As New List(Of BaseElement)

        For Each row As GridViewRow In Me.GRVuser.Rows
            Dim oUser As New BaseElement
            oUser.id = Me.GRVuser.DataKeys(row.RowIndex).Value()
            oUser.isSelected = DirectCast(row.Cells(3).FindControl("CBselect"), CheckBox).Checked
            oUserList.Add(oUser)
        Next
        Return oUserList
    End Function

    Protected Friend Property SelectedCommunitiesId() As System.Collections.Generic.List(Of Integer) Implements PresentationLayer.IviewSearchUser.SelectedCommunitiesId
        Get
            'Return _SelectedCommunitiesId
            Return ViewState("ComIdList")
        End Get
        Set(ByVal value As List(Of Integer))
            ViewState("ComIdList") = value
            '_SelectedCommunitiesId = value
        End Set
    End Property
    Private Sub CBshowPreview_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBshowPreview.CheckedChanged
        LBnoRecordPreview.Visible = False
        Me.CurrentPresenter.BindPreview()
        SetControlsVisibility()
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub
    Private Sub BTNupdatePreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupdatePreview.Click
        Me.CurrentPresenter.BindPreview()
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub
    Private Sub BTNsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsearch.Click
        LBnoRecord.Visible = False
        GRVuser.Visible = True
        If CBshowPreview.Checked Then
            BTNupdatePreview_Click(sender, e)
        End If
        Me.CurrentPresenter.Search()
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub
    Public Sub BindSearchResult(ByRef oUserList As System.Collections.Generic.List(Of Comol.Entity.MemberContact)) Implements PresentationLayer.IviewSearchUser.BindSearchResult
        'la gridview viene popolata
        If oUserList.Count > 0 Then
            GRVuser.DataSource = oUserList
            GRVuser.DataBind()
        Else
            LBnoRecord.Visible = True
            GRVuser.Visible = False
        End If

    End Sub
    Private Sub DDLnumUserPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLnumUserPerPage.SelectedIndexChanged
        GridPageSize = DDLnumUserPerPage.SelectedValue
        Me.CurrentPresenter.Search()
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub
    Private Sub GRVuser_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRVuser.DataBinding
        _SelectedCommunitiesId = Me.CurrentPresenter.GetTemporaryUserIdList()
        GRVuser.Columns(5).Visible = ShowMail
    End Sub
    Private Sub GRVuser_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRVuser.DataBound
        'Each time the data is bound to the grid we need to build up the CheckBoxIDs array
        If GRVuser.Rows.Count > 0 Then
            'Get the header CheckBox
            Dim cbHeader As CheckBox = CType(GRVuser.HeaderRow.FindControl("CBselectAll"), CheckBox)

            If Me.AjaxEnabled Then
                cbHeader.AutoPostBack = True
                AddHandler cbHeader.CheckedChanged, AddressOf CBselectAll_CheckedChanged
            Else
                'Run the ChangeCheckBoxState client-side function whenever the
                'header checkbox is checked/unchecked
                cbHeader.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
                cbHeader.AutoPostBack = False
                'Add the CheckBox's ID to the client-side CheckBoxIDs array
                Dim ArrayValues As New List(Of String)
                ArrayValues.Add(String.Concat("'", cbHeader.ClientID, "'"))

                For Each gvr As GridViewRow In GRVuser.Rows
                    'Get a programmatic reference to the CheckBox control
                    Dim cb As CheckBox = CType(gvr.FindControl("CBselect"), CheckBox)

                    'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
                    cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"

                    'Add the CheckBox's ID to the client-side CheckBoxIDs array
                    ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                Next

                'Output the array to the Literal control (CheckBoxIDsArray)
                CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf & _
                   "<!--" & vbCrLf & _
                   String.Concat("var CheckBoxIDs =  new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf & _
                   "// -->" & vbCrLf & _
                   "</script>"
            End If
        End If
    End Sub
    Protected Friend Sub CBselectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each oRow As GridViewRow In GRVuser.Rows
            Dim oCheckBox As CheckBox = CType(oRow.FindControl("CBselect"), CheckBox)
            If Not IsNothing(oCheckBox) Then
                oCheckBox.Checked = DirectCast(sender, CheckBox).Checked
            End If
        Next
    End Sub

    Private Sub GRVuser_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVuser.PageIndexChanging
        Me.CurrentPresenter.BindPreview()
        Me.CurrentPresenter.GoToPage(e.NewPageIndex + 1)
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub
    Public Sub init(ByRef oRoleList As System.Collections.Generic.List(Of Comol.Entity.Role)) Implements PresentationLayer.IviewSearchUser.init
        BindRolesByCommunities(oRoleList)
        SetInternazionalizzazione()
        GRVuser.DataSource = Nothing
        GRVuser.DataBind()
        RPTuserListPreview.DataSource = Nothing
        RPTuserListPreview.DataBind()
    End Sub
    Public ReadOnly Property AscendingImage() As String
        Get
            Return Me.BaseUrl & "images/Grid/Ascending.gif"
        End Get
    End Property
    Public ReadOnly Property DescendingImage() As String
        Get
            Return Me.BaseUrl & "images/Grid/Descending.gif"
        End Get
    End Property

    Private Sub GRVuser_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVuser.RowCommand

    End Sub
    'Private Sub GRVuser_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVuser.RowCreated
    '    If e.Row.RowType = DataControlRowType.Header Then
    '        Dim CurrenColumn As Integer = 0
    '        For Each oCell As TableCell In e.Row.Cells
    '            If oCell.HasControls And oCell.Visible Then
    '                If TypeOf (oCell.Controls(0)) Is System.Web.UI.LiteralControl Then
    '                    Dim oCBselectAll As CheckBox = CType(e.Row.FindControl("CBselectAll"), CheckBox)
    '                    If Not oCBselectAll Is Nothing Then
    '                        oCBselectAll.Attributes.Add("onClick", "SelectAllRows(this)")
    '                    End If
    '                    oCBselectAll.ToolTip = Me.Resource.getValue("SelectAllRows")
    '                Else
    '                    Dim oControl As WebControl = oCell.Controls(0)
    '                    If TypeOf (oControl) Is System.Web.UI.WebControls.LinkButton Then
    '                        Dim oLinkbutton As LinkButton = DirectCast(oControl, LinkButton)
    '                        Dim oImageUp As System.Web.UI.WebControls.ImageButton = New System.Web.UI.WebControls.ImageButton
    '                        Dim oImageDown As System.Web.UI.WebControls.ImageButton = New System.Web.UI.WebControls.ImageButton
    '                        oImageUp.ImageUrl = Me.AscendingImage
    '                        oImageUp.CausesValidation = False
    '                        oImageUp.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)
    '                        oImageUp.AlternateText = oImageUp.ToolTip
    '                        oImageUp.CommandName = "Sort_" & Comol.Entity.sortDirection.Ascending
    '                        oImageUp.CommandArgument = oLinkbutton.CommandArgument
    '                        oImageDown.ImageUrl = Me.DescendingImage
    '                        oImageDown.CausesValidation = False
    '                        oImageDown.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)
    '                        oImageDown.AlternateText = oImageDown.ToolTip
    '                        oImageDown.CommandName = "Sort_" & Comol.Entity.sortDirection.Descending
    '                        oImageDown.CommandArgument = oLinkbutton.CommandArgument
    '                        If oLinkbutton.CommandArgument = Me.OrderBy Then
    '                            oImageUp.Enabled = IIf(Me.Direction = Comol.Entity.sortDirection.Ascending, False, True)
    '                            oImageDown.Enabled = Not oImageUp.Enabled
    '                        End If
    '                        oLinkbutton.ToolTip = Me.GRVuser.Columns(CurrenColumn).AccessibleHeaderText
    '                        oCell.Controls.Add(New LiteralControl(" "))
    '                        oCell.Controls.Add(oImageUp)
    '                        oCell.Controls.Add(oImageDown)
    '                    End If
    '                End If
    '            End If
    '            CurrenColumn += 1
    '        Next
    '    ElseIf e.Row.RowType = DataControlRowType.Pager Then
    '        Try
    '            Dim oRowsPager As TableRowCollection
    '            oRowsPager = DirectCast(e.Row.Cells(0).Controls(0), Table).Rows

    '            For Each oCell As TableCell In oRowsPager(0).Cells
    '                If TypeOf oCell.Controls(0) Is Label Then
    '                    Dim oLabelPage As Label = DirectCast(oCell.Controls(0), Label)
    '                    oLabelPage.CssClass = "PagerSpan"
    '                    oLabelPage.ToolTip = String.Format(Me.Resource.getValue("ActualPage"), oLabelPage.Text)
    '                ElseIf TypeOf oCell.Controls(0) Is LinkButton Then
    '                    Dim oLinkPage As LinkButton = DirectCast(oCell.Controls(0), LinkButton)
    '                    oLinkPage.CssClass = "PagerLink"
    '                    oLinkPage.ToolTip = String.Format(Me.Resource.getValue("GoToPage"), oLinkPage.Text)
    '                End If
    '            Next
    '            DirectCast(e.Row.Cells(0).Controls(0), Table).Attributes.Add("summary", String.Format(Me.Resource.getValue("Summary_Navigazione"), Me.GridCurrentPage))
    '        Catch ex As Exception

    '        End Try
    '    End If

    'End Sub
    Private Sub GRVuser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVuser.RowDataBound
        Dim oMember As New MemberContact
        Dim LBroles As New Label
        Dim concatRoles As String
        Dim SelectedCommunitiesId As List(Of Integer) = _SelectedCommunitiesId

        oMember = DirectCast(e.Row.DataItem, MemberContact)
        If Not oMember Is Nothing Then
            For Each oMembershipInfo As MembershipInfo In oMember.MembershipInfo

                For Each oRole As Role In _oRoleList
                    If oRole.ID = oMembershipInfo.MemberRole.ID Then
                        concatRoles &= oRole.Name & ", "
                        Exit For
                    End If
                Next
            Next
            LBroles.Text = concatRoles
            e.Row.Cells(6).Controls.Add(LBroles)
        End If
        If SelectedCommunitiesId.Count > 0 AndAlso e.Row.RowIndex >= 0 AndAlso SelectionMode = ListSelectionMode.Multiple Then
            If SelectedCommunitiesId.Contains(GRVuser.DataKeys(e.Row.RowIndex).Value) Then
                Dim oCB As New CheckBox
                DirectCast(e.Row.Cells(2).Controls(1), CheckBox).Checked = True
            End If
        End If
        'If SelectionMode = ListSelectionMode.Single AndAlso se Then
        '    'ROW_Selezionate_Small()

        'End If
    End Sub
    Private Sub GRVuser_SelectedIndexChanging1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles GRVuser.SelectedIndexChanging

        Me.CurrentPresenter.setSingleUser(Me.GRVuser.DataKeys(e.NewSelectedIndex).Value)
        Me.CurrentPresenter.BindPreview()
        If Me.AjaxEnabled Then
            RaiseEvent AjaxEventUpdate()
        End If
    End Sub

    Public ReadOnly Property GetSearchButtonControl() As Button
        Get
            Return Me.BTNsearch
        End Get
    End Property

    'Public Overrides ReadOnly Property AlwaysBind As Boolean
    '    Get
    '        Return False
    '    End Get
    'End Property

    'Public Overrides ReadOnly Property VerifyAuthentication As Boolean
    '    Get
    '        Return False
    '    End Get
    'End Property
    Public ReadOnly Property SearchButtonUniqueID As String
        Get
            Return Me.BTNsearch.UniqueID
        End Get
    End Property
    Public ReadOnly Property SearchDefaultTextField As String
        Get
            Return Me.TXBsurname.UniqueID
        End Get
    End Property
End Class