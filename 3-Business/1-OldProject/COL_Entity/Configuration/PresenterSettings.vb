Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class PresenterSettings

#Region "Private properties"
        Private _isNew As Boolean
        Private _RepositoryConfiguration As New RepositoryConfiguration
        Private _DefaultCommunityModule As String
        Private _DefaultStartPage As String
        Private _DefaultDisplayNameMode As Integer
        Private _FullDefaultStartPage As String
        Private _DefaultTitle As String
        Private _DefaultRemoveServiceNotification As String
        Private _DefaultManagement As String
        Private _EditorItemHandlerPath As String
        Private _AjaxTimer As Integer
        Private _DefaultNation As Integer
        Private _DefaultProvince As Integer
        Private _DefaultNoProvinceID As Integer
        Private _DefaultHomeHeaderLink As String
        Private _DefaultSubscriptionsLink As String
        Private _DefaultLogonPage As String
        Private _DefaultTaxCodeRequired As Boolean
        Private _DefaultHighSchoolTypeID As Integer
        Private _DefaultProfileTypesToActivate As List(Of Integer)
        Private _DefaultSplitMailRecipients As Integer
        Private _DefaultMatricolaRequired As Boolean
        Private _DefaultBirthDateRequired As Boolean
        Private _ScormEnabled As Boolean
        Private _DefaultScormDownload As Boolean
        Private _DefaultAllowScormDownload As Boolean
        Private _DefaultFileCategoryID As Integer
        Private _DefaultFolderCategoryID As Integer
        Private _EnabledPortalHomeModal As Boolean
        Private _EnabledLogonAs As Boolean
        Private _AllowUserRegistration As Boolean
        Private _EnabledAdministrativeLogonAs As Boolean
#End Region

