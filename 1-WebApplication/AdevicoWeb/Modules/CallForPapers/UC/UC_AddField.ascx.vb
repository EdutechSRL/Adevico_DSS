Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_AddField
    Inherits BaseControl
    Implements IViewAddField

#Region "Context"
    Private _Presenter As AddFieldPresenter
    Private ReadOnly Property CurrentPresenter() As AddFieldPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddFieldPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentType As FieldType Implements IViewAddField.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", FieldType.None)
        End Get
        Set(value As FieldType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property CurrentDisclaimerType As DisclaimerType Implements IViewAddField.CurrentDisclaimerType
        Get
            Return ViewStateOrDefault("CurrentDisclaimerType", DisclaimerType.None)
        End Get
        Set(value As DisclaimerType)
            ViewState("CurrentDisclaimerType") = value
        End Set
    End Property

    Private Property IdCall As Long Implements IViewAddField.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
#End Region


#Region ""
    Public Event RefreshContainer()
    Public Property AjaxEnabled As Boolean
        Get
            Return ViewStateOrDefault("AjaxEnabled", False)
        End Get
        Set(value As Boolean)
            ViewState("AjaxEnabled") = value
        End Set
    End Property
    Private Property AvailableFields As List(Of dtoSubmissionValueField)
        Get
            Return ViewStateOrDefault("AvailableFields", New List(Of dtoSubmissionValueField))
        End Get
        Set(value As List(Of dtoSubmissionValueField))
            ViewState("AvailableFields") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBgenericFields)
            .setLabel(LBspecificFields)
            .setLabel(LBfieldOptions)
            .setLabel(LBdisclaimerFields)
            .setLabel(LBdateFields)
            .setLabel(LBcompanyFields)
            .setLabel(LBstandardOptionsDescription)
            .setLabel(LBstandardFieldsNumber_t)
            .setLabel(LBstandardFieldsNumberHelp)

            .setLabel(LBmultipleFieldOptions)
            .setLabel(LBadvancedFieldOptionsDescription)
            .setLabel(LBadvancedFieldOptionsList)
            .setLabel(LBadvancedFieldOptionsListHelp)

            '.setLabel(LBTableSimpleDescription)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idCall As Long) Implements IViewAddField.InitializeControl
        Me.CurrentPresenter.InitView(idCall)
    End Sub

    Private Sub LoadAvailableTypes(items As List(Of DisplayFieldType)) Implements IViewAddField.LoadAvailableTypes
        Me.MLVaddField.SetActiveView(VIWfield)
        For Each item As DisplayFieldType In items
            item.Name = Resource.getValue("FieldSelctor.FieldType." & item.Type.ToString & IIf(item.DisclaimerType = DisclaimerType.None, "", "." & item.DisclaimerType.ToString))
        Next
        For Each item As DisplayFieldType In items
            Dim oLabel As Label = Me.FindControl("LBtypes" & item.Type.ToString & IIf(item.DisclaimerType = DisclaimerType.None, "", item.DisclaimerType.ToString))
            oLabel.Text = item.Name
        Next
        Me.RBtypeSingleLine.Checked = True
        UpdatePreview(FieldType.SingleLine, DisclaimerType.None)
    End Sub

    Private Sub LoadFields(fields As List(Of dtoSubmissionValueField)) Implements IViewAddField.LoadFields
        For Each item As dtoSubmissionValueField In fields
            item.Field = GenerateCallField(item.Field)
        Next
        AvailableFields = fields
    End Sub

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVaddField.SetActiveView(VIWempty)
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVaddField.SetActiveView(VIWempty)
    End Sub

    Public Function CreateFields(sections As List(Of dtoCallSection(Of dtoCallField)), idSection As Long) As List(Of FieldDefinition) Implements IViewAddField.CreateFields
        Return Me.CurrentPresenter.CreateFields(sections, idSection, GetFieldsToCreate())
    End Function

    Public Function GetFieldsToCreate() As List(Of dtoCallField) Implements IViewAddField.GetFieldsToCreate
        Dim items As New List(Of dtoCallField)

        Dim fType As FieldType = Me.CurrentType
        Dim fDisclaimerType = Me.CurrentDisclaimerType
        Dim fieldCount As Integer = 1
        If (fType = FieldType.MultiLine OrElse fType = FieldType.SingleLine OrElse fType = FieldType.FileInput OrElse fDisclaimerType = DisclaimerType.CustomDisplayOnly OrElse fDisclaimerType = DisclaimerType.Standard) AndAlso Not String.IsNullOrEmpty(TXBstandardFieldsNumber.Text) AndAlso IsNumeric(TXBstandardFieldsNumber.Text) Then
            fieldCount = CInt(TXBstandardFieldsNumber.Text)
        ElseIf fDisclaimerType = DisclaimerType.CustomSingleOption OrElse fDisclaimerType = DisclaimerType.CustomSingleOption AndAlso Not String.IsNullOrEmpty(TXBspecialFieldsNumber.Text) AndAlso IsNumeric(TXBspecialFieldsNumber.Text) Then
            fieldCount = CInt(TXBspecialFieldsNumber.Text)
        End If
        If fieldCount < 1 Then
            fieldCount = 1
        End If
        For i As Integer = 1 To fieldCount
            Dim dto As New dtoCallField()
            With dto
                .Mandatory = False
                .MaxLength = 0
                .Type = fType
                .DisclaimerType = fDisclaimerType
                .ToolTip = ""

                If fType = FieldType.CheckboxList OrElse fType = FieldType.DropDownList OrElse fType = FieldType.RadioButtonList OrElse fDisclaimerType = DisclaimerType.CustomSingleOption OrElse fDisclaimerType = DisclaimerType.CustomMultiOptions Then
                    .Options = New List(Of dtoFieldOption)
                    .MinOption = IIf(fType = FieldType.CheckboxList OrElse fDisclaimerType = DisclaimerType.CustomMultiOptions, 0, 1)
                    Dim index As Integer = 1
                    If Not String.IsNullOrEmpty(Me.TXBfieldOptions.Text) Then
                        Dim fOptions As List(Of String) = Me.TXBfieldOptions.Text.Split(CChar(vbCrLf)).ToList()
                        For Each opt As String In fOptions.Where(Function(f) Not String.IsNullOrEmpty(f)).ToList
                            .Options.Add(New dtoFieldOption() With {.Name = opt, .Value = index})
                            index += 1
                        Next
                    Else
                        For index = 1 To 3
                            .Options.Add(New dtoFieldOption() With {.Name = String.Format(Resource.getValue("OptionDefault"), index), .Value = index})
                        Next
                    End If
                    .MaxOption = IIf(fType = FieldType.CheckboxList OrElse fDisclaimerType = DisclaimerType.CustomMultiOptions, .Options.Count, 1)
                End If

                .Name = Resource.getValue("FieldCreator.FieldType." & fType.ToString & ".Text")
                .Description = Resource.getValue("FieldCreator.FieldType." & fType.ToString & ".Description" & IIf(fDisclaimerType = DisclaimerType.None, "", "." & fDisclaimerType.ToString))
                If fType = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList OrElse (fDisclaimerType = DisclaimerType.CustomMultiOptions) Then
                    If dto.MinOption > 0 AndAlso dto.MaxOption Then
                        dto.ToolTip = String.Format(Resource.getValue("FieldCreator.FieldType." & dto.Type.ToString & ".Description" & IIf(fDisclaimerType <> DisclaimerType.CustomMultiOptions, "", "." & fDisclaimerType.ToString) & ".Min.Max"), dto.MinOption, dto.MaxOption)
                    ElseIf dto.MaxOption > 0 Then
                        dto.ToolTip = String.Format(Resource.getValue("FieldCreator.FieldType." & dto.Type.ToString & ".Description" & IIf(fDisclaimerType <> DisclaimerType.CustomMultiOptions, "", "." & fDisclaimerType.ToString) & ".Max"), dto.MaxOption)
                    ElseIf dto.MinOption > 0 Then
                        dto.ToolTip = String.Format(Resource.getValue("FieldCreator.FieldType." & dto.Type.ToString & ".Description" & IIf(fDisclaimerType <> DisclaimerType.CustomMultiOptions, "", "." & fDisclaimerType.ToString) & ".Min"), dto.MinOption)
                    End If
                ElseIf fType <> FieldType.Note Then
                    .ToolTip = Resource.getValue("FieldCreator.FieldCreator." & fType.ToString & ".Help")
                End If
            End With
            items.Add(dto)
        Next
        Return items
    End Function
