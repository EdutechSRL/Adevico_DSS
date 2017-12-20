Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Chat

Public Class ChatFileManager_Ext
    Inherits System.Web.UI.Page

	Private _PageUtility As OLDpageUtility
	Private ReadOnly Property PageUtility() As OLDpageUtility
		Get
			If IsNothing(_PageUtility) Then
				_PageUtility = New OLDpageUtility(Me.Context)
			End If
			PageUtility = _PageUtility
		End Get
	End Property

    Protected oResource As ResourceManager
    Dim oMessaggi As New WS_Chat.WS_ChatSoapClient

    Protected WithEvents BtnDownload As System.Web.UI.WebControls.Button
    Protected WithEvents LBxFile As System.Web.UI.WebControls.ListBox
    Protected WithEvents BtFileInfo As System.Web.UI.WebControls.Button
    Protected WithEvents Lab_FileDis_t As System.Web.UI.WebControls.Label
    Protected WithEvents BtRemove As System.Web.UI.WebControls.Button
    Protected WithEvents LblFileInfo1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblFileInfo2 As System.Web.UI.WebControls.Label
    Protected WithEvents Lab_FileDisp_t As System.Web.UI.WebControls.Label
    Protected WithEvents BtHelp As System.Web.UI.WebControls.Button


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    Protected WithEvents PostedFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents BtSend As System.Web.UI.WebControls.Button
    Protected WithEvents IBAggiorna As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblNumFile As System.Web.UI.WebControls.Label
    Protected WithEvents ImgOnOff As System.Web.UI.WebControls.Image
    Protected WithEvents Lab_State As System.Web.UI.WebControls.Label

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
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Not oMessaggi.IsUploadTime Then
            Me.ImgOnOff.ImageUrl = "./../images/ON.gif"
            Me.ImgOnOff.AlternateText = Me.oResource.getValue("UpOK")
            If Me.Lab_State.Text = Me.oResource.getValue("UpKO") Then
                Me.Lab_State.Text = ""
            End If
        Else
            Me.ImgOnOff.ImageUrl = "./../images/OFF.gif"
            Me.ImgOnOff.AlternateText = Me.oResource.getValue("UpKO")
            Me.Lab_State.Text = Me.oResource.getValue("UpKO")
        End If
        If Not Page.IsPostBack Then
            Me.AggiornaListaFile()
            Me.BtHelp.Attributes.Add("onclick=", "window.resizeTo(450,650);return true;")
        End If
    End Sub

    Private Sub AggiornaListaFile()
        Dim oDataset As New DataSet

        Try
            oDataset = oMessaggi.GetFileList(Session("objPersona").Id, Session("IDCOmunita"))
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
        Catch ex As Exception
            Me.LblNumFile.Text = 0
        End Try

    End Sub

    Private Sub SendFile(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtSend.Click

        'Riferimento al file appena inviato
        Me.Lab_State.Text = Me.oResource.getValue("UpInCorso")
        Me.Lab_State.DataBind()

        Dim file_sent As HttpPostedFile = PostedFile.PostedFile
        'HttpPostedFile file_sent = PostedFile.PostedFile;

        'Dimensione del file inviato
        Dim file_size As Integer = file_sent.ContentLength
        If file_size > 2097152 Then '2 Mb
            'Codice gestione errore: file troppo grande
            Me.Lab_State.Text = Me.oResource.getValue("ErrSize")
            Exit Sub
        End If
        Dim Str As String = file_sent.FileName

        'Vediamo se così funziona... Al max un LowCase o UPPERCASE e kissenefrega!!!
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

        Me.LblFileInfo1.Text = Me.oResource.getValue("Nota1")
        Me.LblFileInfo2.Text &= Me.oResource.getValue("Nota2")



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
            If Me.oMessaggi.IsFileExist(Str, Me.IDCom) Then   'Vedere se ne vale la pena o se farlo sempre, magari in maniera interna e 'sciao!!!
                Str = Str.Insert(1, Session("IDComunita").ToString & "/")
                Me.Lab_State.Text = Me.oResource.getValue("Nota3") & " " & Str & "<br>"
            End If
            'Creazione di un buffer di byte
            Dim dati(file_size) As Byte
            'byte[] dati=new byte[file_size];

            '//Lettura dal file e riempimento del buffer
            file_sent.InputStream.Read(dati, 0, file_size)
            Dim ErrorMsg As String = oMessaggi.UploadFile(dati, Str, Session("objPersona").Anagrafica, Session("objPersona").Id, "", Session("IdComunita"))
            'file_sent.InputStream.Read(dati, 0, file_size)
            If Not ErrorMsg = "" Then
                Select Case ErrorMsg
                    Case "FL1"
                        Me.Lab_State.Text = Me.oResource.getValue("ErrExist")
                    Case Else
                        Me.Lab_State.Text = Me.oResource.getValue("ErrNonPrev") & " " & ErrorMsg
                End Select
            Else
                Me.Lab_State.Text &= Me.oResource.getValue("ErrOK")
            End If
            'string filename = file_sent.FileName;
        End If
        'End If
        Me.AggiornaListaFile()
    End Sub

    Private Sub IBAggiorna_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBAggiorna.Click
        Me.AggiornaListaFile()
    End Sub

    Private Sub BtnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDownload.Click
        Me.Lab_State.Text = ""
        'Me.Lab_State.Text = Me.oMessaggi.Url
        Dim WbsTmpDir As String = oMessaggi.GetTmpDir(Session("objPersona").Id, Session("IdComunita")) 'Sostituirlo con una funzione WBS che restituisce direttamente il LINK!!!
        If WbsTmpDir = "Non autorizzato" Then
            Me.Lab_State.Text = Me.oResource.getValue("ErrNoAut")
            Exit Sub
        End If

		Dim WbsUrlStr As String = Me.PageUtility.SystemSettings.ChatService.DefaultUrl	'oMessaggi.Url


        'chr(42) = / chr(92) = \
        Dim pos As Integer

        If InStr(WbsUrlStr, Chr(47)) > 0 Then
            pos = InStrRev(WbsUrlStr, Chr(47))
            WbsUrlStr = Left(WbsUrlStr, pos - 1)
        ElseIf InStr(WbsUrlStr, Chr(92)) > 0 Then
            pos = InStrRev(WbsUrlStr, Chr(92))
            WbsUrlStr = Left(WbsUrlStr, pos - 1)
        End If

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
                        nomeFile = WbsUrlStr & WbsTmpDir & Session("IdComunita") & Me.LBxFile.Items(i).Value
                        nomeFile = nomeFile.Replace("\", "/")

                        nomeFile = Me.Server.UrlPathEncode(nomeFile)

                        Me.LblFileInfo2.Text &= "<br> <a href=" & """" & nomeFile & """" & " target=_blank>" & Me.LBxFile.Items(i).Value & "</a>"
                        'WebClient.DownloadFile(WbsUrlStr & WbsTmpDir & Me.LBxFile.Items(i).Value, "C:\temp" & Me.LBxFile.Items(i).Value)
                        'Response.Redirect(WbsUrlStr & Me.LBxFile.Items(i).Value)
                        'Me.Lab_State.Text &= "File scaricato: " & Me.LBxFile.Items(i).Value & "<br>"
                        Me.Lab_State.Text = Me.oResource.getValue("Nota4")
                    Catch Ex As Exception
                        Me.Lab_State.Text &= Me.oResource.getValue("ErrNonPrev") & " " & Ex.ToString
                    End Try
                End If
            Next
        End If
    End Sub

    Private Sub BtFileInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtFileInfo.Click
        Me.LblFileInfo1.Text = Me.oResource.getValue("Nota4") & "<br><br>"
        Dim DsFile As New DataSet
        DsFile = oMessaggi.GetFileList(Session("objPersona").Id, Session("IDCOmunita"))
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

                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oResource.getValue("FileInfoNome") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Nome")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oResource.getValue("FileInfoDim") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("ByteSize") & " " & Me.oResource.getValue("FileInfoDimSize")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oResource.getValue("FileInfoOraUpl") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Tempo")
                Me.LblFileInfo2.Text &= "<br> <b>" & Me.oResource.getValue("FileInfoUploader") & "</b> "
                Me.LblFileInfo2.Text &= DsFile.Tables(0).Rows(i).Item("Uploder")
                'Me.LblFileInfo.Text &= "<br> <b>Note:</b> "
                'Me.LblFileInfo.Text &= DsFile.Tables(0).Rows(i).Item("Note")
                Me.LblFileInfo2.Text &= "<br>"
            End If
        Next

    End Sub

    Private Sub BtRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtRemove.Click
        Dim ErrStr As String = "File eliminato con successo!"
        For i As Integer = 0 To Me.LBxFile.Items.Count - 1
            If Me.LBxFile.Items(i).Selected Then
                ErrStr = oMessaggi.RemFile(Me.LBxFile.Items(i).Value, Session("objPersona").Id, Me.IDCom)
            End If
        Next
        Me.Lab_State.Text = ErrStr
        Me.AggiornaListaFile()
    End Sub

    Private Sub BtHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtHelp.Click
        Response.Redirect("ChatFileHelp_IT.htm")
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
        oResource.ResourcesName = "pg_ChatFileManager_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Function
    Public Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.Lab_FileDisp_t)
            .setButton(Me.BtFileInfo, False, False, False, False)
            .setButton(Me.BtnDownload, False, False, False, False)
            .setButton(Me.BtRemove, False, False, False, False)
            .setButton(Me.BtHelp, False, False, False, False)
            .setButton(Me.BtSend, False, False, False, False)

            .setLabel(Me.LblFileInfo1)
            .setLabel(Me.LblFileInfo2)

            .setImageButton(Me.IBAggiorna, True, True, True, False)

        End With
    End Sub
#End Region

    Public Property IDCom() As Integer
        Get
            Return CInt(ViewState("IDCom"))
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDCom") = Value
        End Set
    End Property

End Class
