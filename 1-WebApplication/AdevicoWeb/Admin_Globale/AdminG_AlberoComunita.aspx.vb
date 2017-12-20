Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports Telerik.WebControls

Public Class AdminG_AlberoComunita
    Inherits System.Web.UI.Page


    Protected Enum AzioneTree
        Aggiorna = 1
        Dettagli = 2
        Entra = 3
        Iscrivi = 4
        Modifica = 5
        Cancella = 6
        GestioneServizi = 7
        GestioneUtenti = 8
        LogonAs = 9
        LogonAsRuolo = 10
    End Enum

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

#Region "Filtro"
    Protected WithEvents TBRfiltro As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoFiltro As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRcorsi As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBCorganizzazione_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCorganizzazione As System.Web.UI.WebControls.TableCell
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
#End Region

#Region "FORM TreeView"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBaggiorna As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton
    ' Protected WithEvents LNBelenco As System.Web.UI.WebControls.LinkButton

    Protected WithEvents RBLvisualizza As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView
    Protected WithEvents HDN_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_Azione As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Form Dettagli"
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
    Protected WithEvents BTNnascondi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNentra As System.Web.UI.WebControls.Button
    Protected WithEvents BTNiscrivi As System.Web.UI.WebControls.Button
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "Form messaggio"
    Protected WithEvents PNLmessaggio As System.Web.UI.WebControls.Panel
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents LBtreeView As System.Web.UI.WebControls.Label
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
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

        Me.Master.ServiceTitle = "Elenco comunità"

        Dim oPersona As New COL_Persona

        Try
            Dim iCMNT_ID As Integer
            Dim iCMNT_PAth, elenco() As String

            If Page.IsPostBack = False Then
                'Se entro la prima volta
                oPersona = Session("objPersona")
                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Then
                    Me.SetupFiltri()
                    'Me.TBRfiltro.Visible = False
                    Select Case Me.Request.QueryString("show")
                        Case ""
                            Me.RBLvisualizza.SelectedValue = 1
                        Case Is = "1"
                            Me.RBLvisualizza.SelectedValue = 1
                        Case Is = "2"
                            Me.RBLvisualizza.SelectedValue = 2
                            Me.TBRfiltro.Visible = True
                        Case Else
                            Me.RBLvisualizza.SelectedValue = 1
                    End Select
                    Me.SetStartupScript()
                    Me.Bind_TreeView()
                Else
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                    Me.LBNopermessi.Text = "Non si dispone dei permessi necessari per accedere a questa pagina."
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function SetStartupScript()
        Me.LNBaggiorna.Attributes.Add("onclick", "window.status='Aggiorna albero comunita a cui si è iscritti.';return true;")
        Me.LNBaggiorna.Attributes.Add("onfocus", "window.status='Aggiorna albero comunita a cui si è iscritti.';return true;")
        Me.LNBaggiorna.Attributes.Add("onmouseover", "window.status='Aggiorna albero comunita a cui si è iscritti.';return true;")
        Me.LNBaggiorna.Attributes.Add("onmouseout", "window.status='';return true;")

        'Me.LNBelenco.Attributes.Add("onclick", "window.status='Visualizza elenco comunità a cui si è iscritti.';return true;")
        'Me.LNBelenco.Attributes.Add("onfocus", "window.status='Visualizza elenco comunità a cui si è iscritti.';return true;")
        'Me.LNBelenco.Attributes.Add("onmouseover", "window.status='Visualizza elenco comunità a cui si è iscritti.';return true;")
        'Me.LNBelenco.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LNBcomprimi.Attributes.Add("onclick", "window.status='Comprimi tutto.';CollapseAll();return false;")
        Me.LNBcomprimi.Attributes.Add("onfocus", "window.status='Comprimi tutto.';return true;")
        Me.LNBcomprimi.Attributes.Add("onmouseover", "window.status='Comprimi tutto.';return true;")
        Me.LNBcomprimi.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LNBespandi.Attributes.Add("onclick", "window.status='Espandi tutto.';ExpandAll();return false;")
        Me.LNBespandi.Attributes.Add("onfocus", "window.status='Espandi tutto.';return true;")
        Me.LNBespandi.Attributes.Add("onmouseover", "window.status='Espandi tutto.';return true;")
        Me.LNBespandi.Attributes.Add("onmouseout", "window.status='';return true;")

    End Function

