Imports lm.Comol.UI.Presentation

Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Chat
Public Class Chat_Ext
    Inherits System.Web.UI.Page

#Region "Oggetti e proprieta chat"
    Protected oResource As ResourceManager
    Dim Messaggi As New WS_Chat.WS_ChatSoapClient
    Dim MsgForeColor As String
    Dim MinRef As Integer = 10 '10000 'Tempo di refresh minimo: 10"
    Dim oPersona As New COL_BusinessLogic_v2.CL_persona.COL_Persona
    Dim NumUtenti As Integer = 0
    Dim IdComunita As Integer


    Public Property IDCom() As Integer
        Get
            Return CInt(ViewState("IDCom"))
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDCom") = Value
        End Set
    End Property

#End Region

#Region "Form Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region
#Region "Form Pannelli"

    Protected WithEvents IBPan1_Chat As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Chat As System.Web.UI.WebControls.Panel

    Protected WithEvents IBPan2_Stili As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Stili As System.Web.UI.WebControls.Panel
    Protected WithEvents UC_ChatStili As Comunita_OnLine.UC_StiliChat

    Protected WithEvents IBPan3_Smile As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Smile As System.Web.UI.WebControls.Panel

    Protected WithEvents IBPan4_Tools As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Utility As System.Web.UI.WebControls.Panel
    Protected WithEvents UC_ChatUtility As Comunita_OnLine.UC_UtilityChat

    Protected WithEvents IBPan5_File As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_File As System.Web.UI.WebControls.Panel
    Protected WithEvents UC_ChatFile As Comunita_OnLine.UC_ChatFile

    Protected WithEvents IBPan6_Utenti As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Utenti As System.Web.UI.WebControls.Panel
    Protected WithEvents UC_ChatUtenti As Comunita_OnLine.UC_Partecipanti_Chat

    Protected WithEvents IBPan7_Comunita As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Pan_Comunita As System.Web.UI.WebControls.Panel
    Protected WithEvents UC_ChatComunita As Comunita_OnLine.UC_comunitaChat

    Protected WithEvents ImgLinkHelp As System.Web.UI.WebControls.Image

    Protected WithEvents PanPulsantiPannelli As System.Web.UI.WebControls.Panel

    Protected WithEvents IBExit As System.Web.UI.WebControls.ImageButton

