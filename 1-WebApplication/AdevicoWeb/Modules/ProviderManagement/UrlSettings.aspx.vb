Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Public Class UrlSettings
    Inherits PageBase
    Implements IViewUrlProviderSettings

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
    Private _Presenter As UrlProviderSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As UrlProviderSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UrlProviderSettingsPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProvider As Long Implements IViewUrlProviderSettings.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProvider As Long Implements IViewUrlProviderSettings.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CLng(Request.QueryString("IdProvider"))
            Else : Return CLng(0)
            End If
        End Get
    End Property
    Public Property AllowEdit As Boolean Implements IViewUrlProviderSettings.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowEdit") = value
            Me.HYPbackToProviderEdit.Visible = value
            HYPbackToProviderEdit.NavigateUrl = BaseUrl & RootObject.EditProvider(IdProvider)
            Me.BTNaddLoginFormat.Visible = value
            Me.UDPloginFormatList.Update()
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewUrlProviderSettings.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagementTop.Visible = value
            Me.HYPbackToManagementTop.NavigateUrl = BaseUrl & RootObject.Management(IdProvider)
        End Set
    End Property
    Public Property EncryptionInfo As lm.Comol.Core.Authentication.Helpers.EncryptionInfo Implements IViewUrlProviderSettings.EncryptionInfo
        Get
            Dim dto As New lm.Comol.Core.Authentication.Helpers.EncryptionInfo()

            dto.InitializationVector = Me.TXBinitializationVector.Text
            dto.Key = Me.TXBkey.Text
            dto.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgorithm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)
            Return dto
        End Get
        Set(value As lm.Comol.Core.Authentication.Helpers.EncryptionInfo)
            Me.TXBkey.Text = value.Key
            Me.TXBinitializationVector.Text = value.InitializationVector
            Me.DDLencryptionAlgorithm.SelectedValue = value.EncryptionAlgorithm.ToString
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property BackGroundItem(ByVal deleted As lm.Comol.Core.DomainModel.BaseStatusDeleted, itemType As ListItemType) As String
        Get
            If deleted = lm.Comol.Core.DomainModel.BaseStatusDeleted.None Then
                Return IIf(itemType = ListItemType.AlternatingItem, "ROW_Alternate_Small", "ROW_Normal_Small")
            Else
                Return "ROW_Disabilitate_Small"
            End If
        End Get
    End Property
    Public Function TranslateModalView(viewName As String) As String
        Return Resource.getValue(viewName)
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
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
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceUrlSettingsProviderTitle")
            Me.Master.ServiceNopermission = .getValue("serviceUrlSettingsProviderNopermission")

            .setButton(Me.BTNaddLoginFormat, True, , , True)
            .setHyperLink(Me.HYPbackToManagementTop, True, True)
            .setHyperLink(Me.HYPbackToProviderEdit, True, True)

            .setLiteral(UPPformatsLoading.FindControl("LTprogress"))
            .setButton(BTNvalidate, True, False, , True)
            .setLabel(LBencryptionAlgorithm_t)
            .setLabel(LBkey_t)
            .setLabel(LBinitializationVector_t)
            .setLabel(LBencryptedValue_t)
            .setLabel(LBdecrypted_t)
            .setLabel(LBdecryptedValue_t)
            .setLabel(LBcrypted_t)
            .setButton(BTNdecrypt, True, False, , True)
            .setButton(BTNcrypt, True, False, , True)

            For Each item As ListItem In DDLencryptionAlgorithm.Items
                item.Text = Resource.getValue("EncryptionAlgorithm." & item.Value)
            Next

            .setButton(BTNupdateConfigurationParameters, True, False, , True)
            .setButton(Me.BTNundoSaveTop, True, , , True)
            .setButton(Me.BTNsaveTop, True, , , True)
            .setButton(Me.BTNundoSaveBottom, True, , , True)
            .setButton(Me.BTNsaveBottom, True, , , True)

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub LoadProviderInfo(provider As dtoUrlProvider) Implements IViewUrlProviderSettings.LoadProviderInfo
        Me.EncryptionInfo = provider.EncryptionInfo
        Me.SPNdecrypted.Visible = False
        Me.SPNcrypted.Visible = False
        LoadLoginFormats(provider.LoginFormats)
    End Sub


