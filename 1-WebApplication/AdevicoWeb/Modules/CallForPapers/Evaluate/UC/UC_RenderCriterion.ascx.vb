Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_RenderCriterion
    Inherits BaseControl

    '    Implements IViewRenderCriterion

    '#Region "Context"
    '    Private _Presenter As RenderCriterionPresenter
    '    Private ReadOnly Property CurrentPresenter() As RenderCriterionPresenter
    '        Get
    '            If IsNothing(_Presenter) Then
    '                _Presenter = New RenderCriterionPresenter(Me.PageUtility.CurrentContext, Me)
    '            End If
    '            Return _Presenter
    '        End Get
    '    End Property
    '#End Region

    '#Region "Implements"

    '    Private Property Disabled As Boolean Implements IViewRenderCriterion.Disabled
    '        Get
    '            Return ViewStateOrDefault("Disabled", False)
    '        End Get
    '        Set(value As Boolean)
    '            ViewState("Disabled") = value
    '        End Set
    '    End Property
    '    Private Property MaxChars As Integer Implements IViewRenderCriterion.MaxChars
    '        Get
    '            Return ViewStateOrDefault("MaxChars", CInt(0))
    '        End Get
    '        Set(value As Integer)
    '            ViewState("MaxChars") = value
    '        End Set
    '    End Property
    '    Private Property MaxOptions As Integer Implements IViewRenderCriterion.MaxOptions
    '        Get
    '            Return ViewStateOrDefault("MaxOptions", CInt(1))
    '        End Get
    '        Set(value As Integer)
    '            ViewState("MaxOptions") = value
    '        End Set
    '    End Property
    '    Private Property MinOptions As Integer Implements IViewRenderCriterion.MinOptions
    '        Get
    '            Return ViewStateOrDefault("MinOptions", CInt(0))
    '        End Get
    '        Set(value As Integer)
    '            ViewState("MinOptions") = value
    '        End Set
    '    End Property
    '    Public Property CurrentError As lm.Comol.Modules.CallForPapers.Domain.FieldError Implements IViewRenderCriterion.CurrentError
    '        Get
    '            Return ViewStateOrDefault("CurrentError", lm.Comol.Modules.CallForPapers.Domain.FieldError.None)
    '        End Get
    '        Set(value As lm.Comol.Modules.CallForPapers.Domain.FieldError)
    '            ViewState("CurrentError") = value
    '        End Set
    '    End Property
    '    Public Property CriterionType As CriterionType Implements IViewRenderCriterion.CriterionType
    '        Get
    '            Return ViewStateOrDefault("CriterionType", CriterionType.None)
    '        End Get
    '        Set(value As CriterionType)
    '            ViewState("CriterionType") = value
    '        End Set
    '    End Property
    '    Public Property IdCriterion As Long Implements IViewRenderCriterion.IdCriterion
    '        Get
    '            Return ViewStateOrDefault("IdCriterion", CLng(0))
    '        End Get
    '        Set(value As Long)
    '            ViewState("IdCriterion") = value
    '        End Set
    '    End Property
    '#End Region

    '#Region "Inherits"
    '    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property
    '#End Region

    '    Protected ReadOnly Property CssError As String
    '        Get
    '            Return IIf(CurrentError = lm.Comol.Modules.CallForPapers.Domain.FieldError.None, "", "error")
    '        End Get
    '    End Property
    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    End Sub

    '#Region "Inherits"
    '    Protected Overrides Sub SetCultureSettings()
    '        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    '    End Sub

    '    Protected Overrides Sub SetInternazionalizzazione()
    '        With Me.Resource

    '        End With
    '    End Sub
    '#End Region
    '    Public Sub InitializeControl(item As dtoCriterionEvaluated, disabled As Boolean, isPublic As Boolean) Implements IViewRenderCriterion.InitializeControl
    '        CurrentError = lm.Comol.Modules.CallForPapers.Domain.FieldError.None
    '        Me.CurrentPresenter.InitView(item, disabled, isPublic)
    '    End Sub
    '    Public Sub InitializeControl(item As dtoCriterionEvaluated, disabled As Boolean, isPublic As Boolean, err As lm.Comol.Modules.CallForPapers.Domain.FieldError) Implements IViewRenderCriterion.InitializeControl
    '        CurrentError = err
    '        Me.CurrentPresenter.InitView(item, disabled, isPublic)

    '        If Not IsNothing(item) AndAlso Not IsNothing(item.Criterion) Then
    '            Dim oLabel As Label = Me.FindControl("LBerrorMessage" & item.Criterion.Type.ToString.ToLower)

    '            If Not IsNothing(oLabel) Then
    '                oLabel.Visible = Not (err = lm.Comol.Modules.CallForPapers.Domain.FieldError.None)

    '                If Not (err = lm.Comol.Modules.CallForPapers.Domain.FieldError.None) Then
    '                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
    '                End If
    '            End If
    '        End If
    '    End Sub
    '    Private Sub SetupView(item As dtoCriterionEvaluated, isPublic As Boolean) Implements IViewRenderCriterion.SetupView
    '        IdCriterion = item.Criterion.Id
    '        CriterionType = item.Criterion.Type


    '        Dim oLiteral As Literal = Me.FindControl("LTmaxChars" & item.Field.Type.ToString.ToLower)
    '        If Not IsNothing(oLiteral) Then
    '            oLiteral.Text = Me.Resource.getValue("MaxCharsInfo")
    '        End If

    '        Dim oGeneric As HtmlGenericControl = Me.FindControl("SPNmaxChar" & item.Field.Type.ToString.ToLower)
    '        If Not IsNothing(oGeneric) AndAlso item.Field.MaxLength > 0 AndAlso (item.Field.Type <> FieldType.CheckboxList AndAlso item.Field.Type <> FieldType.RadioButtonList AndAlso item.Field.Type <> FieldType.DropDownList AndAlso item.Field.Type <> FieldType.Mail) Then
    '            oGeneric.Visible = True
    '        End If

    '        oGeneric = Me.FindControl("DV" & item.Field.Type.ToString.ToLower)


    '        Dim oLabelDescription As Label = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Description")
    '        Dim oLabelText As Label = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Text")

    '        Dim view As System.Web.UI.WebControls.View = Me.FindControl("VIW" & item.Field.Type.ToString.ToLower)
    '        Dim oValidator As RequiredFieldValidator
    '        Select Case item.Field.Type
    '            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
    '                Me.DDLitems.DataSource = item.Field.Options
    '                Me.DDLitems.DataTextField = "Name"
    '                Me.DDLitems.DataValueField = "Id"
    '                Me.DDLitems.DataBind()
    '                If Not String.IsNullOrEmpty(item.Value) Then
    '                    Try
    '                        Me.DDLitems.SelectedValue = item.Value
    '                    Catch ex As Exception

    '                    End Try
    '                End If

    '                Me.MinOptions = item.Field.MinOption
    '                Me.MaxOptions = item.Field.MaxOption

    '            Case FieldType.MultiLine
    '                Me.LBmultilineValue.CssClass = "readonlytextarea"
    '                Me.LBmultilineValue.Text = item.Value
    '                If item.Field.MaxLength > 0 Then
    '                    'Me.LBmultilineValue.Attributes.Add("maxlength", item.Field.MaxLength)
    '                    Me.LTmultilineTotal.Text = item.Field.MaxLength
    '                    Dim used As Integer = (item.Field.MaxLength - Len(item.Value))
    '                    Me.LTmultilineUsed.Text = IIf(used < 0, 0, used)
    '                End If
    '                If String.IsNullOrEmpty(item.Value) Then
    '                    Me.LBmultilineValue.CssClass &= " empty"
    '                End If
    '            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
    '                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode
    '                If item.Field.MaxLength > 0 Then
    '                    Dim used As Integer = (item.Field.MaxLength - Len(item.Value))
    '                    Me.LTsinglelineUsed.Text = IIf(used < 0, 0, used)
    '                    Me.LTsinglelineTotal.Text = item.Field.MaxLength
    '                End If
    '                Me.LBsinglelineValue.Text = item.Value
    '                view = VIWsingleline
    '                Me.LBsinglelineValue.CssClass = "readonlyinput"
    '                If String.IsNullOrEmpty(item.Value) Then
    '                    Me.LBsinglelineValue.CssClass &= " empty"
    '                End If
    '                oGeneric = Me.DVsingleline
    '                oLabelDescription = Me.LBsinglelineDescription
    '                oLabelText = Me.LBsinglelineText
    '        End Select

    '        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note AndAlso Not IsNothing(oLabelText) Then
    '            oLabelText.Text = IIf(item.Field.Mandatory, "(*)", "") & item.Field.Name
    '        End If
    '        If Not IsNothing(oLabelDescription) Then
    '            oLabelDescription.Text = item.Field.Description
    '        End If
    '        If Not IsNothing(oGeneric) Then
    '            'If Not IsNothing(oTextBox) Then
    '            Dim cssClass = oGeneric.Attributes("class")
    '            If (item.FieldError = FieldError.None) Then
    '                cssClass = Replace(cssClass, " error", "")
    '            Else
    '                If Not cssClass.Contains(" error") Then
    '                    cssClass &= " error"
    '                End If
    '            End If
    '            oGeneric.Attributes("class") = cssClass
    '        End If
    '        If Not IsNothing(view) Then
    '            Me.MLVcriterion.SetActiveView(view)
    '        End If
    '    End Sub
    '    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewRenderCriterion.DisplayNoPermission
    '        Me.MLVcriterion.SetActiveView(VIWempty)
    '    End Sub
    '    Private Sub DisplaySessionTimeout() Implements IViewRenderCriterion.DisplaySessionTimeout
    '        Me.MLVcriterion.SetActiveView(VIWempty)
    '    End Sub
    '    Private Sub DisplayEmptyField() Implements IViewRenderCriterion.DisplayEmptyCriterion
    '        Me.MLVcriterion.SetActiveView(VIWempty)
    '    End Sub

    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
End Class