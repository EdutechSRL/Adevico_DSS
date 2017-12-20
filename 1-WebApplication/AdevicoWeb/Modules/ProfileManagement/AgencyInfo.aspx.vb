Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class AgencyInfo
    Inherits PageBase
    Implements IViewAgencyInfo


#Region "Implements"
    Private Property IdAgency As Long Implements IViewAgencyInfo.IdAgency
        Get
            Return ViewStateOrDefault("IdAgency", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAgency") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdAgency As Long Implements IViewAgencyInfo.PreloadedIdAgency
        Get
            If IsNumeric(Request.QueryString("IdAgency")) Then
                Return CLng(Request.QueryString("IdAgency"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
#End Region

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As AgencyInfoPresenter
    Private ReadOnly Property CurrentPresenter() As AgencyInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AgencyInfoPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
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
            Me.Master.ServiceTitle = .getValue("serviceAgencyInfo")
            Me.Master.ServiceNopermission = .getValue("serviceAgencyInfoNopermission")
            .setLabel(LBinfoMessage)
            .setLabel(LBagencyName_t)
            .setLabel(LBagencyDescription_t)
            .setLabel(LBagencyTaxCode_t)
            .setLabel(LBagencyExternalCode_t)
            .setLabel(LBagencyNationalCode_t)
            .setLabel(LBalwaysAvailable_t)
            .setLabel(LBorganizations_t)
            .setLabel(LBisEditable_t)
            .setLabel(LBdefault_t)
            .setLabel(LBdempty_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub LoadAgencyInfo(agency As dtoAgency) Implements IViewAgencyInfo.LoadAgencyInfo
        Me.LBagencyName.Text = agency.Name
        Me.LBagencyDescription.Text = agency.Description
        If Not String.IsNullOrEmpty(agency.TaxCode) Then
            SPNtaxCode.Visible = True
            LBtaxCode.Text = agency.TaxCode
        End If
        If Not String.IsNullOrEmpty(agency.ExternalCode) Then
            SPNexternalCode.Visible = True
            LBexternalCode.Text = agency.ExternalCode
        End If
        If Not String.IsNullOrEmpty(agency.NationalCode) Then
            SPNnationalCode.Visible = True
            LBnationalCode.Text = agency.NationalCode
        End If
        SPNalwaysAvailable.Visible = agency.AlwaysAvailable
        LBalwaysAvailable.Text = Resource.getValue("isEditable." & agency.AlwaysAvailable.ToString)
        If Not agency.AlwaysAvailable Then
            RPTorganizations.DataSource = agency.Organizations
            RPTorganizations.DataBind()
            Me.SPNorganizations.Visible = True
        End If
        LBisEditable.Text = Resource.getValue("isEditable." & agency.IsEditable.ToString)
        SPNdefault.Visible = agency.IsDefault
        LBdefault.Text = Resource.getValue("isEditable." & agency.IsDefault.ToString)
        SPNempty.Visible = agency.IsEmpty
        LBdempty.Text = Resource.getValue("isEditable." & agency.IsEmpty.ToString)
        Me.MLVcontent.SetActiveView(VIWagency)
    End Sub

    Private Sub DisplayNoPermission() Implements IViewAgencyInfo.DisplayNoPermission
        Me.Master.ShowNoPermission = True
    End Sub
    Private Sub DisplayAgencyUnknown() Implements IViewAgencyInfo.DisplayAgencyUnknown
        Me.MLVcontent.SetActiveView(VIWmessages)
        Me.LBinfoMessage.Text = Resource.getValue("DisplayAgencyUnknown")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewAgencyInfo.DisplaySessionTimeout
        Me.MLVcontent.SetActiveView(VIWmessages)
        Me.LBinfoMessage.Text = Resource.getValue("DisplaySessionTimeout")
    End Sub
#End Region


    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class