Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona

Public Class AdminG_ModificaComunita
    Inherits System.Web.UI.Page

#Region "Associazione"
    Protected WithEvents PNLAssociaComunita As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLAssociazione As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNomeComunita As System.Web.UI.WebControls.Label
    Protected WithEvents DGComunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents HDabilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDtutti As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LKBInserisci As System.Web.UI.WebControls.LinkButton

    Protected WithEvents DGComunitaAssociate As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoRecordAssociate As System.Web.UI.WebControls.Label
    Protected WithEvents LBnoCmnt As System.Web.UI.WebControls.Label
    Protected WithEvents LKBAnnulla As System.Web.UI.WebControls.LinkButton
    Protected WithEvents BTNsi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNno As System.Web.UI.WebControls.Button

    Protected WithEvents DDLfiltroOrganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLfiltroTipocomunita As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLfiltroTipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLfiltroAnnoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLfiltroPeriodo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TBRfiltroCorso As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltroCorsoDiStudi As System.Web.UI.WebControls.TableRow
#End Region

#Region "noquerystring"
    Protected WithEvents BTNlistacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents BTNricercacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents LBnoquery As System.Web.UI.WebControls.Label
    Protected WithEvents PNLnoquery As System.Web.UI.WebControls.Panel
#End Region

#Region "Pannello Generale"
    Protected WithEvents PNLComunita As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnomeComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents TXBCmntNome As System.Web.UI.WebControls.TextBox
    Protected WithEvents HDidPadre As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDorgn As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDtpcm As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDcrds As System.Web.UI.HtmlControls.HtmlInputHidden


    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoComunita As System.Web.UI.WebControls.Label

    Protected WithEvents LBdataTermine_c As System.Web.UI.WebControls.Label
    Protected WithEvents TXBCmntdataTermine As System.Web.UI.WebControls.TextBox
    Protected WithEvents datatermine As System.Web.UI.WebControls.CompareValidator

    Protected WithEvents LBdataInizioIscr As System.Web.UI.WebControls.Label
    Protected WithEvents TXBCmntdataInIscriz As System.Web.UI.WebControls.TextBox
    Protected WithEvents RFVdataInizioIscr As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents LBdataFineIscr As System.Web.UI.WebControls.Label
    Protected WithEvents TXBCmntdataFineIscriz As System.Web.UI.WebControls.TextBox
    Protected WithEvents RFVdataFineIscr As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents CMVdateIscrizione As System.Web.UI.WebControls.CompareValidator


    Protected WithEvents LBstatuto_c As System.Web.UI.WebControls.Label
    Protected WithEvents TXBCmntStatuto As System.Web.UI.WebControls.TextBox

    Protected WithEvents RBapertachiusa As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents LBmaxIscritti_c As System.Web.UI.WebControls.Label
    Protected WithEvents TXBmaxIscritti As System.Web.UI.WebControls.TextBox
    Protected WithEvents CHBillimitati As System.Web.UI.WebControls.CheckBox



