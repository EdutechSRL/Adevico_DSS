Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class EditPassword
    Inherits PageBase
    Implements IViewEditPassword


#Region "Context"
    Private _Presenter As EditPasswordPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As EditPasswordPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditPasswordPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBunknownUser)
            .setLabel(LBpasswordConfirm_t)
            .setLabel(LBpasswordNew_t)
            .setLabel(LBpasswordOld_t)
            .setButton(BTNback, True, , , True)
            .setButton(BTNundoSavePassword, True, , , True)
            .setButton(BTNsavePassword, True)
            .setLabel(LBmessage)
            .setLabel(LBunknownUser)
            .setCompareValidator(CMVpassword)
            Me.Master.ServiceTitle = .getValue("serviceTitleEditProfilePassword")
        End With
    End Sub

#Region "Not used"
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#End Region

#Region "Implements"
    Public Sub DisplayProviders() Implements IViewEditPassword.DisplayProviders
        Me.MLVaccountInfo.SetActiveView(VIWinfo)
    End Sub
    Public Sub DisplayEditPassword() Implements IViewEditPassword.DisplayEditPassword
        Me.MLVaccountInfo.SetActiveView(VIWinternal)
        Me.TXBnewPassword.Text = ""
        Me.TXBconfirmPassword.Text = ""
        Me.TXBoldPpassword.Text = ""
    End Sub

    Public Sub DisplayPasswordNotChanged() Implements IViewEditPassword.DisplayPasswordNotChanged
        Me.MLVaccountInfo.SetActiveView(VIWmessage)
        LBmessage.Text = Resource.getValue("PasswordNotChanged")
    End Sub
    Public Sub DisplayInvalidPassword() Implements IViewEditPassword.DisplayInvalidPassword
        Me.MLVaccountInfo.SetActiveView(VIWmessage)
        LBmessage.Text = Resource.getValue("InvalidPassword")
    End Sub
    Public Sub DisplayProviders(providers As List(Of dtoUserProvider)) Implements IViewEditPassword.DisplayProviders
        Me.MLVaccountInfo.SetActiveView(VIWinfo)
        Me.RPTaccountInfo.DataSource = providers
        Me.RPTaccountInfo.DataBind()
    End Sub

    Public Sub LoadUserUnknown() Implements IViewEditPassword.LoadUserUnknown
        Me.MLVaccountInfo.SetActiveView(VIWunknownUser)
    End Sub
#End Region

    Private Sub RPTaccountInfo_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTaccountInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoUserProvider = DirectCast(e.Item.DataItem, dtoUserProvider)

            Dim oLabel As Label = e.Item.FindControl("LBproviderName")

            oLabel.Text = dto.Translation.Name
            oLabel = e.Item.FindControl("LBproviderDescription")
            oLabel.Text = dto.Translation.Description

            oLabel = e.Item.FindControl("LBproviderInfo")
            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPchangePassword")
            Dim oButton As Button = e.Item.FindControl("BTNchangePassword")

            Resource.setHyperLink(oHyperlink, True, True)
            Resource.setButton(oButton, True, , , True)
            oHyperlink.Visible = False
            oButton.Visible = False

            If TypeOf dto Is dtoInternalUserProvider Then
                Dim internal As dtoInternalUserProvider = DirectCast(dto, dtoInternalUserProvider)

                oButton.Visible = True

                oLabel.Visible = (internal.ResetType <> EditType.none)

                If internal.ResetType <> EditType.none AndAlso internal.ModifiedOn.HasValue Then
                    Resource.setLabel(oLabel)
                    oLabel.Text = String.Format(oLabel.Text, internal.ModifiedOn.Value.ToString("dd/MM/yy"), internal.ModifiedOn.Value.ToString("HH:mm"), Resource.getValue("EditType." & internal.ResetType.ToString), internal.ModifiedBy.SurnameAndName)
                End If

                oLabel = e.Item.FindControl("LBpasswordExpiresOn")

                oLabel.Visible = internal.PasswordExpiresOn.HasValue
                If internal.PasswordExpiresOn.HasValue Then

                    Resource.setLabel(oLabel)
                    oLabel.Text = String.Format(oLabel.Text, internal.PasswordExpiresOn.Value.ToString("dd/MM/yy"), internal.PasswordExpiresOn.Value.ToString("HH:mm"))
                End If

            ElseIf TypeOf dto Is dtoExternalUserProvider Then
                Dim external As dtoExternalUserProvider = DirectCast(dto, dtoExternalUserProvider)
                oHyperlink.Visible = Not String.IsNullOrEmpty(external.RemoteUrl)
                oHyperlink.NavigateUrl = external.RemoteUrl
            End If
        End If
    End Sub

    Private Sub RPTaccountInfo_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTaccountInfo.ItemCommand
        If e.CommandName = "edit" Then
            Me.DisplayEditPassword()
           
        End If
    End Sub
    Private Sub BTNsavePassword_Click(sender As Object, e As System.EventArgs) Handles BTNsavePassword.Click
        Me.CurrentPresenter.EditPassword(Me.TXBoldPpassword.Text, Me.TXBnewPassword.Text)
    End Sub

    Private Sub BTNundoSavePassword_Click(sender As Object, e As System.EventArgs) Handles BTNundoSavePassword.Click
        Me.DisplayProviders()
    End Sub

    Private Sub BTNback_Click(sender As Object, e As System.EventArgs) Handles BTNback.Click
        Me.DisplayEditPassword()
    End Sub
End Class
