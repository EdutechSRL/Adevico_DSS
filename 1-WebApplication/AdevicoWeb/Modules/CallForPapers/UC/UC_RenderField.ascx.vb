Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports System.Xml
Imports lm.Comol.Modules.CallForPapers.Advanced.Domain

Public Class UC_RenderField
    Inherits BaseControl
    Implements IViewRenderField

    Public TagHelper As AdvTagHelper

#Region "Context"
    Private _Presenter As RenderFieldPresenter
    Private ReadOnly Property CurrentPresenter() As RenderFieldPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RenderFieldPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Private Property Disabled As Boolean Implements IViewRenderField.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
        End Set
    End Property
    Private Property MaxChars As Integer Implements IViewRenderField.MaxChars
        Get
            Return ViewStateOrDefault("MaxChars", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MaxChars") = value
        End Set
    End Property
    Private Property MaxOptions As Integer Implements IViewRenderField.MaxOptions
        Get
            Return ViewStateOrDefault("MaxOptions", CInt(1))
        End Get
        Set(value As Integer)
            ViewState("MaxOptions") = value
        End Set
    End Property

    Private Property MinOptions As Integer Implements IViewRenderField.MinOptions
        Get
            Return ViewStateOrDefault("MinOptions", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MinOptions") = value
        End Set
    End Property
    Public Property CurrentError As FieldError Implements IViewRenderField.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", FieldError.None)
        End Get
        Set(value As FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Public Property FieldType As FieldType Implements IViewRenderField.FieldType
        Get
            Return ViewStateOrDefault("FieldType", FieldType.None)
        End Get
        Set(value As FieldType)
            ViewState("FieldType") = value
        End Set
    End Property
    Public Property IdField As Long Implements IViewRenderField.IdField
        Get
            Return ViewStateOrDefault("IdField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdField") = value
        End Set
    End Property
    Public Property Selected As Boolean Implements IViewRenderField.Selected
        Get
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList, lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList, lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput, lm.Comol.Modules.CallForPapers.Domain.FieldType.Date, lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime, lm.Comol.Modules.CallForPapers.Domain.FieldType.Time, FieldType.MultiLine

                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    Return oChecBox.Checked
                Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                    FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode
                    Return CBXsinglelineRevisionField.Checked
                Case Else
                    Return False
            End Select
        End Get
        Set(value As Boolean)
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList, lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList, lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput, lm.Comol.Modules.CallForPapers.Domain.FieldType.Date, lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime, lm.Comol.Modules.CallForPapers.Domain.FieldType.Time, FieldType.MultiLine

                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    oChecBox.Checked = value
                Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                    FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode
                    CBXsinglelineRevisionField.Checked = value

            End Select
        End Set
    End Property
    Private Property AllowRevisionCheck As Boolean Implements IViewRenderField.AllowRevisionCheck
        Get
            Return ViewStateOrDefault("AllowRevisionCheck", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowRevisionCheck") = value
        End Set
    End Property
    Private Property RevisionCount As Long Implements IViewRenderField.RevisionCount
        Get
            Return ViewStateOrDefault("RevisionCount", CLng(0))
        End Get
        Set(value As Long)
            ViewState("RevisionCount") = value
        End Set
    End Property
    Public Property ShowFieldChecked As Boolean Implements IViewRenderField.ShowFieldChecked
        Get
            Return ViewStateOrDefault("ShowFieldChecked", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowFieldChecked") = value
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

    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = FieldError.None, "", "error")
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            LBDisclaimerAccept.Text = .getValue("RBLdisclaimer.True")
            LBDisclaimerRefuse.Text = .getValue("RBLdisclaimer.")
        End With
    End Sub
#End Region
    Public Sub InitializeControl(item As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean, revisionCheck As Boolean) Implements IViewRenderField.InitializeControl
        CurrentError = FieldError.None
        AllowRevisionCheck = revisionCheck
        CurrentPresenter.InitView(item, disabled, isPublic)

        Dim css As String = "tagConteinerItem "
        'Tag
        If Not IsNothing(TagHelper) AndAlso TagHelper.HasValue Then
            Me.DVtagContainer.Attributes.Add("class", css + TagHelper.GetTagCssMultiString(item.Field.Tags))
        End If
    End Sub
    Public Sub InitializeControl(item As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean, err As FieldError, revisionCheck As Boolean) Implements IViewRenderField.InitializeControl
        CurrentError = err
        AllowRevisionCheck = revisionCheck
        CurrentPresenter.InitView(item, disabled, isPublic)

        If Not IsNothing(item) AndAlso Not IsNothing(item.Field) Then
            Dim oLabel As Label = FindControl("LBerrorMessage" & item.Field.Type.ToString.ToLower)

            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not (err = FieldError.None)

                If Not (err = FieldError.None) Then
                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
                End If
            End If


            Dim css As String = "tagConteinerItem "
            'Tag
            If Not IsNothing(TagHelper) AndAlso TagHelper.HasValue Then
                Me.DVtagContainer.Attributes.Add("class", css + TagHelper.GetTagCssMultiString(item.Field.Tags))
            End If
        End If
    End Sub
    Private Sub SetupView(item As dtoSubmissionValueField, isPublic As Boolean) Implements IViewRenderField.SetupView

        Dim css As String = "tagConteinerItem "
        'Tag
        If Not IsNothing(TagHelper) AndAlso TagHelper.HasValue Then
            Me.DVtagContainer.Attributes.Add("class", css + TagHelper.GetTagCssMultiString(item.Field.Tags))
        End If

        IdField = item.Field.Id
        RevisionCount = item.RevisionsCount
        FieldType = item.Field.Type

        'Per GetValue():
        IdSubmittedField = item.IdValueField
        Me.IdLink = 0
        Me.DisclaimerType = DisclaimerType.None
        Options = Nothing

        If (Not IsNothing(item.Value) AndAlso Not IsNothing(item.Value.Link)) Then
            Me.IdLink = item.Value.Link.Id
        End If

        If Not IsNothing(item.Field) Then
            Me.DisclaimerType = item.Field.DisclaimerType
            If Not IsNothing(item.Field.Options) Then
                Options = item.Field.Options
            End If
        End If

        Dim oLiteral As Literal = FindControl("LTmaxChars" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oLiteral) Then
            oLiteral.Text = Resource.getValue("MaxCharsInfo")
        End If

        Dim oGeneric As HtmlGenericControl = FindControl("SPNmaxChar" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oGeneric) AndAlso item.Field.MaxLength > 0 AndAlso (item.Field.Type <> FieldType.CheckboxList AndAlso item.Field.Type <> FieldType.RadioButtonList AndAlso item.Field.Type <> FieldType.DropDownList AndAlso item.Field.Type <> FieldType.Mail) Then
            oGeneric.Visible = True
        End If

        oGeneric = FindControl("DV" & item.Field.Type.ToString.ToLower)
        Dim oLabelDescription As Label = FindControl("LB" & item.Field.Type.ToString.ToLower & "Description")
        Dim oLabelText As Label = FindControl("LB" & item.Field.Type.ToString.ToLower & "Text")

        Dim oLabelTag = Nothing

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabelTag = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Tags")
        Else
            oLabelTag = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString & "Tags")
        End If

        If Not IsNothing(oLabelTag) Then
            SetTagLabel(item.Field.Tags, oLabelTag)
        End If



        Dim view As System.Web.UI.WebControls.View = FindControl("VIW" & item.Field.Type.ToString.ToLower)
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
                DDLitems.DataSource = item.Field.Options
                DDLitems.DataTextField = "Name"
                DDLitems.DataValueField = "Id"
                DDLitems.DataBind()
                If Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        DDLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                End If

                MinOptions = item.Field.MinOption
                MaxOptions = item.Field.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                RBLitems.DataSource = item.Field.Options
                RBLitems.DataTextField = "Name"
                RBLitems.DataValueField = "Id"
                RBLitems.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) Then
                    SPNtextOptionRadioButtonList.Visible = True
                    Dim oItem As ListItem = RBLitems.Items.FindByValue(opt.Id)
                    If Not IsNothing(oItem) Then
                        oItem.Attributes.Add("class", "extraoption")
                    End If
                    TXBradiobuttonlist.Text = item.Value.FreeText
                End If

                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        RBLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                End If
                MinOptions = item.Field.MinOption
                MaxOptions = item.Field.MaxOption

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer

                Dim dView As System.Web.UI.WebControls.View = FindControl("VIW" & item.Field.DisclaimerType.ToString)
                If Not IsNothing(dView) Then
                    MLVdisclaimer.SetActiveView(dView)
                End If
                Select Case item.Field.DisclaimerType
                    Case DisclaimerType.Standard
                        RBDisclaimerAccept.Checked = Not String.IsNullOrEmpty(item.Value.Text) AndAlso item.Value.Text.ToLower = "true"
                        RBDisclaimerRefuse.Checked = Not RBDisclaimerAccept.Checked
                    Case DisclaimerType.CustomSingleOption
                        RPTsingleOption.DataSource = GetDisplayOptions(item.Field.Options, item.Value.Text, True)
                        RPTsingleOption.DataBind()
                    Case DisclaimerType.CustomMultiOptions
                        RPTmultiOption.DataSource = GetDisplayOptions(item.Field.Options, item.Value.Text, False)
                        RPTmultiOption.DataBind()

                        SPNminMaxCustomMultiOptions.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                        If item.Field.MinOption > 0 AndAlso Not SPNdisclaimerCheckboxlist.Attributes("class").Contains("min-") Then
                            SPNdisclaimerCheckboxlist.Attributes("class") &= " min-" & item.Field.MinOption.ToString
                            Resource.setLiteral(LTminOptionsCustomMultiOptions)
                            LBminOptionCustomMultiOptions.Text = item.Field.MinOption.ToString
                        Else
                            LTminOptionsCustomMultiOptions.Visible = False
                            LBminOptionCustomMultiOptions.Visible = False
                        End If
                        If item.Field.MaxOption > 0 AndAlso Not SPNdisclaimerCheckboxlist.Attributes("class").Contains("max-") Then
                            SPNdisclaimerCheckboxlist.Attributes("class") &= " max-" & item.Field.MaxOption.ToString
                            Resource.setLiteral(LTmaxOptionsCustomMultiOptions)
                            LBmaxOptionCustomMultiOptions.Text = item.Field.MaxOption.ToString
                        Else
                            LTmaxOptionsCustomMultiOptions.Visible = False
                            LBmaxOptionCustomMultiOptions.Visible = False
                        End If
                End Select

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Dim toUpload As Boolean = IsNothing(item.Value.Link)
                CTRLdisplayItem.Visible = Not toUpload
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
                    'If Disabled Then
                    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                    'Else
                    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                    'End If

                    'initializer.Link = item.Value.Link
                    'CTRLdisplayFile.InsideOtherModule = True
                    'Dim actions As List(Of dtoModuleActionControl)
                    'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, initializer.Display)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                LBdateValue.CssClass = "readonlyinput"
                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value) Then
                    LBdateValue.Text = FormatDateTime(CDate(item.Value.Text), vbShortDate)
                ElseIf Not String.IsNullOrEmpty(item.Value.Text) Then
                    LBdateValue.Text = item.Value.Text
                    LBdateValue.CssClass &= " nodata"
                Else
                    LBdateValue.Text = ""
                    LBdateValue.CssClass &= " empty"
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                LBdatetimeValueData.CssClass = "readonlyinput"
                LBdatetimeValueHour.CssClass = "readonlyinput"
                LBdatetimeValueMinutes.CssClass = "readonlyinput"

                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    LBdatetimeValueData.Text = FormatDateTime(CDate(item.Value.Text), vbShortDate)
                    LBdatetimeValueHour.Text = CDate(item.Value.Text).Hour
                    LBdatetimeValueMinutes.Text = CDate(item.Value.Text).Minute
                Else
                    LBdatetimeValueData.Text = ""
                    LBdatetimeValueHour.Text = ""
                    LBdatetimeValueMinutes.Text = ""
                    LBdatetimeValueData.CssClass &= " empty"
                    LBdatetimeValueHour.CssClass &= " empty"
                    LBdatetimeValueMinutes.CssClass &= " empty"
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                LBtimeValueHour.CssClass = "readonlyinput"
                LBtimeValueMinutes.CssClass = "readonlyinput"
                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    LBtimeValueHour.Text = CDate(item.Value.Text).Hour
                    LBtimeValueMinutes.Text = CDate(item.Value.Text).Minute
                Else
                    LBtimeValueHour.CssClass &= " empty"
                    LBtimeValueMinutes.CssClass &= " empty"
                End If
            Case FieldType.MultiLine
                LBmultilineValue.CssClass = "readonlytextarea"
                LBmultilineValue.Text = item.Value.Text.Replace(Environment.NewLine, "<br />")
                If item.Field.MaxLength > 0 Then
                    'LBmultilineValue.Attributes.Add("maxlength", item.Field.MaxLength)
                    LTmultilineTotal.Text = item.Field.MaxLength
                    Dim used As Integer = item.Field.MaxLength
                    If Not IsNothing(item.Value) Then
                        used = item.Field.MaxLength - Len(item.Value.Text)
                    End If
                    LTmultilineUsed.Text = IIf(used < 0, 0, used)
                End If
                If IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                    LBmultilineValue.CssClass &= " empty"
                End If
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                If item.Field.MaxLength > 0 Then
                    Dim used As Integer = item.Field.MaxLength
                    If Not IsNothing(item.Value) Then
                        used -= Len(item.Value.Text)
                    End If
                    LTsinglelineUsed.Text = IIf(used < 0, 0, used)
                    LTsinglelineTotal.Text = item.Field.MaxLength
                End If
                LBsinglelineValue.Text = item.Value.Text

                SetTagLabel(item.Field.Tags, LBsinglelineTags)

                view = VIWsingleline
                LBsinglelineValue.CssClass = "readonlyinput"
                If IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                    LBsinglelineValue.CssClass &= " empty"
                End If
                oGeneric = DVsingleline
                oLabelDescription = LBsinglelineDescription
                oLabelText = LBsinglelineText
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                oLabelText.Text = item.Field.Name
                Exit Select
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableSimple

                Dim header As String() = New String() {""}
                Dim content As String = ""

                If Not IsNothing(item.Field) Then
                    If Not IsNothing(item.Field.TableFieldSetting) Then
                        header = item.Field.TableFieldSetting.Cols.Split("|")
                    End If

                    'LBtablesimpleText.Text = item.Field.Name
                    'LBtablesimpleDescription.Text = ""

                End If

                If Not IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                    content = item.Value.Text
                End If

                RenderTableContent(header, content)

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableReport
                Dim header As String() = New String() {""}
                Dim content As String = ""

                'Dim hedCols As String = String.Format("{0}|{1}|{2}|{3}", item.Field.TableFieldSetting.Cols, "Quantità", "Prezzo unitario", "Totale")

                Dim MaxTotal As Double = 0

                If Not IsNothing(item.Field) Then
                    If Not IsNothing(item.Field.TableFieldSetting) Then
                        header = item.Field.TableFieldSetting.Cols.Split("|")
                        MaxTotal = item.Field.TableFieldSetting.MaxTotal
                    End If

                End If


                If Not IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                    content = item.Value.Text
                End If

                RenderTableReportContent(header, content, MaxTotal)

                CurrentSummaryItem.Name = item.Field.Name
                CurrentSummaryItem.MaxTotal = item.Field.TableFieldSetting.MaxTotal
                'Case lm.Comol.Modules.CallForPapers.Domain.FieldType.TableReport

            Case Else

        End Select

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note AndAlso Not IsNothing(oLabelText) Then
            oLabelText.Text = IIf(item.Field.Mandatory, "(*)", "") & item.Field.Name
        End If
        If item.Field.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            LTdisclaimerDescription.Text = item.Field.Description
        ElseIf Not IsNothing(oLabelDescription) Then
            oLabelDescription.Text = item.Field.Description
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
        SetupForRevisions(item.Field.Type, item.RevisionsCount)
        If Not IsNothing(view) Then
            MLVfield.SetActiveView(view)
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyField() Implements IViewRenderField.DisplayEmptyField
        MLVfield.SetActiveView(VIWempty)
    End Sub

    Private Sub SetupForRevisions(type As FieldType, count As Long)
        Select Case type
            Case FieldType.CheckboxList, FieldType.DropDownList, _
                FieldType.RadioButtonList, FieldType.Disclaimer, _
                FieldType.FileInput, FieldType.Date, FieldType.DateTime, FieldType.Time, FieldType.MultiLine

                Dim oGeneric As HtmlGenericControl = FindControl("SPN" & FieldType.ToString & "RevisionField")
                If Not IsNothing(oGeneric) Then
                    oGeneric.Visible = (count > 0 OrElse AllowRevisionCheck OrElse ShowFieldChecked)
                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    Dim oLabel As Label = FindControl("LB" & FieldType.ToString & "RevisionField")
                    oLabel.Visible = Not AllowRevisionCheck AndAlso (count > 0)
                    oChecBox.Visible = AllowRevisionCheck OrElse ShowFieldChecked
                    oChecBox.Enabled = Not ShowFieldChecked

                    oLabel.Text = Resource.getValue("RevisionField.Label")
                    oChecBox.Text = Resource.getValue("RevisionField.Check")

                    If ShowFieldChecked Then
                        oChecBox.Checked = True
                    Else
                        oGeneric.Attributes.Add("class", LTrevisionedCssClass.Text)
                    End If
                End If

            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                SPNsinglelineRevisionField.Visible = (count > 0 OrElse AllowRevisionCheck OrElse ShowFieldChecked)
                LBsinglelineRevisionField.Visible = Not AllowRevisionCheck AndAlso (count > 0)
                CBXsinglelineRevisionField.Visible = AllowRevisionCheck OrElse ShowFieldChecked
                CBXsinglelineRevisionField.Enabled = Not ShowFieldChecked
                If ShowFieldChecked Then
                    CBXsinglelineRevisionField.Checked = True
                End If
                LBsinglelineRevisionField.Text = Resource.getValue("RevisionField.Label")
                CBXsinglelineRevisionField.Text = Resource.getValue("RevisionField.Check")
        End Select


    End Sub

    Public Sub HideSelection()
        AllowRevisionCheck = False
        SetupForRevisions(FieldType, RevisionCount)
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
    Private Sub RPTmultiOption_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmultiOption.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBoption")
        oCheck.Checked = item.Selected
        If item.IsFreeValue Then
            SPNtextOptionCheckBoxList.Visible = True
            oCheck.Attributes("class") = oCheck.Attributes("class") & " extraoption"
        End If

    End Sub

    Private Sub RPTsingleOption_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsingleOption.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oRadio As HtmlInputRadioButton = e.Item.FindControl("RBoption")
        oRadio.Checked = item.Selected
        If item.IsFreeValue Then
            SPNtextOptionRadioButtonList.Visible = True
            oRadio.Attributes("class") = oRadio.Attributes("class") & " extraoption"
        End If
    End Sub

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



    Public Function GetField() As dtoSubmissionValueField
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
                        'RBDisclaimerAccept
                        'dto.Value.Text = (Me.RBLdisclaimer.SelectedIndex = 0)
                        dto.Value.Text = (Me.RBDisclaimerAccept.Checked)

                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        'RPTmultiOption => CBoption

                        Dim count As Integer = RPTmultiOption.Items.Count

                        If count = 0 Then
                            dto.Value = New dtoValueField("")
                            'ElseIf count = 1 Then
                            '    Dim itm As RepeaterItem = RPTmultiOption.Items(0)

                            '    dto.Value = New dtoValueField()
                        Else

                            Dim val As String = ""
                            Dim isFirst As Boolean = True
                            For Each item As RepeaterItem In RPTmultiOption.Items

                                Dim opt As HtmlInputCheckBox = item.FindControl("CBoption")
                                If Not IsNothing(opt) AndAlso opt.Checked Then
                                    If (isFirst) Then
                                        val = opt.Value
                                    Else
                                        val = String.Format("{0}|{1}", val, opt.Value)
                                    End If
                                End If
                            Next
                            dto.Value = New dtoValueField(val)
                        End If


                        'Dim items As List(Of String) = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected Select i.Value).ToList
                        'If (items.Count = 0) Then
                        '    dto.Value = New dtoValueField("")
                        'ElseIf (items.Count = 1) Then
                        '    dto.Value = New dtoValueField(items(0))
                        'Else
                        '    dto.Value = New dtoValueField(String.Join("|", items.ToArray))
                        'End If


                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption

                        Dim count As Integer = RPTsingleOption.Items.Count

                        If count = 0 Then
                            dto.Value = New dtoValueField("")
                            'ElseIf count = 1 Then
                            '    Dim itm As RepeaterItem = RPTmultiOption.Items(0)

                            '    dto.Value = New dtoValueField()
                        Else

                            Dim val As String = ""
                            'Dim isFirst As Boolean = True
                            For Each item As RepeaterItem In RPTsingleOption.Items

                                Dim opt As HtmlInputCheckBox = item.FindControl("CBoption")
                                If Not IsNothing(opt) AndAlso opt.Checked Then
                                    val = opt.Value
                                    Exit For
                                End If
                            Next
                            dto.Value = New dtoValueField(val)
                        End If

                        'dto.Value = New dtoValueField(Me.RBLsingleOption.SelectedValue)
                    Case Else
                        dto.Value = New dtoValueField("")
                End Select


            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Exit Select


            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date

                dto.Value = New dtoValueField(Me.LBdatetimeValueData.Text)


                'If Me.RDPdate.SelectedDate.HasValue Then
                '    dto.Value = New dtoValueField(Me.RDPdate.SelectedDate.Value)
                'End If


            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                Dim time As String = Me.LBdatetimeValueData.Text & " " & LBdatetimeValueHour.Text & ":" & LBdatetimeValueMinutes.Text

                dto.Value = New dtoValueField(Me.LBdatetimeValueData.Text)
                'If Me.RDPdatetime.SelectedDate.HasValue Then
                '    dto.Value = New dtoValueField(Me.RDPdatetime.SelectedDate.Value)
                'End If


            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                Dim time As String = LBdatetimeValueHour.Text & ":" & LBdatetimeValueMinutes.Text

                'If Me.RDPtime.SelectedDate.HasValue Then
                '    dto.Value = New dtoValueField(Me.RDPtime.SelectedDate.Value)
                'End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                Exit Select

            Case FieldType.MultiLine
                dto.Value.Text = LBmultilineValue.Text

            Case FieldType.SingleLine, _
                FieldType.CompanyCode, FieldType.CompanyTaxCode, _
                FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, _
                FieldType.VatCode, FieldType.ZipCode, _
                lm.Comol.Modules.CallForPapers.Domain.FieldType.Mail

                'Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)
                'dto.Value.Text = oTextBox.Text

                dto.Value.Text = LBsinglelineValue.Text

            Case FieldType.TableSimple
                dto.Value.Text = LTtableSimpleContent.Text

            Case FieldType.TableReport
                dto.Value.Text = LTtablereportContent.Text.Replace("&nbsp;&euro;", "")

            Case Else
                dto.Value.Text = ""
        End Select
        dto.FieldError = FieldError.None 'GetFieldError()

        Return (dto)
    End Function


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

    'ToDo: SET!!!

    Private Property IdSubmittedField As Long
        Get
            Return ViewStateOrDefault("IdSubmittedField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmittedField") = value
        End Set
    End Property

    Private Property IdLink As Long
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property

    Private Property DisclaimerType As DisclaimerType
        Get
            Return ViewStateOrDefault("DisclaimerType", DisclaimerType.None)
        End Get
        Set(value As DisclaimerType)
            ViewState("DisclaimerType") = value
        End Set
    End Property

    Private Property Options As List(Of dtoFieldOption)
        Get
            Return ViewStateOrDefault("Options", New List(Of dtoFieldOption))
        End Get
        Set(value As List(Of dtoFieldOption))
            ViewState("Options") = value
        End Set
    End Property


    Private Sub RenderTableContent(ByVal Headers() As String, ByVal Content As String)

        Dim tblHd As String = ""

        If Not IsNothing(Headers) AndAlso (Headers.Any()) Then
            For Each hd As String In Headers
                tblHd = String.Format("{0}<th>{1}</th>", tblHd, hd)
            Next
        Else
            tblHd = "<th>&nbsp;</th>"
        End If

        Dim rows As String = ""
        If Not String.IsNullOrEmpty(Content) Then
            If (Content.Contains("<table>")) Then
                rows = Content.Replace("<table>", "")
            End If
            If (rows.Contains("</table>")) Then
                rows = rows.Replace("</table>", "")
            End If
        End If

        LTtableSimpleContent.Text = String.Format("<tr>{0}</tr>{1}", tblHd, rows).Replace(Environment.NewLine, "<br />")

    End Sub

    Private Sub RenderTableReportContent(ByVal Headers() As String, ByVal Content As String, ByVal maxTotal As Double)

        Dim tblHd As String = ""

        If Not IsNothing(Headers) AndAlso (Headers.Any()) Then
            For Each hd As String In Headers
                tblHd = String.Format("{0}<th>{1}</th>", tblHd, hd)
            Next
        Else
            tblHd = "<th>&nbsp;</th>"
        End If

        tblHd = String.Format("{0}<th>{1}</th><th>{2}</th><th>{3}</th>", tblHd, "Quantità", "Prezzo unitario", "Totale")

        'Tolgo il tag table, lo rimetto dopo


        'If Not String.IsNullOrEmpty(Content) Then
        '    If (Content.Contains("<table>")) Then
        '        rows = Content.Replace("<table>", "")
        '    End If
        '    If (rows.Contains("</table>")) Then
        '        rows = rows.Replace("</table>", "")
        '    End If
        'End If


        'Aggiungo l'intestazione e le colonne salvate
        'Dim table As String = String.Format("<tr>{0}</tr>{1}", tblHd, rows)

        Dim table As String = tblHd

        If Not String.IsNullOrEmpty(Content) Then
            'To Helper!!!

            Content = String.Format("<?xml version=""1.0"" encoding=""ISO-8859-1""?>{0}{1}", System.Environment.NewLine, Content)

            Dim doc As New XmlDocument
            doc.LoadXml(Content)
            'Dim trXml As XmlNodeList = doc.GetElementsByTagName("tr")

            For Each trXml As XmlNode In doc.GetElementsByTagName("tr")
                Dim innerRow As String = ""
                For Each tdXml As XmlNode In trXml.ChildNodes
                    Dim tdStr As String = ""
                    If (Not IsNothing(tdXml.Attributes) AndAlso Not IsNothing(tdXml.Attributes("class")) AndAlso Not String.IsNullOrEmpty(tdXml.Attributes("class").Value)) Then
                        If tdXml.Attributes("class").Value = "unitycost" Then
                            tdStr = String.Format("<td class=""unitycost"">{0}&nbsp;&euro;</td>", tdXml.InnerText)
                        ElseIf tdXml.Attributes("class").Value = "total" Then
                            tdStr = String.Format("<td class=""total"">{0}&nbsp;&euro;</td>", tdXml.InnerText)
                        Else
                            tdStr = String.Format("<td class={0}>{1}</td>", tdXml.Attributes("class").Value, tdXml.InnerText)
                        End If
                    Else
                        tdStr = String.Format("<td>{0}</td>", tdXml.InnerText)
                    End If

                    innerRow = String.Format("{0}{1}", innerRow, tdStr)

                Next
                table = String.Format("{0}<tr>{1}</tr>", table, innerRow)
            Next
        End If

        'Aggiunto table e la riga totale
        table = String.Format("<table class=""tableReport table table-striped"">{0}{1}</table>", table, TableReportGetTotalRow(Content, maxTotal))

        LTtablereportContent.Text = table.Replace(Environment.NewLine, "<br />")




    End Sub


    'Private Property TableSetting As lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField
    '    Get
    '        Return ViewStateOrDefault("CurrentTableSettings", New lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField)
    '    End Get
    '    Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoCallTableField)
    '        ViewState("CurrentTableSettings") = value
    '    End Set
    'End Property
    'Private Property IdSubmittedField As Long Implements IViewRenderField.IdSubmittedField
    '    Get
    '        Return ViewStateOrDefault("IdSubmittedField", CLng(0))
    '    End Get
    '    Set(value As Long)
    '        ViewState("IdSubmittedField") = value
    '    End Set
    'End Property

    'Private Property IdLink As Long Implements IViewRenderField.IdLink
    '    Get
    '        Return ViewStateOrDefault("IdLink", CLng(0))
    '    End Get
    '    Set(value As Long)
    '        ViewState("IdLink") = value
    '    End Set
    'End Property

    'Private Property DisclaimerType As DisclaimerType Implements IViewRenderField.DisclaimerType
    '    Get
    '        Return ViewStateOrDefault("DisclaimerType", DisclaimerType.None)
    '    End Get
    '    Set(value As DisclaimerType)
    '        ViewState("DisclaimerType") = value
    '    End Set
    'End Property

    'Private Property Options As List(Of dtoFieldOption) Implements IViewRenderField.Options
    '    Get
    '        Return ViewStateOrDefault("Options", New List(Of dtoFieldOption))
    '    End Get
    '    Set(value As List(Of dtoFieldOption))
    '        ViewState("Options") = value
    '    End Set
    'End Property

    Private Function TableReportGetTotalRow(ByVal content As String, ByVal maxTotal As Double) As String
        Dim row As String = ""

        Dim contentXml As String = String.Format("<?xml version=""1.0"" encoding=""ISO-8859-1""?>{0}{1}", System.Environment.NewLine, content)

        Dim doc As New XmlDocument
        doc.LoadXml(content)

        Dim total As Double = 0

        For Each tdXml As XmlNode In doc.GetElementsByTagName("td")
            If (Not IsNothing(tdXml.Attributes("class")) AndAlso tdXml.Attributes("class").Value = "total") Then
                Dim value As Double = 0
                Double.TryParse(tdXml.InnerText.ToString(), value)

                total += value
            End If
        Next

        Dim colcounts As Integer = doc.GetElementsByTagName("tr").Item(0).ChildNodes.Count()

        If (MaxTotal = 0) Then
            row = String.Format("<tr><td colspan=""{0}"">&nbsp;</td><td>{1}</td><td>{2}&nbsp;&euro;</td></tr>", (colcounts - 2), "Totale", total)
        Else
            row = String.Format("<tr><td colspan=""{0}"">&nbsp;</td><td>{1}</td><td>{2}&nbsp;&euro;&nbsp;/{3}&nbsp;&euro;</td></tr>",
                                (colcounts - 2),
                                "Totale",
                                total,
                                maxTotal)
        End If




        CurrentSummaryItem.Total = total

        Return row
    End Function







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
        LTsummaryTotal.Text = String.Format("{0} &euro;", reports.Total)
        'Else
        'LTsummaryTotal.Text = String.Format("{0} &euro; <span class=""totalHidden""> / {1} &euro;", reports.Total, reports.MaxTotal)
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
            'if(values.MaxTotal = )

        End If


    End Sub


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
    End Class
End Class