#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBlegenda As System.Web.UI.WebControls.Label

    Protected WithEvents Requiredfieldvalidator As System.Web.UI.WebControls.RequiredFieldValidator





    Protected WithEvents TXBcodice As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TXBore As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ore As System.Web.UI.WebControls.RangeValidator
    ' Protected WithEvents TXBanno As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rangeValDate As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents DDLPeriodo As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents RBattivazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TXBprerequisiti As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBobbiettivo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TXBcrediti As System.Web.UI.WebControls.TextBox
    Protected WithEvents crediti As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBdescrizioneprogramma As System.Web.UI.WebControls.TextBox
    Protected WithEvents descrizionerequi As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PNLcorso As System.Web.UI.WebControls.Panel
    Protected WithEvents TXBconferenzaInizio As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaInizio As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBconferenzaFine As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaFine As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents VLDconferenzaInizioFine As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents TXBconferenzaInizioArticolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaInizioArticolo As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBconferenzaFineArticolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaFineArticolo As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents VLDconferenzaInizioFineArticolo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents TXBconferenzaFineRevisione As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaFineRevisione As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBConferenzaInizioDefinizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDconferenzaInizioDefinizione As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBConferenzaFineDefinizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDConferenzaFineDefinizione As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents VLDConferenzaInizioFineDefinizione As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents TXBConferenzaTempoModeratore As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDConferenzaTempoModeratore As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBConferenzaTempoRevisore As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDConferenzaTempoRevisiore As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents PNLconferenza As System.Web.UI.WebControls.Panel
    ' Protected WithEvents DDLCorsoStudiAttivo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBCorsoStudioNumEsami As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDCorsoStudioNumEsami As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents TXBCorsoStudioMIUR As System.Web.UI.WebControls.TextBox
    Protected WithEvents VLDCorsoStudioMIUR As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TXBCorsoStudioAttributo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBCorsoStudioAttributo2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDLCorsoStudiTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLCorsoStudi As System.Web.UI.WebControls.Panel

    Protected WithEvents LBAvviso As System.Web.UI.WebControls.Label
    Protected WithEvents VLDSum As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNCmntModifica As System.Web.UI.WebControls.Button
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents BTNok As System.Web.UI.WebControls.Button
    Protected WithEvents PNLmessaggio As System.Web.UI.WebControls.Panel

    Protected WithEvents DDLanno As System.Web.UI.WebControls.DropDownList

    Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button
    Protected WithEvents LKBassociaPadri As System.Web.UI.WebControls.LinkButton


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

        Me.Master.ServiceTitle = "Modifica Comunità"
        Dim oPersona As New COL_Persona
        Me.CHBillimitati.Attributes.Add("onclick", "Controlla(this);return true;")
        Try
            oPersona = Session("objPersona")

            If Session("idComunita_forAdmin") Is Nothing Then
                Me.PNLcontenuto.Visible = False
                Me.PNLnoquery.Visible = True
                Me.LBnoquery.Text = "Si è verificato un Errore nell'accesso alla pagina."
            Else
                LBlegenda.Text = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
            End If

        Catch ex As Exception
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "Si è verificato un Errore nell'accesso alla pagina."  '"Per qualche motivo hai perso la QueryString ritenta"
        End Try

        If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Then
            Me.PNLpermessi.Visible = False

            Try
                If Not Page.IsPostBack Then
                    Me.SetStartupScript()
                    Me.Bind_Dettagli()
                    Me.Bind_FiltriComunita()
                End If
                'If Session("idComunita_forAdmin") Is Nothing Then
                '    Me.BTNannulla.Visible = False
                'Else
                '    Me.BTNannulla.Visible = True
                'End If

                If Me.CHBillimitati.Checked = True Then
                    Me.TXBmaxIscritti.Enabled = False
                Else
                    Me.TXBmaxIscritti.Enabled = True
                End If

            Catch ex As Exception
                ' Response.Redirect("./GestioneComunita.aspx")
                'ritorna da dove sono venuto
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
                Me.LBNopermessi.Text = "Non si dispone dei permessi necessari per l'utilizzo del servizio"
            End Try
        Else
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
            Me.LBNopermessi.Text = "Non si dispone dei permessi necessari per l'utilizzo del servizio"
        End If

    End Sub

    Private Sub SetStartupScript()
        Me.TXBCmntStatuto.Attributes.Add("onkeypress", "return(LimitText(this,4000));")
        Me.TXBdescrizioneprogramma.Attributes.Add("onkeypress", "return(LimitText(this,200));")
        Me.TXBCmntNome.Attributes.Add("onkeypress", "return(LimitText(this,100));")

        LKBassociaPadri.Attributes.Add("onclick", "window.status='Aggiunta padri' ;return true;")
        LKBassociaPadri.Attributes.Add("onmouseover", "window.status='Aggiunta padri' ;return true;")
        LKBassociaPadri.Attributes.Add("onfocus", "window.status='Aggiunta padri' ;return true;")
        LKBassociaPadri.Attributes.Add("onmouseout", "window.status='';return true;")

        LKBAnnulla.Attributes.Add("onclick", "window.status='Annulla' ;return true;")
        LKBAnnulla.Attributes.Add("onmouseover", "window.status='Annulla' ;return true;")
        LKBAnnulla.Attributes.Add("onfocus", "window.status='Annulla' ;return true;")
        LKBAnnulla.Attributes.Add("onmouseout", "window.status='';return true;")

        LKBInserisci.Attributes.Add("onclick", "window.status='Inserisci padri' ;return true;")
        LKBInserisci.Attributes.Add("onmouseover", "window.status='Inserisci padri' ;return true;")
        LKBInserisci.Attributes.Add("onfocus", "window.status='Inserisci padri' ;return true;")
        LKBInserisci.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.CHBillimitati.Attributes.Add("onclick", "Controlla(this);return true;")
    End Sub

