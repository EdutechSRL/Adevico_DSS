Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_persona.CL_Studente
Imports COL_BusinessLogic_v2.CL_persona.CL_Docente
Imports COL_BusinessLogic_v2.CL_persona.CL_Esterno
Imports COL_BusinessLogic_v2.CL_persona.CL_Dottorando
Imports COL_BusinessLogic_v2.CL_persona.CL_Tecnico
Imports lm.Comol.Core.File

Public Class ADM_InfoPersona
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBCognome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdisclaimer As System.Web.UI.WebControls.Label
    Protected WithEvents LBlogin_c As System.Web.UI.WebControls.Label

#Region "Dettagli Generali"
    Protected WithEvents LBdettagliRuolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoPersona_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLDettagli As System.Web.UI.WebControls.Table
    Protected WithEvents LBlogin As System.Web.UI.WebControls.Label
    Protected WithEvents LBNome As System.Web.UI.WebControls.Label
    Protected WithEvents LBCognome As System.Web.UI.WebControls.Label
    Protected WithEvents LBDataNascita As System.Web.UI.WebControls.Label
    Protected WithEvents LBSex As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents LBCodFiscale As System.Web.UI.WebControls.Label
    Protected WithEvents LBStato As System.Web.UI.WebControls.Label
    Protected WithEvents LBProvincia As System.Web.UI.WebControls.Label
    Protected WithEvents TBRindirizzo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBIndirizzo As System.Web.UI.WebControls.Label
    Protected WithEvents LBMail_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBRcap_citta As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBCap As System.Web.UI.WebControls.Label
    Protected WithEvents LBCap_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBCitta As System.Web.UI.WebControls.Label
    Protected WithEvents LBCitta_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBHomePage_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBRtel1_2 As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBTel1 As System.Web.UI.WebControls.Label
    Protected WithEvents LBTel2 As System.Web.UI.WebControls.Label
    Protected WithEvents LBTel1_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBTel2_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBCel As System.Web.UI.WebControls.Label
    Protected WithEvents LBFax As System.Web.UI.WebControls.Label
    Protected WithEvents LBCel_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBFax_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBRCell_Fax As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBMail As System.Web.UI.WebControls.Label
    Protected WithEvents LBHomePage As System.Web.UI.WebControls.Label
    Protected WithEvents TBRhomePage As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmail As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBRricezSMS As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBlingua As System.Web.UI.WebControls.Label
    Protected WithEvents LBricSMS As System.Web.UI.WebControls.Label
    Protected WithEvents TBRCodFiscale As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdettagliRuolo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBorganizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents LBorganizzazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraMail_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBmostraMail As System.Web.UI.WebControls.Label
