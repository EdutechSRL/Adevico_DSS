Imports TK = lm.Comol.Core.BaseModules.Tickets

'Inherits PageBase
Public Class ListResolver
    Inherits TicketBase
    Implements TK.Presentation.View.iViewTicketListMan
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketListManPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketListManPresenter
        Get

            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketListManPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne
    'Private _OrderCol As TK.Domain.Enums.TicketOrderUser = TK.Domain.Enums.TicketOrderUser.lifeTime
    'Private _OrderAsc As Boolean = True
    Private _EnableOrder As Boolean = False
    Private _HasElements As Boolean = False

    Private ReadOnly Property _OrderCol As TK.Domain.Enums.TicketOrderUser
        Get
            Dim Column As TK.Domain.Enums.TicketOrderUser = TK.Domain.Enums.TicketOrderUser.lifeTime
            Dim Order() As String = ReorderString.Split(".")

            Try
                Column = [Enum].Parse(GetType(TK.Domain.Enums.TicketOrderUser), Order(0))
            Catch ex As Exception

            End Try

            Return Column
        End Get
    End Property

    Private ReadOnly Property _OrderAsc As TK.Domain.Enums.TicketOrderUser
        Get
            Dim Ascending As Boolean = True

            Dim Order() As String = ReorderString.Split(".")

            Try
                Ascending = System.Convert.ToBoolean(Order(1))
            Catch ex As Exception

            End Try

            Return Ascending

        End Get
    End Property


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


#End Region

#Region "Implements"
    'Property della VIEW

    Public Function GetFilters() As TK.Domain.DTO.DTO_ListFilterManager

        Dim flts As New TK.Domain.DTO.DTO_ListFilterManager()

        If (Me.DDLstatus.Items.Count() <= 0) Then
            Return flts
        End If

        With flts

            Dim DateSelection As String = Me.DDLdateField.SelectedValue

            If Not DateSelection = "-1" Then
                Try
                    'Potrebbe non essere ancora inizializzato quando richiamato...
                    .DateField = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.TicketManagerDateFilter), DateSelection), TK.Domain.Enums.TicketManagerDateFilter)
                Catch ex As Exception
                    .DateField = TK.Domain.Enums.TicketManagerDateFilter.Creation
                End Try


                'If (CBXStart.Checked) Then
                .DateStart = Me.RDPstart.SelectedDate
                'Else
                '    .DateStart = Nothing
                'End If

                'If (CBXend.Checked) Then
                .DateEnd = Me.RDPend.SelectedDate
                'Else
                '    .DateEnd = Nothing
                'End If

            End If

            .LanguageCode = Me.DDLlang.SelectedValue

            'RIVEDERE!!!
            .OnlyTicket = TK.Domain.Enums.TicketManagerListOnly.None

            If (CBXonlyNoAnswers.Checked) Then
                .OnlyTicket = .OnlyTicket Or lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketManagerListOnly.noanswers
            End If

            If (CBXonlyNotAssigned.Checked) Then
                .OnlyTicket = .OnlyTicket Or lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketManagerListOnly.notassigned
            End If

            If (CBXonlyWithNews.Checked) Then
                .OnlyTicket = .OnlyTicket Or lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketManagerListOnly.withnews
            End If

            If Me.DDLstatus.SelectedValue = "all" Then
                .ShowAllStatus = True
            Else
                .ShowAllStatus = False
                .Status = TK.Domain.Enums.TicketStatus.open

                Try
                    'Potrebbe non essere ancora inizializzato quando richiamato...
                    .Status = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.TicketStatus), Me.DDLstatus.SelectedValue), TK.Domain.Enums.TicketStatus)
                Catch ex As Exception

                End Try
            End If

            .Title = Me.TXBsubject.Text


            Try
                'Potrebbe non essere ancora inizializzato quando richiamato...
                .Status = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.TicketStatus), Me.DDLstatus.SelectedValue), TK.Domain.Enums.TicketStatus)
            Catch ex As Exception

            End Try

            .CategoryId = Me.CTRLddlCat.SelectedId

            Dim Order() As String = ReorderString.Split(".")
            .OrderField = [Enum].Parse(GetType(TK.Domain.Enums.TicketOrderManRes), Order(0))
            .OrderAscending = System.Convert.ToBoolean(Order(1))

            .PageIndex = CurrentPageIndex
            .PageSize = CurrentPageSize
        End With

        Return flts
    End Function