#Region "Bind_Filtri Comunità Padri"
    Private Sub Bind_FiltriComunita()
        Me.Bind_FiltroOrganizzazione()
        Me.Bind_FiltroTipiComunita()
    End Sub

    Private Sub Bind_FiltroOrganizzazione()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLfiltroOrganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oComunita As New COL_Comunita

                Dim ArrComunita(,) As String
                Dim ORGN_ID As Integer
                Dim show As Boolean = False
                Try

                    If IsArray(Session("ArrComunita")) And Session("limbo") = False Then
                        ArrComunita = Session("ArrComunita")
                        oComunita.Id = ArrComunita(0, 0)
                        oComunita.Estrai()
                        ORGN_ID = oComunita.Organizzazione.Id
                        show = False
                    ElseIf Session("limbo") = True Then
                        show = True
                        ORGN_ID = -1
                    End If
                Catch ex As Exception
                    Try
                        ORGN_ID = Session("ORGN_id")
                        show = False
                    Catch exc As Exception
                        ORGN_ID = -1
                    End Try

                End Try

                Me.DDLfiltroOrganizzazione.DataValueField = "ORGN_id"
                Me.DDLfiltroOrganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLfiltroOrganizzazione.DataSource = oDataset
                Me.DDLfiltroOrganizzazione.DataBind()
                If oDataset.Tables(0).Rows.Count > 1 Then
                    Me.DDLfiltroOrganizzazione.Items.Insert(0, New ListItem("<< tutte >>", -1))
                End If

                If Me.DDLfiltroOrganizzazione.Items.Count > 1 Then
                    Me.DDLfiltroOrganizzazione.Enabled = True

                    If ORGN_ID >= 0 Then
                        Try
                            Me.DDLfiltroOrganizzazione.SelectedValue = ORGN_ID
                        Catch ex As Exception
                            Me.DDLfiltroOrganizzazione.Items.Clear()
                            Me.DDLfiltroOrganizzazione.Items.Add(New ListItem(oComunita.Nome, ORGN_ID))
                            Me.DDLfiltroOrganizzazione.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLfiltroOrganizzazione.SelectedIndex = 0
                    End If
                Else
                    Me.DDLfiltroOrganizzazione.Enabled = False
                End If
            Else
                Me.DDLfiltroOrganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLfiltroOrganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLfiltroOrganizzazione.Items.Clear()
            Me.DDLfiltroOrganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLfiltroOrganizzazione.Enabled = False
        End Try
    End Sub
    Private Sub Bind_FiltroTipiComunita()
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            Me.DDLfiltroTipocomunita.Items.Clear()
            oDataSet = COL_Tipo_Comunita.ElencaForFiltri(Session("LinguaID"), False)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                Me.DDLfiltroTipocomunita.DataSource = oDataSet
                Me.DDLfiltroTipocomunita.DataTextField() = "TPCM_descrizione"
                Me.DDLfiltroTipocomunita.DataValueField() = "TPCM_id"
                Me.DDLfiltroTipocomunita.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                Me.DDLfiltroTipocomunita.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            Me.DDLfiltroTipocomunita.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try

    End Sub
   

    Private Sub DDLfiltroOrganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroOrganizzazione.SelectedIndexChanged
        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub DDLfiltroPeriodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroPeriodo.SelectedIndexChanged
        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub DDLfiltroAnnoAccademico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroAnnoAccademico.SelectedIndexChanged
        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub DDLfiltroTipocomunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroTipocomunita.SelectedIndexChanged
      
            Me.TBRfiltroCorsoDiStudi.Visible = False
            Me.TBRfiltroCorso.Visible = False
            Me.DDLfiltroTipoCorsoDiStudi.SelectedValue = -1
            Me.DDLfiltroPeriodo.SelectedValue = -1
            Me.DDLfiltroAnnoAccademico.SelectedValue = -1

        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub DDLfiltroTipoCorsoDiStudi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLfiltroTipoCorsoDiStudi.SelectedIndexChanged
        Me.Bind_GrigliaDaAssociare()
    End Sub
