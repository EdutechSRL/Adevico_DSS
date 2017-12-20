Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports lm.Comol.Core.File

Imports Telerik

Public Class UC_DettagliComunita
    Inherits System.Web.UI.UserControl
Private oResource As ResourceManager
    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Private Enum maxIscritti_code
        illimitati = 0
        limiteSuperato1 = 1
        limiteSuperato2 = 2
        postoLibero = 3
        postiLiberi = 4
        limiteRaggiunto = 5
    End Enum

    Private Enum TipoIscrizione
        IscriviDeiscrivi = 0
        Iscrivi_NonDeiscrivi = 1
        NonIscrivi_Deiscrivi = 2
        NonIscrivi_NonDeiscivi = 3
    End Enum

    Private Enum OverSubscriptions
        Nessuna = 0
        Una = 1
        Molte = 2
    End Enum

    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
    End Enum

#Region "Tab strip"
    Protected WithEvents TBRtab As System.Web.UI.WebControls.TableRow
 Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Protected WithEvents TBLprincipale As System.Web.UI.WebControls.Table

    Protected WithEvents TBRcreatore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcreatore_sep As System.Web.UI.WebControls.TableRow
#End Region

    Protected WithEvents HDNcomunitaID As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Form Dettagli"

    Protected WithEvents TBLdettagli As System.Web.UI.WebControls.Table
    Protected WithEvents LBNome As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipocomunita As System.Web.UI.WebControls.Label
    Protected WithEvents LBisChiusa As System.Web.UI.WebControls.Label
    Protected WithEvents HYPfoto As System.Web.UI.WebControls.HyperLink
    Protected WithEvents IMFoto As System.Web.UI.WebControls.Image
    Protected WithEvents LBresponsabile As System.Web.UI.WebControls.Label
    Protected WithEvents LBcreatore As System.Web.UI.WebControls.Label
    Protected WithEvents LBmaxIscritti_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBmaxIscritti As System.Web.UI.WebControls.Label
    Protected WithEvents LBiscritti As System.Web.UI.WebControls.Label

    Protected WithEvents LBaccessiSpeciali_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBaccessiSpeciali As System.Web.UI.WebControls.Label

    Protected WithEvents LBoverIscrizioni_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBoverIscrizioni As System.Web.UI.WebControls.Label

    Protected WithEvents TBRdataAperturaChiusura As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdataAperturaChiusura_sep As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBApertura As System.Web.UI.WebControls.Label
    Protected WithEvents LBChiusura As System.Web.UI.WebControls.Label

    Protected WithEvents TBRdataIscrizioni As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdataIscrizioni_sep As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinizioIscrizione As System.Web.UI.WebControls.Label

    Protected WithEvents LBfineIscrizione As System.Web.UI.WebControls.Label



    Protected WithEvents TBRinfoiscrizione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinfoIscrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoIscrizione As System.Web.UI.WebControls.Label
    Protected WithEvents TBRinfoiscrizione_sep As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBRarchiviata As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRarchiviata_sep As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBarchiviata_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBarchiviata As System.Web.UI.WebControls.Label

    Protected WithEvents TBRbloccata As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRbloccata_sep As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBbloccata_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBbloccata As System.Web.UI.WebControls.Label

#End Region