#End Region

#Region "Inherits"

#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Me.Pager) Then
            Me.PGgridBot.Pager = Me.Pager
        End If

    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        'Me.CurrentPresenter.BindTable(GetFilters())
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ListResolver", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            .setLiteral(LTpageTitle_t)
            .setLiteral(LTcontentTitle_t)

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBot, True, True)

            .setLinkButton(LNBfilter, True, True)

            .setLabel(LBstatus_t)
            .setLabel(LBsubject_t)
            .setLabel(LBlang_t)
            .setLabel(LBdateField_t)
            .setLabel(LBcategory_t)
            .setLabel(LBnumTicket_t)

            .setCheckBox(CBXshowId)
            .setCheckBox(CBXshowLang)
            .setCheckBox(CBXshowCommunity)
            .setCheckBox(CBXshowCategory)

            '.setCheckBox(CBXStart)
            '.setCheckBox(CBXend)
            .setCheckBox(CBXonlyNoAnswers)
            .setCheckBox(CBXonlyNotAssigned)
            .setCheckBox(CBXonlyWithNews)

            .setLiteral(LTlkbBulk)

            .setHyperLink(HYPmineTop, True, True)
            HYPmineTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(Me.ViewCommunityId)

            .setHyperLink(HYPmineBot, True, True)
            HYPmineBot.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(Me.ViewCommunityId)


            '.setHyperLink(HYPaddTop, True, True)
            'HYPaddTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketAdd(Me.ViewCommunityId)

            '.setHyperLink(HYPaddBot, True, True)
            'HYPaddBot.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketAdd(Me.ViewCommunityId)


            'RDPstart.a()
            RDPstart.Calendar.CultureInfo = New System.Globalization.CultureInfo(MyBase.LinguaCode)
            RDPend.Calendar.CultureInfo = New System.Globalization.CultureInfo(MyBase.LinguaCode)

            RDPstart.Culture = New System.Globalization.CultureInfo(MyBase.LinguaCode)
            RDPend.Culture = New System.Globalization.CultureInfo(MyBase.LinguaCode)

            'Legend
            .setLiteral(LTleged_t)


            .setLabel(LBfilterTitle)
            .setLabel(LBfiltersShow)
            .setLabel(LBfiltersHide)

            .setLabel(LBfiltDateStart)
            .setLabel(LBfiltDateEnd)


            CBXonlyNoAnswers.LabelAttributes.Add("class", "label")
            CBXonlyNoAnswers.InputAttributes.Add("class", "input")

            CBXonlyNotAssigned.LabelAttributes.Add("class", "label")
            CBXonlyNotAssigned.InputAttributes.Add("class", "input")

            CBXonlyWithNews.LabelAttributes.Add("class", "label")
            CBXonlyWithNews.InputAttributes.Add("class", "input")

            .setLabel(LBfiltcbxLeg_t)
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.TicketListResolver(CommunityId), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region


