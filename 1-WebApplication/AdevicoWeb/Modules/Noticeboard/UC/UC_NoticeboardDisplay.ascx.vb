Imports lm.Comol.Modules.Base.DomainModel

Partial Public Class UC_NoticeboardDisplay
    Inherits BaseControlSession
    Implements IViewMessagePreview

    Private _ObjectPath As ObjectFilePath
    Private _ContenitoreWidth As System.Web.UI.WebControls.Unit
    Private _BachecaWidth As System.Web.UI.WebControls.Unit
    Private _BachecaHeight As System.Web.UI.WebControls.Unit
    Private _isPreview As Boolean
    Private _ViewdInDashBoard As Boolean

    Public Property isViewedInDashBoard() As Boolean
        Get
            isViewedInDashBoard = _ViewdInDashBoard
        End Get
        Set(ByVal value As Boolean)
            _ViewdInDashBoard = value
        End Set
    End Property
    Public Property ContenitoreWidth() As System.Web.UI.WebControls.Unit Implements IViewMessagePreview.ContenitoreWidth
        Get
            ContenitoreWidth = _ContenitoreWidth
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            _ContenitoreWidth = value
        End Set
    End Property
    Public Property ContenitoreBachecaWidth() As System.Web.UI.WebControls.Unit Implements IViewMessagePreview.ContenitoreBachecaWidth
        Get
            ContenitoreBachecaWidth = _BachecaWidth
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            _BachecaWidth = value
        End Set
    End Property
    Public Property ContenitoreBachecaHeight() As System.Web.UI.WebControls.Unit Implements IViewMessagePreview.ContenitoreBachecaHeight
        Get
            ContenitoreBachecaHeight = _BachecaHeight
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            _BachecaHeight = value
        End Set
    End Property
    Public Property isPreview() As Boolean Implements IViewMessagePreview.isPreview
        Get
            isPreview = _isPreview
        End Get
        Set(ByVal value As Boolean)
            _isPreview = value
        End Set
    End Property
    Public ReadOnly Property BachecaHeight() As String Implements IViewMessagePreview.BachecaHeight
        Get
            Return "100%"
        End Get
    End Property

    Public ReadOnly Property BachecaWidth() As String Implements IViewMessagePreview.BachecaWidth
        Get
            Return "100%"
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityNoticeboard", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If
        With MyBase.Resource
            .setButton(BTNstampa, True)
            .setButton(BTNclear, True)
            .setButton(BTNdelete, True)
            .setButton(BTNvirtualDelete, True)
            .setButton(BTNvirtualUndelete, True)
            .setButton(BTNvirtualUndeleteAndActivate, True)
            .setButton(BTNsetActive, True)

            .setHyperLink(HYPeditHTML, True, True)
            .setHyperLink(HYPeditADV, True, True)
            .setLiteral(LTdashBoard)
            .setHyperLink(HYPmoreMessagesDS, True, True)
            .setHyperLink(HYPeditDS, True, True)
            .setHyperLink(HYPstampaDS, True, True)
        End With
        Me.HYPmoreMessagesDS.NavigateUrl = Me.BaseUrl & "Generici/CommunityNoticeBoard.aspx?View=CurrentMessage&SmallView=LastFourMessage&PreviousView=DashBoard"
    End Sub

    Private Function BuildNoticeboard(ByVal oNoticeBoard As NoticeBoard) As String
        Dim oVisualizzato As String = Me.iBackgroundTemplate
        Dim oBackColor As String = ""

        If oNoticeBoard.CreateByAdvancedEditor = False AndAlso Not IsNothing(oNoticeBoard.Style) AndAlso oNoticeBoard.Style.BackGround <> "" Then
            oBackColor = oNoticeBoard.Style.BackGround
        End If

        oVisualizzato = String.Format(oVisualizzato, Me.ApplicationUrlBase() & "Modules/Noticeboard/Display.aspx?idMessage=" & oNoticeBoard.Id & "&h=" + CStr(ContenitoreBachecaHeight.Value), Me.BachecaWidth, Me.BachecaHeight, oBackColor)
        Return oVisualizzato
    End Function

    Public ReadOnly Property iBackgroundTemplate() As String Implements IViewMessagePreview.iBackgroundTemplate
        Get
            Return "<iframe id=""datamain"" src=""{0}"" width=""{1}"" height=""{2}"" style=""margin:0; {3} border: 0px;""></iframe>"
        End Get
    End Property


    Public Event SetActive()
    Public Event DeleteMessage()
    Public Event ClearNoticeBoard()
    Public Event VirtualDeleteMessage()
    Public Event UnDeleteMessage(ByVal Recupera As Boolean)
    '   Public Event MoreMessages()

    Public Sub InitController(ByVal oNoticeBoard As NoticeBoard, ByVal oPermission As ModuleNoticeBoard, ByVal isCurrent As Boolean, ByVal Container As NoticeBoardContext.ViewModeType, ByVal PreviousPage As NoticeBoardContext.ViewModeType, ByVal CommunityID As Integer)
        Me.LTRBacheca.Text = ""
        Me.LTRBacheca.Visible = False

        Me.BTNclear.Visible = False
        Me.BTNdelete.Visible = False
        Me.HYPeditADV.Visible = False
        Me.HYPeditHTML.Visible = False
        Me.BTNvirtualDelete.Visible = False
        Me.BTNvirtualUndelete.Visible = False
        Me.BTNvirtualUndeleteAndActivate.Visible = False
        Me.BTNstampa.Visible = False
        Me.BTNsetActive.Visible = False
        Me.HYPeditDS.Visible = False
        Me.HYPmoreMessagesDS.Visible = False
        Me.HYPstampaDS.Visible = False
        Me.DIVmenuDashboard.Visible = isViewedInDashBoard
        Me.DIVmenu.Visible = Not isViewedInDashBoard
        If Not IsNothing(oNoticeBoard) Then
            If Not String.IsNullOrEmpty(oNoticeBoard.Message) AndAlso Not String.IsNullOrEmpty(Trim(oNoticeBoard.Message)) Then
                Me.LTRBacheca.Text = BuildNoticeboard(oNoticeBoard)
                Me.LTRBacheca.Visible = True
                Me.BTNclear.Visible = isCurrent AndAlso Not Me.isViewedInDashBoard AndAlso (oPermission.EditMessage OrElse oPermission.DeleteMessage OrElse oPermission.ServiceAdministration)
            End If
            If oNoticeBoard.Id > 0 Then
                If isViewedInDashBoard Then
                    Me.DIVmenuDashboard.Style.Add("display", "block")
                    Me.DIVmenuDashboard.Visible = True
                    Me.HYPstampaDS.Visible = oPermission.PrintMessage OrElse oPermission.ServiceAdministration
                    Me.HYPeditDS.Visible = (oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    Me.HYPmoreMessagesDS.Visible = (oPermission.ViewOldMessage AndAlso Not oPermission.ServiceAdministration)
                    Me.DVinfo.Visible = False
                    Me.DVinfo.Style.Add("display", "none")
                    Me.HYPeditDS.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardContainer.aspx?View=Message&SmallView=LastFourMessage&PreviousView=DashBoard&MessageID=" & oNoticeBoard.Id.ToString
                    Me.HYPmoreMessagesDS.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardContainer.aspx?View=CurrentMessage&SmallView=LastFourMessage" & "&CommunityID=" & CommunityID.ToString & "&Container=" & Container.ToString & "&PreviousPage=" & PreviousPage.ToString
                Else
                    Me.HYPeditADV.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardAdvanced.aspx?MessageID=" & oNoticeBoard.Id.ToString & "&CommunityID=" & CommunityID.ToString & "&Container=" & Container.ToString & "&FromPage=" & PreviousPage.ToString
                    Me.HYPeditHTML.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardSimple.aspx?MessageID=" & oNoticeBoard.Id.ToString & "&CommunityID=" & CommunityID.ToString & "&Container=" & Container.ToString & "&FromPage=" & PreviousPage.ToString
                    Me.BTNstampa.Visible = oPermission.PrintMessage OrElse oPermission.ServiceAdministration
                    Me.BTNdelete.Visible = oNoticeBoard.isDeleted AndAlso (oPermission.DeleteMessage OrElse oPermission.ServiceAdministration)
                    Me.HYPeditADV.Visible = Not oNoticeBoard.isDeleted AndAlso (oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    Me.HYPeditHTML.Visible = Not oNoticeBoard.isDeleted AndAlso (oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    Me.BTNvirtualDelete.Visible = Not oNoticeBoard.isDeleted AndAlso (oPermission.DeleteMessage OrElse oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    Me.BTNvirtualUndelete.Visible = oNoticeBoard.isDeleted AndAlso (oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    Me.BTNvirtualUndeleteAndActivate.Visible = Me.BTNvirtualUndelete.Visible
                    Me.DIVmenu.Style.Add("display", "block")
                    Me.DIVmenu.Visible = True
                    If Not isCurrent Then
                        Me.BTNsetActive.Visible = Not oNoticeBoard.isDeleted AndAlso (oPermission.EditMessage OrElse oPermission.ServiceAdministration)
                    End If

                    Me.DVinfo.Style.Add("display", "block")
                    Me.DVinfo.Visible = True
                    If oNoticeBoard.isDeleted Then
                        Me.LTinfoOpen.Text = Resource.getValue("useraction.info.deleted")
                    Else
                        Me.LTinfoOpen.Text = Resource.getValue("useraction.info.notdeleted")
                    End If
                    Me.LTinfoByUser.Text = String.Format(Resource.getValue("useraction.info.fromuser"), oNoticeBoard.ModifiedBy.SurnameAndName)
                    If oNoticeBoard.ModifiedOn.HasValue Then
                        Me.LTinfoOnDate.Text = String.Format(Resource.getValue("useraction.info.onDate"), oNoticeBoard.ModifiedOn.Value.ToString("D", Resource.CultureInfo))
                        Me.LTinfoOnDate.Text &= String.Format(Resource.getValue("useraction.info.onTime"), FormatDateTime(oNoticeBoard.ModifiedOn, DateFormat.ShortTime))
                    End If

                End If
                Me.SetBachecaUrlStampa(oNoticeBoard.Id, CommunityID)
            Else
                Me.DIVmenuDashboard.Visible = False
                Me.DIVmenu.Style.Add("display", "none")
                Me.LTinfoOpen.Text = Me.Resource.getValue("info.nonoticeboard")
            End If
        Else
            Me.DIVmenuDashboard.Visible = False
            Me.DIVmenu.Style.Add("display", "none")
            Me.LTinfoOpen.Text = Me.Resource.getValue("info.nonoticeboard")
        End If
    End Sub

    Protected Sub BTNmanage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case sender.CommandName.ToString.ToLower
            Case "clearnoticeboard"
                RaiseEvent ClearNoticeBoard()
            Case "delete"
                RaiseEvent DeleteMessage()
            Case "virtualdelete"
                RaiseEvent VirtualDeleteMessage()
            Case "virtualundelete"
                RaiseEvent UnDeleteMessage(False)
            Case "virtualundeleteactivate"
                RaiseEvent UnDeleteMessage(True)
            Case "setactive"
                RaiseEvent SetActive()
                'Case "moremessage"
                '    RaiseEvent MoreMessages()
        End Select
    End Sub


    Private Sub SetBachecaUrlStampa(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer)
        Dim UrlStampa As String = ""
        Dim PrintUrl As String = "PersonaID=" & Me.UtenteCorrente.ID & "&LinguaID=" & Me.LinguaID & "&LinguaCode=" & Me.LinguaCode & "&BachecaID=" & NoticeBoardID.ToString & "&ComunitaID=" & CommunityID

        If String.IsNullOrEmpty(PrintUrl) Then
            Me.BTNstampa.Visible = False
            Me.BTNstampa.Attributes("onclick") = "return false;"
        Else
            If NoticeBoardID = 0 Then
                UrlStampa = Me.EncryptedUrl("Comunita.bacheca", PrintUrl, SecretKeyUtil.EncType.Altro)
            Else
                UrlStampa = Me.EncryptedUrl(NoticeBoardID.ToString & ".bacheca", PrintUrl, SecretKeyUtil.EncType.Altro)
            End If

            Me.BTNstampa.Attributes("onclick") = "javascript:window.open('" & UrlStampa & "','','location=no,toolbar=yes,menubar=no,width=640,height=480,resizable=yes,scrollbars=auto');return false;"
            Me.HYPstampaDS.Attributes("onclick") = "javascript:window.open('" & UrlStampa & "','','location=no,toolbar=yes,menubar=no,width=640,height=480,resizable=yes,scrollbars=auto');return false;"
            Me.HYPstampaDS.NavigateUrl = UrlStampa
        End If
    End Sub

    Public Sub InitController1(ByVal oNoticeBoard As lm.Comol.Modules.Base.DomainModel.NoticeBoard, ByVal oPermission As lm.Comol.Modules.Base.DomainModel.ModuleNoticeBoard, ByVal isCurrent As Boolean, ByVal Container As lm.Comol.Modules.Base.DomainModel.NoticeBoardContext.ViewModeType, ByVal CommunityID As Integer, ByVal UserID As Integer) Implements IViewMessagePreview.InitController

    End Sub

    Private Sub BTNdelete_Click(sender As Object, e As System.EventArgs) Handles BTNdelete.Click

    End Sub
End Class