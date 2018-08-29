Imports System.Xml
Imports Comol.Entity.Configuration
Imports Comol.Entity.Configuration.Components

Namespace Comol.Manager
	Public Class ManagerConfigurationSettings
		Inherits ObjectBase

		' Percorso fisico dell'applicazione (web o vB che sia)
		Private _ApplicationPath As String
		' Percorso fisico di base dei file di configurazione (web o vB che sia)
		Private _ConfigurationPath As String
		Private _LanguageSettingsFile As String
		Private _LanguageSettingsPath As String
		Private _ConfigurationFilePath As String
		Private _ConfigurationSmartTags As String

		Public ReadOnly Property ConfigurationFilePath() As String
			Get
				ConfigurationFilePath = _ConfigurationFilePath
			End Get
		End Property
		Public ReadOnly Property LanguageSettingsFile() As String
			Get
				LanguageSettingsFile = _LanguageSettingsFile
			End Get
		End Property
		Public ReadOnly Property LanguageSettingsPath() As String
			Get
				LanguageSettingsPath = _LanguageSettingsPath
			End Get
		End Property
		Public ReadOnly Property ConfigurationSmartTags() As String
			Get
				ConfigurationSmartTags = _ConfigurationSmartTags
			End Get
		End Property

		Sub New()
			_ConfigurationPath = Helpers.AppConfigSetting("ConfigurationPath")
			_ApplicationPath = Helpers.AppConfigSetting("ApplicationRealPath")
			_LanguageSettingsFile = Helpers.AppConfigSetting("LanguageSettingsFile")
			_LanguageSettingsPath = Helpers.AppConfigSetting("LanguageSettingsPath")
			_ConfigurationFilePath = _ConfigurationPath & Helpers.AppConfigSetting("ConfigurationFile")
			_ConfigurationSmartTags = _ConfigurationPath & Helpers.AppConfigSetting("ConfigurationSmartTags")

		End Sub

		Public Function GetSettingsFromConfiguration() As ComolSettings
			Dim oSettings As New ComolSettings

            oSettings.EditorConfigurationPath = Helpers.AppConfigSetting("EditorConfigurations")
            'oSettings.DefaultLanguage = Me.GetDefaultLanguage
            oSettings.Tag = Me.LoadTagSettingsFromFile(_ConfigurationPath & Helpers.AppConfigSetting("ConfigurationTags"))
            oSettings.Quiz = Me.LoadQuizSettingsFromFile(_ConfigurationPath & Helpers.AppConfigSetting("ConfigurationQuiz"))

            oSettings.DBconnectionSettings = Me.GetConnectionSettings


            Dim readerSettings As XmlReaderSettings = New XmlReaderSettings()
            readerSettings.IgnoreComments = True
            Dim reader As XmlReader = XmlReader.Create(_ConfigurationFilePath, readerSettings)


            Dim oFileconfigurazione As New XmlDocument
            Try
                Dim oElement As XmlElement
                Dim oNode As XmlNode
                oFileconfigurazione.Load(reader)

                Try
                    oSettings.Style = Me.GetStyleSettings(oFileconfigurazione.GetElementsByTagName("Style")(0).ChildNodes)
                Catch ex As Exception
                    oSettings.Style = New StyleSettings
                End Try

                Try
                    oSettings.ActionService = Me.GetActionsSettings(oFileconfigurazione.GetElementsByTagName("ActionService")(0).ChildNodes)
                Catch ex As Exception
                    oSettings.ActionService = New ActionsSettings
                End Try

                Try
                    oSettings.NotificationService = Me.GetNotificationSettings(oFileconfigurazione.GetElementsByTagName("NotificationService")(0).ChildNodes)
                Catch ex As Exception
                    oSettings.NotificationService = New NotificationSettings
                End Try

                Try
                    oSettings.NotificationErrorService = Me.GetNotificationErrorSettings(oFileconfigurazione.GetElementsByTagName("NotificationErrorService")(0).ChildNodes)
                Catch ex As Exception
                    oSettings.NotificationErrorService = New NotificationErrorSettings
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("BaseFileRepositoryPath")(0)
                    oSettings.BaseFileRepositoryPath = Me.GetConfigurationPath(oNode.ChildNodes)
                Catch ex As Exception

                End Try


                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("DefaultLanguage")(0)
                    oSettings.DefaultLanguage = Me.GetDefaultLanguage(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.DefaultLanguage = New Lingua(1, "it-IT")
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("Icodeon")(0)
                    oSettings.Icodeon = Me.GetIcodeonSettings(oNode.ChildNodes)
                Catch ex As Exception
                    'oSettings.Icodeon = New Lingua(1, "it-IT")
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ConfigLogin")(0)
                    oSettings.Login = Me.GetLoginSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.Login = New LoginSettings
                End Try



                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ChatService")(0)
                    oSettings.ChatService = Me.GetChatSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.ChatService = New ChatSettings
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("OnErrorShow404")(0)
                    oSettings.OnErrorShow404 = CBool(oNode.InnerText)
                Catch ex As Exception
                    oSettings.OnErrorShow404 = True
                End Try
                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ConfigExtension")(0)
                    oSettings.Extension = Me.GetExtensionSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.Extension = New ExtensionSettings
                End Try


                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("DBcodice")(0)
                    If Not IsNothing(oNode) Then
                        Try
                            oSettings.CodiceDB = CInt(oNode.InnerText)
                        Catch ex As Exception
                            oSettings.CodiceDB = 0
                        End Try
                    Else
                        oSettings.CodiceDB = 0
                    End If
                Catch ex As Exception

                End Try
                'Try
                '	oNode = oFileconfigurazione.GetElementsByTagName("Tutor")(0)
                '	oSettings.Tutor = Me.GetTutorSettings(oNode.ChildNodes, Helpers.AppConfigSetting("ConfigurationFileCSV"))
                'Catch ex As Exception

                'End Try
                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ConfigFileAndPath")(0)
                    oSettings.File = Me.GetFileSettings(oNode.ChildNodes)
                Catch ex As Exception
                End Try
                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ConfigMail")(0)
                    oSettings.Mail = Me.GetMailSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.Mail = New MailSettings
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("BulkInsert")(0)
                    oSettings.BulkInsert = Me.GetBulkInsertSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.BulkInsert = New BulkInsertSettings
                End Try


                Try
                    oElement = CType(oFileconfigurazione.GetElementsByTagName("Latex")(0), XmlElement)
                    oSettings.Latex = Me.GetLatexSettings(oElement)
                Catch ex As Exception
                    oSettings.Latex = New LatexSettings
                End Try
                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("ConfigurationPresenter")(0)
                    oSettings.Presenter = Me.LoadPresenterSettings(oNode.ChildNodes)
                Catch ex As Exception
                    oSettings.Presenter = New PresenterSettings
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("TopBar")(0)
                    oSettings.TopBar = Me.GetTopBarSettings(oNode.ChildNodes)
                Catch ex As Exception
                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("SkinSettings")(0)
                    oSettings.SkinSettings = Me.GetSkinSettings(oNode.ChildNodes)
                Catch ex As Exception

                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("DocTemplateSettings")(0)
                    oSettings.DocTemplateSettings = Me.GetDocTempalteSettings(oNode.ChildNodes)
                Catch ex As Exception

                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("WebConferencingSettings")(0)
                    oSettings.WebConferencingSettings = Me.GetWebConferencingSettings(oNode.ChildNodes)
                Catch ex As Exception

                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("Questionnaire")(0)
                    oSettings.QuestionnaireSettings = Me.GetQuestionnaireSettings(oNode.ChildNodes)
                Catch ex As Exception

                End Try

                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("FederationSettings")(0)
                    oSettings.FederationSettings = oNode.InnerText

                Catch ex As Exception
                    oSettings.FederationSettings = ""
                End Try


                Try
                    oNode = oFileconfigurazione.GetElementsByTagName("FeaturesSettings")(0)
                    oSettings.Features = Me.GetFeaturesSettings(oNode.ChildNodes)
                Catch ex As Exception

                End Try



            Catch ex As Exception

            End Try
            Return oSettings
        End Function

        Private Function GetDefaultLanguage(ByVal oNodes As XmlNodeList) As Lingua
            Dim LinguaCode As String = "it-IT"
            Dim LinguaID As Integer = 1

            If Not (IsNothing(oNodes) AndAlso oNodes.Count = 0) Then
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "ID"
                            Try
                                LinguaID = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "Code"
                            Try
                                LinguaCode = oNode.InnerText
                            Catch ex As Exception

                            End Try
                    End Select
                Next
            End If
            Return Lingua.CreateByCode(LinguaID, LinguaCode)
        End Function

        Private Function GetConnectionSettings() As DBconnectionSettings
            Dim oDBconnectionSettings As New DBconnectionSettings

            Try
                For Each oConnectionStrings As System.Configuration.ConnectionStringSettings In System.Configuration.ConfigurationManager.ConnectionStrings
                    Select Case LCase(oConnectionStrings.Name)
                        Case "errori"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Errori)

                        Case "esse3connection"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Esse3)

                        Case "questionari"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Questionari)
                        Case "questionari"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Questionari)
                        Case "knowledgetutor"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.KnowledgeTutor)
                        Case "mail"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Mail)
                        Case "statistiche"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.Statistiche)
                        Case "comol"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.COMOL)
                        Case "scormplayer"
                            Dim oConnection As New ConnectionDB(oConnectionStrings.Name, oConnectionStrings.ConnectionString, oConnectionStrings.ProviderName, ConnectionType.SQL)
                            oDBconnectionSettings.AddConnection(oConnection, DBconnectionSettings.DBsetting.ScormPlayer)
                    End Select
                Next
            Catch ex As Exception

            End Try
            Return oDBconnectionSettings
        End Function