#End Region

    Private Function GenerateCallField(ByVal dto As dtoCallField) As dtoCallField
        Dim result As dtoCallField = dto

        If result.Type <> FieldType.Note Then
            result.Name = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Text")
        End If
        result.Description = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description" & IIf(dto.DisclaimerType = DisclaimerType.None, "", "." & dto.DisclaimerType.ToString))

        If result.Type <> FieldType.Note Then
            result.ToolTip = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Help")
        End If

        Select Case dto.Type
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                If dto.MinOption > 0 AndAlso dto.MaxOption Then
                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
                ElseIf dto.MaxOption > 0 Then
                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
                ElseIf dto.MinOption > 0 Then
                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
                End If
                For Each opt As dtoFieldOption In dto.Options
                    opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
                Next
            Case FieldType.DropDownList, FieldType.RadioButtonList
                For Each opt As dtoFieldOption In dto.Options
                    opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
                Next
            Case FieldType.Disclaimer
                Select Case dto.DisclaimerType
                    Case DisclaimerType.CustomMultiOptions
                        If dto.MinOption > 0 AndAlso dto.MaxOption Then
                            dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description." & dto.DisclaimerType.ToString & ".Min.Max"), dto.MinOption, dto.MaxOption)
                        ElseIf dto.MaxOption > 0 Then
                            dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description." & dto.DisclaimerType.ToString & ".Max"), dto.MaxOption)
                        ElseIf dto.MinOption > 0 Then
                            dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description." & dto.DisclaimerType.ToString & ".Min"), dto.MinOption)
                        End If
                End Select
            Case FieldType.TableSimple
                dto.ToolTip = "SimpleTable"

            Case FieldType.TableReport
                dto.ToolTip = "Table Report"
        End Select
        Return result
    End Function

    Protected Sub RBfieldType_CheckedChanged(sender As Object, e As System.EventArgs)
        Dim value As String = sender.attributes("value")
        Dim itemType As FieldType = CInt(value.Split(".")(0))
        Dim disclaimerType As DisclaimerType = CInt(value.Split(".")(1))
        UpdatePreview(itemType, disclaimerType)
    End Sub

    Private Sub UpdatePreview(itemType As FieldType, disclaimerType As DisclaimerType)
        CurrentType = itemType
        CurrentDisclaimerType = disclaimerType
        Dim fieldName As String = Resource.getValue("FieldSelctor.FieldType." & itemType.ToString & IIf(disclaimerType = disclaimerType.None, "", "." & disclaimerType.ToString))
        LBfieldName.Text = fieldName
        Me.CTRLinputField.InitializeControl(0, 0, New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With {.Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal}, AvailableFields.Where(Function(f) f.Field.Type = itemType AndAlso f.Field.DisclaimerType = disclaimerType).FirstOrDefault(), True, False)

        TXBstandardFieldsNumber.Text = 1
        TXBspecialFieldsNumber.Text = 1
        If itemType = FieldType.CheckboxList _
            OrElse itemType = FieldType.DropDownList _
            OrElse itemType = FieldType.RadioButtonList _
            OrElse disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions _
            OrElse disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption Then
            Me.Resource.setLabel(LBmultipleFieldOptions)
            LBmultipleFieldOptions.Text = String.Format(LBmultipleFieldOptions.Text, fieldName)
            DVspecialFieldsNumber.Visible = (disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly OrElse disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard)
            Me.MLVoptions.SetActiveView(VIWmultipleChoiceFields)
        ElseIf itemType = FieldType.Disclaimer Then
            Me.Resource.setLabel(LBfieldOptions)
            LBfieldOptions.Text = String.Format(LBfieldOptions.Text, fieldName)
            Me.MLVoptions.SetActiveView(VIWstandardFields)
        ElseIf itemType = FieldType.MultiLine OrElse itemType = FieldType.SingleLine OrElse itemType = FieldType.FileInput Then
            Me.Resource.setLabel(LBfieldOptions)
            LBfieldOptions.Text = String.Format(LBfieldOptions.Text, fieldName)
            Me.MLVoptions.SetActiveView(VIWstandardFields)
        ElseIf itemType = FieldType.TableSimple OrElse FieldType.TableReport Then
            Me.MLVoptions.SetActiveView(VIWstandardFields)
            'Me.Resource.setLabel_To_Value(LBstandardOptionsDescription, "table")
            LBstandardOptionsDescription.Text = "Opzioni di creazione campo tabella"
        ElseIf itemType = FieldType.TableSummary Then
            Me.MLVoptions.SetActiveView(VIWstandardFields)
            LBstandardOptionsDescription.Text = "Tabella riepilogo"

            'LBmultipleFieldOptions.Text = String.Format(LBmultipleFieldOptions.Text, fieldName)
            'DVspecialFieldsNumber.Visible = (disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly OrElse disclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard)
            'Me.MLVoptions.SetActiveView(VIWmultipleChoiceFields)
        Else

            Me.MLVoptions.SetActiveView(VIWnoOptions)
        End If
        UpdateCssClass(itemType)
    End Sub

    Private Sub UpdateCssClass(itemType As FieldType)
        DVcompanyFields.Attributes("class") = LTcssClass.Text & "compressed"
        DVdateFields.Attributes("class") = LTcssClass.Text & "compressed"
        DVdisclaimerFields.Attributes("class") = LTcssClass.Text & "compressed"
        DVgenericFields.Attributes("class") = LTcssClass.Text & "compressed"
        DVspecificFields.Attributes("class") = LTcssClass.Text & "compressed"
        Select Case itemType
            Case FieldType.CheckboxList, FieldType.DropDownList, FieldType.MultiLine, FieldType.Note, FieldType.RadioButtonList, FieldType.SingleLine, FieldType.TableSimple, FieldType.TableReport, FieldType.TableSummary
                DVgenericFields.Attributes("class") = Replace(DVcompanyFields.Attributes("class"), "compressed", "expanded")
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode
                DVcompanyFields.Attributes("class") = Replace(DVcompanyFields.Attributes("class"), "compressed", "expanded")

            Case FieldType.Date, FieldType.DateTime, FieldType.Time
                DVdateFields.Attributes("class") = Replace(DVcompanyFields.Attributes("class"), "compressed", "expanded")

            Case FieldType.Disclaimer
                DVdisclaimerFields.Attributes("class") = Replace(DVcompanyFields.Attributes("class"), "compressed", "expanded")
            Case Else
                DVspecificFields.Attributes("class") = Replace(DVcompanyFields.Attributes("class"), "compressed", "expanded")

        End Select
    End Sub
