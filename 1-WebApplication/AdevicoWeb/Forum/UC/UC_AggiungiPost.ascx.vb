Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2


Public Class UC_AggiungiPost
    Inherits System.Web.UI.UserControl

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
    Private oResourceAggiungi As ResourceManager


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

    Public Enum Action
        Modifica = 0
        Inserisci = 1
    End Enum

#Region "Pannello Inserisci Post"

    Protected WithEvents TBLrevisione As HtmlControl
    Protected WithEvents TBRevidenzia As HtmlControl

    Protected WithEvents LBmessaggioReply As System.Web.UI.WebControls.Label
    'Protected WithEvents TBLmessage As System.Web.UI.WebControls.Table
    Protected WithEvents LBlegenda As System.Web.UI.WebControls.Label
    Protected WithEvents LBoggetto_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBsubject As System.Web.UI.WebControls.TextBox
    'Protected WithEvents RFVsubject As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LBtesto_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBevidenzia_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLevidenzia As System.Web.UI.WebControls.RadioButtonList

    'Protected WithEvents TXAbody As System.Web.UI.HtmlControls.HtmlTextArea

    Protected WithEvents CTRLeditor As Global.Comunita_OnLine.UC_Editor
#End Region

#Region "Campi nascosti"
    Protected WithEvents HDNpost_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpost_ParentID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpostFRUM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpostThread_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpost_visibile As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpost_Level As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNpost_PRSN_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region


    Public Property Post_ID() As Integer
        Get
            If Me.HDNpost_ID.Value = "" Then
                Me.HDNpost_ID.Value = 0
            End If
            Post_ID = Me.HDNpost_ID.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNpost_ID.Value = Value
        End Set
    End Property
    Public Property POST_ParentId() As Integer
        Get
            If Me.HDNpost_ParentID.Value = "" Then
                Me.HDNpost_ParentID.Value = 0
            End If
            POST_ParentId = Me.HDNpost_ParentID.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNpost_ParentID.Value = Value
            If Value = 0 Then
                Me.TBLrevisione.Visible = False
            Else
                Me.TBLrevisione.Visible = True
            End If
        End Set
    End Property

    Public Property Azione() As Action
        Get
            Azione = Me.HDNazione.Value
        End Get
        Set(ByVal Value As Action)
            Me.HDNazione.Value = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceAggiungi) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.Page.IsPostBack = False Then
            Dim oRuoloForum As New Main.RuoloForumStandard
            Me.SetupInternazionalizzazione()

            oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
            Me.RBLevidenzia.SelectedValue = 1
            If oRuoloForum = Main.RuoloForumStandard.Partecipante Then
                Me.TBRevidenzia.Visible = False
            Else
                Me.TBRevidenzia.Visible = True
            End If
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceAggiungi = New ResourceManager

        oResourceAggiungi.UserLanguages = Code
        oResourceAggiungi.ResourcesName = "pg_UC_AggiungiPost"
        oResourceAggiungi.Folder_Level1 = "Forum"
        oResourceAggiungi.Folder_Level2 = "UC"
        oResourceAggiungi.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceAggiungi
            .setLabel(LBlegenda)
            .setLabel(LBoggetto_t)
            .setLabel(LBtesto_t)
            .setLabel(LBevidenzia_t)
            .setLabel(Me.LBmessaggioReply)
            .setRadioButtonList(RBLevidenzia, 1)
            .setRadioButtonList(RBLevidenzia, 2)
            .setRadioButtonList(RBLevidenzia, 3)
            .setRadioButtonList(RBLevidenzia, 5)

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

