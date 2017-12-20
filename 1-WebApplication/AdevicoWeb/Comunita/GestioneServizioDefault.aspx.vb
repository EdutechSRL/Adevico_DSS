Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona

Public Class GestioneServizioDefault
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Protected WithEvents LKBaggiorna As System.Web.UI.WebControls.LinkButton

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label

#Region "Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello Principale"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents LBpaginaDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLpagineDefault As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oServizio As New UCServices.Services_AmministraComunita
        Dim oServizioCover As New UCServices.Services_Cover

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If


        If Not SessioneScaduta() Then
            If Page.IsPostBack = False Then

                Dim oPersona As COL_Persona
                Dim forAdmin As Boolean = False
                Dim CMNT_ID As Integer

                oPersona = Session("objPersona")
                Try
                    If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                        CMNT_ID = Session("idComunita_forAdmin")
                        forAdmin = True
                    Else
                        CMNT_ID = Session("idComunita")
                    End If
                Catch ex As Exception
                    CMNT_ID = Session("idComunita")
                End Try

                Try
                    If forAdmin Then
                        oServizio.PermessiAssociati = oPersona.GetPermessiForServizioForAdmin(CMNT_ID, oServizio.Codex, False, oServizio.GetPermission_Admin, oServizio.GetPermission_Moderate)
                    Else
                        oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                    End If
                Catch ex As Exception
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End Try

                If oServizio.Admin Or oServizio.Moderate Then
                    Bind_PagineDefault()
                    BindDefaultSelezionato()
                    Me.PNLcontenuto.Visible = True
                    Me.PNLpermessi.Visible = False
                    Me.LKBaggiorna.Visible = True
                Else
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                    Me.LKBaggiorna.Visible = False
                End If

            End If
        Else
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
            Return True
        Else
            Try
                Dim CMNT_ID As Integer = 0
                Try
                    If Session("AdminForChange") = True Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("idComunita")
                    End If
                Catch ex As Exception
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex2 As Exception
                        CMNT_ID = 0
                    End Try

                End Try

                If CMNT_ID <= 0 Then
                    Me.ExitToLimbo()
                    Return True
                End If
            Catch ex As Exception
                Me.ExitToLimbo()
                Return True
            End Try
        End If
    End Function

    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.Response.Redirect("./EntrataComunita.aspx", True)
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_GestioneServizioDefault"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBTitolo)
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
            .setLabel(LBNopermessi)
            .setLabel(LBavviso)
            .setLinkButton(LKBaggiorna, True, True)
            .setLabel(Me.LBpaginaDefault_t)

        End With
    End Sub
#End Region

    Private Sub BindDefaultSelezionato()
        'seleziono la page di default
        Dim oComunita As New COL_Comunita
        Dim percorso, codice As String
        Dim DFLP_id As Integer
        oComunita.GetDefaultPage(Session("idComunita"), percorso, codice, DFLP_id)
        If DFLP_id > 0 Then
            Me.DDLpagineDefault.SelectedValue = DFLP_id
        End If

    End Sub
    Private Sub Bind_PagineDefault()
        Dim oComunita As New COL_Comunita

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        Me.DDLpagineDefault.Items.Clear()
        Try
            Dim i, totale As Integer
            Dim oDataset As New DataSet
            oDataset = oComunita.ElencoPagineDefault(Session("idComunita"), Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(i)

                If oRow.Item("SRVZ_Attivato") = False Or oRow.Item("SRVC_isAbilitato") = False Then
                    oRow("DFLP_Nome") = oRow("DFLP_Nome") & " (" & Me.oResource.getValue("disattivato") & ")"
                End If
            Next
            If totale > 0 Then
                Me.DDLpagineDefault.DataSource = oDataset
                Me.DDLpagineDefault.DataTextField = "DFLP_Nome"
                Me.DDLpagineDefault.DataValueField = "DFLP_ID"
                Me.DDLpagineDefault.DataBind()
            Else
                ' aggiungere riga "nessuno"
            End If

        Catch ex As Exception
            ' aggiungere riga "nessuno"
        End Try
    End Sub

    Private Sub LKBaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiorna.Click
        'salva il servizio default
        Try
            Dim oComunita As New COL_Comunita
            oComunita.SetDefaultPage(Session("idcomunita"), DDLpagineDefault.SelectedValue)
            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Modules.Standard.Menu.Domain.CacheKeys.RenderCommunity(Session("idcomunita")))
        Catch ex As Exception

        End Try
    End Sub


    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class