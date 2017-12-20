Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_GlossarySettings
    Inherits GLbaseControl
    Implements IViewUC_GlossarySettings

    Private _Presenter As UC_GlossarySettingsPresenter

    Private ReadOnly Property CurrentPresenter() As UC_GlossarySettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_GlossarySettingsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property


    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBpagedList_t)
            .setLabel(LBthreeColumn_t)
            .setLabel(LBdefaultGlossary_t)
            .setLabel(LBpageSize)
            .setLiteral(LTdefaultview_t)
            .setLabel(LBstatus_t)
            .setLabel(LBlanguage_t)
            .setLinkButton(LNBsaveGlossary, False, True)
            .setLiteral(LTbasicGlossarySettings)
            .setLiteral(LTglossaryName_t)
            .setLiteral(LTglossaryDescription_t)
            .setLiteral(LTmessageheaderCss)
            .setLiteral(LTmarkedFields_t)
            .setLiteral(LTareMandatory_t)
            .setLinkButton(LNBback, False, True)
            SWHpublishGlossary.SetText(Resource, True, True)
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Property ItemData As DTO_Glossary Implements IViewUC_GlossarySettings.ItemData
        Get
            Dim Data As DTO_Glossary = New DTO_Glossary()
            Data.Id = IdGlossary
            Data.Name = TXBname.Text
            Data.Description = TXBdescription.Text
            Data.IsPublished = SWHpublishGlossary.Status
            Data.IsDefault = CBXisDefault.Checked
            Data.IdCommunity = IdCommunity

            Dim pageSize As Int32 = 10
            Dim pageSizeStr As String = TXBpageSize.Text.Trim()
            Int32.TryParse(pageSizeStr, pageSize)
            Data.TermsPerPage = pageSize

            Data.IdLanguage = RBLanguages.SelectedValue
            Data.TermsArePaged = RBpagedList.Checked
            Return Data
        End Get
        Set(value As DTO_Glossary)
            TXBname.Text = value.Name
            TXBdescription.Text = value.Description
            SWHpublishGlossary.Status = value.IsPublished
            CBXisDefault.Checked = value.IsDefault
            RBLanguages.SelectedValue = value.IdLanguage
            RBpagedList.Checked = value.TermsArePaged
            RBthreeColumn.Checked = Not value.TermsArePaged

            If value.TermsPerPage Then
                TXBpageSize.Text = value.TermsPerPage
            Else
                TXBpageSize.Text = 10
            End If
        End Set
    End Property

    Public Sub SetTitle(ByVal name As String) Implements IViewUC_GlossarySettings.SetTitle
    End Sub

    Public Sub BindDati(ByVal idCommunity As Int32, ByVal idGlossary As Int64, ByVal fromAdd As Boolean)
        Me.IdGlossary = idGlossary
        If fromAdd Then
            ShowErrors(SaveStateEnum.Saved, MessageType.success)
        End If
        CurrentPresenter.InitView()
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal languages As List(Of BaseLanguageItem)) Implements IViewUC_GlossarySettings.LoadViewData
        SWHpublishGlossary.Enabled = True
        SWHpublishGlossary.SetText(Resource, True, True)
        RBLanguages.Items.Clear()
        RBLanguages.Items.Add(New ListItem("MULTI", -1))
        For Each item As BaseLanguageItem In languages.OrderBy(Function(f) f.Id)
            Dim itemList As New ListItem(Resource.getValue(item.Code), item.Id)
            itemList.Attributes.Add("class", "item")
            RBLanguages.Items.Add(itemList)
        Next
        ItemData = glossary
    End Sub

    Public Sub GoToGlossaryList() Implements IViewUC_GlossarySettings.GoToGlossaryList
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryList(IdCommunity))
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewUC_GlossarySettings.ShowErrors
        Dim errors As New StringBuilder()
        For Each key As String In resourceErrorList
            errors.AppendLine(Resource.getValue(key))
        Next
        SetErrorMessage(errors.ToString(), type)
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewUC_GlossarySettings.ShowErrors
        SetErrorMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetErrorMessage(ByVal message As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, type)
        End If
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBsaveGlossary.Click
        Dim value = ItemData
        SetErrorMessage(String.Empty, MessageType.none)
        If (CurrentPresenter.ValidateFields(value)) Then
            CurrentPresenter.SaveOrUpdate(value)
        Else
            SetErrorMessage(Resource.getValue("NameIsMandatory"), MessageType.error)
        End If
    End Sub

    Private Sub LNBback_Click(sender As Object, e As EventArgs) Handles LNBback.Click
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryList(IdCommunity))
    End Sub

    Public Sub GoToGlossaryEdit(ByVal idGlossary As Long) Implements IViewUC_GlossarySettings.GoToGlossaryEdit
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryEdit(idGlossary, IdCommunity, False, True))
    End Sub

    'Public Function IsPagedListViewActive() As String
    '    Dim result = "inputgroup"
    '    If ItemData.TermsArePaged Then
    '        result = "inputgroup active"
    '    End If
    '    Return result
    'End Function

    'Public Function IsThreeColumnViewActive() As String
    '    Dim result = "inputgroup"
    '    If Not ItemData.TermsArePaged Then
    '        result = "inputgroup active"
    '    End If
    '    Return result
    'End Function
End Class