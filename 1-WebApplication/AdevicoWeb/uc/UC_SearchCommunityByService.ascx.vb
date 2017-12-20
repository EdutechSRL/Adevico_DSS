Public Partial Class UC_SearchCommunityByService
    Inherits BaseControlSession
    Implements IviewSearchCommunity



    Private _Presenter As PresenterSearchCommunityByService
    Private _CurrentSubscriber As Person

    Public Event SelectedCommunityChanged(ByVal CommunityID As Integer)

    Public ReadOnly Property CurrentCommunity() As Comol.Entity.Community Implements PresentationLayer.IviewSearchCommunity.CurrentCommunity
        Get
            If MyBase.ComunitaCorrente Is Nothing Then
                Return Nothing
            Else
                Return New Community(MyBase.ComunitaCorrente.Id, MyBase.ComunitaCorrente.Nome, MyBase.ComunitaCorrente.IdPadre)
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentSubscriber() As Person Implements PresentationLayer.IviewSearchCommunity.CurrentSubscriber
        Get
            If IsNothing(_CurrentSubscriber) Then
                If MyBase.UtenteCorrente Is Nothing Then
                    'Return New Person(2, "luigi", "colazzo")
                    'Return New Person(149, "francesco", "conte")
                    _CurrentSubscriber = New Person(0, "", "")
                Else
                    'Return New Person(149, "francesco", "conte")
                    _CurrentSubscriber = New Person(MyBase.UtenteCorrente.ID, MyBase.UtenteCorrente.Nome, MyBase.UtenteCorrente.Cognome)
                End If
                '	_CurrentSubscriber = New Person(MyBase.UtenteCorrente.ID, MyBase.UtenteCorrente.Nome, MyBase.UtenteCorrente.Cognome)
            End If
            Return _CurrentSubscriber
        End Get
    End Property
    Public ReadOnly Property CurrentLanguage() As Comol.Entity.Lingua Implements PresentationLayer.IviewSearchCommunity.CurrentLanguage
        Get
            Return MyBase.UserSessionLanguage
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As PresentationLayer.PresenterSearchCommunityByService Implements PresentationLayer.IviewSearchCommunity.CurrentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PresenterSearchCommunityByService(Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "Grid Proprerty"
    Public Property Direction() As Comol.Entity.sortDirection Implements PresentationLayer.IviewSearchCommunity.Direction
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
    Public Property OrderBy() As String Implements PresentationLayer.IviewSearchCommunity.OrderBy
        Get
            Return Me.ViewState("SortExpression")
        End Get
        Set(ByVal value As String)
            Me.ViewState("SortExpression") = value
        End Set
    End Property
    Public Property GridCurrentPage() As Integer Implements PresentationLayer.IviewSearchCommunity.GridCurrentPage
        Get
            Return Me.ViewState("GridCurrentPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("GridCurrentPage") = value
            Me.GRVcomunita.PageIndex = value - 1
        End Set
    End Property
    Public Property GridMaxPage() As Integer Implements PresentationLayer.IviewSearchCommunity.GridMaxPage
        Get
            Return Me.ViewState("GridMaxPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("GridMaxPage") = value
        End Set
    End Property
    Public Property GridPageSize() As Integer Implements PresentationLayer.IviewSearchCommunity.GridPageSize
        Get
            Return Me.GRVcomunita.PageSize
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                Me.GRVcomunita.AllowPaging = False
            Else
                Me.GRVcomunita.AllowPaging = True
                Me.GRVcomunita.PageSize = value
            End If
        End Set
    End Property
#End Region

    Public Property HasCommunity() As Boolean Implements PresentationLayer.IviewSearchCommunity.HasCommunity
        Get
            Try
                Return CBool(Me.ViewState("HasCommunity"))
            Catch ex As Exception
                Return False
            End Try
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasCommunity") = value
        End Set
    End Property

    Public Property ServiceClauses() As GenericClause(Of ServiceClause) Implements PresentationLayer.IviewSearchCommunity.ServiceClauses
        Get
            Dim iResponse As GenericClause(Of ServiceClause)
            iResponse = TryCast(Me.ViewState("ServiceClauses"), GenericClause(Of ServiceClause))
            Return iResponse
        End Get
        Set(ByVal value As GenericClause(Of ServiceClause))
            Me.ViewState("ServiceClauses") = value
        End Set
    End Property

    Public Property SelectedCommunitiesID() As List(Of Integer) Implements PresentationLayer.IviewSearchCommunity.SelectedCommunitiesID
        Get
            Dim names As Dictionary(Of Integer, String) = Me.SelectedCommunities
            Dim SelectedID As List(Of Integer) = ViewStateOrDefault("SelectedCommunities", New List(Of Integer))
            If Me.SelectionMode = ListSelectionMode.Multiple Then
                For Each oRow As GridViewRow In Me.GRVcomunita.Rows
                    Dim oCheck As CheckBox = oRow.Cells(2).FindControl("CBXselect")
                    Dim oLabel As Label = oRow.Cells(2).FindControl("LBcommunityID")
                    If Not IsNothing(oCheck) AndAlso Not IsNothing(oLabel) Then
                        Dim idCommunity As Integer = CInt((oLabel.Text))
                        If oCheck.Checked Then
                            If Not SelectedID.Contains(idCommunity) Then
                                SelectedID.Add(idCommunity)
                            End If
                        Else
                            SelectedID.Remove(idCommunity)
                        End If
                        If names.ContainsKey(idCommunity) Then
                            If Not oCheck.Checked Then
                                names.Remove(idCommunity)
                            End If
                        ElseIf oCheck.Checked Then
                            names(idCommunity) = oRow.Cells(3).Text
                        End If
                    End If
                Next
            End If
            Me.SelectedCommunities = names
            Return SelectedID
        End Get
        Set(ByVal value As List(Of Integer))
            If IsNothing(value) Then
                value = New List(Of Integer)
            End If
            If value.Count = 0 Then
                Me.GRVcomunita.SelectedIndex = -1
            End If
            Me.ViewState("SelectedCommunities") = value

            If Me.AllowCommunityChangedEvent And Me.SelectionMode = ListSelectionMode.Single Then
                If value.Count = 0 Then
                    RaiseEvent SelectedCommunityChanged(-1)
                Else
                    RaiseEvent SelectedCommunityChanged(value(0))
                End If
            End If
        End Set
    End Property

    Public Function GetCommunityName(ByVal idCommunity) As String
        If SelectedCommunities.ContainsKey(idCommunity) Then
            Return SelectedCommunities(idCommunity)
        Else
            Return COL_BusinessLogic_v2.Comunita.COL_Comunita.EstraiNomeBylingua(idCommunity, LinguaID)
        End If
    End Function
    Private Property SelectedCommunities() As Dictionary(Of Integer, String) Implements IviewSearchCommunity.SelectedCommunities
        Get
            Return ViewStateOrDefault("SelectedCommunitiesName", New Dictionary(Of Integer, String))
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            Me.ViewState("SelectedCommunitiesName") = value
        End Set
    End Property
    Public Property SelectionMode() As ListSelectionMode Implements PresentationLayer.IviewSearchCommunity.SelectionMode
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
                Me.GRVcomunita.Columns(1).Visible = True
                Me.GRVcomunita.Columns(2).Visible = False
            Else
                Me.GRVcomunita.Columns(1).Visible = False
                Me.GRVcomunita.Columns(2).Visible = True
            End If
        End Set
    End Property

    Public Property AllowCommunityChangedEvent() As Boolean Implements PresentationLayer.IviewSearchCommunity.AllowCommunityChangedEvent
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowCommunityChangedEvent")) Then
                Me.ViewState("AllowCommunityChangedEvent") = False
            End If
            Return DirectCast(ViewState("AllowCommunityChangedEvent"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowCommunityChangedEvent") = value
        End Set
    End Property

    Public Property AllowMultipleOrganizationSelection() As Boolean Implements PresentationLayer.IviewSearchCommunity.AllowMultipleOrganizationSelection
        Get
            If String.IsNullOrEmpty(Me.ViewState("AllowMultipleOrganizationSelection")) Then
                Me.ViewState("AllowMultipleOrganizationSelection") = False
            End If
            Return DirectCast(ViewState("AllowMultipleOrganizationSelection"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowMultipleOrganizationSelection") = value
        End Set
    End Property

    'Public Property SelectedCommunity() As Comol.Entity.Community Implements PresentationLayer.IviewSearchCommunity.SelectedCommunity
    '	Get
    '		Try
    '			If Me.GRVcomunita.SelectedRow Is Nothing Then
    '				Return Nothing
    '			Else
    '				If Me.GRVcomunita.SelectedIndex < 0 Then
    '					Return Nothing
    '				Else
    '					Return New Community(Me.GRVcomunita.DataKeys(Me.GRVcomunita.SelectedIndex).Value)
    '				End If

    '			End If
    '		Catch ex As Exception
    '			Return Nothing
    '		End Try
    '	End Get
    '	Set(ByVal value As Comol.Entity.Community)
    '		If value Is Nothing Then
    '			Me.GRVcomunita.SelectedIndex = -1
    '		End If
    '	End Set
    'End Property

#Region "Filters properties"
    Public Property AutoUpdateList() As Boolean Implements PresentationLayer.IviewSearchCommunity.AutoUpdateList
        Get
            If String.IsNullOrEmpty(Me.ViewState("AutoUpdateList")) Then
                Return True
            Else
                Try
                    Return DirectCast(Me.ViewState("AutoUpdateList"), Boolean)
                Catch ex As Exception
                    Return True
                End Try
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AutoUpdateList") = value
        End Set
    End Property
    Public Property SearchBy() As String Implements PresentationLayer.IviewSearchCommunity.SearchBy
        Get
            SearchBy = Me.TXBValore.Text
        End Get
        Set(ByVal value As String)
            Me.TXBValore.Text = value
        End Set
    End Property
    Public ReadOnly Property CurrentSearch() As Comol.Entity.StandardCommunitySearch Implements PresentationLayer.IviewSearchCommunity.CurrentSearch
        Get
            Try
                CurrentSearch = Me.DDLTipoRicerca.SelectedValue
            Catch ex As Exception
                CurrentSearch = StandardCommunitySearch.All
            End Try
        End Get
    End Property
    Public Property CurrentCommunityType() As Comol.Entity.CommunityType Implements PresentationLayer.IviewSearchCommunity.CurrentCommunityType
        Get
            If Me.DDLTipo.Items.Count > 0 Then
                Return New CommunityType(Me.DDLTipo.SelectedValue, Me.DDLTipo.SelectedItem.Text)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Comol.Entity.CommunityType)
            If Me.DDLTipo.Items.Count > 0 AndAlso Not IsNothing(Me.DDLTipo.Items.FindByValue(value.ID)) Then
                Me.DDLTipo.SelectedValue = value.ID
            End If
            Me.DIVcourse.Style("display") = "none"
            Me.DIVdegreeType.Style("display") = "none"
            Select Case value.ID
                Case StandardCommunityType.Degree
                    Me.DIVdegreeType.Style("display") = "block"
                Case StandardCommunityType.UniversityCourse
                    Me.DIVcourse.Style("display") = "block"
            End Select
        End Set
    End Property
    Public ReadOnly Property CurrentStatus() As CommunityStatus Implements PresentationLayer.IviewSearchCommunity.CurrentStatus
        Get
            If Me.DDLstatoComunita.Items.Count = 0 Then
                Return CommunityStatus.None
            Else
                Return Me.DDLstatoComunita.SelectedValue
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentAccademicYear() As AcademicYear Implements PresentationLayer.IviewSearchCommunity.CurrentAcademicYear
        Get
            If Me.DDLannoAccademico.Items.Count > 0 Then
                Return New AcademicYear(Me.DDLannoAccademico.SelectedValue, Me.DDLannoAccademico.SelectedItem.Text)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentResponsibleID() As Integer Implements PresentationLayer.IviewSearchCommunity.CurrentResponsibleID
        Get
            Return -1
        End Get
    End Property
    Public ReadOnly Property CurrentDegreeType() As Comol.Entity.TypeDegree Implements PresentationLayer.IviewSearchCommunity.CurrentDegreeType
        Get
            If Me.DDLtipoCorsoDiStudi.Items.Count > 0 Then
                Return New TypeDegree(Me.DDLtipoCorsoDiStudi.SelectedValue, Me.DDLtipoCorsoDiStudi.SelectedItem.Text)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentOrganization() As Organization Implements PresentationLayer.IviewSearchCommunity.CurrentOrganization
        Get
            If Me.DDLorganizzazione.Items.Count > 0 Then
                Return New Organization(Me.DDLorganizzazione.SelectedValue, Me.DDLorganizzazione.SelectedItem.Text, True)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentPeriodo() As Periodo Implements PresentationLayer.IviewSearchCommunity.CurrentPeriodo
        Get
            If Me.DDLperiodo.Items.Count > 0 Then
                Return New Periodo(Me.DDLperiodo.SelectedValue, Me.DDLperiodo.SelectedItem.Text)
            Else
                Return Nothing
            End If
        End Get
    End Property
#End Region

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetCultureSettings()
            Me.SetInternazionalizzazione()
        End If
    End Sub

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.Init(0)
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_SearchCommunityByService", "UC")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setDropDownList(Me.DDLTipoRicerca, StandardCommunitySearch.NameContains)
            .setDropDownList(Me.DDLTipoRicerca, StandardCommunitySearch.NameStartWith)

            .setLabel(LBorganizzazione_c)
            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBtipoRicerca_c)
            '    .setLabel(Me.LBvalore_)
            .setButton(Me.BTNCerca)
            .setLabel(Me.LBcorsoDiStudi_t)

            .setHeaderGridView(Me.GRVcomunita, 1, "Opzioni", True)
            '.setHeaderGridView(Me.GRVcomunita, 2, "Opzioni", True)
            .setHeaderGridView(Me.GRVcomunita, 3, "Name", True)
            .setHeaderGridView(Me.GRVcomunita, 4, "Type", True)
        End With
    End Sub

    Public Sub LoadAccademicYears(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadAcademicYears
        Me.DDLannoAccademico.DataSource = oList
        Me.DDLannoAccademico.DataTextField = "Text"
        Me.DDLannoAccademico.DataValueField = "Value"
        Me.DDLannoAccademico.DataBind()

        If Me.DDLannoAccademico.Items.Count > 1 Then
            Me.DDLannoAccademico.Enabled = True
            'Me.DDLannoAccademico.Items.Insert(0, New ListItem("All", -1))
            'Me.Resource.setDropDownList(Me.DDLannoAccademico, -1)
        Else
            Me.DDLannoAccademico.Enabled = False
        End If
    End Sub
    Public Sub LoadCommunityTypes(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadCommunityTypes
        Me.DDLTipo.DataSource = oList
        Me.DDLTipo.DataTextField = "Text"
        Me.DDLTipo.DataValueField = "Value"
        Me.DDLTipo.DataBind()

        If Me.DDLTipo.Items.Count > 1 Then
            Me.DDLTipo.Enabled = True
            'Me.DDLTipo.Items.Insert(0, New ListItem("All", -1))
            'Me.Resource.setDropDownList(Me.DDLTipo, -1)
        Else
            Me.DDLTipo.Enabled = False
        End If

        Me.DIVcourse.Style("display") = "none"
        Me.DIVdegreeType.Style("display") = "none"
        Select Case Me.DDLTipo.SelectedValue
            Case StandardCommunityType.Degree
                Me.DIVdegreeType.Style("display") = "block"
            Case StandardCommunityType.UniversityCourse
                Me.DIVcourse.Style("display") = "block"
        End Select
    End Sub
    Public Sub LoadDegreeTypes(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadDegreeTypes
        Me.DDLtipoCorsoDiStudi.DataSource = oList
        Me.DDLtipoCorsoDiStudi.DataTextField = "Text"
        Me.DDLtipoCorsoDiStudi.DataValueField = "Value"
        Me.DDLtipoCorsoDiStudi.DataBind()
        If Me.DDLtipoCorsoDiStudi.Items.Count > 1 Then
            Me.DDLtipoCorsoDiStudi.Enabled = True
            'Me.DDLtipoCorsoDiStudi.Items.Insert(0, New ListItem("All", -1))
            'Me.Resource.setDropDownList(Me.DDLtipoCorsoDiStudi, -1)
        Else
            Me.DDLtipoCorsoDiStudi.Enabled = False
        End If
    End Sub
    Public Sub LoadOrganizations(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadOrganizations
        Me.DDLorganizzazione.DataSource = oList
        Me.DDLorganizzazione.DataTextField = "Text"
        Me.DDLorganizzazione.DataValueField = "Value"
        Me.DDLorganizzazione.DataBind()
    End Sub
    Public Sub LoadPeriodi(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadPeriodi
        Me.DDLperiodo.DataSource = oList
        Me.DDLperiodo.DataTextField = "Text"
        Me.DDLperiodo.DataValueField = "Value"
        Me.DDLperiodo.DataBind()

        If Me.DDLperiodo.Items.Count > 1 Then
            Me.DDLperiodo.Enabled = True
            'Me.DDLperiodo.Items.Insert(0, New ListItem("All", -1))
            'Me.Resource.setDropDownList(Me.DDLperiodo, -1)
        Else
            Me.DDLperiodo.Enabled = False
        End If
    End Sub
    Public Sub LoadCommunities(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadCommunities


        Me.GRVcomunita.DataSource = oList
        Me.GRVcomunita.DataBind()
    End Sub
    Public Sub LoadStatus(ByVal oList As System.Collections.IList) Implements PresentationLayer.IviewSearchCommunity.LoadStatus
        Dim oCurrentStatus As Integer = CommunityStatus.None

        If DDLstatoComunita.Items.Count > 0 Then
            oCurrentStatus = DDLstatoComunita.SelectedValue
        End If
        If IsNothing(oList) OrElse oList.Count = 0 Then
            DDLstatoComunita.Enabled = False
            DDLstatoComunita.Items.Clear()

        Else
            DDLstatoComunita.Enabled = True
            DDLstatoComunita.DataSource = oList
            DDLstatoComunita.DataTextField = "Text"
            DDLstatoComunita.DataValueField = "Value"
            DDLstatoComunita.DataBind()
        End If
        If oCurrentStatus = CommunityStatus.None AndAlso Not IsNothing(DDLstatoComunita.Items.FindByValue(CommunityStatus.OnlyActivated)) Then
            DDLstatoComunita.SelectedValue = CommunityStatus.OnlyActivated
        End If
    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.CurrentPresenter.ChangeOrganization()
    End Sub
    Private Sub DDLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstatoComunita.SelectedIndexChanged
        Me.CurrentPresenter.ChangeStatus()
    End Sub
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        Me.DIVcourse.Style("display") = "none"
        Me.DIVdegreeType.Style("display") = "none"

        Select Case Me.DDLTipo.SelectedValue
            Case StandardCommunityType.Degree
                Me.DIVdegreeType.Style("display") = "block"
            Case StandardCommunityType.UniversityCourse
                Me.DIVcourse.Style("display") = "block"
        End Select
        Me.CurrentPresenter.ChangeCommunityType()
    End Sub
    Private Sub DDLannoAccademico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLannoAccademico.SelectedIndexChanged
        Me.CurrentPresenter.ChangeAccademicYear()
    End Sub

    Private Sub DDLperiodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLperiodo.SelectedIndexChanged
        Me.CurrentPresenter.ChangePeriodo()
    End Sub

    Private Sub DDLtipoCorsoDiStudi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoCorsoDiStudi.SelectedIndexChanged
        Me.CurrentPresenter.ChangeDegree()
    End Sub

#Region "select community"

    Private Sub GRVcomunita_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVcomunita.RowCommand
        If e.CommandName.Contains("Sort_") Then
            Me.Direction = IIf(e.CommandName.EndsWith("_" & Comol.Entity.sortDirection.Ascending), Comol.Entity.sortDirection.Ascending, Comol.Entity.sortDirection.Descending)
            Me.OrderBy = e.CommandArgument
            Me.CurrentPresenter.LoadCommunityList()
        End If
    End Sub
    Private Sub GRVcomunita_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVcomunita.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Me.SelectionMode = ListSelectionMode.Single Then
                'Dim oSubscription As Subscription = DirectCast(e.Row.DataItem, Subscription)
                'Dim oLinkButton As LinkButton = e.Row.Cells(1).Controls(0)
                'oLinkButton.Enabled = Me.CurrentPresenter.ServiceAvailable(oSubscription.CommunitySubscripted.ID)
            Else
                Dim oSubscription As Subscription = DirectCast(e.Row.DataItem, Subscription)
                Dim oCheckBox As CheckBox = CType(e.Row.FindControl("CBXselect"), CheckBox)

                'oCheckBox.Enabled = Me.CurrentPresenter.ServiceAvailable(oSubscription.CommunitySubscripted.ID)

                If oCheckBox.Enabled Then
                    Me.Page.RegisterArrayDeclaration("CheckCommunityBoxIDs", String.Concat("'", oCheckBox.ClientID, "'"))
                    oCheckBox.ToolTip = Me.Resource.getValue("SelectRow")
                    ' oCheckBox.Attributes.Add("onClick", "SelectRow()")

                    If Me.SelectedCommunitiesID.Contains(oSubscription.CommunitySubscriptedID) Then
                        oCheckBox.Checked = True
                    Else
                        oCheckBox.Checked = False
                    End If
                ElseIf Me.SelectedCommunitiesID.Contains(oSubscription.CommunitySubscriptedID) Then
                    Me.SelectedCommunitiesID.Remove(oSubscription.CommunitySubscriptedID)
                End If
            End If
        End If
    End Sub
    Private Sub GRVcomunita_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVcomunita.PageIndexChanging
        If Me.SelectionMode = ListSelectionMode.Multiple Then
            UpdateSelection()
        End If
        Me.CurrentPresenter.GoToPage(e.NewPageIndex + 1)
    End Sub
    Private Sub GRVcomunita_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVcomunita.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim CurrenColumn As Integer = 0
            For Each oCell As TableCell In e.Row.Cells
                If oCell.HasControls And oCell.Visible Then
                    If TypeOf (oCell.Controls(0)) Is System.Web.UI.LiteralControl Then
                        Dim oCBXselectAll As CheckBox = CType(e.Row.FindControl("CBXselectAll"), CheckBox)
                        If Not oCBXselectAll Is Nothing Then
                            oCBXselectAll.Attributes.Add("onClick", "ChangeCommunityAllCheckBoxStates(this.checked);")
                        End If
                        oCBXselectAll.ToolTip = Me.Resource.getValue("SelectAllRows")
                    Else
                        Dim oControl As WebControl = oCell.Controls(0)
                        If TypeOf (oControl) Is System.Web.UI.WebControls.LinkButton Then
                            Dim oLinkbutton As LinkButton = DirectCast(oControl, LinkButton)
                            Dim oImageUp As System.Web.UI.WebControls.ImageButton = New System.Web.UI.WebControls.ImageButton
                            Dim oImageDown As System.Web.UI.WebControls.ImageButton = New System.Web.UI.WebControls.ImageButton
                            oImageUp.ImageUrl = Me.AscendingImage
                            oImageUp.CausesValidation = False
                            oImageUp.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)
                            oImageUp.AlternateText = oImageUp.ToolTip
                            oImageUp.CommandName = "Sort_" & Comol.Entity.sortDirection.Ascending
                            oImageUp.CommandArgument = oLinkbutton.CommandArgument
                            oImageDown.ImageUrl = Me.DescendingImage
                            oImageDown.CausesValidation = False
                            oImageDown.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)
                            oImageDown.AlternateText = oImageDown.ToolTip
                            oImageDown.CommandName = "Sort_" & Comol.Entity.sortDirection.Descending
                            oImageDown.CommandArgument = oLinkbutton.CommandArgument
                            If oLinkbutton.CommandArgument = Me.OrderBy Then
                                oImageUp.Enabled = IIf(Me.Direction = Comol.Entity.sortDirection.Ascending, False, True)
                                oImageDown.Enabled = Not oImageUp.Enabled
                            End If
                            oLinkbutton.ToolTip = Me.GRVcomunita.Columns(CurrenColumn).AccessibleHeaderText

                            Dim oLabel As New Label With {.Text = oLinkbutton.Text}
                            oCell.Controls.RemoveAt(0)
                            oCell.Controls.AddAt(0, oLabel)

                            oCell.Controls.Add(New LiteralControl(" "))
                            oCell.Controls.Add(oImageUp)
                            oCell.Controls.Add(oImageDown)
                        End If
                    End If
                End If
                CurrenColumn += 1
            Next
        ElseIf e.Row.RowType = DataControlRowType.Pager Then
            Try
                Dim oRowsPager As TableRowCollection
                oRowsPager = DirectCast(e.Row.Cells(0).Controls(0), Table).Rows

                For Each oCell As TableCell In oRowsPager(0).Cells
                    If TypeOf oCell.Controls(0) Is Label Then
                        Dim oLabelPage As Label = DirectCast(oCell.Controls(0), Label)
                        oLabelPage.CssClass = "PagerSpan"
                        oLabelPage.ToolTip = String.Format(Me.Resource.getValue("ActualPage"), oLabelPage.Text)
                    ElseIf TypeOf oCell.Controls(0) Is LinkButton Then
                        Dim oLinkPage As LinkButton = DirectCast(oCell.Controls(0), LinkButton)
                        oLinkPage.CssClass = "PagerLink"
                        oLinkPage.ToolTip = String.Format(Me.Resource.getValue("GoToPage"), oLinkPage.Text)
                    End If
                Next
                DirectCast(e.Row.Cells(0).Controls(0), Table).Attributes.Add("summary", String.Format(Me.Resource.getValue("Summary_Navigazione"), Me.GridCurrentPage))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub GRVcomunita_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles GRVcomunita.SelectedIndexChanging
        Me.CurrentPresenter.SelectCommunity(Me.GRVcomunita.DataKeys(e.NewSelectedIndex).Value, Me.GRVcomunita.Rows(e.NewSelectedIndex).Cells(3).Text)

    End Sub
    Private Sub GRVcomunita_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GRVcomunita.Sorting
        e.Cancel = True
        If Me.OrderBy <> e.SortExpression Then
            Me.Direction = Comol.Entity.sortDirection.Ascending
        Else
            Me.Direction = IIf(Me.Direction = Comol.Entity.sortDirection.Ascending, Comol.Entity.sortDirection.Descending, Comol.Entity.sortDirection.Ascending)
        End If
        Me.OrderBy = e.SortExpression
        If Me.SelectionMode = ListSelectionMode.Multiple Then
            UpdateSelection()
        End If
        Me.CurrentPresenter.LoadCommunityList()
    End Sub
#End Region

    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        If Me.SelectionMode = ListSelectionMode.Multiple Then
            UpdateSelection()
        End If
        Me.CurrentPresenter.LoadCommunityList()
    End Sub

    Private Sub UpdateSelection()
        Dim SelectedList As List(Of Integer) = Me.SelectedCommunitiesID
        Dim names As Dictionary(Of Integer, String) = Me.SelectedCommunities
        For Each oRow As GridViewRow In Me.GRVcomunita.Rows
            Dim oCheck As CheckBox = oRow.Cells(2).FindControl("CBXselect")
            Dim oLabel As Label = oRow.Cells(2).FindControl("LBcommunityID")
            If Not IsNothing(oCheck) AndAlso Not IsNothing(oLabel) Then
                Dim idCommunity As Integer = CInt((oLabel.Text))
                If oCheck.Checked Then
                    If SelectedList.Contains(idCommunity) = False Then
                        SelectedList.Add(idCommunity)
                    End If
                Else
                    SelectedList.Remove(idCommunity)
                End If
                If names.ContainsKey(idCommunity) Then
                    If Not oCheck.Checked Then
                        names.Remove(idCommunity)
                    End If
                ElseIf oCheck.Checked Then
                    names(idCommunity) = oRow.Cells(3).Text
                End If
            End If
        Next
        Me.SelectedCommunities = names
        'Else
        '    For Each oRow As GridViewRow In Me.GRVcomunita.Rows
        '        Dim oCheck As CheckBox = oRow.Cells(2).FindControl("CBXselect")
        '        Dim oLabel As Label = oRow.Cells(2).FindControl("LBcommunityID")
        '        If Not IsNothing(oCheck) AndAlso Not IsNothing(oLabel) Then
        '            If oCheck.Checked Then
        '                SelectedList.Add(oLabel.Text)
        '            End If
        '        End If
        '    Next
        'End If
        Me.SelectedCommunitiesID = SelectedList
    End Sub

    Public Property ExludeCommunities() As List(Of Integer) Implements PresentationLayer.IviewSearchCommunity.ExludeCommunities
        Get
            Return ViewStateOrDefault("ExludeCommunities", New List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Me.ViewState("ExludeCommunities") = IIf(value Is Nothing, New List(Of Integer), value)
        End Set
    End Property
    Public Sub InitializeControl(ByVal idCommunity As Integer, Optional ByVal idCommunitiesToRemove As List(Of Integer) = Nothing)
        If Not IsNothing(idCommunitiesToRemove) Then
            ExludeCommunities = idCommunitiesToRemove
        End If

        'Versione precedente
        'Me.CurrentPresenter.Init(idCommunity)

        'Versione OK
        If Not IsNothing(idCommunitiesToRemove) AndAlso (idCommunitiesToRemove.Count() > 0) Then
            Me.CurrentPresenter.Init(idCommunitiesToRemove)
        Else
            Me.CurrentPresenter.Init(idCommunity)
        End If
    End Sub

    Public ReadOnly Property SearchButtonUniqueID As String
        Get
            Return Me.BTNCerca.UniqueID
        End Get
    End Property
    Public ReadOnly Property SearchDefaultTextField As String
        Get
            Return Me.TXBValore.UniqueID
        End Get
    End Property
End Class