Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_persona.CL_Studente
Imports COL_BusinessLogic_v2.CL_persona.CL_Docente
Imports COL_BusinessLogic_v2.CL_persona.CL_Esterno
Imports COL_BusinessLogic_v2.CL_persona.CL_Dottorando
Imports COL_BusinessLogic_v2.CL_persona.CL_Tecnico
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.File


Public Class InfoIscritto
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

    Protected WithEvents LBinfo As System.Web.UI.WebControls.Label
    Protected WithEvents LBanagrafica As System.Web.UI.WebControls.Label
    Protected WithEvents LBlogin_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBCognome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdisclaimer As System.Web.UI.WebControls.Label

#Region "Dettagli Generali"
    Protected WithEvents TBLDettagli As System.Web.UI.WebControls.Table
    Protected WithEvents LBlogin As System.Web.UI.WebControls.Label
    Protected WithEvents LBNome As System.Web.UI.WebControls.Label
    Protected WithEvents LBCognome As System.Web.UI.WebControls.Label

    Protected WithEvents LBlingua_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBMail_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBHomePage_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBdettagliRuolo As System.Web.UI.WebControls.Label

    Protected WithEvents LBMail As System.Web.UI.WebControls.Label
    Protected WithEvents LBHomePage As System.Web.UI.WebControls.Label
    Protected WithEvents TBRhomePage As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmail As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBtipoPersona_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoPersona As System.Web.UI.WebControls.Label

    Protected WithEvents LBdataIscrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdataIscrizione As System.Web.UI.WebControls.Label

    Protected WithEvents LBorganizzazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBorganizzazione As System.Web.UI.WebControls.Label

    Protected WithEvents LBlingua As System.Web.UI.WebControls.Label
    Protected WithEvents TBRdettagliRuolo As System.Web.UI.WebControls.TableRow

#End Region




#Region "Dettagli Esterno"
    Protected WithEvents TBRmansione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBMansione_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBMansione As System.Web.UI.WebControls.Label
