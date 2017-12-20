Imports lm.Comol.Modules.CallForPapers.Domain

Public Class UC_EditField
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Public Event AddOption(idField As Long, name As String, isDefault As Boolean, isFreeText As Boolean)
    Public Event SetAsDefaultOption(idOption As Long, isDefault As Boolean)
    Public Event RemoveOption(idOption As Long)
    Public Event SaveDisclaimerType(idField As Long, dType As lm.Comol.Modules.CallForPapers.Domain.DisclaimerType)

    Public ReadOnly Property GetField As lm.Comol.Modules.CallForPapers.Domain.dtoCallField
        Get
            Dim dto As New lm.Comol.Modules.CallForPapers.Domain.dtoCallField
            dto.Description = Me.TXBdescription.Text
            dto.ToolTip = Me.TXBhelp.Text
            dto.Type = FieldType
            dto.DisclaimerType = DisclaimerType
            dto.Tags = Me.Tags

            Select Case dto.Type
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.None
                    Return dto
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TaxCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.VatCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.ZipCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine
                    If IsNumeric(Me.TXBmaxChar.Text) Then
                        dto.MaxLength = CInt(Me.TXBmaxChar.Text)
                    End If
                    'Return dto
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableReport, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.TableSimple

                    dto.TableFieldSetting = New lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField()
                    dto.TableFieldSetting.Cols = Me.TXBtableCols.Text


                    If IsNumeric(Me.TXBmaxRows.Text) Then
                        dto.TableFieldSetting.MaxRows = CInt(Me.TXBmaxRows.Text)
                    End If

                    dto.TableFieldSetting.MaxTotal = 0

                    If (dto.Type = FieldType.TableReport) Then
                        Try
                            Double.TryParse(TXBtableMaxTotal.Text, dto.TableFieldSetting.MaxTotal)
                        Catch ex As Exception
                            dto.TableFieldSetting.MaxTotal = 0
                        End Try
                    End If



                    'Return dto
                Case Else
                    If Me.MLVadvanced.GetActiveView Is VIWmultipleChoice Then
                        dto.Options = GetOptions(RPToptions)
                        If CBXmultipleChoice.Checked Then

                            If IsNumeric(Me.TXTminOptions.Text) Then
                                dto.MinOption = CInt(Me.TXTminOptions.Text)
                            Else
                                Me.TXTminOptions.Text = 0
                                dto.MinOption = 0
                            End If
                            If dto.MinOption < 0 Then
                                dto.MinOption = 0
                            End If
                            If IsNumeric(Me.TXTmaxOptions.Text) Then
                                dto.MaxOption = CInt(Me.TXTmaxOptions.Text)
                            Else
                                Me.TXTmaxOptions.Text = 1
                                dto.MaxOption = 1
                            End If

                            If dto.MaxOption < dto.MinOption Then
                                Me.TXTmaxOptions.Text = dto.MinOption
                                Me.TXTminOptions.Text = dto.MaxOption
                                dto.MaxOption = dto.MinOption
                                dto.MinOption = CInt(Me.TXTminOptions.Text)
                            End If
                            If (FieldType <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer) Then
                                FieldType = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                                dto.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                            End If

                            If (dto.Mandatory AndAlso dto.MinOption = 0) Then
                                dto.MinOption = 1
                            End If

                        Else
                            If (FieldType <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer) Then
                                FieldType = lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                                dto.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                            End If
                            dto.MinOption = 1
                            dto.MaxOption = 1
                        End If
                    ElseIf Me.MLVadvanced.GetActiveView Is VIWdropdownlist Then
                        dto.Options = GetOptions(RPTcomboOptions)
                        dto.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                        dto.MinOption = 1
                        dto.MaxOption = 1
                    End If
                    'Return dto
            End Select

            Return dto
        End Get
    End Property
    Protected Property IdField As Long
        Get
            Return ViewStateOrDefault("IdField", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdField") = value
        End Set
    End Property
    Private Property FieldType As lm.Comol.Modules.CallForPapers.Domain.FieldType
        Get
            Return ViewStateOrDefault("FieldType", lm.Comol.Modules.CallForPapers.Domain.FieldType.None)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.FieldType)
            Me.ViewState("FieldType") = value
        End Set
    End Property
    Private Property DisclaimerType As lm.Comol.Modules.CallForPapers.Domain.DisclaimerType
        Get
            If (RBLdisclaimerType.SelectedIndex < 0) Then
                Return lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.DisclaimerType).GetByString(RBLdisclaimerType.SelectedValue, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None)
            End If
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.DisclaimerType)
            If value <> lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None Then
                RBLdisclaimerType.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property AllowSave As Boolean
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property AllowFreeOption As Boolean
        Get
            Return ViewStateOrDefault("AllowFreeOption", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowFreeOption") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBfieldDescription_t)
            .setLabel(LBfieldMaxChar_t)
            .setLabel(LBfieldHelp_t)
            .setLabel(LBdisclaimer_t)
            .setLabel(LBfieldDisclaimerType_t)
            .setLabel(LBmultipleChoice_t)
            .setLabel(LBminOptions_t)
            .setLabel(LBmaxOptions_t)
            .setRadioButtonList(RBLdisclaimerType, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard.ToString)
            .setRadioButtonList(RBLdisclaimerType, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly.ToString)
            .setRadioButtonList(RBLdisclaimerType, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions.ToString)
            .setRadioButtonList(RBLdisclaimerType, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption.ToString)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(item As lm.Comol.Modules.CallForPapers.Domain.dtoCallField, containerAllowSave As Boolean)
        AllowSave = containerAllowSave
        AllowFreeOption = containerAllowSave AndAlso item.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer AndAlso item.Options.Any() AndAlso Not item.Options.Where(Function(o) o.IsFreeValue).Any()
        IdField = item.Id

        Me.TXBdescription.Enabled = containerAllowSave
        Me.TXBhelp.Enabled = containerAllowSave
        Me.TXBmaxChar.Enabled = containerAllowSave
        Me.TXTmaxOptions.Enabled = containerAllowSave
        Me.TXTminOptions.Enabled = containerAllowSave

        Me.TXBtableCols.Enabled = containerAllowSave
        Me.TXBmaxRows.Enabled = containerAllowSave
        Me.TXBtableMaxTotal.Enabled = containerAllowSave

        Me.TXBtags.Text = item.Tags

        DVdisclaimerType.Visible = (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer)
        If IsNothing(item) Then
            Me.MLVfield.SetActiveView(VIWempty)
            FieldType = lm.Comol.Modules.CallForPapers.Domain.FieldType.None
        ElseIf item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.None Then
            Me.MLVfield.SetActiveView(VIWempty)
            FieldType = item.Type
        Else
            Me.MLVfield.SetActiveView(VIWfield)
            LBdisclaimer_t.Visible = (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer)
            LBfieldDescription_t.Visible = Not (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer)

            Me.DVhelp.Visible = Not (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Note OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList OrElse item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList)
            Me.DVmaxChar.Visible = Not (item.Type = FieldType.Note OrElse item.Type = FieldType.Disclaimer OrElse item.Type = FieldType.RadioButtonList _
                                    OrElse item.Type = FieldType.CheckboxList OrElse item.Type = FieldType.DropDownList OrElse item.Type = FieldType.Mail)
            Select Case item.Type
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TaxCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.VatCode _
                 , lm.Comol.Modules.CallForPapers.Domain.FieldType.ZipCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine
                    Me.TXBmaxChar.Text = item.MaxLength
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList, lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                    Me.RPToptions.DataSource = item.Options
                    Me.RPToptions.DataBind()
                    Me.CBXmultipleChoice.Checked = (item.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList)
                    Me.TXTmaxOptions.Text = item.MaxOption
                    Me.TXTminOptions.Text = item.MinOption

                    Me.MLVadvanced.SetActiveView(VIWmultipleChoice)
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer
                    Select Case item.DisclaimerType
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                            Me.RPToptions.DataSource = item.Options
                            Me.RPToptions.DataBind()
                            Me.CBXmultipleChoice.Checked = (item.DisclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions)
                            CBXmultipleChoice.Disabled = True
                            Me.TXTmaxOptions.Text = item.MaxOption
                            Me.TXTminOptions.Text = item.MinOption

                            FSmultipleChoice.Visible = (item.DisclaimerType = lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions)
                            Me.MLVadvanced.SetActiveView(VIWmultipleChoice)
                        Case Else
                            Me.MLVadvanced.SetActiveView(VIWnone)
                    End Select
                    DisclaimerType = item.DisclaimerType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                    Me.MLVadvanced.SetActiveView(VIWdropdownlist)
                    Me.RPTcomboOptions.DataSource = item.Options
                    Me.RPTcomboOptions.DataBind()
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableSimple,
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.TableReport
                    Me.MLVadvanced.SetActiveView(VIWtable)
                    DVmaxChar.Visible = False

                    If IsNothing(item.TableFieldSetting) Then
                        item.TableFieldSetting = New dtoCallTableField()
                    End If
                    Me.TXBmaxRows.Text = item.TableFieldSetting.MaxRows
                    Me.TXBtableCols.Text = item.TableFieldSetting.Cols
                    Me.TXBtableMaxTotal.Text = item.TableFieldSetting.MaxTotal

                Case Else
                    Me.MLVadvanced.SetActiveView(VIWnone)
            End Select
            Me.TXBdescription.Text = item.Description
            Me.TXBhelp.Text = item.ToolTip
            FieldType = item.Type
        End If
    End Sub

    Private Function GetOptions(ByVal rp As Repeater) As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption)
        Dim options As New List(Of lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption)
        Dim displayOrder As Integer = 1

        For Each row As RepeaterItem In (From r As RepeaterItem In rp.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim dtoOption As New lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption
            Dim oLiteral As Literal = row.FindControl("LTidOption")
            Dim oTextBox As TextBox = row.FindControl("TXBoptionName")

            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                dtoOption.DisplayOrder = CInt(hidden.Value)
            Else
                dtoOption.DisplayOrder = displayOrder
            End If

            dtoOption.Id = CLng(oLiteral.Text)
            dtoOption.Name = oTextBox.Text
            oLiteral = row.FindControl("LTisFreeValue")
            Boolean.TryParse(oLiteral.Text, dtoOption.IsFreeValue)
            oLiteral = row.FindControl("LTisDefault")
            Boolean.TryParse(oLiteral.Text, dtoOption.IsDefault)

            displayOrder += 1
            options.Add(dtoOption)
        Next
        displayOrder = 1
        For Each opt As lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption In options.OrderBy(Function(o) o.DisplayOrder).ToList
            opt.DisplayOrder = displayOrder
            displayOrder += 1
        Next
        Return options.OrderBy(Function(o) o.DisplayOrder).ToList
    End Function

    Private Sub RPToptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPToptions.ItemDataBound, RPTcomboOptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim fOption As lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.dtoFieldOption)

            'Dim oLiteral As Literal = e.Item.FindControl("LTidOption")
            'oLiteral.Text = fOption.Id
            Dim oTextBox As TextBox = e.Item.FindControl("TXBoptionName")
            oTextBox.Text = fOption.Name

            Dim oButton As Button = e.Item.FindControl("BTNdeleteOption")
            oButton.CommandArgument = fOption.Id
            oButton.Visible = AllowSave
            Me.Resource.setButton(oButton, True, False, False, True)

            If AllowSave Then
                Select Case DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None
                        oButton = e.Item.FindControl("BTNremoveAsDefaultOption")
                        oButton.CommandArgument = fOption.Id
                        oButton.Visible = fOption.IsDefault
                        Me.Resource.setButton(oButton, True, False, False, True)

                        oButton = e.Item.FindControl("BTNsetDefaultOption")
                        oButton.CommandArgument = fOption.Id
                        oButton.Visible = Not fOption.IsDefault
                        Me.Resource.setButton(oButton, True, False, False, True)
                End Select
            End If

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = fOption.DisplayOrder

            Dim oLabel As Label = e.Item.FindControl("LBmoveOption")
            oLabel.ToolTip = Resource.getValue("LBmoveOption.Text")
            oLabel.Visible = AllowSave

            Dim oLiteral As Literal = e.Item.FindControl("LToptionInfo")
            oLiteral.Text = Resource.getValue("LToptionInfo." & fOption.IsDefault & "." & fOption.IsFreeValue)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LToptionsTitle")
            Me.Resource.setLiteral(oLiteral)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oButton As Button = e.Item.FindControl("BTNaddOption")
            oButton.CommandName = "addoption"
            oButton.Visible = AllowSave
            Me.Resource.setButton(oButton, True, False, False, True)

            oButton = e.Item.FindControl("BTNaddFreeOption")
            If Not IsNothing(oButton) Then
                oButton.Visible = AllowFreeOption
                Me.Resource.setButton(oButton, True, False, False, True)
            End If

            Dim oGeneric As HtmlControl = e.Item.FindControl("DVfooter")
            oGeneric.Visible = AllowSave
        End If
    End Sub

    Private Sub RPToptions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPToptions.ItemCommand, RPTcomboOptions.ItemCommand
        Dim idOption As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idOption = CLng(e.CommandArgument)
        End If
        Select Case e.CommandName
            Case "virtualDelete"
                RaiseEvent RemoveOption(idOption)
            Case "addoption"
                Dim oText As TextBox = e.Item.FindControl("TXBoptionAddName")
                RaiseEvent AddOption(IdField, oText.Text, False, False)
            Case "addFreeOption"
                Dim oText As TextBox = e.Item.FindControl("TXBoptionAddName")
                RaiseEvent AddOption(IdField, oText.Text, False, True)
            Case "setDefault"
                RaiseEvent SetAsDefaultOption(idOption, True)
            Case "removeDefault"
                RaiseEvent SetAsDefaultOption(idOption, False)
        End Select
    End Sub

    Private Sub RBLdisclaimerType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLdisclaimerType.SelectedIndexChanged
        Dim dType As lm.Comol.Modules.CallForPapers.Domain.DisclaimerType = DisclaimerType
        Select Case dType
            Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                Me.MLVadvanced.SetActiveView(VIWnone)
            Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                Dim maxOption As Integer = 1, minOption As Integer = 0
                If IsNumeric(Me.TXTmaxOptions.Text) Then
                    maxOption = CInt(Me.TXTmaxOptions.Text)
                    If maxOption <= 0 Then
                        maxOption = 1
                    End If
                End If
                If IsNumeric(Me.TXTminOptions.Text) Then
                    minOption = CInt(Me.TXTminOptions.Text)
                    If minOption < 0 Then
                        minOption = 0
                    End If
                End If

                FSmultipleChoice.Visible = True
                Me.CBXmultipleChoice.Checked = True
                CBXmultipleChoice.Disabled = True
                Me.TXTmaxOptions.Text = maxOption
                Me.TXTminOptions.Text = minOption
                Me.MLVadvanced.SetActiveView(VIWmultipleChoice)
            Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                FSmultipleChoice.Visible = False
                Me.MLVadvanced.SetActiveView(VIWmultipleChoice)
        End Select
        RaiseEvent SaveDisclaimerType(IdField, dType)
    End Sub

    Public Property Tags As String
        Get
            Return Me.TXBtags.Text
        End Get
        Set(value As String)
            Me.TXBtags.Text = value
        End Set
    End Property
End Class