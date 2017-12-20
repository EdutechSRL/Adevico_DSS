Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.Authentication
Imports System.Linq

Public Class UC_IMfieldsMatcher
    Inherits BaseControl
    Implements IViewFieldsMatcher

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As FieldsMatcherPresenter
    Private ReadOnly Property CurrentPresenter() As FieldsMatcherPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FieldsMatcherPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewFieldsMatcher.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Private Property AddPassword As Boolean Implements IViewFieldsMatcher.AddPassword
        Get
            Return (Me.SelectedIdProvider = IdInternalProvider) AndAlso Me.CBXallowPWDinsert.Checked
        End Get
        Set(value As Boolean)
            Me.CBXallowPWDinsert.Checked = value
        End Set
    End Property
    Private Property AutoGenerateLogin As Boolean Implements IViewFieldsMatcher.AutoGenerateLogin
        Get
            Return CBXdefaultLogin.Checked
        End Get
        Set(value As Boolean)
            CBXdefaultLogin.Checked = value
        End Set
    End Property
    Private Property SelectedIdProvider As Long Implements IViewFieldsMatcher.SelectedIdProvider
        Get
            If Me.DDLauthenticationProviders.SelectedIndex < 0 Then
                Return 0
            Else
                Return CLng(Me.DDLauthenticationProviders.SelectedValue)
            End If
        End Get
        Set(value As Long)
            Try
                Me.DDLauthenticationProviders.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Private Property SelectedProfileTypeId As Integer Implements IViewFieldsMatcher.SelectedProfileTypeId
        Get
            If Me.RBLuserTypes.SelectedIndex < 0 Then
                Return 0
            Else
                Return Me.RBLuserTypes.SelectedValue
            End If
        End Get
        Set(ByVal value As Integer)
            Try
                Me.RBLuserTypes.SelectedValue = value
            Catch ex As Exception

            End Try
            If RBLuserTypes.SelectedIndex > -1 Then
                DVagency.Visible = (Me.RBLuserTypes.SelectedValue = UserTypeStandard.Employee)
            End If
        End Set
    End Property
    Public Property Fields As List(Of ProfileColumnComparer(Of String)) Implements IViewFieldsMatcher.Fields
        Get
            Dim results As New List(Of ProfileColumnComparer(Of String))
            Dim MaxNumber As Integer = 0
            For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim column As New ProfileColumnComparer(Of String)
                column.SourceColumn = DirectCast(row.FindControl("LBsourceName"), Label).Text
                Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
                Try
                    column.DestinationColumn = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileAttributeType).GetByString(oDropDownList.SelectedValue, ProfileAttributeType.skip)
                Catch ex As Exception
                    column.DestinationColumn = ProfileAttributeType.skip
                End Try
                Dim number As String = DirectCast(row.FindControl("LTcolumnNumber"), Literal).Text
                Integer.TryParse(number, column.Number)
                If MaxNumber < column.Number Then
                    MaxNumber = column.Number
                End If
                results.Add(column)
            Next

            'If AutoGenerateLogin AndAlso results.Where(Function(c) c.DestinationColumn = ProfileAttributeType.login).Any() = False Then
            '    Dim column As New ProfileColumnComparer(Of String)
            '    column.SourceColumn = ""
            '    column.DestinationColumn = ProfileAttributeType.login
            '    column.Number = MaxNumber + 1
            '    results.Add(column)
            'End If
            Return results
        End Get
        Set(value As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String)))

        End Set
    End Property
    Public Property Columns As List(Of ProfileColumnComparer(Of String)) Implements IViewFieldsMatcher.Columns
        Get
            Return ViewStateOrDefault("Columns", New List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String)))
        End Get
        Set(value As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String)))
            ViewState("Columns") = value
        End Set
    End Property
    Private ReadOnly Property AddTaxCode As Boolean Implements IViewFieldsMatcher.AddTaxCode
        Get
            Return SystemSettings.Presenter.DefaultTaxCodeRequired
        End Get
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewFieldsMatcher.isValid
        Get
            Dim results As List(Of dtoInvalidMatch(Of ProfileAttributeType)) = Me.CurrentPresenter.GetInvalidItems(Fields)

            ChangeAllDestinationColumnClass("")
            If IsNothing(results) OrElse results.Count = 0 Then
                Me.LBinfo.Text = ""
                Return True
            Else
                If Me.RPTmatcher.Items.Count > 1 AndAlso (Me.RPTmatcher.Items.Count - 1) = results.Where(Function(r) r.InvalidMatch = InvalidMatch.IgnoredRequiredItem).Count Then
                    Me.LBinfo.Text = Resource.getValue("InvalidMatch." & InvalidMatch.IgnoredAllItems.ToString)
                Else
                    Dim temp As String = ""
                    Dim translations As List(Of TranslatedItem(Of ProfileAttributeType)) = TranslatedAttributesForSearch
                    Dim ignoredRequiredItem As List(Of dtoInvalidMatch(Of ProfileAttributeType)) = results.Where(Function(r) r.InvalidMatch = InvalidMatch.IgnoredRequiredItem).ToList()
                   
                    temp = TranslateErrors(results, InvalidMatch.DuplicatedItem, translations)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    temp &= TranslateErrors(results, InvalidMatch.IgnoredAlternativeRequiredItem, translations)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    temp &= TranslateErrors(results, InvalidMatch.IgnoredRequiredItem, translations)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    Me.LBinfo.Text = temp
                    ChangeDestinationColumnClass(results, "DDLerror")
                End If
                Return False
            End If
        End Get
    End Property
    Private Function TranslateErrors(sourceItems As List(Of dtoInvalidMatch(Of ProfileAttributeType)), ByVal match As InvalidMatch, translations As List(Of TranslatedItem(Of ProfileAttributeType))) As String
        Dim translation As String = ""
        Dim items As List(Of dtoInvalidMatch(Of ProfileAttributeType)) = sourceItems.Where(Function(r) r.InvalidMatch = match).ToList()

        If items.Count = 1 Then
            translation &= String.Format(Resource.getValue("InvalidMatch." & match.ToString), translations.Where(Function(t) t.Id = items(0).Attribute).Select(Function(t) t.Translation).FirstOrDefault())
        ElseIf items.Count > 1 Then
            translation &= String.Format(Resource.getValue("InvalidMatch." & match.ToString), String.Join(", ", translations.Where(Function(t) items.Select(Function(d) d.Attribute).ToList.Contains(t.Id)).Select(Function(t) t.Translation).ToArray))
        End If
        Return translation
    End Function
    Private Property IdInternalProvider As Long
        Get
            Return ViewStateOrDefault("IdInternalProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdInternalProvider") = value
        End Set
    End Property
    Private Property Attributes As List(Of dtoProfileAttributeType)
        Get
            Return ViewStateOrDefault("Attributes", New List(Of dtoProfileAttributeType))
        End Get
        Set(value As List(Of dtoProfileAttributeType))
            ViewState("Attributes") = value
        End Set
    End Property

    Private ReadOnly Property TranslatedAttributes As List(Of TranslatedItem(Of dtoProfileAttributeType))
        Get
            Dim items As List(Of dtoProfileAttributeType) = Attributes
            Dim result As New List(Of TranslatedItem(Of dtoProfileAttributeType))
            Dim skipItem As New TranslatedItem(Of dtoProfileAttributeType) With {.Id = New dtoProfileAttributeType(ProfileAttributeType.skip, False), .Translation = Me.Resource.getValue("ProfileAttributeType.skip")}

            If IsNothing(items) OrElse items.Count = 0 Then
                result.Add(skipItem)
            Else
                result = (From s In items.Where(Function(i) i.Attribute <> ProfileAttributeType.skip) Select New TranslatedItem(Of dtoProfileAttributeType) With {.Id = s, .Translation = IIf(s.Mandatory AndAlso s.UserType <> UserTypeStandard.AllUserType, "(*)", "") & Me.Resource.getValue("ProfileAttributeType." & s.Attribute.ToString)}).OrderBy(Function(t) t.Translation).ToList()
                If items.Select(Function(i) i.Attribute).ToList().Contains(ProfileAttributeType.skip) Then
                    result.Insert(0, skipItem)
                End If
            End If
            Return result
        End Get
    End Property
    Private ReadOnly Property TranslatedAttributesForSearch As List(Of TranslatedItem(Of ProfileAttributeType))
        Get
            Dim items As List(Of dtoProfileAttributeType) = Attributes
            Dim result As New List(Of TranslatedItem(Of ProfileAttributeType))
            Dim skipItem As New TranslatedItem(Of ProfileAttributeType) With {.Id = ProfileAttributeType.skip, .Translation = Me.Resource.getValue("ProfileAttributeType.skip")}

            If IsNothing(items) OrElse items.Count = 0 Then
                result.Add(skipItem)
            Else
                result = (From s In items.Where(Function(i) i.Attribute <> ProfileAttributeType.skip) Select New TranslatedItem(Of ProfileAttributeType) With {.Id = s.Attribute, .Translation = Me.Resource.getValue("ProfileAttributeType." & s.Attribute.ToString)}).OrderBy(Function(t) t.Translation).ToList()
                If items.Select(Function(i) i.Attribute).ToList().Contains(ProfileAttributeType.skip) Then
                    result.Insert(0, skipItem)
                End If
            End If
            Return result
        End Get
    End Property
    Public Property ImportSettings As dtoImportSettings Implements IViewFieldsMatcher.ImportSettings
        Get
            Dim result As New dtoImportSettings
            result.AddPassword = AddPassword
            result.AddTaxCode = SystemSettings.Presenter.DefaultTaxCodeRequired
            result.AutoGenerateLogin = AutoGenerateLogin
            result.IdProfileType = SelectedProfileTypeId
            result.IdProvider = SelectedIdProvider
            result.DefaultNationId = SystemSettings.Presenter.DefaultNationID
            result.DefaultProvinceId = SystemSettings.Presenter.DefaultProvinceID
            result.DefaultAgency = CTRLagency.SelectedLongItem
            Return result
        End Get
        Set(value As dtoImportSettings)
            AddPassword = value.AddPassword
            AutoGenerateLogin = value.AutoGenerateLogin
            SelectedProfileTypeId = value.IdProfileType
            SelectedIdProvider = value.IdProvider
        End Set
    End Property
    
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Public ReadOnly Property SelectedAuthenticationTypeName As String
        Get
            If Me.DDLauthenticationProviders.SelectedIndex < 0 Then
                Return ""
            Else
                Return Me.DDLauthenticationProviders.SelectedItem.Text
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedProfileTypeName As String
        Get
            If Me.RBLuserTypes.SelectedIndex < 0 Then
                Return ""
            Else
                Return Me.RBLuserTypes.SelectedItem.Text
            End If
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBauthenticationProviders_t)
            .setLabel(LBprofileToSelect_t)
            .setLabel(LBallowPWDinsert_t)
            .setLabel(LBdefaultLogin_t)
            .setCheckBox(CBXallowPWDinsert)
            .setCheckBox(CBXdefaultLogin)
            .setLabel(LBdefaultAgency_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(columns As List(Of ProfileColumnComparer(Of String))) Implements IViewFieldsMatcher.InitializeControl
        Me.CurrentPresenter.InitView(columns)
    End Sub
    Public Sub InitializeControl(providers As List(Of dtoBaseProvider)) Implements IViewFieldsMatcher.InitializeControl
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList

        Me.RBLuserTypes.DataSource = oUserTypes
        Me.RBLuserTypes.DataValueField = "ID"
        Me.RBLuserTypes.DataTextField = "Descrizione"
        Me.RBLuserTypes.DataBind()

        If (From t As COL_TipoPersona In oUserTypes Where t.ID = Main.TipoPersonaStandard.StudenteStandard Select t.ID).Any Then
            Me.RBLuserTypes.SelectedValue = CInt(Main.TipoPersonaStandard.StudenteStandard)
            DVagency.Visible = False
        ElseIf oUserTypes.Count > 0 Then
            Me.RBLuserTypes.SelectedIndex = 0
            DVagency.Visible = (Me.RBLuserTypes.SelectedValue = UserTypeStandard.Employee)
        End If

        Me.DDLauthenticationProviders.Items.Clear()
        For Each provider As dtoBaseProvider In providers
            Me.DDLauthenticationProviders.Items.Add(New ListItem(provider.Translation.Name, provider.IdProvider))
        Next

        IdInternalProvider = (From p As dtoBaseProvider In providers Where p.Type = AuthenticationProviderType.Internal Select p.IdProvider).FirstOrDefault()
        If IdInternalProvider > 0 Then
            Me.DDLauthenticationProviders.SelectedValue = IdInternalProvider
            DVlogin.Visible = True
            DVpassword.Visible = True
        ElseIf providers.Count > 0 Then
            Me.DDLauthenticationProviders.SelectedIndex = 0
        End If
        Me.isInitialized = True
        Me.LBinfo.Text = ""
        Me.MLVcontrolData.SetActiveView(VIWmatchColumns)
    End Sub

    Private Sub LoadItems(source As List(Of ProfileColumnComparer(Of String)), attributesList As List(Of dtoProfileAttributeType)) Implements IViewFieldsMatcher.LoadItems
        Attributes = attributesList
        Me.RPTmatcher.DataSource = source
        Me.RPTmatcher.DataBind()
    End Sub
    Private Sub ReloadAvailableAttributes(columns As List(Of ProfileColumnComparer(Of String)), items As List(Of dtoProfileAttributeType)) Implements IViewFieldsMatcher.ReloadAvailableAttributes
        Attributes = items
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown = row.FindControl("DDLdestination")
            Dim selected As String = oDropDownList.SelectedValue
            If selected = ProfileAttributeType.skip.ToString() Then
                Dim literal As Literal = row.FindControl("LTcolumnNumber")
                If Not String.IsNullOrEmpty(Literal.Text) AndAlso IsNumeric(Literal.Text) Then
                    Dim number As Integer = CInt(literal.Text)
                    selected = columns.Where(Function(c) c.Number = number).Select(Function(c) c.DestinationColumn.ToString).FirstOrDefault()
                End If
            End If
            LoadAttributesIntoDropDown(oDropDownList, selected)
        Next

    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewFieldsMatcher.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub

    'Private Sub ChangeDestinationColumnClass(ByVal attribute As ProfileAttributeType, invalid As InvalidMatch, cssClass As String)
    '    For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")

    '        If oDropDownList.SelectedValue = attribute.ToString Then
    '            Dim oLabel As Label = row.FindControl("LBerror")

    '            If String.IsNullOrEmpty(cssClass) Then
    '                oDropDownList.CssClass = "Testo_Campo"
    '            Else
    '                oDropDownList.CssClass = "Testo_Campo" & " " & cssClass
    '                oLabel.ToolTip = Resource.getValue("InvalidMatch." & invalid.ToString)
    '            End If
    '            oLabel.Visible = Not String.IsNullOrEmpty(cssClass)
    '        End If
    '    Next
    'End Sub
    Private Sub ChangeDestinationColumnClass(items As List(Of dtoInvalidMatch(Of ProfileAttributeType)), cssClass As String)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
            Dim item As dtoInvalidMatch(Of ProfileAttributeType) = items.Where(Function(i) i.Attribute.ToString = oDropDownList.SelectedValue).FirstOrDefault()
            Dim oLabel As Label = row.FindControl("LBerror")

            If Not IsNothing(item) Then
                If String.IsNullOrEmpty(cssClass) Then
                    oDropDownList.CssClass = "Testo_Campo"
                Else
                    oDropDownList.CssClass = "Testo_Campo" & " " & cssClass
                    oLabel.ToolTip = Resource.getValue("Field.InvalidMatch." & item.InvalidMatch.ToString)
                End If
                oLabel.Visible = Not String.IsNullOrEmpty(cssClass)
            Else
                oDropDownList.CssClass = "Testo_Campo"
                oLabel.Visible = False
            End If
        Next
    End Sub
    Private Sub ChangeAllDestinationColumnClass(cssClass As String)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
            Dim oLabel As Label = row.FindControl("LBerror")
            If String.IsNullOrEmpty(cssClass) Then
                oDropDownList.CssClass = "Testo_Campo"
            Else
                oDropDownList.CssClass = "Testo_Campo" & " " & cssClass
            End If
            oLabel.Visible = False
        Next
    End Sub
#End Region

    Private Sub RPTmatcher_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmatcher.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType

        If itemType = ListItemType.Item OrElse itemType = ListItemType.AlternatingItem Then

            Dim item As ProfileColumnComparer(Of String) = DirectCast(e.Item.DataItem, ProfileColumnComparer(Of String))
            'Dim oLabel As Label = e.Item.FindControl("LBsourceName")
            'oLabel.Text = item.SourceColumn
            Dim oDropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown = e.Item.FindControl("DDLdestination")
            LoadAttributesIntoDropDown(oDropDownList, item.DestinationColumn.ToString)

        ElseIf itemType = ListItemType.Header Then

            Dim oLabel As Label = e.Item.FindControl("LBsourceColumn")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBdestinationColumn")
            Me.Resource.setLabel(oLabel)
        End If
    End Sub

    Private Sub CBXallowPWDinsert_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXallowPWDinsert.CheckedChanged
        Me.LBinfo.Text = ""
        Me.CurrentPresenter.ReloadItems(Me.SelectedProfileTypeId, Me.SelectedIdProvider, Me.AddTaxCode, AddPassword, AutoGenerateLogin)
    End Sub
    Private Sub CBXdefaultLogin_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXdefaultLogin.CheckedChanged
        Me.LBinfo.Text = ""
        Me.CurrentPresenter.ReloadItems(Me.SelectedProfileTypeId, Me.SelectedIdProvider, Me.AddTaxCode, AddPassword, AutoGenerateLogin)
    End Sub
    Private Sub DDLauthenticationProviders_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLauthenticationProviders.SelectedIndexChanged
        Me.LBinfo.Text = ""
        DVlogin.Visible = (Me.SelectedIdProvider = IdInternalProvider)
        DVpassword.Visible = (Me.SelectedIdProvider = IdInternalProvider)
        Me.CurrentPresenter.ReloadItems(Me.SelectedProfileTypeId, Me.SelectedIdProvider, Me.AddTaxCode, AddPassword, AutoGenerateLogin)
    End Sub
    Private Sub RBLuserTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLuserTypes.SelectedIndexChanged
        Me.LBinfo.Text = ""
        DVagency.Visible = (SelectedProfileTypeId = UserTypeStandard.Employee)
        If SelectedProfileTypeId = UserTypeStandard.Employee Then
            Me.CTRLagency.InitalizeControl()
            Me.CTRLagency.SelectedLongItem = CurrentPresenter.GetDefaultAgency()
        End If
        Me.CurrentPresenter.ReloadItems(Me.SelectedProfileTypeId, Me.SelectedIdProvider, Me.AddTaxCode, AddPassword, AutoGenerateLogin)
    End Sub

    Private Sub LoadAttributesIntoDropDown(ByVal dropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown, selectedValue As String)

        Dim items As List(Of TranslatedItem(Of dtoProfileAttributeType)) = TranslatedAttributes

        dropDownList.Items.Clear()

        Dim skipItem As TranslatedItem(Of dtoProfileAttributeType) = items.Where(Function(a) a.Id.Attribute = ProfileAttributeType.skip).FirstOrDefault()
        If Not IsNothing(skipItem) Then
            dropDownList.Items.Add(New ListItem() With {.Value = skipItem.Id.Attribute.ToString, .Text = skipItem.Translation})
        End If

        If (items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> ProfileAttributeType.skip).Any()) Then
            dropDownList.AddItemGroup(Resource.getValue("GroupBy.MandatoryCommon"))
            AddItems(dropDownList, items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList())
        End If
        If (items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType).Any()) Then
            If Me.RBLuserTypes.SelectedIndex > -1 Then
                dropDownList.AddItemGroup(String.Format(Resource.getValue("GroupBy.UserTypeStandard"), Me.RBLuserTypes.SelectedItem.Text))
            Else
                dropDownList.AddItemGroup(String.Format(Resource.getValue("GroupBy.UserTypeStandard"), " -- "))
            End If

            AddItems(dropDownList, items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType).OrderBy(Function(i) i.Translation).ToList())
        End If
        If (items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> ProfileAttributeType.skip).Any()) Then
            dropDownList.AddItemGroup(Resource.getValue("GroupBy.NotMandatoryCommon"))
            AddItems(dropDownList, items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList())
        End If
        Try
            If String.IsNullOrEmpty(selectedValue) Then
                dropDownList.SelectedValue = ProfileAttributeType.skip.ToString
            Else
                dropDownList.SelectedValue = selectedValue
            End If
        Catch ex As Exception
            dropDownList.SelectedValue = ProfileAttributeType.skip.ToString
        End Try
    End Sub

    Private Sub AddItems(ByVal dropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown, items As List(Of TranslatedItem(Of dtoProfileAttributeType)))
        For Each item As TranslatedItem(Of dtoProfileAttributeType) In items
            dropDownList.Items.Add(New ListItem() With {.Value = item.Id.Attribute.ToString, .Text = item.Translation})
        Next
    End Sub

 
End Class