#Region "Implements"
    'Sub e function della View
    Public Sub BindCategoriesFilter(Categories As IList(Of TK.Domain.DTO.DTO_CategoryTree), SelectedCategory As TK.Domain.DTO.DTO_CategoryTree) Implements TK.Presentation.View.iViewTicketListMan.BindCategoriesFilter

        'DDL categorie
        Dim AllCategories As New List(Of TK.Domain.DTO.DTO_CategoryTree)()

        Dim ALLCate As New TK.Domain.DTO.DTO_CategoryTree()
        With ALLCate
            .Children = Nothing
            .Description = Resource.getValue("DDLcate.All.Desciption")
            .Name = Resource.getValue("DDLcate.All.Name")
            .Id = -1
            .Order = -1
            .Icon = ""
            .IsSelectable = True
        End With

        AllCategories.Add(ALLCate)

        AllCategories = AllCategories.Union(Categories).ToList()

        If IsNothing(SelectedCategory) OrElse SelectedCategory.Id <= 0 Then
            Me.CTRLddlCat.InitDDL(AllCategories, -1, Resource.getValue("DDLcate.All.Name"))
        Else
            Me.CTRLddlCat.InitDDL(AllCategories, SelectedCategory.Id, Resource.getValue("DDLcate.All.Name"))
        End If

    End Sub

    Public Sub BindFilters(Languages As IList(Of lm.Comol.Core.DomainModel.Languages.BaseLanguageItem)) Implements TK.Presentation.View.iViewTicketListMan.BindFilters

        'DDL STATUS

        Me.DDLstatus.Items.Clear()

        Dim AllItm As New ListItem()
        AllItm.Value = "all"
        AllItm.Text = Resource.getValue("Status.All")
        AllItm.Selected = True
        Me.DDLstatus.Items.Add(AllItm)

        Dim LstTkStatus As Array = [Enum].GetValues(GetType(TK.Domain.Enums.TicketStatus))

        For i As Integer = 0 To LstTkStatus.Length - 1
            If LstTkStatus(i) <> TK.Domain.Enums.TicketStatus.draft Then
                Me.DDLstatus.Items.Add(New ListItem( _
                      Resource.getValue("Status." & LstTkStatus(i).ToString()), _
                        LstTkStatus(i).ToString()))
            End If
        Next

        'DDL Lingue
        Me.DDLlang.Items.Clear()
        AllItm = New ListItem()
        AllItm.Value = "all"
        AllItm.Text = Resource.getValue("Languages.All")
        AllItm.Selected = True
        Me.DDLlang.Items.Add(AllItm)

        If IsNothing(Languages) OrElse Not Languages.Any() Then
            Me.DDLlang.Enabled = False
        Else
            For Each lang As lm.Comol.Core.DomainModel.Languages.BaseLanguageItem In Languages
                Me.DDLlang.Items.Add(New ListItem(lang.Name, lang.Code))
            Next
        End If

        'Date Field
        Me.DDLdateField.Items.Clear()
        Dim liDefaultDate As New ListItem( _
                 Resource.getValue("DateField.DEFAULT"), "-1")
        liDefaultDate.Selected = True
        DDLdateField.Items.Add(liDefaultDate)

        Dim LstTkDatefields As Array = [Enum].GetValues(GetType(TK.Domain.Enums.TicketManagerDateFilter))

        For i As Integer = 0 To LstTkDatefields.Length - 1
            Me.DDLdateField.Items.Add(New ListItem( _
                  Resource.getValue("DateField." & LstTkDatefields(i).ToString()), _
                    LstTkDatefields(i).ToString()))
        Next

    End Sub



    Public Sub bindInfo(FilterInit As TK.Domain.DTO.DTO_ListInfo) Implements TK.Presentation.View.iViewTicketListMan.bindInfo

    End Sub

    Public Sub BindTable(Items As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_TicketListItemManager), TotalRec As Integer, CurrentPage As Integer, HasDraft As Boolean) Implements TK.Presentation.View.iViewTicketListMan.BindTable

        Dim ItmNum As Integer = Items.Count()

        Me._EnableOrder = If(ItmNum > 1, True, False)
        Me._HasElements = If(ItmNum > 0, True, False)


        If (TotalRec > 0) Then
            Dim Cur_Pager As New lm.Comol.Core.DomainModel.PagerBase()
            If (TotalRec > 0) Then
                Cur_Pager.Count = TotalRec - 1
            Else
                Cur_Pager.Count = TotalRec
            End If

            Cur_Pager.PageSize = CurrentPageSize    'Dimensione pagina
            Cur_Pager.PageIndex = CurrentPage         'Indice corrente
            CurrentPageIndex = CurrentPage

            Me.Pager = Cur_Pager

            Me.PGgridBot.Visible = True
            Me.PGgridBot.Pager = Cur_Pager

        Else
            CurrentPageIndex = 0
            Me.PGgridBot.Visible = False
        End If


        'Dim Pager As New lm.Comol.Core.DomainModel.PagerBase()
        'Pager.Count = TotalRec
        'Pager.PageIndex = CurrentPage
        'CurrentPageIndex = CurrentPage
        'Pager.PageSize = CurrentPageSize

        'PGgridBot.Pager = Pager

        'Me.Pager = Pager

        'PGgridBot.Visible = _HasElements
        'Me.PNLcolumns.Visible = _HasElements

        Me.RptTicket.DataSource = Items
        Me.RptTicket.DataBind()

        Resource.setHyperLink(HYPaddTop, True, True, False, False)
        HYPaddTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketAddFromList(Me.ViewCommunityId, 0)

        Resource.setHyperLink(HYPaddBot, True, True, False, False)
        HYPaddBot.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketAddFromList(Me.ViewCommunityId, 0)

    End Sub

    ''' <summary>
    ''' Imposta le info sul numero di Ticket per i vari stati
    ''' </summary>
    ''' <param name="Info">Le info da impostare</param>
    Public Sub SetInfo(Info As TK.Domain.DTO.DTO_ListInfo) Implements TK.Presentation.View.iViewTicketListMan.SetInfo

        With Info

            'If (.Draft + .Open + .SolvedClosed + .UnSolvedClosed + .Waiting + .WorkingOn) <= 0 Then
            '    Me.PNLdetails.Visible = False

            'Else
            'Me.PNLdetails.Visible = True

            'SetLitRealInfo(LTnumOpen, .Open, TicketHelper.InfoFields.open)
            'SetLitRealInfo(LTnumInProg, .WorkingOn, TicketHelper.InfoFields.inprogress)
            'SetLitRealInfo(LTnumRequest, .Waiting, TicketHelper.InfoFields.request)
            'SetLitRealInfo(LTnumSolved, .SolvedClosed, TicketHelper.InfoFields.closedSolved)
            'SetLitRealInfo(LTnumClosed, .UnSolvedClosed, TicketHelper.InfoFields.closedUnsolved)
            
            If ((.Draft + .Open + .WorkingOn + .Waiting + .SolvedClosed + .UnSolvedClosed) > 0) Then
                Dim lInfosList As New List(Of Comunita_OnLine.InfoItemSimple)()
                'lInfosList.Add(New Comunita_OnLine.InfoItemSimple("draft", .Draft))
                lInfosList.Add(New Comunita_OnLine.InfoItemSimple("open", .Open))
                lInfosList.Add(New Comunita_OnLine.InfoItemSimple("inprogress", .WorkingOn))
                lInfosList.Add(New Comunita_OnLine.InfoItemSimple("request", .Waiting))
                lInfosList.Add(New Comunita_OnLine.InfoItemSimple("solved", .SolvedClosed))
                lInfosList.Add(New Comunita_OnLine.InfoItemSimple("closed", .UnSolvedClosed))

                CTRLlistInfo.InitControl(Me.Resource, lInfosList, True, True)
                PNLdetails.Visible = True

                'SPNinfo.Attributes("class") = SPNinfo.Attributes("class").Replace(" empty", "")
            Else
                PNLdetails.Visible = False
                'SPNinfo.Attributes("class") = SPNinfo.Attributes("class") & " empty"
            End If


        End With
    End Sub

    Public Function GetChangeStatusMessage(NewStatus As TK.Domain.Enums.TicketStatus) As String Implements TK.Presentation.View.iViewTicketListMan.GetChangeStatusMessage
        Return Resource.getValue("ChangeStatus.message").Replace("{newstatus}", Resource.getValue("Status." & NewStatus.ToString()))
    End Function

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewTicketListMan.ShowNoPermission
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(Me.ViewCommunityId))
    End Sub

    Public Sub DisableAddNew() Implements TK.Presentation.View.iViewTicketListMan.DisableAddNew

        HYPaddTop.Visible = False
        HYPaddBot.Visible = False

    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

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
                               ByVal OrderValue As TK.Domain.Enums.TicketOrderManRes, _
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
    ' ''' <summary>
    ' ''' Imposta un literal per le info sul numero dei ticket
    ' ''' </summary>
    ' ''' <param name="Lit"></param>
    ' ''' <param name="num"></param>
    ' ''' <remarks></remarks>
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

    Private Sub RptTicket_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RptTicket.ItemCommand

        Dim Code As Int64 = 0
        Try
            Code = System.Convert.ToInt64(e.CommandArgument)
        Catch ex As Exception
        End Try

        Dim Status As TK.Domain.Enums.TicketStatus = TK.Domain.Enums.TicketStatus.draft
        Dim IsManager As Boolean = False

        Select Case e.CommandName
            Case "Reorder"
                Me.ReorderString = e.CommandArgument
                Me.CurrentPresenter.BindTable(Me.GetFilters())
                Return
            Case "Reopen"
                Status = TK.Domain.Enums.TicketStatus.open
            Case "CloseSolved"
                Status = TK.Domain.Enums.TicketStatus.closeSolved
            Case "CloseUnsolved"
                Status = TK.Domain.Enums.TicketStatus.closeUnsolved
            Case "M_Reopen"
                Status = TK.Domain.Enums.TicketStatus.open
                IsManager = True
            Case "M_CloseSolved"
                Status = TK.Domain.Enums.TicketStatus.closeSolved
                IsManager = True
            Case "M_CloseUnsolved"
                Status = TK.Domain.Enums.TicketStatus.closeUnsolved
                IsManager = True
        End Select

        If (Not Status = TK.Domain.Enums.TicketStatus.draft) Then
            Dim UserType As TK.Domain.Enums.MessageUserType = TK.Domain.Enums.MessageUserType.Resolver
            If IsManager Then
                UserType = TK.Domain.Enums.MessageUserType.Manager
            End If
            'DA FINIRE!!!
            Me.CurrentPresenter.ChangeStatus(Code, Status, UserType)
            Me.CurrentPresenter.BindTable(Me.GetFilters())
        End If
    End Sub

    Private Sub RptTicket_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RptTicket.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then

            TicketHelper.SetRPTLiteral(e.Item, "LTtblSelAll_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTtblDeSelAll_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTcode_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTsubject_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTlang_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTcommunity_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTass_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTcategory_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTsub_t", Me.Resource)
            TicketHelper.SetRPTLiteral(e.Item, "LTstatus_t", Me.Resource)
            'TicketHelper.SetRPTLiteral(e.Item, "LTact_t", Me.Resource)

            SetRPTorderLink(e.Item, "LNBorderByIdUp", TK.Domain.Enums.TicketOrderManRes.code, True)
            SetRPTorderLink(e.Item, "LNBorderByIdDown", TK.Domain.Enums.TicketOrderManRes.code, False)

            SetRPTorderLink(e.Item, "LNBorderBySubjUp", TK.Domain.Enums.TicketOrderManRes.subject, True)
            SetRPTorderLink(e.Item, "LNBorderBySubjDown", TK.Domain.Enums.TicketOrderManRes.subject, False)

            SetRPTorderLink(e.Item, "LNBorderByLangUp", TK.Domain.Enums.TicketOrderManRes.language, True)
            SetRPTorderLink(e.Item, "LNBorderByLangDown", TK.Domain.Enums.TicketOrderManRes.language, False)

            SetRPTorderLink(e.Item, "LNBorderByComUp", TK.Domain.Enums.TicketOrderManRes.community, True)
            SetRPTorderLink(e.Item, "LNBorderByComDown", TK.Domain.Enums.TicketOrderManRes.community, False)

            SetRPTorderLink(e.Item, "LNBorderByAssUp", TK.Domain.Enums.TicketOrderManRes.association, True)
            SetRPTorderLink(e.Item, "LNBorderByAssDown", TK.Domain.Enums.TicketOrderManRes.association, False)

            SetRPTorderLink(e.Item, "LNBorderByCatUp", TK.Domain.Enums.TicketOrderManRes.category, True)
            SetRPTorderLink(e.Item, "LNBorderByCatDown", TK.Domain.Enums.TicketOrderManRes.category, False)

            SetRPTorderLink(e.Item, "LNBorderBySubmUp", TK.Domain.Enums.TicketOrderManRes.lifeTime, True)
            SetRPTorderLink(e.Item, "LNBorderBySubmDown", TK.Domain.Enums.TicketOrderManRes.lifeTime, False)

            SetRPTorderLink(e.Item, "LNBorderByStatUp", TK.Domain.Enums.TicketOrderManRes.status, True)
            SetRPTorderLink(e.Item, "LNBorderByStatDown", TK.Domain.Enums.TicketOrderManRes.status, False)

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

            Dim Ticket As TK.Domain.DTO.DTO_TicketListItemManager = e.Item.DataItem
            If Not IsNothing(Ticket) Then
                With Ticket

                    'TITLE
                    Dim TxtStatus As String = Resource.getValue("Status." & .Status.ToString())

                    If Not .Condition = TK.Domain.Enums.TicketCondition.active Then

                        TxtStatus &= " (" & Resource.getValue("Condition." & .Condition.ToString()) & ")"
                        
                    End If

                    Dim showCategoryExtraInfo As Boolean = (.CategoryFilter = TK.Domain.Enums.CategoryFilterStatus.Creation OrElse .CategoryFilter = TK.Domain.Enums.CategoryFilterStatus.History)
                    
                    Dim trItem As System.Web.UI.HtmlControls.HtmlTableRow = e.Item.FindControl("trItem")
                    If Not IsNothing(trItem) Then

                        
                        ' CSS CLASS
                        Dim CssClass As String = ""

                        If .HasNews Then
                            CssClass &= " hasnews"
                        End If
                        If .IsForManager Then
                            CssClass &= " ismanager"
                        End If
                        If .HasNoAnswers Then
                            CssClass &= " noanswers"
                        End If

                        If Not (.Condition = TK.Domain.Enums.TicketCondition.active) Then
                            Select Case .Condition
                                Case TK.Domain.Enums.TicketCondition.blocked
                                    CssClass &= " islocked"
                                Case TK.Domain.Enums.TicketCondition.cancelled
                                    CssClass &= " isdeleted"
                                Case TK.Domain.Enums.TicketCondition.flagged
                                    CssClass &= " isreported"
                            End Select
                        End If

                        If showCategoryExtraInfo Then
                            CssClass &= " filtercategory"
                        End If


                        trItem.Attributes.Add("class", CssClass)

                        trItem.Attributes.Add("title", TxtStatus)

                    End If

                    Dim tdSubject As System.Web.UI.HtmlControls.HtmlTableCell = e.Item.FindControl("tdSubject")

                    If Not IsNothing(tdSubject) Then
                        If .IsAsgnToMe Then
                            tdSubject.Attributes.Add("class", "subject mine")
                        Else
                            tdSubject.Attributes.Add("class", "subject")
                        End If
                    End If

                    TicketHelper.SetRPTLiteral(e.Item, "LTcode", .Code)
                    TicketHelper.SetRPTLiteral(e.Item, "LTlang", .LangCode)
                    TicketHelper.SetRPTLiteral(e.Item, "LTcom", .CommunityName)

                    Dim CategoryText As String = .LastAssignment.AssignedCategory.GetTranslatedName(MyBase.LinguaCode)

                    If showCategoryExtraInfo Then
                        CategoryText &= "<span class=""filtercategory"">*</span>"
                    End If

                    TicketHelper.SetRPTLiteral(e.Item, "LTcurCate", CategoryText)
                    TicketHelper.SetRPTLiteral(e.Item, "LTsendedOn", .SendedOn, TicketHelper.DateTimeMode.DateAndTime)


                    Dim LITassUsr As Literal = e.Item.FindControl("LTassUsr")
                    If Not IsNothing(LITassUsr) Then
                        If .IsAsgnToMe Then
                            LITassUsr.Text = LITassUsr.Text.Replace("{class}", "mine")
                        Else
                            LITassUsr.Text = LITassUsr.Text.Replace("{class}", "")
                        End If

                        If (String.IsNullOrEmpty(.LastAssignment.AssignetToSurNameAndName)) Then
                            LITassUsr.Text = LITassUsr.Text.Replace("{text}", Resource.getValue("AssignmentTo.None"))
                        Else
                            LITassUsr.Text = LITassUsr.Text.Replace("{text}", .LastAssignment.AssignetToSurNameAndName)
                        End If
                    End If

                    Dim HYPsubject As HyperLink = e.Item.FindControl("HYPsubject")
                    If Not IsNothing(HYPsubject) Then
                        HYPsubject.Text = .Title
                        HYPsubject.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditResolver(Me.ViewCommunityId, .Code)
                    End If

                    Dim LITattach As Literal = e.Item.FindControl("LTattach")
                    If Not IsNothing(LITattach) Then
                        If (.NumAttachments > 0) Then
                            LITattach.Text = LITattach.Text.Replace("{text}", Resource.getValue("Attachments.text").Replace("{num}", .NumAttachments.ToString()))
                        Else
                            LITattach.Text = ""
                        End If

                    End If

                    Dim LITstatus As Literal = e.Item.FindControl("LTstatus")
                    If Not IsNothing(LITstatus) Then
                        LITstatus.Text = LITstatus.Text.Replace("{title}", "").Replace("{text}", TxtStatus)
                    End If

                    Dim LKBassign As LinkButton = e.Item.FindControl("LNBassign")
                    If Not IsNothing(LKBassign) Then
                        Resource.setLinkButton(LKBassign, True, True, False, False)
                        LKBassign.Visible = LKBassign.Visible And .IsForManager

                    End If

                    Dim HYPedit As HyperLink = e.Item.FindControl("HYPedit")
                    If Not IsNothing(HYPedit) Then
                        Resource.setHyperLink(HYPedit, True, True)
                        HYPedit.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditResolver(Me.ViewCommunityId, .Code)
                    End If

                    Dim LKBreopen As LinkButton = e.Item.FindControl("LNBreopen")
                    If Not IsNothing(LKBreopen) Then
                        If (.Status = TK.Domain.Enums.TicketStatus.closeSolved OrElse .Status = TK.Domain.Enums.TicketStatus.closeUnsolved) Then
                            Resource.setLinkButton(LKBreopen, True, True)
                            LKBreopen.Visible = True
                            LKBreopen.CommandArgument = .Id
                            If (.IsForManager) Then
                                LKBreopen.CommandName = "M_Reopen"
                            Else
                                LKBreopen.CommandName = "Reopen"
                            End If

                        Else
                            LKBreopen.Visible = False
                        End If
                    End If

                    Dim LKBcloseSolved As LinkButton = e.Item.FindControl("LNBcloseSolved")
                    If Not IsNothing(LKBcloseSolved) Then
                        If (.Status = TK.Domain.Enums.TicketStatus.closeSolved OrElse .Status = TK.Domain.Enums.TicketStatus.closeUnsolved) Then
                            LKBcloseSolved.Visible = False
                        Else
                            Resource.setLinkButton(LKBcloseSolved, True, True)
                            LKBcloseSolved.Visible = True
                            LKBcloseSolved.CommandArgument = .Id
                            If (.IsForManager) Then
                                LKBcloseSolved.CommandName = "M_CloseSolved"
                            Else
                                LKBcloseSolved.CommandName = "CloseSolved"
                            End If

                        End If
                    End If

                    Dim LKBcloseUnSolved As LinkButton = e.Item.FindControl("LNBcloseUnSolved")
                    If Not IsNothing(LKBcloseUnSolved) Then
                        If (.Status = TK.Domain.Enums.TicketStatus.closeSolved OrElse .Status = TK.Domain.Enums.TicketStatus.closeUnsolved) Then
                            LKBcloseUnSolved.Visible = False
                        Else
                            Resource.setLinkButton(LKBcloseUnSolved, True, True)
                            LKBcloseUnSolved.Visible = True
                            LKBcloseUnSolved.CommandArgument = .Id
                            If (.IsForManager) Then
                                LKBcloseUnSolved.CommandName = "M_CloseUnsolved"
                            Else
                                LKBcloseUnSolved.CommandName = "CloseUnsolved"
                            End If
                        End If
                    End If

                End With
            End If

        End If

    End Sub
    Private Sub LNBfilter_Click(sender As Object, e As System.EventArgs) Handles LNBfilter.Click
        Me.CurrentPresenter.BindTable(GetFilters())
    End Sub

