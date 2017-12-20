Imports lm.Comol.Modules.Standard.ProjectManagement.Domain

Public Class UC_SelectResources
    Inherits BaseUserControl

#Region "Internal"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource

            SLBresources.Attributes.Add("data-placeholder", Resource.getValue("SelectResources.data-placeholder"))
        End With
    End Sub
#End Region

    Public Sub InitializeControl(resources As List(Of dtoResource), selectedResources As List(Of dtoResource), labelText As String, dataPlaceholder As String, Optional ByVal selectFirst As Boolean = False)
        SLBresources.DataSource = resources
        SLBresources.DataTextField = "LongName"
        SLBresources.DataValueField = "IdResource"
        SLBresources.DataBind()
        If selectedResources.Any() Then
            For Each resource As dtoResource In selectedResources
                Dim oListItem As ListItem = SLBresources.Items.FindByValue(resource.IdResource)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        ElseIf selectFirst AndAlso SLBresources.Items.Count > 0 Then
            SLBresources.Items(0).Selected = True
        End If
        InitializeTranslation(labelText, dataPlaceholder)
    End Sub
    Public Sub InitializeControl(resources As List(Of dtoProjectResource), Optional labelText As String = "", Optional dataPlaceholder As String = "", Optional selectedResources As List(Of dtoProjectResource) = Nothing, Optional selectFirst As Boolean = False)
        SLBresources.DataSource = resources
        SLBresources.DataTextField = "LongName"
        SLBresources.DataValueField = "IdResource"
        SLBresources.DataBind()
        If Not IsNothing(selectedResources) AndAlso selectedResources.Any() Then
            For Each resource As dtoResource In selectedResources
                Dim oListItem As ListItem = SLBresources.Items.FindByValue(resource.IdResource)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        ElseIf selectFirst AndAlso SLBresources.Items.Count > 0 Then
            SLBresources.Items(0).Selected = True
        End If
        InitializeTranslation(labelText, dataPlaceholder)
    End Sub
    Public Sub InitializeControl(disable As Boolean, rItems As List(Of dtoResource), selectedResources As List(Of Long), Optional labelText As String = "", Optional dataPlaceholder As String = "", Optional selectFirst As Boolean = False)
        SLBresources.DataSource = rItems
        SLBresources.DataTextField = "LongName"
        SLBresources.DataValueField = "IdResource"
        SLBresources.DataBind()
        If Not IsNothing(selectedResources) AndAlso selectedResources.Any() Then
            For Each idResource As Long In selectedResources
                Dim oListItem As ListItem = SLBresources.Items.FindByValue(idResource)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        ElseIf selectFirst AndAlso SLBresources.Items.Count > 0 Then
            SLBresources.Items(0).Selected = True
        End If
        SLBresources.Disabled = disable
        InitializeTranslation(labelText, dataPlaceholder)
    End Sub



    Private Sub InitializeTranslation(Optional ByVal labelText As String = "", Optional ByVal dataPlaceholder As String = "")
        If Not String.IsNullOrEmpty(labelText) Then
            LBselectResources_t.Text = labelText
        Else
            Resource.setLabel(LBselectResources_t)
        End If
        If Not String.IsNullOrEmpty(dataPlaceholder) Then
            SLBresources.Attributes.Add("data-placeholder", dataPlaceholder)
        Else
            SLBresources.Attributes.Add("data-placeholder", Resource.getValue("SelectResources.data-placeholder"))
        End If
    End Sub

    Public Function GetSelectedIdResources() As List(Of Long)
        If SLBresources.SelectedIndex > -1 Then
            Return (From i As ListItem In SLBresources.Items Where i.Selected Select CLng(i.Value)).ToList
        Else
            Return New List(Of Long)
        End If
    End Function

    Public Sub ChangeResourceSelection(ByVal idResource As Long, ByVal selected As Boolean)
        Dim oItem As ListItem = SLBresources.Items.FindByValue(idResource)
        If Not IsNothing(oItem) Then
            oItem.Selected = selected
        End If
    End Sub

End Class