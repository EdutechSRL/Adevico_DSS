Imports COL_BusinessLogic_v2

Imports lm.Comol.Modules.Chat
Public Class ChatBuffer_Ext
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
#Region "Oggetti"
    Dim messaggi As New WS_Chat.WS_ChatSoapClient
    Protected oResource As ResourceManager
#End Region
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DGMessaggi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DGUtenti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents LbWBSUrl As System.Web.UI.WebControls.Label

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

        If Session("objPersona") Is Nothing Or Session("IDComunita") Is Nothing Then
            'Response.Redirect("./../login.aspx")
            Exit Sub
        End If

        If messaggi.GetLvl(Session("objPersona").Id, Session("IdComunita")) < 4 Then
            'Response.Redirect("./../login.aspx")
            Exit Sub
        End If
        DGMessaggi.DataSource = messaggi.RecuperaTutti(Session("objPersona").Id, Session("IdComunita"))
        DGUtenti.DataSource = messaggi.RecuperaListaUtenti(Session("objPersona").Id, Session("IdComunita"))
        DGMessaggi.DataBind()
        DGUtenti.DataBind()

        'Me.Label1.Text = "ID della comunità corrente: " & Session("IdComunita").ToString
        Me.Label1.Text = Me.oResource.getValue("Id_Com_t") & " " & Session("IdComunita").ToString
        Me.Label1.DataBind()
		Me.LbWBSUrl.Text = Me.oResource.getValue("Url_t") & " " & Me.PageUtility.SystemSettings.ChatService.DefaultUrl	'Me.messaggi.

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
        oResource.ResourcesName = "pg_ChatBuffer_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()


    End Function

    Public Sub SetupInternazionalizzazione()
        With oResource
            'Label dinamiche!
        End With
    End Sub
#End Region
End Class