#Region "Provider utilities"
    Private Sub BTNvalidate_Click(sender As Object, e As System.EventArgs) Handles BTNvalidate.Click
        If String.IsNullOrEmpty(Me.TXBkey.Text) Then
            Me.LBkeyValidated.Visible = False
        Else
            Me.LBkeyValidated.Visible = True
            Me.LBkeyValidated.Text = Resource.getValue("keyValidated." & CurrentPresenter.ValidateUrlProviderKey(Me.TXBkey.Text))
        End If
        If String.IsNullOrEmpty(Me.TXBinitializationVector.Text) Then
            Me.LBvectorValidated.Visible = False
        Else
            Me.LBvectorValidated.Visible = True
            Me.LBvectorValidated.Text = Resource.getValue("vectorValidated." & CurrentPresenter.ValidateUrlProviderInitializationVector(Me.TXBinitializationVector.Text))
        End If
    End Sub
    Private Sub BTNcrypt_Click(sender As Object, e As System.EventArgs) Handles BTNcrypt.Click
        Me.LBcryptedValue.Text = CurrentPresenter.CryptString(EncryptionInfo, Me.TXBdencryptedValue.Text)
        Me.SPNcrypted.Visible = Not String.IsNullOrEmpty(Me.LBcryptedValue.Text)
    End Sub
    Private Sub BTNdecrypt_Click(sender As Object, e As System.EventArgs) Handles BTNdecrypt.Click
        Me.LBdecryptedValue.Text = CurrentPresenter.DecryptString(EncryptionInfo, Me.TXBencryptedValue.Text)
        Me.SPNdecrypted.Visible = Not String.IsNullOrEmpty(Me.LBdecryptedValue.Text)
    End Sub
#End Region

#Region "Display message"
    Public Sub DisplayDeletedProvider(name As String, type As lm.Comol.Core.Authentication.AuthenticationProviderType) Implements lm.Comol.Core.BaseModules.ProviderManagement.Presentation.IViewUrlProviderSettings.DisplayDeletedProvider
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = String.Format(Me.Resource.getValue("DisplayDeletedProvider"), name, Me.Resource.getValue("AuthenticationProviderType." & type.ToString))
    End Sub

    Public Sub DisplayNoPermission() Implements IViewUrlProviderSettings.DisplayNoPermission
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayNoPermission")
    End Sub

    Public Sub DisplayProviderUnknown() Implements IViewUrlProviderSettings.DisplayProviderUnknown
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayProviderUnknown")
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewUrlProviderSettings.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditUrlProviderSettings(IIf(IdProvider > 0, IdProvider, PreloadedIdProvider))
        webPost.Redirect(dto)
    End Sub
#End Region