#Region "Presenter"
        Private Function LoadPresenterSettings(ByVal oNodes As XmlNodeList) As PresenterSettings
            Dim oPresenterSettings As New PresenterSettings

            Try
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "FullDefaultStartPage"
                            Try
                                oPresenterSettings.FullDefaultStartPage = oNode.InnerText
                            Catch ex As Exception
                                oPresenterSettings.FullDefaultStartPage = ""
                            End Try
                        Case "DefaultStartPage"
                            Try
                                oPresenterSettings.DefaultStartPage = oNode.InnerText
                            Catch ex As Exception
                                oPresenterSettings.DefaultStartPage = ""
                            End Try
                        Case "DefaultDisplayNameMode"
                            Try
                                oPresenterSettings.DefaultDisplayNameMode = CInt(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.DefaultDisplayNameMode = 2
                            End Try
                        Case "DefaultManagement"
                            Try
                                oPresenterSettings.DefaultManagement = oNode.InnerText
                            Catch ex As Exception
                            End Try
                        Case "DefaultTitle"
                            Try
                                oPresenterSettings.DefaultTitle = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "isNew"
                            Try
                                oPresenterSettings.isNew = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultCommunityModule"
                            Try
                                oPresenterSettings.DefaultCommunityModule = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "EnabledPortalHomeModal"
                            Try
                                oPresenterSettings.EnabledPortalHomeModal = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultRemoveServiceNotification"
                            Try
                                oPresenterSettings.DefaultRemoveServiceNotification = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "EditorItemHandlerPath"
                            Try
                                oPresenterSettings.EditorItemHandlerPath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "AjaxTimer"
                            Try
                                oPresenterSettings.AjaxTimer = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultNation"
                            Try
                                oPresenterSettings.DefaultNationID = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultProvince"
                            Try
                                oPresenterSettings.DefaultProvinceID = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultNoProvinceID"
                            Try
                                oPresenterSettings.DefaultNoProvinceID = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultHomeHeaderLink"
                            Try
                                oPresenterSettings.DefaultHomeHeaderLink = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "DefaultSubscriptionsLink"
                            Try
                                oPresenterSettings.DefaultSubscriptionsLink = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "DefaultLogonPage"
                            Try
                                oPresenterSettings.DefaultLogonPage = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "DefaultTaxCodeRequired"
                            Try
                                oPresenterSettings.DefaultTaxCodeRequired = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultHighSchool"
                            Try
                                oPresenterSettings.DefaultHighSchoolTypeID = CInt(oNode.InnerText)
                            Catch ex As Exception

                            End Try

                        Case "DefaultProfileTypesToActivate"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    If oNode.InnerText.Contains(",") Then
                                        oPresenterSettings.DefaultProfileTypesToActivate.AddRange((From o In oNode.InnerText.Split(","c) Select CInt(o)).ToList)
                                    Else
                                        oPresenterSettings.DefaultProfileTypesToActivate.Add(CInt(oNode.InnerText))
                                    End If

                                End If

                            Catch ex As Exception

                            End Try
                        Case "DefaultSplitMailRecipients"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    If IsNumeric(oNode.InnerText) Then
                                        oPresenterSettings.DefaultSplitMailRecipients = CInt(oNode.InnerText)
                                    Else
                                        oPresenterSettings.DefaultSplitMailRecipients = 100
                                    End If

                                End If

                            Catch ex As Exception

                            End Try
                        Case "DefaultMatricolaRequired"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    oPresenterSettings.DefaultMatricolaRequired = CBool(oNode.InnerText)
                                End If
                            Catch ex As Exception

                            End Try
                        Case "DefaultBirthDateRequired"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    oPresenterSettings.DefaultBirthDateRequired = CBool(oNode.InnerText)
                                End If
                            Catch ex As Exception

                            End Try
                        Case "RepositoryConfiguration"
                            Try
                                oPresenterSettings.RepositoryConfiguration = CreateRepositoryConfiguration(oNode.ChildNodes)
                            Catch ex As Exception
                                oPresenterSettings.RepositoryConfiguration = New RepositoryConfiguration
                            End Try
                            'Case "EducationalPathConfiguration"
                            '    Try
                            '        oPresenterSettings.EduPathConfiguration = CreateEducationalPathConfiguration(oNode.ChildNodes)
                            '    Catch ex As Exception
                            '        oPresenterSettings.EduPathConfiguration = New EducationalPathConfiguration
                            '    End Try
                        Case "AllowUserRegistration"
                            Try
                                oPresenterSettings.AllowUserRegistration = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.AllowUserRegistration = True
                            End Try
                        Case "AllowSaveAndContinueQuestionnaire"
                            Try
                                oPresenterSettings.AllowSaveAndContinueQuestionnaire = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.AllowSaveAndContinueQuestionnaire = True
                            End Try
                        Case "AllowSaveBetweenPageQuestionnaire"
                            Try
                                oPresenterSettings.AllowSaveBetweenPageQuestionnaire = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.AllowSaveBetweenPageQuestionnaire = True
                            End Try
                        Case "AllowOverWriteQuestionnaireSavingPolicy"
                            Try
                                oPresenterSettings.AllowOverWriteQuestionnaireSavingPolicy = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.AllowOverWriteQuestionnaireSavingPolicy = True
                            End Try
                        Case "AllowDssUse"
                            Try
                                oPresenterSettings.AllowDssUse = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.AllowDssUse = True
                            End Try
                        Case "EnabledLogonAs"
                            Try
                                oPresenterSettings.EnabledLogonAs = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.EnabledLogonAs = False
                            End Try

                        Case "EnabledAdministrativeLogonAs"
                            Try
                                oPresenterSettings.EnabledAdministrativeLogonAs = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oPresenterSettings.EnabledAdministrativeLogonAs = False
                            End Try
                        Case "DefaultFileCategoryID"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    oPresenterSettings.DefaultFileCategoryID = CInt(oNode.InnerText)
                                End If
                            Catch ex As Exception

                            End Try

                        Case "DefaultFolderCategoryID"
                            Try
                                If Not String.IsNullOrEmpty(oNode.InnerText) Then
                                    oPresenterSettings.DefaultFolderCategoryID = CInt(oNode.InnerText)
                                End If
                            Catch ex As Exception

                            End Try

                        Case "PortalDisplayInfo"
                            Try
                                oPresenterSettings.PortalDisplay = CreatePortalDisplayInfoConfiguration(oNode.ChildNodes)
                            Catch ex As Exception
                                oPresenterSettings.PortalDisplay = New PortalDisplayInfo
                            End Try

                    End Select
                Next
            Catch ex As Exception

            End Try
            Return oPresenterSettings
        End Function

        Private Function CreateRepositoryConfiguration(ByVal nodes As XmlNodeList) As RepositoryConfiguration
            Dim config As New RepositoryConfiguration()

            For Each node As XmlNode In nodes
                Select Case node.Name
                    Case "AvailableItemType"
                        Try
                            config.AvailableItemType = (From i In node.InnerText.Split(","c) Select CInt(i)).ToList
                        Catch ex As Exception
                        End Try
                    Case "AllowDownload"
                        Try
                            config.AllowDownload = (From i In node.InnerText.Split(","c) Select CInt(i)).ToList
                        Catch ex As Exception
                        End Try
                    Case "DefaultDownload"
                        Try
                            config.DefaultDownload = (From i In node.InnerText.Split(","c) Select CInt(i)).ToList
                        Catch ex As Exception
                        End Try
                    Case "UrlConfigurations"
                        Try
                            config.UrlConfigurations = CreateRepositoryUrlConfiguration(node.ChildNodes())
                        Catch ex As Exception
                        End Try
                End Select
            Next
            Return config
        End Function
        Private Function CreatePortalDisplayInfoConfiguration(ByVal nodes As XmlNodeList) As PortalDisplayInfo
            Dim config As New PortalDisplayInfo()

            For Each node As XmlNode In nodes
                Select Case node.Name
                    Case "Name"
                        For Each oLocName As XmlNode In node.ChildNodes(0)
                            Try
                                Dim Key As Integer = CInt(oLocName.Attributes("LanguageId").Value)
                                Dim Value As String = oLocName.InnerText

                                If Not config.Name.LocalNames.ContainsKey(Key) Then
                                    config.Name.LocalNames.Add(Key, Value)
                                End If


                            Catch ex As Exception
                                Dim err As String = ex.ToString
                                err &= ""
                            End Try

                        Next
                    Case "Home"
                        For Each oLocName As XmlNode In node.ChildNodes(0)
                            Try
                                Dim Key As Integer = CInt(oLocName.Attributes("LanguageId").Value)
                                Dim Value As String = oLocName.InnerText

                                If Not config.Home.LocalNames.ContainsKey(Key) Then
                                    config.Home.LocalNames.Add(Key, Value)
                                End If
                            Catch ex As Exception
                                Dim err As String = ex.ToString
                                err &= ""
                            End Try

                        Next
                    Case "IstanceName"
                        For Each oLocName As XmlNode In node.ChildNodes(0)
                            Try
                                Dim Key As Integer = CInt(oLocName.Attributes("LanguageId").Value)
                                Dim Value As String = oLocName.InnerText

                                If Not config.IstanceName.LocalNames.ContainsKey(Key) Then
                                    config.IstanceName.LocalNames.Add(Key, Value)
                                End If
                            Catch ex As Exception
                                Dim err As String = ex.ToString
                                err &= ""
                            End Try

                        Next
                End Select
            Next
            Return config
        End Function
        'Private Function CreateEducationalPathConfiguration(ByVal nodes As XmlNodeList) As EducationalPathConfiguration
        '    Dim config As New EducationalPathConfiguration()
        '    For Each node As XmlNode In nodes
        '        Select Case node.Name
        '            Case "AllowDeleteStatistics"
        '                Try
        '                    config.AllowDeleteStatistics = CBool(node.InnerText)
        '                Catch ex As Exception
        '                End Try
        '            Case "AllowFullDeleteStatistics"
        '                Try
        '                    config.AllowFullDeleteStatistics = CBool(node.InnerText)
        '                Catch ex As Exception
        '                End Try
        '            Case Else
        '                If node.Name.StartsWith("Stat") Then
        '                    For Each item As EducationalPathConfiguration.Statistics In [Enum].GetValues(GetType(EducationalPathConfiguration.Statistics))
        '                        Try
        '                            If node.Name.EndsWith(item.ToString & "Identifiers") Then
        '                                config.StatisticsIdentifiers(item) = CBool(node.InnerText)
        '                                Exit For
        '                            ElseIf node.Name.EndsWith(item.ToString & "AgencyInfo") Then
        '                                config.StatisticsAgency(item) = CBool(node.InnerText)
        '                                Exit For
        '                            ElseIf node.Name.EndsWith(item.ToString & "TaxCode") Then
        '                                config.StatisticsTaxCode(item) = CBool(node.InnerText)
        '                                Exit For
        '                            ElseIf node.Name.EndsWith(item.ToString & "QuestionnaireAttempts") Then
        '                                config.FullQuestionnaireAttempts(item) = CBool(node.InnerText)
        '                                Exit For
        '                            End If
        '                        Catch ex As Exception

        '                        End Try
        '                    Next
        '                End If
        '        End Select
        '    Next
        '    Return config
        'End Function
        Private Function CreateRepositoryUrlConfiguration(ByVal nodes As XmlNodeList) As List(Of RepositoryUrlConfiguration)
            Dim list As New List(Of RepositoryUrlConfiguration)

            For Each node As XmlNode In nodes
                If node.ChildNodes.Count > 0 Then
                    Try
                        Dim url As New RepositoryUrlConfiguration
                        For Each child As XmlNode In node.ChildNodes
                            Select Case child.Name
                                Case "PlayerUrl"
                                    url.PlayerUrl = child.InnerText
                                Case "RepositoryItemType"
                                    url.RepositoryItemType = CInt(child.InnerText)
                                Case "RedirectToFilePage"
                                    url.RedirectToFilePage = CBool(child.InnerText)
                            End Select
                        Next
                        list.Add(url)
                    Catch ex As Exception

                    End Try

                End If
            Next
            Return list
        End Function


#End Region

#Region "Tag"
        Private Function LoadTagSettingsFromFile(ByVal FileName As String) As TagSettings
            Dim oTagSettings As TagSettings

            Try
                Dim oFileTag As New XmlDocument
                oFileTag.Load(FileName)
                Dim oNode As XmlNode = oFileTag.GetElementsByTagName("Config")(0)
                oTagSettings = Me.LoadTagSettings(oNode.ChildNodes)
                Return oTagSettings
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Private Function LoadTagSettings(ByVal oNodes As XmlNodeList) As TagSettings
            Dim oTagSettings As New TagSettings

            Try
                If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                    oTagSettings.AddTags(New List(Of TemplateTag), TagSettings.TagType.Questionario)
                Else
                    For Each oNode As XmlNode In oNodes
                        Select Case oNode.Name
                            Case "TagQuestionario"
                                Dim oLista As New List(Of TemplateTag)
                                For Each oNodeTag As XmlNode In oNode.ChildNodes
                                    Dim oTemplateTag As New TemplateTag(Helpers.GetXMLattribute(oNodeTag, "name"),
                                    Helpers.GetXMLattribute(oNodeTag, "value"), Helpers.GetXMLattribute(oNodeTag, "proprieta"),
                                    GenericValidator.ValBool(Helpers.GetXMLattribute(oNodeTag, "isObligatory"), True), GenericValidator.ValInteger(Helpers.GetXMLattribute(oNodeTag, "fase"), 0))

                                    oLista.Add(oTemplateTag)
                                Next
                                oTagSettings.AddTags(oLista, TagSettings.TagType.Questionario)
                        End Select
                    Next
                End If

            Catch ex As Exception

            End Try
            Return oTagSettings
        End Function
#End Region

#Region "CSV"
        Private Function LoadConfigurationCSVfromFile(ByVal FileName As String) As CSVsettings
            Dim oCSVsettings As CSVsettings

            Try
                Dim oFile As New XmlDocument
                oFile.Load(Me._ConfigurationPath & FileName)
                Dim oNode As XmlNode = oFile.GetElementsByTagName("Config")(0)
                oCSVsettings = Me.LoadCSVsettings(oNode.ChildNodes)
                Return oCSVsettings
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Private Function LoadCSVsettings(ByVal oNodes As XmlNodeList) As CSVsettings
            Dim oCSVsettings As New CSVsettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                oCSVsettings.AddSettings(New FileCSV, CSVsettings.CSVtype.KnowledgeTutor)
            Else

                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "CSVknowledgeTutor"
                            Dim oFileCSV As New FileCSV
                            oFileCSV.TextDelimeter = Helpers.GetXMLattribute(oNode, "textDelimeter")
                            oFileCSV.ColumnDelimeter = Helpers.GetXMLattribute(oNode, "columnDelimeter")

                            For Each oNodeCSVfield As XmlNode In oNode.ChildNodes
                                Dim oField As New FileCsvField(Helpers.GetXMLattribute(oNodeCSVfield, "name"),
                                Helpers.GetXMLattribute(oNodeCSVfield, "DBfield"), Helpers.GetXMLattribute(oNodeCSVfield, "property"))
                                oFileCSV.Fields.Add(oField)

                            Next
                            oCSVsettings.AddSettings(oFileCSV, CSVsettings.CSVtype.KnowledgeTutor)
                    End Select
                Next
            End If
            Return oCSVsettings
        End Function
#End Region

        Private Function GetLoginSettings(ByVal oNodes As XmlNodeList) As LoginSettings
            Dim oLoginSettings As New LoginSettings
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oLoginSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "RichiediSSL"
                            Try
                                oLoginSettings.isSSLrequired = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oLoginSettings.isSSLrequired = False
                            End Try
                        Case "RichiediLoginSSL"
                            Try
                                oLoginSettings.isSSLloginRequired = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "aggiornamentoXML"
                            Try
                                oLoginSettings.DaysToUpdateProfile = CInt(oNode.InnerText)
                            Catch ex As Exception
                                oLoginSettings.DaysToUpdateProfile = 4
                            End Try
                        Case "SubscriptionActive"
                            Try
                                oLoginSettings.SubscriptionActive = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "ShowHelpToSubscription"
                            Try
                                oLoginSettings.showHelpToSubscription = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "AlwaysDefaultPageForInternal"
                            Try
                                oLoginSettings.AlwaysDefaultPageForInternal = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oLoginSettings.AlwaysDefaultPageForInternal = False
                            End Try
                        Case "ProfileAutoActivation"
                            Try
                                oLoginSettings.ProfileAutoActivation = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oLoginSettings.ProfileAutoActivation = True
                            End Try
                        Case "SendRegistrationMail"
                            Try
                                oLoginSettings.SendRegistrationMail = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oLoginSettings.SendRegistrationMail = True
                            End Try
                    End Select
                Next
            End If
            If oLoginSettings.isSSLrequired Then
                oLoginSettings.isSSLloginRequired = True
            End If
            Return oLoginSettings
        End Function
        Private Function GetLatexSettings(ByVal oNode As XmlElement) As LatexSettings
            Dim oLatexSettings As New LatexSettings
            If Not IsNothing(oNode) Then
                oLatexSettings.CssStylePath = oNode.GetAttribute("CSS")
                oLatexSettings.JavascriptPath = oNode.GetAttribute("Javascript")

                Try
                    For Each oNodeChild As XmlElement In oNode.ChildNodes
                        If oNodeChild.Name = "ServersRender" Then
                            For Each oNodeServer As XmlElement In oNodeChild.ChildNodes
                                oLatexSettings.Servers.Add(New ForeignRenderServer(oNodeServer.GetAttribute("Name"), oNodeServer.GetAttribute("RemoteUrl"), CBool(oNodeServer.GetAttribute("isDefault")), CBool(oNodeServer.GetAttribute("isDefault"))))
                            Next
                        End If
                    Next
                Catch ex As Exception

                End Try
            End If
            Return oLatexSettings
        End Function
        Private Function GetChatSettings(ByVal oNodes As XmlNodeList) As ChatSettings
            Dim oChatSettings As New ChatSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oChatSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Enabled"
                            Try
                                oChatSettings.Enabled = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oChatSettings.Enabled = False
                            End Try
                        Case "DefaultUrl"
                            Try
                                oChatSettings.DefaultUrl = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "DefaultFileUrl"
                            Try
                                oChatSettings.DefaultFileUrl = oNode.InnerText
                            Catch ex As Exception

                            End Try
                    End Select
                Next
            End If
            Return oChatSettings
        End Function

        Private Function GetStyleSettings(ByVal oNodes As XmlNodeList) As StyleSettings
            Dim oSettings As New StyleSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        'Case "Header"
                        '    Try
                        '        oSettings.Header = oNode.InnerText
                        '    Catch ex As Exception

                        '    End Try
                        'Case "Login"
                        '    Try
                        '        oSettings.Login = oNode.InnerText
                        '    Catch ex As Exception

                        '    End Try
                        Case "NewMenu"
                            Try
                                oSettings.Menu = oNode.InnerText
                            Catch ex As Exception

                            End Try
                            'Case "SkinCss"
                            '    Try
                            '        oSettings.SkinCss = oNode.InnerText
                            '    Catch ex As Exception

                            '    End Try

                            'Case "DefaultImgFolder"
                            '    Try
                            '        oSettings.DefaultImgFolder = oNode.InnerText
                            '    Catch ex As Exception

                            '    End Try
                            'Case "BaseFolderUrl"
                            '    Try
                            '        oSettings.BaseFolderUrl = oNode.InnerText
                            '    Catch ex As Exception

                            '    End Try
                            'Case "DefaultUrl"
                            '    Try
                            '        oSettings.DefaultUrl = oNode.InnerText
                            '    Catch ex As Exception

                            '    End Try

                        Case "jqueryTheme"
                            Try
                                oSettings.jqueryTheme = oNode.InnerText
                                If String.IsNullOrEmpty(oSettings.jqueryTheme) Then
                                    oSettings.jqueryTheme = "Base"
                                End If
                            Catch ex As Exception
                                oSettings.jqueryTheme = "Base"
                            End Try
                        Case "jqueryVersion"
                            Try
                                oSettings.jqueryVersion = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "jqueryUIVersion"
                            Try
                                oSettings.jqueryUIVersion = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "jqueryMigrateVersion"
                            Try
                                oSettings.jqueryMigrateVersion = oNode.InnerText
                            Catch ex As Exception

                            End Try
                            'Case "UseNewSkin"
                            '    oSettings.UseNewSkin = True
                    End Select
                Next
            End If
            Return oSettings
        End Function
        Private Function GetActionsSettings(ByVal oNodes As XmlNodeList) As ActionsSettings
            Dim oSettings As New ActionsSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Enabled"
                            Try
                                oSettings.Enabled = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "EnableWebPresence"
                            Try
                                oSettings.EnableWebPresence = CBool(oNode.InnerText)
                            Catch ex As Exception
                            End Try
                        Case "EnableBrowser"
                            Try
                                oSettings.EnableBrowser = CBool(oNode.InnerText)
                            Catch ex As Exception
                            End Try
                        Case "EnableAction"
                            Try
                                oSettings.EnableAction = CBool(oNode.InnerText)
                            Catch ex As Exception
                            End Try
                    End Select
                Next
            End If
            Return oSettings
        End Function
        Private Function GetNotificationSettings(ByVal oNodes As XmlNodeList) As NotificationSettings
            Dim oSettings As New NotificationSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Enabled"
                            Try
                                oSettings.Enabled = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "Services"
                            oSettings.Services = GetServicesToNotify(oNode.ChildNodes)
                    End Select
                Next

            End If
            Return oSettings
        End Function
        Private Function GetServicesToNotify(ByVal oNodes As XmlNodeList) As List(Of ServiceToNotify)
            Dim oList As New List(Of ServiceToNotify)
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oList
            Else
                For Each oNode As XmlNode In oNodes
                    Dim oService As New ServiceToNotify
                    Try
                        oService.Name = oNode.Attributes("Name").Value
                        oService.Enabled = CBool(oNode.Attributes("Enabled").Value)
                        oService.Code = oNode.Attributes("Code").Value
                        oList.Add(oService)
                    Catch ex As Exception

                    End Try
                Next
            End If
            Return oList
        End Function
        Private Function GetNotificationErrorSettings(ByVal oNodes As XmlNodeList) As NotificationErrorSettings
            Dim oSettings As New NotificationErrorSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Enabled"
                            Try
                                oSettings.Enabled = CBool(oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "ComolUniqueID"
                            Try
                                oSettings.ComolUniqueID = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "ErrorsType"
                            For Each o As XmlNode In oNode.ChildNodes
                                oSettings.ErrorsType.Add(
                                    New dtoErrorType(o.Attributes("Enabled").Value,
                                    o.Attributes("ErrorType").Value,
                                    o.Attributes("PersistTo").Value))
                            Next

                            'oSettings.ErrorsType.AddRange((
                            '                              From o As XmlNode In oNode.ChildNodes
                            '                              Select New dtoErrorType(
                            '                                  o.Attributes("Enabled").Value,
                            '                                  o.Attributes("ErrorType").Value,
                            '                                  o.Attributes("PersistTo").Value)).ToList)
                    End Select
                Next

            End If
            Return oSettings
        End Function
        'Private Function GetServicesToNotify(ByVal oNodes As XmlNodeList) As List(Of ServiceToNotify)
        '    Dim oList As New List(Of ServiceToNotify)
        '    If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
        '        Return oList
        '    Else
        '        For Each oNode As XmlNode In oNodes
        '            Dim oService As New ServiceToNotify
        '            Try
        '                oService.Name = oNode.Attributes("Name").Value
        '                oService.Enabled = oNode.Attributes("Enabled").Value
        '                oService.Code = oNode.Attributes("Code").Value
        '                oList.Add(oService)
        '            Catch ex As Exception

        '            End Try
        '        Next
        '    End If
        '    Return oList
        'End Function
        Private Function GetFileSettings(ByVal oNodes As XmlNodeList) As FileSettings
            Dim oFileSettings As New FileSettings
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return New FileSettings
            Else
                Dim addedThumbnail As Boolean = False
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "File"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.File)
                            Catch ex As Exception

                            End Try
                        Case "FileThumbnail"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.FileThumbnail)
                                addedThumbnail = True
                            Catch ex As Exception

                            End Try
                        Case "VideoCast"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.VideoCast)
                            Catch ex As Exception

                            End Try
                        Case "Scorm"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Scorm)
                            Catch ex As Exception

                            End Try
                        Case "Cover"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Cover)
                            Catch ex As Exception
                            End Try
                        Case "Mail"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Mail)
                            Catch ex As Exception

                            End Try
                        Case "Profile"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Profilo)
                            Catch ex As Exception

                            End Try
                        Case "Noticeboard"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Noticeboard)
                            Catch ex As Exception

                            End Try
                        Case "Glossary"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Glossary)
                            Catch ex As Exception

                            End Try
                        Case "Tiles"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Tile)
                            Catch ex As Exception

                            End Try
                        Case "Wiki"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Wiki)
                            Catch ex As Exception

                            End Try
                        Case "FileTesi"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.FileTesi)
                            Catch ex As Exception

                            End Try
                        Case "Repository"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.Repository)
                            Catch ex As Exception

                            End Try
                        Case "UserCertifications"
                            Try
                                oFileSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), FileSettings.ConfigType.UserCertifications)
                            Catch ex As Exception

                            End Try
                    End Select
                Next
                If Not addedThumbnail Then
                    Dim thumbnail As New ConfigurationPath
                    If Not String.IsNullOrWhiteSpace(oFileSettings.Materiale.DrivePath) Then
                        thumbnail.DrivePath = oFileSettings.Materiale.DrivePath & "Thumbnails\"
                    End If
                    If Not String.IsNullOrWhiteSpace(oFileSettings.Materiale.VirtualPath) Then
                        thumbnail.VirtualPath = oFileSettings.Materiale.VirtualPath & "Thumbnails/"
                    End If
                    If Not String.IsNullOrWhiteSpace(oFileSettings.Materiale.ServerPath) Then
                        thumbnail.ServerPath = oFileSettings.Materiale.ServerPath & "Thumbnails\"
                    End If
                    If Not String.IsNullOrWhiteSpace(oFileSettings.Materiale.ServerVirtualPath) Then
                        thumbnail.VirtualPath = oFileSettings.Materiale.ServerVirtualPath & "Thumbnails/"
                    End If
                    thumbnail.isOnThisServer = oFileSettings.Materiale.isOnThisServer
                    thumbnail.MaxSize = oFileSettings.Materiale.MaxSize
                    thumbnail.MaxUploadSize = oFileSettings.Materiale.MaxUploadSize
                    oFileSettings.AddSettings(thumbnail, FileSettings.ConfigType.FileThumbnail)
                End If
            End If
            Return oFileSettings
        End Function
        Private Function GetBulkInsertSettings(ByVal oNodes As XmlNodeList) As BulkInsertSettings
            Dim oBulkInsertSettings As New BulkInsertSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Questionario"
                            Try
                                oBulkInsertSettings.AddSettings(Me.GetConfigurationPath(oNode.ChildNodes), BulkInsertSettings.ConfigType.Questionari)
                            Catch ex As Exception

                            End Try
                    End Select
                Next
            End If
            Return oBulkInsertSettings
        End Function

        Private Function GetIcodeonSettings(ByVal oNodes As XmlNodeList) As IcodeonSettings
            Dim oIcodeonSettings As New IcodeonSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "PlayerUrl"
                            oIcodeonSettings.PlayerBaseUrl = oNode.InnerText
                        Case "Database"
                            oIcodeonSettings.DBname = oNode.InnerText
                        Case "SiteDownloadScorm"
                            oIcodeonSettings.SiteDownloadScorm = CBool(oNode.InnerText)
                        Case "MappingPath" ' Aggiunta - Mirco - 06/07/2010 - testato
                            oIcodeonSettings.MappingPath = oNode.InnerText

                        Case "OverrideSSLsettings"
                            oIcodeonSettings.OverrideSSLsettings = CBool(oNode.InnerText)
                        Case "StatisticsRules"
                            If oNode.HasChildNodes Then
                                Try
                                    For Each node As XmlNode In oNode.ChildNodes
                                        Dim dto As New IcodeonSettings.IcodeonStatistics
                                        dto.Restricted = CBool(node.Attributes("restricted").Value)
                                        dto.RestrictedRoles = GetIcodeonRestrictedRoles(node.FirstChild.ChildNodes)
                                        oIcodeonSettings.StatisticsRules.Add(CInt(node.Attributes("id").Value), dto)
                                    Next
                                Catch ex As Exception

                                End Try

                            End If
                        Case "AutoEvaluate"
                            oIcodeonSettings.AutoEvaluate = CBool(oNode.InnerText)
                    End Select
                Next
            End If
            Return oIcodeonSettings
        End Function
        Private Function GetIcodeonRestrictedRoles(ByVal oNodes As XmlNodeList) As List(Of Integer)
            Dim roles As New List(Of Integer)
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return roles
            Else
                For Each r As XmlNode In oNodes
                    If IsNumeric(r.InnerText) Then
                        roles.Add(CInt(r.InnerText))
                    End If
                Next

                'roles = (From r As XmlNode In oNodes Where IsNumeric(r.InnerText) Select CInt(r.InnerText)).ToList
            End If
            Return roles
        End Function

