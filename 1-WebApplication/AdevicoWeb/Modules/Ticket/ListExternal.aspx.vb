Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class ListExternal
    Inherits TicketBase
    Implements TK.Presentation.View.iViewTicketListExt

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketListExtPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketListExtPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketListExtPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne
    Private _OrderCol As TK.Domain.Enums.TicketOrderUser = TK.Domain.Enums.TicketOrderUser.lifeTime
    Private _OrderAsc As Boolean = True
    Private _EnableOrder As Boolean = False
    Private _HasElements As Boolean = False
    Private _HasDraft As Boolean = False

    ''' <summary>
    ''' Stringa dell'ordinamento corrente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property ReorderString As String
        Get
            Return ViewStateOrDefault("OrderString", TK.Domain.Enums.TicketOrderUser.subject.ToString() & "." & True.ToString())
        End Get
        Set(value As String)
            ViewState("OrderString") = value
        End Set
    End Property

    ''' <summary>
    ''' Dimensione pagina corrente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property CurrentPageSize As Integer
        Get
            Dim CPS As Integer = 25
            Try
                CPS = System.Convert.ToInt32(ViewStateOrDefault("CurrentPageSize", 25))
            Catch ex As Exception

            End Try
            Return CPS
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property

    ''' <summary>
    ''' Indice corrente paginazione
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property CurrentPageIndex As Integer
        Get
            Dim CPI As Integer = 0
            Try
                CPI = ViewStateOrDefault("CurrentPageIndex", 0)
            Catch ex As Exception

            End Try
            Return CPI
        End Get
        Set(value As Integer)
            ViewState("CurrentPageIndex") = value
        End Set
    End Property

    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase
        Get
            Try
                Return Me.ViewState("pager")
            Catch ex As Exception
            End Try
            Return Nothing
        End Get
        Set(value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("pager") = value
        End Set
    End Property

#End Region

#Region "Implements"
    'Property della VIEW
    Public ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User Implements TK.Presentation.View.iViewTicketListExt.CurrentUser
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property
#End Region

#Region "Inherits"
    'Property del PageBase

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Master.Page_Title = Resource.getValue("Page.Title.External")

        If Not IsNothing(Me.Pager) Then
            Me.PGgridBot.Pager = Me.Pager
        End If

        If Not Page.IsPostBack() Then
            Me.Master.BindSkin()
            Me.InitInternalFilters()
            Me.CurrentPresenter.InitView()
            Me.Reorder("")
        End If

    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        'ATTENZIONE!
        'BindDati non viene eseguito allo SCADERE DELLA SESSIONE!!!
        'Intanto messo in Page_Load
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ListUser", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            .setLiteral(LTpageTitle_t)
            .setLiteral(LTcontentTitle_t)

            .setHyperLink(HYPadd, True, True)
            HYPadd.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalAdd()

            .setLabel(LBStatus_t)
            .setLabel(LBtitle_t)
            .setLabel(LBdate_t)

            '.setCheckBox(CBXStart)
            '.setCheckBox(CBXend)
            .setCheckBox(CBXupdatedOnly)

            .setLinkButton(LNBfilter, True, True)

            '.setDropDownList(DDLdateField, "CreateOn")
            '.setDropDownList(DDLdateField, "LastModify")
            .setRadioButtonList(RBLdateField, "CreatedOn")
            .setRadioButtonList(RBLdateField, "LastModify")
            'ListItem		*Creazione			Value="CreatedOn"
            'ListItem		*Ultima modifica	Value="LastModify"


            '.setLabel(LBLusr_t)
            .setLabel(LBnumTicket_t)

            .setCheckBox(CBXshowPreview)
            .setCheckBox(CBXshowId)
            .setCheckBox(CBXshowCategory)
            .setCheckBox(CBXshowLastUpd)

            '.setHyperLink(HYPmanagement, True, True)
            'HYPmanagement.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListResolver

            .setLiteral(LTnoAccess)


            'Legend
            .setLiteral(LTleged_t)


            .setLabel(LBfilterTitle)
            .setLabel(LBfiltersShow)
            .setLabel(LBfiltersHide)

            .setLabel(LBfiltDateStart)
            .setLabel(LBfiltDateEnd)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        Me.CTRLtopBar.LogOut()
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.PNLcontent.Visible = False
        Me.PNLserviceDisabled.Visible = True
    End Sub

#End Region

#Region "Implements"
    'Sub e function della View
    Public Sub InitFilters(Filters As TK.Domain.DTO.DTO_ListInit) Implements TK.Presentation.View.iViewTicketListExt.InitFilters

    End Sub

    Public Sub SetInfo(Info As TK.Domain.DTO.DTO_ListInfo) Implements TK.Presentation.View.iViewTicketListExt.SetInfo
        With Info
            'SetLitRealInfo(LTnumDraft, .Draft, TicketHelper.InfoFields.draft)
            'SetLitRealInfo(LTnumOpen, .Open, TicketHelper.InfoFields.open)
            'SetLitRealInfo(LTnumInProg, .WorkingOn, TicketHelper.InfoFields.inprogress)
            'SetLitRealInfo(LTnumRequest, .Waiting, TicketHelper.InfoFields.request)
            'SetLitRealInfo(LTnumSolved, .SolvedClosed, TicketHelper.InfoFields.closedSolved)
            'SetLitRealInfo(LTnumClosed, .UnSolvedClosed, TicketHelper.InfoFields.closedUnsolved)

            Dim lInfosList As New List(Of Comunita_OnLine.InfoItemSimple)()
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("draft", .Draft))
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("open", .Open))
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("inprogress", .WorkingOn))
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("request", .Waiting))
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("solved", .SolvedClosed))
            lInfosList.Add(New Comunita_OnLine.InfoItemSimple("closed", .UnSolvedClosed))


            CTRLlistInfo.InitControl(Me.Resource, lInfosList, True, True)

        End With

    End Sub

    Public Sub SetTickets(Tickets As System.Collections.Generic.List(Of TK.Domain.DTO.DTO_TicketListItemUser), PageIndex As Integer, RecordTotal As Integer, ShowManagement As Boolean, HasDraft As Boolean) Implements TK.Presentation.View.iViewTicketListExt.SetTickets

        _HasDraft = HasDraft

        Me.BindTable(Tickets)

        If (RecordTotal > 0) Then
            Dim Cur_Pager As New lm.Comol.Core.DomainModel.PagerBase()
            If (RecordTotal > 0) Then
                Cur_Pager.Count = RecordTotal - 1
            Else
                Cur_Pager.Count = RecordTotal
            End If

            Cur_Pager.PageSize = CurrentPageSize    'Dimensione pagina
            'Cur_Pager.LastPage =
            Cur_Pager.PageIndex = PageIndex         'Indice corrente
            CurrentPageIndex = PageIndex


            Me.Pager = Cur_Pager

            Me.PGgridBot.Visible = True
            Me.PGgridBot.Pager = Cur_Pager
        Else
            CurrentPageIndex = 0
            Me.PGgridBot.Visible = False
        End If

    End Sub

    Public Function GetFilters() As TK.Domain.DTO.DTO_ListFilterUser Implements TK.Presentation.View.iViewTicketListExt.GetFilters

        Dim Order() As String = ReorderString.Split(".")

        Dim filters As New TK.Domain.DTO.DTO_ListFilterUser()

        With filters
            'Ordinamenti
            .OrderField = [Enum].Parse(GetType(TK.Domain.Enums.TicketOrderUser), Order(0))
            .OrderAscending = System.Convert.ToBoolean(Order(1))

            'filtri
            Dim Title As String = Me.TXBtitle_t.Text
            If (String.IsNullOrEmpty(Title)) Then
                .Title = ""
            Else
                .Title = Title
            End If

            .ShowOnlyNews = Me.CBXupdatedOnly.Checked


            If Me.DDLStatus.SelectedValue = "all" Then
                .ShowAllStatus = True
            Else
                .ShowAllStatus = False
                .Status = TK.Domain.Enums.TicketStatus.open

                Try
                    'Potrebbe non essere ancora inizializzato quando richiamato...
                    .Status = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.TicketStatus), Me.DDLStatus.SelectedValue), TK.Domain.Enums.TicketStatus)
                Catch ex As Exception

                End Try
            End If

            If Me.RBLdateField.SelectedValue = "CreatedOn" Then
                .DateField = TK.Domain.Enums.TicketUserDateFilter.Creation
            Else
                .DateField = TK.Domain.Enums.TicketUserDateFilter.LastModify
            End If


            'If (Me.CBXStart.Checked) Then
            .DateStart = Me.RDPstart.SelectedDate
            'Else
            '.DateStart = Nothing
            'End If

            'If (Me.CBXend.Checked) Then
            .DateEnd = Me.RDPend.SelectedDate
            'Else
            '.DateEnd = Nothing
            'End If

            .PageSize = CurrentPageSize
            .PageIndex = CurrentPageIndex
        End With

        Return filters

    End Function

    Public Function GetBasePath() As String Implements TK.Presentation.View.iViewTicketListExt.GetBasePath
        'da \Modules\CallForPeaper\Request.aspx.vb riga 537-544
        Dim basePath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            basePath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\"
        Else
            basePath = Me.SystemSettings.File.Materiale.DrivePath & "\"
        End If
        basePath = Replace(basePath, "\\", "\")
        Return basePath
    End Function

    Public Sub ShowDeletMessage(ErrorType As TK.Domain.Enums.TicketDraftDeleteError) Implements TK.Presentation.View.iViewTicketListExt.ShowDeletMessage

        If ErrorType = TK.Domain.Enums.TicketDraftDeleteError.hide Then
            Me.PNLmessages.Visible = False
            CTRLmessagesInfo.Visible = False
        Else
            Me.PNLmessages.Visible = True
            CTRLmessagesInfo.Visible = True

            Dim Message As String = Resource.getValue("Message.DeleteDraft." & ErrorType.ToString())

            If (ErrorType = TK.Domain.Enums.TicketDraftDeleteError.none) Then
                CTRLmessagesInfo.InitializeControl(Message, lm.Comol.Core.DomainModel.Helpers.MessageType.success)
            Else
                CTRLmessagesInfo.InitializeControl(Message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
            End If
        End If

    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"
    'Private Sub SetLitRealInfo(ByVal Lit As Literal, ByVal num As Integer, ByVal field As TicketHelper.InfoFields)

    '    Dim Source As String = Me.LTnumLayout.Text

    '    If (num <= 0) Then
    '        Source = Source.Replace("{emptyCss}", " empty")
    '        Source = Source.Replace("{num}", "0")
    '    Else
    '        Source = Source.Replace("{emptyCss}", "")
    '        Source = Source.Replace("{num}", num.ToString())
    '    End If

    '    Source = Source.Replace("{title}", Resource.getValue(Lit.ID & ".title"))
    '    Source = Source.Replace("{text}", Resource.getValue(Lit.ID & ".text"))
    '    Source = Source.Replace("{field}", field.ToString())
    '    Lit.Text = Source

    'End Sub

    Private Sub BindTable(ByVal Tickets As IList(Of TK.Domain.DTO.DTO_TicketListItemUser))

        Dim ds As String = Request.QueryString("ds")

        If Not Page.IsPostBack() AndAlso Not String.IsNullOrEmpty(ds) AndAlso ds = "1" Then
            ShowDeletMessage(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketDraftDeleteError.none)
        Else
            ShowDeletMessage(TK.Domain.Enums.TicketDraftDeleteError.hide)
        End If

        Dim ItmNum As Integer = Tickets.Count()

        Me._EnableOrder = If(ItmNum > 1, True, False)
        Me._HasElements = If(ItmNum > 0, True, False)

        Me.RPTtickets.DataSource = Tickets
        Me.RPTtickets.DataBind()

    End Sub

    ''' <summary>
    ''' Imposta i link per il riordino nell'header della tabella
    ''' </summary>
    ''' <param name="Item">Item corrente</param>
    ''' <param name="LkbName">Nome del linkbutton</param>
    ''' <param name="OrderValue">Valore per l'ordinamento</param>
    ''' <param name="IsAscending">Direzione dell'ordinamento</param>
    ''' <remarks></remarks>
    Private Sub SetRPTorderLink( _
                               ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                               ByVal LkbName As String, _
                               ByVal OrderValue As TK.Domain.Enums.TicketOrderUser, _
                               ByVal IsAscending As Boolean)

        Dim lkb As LinkButton = Item.FindControl(LkbName)

        If Not IsNothing(lkb) Then
            Resource.setLinkButton(lkb, True, True)
            lkb.CommandName = "Reorder"
            lkb.CommandArgument = OrderValue.ToString() + "." + IsAscending.ToString()

            If Me._EnableOrder And Not (_OrderCol = OrderValue AndAlso IsAscending = Me._OrderAsc) Then
                lkb.Enabled = True
            Else
                lkb.Enabled = False
                lkb.CssClass &= " disable"
            End If
        End If

    End Sub

    Private Sub InitInternalFilters()
        Me.DDLStatus.Items.Clear()

        Dim AllItm As New ListItem()
        AllItm.Value = "all"
        AllItm.Text = Resource.getValue("Status.All")
        AllItm.Selected = True
        Me.DDLStatus.Items.Add(AllItm)

        Dim LstTkStatus As Array = [Enum].GetValues(GetType(TK.Domain.Enums.TicketStatus))

        For i As Integer = 0 To LstTkStatus.Length - 1
            Me.DDLStatus.Items.Add(New ListItem( _
                      Resource.getValue("Status." & LstTkStatus(i).ToString()), _
                        LstTkStatus(i).ToString()))
        Next

        Me.PGgridBot.Visible = False

    End Sub

    Private Sub Reorder(ByVal OrderString As String)

        If String.IsNullOrEmpty(OrderString) Then
            OrderString = ReorderString
        Else
            ReorderString = OrderString
        End If

        Me.CurrentPresenter.BindList()

    End Sub

    Public Function GetActionTitle() As String
        Return Resource.getValue("TH.Action.Title")
    End Function

    Public Function GetLegendTitle(ByVal value As String) As String
        Dim out As String = Resource.getValue("Legend." & value & ".Title")
        If String.IsNullOrEmpty(out) Then
            Return value
        End If
        Return out
    End Function

    Public Function GetLegendText(ByVal value As String) As String
        Dim out As String = Resource.getValue("Legend." & value & ".Text")
        If String.IsNullOrEmpty(out) Then
            Return value
        End If
        Return out
    End Function

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

    Private Sub RPTtickets_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTtickets.ItemCommand

        Select Case e.CommandName
            Case "Reorder"
                Reorder(e.CommandArgument)
            Case "DeleteDraft"

                Dim idTicket As Int64 = 0


                If idTicket = 0 Then
                    idTicket = Int64.TryParse(e.CommandArgument, idTicket)
                End If


                If idTicket = 0 Then
                    idTicket = TK.Domain.RootObject.GetTicketId(e.CommandArgument)
                End If

                CurrentPresenter.DeleteDraft(idTicket, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End Select

    End Sub

    Private Sub RPTtickets_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtickets.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            TicketHelper.SetRPTLiteral(e.Item, "LTcode_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTtitle_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTcategory_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTstatus_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTlastModify_t", Me.Resource)

            SetRPTorderLink(e.Item, "LNBorderByIdUp", TK.Domain.Enums.TicketOrderUser.code, True)
            SetRPTorderLink(e.Item, "LNBorderByIdDown", TK.Domain.Enums.TicketOrderUser.code, False)
            SetRPTorderLink(e.Item, "LNBorderByTitleUp", TK.Domain.Enums.TicketOrderUser.subject, True)
            SetRPTorderLink(e.Item, "LNBorderByTitleDown", TK.Domain.Enums.TicketOrderUser.subject, False)
            SetRPTorderLink(e.Item, "LNBorderByCatUp", TK.Domain.Enums.TicketOrderUser.category, True)
            SetRPTorderLink(e.Item, "LNBorderByCatDown", TK.Domain.Enums.TicketOrderUser.category, False)
            SetRPTorderLink(e.Item, "LNBorderByStatusUp", TK.Domain.Enums.TicketOrderUser.status, True)
            SetRPTorderLink(e.Item, "LNBorderByStatusDown", TK.Domain.Enums.TicketOrderUser.status, False)
            SetRPTorderLink(e.Item, "LNBorderByLastModifyUp", TK.Domain.Enums.TicketOrderUser.lifeTime, True)
            SetRPTorderLink(e.Item, "LNBorderByLastModifyDown", TK.Domain.Enums.TicketOrderUser.lifeTime, False)

            '_HasDraft
            Dim THaction As System.Web.UI.HtmlControls.HtmlControl = e.Item.FindControl("THaction")

            If Not IsNothing(THaction) Then

                If Not _HasDraft Then
                    THaction.Attributes.Add("class", "actions hide")
                Else
                    THaction.Attributes.Add("class", "actions")
                End If

            End If

        ElseIf e.Item.ItemType = ListItemType.Footer Then

            Dim PLHfooterVoid As PlaceHolder = e.Item.FindControl("PLHfooterVoid")
            If Not IsNothing(PLHfooterVoid) Then
                If (_HasElements) Then
                    PLHfooterVoid.Visible = False
                Else
                    PLHfooterVoid.Visible = True
                    Dim LITempty As Literal = e.Item.FindControl("LTempty")
                    If Not IsNothing(LITempty) Then
                        Resource.setLiteral(LITempty)
                    End If
                End If
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim Ticket As TK.Domain.DTO.DTO_TicketListItemUser = e.Item.DataItem
            If Not IsNothing(Ticket) Then
                With Ticket
                    TicketHelper.SetRPTLabel(e.Item, "LBcode", .Code)

                    Dim HYPsubject As HyperLink = e.Item.FindControl("HYPsubject")
                    If Not IsNothing(HYPsubject) Then
                        HYPsubject.Text = .Title
                        If (Ticket.IsDraft) Then
                            HYPsubject.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalAdd(.Code)
                        Else
                            HYPsubject.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalEdit(.Code)

                        End If

                    End If

                    TicketHelper.SetRPTLabel(e.Item, "LBcategory", .CreationCategoryName, 0)
                    TicketHelper.SetRPTLiteral(e.Item, "LTDescription", .FirstMessage)


                    Dim StatusText As String = Resource.getValue("Status." & .Status.ToString())
                    If Not .Condition = TK.Domain.Enums.TicketCondition.active Then
                        StatusText &= " (" & Resource.getValue("Conditions." & .Condition.ToString()) & ")"
                    End If

                    TicketHelper.SetRPTLabel(e.Item, "LBstatus", StatusText, 0)

                    If (.IsDraft OrElse IsNothing(.LastModify)) Then
                        TicketHelper.SetRPTLabel(e.Item, "LBtime", "--")
                        TicketHelper.SetRPTLiteral(e.Item, "LTtimeMore", "")
                    Else
                        TicketHelper.SetRPTLabel(e.Item, "LBtime", _
                                        .CreateOn, TicketHelper.DateTimeMode.DateAndTime)

                        TicketHelper.SetRPTLiteral(e.Item, "LTtimeMore", _
                                        .LastModify, TicketHelper.DateTimeMode.DateAndTime)
                    End If

                    Dim trItem As System.Web.UI.HtmlControls.HtmlTableRow = e.Item.FindControl("trItem")
                    If Not IsNothing(trItem) Then
                        If .IsDraft Then
                            trItem.Attributes.Add("class", " isdraft")
                        ElseIf .HasNews Then
                            trItem.Attributes.Add("class", " isunread")
                        End If
                    End If

                    Dim LNBdelete As LinkButton = e.Item.FindControl("LNBdelete")

                    If Not IsNothing(LNBdelete) Then

                        If .IsDraft Then
                            LNBdelete.Visible = True
                            Resource.setLinkButton(LNBdelete, True, True, False, True)
                            LNBdelete.CommandName = "DeleteDraft"
                            LNBdelete.CommandArgument = .Id
                        Else
                            LNBdelete.Visible = False
                        End If


                    End If
                End With

                Dim TDaction As System.Web.UI.HtmlControls.HtmlControl = e.Item.FindControl("TDaction")

                If Not IsNothing(TDaction) Then

                    If Not _HasDraft Then
                        TDaction.Attributes.Add("class", "actions hide")
                    Else
                        TDaction.Attributes.Add("class", "actions")
                    End If

                End If
            End If

        End If
    End Sub

    Private Sub LNBfilter_Click(sender As Object, e As System.EventArgs) Handles LNBfilter.Click
        Me.PNLmessages.Visible = False
        Reorder("")
    End Sub

    Private Sub CTRLtopBar_UpdateInternationalization(oLingua As Comol.Entity.Lingua) Handles CTRLtopBar.UpdateInternationalization
        MyBase.UpdateLanguage(oLingua)

        Me.Master.BindSkin()
        Me.InitInternalFilters()
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub PGgridBot_OnPageSelected() Handles PGgridBot.OnPageSelected
        Me.PNLmessages.Visible = False
        Me.CurrentPageIndex = PGgridBot.Pager.PageIndex
        Me.CurrentPageSize = PGgridBot.Pager.PageSize
        Reorder("")
    End Sub

#End Region

    Private Sub CTRLlistInfo_SendAction(Action As String) Handles CTRLlistInfo.SendAction
        setFilterByInfo(Action)
        Reorder("")
    End Sub

    Private Sub setFilterByInfo(ByVal Status As String)
        Select Case Status
            Case "draft"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.draft.ToString()
            Case "open"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.open.ToString()
            Case "inprogress"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.inProcess.ToString()
            Case "request"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.userRequest.ToString()
            Case "solved"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.closeSolved.ToString()
            Case "closed"
                Me.DDLStatus.SelectedValue = TK.Domain.Enums.TicketStatus.closeUnsolved.ToString()
        End Select

        'Me.TXBbehalf.Text = ""
        Me.TXBtitle_t.Text = ""
        'Me.CBXStart.Checked = False
        'Me.CBXend.Checked = False

        Me.RDPstart.SelectedDate = Nothing
        Me.RDPend.SelectedDate = Nothing


        Me.CBXupdatedOnly.Checked = False

        'RBLbehalf.SelectedValue = BehalfValue
    End Sub
End Class