#Region "Load Format settings"
    Public Sub LoadLoginFormats(items As List(Of dtoLoginFormat)) Implements IViewUrlProviderSettings.LoadLoginFormats
        Me.RPTproviders.Visible = (items.Count > 0)
        If items.Count > 0 Then
            Me.RPTproviders.DataSource = items
            Me.RPTproviders.DataBind()
        End If
        Me.UDPloginFormatList.Update()
    End Sub
    Private Sub RPTproviders_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTproviders.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBactions_t")
            Me.Resource.setLabel(oLabel)
          
            oLabel = e.Item.FindControl("LBdefault_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBformatName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBformatBefore_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBformatAfter_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim loginFormat As dtoLoginFormat = DirectCast(e.Item.DataItem, dtoLoginFormat)
            Dim olinkButton As LinkButton = e.Item.FindControl("LNBedit")

            If loginFormat.isDefault Then
                olinkButton.Visible = True
                Resource.setLinkButton(olinkButton, True, True)

                olinkButton = e.Item.FindControl("LNBvirtualUnDelete")
                olinkButton.Visible = (loginFormat.Deleted <> BaseStatusDeleted.None)

            ElseIf loginFormat.Deleted = BaseStatusDeleted.None Then
                olinkButton.Visible = True
                Resource.setLinkButton(olinkButton, True, True)

                olinkButton = e.Item.FindControl("LNBvirtualDelete")
                olinkButton.Visible = True
                Resource.setLinkButton(olinkButton, True, True)
            Else
                olinkButton = e.Item.FindControl("LNBvirtualUnDelete")
                olinkButton.Visible = True
                Resource.setLinkButton(olinkButton, True, True)
                olinkButton = e.Item.FindControl("LNBdelete")
                olinkButton.Visible = True
                Resource.setLinkButton(olinkButton, True, True)
            End If

            Dim oLabel As Label = e.Item.FindControl("LBisDefault")
            oLabel.Text = Me.Resource.getValue("LBisDefault." & loginFormat.isDefault.ToString())
            oLabel.CssClass &= IIf(loginFormat.isDefault, "yes", "no")
        End If
    End Sub
    Protected Sub RPTproviders_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTproviders.ItemCommand
        Dim idFormat As Long = 0
        If IsNumeric(e.CommandArgument) Then
            idFormat = CLng(e.CommandArgument)
        End If

        Select Case e.CommandName
            Case "virtualDelete"
                CurrentPresenter.VirtualDeleteFormat(idFormat)
            Case "undelete"
                CurrentPresenter.VirtualUndeleteFormat(idFormat)
            Case "delete"
                CurrentPresenter.DeleteFormat(idFormat)
            Case "edit"
                OpenDialog("LoginFormat")
                Me.CTRLloginFormat.InitializeControl(Me.CurrentPresenter.GetLoginFormat(idFormat))
                Me.UDPloginFormatView.Update()

        End Select
    End Sub


#End Region

#Region "Dialog"
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenCloseDialog(ByVal closeId As String, ByVal openId As String)
        Dim script As String = String.Format("closeDialog('{0}');showDialog('{1}');", closeId, openId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
#End Region

#Region "Manage Format Setting"
    Private Sub BTNaddLoginFormat_Click(sender As Object, e As System.EventArgs) Handles BTNaddLoginFormat.Click
        Me.CTRLloginFormat.InitializeControl(New dtoLoginFormat())
        Me.OpenDialog("LoginFormat")
        Me.UDPloginFormatView.Update()
    End Sub
    Private Sub BTNsaveTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveTop.Click, BTNsaveBottom.Click
        Me.CloseDialog("LoginFormat")
        Me.CurrentPresenter.SaveLoginFormat(Me.CTRLloginFormat.CurrentLoginFormat)
        Me.UDPloginFormatView.Update()
    End Sub
    Private Sub BTNundoSaveTop_Click(sender As Object, e As System.EventArgs) Handles BTNundoSaveTop.Click, BTNundoSaveBottom.Click
        Me.CloseDialog("LoginFormat")
        Me.UDPloginFormatView.Update()
    End Sub
#End Region

    Public Sub GotoManagement() Implements IViewUrlProviderSettings.GotoManagement
        PageUtility.RedirectToUrl(RootObject.Management(IdProvider))
    End Sub

    Private Sub BTNupdateConfigurationParameters_Click(sender As Object, e As System.EventArgs) Handles BTNupdateConfigurationParameters.Click
        Dim dto As New lm.Comol.Core.Authentication.Helpers.EncryptionInfo
        dto.Key = Me.TXBkey.Text
        dto.InitializationVector = Me.TXBinitializationVector.Text
        dto.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgorithm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)

        Me.CurrentPresenter.UpdateEncryptionInfo(IdProvider, dto)
    End Sub
End Class