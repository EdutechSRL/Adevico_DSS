Imports COL_BusinessLogic_v2.Localizzazione

Namespace AdevicoHelpers

    Public Class Helper

        Public Shared Function GetResourceConfig(ByVal LinguaCode As String) As ResourceManager
            Dim oResourceConfig = New ResourceManager

            If LinguaCode = "" Then
                LinguaCode = "it-IT"
            End If
            oResourceConfig.UserLanguages = LinguaCode
            oResourceConfig.ResourcesName = System.Configuration.ConfigurationSettings.AppSettings("configFile")
            oResourceConfig.Folder_Level1 = "Root"

            oResourceConfig.setCulture()
            Return oResourceConfig
        End Function


    End Class
End Namespace