End Class

'Imports lm.Comol.UI.Presentation
'Imports lm.Comol.Core.DomainModel
'Imports lm.Comol.Modules.CallForPapers.Presentation
'Imports lm.Comol.Modules.CallForPapers.Domain
'Imports lm.ActionDataContract
'Imports System.Linq
'Imports System.Collections.Generic
'Public Class UC_AddField
'    Inherits BaseControl
'    Implements IViewAddField

'#Region "Context"
'    Private _Presenter As AddFieldPresenter
'    Private ReadOnly Property CurrentPresenter() As AddFieldPresenter
'        Get
'            If IsNothing(_Presenter) Then
'                _Presenter = New AddFieldPresenter(Me.PageUtility.CurrentContext, Me)
'            End If
'            Return _Presenter
'        End Get
'    End Property
'#End Region

'#Region "Inherits"
'    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
'        Get
'            Return False
'        End Get
'    End Property
'#End Region

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

'    End Sub

'#Region "Inherits"
'    Protected Overrides Sub SetCultureSettings()
'        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
'    End Sub

'    Protected Overrides Sub SetInternazionalizzazione()
'        With Me.Resource
'            .setLabel(LBgenericFields)
'            .setLabel(LBspecificFields)
'        End With
'    End Sub
'#End Region

'#Region "Implements"
'    Public Sub InitializeControl(idCall As Long) Implements IViewAddField.InitializeControl
'        Me.CurrentPresenter.InitView(idCall)
'    End Sub

