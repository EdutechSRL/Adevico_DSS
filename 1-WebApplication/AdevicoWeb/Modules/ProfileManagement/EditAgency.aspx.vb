Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication
Imports System.Linq

Public Class EditAgency
    Inherits PageBase
    Implements IViewEditAgency


#Region "Context"
    Private _Presenter As EditAgencyPresenter
    Private ReadOnly Property CurrentPresenter() As EditAgencyPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditAgencyPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadedIdAgency As Long Implements IViewEditAgency.PreloadedIdAgency
        Get
            If IsNumeric(Request.QueryString("IdAgency")) Then
                Return CLng(Request.QueryString("IdAgency"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private Property AllowEdit As Boolean Implements IViewEditAgency.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            LNBsaveBottom.Visible = value
            LNBsaveTop.Visible = value
        End Set
    End Property
    Private Property IdAgency As Long Implements IViewEditAgency.IdAgency
        Get
            Return ViewStateOrDefault("IdAgency", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAgency") = value
        End Set
    End Property
    Private WriteOnly Property AllowManagement As Boolean Implements IViewEditAgency.AllowManagement
        Set(value As Boolean)
            HYPbackToManagement.Visible = value
            HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.ManagementAgenciesWithFilters
            HYPbackToManagementBottom.Visible = value
            HYPbackToManagementBottom.NavigateUrl = BaseUrl & RootObject.ManagementAgenciesWithFilters
        End Set
    End Property

    Public ReadOnly Property CurrentAgency As dtoAgency Implements IViewEditAgency.CurrentAgency
        Get
            Dim agency As New dtoAgency
            With agency
                .Id = IdAgency
                .AlwaysAvailable = Me.CBXalwaysAvailable.Checked
                .Description = Me.TXBdescription.Text
                .ExternalCode = Me.TXBexternalCode.Text
                .IsDefault = Me.CBXdefault.Checked
                .IsEditable = True
                .IsEmpty = Me.CBXempty.Checked
                .Name = Me.TXBname.Text
                .NationalCode = Me.TXBnationalCode.Text
                .TaxCode = Me.TXBtaxCode.Text

                If Not .AlwaysAvailable AndAlso CBLorganizations.SelectedIndex > -1 Then
                    .Organizations = (From item As ListItem In CBLorganizations.Items Where item.Selected Select New dtoOrganizationAffiliation() With {.Id = CInt(item.Value)}).ToList()
                Else
                    Me.CBXalwaysAvailable.Checked = True
                    .AlwaysAvailable = True
                End If
            End With
            Return agency
        End Get
    End Property

    Private Property IsEditable As Boolean
        Get
            Return ViewStateOrDefault("IsEditable", True)
        End Get
        Set(value As Boolean)
            ViewState("IsEditable") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(PreloadedIdAgency)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AgencyManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = IIf(PreloadedIdAgency = 0, .getValue("serviceTitleAddAgency"), .getValue("serviceTitleEditAgency"))
            Me.Master.ServiceNopermission = .getValue("serviceEditAgencyNopermission")
            .setHyperLink(HYPbackToManagement, True, True)
            HYPbackToManagementBottom.Text = HYPbackToManagement.Text
            HYPbackToManagementBottom.ToolTip = HYPbackToManagement.ToolTip
            .setLinkButton(LNBsaveBottom, True, True)
            .setLinkButton(LNBsaveTop, True, True)

            .setLabel(LBagencyName_t)
            .setLabel(LBagencyDescription_t)
            .setLabel(LBagencyTaxCode_t)
            .setLabel(LBagencyExternalCode_t)
            .setLabel(LBagencyNationalCode_t)
            .setLabel(LBalwaysAvailable_t)
            .setLabel(LBavailableForOrganizations_t)
            .setLabel(LBdefault_t)
            .setLabel(LBdempty_t)
            .setLabel(LBexternalCodeDuplicate)
            .setLabel(LBtaxCodeDuplicate)
            .setLabel(LBnationalCodeDuplicate)
            .setLabel(LBagencyNameDuplicated)

        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub InitializeForm(organizations As List(Of dtoAvailableAffiliation))
        Me.MLVagency.SetActiveView(VIWedit)

        Me.CBLorganizations.Items.Clear()
        For Each dto As dtoAvailableAffiliation In organizations
            Dim oListitem As New ListItem
            oListitem.Enabled = dto.IsEditable
            oListitem.Text = dto.Organization.Name
            oListitem.Value = dto.Organization.Id
            Me.CBLorganizations.Items.Add(oListitem)
        Next
        Me.SPNorganizations.Visible = False
        Me.CBXalwaysAvailable.Checked = True
        Me.CBXdefault.Checked = False
        Me.CBXempty.Checked = False
        Me.TXBdescription.Text = ""
        Me.TXBexternalCode.Text = ""
        Me.TXBname.Text = ""
        Me.TXBnationalCode.Text = ""
        Me.TXBtaxCode.Text = ""
    End Sub
    Private Sub InitializeForAdd(organizations As List(Of dtoAvailableAffiliation)) Implements IViewEditAgency.InitializeForAdd
        InitializeForm(organizations)
    End Sub

    Private Sub LoadAgency(agency As dtoAgency, organizations As List(Of dtoAvailableAffiliation)) Implements IViewEditAgency.LoadAgency
        InitializeForm(organizations)
        With agency
            Me.CBXalwaysAvailable.Checked = .AlwaysAvailable

            If .AlwaysAvailable Then
                Me.CBXdefault.Checked = .IsDefault
                Me.CBXempty.Checked = .IsEmpty
            Else
                Me.CBXdefault.Checked = organizations.Any() AndAlso organizations.Where(Function(o) o.IsDefault).Any()
                Me.CBXempty.Checked = organizations.Any() AndAlso organizations.Where(Function(o) o.IsEmpty).Any()
            End If

            Me.TXBdescription.Text = .Description
            Me.TXBexternalCode.Text = .ExternalCode
            Me.TXBname.Text = .Name
            Me.TXBnationalCode.Text = .NationalCode
            Me.TXBtaxCode.Text = .TaxCode

            If Not .AlwaysAvailable Then
                Me.SPNorganizations.Visible = True
                For Each item As ListItem In CBLorganizations.Items
                    If (.Organizations.Where(Function(o) o.Id = CInt(item.Value)).Any()) Then
                        item.Selected = True
                    Else
                        item.Selected = False
                    End If
                Next
            End If

            IsEditable = .IsEditable
            Me.CBXalwaysAvailable.Enabled = .IsEditable
            Me.CBXempty.Enabled = Not .IsEditable OrElse (.IsEditable AndAlso Not .AlwaysAvailable)
            '     Me.CBXdefault.Enabled = Not .IsEditable OrElse (.IsEditable AndAlso Not .AlwaysAvailable)

            Me.TXBexternalCode.ReadOnly = Not .IsEditable
            Me.TXBnationalCode.ReadOnly = Not .IsEditable
            Me.TXBtaxCode.ReadOnly = Not .IsEditable
        End With
    End Sub
    Private Sub LoadAgencyName(displayName As String) Implements IViewEditAgency.LoadAgencyName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleEditNamedAgency"), displayName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitleEditNamedAgency"), displayName)
    End Sub
    Private Sub DisplayAgencyUnknown() Implements IViewEditAgency.DisplayAgencyUnknown
        Me.MLVagency.SetActiveView(VIWdefault)
        Me.LBmessage.Text = Resource.getValue("EditDisplayAgencyUnknown")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEditAgency.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditAgency(IIf(IdAgency > 0, IdAgency, PreloadedIdAgency))
        webPost.Redirect(dto)
    End Sub

    Private Sub GotoManagement() Implements IViewEditAgency.GotoManagement
        PageUtility.RedirectToUrl(RootObject.ManagementAgenciesWithFilters)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewEditAgency.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub
    Private Sub DisplayErrorSaving() Implements IViewEditAgency.DisplayErrorSaving
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayErrorSaving"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub LoadErrors(errors As List(Of AgencyError)) Implements IViewEditAgency.LoadErrors
        Me.LBexternalCodeDuplicate.Visible = errors.Contains(AgencyError.externalCodeDuplicate)
        Me.LBtaxCodeDuplicate.Visible = errors.Contains(AgencyError.taxCodeDuplicate)
        Me.LBnationalCodeDuplicate.Visible = errors.Contains(AgencyError.nationalCodeDuplicate)
        Me.LBagencyNameDuplicated.Visible = errors.Contains(AgencyError.nameDuplicate)
        DisplayErrorSaving()
    End Sub
#End Region

    Private Sub LNBsaveBottom_Click(sender As Object, e As System.EventArgs) Handles LNBsaveBottom.Click, LNBsaveTop.Click
        CTRLmessages.Visible = False
        Me.CurrentPresenter.Save(Me.CurrentAgency)
    End Sub


    Private Sub CBXalwaysAvailable_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXalwaysAvailable.CheckedChanged
        Me.SPNorganizations.Visible = Not Me.CBXalwaysAvailable.Checked
        If IsEditable AndAlso Me.CBXalwaysAvailable.Checked Then
            Me.CBXempty.Checked = False
        End If
        Me.CBXempty.Enabled = Not IsEditable OrElse (IsEditable AndAlso Not Me.CBXalwaysAvailable.Checked)

    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class