#Region "Bind_Filtri"
    Private Sub SetupFiltri()

        Me.Bind_Organizzazioni()
        If Me.Request.QueryString("from") = "" Then
            Me.Bind_TipiComunita()
        Else
            Me.Bind_TipiComunita()
            Select Case LCase(Me.Request.QueryString("from"))
                Case "ricercacomunita"
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("ListaComunita")("organizzazione")
                    Catch ex As Exception
                        Me.Response.Cookies("ListaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
                    End Try
                
                    Me.SetupSearchParameters()
                Case "ricercabypersona"
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunita")("organizzazione")
                    Catch ex As Exception
                        Me.Response.Cookies("RicercaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
                    End Try
                 
                    Me.SetupSearchParameters()
            End Select
        End If

    End Sub

    Private Sub SetupSearchParameters()
        Dim stringaCookie As String

        Try

            If LCase(Me.Request.QueryString("from")) = "ricercacomunita" Then
                stringaCookie = "ListaComunita"

                Try
                    If IsNumeric(Me.Request.Cookies(stringaCookie)("annoAccademico")) Then
                        Try
                            Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies(stringaCookie)("annoAccademico")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try

                ' Setto il periodo
                Try
                    If IsNumeric(Me.Request.Cookies(stringaCookie)("periodo")) Then
                        Try
                            Me.DDLperiodo.SelectedValue = Me.Request.Cookies(stringaCookie)("periodo")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try
            Else
                stringaCookie = "RicercaComunita"
            End If
            ' Setto l'anno accademico


            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies(stringaCookie)("tipo")) Then
                    Me.DDLTipo.SelectedValue = Me.Request.Cookies(stringaCookie)("tipo")
                    If Me.DDLTipo.SelectedValue = 1 Then
                        Me.TBRcorsi.Visible = True
                    Else
                        Me.TBRcorsi.Visible = False
                    End If
                Else
                    Me.TBRcorsi.Visible = False
                End If
            Catch ex As Exception
                Me.TBRcorsi.Visible = False
            End Try

        Catch ex As Exception

        End Try
    End Sub


    Private Function Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True

                    If IsNothing(Session("ORGN_id")) = False Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = Session("ORGN_id")
                        Catch ex As Exception
                            Me.DDLorganizzazione.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                    End If
                    Me.TBCorganizzazione.Visible = True
                    Me.TBCorganizzazione_c.Visible = True
                Else
                    Me.DDLorganizzazione.Enabled = False
                    Me.TBCorganizzazione.Visible = False
                    Me.TBCorganizzazione_c.Visible = False
                End If
            Else
                Me.TBCorganizzazione.Visible = False
                Me.TBCorganizzazione_c.Visible = False
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.TBCorganizzazione.Visible = False
            Me.TBCorganizzazione_c.Visible = False
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
    End Function


    Private Function Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try

    End Function

    Private Sub DLLtipoFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        If Me.DDLTipo.SelectedValue <> 1 Then
            Me.TBRcorsi.Visible = False
        Else
            Me.TBRcorsi.Visible = True
        End If
        Me.Bind_TreeView()
    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_TreeView()
    End Sub
 
#End Region

#Region "Gestione Comunità"
    'Visualizzo i dettagli della comunità
    Private Sub VisualizzaDettagli(ByVal CMNT_Path As String)
        Dim CMNT_ID As Integer
        Dim Elenco() As String

        Elenco = CMNT_Path.Split(".")
        CMNT_ID = Elenco(UBound(Elenco) - 1)
        Try
            Dim oRuoloComunita As New COL_RuoloPersonaComunita

            Me.PNLdettagli.Visible = True
            Me.PNLtreeView.Visible = False
            Me.HDN_Path.Value = CMNT_Path

            Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)
            oRuoloComunita.EstraiByLinguaDefault(CMNT_ID, Session("objPersona").id)

            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.TipoRuolo.Id > 0 Then
                    Me.BTNentra.Visible = True
                Else
                    Me.BTNentra.Visible = False
                End If
            Else
                Me.BTNentra.Visible = False
            End If
        Catch ex As Exception
            Me.BTNentra.Visible = False
        End Try

    End Sub

    Private Sub Entra_Comunita(ByVal CMNT_Path As String)
        Dim CMNT_ID, PRSN_ID As Integer
        Dim Elenco_CMNT_ID() As String
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita

        Elenco_CMNT_ID = CMNT_Path.Split(".")
        CMNT_ID = Elenco_CMNT_ID(UBound(Elenco_CMNT_ID) - 1)

        Try

            oPersona = Session("objPersona")
            PRSN_ID = oPersona.Id

            oTreeComunita.Directory = Server.MapPath("./../profili/") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try


        Try
            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
            If oComunita.Errore = Errori_Db.None Then
                Dim i, j, totale, ORGN_ID, TPRL_ID As Integer
                Dim oRuolo As New COL_RuoloPersonaComunita
                'Dim oResponsabile As New COL_Persona
                'Dim Responsabile As String

                'oResponsabile = oComunita.GetResponsabile()
                'If oResponsabile.Nome <> "" And oResponsabile.Cognome <> "" Then
                '    Responsabile = oResponsabile.Cognome & " " & oResponsabile.Nome
                'ElseIf oResponsabile.Nome <> "" Then
                '    Responsabile = oResponsabile.Nome
                'ElseIf oResponsabile.Cognome <> "" Then
                '    Responsabile = oResponsabile.Cognome
                'Else
                '    Responsabile = "n.d."
                'End If

                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)

                If oRuolo.Errore = Errori_Db.None Then
                    If oRuolo.Attivato And oRuolo.Abilitato Then
                        With oComunita
                            Session("IdComunita") = CMNT_ID
                            Session("ORGN_id") = .Organizzazione.Id
                            Session("RLPC_id") = oRuolo.Id
                            Session("IdRuolo") = oRuolo.TipoRuolo.Id

                            'carico il ruolo che la persona adempie nella comunità selezionata
                            Try

                                Dim oServizio As New COL_Servizio
                                Dim oDataSet As New DataSet
                                oDataSet = oServizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
                                totale = oDataSet.Tables(0).Rows.Count - 1

                                Dim ArrPermessi(totale, 2) As String
                                For i = 0 To totale
                                    Dim oRow As DataRow
                                    oRow = oDataSet.Tables(0).Rows(i)
                                    ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                                    ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                                    ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
                                Next
                                Session("ArrPermessi") = ArrPermessi
                            Catch ex As Exception

                            End Try

                            Try
                                oRuolo.UpdateUltimocollegamento()

                            Catch ex As Exception

                            End Try

                            'Aggiorno gli array relativi al menu history !!!

                            Dim ArrComunita(,) As String
                            Dim tempArray(,), Path As String

                            If Session("limbo") = True Then
                                j = 0
                                For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                    If IsNumeric(Elenco_CMNT_ID(i)) Then
                                        If Elenco_CMNT_ID(i) > 0 Then
                                            ReDim Preserve ArrComunita(3, j)
                                            ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                            ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                            If Path = "" Then
                                                Path = "." & Elenco_CMNT_ID(i) & "."
                                            Else
                                                Path = Path & Elenco_CMNT_ID(i) & "."
                                            End If
                                            ArrComunita(2, j) = Path

                                            ' Ruolo svolto..........
                                            ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                            j = j + 1
                                        End If
                                    End If
                                Next

                                Session("ArrComunita") = ArrComunita
                                Session("limbo") = False

                            Else 'altrimento lo faccio per passi successivi

                                'caricamento navigazione albero comunità
                                Try
                                    ArrComunita = Session("ArrComunita")
                                    totale = UBound(ArrComunita, 2) 'recupero il numero di comunità dell'array

                                    If oComunita.IdPadre = 0 Then 'se sono in cima all'albero allora inserisco solo il primo elemento
                                        j = 0
                                        For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                            If IsNumeric(Elenco_CMNT_ID(i)) Then
                                                If Elenco_CMNT_ID(i) > 0 Then
                                                    ReDim Preserve ArrComunita(3, j)
                                                    ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                                    ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                                    If Path = "" Then
                                                        Path = "." & Elenco_CMNT_ID(i) & "."
                                                    Else
                                                        Path = Path & Elenco_CMNT_ID(i) & "."
                                                    End If
                                                    ArrComunita(2, j) = Path

                                                    ' Ruolo svolto..........
                                                    ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                                    j = j + 1
                                                End If
                                            End If
                                        Next
                                    Else
                                        ' Cerco di recuperare solo gli id delle nuove comunià da agiungere
                                        ' nell'array


                                        'recupero l'ultimo path presente nell'history
                                        Path = ArrComunita(2, totale)
                                        CMNT_Path = Right(CMNT_Path, CMNT_Path.Length - Path.Length)
                                        Elenco_CMNT_ID = CMNT_Path.Split(".")

                                        j = totale + 1
                                        For i = 0 To UBound(Elenco_CMNT_ID) - 1
                                            If IsNumeric(Elenco_CMNT_ID(i)) Then
                                                If Elenco_CMNT_ID(i) > 0 Then
                                                    ReDim Preserve ArrComunita(3, j)
                                                    ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                                    ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                                    If Path = "" Then
                                                        Path = "." & Elenco_CMNT_ID(i) & "."
                                                    Else
                                                        Path = Path & Elenco_CMNT_ID(i) & "."
                                                    End If
                                                    ArrComunita(2, j) = Path
                                                    ' Ruolo svolto..........
                                                    ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                                    j = j + 1
                                                End If
                                            End If
                                        Next
                                    End If
                                    Session("ArrComunita") = ArrComunita
                                    Session("limbo") = False
                                Catch ex As Exception

                                End Try
                            End If
                        End With

                        Session("RLPC_ID") = oRuolo.Id
                        oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                        Response.Redirect("./comunita.aspx")

                    Else
                        ' non si ha accesso alla comunità
                        Me.PNLmessaggio.Visible = True
                        Me.PNLtreeView.Visible = False
                        Me.HDN_Azione.Value = ""
                        Me.HDN_Path.Value = ""
                        If Not (oRuolo.Attivato) Then
                            Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, si è in attesa di attivazione da parte del relativo amministratore/responsabile."
                        ElseIf Not oRuolo.Abilitato Then
                            Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, l'accesso è stato bloccato dal relativo amministratore/responsabile."
                        Else
                            Me.LBmessaggio.Text = "ATTENZIONE: l'accesso alla comunità non è al momento consentito."
                        End If
                        oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                    End If
                Else
                    'Spiacente, non si ha accesso alla comunità
                    Me.PNLtreeView.Visible = False
                    Me.PNLmessaggio.Visible = True
                    Me.HDN_Azione.Value = ""
                    Me.HDN_Path.Value = ""
                    Me.LBmessaggio.Text = "ATTENZIONE: l'accesso alla comunità non è al momento consentito."

                    ' la comunità non esiste più o non si è più iscritti !
                    oTreeComunita.Delete(CMNT_ID, CMNT_Path)
                End If
            Else
                ' la comunità non esiste più !!
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
                Me.PNLtreeView.Visible = False
                Me.PNLmessaggio.Visible = True
                Me.LBmessaggio.Text = "ATTENZIONE: errore di accesso al sistema, la comunità prescelta sembra non essere presente nel sistema."
                Me.HDN_Azione.Value = ""
                Me.HDN_Path.Value = ""
            End If
        Catch ex As Exception
            Me.PNLtreeView.Visible = False
            Me.PNLmessaggio.Visible = True
            Me.HDN_Azione.Value = ""
            Me.HDN_Path.Value = ""
            Me.LBmessaggio.Text = "ATTENZIONE: errore del sistema,riprovare successivamente o contattare l'amministratore del sistema."
        End Try
    End Sub

    Private Sub BTNnascondi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNnascondi.Click
        Me.HDN_Azione.Value = ""
        Me.HDN_Path.Value = ""
        Me.PNLdettagli.Visible = False
        Me.PNLtreeView.Visible = True
        Me.PNLcontenuto.Visible = True
        '    Me.Bind_TreeView()
    End Sub

    Private Sub Iscrivi_Comunita(ByVal CMNT_Path As String)
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oRuolo As New COL_RuoloPersonaComunita
        Dim PRSN_ID, TPRL_ID, CMNT_ID As Integer
        Dim Elenco() As String

        Elenco = CMNT_Path.Split(".")
        CMNT_ID = Elenco(UBound(Elenco) - 1)

        Try
            Dim oTreeComunita As New COL_TreeComunita

            oComunita.Id = CMNT_ID
            oPersona = Session("objPersona")
            PRSN_ID = oPersona.Id
            TPRL_ID = oComunita.RuoloDefault()

            ' se esiste già  e sono il creatore, mi iscrive come admin.....

            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
            If oRuolo.Errore = Errori_Db.None Then
                If oRuolo.TipoRuolo.Id = -2 Then
                    ' Se è il creatore...
                    TPRL_ID = CType(Main.TipoRuoloStandard.AdminComunità, Main.TipoRuoloStandard)
                    oRuolo.TipoRuolo.Id = TPRL_ID
                    oRuolo.Modifica()
                ElseIf oRuolo.TipoRuolo.Id = -3 Then
                    'TRATTASI DI PASSANTE....
                    oRuolo.TipoRuolo.Id = TPRL_ID
                    oRuolo.Modifica()
                End If
                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
                Me.Bind_TreeView()
            Else
                'Non sono iscritto.....
                Dim responso, i As Integer
                Dim Ricezione_SMS, Attivato, Responsabile, Abilitato, isAttivato As Boolean
                Dim ORGN_ID As Integer
                oComunita.Estrai()
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
                'RuoloPersonaComunita.Abilitato = 1 'sempre a 1! si viene disabilitati solo per comportamento scorretto
                'confronto l'id dell'organizzazione di appartenenza della comunità con quello dell'organizzazione di default della persona che si sta registrando
                ORGN_ID = oPersona.GetOrganizzazioneDefault
                TPRL_ID = oComunita.RuoloDefault() 'qui si deve usare il tipo di ruolo predefinito per il tipo d comunita della comunita

                If oComunita.Organizzazione.Id = ORGN_ID Then
                    ' se la persona fa parte della stessa organizzazione della comunita la iscrivo col ruolo di default
                    Attivato = Not (oComunita.IsChiusa)
                    Abilitato = Not (oComunita.IsChiusa)
                Else 'se uno non è di quell'organizzazione come default lo iscrivo disabilitato e disattivato
                    Attivato = False
                    Abilitato = False
                End If

                oPersona.AssociaComunita(TPRL_ID, CMNT_ID, oPersona.RicezioneSMS, Attivato, Abilitato, False)

                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
                Attivato = oRuolo.Attivato
                Abilitato = oRuolo.Abilitato

                Dim ArrComunita(,) As String
                Try
                    ArrComunita = Session("ArrComunita")

                    CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2)) & CMNT_ID & "."

                Catch ex As Exception
                    CMNT_Path = "." & CMNT_ID & "."
                End Try
                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)

                'oRuolo.Abilitato = Abilitato
                'oRuolo.Attivato = Attivato
                'oRuolo.Comunita.Id = CMNT_ID
                'oRuolo.Persona.Id = PRSN_ID
                'oRuolo.TipoRuolo.Id = TPRL_ID
                'oRuolo.TipoRuolo.Descrizione()
                oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                oTreeComunita.Nome = PRSN_ID & ".xml"
                oTreeComunita.Insert(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)


                If oPersona.Errore = Errori_Db.None Then
                    If Attivato Then
                        Me.Bind_TreeView()
                    Else
                        Me.LBmessaggio.Text = "Sei stato iscritto correttamente, ma non sei ancora abilitato!" & vbCrLf & " La tua registrazione dovrà essere valutata!"
                        Me.PNLmessaggio.Visible = True
                        Me.BTNnascondi.Visible = True
                        Me.PNLtreeView.Visible = False
                    End If
                    'Me.Bind_Griglia()  'ricarico, così evito che una persona veda i dati vecchi (nn vengono mostrate le comunità a cui si è già iscritti)
                Else
                    Me.LBmessaggio.Text = "Spiacenti, Si è verificato un errore nella registrazione."
                    Me.PNLmessaggio.Visible = True
                    Me.BTNnascondi.Visible = True
                    Me.PNLtreeView.Visible = False
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Gestione TreeView"

    Private Function FiltraggioDati() As DataSet
        Dim oDataset As New DataSet
        Dim oComunita As New COL_Comunita

        Try
            Dim i, totale, max As Integer

            'If Me.RBLvisualizza.SelectedValue = 1 Then
            '    oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), 0, 100, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, , Session("objPersona").id, Me.DDLTipo.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlbero)

            '    '(0, 100, Me.DDLorganizzazione.SelectedValue, , , Me.DDLTipo.SelectedValue, COL_Comunita.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlbero)
            'Else
            '    oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), 0, 100, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, , Session("objPersona").id, Me.DDLTipo.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlberoOrganizzativo)
            '    'oDataset = oComunita.ElencaGlobale_Paginate(0, 100, Me.DDLorganizzazione.SelectedValue, , , Me.DDLTipo.SelectedValue, COL_Comunita.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlberoOrganizzativo)
            'End If

         
                oDataset = oComunita.RicercaComunitaAlberoForManagement(Session("LinguaID"), Me.DDLorganizzazione.SelectedValue, , , Session("objPersona").id, , , , Me.DDLTipo.SelectedValue, , , , , Main.FiltroStatoComunita.Tutte, Main.FiltroRicercaComunitaByIscrizione.tutte)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Sub Bind_TreeView()
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet

        Dim iCMNT_ID As Integer
        Dim iCMNT_Path, elenco(), Start_Path As String
        Dim ImageBaseDir As String

        'ImageBaseDir = "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")


        Me.RDTcomunita.Nodes.Clear()
        Try
            Dim i, tot, totaleHistory, CMNT_idPadre, CMNT_id As Integer
            Dim ArrComunita(,) As String


            oPersona = Session("objPersona")

            oDataset = Me.FiltraggioDati

            Me.RDTcomunita.Nodes.Clear()


            Dim nodeRoot As New RadTreeNode
            nodeRoot.Text = "Elenco comunità: "
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            nodeRoot.ToolTip = "Elenco comunità: "
            nodeRoot.ContextMenuName = "Base"

            Me.RDTcomunita.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                ' nessuna comunità a cui si è iscritti
                Me.GeneraNoNode()
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("CMNT_ID"), oDataset.Tables(0).Columns("CMNT_idPadre_Link"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("CMNT_idPadre") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulate(dbRow, node)
                    End If
                Next dbRow
                Me.PNLtreeView.Visible = True
            End If

            'Dim img, CMNT_path, CMNT_REALpath, CMNT_pathPadre, CMNT_Nome, CMNT_Responsabile, TPCM_icona, CMNT_NomeVisibile As String
            'Dim CMNT_isIscritto, CMNT_IsChiusa As Boolean
            'Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime
            'Dim RLPC_TPRL_id As Integer

            'If oDataset.Tables(0).Rows.Count = 0 Then
            '    ' nessuna comunità a cui si è iscritti
            '    Me.GeneraNoNode()
            'Else
            'Dim oRootNode As New RadTreeNode
            'Dim OldNode As New RadTreeNode

            'tot = oDataset.Tables(0).Rows.Count - 1

            'Dim start As Integer
            'Dim continue As Boolean = False
            'start = 0

            'For i = 0 To tot
            '    Dim oRow As DataRow
            '    oRow = oDataset.Tables(0).Rows(i)

            '    CMNT_id = oRow.Item("CMNT_id")

            '    If CMNT_id < 0 Then
            '        Dim filtro As String
            '        Dim oDataview As DataView
            '        oDataview = oDataset.Tables(0).DefaultView

            '        filtro = "CMNT_path like '" & oRow.Item("CMNT_path") & "%'"
            '        oDataview.RowFilter = filtro
            '        If oDataview.Count <= 1 Then
            '            continue = True
            '        Else
            '            continue = False
            '        End If
            '    Else
            '        continue = False
            '        If Me.DDLTipo.SelectedValue >= 0 Then
            '            If Not IsDBNull(oRow.Item("CMNT_path")) Then
            '                Dim filtro As String
            '                Dim oDataview As DataView
            '                oDataview = oDataset.Tables(0).DefaultView

            '                CMNT_path = oRow.Item("CMNT_path")
            '                filtro = "CMNT_path like '" & oRow.Item("CMNT_path") & "%' and CMNT_TPCM_id=" & Me.DDLTipo.SelectedValue

            '                If Me.TBRcorsi.Visible Then
            '                    If Me.DDLannoAccademico.SelectedValue > 0 Then
            '                        filtro = filtro & " and CMNT_Anno=" & Me.DDLannoAccademico.SelectedValue
            '                    End If

            '                    If Me.DDLperiodo.SelectedValue > 0 Then
            '                        filtro = filtro & " and CMNT_PRDO_id=" & Me.DDLperiodo.SelectedValue
            '                    End If
            '                End If
            '                oDataview.RowFilter = filtro
            '                If oDataview.Count < 1 Then
            '                    continue = True
            '                End If
            '            End If
            '        End If
            '        End If

            '        If IsDBNull(oRow.Item("RLPC_TPRL_id")) Then
            '            RLPC_TPRL_id = -1
            '        Else
            '            RLPC_TPRL_id = oRow.Item("RLPC_TPRL_id")
            '        End If

            '        If Not continue Then
            '            CMNT_idPadre = oRow.Item("CMNT_idPadre")
            '            If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
            '                CMNT_Responsabile = ""
            '                If Not IsDBNull(oRow.Item("AnagraficaCreatore")) Then
            '                    CMNT_Responsabile = " (creata da: " & oRow.Item("AnagraficaCreatore") & ") "
            '                End If
            '            Else
            '                CMNT_Responsabile = " (" & oRow.Item("CMNT_Responsabile") & ") "
            '            End If
            '            If IsDBNull(oRow.Item("TPCM_icona")) Then
            '                img = ""
            '            Else
            '            img = oRow.Item("TPCM_icona")
            '            img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
            '            End If
            '            If IsDBNull(oRow.Item("CMNT_isIscritto")) Then
            '                CMNT_isIscritto = True
            '            Else
            '                CMNT_isIscritto = oRow.Item("CMNT_isIscritto")
            '            End If

            '            CMNT_Nome = oRow.Item("CMNT_Nome")

            '            CMNT_NomeVisibile = CMNT_Nome

            '            If CMNT_id > 0 Then
            '                CMNT_Nome = CMNT_Nome & CMNT_Responsabile
            '                CMNT_NomeVisibile = CMNT_Nome
            '                If CBool(oRow.Item("CMNT_IsChiusa")) Then
            '                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_closed.gif", "Comunità chiusa")
            '                Else
            '                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_open.gif", "Comunità aperta")
            '                End If
            '            Else
            '                CMNT_NomeVisibile = CMNT_Nome

            '            End If
            '            If IsDBNull(oRow.Item("CMNT_path")) Then
            '                CMNT_path = "." & CMNT_idPadre & "."
            '            Else
            '                CMNT_path = oRow.Item("CMNT_path")
            '            End If

            '            If IsDBNull(oRow.Item("CMNT_REALpath")) Then
            '                CMNT_REALpath = "." & CMNT_idPadre & "."
            '            Else
            '                CMNT_REALpath = oRow.Item("CMNT_REALpath")
            '            End If
            '            If Not (start = 0 And totaleHistory > -1) And CMNT_id > 0 Then

            '                If CMNT_isIscritto Then
            '                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_REALpath, "Dettagli", "dettagli", "Visualizza i dettagli  relativi alla comunità.")

            '                    If RLPC_TPRL_id = -2 Then
            '                        'Si tratta del creatore,se si iscrive diventa admin.....
            '                        CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_path, "Iscrivi", "iscrivi", "Iscrivi")
            '                    ElseIf RLPC_TPRL_id = -3 Then
            '                        ' Sono solo un passante !!
            '                    Else
            '                        CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_REALpath, "Entra", "entra", "Accede alla comunità selezionata.")
            '                    End If
            '                Else
            '                    CMNT_Nome = CMNT_Nome & "  " & Me.GenerateURLFromPath(CMNT_REALpath, "Dettagli", "dettagli", "Visualizza i dettagli  relativi alla comunità.") & "  "
            '                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
            '                        CMNT_dataInizioIscrizione = oRow.Item("CMNT_dataInizioIscrizione")
            '                        If CMNT_dataInizioIscrizione > Now Then
            '                            ' devo iscrivermi, ma iscrizioni non aperte !
            '                            CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & " (iscrizioni aperte il " & CMNT_dataInizioIscrizione & ")"
            '                        Else
            '                            If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
            '                                CMNT_dataFineIscrizione = oRow.Item("CMNT_dataFineIscrizione")
            '                                If CMNT_dataFineIscrizione < Now Then
            '                                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & "(Iscrizioni chiuse)"
            '                                Else
            '                                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_path, "Iscrivi", "iscrivi", "Iscrivi")
            '                                End If
            '                            Else
            '                                CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_path, "Iscrivi", "iscrivi", "Iscrivi")
            '                            End If
            '                        End If
            '                    Else
            '                        CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_path, "Iscrivi", "iscrivi", "Iscrivi")
            '                    End If
            '                End If
            '            End If

            '            If start = 0 And totaleHistory > -1 Then
            '                start = 1

            '                If CMNT_id > 0 Then
            '                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & Me.GenerateURLFromPath(CMNT_REALpath, "Dettagli", "dettagli", "Visualizza i dettagli  relativi alla comunità.")
            '                End If
            '                oRootNode = New RadTreeNode
            '                oRootNode.Text = CMNT_Nome
            '                oRootNode.Value = CMNT_path
            '                oRootNode.Expanded = True
            '                oRootnode.ImageUrl = img
            '                oRootNode.ToolTip = CMNT_NomeVisibile
            '                oRootNode.Category = oRow.Item("lvl")

            '                If RLPC_TPRL_id = -2 Then
            '                    oRootNode.CssClass = "TreeNodeDisabled"
            '                ElseIf RLPC_TPRL_id = -3 Then
            '                    oRootNode.CssClass = "TreeNodeDisabled"
            '                End If
            '                OldNode = oRootNode
            '                Start_Path = CMNT_path
            '            Else

            '                If Not (totaleHistory > -1) And start = 0 Then
            '                    oRootNode = New RadTreeNode
            '                    oRootNode.Text = "Elenco comunità: "
            '                    oRootNode.Value = ""
            '                    oRootNode.Expanded = True
            '                    oRootnode.ImageUrl = "./../folder.gif"
            '                    oRootNode.ToolTip = "Elenco comunità: " 'CMNT_NomeVisibile
            '                    oRootNode.Category = 0

            '                    OldNode = oRootNode
            '                End If
            '                start = 1
            '                If CMNT_idPadre > 0 Then
            '                    CMNT_idPadre = GetFatherFromPath(CMNT_path)
            '                    CMNT_pathPadre = GetFatherPath(CMNT_path)
            '                    If Start_Path = CMNT_pathPadre Then
            '                        Dim oNode As New RadTreeNode
            '                        oNode.Text = CMNT_Nome
            '                        oNode.Value = CMNT_path
            '                        oNode.ImageUrl = img
            '                        oNode.ToolTip = CMNT_NomeVisibile
            '                        oNode.Category = oRow.Item("lvl")

            '                        If RLPC_TPRL_id = -2 Then
            '                            oNode.CssClass = "TreeNodeDisabled"
            '                        ElseIf RLPC_TPRL_id = -3 Then
            '                            oNode.CssClass = "TreeNodeDisabled"
            '                        End If

            '                        OldNode = oNode
            '                        oRootNode.Nodes.Add(oNode)
            '                    Else
            '                        Dim oNodeNew As New RadTreeNode
            '                        Dim oNodePrec As New RadTreeNode

            '                        oNodeNew.Text = CMNT_Nome
            '                        oNodeNew.Value = CMNT_path
            '                        oNodeNew.Image = img
            '                        oNodeNew.ToolTip = CMNT_NomeVisibile
            '                        oNodeNew.Category = oRow.Item("lvl")

            '                        If RLPC_TPRL_id = -2 Then
            '                            oNodeNew.CssClass = "TreeNodeDisabled"
            '                        ElseIf RLPC_TPRL_id = -3 Then
            '                            oNodeNew.CssClass = "TreeNodeDisabled"
            '                        End If


            '                        If OldNode.Value = CMNT_pathPadre Then
            '                            OldNode.Nodes.Add(oNodeNew)
            '                            OldNode = oNodeNew
            '                        Else
            '                            Dim j, totale As Integer

            '                            If oNodeNew.Category > 1 Then
            '                                While OldNode.Category <> oNodeNew.Category - 2
            '                                    OldNode = OldNode.Parent
            '                                End While
            '                            Else
            '                                OldNode = oRootNode
            '                            End If
            '                            For j = 0 To OldNode.Nodes.Count - 1
            '                                Dim oNode As New RadTreeNode
            '                                oNode = OldNode.Nodes(j)
            '                                If OldNode.Nodes(j).Value = CMNT_pathPadre Then

            '                                    OldNode.Nodes(j).Nodes.Add(oNodeNew)
            '                                    OldNode = oNodeNew
            '                                    Exit For
            '                                End If
            '                            Next
            '                        End If
            '                    End If
            '                Else
            '                    Dim oNode As New RadTreeNode
            '                    oNode.Text = CMNT_Nome
            '                    oNode.Value = CMNT_path
            '                    oNode.ImageUrl = img
            '                    oNode.ToolTip = CMNT_NomeVisibile
            '                    oNode.Category = oRow.Item("lvl")
            '                    OldNode = oNode
            '                    oRootNode.Nodes.Add(oNode)
            '                End If
            '            End If
            '        End If
            'Next
            'Me.RDTcomunita.Nodes.Add(oRootNode)
            'End If
        Catch ex As Exception
            Me.GeneraNoNode()
        End Try
    End Sub

    Private Function AggiornaTreeView()
        Dim oPersona As New COL_Persona
        Dim oTreeComunita As New COL_TreeComunita
        Dim PRSN_ID As Integer

        oPersona = Session("objPersona")

        Try
            'TEMPORANEO
            'Dim odataset As New DataSet
            'odataset = oPersona.Elenca
            'Dim i, totale As Integer
            'totale = odataset.Tables(0).Rows.Count - 1

            'For i = 0 To totale
            ' PRSN_ID =  odataset.Tables(0).Rows(i).Item("PRSN_ID")
            PRSN_ID = oPersona.Id
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"

            oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"))
            'Next
        Catch ex As Exception

        End Try


    End Function

    Private Function GenerateImage(ByVal ImageName As String, Optional ByVal Status As String = "") As String
        Dim imageUrl As String
        Dim quote As String
        quote = """"

        imageUrl = "<img  align=absmiddle src=" & quote & ImageName & quote & " alt=" & quote & Status & quote

        imageUrl = imageUrl & " " & " onmouseover=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onfocus=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onmouseout=" & quote & "window.status='';return true;" & """" & " "
        imageUrl = imageUrl & " >"

        Return imageUrl
    End Function
    Private Function GeneraNoNode()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Text = "Comunità: "
        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
        oRootNode.Category = 0
        oRootNode.ContextMenuName = "Base"

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Text = "Non si è iscritti ad alcuna comunità"
        oNode.Value = ""
        oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
        oNode.Category = 0
        oNode.Checkable = False
        oRootNode.Nodes.Add(oNode)

        Me.RDTcomunita.Nodes.Clear()
        Me.RDTcomunita.Nodes.Add(oRootNode)
    End Function
    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
        Dim POST_Id, POST_ParentID, FRIM_id, POST_Approved As Integer
        Dim POST_Subject, POST_PostDate, PRSN_Anagrafica, FRIM_Image As String
        Dim oData, oDataAttuale As DateTime
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False)
            node.Nodes.Add(childNode)
            RecursivelyPopulate(childRow, childNode)
            ' End If
        Next childRow
    End Sub 'RecursivelyPopulate
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
        Dim node As New RadTreeNode

        Dim start As Integer
        Dim [continue] As Boolean = False
        Dim numIscritti, maxIscritti, iscritti As Integer
        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile, img As String
        Dim CMNT_isIscritto As Boolean
        CMNT_id = dbRow.Item("CMNT_id")

        If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
            RLPC_TPRL_id = -1
        Else
            RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")
        End If

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

        If IsDBNull(dbRow.Item("CMNT_Iscritti")) = False Then
            'Dim oComunita As New COL_Comunita

            'oComunita.Id = CMNT_id
            'oComunita.Estrai()
            maxIscritti = dbRow.Item("CMNT_MaxIscritti")

            numIscritti = dbRow.Item("CMNT_Iscritti")

            If maxIscritti <= 0 Then
                dbRow.Item("CMNT_Iscritti") = 0
                iscritti = 0
            Else
                If numIscritti > maxIscritti Then
                    dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                    iscritti = maxIscritti - numIscritti
                ElseIf numIscritti = maxIscritti Then
                    dbRow.Item("CMNT_Iscritti") = -1
                    iscritti = -1
                Else
                    dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                    iscritti = maxIscritti - numIscritti
                End If
            End If
        Else
            dbRow.Item("CMNT_Iscritti") = 0
        End If
        'TROVO IL RESPONSABILE
        If IsDBNull(dbRow.Item("CMNT_Responsabile")) Then
            CMNT_Responsabile = ""
            If Not IsDBNull(dbRow.Item("AnagraficaCreatore")) Then
                CMNT_Responsabile = " (creata da: " & dbRow.Item("AnagraficaCreatore") & ") "
            End If
        Else
            CMNT_Responsabile = " (" & dbRow.Item("CMNT_Responsabile") & ") "
        End If
        If IsDBNull(dbRow.Item("TPCM_icona")) Then
            img = ""
        Else
            img = dbRow.Item("TPCM_icona")
            img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
            ' img = ImageBaseDir & img
        End If
        If IsDBNull(dbRow.Item("CMNT_isIscritto")) Then
            CMNT_isIscritto = True
        Else
            CMNT_isIscritto = dbRow.Item("CMNT_isIscritto")
        End If

        Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
        Dim CMNT_IsChiusa As Boolean

        CMNT_Nome = dbRow.Item("CMNT_Nome")
        CMNT_NomeVisibile = CMNT_Nome
        CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
        If dbRow.Item("CMNT_isChiusaForPadre") = True Then
            CMNT_IsChiusa = True
        End If

        If CMNT_id > 0 Then
            CMNT_Nome = CMNT_Nome & CMNT_Responsabile
            CMNT_NomeVisibile = CMNT_Nome
            If CMNT_IsChiusa Then
                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_closed.gif", "Comunità chiusa")
            Else
                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_open.gif", "Comunità aperta")
            End If

            If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
                If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
                    CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
                End If
            End If
        Else
            CMNT_NomeVisibile = CMNT_Nome
        End If
        CMNT_path = dbRow.Item("CMNT_path")
        CMNT_REALpath = dbRow.Item("CMNT_REALpath")
        'End If
        If CMNT_id > 0 Then
            If CMNT_isIscritto And RLPC_TPRL_id <> -2 And RLPC_TPRL_id <> -3 Then
                node.ContextMenuName = "AdminEntra"
            Else
                Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

                If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
                    If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
                        CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
                        If CMNT_dataInizioIscrizione > Now Then
                            ' devo iscrivermi, ma iscrizioni non aperte !
                            CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (iscrizioni aperte il " & CMNT_dataInizioIscrizione & ")"
                            node.ContextMenuName = "AdminIscrivi"
                        Else
                            If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
                                CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")
                                If CMNT_dataFineIscrizione < Now Then
                                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & "(Iscrizioni chiuse)"
                                    node.ContextMenuName = "AdminIscrivi"
                                Else
                                    node.ContextMenuName = "AdminIscrivi"
                                End If
                            Else
                                node.ContextMenuName = "AdminIscrivi"
                            End If
                        End If
                    Else
                        node.ContextMenuName = "AdminIscrivi"
                    End If
                ElseIf RLPC_TPRL_id = -2 Then
                    node.ContextMenuName = "AdminIscrivi"
                Else
                    'Non c'è spazio per nuovi iscritti !!!
                    node.ContextMenuName = "AdminIscrivi"
                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (n° max iscritti raggiunto)"
                End If
            End If
        End If

        node.Text = CMNT_Nome
        node.Value = CMNT_id & "," & CMNT_REALpath
        node.Expanded = expanded
        node.ImageUrl = img
        node.ToolTip = CMNT_NomeVisibile
        node.Category = CMNT_IsChiusa


        node.Checkable = True
        If RLPC_TPRL_id = -2 Then
            node.CssClass = "TreeNodeDisabled"
        ElseIf RLPC_TPRL_id = -3 Then
            node.CssClass = "TreeNodeDisabled"
        ElseIf CMNT_isIscritto = False Then
            node.CssClass = "TreeNodeDisabled"
        End If
        node.Checkable = True


        Return node
    End Function


    Private Sub RDTcomunita_NodeContextClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTcomunita.NodeContextClick
        Dim isChiusa, isIscritto As Boolean
        Dim CMNT_ID As Integer
        Dim iCMNT_PAth, Elenco() As String
        Dim oNode As Telerik.WebControls.RadTreeNode
        oNode = e.NodeClicked

        Try
            Dim oRuoloComunita As New COL_RuoloPersonaComunita

            isChiusa = CBool(oNode.Category)
            isIscritto = False
            Elenco = oNode.Value.Split(",")

            CMNT_ID = CInt(Elenco(0))
            oRuoloComunita.EstraiByLinguaDefault(CMNT_ID, Session("objPersona").id)
            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.TipoRuolo.Id > 0 Then
                    isIscritto = True
                End If
            End If

            iCMNT_PAth = Elenco(1)

            'vai a:
            Dim link As String
            If Me.Request.QueryString("from") = "" Then
                link = "?FromAlbero=true"
            Else
                Select Case LCase(Me.Request.QueryString("from"))
                    Case "ricercacomunita"
                        link = "?from=RicercaComunita&FromAlbero=true"
                    Case "ricercabypersona"
                        link = "?from=ricercabypersona&FromAlbero=true"
                    Case Else
                        link = "?FromAlbero=true"
                End Select
            End If

            Select Case e.ContextMenuItemID
                Case AzioneTree.Aggiorna
                    Me.Bind_TreeView()
                Case AzioneTree.Dettagli
                    Me.HDN_Path.Value = iCMNT_PAth
                    Me.HDNcmnt_ID.Value = CMNT_ID
                    Me.HDNcmnt_ID.Value = CMNT_ID
                    Me.PNLcontenuto.Visible = False
                    Me.PNLdettagli.Visible = True

                    Me.VisualizzaDettagli(iCMNT_PAth)

                Case AzioneTree.Entra
                    Me.HDN_Path.Value = iCMNT_PAth
                    Session("Azione") = "entra"
                    Me.Entra_Comunita(iCMNT_PAth)
                Case AzioneTree.Iscrivi
                    Me.PNLmessaggio.Visible = False
                    Me.HDN_Path.Value = iCMNT_PAth
                    Session("Azione") = "iscrivi"
                    Me.Iscrivi_Comunita(iCMNT_PAth)
                Case AzioneTree.GestioneServizi
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.Response.Redirect("./AdminG_GestioneServizi.aspx" & link)
                Case AzioneTree.Modifica
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.Response.Redirect("./AdminG_ModificaComunita.aspx" & link)
                Case AzioneTree.GestioneUtenti
                    Session("idComunita_forAdmin") = CMNT_ID
                    Session("CMNT_path_forAdmin") = iCMNT_PAth

                    Me.Response.Redirect("./AdminG_GestioneIscritti.aspx" & link)
            End Select
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Pannello Dettagli"

    Private Sub BTNentra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNentra.Click
        Me.Entra_Comunita(Me.HDNcmnt_Path.Value)
    End Sub

    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        Me.PNLtreeView.Visible = True
        Me.PNLmessaggio.Visible = False
        '  Me.Bind_TreeView()
    End Sub

#End Region

    Private Sub LNBaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaggiorna.Click
        Me.AggiornaTreeView()
        Me.Bind_TreeView()
    End Sub

    Private Sub RBLvisualizza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizza.SelectedIndexChanged
        Try
            Me.TBRfiltro.Visible = True
            Select Case Me.RBLvisualizza.SelectedValue

                Case 1
                    Me.Bind_TreeView()
                Case 2
                    Me.Bind_TreeView()

                Case 3
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
                Case Else
                    Me.Bind_TreeView()
            End Select
        Catch ex As Exception

        End Try
    End Sub
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class