'    Private Sub LoadAvailableTypes(items As List(Of DisplayFieldType)) Implements IViewAddField.LoadAvailableTypes
'        Me.MLVaddField.SetActiveView(VIWfield)
'        For Each item As DisplayFieldType In items
'            item.Name = Resource.getValue("FieldSelctor.FieldType." & item.Type.ToString)
'        Next
'        Me.RPTgenericItems.DataSource = items.Where(Function(i) i.isGeneric).ToList()
'        Me.RPTgenericItems.DataBind()

'        Me.RPTspecificItems.DataSource = items.Where(Function(i) Not i.isGeneric).ToList()
'        Me.RPTspecificItems.DataBind()
'    End Sub

'    Private Sub LoadFields(fields As List(Of dtoSubmissionValueField)) Implements IViewAddField.LoadFields
'        Me.RPTrenderFields.DataSource = fields
'        Me.RPTrenderFields.DataBind()
'    End Sub

'    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
'        Me.MLVaddField.SetActiveView(VIWempty)
'    End Sub

'    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
'        Me.MLVaddField.SetActiveView(VIWempty)
'    End Sub

'    Public Function GetFieldToCreate(idSection As Long, fType As FieldType) As dtoCallField Implements IViewAddField.GetFieldToCreate
'        Dim dto As New dtoCallField()
'        With dto
'            .IdSection = idSection
'            .Name = ""
'            .Description = ""
'            .DisplayOrder = 0
'            .Mandatory = False
'            .MaxLength = 0
'            .Type = fType 'GetSelectedType()

