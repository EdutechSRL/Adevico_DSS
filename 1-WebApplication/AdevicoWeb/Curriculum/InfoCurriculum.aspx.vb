Imports Comunita_OnLine.ModuloGenerale
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class InfoCurriculum
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Protected WithEvents PNLcurriculum As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBclose As System.Web.UI.WebControls.LinkButton
    Protected WithEvents HDN_Studente_ID As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Struttura"
    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Private Enum TabIndex
        Dati = 0
        Competenze = 1
        Formazione = 2
        Lingua = 3
        Esperienze = 4
    End Enum
    Protected WithEvents TBRdati As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRformazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlingua As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBResperienze As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcompetenze As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_infoDatiCurriculum
    Protected WithEvents CTRLformazione As Comunita_OnLine.UC_infoFormazione
    Protected WithEvents CTRLlingua As Comunita_OnLine.UC_infoConoscenzaLingua
    Protected WithEvents CTRLlavoro As Comunita_OnLine.UC_infoEsperienzeLavorative
    Protected WithEvents CTRLcompetenze As Comunita_OnLine.UC_infoCompetenze
#End Region

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"), "page_CurriculumIscrittiComunita")
        End If

        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
            If oPersona.Id <> 0 Then

            End If
        Catch ex As Exception
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            oPersona = Nothing
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "this.window.close();" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "');</script>")
            Response.End()
            Exit Sub
            'Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            'Me.Response.Redirect("./../index.aspx", True)
        End Try

        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()

            Try
                If Me.Request.QueryString("Studente_ID") > 0 Then
                    Me.HDN_Studente_ID.Value = Me.Request.QueryString("Studente_ID")
                    Bind_Dati()
                Else
                    Response.Write("<script language='javascript'>window.close();</script>")
                    Response.End()
                End If

            Catch ex As Exception
                Response.Write("<script language='javascript'>window.close();</script>")
                Response.End()
            End Try
            Me.LNBclose.Attributes.Add("onclick", "window.close();return false;")
        End If
    End Sub

#Region "Internazionalizzazione"
    Public Function SetCulture(ByVal Code As String, ByVal ResourcesName As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_CurriculumIscrittiComunita"
        oResource.Folder_Level1 = "Curriculum"
        oResource.setCulture()
    End Function
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLinkButton(Me.LNBclose, True, False)
            TBSmenu.Tabs(TabIndex.Dati).Text = .getValue("TABdati.Text")
            TBSmenu.Tabs(TabIndex.Dati).ToolTip = .getValue("TABdati.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("TABcompetenze.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABcompetenze.ToolTip")
            TBSmenu.Tabs(2).Text = .getValue("TABformazione.Text")
            TBSmenu.Tabs(2).ToolTip = .getValue("TABformazione.ToolTip")
            TBSmenu.Tabs(3).Text = .getValue("TABlingua.Text")
            TBSmenu.Tabs(3).ToolTip = .getValue("TABlingua.ToolTip")
            TBSmenu.Tabs(TabIndex.Esperienze).Text = .getValue("TABesperienze.Text")
            TBSmenu.Tabs(TabIndex.Esperienze).ToolTip = .getValue("TABesperienze.ToolTip")
        End With
    End Sub
#End Region



    Private Sub Bind_Dati()
        Dim oPersona As New COL_Persona
        Dim PRSN_id As Integer

        PRSN_id = Me.HDN_Studente_ID.Value

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"), "page_VetrinaCurriculum")
        End If

        Me.PNLcurriculum.Visible = True
        Me.TBRdati.Visible = True
        Me.TBRformazione.Visible = False
        Me.TBRlingua.Visible = False
        Me.TBResperienze.Visible = False
        Me.TBRcompetenze.Visible = False
        Me.TBSmenu.SelectedIndex = 0

        Me.CTRLdati.PRSN_ID = PRSN_id
        Me.CTRLformazione.PRSN_ID = PRSN_id
        Me.CTRLlingua.PRSN_ID = PRSN_id
        Me.CTRLlavoro.PRSN_ID = PRSN_id
        Me.CTRLcompetenze.PRSN_ID = PRSN_id

        Me.CTRLdati.start()
        Me.TBSmenu.Tabs(TabIndex.Competenze).Visible = Me.CTRLcompetenze.start()
        Me.TBSmenu.Tabs(TabIndex.Formazione).Visible = Me.CTRLformazione.start()
        Me.TBSmenu.Tabs(TabIndex.Lingua).Visible = Me.CTRLlingua.start()
        Me.TBSmenu.Tabs(TabIndex.Esperienze).Visible = Me.CTRLlavoro.start()
        If Not (Me.TBSmenu.Tabs(TabIndex.Competenze).Visible = True And TBSmenu.Tabs(TabIndex.Formazione).Visible = True And TBSmenu.Tabs(TabIndex.Lingua).Visible = True And TBSmenu.Tabs(TabIndex.Esperienze).Visible = True) Then
            Dim larghezza As Integer = 0
            If TBSmenu.Tabs(TabIndex.Dati).Visible = True Then
                larghezza = larghezza + 170
            End If
            If Me.TBSmenu.Tabs(TabIndex.Competenze).Visible = True Then
                larghezza = larghezza + 125
            End If
            If TBSmenu.Tabs(TabIndex.Formazione).Visible = True Then
                larghezza = larghezza + 105
            End If
            If TBSmenu.Tabs(TabIndex.Lingua).Visible = True Then
                larghezza = larghezza + 180
            End If
            If TBSmenu.Tabs(TabIndex.Esperienze).Visible = True Then
                larghezza = larghezza + 180
            End If
            TBSmenu.Width = System.Web.UI.WebControls.Unit.Pixel(larghezza)
        End If

    End Sub


#Region "tab"
    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.TBRdati.Visible = False
        Me.TBRformazione.Visible = False
        Me.TBRlingua.Visible = False
        Me.TBResperienze.Visible = False
        Me.TBRcompetenze.Visible = False

        Select Case Me.TBSmenu.SelectedIndex
            Case 0
                Me.TBRdati.Visible = True
                Me.CTRLdati.start()
            Case 1
                Me.TBRcompetenze.Visible = True
                Me.CTRLcompetenze.start()
            Case 2
                Me.TBRformazione.Visible = True
                Me.CTRLformazione.start()
            Case 3
                Me.TBRlingua.Visible = True
                Me.CTRLlingua.start()
            Case 4
                Me.TBResperienze.Visible = True
                Me.CTRLlavoro.start()
        End Select
    End Sub
#End Region


End Class
