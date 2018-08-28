Imports System.Xml
Imports Comol.Entity.Configuration
Imports Comol.Entity.Configuration.Components

Namespace Configuration.Facility
    Public Class ConfigurationErrorSettings
        Inherits ObjectBase

        Private _ConfigurationPath As String
        Private _ConfigurationFilePath As String

        ReadOnly Property ConfigurationFilePath() As String
            Get
                Return _ConfigurationFilePath
            End Get
        End Property
        Sub New()
            _ConfigurationPath = Helpers.AppConfigSetting("ConfigurationPath")
            _ConfigurationFilePath = _ConfigurationPath & Helpers.AppConfigSetting("ConfigurationFile")
        End Sub

        Public Function GetSettingsFromConfiguration() As NotificationErrorSettings
            Dim oSettings As New NotificationErrorSettings
            Dim oFileconfigurazione As New XmlDocument

            Try
                oFileconfigurazione.Load(_ConfigurationFilePath)
                oSettings = Me.GetNotificationErrorSettings(oFileconfigurazione.GetElementsByTagName("NotificationErrorService")(0).ChildNodes)
            Catch ex As Exception
                oSettings = New NotificationErrorSettings
            End Try
            Return oSettings
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
                                oSettings.Enabled = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "ComolUniqueID"
                            Try
                                oSettings.ComolUniqueID = oNode.InnerText
                            Catch ex As Exception

                            End Try
                        Case "ErrorsType"
                            oSettings.ErrorsType.AddRange((From o As XmlNode In oNode.ChildNodes Select New dtoErrorType(o.Attributes("Enabled").Value, o.Attributes("ErrorType").Value, o.Attributes("PersistTo").Value)).ToList)
                    End Select
                Next

            End If
            Return oSettings
        End Function

    End Class
End Namespace