'            Dim fieldCount As Integer = 1
'            Dim optString As String = ""
'            For Each row As RepeaterItem In (From r As RepeaterItem In RPTrenderFields.Items _
'                                             Where (r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item) _
'                                             AndAlso (DirectCast(r.FindControl("LTfieldType"), Literal).Text = fType) _
'                                             Select r).ToList
'                Dim oTextBox As TextBox
'                Dim mView As MultiView = row.FindControl("MLVoptions")
'                If fType = FieldType.CheckboxList OrElse fType = FieldType.DropDownList OrElse fType = FieldType.RadioButtonList Then
'                    oTextBox = row.FindControl("TXBfieldOptions")
'                    optString = oTextBox.Text
'                ElseIf fType = FieldType.MultiLine OrElse fType = FieldType.SingleLine Then
'                    oTextBox = row.FindControl("TXBstandardFieldsNumber")
'                    If Not String.IsNullOrEmpty(oTextBox.Text) AndAlso IsNumeric(oTextBox.Text) AndAlso CInt(oTextBox.Text) > 0 Then
'                        fieldCount = CInt(oTextBox.Text)
'                    End If
'                End If

'            Next



'        End With
'        Return dto
'    End Function

'    'Private Function GetSelectedType() As FieldType
'    '    Dim type As FieldType = FieldType.SingleLine

