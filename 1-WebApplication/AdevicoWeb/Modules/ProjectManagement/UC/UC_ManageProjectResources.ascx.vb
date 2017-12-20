Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain

Public Class UC_ManageProjectResources
    Inherits BaseControl
    Implements IViewManageProjectResources


#Region "Context"
    Private _Presenter As ManageProjectResourcesPresenter
    Private ReadOnly Property CurrentPresenter() As ManageProjectResourcesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManageProjectResourcesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Protected ReadOnly Property MaxDisplayUsers As Integer Implements IViewManageProjectResources.MaxDisplayUsers
        Get
            Dim maxItems As Integer = 5

            If Not String.IsNullOrEmpty(LTdataMax.Text) AndAlso IsNumeric(LTdataMax.Text) Then
                Integer.TryParse(LTdataMax.Text, maxItems)
            End If
            Return maxItems
        End Get

    End Property
    Public WriteOnly Property SetMaxDisplayUsers As String Implements IViewManageProjectResources.SetMaxDisplayUsers
        Set(value As String)
            Dim maxItems As Integer = 5

            If Not String.IsNullOrEmpty(value) AndAlso IsNumeric(value) Then
                LTdataMax.Text = value
            Else
                LTdataMax.Text = maxItems
            End If
        End Set

    End Property
    Public Property DisplayDescription As Boolean Implements IViewManageProjectResources.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewManageProjectResources.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewManageProjectResources.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property RaiseDeleteEvent As Boolean
        Get
            Return ViewStateOrDefault("RaiseDeleteEvent", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseDeleteEvent") = value
        End Set
    End Property
    Public Event VirtualDeleteResource(ByVal idResource As Long, ByVal resourceName As String)
    Public Event SaveSettings(resources As List(Of dtoProjectResource))
    Public Event CloseWindow()
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTlongName_t)
            .setLiteral(LTshortName_t)
            .setLiteral(LTprojectRole_t)
            .setLiteral(LTprojectVisibility)
            .setLiteral(LTaction_t)

            .setButton(BTNsaveProjectResources, True)
            .setButton(BTNcancelSaveProjectResources, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(resources As List(Of dtoProjectResource), Optional ByVal description As String = "") Implements IViewManageProjectResources.InitializeControl
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LBdescription.Text = description
        End If
        LoadResources(resources)
    End Sub
    Public Sub InitializeControl(idProject As Long, allowEdit As Boolean, unknownUser As String, Optional ByVal description As String = "") Implements IViewManageProjectResources.InitializeControl
        CurrentPresenter.InitView(idProject, allowEdit, unknownUser)
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LBdescription.Text = description
        End If
    End Sub
    Public Function GetResources() As List(Of dtoProjectResource) Implements IViewManageProjectResources.GetResources
        Dim items As New List(Of dtoProjectResource)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTusers.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim dto As New dtoProjectResource
            Dim oLiteral As Literal = row.FindControl("LTidRole")


            With dto
                oLiteral = row.FindControl("LTidResource")
                .IdResource = CLng(oLiteral.Text)

                oLiteral = row.FindControl("LTidPerson")
                .IdPerson = CInt(oLiteral.Text)

                Dim oCheckBox As HtmlInputCheckBox = row.FindControl("CBmanager")
                If CInt(oLiteral.Text) <> CInt(ActivityRole.ProjectOwner) Then
                    .ProjectRole = IIf(oCheckBox.Checked, ActivityRole.Manager, ActivityRole.Resource)
                    oCheckBox = row.FindControl("CBvisibility")
                    .Visibility = IIf(oCheckBox.Checked, ProjectVisibility.Full, ProjectVisibility.InvolvedTasks)
                Else
                    .ProjectRole = ActivityRole.ProjectOwner
                    .Visibility = ProjectVisibility.Full
                End If

                Dim oTextBox As TextBox = row.FindControl("TXBlongName")
                .LongName = oTextBox.Text

                oTextBox = row.FindControl("TXBshortName")
                .ShortName = oTextBox.Text
            End With
            items.Add(dto)

        Next
        Return items
    End Function

    Private Sub LoadResources(resources As List(Of dtoProjectResource)) Implements IViewManageProjectResources.LoadResources
        RPTusers.DataSource = resources
        RPTusers.DataBind()
    End Sub
#End Region

#Region "Internal"
    Protected Function ContainerCssClass(t As ResourceType) As String
        Select Case t
            Case ResourceType.Removed
                Return ResourceType.Removed.ToString.ToLower & " " & ResourceType.Internal.ToString.ToLower
            Case Else
                Return t.ToString.ToLower
        End Select
    End Function
    Protected Sub RPTusers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTusers.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoProjectResource = DirectCast(e.Item.DataItem, dtoProjectResource)

            Dim oLiteral As Literal = e.Item.FindControl("LTlongName")
            Dim oTextBox As TextBox = e.Item.FindControl("TXBlongName")
            oLiteral.Visible = Not dto.AllowEdit OrElse dto.isOwner OrElse dto.IdPerson > 0
            oTextBox.Visible = dto.AllowEdit AndAlso Not dto.isOwner AndAlso dto.IdPerson = 0

            oLiteral = e.Item.FindControl("LTshortName")
            oTextBox = e.Item.FindControl("TXBshortName")
            oLiteral.Visible = Not dto.AllowEdit
            oTextBox.Visible = dto.AllowEdit
            If dto.DisplayErrors <> EditingErrors.None Then
                oTextBox.CssClass = "duplicate"
            Else
                oTextBox.CssClass = ""
            End If

            Dim oCheckBox As HtmlInputCheckBox = e.Item.FindControl("CBmanager")
            oCheckBox.Checked = (dto.ProjectRole = ActivityRole.Manager OrElse dto.ProjectRole = ActivityRole.ProjectOwner) AndAlso dto.ResourceType = ResourceType.Internal
            oCheckBox.Disabled = Not dto.AllowEdit OrElse dto.isOwner OrElse dto.ResourceType <> ResourceType.Internal

            Dim oLabel As Label = e.Item.FindControl("LBprojectRole")
            If dto.isOwner Then
                oCheckBox.Visible = False
                oLabel.Text = Resource.getValue("LBprojectRole.Owner")
            Else
                Resource.setLabel(oLabel)
            End If

            oCheckBox = e.Item.FindControl("CBvisibility")
            oCheckBox.Checked = (dto.Visibility = ProjectVisibility.Full)
            oCheckBox.Disabled = Not dto.AllowEdit OrElse dto.isOwner
            'If dto.isOwner OrElse dto.Role = TaskRole.Manager Then
            '    oCheckBox.Visible = False
            'End If
            oLabel = e.Item.FindControl("LBprojectVisibility")
            Resource.setLabel(oLabel)

            Dim oButton As Button = e.Item.FindControl("BTNdelete")
            Resource.setButton(oButton, True)
            oButton.Visible = dto.AllowEdit AndAlso dto.AllowDelete

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim tableRow As HtmlTableRow = e.Item.FindControl("TRfooter")
            If RPTusers.Items.Count > MaxDisplayUsers Then
                tableRow.Visible = True
                Dim oLabel As Label = e.Item.FindControl("LBshowextraUserItems")
                Resource.setLabel(oLabel)

                oLabel = e.Item.FindControl("LBhideextraUserItems")
                Resource.setLabel(oLabel)
            Else
                tableRow.Visible = False
            End If
        End If
    End Sub
    Protected Sub RPTusers_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTusers.ItemCommand
        If e.CommandName = "virtualdelete" Then
            If RaiseDeleteEvent Then
                Dim oTextbox As TextBox = e.Item.FindControl("TXBlongName")
                Dim name As String = ""
                If Not IsNothing(oTextbox) Then
                    name = oTextbox.Text
                    If String.IsNullOrEmpty(name) Then
                        oTextbox = e.Item.FindControl("TXBshortName")
                        name = oTextbox.Text
                    End If
                End If

                RaiseEvent VirtualDeleteResource(CLng(e.CommandArgument), name)

            End If
        End If
    End Sub
    Private Sub BTNcancelSaveProjectResources_Click(sender As Object, e As System.EventArgs) Handles BTNcancelSaveProjectResources.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub

    Private Sub BTNsaveProjectResources_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectResources.Click
        If RaiseCommandEvents Then
            RaiseEvent SaveSettings(GetResources())
        End If
    End Sub
#End Region

   
End Class