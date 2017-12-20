Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class AdminG_UtentiConnessi
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

#Region "Gestione Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLcontenuto As System.Web.UI.WebControls.Table
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBlista As System.Web.UI.WebControls.LinkButton

#Region "Griglia Dati"
    Protected WithEvents DGpersona As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLpersona As System.Web.UI.WebControls.Panel
    'Pannelli paginazione
    Protected WithEvents DDLpaginazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLpaginazione As System.Web.UI.WebControls.Panel
    'Pannelli NO Record
    Protected WithEvents PNLnorecord As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecord As System.Web.UI.WebControls.Label


#End Region



#Region "Gestione lista connessi"
    Protected WithEvents TBRlista As System.Web.UI.WebControls.TableRow
#End Region

#Region "Filtri"
    Protected WithEvents LBorganizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBvalore As System.Web.UI.WebControls.TextBox

    Protected WithEvents LBtipoPersona As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoPersona As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBXaggiorna As System.Web.UI.WebControls.CheckBox

    Protected WithEvents LBtipoAutenticazione As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoAutenticazione As System.Web.UI.WebControls.DropDownList


    Protected WithEvents BTNcerca As System.Web.UI.WebControls.Button
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
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

        Me.Master.ServiceTitle = "Comunicazioni generali"

        Dim numUtenti As Integer
        Dim oPersona As COL_Persona

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            oPersona = Session("objPersona")
            If Page.IsPostBack = False And Not IsNothing(oPersona) Then

                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.ID = Main.TipoPersonaStandard.AdminSecondario Or oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Amministrativo Then
                    Me.Bind_Filtri()
                    Me.ViewState("intCurPage") = 0
                    Me.ViewState("intAnagrafica") = CType(Main.FiltroAnagrafica.tutti, Main.FiltroAnagrafica)
                    Me.LKBtutti.CssClass = "lettera_Selezionata"
                    Me.TBRlista.Visible = True
                    Me.SetStartupScripts()

                    Me.PNLcontenuto.Visible = True
                    Me.PNLpermessi.Visible = False
                    Me.PNLmenu.Visible = True
                Else
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                    Me.PNLmenu.Visible = False
                End If
            ElseIf IsNothing(oPersona) Then
                Dim PageUtility As New OLDpageUtility(Me.Context)
                Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
                Exit Sub
            End If
        Catch ex As Exception
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
            Me.PNLmenu.Visible = False
        End Try


        Try
            oPersona = Session("objPersona")
            numUtenti = oPersona.GetNumeroConnessioni()
            Me.Application.Lock()
            If numUtenti > 0 And numUtenti <> Me.Application.Item("utentiConnessi") Then
                Me.Application.Item("utentiConnessi") = numUtenti
            Else
                numUtenti = Me.Application.Item("utentiConnessi")
            End If
            Me.Application.UnLock()
        Catch ex As Exception

        End Try
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
            Return False
        End If
    End Function


    Private Sub SetStartupScripts()
        Me.LNBlista.Attributes.Add("onmouseout", "window.status='';return true;")
        Me.LNBlista.Attributes.Add("onfocus", "window.status='Lista utenti connessi.';return true;")
        Me.LNBlista.Attributes.Add("onmouseover", "window.status='Lista utenti connessi.';return true;")
        Me.LNBlista.Attributes.Add("onclick", "window.status='Lista utenti connessi.';return true;")

    End Sub

