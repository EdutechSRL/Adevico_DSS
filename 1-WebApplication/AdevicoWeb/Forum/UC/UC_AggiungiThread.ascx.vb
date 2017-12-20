Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.CL_persona


Public Class UC_AggiungiThread
    Inherits System.Web.UI.UserControl
    Private oResourceThread As ResourceManager

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
    'Protected WithEvents TBLmessage As System.Web.UI.WebControls.Table
    Protected WithEvents LBoggetto_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBsubject As System.Web.UI.WebControls.TextBox
    'Protected WithEvents RFVsubject As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LBtesto_t As System.Web.UI.WebControls.Label
    'Protected WithEvents TXAbody As System.Web.UI.HtmlControls.HtmlTextArea
    Protected WithEvents TBRevidenzia As HtmlControl
    Protected WithEvents LBevidenzia_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLevidenzia As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents CTRLeditor As Global.Comunita_OnLine.UC_Editor

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceThread) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Page.IsPostBack = False Then
            Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
            Me.SetupInternazionalizzazione()

            Dim oRuoloForum As New Main.RuoloForumStandard
            oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
            Me.RBLevidenzia.SelectedValue = 1
            If oRuoloForum = Main.RuoloForumStandard.Partecipante Then
                Me.TBRevidenzia.Visible = False
            Else
                Me.TBRevidenzia.Visible = True
            End If
        End If

    End Sub

    Public Function Inserisci() As Boolean
        Dim oPersona As New COL_Persona
        Dim oRuoloForum As New Main.RuoloForumStandard
        oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)

        Try
            If Session("Azione") = "insert" Then

                Dim oForum As New COL_Forums
                Dim oResourceConfig As New ResourceManager
                oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                ' Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request)
                '

                oForum.Id = Session("IdForum")
                oForum.Estrai()
				'oForum.AggiungiTopic(oRuoloForum, Session("objPersona").id, Me.TXBsubject.Text, Server.HtmlDecode(Me.TXAbody.Value), Me.RBLevidenzia.SelectedValue, Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request), oResourceConfig)

				Dim oUtility As New OLDpageUtility(Me.Context)
                Dim UrlBase As String = oUtility.ApplicationUrlBase & "Forum/LoadForum.aspx?CommunityID=" & PageUtility.WorkingCommunityID.ToString 'oUtility.ApplicationUrlBase & oUtility.SystemSettings.Presenter.DefaultForumLogin
				Dim UrlBaseNotifica As String = oUtility.ApplicationUrlBase & oUtility.SystemSettings.Presenter.DefaultRemoveServiceNotification

                'oForum.AggiungiTopic(oRuoloForum, Session("objPersona"), Me.TXBsubject.Text, Server.HtmlDecode(Me.TXAbody.Value), Me.RBLevidenzia.SelectedValue, UrlBase, UrlBaseNotifica, oUtility.LocalizedMail)
                Dim TopicID As Integer = Me.AggiungiTopic(oForum.Id, oRuoloForum, Session("objPersona"), Me.TXBsubject.Text, Me.CTRLeditor.HTML, Me.RBLevidenzia.SelectedValue, UrlBase, UrlBaseNotifica, oUtility.LocalizedMail)
                If TopicID > 0 Then
                    Cancellaform()
                End If
            End If
			Session("Azione") = "loaded"
		Catch ex As Exception
			Return False
		End Try
        Return True
        ' Me.PNLnewThread.Visible = False
        ' Me.PNLthread.Visible = True
    End Function
    Public Function Cancellaform()
        'Me.TXAbody.Value = ""
        Me.CTRLeditor.HTML = ""
        Me.TXBsubject.Text = ""
        Me.RBLevidenzia.SelectedIndex = 0
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceThread = New ResourceManager

        oResourceThread.UserLanguages = Code
        oResourceThread.ResourcesName = "pg_UC_AggiungiThread"
        oResourceThread.Folder_Level1 = "Forum"
        oResourceThread.Folder_Level2 = "UC"
        oResourceThread.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResourceThread
            .setLabel(Me.LBtesto_t)
            .setLabel(Me.LBoggetto_t)
            .setLabel(Me.LBevidenzia_t)

            .setRadioButtonList(Me.RBLevidenzia, 1)
            .setRadioButtonList(Me.RBLevidenzia, 2)
            .setRadioButtonList(Me.RBLevidenzia, 3)
            .setRadioButtonList(Me.RBLevidenzia, 5)

            Dim i As Integer
            Dim smile(22) As String

            smile(1) = ":-)"
            smile(2) = ":D"
            smile(3) = ":-O"
            smile(4) = ":-p"
            smile(5) = ";-)"
            smile(6) = "(H)"
            smile(7) = ":$"
            smile(8) = "|-)"
            smile(9) = ":("
            smile(10) = ";-("
            smile(11) = ":S"
            smile(12) = ":@"
            smile(13) = "(*)"
            smile(14) = "(L)"
            smile(15) = "(U)"
            smile(16) = "(Y)"
            smile(17) = "(N)"
            smile(18) = "(pp)"
            smile(19) = "8-|"
            smile(20) = "(6)"
            smile(21) = "(?)"

            For i = 1 To 21
                Dim oImagebutton As ImageButton
                oImagebutton = FindControl("IMG_" & i)
                If Not IsNothing(oImagebutton) Then
                    .setImageButton(oImagebutton, False, True, True)
                    oImagebutton.Attributes.Add("onClick", "faccine('" & smile(i) & "') ;return false;")
                End If
            Next
        End With




    End Sub
