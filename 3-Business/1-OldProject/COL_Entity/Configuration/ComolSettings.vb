Imports Comol.Entity.File
Imports Comol.Entity.Configuration.Components
Imports System.Net.Mail

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class ComolSettings

#Region "Private properties"
        Private _EditorConfigurationPath As String
        Private _DefaultLanguage As Lingua
        Private _CodiceDB As Integer
        'Private _TutorSettings As TutorSettings
        Private _MailSettings As MailSettings
        Private _LoginSettings As LoginSettings
        Private _FileSettings As FileSettings
        Private _ExtensionSettings As ExtensionSettings
        Private _TagSettings As TagSettings
        Private _OnErrorShow404 As Boolean
        Private _LatexSettings As LatexSettings
        Private _PresenterSettings As PresenterSettings
        Private _BulkInsertSettings As BulkInsertSettings
        Private _DBconnectionSettings As DBconnectionSettings
        Private _SmartTags As SmartTags
        Private _QuizSettings As QuizSettings
        Private _IcodeonSettings As IcodeonSettings
        Private _BaseFileRepositoryPath As ConfigurationPath
        Private _ActionsSettings As ActionsSettings
        Private _NotificationService As NotificationSettings
        Private _ChatService As ChatSettings
        Private _Style As StyleSettings
        Private _NotificationErrorService As NotificationErrorSettings

        Private _TopBar As TopBarSetting
        Private _SkinSettings As SkinSettings

        Private _DocTemplateSettings As DocTemplateSettings

        Private _WebConferencingSettings As WebConferencingSettings

        Private _QuestionnaireSettings As QuestionnaireSettings

        Private _FederationSettings As String

        Private _Features As New FeaturesSettings
#End Region