#Region "Bind_Dati"

    Private Sub Bind_UtentiConnessi()

    End Sub

    Private Sub Bind_Filtri()
        Me.Bind_Organizzazione()
        Me.Bind_TipoPersona()
        Me.Bind_TipoAutenticazione()

        Me.DDLorganizzazione.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLtipoPersona.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLtipoAutenticazione.AutoPostBack = Me.CBXaggiorna.Checked
    End Sub
    Private Sub Bind_Organizzazione()
        Dim oDataSet As New DataSet
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim ISTT_ID, i, Totale As Integer

        oPersona = Session("objPersona")
        ISTT_ID = oPersona.Istituzione.Id

        Try
            oDataSet = oPersona.GetOrganizzazioniByIstituzione(ISTT_ID)
            Me.DDLorganizzazione.Items.Clear()

            Totale = oDataSet.Tables(0).Rows.Count() - 1
            For i = 0 To Totale
				Dim oListItem As New ListItem
                If IsDBNull(oDataSet.Tables(0).Rows(i).Item("CMNT_nome")) Then
                    oListItem.Text = "--"
                Else
                    oListItem.Text = oDataSet.Tables(0).Rows(i).Item("CMNT_nome")
                End If
                oListItem.Value = oDataSet.Tables(0).Rows(i).Item("ORGN_id") 'oDataSet.Tables(0).Rows(i).Item("CMNT_id") & "," & 
                Me.DDLorganizzazione.Items.Add(oListItem)
                'metto nel value della ddl l'id della comunita e l'id dell'organizzazione facente parte
                'nella ddlorgn invece metto solo gli id delle organizzazioni come values
            Next
            '-1 significa TUTTE !!!!!!!!!!!!!!
            DDLorganizzazione.Items.Insert(0, New ListItem("-- Tutte --", -1))
            'If DDLorganizzazione.Items.Count > 1 Then
            Me.DDLorganizzazione.SelectedIndex = 0
            'End If
        Catch ex As Exception
            '-1 significa TUTTE !!!!!!!!!!!!!!
            DDLorganizzazione.Items.Insert(0, New ListItem("-- Tutte --", -1))
        End Try

    End Sub
    Private Sub Bind_TipoPersona(Optional ByVal TipoPersona As Integer = -1)
        Dim oDataset As DataSet
        Dim oTipoPersona As New COL_TipoPersona
        Dim oListItem As New ListItem

        Try
            oDataset = oTipoPersona.ElencaConnessi((Session("LinguaID")))
            DDLtipoPersona.Items.Clear()
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLtipoPersona.DataSource = oDataset
                DDLtipoPersona.DataTextField() = "TPPR_descrizione"
                DDLtipoPersona.DataValueField() = "TPPR_id"
                DDLtipoPersona.DataBind()

                If Me.DDLtipoPersona.Items.Count > 1 Then
                    Me.DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
                    Me.DDLtipoPersona.SelectedIndex = 0
                End If

                'Try
                '    Me.DDLtipoPersona.SelectedValue = CType(Main.TipoPersonaStandard.Docente, Main.TipoPersonaStandard)
                'Catch ex As Exception
                '    Me.DDLtipoPersona.SelectedIndex = 1
                'End Try
                Try
                    If TipoPersona <> -1 Then
                        Me.DDLtipoPersona.SelectedValue = TipoPersona
                    End If
                Catch ex As Exception
                    Me.DDLtipoPersona.SelectedIndex = 0
                End Try
            Else
                DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
    End Sub
    Private Sub Bind_TipoAutenticazione()
		Dim oLista As New List(Of COL_Autenticazione)
        Dim oAutenticazione As New COL_Autenticazione

        Try
			oLista = oAutenticazione.Elenca
            DDLtipoAutenticazione.Items.Clear()
			If oLista.Count > 0 Then
				DDLtipoAutenticazione.DataSource = oLista
				DDLtipoAutenticazione.DataTextField() = "Nome"
				DDLtipoAutenticazione.DataValueField() = "ID"
				DDLtipoAutenticazione.DataBind()

				DDLtipoAutenticazione.Items.Insert(0, New ListItem("-- Tutti --", -1))
			Else
				Me.DDLtipoAutenticazione.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
        Catch ex As Exception
            Me.DDLtipoAutenticazione.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
    End Sub

    Public Sub BindGriglia(Optional ByVal ricalcola As Boolean = False)
        Dim dstable As New DataSet

        dstable = FiltraggioDati(ricalcola)

        Try
            Dim oEncrypter As New COL_Encrypter
            Dim oPRSN_invisible As New DataColumn
            oPRSN_invisible.ColumnName = "oPRSN_invisible"
            dstable.Tables(0).Columns.Add(oPRSN_invisible)
            dstable.Tables(0).Columns.Add(New DataColumn("oPRSN_datanascita"))
            dstable.Tables(0).Columns.Add(New DataColumn("oPRSN_pwd"))
            dstable.Tables(0).Columns.Add(New DataColumn("oPRSN_cambiapwd"))
            Dim i, pos, TotaleRecord As Integer
            TotaleRecord = dstable.Tables(0).Rows.Count


            'If Me.DDLtipoAutenticazione.SelectedValue = Main.TipoAutenticazione.LDAP Then
            '    Me.DGpersona.Columns(10).Visible = False
            'Else
            '    Me.DGpersona.Columns(10).Visible = True
            'End If
            For i = 0 To TotaleRecord - 1
                Dim oRow As DataRow
                oRow = dstable.Tables(0).Rows(i)
                Try
                    oRow.Item("oPRSN_datanascita") = FormatDateTime(oRow.Item("PRSN_datanascita"), DateFormat.ShortDate)

                    'If oRow.Item("PRSN_AUTN_ID") = Main.TipoAutenticazione.LDAP Then
                    '    oRow.Item("oPRSN_pwd") = "" '"su LDAP"
                    'ElseIf oRow.Item("PRSN_AUTN_ID") = Main.TipoAutenticazione.ComunitaOnLine Then
                    '    oRow.Item("oPRSN_pwd") = oEncrypter.Decrypt(oRow.Item("PRSN_pwd"))
                    'Else
                    '    oRow.Item("oPRSN_pwd") = "" '"altro sist."
                    'End If

                    'oRow.Item("oPRSN_cambiapwd") = "Cambia PWD"
                Catch ex As Exception

                End Try
            Next

            If TotaleRecord > 0 Then
                Mod_Visualizzazione(TotaleRecord - 1)

                Me.PNLpersona.Visible = True
                PNLnorecord.Visible = False

                Me.DGpersona.DataSource = dstable
                Me.DGpersona.DataBind()
            Else
                Me.PNLpersona.Visible = False
                PNLpaginazione.Visible = False
                PNLnorecord.Visible = True
                LBnorecord.Text = "Spiacente, al momento non ci utenti connessi in base ai parametri di ricerca specificati."
            End If
        Catch ex As Exception
            Me.PNLpersona.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecord.Visible = True
            LBnorecord.Text = "Spiacente, al momento non ci sono utenti connessi in base ai parametri di ricerca specificati."
        End Try
    End Sub
