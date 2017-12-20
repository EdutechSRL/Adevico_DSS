Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports System.Linq

Public Class UC_InLineResources
    Inherits BaseUserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub InitializeControl(ByVal items As List(Of dtoResource), Optional ByVal loadNoResource As Boolean = False)
        If Not IsNothing(items) AndAlso items.Any() Then
            If Not IsNothing(items) AndAlso items.Any() Then
                SPNresourceslist.Attributes.Add("title", String.Join(", ", items.Select(Function(i) i.LongName).ToList()))
            End If
        ElseIf loadNoResource Then
            items = New List(Of dtoResource)
            items.Add(New dtoResource() With {.IdResource = 0, .LongName = Resource.getValue("NoResource.LongName"), .ShortName = Resource.getValue("NoResource.ShortName")})
        End If
        RPTresources.DataSource = items
        RPTresources.DataBind()
    End Sub
End Class