#End Region

    Private Sub CTRLlistInfo_SendAction(Action As String) Handles CTRLlistInfo.SendAction

        'Reset Filtro Categoria ed altri filtri...
        Me.CurrentPresenter.ReBindDDLCategory(-1)

        Select Case Action
            Case "draft"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.draft.ToString()
            Case "open"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.open.ToString()
            Case "inprogress"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.inProcess.ToString()
            Case "request"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.userRequest.ToString()
            Case "solved"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.closeSolved.ToString()
            Case "closed"
                Me.DDLstatus.SelectedValue = TK.Domain.Enums.TicketStatus.closeUnsolved.ToString()
        End Select

        'Reset Altri filtri
        Me.TXBsubject.Text = ""
        Me.CBXonlyNoAnswers.Checked = False
        Me.CBXonlyNotAssigned.Checked = False
        Me.CBXonlyWithNews.Checked = False
        Me.DDLlang.SelectedValue = "all"

        'Me.CBXStart.Checked = False
        'Me.CBXend.Checked = False

        Me.RDPstart.SelectedDate = Nothing
        Me.RDPend.SelectedDate = Nothing

        Me.CurrentPresenter.BindTable(GetFilters())
    End Sub

    Private Sub PGgridBot_OnPageSelected() Handles PGgridBot.OnPageSelected
        Me.CurrentPageIndex = PGgridBot.Pager.PageIndex
        Me.CurrentPageSize = PGgridBot.Pager.PageSize

        Me.CurrentPresenter.BindTable(GetFilters())

    End Sub

    Public Function GetActionTitle() As String
        Return Resource.getValue("TH.Action.Title")
    End Function


End Class