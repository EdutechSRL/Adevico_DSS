Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports System.Linq

Public Class UC_IMagencyOrganizations
    Inherits BaseControl
    Implements IViewAgencyOrganizations

#Region "Context"
    Private _Presenter As AgencyOrganizationsPresenter
    Private ReadOnly Property CurrentPresenter() As AgencyOrganizationsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AgencyOrganizationsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property HasAvailableOrganizations As Boolean Implements IViewAgencyOrganizations.HasAvailableOrganizations
        Get
            Return (Me.CBLorganizations.Items.Count > 0)
        End Get
    End Property
    Public Property AvailableForAll As Boolean Implements IViewAgencyOrganizations.AvailableForAll
        Get
            Return Me.CBXalwaysAvailable.Checked
        End Get
        Set(value As Boolean)
            Me.CBXalwaysAvailable.Checked = value
           
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewAgencyOrganizations.isValid
        Get
            Return Me.CBXalwaysAvailable.Checked OrElse Me.CBLorganizations.SelectedIndex >= 0
        End Get
    End Property
    Public Property isInitialized As Boolean Implements IViewAgencyOrganizations.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property AvailableOrganizations As List(Of Integer) Implements IViewAgencyOrganizations.AvailableOrganizations
        Get
            If Me.CBLorganizations.Items.Count = 0 Then
                Return New List(Of Integer)
            Else
                Return (From i As ListItem In Me.CBLorganizations.Items Select CInt(i.Value)).ToList
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedOrganizations As Dictionary(Of Integer, String) Implements IViewAgencyOrganizations.SelectedOrganizations
        Get
            Dim items As New Dictionary(Of Integer, String)

            For Each l As ListItem In (From t As ListItem In CBLorganizations.Items Where t.Selected Select t).ToList()
                items.Add(CInt(l.Value), l.Text)
            Next

            Return items
        End Get
    End Property
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
        MyBase.SetCulture("pg_AgencyManagement", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBimportAlwaysAvailable_t)
            .setLabel(LBimportAlwaysAvailable)
            .setLabel(LBimportAvailableForOrganizations_t)
            LBemptyMessage.Text = Resource.getValue("DisplaySessionTimeout.AgencyOrganizations")
        End With
    End Sub
#End Region

    Public Sub InitializeControl() Implements IViewAgencyOrganizations.InitializeControl
        Me.CBLorganizations.Enabled = False
        Me.CurrentPresenter.InitView()
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewAgencyOrganizations.DisplaySessionTimeout
        MLVcontrolData.SetActiveView(VIWempty)
    End Sub

    Public Sub LoadItems(items As Dictionary(Of Integer, String)) Implements IViewAgencyOrganizations.LoadItems
        MLVcontrolData.SetActiveView(VIWselectOrganizations)
        Me.CBLorganizations.DataSource = items
        Me.CBLorganizations.DataTextField = "Value"
        Me.CBLorganizations.DataValueField = "Key"
        Me.CBLorganizations.DataBind()
    End Sub

    Private Sub CBXalwaysAvailable_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXalwaysAvailable.CheckedChanged
        Me.CBLorganizations.Enabled = Not CBXalwaysAvailable.Checked
        If CBXalwaysAvailable.Checked Then
            Me.CBLorganizations.SelectedIndex = -1
        End If
    End Sub
End Class