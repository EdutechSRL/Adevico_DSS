Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class ViewGlossary
    Inherits GLpageBase
    Implements IViewGlossaryView

    Private Const termDeleteCommand As String = "termDeleteCommand"

#Region "Context"

    Private _Presenter As GlossaryViewPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryViewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryViewPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property


    ''' <summary>
    '''     Ultima Lettera Mostrata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property LastChar() As String

    Public Property ChildList As List(Of DTO_Term)
        Get
            Return Me.ViewStateOrDefault("ChildList", New List(Of DTO_Term))
        End Get
        Set(value As List(Of DTO_Term))
            Me.ViewState("ChildList") = value
        End Set
    End Property


    ''' <summary>
    '''     Ultima Lettera Filtrata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FilterLetter As String
        Get
            Return Me.ViewStateOrDefault("FilterLetter", " ")
        End Get
        Set(value As String)
            Me.ViewState("FilterLetter") = value
        End Set
    End Property


    ''' <summary>
    '''     Controllo Pager
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Pager As PagerBase
        Get
            Try
                Return Me.ViewState("pager")
            Catch ex As Exception
            End Try
            Return Nothing
        End Get
        Set(value As PagerBase)
            Me.ViewState("pager") = value
        End Set
    End Property


    ''' <summary>
    '''     Dimensione Pagina
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Property CurrentPageSize As Integer
        Get
            Dim CPS As Integer = 25
            Try
                CPS = Convert.ToInt32(ViewStateOrDefault("CurrentPageSize", 25))
            Catch ex As Exception

            End Try
            Return CPS
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property

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

    Private Property CurrentCharInfo As CharInfo
        Get
            Return ViewStateOrDefault("CurrentCharInfo", New CharInfo())
        End Get
        Set(value As CharInfo)
            ViewState("CurrentCharInfo") = value
        End Set
    End Property


    ''' <summary>
    '''     Glossario Corrente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Property Glossary As DTO_Glossary
        Get
            Return ViewStateOrDefault("Glossary", New DTO_Glossary())
        End Get
        Set(value As DTO_Glossary)
            ViewState("Glossary") = value
        End Set
    End Property

    Private Property WordUsingDictionary As Dictionary(Of Char, UInt16)
        Get
            Return ViewStateOrDefault("WordUsingDictionary", New Dictionary(Of Char, UInt16)())
        End Get
        Set(value As Dictionary(Of Char, UInt16))
            ViewState("WordUsingDictionary") = value
        End Set
    End Property

    Public Property IdTerm() As Long Implements IViewPageBase.IdTerm

    Public Property _IdCookies As String
        Get
            Return Me.ViewStateOrDefault("IdCookies", " ")
        End Get
        Set(value As String)
            Me.ViewState("IdCookies") = value
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        PGgrid.Pager = Pager
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryView.SetTitle
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        Master.DisplayTitleRow = False
        With Resource
            .setHyperLink(HYPglossaryList, False, False, False, False)
            .setHyperLink(HYPRecycleBin, False, False, False, False)
            .setHyperLink(HYPtermAdd, False, False, False, False)
            .setHyperLink(HYPmanageShareGlossary, False, False, False, False)

            .setLabel(LBclickExpand_t)
            .setLabel(LBclickCollapse_t)
            .setLabel(LBlemma_t)
            .setLabel(LBlemmacontent_t)
            .setLabel(LBstatus_t)
            .setLinkButton(LNBreset, True, True, False, False)
            .setLinkButton(LNBsearchApply, True, True, False, False)
            .setLinkButton(LNBsearch, True, True, False, False)

            .setLiteral(LTsearchTools_t)
            .setLabel(LBchangeViewStyle_t)

            HYPviewMapList.ToolTip = GetViewAsStackedListText()
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