#Region "Extension Common"
        Private Function GetConfigurationPath(ByVal oNodes As XmlNodeList) As ConfigurationPath
            Dim oConfigurationPath As New ConfigurationPath
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                oConfigurationPath.isInvalid = True
                oConfigurationPath.isOnThisServer = True
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "VirtualPath"
                            Try
                                oConfigurationPath.VirtualPath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "PhisicalPath"
                            Try
                                oConfigurationPath.DrivePath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "SameServer"
                            Try
                                oConfigurationPath.isOnThisServer = CBool(oNode.InnerText)
                            Catch ex As Exception
                                oConfigurationPath.isOnThisServer = True
                            End Try
                        Case "ServerPath"
                            Try
                                oConfigurationPath.ServerPath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "ServerVirtualPath"
                            Try
                                oConfigurationPath.ServerVirtualPath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "RewritePath"
                            Try
                                oConfigurationPath.RewritePath = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "MaxSize"
                            Try
                                oConfigurationPath.MaxSize = CInt(oNode.InnerText)
                            Catch ex As Exception
                                oConfigurationPath.MaxSize = -1
                            End Try
                        Case "MaxUploadSize"
                            Try
                                oConfigurationPath.MaxUploadSize = CInt(oNode.InnerText)
                            Catch ex As Exception
                                oConfigurationPath.MaxUploadSize = -1
                            End Try
                    End Select
                Next
                If oConfigurationPath.VirtualPath <> "" OrElse oConfigurationPath.DrivePath <> "" OrElse oConfigurationPath.ServerPath <> "" OrElse oConfigurationPath.ServerVirtualPath <> "" Then
                    oConfigurationPath.isInvalid = False
                Else
                    oConfigurationPath.isInvalid = True
                End If
            End If
            Return oConfigurationPath
        End Function
