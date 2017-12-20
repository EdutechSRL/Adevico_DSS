Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_AuthenticationProviderTranslation
    Inherits BaseControl
    Implements IViewTranslationData

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
    Private _Presenter As TranslationDataPresenter
    Private ReadOnly Property CurrentPresenter() As TranslationDataPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TranslationDataPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property idLanguage As Integer Implements IViewTranslationData.idLanguage
        Get
            Return ViewStateOrDefault("idLanguage", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("idLanguage") = value
        End Set
    End Property
    Public Property IdProvider As Long Implements IViewTranslationData.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Public Property IdTranslation As Long Implements IViewTranslationData.IdTranslation
        Get
            Return ViewStateOrDefault("IdTranslation", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdTranslation") = value
        End Set
    End Property
    Public ReadOnly Property Translation As dtoProviderTranslation Implements IViewTranslationData.Translation
        Get
            Dim dto As New dtoProviderTranslation
            With dto
                .IdAuthenticationProvider = IdProvider
                .idLanguage = idLanguage
                .Id = IdTranslation
                .Name = Me.TXBname.Text
                .Description = Me.TXBdescription.Text
                .ForSubscribeName = Me.TXBforSubscribeName.Text
                .ForSubscribeDescription = Me.TXBforSubscribeDescription.Text
                .FieldLong = Me.TXBfieldLong.Text
                .FieldString = Me.TXBfieldString.Text
            End With
            Return dto
        End Get
    End Property
    Public Property isInitialized As Boolean Implements IViewTranslationData.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
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
            .setLabel(LBtranslationName_t)
            .setLabel(LBtranslationDescription_t)
            .setLabel(LBtranslationForSubscribeName_t)
            .setLabel(LBforSubscribeDescription_t)
            .setLabel(LBfieldLong_t)
            .setLabel(LBfieldString_t)
        End With
    End Sub

#End Region

    Public Sub InitializeControl(idProvider As Long, idLanguage As Integer, fields As IdentifierField) Implements IViewTranslationData.InitializeControl
        Me.isInitialized = True
        Me.MLVcontrolData.SetActiveView(VIWdata)
        Me.CurrentPresenter.InitView(idProvider, idLanguage)
        Me.SPNfieldLong.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, fields)
        Me.SPNfieldString.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, fields)
    End Sub
    Public Sub LoadTranslation(translation As dtoProviderTranslation) Implements IViewTranslationData.LoadTranslation
        With translation
            IdProvider = .IdAuthenticationProvider
            idLanguage = .idLanguage
            IdTranslation = .Id
            Me.TXBname.Text = .Name
            Me.TXBdescription.Text = .Description
            Me.TXBforSubscribeName.Text = .ForSubscribeName
            Me.TXBforSubscribeDescription.Text = .ForSubscribeDescription
            Me.TXBfieldLong.Text = .FieldLong
            Me.TXBfieldString.Text = .FieldString
        End With
    End Sub

    Public Function SaveData() As Boolean Implements IViewTranslationData.SaveData
        If ValidateContent() Then
            Me.CurrentPresenter.SaveTranslation()
        Else
            Return False
        End If
    End Function

    Public Function ValidateContent() As Boolean Implements IViewTranslationData.ValidateContent
        Page.Validate()
        Return Page.IsValid
    End Function

    Public Sub DisplayProviderUnknown() Implements IViewTranslationData.DisplayProviderUnknown
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub

   
    Public Sub UpdateTranslationView(fields As IdentifierField) Implements IViewTranslationData.UpdateTranslationView
        Me.SPNfieldLong.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, fields)
        Me.SPNfieldString.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, fields)
        If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.longField, fields) Then
            Me.TXBfieldLong.Text = ""
        End If
        If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(IdentifierField.stringField, fields) Then
            Me.TXBfieldString.Text = ""
        End If
    End Sub
End Class