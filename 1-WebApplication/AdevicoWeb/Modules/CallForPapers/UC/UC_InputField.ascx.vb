Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports System.Xml
Imports lm.Comol.Modules.CallForPapers.Advanced.Domain
Imports NHibernate.Hql.Ast.ANTLR
Imports NHibernate.Mapping
Imports Telerik.Web.UI

Public Class UC_InputField
    Inherits BaseControl
    Implements IViewInputField

    Public Event FieldUpdated()


    Public TagHelper As AdvTagHelper

#Region "Context"
    Private _Presenter As InputFieldPresenter
    Private ReadOnly Property CurrentPresenter() As InputFieldPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InputFieldPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property Disabled As Boolean Implements IViewInputField.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            Me.TXBcompanycode.Enabled = Not value
            Me.TXBcompanytaxcode.Enabled = Not value
            Me.TXBmail.Enabled = Not value
            'Me.TXBmultiline.Enabled = Not value
            UC_TextArea.Enabled = Not value
            Me.TXBname.Enabled = Not value
            Me.TXBsingleline.Enabled = Not value
            Me.TXBsurname.Enabled = Not value
            Me.TXBtaxcode.Enabled = Not value
            Me.TXBtelephonenumber.Enabled = Not value
            Me.TXBvatcode.Enabled = Not value
            Me.TXBzipcode.Enabled = Not value
            Me.BTNremoveFile.Enabled = Not value
            Me.DDLitems.Enabled = Not value
            Me.RBLdisclaimer.Enabled = Not value
            Me.RBLitems.Enabled = Not value
            Me.RDPdate.Enabled = Not value
            Me.RDPdatetime.Enabled = Not value
            Me.RDPtime.Enabled = Not value
            Me.RBLsingleOption.Enabled = Not value
            Me.CBLmultiOptions.Enabled = Not value

            For Each row As RepeaterItem In RPTcheckboxlist.Items
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBoption")
                oCheck.Disabled = value
            Next
            If value Then
                TXBcheckboxlist.Enabled = False
                TXBradiobuttonlist.Enabled = False
                'TXBcheckboxlist.ReadOnly = True
                'TXBradiobuttonlist.ReadOnly = True
            End If
        End Set
    End Property
    Public Property FieldType As FieldType Implements IViewInputField.FieldType
        Get
            Return ViewStateOrDefault("FieldType", FieldType.None)
        End Get
        Set(value As FieldType)
            ViewState("FieldType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewInputField.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Public Property IdField As Long Implements IViewInputField.IdField
        Get
            Return ViewStateOrDefault("IdField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdField") = value
        End Set
    End Property
    Private Property IdSubmittedField As Long Implements IViewInputField.IdSubmittedField
        Get
            Return ViewStateOrDefault("IdSubmittedField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmittedField") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewInputField.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdLink As Long Implements IViewInputField.IdLink
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property

    Private Property Mandatory As Boolean Implements IViewInputField.Mandatory
        Get
            Return ViewStateOrDefault("Mandatory", False)
        End Get
        Set(value As Boolean)
            ViewState("Mandatory") = value
        End Set
    End Property

    Public ReadOnly Property isValid As Boolean Implements IViewInputField.isValid
        Get
            Dim isMandatory As Boolean = Me.Mandatory
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                    Dim selected As Integer = GetSelectedItems(False).Count
                    Return (selected >= MinOptions AndAlso selected <= MaxOptions)
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                    Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
                    Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))

                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                    , lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine _
                    , lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, lm.Comol.Modules.CallForPapers.Domain.FieldType.Name _
                    , FieldType.TaxCode, FieldType.VatCode, FieldType.ZipCode
                    Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)

                    Return (Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(oTextBox.Text)))

                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine
                    Return (Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(UC_TextArea.Text)))


                Case FieldType.TelephoneNumber
                    Return Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(Me.TXBtelephonenumber.Text))
                Case FieldType.Time
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPtime.SelectedDate.HasValue)
                Case FieldType.Date
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPdate.SelectedDate.HasValue)
                Case FieldType.DateTime
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPdatetime.SelectedDate.HasValue)
                Case FieldType.Disclaimer
                    Select Case DisclaimerType
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                            Return True
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                            Return (Me.RBLdisclaimer.SelectedIndex = 0)
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                            Dim selected As Integer = (From i As ListItem In Me.RBLsingleOption.Items Where i.Selected).Count
                            Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                            Dim selected As Integer = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected).Count
                            Return (selected >= MinOptions AndAlso selected <= MaxOptions)
                    End Select

                Case FieldType.DropDownList
                    Return Me.DDLitems.SelectedIndex <> -1
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.IdLink >= 0)
                Case FieldType.Mail
                    Dim mail As String = TXBmail.Text
                    If Not String.IsNullOrEmpty(mail) Then
                        mail = mail.Trim
                        TXBmail.Text = mail
                    End If
                    Return (Not isMandatory AndAlso String.IsNullOrEmpty(mail)) OrElse (lm.Comol.Core.Authentication.Helpers.ValidationHelpers.Mail(mail, REVmail.ValidationExpression) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(mail))))
                Case FieldType.Note
                    Return True
            End Select
        End Get
    End Property

    Private Property MaxChars As Integer Implements IViewInputField.MaxChars
        Get
            Return ViewStateOrDefault("MaxChars", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MaxChars") = value
        End Set
    End Property
    Private Property MaxOptions As Integer Implements IViewInputField.MaxOptions
        Get
            Return ViewStateOrDefault("MaxOptions", CInt(1))
        End Get
        Set(value As Integer)
            ViewState("MaxOptions") = value
        End Set
    End Property
    Private Property MinOptions As Integer Implements IViewInputField.MinOptions
        Get
            Return ViewStateOrDefault("MinOptions", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MinOptions") = value
        End Set
    End Property
    Private Property Options As List(Of dtoFieldOption) Implements IViewInputField.Options
        Get
            Return ViewStateOrDefault("Options", New List(Of dtoFieldOption))
        End Get
        Set(value As List(Of dtoFieldOption))
            ViewState("Options") = value
        End Set
    End Property
    Public Property CurrentError As FieldError Implements IViewInputField.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", FieldError.None)
        End Get
        Set(value As FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property DisclaimerType As DisclaimerType Implements IViewInputField.DisclaimerType
        Get
            Return ViewStateOrDefault("DisclaimerType", DisclaimerType.None)
        End Get
        Set(value As DisclaimerType)
            ViewState("DisclaimerType") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewInputField.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
        End Set
    End Property
    Public Property ReviewMode As Boolean Implements IViewInputField.ReviewMode
        Get
            Return ViewStateOrDefault("ReviewMode", False)
        End Get
        Set(value As Boolean)
            ViewState("ReviewMode") = value
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
#Region "internal"
    Private _PostBackTriggers As String
    Public Property PostBackTriggers As String
        Get
            Return _PostBackTriggers
        End Get
        Set(value As String)
            _PostBackTriggers = value
        End Set
    End Property
    Public Event RemoveFile(ByVal idSubmittedField As Long)
    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = FieldError.None, "", "error")
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
            .setButton(BTNremoveFile, True, , , True)
            .setRadioButtonList(RBLdisclaimer, "True")
            .setRadioButtonList(RBLdisclaimer, "")
        End With
    End Sub
#End Region
    Public Sub InitializeControl(
                                idCall As Long,
                                idSubmission As Long,
                                identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier,
                                field As dtoSubmissionValueField,
                                disabled As Boolean,
                                isPublic As Boolean) _
                            Implements IViewInputField.InitializeControl

        CurrentError = FieldError.None
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, field, disabled, isPublic)

    End Sub


    

    Public Sub InitializeControl(
                                idCall As Long,
                                idSubmission As Long,
                                identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier,
                                item As dtoSubmissionValueField,
                                disabled As Boolean,
                                isPublic As Boolean,
                                err As FieldError) _
                            Implements IViewInputField.InitializeControl
        CurrentError = err
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, item, disabled, isPublic)

        If Not IsNothing(item) AndAlso Not IsNothing(item.Field) Then
            Dim oLabel As Label = Me.FindControl("LBerrorMessage" & item.Field.Type.ToString.ToLower)

            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not (err = FieldError.None)

                If Not (err = FieldError.None) Then
                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
                End If
            End If
        End If
    End Sub

    Private Sub SetupView(item As dtoSubmissionValueField, idUploader As Integer, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, isPublic As Boolean) Implements IViewInputField.SetupView

        Dim css As String = "tagConteinerItem "
        'Tag
        If Not IsNothing(TagHelper) AndAlso TagHelper.HasValue Then
            Me.DVtagContainer.Attributes.Add("class", css + TagHelper.GetTagCssMultiString(item.Field.Tags))
        End If

        
        IdSubmittedField = item.IdValueField
        IdField = item.IdField
        FieldType = item.Field.Type

        Dim oLabel As Label = Nothing
        Dim oGeneric As HtmlGenericControl = Nothing
        If ReviewMode Then
            oGeneric = FindControl("SPN" & FieldType.ToString & "RevisionField")
            If Not IsNothing(oGeneric) Then
                oGeneric.Visible = True
                oLabel = FindControl("LB" & FieldType.ToString & "RevisionField")
                oLabel.Text = Resource.getValue("RevisionField.LabelInfo")
            End If
        End If





        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Description")
        Else
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & item.Field.DisclaimerType.ToString & "Description")
        End If
        If Not IsNothing(oLabel) Then
            If Not String.IsNullOrEmpty(item.Field.Description) Then
                oLabel.Text = item.Field.Description.Replace(Environment.NewLine, "<br/>")
            Else
                oLabel.Text = ""
            End If

        End If

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer AndAlso item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note Then
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Help")
            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not String.IsNullOrEmpty(item.Field.ToolTip)
                oLabel.Text = item.Field.ToolTip
            End If
        End If

        Dim oLabelTag = Nothing

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabelTag = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Tags")
        Else
            oLabelTag = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString & "Tags")
        End If

        If Not IsNothing(oLabelTag) Then
            SetTagLabel(item.Field.Tags, oLabelTag)
        End If


        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Text")
        Else
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString & "Text")
        End If

        If Not IsNothing(oLabel) Then
            If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note Then
                oLabel.Text = IIf(item.Field.Mandatory, "(*)", "") & item.Field.Name
            Else
                oLabel.Text = item.Field.Name
            End If
        End If

        Dim oLiteral As Literal = Me.FindControl("LTmaxChars" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oLiteral) Then
            oLiteral.Text = Me.Resource.getValue("MaxCharsInfo")
        End If

        oGeneric = Me.FindControl("SPNmaxChar" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oGeneric) AndAlso item.Field.MaxLength > 0 AndAlso (item.Field.Type <> FieldType.CheckboxList AndAlso item.Field.Type <> FieldType.RadioButtonList AndAlso item.Field.Type <> FieldType.DropDownList AndAlso item.Field.Type <> FieldType.Mail) Then
            oGeneric.Visible = True
        End If

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oGeneric = Me.FindControl("DV" & item.Field.Type.ToString.ToLower)
        Else
            oGeneric = Me.FindControl("DV" & item.Field.Type.ToString.ToLower & item.Field.DisclaimerType.ToString)
        End If


        If Not IsNothing(oGeneric) Then
            'If Not IsNothing(oTextBox) Then
            Dim cssClass = oGeneric.Attributes("class")
            If (item.FieldError = FieldError.None) Then
                cssClass = Replace(cssClass, " error", "")
            Else
                If Not cssClass.Contains(" error") Then
                    cssClass &= " error"
                End If
            End If
            oGeneric.Attributes("class") = cssClass
        End If

        Dim oValidator As RequiredFieldValidator
        Select Case item.Field.Type
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                Dim values As String = ""
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    values = item.Value.Text
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    values = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id
                End If
                Me.RPTcheckboxlist.DataSource = GetDisplayOptions(item.Field.Options, values, False)
                Me.RPTcheckboxlist.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    If Not IsNothing(opt) Then
                        Me.TXBcheckboxlist.Text = item.Value.FreeText
                    End If
                End If

                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption

                Me.SPNminMaxcheckboxlist.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                If item.Field.MinOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("min-") Then
                    SPNcheckboxlist.Attributes("class") &= " min-" & item.Field.MinOption.ToString
                    Me.Resource.setLiteral(LTminOptionscheckboxlist)
                    LBminOptioncheckboxlist.Text = item.Field.MinOption.ToString
                Else
                    LTminOptionscheckboxlist.Visible = False
                    LBminOptioncheckboxlist.Visible = False
                End If
                If item.Field.MaxOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("max-") Then
                    SPNcheckboxlist.Attributes("class") &= " max-" & item.Field.MaxOption.ToString
                    Me.Resource.setLiteral(LTmaxOptionscheckboxlist)
                    LBmaxOptioncheckboxlist.Text = item.Field.MaxOption.ToString
                Else
                    LTmaxOptionscheckboxlist.Visible = False
                    LBmaxOptioncheckboxlist.Visible = False
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                Me.DDLitems.DataSource = item.Field.Options
                Me.DDLitems.DataTextField = "Name"
                Me.DDLitems.DataValueField = "Id"
                Me.DDLitems.DataBind()
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        Me.DDLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    Try
                        Me.DDLitems.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id
                    Catch ex As Exception

                    End Try
                End If

                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                Me.RBLitems.DataSource = item.Field.Options
                Me.RBLitems.DataTextField = "Name"
                Me.RBLitems.DataValueField = "Id"
                Me.RBLitems.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) Then
                    Me.SPNtextOptionRadioButtonList.Visible = True
                    Dim oItem As ListItem = Me.RBLitems.Items.FindByValue(opt.Id)
                    If Not IsNothing(oItem) Then
                        oItem.Attributes.Add("class", "extraoption")
                    End If
                    Me.TXBradiobuttonlist.Text = item.Value.FreeText

                    If Not (item.Value.Text = opt.Id.ToString()) Then
                        Me.TXBradiobuttonlist.CssClass &= "disabled"
                        Me.SPNtextOptionRadioButtonList.Attributes.Add("class", "textoption disabled")
                    End If

                End If

                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        Me.RBLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    Try
                        RBLitems.SelectedIndex = -1
                        Dim oItem As ListItem = Me.RBLitems.Items.FindByValue(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id)
                        If Not IsNothing(oItem) Then
                            oItem.Selected = True

                        End If
                    Catch ex As Exception

                    End Try
                End If


                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption
                '               Me.SPNminMaxradioButtonlist.Visible = False
                '(item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)
                '               If item.Field.MinOption > 0 AndAlso Not Me.RBLitems.CssClass.Contains("min-") Then
                '                   Me.RBLitems.CssClass &= "min-" & item.Field.MinOption.ToString
                '                   Me.Resource.setLiteral(LTminOptionsradioButtonlist)
                '                   LBminOptionradioButtonlist.Text = item.Field.MinOption.ToString
                '               End If
                '               If item.Field.MaxOption > 0 AndAlso Not Me.RBLitems.CssClass.Contains("max-") Then
                '                   Me.RBLitems.CssClass &= "max-" & item.Field.MaxOption.ToString
                '                   Me.Resource.setLiteral(LTmaxOptionsradioButtonlist)
                '                   LBmaxOptionradioButtonlist.Text = item.Field.MaxOption.ToString
                '               End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer

                Select Case item.Field.DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        Exit Select
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso item.Value.Text.ToLower = "true" Then
                            Me.RBLdisclaimer.SelectedIndex = 0
                        Else
                            Me.RBLdisclaimer.SelectedIndex = 1
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Me.CBLmultiOptions.DataSource = item.Field.Options
                        Me.CBLmultiOptions.DataTextField = "Name"
                        Me.CBLmultiOptions.DataValueField = "Id"
                        Me.CBLmultiOptions.DataBind()
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                            Dim values As List(Of String) = item.Value.Text.Split("|").ToList
                            For Each value As String In values.Where(Function(v) Not String.IsNullOrEmpty(v)).ToList
                                Dim oItem As ListItem = Me.CBLmultiOptions.Items.FindByValue(value)
                                If Not IsNothing(oItem) Then
                                    oItem.Selected = True
                                End If
                            Next
                        ElseIf item.IdValueField = 0 AndAlso item.Field.Options.Where(Function(o) o.IsDefault).Any Then
                            Me.CBLmultiOptions.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault.Id
                        End If

                        Me.MinOptions = item.Field.MinOption
                        Me.MaxOptions = item.Field.MaxOption

                        Me.SPNminMaxCustomMultiOptions.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                        If item.Field.MinOption > 0 AndAlso Not CBLmultiOptions.CssClass.Contains("min-") Then
                            CBLmultiOptions.CssClass &= " min-" & item.Field.MinOption.ToString
                            Me.Resource.setLiteral(LTminOptionsCustomMultiOptions)
                            LBminOptionCustomMultiOptions.Text = item.Field.MinOption.ToString
                        Else
                            LTminOptionsCustomMultiOptions.Visible = False
                            LBminOptionCustomMultiOptions.Visible = False
                        End If
                        If item.Field.MaxOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("max-") Then
                            CBLmultiOptions.CssClass &= " max-" & item.Field.MaxOption.ToString
                            Me.Resource.setLiteral(LTmaxOptionsCustomMultiOptions)
                            LBmaxOptionCustomMultiOptions.Text = item.Field.MaxOption.ToString
                        Else
                            LTmaxOptionsCustomMultiOptions.Visible = False
                            LBmaxOptionCustomMultiOptions.Visible = False
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        Me.RBLsingleOption.DataSource = item.Field.Options
                        Me.RBLsingleOption.DataTextField = "Name"
                        Me.RBLsingleOption.DataValueField = "Id"
                        Me.RBLsingleOption.DataBind()
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                            Try
                                Me.RBLsingleOption.SelectedValue = item.Value.Text
                            Catch ex As Exception

                            End Try
                        ElseIf item.IdValueField = 0 AndAlso item.Field.Options.Where(Function(o) o.IsDefault).Any Then
                            Me.RBLsingleOption.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault.Id
                        End If
                        Me.MinOptions = item.Field.MinOption
                        Me.MaxOptions = item.Field.MaxOption
                    Case Else

                End Select
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Dim toUpload As Boolean = (IsNothing(item.Value) OrElse IsNothing(item.Value.Link))
                'Me.CTRLfileUploader.AjaxEnabled = Not Disabled
                CTRLinternalUploader.Enabled = Not Disabled
                CTRLinternalUploader.Visible = toUpload

                Me.BTNremoveFile.Visible = Not toUpload
                Me.CTRLdisplayItem.Visible = Not toUpload
                SetInternazionalizzazione()
                If Not toUpload Then
                    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
                    initializer.RefreshContainerPage = False
                    initializer.SaveObjectStatistics = True
                    initializer.Link = item.Value.Link
                    initializer.SetOnModalPageByItem = True
                    initializer.SetPreviousPage = False
                    Dim actions As List(Of dtoModuleActionControl)
                    If Disabled Then
                        CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
                    Else
                        CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
                    End If
                    'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                    '' DIMENSIONI IMMAGINI
                    'initializer.IconSize = Helpers.IconSize.Small
                    'CTRLdisplayFile.EnableAnchor = True
                    'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                    'initializer.Link = item.Value.Link
                    'CTRLdisplayFile.InsideOtherModule = True
                    'Dim actions As List(Of dtoModuleActionControl)
                    'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
                    Me.IdLink = item.Value.Link.Id
                    Me.LBfileinputHelp.Visible = False
                Else
                    CTRLinternalUploader.PostbackTriggers = PostBackTriggers
                    CTRLinternalUploader.AllowAnonymousUpload = isPublic
                    CTRLinternalUploader.InitializeControl(idUploader, identifier)
                    Me.IdLink = 0
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                RDPdate.MinDate = New DateTime(1900, 1, 1)
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPdate.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPdate.SelectedDate = Nothing
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                RDPdatetime.MinDate = New DateTime(1900, 1, 1)
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPdatetime.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPdatetime.SelectedDate = Nothing
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPtime.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPtime.SelectedDate = Nothing
                End If
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname,
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                Dim oTextBox As TextBox = Me.FindControl("TXB" & item.Field.Type.ToString.ToLower)

                If item.Field.MaxLength > 0 Then
                    'If item.Field.Type = FieldType.MultiLine Then
                    '    oTextBox.Attributes.Add("maxlength", item.Field.MaxLength)
                    'Else
                    oTextBox.MaxLength = item.Field.MaxLength
                    'End If
                End If
                If IsNothing(item.Value) Then
                    oTextBox.Text = ""
                Else
                    oTextBox.Text = item.Value.Text
                End If

            Case FieldType.MultiLine
                If item.Field.MaxLength > 0 Then
                    UC_TextArea.MaxLenght = item.Field.MaxLength
                    LTmaxCharsmultiline_Value.Text = item.Field.MaxLength
                Else
                    LTmaxCharsmultiline_Value.Text = ""
                End If

                If IsNothing(item.Value) Then
                    UC_TextArea.Text = ""
                Else
                    UC_TextArea.Text = item.Value.Text
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableSimple
                'Resource.setLabel(LBTableSimpleDescription)
                _addRowIndex = -1
                _remRowIndex = -1
                Me.TableSetting = item.Field.TableFieldSetting
                BindTableData(item.Value.Text)

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableReport
                'Resource.setLabel(LBTableSimpleDescription)
                _addRowIndex = -1
                _remRowIndex = -1
                Me.TableSetting = item.Field.TableFieldSetting
                BindTableReportData(item.Value.Text)

                'If IsNothing(CurrentSummaryItem) Then
                '    CurrentSummaryItem = New dtoSummaryTableReportItem()
                'End If
                CurrentSummaryItem.Name = item.Field.Name


            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                Exit Select
            Case Else

        End Select
        Dim name As String = ""
        If DisclaimerType <> lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None Then
            name = "VIW" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString
        Else
            name = "VIW" & item.Field.Type.ToString.ToLower
        End If
        Dim view As System.Web.UI.WebControls.View = Me.FindControl(name)
        If Not IsNothing(view) Then
            Me.MLVfield.SetActiveView(view)
        End If
    End Sub

    'Private Property TabelData As String
    '    Get
    '        Return Me.ViewStateOrDefault("SourceTableData", "")
    '    End Get
    '    Set(value As String)
    '        Me.ViewState("SourceTableData") = value
    '    End Set
    'End Property


    'Private Property TableMaxRow As Integer
    '    Get
    '        Dim out As Integer = 50
    '        Try
    '            out = CInt(Me.ViewStateOrDefault("TableMaxRow", 50))
    '        Catch ex As Exception

    '        End Try
    '        Return out
    '    End Get
    '    Set(value As Integer)
    '        Me.ViewState("SourceTableData") = value
    '    End Set
    'End Property


    Private Property TableSetting As lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField
        Get
            Return ViewStateOrDefault("CurrentTableSettings", New lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField)
            ViewState("CurrentTableSettings") = value
        End Set
    End Property



    Public Function GetField() _
        As dtoSubmissionValueField _
        Implements IViewInputField.GetField


        Dim dto As New dtoSubmissionValueField
        dto.Field = New dtoCallField

        dto.IdValueField = IdSubmittedField
        dto.Value = New dtoValueField("")
        dto.Value.IdLink = IdLink
        dto.Field.Id = IdField
        dto.Field.Type = FieldType
        dto.Field.DisclaimerType = DisclaimerType

        Dim oLabel As Label = Nothing
        If dto.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabel = Me.FindControl("LB" & dto.Field.Type.ToString.ToLower & "Text")
        Else
            oLabel = Me.FindControl("LB" & dto.Field.Type.ToString.ToLower & dto.Field.DisclaimerType.ToString & "Text")
        End If
        If Not IsNothing(oLabel) Then
            dto.Field.Name = oLabel.Text
        End If


        Select Case FieldType
            Case FieldType.CheckboxList
                Dim items As List(Of String) = GetSelectedItems(False)
                If (items.Count = 0) Then
                    dto.Value.Text = ""
                ElseIf (items.Count = 1) Then
                    dto.Value.Text = items(0)
                Else
                    dto.Value.Text = String.Join("|", items.ToArray)
                End If
                Dim opt As dtoFieldOption = Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) AndAlso items.Contains(opt.Id.ToString) Then
                    dto.Value.FreeText = Me.TXBcheckboxlist.Text
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                Try
                    dto.Value.Text = Me.DDLitems.SelectedValue
                Catch ex As Exception

                End Try
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                If Me.RBLitems.SelectedIndex <> -1 Then
                    dto.Value.Text = Me.RBLitems.SelectedValue
                    Dim opt As dtoFieldOption = Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                    If Not IsNothing(opt) AndAlso Me.RBLitems.SelectedValue = opt.Id.ToString Then
                        dto.Value.FreeText = Me.TXBradiobuttonlist.Text
                    End If
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer
                Select Case dto.Field.DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        dto.Value.Text = "True"
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        dto.Value.Text = (Me.RBLdisclaimer.SelectedIndex = 0)
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Dim items As List(Of String) = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected Select i.Value).ToList
                        If (items.Count = 0) Then
                            dto.Value = New dtoValueField("")
                        ElseIf (items.Count = 1) Then
                            dto.Value = New dtoValueField(items(0))
                        Else
                            dto.Value = New dtoValueField(String.Join("|", items.ToArray))
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        dto.Value = New dtoValueField(Me.RBLsingleOption.SelectedValue)
                    Case Else
                        dto.Value = New dtoValueField("")
                End Select

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                'dto.Value = New dtoValueField(Me.CTRLdisplayItem.CurrentFileName)
                Exit Select

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                If Me.RDPdate.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPdate.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                If Me.RDPdatetime.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPdatetime.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                If Me.RDPtime.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPtime.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                Exit Select
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname,
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.Mail
                Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)
                dto.Value.Text = oTextBox.Text
            Case FieldType.MultiLine
                dto.Value.Text = UC_TextArea.Text
            Case FieldType.TableSimple
                dto.Value.Text = GetTableXmlValue(True)
                dto.Field.TableFieldSetting = TableSetting
            Case FieldType.TableReport
                dto.Value.Text = GetTableReportXmlValue(True)
                dto.Field.TableFieldSetting = TableSetting
            Case Else
                dto.Value.Text = ""
        End Select
        dto.FieldError = GetFieldError()

        Return (dto)
    End Function

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyField() Implements IViewInputField.DisplayEmptyField
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub

    Public Sub DisplayInputError() Implements IViewInputField.DisplayInputError

    End Sub
    Private Sub HideInputError() Implements IViewInputField.HideInputError

    End Sub

    Private Sub RefreshFileField(link As lm.Comol.Core.DomainModel.ModuleLink) Implements IViewInputField.RefreshFileField
        Dim uploadFile As Boolean = IsNothing(link)
        '  CTRLinternalUploader.AjaxEnabled = uploadFile
        CTRLinternalUploader.Visible = uploadFile
        Me.BTNremoveFile.Visible = Not uploadFile
        Me.CTRLdisplayItem.Visible = Not uploadFile

        If Not uploadFile Then
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = New liteModuleLink()
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            Dim actions As List(Of dtoModuleActionControl)
            If Disabled Then
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
            Else
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
            End If
            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'CTRLdisplayFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            'initializer.Link = link
            'CTRLdisplayFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
            Me.IdLink = link.Id
        Else
            Me.IdLink = 0
        End If
    End Sub


    Public Function AddInternalFile(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As dtoSubmissionFileValueField 'Implements IViewInputField.AddInternalFile
        If IdLink > 0 Then
            Return Nothing
        Else
            Dim items As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) = CTRLinternalUploader.AddModuleInternalFiles(submission, submission.Id, objectType, moduleCode, moduleAction)

            If IsNothing(items) OrElse Not items.Where(Function(i) i.IsAdded).Any Then
                Return Nothing
            Else
                Dim uploadedFile As New dtoSubmissionFileValueField
                uploadedFile.IdField = IdField
                uploadedFile.ActionLink = items.Where(Function(i) i.IsAdded).Select(Function(i) i.Link).FirstOrDefault()
                Return uploadedFile
            End If

            'Dim uploadedFile As New dtoSubmissionFileValueField
            'uploadedFile.ActionLink = CTRLfileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, submission, moduleCode, moduleAction, objectType)

            'If uploadedFile.ActionLink Is Nothing Then
            '    uploadedFile = Nothing
            'Else
            '    uploadedFile.IdField = IdField
            'End If
            'Return uploadedFile
        End If
    End Function

    Private Sub CTRLinternalUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLinternalUploader.IsValidOperation
        isvalid = True
    End Sub