#End Region

#Region "Extension Settings"
        Private Function GetExtensionSettings(ByVal oNodes As XmlNodeList) As ExtensionSettings
            Dim oExtensionSettings As ExtensionSettings
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                oExtensionSettings = New ExtensionSettings
            Else
                Dim FileIcon As String = "", FileMime As String = "", DefaultIcon As String = "", ExtensionToShow As String = ""
                Dim iMimeTypes As New List(Of MimeType)
                Dim iIconElements As New List(Of IconElement)

                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "FileIcon"
                            Try
                                iIconElements = LoadIcons(_ConfigurationPath & oNode.InnerText)
                            Catch ex As Exception
                            End Try
                        Case "FileMime"
                            Try
                                iMimeTypes = LoadMimeTypes(_ConfigurationPath & oNode.InnerText)
                            Catch ex As Exception

                            End Try
                        Case "DefaultIcon"
                            DefaultIcon = oNode.InnerText
                        Case "ExtensionToShow"
                            ExtensionToShow = oNode.InnerText
                            If ExtensionToShow = "" Then
                                ExtensionToShow = ".iconFile"
                            End If
                    End Select
                Next
                oExtensionSettings = New ExtensionSettings(DefaultIcon, ExtensionToShow, iMimeTypes, iIconElements)
            End If
            Return oExtensionSettings
        End Function
        Private Function LoadIcons(ByVal FileName As String) As List(Of IconElement)
            Dim oList As New List(Of IconElement)
            Dim cacheKey As String = CachePolicy.SettingsIcon()

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oFileXML As New XmlDocument
                Try
                    oFileXML.Load(FileName)
                    For Each oNode As XmlElement In oFileXML.GetElementsByTagName("item")
                        oList.Add(New IconElement(oNode.GetAttribute("name"), oNode.InnerText))
                    Next
                Catch ex As Exception

                End Try
                ObjectBase.Cache.Insert(cacheKey, oList, New Caching.CacheDependency(FileName), DateTime.MaxValue, TimeSpan.Zero)
            Else
                oList = CType(ObjectBase.Cache(cacheKey), List(Of IconElement))
            End If
            Return oList
        End Function
        Private Function LoadMimeTypes(ByVal FileName As String) As List(Of MimeType)
            Dim oList As New List(Of MimeType)
            Dim cacheKey As String = CachePolicy.SettingsMimeType

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oFileXML As New XmlDocument
                Try
                    oFileXML.Load(FileName)
                    For Each oNode As XmlElement In oFileXML.GetElementsByTagName("item")
                        oList.Add(New MimeType(oNode.GetAttribute("name"), oNode.InnerText))
                    Next
                Catch ex As Exception

                End Try
                ObjectBase.Cache.Insert(cacheKey, oList, New Caching.CacheDependency(FileName), DateTime.MaxValue, TimeSpan.Zero)
            Else
                oList = CType(ObjectBase.Cache(cacheKey), List(Of MimeType))
            End If
            Return oList
        End Function
#End Region

