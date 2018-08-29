Imports System.Xml
Imports Comol.Entity.Configuration
Imports Comol.Entity.Configuration.Components

Namespace Comol.Manager
	Public Class ManagerConfiguration
		Inherits ObjectBase

        'Public Shared UrlBase As String
        'Public Shared SystemSmartTags As SmartTags = GetSmartTags(UrlBase)
        'Public Shared SystemSettings As ComolSettings = ManagerConfiguration.GetInstance

        Public Shared Function GetInstance(Optional ByVal ForceLoad As Boolean = False) As ComolSettings
            Dim cacheKey As String = CachePolicy.GlobalSettings
            Dim oSettings As ComolSettings

            If ObjectBase.Cache(cacheKey) Is Nothing OrElse ForceLoad Then
                Dim oManager As New ManagerConfigurationSettings

                oSettings = oManager.GetSettingsFromConfiguration()
                ObjectBase.Cache.Insert(cacheKey, oSettings, New Caching.CacheDependency(oManager.ConfigurationFilePath), DateTime.MaxValue, TimeSpan.Zero)
            Else
                oSettings = CType(ObjectBase.Cache(cacheKey), ComolSettings)
            End If
            Return oSettings
        End Function

        Public Shared Function GetMailLocalized(ByVal oLingua As Lingua) As MailLocalized
			Dim oManager As New ManagerConfigurationSettings
			Dim oMailLocalized As MailLocalized
			Dim oSettings As ComolSettings = ManagerConfiguration.GetInstance
			If IsNothing(oLingua) Then
				oLingua = oSettings.DefaultLanguage
			End If

			Dim cacheKey As String = CachePolicy.LocalizedMailSettings(oLingua.Codice)
			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oMailLocalized = oManager.GetLocalizedMailSettings(oSettings.Mail, oLingua)
				ObjectBase.Cache.Insert(cacheKey, oMailLocalized, New Caching.CacheDependency(oManager.ConfigurationFilePath), DateTime.MaxValue, TimeSpan.Zero)
			Else
				oMailLocalized = CType(ObjectBase.Cache(cacheKey), MailLocalized)
			End If
			Return oMailLocalized
        End Function
        Public Shared Function GetSmartTags(ByVal UrlBase As String) As SmartTags
            Dim oSmartTags As New SmartTags
            Dim oManager As New ManagerConfigurationSettings

            Dim cacheKey As String = CachePolicy.SmartTags
            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oSmartTags = oManager.GetSmartTags(UrlBase)
                ObjectBase.Cache.Insert(cacheKey, oSmartTags, New Caching.CacheDependency(oManager.ConfigurationSmartTags), DateTime.MaxValue, TimeSpan.Zero)
            Else
                oSmartTags = CType(ObjectBase.Cache(cacheKey), SmartTags)
            End If
            Return oSmartTags

        End Function
	End Class
End Namespace