#End Region

#Region "Gestione Filtro"
    Private Sub CBXaggiorna_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXaggiorna.CheckedChanged
        Me.DDLorganizzazione.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLtipoPersona.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLtipoAutenticazione.AutoPostBack = Me.CBXaggiorna.Checked
    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.BindGriglia(True)
    End Sub
    Private Sub DDLtipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoPersona.SelectedIndexChanged
        Me.BindGriglia(True)
    End Sub
    Private Sub DDLtipoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoRicerca.SelectedIndexChanged
        Me.BindGriglia(True)
    End Sub
    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Try
            Me.Bind_TipoPersona(Me.DDLtipoPersona.SelectedValue)
        Catch ex As Exception

        End Try
        Me.BindGriglia(True)
    End Sub
    Private Sub DDLtipoAutenticazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoAutenticazione.SelectedIndexChanged
        Me.BindGriglia(True)
    End Sub
    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        Me.DGpersona.CurrentPageIndex = 0
        Me.BindGriglia(True)
    End Sub

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet

        Try
            Dim oPersona As New COL_Persona
            Dim ISTT_ID As Integer
            Dim PRSN_TPPR_id As Integer
            Dim oFiltroOrdinamento As New Main.FiltroOrdinamento
            Dim oFiltroAnagrafica As New Main.FiltroAnagrafica
            Dim oCampoOrdinePersona As New Main.FiltroCampoOrdinePersona
            oPersona = Session("objPersona")
            ISTT_ID = oPersona.Istituzione.Id
            PRSN_TPPR_id = Me.DDLtipoPersona.SelectedValue

            If PRSN_TPPR_id > 0 Then
                Me.DGpersona.Columns(6).Visible = False
            Else
                Me.DGpersona.Columns(6).Visible = True
            End If

            '   oPersona.FiltroAnagrafica = "-1" 'tutti PROVVISORIO!!!!!!!!!!!

            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "prsn_anagrafica" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            ElseIf LCase(viewstate("SortExspression")) = "prsn_datanascita" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.dataNascita
            ElseIf LCase(viewstate("SortExspression")) = "prsn_mail" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.mail
            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.login
            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.tipoPersona
            ElseIf LCase(viewstate("SortExspression")) = "utcn_dataconnessione" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.dataConnessione
            ElseIf LCase(viewstate("SortExspression")) = "utcn_ip" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.indirizzoIP
            ElseIf LCase(viewstate("SortExspression")) = "utcn_browser" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.browser
            Else
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            End If

            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            'definisco il filtraggio per lettera !
            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try

            Dim valore As String
            Dim oFiltroPersona As Main.FiltroPersona

            oFiltroPersona = Main.FiltroPersona.tutte

            Select Case Me.DDLtipoRicerca.SelectedValue
                Case Main.FiltroPersona.tutte
                    valore = ""
                Case Main.FiltroPersona.nome
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.nome
                    End If
                Case Main.FiltroPersona.cognome
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.cognome
                    End If
                Case Main.FiltroPersona.codiceFiscale
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        If Len(valore) > 16 Then
                            valore = Left(valore, 16)
                            Me.TXBvalore.Text = valore
                        End If
                        oFiltroPersona = Main.FiltroPersona.codiceFiscale
                    End If
                Case Main.FiltroPersona.mail
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        If Len(valore) > 255 Then
                            valore = Left(valore, 255)
                            Me.TXBvalore.Text = valore
                        End If
                        oFiltroPersona = Main.FiltroPersona.mail
                    End If
                Case Main.FiltroPersona.matricola
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.matricola
                    End If
                Case Main.FiltroPersona.dataNascita
                    If Me.TXBvalore.Text <> "" Then
                        Try
                            If IsDate(Me.TXBvalore.Text) Then
                                valore = Me.TXBvalore.Text
                                valore = Main.DateToString(valore, False)
                                oFiltroPersona = Main.FiltroPersona.dataNascita
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                Case Main.FiltroPersona.login
                    If Me.TXBvalore.Text <> "" Then
                        valore = Me.TXBvalore.Text
                        oFiltroPersona = Main.FiltroPersona.login
                    End If

                Case Else
                    valore = ""
                    oFiltroPersona = Main.FiltroPersona.tutte
            End Select

            If oFiltroPersona <> Main.FiltroPersona.tutte Then
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.ViewState("intAnagrafica") = CType(Main.FiltroAnagrafica.tutti, Main.FiltroAnagrafica)
            End If

            Dim totale As Decimal
            Try
                oDataset = oPersona.GetUtentiConnessi(-1, Me.DGpersona.PageSize, Me.ViewState("intCurPage"), PRSN_TPPR_id, Me.DDLorganizzazione.SelectedValue, oFiltroAnagrafica, oFiltroOrdinamento, oCampoOrdinePersona, oFiltroPersona, valore, Me.DDLtipoAutenticazione.SelectedValue, ISTT_ID)
                If oDataset.Tables(0).Rows.Count > 0 Then
                    Me.DGpersona.VirtualItemCount = oDataset.Tables(0).Rows(0).Item("totale")
                    totale = Decimal.Parse(Me.DGpersona.VirtualItemCount / Me.DGpersona.PageSize)
                    If ricalcola Then
                        Me.ViewState("intCurPage") = 0
                        Me.DGpersona.CurrentPageIndex = 0
                    End If
                    Return oDataset
                Else
                    Me.ViewState("intCurPage") = 0
                    Me.DGpersona.VirtualItemCount = 0
                    Me.DGpersona.CurrentPageIndex = 0
                    Return oDataset
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Me.DGpersona.VirtualItemCount = 0
                Me.DGpersona.CurrentPageIndex = 0
                Return oDataset
            End Try
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_UtentiConnessi"
        oResource.Folder_Level1 = "Admin_globale"
        oResource.setCulture()
    End Sub
