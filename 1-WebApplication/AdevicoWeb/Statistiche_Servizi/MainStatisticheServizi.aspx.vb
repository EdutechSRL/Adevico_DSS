Imports lm.Comol.Core.Statistiche

Partial Public Class MainStatisticheServizi
    Inherits PageBase
    Implements IViewMainStat

	Private _GenericStatUCCom As IcommunityContainer

	Private _GenericStatUcUser As IpersonalContainer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.TBSstat.SelectedIndex = 0
            Me.BindListaServizi()
        End If
        Me.LoadUC()
    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("Pg_MainStatisticheServizi", "StatisticheService")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBTitolo)
            .setLabel(Lbl_Service_t)

            .setLinkButton(Lnb_Cerca, True, True, False, False)

            For Each TabT As Telerik.Web.UI.RadTab In TBSstat.GetAllTabs
                TabT.Text = .getValue("TBSstat." & TabT.Value)
            Next
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

        End Get
    End Property

    Public Sub BindControl() Implements lm.Comol.Core.Statistiche.IViewMainStat.BindControl

    End Sub

    Public Sub BindService(ByVal ServiceList As System.Collections.Generic.IList(Of Object)) Implements lm.Comol.Core.Statistiche.IViewMainStat.BindService

    End Sub

    Public ReadOnly Property CommunityID() As Integer Implements lm.Comol.Core.Statistiche.IViewMainStat.CommunityID
        Get

        End Get
    End Property

    Public Property IsGlobal() As Boolean Implements lm.Comol.Core.Statistiche.IViewMainStat.IsGlobal
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Sub LoadControl1(ByVal ControlPath As String) Implements lm.Comol.Core.Statistiche.IViewMainStat.LoadControl

    End Sub

    Public ReadOnly Property ServiceID() As Integer Implements lm.Comol.Core.Statistiche.IViewMainStat.ServiceID
        Get

        End Get
    End Property

    Public Property ShowGlobal() As Boolean Implements lm.Comol.Core.Statistiche.IViewMainStat.ShowGlobal
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Private Sub LoadUC()
        Dim Service As Services = Me.DDL_Service.SelectedValue
        Dim IsUser As Boolean = Me.TBSstat.SelectedTab.Value

        Dim strCtlPath As String = Me.GetPath(Service, IsUser)

        Me.PHStat.Controls.Clear()

        If IsUser Then
            Try
                Me._GenericStatUcUser = LoadControl(strCtlPath)
                Me.PHStat.Controls.Add(Me._GenericStatUcUser)
                Me._GenericStatUcUser.Bind(MyBase.ComunitaCorrenteID, MyBase.UtenteCorrente.ID)
            Catch ex As Exception
                Me._GenericStatUcUser = LoadControl("./UC/UC_Void_StatPers.ascx")
                Me.PHStat.Controls.Add(Me._GenericStatUcUser)
                Me._GenericStatUcUser.Bind(MyBase.ComunitaCorrenteID, MyBase.UtenteCorrente.ID)
            End Try
            
        Else
            Try
                Me._GenericStatUCCom = LoadControl(strCtlPath)
                Me.PHStat.Controls.Add(Me._GenericStatUCCom)
                Me._GenericStatUCCom.Bind(MyBase.ComunitaCorrenteID)
            Catch ex As Exception
                Me._GenericStatUCCom = LoadControl("./UC/UC_Void_StatCom.ascx")
                Me.PHStat.Controls.Add(Me._GenericStatUCCom)
                Me._GenericStatUCCom.Bind(MyBase.ComunitaCorrenteID)
            End Try
            
        End If
        'myControl = LoadControl(strCtlPath)


        'Me.PHHeader.Controls.Add(LoadControl(myControl.GetType, Nothing)) 'objParams))
        'Me.RealHeader = DirectCast(Me.PHHeader.Controls(0), IviewHeaderComunita)

    End Sub
    Private Enum Services As Integer
        Scorm = 1
        File = 2
    End Enum

    Private Function GetPath(ByVal Service As Services, ByVal IsUser As Boolean) As String

        Dim CTRLList As List(Of ServiceObject) = StatConfigurator.GetFullList("E:\ProgettiWeb\LM_Comol\Src\Comunita_OnLine\Statistiche_Servizi\StatService.xml")

        Dim CtrlPath As String = "./UC/"

        Dim Query = (From oCtrl In CTRLList Where oCtrl.Name = Service.ToString Select oCtrl).First()
        Dim Ctrl As ServiceObject
        Try
            Ctrl = Query
        Catch ex As Exception

        End Try

        If Not IsNothing(Ctrl) Then
            If IsUser Then
                CtrlPath &= Ctrl.Uc_Personal
            Else
                CtrlPath &= Ctrl._UC_Community
            End If
        End If

        Return CtrlPath
    End Function

    Private Sub BindListaServizi()
        Me.DDL_Service.Items.Clear()

        For Each Item As KeyValue In StatConfigurator.GetListItem("E:\ProgettiWeb\LM_Comol\Src\Comunita_OnLine\Statistiche_Servizi\StatService.xml")
            'Me.DDL_Service.Items.Add(New ListItem(Item.Value, Item.Key))
            Me.DDL_Service.Items.Add(New ListItem(MyBase.Resource.getValue("Service." & Item.Value), Item.Key))
        Next

        If Me.DDL_Service.Items.Count = 0 Then
            Me.DDL_Service.Items.Add(New ListItem("No Item", 0))
        End If

        Me.DDL_Service.DataBind()
    End Sub

End Class