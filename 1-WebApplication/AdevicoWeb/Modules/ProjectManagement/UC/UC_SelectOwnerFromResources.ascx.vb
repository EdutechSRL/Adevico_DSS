Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_SelectOwnerFromResources
    Inherits BaseControl
    Implements IViewSelectOwnerFromResources


#Region "Implements"

#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewSelectOwnerFromResources.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewSelectOwnerFromResources.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewSelectOwnerFromResources.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
#End Region

#End Region

#Region "Property"
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property

    Public Event CloseWindow()
    Public Event SelectedResource(ByVal idResource As Long)
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNcancelSelectResourceOwner, True)
            .setButton(BTNselectResourceOwner, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(resources As List(Of dtoProjectResource), Optional description As String = "") Implements IViewSelectOwnerFromResources.InitializeControl
        Me.LBdescription.Text = description
        If DisplayDescription AndAlso String.IsNullOrEmpty(description) Then
            DisplayDescription = False
        End If
        SLBownerFromResources.DataSource = resources
        SLBownerFromResources.DataTextField = "LongName"
        SLBownerFromResources.DataValueField = "IdResource"
        SLBownerFromResources.DataBind()
        SLBownerFromResources.Attributes.Add("data-placeholder", Resource.getValue("DefaulOwnerResources.data-placeholder"))
    End Sub
    Public Function GetSelectedIdResource() As Long Implements IViewSelectOwnerFromResources.GetSelectedIdResource
        If SLBownerFromResources.SelectedIndex < 0 Then
            Return 0
        Else
            Return CLng(SLBownerFromResources.Value)
        End If
    End Function
#End Region

    Private Sub BTNcancelSelectResourceOwner_Click(sender As Object, e As System.EventArgs) Handles BTNcancelSelectResourceOwner.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
    Private Sub BTNselectResourceOwner_Click(sender As Object, e As System.EventArgs) Handles BTNselectResourceOwner.Click
        If RaiseCommandEvents Then
            RaiseEvent SelectedResource(GetSelectedIdResource())
        End If
    End Sub
End Class