#End Region
#Region "Gestione Griglia"

    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        Me.PNLpaginazione.Visible = False
        If oRecord > Me.DGpersona.PageSize Or oRecord > Me.DDLpaginazione.Items(0).Value Or Me.DGpersona.VirtualItemCount > Me.DGpersona.PageSize Then
            Me.DGpersona.AllowPaging = True
            Me.DGpersona.PageSize = Me.DDLpaginazione.SelectedItem.Value
            PNLpaginazione.Visible = True
        Else
            Me.DGpersona.AllowPaging = False
            PNLpaginazione.Visible = False
        End If
        If oRecord < 0 Then
            Me.PNLpersona.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecord.Visible = True
            LBnorecord.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
        End If
    End Sub

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGpersona.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        BindGriglia()
    End Sub
    Private Sub CambioPagina(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGpersona.PageIndexChanged
        Dim oSortExpression, oSortDirection As String

        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        BindGriglia()
    End Sub
    Private Sub Cambio_NumPagine(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpaginazione.SelectedIndexChanged
        DGpersona.PageSize = DDLpaginazione.Items(DDLpaginazione.SelectedIndex).Value
        DGpersona.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        BindGriglia()
    End Sub

    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGpersona.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
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

                    'oLabelAfter.font.name = "webdings"
                    'oLabelAfter.font.size = FontUnit.XSmall
                    'oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGpersona.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Crescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Decrescente)
                                End If


                                'oLabelAfter.font.name = oLinkbutton.Font.Name
                                'oLabelAfter.font.size = oLinkbutton.Font.Size
                                oLabelAfter.CssClass = Me.DGpersona.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                oLinkbutton.Font.Name = "webdings"
                                oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = oText
                                ' oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                '  oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        Else
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGpersona, oLinkbutton, FiltroOrdinamento.Crescente)

                                'oLabelAfter.font.name = oLinkbutton.Font.Name
                                'oLabelAfter.font.size = oLinkbutton.Font.Size
                                oLabelAfter.CssClass = Me.DGpersona.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                oLinkbutton.Font.Name = "webdings"
                                oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = "5"
                                ' oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                ' oCell.Controls.AddAt(0, oLabelBefore)
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        End If
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
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "ROW_PagerSpan_Small"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    oResource.setPageDatagrid(Me.DGpersona, oLinkbutton)
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim PRSN_ID, TPPR_id, PRSN_AUTN_ID As Integer
            'blocca sblocca

            Try
                PRSN_ID = e.Item.DataItem("PRSN_ID")
                PRSN_AUTN_ID = e.Item.DataItem("PRSN_AUTN_ID")
            Catch ex As Exception

            End Try
            Dim Cell As New TableCell


            ''bottone login come altro utente
            'Dim oLinkbutton3 As LinkButton
            ''Cell As New TableCell
            '' Dim oWebControl As WebControl
            ''Dim PRSN_ID2 As Integer
            'Try
            '    'PRSN_ID = e.Item.DataItem("PRSN_ID")
            '    '  PRSN_ID2 = CInt(DGpersona.DataKeys.Item(e.Item.ItemIndex))

            '    Cell = CType(e.Item.Cells(0), TableCell)

            '    oLinkbutton3 = Cell.FindControl("PRSN_Anagrafica")

            '    oLinkbutton3.Attributes.Add("onfocus", "window.status='Logga come utente';return true;")
            '    oLinkbutton3.Attributes.Add("onmouseover", "window.status='Logga come utente';return true;")
            '    oLinkbutton3.Attributes.Add("onclick", "window.status='Logga come utente';return true;")
            '    oLinkbutton3.Attributes.Add("onmouseout", "window.status='';return true;")

            '    oLinkbutton3.ToolTip = "cambia la password"

            'Catch ex As Exception

            'End Try

            'bottone informazioni
            Dim oImagebutton As ImageButton
            Dim Cell2 As New TableCell
            ' Dim TPPR_ID As Integer

            'Dim PRSN_ID3, TPPR_id2 As Integer
            Try
                'PRSN_ID3 = e.Item.DataItem("PRSN_ID")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./../Admin/ADM_InfoPersona.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell2 = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell2.FindControl("IMBinfo")
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','540','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','590','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','600','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','520','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Copisteria
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Dottorando
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.DocenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','620','no','yes');return false;")
                    Case Main.TipoPersonaStandard.StudenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','610','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                End Select
                oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGgriglia_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGpersona.ItemCommand
        Dim oPersona As New COL_Persona
		Dim oUtility As New OLDpageUtility(Me.Context)

        If e.CommandName = "login" Then
            Dim oPersonaAttuale As New COL_Persona
      

            oPersonaAttuale = Session("objPersona")

            oPersona.Id = DGpersona.DataKeys.Item(e.Item.ItemIndex)
			oPersona.LogonAsUser(DGpersona.DataKeys.Item(e.Item.ItemIndex), Me.Request.UserHostAddress, Session.SessionID, Me.Request.Browser.Browser, oUtility.SystemSettings.CodiceDB)
            If oPersona.Errore = Errori_Db.None Then
                oPersonaAttuale.Logout()
                oPersonaAttuale.CancellaConnessione(oPersonaAttuale.Id, Session.SessionID)

                Try
					Dim oLingua As New Lingua
                    oLingua = oPersona.Lingua
					oLingua = ManagerLingua.GetByID(oPersona.Lingua.Id)
                    Session("LinguaID") = oLingua.Id
                    Session("LinguaCode") = oLingua.Codice
                Catch ex As Exception
                    Session("LinguaID") = 2
                    Session("LinguaCode") = "en-US"
                End Try



                Dim oTreeComunita As New COL_TreeComunita
                Try
					oTreeComunita.Directory = Server.MapPath(oUtility.ApplicationUrlBase & "profili/") & oPersona.Id & "\"
                    oTreeComunita.Nome = oPersona.Id & ".xml"
                    If Not oTreeComunita.Exist Then
						oTreeComunita.AggiornaInfo(oPersona.Id, oUtility.LinguaID, oUtility.SystemSettings.Login.DaysToUpdateProfile, True)
                    End If

                Catch ex As Exception

                End Try

                'Memorizzo impostazioni utente
                Dim oImpostazioni As New COL_ImpostazioniUtente
                Try
                    oImpostazioni.Directory = Server.MapPath("./../profili/") & oPersona.Id & "\"
                    oImpostazioni.Nome = "settings_" & oPersona.Id & ".xml"
                    If oImpostazioni.Exist Then
                        oImpostazioni.RecuperaImpostazioni()
                        Session("oImpostazioni") = oImpostazioni
                    Else
                        Session("oImpostazioni") = Nothing
                    End If
                Catch ex As Exception
                    Session("oImpostazioni") = Nothing
                End Try
                'session.abandon non funziona!!!
                Session("limbo") = Nothing
                Session("objPersona") = Nothing
                Session("ORGN_id") = Nothing
                Session("Istituzione") = Nothing
                Session("IdRuolo") = Nothing
                Session("IdComunita") = Nothing


                'bisognerebbe andare a cercare le altre sessioni per settarle a nothing

                Session("limbo") = True
                Session("objPersona") = oPersona
                Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
                Session("Istituzione") = oPersona.GetIstituzione

                'Session("IdRuolo")
				Dim LinkElenco As String = ""
                Try
                    If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
						LinkElenco = "Comunita/EntrataComunita.aspx"
                    Else
						LinkElenco = "Comunita/NavigazioneTreeView.aspx"
                    End If
                Catch ex As Exception
					LinkElenco = "Comunita/EntrataComunita.aspx"
                End Try

				Response.Redirect(oUtility.ApplicationUrlBase & LinkElenco)

			Else
				'errore in fase di login
			End If
		ElseIf e.CommandName = "disconnetti" Then
			' cancella la connessione al server !!!
			oPersona.CancellaConnessioneById(DGpersona.DataKeys.Item(e.Item.ItemIndex), e.Item.Cells(12).Text)
			Me.BindGriglia(True)
		Else
			Me.BindGriglia(False)
		End If

    End Sub
#End Region


#Region "Menu"
    Private Sub LNBlista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlista.Click
        Me.TBRlista.Visible = True
        Try
            Me.Bind_TipoPersona(Me.DDLtipoPersona.SelectedValue)
        Catch ex As Exception

        End Try

        Me.BindGriglia()
    End Sub


#End Region
    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property


End Class