#Region "Public properties"
        Public AllowSaveAndContinueQuestionnaire As Boolean
        Public AllowSaveBetweenPageQuestionnaire As Boolean
        Public AllowOverWriteQuestionnaireSavingPolicy As Boolean
        Public AllowDssUse As Boolean


        Public Property PortalDisplay As New PortalDisplayInfo()
        Public Property AllowUserRegistration() As Boolean
            Get
                AllowUserRegistration = _AllowUserRegistration
            End Get
            Set(ByVal value As Boolean)
                _AllowUserRegistration = value
            End Set
        End Property
        Public Property isNew() As Boolean
            Get
                isNew = _isNew
            End Get
            Set(ByVal value As Boolean)
                _isNew = value
            End Set
        End Property
        Public Property DefaultCommunityModule() As String
            Get
                DefaultCommunityModule = _DefaultCommunityModule
            End Get
            Set(ByVal value As String)
                _DefaultCommunityModule = value
            End Set
        End Property
        Public Property EnabledPortalHomeModal() As Boolean
            Get
                EnabledPortalHomeModal = _EnabledPortalHomeModal
            End Get
            Set(ByVal value As Boolean)
                _EnabledPortalHomeModal = value
            End Set
        End Property
        Public Property EnabledLogonAs() As Boolean
            Get
                EnabledLogonAs = _EnabledLogonAs
            End Get
            Set(ByVal value As Boolean)
                _EnabledLogonAs = value
            End Set
        End Property
        Public Property EnabledAdministrativeLogonAs() As Boolean
            Get
                EnabledAdministrativeLogonAs = _EnabledAdministrativeLogonAs
            End Get
            Set(ByVal value As Boolean)
                _EnabledAdministrativeLogonAs = value
            End Set
        End Property
        Public Property DefaultStartPage() As String
            Get
                DefaultStartPage = _DefaultStartPage
            End Get
            Set(ByVal value As String)
                _DefaultStartPage = value
            End Set
        End Property
        Public Property DefaultDisplayNameMode() As Integer
            Get
                DefaultDisplayNameMode = _DefaultDisplayNameMode
            End Get
            Set(ByVal value As Integer)
                _DefaultDisplayNameMode = value
            End Set
        End Property


        Public Property FullDefaultStartPage() As String
            Get
                FullDefaultStartPage = _FullDefaultStartPage
            End Get
            Set(ByVal value As String)
                _FullDefaultStartPage = value
            End Set
        End Property
        Public Property DefaultTitle() As String
            Get
                DefaultTitle = _DefaultTitle
            End Get
            Set(ByVal value As String)
                _DefaultTitle = value
            End Set
        End Property
        Public Property DefaultRemoveServiceNotification() As String
            Get
                DefaultRemoveServiceNotification = _DefaultRemoveServiceNotification
            End Get
            Set(ByVal value As String)
                _DefaultRemoveServiceNotification = value
            End Set
        End Property

        Public Property DefaultManagement() As String
            Get
                DefaultManagement = _DefaultManagement
            End Get
            Set(ByVal value As String)
                _DefaultManagement = value
            End Set
        End Property


        Public Property EditorItemHandlerPath() As String
            Get
                EditorItemHandlerPath = _EditorItemHandlerPath
            End Get
            Set(ByVal value As String)
                _EditorItemHandlerPath = value
            End Set
        End Property
        Public Property AjaxTimer() As Integer
            Get
                AjaxTimer = _AjaxTimer
            End Get
            Set(ByVal value As Integer)
                _AjaxTimer = value
            End Set
        End Property

        Public Property DefaultNationID() As Integer
            Get
                DefaultNationID = _DefaultNation
            End Get
            Set(ByVal value As Integer)
                _DefaultNation = value
            End Set
        End Property
        Public Property DefaultProvinceID() As Integer
            Get
                DefaultProvinceID = _DefaultProvince
            End Get
            Set(ByVal value As Integer)
                _DefaultProvince = value
            End Set
        End Property
        Public Property DefaultNoProvinceID() As Integer
            Get
                DefaultNoProvinceID = _DefaultNoProvinceID
            End Get
            Set(ByVal value As Integer)
                _DefaultNoProvinceID = value
            End Set
        End Property
        Public Property DefaultHomeHeaderLink() As String
            Get
                DefaultHomeHeaderLink = _DefaultHomeHeaderLink
            End Get
            Set(ByVal value As String)
                _DefaultHomeHeaderLink = value
            End Set
        End Property
        Public Property DefaultSubscriptionsLink() As String
            Get
                DefaultSubscriptionsLink = _DefaultSubscriptionsLink
            End Get
            Set(ByVal value As String)
                _DefaultSubscriptionsLink = value
            End Set
        End Property
        Public Property DefaultLogonPage() As String
            Get
                DefaultLogonPage = _DefaultLogonPage
            End Get
            Set(ByVal value As String)
                _DefaultLogonPage = value
            End Set
        End Property


        Public Property DefaultTaxCodeRequired() As Boolean
            Get
                DefaultTaxCodeRequired = _DefaultTaxCodeRequired
            End Get
            Set(ByVal value As Boolean)
                _DefaultTaxCodeRequired = value
            End Set
        End Property
        Public Property DefaultHighSchoolTypeID() As Integer
            Get
                DefaultHighSchoolTypeID = _DefaultHighSchoolTypeID
            End Get
            Set(ByVal value As Integer)
                _DefaultHighSchoolTypeID = value
            End Set
        End Property

        Public Property DefaultProfileTypesToActivate() As List(Of Integer)
            Get
                DefaultProfileTypesToActivate = _DefaultProfileTypesToActivate
            End Get
            Set(ByVal value As List(Of Integer))
                _DefaultProfileTypesToActivate = value
            End Set
        End Property

        Public Property DefaultSplitMailRecipients() As Integer
            Get
                DefaultSplitMailRecipients = _DefaultSplitMailRecipients
            End Get
            Set(ByVal value As Integer)
                _DefaultSplitMailRecipients = value
            End Set
        End Property
        Public Property DefaultMatricolaRequired() As Boolean
            Get
                DefaultMatricolaRequired = _DefaultMatricolaRequired
            End Get
            Set(ByVal value As Boolean)
                _DefaultMatricolaRequired = value
            End Set
        End Property

        Public Property DefaultBirthDateRequired() As Boolean
            Get
                DefaultBirthDateRequired = _DefaultBirthDateRequired
            End Get
            Set(ByVal value As Boolean)
                _DefaultBirthDateRequired = value
            End Set
        End Property

        'Public Property DefaultScormEnabled() As Boolean
        '    Get
        '        DefaultScormEnabled = _ScormEnabled
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _ScormEnabled = value
        '    End Set
        'End Property
        'Public Property DefaultScormDownload() As Boolean
        '    Get
        '        DefaultScormDownload = _DefaultScormDownload
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _DefaultScormDownload = value
        '    End Set
        'End Property
        'Public Property DefaultAllowScormDownload() As Boolean
        '    Get
        '        DefaultAllowScormDownload = _DefaultAllowScormDownload
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _DefaultAllowScormDownload = value
        '    End Set
        'End Property

        Public Property RepositoryConfiguration() As RepositoryConfiguration
            Get
                RepositoryConfiguration = _RepositoryConfiguration
            End Get
            Set(ByVal value As RepositoryConfiguration)
                _RepositoryConfiguration = value
            End Set
        End Property
        Public Property DefaultFileCategoryID() As Integer
            Get
                DefaultFileCategoryID = _DefaultFileCategoryID
            End Get
            Set(ByVal value As Integer)
                _DefaultFileCategoryID = value
            End Set
        End Property
        Public Property DefaultFolderCategoryID() As Integer
            Get
                DefaultFolderCategoryID = _DefaultFolderCategoryID
            End Get
            Set(ByVal value As Integer)
                _DefaultFolderCategoryID = value
            End Set
        End Property