#End Region



    '  Protected WithEvents TBRfoto As System.Web.UI.WebControls.TableRow
    Protected WithEvents IMFoto As System.Web.UI.WebControls.Image
    Protected WithEvents BTNOk As System.Web.UI.WebControls.Button
    Protected WithEvents HLmail As System.Web.UI.WebControls.HyperLink


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
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            BTNOk.Attributes.Add("onclick", "ChiudiMi();return false;")
        End If
        BindScheda()

    End Sub

    Private Sub BindScheda()
        Try

            Dim TPPR_id As Integer
            TPPR_id = Request.QueryString("TPPR_ID")

          If TPPR_id = Main.TipoPersonaStandard.Esterno Then  'esterno
                Me.Bind_SchedaEsterno()
     
            Else
                Me.Bind_schedaStandard()
            End If

            Me.TBRhomePage.Visible = Not (Me.LBHomePage.Text = "")
            Me.TBRindirizzo.Visible = Not (Me.LBIndirizzo.Text = "")
            Me.TBRCell_Fax.Visible = Not (Me.LBFax.Text = "" And Me.LBCel.Text = "")
            Me.TBRCodFiscale.Visible = Not (Me.LBCodFiscale.Text = "")
            Me.TBRcap_citta.Visible = Not (Me.LBCap.Text = "" And Me.LBCitta.Text = "")
            Me.LBCap_c.Visible = Not (Me.LBCap.Text = "")
            Me.LBCitta_c.Visible = Not (Me.LBCitta.Text = "")
            Me.LBTel1_c.Visible = Not (Me.LBTel1.Text = "")
            Me.LBTel2_c.Visible = Not (Me.LBTel2.Text = "")
            Me.LBCel_c.Visible = Not (Me.LBCel.Text = "")
            Me.LBFax_c.Visible = Not (Me.LBFax.Text = "")
            Me.TBRmail.Visible = Not (Me.LBMail.Text = "")
            Me.TBRhomePage.Visible = Not (Me.LBHomePage.Text = "")
         

            Me.TBRtel1_2.Visible = Not (Me.LBTel1.Text = "" And Me.LBTel2.Text = "")
        
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Bind_SchedaEsterno()
        Dim oEsterno As New COL_Esterno
        oEsterno.ID = Request.QueryString("PRSN_ID")
        oEsterno.EstraiTutto(Session("LinguaID"))
        LBorganizzazione.Text = oEsterno.ORGNDefault_ragioneSociale
        Me.LBlogin.Text = oEsterno.Login
        Me.LBNome.Text = oEsterno.Nome
        Me.LBCognome.Text = oEsterno.Cognome
        Me.LBDataNascita.Text = oEsterno.DataNascita
        If oEsterno.Sesso = 1 Then
            Me.LBSex.Text = "Uomo"
        Else
            Me.LBSex.Text = "Donna"
        End If
        Me.LBlingua.Text = oEsterno.Lingua.Nome
        Me.LBCodFiscale.Text = oEsterno.CodFiscale
        Me.LBStato.Text = oEsterno.Stato.Descrizione
        Me.LBProvincia.Text = oEsterno.Provincia.Nome
        Me.LBIndirizzo.Text = oEsterno.Indirizzo
        Me.LBCap.Text = oEsterno.Cap
        Me.LBCitta.Text = oEsterno.Citta
        Me.LBTel1.Text = oEsterno.Telefono1
        Me.LBTel2.Text = oEsterno.Telefono2
        Me.LBCel.Text = oEsterno.Cellulare
        Me.LBFax.Text = oEsterno.Fax
        If oEsterno.Mail = "" Then
            Me.TBRmail.Visible = False
        Else
            Me.LBMail.Text = oEsterno.Mail
            HLmail.NavigateUrl = "mailto:" & oEsterno.Mail
        End If
        If oEsterno.MostraMail = 1 Then
            Me.LBmostraMail.Text = "Sì"
        Else
            Me.LBmostraMail.Text = "No"
        End If
        Me.LBHomePage.Text = oEsterno.HomePage
        If oEsterno.RicezioneSMS = 1 Then
            Me.LBricSMS.Text = "Sì"
        Else
            Me.LBricSMS.Text = "No"
        End If



        If oEsterno.FotoPath = Nothing Then 'nessuna immagine nel database
            IMFoto.Visible = False
            ' Me.TBRfoto.Visible = False
        Else 'immagine trovata nel db
            Dim url As String = "./../profili/" & oEsterno.Id & "/" & oEsterno.FotoPath

            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato

                IMFoto.ImageUrl = url
                IMFoto.Visible = True
                'Me.TBRfoto.Visible = True
            Else
                IMFoto.Visible = False 'file non trovato su disco
                'Me.TBRfoto.Visible = False
            End If
        End If
    End Sub
    Private Sub Bind_schedaStandard()
        Dim oPersona As New COL_Persona
        oPersona.Id = Request.QueryString("PRSN_ID")
        oPersona.EstraiTutto(Session("LinguaID"))

        If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
            Me.LBCognome_t.Text = "Società:"
            Me.LBnome_t.Text = "Facoltà:"
        Else
            Me.LBCognome_t.Text = "Cognome:"
            Me.LBnome_t.Text = "Nome:"
        End If
        LBorganizzazione.Text = oPersona.ORGNDefault_ragioneSociale
        Me.LBlogin.Text = oPersona.Login
        Me.LBNome.Text = oPersona.Nome
        Me.LBCognome.Text = oPersona.Cognome
        Me.LBDataNascita.Text = oPersona.DataNascita
        If oPersona.Sesso = 1 Then
            Me.LBSex.Text = "Uomo"
        Else
            Me.LBSex.Text = "Donna"
        End If
        Me.LBlingua.Text = oPersona.Lingua.Nome
        Me.LBCodFiscale.Text = oPersona.CodFiscale
        Me.LBStato.Text = oPersona.Stato.Descrizione
        Me.LBProvincia.Text = oPersona.Provincia.Nome
        Me.LBIndirizzo.Text = oPersona.Indirizzo
        Me.LBCap.Text = oPersona.Cap
        Me.LBCitta.Text = oPersona.Citta
        Me.LBTel1.Text = oPersona.Telefono1
        Me.LBTel2.Text = oPersona.Telefono2
        Me.LBCel.Text = oPersona.Cellulare
        Me.LBFax.Text = oPersona.Fax
        If oPersona.Mail = "" Then
            Me.TBRmail.Visible = False
        Else
            Me.LBMail.Text = oPersona.Mail
            HLmail.NavigateUrl = "mailto:" & oPersona.Mail
        End If
        If oPersona.MostraMail = 1 Then
            Me.LBmostraMail.Text = "Sì"
        Else
            Me.LBmostraMail.Text = "No"
        End If
        Me.LBHomePage.Text = oPersona.HomePage
        If oPersona.RicezioneSMS = 1 Then
            Me.LBricSMS.Text = "Sì"
        Else
            Me.LBricSMS.Text = "No"
        End If

        If oPersona.FotoPath = Nothing Then 'nessuna immagine nel database
            IMFoto.Visible = False
        Else 'immagine trovata nel db
            Dim url As String = "./../profili/" & oPersona.Id & "/" & oPersona.FotoPath
            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato
                IMFoto.ImageUrl = url
                IMFoto.Visible = True
            Else
                IMFoto.Visible = False 'file non trovato su disco
            End If
        End If
    End Sub

#Region "Internazionalizzazione"
    ' Inizializzazione oggetto risorse: SEMPRE DA FARE
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_InfoIscritto"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBinfo)
            '.setLabel(Me.LBanagrafica)
            .setLabel(Me.LBlogin_c)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBCognome_t)
            .setLabel(Me.LBorganizzazione_t)
            .setLabel(Me.LBtipoPersona_t)
            .setLabel(Me.LBmostraMail_c)
            .setLabel(Me.LBlogin_c)

            .setLabel(Me.LBMail_c)
            .setLabel(Me.LBHomePage_c)
            .setLabel(Me.LBdettagliRuolo)
          
            .setLabel(Me.LBdisclaimer)

            .setButton(Me.BTNOk)

           

            Me.IMFoto.ToolTip = Me.oResource.getValue("IMFoto.ToolTip")
        End With
    End Sub

#End Region
End Class