#Region "Bind_Dati"
    Public Function Bind_MessageForReply(ByVal newMessage As Boolean, ByVal ReplyWithQuote As Boolean, ByVal Post_Id As Integer) As Boolean
        Dim oRuoloForum As Main.RuoloForumStandard
        Dim oForumPost As New COL_Forum_posts

        Try
            oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
        Catch ex As Exception
            oRuoloForum = Main.RuoloForumStandard.Ospite
        End Try
        If IsNothing(oResourceAggiungi) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.RBLevidenzia.SelectedIndex = 0

        Try
            Dim oDataset As New DataSet
            Me.HDNpost_ID.Value = Post_Id

            oForumPost.Id = Me.HDNpost_ID.Value
            oDataset = oForumPost.EstraiEsteso(False)
            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(0)

                Me.RBLevidenzia.SelectedValue = oRow.Item("FRIM_Id")
                Me.HDNpostFRUM_ID.Value = oRow.Item("POST_FRUM_Id")
                Me.HDNpostThread_ID.Value = oRow.Item("POST_THRD_ID")
                Me.HDNpost_Level.Value = oRow.Item("POST_PostLevel")
                Me.HDNpost_visibile.Value = oRow.Item("POST_Approved")
                Me.HDNpost_PRSN_ID.Value = oRow.Item("POST_PRSN_ID")



                If IsDBNull(oRow.Item("POST_Subject")) Then
                    oRow.Item("POST_Subject") = "--"
                End If
                If IsDBNull(oRow.Item("POST_Body")) Then
                    oRow.Item("POST_Body") = "--"
                Else
                    oRow.Item("POST_Body") = Replace(oRow.Item("POST_Body"), "<br>", vbCrLf)
                End If

                oRow.Item("POST_Body") = Replace(oRow.Item("POST_Body"), "#%_234_%#", "")

                Me.TBLrevisione.Visible = True
				Me.TXBsubject.Text = GenericValidator.ValString(oRow.Item("POST_Subject"), "")
                If newMessage Then
                    Me.TBLrevisione.Visible = False

                    Me.TXBsubject.Text = ""
                    'Me.TXAbody.Value = ""
                    Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
                    Me.CTRLeditor.HTML = ""
				ElseIf ReplyWithQuote Then
					If Not Me.TXBsubject.Text.Contains("Re:") Then
						Me.TXBsubject.Text = "Re: " & Me.TXBsubject.Text
					End If

                    'Me.TXAbody.Value = vbCrLf & "[quote] " & oRow.Item("POST_Body") & " [/quote]" & vbCrLf
                    Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
                    Me.CTRLeditor.HTML = vbCrLf & "[quote] " & oRow.Item("POST_Body") & " [/quote]" & vbCrLf
				Else
					If Not Me.TXBsubject.Text.Contains("Re:") Then
						Me.TXBsubject.Text = "Re: " & Me.TXBsubject.Text
					End If

                    'Me.TXAbody.Value = ""
                    Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
                    Me.CTRLeditor.HTML = ""
				End If
				Me.oResourceAggiungi.setLabel_To_Value(Me.LBlegenda, "LBlegenda." & Me.Action.Inserisci)
				Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Bind_MessageForModifica(ByVal Post_Id As Integer) As Boolean
        Dim oRuoloForum As Main.RuoloForumStandard
        Dim oForumPost As New COL_Forum_posts
        Try
            oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)
        Catch ex As Exception
            oRuoloForum = Main.RuoloForumStandard.Ospite
        End Try

        Me.RBLevidenzia.SelectedIndex = 0

        If IsNothing(oResourceAggiungi) Then
            SetCulture(Session("LinguaCode"))
        End If

        Me.oResourceAggiungi.setLabel_To_Value(Me.LBlegenda, "LBlegenda." & Me.Action.Modifica)


        Try
            Dim oDataset As New DataSet

            Me.HDNpost_ID.Value = Post_Id
            oForumPost.Id = Post_Id
            oDataset = oForumPost.EstraiEsteso(False)

            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(0)

                Me.RBLevidenzia.SelectedValue = oRow.Item("FRIM_Id")
                Me.TXBsubject.Text = oRow.Item("POST_Subject")
                'Me.TXAbody.Value = oRow.Item("POST_Body")
                Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
                Me.CTRLeditor.HTML = oRow.Item("POST_Body")


                Me.HDNpostFRUM_ID.Value = oRow.Item("POST_FRUM_Id")
                Me.HDNpostThread_ID.Value = oRow.Item("POST_THRD_ID")
                Me.HDNpost_Level.Value = oRow.Item("POST_PostLevel")
                Me.HDNpost_visibile.Value = oRow.Item("POST_Approved")
                Me.HDNpost_PRSN_ID.Value = oRow.Item("POST_PRSN_ID")

                If oRow.Item("POST_ParentID") = 0 Then
                    Me.TBLrevisione.Visible = False
                Else
                    Me.TBLrevisione.Visible = True
                End If
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
#End Region


    Public Function Rispondi() As Boolean
        Dim oPersona As New COL_Persona
        Try

            Dim oRuoloForum As New Main.RuoloForumStandard
            oRuoloForum = CType(Session("RuoloForum"), Main.RuoloForumStandard)

            Dim oForumPost As New COL_Forum_posts
            Dim oForum As New COL_Forums
            Dim ForumID As Integer = Session("IdForum")
            Dim RLPC_ID As Integer = Session("RuoloForum")

            Dim BodyPost As String
            'BodyPost = Server.HtmlDecode(Me.TXAbody.Value)
            BodyPost = Me.CTRLeditor.HTML

            oPersona = Session("objPersona")
            If Azione = Action.Modifica Then
                Dim oDataset As New DataSet
                oForumPost.Id = Me.HDNpost_ID.Value

                oDataset = oForumPost.EstraiEsteso(False)
                If oDataset.Tables(0).Rows.Count > 0 Then
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(0)

                    oForumPost.FRUM_id = oRow.Item("POST_FRUM_Id")
                    oForumPost.ParentID = oRow.Item("POST_ParentId")
                    oForumPost.ThreadID = oRow.Item("POST_THRD_ID")
                    oForumPost.PostLevel = oRow.Item("POST_PostLevel")
                    oForumPost.Approved = oRow.Item("POST_Approved")
                    oForumPost.PRSN_Id = oRow.Item("POST_PRSN_ID")

                    oForumPost.Subject = Me.TXBsubject.Text
                    oForumPost.Body = BodyPost
                    oForumPost.PostImageID = Me.RBLevidenzia.SelectedValue
                    oForumPost.Modifica()
                    Return True
                Else
                    Return False
                End If
            ElseIf Azione = Action.Inserisci Then
                Dim TopicID As Integer = CInt(Me.HDNpostThread_ID.Value)
                oForum.Id = ForumID
                oForum.Estrai()

                oForumPost.Body = BodyPost
                oForumPost.ParentID = Me.HDNpost_ID.Value
                oForumPost.PostLevel = Me.HDNpost_Level.Value + 1
                oForumPost.Subject = Me.TXBsubject.Text
                oForumPost.ThreadID = Me.HDNpostThread_ID.Value
                oForumPost.PRSN_Id = oPersona.ID
                oForumPost.FRUM_id = ForumID
                oForumPost.PostImageID = Me.RBLevidenzia.SelectedValue
                If oForum.Moderated And (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                    oForumPost.Approved = Main.PostApprovazione.InAttesa
                Else
                    oForumPost.Approved = Main.PostApprovazione.Approvato
                End If
                oForumPost.Aggiungi(RLPC_ID)

                If oForumPost.Errore = Errori_Db.None Then
                    Dim oResourceConfig As New ResourceManager
                    oResourceConfig = GetResourceConfig(Session("LinguaCode"))

                    Dim oUtility As New OLDpageUtility(Me.Context)

                    Dim UrlBegin As String = oUtility.ApplicationUrlBase
                    If Not UrlBegin.EndsWith("/") Then
                        UrlBegin = UrlBegin & "/"
                    End If


                    Dim UrlBase As String = _
                        String.Format("{0}Forum/LoadForum.aspx?CommunityID={1}", _
                                                          UrlBegin, _
                                                          PageUtility.WorkingCommunityID.ToString)

                    'oUtility.ApplicationUrlBase & "Forum/LoadForum.aspx?CommunityID=" & PageUtility.WorkingCommunityID.ToString  ' oUtility.ApplicationUrlBase &  oUtility.SystemSettings.Presenter.DefaultForumLogin

                    Dim UrlBaseNotifica As String = UrlBegin & oUtility.SystemSettings.Presenter.DefaultRemoveServiceNotification


                    Dim oServiceNotification As New ForumNotificationUtility(Me.PageUtility)
                    If oForum.Moderated And (oRuoloForum = Main.RuoloForumStandard.Partecipante Or oRuoloForum = Main.RuoloForumStandard.Ospite) Then
                        oForumPost.Approved = Main.PostApprovazione.InAttesa

                        oForumPost.NotificaViaMail(UrlBaseNotifica, UrlBase, oPersona, oUtility.LocalizedMail, Main.RuoloForumStandard.Amministratore)
                        oForumPost.NotificaViaMail(UrlBaseNotifica, UrlBase, oPersona, oUtility.LocalizedMail, Main.RuoloForumStandard.Moderatore)


                        oServiceNotification.NotifyPostToModerate(Me.PageUtility.WorkingCommunityID, ForumID, oForum.Name, TopicID, COL_Forum_threads.EstraiSubject(TopicID), oForumPost.Id, oForumPost.Subject, Me.PageUtility.CurrentUser.ID)
                    Else
                        oForumPost.Approved = Main.PostApprovazione.Approvato
                        'se n c'è errore mando le mail a coloro che lo hanno richiesto
                        oForumPost.NotificaViaMail(UrlBaseNotifica, UrlBase, oPersona, oUtility.LocalizedMail)


                        oServiceNotification.NotifyAddPost(Me.PageUtility.WorkingCommunityID, ForumID, oForum.Name, TopicID, COL_Forum_threads.EstraiSubject(TopicID), oForumPost.Id, oForumPost.Subject)
                    End If
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    'Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
    '    Me.CTRLeditor.InitializeControl(COL_BusinessLogic_v2.UCServices.Services_Forum.Codex)
    'End Sub
End Class