#End Region


        Private Sub SetNullValue()
            _isNew = False
            _DefaultHomeHeaderLink = ""
            _DefaultLogonPage = ""
            _DefaultStartPage = ""
            _DefaultTitle = ""
            _DefaultRemoveServiceNotification = ""
            _AjaxTimer = 20000
            _DefaultNation = 193
            _DefaultProvince = 92
            _DefaultNoProvinceID = 0
            _DefaultManagement = ""
            _DefaultTaxCodeRequired = True
            _DefaultProfileTypesToActivate = New List(Of Integer)
            _DefaultSplitMailRecipients = 100
            _DefaultBirthDateRequired = False
            _EnabledPortalHomeModal = False
            _EnabledLogonAs = False
            _AllowUserRegistration = True
            AllowSaveAndContinueQuestionnaire = True
            AllowSaveBetweenPageQuestionnaire = True
            AllowOverWriteQuestionnaireSavingPolicy = True
        End Sub
        Public Sub New()
            Me.SetNullValue()
        End Sub


    End Class

    <Serializable(), CLSCompliant(True)> Public Class RepositoryConfiguration
        Public AvailableItemType As New List(Of Integer)
        Public AllowDownload As New List(Of Integer)
        Public DefaultDownload As New List(Of Integer)
        Public UrlConfigurations As New List(Of RepositoryUrlConfiguration)
    End Class

    <Serializable(), CLSCompliant(True)> Public Class RepositoryUrlConfiguration
        Public RepositoryItemType As Integer
        Public PlayerUrl As String
        Public RedirectToFilePage As Boolean
    End Class

    <Serializable(), CLSCompliant(True)> Public Class PortalDisplayInfo
        Public Name As New PortalTranslation()
        Public Home As New PortalTranslation()
        Public IstanceName As New PortalTranslation()

        Public Function LocalizeName(idLanguage As Integer) As String
            If Name.LocalNames.ContainsKey(idLanguage) Then
                Return Name.Localize(idLanguage)
            Else
                Return ""
            End If
        End Function
        Public Function LocalizeHome(idLanguage As Integer) As String
            If Home.LocalNames.ContainsKey(idLanguage) Then
                Return Home.Localize(idLanguage)
            Else
                Return ""
            End If
        End Function
        Public Function LocalizeIstanceName(idLanguage As Integer) As String
            If IstanceName.LocalNames.ContainsKey(idLanguage) Then
                Return IstanceName.Localize(idLanguage)
            Else
                Return ""
            End If
        End Function
    End Class

    <Serializable(), CLSCompliant(True)> Public Class PortalTranslation
        Private _LocalNames As IDictionary(Of Integer, String)
        Public Property LocalNames As IDictionary(Of Integer, String)
            Get
                Return _LocalNames
            End Get
            Set(ByVal value As IDictionary(Of Integer, String))
                _LocalNames = value
            End Set
        End Property
        Public Sub New()
            LocalNames = New Dictionary(Of Integer, String)
        End Sub
        Public Function Localize(idLanguage As Integer) As String
            If (LocalNames.ContainsKey(idLanguage)) Then
                Return LocalNames(idLanguage)
            Else
                If LocalNames.ContainsKey(-1) Then
                    Return LocalNames(-1)
                Else
                    Return ""
                End If
            End If
        End Function
    End Class
End Namespace