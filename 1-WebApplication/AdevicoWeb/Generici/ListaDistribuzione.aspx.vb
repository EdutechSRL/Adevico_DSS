Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Entities.Configuration
Imports lm.Comol.Core.File

Partial Public Class ListaDistribuzione
    Inherits PageBase
    Implements IviewLsMail
    Private _Servizio As Services_Mail


    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property

    Private ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Mail Implements IviewLsMail.Servizio
        Get
            If IsNothing(_Servizio) Then
                If Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_Mail(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_Mail.Codex))
                Else
                    _Servizio = MyBase.ElencoServizi.Find(Services_Mail.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = New Services_Mail("00000000000000000000000000000000")
                    End If
                End If
            End If
            Servizio = _Servizio
        End Get
    End Property


#Region "Private Property"
    ''' <summary>
    ''' Indice dei possibili avvisi
    ''' </summary>
    ''' <remarks>
    ''' Utilizzato soprattuto per l'internazionalizzazione ed una miglior gestione degli errori
    ''' </remarks>
    Private Enum Avviso
        zeroByte = 0
        Esiste = 1
        Uploaded = 2
        OverByte = 3
        NonEsiste = 4
        ErroreGenerico = 5
    End Enum

    Private oPresenter As PresenterLsMail
    Private _ObjectPath As ObjectFilePath

    Public ReadOnly Property ScormDrivePath() As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Scorm)
            End If
            Path = _ObjectPath.Drive & Me.ComunitaLavoroID & "\"
            Path = Replace(Path, "\/", "\")
            Return Path
        End Get
    End Property
    Private ReadOnly Property MailListDirectoryFull() As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Mail)
            End If
            Path = _ObjectPath.Drive & "Liste\" & Me.UtenteCorrente.ID & "\"
            Path = Replace(Path, "\/", "\")
            Return Path
            'Return me.BaseUrl a Server.MapPath("./../Mail/Liste/") & Me.UtenteCorrente.Id & "\"
        End Get
    End Property
    Private ReadOnly Property MailListDirectoryRel() As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Mail)
            End If
            Path = _ObjectPath.Virtual & "Liste/" & Me.UtenteCorrente.ID & "/"
            Path = Replace(Path, "\/", "/")
            Return Path
            'Return "./Mail/Liste/" & Me.UtenteCorrente.Id & "/"
        End Get
    End Property

    ''' <summary>
    ''' Restituisce un oggetto mail compilato correttamente con tutti i parametri necessari
    ''' </summary>
    ''' <value></value>
    ''' <returns>COL_BusinessLogic_v2.COL_E_Mail contenente i dati selezionati dall'utente</returns>
    ''' <remarks>AGGIUNGERE I CONTROLLI!!!</remarks>
    Public ReadOnly Property oMail() As COL_BusinessLogic_v2.COL_E_Mail Implements IviewLsMail.oMail
        Get
            Dim arrListaFiles As Array
            Dim NumFile As Integer

            Dim _Mail As New COL_E_Mail(Me.LocalizedMail)
            With _Mail
                .Mittente = New MailAddress(Me.UtenteCorrente.Mail, Me.UtenteCorrente.Anagrafica)

                .Oggetto = Me.TXBObj.Text
                .Body = Me.TXBbody.Text

                For Each item As ListItem In Me.CBL_Liste.Items
                    If item.Selected Then
                        MailingManager.AddMailAddressCollection(item.Value, .IndirizziTO, .IndirizziCC)
                    End If
                Next

                'Carico attachments
                'Rivedere L'UPLOAD!!!

                'Dim ofile As New COL_File

                Dim fMSG As FileMessage = ContentOf.Directory(Me.MailListDirectoryFull, NumFile, arrListaFiles, True)
                If NumFile > 0 Then
                    Dim ArrListFilesTemp As ArrayList
                    For Each fileName As String In arrListaFiles
                        ArrListFilesTemp.Add(fileName)
                    Next
                    .Attachment = ArrListFilesTemp
                End If

                Try
                    .NotificaRicezione = Me.CBXricezione.Checked
                Catch ex As Exception
                    oMail.NotificaRicezione = False
                End Try
                Try
                    .HasCopiaMittente = Me.CBXcopiamittente.Checked
                Catch ex As Exception
                    .HasCopiaMittente = True
                End Try

            End With
            Return _Mail
        End Get
    End Property
    ''' <summary>
    ''' Mostra nella pagina il mittente della mail (l'utente corrente)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property Mittente() As String Implements IviewLsMail.Mittente
        Set(ByVal value As String)
            Me.TXBFrom.Text = value
        End Set
    End Property
#End Region

    Private Sub MailingListSender_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oPresenter = New PresenterLsMail(Me)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim stringa As String
        stringa = TXBnascosto.Value
        If stringa <> "" Then
            EliminaAllegato(stringa)
            TXBnascosto.Value = ""
        End If
    End Sub

#Region "PageBase"
    Public Overrides Sub BindDati()

        '	If Not Page.IsPostBack Then
        Me.UC_MailingList1.BindInterni() 'Così fa il bind del CTRL sorgente comunità
        oPresenter.Init()
        Me.NominativiShow(Nominativi.HideAll)
        '	End If

        'Dim stringa As String
        'stringa = TXBnascosto.Value
        'If stringa <> "" Then
        '	EliminaAllegato(stringa)
        '	TXBnascosto.Value = ""
        'End If

        'Ora, se sono nel portale, controllo HasComunità, perchè i permessi son già lì...
        'If Not Page.IsPostBack Then
        If Me.isPortalCommunity Then
            If Me.UC_MailingList1.HasComunita Then
                Me.Show(IviewLsMail.Panel.InvioMail)
            Else
                Me.BindNoPermessi()
            End If
        Else
            Me.Show(IviewLsMail.Panel.InvioMail)
        End If
        'End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Show(IviewLsMail.Panel.NoPermessi)
        'Me.PNLcontenuto.Visible = False
        'Me.PNLinviato.Visible = False
        'Me.PNLerrore.Visible = False
        'Me.PNLpermessi.Visible = True
        'Me.Resource.setLabel_To_Value(LBNopermessi, "LBNopermessi.text")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If Me.isPortalCommunity Then
            Return True
        Else
            Return Servizio.Admin Or Servizio.SendMail
        End If
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MailingListSender", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBfrom)
            .setLabel(Me.LBa)
            .setLabel(Me.LBobj)
            .setLabel(Me.LBattach)
            .setLabel(Me.LBbody)
            .setLabel(LBL_DestMail)

            .setLabel(Me.LblErrore_Dest)
            Me.LblErrore_Dest.Text = "<br />" & Me.LblErrore_Dest.Text & "<br />"

            .setCheckBox(Me.CBXcopiamittente)
            .setCheckBox(Me.CBXricezione)
            .setButton(Me.BTallega)
            .setButton(Me.Btn_ModificaLista)

            .setLabel(Me.Lbl_InvioOK_t)
            .setButton(Me.BTN_invioOk_Back)
            .setLabel(Me.Lbl_InvioErr_t)
            .setButton(Me.BTN_invioErr_Back)

            .setRegularExpressionValidator(Me.REVvalid)
            .setRequiredFieldValidator(Me.RFV_Subject, True, False)

            .setButton(Me.BTN_DelAllAttach, True, False, True, True)

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region
#Region "liste"
    Private Sub Btn_ModificaLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_ModificaLista.Click

        SalvaIdListe()

        Me.UC_MailingList1.BindSelected(Me.ViewState("IdListe"))
        Me.Show(IviewLsMail.Panel.GestioneListe)
    End Sub
    Private Sub SalvaIdListe()
        Dim IdListe As New System.Collections.Generic.List(Of Integer)
        For Each item As ListItem In Me.CBL_Liste.Items
            If item.Selected Then
                IdListe.Add(CInt(item.Value))
            End If
        Next
        Me.ViewState("IdListe") = IdListe
    End Sub

    Private Sub UC_MailingList1_Selected(ByVal SelectedList As System.Collections.Generic.List(Of Integer)) Handles UC_MailingList1.Selected

        If Not IsNothing(SelectedList) Then
            Me.ViewState("IdListe") = SelectedList
        Else
            Me.ViewState("IdListe") = Nothing
        End If

        'Dim IdListe As New System.Collections.Generic.List(Of Integer)
        'Try
        '    IdListe = Me.ViewState("IdListe")
        'Catch ex As Exception
        'End Try

        'For Each IdLista As Integer In SelectedList
        '    If Not IdListe.Contains(IdLista) Then
        '        IdListe.Add(IdLista)
        '    Else
        '        IdListe.Remove(IdLista)
        '    End If

        '    Me.ViewState("IdListe") = IdListe
        'Next        

        Me.oPresenter.BindListe()
        Me.Show(IviewLsMail.Panel.InvioMail)
    End Sub
    ''' <summary>
    ''' Aggiorna l'elenco delle liste con i valori passati
    ''' </summary>
    ''' <param name="Items">Elenco di oggetti "COL_BusinessLogic_v2.MailingList" da visualizzare nella pagina.</param>
    ''' <remarks>
    ''' Utilizzato dal presenter.
    ''' L'elenco DEVE essere paginato e contenere SOLO gli oggetti da visualizzare.
    ''' </remarks>
    Public Sub BindListe(ByVal Items As System.Collections.Generic.List(Of COL_BusinessLogic_v2.MailingList)) Implements IviewLsMail.BindListe
        If Not IsNothing(Items) Then
            Me.CBL_Liste.Items.Clear()

            Dim CBitem As ListItem
            Dim IdListe As New System.Collections.Generic.List(Of Integer)
            Try
                IdListe = Me.ViewState("IdListe")
            Catch ex As Exception
            End Try

            For Each oMailList As COL_BusinessLogic_v2.MailingList In Items
                CBitem = New ListItem()
                With CBitem
                    .Text = oMailList.Nome
                    .Value = oMailList.Id
                    If Not IsNothing(IdListe) Then
                        .Selected = IdListe.Contains(CInt(oMailList.Id))
                    End If
                End With
                Me.CBL_Liste.Items.Add(CBitem)
            Next

            'Me.CBL_Liste.AutoPostBack = Me.Cbx_ShowNominativi.Checked


            Me.CBL_Liste.DataBind()

            Me.LBL_NoListe.Visible = False
            Me.CBL_Liste.Visible = True
            Me.BTNinvia.Disabled = False
        Else
            Me.CBL_Liste.Visible = False
            Me.LBL_NoListe.Visible = True
            Me.BTNinvia.Disabled = True
        End If
    End Sub
#End Region
#Region "Gestione Allegati"
    ''' <summary>
    ''' Carica gli allegati
    ''' </summary>
    ''' <param name="mostraErrore">Indica se mostrare o meno messaggi di errore</param>
    ''' <param name="Bind">Indica se ricaricare l'elenco dopo l'upload</param>
    ''' <param name="StrErrore">Stringa da visualizzare in caso di errore</param>
    ''' <remarks>
    ''' Gli allegati vengono caricati nel server, per poter essere successivamente inseriti nella mail
    ''' </remarks>
    Public Sub LoadAttachments(Optional ByVal mostraErrore As Boolean = True, Optional ByVal Bind As Boolean = False, Optional ByVal StrErrore As String = "") Implements IviewLsMail.LoadAttachments

        Try
            'Dim CMNT_ID As Integer = Session("IdComunita")
            Dim arrListaFiles As Array
            'Dim oFile As New COL_File
            Dim nomeFile As String
            Dim numFile, dimFile, i As Integer
            Dim fMSG As FileMessage

            Environment.CurrentDirectory = Server.MapPath("./../")
            fMSG = ContentOf.Directory(Me.MailListDirectoryFull, numFile, arrListaFiles, False)

            dimFile = ContentOf.Directory_Size(Me.MailListDirectoryFull, False)
            Dim HasFile As Boolean = False

            If fMSG = FileMessage.Read Then
                If dimFile > 6291456 Then '6MB in byte
                    Me.BTallega.Enabled = False
                    If mostraErrore = True Then
                        Me.LBLErroreAttach.Text = Me.Resource.getValue("LBAvviso." & Me.Avviso.OverByte)
                        'Me.LBLErroreAttach.Visible = True
                        '    Me.PNLcontenuto.Visible = False
                        '    PNLerrore.Visible = True
                    Else
                        '    Me.PNLcontenuto.Visible = True
                        '    PNLerrore.Visible = False
                        'Me.LBLErroreAttach.Text = ""
                    End If

                    '  LBerrore.Text = "Gli allegati sono troppo pesanti: " & dimFile & " Kbyte.Max 2000 Kbyte"
                    'oResource.setLabel_To_Value(LBerrore, "InfoErrore1.text")
                    Me.LBLErroreAttach.Text = Replace(Me.LBLErroreAttach.Text, "#%%#", dimFile)
                    BTNinvia.Disabled = True
                    'BTNinviaAlto.Disabled = True
                Else
                    Me.BTallega.Enabled = True
                    BTNinvia.Disabled = False
                    Me.LBLErroreAttach.Text = StrErrore
                    'BTNinviaAlto.Disabled = False
                End If

                i = 1
                Dim row As New TableRow
                For Each nomeFile In arrListaFiles
                    HasFile = True

                    Dim oButton As New System.Web.UI.WebControls.ImageButton
                    If i Mod 2 <> 0 Then
                        row = New TableRow
                    End If
                    Dim cella As New TableCell
                    Dim cellaB As New TableCell
                    cellaB.Width = New System.Web.UI.WebControls.Unit(15)
                    cella.Width = New System.Web.UI.WebControls.Unit(320)

                    oButton.CssClass = ""
                    oButton.CommandName = Me.Resource.getValue("BTN.elimina")
                    oButton.AlternateText = Me.Resource.getValue("BTN.X")
                    oButton.CommandArgument = nomeFile
                    oButton.ImageUrl = ".\..\images\cestino.gif"
                    oButton.Attributes.Add("onclick", "document.forms[0]." & Replace(Me.TXBnascosto.UniqueID, ":", "_") & ".value='" & nomeFile.Replace("'", "\'") & "';document.forms[0].submit();return false;")
                    cellaB.Controls.Add(oButton)
                    row.Cells.Add(cellaB)

                    cella.Controls.Add(New LiteralControl(nomeFile))
                    row.Cells.Add(cella)

                    If i Mod 2 = 0 Then
                        TBLattach.Rows.Add(row)
                    ElseIf i Mod 2 <> 0 And i = numFile Then
                        row.Cells.Add(New TableCell)
                        row.Cells.Add(New TableCell)
                        TBLattach.Rows.Add(row)
                    End If
                    i = i + 1
                Next
                'If arrListaFiles.Count > 0 Then
                '	'Me.MostraAttach()
                'End If
            End If
            If Not HasFile Then
                Me.BTN_DelAllAttach.Visible = False
                Me.TBLattach.Rows.Clear()
                Me.TBLattach.Visible = False
            Else
                Me.TBLattach.Visible = True
                Me.BTN_DelAllAttach.Visible = True
            End If

            If Bind Then
                Me.TBLattach.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' Elimina un allegato specifico
    ''' </summary>
    ''' <param name="nomeFile"></param>
    ''' <remarks></remarks>
    Private Sub EliminaAllegato(ByVal nomeFile As String)
        Dim errorstring As String = ""
        Try
            Dim oFile As New COL_File
            If oFile.Errore = Errore_File.fileNotFound Then
                errorstring = Me.Resource.getValue("LBAvviso." & Me.Avviso.NonEsiste)
            Else
                Environment.CurrentDirectory = Server.MapPath("./../")
                Delete.File_FM(Me.MailListDirectoryRel & nomeFile)
            End If
        Catch ex As Exception
            errorstring = Me.Resource.getValue("LBAvviso." & Me.Avviso.ErroreGenerico)
        End Try
        Me.LoadAttachments(False, True, errorstring)
    End Sub
    ''' <summary>
    ''' Elimina tutti gli allegati relativi all'utente corrente.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EliminaTuttiAllegati()
        Try
            Environment.CurrentDirectory = Server.MapPath("./../")
            Delete.Directory(Me.MailListDirectoryRel, True)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BTallega_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTallega.Click

        If (fileAllega.PostedFile.FileName.ToString <> "") Then
            'BTconferma.Enabled = False
            'PNLattach.Visible = False
            'PNLtransfer.Visible = True
            Dim ofile As New COL_File

            Environment.CurrentDirectory = Server.MapPath("./../")

            'prima di fare l'upload devo controllare che la dimensione totale attuale non superi quella massima

            ofile.Upload(fileAllega, Me.MailListDirectoryFull)

            Dim strErrore As String
            Select Case ofile.Errore
                Case Errore_File.zeroByte
                    'Response.Write("zerobyte")
                    '           Me.LBLErroreAttach.Text()
                    strErrore = Me.Resource.getValue("LBAvviso." & Me.Avviso.zeroByte)
                    'Me.LBLErroreAttach.Visible = True
                Case Errore_File.exsist
                    'Me.LBLErroreAttach.Text 
                    strErrore = Me.Resource.getValue("LBAvviso." & Me.Avviso.Esiste)
                    'Me.LBLErroreAttach.Visible = True
                Case Errore_File.none
                    'Me.LBLErroreAttach.Text 
                    strErrore = Me.Resource.getValue("LBAvviso." & Me.Avviso.Uploaded)
                    'Me.LBLErroreAttach.Visible = False
            End Select

            Me.TBLattach.Rows.Clear()
            Me.LoadAttachments(, True, strErrore)
        End If
    End Sub
#End Region

    Private Sub UC_MailingList1_Undo() Handles UC_MailingList1.Undo
        Me.Show(IviewLsMail.Panel.InvioMail)
    End Sub
    ''' <summary>
    ''' Invia la mail
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' La sub che viene eseguita dal comando della pagina.
    ''' Richiama i metodi del presenter per inviare la mail.
    ''' </remarks>
    Sub SendMail(ByVal obj As Object, ByVal e As EventArgs)
        If Me.HasPermessi Then
            If Page.IsValid Then
                Me.oPresenter.SendMail()
            End If
        End If
    End Sub
    Public Sub Show(ByVal Id As IviewLsMail.Panel) Implements IviewLsMail.Show
        Me.LblErrore_Dest.Visible = False

        Me.PNLcontenuto.Visible = False
        Me.PNLGestioneListe.Visible = False
        Me.PNLinviato.Visible = False
        Me.PNLerrore.Visible = False
        Me.PNLpermessi.Visible = False

        Select Case Id
            Case IviewLsMail.Panel.NoPermessi
                Me.PNLpermessi.Visible = True
            Case IviewLsMail.Panel.InvioMail
                Me.PNLcontenuto.Visible = True
            Case IviewLsMail.Panel.GestioneListe
                Me.UC_MailingList1.ResetVista()
                Me.PNLGestioneListe.Visible = True
            Case IviewLsMail.Panel.MessageOK
                Me.PNLinviato.Visible = True
            Case IviewLsMail.Panel.MessageError
                Me.PNLerrore.Visible = True
        End Select


    End Sub


    Private Sub BTN_invioErr_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_invioErr_Back.Click
        Me.Show(IviewLsMail.Panel.InvioMail)
    End Sub

    Private Sub BTN_invioOk_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_invioOk_Back.Click
        Me.TXBObj.Text = ""
        Me.TXBbody.Text = ""

        Me.ViewState("IdListe") = Nothing
        Me.BindDati()
        Me.LoadAttachments()
        Me.Show(IviewLsMail.Panel.InvioMail)
    End Sub

    Public Sub ShowNoDestinatari(ByVal Show As Boolean) Implements IviewLsMail.ShowNoDestinatari
        Me.LblErrore_Dest.Visible = Show
    End Sub


#Region "Nominativi"

    Private Sub BindNominativi()

        Dim oAddress As System.Net.Mail.MailAddress
        Dim AddressList As New System.Net.Mail.MailAddressCollection
        'oAddress.Address
        'oAddress.DisplayName
        Dim oMailTmp As COL_BusinessLogic_v2.COL_E_Mail = Me.oMail

        For Each oAddress In oMailTmp.IndirizziTO
            AddressList.Add(oAddress)
        Next
        For Each oAddress In oMailTmp.IndirizziCC
            AddressList.Add(oAddress)
        Next
        For Each oAddress In oMailTmp.IndirizziCCN
            AddressList.Add(oAddress)
        Next

        If AddressList.Count > 0 Then
            Me.NominativiShow(Nominativi.Show)
            Me.RPT_Address.DataSource = AddressList
            Me.RPT_Address.DataBind()
        Else
            Me.NominativiShow(Nominativi.ShowNoRecord)
        End If
    End Sub
#End Region

    Private Sub Cbx_ShowNominativi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbx_ShowNominativi.CheckedChanged
        'SalvaIdListe()
        'Me.oPresenter.BindListe()

        If Me.Cbx_ShowNominativi.Checked Then
            Me.BindNominativi()
        Else
            Me.NominativiShow(Nominativi.HideAll)
        End If
    End Sub

    'Private Sub CBL_Liste_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBL_Liste.SelectedIndexChanged
    '    If Me.Cbx_ShowNominativi.Checked Then
    '        Me.BindNominativi()
    '    End If
    'End Sub

    Private Sub Btn_AggiornaDestinatari_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_AggiornaDestinatari.Click
        If Me.Cbx_ShowNominativi.Checked Then
            Me.BindNominativi()
        End If
    End Sub

    Private Enum Nominativi
        HideAll = 0
        ShowNoRecord = 1
        Show = 2
    End Enum

    Private Sub NominativiShow(ByVal Element As Nominativi)
        Select Case Element
            Case Nominativi.HideAll
                LBL_Nominativi.Visible = False
                Me.PNL_Destinatari.Visible = False
                Btn_AggiornaDestinatari.Visible = False
            Case Nominativi.Show
                LBL_Nominativi.Visible = True
                LBL_Nominativi.Text = "<br />" & Resource.getValue("Nominativi.Selezionati") & "<br />" & "<br />"
                Me.PNL_Destinatari.Visible = True
                Btn_AggiornaDestinatari.Visible = True
            Case Nominativi.ShowNoRecord
                LBL_Nominativi.Visible = True
                LBL_Nominativi.Text = "<br />" & Resource.getValue("Nominativi.NoSelezione") & "<br />" & "<br />"
                Me.PNL_Destinatari.Visible = False
                Btn_AggiornaDestinatari.Visible = True
        End Select
    End Sub

End Class