Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports System.Linq

Public Class UC_InLineProjectRoles
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

    Public Sub InitializeControl(ByVal items As List(Of dtoProjectRole))
        If Not IsNothing(items) AndAlso items.Any() Then
            If Not IsNothing(items) AndAlso items.Any() Then
                SPNroleslist.Attributes.Add("title", String.Join(", ", items.Select(Function(i) i.LongName).ToList()))
            End If
        End If
        RPTroles.DataSource = items
        RPTroles.DataBind()
    End Sub
End Class