#End Region
#Region "Form altro"

    Protected WithEvents LBError As System.Web.UI.WebControls.Label
    Protected WithEvents LblChatOut As System.Web.UI.WebControls.Label
    Protected WithEvents LblComunita As System.Web.UI.WebControls.Label

    Protected WithEvents Table1 As System.Web.UI.WebControls.Table
    Protected WithEvents Pan_Write As System.Web.UI.WebControls.Panel

    Protected WithEvents TBInput As System.Web.UI.HtmlControls.HtmlTextArea

    Protected WithEvents IMBaggiorna As System.Web.UI.WebControls.ImageButton
    Protected WithEvents BtSendAll As System.Web.UI.WebControls.Button
    Protected WithEvents BtSendTo As System.Web.UI.WebControls.Button

    Protected WithEvents BtPrivate As System.Web.UI.WebControls.Button
    Protected WithEvents Lbl_Link_Help As System.Web.UI.WebControls.Label 
    Protected WithEvents UtentiCom As System.Web.UI.WebControls.Label
    Protected WithEvents Output As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents LblNumUtenti As System.Web.UI.WebControls.Label
    Protected WithEvents HL_Help As System.Web.UI.WebControls.HyperLink

    Protected WithEvents IBClear As System.Web.UI.WebControls.ImageButton

    'Protected WithEvents UC_ChatHead As Comunita_OnLine.UC_ChatHead

    Protected WithEvents Pnl_Total As System.Web.UI.WebControls.Panel
    Protected WithEvents Pnl_NoService As System.Web.UI.WebControls.Panel
    Protected WithEvents Lbl_NoService As System.Web.UI.WebControls.Label

    Protected WithEvents Lit_Skin As System.Web.UI.WebControls.Literal

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

        'Localizzazione
        If Not Page.IsPostBack Then
            Me.setupLingua()
        End If
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If


        'Verifico la disponibilità del WebService.
        'Se non è disponibile mostro un messaggio e non faccio altro.
        Dim IsWbs_ChatAlive As Boolean = False
        Try
            IsWbs_ChatAlive = Me.Messaggi.IsAlive
        Catch ex As Exception
        End Try

        Me.ShowServiceAvaiable(IsWbs_ChatAlive)
        If Not IsWbs_ChatAlive Then
            Exit Sub
        End If
        

        'Dim IdPersona As Integer

        'Aggiorno una volta al giorno In questo modo se aggiorno con F5 non perdo l'ID comunita'...
        Me.Response.AppendHeader("Refresh", 86400)

        'Refresh
        If Not Page.IsPostBack Then
            If Session("RefTime") Is Nothing Then
                Me.changeRef(MinRef)
            End If
        End If

        'Se non ho l'oggetto persona, visualizzo il pannello login
        'In questo modo e' sufficente distruggerlo per "sloggarsi"...
        Dim oPersona As New COL_Persona

        If IsNothing(Session("objPersona")) Then
            Response.Redirect("Chat_Ext_Login.aspx")
        Else
            Try
                oPersona = Session("objPersona")
            Catch ex As Exception
                Response.Redirect("Chat_Ext_Login.aspx")
            End Try
            If IsNothing(oPersona) Then
                Response.Redirect("Chat_Ext_Login.aspx")
            ElseIf oPersona.ID <= 0 Then
                Response.Redirect("Chat_Ext_Login.aspx")
            End If
        End If

        'Se e' in fase di aggionamento...
        If Me.Application.Item("SystemAcess") = False Then
            ' Accesso consentito solo agli admin
            If oPersona.TipoPersona.ID <> Main.TipoPersonaStandard.SysAdmin Then
                'Rimando direttamente alla pagina di login:
                'Sito in fase di aggiornamento...
                Response.Redirect("Chat_Ext_Login.aspx")
            End If
        End If

        'Recupero l'ID Comunita'
        'Vedo se e' stato passato
        Dim IdComNew As Integer
        Try
            IdComNew = CInt(Request.QueryString.Item("IDCom"))
        Catch ex As Exception
            IdComNew = 0
        End Try

        If IdComNew <> 0 Then
            Me.IDCom = IdComNew
        End If
        'Se non e' stato passato verifico se ce n'e' uno in sessione (Solo per compatibilita')
        If IsNothing(Me.IDCom) Or Me.IDCom = 0 Then
            Me.IDCom = Session("IdComunita")
        End If

        'Se NON ho ancora un ID Comunita' visualizzo il pannello comunita' (senza i pulsanti):
        If Me.IDCom = 0 Then
            Me.PanPulsantiPannelli.Visible = False
            Me.PanView(7)
            Me.View_ByLvl(0)
            Exit Sub
        Else 'altrimenti setto l'Id negli User control
            Me.PanPulsantiPannelli.Visible = True
            Me.SetComunita()
        End If

        'A questo punto ho un ID comunita' ed un oggetto persona!
        Dim oChat As New Services_CHAT
        Dim PermessiAssociati As String

        Dim oComunita As New COL_Comunita

        Try 'Recupero i permessi
            oComunita.Id = Me.IDCom
            PermessiAssociati = oComunita.GetPermessiForServizioByPersona(oPersona.ID, Me.IDCom, oChat.Codex)

            'Dim oRuolo As New COL_RuoloPersonaComunita
            'oRuolo.Estrai(Me.IDCom, oPersona.Id)

            If Not (PermessiAssociati = "") Then
                oChat.PermessiAssociati = PermessiAssociati
            End If
        Catch ex As Exception
            'Non dovrebbe verificarsi...
        End Try

        If Not (oChat.Read Or oChat.Write Or oChat.Admin) Then
            'NON HO PERMESSI SU COL!
            Response.Redirect("Chat_Ext_Login.aspx")
        Else
            Me.PNLpermessi.Visible = False
            Dim ChatLvl As Byte = 0
            Try
                'Dim oRuolo As New COL_RuoloPersonaComunita
                'oRuolo.Estrai(Me.IDCom, oPersona.Id)
                'Controllo se l'utente e' presente nel web service e lo inserisco
                ChatLvl = Messaggi.GetLvl(oPersona.ID, Me.IDCom)
                If ChatLvl = 0 Then 'Messaggi.GetLvl(oPersona.Id, Me.IDCom) = 0 Then
                    Messaggi.NuovoUtente(oPersona.ID, oPersona.Anagrafica, Me.IDCom)
                    ChatLvl = Messaggi.GetLvl(oPersona.ID, Me.IDCom)
                End If

                If Not Page.IsPostBack Then
                    Me.SetupStartupScript()
                End If

            Catch ex As Exception
                Me.PanView(7)
                Me.View_ByLvl(0)
                'Esco dalla sub
                Exit Sub
            End Try

            'Visulizzo in base al livello di un utente...
            Me.View_ByLvl(ChatLvl)
        End If
    End Sub

#Region "Gestione Refresh"

    Private Sub changeRef(ByVal time As Integer)
        Session("RefTime") = time.ToString
        'Per ora e' costante in tutte le "finestre CHAT".
        'Fare come per ID Comunita' se necessario
    End Sub

#End Region

#Region "Sub di inizializzazione"

    Private Sub SetupStartupScript()
    End Sub

#End Region

#Region " Pulsanti SEND"
    Private Sub BtSendAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtSendAll.Click

        If Not TBInput.Value = "" Then
            Messaggi.InviaMessaggio(Session("objPersona").Anagrafica, TBInput.Value, Session("objPersona").Id, Me.IDCom, Me.UC_ChatStili.IsBold, Me.UC_ChatStili.IsItalic, Me.UC_ChatStili.IsUnder, Me.UC_ChatStili.GetColor("Bg"), Me.UC_ChatStili.GetColor("Fore"))
        End If
        TBInput.Value = ""
        LBError.Visible = False

    End Sub

    Private Sub BtSendTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtSendTo.Click

        Dim DtUtenti As New DataTable

        Dim i As Integer
        Dim Errore As Boolean = True

        If Not Me.TBInput.Value = "" Then
            DtUtenti = Me.UC_ChatUtenti.GetUtenti.Copy

            If Not DtUtenti.Rows.Count <= 0 Then
                For i = 0 To (DtUtenti.Rows.Count - 1)
                    If Not (DtUtenti.Rows(i).Item("ID") = Session("objPersona").Id) Then
                        Messaggi.InviaMessaggio(Session("objPersona").Anagrafica, "[Rivolto a: " & DtUtenti.Rows(i).Item("Nome") & "] " & TBInput.Value, Session("objPersona").Id, Me.IDCom, Me.UC_ChatStili.IsBold, Me.UC_ChatStili.IsItalic, Me.UC_ChatStili.IsUnder, Me.UC_ChatStili.GetColor("Bg"), Me.UC_ChatStili.GetColor("Fore"))
                    End If
                Next
                Me.TBInput.Value = ""
                TBInput.DataBind()

                LBError.Visible = False
            Else
                LBError.Visible = True
            End If
        End If
        'Me.View_ByLvl(Messaggi.GetLvl(oPersona.Id, Me.IDCom))
    End Sub

    Private Sub BtPrivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtPrivate.Click
        Dim DtUtenti As New DataTable

        Dim i As Integer
        Dim Errore As Boolean = True

        If Not Me.TBInput.Value = "" Then
            DtUtenti = Me.UC_ChatUtenti.GetUtenti.Copy

            If Not DtUtenti.Rows.Count <= 0 Then
                For i = 0 To (DtUtenti.Rows.Count - 1)
                    If Not (DtUtenti.Rows(i).Item("ID") = Session("objPersona").Id) Then
                        Messaggi.InviaMessaggioPrivato(Session("objPersona").Anagrafica, "[PRIVATO per: " & DtUtenti.Rows(i).Item("Nome") & "] " & TBInput.Value, Session("objPersona").Id, DtUtenti.Rows(i).Item("ID"), Me.IDCom, Me.UC_ChatStili.IsBold, Me.UC_ChatStili.IsItalic, Me.UC_ChatStili.IsUnder, Me.UC_ChatStili.GetColor("Bg"), Me.UC_ChatStili.GetColor("Fore"))
                    End If
                Next
                Me.TBInput.Value = ""
                LBError.Visible = False
            Else
                LBError.Visible = True
            End If
        End If
        'Me.View_ByLvl(Messaggi.GetLvl(oPersona.Id, Me.IDCom))
    End Sub


#End Region

#Region "Altri pulsanti"
    Private Sub IBExit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBExit.Click
        Try
            Session("ChatMsg" & Me.IDCom.ToString) = ""
            'Togliere l'utente da wbschat
            Me.Messaggi.RemUtente(Session("objPersona").id, Me.IDCom)
        Catch ex As Exception
            Me.IDCom = 0
        End Try
        'Per ora effettua solo il logout...
        'Ma non cancello loggetto persona in sessione... da rivedere... :p
        'Me.Response.Close()
        Response.Redirect("Chat_Ext_Login.aspx")
    End Sub

    Private Sub IBClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBClear.Click
        Session("ChatMsg" & Me.IDCom) = ""
    End Sub
#End Region

#Region "Visualizzazione pannelli e per livello"

    Private Sub View_ByLvl(ByVal Lvl As Integer)
        Select Case Lvl
            Case 4 ' Admin
                'tasti di selezione pannelli
                Me.UC_ChatUtenti.IsAdmin = True
                Me.UC_ChatUtenti.IsWriter = True
                'tasti invio messaggi
                Me.BtPrivate.Enabled = True
                Me.BtPrivate.Visible = True

                Me.BtSendAll.Enabled = True
                Me.BtSendAll.Visible = True

                Me.BtSendTo.Enabled = True
                Me.BtSendAll.Visible = True

                'Pannello scrittura
                Me.Pan_Write.Enabled = True
                Me.Pan_Write.Visible = True

            Case 2 ' Writer
                'tasti di selezione pannelli
                Me.UC_ChatUtenti.IsAdmin = False
                Me.UC_ChatUtenti.IsWriter = True

                'tasti invio messaggi
                Me.BtPrivate.Enabled = True
                Me.BtPrivate.Visible = True

                Me.BtSendAll.Enabled = True
                Me.BtSendAll.Visible = True

                Me.BtSendTo.Enabled = True
                Me.BtSendAll.Visible = True

                'Pannello scrittura
                Me.Pan_Write.Enabled = True
                Me.Pan_Write.Visible = True

            Case 1 ' Reader
                Me.UC_ChatUtenti.IsAdmin = True
                Me.UC_ChatUtenti.IsWriter = True

                'tasti invio messaggi
                Me.BtPrivate.Enabled = False
                Me.BtPrivate.Visible = False

                Me.BtSendAll.Enabled = False
                Me.BtSendAll.Visible = False

                Me.BtSendTo.Enabled = False
                Me.BtSendAll.Visible = False

                'Pannello scrittura
                Me.Pan_Write.Enabled = False
                Me.Pan_Write.Visible = False

            Case Else

                Me.UC_ChatUtenti.IsAdmin = False
                Me.UC_ChatUtenti.IsWriter = False

                'tasti invio messaggi
                Me.BtPrivate.Enabled = False
                Me.BtPrivate.Visible = False

                Me.BtSendAll.Enabled = False
                Me.BtSendAll.Visible = False

                Me.BtSendTo.Enabled = False
                Me.BtSendAll.Visible = False

                'Pannello scrittura
                Me.Pan_Write.Enabled = False
                Me.Pan_Write.Visible = False
                Me.PanView(-1)
        End Select
    End Sub


    Private Sub PanView(ByVal Id As Integer)
        '1  -   Chat
        '2  -   Stili
        '3  -   Faccine
        '4  -   Utility
        '5  -   File
        '6  -   Utenti
        '7  -   Comunita
        '8  -   Help
        '9  -   LOGIN!!!
        Select Case Id
            Case 1 'Chat - Primary - OK!

                Me.Pan_Chat.Visible = True


                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = False

            Case 2 'Stili - Half - OK!
                Me.Pan_Chat.Visible = True

                Me.Pan_Stili.Visible = True '20
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = False

            Case 3 'Faccine - Half - OK!
                Me.Pan_Chat.Visible = True

                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = True '60
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = False

            Case 4 'Utility - Half - OK!
                Me.Pan_Chat.Visible = True

                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = True   '20
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = False

            Case 5 ' File - FULL
                Me.Pan_Chat.Visible = False

                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = True  'Full
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = False

            Case 6 ' Utenti - Half
                Me.Pan_Chat.Visible = True

                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = True    '130
                Me.Pan_Comunita.Visible = False

            Case 7 ' Comunita - FULL
                'Se non ci sono comunita' in elenco provo a tirarle su di nuovo... Ma solo in quel caso!!!
                If Me.UC_ChatComunita.NumCom < 1 Then
                    Me.UC_ChatComunita.BindComunita()
                End If

                Me.Pan_Chat.Visible = False

                Me.Pan_Stili.Visible = False
                Me.Pan_Smile.Visible = False
                Me.Pan_Utility.Visible = False
                Me.Pan_File.Visible = False
                Me.Pan_Utenti.Visible = False
                Me.Pan_Comunita.Visible = True  'full
            Case -1 'No Chat o errore
                Me.PNLpermessi.Visible = True
                Me.Pan_Chat.Visible = False
        End Select

    End Sub

    Private Sub IBPan1_Chat_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan1_Chat.Click
        PanView(1)
    End Sub
    Private Sub IBPan2_Stili_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan2_Stili.Click
        PanView(2)
    End Sub
    Private Sub IBPan3_Smile_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan3_Smile.Click
        PanView(3)
    End Sub
    Private Sub IBPan4_Tools_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan4_Tools.Click
        PanView(4)
    End Sub
    Private Sub IBPan5_File_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan5_File.Click
        PanView(5)
    End Sub
    Private Sub IBPan6_Utenti_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan6_Utenti.Click
        Me.UC_ChatUtenti.Refresh(Me.IDCom)
        PanView(6)
    End Sub
    Private Sub IBPan7_Comunita_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBPan7_Comunita.Click
        Me.UC_ChatComunita.BindComunita()
        PanView(7)
    End Sub

    Private Sub EnableButton(ByVal Value As Boolean)
        'fare un unico panel dove inserirli tutti?
        Me.BtPrivate.Enabled = Value
        Me.BtSendAll.Enabled = Value
        Me.BtSendTo.Enabled = Value

        Me.IBPan1_Chat.Enabled = Value
        Me.IBPan2_Stili.Enabled = Value
        Me.IBPan3_Smile.Enabled = Value
        Me.IBPan4_Tools.Enabled = Value
        Me.IBPan5_File.Enabled = Value
        Me.IBPan6_Utenti.Enabled = Value
        Me.IBPan7_Comunita.Enabled = Value
        'Me.IBPan8_Help.Enabled = Value

    End Sub
#End Region

#Region "Localizzazione"

    Private Sub setupLingua()
        Try
            If IsNumeric(Session("LinguaID")) And Session("LinguaCode") <> "" Then

            Else
                Dim LinguaCode As String

                LinguaCode = "en-US"
                Try
                    LinguaCode = Request.UserLanguages(0)
                Catch ex As Exception
                    LinguaCode = "en-US"
                End Try
                If Request.Browser.Cookies = True Then
                    Try
                        LinguaCode = Request.Cookies("LinguaCode").Value
                    Catch ex As Exception

                    End Try
                End If
                'Setto ora il valore nelle variabili di sessione.....
				Dim oLingua As New Lingua
				oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
				If Not IsNothing(oLingua) Then
					Session("LinguaID") = oLingua.Id
					Session("LinguaCode") = oLingua.Codice
				Else
					Session("LinguaID") = 2
					Session("LinguaCode") = "en-US"
				End If
            End If

            SetCulture(Session("LinguaCode"))

            Me.SetupInternazionalizzazione()
        Catch exUserLanguages As Exception
        End Try
    End Sub

    Public Function SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()


    End Function

    Public Sub SetupInternazionalizzazione()
        With oResource
            'Pulsanti send
            .setButton(Me.BtSendAll, False, False, False, False)
            .setButton(Me.BtSendTo, False, False, False, False)
            .setButton(Me.BtPrivate, False, False, False, False)
            'Errore
            Me.SelLabelError("SelUt")
            'Strumenti
            .setImageButton(Me.IBPan1_Chat, True, True, True, False)
            .setImageButton(Me.IBClear, True, True, False, False)
            .setImageButton(Me.IBPan2_Stili, True, True, True, False)
            .setImageButton(Me.IBPan3_Smile, True, True, True, False)
            .setImageButton(Me.IBPan4_Tools, True, True, True, False)
            .setImageButton(Me.IBPan5_File, True, True, True, False)
			'Me.IBPan5_File.Enabled = False 'Disabilitato fino alla prossima versione file

            .setImageButton(Me.IBPan6_Utenti, True, True, True, False)
            .setImageButton(Me.IBPan7_Comunita, True, True, True, False)
            .setImage(Me.ImgLinkHelp, True)
            .setImageButton(Me.IBExit, True, True, True, False)

            .setLabel(Me.Lbl_Link_Help)

            .setLabel(Me.UtentiCom)

            .setLabel(Me.LblChatOut)
        End With
    End Sub

    Public Sub SelLabelError(ByVal Tipo As String)
        'SelUT - Utente non selezionato
        Me.LBError.Text = Me.oResource.getValue(Tipo) 'oNome as String
    End Sub

#End Region

#Region "Gestione Comunita"

    Private Sub SetComunita()
        'Passo il valore per visualizzare la comunita' nell'header

        Dim oDataset As New DataSet
        Dim NomeCom As String = ""
        If IsNothing(Session("ListaCom")) Then
            Me.UC_ChatComunita.BindComunita()
        End If

        If Not IsNothing(Session("ListaCom")) Then
            oDataset = Session("ListaCom")
            For Each row As DataRow In oDataset.Tables(0).Rows
                If row.Item("CMNT_id") = Me.IDCom Then
                    NomeCom = row.Item("CMNT_nome")
                    Dim i As Integer = row.Item("NumUtenti")
                    If i = 0 Then i += 1
                    Me.LblNumUtenti.Text = i.ToString
                    Exit For
                End If
            Next
        Else
            Me.View_ByLvl(0)
        End If


        'Passo il valore all'iframe...
        'Non potendo intervenire direttamente sulla proprieta' dell'IFRAME
        'lo costruisco come stringa all'interno di una label...
        Me.LblChatOut.Text = "<IFRAME id=Output name=Output marginWidth=0 marginHeight=0 src=ChatOutput_Ext.aspx?IDCom=" & Me.IDCom.ToString & " frameBorder=yes width=99% scrolling=yes height=99% runat=server align=top></IFRAME>"
        'e cosi' funziona!!!
        Me.LblComunita.Text = NomeCom
        Me.UC_ChatUtility.IDCom = Me.IDCom
        Me.UC_ChatFile.IDCom = Me.IDCom
        Lbl_Link_Help.Text = "<a href=ChatPrint.aspx?NmCom=" & NomeCom.Replace(" ", "%20") & "&IDCom=" & Me.IDCom.ToString & " target=_Blank >"
        Lbl_Link_Help.Text &= "<img src=../images/print.gif alt=Print></a>"
    End Sub

#End Region


    Private Sub ShowServiceAvaiable(ByVal IsAlive As Boolean)
        Me.Pnl_Total.Visible = IsAlive
        Me.Pnl_NoService.Visible = Not IsAlive
    End Sub

    'Usare "Me.ComID" per l'ID comunità, che localmente può cambiare!

#Region "SkinNew - CSS"


    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        If (String.IsNullOrEmpty(Me.Lit_Skin.Text)) Then
            Me.Lit_Skin.Text = Me.SkinStyle()
        End If
    End Sub

    Public ReadOnly Property SkinStyle As String
        Get

            Dim HTMLStyleSkin As String

            'NEW SKIN
            Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath

            Dim Organization_Id As Integer = PageUtility.GetSkinIdOrganization
            Dim Community_Id As Integer = Me.IDCom 'Me.CurrentContext.UserContext.CurrentCommunityID


            'Main CSS
            HTMLStyleSkin = ServiceSkinNew.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf

            'Admin CSS
            'HTMLStyleSkin &= SkinService.GetCSSHtml( _
            '    Me.CurrentContext.UserContext.CurrentCommunityID, _
            '    Me.CurrentContext.UserContext.CurrentCommunityOrganizationID, _
            '    VirPath, _
            '    Me.SystemSettings.DefaultLanguage.Codice, _
            '    lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin) & vbCrLf

            'IE CSS
            If (Request.Browser.Browser.Equals("IE")) Then
                HTMLStyleSkin &= ServiceSkinNew.GetCSSHtml( _
                    Community_Id, _
                    Organization_Id, _
                    VirPath, _
                    Me.SystemSettings.DefaultLanguage.Codice, _
                    lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE, _
                    Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf
            End If
            'End If

            Return HTMLStyleSkin
        End Get
    End Property

    Public ReadOnly Property BaseUrl() As String
        Get
            Dim url As String = Me.Request.ApplicationPath
            If url.EndsWith("/") Then
                Return url
            Else
                Return url + "/"
            End If
        End Get
    End Property

    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property

    'Private Function GetSkinOrganizationId() As Integer
    '    Dim Organization_Id As Integer = 0
    '    If Me.IDCom > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
    '        'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
    '    Else
    '        'Non funziona nessuno dei due...
    '        'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
    '        Organization_Id = PageUtility.UserDefaultIdOrganization
    '    End If
    '    Return Organization_Id
    'End Function

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private _PageUtility As OLDpageUtility

    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property

#End Region
End Class
