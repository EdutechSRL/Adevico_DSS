Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication
Public Class UC_AuthenticationProvider
    Inherits BaseControl
    Implements IViewProviderData

    Public Event UpdateIdentifierFields(ByVal fields As IdentifierField)

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
    Private _Presenter As ProviderDataPresenter
    Private ReadOnly Property CurrentPresenter() As ProviderDataPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProviderDataPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProvider As Long Implements IViewProviderData.IdProvider
        Get
            Return Me.ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdProvider") = value
        End Set
    End Property
    Public Property ProviderType As AuthenticationProviderType Implements IViewProviderData.ProviderType
        Get
            Return Me.ViewStateOrDefault("ProviderType", AuthenticationProviderType.None)
        End Get
        Set(value As AuthenticationProviderType)
            Me.ViewState("ProviderType") = value
        End Set
    End Property
    Public Property CurrentProvider As dtoProvider Implements IViewProviderData.Current
        Get
            Dim dtoProvider As dtoProvider
            Dim l As LogoutMode = LogoutMode.none
            If Me.DDLlogoutMode.SelectedIndex > -1 Then
                l = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of LogoutMode).GetByString(Me.DDLlogoutMode.SelectedValue, LogoutMode.none)
            End If
            Select Case ProviderType
                Case AuthenticationProviderType.Internal
                    dtoProvider = New dtoInternalProvider
                    dtoProvider.LogoutMode = IIf(l = LogoutMode.none, LogoutMode.internalLogonPage, l)
                Case AuthenticationProviderType.Url
                    dtoProvider = New dtoUrlProvider
                    dtoProvider.LogoutMode = IIf(l = LogoutMode.none, LogoutMode.externalPage, l)
                Case AuthenticationProviderType.UrlMacProvider
                    dtoProvider = New dtoMacUrlProvider
                    dtoProvider.LogoutMode = IIf(l = LogoutMode.none, LogoutMode.logoutMessage, l)
                Case Else
                    dtoProvider = New dtoProvider
            End Select
            dtoProvider.IdProvider = IdProvider
            dtoProvider.Type = Me.ProviderType

            With dtoProvider
                .Name = Me.TXBname.Text
                .UniqueCode = Me.TXBuniqueCode.Text
                .DisplayToUser = Me.CBXdisplayToUser.Checked
                .AllowAdminProfileInsert = Me.CBXallowAdminProfileInsert.Checked
                .AllowMultipleInsert = Me.CBXallowMultipleInsert.Checked
                .isEnabled = Me.CBXenabled.Checked
                Dim list As List(Of ListItem) = (From i As ListItem In Me.CBLidentifierFields.Items Where i.Selected Select i).ToList
                If list.Count = 0 Then
                    .IdentifierFields = IdentifierField.none
                ElseIf list.Count = 1 Then
                    .IdentifierFields = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IdentifierField).GetByString(list(0).Value, IdentifierField.none)
                Else
                    .IdentifierFields = (IdentifierField.longField Or IdentifierField.stringField)
                End If
                If SPNmultipleValues.Visible Then
                    .MultipleItemsForRecord = CBXseparator.Checked
                    .MultipleItemsSeparator = TXBseparator.Text
                End If
            End With

            Select Case dtoProvider.Type
                Case AuthenticationProviderType.Internal
                    Dim dtoInternalProvider As dtoInternalProvider = DirectCast(dtoProvider, dtoInternalProvider)
                    dtoInternalProvider.ChangePasswordAfterDays = 30 * Me.DDLpasswordDays.SelectedValue
                    dtoProvider = dtoInternalProvider
                Case AuthenticationProviderType.Url
                    Dim urlProvider As dtoUrlProvider = DirectCast(dtoProvider, dtoUrlProvider)
                    With urlProvider
                        .SenderUrl = Me.TXBsenderUrl.Text
                        .VerifyRemoteUrl = Me.CBXverifyRemoteUrl.Checked
                        .RemoteLoginUrl = Me.TXBremoteLoginUrl.Text
                        .UrlIdentifier = Me.TXBurlIdentifier.Text
                        If String.IsNullOrEmpty(Me.TXBdeltaTime.Text) Then
                            .DeltaTime = New TimeSpan(0, 0, 5)
                        Else
                            .DeltaTime = New TimeSpan(0, 0, CInt(Me.TXBdeltaTime.Text))
                        End If
                        .TokenFormat = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of UrlUserTokenFormat).GetByString(DDLtokenFormat.SelectedValue, UrlUserTokenFormat.LoginDateTime)
                        .EncryptionInfo = New lm.Comol.Core.Authentication.Helpers.EncryptionInfo
                        .EncryptionInfo.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgoritm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)
                        If .EncryptionInfo.EncryptionAlgorithm <> lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.None Then
                            .EncryptionInfo.Key = Me.TXBkey.Text
                            .EncryptionInfo.InitializationVector = Me.TXBinitializationVector.Text
                        End If
                    End With
                    dtoProvider = urlProvider
                Case AuthenticationProviderType.UrlMacProvider
                    Dim macProvider As dtoMacUrlProvider = DirectCast(dtoProvider, dtoMacUrlProvider)
                    With macProvider
                        .SenderUrl = Me.TXBsenderUrl.Text
                        .VerifyRemoteUrl = Me.CBXverifyRemoteUrl.Checked
                        .RemoteLoginUrl = Me.TXBremoteLoginUrl.Text
                        If String.IsNullOrEmpty(Me.TXBdeltaTime.Text) Then
                            .DeltaTime = New TimeSpan(0, 0, 5)
                        Else
                            .DeltaTime = New TimeSpan(0, 0, CInt(Me.TXBdeltaTime.Text))
                        End If
                        .EncryptionInfo = New lm.Comol.Core.Authentication.Helpers.EncryptionInfo
                        .EncryptionInfo.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgoritm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)
                        If .EncryptionInfo.EncryptionAlgorithm = lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.None Then
                            .EncryptionInfo.Key = ""
                            .EncryptionInfo.InitializationVector = ""
                        ElseIf .EncryptionInfo.EncryptionAlgorithm = lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5 Then
                            .EncryptionInfo.Key = Me.TXBprivatekey.Text
                        Else
                            .EncryptionInfo.Key = Me.TXBkey.Text
                            .EncryptionInfo.InitializationVector = Me.TXBinitializationVector.Text
                        End If
                        .AllowTaxCodeDuplication = CBXallowTaxCodeDuplication.Checked
                        .AutoEnroll = CBXautoEnroll.Checked
                        .AutoAddAgency = CBXautoAddAgency.Checked
                        .AllowRequestFromIpAddresses = TXBallowRequestFromIpAddresses.Text
                        .DenyRequestFromIpAddresses = TXBdenyRequestFromIpAddresses.Text
                    End With
                    dtoProvider = macProvider
            End Select
            Return dtoProvider
        End Get
        Set(value As dtoProvider)
            IdProvider = value.IdProvider
            ProviderType = value.Type
            Me.LBproviderType.Text = Resource.getValue("AuthenticationProviderType." & value.Type.ToString)
            If ProviderType = AuthenticationProviderType.Shibboleth AndAlso lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(value.IdentifierFields, IdentifierField.stringField) Then
                Me.SPNmultipleValues.Visible = True
                Me.CBXseparator.Checked = value.MultipleItemsForRecord
                Me.TXBseparator.Text = value.MultipleItemsSeparator
            Else
                Me.SPNmultipleValues.Visible = False
                Me.CBXseparator.Checked = False
                Me.TXBseparator.Text = ""
            End If
            Me.CBXenabled.Checked = value.isEnabled
            Me.CBXenabled.Enabled = Not (value.Type = AuthenticationProviderType.UrlMacProvider AndAlso value.IdProvider = 0)
            Select Case value.Type
                Case AuthenticationProviderType.Internal
                    Dim days = DirectCast(value, dtoInternalProvider).ChangePasswordAfterDays
                    If days = 0 Then
                        Me.DDLpasswordDays.SelectedValue = days
                    Else
                        Dim month As Integer = (days / 30)
                        If IsNothing(Me.DDLpasswordDays.Items.FindByValue(month)) Then
                            month = 6
                        End If
                        Me.DDLpasswordDays.SelectedValue = month
                    End If
                    Me.MLVtypes.SetActiveView(VIWinternalProvider)
                Case AuthenticationProviderType.Url
                    Dim urlProvider As dtoUrlProvider = DirectCast(value, dtoUrlProvider)
                    With urlProvider
                        Me.TXBsenderUrl.Text = .SenderUrl
                        Me.CBXverifyRemoteUrl.Checked = .VerifyRemoteUrl
                        Me.TXBremoteLoginUrl.Text = .RemoteLoginUrl
                        Me.TXBurlIdentifier.Text = .UrlIdentifier
                        Me.TXBdeltaTime.Text = .DeltaTime.TotalSeconds
                        Try
                            Me.DDLtokenFormat.SelectedValue = .TokenFormat.ToString
                        Catch ex As Exception

                        End Try
                        If IsNothing(.EncryptionInfo) Then
                            Me.TXBkey.Text = ""
                            Me.TXBinitializationVector.Text = ""
                            Me.DDLencryptionAlgoritm.SelectedValue = lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael.ToString
                        Else
                            Me.TXBkey.Text = .EncryptionInfo.Key
                            Me.TXBinitializationVector.Text = .EncryptionInfo.InitializationVector
                            Me.DDLencryptionAlgoritm.SelectedValue = .EncryptionInfo.EncryptionAlgorithm.ToString
                        End If

                        Select Case .EncryptionInfo.EncryptionAlgorithm
                            Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5
                                Me.RFVprivateKey.Enabled = True
                                MLVencryptionKeys.SetActiveView(VIWprivatekey)
                            Case Else
                                Me.RFVinitializationVector.Enabled = True
                                Me.RFVkey.Enabled = True
                                MLVencryptionKeys.SetActiveView(VIWkeys)
                        End Select
                    End With
                    SPNurlIdentifier.Visible = True
                    RFVidentifier.Enabled = True
                    Me.MLVtypes.SetActiveView(VIWgenericUrlProvider)
                    Me.MLVurlProvider.SetActiveView(VIWurlProvider)
                Case AuthenticationProviderType.UrlMacProvider

                    Dim macProvider As dtoMacUrlProvider = DirectCast(value, dtoMacUrlProvider)
                    With macProvider
                        Me.TXBsenderUrl.Text = .SenderUrl
                        Me.CBXverifyRemoteUrl.Checked = .VerifyRemoteUrl
                        Me.TXBremoteLoginUrl.Text = .RemoteLoginUrl
                        Me.TXBdeltaTime.Text = .DeltaTime.TotalSeconds

                        If IsNothing(.EncryptionInfo) Then
                            Me.TXBprivatekey.Text = ""
                            Me.DDLencryptionAlgoritm.SelectedValue = lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5.ToString
                        Else
                            If .EncryptionInfo.EncryptionAlgorithm = lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5 Then
                                Me.TXBprivatekey.Text = .EncryptionInfo.Key
                            Else
                                Me.TXBkey.Text = .EncryptionInfo.Key
                                Me.TXBinitializationVector.Text = .EncryptionInfo.InitializationVector
                            End If

                            Me.DDLencryptionAlgoritm.SelectedValue = .EncryptionInfo.EncryptionAlgorithm.ToString
                        End If
                        Me.CBXautoAddAgency.Checked = .AutoAddAgency
                        Me.CBXautoEnroll.Checked = .AutoEnroll
                        CBXallowTaxCodeDuplication.Checked = .AllowTaxCodeDuplication
                        TXBallowRequestFromIpAddresses.Text = .AllowRequestFromIpAddresses
                        TXBdenyRequestFromIpAddresses.Text = .DenyRequestFromIpAddresses
                        If .IdProvider = 0 Then
                            Dim aAttribute As New lm.Comol.Core.Authentication.ApplicationAttribute()
                            aAttribute.Name = Resource.getValue("ApplicationAttribute.Name")
                            aAttribute.Description = Resource.getValue("ApplicationAttribute.Description")
                            aAttribute.QueryStringName = "ApplicationID"
                            aAttribute.Value = String.Format(Resource.getValue("ApplicationAttribute.Value"), IdProvider.ToString)
                            .Attributes.Add(aAttribute)

                            Dim fAttribute As New lm.Comol.Core.Authentication.FunctionAttribute()
                            fAttribute.Name = Resource.getValue("FunctionAttribute.Name")
                            fAttribute.Description = Resource.getValue("FunctionAttribute.Description")
                            fAttribute.QueryStringName = Resource.getValue("FunctionAttribute.QueryStringName")
                            fAttribute.Value = String.Format(Resource.getValue("FunctionAttribute.Value"), IdProvider.ToString)
                            .Attributes.Add(fAttribute)

                            Dim tAttribute As New lm.Comol.Core.Authentication.TimestampAttribute()
                            tAttribute.Name = Resource.getValue("TimestampAttribute.Name")
                            tAttribute.Description = Resource.getValue("TimestampAttribute.Description")
                            tAttribute.QueryStringName = Resource.getValue("TimestampAttribute.QueryStringName")
                            tAttribute.Format = lm.Comol.Core.Authentication.TimestampFormat.aaaammgghhmmss
                            .Attributes.Add(tAttribute)
                        End If
                        Select Case .EncryptionInfo.EncryptionAlgorithm
                            Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.None
                                MLVencryptionKeys.SetActiveView(VIWnoKeys)
                            Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5
                                Me.RFVprivateKey.Enabled = True
                                MLVencryptionKeys.SetActiveView(VIWprivatekey)
                            Case Else
                                Me.RFVinitializationVector.Enabled = True
                                Me.RFVkey.Enabled = True
                                MLVencryptionKeys.SetActiveView(VIWkeys)
                        End Select
                    End With

                    Me.MLVtypes.SetActiveView(VIWgenericUrlProvider)
                    Me.MLVurlProvider.SetActiveView(VIWmacUrlProvider)
                Case Else
                    Me.MLVtypes.SetActiveView(VIWbase)
            End Select
            With value
                Me.TXBname.Text = .Name
                Me.TXBuniqueCode.Text = .UniqueCode
                Me.CBXdisplayToUser.Checked = .DisplayToUser
                Me.CBXallowAdminProfileInsert.Checked = .AllowAdminProfileInsert
                Me.CBXallowMultipleInsert.Checked = .AllowMultipleInsert

                Me.CBLidentifierFields.SelectedIndex = -1
                Dim listItem As ListItem
                If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, .IdentifierFields) Then
                    listItem = Me.CBLidentifierFields.Items.FindByValue(IdentifierField.longField.ToString)
                    If Not IsNothing(listItem) Then
                        listItem.Selected = True
                    End If
                End If
                If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, .IdentifierFields) Then
                    listItem = Me.CBLidentifierFields.Items.FindByValue(IdentifierField.stringField.ToString)
                    If Not IsNothing(listItem) Then
                        listItem.Selected = True
                    End If
                End If
            End With
            Me.CBXallowMultipleInsert.Enabled = (value.Type <> AuthenticationProviderType.Internal)

            Me.SPNlogoutMode.Visible = value.GetAvailableLogoutMode.Any()
            If value.GetAvailableLogoutMode.Any() Then
                Dim items As List(Of TranslatedItem(Of String)) = (From s In value.GetAvailableLogoutMode Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("LogoutMode." & s.ToString)}).ToList

                Me.DDLlogoutMode.DataSource = items.OrderBy(Function(t) t.Translation).ToList
                Me.DDLlogoutMode.DataValueField = "Id"
                Me.DDLlogoutMode.DataTextField = "Translation"
                Me.DDLlogoutMode.DataBind()
                If Me.DDLlogoutMode.Items.Count > 0 Then
                    If value.GetAvailableLogoutMode.Contains(value.LogoutMode) Then
                        Me.DDLlogoutMode.SelectedValue = value.LogoutMode.ToString
                    Else
                        Me.DDLlogoutMode.SelectedIndex = 0
                    End If
                End If
            End If
        End Set
    End Property
    Public Property isInitialized As Boolean
        Get
            Return Me.ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("isInitialized") = value
        End Set
    End Property

    Public Property IdentifierFields As IdentifierField Implements IViewProviderData.IdentifierFields
        Get
            Dim fields As IdentifierField
            Dim list As List(Of ListItem) = (From i As ListItem In Me.CBLidentifierFields.Items Where i.Selected Select i).ToList
            If list.Count = 0 Then
                fields = IdentifierField.none
            ElseIf list.Count = 1 Then
                fields = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IdentifierField).GetByString(list(0).Value, IdentifierField.none)
            Else
                fields = (IdentifierField.longField Or IdentifierField.stringField)
            End If
            Return fields
        End Get
        Set(value As IdentifierField)
            Me.CBLidentifierFields.SelectedIndex = -1
            Dim listItem As ListItem
            If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, value) Then
                listItem = Me.CBLidentifierFields.Items.FindByValue(IdentifierField.longField.ToString)
                If Not IsNothing(listItem) Then
                    listItem.Selected = True
                End If
            End If
            If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, value) Then
                listItem = Me.CBLidentifierFields.Items.FindByValue(IdentifierField.stringField.ToString)
                If Not IsNothing(listItem) Then
                    listItem.Selected = True
                End If
            End If
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Property UpdateContainer As Boolean
        Get
            Return ViewStateOrDefault("UpdateContainer", False)
        End Get
        Set(value As Boolean)
            Me.CBLidentifierFields.AutoPostBack = value
            Me.ViewState("UpdateContainer") = value
        End Set
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBproviderName_t)
            .setLabel(LBuniqueCode_t)
            .setLabel(LBproviderType_t)
            .setHyperLink(HYPadvancedSettings, True, True)
            .setLabel(LBdisplayToUser_t)
            .setCheckBox(CBXdisplayToUser)
            .setLabel(LBallowAdminProfileInsert_t)
            .setCheckBox(CBXallowAdminProfileInsert)
            .setLabel(LBallowMultipleInsert_t)
            .setCheckBox(CBXallowMultipleInsert)
            .setLabel(LBidentifierFields_t)

            .setCheckBoxList(CBLidentifierFields, "IdentifierField." & IdentifierField.longField.ToString())
            .setCheckBoxList(CBLidentifierFields, "IdentifierField." & IdentifierField.stringField.ToString())
            .setLabel(LBdateToChangePassword_t)
            .setDropDownList(DDLpasswordDays, "3")
            .setDropDownList(DDLpasswordDays, "6")
            .setDropDownList(DDLpasswordDays, "9")
            .setDropDownList(DDLpasswordDays, "12")
            .setDropDownList(DDLpasswordDays, "24")
            .setDropDownList(DDLpasswordDays, "36")
            .setDropDownList(DDLpasswordDays, "0")
            .setLabel(LBsenderUrl_t)
            .setLabel(LBverifyRemoteUrl_t)
            .setCheckBox(CBXverifyRemoteUrl)
            .setLabel(LBremoteLoginUrl_t)
            .setLabel(LBurlIdentifier_t)
            .setLabel(LBdeltaTime_t)
            .setLabel(LBdeltaTime)
            .setLabel(LBtokenFormat_t)

            For Each item As ListItem In DDLtokenFormat.Items
                item.Text = Resource.getValue("UrlUserTokenFormat." & item.Value)
            Next

            .setLabel(LBkey_t)
            .setLabel(LBinitializationVector_t)
            .setLabel(LBencryptionAlgorithm_t)
            .setLabel(LBduplicateUniqueCode)
            For Each item As ListItem In Me.DDLencryptionAlgoritm.Items
                item.Text = Resource.getValue("EncryptionAlgorithm." & item.Value)
            Next

            .setButton(BTnvalidateKey, True, , , True)
            .setButton(BTNvalidateVector, True, , , True)
            .setLabel(LBmultipleValue_t)
            LBmultipleValue_t.ToolTip = Resource.getValue("LBmultipleValue_t.Tooltip")
            .setCheckBox(CBXseparator)

            .setLabel(LBenabledProvider_t)
            .setLabel(LBprivateKey_t)
            .setLabel(LBautoEnroll_t)
            .setLabel(LBautoAddAgency_t)
            .setLabel(LBallowTaxCodeDuplication)
            .setCheckBox(CBXautoEnroll)
            .setCheckBox(CBXautoAddAgency)
            .setCheckBox(CBXenabled)
            .setCheckBox(CBXallowTaxCodeDuplication)
            .setLabel(LBlogoutMode_t)
            .setLabel(LBdenyRequestFromIpAddresses_t)
            .setLabel(LBallowRequestFromIpAddresses_t)
            LBallowRequestFromIpAddressesInfo.Text = .getValue("FormatiIP")
            LBdenyRequestFromIpAddressesInfo.Text = .getValue("FormatiIP")
            .setLabel(LBnotifySubscriptionTo_t)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(provider As dtoProvider, allowAdvancedSettings As Boolean) Implements IViewProviderData.InitializeControl
        Me.isInitialized = True
        LBduplicateUniqueCode.Visible = False
        Me.LoadProvider(provider, allowAdvancedSettings AndAlso provider.Type <> AuthenticationProviderType.Internal)
    End Sub

    Public Sub LoadProvider(provider As dtoProvider, showAdvancedSettings As Boolean) Implements IViewProviderData.LoadProvider
        Me.MLVcontrolData.SetActiveView(VIWdata)
        Me.HYPadvancedSettings.Visible = showAdvancedSettings
        Me.HYPadvancedSettings.NavigateUrl = BaseUrl & RootObject.EditProviderSettings(provider.IdProvider, provider.Type)

        MLVencryptionKeys.ActiveViewIndex = 0
        Me.RFVinitializationVector.Enabled = False
        Me.RFVkey.Enabled = False
        Me.RFVprivateKey.Enabled = False
        RFVidentifier.Enabled = False
        Me.CurrentProvider = provider
       
    End Sub
    Public Sub DisplayDuplicateCode() Implements IViewProviderData.DisplayDuplicateCode
        LBduplicateUniqueCode.Visible = True
    End Sub
    Public Sub DisplayProviderUnknown() Implements IViewProviderData.DisplayProviderUnknown
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub

    Public Function SaveData() As Boolean Implements IViewProviderData.SaveData
        If ValidateContent() Then
            LBduplicateUniqueCode.Visible = False
            Return Me.CurrentPresenter.SaveProvider()
        Else
            Return False
        End If
    End Function

    Public Function ValidateContent() As Boolean Implements IViewProviderData.ValidateContent
        Select Case ProviderType
            Case AuthenticationProviderType.Url
                RFVidentifier.Enabled = True
            Case Else
                RFVidentifier.Enabled = False
        End Select

        Me.RFVinitializationVector.Enabled = False
        Me.RFVkey.Enabled = False
        Me.RFVprivateKey.Enabled = False
        Select ProviderType
            Case AuthenticationProviderType.Url, AuthenticationProviderType.UrlMacProvider
                Dim alg As lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgoritm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)
                Select Case alg
                    Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5
                        Me.RFVprivateKey.Enabled = True
                    Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.None
                        Exit Select
                    Case Else
                        Me.RFVinitializationVector.Enabled = True
                        Me.RFVkey.Enabled = True
                End Select
            Case Else
              
        End Select
        Return Me.Page.IsValid
    End Function

    Private Sub CBLidentifierFields_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles CBLidentifierFields.SelectedIndexChanged
        If ProviderType = AuthenticationProviderType.Shibboleth AndAlso lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierFields, IdentifierField.stringField) Then
            Me.SPNmultipleValues.Visible = True
        Else
            Me.SPNmultipleValues.Visible = False
        End If
        If UpdateContainer Then
            RaiseEvent UpdateIdentifierFields(IdentifierFields)
        End If
    End Sub

    Private Sub BTNvalidateKey_Click(sender As Object, e As System.EventArgs) Handles BTNvalidateKey.Click
        If String.IsNullOrEmpty(Me.TXBkey.Text) Then
            Me.LBkeyValidated.Visible = False
        Else
            Me.LBkeyValidated.Visible = True
            Me.LBkeyValidated.Text = Resource.getValue("keyValidated." & CurrentPresenter.ValidateUrlProviderKey(Me.TXBkey.Text))
        End If
    End Sub

    Private Sub BTNvalidateVector_Click(sender As Object, e As System.EventArgs) Handles BTNvalidateVector.Click
        If String.IsNullOrEmpty(Me.TXBinitializationVector.Text) Then
            Me.LBvectorValidated.Visible = False
        Else
            Me.LBvectorValidated.Visible = True
            Me.LBvectorValidated.Text = Resource.getValue("vectorValidated." & CurrentPresenter.ValidateUrlProviderInitializationVector(Me.TXBinitializationVector.Text))
        End If
    End Sub

    Private Sub DDLencryptionAlgoritm_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLencryptionAlgoritm.SelectedIndexChanged
        Select Case Me.DDLencryptionAlgoritm.SelectedValue
            Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.None.ToString
                MLVencryptionKeys.SetActiveView(VIWnoKeys)
            Case lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Md5.ToString
                MLVencryptionKeys.SetActiveView(VIWprivatekey)
            Case Else
                MLVencryptionKeys.SetActiveView(VIWkeys)
        End Select
    End Sub
End Class