#Region "Update Data"

    Public Sub LoadViewData(ByVal glossary As DTO_Glossary, ByVal words As Dictionary(Of String, CharInfo), ByVal fromIdCommunity As Int32, ByVal manageEnabled As Boolean, ByVal manage As Boolean, ByVal loadFromCookies As Boolean, ByVal idCookies As String, ByVal page As Int32) Implements IViewGlossaryView.LoadViewData
        Me.Glossary = glossary
        If glossary IsNot Nothing Then
            LTglossaryTitle.Text = glossary.Name
        End If
        Me.WordUsingDictionary = WordUsingDictionary
        'Init Buttons
        HYPglossaryList.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(fromIdCommunity, glossary.Id)
        HYPtermAdd.Visible = glossary.Permission.AddTerm
        HYPtermAdd.NavigateUrl = ApplicationUrlBase & RootObject.TermAdd(glossary.Id, fromIdCommunity, False)

        HYPRecycleBin.Visible = glossary.Permission.DeleteTerm
        HYPRecycleBin.NavigateUrl = ApplicationUrlBase & RootObject.RecycleBinTerms(fromIdCommunity, glossary.Id, False)


        If manageEnabled AndAlso glossary.IsShared AndAlso Not glossary.IdCommunity = fromIdCommunity Then
            HYPmanageShareGlossary.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(glossary.Id, fromIdCommunity, True, Not manage)
        Else
            HYPmanageShareGlossary.Visible = False
        End If

        FilterLetter = String.Empty

        If glossary.TermsPerPage < 5 Then
            CurrentPageSize = 5
        Else
            CurrentPageSize = glossary.TermsPerPage
        End If

        CTRLalphabetSelector.InitializeControl(words.Keys.ToList(), FilterLetter)
        SSshareState.InitializeControl(fromIdCommunity, glossary.Id, manage)

        LTpageTitle_t.Text = glossary.Name

        DDLsearchType.Items.Clear()
        DDLsearchType.DataSource = GetDDLElements(GetType(FilterTypeEnum))
        DDLsearchType.DataTextField = "Description"
        DDLsearchType.DataValueField = "Code"
        DDLsearchType.DataBind()

        DDLsearchType.SelectedIndex = 0

        DDLsearchVisibility.Items.Clear()
        DDLsearchVisibility.DataSource = GetDDLElements(GetType(FilterVisibilityTypeEnum))
        DDLsearchVisibility.DataTextField = "Description"
        DDLsearchVisibility.DataValueField = "Code"
        DDLsearchVisibility.DataBind()

        DDLsearchVisibility.SelectedIndex = 0
        PGgrid.EnableQueryString = True

        If loadFromCookies Then
            LoadCookies(idCookies)
        Else
            idCookies = Guid.NewGuid().ToString()
        End If
        _IdCookies = idCookies
        Me.IdCommunity = fromIdCommunity
        CurrentPageIndex = page
        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(False))
        HYPviewMapList.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(glossary.Id, fromIdCommunity, True, False, True, idCookies)
        PGgrid.BaseNavigateUrl = ApplicationUrlBase & RootObject.GlossaryViewForMap(glossary.Id, fromIdCommunity, False, False, True, idCookies)
    End Sub

    Public Function GetDDLElements(ByVal enumType As Type) As List(Of DDLElement)
        Dim list As New List(Of DDLElement)
        For Each enumItem As String In [Enum].GetNames(enumType)
            Dim item As New DDLElement
            item.Code = enumItem
            item.Description = Resource.getValue(enumType.Name & "." & enumItem)
            list.Add(item)
        Next
        Return list
    End Function

    Public ReadOnly Property HyPglossaryListProperty() As HyperLink
        Get
            Return HYPglossaryList
        End Get
    End Property

    Private Sub SetPager(ByVal records As Integer, ByVal currentPage As Integer)
        CurrentPageIndex = currentPage
        If (records > 0 And CurrentPageSize > 0) Then
            Dim Cur_Pager As New PagerBase()
            If (records > 0) Then
                Cur_Pager.Count = records - 1
            Else
                Cur_Pager.Count = records
            End If

            Cur_Pager.PageSize = CurrentPageSize    'Dimensione pagina
            Cur_Pager.PageIndex = CurrentPageIndex  'Indice corrente

            Pager = Cur_Pager
            PGgrid.Visible = True
            PGgrid.Pager = Cur_Pager

        Else
            CurrentPageIndex = 0
            PGgrid.Visible = False
        End If

        If currentPage = 0 And records < CurrentPageSize Then
            PGgrid.Visible = False
        End If
    End Sub

    'IEnumerable<DTO_Term> termList, char letter, Int32 records, Int32 currentPage
    Public Sub BindRepeaterList(ByVal termList As IEnumerable(Of DTO_Term), ByVal words As Dictionary(Of String, CharInfo), ByVal letter As Char, ByVal records As Int32, ByVal currentPage As Int32, ByVal letters As List(Of CharInfo)) Implements IViewGlossaryView.BindRepeaterList
        ChildList = termList.ToList()
        SetPager(records, currentPage)

        For index As Integer = 0 To letters.Count - 1
            Dim item As CharInfo = letters(index)
            If (index = 0) Then
                item.CssClass = "first"
            ElseIf (index = letters.Count - 1) Then
                item.CssClass = "last"
            End If
        Next

        RPTlist.DataSource = letters
        RPTlist.DataBind()

        If ChildList.Count() = 0 Then
            SetMessage(Resource.getValue("no_terms"), MessageType.norecords)
        Else
            SetMessage(String.Empty, MessageType.none)
        End If

        CTRLalphabetSelector.InitializeControl(words.Keys.ToList(), FilterLetter)
    End Sub

    Private Sub LNBsearch_Click(sender As Object, e As EventArgs) Handles LNBsearch.Click
        FilterLetter = String.Empty
        CurrentPageIndex = 0
        TXBsearchLemma.Text = TXBsearch.Text

        If Not String.IsNullOrWhiteSpace(TXBsearch.Text) Or Not String.IsNullOrWhiteSpace(TXBsearchLemma.Text) Then
            CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(True))
        Else
            CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, Nothing)
        End If
    End Sub

    Private Sub LNBsearchApply_Click(sender As Object, e As EventArgs) Handles LNBsearchApply.Click
        FilterLetter = String.Empty
        CurrentPageIndex = 0
        TXBsearch.Text = TXBsearchLemma.Text

        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(False))
    End Sub

    Public Function CreateFilter(ByVal simple As Boolean) As GlossaryFilter
        Dim filter As New GlossaryFilter

        If simple Then
            filter.LemmaString = TXBsearch.Text
            filter.LemmaContentString = String.Empty
            filter.LemmaSearchType = 0
            filter.LemmaVisibilityType = 0
        Else
            filter.LemmaString = TXBsearchLemma.Text
            filter.LemmaContentString = TXBsearchLemmaContent.Text
            filter.LemmaSearchType = DDLsearchType.SelectedIndex
            filter.LemmaVisibilityType = DDLsearchVisibility.SelectedIndex
        End If

        SaveCookies(filter, _IdCookies)
        Return filter
    End Function

    Private Sub SaveCookies(ByVal filter As GlossaryFilter, ByVal id As String)
        Dim key As String = String.Format("comol_glossarysearch_{0}", id)

        Dim myCookie As New HttpCookie(key)
        myCookie.Expires = DateTime.Now.AddHours(2)

        myCookie("lemma") = filter.LemmaString
        myCookie("definition") = filter.LemmaContentString
        myCookie("searchType") = filter.LemmaSearchType
        myCookie("searchVisibility") = filter.LemmaVisibilityType

        If Request.Cookies.AllKeys.Contains(key) Then
            Response.Cookies.Set(myCookie)
        Else
            Response.Cookies.Add(myCookie)
        End If
    End Sub

    Private Sub ClearCookies(ByVal id As String)
        Dim key As String = String.Format("comol_glossarysearch_{0}", id)

        Dim myCookie As New HttpCookie(key)
        myCookie.Expires = DateTime.Now.AddHours(2)

        myCookie("lemma") = String.Empty
        myCookie("definition") = String.Empty
        myCookie("searchType") = 0
        myCookie("searchVisibility") = 0

        If Request.Cookies.AllKeys.Contains(key) Then
            Response.Cookies.Set(myCookie)
        Else
            Response.Cookies.Add(myCookie)
        End If
    End Sub

    Private Sub LoadCookies(ByVal id As String)
        Dim key As String = String.Format("comol_glossarysearch_{0}", id)
        Dim myCookie As HttpCookie = Request.Cookies(key)
        If Not IsNothing(myCookie) Then
            TXBsearch.Text = myCookie("lemma")
            TXBsearchLemma.Text = myCookie("lemma")
            TXBsearchLemmaContent.Text = myCookie("definition")
            DDLsearchType.SelectedIndex = CInt(myCookie("searchType"))
            DDLsearchVisibility.SelectedIndex = CInt(myCookie("searchVisibility"))
        End If
    End Sub

