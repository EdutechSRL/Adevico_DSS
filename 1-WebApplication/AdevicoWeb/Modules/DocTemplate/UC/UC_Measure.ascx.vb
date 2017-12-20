Imports TemplateVersHelper = lm.Comol.Core.DomainModel.DocTemplateVers.Helpers
''' <summary>
''' UC per l'immissione e la modifica di dimensioni (mm, cm, px, inch), formato da:
''' - Label come etichetta del campo
''' - TextBox per la modifica del valore
''' - DropDownList per l'impostazione delle misure.
''' 
''' Utilizzo:
''' Per impostare o leggere il valore utilizzare le property: mm, cm, px ed inch con le relative misure.
''' Per INIZIALIZZARE il controllo ed impostare la misura selezionata, utilizzare SetMeasure() nelle sue varianti.
''' ShoDot indica se verranno o meno motrati i px come unità di misura.''' 
''' </summary>
''' <remarks>Si basa su una risoluzione di 72 px/inch, come da direttive iTextsharp.</remarks>
Public Class UC_Measure
    Inherits System.Web.UI.UserControl

#Region "Internal"
    Public Event SelectedMeasureChange()

    Private _PixelValue As Single
    Private _ShowDot As Boolean

    Public Property Px As Single
        Get
            GetValue()
            Return _PixelValue
        End Get
        Set(value As Single)
            _PixelValue = Math.Round(value, 0)
            SetValue()
        End Set
    End Property
    Public Property cm
        Get
            GetValue()
            Return TemplateVersHelper.Measure.Px_To_cm(_PixelValue) ' Math.Round(_PixelValue / (0.393700787 * 72), 2)
        End Get
        Set(value)
            _PixelValue = TemplateVersHelper.Measure.cm_To_Px(value) 'value * 0.393700787 * 72
            SetValue()
        End Set
    End Property
    Public Property mm
        Get
            GetValue()
            Return TemplateVersHelper.Measure.Px_To_mm(_PixelValue) 'Math.Round(_PixelValue / (0.0393700787 * 72), 2)
        End Get
        Set(value)
            _PixelValue = TemplateVersHelper.Measure.mm_To_Px(value) 'value * 0.039370078699999998 * 72
            SetValue()
        End Set
    End Property
    Public Property Inch
        Get
            GetValue()
            Return TemplateVersHelper.Measure.Px_To_Inch(_PixelValue) 'Math.Round(_PixelValue / 72, 2)
        End Get
        Set(value)
            _PixelValue = TemplateVersHelper.Measure.Inch_To_Px(value) 'value * 72
            SetValue()
        End Set
    End Property

    Public Property ShowDot As Boolean
        Get
            Return _ShowDot
        End Get
        Set(value As Boolean)
            _ShowDot = value
        End Set
    End Property

    Public Property Visible As Boolean
        Get
            Return Me.PNLMeasure.Visible
        End Get
        Set(value As Boolean)
            Me.PNLMeasure.Visible = value
        End Set
    End Property

    Public Property Label As String
        Get
            Return Me.LBLfield_t.Text
        End Get
        Set(value As String)
            Me.LBLfield_t.Text = value
        End Set
    End Property
    Public WriteOnly Property Enabled As Boolean
        Set(value As Boolean)
            Me.TXBvalue.Enabled = value
            Me.DDLunit.Enabled = value
        End Set
    End Property
    Public Property CssClassLabel As String
        Get
            Return Me.LBLfield_t.CssClass
        End Get
        Set(value As String)
            Me.LBLfield_t.CssClass = value
        End Set
    End Property
    Public Property CssClassTextBox As String
        Get
            Return Me.TXBvalue.CssClass
        End Get
        Set(value As String)
            Me.TXBvalue.CssClass = value
        End Set
    End Property
    Public Property CssClassDropDown As String
        Get
            Return Me.DDLunit.CssClass
        End Get
        Set(value As String)
            Me.DDLunit.CssClass = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Page.IsPostBack) Then
        '    GetValue(Me.DDLunit.SelectedItem)
        'End If
    End Sub

#Region "Internal"
    Private Sub GetValue()

        Dim Value As Single = 0

        Try
            Value = System.Convert.ToSingle(Me.TXBvalue.Text)
        Catch ex As Exception

        End Try

        Select Case Me.DDLunit.SelectedValue
            Case Units.mm.ToString()
                Value = TemplateVersHelper.Measure.mm_To_Px(Value) ' * 0.0393700787 * 72
            Case Units.cm.ToString()
                'Value = Value * 0.393700787 * 72
                Value = TemplateVersHelper.Measure.cm_To_Px(Value)
            Case Units.inch.ToString()
                Value = TemplateVersHelper.Measure.Inch_To_Px(Value)
                'Value = Value * 72
                'Case Units.px.ToString()
                'Case Else
                '    Value = Value
        End Select
        _PixelValue = System.Math.Round(Value, 0)
    End Sub
    Private Sub SetValue()
        Select Case Me.DDLunit.SelectedValue
            Case Units.mm.ToString()
                Me.TXBvalue.Text = TemplateVersHelper.Measure.Px_To_mm(_PixelValue) ' System.Math.Round(_PixelValue / (0.0393700787 * 72), 0)
            Case Units.cm.ToString()
                Me.TXBvalue.Text = TemplateVersHelper.Measure.Px_To_cm(_PixelValue) 'System.Math.Round(_PixelValue / (0.393700787 * 72), 1)
            Case Units.inch.ToString()
                Me.TXBvalue.Text = TemplateVersHelper.Measure.Px_To_Inch(_PixelValue) 'System.Math.Round(_PixelValue / 72, 1)
            Case Units.px.ToString()
                Me.TXBvalue.Text = System.Math.Round(_PixelValue, 0)
        End Select

    End Sub
    Public Function GetCurrentMeasure() As Units
        Select Case Me.DDLunit.SelectedValue
            Case "mm"
                Return Units.mm
            Case "cm"
                Return Units.cm
            Case "inch"
                Return Units.inch
            Case Else
                Return Units.px
        End Select
    End Function
    Public Sub SetMeasure()
        SetMeasure(Units.mm)
    End Sub
    Public Sub SetMeasure(ByVal ShowDot As Boolean)
        _ShowDot = ShowDot
        SetMeasure()
    End Sub
    Public Sub SetMeasure(ByVal ShowDot As Boolean, ByVal SelectedUnit As Units)
        _ShowDot = ShowDot
        SetMeasure(SelectedUnit)
    End Sub
    Public Sub SetMeasure(ByVal SelectedUnit As Units)

        Me.DDLunit.Items.Clear()

        Dim item As New ListItem()

        item = New ListItem()
        item.Value = "mm"
        item.Text = "mm"
        item.Selected = If(SelectedUnit = Units.mm, True, False)
        Me.DDLunit.Items.Add(item)

        item = New ListItem()
        item.Value = "cm"
        item.Text = "cm"
        item.Selected = If(SelectedUnit = Units.cm, True, False)
        Me.DDLunit.Items.Add(item)

        item = New ListItem()
        item.Value = "inch"
        item.Text = "inch"
        item.Selected = If(SelectedUnit = Units.inch, True, False)
        Me.DDLunit.Items.Add(item)

        If _ShowDot Then
            item = New ListItem()
            item.Value = "px"
            item.Text = "px"
            item.Selected = If(SelectedUnit = Units.px, True, False)
            Me.DDLunit.Items.Add(item)
        End If

        Me.DDLunit.DataBind()

        Me.SetValue() 'SelectedUnit)
    End Sub
#End Region

#Region "Handler"
    Private Sub DDLunit_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLunit.SelectedIndexChanged
        RaiseEvent SelectedMeasureChange()
    End Sub
#End Region

    Public Enum Units
        mm
        cm
        inch
        px
    End Enum

End Class