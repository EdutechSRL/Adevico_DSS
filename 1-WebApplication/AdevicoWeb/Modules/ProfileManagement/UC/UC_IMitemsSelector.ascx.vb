Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.Authentication
Imports System.Linq

Public Class UC_IMitemsSelector
    Inherits BaseControl
    Implements IViewItemsSelector

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
    Private _Presenter As ItemsSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As ItemsSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemsSelectorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewItemsSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property TranslatedAutoLogin As String Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewItemsSelector.TranslatedAutoLogin
        Get
            Return "Login utente"
        End Get
    End Property
    Public ReadOnly Property AddTaxCode As Boolean Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewItemsSelector.AddTaxCode
        Get
            Return SystemSettings.Presenter.DefaultTaxCodeRequired
        End Get
    End Property

    Public Property AutoGenerateLogin As Boolean Implements IViewItemsSelector.AutoGenerateLogin
        Get
            Return ViewStateOrDefault("AutoGenerateLogin", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoGenerateLogin") = value
        End Set
    End Property

    Public ReadOnly Property ItemsToSelect As Integer Implements IViewItemsSelector.ItemsToSelect
        Get
            Dim repeater As Repeater = Nothing
            If RPTitems.Items.Count = 1 Then
                repeater = RPTitems.Items(0).FindControl("RPTitemRow")
            End If

            If Not IsNothing(repeater) Then
                Return (From r As RepeaterItem In repeater.Items Where DirectCast(r.FindControl("CBXselect"), HtmlInputCheckBox).Disabled = False).Count
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property HasSelectedItems As Boolean Implements IViewItemsSelector.HasSelectedItems
        Get
            Dim repeater As Repeater = Nothing
            If RPTitems.Items.Count = 1 Then
                repeater = RPTitems.Items(0).FindControl("RPTitemRow")
            End If

            If Not IsNothing(repeater) Then
                Return (From r As RepeaterItem In repeater.Items Where DirectCast(r.FindControl("CBXselect"), HtmlInputCheckBox).Checked).Any()
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedItems As ProfileExternalResource Implements IViewItemsSelector.SelectedItems
        Get
            Dim repeater As Repeater = Nothing
            If RPTitems.Items.Count = 1 Then
                repeater = RPTitems.Items(0).FindControl("RPTitemRow")
            End If
            Dim rowItems As New List(Of List(Of String))
            If Not IsNothing(repeater) Then
                Dim selectedRows As List(Of Integer) = (From p As RepeaterItem In repeater.Items Where DirectCast(p.FindControl("CBXselect"), HtmlInputCheckBox).Checked Select CInt(TryCast(p.FindControl("LTrowIndex"), Literal).Text)).ToList()
                Dim source As ProfileExternalResource = SourceItems
                Dim result As New ProfileExternalResource
                result.ColumHeader = SourceItems.ColumHeader
                result.Rows = source.Rows.Where(Function(r) selectedRows.Contains(r.Number)).ToList
                Return result
            End If
            Return New ProfileExternalResource(SourceColumns(), rowItems)
        End Get
    End Property

    'Private Function GetColumns() As List(Of ProfileAttributeColumn)
    '    Dim columns As New List(Of ProfileAttributeColumn)
    '    Dim repeater As Repeater = Nothing
    '    If RPTitems.Items.Count = 1 Then
    '        repeater = RPTitems.Items(0).FindControl("RPTcolumns")
    '    End If
    '    If Not IsNothing(repeater) Then
    '        For Each r As RepeaterItem In repeater.Items
    '            Dim column As New ProfileAttributeColumn
    '            column.Number = Int32.Parse(TryCast(r.FindControl("LTcolumnIndex"), Literal).Text)
    '            column.Attribute = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileAttributeType).GetByString(TryCast(r.FindControl("LTcolumnAttribute"), Literal).Text, ProfileAttributeType.skip)
    '            columns.Add(column)
    '        Next
    '    End If
    '    Return columns
    'End Function
    Private Property SourceColumns As List(Of ProfileAttributeColumn)
        Get
            Return ViewStateOrDefault("SourceColumns", New List(Of ProfileAttributeColumn))
        End Get
        Set(value As List(Of ProfileAttributeColumn))
            Me.ViewState("SourceColumns") = value
        End Set
    End Property
    Private Property SourceItems As ProfileExternalResource
        Get
            Return ViewStateOrDefault("SourceItems", New ProfileExternalResource())
        End Get
        Set(value As ProfileExternalResource)
            Me.ViewState("SourceItems") = value
        End Set
    End Property

    Public ReadOnly Property SelectedItemsCount As Integer Implements IViewItemsSelector.SelectedItemsCount
        Get
            Dim repeater As Repeater = Nothing
            If RPTitems.Items.Count = 1 Then
                repeater = RPTitems.Items(0).FindControl("RPTitemRow")
            End If
            Dim rowItems As New List(Of List(Of String))
            If Not IsNothing(repeater) Then
                Return (From p As RepeaterItem In repeater.Items Where DirectCast(p.FindControl("CBXselect"), HtmlInputCheckBox).Checked Select p).Count
            Else : Return 0
            End If
        End Get
    End Property

    Private Property SelectedRows As List(Of Integer) Implements IViewItemsSelector.SelectedRows
        Get
            Return ViewStateOrDefault("SelectedRows", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("SelectedRows") = value
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

    Protected ReadOnly Property TranslateInvalidItem(invalidItem As InvalidImport) As String
        Get
            Return Resource.getValue("InvalidImport." & invalidItem.ToString() + ".Description")
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
            .setLabel(LBselectedProvider_t)
            .setLabel(LBselectedProfile_t)

            '.setLabel(LBauthenticationProviders_t)
            '.setLabel(LBprofileToSelect_t)
            '.setLabel(LBallowPWDinsert_t)
            '.setLabel(LBdefaultLogin_t)
            '.setCheckBox(CBXallowPWDinsert)
            '.setCheckBox(CBXdefaultLogin)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(source As ProfileExternalResource, settings As dtoImportSettings) Implements IViewItemsSelector.InitializeControl
        Me.SourceColumns = Nothing
        Me.CurrentPresenter.InitView(source, settings)
    End Sub
    Public Sub InitializeControlAfterImport(source As ProfileExternalResource, settings As dtoImportSettings) Implements IViewItemsSelector.InitializeControlAfterImport
        Dim repeater As Repeater = Nothing
        Me.SourceColumns = Nothing
        If RPTitems.Items.Count = 1 Then
            repeater = RPTitems.Items(0).FindControl("RPTitemRow")
        End If
        Dim selectedItems As New List(Of Integer)
        If Not IsNothing(repeater) Then
            Dim query = (From p As RepeaterItem In repeater.Items Where DirectCast(p.FindControl("CBXselect"), HtmlInputCheckBox).Checked Select p)
            For Each r As RepeaterItem In query
                Dim oliteral As Literal = r.FindControl("LTrowIndex")
                selectedItems.Add(CInt(oliteral.Text))
            Next
        End If
        SelectedRows = selectedItems
        Me.CurrentPresenter.InitView(source, settings)
    End Sub

    Public Sub LoadItems(items As List(Of ProfileExternalResource), invalidItems As List(Of InvalidImport), idProfileType As Integer, providerName As String) Implements IViewItemsSelector.LoadItems
        Me.MLVcontrolData.SetActiveView(VIWselectItems)
        Me.LBselectedProvider.Text = providerName

        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList
        RPTinvalidItems.Visible = invalidItems.Any
        If invalidItems.Any Then
            Me.RPTinvalidItems.DataSource = invalidItems
            Me.RPTinvalidItems.DataBind()
        End If
        Me.LBselectedProfile.Text = oUserTypes.Where(Function(t) t.ID = idProfileType).Select(Function(t) t.Descrizione).FirstOrDefault
        Me.RPTitems.DataSource = items
        Me.RPTitems.DataBind()
        SelectedRows = New List(Of Integer)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewItemsSelector.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
#End Region

    'Public Function GetItemRowClass(row As ProfileAttributesRow) As String
    '    Dim iResult As String = "off"

    '    If row.isValid AndAlso Not row.HasDuplicatedValues AndAlso Not row.Cells.Where(Function(c) c.isDBduplicate).Any() Then
    '        iResult = "on"
    '    End If
    '    Return iResult
    'End Function

    Protected Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim source As ProfileExternalResource = DirectCast(e.Item.DataItem, ProfileExternalResource)
            Dim oLiteral As Literal = e.Item.FindControl("LTselectRow_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTrowInfo_t")
            Resource.setLiteral(oLiteral)
            Me.SourceItems = source
            'Me.SourceColumns = source.ColumHeader
        End If
    End Sub
    Protected Sub RPTitemRow_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim row As ProfileAttributesRow = DirectCast(e.Item.DataItem, ProfileAttributesRow)
            Dim oChecbox As HtmlInputCheckBox = e.Item.FindControl("CBXselect")
            oChecbox.Visible = row.isValid
            oChecbox.Disabled = Not row.isValid
            oChecbox.Checked = row.isValid AndAlso Not row.HasDuplicatedValues AndAlso (SelectedRows.Count = 0 OrElse (SelectedRows.Contains(row.Number)))
            oChecbox.Value = row.Number
            If row.HasDuplicatedValues Then
                Dim disable As String = ""
                Dim value As String = ","
                For Each cell As Integer In row.DuplicatedRows.Where(Function(r) r <> row.Number).ToList
                    disable &= "$(""input:checkbox[value=" & cell & "]"").attr(""checked"", false);"
                    disable &= "$(""input:checkbox[value=" & cell & "]"").parent().parent().addClass('off');"

                    value = cell & ","
                Next

                If Not String.IsNullOrEmpty(disable) Then
                    oChecbox.Attributes.Add("valueToDisable", value)
                    'oChecbox.Attributes.Add("onclick", "if ($(this).is(':checked')) {" & disable & "}")
                End If
            End If


            Dim oLabel As Label = e.Item.FindControl("LBrowInfo")

            If Not row.AllowImport OrElse row.HasDuplicatedValues OrElse row.HasDBDuplicatedValues OrElse Not row.isValid Then
                oLabel.ToolTip = GenerateErrorToolTip(row)
                oLabel.Text = "#"
                oLabel.CssClass = "img_span ico_warning_s"
            Else
                oLabel.Text = " "
                oLabel.CssClass = ""
            End If

            'If Not row.AllowImport Then
            '    ' ITEMS NOT available for import

            'ElseIf Not row.isValid Then
            '    ' ITEMS with invalid data
            '    oLabel.Text = "#" '"dati invalidi"
            '    oLabel.ToolTip = GenerateErrorToolTip(row)
            'Else
            If row.Cells.Where(Function(c) c.isDBduplicate).Any() Then
                oChecbox.Disabled = True
                oChecbox.Checked = False
            End If
        End If


    End Sub

    Private Function GenerateErrorToolTip(row As ProfileAttributesRow) As String
        Dim errors As String = ""
        If row.Cells.Where(Function(c) Not c.isValid).Any Then
            errors = TranslateInvalidItem(InvalidImport.InvalidData) & ": " & String.Join(" ", row.Cells.Where(Function(c) Not c.isValid).OrderBy(Function(c) c.Column.Number).Select(Function(c) Resource.getValue("ProfileAttributeType." & c.Column.Attribute.ToString)).ToArray)
        End If
        If row.Cells.Where(Function(c) c.isDuplicate).Any Then
            If Not String.IsNullOrEmpty(errors) Then
                errors = " / "
            End If
            errors = TranslateInvalidItem(InvalidImport.SourceDuplicatedData) & ": " & String.Join(" ", row.Cells.Where(Function(c) c.isDuplicate).OrderBy(Function(c) c.Column.Number).Select(Function(c) Resource.getValue("ProfileAttributeType." & c.Column.Attribute.ToString)).ToArray)
        End If
        If row.Cells.Where(Function(c) c.isDBduplicate).Any Then
            If Not String.IsNullOrEmpty(errors) Then
                errors = " / "
            End If
            errors = TranslateInvalidItem(InvalidImport.AlreadyExist) & ": " & String.Join(" ", row.Cells.Where(Function(c) c.isDBduplicate).OrderBy(Function(c) c.Column.Number).Select(Function(c) Resource.getValue("ProfileAttributeType." & c.Column.Attribute.ToString)).ToArray)
        End If
        Return errors
    End Function

    Protected Sub RPTcolumns_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim column As ProfileAttributeColumn = DirectCast(e.Item.DataItem, ProfileAttributeColumn)
            Dim oLiteral As Literal = e.Item.FindControl("LTcolumn")

            oLiteral.Text = Resource.getValue("ProfileAttributeType." & column.Attribute.ToString)
        End If
    End Sub
    Protected Sub RPTcells_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As ProfileAttributeCell = DirectCast(e.Item.DataItem, ProfileAttributeCell)
            Dim oLiteral As Literal = e.Item.FindControl("LTitem")
            Dim invalidString As String = " "
            If Not item.isValid Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.InvalidData) & """>" & Resource.getValue("InvalidImport." & InvalidImport.InvalidData.ToString) & "</span>" & " "
            End If
            If item.isDuplicate Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.SourceDuplicatedData) & """>" & Resource.getValue("InvalidImport." & InvalidImport.SourceDuplicatedData.ToString) & "</span>" & " "
            End If
            If item.isDBduplicate Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.AlreadyExist) & """>" & Resource.getValue("InvalidImport." & InvalidImport.AlreadyExist.ToString) & "</span>"
            End If
            Dim displayText As String = ""
            If TypeOf item Is ProfileAttributeComputedCell Then
                displayText = DirectCast(item, ProfileAttributeComputedCell).DisplayValue
            Else
                displayText = item.Value
            End If

            If displayText.Contains(lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator) Then
                Dim div As HtmlControl = e.Item.FindControl("DVmultiple")
                div.Visible = True
                Dim oControl As UCtoolTip = e.Item.FindControl("CTRLtoolTip")
                Dim myDelims As String() = New String() {lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator}
                Dim values As List(Of String) = item.Value.Split(myDelims, StringSplitOptions.RemoveEmptyEntries).ToList
                oControl.InitializeControl(Resource.getValue("Details"), values)
                Dim oDisplay As Literal = e.Item.FindControl("LTdisplayName")
                oDisplay.Text = invalidString.Trim & values.Count.ToString & " " & Resource.getValue("ProfileAttributeType." & item.Column.Attribute.ToString)
            Else
                oLiteral.Text = invalidString.Trim & IIf(String.IsNullOrEmpty(displayText), "&nbsp;", displayText)
            End If

            If item.Column.Attribute = ProfileAttributeType.autoGeneratedLogin AndAlso (Not item.Row.AllowImport OrElse item.Row.HasDBDuplicatedValues) Then
                oLiteral.Text = "   //   "
            End If

        End If
    End Sub

    Protected ReadOnly Property isImported(row As ProfileAttributesRow) As String
        Get
            Return IIf(row.HasDBDuplicatedValues, "imported", IIf(Not row.AllowImport, "off", ""))
        End Get
    End Property
    Protected ReadOnly Property GetCellCss(col As ProfileAttributeColumn) As String
        Get
            If IsNothing(col) Then
                Return ""
            Else
                Return GetColumnCss(col.Type)
            End If
        End Get
    End Property
    Protected ReadOnly Property GetColumnCss(type As lm.Comol.Core.DomainModel.Helpers.ColumnType) As String
        Get
            Select Case type
                Case ColumnType.computed
                    Return "computed"
                Case Else
                    Return ""
            End Select
        End Get
    End Property
End Class