#End Region

#Region "Bind_form e DDL"

    Public Sub Bind_Dettagli()
        Dim oComunita As New COL_Comunita

        Try
            oComunita.Id = Session("idComunita_forAdmin")
            oComunita.Estrai()

            Me.PNLComunita.Visible = True
            Me.HDorgn.Value = oComunita.Organizzazione.Id
            Me.HDtpcm.Value = oComunita.TipoComunita.ID
          
                    Me.TXBCmntStatuto.Text = oComunita.Statuto
                    Me.LBtipoComunita.Text = oComunita.TipoComunita.Descrizione
                    Me.TXBCmntNome.Text = oComunita.Nome
                    Me.HDidPadre.Value = oComunita.IdPadre
                    Me.RBapertachiusa.SelectedValue = oComunita.IsChiusa.ToString
                    If Not Equals(oComunita.DataInizioIscrizione, New Date) Then
                        Me.TXBCmntdataInIscriz.Text = oComunita.DataInizioIscrizione
                    End If
                    If Not Equals(oComunita.DataFineIscrizione, New Date) Then
                        Me.TXBCmntdataFineIscriz.Text = oComunita.DataFineIscrizione
                    End If
                    If Not Equals(oComunita.DataCessazione, New Date) Then
                        Me.TXBCmntdataTermine.Text = oComunita.DataCessazione
                    End If
                    If oComunita.MaxIscritti = "0" Then
                        Me.CHBillimitati.Checked = True
                    Else
                        Me.TXBmaxIscritti.Text = oComunita.MaxIscritti
                        Me.CHBillimitati.Checked = False
                    End If

        Catch ex As Exception
            Me.ReturnToSearchPage()
        End Try

    End Sub