'    '    For Each row As RepeaterItem In (From r As RepeaterItem In RPTgenericItems.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
'    '        Dim ocontrol As HtmlControl = row.FindControl("type")

'    '    Next
'    '    Dim items As List(Of String) = Request.Form.AllKeys.Where(Function(c) c.Contains("addField")).ToList

'    '    'Dim strin As String = Request.Form("type")
'    '    'strin = HDNselectedType.Value
'    '    Return type
'    'End Function
'#End Region

'    Private Sub RPTrenderFields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrenderFields.ItemDataBound
'        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
'            Dim item As dtoSubmissionValueField = DirectCast(e.Item.DataItem, dtoSubmissionValueField)
'            Dim oLabel As Label = e.Item.FindControl("LBfieldName")
'            Dim fieldName As String = Resource.getValue("FieldSelctor.FieldType." & item.Field.Type.ToString)
'            oLabel.Text = String.Format(Resource.getValue("Preview.FieldType"), fieldName)

'            item.Field = GenerateCallField(item.Field)
'            Dim oControl As UC_InputField = e.Item.FindControl("CTRLinputField")
'            oControl.InitializeControl(0, 0, item, True)

'            oLabel = e.Item.FindControl("LBfieldOptions")
'            Resource.setLabel(oLabel)
'            oLabel.Text = String.Format(oLabel.Text, fieldName)
'            oLabel = e.Item.FindControl("LBstandardOptionsDescription")
'            Resource.setLabel(oLabel)
'            oLabel = e.Item.FindControl("LBstandardFieldsNumber_t")
'            Resource.setLabel(oLabel)
'            oLabel = e.Item.FindControl("LBstandardFieldsNumberHelp")
'            Resource.setLabel(oLabel)
'            oLabel = e.Item.FindControl("LBmultipleFieldOptions")
'            Resource.setLabel(oLabel)
'            oLabel.Text = String.Format(oLabel.Text, fieldName)

'            oLabel = e.Item.FindControl("LBadvancedFieldOptionsDescription")
'            Resource.setLabel(oLabel)
'            oLabel = e.Item.FindControl("LBadvancedFieldOptionsList")
'            Resource.setLabel(oLabel)
'            oLabel = e.Item.FindControl("LBadvancedFieldOptionsListHelp")
'            Resource.setLabel(oLabel)

'            Dim oTextBox As TextBox = e.Item.FindControl("TXBstandardFieldsNumber")
'            oTextBox.Text = "1"

'            Dim mView As MultiView = e.Item.FindControl("MLVoptions")

'            Dim type As FieldType = item.Field.Type
'            If type = FieldType.CheckboxList OrElse type = FieldType.DropDownList OrElse type = FieldType.RadioButtonList Then

'                mView.SetActiveView(e.Item.FindControl("VIWmultipleChoiceFields"))
'            ElseIf type = FieldType.MultiLine OrElse type = FieldType.SingleLine Then
'                mView.SetActiveView(e.Item.FindControl("VIWstandardFields"))
'            End If
'        End If
'    End Sub

'    Private Function GenerateCallField(ByVal dto As dtoCallField) As dtoCallField
'        Dim result As dtoCallField = dto

'        If result.Type <> FieldType.Note Then
'            result.Name = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Text")
'        End If
'        result.Description = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description")

'        If result.Type <> FieldType.Note Then
'            result.ToolTip = Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Help")
'        End If

'        Select Case dto.Type
'            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
'                If dto.MinOption > 0 AndAlso dto.MaxOption Then
'                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Min.Max"), dto.MinOption, dto.MaxOption)
'                ElseIf dto.MaxOption > 0 Then
'                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Max"), dto.MaxOption)
'                ElseIf dto.MinOption > 0 Then
'                    dto.ToolTip = String.Format(Resource.getValue("FieldSelctor.FieldType." & dto.Type.ToString & ".Description.Min"), dto.MinOption)
'                End If
'                For Each opt As dtoFieldOption In dto.Options
'                    opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
'                Next
'            Case FieldType.DropDownList, FieldType.RadioButtonList
'                For Each opt As dtoFieldOption In dto.Options
'                    opt.Name = String.Format(Resource.getValue("OptionDefault"), opt.Value)
'                Next
'        End Select
'        Return result
'    End Function


'End Class