#End Region

#Region "Events"

    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        FilterLetter = letter
        CurrentPageIndex = 0
        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(False))
    End Sub

    Private Sub RPTlist_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTlist.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            CurrentCharInfo = e.Item.DataItem

            Dim ltLetter As Literal = e.Item.FindControl("LTletter")
            If Not IsNothing(ltLetter) Then
                ltLetter.Text = CurrentCharInfo.CurrentChar.ToUpper()
            End If

            Dim LTletterTop As Literal = e.Item.FindControl("LTletterTop")
            If Not IsNothing(LTletterTop) Then
                LTletterTop.Text = CurrentCharInfo.CurrentChar.ToUpper()
            End If

            Dim PNLLetterTop As Panel = e.Item.FindControl("PNLLetterTop")
            If Not IsNothing(PNLLetterTop) Then
                PNLLetterTop.Visible = CurrentCharInfo.HasWordInPreviousPage
            End If

            Dim LTmoreTerms As Literal = e.Item.FindControl("LTmoreTerms")
            If Not IsNothing(LTmoreTerms) Then
                Resource.setLiteral(LTmoreTerms)
            End If

            Dim LTonPreviousPageTop As Literal = e.Item.FindControl("LTonPreviousPageTop")
            If Not IsNothing(LTonPreviousPageTop) Then
                Resource.setLiteral(LTonPreviousPageTop)
            End If

            Dim HPYPreviuosPage As HyperLink = e.Item.FindControl("HPYPreviuosPage")
            If Not IsNothing(HPYPreviuosPage) Then
                HPYPreviuosPage.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryViewPage(Glossary.Id, Me.IdCommunity, False, False, True, IdCookies, CurrentPageIndex - 1)
            End If

            Dim RPTterm As Repeater = e.Item.FindControl("RPTterm")
            If Not IsNothing(RPTterm) Then
                AddHandler RPTterm.ItemDataBound, AddressOf RPTterm_ItemDataBound
                AddHandler RPTterm.ItemCommand, AddressOf RPTterm_ItemCommand
                Dim list As List(Of DTO_Term) = ChildList.Where(Function(f) f.FirstLetter = CurrentCharInfo.FirstLetter).OrderBy(Function(f) f.Name).ToList()

                For index As Integer = 0 To list.Count - 1
                    Dim item As DTO_Term = list(index)
                    If (index = 0) Then
                        item.CssClass = "first"
                    ElseIf (index = list.Count - 1) Then
                        item.CssClass = "last"
                    End If
                Next

                RPTterm.DataSource = list
                RPTterm.DataBind()
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            'Dim charInfo As CharInfo = e.Item.DataItem
            'Dim LTletterBottom As Literal = e.Item.FindControl("LTletterBottom")
            'If Not IsNothing(LTletterBottom) Then
            '    LTletterBottom.Text = charInfo.CurrentChar.ToUpper()
            'End If
        End If
    End Sub

    Private Sub RPTterm_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoTerm As DTO_Term = e.Item.DataItem

            Dim LTterm As Literal = e.Item.FindControl("LTterm")
            If Not IsNothing(LTterm) Then
                LTterm.Text = dtoTerm.Name
            End If

            Dim LTinfotag As Literal = e.Item.FindControl("LTinfotag")
            If Not IsNothing(LTinfotag) AndAlso Not dtoTerm.IsPublished Then
                LTinfotag.Text = "*Unpublished"
            End If

            Dim LTdescription As Literal = e.Item.FindControl("LTdescription")
            If Not IsNothing(LTdescription) Then
                LTdescription.Text = dtoTerm.Description
            End If

            Dim LTauthor As Literal = e.Item.FindControl("LTauthor")
            If Not IsNothing(LTauthor) Then
                LTauthor.Text = dtoTerm.ModifiedBy
            End If

            Dim LTlastUpdate As Literal = e.Item.FindControl("LTlastUpdate")
            If Not IsNothing(LTlastUpdate) Then
                LTlastUpdate.Text = dtoTerm.ModifiedOn.ToShortDateString()
            End If

            Dim LNBviewTerm As LinkButton = e.Item.FindControl("LNBviewTerm")
            If Not IsNothing(LNBviewTerm) Then
                Me.Resource.setLinkButton(LNBviewTerm, True, True, False, False)
                LNBviewTerm.CommandName = "ViewTerm"
                LNBviewTerm.CommandArgument = dtoTerm.Id
            End If

            Dim LBtermUnpublished As Label = e.Item.FindControl("LBtermUnpublished")
            If Not IsNothing(LBtermUnpublished) Then
                Resource.setLabel(LBtermUnpublished)
                LBtermUnpublished.Visible = Not dtoTerm.IsPublished
            End If

            Dim LNBvirtualDeleteTerm As LinkButton = e.Item.FindControl("LNBvirtualDeleteTerm")
            If Not IsNothing(LNBvirtualDeleteTerm) Then
                LNBvirtualDeleteTerm.Visible = Glossary.Permission.DeleteTerm
                Resource.setLinkButton(LNBvirtualDeleteTerm, True, True, False, True)
                LNBvirtualDeleteTerm.CommandName = termDeleteCommand
                LNBvirtualDeleteTerm.CommandArgument = dtoTerm.Id
            End If

            Dim HYPeditTerm As HyperLink = e.Item.FindControl("HYPeditTerm")
            If Not IsNothing(HYPeditTerm) Then
                HYPeditTerm.Visible = Glossary.Permission.EditTerm
                HYPeditTerm.NavigateUrl = ApplicationUrlBase & RootObject.TermEdit(dtoTerm.IdGlossary, dtoTerm.Id, IdCommunity, 1, True, _IdCookies, CurrentPageIndex)
                Resource.setHyperLink(HYPeditTerm, True, True)
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim LTletterBottom As Literal = e.Item.FindControl("LTletterBottom")
            If Not IsNothing(LTletterBottom) Then
                LTletterBottom.Text = CurrentCharInfo.CurrentChar.ToUpper()
            End If
            Dim PNLLetterBottom As Panel = e.Item.FindControl("PNLLetterBottom")
            If Not IsNothing(PNLLetterBottom) Then
                PNLLetterBottom.Visible = CurrentCharInfo.HasWordInNextPage
            End If

            Dim LTmoreTermsBottom As Literal = e.Item.FindControl("LTmoreTermsBottom")
            If Not IsNothing(LTmoreTermsBottom) Then
                Resource.setLiteral(LTmoreTermsBottom)
            End If

            Dim LTonNextPageBottom As Literal = e.Item.FindControl("LTonNextPageBottom")
            If Not IsNothing(LTonNextPageBottom) Then
                Resource.setLiteral(LTonNextPageBottom)
            End If

            Dim HPYNextPage As HyperLink = e.Item.FindControl("HPYNextPage")
            If Not IsNothing(HPYNextPage) Then
                HPYNextPage.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryViewPage(Glossary.Id, Me.IdCommunity, False, False, True, IdCookies, CurrentPageIndex + 1)
            End If


        End If
    End Sub

    Public Sub RPTterm_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim idTerm As Int64 = Convert.ToInt64(e.CommandArgument)
        If idTerm > 0 Then
            Select Case e.CommandName
                Case termDeleteCommand
                    Dim errors As String
                    If CurrentPresenter.VirtualDeleteTerm(idTerm, errors) Then
                        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(False))
                    End If
                Case Else

            End Select
        End If
    End Sub

    Private Sub PGgridBot_OnPageSelected() Handles PGgrid.OnPageSelected
        CurrentPageIndex = PGgrid.Pager.PageIndex
        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, CreateFilter(False))
    End Sub