#End Region

    Private Sub ReturnToSearchPage()
        Try
            If Me.Request.QueryString("from") = "" Then
                Me.Response.Redirect("./AdminG_ListaComunita.aspx")
            Else
                Select Case LCase(Me.Request.QueryString("from"))
                    Case "ricercacomunita"
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx?re_set=true")
                    Case "ricercabypersona"
                        Me.Response.Redirect("./AdminG_RicercaComunita.aspx?re_set=true")
                    Case Else
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx")
                End Select
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "MODIFICA "

    Private Sub BTNCmntModifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCmntModifica.Click
        If Me.TXBmaxIscritti.Text <> "" Or Me.CHBillimitati.Checked = True Then
            Dim oComunita As New COL_Comunita
            Dim idPadre As Integer

            Try
                idPadre = Me.HDidPadre.Value
               
                        With oComunita
                            .Id = Session("idComunita_forAdmin")
                            oComunita.Estrai()

                            .Organizzazione.Id = Me.HDorgn.Value
                            .TipoComunita.ID = Me.HDtpcm.Value
                            .Nome = Replace(Me.TXBCmntNome.Text, vbCrLf, " ")
                            If TXBCmntdataTermine.Text <> "" Then
                                .DataCessazione = TXBCmntdataTermine.Text
                            End If
                            .DataInizioIscrizione = TXBCmntdataInIscriz.Text
                            If TXBCmntdataFineIscriz.Text <> "" Then
                                .DataFineIscrizione = TXBCmntdataFineIscriz.Text
                            End If
                            If TXBCmntStatuto.Text <> "" Then
                                .Statuto = TXBCmntStatuto.Text
                            End If
                            .IsChiusa = Me.RBapertachiusa.SelectedValue
                            .IdPadre = idPadre
                            If Me.CHBillimitati.Checked = True Then
                                .MaxIscritti = "0"
                            Else
                                .MaxIscritti = Me.TXBmaxIscritti.Text
                            End If
                        End With
                        Try
                            oComunita.Modifica()
                        Catch ex As Exception

                        End Try
                        If oComunita.Errore = Errori_Db.DBChange Or oComunita.Id = "-1" Then
                            Me.BTNCmntModifica.Visible = False
                            Me.PNLComunita.Visible = False
                            Me.PNLconferenza.Visible = False
                            Me.PNLcorso.Visible = False
                            Me.PNLCorsoStudi.Visible = False
                            PNLmessaggio.Visible = True
                            Me.LBmessaggio.Text = "Spiacenti si è verificato un Errore nella modifica della Comunità."
                        Else
                            Me.ReturnToSearchPage()
                        End If

            Catch ex As Exception
                Me.BTNCmntModifica.Visible = False
                Me.PNLComunita.Visible = False
                Me.PNLconferenza.Visible = False
                Me.PNLcorso.Visible = False
                Me.PNLCorsoStudi.Visible = False
                PNLmessaggio.Visible = True
                Me.LBmessaggio.Text = "Spiacenti si è verificato un Errore nella modifica della Comunità."
            End Try
        End If
    End Sub
    'Private Sub BTNok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNok.Click
    '    If Request.QueryString("CMNT_ID") Is Nothing Then

    '    Else
    '        Response.Redirect("./GestioneComunita.aspx")
    '    End If
    'End Sub

    Private Sub BTNannulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNannulla.Click
        Me.ReturnToSearchPage()
    End Sub

    Private Sub LKBassociaPadri_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBassociaPadri.Click

        Session("azione") = "associa"
        If Session("idComunita_forAdmin") Is Nothing Then
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "Per qualche motivo hai perso  ritenta"
        Else

            Me.PNLcontenuto.Visible = True
            Me.PNLpermessi.Visible = False
            If Session("azione") = "associa" Then

                ' Session("idComunitaCreata") = Session("idComunita_forAdmin")
                Me.LBNomeComunita.Text = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
                LBlegenda.Text = Me.LBNomeComunita.Text

                Me.Bind_GrigliaDaAssociare()
                Bind_GrigliaAssociate()

                NascondiPanel()
                Me.PNLAssociaComunita.Visible = True


            End If
        End If
    End Sub
#End Region

    Private Sub BTNlistacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlistacmnt.Click
        Response.Redirect("./adming_listacomunita.aspx")
    End Sub
    Private Sub BTNricercacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNricercacmnt.Click
        Response.Redirect("./adming_ricercacomunita.aspx")
    End Sub


#Region "Associa Padri"

    Private Sub NascondiPanel()
        Me.PNLComunita.Visible = False
        Me.PNLconferenza.Visible = False
        Me.PNLcorso.Visible = False
        Me.PNLCorsoStudi.Visible = False
        Me.PNLmessaggio.Visible = False
        Me.PNLnoquery.Visible = False
        Me.PNLpermessi.Visible = False

        Me.LKBassociaPadri.Visible = False
        Me.BTNannulla.Visible = False
        Me.BTNCmntModifica.Visible = False
    End Sub

    Private Sub LKBAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBAnnulla.Click

        If Session("azione") = "associa" Then
            Session("azione") = Nothing
            Me.ReturnToSearchPage()
            'If Session("idComunita_forAdmin") Is Nothing Then
            '    Me.PNLcontenuto.Visible = False
            '    Me.PNLnoquery.Visible = True
            '    Me.LBnoquery.Text = "errore non grave riprova"
            'ElseIf Request.QueryString("FROM") = "ricercabypersona" Then
            '    Response.Redirect("./adming_ricercacomunita.aspx")
            'Else
            '    Me.PNLcontenuto.Visible = False
            '    Me.PNLnoquery.Visible = True
            '    Me.LBnoquery.Text = "errore non grave di posizionamento riprova"
            'End If
        End If

    End Sub

    Private Sub LKBInserisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBInserisci.Click
        'assegna i nuovi padri alla comunità

        Dim oComunita As New COL_Comunita
        oComunita.Id = Session("idComunita_forAdmin") 'id della comunità 

        Try

            oComunita.AssociaPadri(Me.HDabilitato.Value)
            DGComunita.CurrentPageIndex = 0
            DGComunitaAssociate.CurrentPageIndex = 0
            Me.Bind_GrigliaDaAssociare() 'rebindo per aggiornare i dati
            Bind_GrigliaAssociate()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNsi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNsi.Click
        NascondiPanel()
        PNLAssociaComunita.Visible = True
        Me.LBNomeComunita.Text = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub BTNno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNno.Click
        'sono da mettere gli altri else che si faranno
        If Session("idComunita_forAdmin") Is Nothing Then
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "errore non grave di posizionamento riprova"
        ElseIf Request.QueryString("FROM") = "ricercabypersona" Then
            Response.Redirect("./adming_ricercacomunita.aspx")
        Else
            Me.PNLcontenuto.Visible = False
            Me.PNLnoquery.Visible = True
            Me.LBnoquery.Text = "errore non grave di posizionamento riprova"
        End If
    End Sub

