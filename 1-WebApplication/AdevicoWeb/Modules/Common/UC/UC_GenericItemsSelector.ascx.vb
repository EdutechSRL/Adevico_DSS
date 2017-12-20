Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports System.Linq
Imports lm.Comol.Core.BaseModules.Repository.Presentation

Public Class UC_GenericItemsSelector
    Inherits BaseControl
    Implements IViewGenericItemsSelector



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
    Private _Presenter As GenericItemsSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As GenericItemsSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenericItemsSelectorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewGenericItemsSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property ItemsToSelect As Integer Implements IViewGenericItemsSelector.ItemsToSelect
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
    Public ReadOnly Property HasSelectedItems As Boolean Implements IViewGenericItemsSelector.HasSelectedItems
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

    Public ReadOnly Property SelectedItems As ExternalResource Implements IViewGenericItemsSelector.SelectedItems
        Get
            Dim repeater As Repeater = Nothing
            If RPTitems.Items.Count = 1 Then
                repeater = RPTitems.Items(0).FindControl("RPTitemRow")
            End If
            Dim rowItems As New List(Of List(Of String))
            If Not IsNothing(repeater) Then
                Dim query = (From p As RepeaterItem In repeater.Items Where DirectCast(p.FindControl("CBXselect"), HtmlInputCheckBox).Checked Select p)
                For Each r As RepeaterItem In query
                    Dim row As New List(Of String)

                    Dim itemsRepeater As Repeater = r.FindControl("RPTcells")
                    row.AddRange((From c As RepeaterItem In itemsRepeater.Items Select TryCast(c.FindControl("LTitem"), Literal).Text).ToList())
                    rowItems.Add(row)
                Next
            End If
            Return New ExternalResource(Columns, rowItems)
        End Get
    End Property
    Private Property Columns As List(Of ExternalColumn) Implements IViewGenericItemsSelector.Columns
        Get
            Return ViewStateOrDefault("Columns", New List(Of ExternalColumn))
        End Get
        Set(value As List(Of ExternalColumn))
            ViewState("Columns") = value
        End Set
    End Property
    Public ReadOnly Property SelectedItemsCount As Integer Implements IViewGenericItemsSelector.SelectedItemsCount
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
    Protected ReadOnly Property isImported(row As ExternalRow) As String
        Get
            Return IIf(row.HasDBDuplicatedValues, "imported", IIf(Not row.AllowImport, "off", ""))
        End Get
    End Property
    Private Property SelectedRows As List(Of Integer) Implements IViewGenericItemsSelector.SelectedRows
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
        MyBase.SetCulture("pg_CSVgenericImport", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(source As ExternalResource, noDBfields As List(Of ExternalColumnComparer(Of String, Integer)), notEmptyColumns As List(Of Integer), notDuplicatedColumns As List(Of Integer)) Implements IViewGenericItemsSelector.InitializeControl
        Me.CurrentPresenter.InitView(source, noDBfields, notEmptyColumns, notDuplicatedColumns)
    End Sub
    Public Sub InitializeControlAfterImport(source As ExternalResource, noDBfields As List(Of ExternalColumnComparer(Of String, Integer)), notEmptyColumns As List(Of Integer), notDuplicatedColumns As List(Of Integer)) Implements IViewGenericItemsSelector.InitializeControlAfterImport
        Dim repeater As Repeater = Nothing
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
        Me.CurrentPresenter.InitView(source, noDBfields, notEmptyColumns, notDuplicatedColumns)
    End Sub
    Public Event RequireDBValidation(item As DestinationItem(Of Integer), cells As List(Of ExternalCell), ByRef errors As List(Of ExternalCell))

    Public Function ValidateDBItems(item As DestinationItem(Of Integer), cells As List(Of ExternalCell)) As List(Of ExternalCell) Implements IViewGenericItemsSelector.ValidateDBItems
        Dim errors As New List(Of ExternalCell)
        RaiseEvent RequireDBValidation(item, cells, errors)
        Return errors
    End Function

    Public Sub InitializeControl(source As ExternalResource, noDBfields As List(Of ExternalColumnComparer(Of String, Integer)), notEmptyColumns As List(Of ExternalColumnComparer(Of String, Integer)), notDuplicatedColumns As List(Of ExternalColumnComparer(Of String, Integer))) Implements IViewGenericItemsSelector.InitializeControl
        Me.CurrentPresenter.InitView(source, noDBfields, notEmptyColumns, notDuplicatedColumns)
    End Sub

    Public Sub InitializeControlAfterImport(source As ExternalResource, noDBfields As List(Of ExternalColumnComparer(Of String, Integer)), notEmptyColumns As List(Of ExternalColumnComparer(Of String, Integer)), notDuplicatedColumns As List(Of ExternalColumnComparer(Of String, Integer))) Implements IViewGenericItemsSelector.InitializeControlAfterImport
        Dim repeater As Repeater = Nothing
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
        Me.CurrentPresenter.InitView(source, noDBfields, notEmptyColumns, notDuplicatedColumns)
    End Sub

    Public Event RequireDBValidationAlternativeItems(items As List(Of DestinationItemCells(Of Integer)), ByRef errors As List(Of ExternalCell))
    Public Function ValidateAlternateDBItems(items As List(Of DestinationItemCells(Of Integer))) As List(Of ExternalCell) Implements IViewGenericItemsSelector.ValidateAlternateDBItems
        Dim errors As New List(Of ExternalCell)
        RaiseEvent RequireDBValidationAlternativeItems(items, errors)
        Return errors
    End Function

    Public Sub LoadItems(items As List(Of ExternalResource), invalidItems As List(Of InvalidImport)) Implements IViewGenericItemsSelector.LoadItems
        Me.MLVcontrolData.SetActiveView(VIWselectItems)
        RPTinvalidItems.Visible = invalidItems.Any
        If invalidItems.Any Then
            Me.RPTinvalidItems.DataSource = invalidItems
            Me.RPTinvalidItems.DataBind()
        End If
        Me.RPTitems.DataSource = items
        Me.RPTitems.DataBind()
        SelectedRows = New List(Of Integer)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewGenericItemsSelector.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
#End Region


    Protected Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLiteral As Literal = e.Item.FindControl("LTselectRow_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTrowInfo_t")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
    Protected Sub RPTitemRow_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim row As ExternalRow = DirectCast(e.Item.DataItem, ExternalRow)
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

    Private Function GenerateErrorToolTip(row As ExternalRow) As String
        Dim errors As String = ""
        If row.Cells.Where(Function(c) Not c.isValid).Any Then
            errors = TranslateInvalidItem(InvalidImport.InvalidData) & ": " & String.Join(" ", row.Cells.Where(Function(c) Not c.isValid).OrderBy(Function(c) c.Column.Number).Select(Function(c) c.Column.Name).ToArray)
        End If
        If row.Cells.Where(Function(c) c.isDuplicate).Any Then
            If Not String.IsNullOrEmpty(errors) Then
                errors = " / "
            End If
            errors = TranslateInvalidItem(InvalidImport.SourceDuplicatedData) & ": " & String.Join(" ", row.Cells.Where(Function(c) c.isDuplicate).OrderBy(Function(c) c.Column.Number).Select(Function(c) c.Column.Name).ToArray)
        End If
        If row.Cells.Where(Function(c) c.isDBduplicate).Any Then
            If Not String.IsNullOrEmpty(errors) Then
                errors = " / "
            End If
            errors = TranslateInvalidItem(InvalidImport.AlreadyExist) & ": " & String.Join(" ", row.Cells.Where(Function(c) c.isDBduplicate).OrderBy(Function(c) c.Column.Number).Select(Function(c) c.Column.Name).ToArray)
        End If
        Return errors
    End Function

    Protected Sub RPTcolumns_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim column As ExternalColumn = DirectCast(e.Item.DataItem, ExternalColumn)
            Dim oLiteral As Literal = e.Item.FindControl("LTcolumn")

            oLiteral.Text = column.Name
        End If
    End Sub
    Protected Sub RPTcells_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As ExternalCell = DirectCast(e.Item.DataItem, ExternalCell)
            Dim oLiteral As Literal = e.Item.FindControl("LTitem")
            Dim invalidString As String = ""
            If Not item.isValid Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.InvalidData) & """>" & Resource.getValue("InvalidImport." & InvalidImport.InvalidData.ToString) & "</span>" & " "
            End If
            If item.isDuplicate Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.SourceDuplicatedData) & """>" & Resource.getValue("InvalidImport." & InvalidImport.SourceDuplicatedData.ToString) & "</span>" & " "
            End If
            If item.isDBduplicate Then
                invalidString &= "<span title=""" & TranslateInvalidItem(InvalidImport.AlreadyExist) & """>" & Resource.getValue("InvalidImport." & InvalidImport.AlreadyExist.ToString) & "</span>"
            End If

            If String.IsNullOrEmpty(item.Value) Then
                oLiteral.Visible = False
                oLiteral = e.Item.FindControl("LTitemEmpty")
                oLiteral.Visible = True
            Else
                oLiteral.Text = item.Value
            End If
            oLiteral = e.Item.FindControl("LTitemInfo")
            oLiteral.Text = invalidString
        End If
    End Sub
   
End Class