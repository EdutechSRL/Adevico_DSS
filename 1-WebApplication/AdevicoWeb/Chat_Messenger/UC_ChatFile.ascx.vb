Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Chat

Public Class UC_ChatFile
    Inherits System.Web.UI.UserControl
    'Inherits PagerBaseControl

    Protected oLocate As ResourceManager

    Dim oMessaggi As New WS_Chat.WS_ChatSoapClient
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumFile As System.Web.UI.WebControls.Label

    Protected WithEvents BtnDownload As System.Web.UI.WebControls.Button
    Protected WithEvents BtRemove As System.Web.UI.WebControls.Button
    'Protected WithEvents BtHelp As System.Web.UI.WebControls.Button
    Protected WithEvents HL_Help As System.Web.UI.WebControls.HyperLink

    Protected WithEvents BtSend As System.Web.UI.WebControls.Button

    Protected WithEvents BtFileInfo As System.Web.UI.WebControls.Button
    Protected WithEvents LBxFile As System.Web.UI.WebControls.ListBox
    Protected WithEvents LblFileInfo1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblFileInfo2 As System.Web.UI.WebControls.Label
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    Protected WithEvents IBAggiorna As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ImgOnOff As System.Web.UI.WebControls.Image
    Protected WithEvents InputFileName As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Lab_State As System.Web.UI.WebControls.Label

    Public Property IDCom() As Integer
        Get
            Return CInt(ViewState("IDCom"))
            'Return IdComunita
        End Get
        Set(ByVal Value As Integer)
            'IdComunita = Value
            ViewState("IDCom") = Value
        End Set
    End Property
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
        If Not Page.IsPostBack Then ' = False
            Me.setupLingua()
        End If
        If IsNothing(oLocate) Then
            SetCulture(Session("LinguaCode"), "page_UC_ChatFile")
        End If

        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            If Not oMessaggi.IsUploadTimeCom(Me.IDCom) Then
                'If Not oMessaggi.IsUploadTime() Then
                Me.ImgOnOff.ImageUrl = "./../images/ON.gif"
                Me.ImgOnOff.AlternateText = Me.oLocate.getValue("UplOK") ' "Upload disponibile"
                If Me.Lab_State.Text = Me.oLocate.getValue("UplKO") Then '"Upload NON disponibile. Riprovare più tardi" Then
                    Me.Lab_State.Text = ""
                End If
            Else
                Me.ImgOnOff.ImageUrl = "./../images/OFF.gif"
                Me.ImgOnOff.AlternateText = Me.oLocate.getValue("UplKO")
                Me.Lab_State.Text = Me.oLocate.getValue("UplKO")
            End If
        Catch ex As Exception
            Exit Sub 'se ci sono errori col wbs: quando l'utente non e' loggato... :p
        End Try


        If Not Page.IsPostBack Then
            Me.AggiornaListaFile()
            'Me.BtHelp.Attributes.Add("onclick=", "window.resizeTo(450,650);return true;")
        End If

    End Sub

    Private Sub AggiornaListaFile()
        Dim oDataset As New DataSet

        Try
            oDataset = oMessaggi.GetFileList(Session("objPersona").Id, Me.IDCom)
            'For Each Row As DataRow In DsFile.Tables(0).Rows
            With Me.LBxFile
                .DataSource = oDataset.Tables(0)
                .DataTextField = "Nome"
                .DataValueField = "Nome"
                .SelectionMode = ListSelectionMode.Multiple
                .DataBind()
            End With
            'Next
            Me.LblNumFile.Text = LBxFile.Items.Count.ToString
            'Me.LBxFile.DataBind()
            If Me.LBxFile.Items.Count = 0 Then
                Me.LBxFile.Visible = False
            Else
                Me.LBxFile.Visible = True
            End If
        Catch ex As Exception
            Me.LblNumFile.Text = 0
            Me.LBxFile.Visible = False
        End Try

    End Sub

    Private Sub SendFile(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtSend.Click
        If IsNothing(oLocate) Then
            SetCulture(Session("LinguaCode"), "page_UC_ChatFile")
        End If
        'Riferimento al file appena inviato
        Me.Lab_State.Text = Me.oLocate.getValue("UplInCorso")

        Dim file_sent As HttpPostedFile = InputFileName.PostedFile
        'HttpPostedFile file_sent = PostedFile.PostedFile;

        'Dimensione del file inviato
        Dim file_size As Integer = file_sent.ContentLength
        If file_size > 2097152 Then '2 Mb
            'Codice gestione errore: file troppo grande
            Me.Lab_State.Text = Me.oLocate.getValue("ErrSize")
            Exit Sub
        End If
        Dim Str As String = file_sent.FileName

        'Vediamo se così funziona... Al max un LowCase o UPPERCASE e via...
        Str = Str.Replace(".exe", ".ex_")
        Str = Str.Replace(".Exe", ".ex_")
        Str = Str.Replace(".eXe", ".ex_")
        Str = Str.Replace(".exE", ".ex_")
        Str = Str.Replace(".EXe", ".EX_")
        Str = Str.Replace(".ExE", ".EX_")
        Str = Str.Replace(".eXE", ".EX_")
        Str = Str.Replace(".EXE", ".EX_")

        Str = Str.Replace(".com", ".co_")
        Str = Str.Replace(".Com", ".co_")
        Str = Str.Replace(".cOm", ".co_")
        Str = Str.Replace(".coM", ".co_")
        Str = Str.Replace(".CoM", ".CO_")
        Str = Str.Replace(".cOM", ".CO_")
        Str = Str.Replace(".COm", ".CO_")
        Str = Str.Replace(".COM", ".CO_")

        Me.LblFileInfo1.Text = Me.oLocate.getValue("Nota1")
        Me.LblFileInfo2.Text = Me.oLocate.getValue("Nota2")
        'Str.Remove(0, Str.LastIndexOf("\"))
        'Str.Remove(0, Str.LastIndexOf("/"))

        For i As Integer = Str.Length - 1 To 0 Step -1
            If Str.Chars(i) = Chr(47) Or Str.Chars(i) = Chr(92) Then 'chr(42) = / chr(92) = \
                Str = Str.Remove(0, i)
                Exit For
            End If
        Next

        'Vari controlli (dimensione maggiore di zero e diverso da null)
        'If Not file_sent =  Then 'RIVEDERE!!!

        If file_size > 0 Then
            If Me.oMessaggi.IsFileExist(Str, Me.IDCom) Then  'Vedere se ne vale la pena o se farlo sempre, magari in maniera interna e 'sciao!!!
                Str = Str.Insert(1, Me.IDCom.ToString & "/")
                Me.Lab_State.Text = Me.oLocate.getValue("NotaFileMod") & " " & Str & "<br>"
            End If
            'Creazione di un buffer di byte
            Dim dati(file_size) As Byte
            'byte[] dati=new byte[file_size];

            '//Lettura dal file e riempimento del buffer
            Try

                file_sent.InputStream.Read(dati, 0, file_size)
                Dim ErrorMsg As String = oMessaggi.UploadFile(dati, Str, Session("objPersona").Anagrafica, Session("objPersona").Id, "", Me.IDCom)
                'file_sent.InputStream.Read(dati, 0, file_size)
                If Not ErrorMsg = "" Then
                    Select Case ErrorMsg
                        Case "FL1"
                            Me.Lab_State.Text = Me.oLocate.getValue("ErrFileExist")
                        Case Else
                            Me.Lab_State.Text = Me.oLocate.getValue("ErrGenerico") & " " & ErrorMsg
                    End Select
                Else
                    Me.Lab_State.Text &= Me.oLocate.getValue("ErrNoError")
                End If

            Catch ex As Exception
                Me.Lab_State.Text = Me.oLocate.getValue("ErrGenerico")
            End Try

        End If
        Me.AggiornaListaFile()
    End Sub

    Private Sub IBAggiorna_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBAggiorna.Click
        Me.AggiornaListaFile()
        Try
            Me.SetupInternazionalizzazione()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDownload.Click
        Me.Lab_State.Text = ""
        Dim WbsTmpDir As String = oMessaggi.GetTmpDir(Session("objPersona").Id, Me.IDCom)
        If WbsTmpDir = "Non autorizzato" Then
            Me.Lab_State.Text = Me.oLocate.getValue("ErrNoAut")
            Exit Sub
        Else
			WbsTmpDir = Me.IDCom & "\"
            'Sì, è una porcata, ma poi restituirà direttamente TUTTO il percorso completo...
        End If

        Dim WbsUrlStr As String
        WbsUrlStr = SystemSettings.ChatService.DefaultFileUrl
        ' oMessaggi.GetFileBaseUrl 'GetPercorsoApplicazione(Me.Request) & "/wbs_chat"

        'chr(42) = / chr(92) = \
        Dim pos As Integer

        'If InStr(WbsUrlStr, Chr(47)) > 0 Then
        '    pos = InStrRev(WbsUrlStr, Chr(47))
        '    WbsUrlStr = Left(WbsUrlStr, pos - 1)
        'ElseIf InStr(WbsUrlStr, Chr(92)) > 0 Then
        '    pos = InStrRev(WbsUrlStr, Chr(92))
        '    WbsUrlStr = Left(WbsUrlStr, pos - 1)
        'End If

        If Me.LBxFile.Items.Count > 0 Then
            Me.LblFileInfo1.Text = ""
            Me.LblFileInfo2.Text = ""
            For i As Integer = 0 To Me.LBxFile.Items.Count - 1
                If Me.LBxFile.Items(i).Selected Then
                    Dim WebClient As New System.Net.WebClient
                    'Codice per aprire la finestra ed iniziare il download
                    'Me.Lab_State.Text &= "|" & WbsUrlStr & WbsUrlStr & Me.LBxFile.Items(i).Value
                    Try
                        Dim nomeFile As String
                        nomeFile = WbsUrlStr & WbsTmpDir & Me.LBxFile.Items(i).Value '
                        nomeFile = nomeFile.Replace("\", "/")

                        nomeFile = Me.Server.UrlPathEncode(nomeFile)

                        Me.LblFileInfo2.Text &= "<br> <a href=" & """" & nomeFile & """" & " target=_blank>" & Me.LBxFile.Items(i).Value & "</a>"
                        'WebClient.DownloadFile(WbsUrlStr & WbsTmpDir & Me.LBxFile.Items(i).Value, "C:\temp" & Me.LBxFile.Items(i).Value)
                        'Response.Redirect(WbsUrlStr & Me.LBxFile.Items(i).Value)
                        'Me.Lab_State.Text &= "File scaricato: " & Me.LBxFile.Items(i).Value & "<br>"
                        'Me.Lab_State.Text = "Clicca sotto per scaricare"
                        Me.Lab_State.Text = oLocate.getValue("Download")
                    Catch Ex As Exception
                        Me.Lab_State.Text &= oLocate.getValue("ErrGenerico") '"Errore interno" '& Ex.ToString
                    End Try
                End If
            Next
        End If
    End Sub

    Private Sub BtFileInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtFileInfo.Click

        Me.SetLabelError("InfoFile")
        Dim DsFile As New DataSet
        DsFile = oMessaggi.GetFileList(Session("objPersona").Id, Me.IDCom)

        If Me.LBxFile.Items.Count > 0 Then
            Me.LblFileInfo2.Text = ""
        End If

        For i As Integer = 0 To Me.LBxFile.Items.Count - 1
            If Me.LBxFile.Items(i).Selected Then
                'For Each row As DataRow In DsFile.Tables(0).Rows
                '    If row.Item("Nome") = Me.LBxFile.Items(i).Value Then
                '        Me.LblFileInfo.Text &= "<br> <b>Nome:</b> "
                '        Me.LblFileInfo.Text &= row.Item("Nome")
                '        Me.LblFileInfo.Text &= "<br> <b>Dimensione:</b> "
                '        Me.LblFileInfo.Text &= row.Item("ByteSize") & " Byte"
                '        Me.LblFileInfo.Text &= "<br> <b>Ora Upload:</b> "
                '        Me.LblFileInfo.Text &= row.Item("Tempo")
                '        Me.LblFileInfo.Text &= "<br> <b>Uploder:</b> "
                '        Me.LblFileInfo.Text &= row.Item("Uploder")
                '        Me.LblFileInfo.Text &= "<br> <b>Note:</b> "
                '        Me.LblFileInfo.Text &= row.Item("Note")
                '    End If
                'Next
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oLocate.getValue("InfoNome") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Nome")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oLocate.getValue("InfoDim") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("ByteSize") & Me.oLocate.getValue("InfoDimByte")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oLocate.getValue("InfoTempo") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Tempo")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oLocate.getValue("InfoUploader") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Uploder")
                'Me.LblFileInfo.Text &= "<br> <b>" & Me.oLocate.getValue("InfoNote") & "</b> "
                'Me.LblFileInfo.Text &= DsFile.Tables(0).Rows(i).Item("Note")
                Me.LblFileInfo2.Text &= "<br>"
            End If
        Next

    End Sub

    Private Sub BtRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtRemove.Click
        Dim ErrStr As String = Me.oLocate.getValue("ErrElimOk")
        For i As Integer = 0 To Me.LBxFile.Items.Count - 1
            If Me.LBxFile.Items(i).Selected Then
                ErrStr = oMessaggi.RemFile(Me.LBxFile.Items(i).Value, Session("objPersona").Id, Me.IDCom)
            End If
        Next
        Me.Lab_State.Text = ErrStr
        Me.AggiornaListaFile()

    End Sub


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
                    Session("LinguaID") = oLingua.ID
                    Session("LinguaCode") = oLingua.Codice
                Else
                    Session("LinguaID") = 1
                    Session("LinguaCode") = "it-IT"
                End If
            End If

            SetCulture(Session("LinguaCode"), "page_UC_ChatFile")

            Me.SetupInternazionalizzazione()
        Catch exUserLanguages As Exception
        End Try
    End Sub
    Public Function SetCulture(ByVal Code As String, ByVal ResourcesName As String)
        '' localizzazione utente
        'oLocate = New COL_Localizzazione
        'oLocate.UserLanguages = Code
        'oLocate.ProjectName = "Comunita_OnLine"
        'oLocate.ResourcesName = ResourcesName
        '' Nel getType mettere sempre il nome della classe relativa alla pagina corrente !!!!
        'oLocate.FileAssembly = GetType(UC_ChatFile).Assembly 'GestioneIscritti
        'oLocate.setCulture()
        oLocate = New ResourceManager
        oLocate.UserLanguages = Code
        oLocate.ResourcesName = "pg_UC_ChatFile"
        oLocate.Folder_Level1 = "Chat_Messenger"
        oLocate.Folder_Level2 = "UC"
        oLocate.setCulture()




        'oResource = New ResourceManager
        'oResource.UserLanguages = Code
        'oResource.ResourcesName = "pg_Chat_Ext"
        'oResource.Folder_Level1 = "Chat_Messenger"
        'oResource.setCulture()

    End Function

    Public Sub SetupInternazionalizzazione()
        With oLocate
            .setLabel(Me.Label1)
            .setButton(Me.BtFileInfo, False, False, False, False)
            .setButton(Me.BtnDownload, False, False, False, False)
            .setButton(Me.BtRemove, False, False, False, False)
            .setButton(Me.BtSend, False, False, False, False)
            .setHyperLink(Me.HL_Help, True, False, , )
            Me.SetLabelError("Responsab")
            .setImageButton(Me.IBAggiorna, True, True, True, False)

        End With
    End Sub

    Public Sub SetLabelError(ByVal Tipo As String)
        'Tipo:
        'Responsab -> nota sulla responsabilita' dei contenuti
        'InfoFile  -> Setta il titolo e svuota la descrizione

        Me.LblFileInfo1.Text = oLocate.getValue(Tipo & ".Titolo")
        Me.LblFileInfo2.Text = oLocate.getValue(Tipo & ".Desc")

    End Sub

#End Region

    Public ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
End Class