#Region "Gestione griglia_CMNT da associare"

    Private Sub Bind_GrigliaDaAssociare()

        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            Dim oServizioAmministraComunita As New UCServices.Services_AmministraComunita
            Dim CMNT_ID As Integer = Session("idComunita_forAdmin") 'id della comunità appena creata o di quella passata da querystring
            Dim Codice, Posizione As String

            oPersona = Session("objPersona")
            ''carico la lista delle comunità     !!! ATTENZIONE METTERE VALORI REALI !!!
            'oDataset = oComunita.ElencaPadriDirettiByPermesso(12, oPersona.Id, 2, CMNT_ID)

            Codice = UCServices.Services_AmministraComunita.Codex
            Posizione = CType(UCServices.Abstract.MyServices.PermissionType.Change, UCServices.Abstract.MyServices.PermissionType)
            oDataset = oComunita.ElencaPossibiliPadriByServizio(Codice, oPersona.Id, Posizione, CMNT_ID, Me.DDLfiltroOrganizzazione.SelectedValue, Me.DDLfiltroTipocomunita.SelectedValue, Me.DDLfiltroTipoCorsoDiStudi.SelectedValue)
            oDataset.Tables(0).Columns.Add(New DataColumn("oCheckAbilitato"))

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                DGComunita.Visible = False
                Me.LBnoCmnt.Visible = True
                Me.LBNomeComunita.Text = "Nessun padre associabile alla comunità creata"
                Me.LKBInserisci.Visible = False
            Else
                Me.LBNomeComunita.Text = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
                Me.DGComunita.Visible = True
                Me.LBnoCmnt.Visible = False
                Me.LKBInserisci.Visible = True
                'cicla gli elementi del dataset e prepara i dati per la successiva visualizzazione nella datagrid

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)
                    If viewstate("RipristinaCheck") <> "si" Then
                        Me.HDtutti.Value += oRow.Item("CMNT_id") & ","
                    Else
                        Dim t As Integer
                        Dim selezionato() As String
                        selezionato = Me.HDabilitato.Value.Split(",")
                        For t = 1 To selezionato.Length - 2
                            If oRow.Item("CMNT_id") = selezionato(t) Then
                                oRow.Item("oCheckAbilitato") = "checked"
                                Exit For
                            End If
                        Next
                    End If

                    'icona relativa al tipo comunità
                    oRow.Item("TPCM_Icona") = "./../" & oRow.Item("TPCM_Icona")
                Next

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "CMNT_dataCreazione"
                    viewstate("SortDirection") = "desc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")


                DGComunita.DataSource = oDataview
                DGComunita.DataBind()
                'se arriva fin qui mostro la datagrid
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore

            DGComunita.Visible = False
            Me.LBnoCmnt.Visible = True
            Me.LBNomeComunita.Text = "Nessun padre associabile alla comunità creata"
            Me.LKBInserisci.Visible = False

        End Try
    End Sub

    Private Sub DGComunitaDaAssociare_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")
            If oSortDirection = "asc" Then
                oText = "5"
            Else
                oText = "6"
            End If

            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelAfter.font.name = "webdings"
                    oLabelAfter.font.size = FontUnit.XSmall
                    oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            If oSortDirection = "asc" Then
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescentep per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                            Else
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                            End If

                            oLabelAfter.font.name = oLinkbutton.Font.Name
                            oLabelAfter.font.size = oLinkbutton.Font.Size
                            oLabelAfter.text = oLinkbutton.Text & " "
                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        Catch ex As Exception
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")

                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oCell.Controls.AddAt(0, oLabelBefore)
                        oCell.Controls.Add(oLabelAfter)
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "smsStyle_PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            'riferimento alla cella corrente - aggiungo il testo 
            ' occupo tutto le colonne 
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count

            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oLinkbutton As LinkButton
            Try
                Dim oCell As New TableCell
                Dim oWebControl As WebControl

                oCell = CType(e.Item.Cells(2), TableCell)
                oWebControl = oCell.Controls(0)
                Try

                    oLinkbutton = oWebControl

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")

                Catch ex As Exception

                End Try
                oCell = New TableCell
                oCell = CType(e.Item.Cells(3), TableCell)
                oWebControl = oCell.Controls(0)
                Try

                    oLinkbutton = oWebControl

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='" & oLinkbutton.Text & "';return true;")

                Catch ex As Exception

                End Try

            Catch ex As Exception

            End Try

            If viewstate("RowSelected") <> Nothing Then
                Try

                    Dim RowSelected As String = viewstate("RowSelected")
                    Dim ItemFind As String = "," & CStr(e.Item.DataItem("CMNT_id")) & ","

                    If InStr(RowSelected, ItemFind) > 0 Then
                        e.Item.CssClass = "Righe_Disattivate_center"
                    End If
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Sub DGComunitaDaAssociare_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged

        DGComunita.CurrentPageIndex = e.NewPageIndex
        viewstate("RipristinaCheck") = "si"
        Me.Bind_GrigliaDaAssociare()

    End Sub

    Private Sub SortElencoDaAssociare(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        Me.Bind_GrigliaDaAssociare()
    End Sub

    Private Sub DGComunitaDaAssociare_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
        'Select Case e.CommandName
        '    Case "Associa"
        '        Dim oComunita As New COL_Comunita
        '        oComunita.Id = viewstate("IdComunita")
        '        oComunita.AssociaPadre(CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))) 'questo è l'id del padre
        '        If oComunita.Errore <> Errori_Db.None Then
        '            'errore
        '        Else
        '            If e.Item.CssClass = "Righe_Disattivate_center" Then 'significa che l'elemento era già selezionato e quindi deve essere deselezionato
        '            End If
        '            'disabilito la riga selezionata
        '            e.Item.CssClass = "Righe_Disattivate_center"
        '            'inserisco in una stringa gli id degli elementi selezionati
        '            If viewstate("RowSelected") = Nothing Then 'quando la lista degli id selezionati è vuota:
        '                viewstate("RowSelected") = "," & CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex)) & ","
        '            Else
        '                Dim RowSelected As String
        '                RowSelected = viewstate("RowSelected") & CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex)) & ","
        '                viewstate("RowSelected") = RowSelected
        '            End If
        '            'devo ricaricare la griglia per aggiornare la selezione deegli elementi
        '            Me.Bind_Griglia()
        '        End If
        'End Select
    End Sub
#End Region

#Region "Gestione griglia CMNT associate"

    Private Sub Bind_GrigliaAssociate()

        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")
            Dim CMNT_ID As Integer = Session("idComunita_forAdmin") 'id della comunità appena creata o di quella passata da querystring
            oComunita.Id = CMNT_ID
            'carico la lista delle comunità     !!! ATTENZIONE METTERE VALORI REALI !!!
            oDataset = oComunita.GetPadri(Session("LinguaID"))
            'oDataset.Tables(0).Columns.Add(New DataColumn("oCheckAbilitato"))

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                DGComunitaAssociate.Visible = False
                Me.LBnoRecordAssociate.Visible = True
            Else
                Me.DGComunitaAssociate.Visible = True
                Me.LBnoRecordAssociate.Visible = False
                'cicla gli elementi del dataset e prepara i dati per la successiva visualizzazione nella datagrid

                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    'If viewstate("RipristinaCheck") <> "si" Then
                    '    Me.HDtutti.Value += oRow.Item("CMNT_id") & ","
                    'Else
                    '    Dim t, b As Integer
                    '    Dim selezionato() As String
                    '    selezionato = Me.HDabilitato.Value.Split(",")
                    '    For t = 1 To selezionato.Length - 2
                    '        If oRow.Item("CMNT_id") = selezionato(t) Then
                    '            oRow.Item("oCheckAbilitato") = "checked"
                    '            Exit For
                    '        End If
                    '    Next
                    'End If

                    'icona relativa al tipo comunità
                    oRow.Item("TPCM_Icona") = "./../" & oRow.Item("TPCM_Icona")
                Next

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "CMNT_dataCreazione"
                    viewstate("SortDirection") = "desc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                DGComunitaAssociate.DataSource = oDataview
                DGComunitaAssociate.DataBind()
                'se arriva fin qui mostro la datagrid
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore

            DGComunitaAssociate.Visible = False
            Me.LBnoRecordAssociate.Visible = True
        End Try
    End Sub


    Private Sub DGComunitaAssociate_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunitaAssociate.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")
            If oSortDirection = "asc" Then
                oText = "5"
            Else
                oText = "6"
            End If

            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelAfter.font.name = "webdings"
                    oLabelAfter.font.size = FontUnit.XSmall
                    oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            If oSortDirection = "asc" Then
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescentep per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                            Else
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                            End If

                            oLabelAfter.font.name = oLinkbutton.Font.Name
                            oLabelAfter.font.size = oLinkbutton.Font.Size
                            oLabelAfter.text = oLinkbutton.Text & " "
                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        Catch ex As Exception
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")

                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oCell.Controls.AddAt(0, oLabelBefore)
                        oCell.Controls.Add(oLabelAfter)
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "smsStyle_PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            'riferimento alla cella corrente - aggiungo il testo 
            ' occupo tutto le colonne 
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count

            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oLinkbutton As LinkButton
            Try
                Dim oCell As New TableCell
                Dim oWebControl As WebControl

                oCell = CType(e.Item.Cells(2), TableCell)
                oWebControl = oCell.Controls(0)
                Try

                    oLinkbutton = oWebControl

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Vai alla comunità:" & Replace(oLinkbutton.Text, "'", "\'") & "';return true;")

                Catch ex As Exception

                End Try
                oCell = New TableCell
                oCell = CType(e.Item.Cells(3), TableCell)
                oWebControl = oCell.Controls(0)
                Try

                    oLinkbutton = oWebControl

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='" & oLinkbutton.Text & "';return true;")

                Catch ex As Exception

                End Try

            Catch ex As Exception

            End Try

            If viewstate("RowSelected") <> Nothing Then
                Try

                    Dim RowSelected As String = viewstate("RowSelected")
                    Dim ItemFind As String = "," & CStr(e.Item.DataItem("CMNT_id")) & ","

                    If InStr(RowSelected, ItemFind) > 0 Then
                        e.Item.CssClass = "Righe_Disattivate_center"
                    End If
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Private Sub SortElencoAssociate(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunitaAssociate.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        Me.Bind_GrigliaAssociate()
    End Sub

#End Region

#End Region


    'Private Sub LNBbackToRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBbackToRicerca.Click
    '    Try
    '        If Me.Request.QueryString("from") = "" Then
    '            Me.Response.Redirect("./AdminG_ListaComunita.aspx")
    '        Else
    '            Select Case LCase(Me.Request.QueryString("from"))
    '                Case "ricercacomunita"
    '                    Me.Response.Redirect("./AdminG_ListaComunita.aspx?re_set=true")
    '                Case "ricercabypersona"
    '                    Me.Response.Redirect("./AdminG_RicercaComunita.aspx")
    '                Case Else
    '                    Me.Response.Redirect("./AdminG_ListaComunita.aspx")
    '            End Select
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class