#End Region

    Public Function AggiungiTopic(ByVal ForumID As Integer, ByVal oRuoloForum As Main.RuoloForumStandard, ByVal oPersona As COL_Persona, ByVal Subject As String, ByVal TestoMessaggio As String, ByVal PostImageID As Integer, ByVal BaseUrl As String, ByVal RemoveNotifica As String, ByVal oLocalizedMail As MailLocalized) As Integer
        Dim oForum As New COL_Forums
        Dim TopicID As Integer = 0
        'Dim ForumID As Integer = Session("IdForum")
        'Dim RLPC_ID As Integer = Session("RuoloForum")
        Dim oThread As New COL_Forum_threads

        oForum.Id = ForumID
        oForum.Estrai()

        If oForum.Errore = Errori_Db.None Then
            oThread.PRSN_Id = oPersona.ID
            oThread.Subject = Subject
            oThread.ThreadImage = 1
            oThread.ThreadForum = ForumID

            If oForum.Moderated And (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                oThread.Hide = 1
            Else
                oThread.Hide = 0
            End If
            oThread.Aggiungi(ForumID)
            If oThread.Errore = Errori_Db.None Then
                TopicID = oThread.Id
                Dim oForumPost As New COL_Forum_posts

                If oForum.Moderated And (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                    oThread.NotificaViaMail(RemoveNotifica, BaseUrl, oPersona, oLocalizedMail, Main.RuoloForumStandard.Amministratore)
                    oThread.NotificaViaMail(RemoveNotifica, BaseUrl, oPersona, oLocalizedMail, Main.RuoloForumStandard.Moderatore)
                Else
                    oThread.NotificaViaMail(RemoveNotifica, BaseUrl, oPersona, oLocalizedMail)
                End If

                If oForum.Moderated And (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                    oThread.Nascondi()
                End If

                oForumPost.PostDate = oThread.PostDate
                oForumPost.Body = TestoMessaggio
                oForumPost.ParentID = 0
                oForumPost.PostLevel = 1
                oForumPost.Subject = Subject
                oForumPost.ThreadID = oThread.Id
                oForumPost.PRSN_Id = oPersona.ID
                oForumPost.FRUM_id = ForumID
                oForumPost.PostImageID = PostImageID
                If oForum.Moderated AndAlso (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                    oForumPost.Approved = Main.PostApprovazione.InAttesa
                Else
                    oForumPost.Approved = Main.PostApprovazione.Approvato
                End If


                oForumPost.Aggiungi(oRuoloForum)
                If oForumPost.Errore = Errori_Db.None Then
                    Dim oServiceNotification As New ForumNotificationUtility(Me.PageUtility)
                    If oForum.Moderated AndAlso (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                        oServiceNotification.NotifyPostToModerate(Me.PageUtility.WorkingCommunityID, oForum.Id, oForum.Name, TopicID, COL_Forum_threads.EstraiSubject(TopicID), oForumPost.Id, oForumPost.Subject, Me.PageUtility.CurrentUser.ID)
                    Else
                        oServiceNotification.NotifyAddTopic(Me.PageUtility.WorkingCommunityID, oForum.Id, oForum.Name, TopicID, Me.TXBsubject.Text)
                    End If
                Else
                    oThread.Elimina()
                End If
            End If
        End If
        Return TopicID
    End Function

    'Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
    '    Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
    'End Sub
End Class