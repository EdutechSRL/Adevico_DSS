Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Curriculum
Imports lm.ActionDataContract

Imports System.IO
Imports iTextSharp

Public Class CurriculumEuropeo
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

#Region "private"
	Private _PageUtility As OLDpageUtility
	Public ReadOnly Property PageUtility() As OLDpageUtility
		Get
			If IsNothing(_PageUtility) Then
				_PageUtility = New OLDpageUtility(Me.Context)
			End If
			Return _PageUtility
		End Get
	End Property
#End Region


#Region "Struttura"

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property


    Protected WithEvents LKBesportaPDF As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBesportaRTF As System.Web.UI.WebControls.LinkButton

   Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip

    Protected WithEvents TBRdati As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRformazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlingua As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBResperienze As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcompetenze As System.Web.UI.WebControls.TableRow
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_DatiCurriculum
    Protected WithEvents CTRLformazione As Comunita_OnLine.UC_Formazione
    Protected WithEvents CTRLlingua As Comunita_OnLine.UC_ConoscenzaLingua
    Protected WithEvents CTRLlavoro As Comunita_OnLine.UC_EsperienzeLavorative
    Protected WithEvents CTRLcompetenze As Comunita_OnLine.UC_Competenze
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
#End Region

#Region "Inserimento"
    Protected WithEvents LKBinserisciFormazione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBinserisciLingua As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBinserisciEsperienza As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaggiungiCurriculum As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodificaCurriculum As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaggiungiCompetenza As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodificaCompetenza As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LKBaggiungiFormazione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodificaFormazione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBannullaFormazione As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LKBaggiungiLingua As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodificaLingua As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBannullaLingua As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LKBaggiungiEsperienza As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBmodificaEsperienza As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBannullaEsperienza As System.Web.UI.WebControls.LinkButton
#End Region

    Protected WithEvents IMBword As System.Web.UI.WebControls.ImageButton

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
            SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Page.IsPostBack = False Then
            Try
                Me.SetupInternazionalizzazione()
                Dim oPersona As New COL_Persona
                oPersona = Session("objPersona")

                Me.PNLmenu.Visible = True
                Me.PNLcontenuto.Visible = True
                Me.LKBaggiungiCurriculum.Visible = False
                Me.LKBmodificaCurriculum.Visible = True
                Me.LKBesportaRTF.Visible = True
                Me.LKBesportaPDF.Visible = True

                Me.CTRLdati.PRSN_ID = oPersona.Id

                Me.CTRLformazione.PRSN_ID = oPersona.Id
                Me.CTRLlingua.PRSN_ID = oPersona.Id
                Me.CTRLlavoro.PRSN_ID = oPersona.Id
                Me.CTRLcompetenze.PRSN_ID = oPersona.Id

                ' REGISTRAZIONE EVENTO

				Try
					Dim oCurriculum As New COL_CurriculumEuropeo
					oCurriculum.EstraiByPersona(oPersona.ID)
					If oCurriculum.Errore = Errori_Db.None Then
						If oCurriculum.ID > 0 Then
							Me.CTRLformazione.CurriculumID = oCurriculum.ID
							Me.CTRLlingua.CurriculumID = oCurriculum.ID
							Me.CTRLlavoro.CurriculumID = oCurriculum.ID
							Me.CTRLcompetenze.CurriculumID = oCurriculum.ID

							Me.PageUtility.AddAction(ActionType.ShowCurriculum, Me.PageUtility.CreateObjectsList(ObjectType.Curriculum, oCurriculum.ID), InteractionType.UserWithLearningObject)
						Else
							Me.PageUtility.AddAction(ActionType.ShowCurriculum, Nothing, InteractionType.UserWithLearningObject)
						End If
					Else
						Me.PageUtility.AddAction(ActionType.GenericError, , InteractionType.UserWithLearningObject)
					End If
				Catch ex As Exception

				End Try
			Catch ex As Exception
				Me.PNLmenu.Visible = False
			End Try
		End If
	End Sub

	Private Function SessioneScaduta() As Boolean
		Dim oPersona As COL_Persona
		Dim isScaduta As Boolean = True
		Try
			oPersona = Session("objPersona")
			If oPersona.ID > 0 Then
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
            oPersona = Nothing
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & Me.PageUtility.GetDefaultLogoutPage & "');</script>")
			Response.End()
			Return True
		End If
	End Function