#Region "Mail Settings"
        Private Function GetMailSettings(ByVal oNodes As XmlNodeList) As MailSettings
            Dim oMailSettings As New MailSettings

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                oMailSettings.SendMailByReply = False
            Else
                For Each oNode As XmlNode In oNodes
                    If oNode.InnerText <> "" Then
                        Select Case oNode.Name
                            Case "ServerSMTP"
                                oMailSettings.ServerSMTP = oNode.InnerText
                            Case "SendMailError"
                                Try
                                    oMailSettings.isErrorSendingActivated = CBool(oNode.InnerText)
                                Catch ex As Exception
                                    oMailSettings.isErrorSendingActivated = False
                                End Try
                            Case "RegistraMail"
                                Try
                                    oMailSettings.isSaveActivated = CBool(oNode.InnerText)
                                Catch ex As Exception
                                    oMailSettings.isSaveActivated = False
                                End Try
                            Case "systemMailError"
                                Try
                                    oMailSettings.SendMailErrorTo = New MailAddress(oNode.InnerText, oNode.InnerText)
                                Catch ex As Exception
                                    oMailSettings.SendMailErrorTo = Nothing
                                End Try
                            Case "systemMail"
                                oMailSettings.SystemSender = New MailAddress(oNode.InnerText, oNode.InnerText)
                            Case "systemErrorMailSender"
                                oMailSettings.ErrorSystemSender = New MailAddress(oNode.InnerText, oNode.InnerText)
                            Case "SendMailByReply"
                                Try
                                    oMailSettings.SendMailByReply = CBool(oNode.InnerText)
                                Catch ex As Exception
                                    oMailSettings.SendMailByReply = False
                                End Try
                            Case "RealMailSenderAccount"
                                oMailSettings.RealMailSenderAccount = New MailAddress(oNode.InnerText, oNode.InnerText)
                            Case "AuthenticationEnabled"
                                Try
                                    oMailSettings.AuthenticationEnabled = CBool(oNode.InnerText)
                                Catch ex As Exception

                                End Try
                            Case "UseSsl"
                                Try
                                    oMailSettings.UseSsl = CBool(oNode.InnerText)
                                Catch ex As Exception

                                End Try
                            Case "HostPort"
                                Try
                                    oMailSettings.HostPort = CInt(oNode.InnerText)
                                Catch ex As Exception
                                    oMailSettings.HostPort = 25
                                End Try
                            Case "CredentialsUsername"
                                oMailSettings.CredentialsUsername = oNode.InnerText
                            Case "CredentialsPassword"
                                oMailSettings.CredentialsPassword = oNode.InnerText
                        End Select
                    End If
                Next
            End If
            Return oMailSettings
        End Function


        Public Function GetLocalizedMailSettings(ByVal oMailSettings As MailSettings, ByVal oLingua As Lingua) As MailLocalized
            Dim oMailLocalized As New MailLocalized
            Dim oResourceConfig As ResourceManager = GetLocalizedResource(oLingua)

            With oMailLocalized
                Try
                    If oResourceConfig.getValue("systemErrorMailSenderName") <> "" Then
                        .ErrorSender = New MailAddress(oMailSettings.ErrorSystemSender.Address, oResourceConfig.getValue("systemErrorMailSenderName"))
                    Else
                        .ErrorSender = New MailAddress(oMailSettings.ErrorSystemSender.Address)
                    End If
                Catch ex As Exception
                    .ErrorSender = Nothing
                End Try

                .ErrorSubject = oResourceConfig.getValue("ErrorSubject")
                If .ErrorSubject = "" Then
                    .ErrorSubject = "Notifica Errore Automatica"
                End If
                .Language = oLingua
                .RealMailSenderAccount = oMailSettings.RealMailSenderAccount
                .SendErrorTo = oMailSettings.SendMailErrorTo
                .SendMailByReply = oMailSettings.SendMailByReply
                .ServerSMTP = oMailSettings.ServerSMTP
                .AuthenticationEnabled = oMailSettings.AuthenticationEnabled
                .UseSsl = oMailSettings.UseSsl
                .HostPort = oMailSettings.HostPort
                .CredentialsPassword = oMailSettings.CredentialsPassword
                .CredentialsUsername = oMailSettings.CredentialsUsername
                .SubjectForSenderCopy = oResourceConfig.getValue("copy")
                .SubjectPrefix = oResourceConfig.getValue("systemMailSubject")
                .SystemFirma = oResourceConfig.getValue("systemMailFirma")
                .SystemFirmaNotifica = oResourceConfig.getValue("systemMailFirmaNotifica")

                Try
                    If oResourceConfig.getValue("systemMailSender") <> "" Then
                        .SystemSender = New MailAddress(oMailSettings.SystemSender.Address, oResourceConfig.getValue("systemMailSender"))
                    Else
                        .SystemSender = oMailSettings.SystemSender
                    End If
                Catch ex As Exception
                    .SystemSender = Nothing
                End Try
            End With
            Me.LoadNotificationMessage(oMailLocalized, oResourceConfig)
            Return oMailLocalized
        End Function

        Private Function GetLocalizedResource(ByVal oLingua As Lingua) As ResourceManager
            Dim oResourceConfig = New ResourceManager

            If oLingua.Codice = "" Then
                oLingua = New Lingua(1, "it-IT")
            End If
            oResourceConfig.UserLanguages = oLingua.Codice
            oResourceConfig.ResourcesName = Me._LanguageSettingsFile
            oResourceConfig.Folder_Level1 = Me._LanguageSettingsPath
            oResourceConfig.setCulture()
            Return oResourceConfig
        End Function

        Private Sub LoadNotificationMessage(ByVal oMailLocalized As MailLocalized, ByVal oResourceConfig As ResourceManager)
            Dim oNotification As NotificationMessage
            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.Hour0)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaAttivazione_day0")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.Hour0)
            Catch ex As Exception

            End Try
            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.Hour12)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaAttivazione_day12")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.Hour12)
            Catch ex As Exception

            End Try
            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.Hour24)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaAttivazione_day24")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.Hour24)
            Catch ex As Exception

            End Try

            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.AreaSubscription)
            '    oNotification.Message = oResourceConfig.getValue("mail_NotificaAreaChiusa_Body")
            '    oNotification.Subject = oResourceConfig.getValue("mail_NotificaAreaChiusa_Subject")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.AreaSubscription)
            'Catch ex As Exception

            'End Try

            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.AreaAcceptSupscription)
            '    oNotification.Message = oResourceConfig.getValue("mail_NotificaAreaAttivazione_Body")
            '    oNotification.Subject = oResourceConfig.getValue("mail_NotificaAreaAccettazione_Subject")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.AreaAcceptSupscription)
            'Catch ex As Exception

            'End Try

            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.NewPostComunita)
                oNotification.Message = oResourceConfig.getValue("bodyForum")
                oNotification.Subject = oResourceConfig.getValue("oggettoForum")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.NewPostComunita)
            Catch ex As Exception

            End Try
            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.NewTopicComunita)
                oNotification.Message = oResourceConfig.getValue("bodyForum.2")
                oNotification.Subject = oResourceConfig.getValue("oggettoForum.2")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.NewTopicComunita)
            Catch ex As Exception

            End Try
            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.NewPostArea)
            '    oNotification.Message = oResourceConfig.getValue("bodyForumArea")
            '    oNotification.Subject = oResourceConfig.getValue("oggettoForumArea")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.NewPostArea)
            'Catch ex As Exception

            'End Try

            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.NewTopicArea)
            '    oNotification.Message = oResourceConfig.getValue("bodyForumArea.2")
            '    oNotification.Subject = oResourceConfig.getValue("oggettoForumArea.2")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.NewTopicArea)
            'Catch ex As Exception

            'End Try

            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.ComunitySubscription)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaChiusa_Body")
                oNotification.Subject = oResourceConfig.getValue("mail_NotificaChiusa_Subject")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.ComunitySubscription)
            Catch ex As Exception

            End Try

            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.CommunityAcceptSupscription)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaAccettazione_Body")
                oNotification.Subject = oResourceConfig.getValue("mail_NotificaAccettazione_Subject")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.CommunityAcceptSupscription)
            Catch ex As Exception

            End Try

            Try
                oNotification = New NotificationMessage(NotificationMessage.NotificationType.ConfermaIscrizionePortale)
                oNotification.Message = oResourceConfig.getValue("mail_NotificaAttivazione_Body")
                oNotification.Subject = oResourceConfig.getValue("mail_NotificaAttivazione_Subject")
                oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.ConfermaIscrizionePortale)
            Catch ex As Exception

            End Try

            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.Login)
            '    oNotification.Message = oResourceConfig.getValue("mail_NotificaLogin_Body")
            '    oNotification.Subject = oResourceConfig.getValue("mail_NotificaLogin_Subject")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.Login)
            'Catch ex As Exception

            'End Try

            'Try
            '    oNotification = New NotificationMessage(NotificationMessage.NotificationType.LoginLDAP)
            '    oNotification.Message = oResourceConfig.getValue("mail_NotificaLogin_Body_ldap")
            '    oNotification.Subject = oResourceConfig.getValue("mail_NotificaLogin_Subject")
            '    oMailLocalized.AddNotification(oNotification, NotificationMessage.NotificationType.LoginLDAP)
            'Catch ex As Exception

            'End Try
        End Sub

#End Region

