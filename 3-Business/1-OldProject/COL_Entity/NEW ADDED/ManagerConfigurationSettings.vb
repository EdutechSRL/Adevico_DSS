Imports System.Xml
Imports Comol.Entity.Configuration
Imports Comol.Entity.Configuration.Components
Imports System.Web

Namespace Configuration.Facility
    Public Class ManagerConfigurationSettings
        Inherits ObjectBase

        Public Shared Function GetInstance() As NotificationErrorSettings
            Dim cacheKey As String = CachePolicy.NotificationErrorSettings
            Dim oSettings As NotificationErrorSettings

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oManager As New ConfigurationErrorSettings

                oSettings = oManager.GetSettingsFromConfiguration()
                ObjectBase.Cache.Insert(cacheKey, oSettings, New Caching.CacheDependency(oManager.ConfigurationFilePath), DateTime.MaxValue, TimeSpan.Zero)
            Else
                oSettings = CType(ObjectBase.Cache(cacheKey), NotificationErrorSettings)
            End If
            Return oSettings
        End Function

    End Class
End Namespace