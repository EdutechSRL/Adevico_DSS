Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class UC_DashboardBaseSettings
    Inherits DBbaseControl

#Region "Internal"
    Public Property IdSettingsCommunity As Integer
        Get
            Return ViewStateOrDefault("IdSettingsCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdSettingsCommunity") = value
        End Set
    End Property
    Public Property IdSettings As Long
        Get
            Return ViewStateOrDefault("IdSettings", 0)
        End Get
        Set(value As Long)
            ViewState("IdSettings") = value
        End Set
    End Property
    Public Property DashboardType As DashboardType
        Get
            Return ViewStateOrDefault("DashboardType", DashboardType.Portal)
        End Get
        Set(value As DashboardType)
            ViewState("DashboardType") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTbaseSettingsTitle)
            .setLiteral(LTsettingsName)
            .setLiteral(LTsettingsDescription)
            .setLiteral(LTmandatoryInfos)
            If LTmandatoryInfos.Text.Contains("{0}") Then
                LTmandatoryInfos.Text = String.Format(LTmandatoryInfos.Text, LTmandatoryTemplate.Text)
            End If
        

            .setLabel(LBbaseSettingsForAll_t)
            .setLabel(LBbaseSettingsForAll)
            '.setLabel(LBbaseSettingsAssignments)
            If Not Page.IsPostBack Then
                SPNassignmentSelectAll.Attributes.Add("title", Resource.getValue("SPNassignmentSelectAll.ToolTip"))
                SPNassignmentSelectNone.Attributes.Add("title", Resource.getValue("SPNassignmentSelectNone.ToolTip"))
            End If
        End With
      
    End Sub
#End Region
#Region "Internal"
    Public Sub InitializeControl(settings As liteDashboardSettings, items As List(Of lm.Comol.Core.DomainModel.dtoTranslatedProfileType), selected As List(Of Integer))
        InitializeControl(settings)
        SLBassignments.DataSource = items
        SLBassignments.DataTextField = "Name"
        SLBassignments.DataValueField = "Id"
        SLBassignments.DataBind()
        SetSelectedItems(settings, selected)
    End Sub
    Public Sub InitializeControl(settings As liteDashboardSettings, items As List(Of lm.Comol.Core.DomainModel.dtoTranslatedRoleType), selected As List(Of Integer))
        InitializeControl(settings)

        SLBassignments.DataSource = items
        SLBassignments.DataTextField = "Name"
        SLBassignments.DataValueField = "Id"
        SLBassignments.DataBind()
        SetSelectedItems(settings, selected)
    End Sub
    Public Sub InitializeControl(settings As liteDashboardSettings)
        SLBassignments.Attributes.Add("data-placeholder", Resource.getValue("Assignments.data-placeholder.DashboardType." & settings.Type.ToString))
        SLBassignments.Disabled = settings.ForAll
        LTbaseSettingsAssignmentsTitle.Text = Resource.getValue("LTbaseSettingsAssignmentsTitle.DashboardType." & settings.Type.ToString)
        LBbaseSettingsAssignments.Text = Resource.getValue("LBbaseSettingsAssignments.DashboardType." & settings.Type.ToString)
        
        IdSettings = settings.Id
        IdSettingsCommunity = settings.IdCommunity
        DashboardType = settings.Type
        TXBsettingsName.Text = settings.Name
        TXBsettingsDescription.Text = settings.Description

        CBXisForAll.Checked = settings.ForAll
        CBXisForAll.Enabled = Not (settings.ForAll AndAlso settings.Active)
        DVassignments.Visible = CBXisForAll.Enabled AndAlso settings.Type <> lm.Comol.Core.Dashboard.Domain.DashboardType.AllCommunities
    End Sub
    Private Sub SetSelectedItems(settings As liteDashboardSettings, selected As List(Of Integer))
        If Not settings.ForAll AndAlso selected.Count > 0 Then
            For Each idItem As Integer In selected
                Dim oListItem As ListItem = SLBassignments.Items.FindByValue(idItem)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
        End If
    End Sub
    Public Function GetSettings() As dtoBaseDashboardSettings
        Dim dto As New dtoBaseDashboardSettings
        With dto
            .Description = TXBsettingsDescription.Text
            .ForAll = CBXisForAll.Checked
            .Name = TXBsettingsName.Text
            .Type = DashboardType
            .Id = IdSettings
            .IdCommunity = IdSettingsCommunity
            If Not .ForAll Then
                For Each item As ListItem In SLBassignments.Items
                    If item.Selected Then
                        Dim aItem As New dtoDashboardAssignment()
                        aItem.Type = IIf(DashboardType = lm.Comol.Core.Dashboard.Domain.DashboardType.Portal, DashboardAssignmentType.ProfileType, DashboardAssignmentType.RoleType)
                        aItem.IdProfileType = IIf(DashboardType = lm.Comol.Core.Dashboard.Domain.DashboardType.Portal, CInt(item.Value), 0)
                        aItem.IdRole = IIf(DashboardType <> lm.Comol.Core.Dashboard.Domain.DashboardType.Portal, CInt(item.Value), 0)
                        .Assignments.Add(aItem)
                    End If
                Next
            End If
        End With

        Return dto
    End Function
    Public Sub EnableSelector(enable As Boolean)
        SLBassignments.Disabled = Not enable
    End Sub
#End Region
End Class