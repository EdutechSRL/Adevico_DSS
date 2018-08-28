Imports System.Xml
Imports System.Reflection
Imports System.Configuration

Namespace Configuration.Facility
    Public Class Helpers

        Public Shared Function GetNested(ByVal WorkObj As Object, ByVal PropertiesList As String) As Object
            If WorkObj IsNot Nothing Then
                Dim WorkingType As Type = WorkObj.GetType
                Dim Properties() As String
                Dim WorkingObject As Object
                Properties = PropertiesList.Split(".")
                Try
                    WorkingObject = WorkObj

                    For i As Integer = 0 To Properties.Length - 1
                        Dim y As PropertyInfo = WorkingType.GetProperty(Properties(i))
                        WorkingObject = y.GetValue(WorkingObject, Nothing)
                        WorkingType = y.PropertyType
                    Next

                    Return WorkingObject
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If

        End Function

        Public Shared Function GetXMLattribute(ByVal oXmlNode As XmlNode, ByVal attribute As String) As String
            Try
                Return CType(oXmlNode, XmlElement).GetAttribute(attribute)
            Catch ex As Exception
                Return ""
            End Try
            Return ""
        End Function

        Public Shared Function AppConfigSetting(ByVal SectionName As String) As String
            Return System.Configuration.ConfigurationManager.AppSettings(SectionName)
        End Function
    End Class
End Namespace