#Region "Label Dettagli"
    Protected WithEvents LB_Nome As System.Web.UI.WebControls.Label
    Protected WithEvents LB_tipocomunita As System.Web.UI.WebControls.Label
    Protected WithEvents LBresponsabile_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBcreatore_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBiscritti_c As System.Web.UI.WebControls.Label
    Protected WithEvents LB_isChiusa As System.Web.UI.WebControls.Label
    Protected WithEvents LB_apertura As System.Web.UI.WebControls.Label

    Protected WithEvents LBiscrizioni_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBiscrizioni As System.Web.UI.WebControls.Label
    Protected WithEvents LB_chiusura As System.Web.UI.WebControls.Label
    Protected WithEvents LB_inizioIscrizione As System.Web.UI.WebControls.Label
    Protected WithEvents LB_fineIscrizione As System.Web.UI.WebControls.Label

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
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

    Public Sub SetupDettagliComunita(ByVal ComunitaID As Integer)
        Me.LBiscritti.Text = ""
        Me.HDNcomunitaID.Value = ComunitaID
		VisualizzaDettagli(ComunitaID)
		Me.TBLdettagli.Visible = True
    End Sub

    'Visualizzo i dettagli della comunità
    Private Sub VisualizzaDettagli(ByVal CMNT_Id As Integer)
        Try
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            oComunita.Id = CMNT_Id
            oComunita.Estrai()

            oPersona = Session("objPersona")
            'Me.PNLDettagli.Visible = True

            oComunita.EstraiByLingua(Session("LinguaID"))

            If IsNothing(oResource) Then
                Me.SetCulture(Session("LinguaCode"))
            End If
            If oComunita.Errore = Errori_Db.None Then
                Dim oResponsabile As New COL_Persona

                Try
                    oResponsabile = oComunita.GetResponsabile
                    If oResponsabile.Errore = Errori_Db.None Then
                        Dim Responsabile As String

                        If oResponsabile.Nome <> "" And oResponsabile.Cognome <> "" Then
                            Responsabile = oResponsabile.Cognome & " " & oResponsabile.Nome
                        ElseIf oResponsabile.Nome <> "" Then
                            Responsabile = oResponsabile.Nome
                        ElseIf oResponsabile.Cognome <> "" Then
                            Responsabile = oResponsabile.Cognome
                        Else
                            Responsabile = oResource.getValue("nonDisponibile")
                        End If
                        If oResponsabile.Mail <> "" Then
                            Responsabile = Responsabile & " mail: <a href=mailto:" & oResponsabile.Mail & ">" & oResponsabile.Mail & "</a>"
                        End If
                        Me.LBresponsabile.Text = Responsabile

                        If oPersona.InfoRicevimento <> "" Then
                            Me.LBresponsabile.Text = Me.LBresponsabile.Text & oResource.getValue("ricevimento")
                            Me.LBresponsabile.Text = Replace(Me.LBresponsabile.Text, "#riceve#", oPersona.InfoRicevimento)
                        End If



                        If oResponsabile.FotoPath = Nothing Then
                            Me.IMFoto.Visible = False
                            Me.HYPfoto.Visible = False
                        Else
                            Dim url As String = PageUtility.BaseUrl & "profili/" & oResponsabile.ID & "/" & oResponsabile.FotoPath

                            Me.HYPfoto.NavigateUrl = "mailto:" & oResponsabile.Mail
                            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato
                                IMFoto.ImageUrl = url
                                IMFoto.Visible = True
                                Me.HYPfoto.Visible = True
                            Else
                                IMFoto.ImageUrl = "./../images/noImage.jpg"
                                IMFoto.Visible = True
                                Me.HYPfoto.Visible = True
                            End If
                        End If
                    Else
                        oResource.setLabel_To_Value(Me.LBresponsabile, "nonDisponibile")
                    End If
                Catch ex As Exception
                    oResource.setLabel_To_Value(Me.LBresponsabile, "nonDisponibile")
                End Try

                'creator
                Dim oCreatore As New COL_Persona
                Try
                    oCreatore = oComunita.GetCreatore
                    If oCreatore.Errore = Errori_Db.None Then
                        Dim Creatore As String

                        If oCreatore.Id = oResponsabile.Id Then
                            Me.TBRcreatore.Visible = False
                            Me.TBRcreatore_sep.Visible = False
                        Else
                            Me.TBRcreatore.Visible = True
                            Me.TBRcreatore_sep.Visible = True

                            If oCreatore.Nome <> "" And oCreatore.Cognome <> "" Then
                                Creatore = oCreatore.Cognome & " " & oCreatore.Nome
                            ElseIf oCreatore.Nome <> "" Then
                                Creatore = oCreatore.Nome
                            ElseIf oCreatore.Cognome <> "" Then
                                Creatore = oCreatore.Cognome
                            Else
                                Creatore = oResource.getValue("nonDisponibile")
                            End If
                            If oCreatore.Mail <> "" Then
                                Creatore = Creatore & " mail: <a href=mailto:" & oCreatore.Mail & ">" & oCreatore.Mail & "</a>"
                            End If
                            Me.LBcreatore.Text = Creatore

                            If oCreatore.InfoRicevimento <> "" Then
                                Me.LBcreatore.Text = Me.LBcreatore.Text & oResource.getValue("ricevimento")
                                Me.LBcreatore.Text = Replace(Me.LBcreatore.Text, "#riceve#", oCreatore.InfoRicevimento)
                            End If
                        End If

                        If Me.IMFoto.Visible = False Then
                            Dim url As String = PageUtility.BaseUrl & "profili/" & oCreatore.ID & "/" & oCreatore.FotoPath

                            Me.HYPfoto.NavigateUrl = "mailto:" & oCreatore.Mail
                            If Exists.File(Server.MapPath(url)) Then 'file su disco trovato
                                IMFoto.ImageUrl = url
                            Else
                                IMFoto.ImageUrl = "./../images/noImage.jpg"
                            End If
                            IMFoto.Visible = True
                            Me.HYPfoto.Visible = True
                        End If
                    Else
                        oResource.setLabel_To_Value(Me.LBcreatore, "nonDisponibile")
                    End If
                Catch ex As Exception
                    oResource.setLabel_To_Value(Me.LBcreatore, "nonDisponibile")
                End Try

                'totale iscritti
                Dim totale, TotaleIscritti As Integer
                Try
					' tipo ruolo = -1
					If oComunita.IsChiusa Then
						totale = oComunita.GetTotaleIscrittiForRuoloDefault(Abilitazione.Tutti, Main.FiltroUtenti.NoPassantiNoCreatori)
					Else
						totale = oComunita.GetTotaleIscrittiForRuoloDefault(Abilitazione.AttivatoAbilitato, Main.FiltroUtenti.NoPassantiNoCreatori)
					End If

					If totale <> Nothing Then
						'    Me.LBiscritti.Text = totale
					Else
						'    Me.LBiscritti.Text = "n.d."
						totale = 0
					End If
				Catch ex As Exception
					' Me.LBiscritti.Text = "n.d."
					totale = 0
				End Try

                Dim oDataset As New DataSet
                Dim stringaRuoli, RuoloDefault As String
                TotaleIscritti = 0
                Try
                    Dim i As Integer

					If oComunita.IsChiusa Then
						oDataset = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, -1, Main.FiltroAbilitazione.Tutti, Main.FiltroUtenti.NoPassantiNoCreatori, False)
					Else
						oDataset = oComunita.RuoliAssociatiByIscrizione(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest, -1, Main.FiltroAbilitazione.AttivatoAbilitato, Main.FiltroUtenti.NoPassantiNoCreatori, False)
					End If

					For i = 0 To oDataset.Tables(0).Rows.Count - 1
						Dim oDataRow As DataRow
						oDataRow = oDataset.Tables(0).Rows(i)

						If oDataRow.Item("TPRL_isDefault") And oComunita.MaxIscritti > 0 Then
							stringaRuoli = "<b>" & oDataRow.Item("TPRL_Nome") & ": " & oDataRow.Item("TPRL_Totale") & "</b> - " & oResource.getValue("defaultRole")
						Else
							stringaRuoli = oDataRow.Item("TPRL_Nome") & ": " & oDataRow.Item("TPRL_Totale")
						End If

						If oDataRow.Item("TPRL_isDefault") Then
							RuoloDefault = oDataRow.Item("TPRL_Nome")
						End If

						If Me.LBiscritti.Text = "" Then
							Me.LBiscritti.Text = stringaRuoli
						Else
							Me.LBiscritti.Text = Me.LBiscritti.Text & "<br>" & stringaRuoli
						End If
						TotaleIscritti = TotaleIscritti + oDataRow.Item("TPRL_Totale")
					Next
				Catch ex As Exception
					oResource.setLabel_To_Value(LBiscritti, "LBiscritti.0")
					totale = 0
				End Try

                Dim stringaIscritti As String
                If TotaleIscritti > 1 Then
                    stringaIscritti = oResource.getValue("LBiscritti.1")
                    stringaIscritti = stringaIscritti.Replace("#%%#", TotaleIscritti)
                    Me.LBiscritti.Text = stringaIscritti & Me.LBiscritti.Text
                End If
                If oComunita.MaxIscritti <= 0 Then
                    oResource.setLabel_To_Value(LBmaxIscritti, "LBmaxIscritti.0")
                Else
                    Dim rimanenti As Integer

                    If totale > oComunita.MaxIscritti Then
                        rimanenti = totale - oComunita.MaxIscritti
                        If rimanenti > 1 Then
                            oResource.setLabel_To_Value(Me.LBmaxIscritti, "LBmaxIscritti." & CType(maxIscritti_code.limiteSuperato2, maxIscritti_code))
                            Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("%##%", rimanenti)
                            'Me.LBmaxIscritti.Text = oComunita.MaxIscritti & "&nbsp;(limite superato di " & rimanenti & " iscritti)"
                        Else
                            oResource.setLabel_To_Value(Me.LBmaxIscritti, "LBmaxIscritti." & CType(maxIscritti_code.limiteSuperato1, maxIscritti_code))
                        End If
                        Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("#%%#", oComunita.MaxIscritti)
                    ElseIf totale = oComunita.MaxIscritti Then
                        'Me.LBmaxIscritti.Text = oComunita.MaxIscritti & "&nbsp;(limite raggiunto)"
                        oResource.setLabel_To_Value(Me.LBmaxIscritti, "LBmaxIscritti." & CType(maxIscritti_code.limiteRaggiunto, maxIscritti_code))
                        Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("#%%#", oComunita.MaxIscritti)
                    Else
                        rimanenti = oComunita.MaxIscritti - totale
                        If rimanenti > 1 Then
                            'Me.LBmaxIscritti.Text = oComunita.MaxIscritti & "&nbsp;(ancora " & rimanenti & " posti disponibili)"
                            oResource.setLabel_To_Value(Me.LBmaxIscritti, "LBmaxIscritti." & CType(maxIscritti_code.postiLiberi, maxIscritti_code))
                            Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("#%%#", oComunita.MaxIscritti)
                            Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("%##%", rimanenti)
                        Else
                            'Me.LBmaxIscritti.Text = oComunita.MaxIscritti & "&nbsp;(ancora un posto disponibile)"
                            oResource.setLabel_To_Value(Me.LBmaxIscritti, "LBmaxIscritti." & CType(maxIscritti_code.postoLibero, maxIscritti_code))
                            Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text.Replace("#%%#", oComunita.MaxIscritti)
                        End If
                    End If
                    'If Me.LBmaxIscritti.Text <> "" And RuoloDefault <> "" Then
                    '    Me.LBmaxIscritti.Text = Me.LBmaxIscritti.Text & " - Limite posto sul ruolo [<b>" & RuoloDefault & "</b>]"
                    'End If
                End If

                If oComunita.CanUnsubscribe And oComunita.CanSubscribe Then
                    oResource.setLabel_To_Value(LBiscrizioni, "LBiscrizioni." & TipoIscrizione.IscriviDeiscrivi)
                ElseIf oComunita.CanSubscribe Then
                    oResource.setLabel_To_Value(Me.LBiscrizioni, "LBiscrizioni." & TipoIscrizione.Iscrivi_NonDeiscrivi)
                ElseIf oComunita.CanUnsubscribe Then
                    oResource.setLabel_To_Value(LBiscrizioni, "LBiscrizioni." & TipoIscrizione.NonIscrivi_Deiscrivi)
                Else
                    oResource.setLabel_To_Value(LBiscrizioni, "LBiscrizioni." & TipoIscrizione.NonIscrivi_NonDeiscivi)
                End If

                Me.LBoverIscrizioni_t.Visible = (oComunita.OverMaxIscritti <> 0)
                Me.LBoverIscrizioni.Visible = Me.LBoverIscrizioni_t.Visible

                If oComunita.OverMaxIscritti <= 0 Then
                    oResource.setLabel_To_Value(Me.LBoverIscrizioni, "LBoverIscrizioni." & OverSubscriptions.Nessuna)
                ElseIf oComunita.OverMaxIscritti = 1 Then
                    oResource.setLabel_To_Value(Me.LBoverIscrizioni, "LBoverIscrizioni." & OverSubscriptions.Una)
                Else
                    oResource.setLabel_To_Value(Me.LBoverIscrizioni, "LBoverIscrizioni." & OverSubscriptions.Molte)
                    Me.LBoverIscrizioni.Text = Replace(Me.LBoverIscrizioni.Text, "#%%#", oComunita.OverMaxIscritti)
                End If

                Me.LBaccessiSpeciali.Text = Me.oResource.getValue("AccessoCopisteria." & oComunita.HasAccessoCopisteria)
                If oComunita.HasAccessoLibero Then
                    Me.LBaccessiSpeciali.Text &= Me.oResource.getValue("AccessoLibero." & oComunita.HasAccessoLibero)
                End If



                Dim ShowTab As Boolean = False
                Me.Bind_infoEsclusioneComunita(CMNT_Id)
                With oComunita
                    Dim hasApertura As Boolean = False
                    Dim hasChiusura As Boolean = False

                    Me.LBNome.Text = .Nome
                    Me.LBtipocomunita.Text = .TipoComunita.Descrizione

                    oResource.setLabel_To_Value(LBisChiusa, "LBisChiusa." & .IsChiusa.ToString)

                    Try
                        If Not (Equals(.DataCreazione, New Date) Or IsNothing(.DataCreazione) Or IsDate(.DataCreazione) = False) Then
                            hasApertura = True
                            Me.LBApertura.Text = .DataCreazione
                        Else
                            Me.LBApertura.Text = "//"
                        End If
                    Catch ex As Exception
                        Me.LBApertura.Text = "//"
                    End Try
                    Try
                        If Not (Equals(.DataCessazione, New Date) Or IsNothing(.DataCessazione) Or IsDate(.DataCessazione) = False) Then
                            hasChiusura = True
                            Me.LBChiusura.Text = FormatDateTime(.DataCessazione, DateFormat.ShortDate)

                        End If
                    Catch ex As Exception

                    End Try
                    Me.LBChiusura.Visible = hasChiusura
                    Me.LB_chiusura.Visible = hasChiusura
                    Me.TBRdataAperturaChiusura.Visible = hasApertura Or hasChiusura
                    Me.TBRdataAperturaChiusura_sep.Visible = hasApertura Or hasChiusura


                    Me.TBRdataIscrizioni.Visible = True
                    Try
                        If Not (Equals(.DataInizioIscrizione, New Date) Or IsNothing(.DataInizioIscrizione) Or IsDate(.DataInizioIscrizione) = False) Then
                            Me.LBinizioIscrizione.Text = FormatDateTime(.DataInizioIscrizione, DateFormat.ShortDate)
                        Else
                            Me.LBinizioIscrizione.Text = "//"
                        End If
                    Catch ex As Exception
                        Me.LBinizioIscrizione.Text = "//"
                    End Try

                    Me.LB_fineIscrizione.Visible = False
                    Me.LBfineIscrizione.Visible = False
                    Try
                        If Not (Equals(.DataFineIscrizione, New Date) Or IsNothing(.DataFineIscrizione) Or IsDate(.DataFineIscrizione) = False) Then
                            Me.LBfineIscrizione.Text = FormatDateTime(.DataFineIscrizione, DateFormat.ShortDate)
                            Me.LB_fineIscrizione.Visible = True
                            Me.LBfineIscrizione.Visible = True
                        End If
                    Catch ex As Exception

                    End Try

                    'archiviata/bloccata
                    If .Archiviata = True Then
                        Me.TBRarchiviata.Visible = True
                        Me.TBRarchiviata_sep.Visible = True
                        Me.LBarchiviata.Text = oResource.getValue("archiviata")
                    Else
                        Me.TBRarchiviata.Visible = False
                        Me.TBRarchiviata_sep.Visible = False
                    End If
                    If .Bloccata = True Then
                        Me.TBRbloccata.Visible = True
                        Me.TBRbloccata_sep.Visible = True
                        Me.LBbloccata.Text = oResource.getValue("bloccata")
                    Else
                        Me.TBRbloccata.Visible = False
                        Me.TBRbloccata_sep.Visible = False
                    End If




                    Me.TBRtab.Visible = ShowTab

                    Me.TBSmenu.SelectedIndex = 0
                    Me.TBLprincipale.Visible = True
                End With
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Bind_infoEsclusioneComunita(ByVal CMNT_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet
        Dim Quote As String = """"
        Try
            Dim i, totale As Integer
            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"

            oComunita.Id = CMNT_ID
            oDataset = oComunita.ElencaComunitaEscluse(Session("objPersona").id, Session("LinguaID"))

            totale = oDataset.Tables(0).Rows.Count - 1

            Dim oTable As New Table
            Dim oTableRow As New TableRow
            Dim oTableCell As New TableCell

            Me.LBinfoIscrizione.Controls.Clear()
            If totale >= 0 Then
                oTableCell.Text = oResource.getValue("infoIscrizione")
                oTableCell.ColumnSpan = 3
                oTableRow.Cells.Add(oTableCell)
                oTable.Rows.Add(oTableRow)

                Me.TBRinfoiscrizione.Visible = True
            Else
                Me.TBRinfoiscrizione.Visible = False
            End If

            For i = 0 To totale
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)

                oTableRow = New TableRow
                oTableCell = New TableCell

                If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                    oRow.Item("CMNT_AnnoAccademico") = "&nbsp;"
                End If
                If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                    oRow.Item("PRDO_descrizione") = "&nbsp;"
                End If

                If IsDBNull(oRow.Item("TPCM_icona")) Then
                    oRow.Item("TPCM_icona") = ""
                Else
                    img = oRow.Item("TPCM_icona")
                    img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                    oRow.Item("TPCM_icona") = img
                End If

                Dim CMNT_Responsabile, CMNT_Creatore As String

                If Not IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    CMNT_Responsabile = oRow.Item("CMNT_Responsabile")
                Else
                    CMNT_Responsabile = ""
                End If

                If Not IsDBNull(oRow.Item("AnagraficaCreatore")) Then
                    CMNT_Creatore = oRow.Item("AnagraficaCreatore")
                Else
                    CMNT_Creatore = ""
                End If


                oTableCell.CssClass = "top"
                oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(30)
                If oRow.Item("TPCM_icona") <> "" Then
                    oTableCell.Text = "<img src=" & Quote & oRow.Item("TPCM_icona") & Quote & " border=0>"
                Else
                    oTableCell.Text = "&nbsp;"
                End If
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.CssClass = "top"
                oTableCell.Text = "<img src=" & Quote & GetPercorsoApplicazione(Me.Request) & "/RadControls/TreeView/Skins/Comunita/" & oResource.getValue("stato.image." & oRow.Item("CMNT_isChiusa")) & Quote & " border=0>"
                If CMNT_Responsabile = "" And CMNT_Creatore = "" Then
                    oTableCell.Text = oTableCell.Text & "&nbsp;" & oRow.Item("CMNT_Nome")
                ElseIf CMNT_Responsabile <> "" Then
                    oTableCell.Text = oTableCell.Text & "&nbsp;" & oRow.Item("CMNT_Nome") & " (" & CMNT_Responsabile & ")"
                ElseIf CMNT_Creatore <> "" Then
                    oTableCell.Text = oTableCell.Text & "&nbsp;" & oRow.Item("CMNT_Nome") & " (" & CMNT_Creatore & ")"
                End If

                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(80)
                oTableCell.CssClass = "top"

                Try
                    If oRow.Item("RLPC_TPRL_ID") > 0 Then
                        oTableCell.Text = "<b>" & oResource.getValue("infoIscrizione.True") & "</b>"
                    Else
                        oTableCell.Text = oResource.getValue("infoIscrizione.False")
                    End If
                Catch ex As Exception
                    oTableCell.Text = oResource.getValue("infoIscrizione.False")
                End Try
                If oTableCell.Text = "" Then
                    oTableCell.Text = "&nbsp;"
                End If
                oTableRow.Cells.Add(oTableCell)
                oTable.Rows.Add(oTableRow)
            Next
            If totale >= 0 Then
                Me.LBinfoIscrizione.Controls.Add(oTable)
                Me.TBRinfoiscrizione.Visible = True
                Me.TBRinfoiscrizione_sep.Visible = True
            Else
                Me.TBRinfoiscrizione.Visible = False
            End If
        Catch ex As Exception
            Me.TBRinfoiscrizione.Visible = False
            Me.TBRinfoiscrizione_sep.Visible = False
        End Try
    End Sub

#Region "Internazionalizzazione"
    ' Inizializzazione oggetto risorse: SEMPRE DA FARE
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_DettagliComunita"
        oResource.Folder_Level1 = "UC"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LB_Nome)
            .setLabel(Me.LB_tipocomunita)
            .setLabel(Me.LBresponsabile_c)
            .setLabel(Me.LBcreatore_c)
            .setLabel(Me.LBmaxIscritti_c)
            .setLabel(Me.LBiscritti_c)
            .setLabel(Me.LB_isChiusa)
            .setLabel(Me.LB_apertura)

            .setLabel(Me.LB_chiusura)
            .setLabel(Me.LB_inizioIscrizione)
            .setLabel(Me.LB_fineIscrizione)

          
            .setLabel(Me.LBiscrizioni_t)
            .setLabel(Me.LBoverIscrizioni_t)
            .setLabel(Me.LBinfoIscrizione_t)

 

            TBSmenu.Tabs(1).Text = .getValue("TABdettagli.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABdettagli.ToolTip")
            TBSmenu.Tabs(0).Text = .getValue("TABprincipale.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("TABprincipale.ToolTip")
            .setLabel(Me.LBarchiviata_t)
            .setLabel(Me.LBbloccata_t)
        End With
    End Sub
#End Region

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        If Me.TBSmenu.SelectedIndex = 0 Then
            Me.TBLprincipale.Visible = True
            Me.Bind_infoEsclusioneComunita(Me.HDNcomunitaID.Value)
        Else
            Me.TBLprincipale.Visible = False
        End If
    End Sub
End Class