#Region "Smart Tag"
        Public Function GetSmartTags(ByVal UrlBase As String) As SmartTags
            Dim oSmartTags As New SmartTags

            Try
                Dim oFile As New XmlDocument
                oFile.Load(Me._ConfigurationSmartTags)
                Dim oNode As XmlNode = oFile.GetElementsByTagName("SmartTags")(0)
                oSmartTags = Me.GetSmartTagsFromFile(UrlBase, oNode.ChildNodes)
                Return oSmartTags
            Catch ex As Exception

            End Try
            Return Nothing
        End Function

        Private Function GetSmartTagsFromFile(ByVal UrlBase As String, ByVal oNodes As XmlNodeList) As SmartTags
            Dim oSmartTags As New SmartTags
            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSmartTags
            Else
                For Each oNode As XmlElement In oNodes
                    Select Case oNode.GetAttribute("Class")
                        Case "LatexSmartTag"
                            Try
                                If CBool(IIf(CBool(oNode.GetAttribute("Enabled")) = True, True, False)) Then
                                    Dim Details() As String = oNode.GetAttribute("Details").Split("|"c)
                                    Dim oDpi As Integer = 160
                                    Dim oAddress As String = "", oAddressPopup As String = ""
                                    For Each oDetail As String In Details
                                        If oDetail.Contains("Address=") Then
                                            oAddress = oDetail.Replace("Address=", "")
                                            If oAddress.Contains("#PortalBaseUrl#") Then
                                                oAddress = Replace(oAddress, "#PortalBaseUrl#", UrlBase)
                                            End If
                                        ElseIf oDetail.Contains("AddressPopup=") Then
                                            oAddressPopup = oDetail.Replace("AddressPopup=", "")
                                            If oAddressPopup.Contains("#PortalBaseUrl#") Then
                                                oAddressPopup = Replace(oAddressPopup, "#PortalBaseUrl#", UrlBase)
                                            End If
                                        ElseIf oDetail.Contains("DPI=") Then
                                            Try
                                                oDpi = CInt(oDetail.Replace("DPI=", ""))
                                            Catch ex As Exception
                                                oDpi = 160
                                            End Try

                                        End If
                                    Next
                                    Dim iImage As String = oNode.GetAttribute("Image")
                                    If iImage.Contains("#PortalBaseUrl#") Then
                                        iImage = Replace(iImage, "#PortalBaseUrl#", UrlBase)
                                    End If
                                    Dim oLatexTag As New LatexSmartTag(oNode.GetAttribute("Name"), oNode.GetAttribute("Tag"), oAddress, oDpi, oAddressPopup, iImage)
                                    oLatexTag.DisplayWidth = oNode.GetAttribute("DisplayWidth")
                                    If oLatexTag.DisplayWidth = "" Then
                                        oLatexTag.DisplayWidth = "700"
                                    End If
                                    oLatexTag.DisplayHeight = oNode.GetAttribute("DisplayHeight")
                                    If oLatexTag.DisplayHeight = "" Then
                                        oLatexTag.DisplayHeight = "300"
                                    End If
                                    oSmartTags.Add(oLatexTag)
                                End If

                            Catch ex As Exception

                            End Try
                        Case "YoutubeSmartTag"
                            Try
                                If CBool(IIf(CBool(oNode.GetAttribute("Enabled")) = True, True, False)) Then
                                    Dim oList As ArrayList = New ArrayList

                                    If oNode.HasChildNodes Then
                                        For Each oNodeChild As XmlElement In oNode.ChildNodes(0).ChildNodes
                                            'isRegEx
                                            oList.Add(New ReplaceIt(oNodeChild.GetAttribute("Original"), oNodeChild.GetAttribute("Replaced"), CBool(oNodeChild.GetAttribute("isRegEx"))))
                                        Next
                                    End If
                                    Dim iImage As String = oNode.GetAttribute("Image")
                                    If iImage.Contains("#PortalBaseUrl#") Then
                                        iImage = Replace(iImage, "#PortalBaseUrl#", UrlBase)
                                    End If
                                    Dim oYouTubeSmart As New YoutubeSmartTag(oNode.GetAttribute("Name"), oNode.GetAttribute("Tag"), CInt(oNode.GetAttribute("Width")), CInt(oNode.GetAttribute("Height")), oList, iImage)
                                    oYouTubeSmart.DisplayWidth = oNode.GetAttribute("DisplayWidth")
                                    If oYouTubeSmart.DisplayWidth = "" Then
                                        oYouTubeSmart.DisplayWidth = "700"
                                    End If
                                    oYouTubeSmart.DisplayHeight = oNode.GetAttribute("DisplayHeight")
                                    If oYouTubeSmart.DisplayHeight = "" Then
                                        oYouTubeSmart.DisplayHeight = "300"
                                    End If
                                    oSmartTags.Add(oYouTubeSmart)
                                End If

                            Catch ex As Exception

                            End Try
                        Case "SlideShareSmartTag"
                            Try
                                If CBool(IIf(CBool(oNode.GetAttribute("Enabled")) = True, True, False)) Then
                                    Dim oList As ArrayList = New ArrayList

                                    If oNode.HasChildNodes Then
                                        For Each oNodeChild As XmlElement In oNode.ChildNodes(0).ChildNodes
                                            'isRegEx
                                            oList.Add(New ReplaceIt(oNodeChild.GetAttribute("Original"), oNodeChild.GetAttribute("Replaced"), CBool(oNodeChild.GetAttribute("isRegEx"))))
                                        Next
                                    End If
                                    Dim iImage As String = oNode.GetAttribute("Image")
                                    If iImage.Contains("#PortalBaseUrl#") Then
                                        iImage = Replace(iImage, "#PortalBaseUrl#", UrlBase)
                                    End If
                                    Dim oSlideShareTag As New SlideShareSmartTag(oNode.GetAttribute("Name"), oNode.GetAttribute("Tag"), CInt(oNode.GetAttribute("Width")), CInt(oNode.GetAttribute("Height")), oList, iImage)
                                    oSlideShareTag.DisplayWidth = oNode.GetAttribute("DisplayWidth")
                                    If oSlideShareTag.DisplayWidth = "" Then
                                        oSlideShareTag.DisplayWidth = "700"
                                    End If
                                    oSlideShareTag.DisplayHeight = oNode.GetAttribute("DisplayHeight")
                                    If oSlideShareTag.DisplayHeight = "" Then
                                        oSlideShareTag.DisplayHeight = "300"
                                    End If
                                    oSmartTags.Add(oSlideShareTag)
                                End If

                            Catch ex As Exception

                            End Try
                        Case "DocStocSmartTag"
                            Try
                                If CBool(IIf(CBool(oNode.GetAttribute("Enabled")) = True, True, False)) Then
                                    Dim iImage As String = oNode.GetAttribute("Image")
                                    If iImage.Contains("#PortalBaseUrl#") Then
                                        iImage = Replace(iImage, "#PortalBaseUrl#", UrlBase)
                                    End If
                                    Dim oDocStocTag As New DocStocSmartTag(oNode.GetAttribute("Name"), oNode.GetAttribute("Tag"), CInt(oNode.GetAttribute("Width")), CInt(oNode.GetAttribute("Height")), iImage)
                                    oDocStocTag.DisplayWidth = oNode.GetAttribute("DisplayWidth")
                                    If oDocStocTag.DisplayWidth = "" Then
                                        oDocStocTag.DisplayWidth = "700"
                                    End If
                                    oDocStocTag.DisplayHeight = oNode.GetAttribute("DisplayHeight")
                                    If oDocStocTag.DisplayHeight = "" Then
                                        oDocStocTag.DisplayHeight = "300"
                                    End If
                                    oSmartTags.Add(oDocStocTag)
                                End If

                            Catch ex As Exception

                            End Try
                    End Select
                Next
            End If
            Return oSmartTags
        End Function
#End Region