#Region "Control"
    Private Sub BTNremoveFile_Click(sender As Object, e As System.EventArgs) Handles BTNremoveFile.Click
        RaiseEvent RemoveFile(IdSubmittedField)
    End Sub

    Private Sub RPTcheckboxlist_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcheckboxlist.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBoption")
        oCheck.Checked = item.Selected
        If item.IsFreeValue Then
            Me.SPNtextOptionCheckBoxList.Visible = True
            oCheck.Attributes("class") = oCheck.Attributes("class") & " extraoption"
        End If

    End Sub
    Private Function GetSelectedItems(isSingleSelection As Boolean) As List(Of String)
        Dim results As New List(Of String)
        If isSingleSelection Then
        Else
            For Each row As RepeaterItem In RPTcheckboxlist.Items
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBoption")
                If oCheck.Checked Then
                    results.Add(oCheck.Value)
                End If
            Next
        End If
        Return results
    End Function
    Private Function GetDisplayOptions(options As List(Of dtoFieldOption), value As String, isSingleSelection As Boolean) As List(Of dtoOptionDisplay)
        Dim results As List(Of dtoOptionDisplay) = options.Select(Function(o) New dtoOptionDisplay(o)).ToList
        Dim values As List(Of String) = value.Split("|").ToList
        For Each item As dtoOptionDisplay In results.Where(Function(v) values.Contains(v.Id)).ToList
            item.Selected = True
        Next
        If (isSingleSelection AndAlso results.Where(Function(r) r.Selected).Count > 1) Then
            For Each item As dtoOptionDisplay In results.Where(Function(v) v.Selected).Skip(1).ToList
                item.Selected = False
            Next
        End If
        Return results
    End Function

    Private Class dtoOptionDisplay
        Public Id As String
        Public Name As String
        Public Selected As Boolean
        Public IsFreeValue As Boolean
        Sub New()

        End Sub
        Sub New(opt As dtoFieldOption)
            Id = opt.Id
            Name = opt.Name
            Selected = False
            IsFreeValue = opt.IsFreeValue
        End Sub
    End Class

    Public Function GetFieldError() As FieldError
        Dim isMandatory As Boolean = Me.Mandatory
        Select Case FieldType
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                Dim selected As Integer = GetSelectedItems(False).Count
                Return GetOptionsError(isMandatory, selected, True)
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
                Return GetOptionsError(isMandatory, selected, False)
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, lm.Comol.Modules.CallForPapers.Domain.FieldType.Name _
                , FieldType.TaxCode, FieldType.VatCode, FieldType.ZipCode
                Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)

                If (isMandatory AndAlso String.IsNullOrEmpty(TrimValue(oTextBox.Text))) Then
                    Return FieldError.Mandatory
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine
                If (isMandatory AndAlso String.IsNullOrEmpty(TrimValue(UC_TextArea.Text))) Then
                    Return FieldError.Mandatory
                Else
                    Return UC_TextArea.HasLenghError
                End If

            Case FieldType.TelephoneNumber
                If (isMandatory AndAlso String.IsNullOrEmpty(TrimValue(TXBtelephonenumber.Text))) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Time
                If (isMandatory AndAlso Not Me.RDPtime.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Date
                If (isMandatory AndAlso Not Me.RDPdate.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.DateTime
                If (isMandatory AndAlso Not Me.RDPdatetime.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Disclaimer
                Select Case DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        Return FieldError.None
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        If Not (Me.RBLdisclaimer.SelectedIndex = 0) Then
                            Return FieldError.Invalid
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        Dim selected As Integer = (From i As ListItem In Me.RBLsingleOption.Items Where i.Selected).Count
                        Return GetOptionsError(isMandatory, selected, False)
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Dim selected As Integer = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected).Count
                        Return GetOptionsError(isMandatory, selected, True)
                End Select

            Case FieldType.DropDownList
                If isMandatory AndAlso DDLitems.SelectedIndex = -1 Then
                    Return FieldError.Invalid
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                If (isMandatory AndAlso Me.IdLink <= 0) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Mail
                Dim mail As String = TrimValue(TXBmail.Text)
                If isMandatory AndAlso String.IsNullOrEmpty(mail) Then
                    Return FieldError.Mandatory
                ElseIf Not String.IsNullOrEmpty(mail) AndAlso Not lm.Comol.Core.Authentication.Helpers.ValidationHelpers.Mail(mail, REVmail.ValidationExpression) Then
                    Return FieldError.InvalidFormat
                End If
            Case FieldType.TableSimple, FieldType.TableReport
                If (isMandatory) AndAlso IsTableEmpty Then
                    Return FieldError.Mandatory
                End If
        End Select
        Return FieldError.None
    End Function

    Private Function TrimValue(value As String) As String
        If Not String.IsNullOrEmpty(value) Then
            value = value.Trim
        End If
        Return value
    End Function

    Private Function GetOptionsError(ByVal isMandatory As Boolean, ByVal selected As Integer, isMultiOption As Boolean) As FieldError
        If (selected >= MinOptions AndAlso selected <= MaxOptions) Then
            If Not isMultiOption AndAlso isMandatory AndAlso selected = 0 Then
                Return FieldError.Mandatory
            Else
                Return FieldError.None
            End If
        ElseIf selected > MaxOptions Then
            Return FieldError.MoreOptions
        ElseIf selected < MinOptions Then
            Return FieldError.LessOptions
        End If
        Return FieldError.None
    End Function
#End Region












#Region "Table"
    Private Property _colNum As Integer
        Get
            Return CInt(ViewStateOrDefault("TableColumnsNumber", 1))
        End Get
        Set(value As Integer)
            ViewState("TableColumnsNumber") = value
        End Set
    End Property

    Private currentrow As Integer = 0
    Private _addRowIndex = -1
    Private _remRowIndex = -1
    'Private _tableMaxRow As Integer = 50
    Private Property _canAddrow As Boolean
        Get
            Return ViewStateOrDefault("TableCanAddMoreRows", False)
        End Get
        Set(value As Boolean)
            ViewState("TableCanAddMoreRows") = value
        End Set
    End Property



    Private Sub BindTableData(ByVal content As String) ', ByVal isCalculated As Boolean)

        '_tableMaxRow = tableSetting.MaxRows
        'Headers
        If (Not String.IsNullOrEmpty(TableSetting.Cols)) Then
            Dim header As List(Of String) = TableSetting.Cols.Split("|").ToList()
            _colNum = header.Count

            

                RPTHeader.DataSource = header
                RPTHeader.DataBind()
            
        End If

        'Rows
        Dim rowsCols As New List(Of List(Of String))
        Dim Lastrow As List(Of String)

        If Not String.IsNullOrEmpty(content) Then
            'To Helper!!!

            content = String.Format("<?xml version=""1.0"" encoding=""ISO-8859-1""?>{0}{1}", System.Environment.NewLine, content)

            Dim doc As New XmlDocument
            doc.LoadXml(content)
            'Dim trXml As XmlNodeList = doc.GetElementsByTagName("tr")

            For Each trXml As XmlNode In doc.GetElementsByTagName("tr")
                Dim row As List(Of String) = (From tdXml As XmlNode In trXml Select Server.HtmlDecode(tdXml.InnerText)).ToList()
                rowsCols.Add(row)
                Lastrow = row
            Next

        End If

        _canAddrow = (rowsCols.Count() < TableSetting.MaxRows)

        'SE non ho alcuna riga aggiungo una vuota.
        Dim addLastEmpty As Boolean = Not rowsCols.Any()

        'IsNothing(Lastrow) _
        '                              OrElse Not Lastrow.Any() _
        '                              OrElse Not Lastrow.All(Function(s) String.IsNullOrEmpty(s))

        If addLastEmpty AndAlso _canAddrow Then
            Dim voidrow As New List(Of String)
            For vc As Integer = 0 To (_colNum - 1)
                voidrow.Add("")
            Next
            rowsCols.Add(voidrow)
            _canAddrow = (rowsCols.Count() < TableSetting.MaxRows)
        End If

        
            RPTtableRows.DataSource = rowsCols
            RPTtableRows.DataBind()
        
    End Sub

    Private Sub BindTableReportData(ByVal content As String) ', ByVal isCalculated As Boolean)

        '_tableMaxRow = tableSetting.MaxRows
        'Headers
        Dim header As List(Of String) = New List(Of String)()

        If (Not String.IsNullOrEmpty(TableSetting.Cols)) Then
            header = TableSetting.Cols.Split("|").ToList()
            _colNum = header.Count

        Else
            header.Add("")
            _colNum = 1
        End If

        RPTHeaderReport.DataSource = header
        RPTHeaderReport.DataBind()

        'Rows
        Dim rowsCols As New List(Of TableDataObjet)
        Dim Lastrow As TableDataObjet

        Dim currentTotal As Double = 0

        If Not String.IsNullOrEmpty(content) Then
            'To Helper!!!

            content = String.Format("<?xml version=""1.0"" encoding=""ISO-8859-1""?>{0}{1}", System.Environment.NewLine, content)

            Dim doc As New XmlDocument
            doc.LoadXml(content)
            'Dim trXml As XmlNodeList = doc.GetElementsByTagName("tr")
            
            For Each trXml As XmlNode In doc.GetElementsByTagName("tr")
                
                Dim row As New TableDataObjet
                row.Values = New List(Of String)()

                For Each tdXml As XmlNode In trXml
                    If IsNothing(tdXml.Attributes) OrElse tdXml.Attributes.Count = 0 Then
                        row.Values.Add(Server.HtmlDecode(tdXml.InnerText))
                    ElseIf Not IsNothing(tdXml.Attributes("class")) AndAlso (tdXml.Attributes("class").Value = "quantity") Then
                        Double.TryParse(tdXml.InnerText, row.Quantity)
                    ElseIf Not IsNothing(tdXml.Attributes("class")) AndAlso (tdXml.Attributes("class").Value = "unitycost") Then
                        Double.TryParse(tdXml.InnerText, row.Unitcost)
                    End If
                Next

                currentTotal += row.Quantity * row.Unitcost

                rowsCols.Add(row)
                Lastrow = row
            Next

        End If

        _canAddrow = (rowsCols.Count() < TableSetting.MaxRows)

        'SE non ho alcuna riga aggiungo una vuota.
        Dim addLastEmpty As Boolean = Not rowsCols.Any()

        'IsNothing(Lastrow) _
        '                              OrElse Not Lastrow.Any() _
        '                              OrElse Not Lastrow.All(Function(s) String.IsNullOrEmpty(s))

        If addLastEmpty AndAlso _canAddrow Then
            Dim voidrow As TableDataObjet = New TableDataObjet()
            voidrow.Quantity = 0
            voidrow.Unitcost = 0
            voidrow.Values = New List(Of String)()

            For vc As Integer = 0 To (_colNum - 1)
                voidrow.Values.Add("")
            Next
            rowsCols.Add(voidrow)
            _canAddrow = (rowsCols.Count() < TableSetting.MaxRows)
        End If

        RPTRowsReport.DataSource = rowsCols
        RPTRowsReport.DataBind()

        If (Me.TableSetting.MaxTotal = 0) Then
            LTmaxTotal.Text = "&nbsp;"
        Else
            LTmaxTotal.Text = String.Format("{0} {1} &euro;", "Max:", Me.TableSetting.MaxTotal)

            If (currentTotal > Me.TableSetting.MaxTotal) Then
                TRsummary.Attributes.Add("class", "error")
            Else
                TRsummary.Attributes.Remove("class")
            End If
        End If

        'If IsNothing(CurrentSummaryItem) Then
        '    CurrentSummaryItem = New dtoSummaryTableReportItem()
        'End If
        CurrentSummaryItem.Total = currentTotal
        CurrentSummaryItem.MaxTotal = Me.TableSetting.MaxTotal
    End Sub


    Private Sub RPTHeader_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTHeader.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim value As String = e.Item.DataItem
            Dim LTheadCol As Literal = e.Item.FindControl("LTheadCol")
            If Not IsNothing(LTheadCol) Then
                LTheadCol.Text = value
            End If
        End If
    End Sub

    Private Sub RPTHeaderReport_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTHeaderReport.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim value As String = e.Item.DataItem
            Dim LTheadCol As Literal = e.Item.FindControl("LTheadColReport")
            If Not IsNothing(LTheadCol) Then
                LTheadCol.Text = value
            End If
        End If
    End Sub

    Private Sub RPTtableRows_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtableRows.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cells As List(Of String) = e.Item.DataItem
            Dim RPTinput As Repeater = e.Item.FindControl("RPTinput")

            If Not IsNothing(RPTinput) Then
                AddHandler RPTinput.ItemDataBound, AddressOf RPTinput_ItemDataBound
                RPTinput.DataSource = cells
                RPTinput.DataBind()
            End If

            Dim LKBaddRow As LinkButton = e.Item.FindControl("LKBaddRow")
            If Not IsNothing(LKBaddRow) Then
                If (_canAddrow) Then
                    LKBaddRow.Enabled = True
                    LKBaddRow.Visible = True
                    LKBaddRow.CommandName = "AddRow"
                    LKBaddRow.CommandArgument = currentrow
                Else
                    LKBaddRow.Enabled = False
                    LKBaddRow.Visible = False
                End If
            End If

            Dim LKBremRow As LinkButton = e.Item.FindControl("LKBremRow")
            If Not IsNothing(LKBremRow) Then
                LKBremRow.CommandName = "RemRow"
                LKBremRow.CommandArgument = currentrow
            End If

            currentrow += 1
        End If
    End Sub

    Private Sub RPTRowsReport_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTRowsReport.ItemCommand
        Dim RowPosition As Integer = 0
        If (IsNumeric(e.CommandArgument)) Then
            RowPosition = CInt(e.CommandArgument)
        End If

        Dim cName As String = e.CommandName

        If (cName = "RemRow") Then
            _remRowIndex = RowPosition
            _addRowIndex = -1
        ElseIf (cName = "AddRow") Then
            _remRowIndex = -1
            _addRowIndex = RowPosition
        End If

        BindTableReportData(GetTableReportXmlValue(False))
    End Sub

    Private Sub RPTRowsReport_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTRowsReport.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cells As TableDataObjet = e.Item.DataItem
            Dim RPTinput As Repeater = e.Item.FindControl("RPTinputReport")

            If Not IsNothing(RPTinput) Then
                AddHandler RPTinput.ItemDataBound, AddressOf RPTinput_ItemDataBound
                RPTinput.DataSource = cells.Values
                RPTinput.DataBind()
            End If

            Dim TXBunity As TextBox = e.Item.FindControl("TXBunity")
            If Not IsNothing(TXBunity) Then
                TXBunity.Text = cells.Unitcost.ToString().Replace(",", ".")
            End If

            Dim TXBQuantity As TextBox = e.Item.FindControl("TXBQuantity")
            If Not IsNothing(TXBQuantity) Then
                TXBQuantity.Text = cells.Quantity.ToString().Replace(",", ".")
            End If
            
            Dim LKBaddRow As LinkButton = e.Item.FindControl("LKBaddRow")
            If Not IsNothing(LKBaddRow) Then
                If (_canAddrow) Then
                    LKBaddRow.Enabled = True
                    LKBaddRow.Visible = True
                    LKBaddRow.CommandName = "AddRow"
                    LKBaddRow.CommandArgument = currentrow
                Else
                    LKBaddRow.Enabled = False
                    LKBaddRow.Visible = False
                End If
            End If

            Dim LKBremRow As LinkButton = e.Item.FindControl("LKBremRow")
            If Not IsNothing(LKBremRow) Then
                LKBremRow.CommandName = "RemRow"
                LKBremRow.CommandArgument = currentrow
            End If

            TDspanTotal.ColSpan = _colNum + 1

            currentrow += 1
        End If
    End Sub

    Private Sub RPTtableRows_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTtableRows.ItemCommand

        Dim RowPosition As Integer = 0
        If (IsNumeric(e.CommandArgument)) Then
            RowPosition = CInt(e.CommandArgument)
        End If

        Dim cName As String = e.CommandName

        If (cName = "RemRow") Then
            _remRowIndex = RowPosition
            _addRowIndex = -1
        ElseIf (cName = "AddRow") Then
            _remRowIndex = -1
            _addRowIndex = RowPosition
        End If

        Dim IsReport = FieldType.TableReport = FieldType

        BindTableData(GetTableXmlValue(False))

    End Sub


    Private Function GetTableXmlValue(ByVal forSave As Boolean) As String

        Dim outXml As String = ""

        Dim currentRow As Integer = 0
        'Dim index As Integer = 0

        Dim LastRow As Integer = RPTtableRows.Items.Count - 1
        IsTableEmpty = True

        For Each row As RepeaterItem In RPTtableRows.Items
            Dim isEmpty As Boolean = True

            If (row.ItemType = ListItemType.Item OrElse row.ItemType = ListItemType.AlternatingItem) Then
                Dim RowXml As String = ""

                Dim RPTinput As Repeater = row.FindControl("RPTinput")
                'Dim CellInRow As Integer = 0

                'SE l'indice corrente è da togliere, non lo carico!
                'SE l'indice corrente è uguale o maggior del massimo righe consentito, non lo carico
                If Not IsNothing(RPTinput) _
                    AndAlso Not ((_remRowIndex >= 0) AndAlso (_remRowIndex = currentRow)) _
                    AndAlso TableSetting.MaxRows > currentRow Then

                    For Each cell As RepeaterItem In RPTinput.Items
                        Dim stringvalue As String = ""
                        Dim TXBcellInput As TextBox = cell.FindControl("TXBcellInput")

                        If Not IsNothing(TXBcellInput) Then
                            stringvalue = Server.HtmlEncode(TXBcellInput.Text)
                        End If

                        isEmpty = isEmpty AndAlso String.IsNullOrEmpty(stringvalue)

                        RowXml = String.Format("{0}<td>{1}</td>", RowXml, stringvalue)
                        'CellInRow += 1
                    Next
                Else
                    RowXml = ""
                    currentRow += 1
                End If

                Dim AddEmptyRow As Boolean = _canAddrow And (_addRowIndex >= 0 AndAlso currentRow = _addRowIndex)

                If (Not String.IsNullOrEmpty(RowXml)) Then ' AndAlso Not (forSave AndAlso currentRow = LastRow)) Then
                    outXml = String.Format("{0}<tr>{1}</tr>", outXml, RowXml)
                    currentRow += 1
                End If

                If AddEmptyRow AndAlso Not forSave Then
                    RowXml = ""
                    For c As Integer = 0 To Me._colNum - 1
                        RowXml = String.Format("{0}<td>{1}</td>", RowXml, "")
                    Next
                    outXml = String.Format("{0}<tr>{1}</tr>", outXml, RowXml)
                    currentRow += 1
                End If
            End If

            IsTableEmpty = IsTableEmpty AndAlso isEmpty
        Next



        Return String.Format("<table>{0}</table>", outXml)

    End Function

    Private Function GetTableReportXmlValue(ByVal forSave As Boolean) As String

        Dim outXml As String = ""

        Dim currentRow As Integer = 0
        'Dim index As Integer = 0

        Dim LastRow As Integer = RPTRowsReport.Items.Count - 1

        'Dim currentTotal As Double = 0

        IsTableEmpty = True

        For Each row As RepeaterItem In RPTRowsReport.Items
            Dim isEmpty As Boolean = True

            If (row.ItemType = ListItemType.Item OrElse row.ItemType = ListItemType.AlternatingItem) Then
                Dim RowXml As String = ""

                Dim RPTinput As Repeater = row.FindControl("RPTinputReport")
                'Dim CellInRow As Integer = 0

                'SE l'indice corrente è da togliere, non lo carico!
                'SE l'indice corrente è uguale o maggior del massimo righe consentito, non lo carico
                If Not IsNothing(RPTinput) _
                    AndAlso Not ((_remRowIndex >= 0) AndAlso (_remRowIndex = currentRow)) _
                    AndAlso TableSetting.MaxRows > currentRow Then

                    For Each cell As RepeaterItem In RPTinput.Items
                        Dim stringvalue As String = ""
                        Dim TXBcellInput As TextBox = cell.FindControl("TXBcellInput")

                        If Not IsNothing(TXBcellInput) Then
                            stringvalue = Server.HtmlEncode(TXBcellInput.Text)
                        End If

                        isEmpty = isEmpty AndAlso String.IsNullOrEmpty(stringvalue)

                        RowXml = String.Format("{0}<td>{1}</td>", RowXml, stringvalue)
                        'CellInRow += 1
                    Next


                    Dim TXBQuantity As TextBox = row.FindControl("TXBQuantity")
                    Dim TXBunity As TextBox = row.FindControl("TXBunity")

                    Dim quantity As Double = 0
                    Dim unityCost As Double = 0


                    
                    If Not IsNothing(TXBQuantity) AndAlso IsNumeric(TXBQuantity.Text) Then
                        Double.TryParse(TXBQuantity.Text.Replace(".", ","), quantity) '.Replace(".", ",")
                    End If

                    RowXml = String.Format("{0}<td class=""quantity"">{1}</td>", RowXml, quantity)

                    If Not IsNothing(TXBunity) AndAlso IsNumeric(TXBunity.Text) Then
                        Double.TryParse(TXBunity.Text.Replace(".", ","), unityCost) '.Replace(".", ",")
                    End If

                    RowXml = String.Format("{0}<td class=""unitycost"">{1}</td>", RowXml, unityCost)

                    isEmpty = isEmpty AndAlso quantity > 0 AndAlso unityCost > 0
                    RowXml = String.Format("{0}<td class=""total"">{1}</td>", RowXml, (quantity * unityCost))

                    'currentTotal += (quantity * unityCost)
                Else
                    RowXml = ""
                    'currentRow += 1
                End If



                Dim AddEmptyRow As Boolean = _canAddrow And (_addRowIndex >= 0 AndAlso currentRow = _addRowIndex)



                If (Not String.IsNullOrEmpty(RowXml)) Then ' AndAlso Not (forSave AndAlso currentRow = LastRow)) Then
                    outXml = String.Format("{0}<tr>{1}</tr>", outXml, RowXml)
                    currentRow += 1
                End If




                If AddEmptyRow AndAlso Not forSave Then
                    RowXml = ""
                    For c As Integer = 0 To Me._colNum - 1
                        RowXml = String.Format("{0}<td>{1}</td>", RowXml, "")
                        RowXml = String.Format("{0}<td class=""quantity"">{1}</td>", RowXml, 0)
                        RowXml = String.Format("{0}<td class=""unitycost"">{1}</td>", RowXml, 0)
                    Next
                    outXml = String.Format("{0}<tr>{1}</tr>", outXml, RowXml)
                    currentRow += 1
                End If



            End If

            IsTableEmpty = IsTableEmpty AndAlso isEmpty
        Next



        Return String.Format("<table>{0}</table>", outXml)

    End Function

    Private Property IsTableEmpty As Boolean
        Get
            Return ViewStateOrDefault("IsTableEmpty", False)
        End Get
        Set(value As Boolean)
            ViewState("IsTableEmpty") = value
        End Set
    End Property


    Private Sub RPTinput_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim TXBcellInput As TextBox = e.Item.FindControl("TXBcellInput")
            If Not IsNothing(TXBcellInput) Then
                TXBcellInput.Text = Server.HtmlDecode(e.Item.DataItem.ToString())
            End If
        End If
    End Sub


    Public Property CurrentSummaryItem As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem
        Get
            If IsNothing(_CurrentSummary) Then
                _CurrentSummary = New dtoSummaryTableReportItem()
            End If
            Return _CurrentSummary
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem)
            _CurrentSummary = value
        End Set
    End Property

    Private _CurrentSummary As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem


    Public Sub BindTableSummaryReport(reports As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal)

        Me.MLVfield.SetActiveView(VIWtablesummary)

        RPTtabelSummary.DataSource = reports.Tables
        RPTtabelSummary.DataBind()

        'If (reports.MaxTotal = 0) Then
        LTsummaryTotal.Text = reports.Total
        'Else
        'LTsummaryTotal.Text = String.Format("{0}&euro;/{1}&euro;", reports.Total, reports.MaxTotal)
        'End If

    End Sub


    Private Sub RPTtabelSummary_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtabelSummary.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim values As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem = e.Item.DataItem

            Dim LTtableName As Literal = e.Item.FindControl("LTtableName")
            LTtableName.Text = values.Name

            Dim LTtableTotal As Literal = e.Item.FindControl("LTtableTotal")

            If (values.MaxTotal = 0) Then
                LTtableTotal.Text = String.Format("{0} &euro;", values.Total)
            Else
                LTtableTotal.Text = String.Format("{0} &euro; <span class=""totalHidden""> / {1} &euro;</span>", values.Total, values.MaxTotal)
            End If

        End If


    End Sub


#End Region

    Private Class TableDataObjet
        Public Values As List(Of String)
        Public Quantity As Double
        Public Unitcost As Double
    End Class


    Private Sub SetTagLabel(ByVal tags As String, ByVal label As Label)

        If IsNothing(label) Then
            Return
        End If

        If String.IsNullOrWhiteSpace(tags) Then
            label.Visible = False
            Return
        End If

        label.Visible = True

        Dim isfirst As Boolean = True

        For Each tag As String In tags.Split(",").Distinct.ToList()
            If (isfirst) Then
                label.Text = tag
                isfirst = False
            Else
                label.Text = String.Format("{0}, {1}", label.Text, tag)
            End If

        Next

        If String.IsNullOrEmpty(label.Text) Then
            label.Visible = False
        Else
            label.Text = String.Format("({0})", label.Text)
        End If

    End Sub



End Class