#Region "Localizzazione"
	Public Function SetCulture(ByVal Code As String)
		oResource = New ResourceManager

		oResource.UserLanguages = Code
		oResource.ResourcesName = "pg_CurriculumEuropeo"
		oResource.Folder_Level1 = "Curriculum"
		oResource.setCulture()
	End Function
	Public Sub SetupInternazionalizzazione()
		With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(LBNopermessi)
            TBSmenu.Tabs(0).Text = .getValue("TABdati.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("TABdati.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("TABcompetenze.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABcompetenze.ToolTip")
            TBSmenu.Tabs(2).Text = .getValue("TABformazione.Text")
            TBSmenu.Tabs(2).ToolTip = .getValue("TABformazione.ToolTip")
            TBSmenu.Tabs(3).Text = .getValue("TABlingua.Text")
            TBSmenu.Tabs(3).ToolTip = .getValue("TABlingua.ToolTip")
            TBSmenu.Tabs(4).Text = .getValue("TABesperienze.Text")
            TBSmenu.Tabs(4).ToolTip = .getValue("TABesperienze.ToolTip")

			.setLinkButton(LKBinserisciFormazione, True, True)
			.setLinkButton(LKBinserisciLingua, True, True)
			.setLinkButton(LKBinserisciEsperienza, True, True)

			.setLinkButton(LKBaggiungiCurriculum, True, True)
			.setLinkButton(LKBmodificaCurriculum, True, True)

			.setLinkButton(LKBaggiungiCompetenza, True, True)
			.setLinkButton(LKBmodificaCompetenza, True, True)
			.setLinkButton(LKBaggiungiFormazione, True, True)
			.setLinkButton(LKBmodificaFormazione, True, True)
			.setLinkButton(LKBannullaFormazione, True, True)
			.setLinkButton(LKBaggiungiLingua, True, True)
			.setLinkButton(LKBmodificaLingua, True, True)
			.setLinkButton(LKBannullaLingua, True, True)
			.setLinkButton(LKBaggiungiEsperienza, True, True)
			.setLinkButton(LKBmodificaEsperienza, True, True)
			.setLinkButton(LKBannullaEsperienza, True, True)

			.setLinkButton(Me.LKBesportaPDF, True, True)
			.setLinkButton(Me.LKBesportaRTF, True, True)

		End With
	End Sub
#End Region

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.TBRdati.Visible = False
        Me.TBRformazione.Visible = False
        Me.TBRlingua.Visible = False
        Me.TBResperienze.Visible = False
        Me.TBRcompetenze.Visible = False

        Me.LKBaggiungiCurriculum.Visible = False
        Me.LKBmodificaCurriculum.Visible = False

        Me.LKBaggiungiCompetenza.Visible = False
        Me.LKBmodificaCompetenza.Visible = False

        Me.LKBannullaFormazione.Visible = False
        Me.LKBaggiungiFormazione.Visible = False
        Me.LKBmodificaFormazione.Visible = False
        Me.LKBinserisciFormazione.Visible = False

        Me.LKBannullaEsperienza.Visible = False
        Me.LKBaggiungiEsperienza.Visible = False
        Me.LKBmodificaEsperienza.Visible = False
        Me.LKBinserisciEsperienza.Visible = False

        Me.LKBannullaLingua.Visible = False
        Me.LKBaggiungiLingua.Visible = False
        Me.LKBmodificaLingua.Visible = False
        Me.LKBinserisciLingua.Visible = False

        Me.LKBesportaPDF.Visible = False
        Me.LKBesportaRTF.Visible = False

        Select Case Me.TBSmenu.SelectedIndex
            Case 0
                Me.TBRdati.Visible = True
                'Me.IMpipe.Visible = False
                'se in start si evince che non è stato creato si richiama nascondi tab
                Me.LKBaggiungiCurriculum.Visible = False
                Me.LKBmodificaCurriculum.Visible = True
                Me.LKBesportaPDF.Visible = True
                Me.LKBesportaRTF.Visible = True
                Me.CTRLdati.start()
            Case 1
                Me.LKBaggiungiCompetenza.Visible = False
                Me.LKBmodificaCompetenza.Visible = True
                Me.TBRcompetenze.Visible = True
                'Me.IMpipe.Visible = False
                Me.CTRLcompetenze.start()
                Me.LKBesportaPDF.Visible = True
                Me.LKBesportaRTF.Visible = True
            Case 2

                Me.TBRformazione.Visible = True
                Me.LKBinserisciFormazione.Visible = True
                Me.CTRLformazione.start()
                Me.LKBesportaPDF.Visible = True
                Me.LKBesportaRTF.Visible = True
            Case 3
                Me.TBRlingua.Visible = True
                Me.LKBinserisciLingua.Visible = True
                Me.CTRLlingua.Start()
                Me.LKBesportaPDF.Visible = True
                Me.LKBesportaRTF.Visible = True
            Case 4
                Me.TBResperienze.Visible = True
                Me.LKBinserisciEsperienza.Visible = True
                Me.CTRLlavoro.Setup_Controllo()
                Me.LKBesportaPDF.Visible = True
                Me.LKBesportaRTF.Visible = True
        End Select
    End Sub

#Region "gestione Nascondi/mostra linkbutton"
	Private Sub CTRLcompetenze_MostraAggiungi(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLcompetenze.MostraAggiungi
		Me.LKBaggiungiCompetenza.Visible = True
		Me.LKBmodificaCompetenza.Visible = False
	End Sub
	Private Sub CTRLcompetenze_MostraModifica(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLcompetenze.MostraModifica
		Me.LKBaggiungiCompetenza.Visible = False
		Me.LKBmodificaCompetenza.Visible = True
	End Sub
	Private Sub CTRLformazione_MostraModifica(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLformazione.MostraModifica
		Me.LKBaggiungiFormazione.Visible = False
		Me.LKBmodificaFormazione.Visible = True
		Me.LKBannullaFormazione.Visible = True
		Me.LKBinserisciFormazione.Visible = False
	End Sub
	Private Sub CTRLformazione_MostraAggiungi(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLformazione.MostraAggiungi
		Me.LKBaggiungiFormazione.Visible = True
		Me.LKBmodificaFormazione.Visible = False
		Me.LKBannullaFormazione.Visible = False
		Me.LKBinserisciFormazione.Visible = False
	End Sub
	Private Sub CTRLlingua_MostraModifica(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLlingua.MostraModifica
		Me.LKBaggiungiLingua.Visible = False
		Me.LKBmodificaLingua.Visible = True
		Me.LKBannullaLingua.Visible = True
		Me.LKBinserisciLingua.Visible = False
	End Sub
	Private Sub CTRLlingua_MostraAggiungi(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLlingua.MostraAggiungi
		Me.LKBaggiungiLingua.Visible = True
		Me.LKBmodificaLingua.Visible = False
		Me.LKBannullaLingua.Visible = False
		Me.LKBinserisciLingua.Visible = False
	End Sub
	Private Sub CTRLlavoro_MostraModifica(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLlavoro.MostraModifica
		Me.LKBaggiungiEsperienza.Visible = False
		Me.LKBmodificaEsperienza.Visible = True
		Me.LKBannullaEsperienza.Visible = True
		Me.LKBinserisciEsperienza.Visible = False
	End Sub
	Private Sub CTRLlavoro_MostraAggiungi(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLlavoro.MostraAggiungi
		Me.LKBaggiungiEsperienza.Visible = True
		Me.LKBmodificaEsperienza.Visible = False
		Me.LKBannullaEsperienza.Visible = False
		Me.LKBinserisciEsperienza.Visible = False
	End Sub
#End Region


	Private Sub CTRLdati_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLdati.AggiornaDati
		Response.Redirect("./curriculumEuropeo.aspx")
	End Sub
	Private Sub CTRLdati_NascondiTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLdati.NascondiTab
		Me.LKBaggiungiCurriculum.Visible = True
		Me.LKBmodificaCurriculum.Visible = False
		Me.LKBesportaPDF.Visible = False
		Me.LKBesportaRTF.Visible = False
		Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Pixel(200)

        Me.TBSmenu.Tabs(4).Visible = False
        Me.TBSmenu.Tabs(2).Visible = False
        Me.TBSmenu.Tabs(3).Visible = False
        Me.TBSmenu.Tabs(1).Visible = False


		Me.TBRformazione.Visible = False
		Me.TBRlingua.Visible = False
		Me.TBResperienze.Visible = False
		Me.TBRcompetenze.Visible = False
	End Sub


#Region "export"
	Private Sub IMBword_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBword.Click
		Try

			'    Dim FileName As String = "prova.doc"
			'    Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter
			'    Dim oHTMLWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
			'    Response.Buffer = True
			'    PNLcontenuto.Visible = True
			'    Page.Response.Clear()
			'    Page.Response.ClearContent()
			'    Page.Response.ClearHeaders()

			'    PNLcontenuto.RenderControl(oHTMLWriter)
			'    Page.Response.Write("<html><head runat="server"></head><body>")
			'    Page.Response.Write(oStringWriter)
			'    Page.Response.Write("</body></html>")
			'    Dim OpenType As String = "attachment"
			'    Page.Response.ContentType = "application/msword"
			'    Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)
			'    Page.Response.End()
		Catch ex As Exception
			'    ' TBLExport.Visible = False
		End Try
	End Sub

#End Region

#Region "azioni dati curriculum"
	'inserisci è per visualizzare la form di inserimento
	'aggiungi aggiunge
	Private Sub LKBaggiungiCurriculum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiungiCurriculum.Click
		Me.CTRLdati.AggiungiCurriculum()
		Me.PageUtility.AddAction(ActionType.CreateCurriculum, Me.PageUtility.CreateObjectsList(ObjectType.Curriculum, Me.CTRLdati.CurriculumID), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBmodificaCurriculum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodificaCurriculum.Click
		Me.CTRLdati.ModificaCurriculum()
		Me.PageUtility.AddAction(ActionType.SaveCurriculum, Me.PageUtility.CreateObjectsList(ObjectType.Curriculum, Me.CTRLdati.CurriculumID), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBaggiungiCompetenza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiungiCompetenza.Click
		Me.CTRLcompetenze.ModificaCompetenza()
		Me.PageUtility.AddAction(ActionType.EditCurriculum, Me.PageUtility.CreateObjectsList(ObjectType.Competenza, 0), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBmodificaCompetenza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodificaCompetenza.Click
		Me.CTRLcompetenze.ModificaCompetenza()
		Me.PageUtility.AddAction(ActionType.EditCurriculum, Me.PageUtility.CreateObjectsList(ObjectType.Competenza, 0), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBinserisciFormazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBinserisciFormazione.Click
		Me.CTRLformazione.InserisciFormazione()
		Me.LKBannullaFormazione.Visible = True
		Me.LKBaggiungiFormazione.Visible = True
		Me.LKBmodificaFormazione.Visible = False
		Me.LKBinserisciFormazione.Visible = False
	End Sub
	Private Sub LKBaggiungiFormazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiungiFormazione.Click
		Me.LKBannullaFormazione.Visible = False
		Me.LKBaggiungiFormazione.Visible = False
		Me.LKBmodificaFormazione.Visible = False
		Me.LKBinserisciFormazione.Visible = True
		Me.CTRLformazione.AggiungiFormazione()
		Me.PageUtility.AddAction(ActionType.CreateEducation, Me.PageUtility.CreateObjectsList(ObjectType.Education, 0), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBannullaFormazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBannullaFormazione.Click
		Me.LKBannullaFormazione.Visible = False
		Me.LKBaggiungiFormazione.Visible = False
		Me.LKBmodificaFormazione.Visible = False
		Me.LKBinserisciFormazione.Visible = True
		Me.CTRLformazione.AnnullaFormazione()
	End Sub
	Private Sub LKBmodificaFormazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodificaFormazione.Click
		Me.LKBannullaFormazione.Visible = False
		Me.LKBaggiungiFormazione.Visible = False
		Me.LKBmodificaFormazione.Visible = False
		Me.LKBinserisciFormazione.Visible = True
		Me.CTRLformazione.ModificaFormazione()
		Me.PageUtility.AddAction(ActionType.EditEducation, Me.PageUtility.CreateObjectsList(ObjectType.Education, 0), InteractionType.UserWithLearningObject)

	End Sub
	Private Sub LKBinserisciLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBinserisciLingua.Click
		Me.CTRLlingua.InserisciLingua()
		Me.LKBannullaLingua.Visible = True
		Me.LKBaggiungiLingua.Visible = True
		Me.LKBmodificaLingua.Visible = False
		Me.LKBinserisciLingua.Visible = False
	End Sub
	Private Sub LKBaggiungiLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiungiLingua.Click
		Me.CTRLlingua.AggiungiLingua()
		Me.LKBannullaLingua.Visible = False
		Me.LKBaggiungiLingua.Visible = False
		Me.LKBmodificaLingua.Visible = False
		Me.LKBinserisciLingua.Visible = True

		Dim oUtility As New OLDpageUtility(Me.Context)
		Me.PageUtility.AddAction(ActionType.CreateLanguage, Me.PageUtility.CreateObjectsList(ObjectType.Language, 0), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBannullaLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBannullaLingua.Click
		Me.CTRLlingua.AnnullaLingua()
		Me.LKBannullaLingua.Visible = False
		Me.LKBaggiungiLingua.Visible = False
		Me.LKBmodificaLingua.Visible = False
		Me.LKBinserisciLingua.Visible = True
	End Sub
	Private Sub LKBmodificaLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodificaLingua.Click
		Me.CTRLlingua.ModificaLingua()
		Me.LKBannullaLingua.Visible = False
		Me.LKBaggiungiLingua.Visible = False
		Me.LKBmodificaLingua.Visible = False
		Me.LKBinserisciLingua.Visible = True

		Dim oUtility As New OLDpageUtility(Me.Context)
		Me.PageUtility.AddAction(ActionType.EditLanguage, Me.PageUtility.CreateObjectsList(ObjectType.Language, 0), InteractionType.UserWithLearningObject)
	End Sub

	Private Sub LKBinserisciEsperienza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBinserisciEsperienza.Click
		Me.CTRLlavoro.InserisciEsperienza()
		Me.LKBannullaEsperienza.Visible = True
		Me.LKBaggiungiEsperienza.Visible = True
		Me.LKBmodificaEsperienza.Visible = False
		Me.LKBinserisciEsperienza.Visible = False

	End Sub
	Private Sub LKBaggiungiEsperienza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaggiungiEsperienza.Click
		Me.CTRLlavoro.AggiungiEsperienza()
		Me.LKBannullaEsperienza.Visible = False
		Me.LKBaggiungiEsperienza.Visible = False
		Me.LKBmodificaEsperienza.Visible = False
		Me.LKBinserisciEsperienza.Visible = True

		Dim oUtility As New OLDpageUtility(Me.Context)
		Me.PageUtility.AddAction(ActionType.CreateWorkingExperience, Me.PageUtility.CreateObjectsList(ObjectType.WorkingExperience, 0), InteractionType.UserWithLearningObject)
	End Sub
	Private Sub LKBannullaEsperienza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBannullaEsperienza.Click
		Me.CTRLlavoro.AnnullaEsperienza()
		Me.LKBannullaEsperienza.Visible = False
		Me.LKBaggiungiEsperienza.Visible = False
		Me.LKBmodificaEsperienza.Visible = False
		Me.LKBinserisciEsperienza.Visible = True
	End Sub
	Private Sub LKBmodificaEsperienza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBmodificaEsperienza.Click
		Me.CTRLlavoro.ModificaEsperienza()
		Me.LKBannullaEsperienza.Visible = False
		Me.LKBaggiungiEsperienza.Visible = False
		Me.LKBmodificaEsperienza.Visible = False
		Me.LKBinserisciEsperienza.Visible = True

		Dim oUtility As New OLDpageUtility(Me.Context)
		Me.PageUtility.AddAction(ActionType.EditWorkingExperience, Me.PageUtility.CreateObjectsList(ObjectType.WorkingExperience, 0), InteractionType.UserWithLearningObject)
	End Sub
#End Region

    Private Sub SendPDF(ByVal IsPDF As Boolean)
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        'ToDo

    End Sub

    Private Sub LKBesportaPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBesportaPDF.Click
        Me.SendPDF(True)
    End Sub
    Private Sub LKBesportaRTF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBesportaRTF.Click
        Me.SendPDF(False)
	End Sub

	Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
		PageUtility.CurrentModule = PageUtility.GetModule(Services_Curriculum.Codex)
    End Sub

    Public ReadOnly Property CalendarScript As String
        Get
            Dim Var As String

            Try
                Select Case Session("LinguaCode")
                    Case "it-IT"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" & "></script>"
                    Case "en-US"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                    Case "de-DE"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-de.js" & """" & "></script>"
                    Case "fr-FR"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-fr.js" & """" & "></script>"
                    Case "es-ES"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-es.js" & """" & "></script>"
                    Case Else
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                End Select
            Catch ex As Exception
                Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
            End Try
            Return Var
        End Get
    End Property
End Class