#Region "Quiz"
        Private Function LoadQuizSettingsFromFile(ByVal FileName As String) As QuizSettings
            Dim oSettings As QuizSettings

            Try
                Dim oFileQuiz As New XmlDocument
                oFileQuiz.Load(FileName)
                Dim oNode As XmlNode = oFileQuiz.GetElementsByTagName("Config")(0)
                oSettings = Me.LoadQuizSettings(oNode.ChildNodes)
                Return oSettings
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Private Function LoadQuizSettings(ByVal oNodes As XmlNodeList) As QuizSettings
            Dim oSettings As New QuizSettings

            Try
                If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                    Return oSettings
                Else
                    For Each oNode As XmlNode In oNodes
                        Select Case oNode.Name
                            Case "Urls"
                                For Each oNodeUrl As XmlNode In oNode.ChildNodes
                                    Dim oURLelement As New URLelement(Helpers.GetXMLattribute(oNodeUrl, "Name"), Helpers.GetXMLattribute(oNodeUrl, "Path"))
                                    oSettings.Urls.Add(oURLelement)
                                Next
                            Case "constants"
                                For Each oNodeCostant As XmlNode In oNode.ChildNodes
                                    Select Case Helpers.GetXMLattribute(oNodeCostant, "Name")
                                        Case "vitaSessione_max"
                                            oSettings.SessionTimeout = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "DefaultScalaValutazione"
                                            oSettings.DefaultScalaValutazione = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "nQuestionsPerPage_Default"
                                            oSettings.QuestionForPage = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "tickMassimo"
                                            oSettings.MaxSessionAliveTick = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "autoSaveTimer"
                                            oSettings.AutoSave = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "maxOvertimeSalvataggio"
                                            oSettings.OvertimeSave = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "lunghezzaOpzioniDropDown"
                                            oSettings.ItemForDropDown = CInt(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "popUpHeight"
                                            oSettings.PopUpHeight = Helpers.GetXMLattribute(oNodeCostant, "Value")
                                        Case "popUpWidth"
                                            oSettings.PopUpWidth = Helpers.GetXMLattribute(oNodeCostant, "Value")
                                        Case "nRighePaginaGridView"
                                            oSettings.RowItemsForPage = CLng(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "ValidatorMaxDouble"
                                            oSettings.MaxDoubleSize = CDbl(Helpers.GetXMLattribute(oNodeCostant, "Value"))
                                        Case "DefaultGroupName"
                                            oSettings.DefaultGroupName = Helpers.GetXMLattribute(oNodeCostant, "Value")
                                    End Select
                                Next
                        End Select
                    Next
                End If

            Catch ex As Exception

            End Try
            Return oSettings
        End Function
#End Region

#Region "Top Bar"
        Private Function GetTopBarSettings(ByVal oNodes As XmlNodeList) As TopBarSetting
            Dim oSettings As New TopBarSetting()

            If IsNothing(oNodes) AndAlso oNodes.Count = 0 Then
                Return oSettings
            Else
                For Each oNode As XmlNode In oNodes
                    Select Case oNode.Name
                        Case "Tools"
                            Try
                                For Each oServiceNode As XmlNode In oNode.ChildNodes
                                    oSettings.Tools.Add(GetTool(oServiceNode))
                                Next
                            Catch ex As Exception
                                oSettings.Tools = New List(Of TBS_Tool)
                            End Try
                        Case "Languages"
                            For Each oSubNode As XmlNode In oNode.ChildNodes
                                Try
                                    Dim idLanguage As Integer
                                    Dim Text As String = ""
                                    For Each LangItem As XmlNode In oSubNode
                                        Select Case LangItem.Name
                                            Case "LanguageId"
                                                idLanguage = CInt(LangItem.InnerText)
                                            Case "Text"
                                                Text = LangItem.InnerText
                                        End Select
                                    Next
                                    If Not oSettings.Languages.ContainsKey(idLanguage) Then
                                        oSettings.Languages.Add(idLanguage, Text)
                                    End If
                                Catch ex As Exception
                                End Try
                            Next

                        Case "LanguageUrl"
                            oSettings.LanguageUrl = GetUrl(oNode) 'oNode.InnerText

                        Case "HomeUrl"
                            oSettings.HomeUrl = GetUrl(oNode) 'oNode.InnerText

                        Case "LogoutUrl"
                            oSettings.LogoutUrl = GetUrl(oNode) 'oNode.InnerText

                        Case "HelpUrl"
                            oSettings.HelpUrl = GetUrl(oNode) 'oNode.InnerText
                        Case "ExtHelps"
                            Try
                                For Each oServiceNode As XmlNode In oNode.ChildNodes
                                    oSettings.ExtHelps.Add(GetTool(oServiceNode))
                                Next
                            Catch ex As Exception
                                oSettings.Tools = New List(Of TBS_Tool)
                            End Try
                        Case "UserUrl"
                            oSettings.UserUrl = GetUrl(oNode) 'oNode.InnerText
                        Case "Order"
                            Dim Orders As List(Of TBS_Order) = New List(Of TBS_Order)
                            Try
                                For Each oOrderNode As XmlNode In oNode.ChildNodes
                                    Dim OrderItem As New TBS_Order()
                                    OrderItem.Name = oOrderNode.InnerText
                                    OrderItem.Order = CInt(oOrderNode.Attributes("order").Value)
                                    Orders.Add(OrderItem)
                                Next
                                oSettings.Order = (From oItm As TBS_Order In Orders Order By oItm.Order Select oItm).ToList()
                            Catch ex As Exception
                                oSettings.Order = New List(Of TBS_Order)
                            End Try
                        Case "LanguageLoginUrl"

                            oSettings.LanguageLoginUrl = GetUrl(oNode) 'oNode.InnerText

                            'Case "LoginCss"
                            '    oSettings.LoginCss = oNode.InnerText
                    End Select
                Next
            End If
            Return oSettings
        End Function

        Private Function GetTool(ByVal oServiceNode As XmlNode) As TBS_Tool

            Dim oTool As New TBS_Tool
            For Each oServiceParameter As XmlNode In oServiceNode.ChildNodes

                Select Case oServiceParameter.Name
                    Case "ServiceId"
                        Try
                            oTool.ServiceId = CInt(oServiceParameter.InnerText)
                        Catch ex As Exception
                            oTool.ServiceId = 0
                        End Try

                    Case "Code"
                        oTool.Code = oServiceParameter.InnerText
                    Case "AutenticationIds"
                        Dim i As Decimal
                        Dim StrArr() As String = oServiceParameter.InnerText.Split(","c)

                        ' Check if string can be converted to decimal equivalent
                        If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then
                            oTool.AutenticationIds = Array.ConvertAll(Of String, Integer) _
                                (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList
                        End If
                    Case "PersonTypeIds"
                        Dim i As Decimal
                        Dim StrArr() As String = oServiceParameter.InnerText.Split(","c)

                        If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then
                            oTool.PersonTypeIds = Array.ConvertAll(Of String, Integer) _
                                (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16))
                        End If
                    Case "Url"
                        oTool.Url = GetUrl(oServiceParameter)

                    Case "LocalNames"
                        For Each oLocName As XmlNode In oServiceParameter.ChildNodes
                            Try
                                Dim Key As Integer = CInt(oLocName.Attributes("LanguageId").Value)
                                Dim Value As String = oLocName.InnerText

                                oTool.LocalNames.Add(Key, Value)

                            Catch ex As Exception
                                Dim err As String = ex.ToString
                                err &= ""
                            End Try

                        Next
                    Case "Enabled"
                        If oServiceParameter.InnerText.ToLower() = "false" Then
                            oTool.Enabled = False
                        Else
                            oTool.Enabled = True
                        End If
                End Select
            Next

            Return oTool
        End Function



        Private Function GetUrl(ByVal oServiceParameter As XmlNode) As Url
            Dim item As New Url() With {.Url = "#", .IsHttps = False, .IsTargetBlank = False}

            If Not String.IsNullOrEmpty(oServiceParameter.InnerText) Then
                item.Url = oServiceParameter.InnerText
            End If
            item.IsHttps = Not IsNothing(oServiceParameter.Attributes("https")) AndAlso oServiceParameter.Attributes("https").Value = "1"
            item.IsTargetBlank = Not IsNothing(oServiceParameter.Attributes("TargetBlank")) AndAlso oServiceParameter.Attributes("https").Value = "1"

            Dim idItems As New List(Of Integer)
            If Not IsNothing(oServiceParameter.Attributes("DisabledTypeIds")) AndAlso Not String.IsNullOrEmpty(oServiceParameter.Attributes("DisabledTypeIds").Value) Then
                idItems = (From i As String In oServiceParameter.Attributes("DisabledTypeIds").Value.Split(","c)
                           Where Not String.IsNullOrEmpty(i) AndAlso IsNumeric(i) Select CInt(i)).ToList
            End If
            item.DisabledTypeIds = idItems.ToArray()
            Return item
        End Function
#End Region

#Region "SkinSettings"
        Private Function GetSkinSettings(ByVal oNodes As XmlNodeList) As SkinSettings
            Dim oSkinSet As New SkinSettings()
            Dim i As Decimal
            For Each oNode As XmlNode In oNodes
                Select Case oNode.Name
                    Case "PersonsTypeIds"
                        Try
                            Dim StrArr() As String = oNode.InnerText.Split(","c)

                            ' Check if string can be converted to decimal equivalent
                            If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then
                                oSkinSet.PersonTypeIds = Array.ConvertAll(Of String, Integer) _
                                    (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList
                            End If
                        Catch ex As Exception

                        End Try
                    Case "PersonsIds"
                        Try
                            Dim StrArr() As String = oNode.InnerText.Split(","c)

                            ' Check if string can be converted to decimal equivalent
                            If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then
                                oSkinSet.PersonsIds = Array.ConvertAll(Of String, Integer) _
                                    (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList
                            End If
                        Catch ex As Exception

                        End Try
                    Case "HeadLogo"
                        oSkinSet.HeadLogo = New SkinSettings.Logo()
                        Try
                            With oSkinSet.HeadLogo
                                If Not IsNothing(oNode.Attributes("alt")) AndAlso Not String.IsNullOrEmpty(oNode.Attributes("alt").Value) Then
                                    .Alt = oNode.Attributes("alt").Value
                                Else
                                    .Alt = ""
                                End If
                                If Not IsNothing(oNode.Attributes("link")) AndAlso Not String.IsNullOrEmpty(oNode.Attributes("link").Value) Then
                                    .Link = oNode.Attributes("link").Value
                                Else
                                    .Link = ""
                                End If
                                .Url = oNode.InnerText
                            End With
                        Catch ex As Exception
                            oSkinSet.HeadLogo = New SkinSettings.Logo()
                        End Try


                    Case "FootText"
                        oSkinSet.FootText = System.Web.HttpUtility.HtmlDecode(oNode.InnerText)

                    Case "FootLogos"
                        Dim Logos As List(Of SkinSettings.Logo) = New List(Of SkinSettings.Logo)

                        oSkinSet.FootLogos = New List(Of SkinSettings.Logo)
                        Try
                            For Each oLogoNode As XmlNode In oNode.ChildNodes
                                Dim oLogo As New SkinSettings.Logo
                                With oLogo
                                    If Not IsNothing(oNode.Attributes("alt")) AndAlso Not String.IsNullOrEmpty(oNode.Attributes("alt").Value) Then
                                        .Alt = oNode.Attributes("alt").Value
                                    Else
                                        .Alt = ""
                                    End If
                                    If Not IsNothing(oNode.Attributes("link")) AndAlso Not String.IsNullOrEmpty(oNode.Attributes("link").Value) Then
                                        .Link = oNode.Attributes("link").Value
                                    Else
                                        .Link = ""
                                    End If
                                    .Url = oLogoNode.InnerText
                                End With
                                oSkinSet.FootLogos.Add(oLogo)
                            Next
                        Catch ex As Exception
                            If IsNothing(oSkinSet.FootLogos) Then
                                oSkinSet.FootLogos = New List(Of SkinSettings.Logo)
                            End If
                        End Try

                    Case "MainCSS"
                        oSkinSet.MainCss = oNode.InnerText
                    Case "IECSS"
                        oSkinSet.IECss = oNode.InnerText
                    Case "AdminCSS"
                        oSkinSet.AdminCss = oNode.InnerText
                    Case "LoginCSS"
                        oSkinSet.LoginCss = oNode.InnerText

                    Case "SkinPhisicalPath"
                        Try
                            oSkinSet.SkinPhisicalPath = oNode.InnerText
                        Catch ex As Exception

                        End Try

                    Case "SkinVirtualPath"
                        Try
                            oSkinSet.SkinVirtualPath = oNode.InnerText
                        Catch ex As Exception

                        End Try
                    Case "TranslatedLoginInfos"


                        'For Each oLocName As XmlNode In oServiceParameter.ChildNodes
                        '    Try
                        '        Dim Key As Integer = oLocName.Attributes("LanguageId").Value
                        '        Dim Value As String = oLocName.InnerText

                        '        oTool.LocalNames.Add(Key, Value)

                        '    Catch ex As Exception
                        '        Dim err As String = ex.ToString
                        '        err &= ""
                        '    End Try

                        'Next

                    Case "LoginInfos"
                        Try
                            oSkinSet.LoginInfos = New List(Of String)

                            For Each oInfoNode As XmlNode In oNode.ChildNodes
                                Dim Info As String = oInfoNode.InnerText
                                If Not Info = "" Then
                                    Info.Replace("&lt;", "<")
                                    Info.Replace("&gt;", ">")
                                    oSkinSet.LoginInfos.Add(Info)
                                End If
                            Next

                        Catch ex As Exception
                            oSkinSet.LoginInfos = New List(Of String)
                        End Try

                    Case "PortalSettings"
                        'oSkinSet.PortalSetting = SkinSettings.PortalOrganizationElements.None

                        If Not IsNothing(oNode.ChildNodes) AndAlso oNode.ChildNodes.Count() > 0 Then
                            For Each oSetNode As XmlNode In oNode.ChildNodes
                                Select Case oSetNode.Name.ToLower()
                                    Case "none"
                                        oSkinSet.PortalSetting = SkinSettings.PortalOrganizationElements.None
                                        Exit For
                                    Case "all"
                                        oSkinSet.PortalSetting = SkinSettings.PortalOrganizationElements.ALL
                                        Exit For
                                    Case "css"
                                        oSkinSet.PortalSetting = oSkinSet.PortalSetting Or SkinSettings.PortalOrganizationElements.Css
                                    Case "mainlogo"
                                        oSkinSet.PortalSetting = oSkinSet.PortalSetting Or SkinSettings.PortalOrganizationElements.MainLogo
                                    Case "footer"
                                        oSkinSet.PortalSetting = oSkinSet.PortalSetting Or SkinSettings.PortalOrganizationElements.Footer
                                End Select
                            Next
                        End If

                End Select
            Next
            Return oSkinSet

        End Function
        'Case " AutenticationIds"
        'Dim i As Integer
        'Dim StrArr() As String = oServiceParameter.InnerText.Split(",")

        '' Check if string can be converted to decimal equivalent
        'If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then
        '    oTool.AutenticationIds = Array.ConvertAll(Of String, Integer) _
        '        (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16))
        'End If
#End Region

#Region "DocTemplate"
        Private Function GetDocTempalteSettings(ByVal oNodes As XmlNodeList) As DocTemplateSettings
            Dim oTemplSet As New DocTemplateSettings()

            oTemplSet.FooterFontSize = 0

            For Each oNode As XmlNode In oNodes
                Select Case oNode.Name
                    Case "BasePath"
                        Try
                            oTemplSet.BasePath = oNode.InnerText
                        Catch ex As Exception

                        End Try
                    Case "BaseUrl"
                        Try
                            oTemplSet.BaseUrl = oNode.InnerText
                        Catch ex As Exception

                        End Try
                    Case "DefaultTemplatePath"
                        Try
                            oTemplSet.DefaultTemplatePath = oNode.InnerText
                        Catch ex As Exception

                        End Try

                    Case "BaseTemporaryFolder"
                        Try
                            oTemplSet.BaseTemporaryFolder = oNode.InnerText
                        Catch ex As Exception

                        End Try

                    Case "FooterFontSize"
                        Try
                            oTemplSet.FooterFontSize = System.Convert.ToInt32(oNode.InnerText)
                        Catch ex As Exception
                            oTemplSet.FooterFontSize = 0
                        End Try
                End Select
            Next

            Return oTemplSet
        End Function
#End Region

#Region "Webconferencing"
        Private Function GetWebConferencingSettings(ByVal oNodes As XmlNodeList) As WebConferencingSettings
            Dim WCS As New WebConferencingSettings

            If IsNothing(oNodes) OrElse oNodes.Count() <= 0 Then
                Return WCS
            End If

            For Each oNode As XmlNode In oNodes
                Select Case oNode.Name
                    Case "CanRecord"
                        Try
                            WCS.CanRecord = CBool(oNode.InnerText)
                        Catch ex As Exception
                            WCS.CanRecord = False
                        End Try
                    Case "RecordExpDay"
                        Try
                            WCS.RecordExpDay = CInt(oNode.InnerText)
                        Catch ex As Exception
                            WCS.RecordExpDay = 0
                        End Try
                    Case "CanTrace"
                        Try
                            WCS.CanTrace = CBool(oNode.InnerText)
                        Catch ex As Exception
                            WCS.CanTrace = False
                        End Try
                    Case "TraceExpDay"
                        Try
                            WCS.TraceExpDay = CInt(oNode.InnerText)
                        Catch ex As Exception
                            WCS.TraceExpDay = 0
                        End Try
                    Case "UseDataBase"
                        Try
                            WCS.UseDataBase = CBool(oNode.InnerText)
                        Catch ex As Exception
                            WCS.UseDataBase = True
                        End Try
                    Case "System"
                        Select Case oNode.InnerText
                            Case "eWorks"
                                WCS.System = WebConferencingSettings.WBsystem.eWorks
                            Case "OpenMeeting"
                                WCS.System = WebConferencingSettings.WBsystem.OpenMeeting
                            Case Else
                                WCS.System = WebConferencingSettings.WBsystem.none
                        End Select
                    Case "UseProxy"
                        Try
                            WCS.UseProxy = CBool(oNode.InnerText)
                        Catch ex As Exception
                            WCS.UseProxy = False
                        End Try
                    Case "ProxyUrl"
                        WCS.ProxyUrl = oNode.InnerText
                        If String.IsNullOrEmpty(WCS.ProxyUrl) Then
                            WCS.UseProxy = False
                        End If

                    Case "eWSettings"
                        If WCS.System = WebConferencingSettings.WBsystem.eWorks AndAlso oNode.ChildNodes.Count > 0 Then
                            '    WCS.System = WebConferencingSettings.WBsystem.none
                            'Else
                            WCS.eWSettings = New WebConferencingSettings.eWorksSettings
                            WCS.OMSettings = Nothing
                            WCS.eWSettings.Version = ""
                            For Each innerNode As XmlNode In oNode.ChildNodes
                                Select Case innerNode.Name
                                    Case "BaseUrl"
                                        WCS.eWSettings.BaseUrl = innerNode.InnerText
                                    Case "MainUserId"
                                        WCS.eWSettings.MainUserId = innerNode.InnerText
                                    Case "MainUserPwd"
                                        WCS.eWSettings.MainUserPwd = innerNode.InnerText
                                    Case "MaxUrlChar"
                                        Try
                                            WCS.eWSettings.MaxUrlChar = CInt(innerNode.InnerText)
                                        Catch ex As Exception
                                            WCS.eWSettings.MaxUrlChar = 1500
                                        End Try
                                    Case "Version"
                                        WCS.eWSettings.Version = innerNode.InnerText
                                End Select
                            Next
                        End If

                    Case "OpenMeetingSettings"
                        If WCS.System = WebConferencingSettings.WBsystem.OpenMeeting AndAlso oNode.ChildNodes.Count > 0 Then
                            '    WCS.System = WebConferencingSettings.WBsystem.none
                            'Else
                            WCS.eWSettings = Nothing
                            WCS.OMSettings = New WebConferencingSettings.OpenMeetingSettings

                            For Each innerNode As XmlNode In oNode.ChildNodes
                                Select Case innerNode.Name
                                    Case "BaseUrl"
                                        WCS.OMSettings.BaseUrl = innerNode.InnerText
                                    Case "MainUserLogin"
                                        WCS.OMSettings.MainUserLogin = innerNode.InnerText
                                    Case "MainUserPwd"
                                        WCS.OMSettings.MainUserPwd = innerNode.InnerText
                                End Select
                            Next
                        End If
                    Case "NameWithId"
                        WCS.NameWithId = True
                End Select


            Next

            Return WCS
        End Function
#End Region

#Region "Questionnaire"
        Private Function GetQuestionnaireSettings(ByVal oNodes As XmlNodeList) As QuestionnaireSettings
            Dim QS As New QuestionnaireSettings()

            If IsNothing(oNodes) OrElse oNodes.Count() <= 0 Then
                Return QS
            End If

            Dim i As Decimal

            For Each oNode As XmlNode In oNodes
                Select Case oNode.Name
                    Case "ShowDescription"
                        QS.ShowDescription = True

                    Case "ShowInvitationUrls"

                        If (oNode.Attributes("enabled").Value.ToLower() = "true") Then
                            QS.ShowUrlsSettings.Enabled = True
                        End If


                        If QS.ShowUrlsSettings.Enabled AndAlso oNode.ChildNodes.Count > 0 Then

                            For Each oCN As XmlNode In oNode.ChildNodes
                                Select Case oCN.Name
                                    Case "all"
                                        QS.ShowUrlsSettings.IsForAll = True
                                        Exit For
                                    Case "UserTypeId"
                                        Try
                                            Dim StrArr() As String = oNode.InnerText.Split(","c)

                                            ' Check if string can be converted to decimal equivalent
                                            If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then

                                                QS.ShowUrlsSettings.UsersTypeId = Array.ConvertAll(Of String, Integer) _
                                                    (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList
                                            End If
                                        Catch ex As Exception

                                        End Try

                                    Case "UsersId"
                                        Try
                                            Dim StrArr() As String = oNode.InnerText.Split(","c)

                                            ' Check if string can be converted to decimal equivalent
                                            If StrArr.All(Function(number) Decimal.TryParse(number, i)) Then

                                                QS.ShowUrlsSettings.UsersId = Array.ConvertAll(Of String, Integer) _
                                                    (StrArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList()
                                            End If
                                        Catch ex As Exception

                                        End Try

                                    Case "Community"
                                        'oService.Name = oNode.Attributes("Name").Value
                                        Try
                                            Dim ComArr() As String = oCN.Attributes("id").Value.Split(","c)
                                            Dim RolesArr() As String = oCN.Attributes("roles").Value.Split(","c)

                                            If Not IsNothing(ComArr) AndAlso Not IsNothing(RolesArr) Then
                                                Dim ComIds As List(Of Integer) = Array.ConvertAll(Of String, Integer) _
                                                    (ComArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList()
                                                Dim RolesIds As List(Of Integer) = Array.ConvertAll(Of String, Integer) _
                                                    (RolesArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList()

                                                If Not IsNothing(RolesIds) AndAlso RolesIds.Count() > 0 Then
                                                    For Each comId As Integer In ComIds
                                                        If Not QS.ShowUrlsSettings.EnabledCommunityRoles.ContainsKey(comId) Then
                                                            QS.ShowUrlsSettings.EnabledCommunityRoles.Add(comId, RolesIds)
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Catch ex As Exception
                                        End Try

                                    Case "CommunityType"
                                        'oService.Name = oNode.Attributes("Name").Value
                                        Try
                                            Dim ComTArr() As String = oCN.Attributes("id").Value.Split(","c)
                                            Dim RolesArr() As String = oCN.Attributes("roles").Value.Split(","c)

                                            If Not IsNothing(ComTArr) AndAlso Not IsNothing(RolesArr) Then
                                                Dim ComTIds As List(Of Integer) = Array.ConvertAll(Of String, Integer) _
                                                    (ComTArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList()
                                                Dim RolesIds As List(Of Integer) = Array.ConvertAll(Of String, Integer) _
                                                    (RolesArr, New Converter(Of String, Integer)(AddressOf Convert.ToInt16)).ToList()

                                                If Not IsNothing(RolesIds) AndAlso RolesIds.Count() > 0 Then
                                                    For Each CTId As Integer In ComTIds
                                                        If Not QS.ShowUrlsSettings.EnabledCommunityTypeRoles.ContainsKey(CTId) Then
                                                            QS.ShowUrlsSettings.EnabledCommunityTypeRoles.Add(CTId, RolesIds)
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Catch ex As Exception
                                        End Try

                                End Select
                            Next
                        End If
                End Select
            Next

            Return QS
        End Function
#End Region


        Private Function GetFeaturesSettings(ByVal oNodes As XmlNodeList) As FeaturesSettings
            Dim Features As New FeaturesSettings()

            If IsNothing(oNodes) OrElse oNodes.Count() <= 0 Then
                Return Features
            End If

            Dim i As Integer

            For Each oNode As XmlNode In oNodes
                Select Case oNode.Name
                    Case "Call"
                        If Not IsNothing(oNode.ChildNodes) AndAlso oNode.ChildNodes.Count > 0 Then
                            For Each oChild As XmlNode In oNode.ChildNodes
                                Select Case oChild.Name
                                    Case "AdvanceEvaluation"
                                        If oChild.InnerText = "True" Then
                                            Features.CallAdvanceEvaluation = True
                                        End If
                                End Select
                            Next
                        End If
                    Case "Edupath"
                        If Not IsNothing(oNode.ChildNodes) AndAlso oNode.ChildNodes.Count > 0 Then
                            For Each oChild As XmlNode In oNode.ChildNodes
                                Select Case oChild.Name
                                    Case "Multilanguage"
                                        If oChild.InnerText = "True" Then
                                            Features.EdupathMultilanguage = True
                                        End If
                                    Case "ShowCode"
                                        If oChild.InnerText = "True" Then
                                            Features.EdupathShowCode = True
                                        End If
                                End Select
                            Next

                        End If
                    Case "Users"
                        If Not IsNothing(oNode.ChildNodes) AndAlso oNode.ChildNodes.Count > 0 Then
                            For Each oChild As XmlNode In oNode.ChildNodes
                                Select Case oChild.Name
                                    Case "ShowMansion"
                                        If oChild.InnerText = "True" Then
                                            Features.UsersShowMansion = True
                                        End If
                                End Select
                            Next
                        End If
                    Case "DelaySubscription"
                        If Not IsNothing(oNode.ChildNodes) AndAlso oNode.ChildNodes.Count > 0 Then

                            Features.DelaySubconf.Validity = New List(Of dsValidity)()

                            For Each oChild As XmlNode In oNode.ChildNodes
                                Select Case oChild.Name
                                    Case "AdminTypeId"
                                        Try

                                            Features.DelaySubconf.AdminTypeIds = New List(Of Integer)

                                            For Each value As String In (oChild.InnerText).Split(",")
                                                If IsNumeric(value) Then
                                                    Features.DelaySubconf.AdminTypeIds.Add(CInt(value))
                                                End If
                                            Next

                                        Catch ex As Exception

                                        End Try

                                    Case "Validities"

                                        For Each oChildPeriod As XmlNode In oChild.ChildNodes
                                            Select Case oChildPeriod.Name
                                                Case "Period"
                                                    Dim Validity As New dsValidity()

                                                    For Each oChildper As XmlNode In oChildPeriod.ChildNodes
                                                        Select Case oChildper.Name
                                                            Case "Names"
                                                                For Each oChildNames As XmlNode In oChildper.ChildNodes
                                                                    Dim LangCode As String = ""
                                                                    If Not IsNothing(oChildNames.Attributes("lang")) Then
                                                                        LangCode = oChildNames.Attributes("lang").Value
                                                                    End If

                                                                    Dim Name As String = oChildNames.InnerText

                                                                    If Not String.IsNullOrWhiteSpace(LangCode) AndAlso Not String.IsNullOrWhiteSpace(Name) Then
                                                                        If String.IsNullOrWhiteSpace(Validity.DefaultName) Then
                                                                            Validity.DefaultName = Name
                                                                        End If

                                                                        If Not Validity.Names.ContainsKey(LangCode) Then
                                                                            Validity.Names.Add(LangCode, Name)
                                                                        End If
                                                                    End If
                                                                Next

                                                            Case "value"
                                                                Dim duration As Integer = -1
                                                                Try
                                                                    duration = System.Convert.ToInt32(oChildper.InnerText)
                                                                Catch ex As Exception

                                                                End Try
                                                                Validity.Value = duration
                                                        End Select

                                                        Features.DelaySubconf.Validity.Add(Validity)
                                                    Next
                                            End Select
                                        Next
                                End Select
                            Next
                        End If

                End Select
            Next

            Return Features
        End Function

    End Class
End Namespace