#End Region



    ' Protected WithEvents TBRfoto As System.Web.UI.WebControls.TableRow
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

    Public Sub BindScheda()
        Try

            Dim TPPR_id, CMNT_ID As Integer
            TPPR_id = Request.QueryString("TPPR_ID")

          
            Me.TBRmansione.Visible = False
            If TPPR_id = Main.TipoPersonaStandard.Esterno Then  'esterno
                Bind_SchedaEsterno()
            Else
                Bind_schedaStandard()
            End If

            Me.TBRhomePage.Visible = Not (Me.LBHomePage.Text = "")

            Me.TBRmail.Visible = Not (Me.LBMail.Text = "")
            Me.TBRhomePage.Visible = Not (Me.LBHomePage.Text = "")
       
            Me.TBRmansione.Visible = Not (Me.LBMansione.Text = "")

            Me.TBRdettagliRuolo.Visible = Not (Me.LBMansione.Text = "")
            ' And LBtipoIstituto.Text = "" And LBistituto.Text = "" And Me.LBprovinciaIstituto.Text = "" And Me.LBcittaIstituto.Text = "" And Me.LBviaIstituto.Text = "" And Me.LBclasseSezione.Text = "")

       
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_SchedaEsterno()
        Dim oEsterno As New COL_Esterno
        Me.TBRmansione.Visible = True
        oEsterno.Id = Request.QueryString("PRSN_ID")
        oEsterno.EstraiTutto(Session("LinguaID"))
        Me.LBlogin.Text = oEsterno.Login
        Me.LBNome.Text = oEsterno.Nome
        Me.LBCognome.Text = oEsterno.Cognome

        Me.LBlingua.Text = oEsterno.Lingua.Nome

        Me.LBMail.Text = oEsterno.Mail
        HLmail.NavigateUrl = "mailto:" & oEsterno.Mail

        Me.LBHomePage.Text = oEsterno.HomePage
        Me.LBMansione.Text = oEsterno.Mansione
        Me.TBRmansione.Visible = Not (Me.LBMansione.Text = "")

        If oEsterno.FotoPath = Nothing Then 'nessuna immagine nel database
            IMFoto.ImageUrl = "./../images/noImage.jpg"
            IMFoto.Visible = True
        Else 'immagine trovata nel db
            Dim url As String = "./../profili/" & oEsterno.Id & "/" & oEsterno.FotoPath

            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato

                IMFoto.ImageUrl = url
                IMFoto.Visible = True
                '  Me.TBRfoto.Visible = True
            Else
                IMFoto.ImageUrl = "./../images/noImage.jpg"
                IMFoto.Visible = True
            End If
        End If
        Me.LBdataIscrizione.Text = oEsterno.DataInserimento
        Me.LBtipoPersona.Text = "Esterno"
        'organizzazione
        Dim ORGN_id As Integer
        ORGN_id = oEsterno.GetOrganizzazioneDefault()
        Dim oOrganizzazione As New COL_Organizzazione
        oOrganizzazione.Id = ORGN_id
        oOrganizzazione.Estrai()
        Me.LBorganizzazione.Text = oOrganizzazione.RagioneSociale
    End Sub
    Private Sub Bind_schedaStandard()
        Dim oPersona As New COL_Persona
        oPersona.Id = Request.QueryString("PRSN_ID")
        oPersona.EstraiTutto(Session("LinguaID"))

        If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
            Me.LBCognome_t.Text = oResource.getValue("societa")
            Me.LBnome_t.Text = oResource.getValue("facoltaCopisteria")
            Me.LBtipoPersona.Text = "Copisteria"
        Else
            Me.LBtipoPersona.Text = oPersona.TipoPersona.Descrizione
            oResource.setLabel(Me.LBnome_t)
            oResource.setLabel(Me.LBCognome_t)
        End If
        Me.LBlogin.Text = oPersona.Login
        Me.LBNome.Text = oPersona.Nome
        Me.LBCognome.Text = oPersona.Cognome

        Me.LBlingua.Text = oPersona.Lingua.Nome

        Me.LBMail.Text = oPersona.Mail
        HLmail.NavigateUrl = "mailto:" & oPersona.Mail

        Me.LBHomePage.Text = oPersona.HomePage

        If oPersona.FotoPath = Nothing Then 'nessuna immagine nel database
            IMFoto.ImageUrl = "./../images/noImage.jpg"
            IMFoto.Visible = True
        Else 'immagine trovata nel db
            Dim url As String = "./../profili/" & oPersona.Id & "/" & oPersona.FotoPath

            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato

                IMFoto.ImageUrl = url
                IMFoto.Visible = True
                ' Me.TBRfoto.Visible = True
            Else
                IMFoto.ImageUrl = "./../images/noImage.jpg"
                IMFoto.Visible = True
                'file non trovato su disco
            End If
        End If
        Me.LBdataIscrizione.Text = oPersona.DataInserimento

        'organizzazione
        Dim ORGN_id As Integer
        ORGN_id = oPersona.GetOrganizzazioneDefault()
        Dim oOrganizzazione As New COL_Organizzazione
        oOrganizzazione.Id = ORGN_id
        oOrganizzazione.Estrai()
        Me.LBorganizzazione.Text = oOrganizzazione.RagioneSociale
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
            .setLabel(Me.LBinfo)
            .setLabel(Me.LBanagrafica)
            .setLabel(Me.LBlogin_c)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBCognome_t)
            .setLabel(Me.LBorganizzazione_t)
            .setLabel(Me.LBtipoPersona_t)
            .setLabel(Me.LBdataIscrizione_t)
            .setLabel(Me.LBlingua_c)

            .setLabel(Me.LBMail_c)
            .setLabel(Me.LBHomePage_c)
            .setLabel(Me.LBdettagliRuolo)
          

        
            .setLabel(Me.LBMansione_c)

            .setLabel(Me.LBdisclaimer)

            .setButton(Me.BTNOk)

         

            Me.IMFoto.ToolTip = oResource.getValue("IMFoto.ToolTip")
        End With
    End Sub

#End Region



End Class