#End Region

#End Region

    Public Function GetTileClass(ByVal id As Long) As String
        Dim dto_term As DTO_Term = ChildList.FirstOrDefault(Function(f) f.Id = id)
        If dto_term IsNot Nothing Then
            If Not dto_term.IsPublished Then
                Return " unpublished "
            End If
        End If
        Return String.Empty
    End Function

    Private Sub LNBreset_Click(sender As Object, e As EventArgs) Handles LNBreset.Click
        TXBsearchLemma.Text = String.Empty
        TXBsearch.Text = String.Empty
        TXBsearchLemmaContent.Text = String.Empty
        DDLsearchType.SelectedIndex = 0
        DDLsearchVisibility.SelectedIndex = 0
        FilterLetter = String.Empty
        CurrentPageIndex = 0
        ClearCookies(_IdCookies)
        CurrentPresenter.ChangeLetter(FilterLetter, CurrentPageIndex, CurrentPageSize, Nothing)
    End Sub

    Public Function GetViewAsStackedListText() As String
        'View as stacked list
        Return Resource.getValue("ViewAsStackedList_t")
    End Function

    Public Function GetViewAsListText() As String
        'View as stacked list
        Return Resource.getValue("ViewAsList_t")
    End Function

    Public Sub SetMessage(ByVal message As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, type)
        End If
    End Sub

    Protected Function HasAttachments() As String
        Return "attachments noattachments"
    End Function
End Class

Public Class DDLElement
    Property Code() As String
    Property Description() As String
End Class