#Region "Public properties"
        Public ReadOnly Property PlattformId As String
            Get
                If IsNothing(NotificationErrorService) Then
                    Return "PlattformId" & CodiceDB
                Else
                    Return NotificationErrorService.ComolUniqueID
                End If
            End Get
        End Property

        Public Property EditorConfigurationPath As String
            Get
                Return _EditorConfigurationPath
            End Get
            Set(ByVal value As String)
                _EditorConfigurationPath = value
            End Set
        End Property
        Public Property Style() As StyleSettings
            Get
                Return _Style
            End Get
            Set(ByVal value As StyleSettings)
                _Style = value
            End Set
        End Property
        Public Property ChatService() As ChatSettings
            Get
                Return _ChatService
            End Get
            Set(ByVal value As ChatSettings)
                _ChatService = value
            End Set
        End Property

        Public Property DefaultLanguage() As Lingua
            Get
                Return _DefaultLanguage
            End Get
            Set(ByVal value As Lingua)
                _DefaultLanguage = value
            End Set
        End Property
        Public Property CodiceDB() As Integer
            Get
                CodiceDB = _CodiceDB
            End Get
            Set(ByVal value As Integer)
                _CodiceDB = value
            End Set
        End Property
        'Public Property Tutor() As TutorSettings
        '	Get
        '		Tutor = _TutorSettings
        '	End Get
        '	Set(ByVal value As TutorSettings)
        '		_TutorSettings = value
        '	End Set
        'End Property
        Public Property Mail() As MailSettings
            Get
                Mail = _MailSettings
            End Get
            Set(ByVal value As MailSettings)
                _MailSettings = value
            End Set
        End Property
        Public Property Login() As LoginSettings
            Get
                Login = _LoginSettings
            End Get
            Set(ByVal value As LoginSettings)
                _LoginSettings = value
            End Set
        End Property
        Public Property File() As FileSettings
            Get
                File = _FileSettings
            End Get
            Set(ByVal value As FileSettings)
                _FileSettings = value
            End Set
        End Property
        Public Property Extension() As ExtensionSettings
            Get
                Extension = _ExtensionSettings
            End Get
            Set(ByVal value As ExtensionSettings)
                _ExtensionSettings = value
            End Set
        End Property
        Public Property OnErrorShow404() As Boolean
            Get
                OnErrorShow404 = _OnErrorShow404
            End Get
            Set(ByVal value As Boolean)
                _OnErrorShow404 = value
            End Set
        End Property
        Public Property Tag() As TagSettings
            Get
                Tag = _TagSettings
            End Get
            Set(ByVal value As TagSettings)
                _TagSettings = value
            End Set
        End Property
        Public Property Latex() As LatexSettings
            Get
                Latex = _LatexSettings
            End Get
            Set(ByVal value As LatexSettings)
                _LatexSettings = value
            End Set
        End Property
        Public Property Presenter() As PresenterSettings
            Get
                Presenter = _PresenterSettings
            End Get
            Set(ByVal value As PresenterSettings)
                _PresenterSettings = value
            End Set
        End Property
        Public Property BulkInsert() As BulkInsertSettings
            Get
                BulkInsert = _BulkInsertSettings
            End Get
            Set(ByVal value As BulkInsertSettings)
                _BulkInsertSettings = value
            End Set
        End Property
        Public Property DBconnectionSettings() As DBconnectionSettings
            Get
                Return _DBconnectionSettings
            End Get
            Set(ByVal value As DBconnectionSettings)
                _DBconnectionSettings = value
            End Set
        End Property
        Public Property Quiz() As QuizSettings
            Get
                Return Me._QuizSettings
            End Get
            Set(ByVal value As QuizSettings)
                _QuizSettings = value
            End Set
        End Property
        Public Property Icodeon() As IcodeonSettings
            Get
                Return Me._IcodeonSettings
            End Get
            Set(ByVal value As IcodeonSettings)
                _IcodeonSettings = value
            End Set
        End Property
        Public Property BaseFileRepositoryPath() As ConfigurationPath
            Get
                Return Me._BaseFileRepositoryPath
            End Get
            Set(ByVal value As ConfigurationPath)
                _BaseFileRepositoryPath = value
            End Set
        End Property
        Public Property ActionService() As ActionsSettings
            Get
                Return _ActionsSettings
            End Get
            Set(ByVal value As ActionsSettings)
                _ActionsSettings = value
            End Set
        End Property
        Public Property NotificationService() As NotificationSettings
            Get
                Return _NotificationService
            End Get
            Set(ByVal value As NotificationSettings)
                _NotificationService = value
            End Set
        End Property

        Public Property NotificationErrorService() As NotificationErrorSettings
            Get
                Return _NotificationErrorService
            End Get
            Set(ByVal value As NotificationErrorSettings)
                _NotificationErrorService = value
            End Set
        End Property

        Public Property TopBar As TopBarSetting
            Get
                Return _TopBar
            End Get
            Set(ByVal value As TopBarSetting)
                _TopBar = value
            End Set
        End Property

        Public Property SkinSettings As SkinSettings
            Get
                Return _SkinSettings
            End Get
            Set(ByVal value As SkinSettings)
                _SkinSettings = value
            End Set
        End Property

        Public Property DocTemplateSettings As DocTemplateSettings
            Get
                Return _DocTemplateSettings
            End Get
            Set(ByVal value As DocTemplateSettings)
                _DocTemplateSettings = value
            End Set
        End Property

        Public Property WebConferencingSettings As WebConferencingSettings
            Get
                Return _WebConferencingSettings
            End Get
            Set(ByVal value As WebConferencingSettings)
                _WebConferencingSettings = value
            End Set
        End Property

        Public Property QuestionnaireSettings() As QuestionnaireSettings
            Get
                If IsNothing(_QuestionnaireSettings) Then
                    _QuestionnaireSettings = New QuestionnaireSettings()
                End If

                Return _QuestionnaireSettings
            End Get
            Set(value As QuestionnaireSettings)
                _QuestionnaireSettings = value
            End Set
        End Property

        Public Property FederationSettings() As String
            Get
                Return _FederationSettings
            End Get
            Set(value As String)
                _FederationSettings = value
            End Set
        End Property

        Public Property Features As FeaturesSettings
            Get
                Return _Features
            End Get
            Set(value As FeaturesSettings)
                _Features = value
            End Set
        End Property
#End Region
        Public Sub New()

        End Sub
    End Class
End Namespace