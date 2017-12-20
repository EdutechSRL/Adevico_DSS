Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class ViewGlossaryMap
    Inherits GLpageBase
    Implements IViewGlossaryMapView

    Private Const termDeleteCommand As String = "termDeleteCommand"

#Region "Context"

    Private _Presenter As GlossaryMapViewPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryMapViewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryMapViewPresenter(PageUtility.CurrentContext, Me)
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

    Public Property ChildList As List(Of DTO_TermMap)
        Get
            Return Me.ViewStateOrDefault("ChildList", New List(Of DTO_TermMap))
        End Get
        Set(value As List(Of DTO_TermMap))
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

    Public Property _IdCookies As String
        Get
            Return Me.ViewStateOrDefault("IdCookies", " ")
        End Get
        Set(value As String)
            Me.ViewState("IdCookies") = value
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

    Private Property FromIdCommunity As Int32
        Get
            Return ViewStateOrDefault("FromIdCommunity", 0)
        End Get
        Set(value As Int32)
            ViewState("FromIdCommunity") = value
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

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryMapView.SetTitle
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

            .setLabel(LBchangeViewStyle_t)

            .setLiteral(LTlist_t)
            .setLiteral(LTstack_t)

            .setLiteral(LTsearchTools_t)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

#Region "Update Data"

    Public Sub LoadViewData(ByVal glossary As DTO_Glossary, ByVal words As List(Of String), ByVal fromIdCommunity As Int32, ByVal manageEnabled As Boolean, ByVal manage As Boolean, ByVal loadFromCookies As Boolean, ByVal idCookies As String) Implements IViewGlossaryMapView.LoadViewData
        Me.Glossary = glossary
        IdCommunity = fromIdCommunity

        If glossary IsNot Nothing Then
            LTglossaryTitle.Text = glossary.Name
        End If
        WordUsingDictionary = WordUsingDictionary
        'Init Buttons
        HYPglossaryList.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(IdCommunity, glossary.Id)

        HYPtermAdd.Visible = glossary.Permission.AddTerm
        HYPRecycleBin.Visible = glossary.Permission.DeleteTerm

        HYPtermAdd.NavigateUrl = ApplicationUrlBase & RootObject.TermAdd(glossary.Id, fromIdCommunity, True)
        HYPRecycleBin.NavigateUrl = ApplicationUrlBase & RootObject.RecycleBinTerms(fromIdCommunity, glossary.Id, True)

        If manageEnabled AndAlso glossary.IsShared AndAlso Not glossary.IdCommunity = fromIdCommunity Then
            HYPmanageShareGlossary.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(glossary.Id, IdCommunity, True, Not manage)
        Else
            HYPmanageShareGlossary.Visible = False
        End If

        FilterLetter = String.Empty
        CTRLalphabetSelector.InitializeControl(words, FilterLetter)
        SSshareState.InitializeControl(fromIdCommunity, glossary.Id, manage)

        Master.ServiceTitle = glossary.Name
        Master.ServiceTitleToolTip = glossary.Name
        Master.ServiceNopermission = glossary.Name
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

        If loadFromCookies Then
            LoadCookies(idCookies)
        Else
            idCookies = Guid.NewGuid().ToString()
        End If
        _IdCookies = idCookies
        CurrentPresenter.ChangeLetter(FilterLetter, CreateFilter(False))
        HYPviewList.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(glossary.Id, fromIdCommunity, False, False, True, idCookies)
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

    Public Sub BindRepeaterList(ByVal words As List(Of String), ByVal wordsAll As List(Of String), ByVal termList As IEnumerable(Of DTO_TermMap), ByVal letter As Char) Implements IViewGlossaryMapView.BindRepeaterList
        ChildList = termList.ToList()
        CTRLalphabetSelector.InitializeControl(wordsAll, FilterLetter)

        Dim list As List(Of String) = words.Distinct().ToList()
        list.Sort()

        Dim listSimple As New List(Of SimpleLetter)

        For index As Integer = 0 To list.Count - 1
            Dim item As String = list(index)
            Dim newItem As New SimpleLetter
            newItem.Letter = item
            If (index = 0) Then
                newItem.CssClass = "first"
            ElseIf (index = list.Count - 1) Then
                newItem.CssClass = "last"
            End If
            listSimple.Add(newItem)
        Next

        RPTlist.DataSource = listSimple

        RPTlist.DataBind()

        If ChildList.Count() = 0 Then
            SetMessage(Resource.getValue("no_terms"), MessageType.norecords)
        Else
            SetMessage(String.Empty, MessageType.none)
        End If
    End Sub

    Private Sub LNBsearch_Click(sender As Object, e As EventArgs) Handles LNBsearch.Click
        TXBsearchLemma.Text = TXBsearch.Text
        If Not String.IsNullOrWhiteSpace(TXBsearch.Text) Or Not String.IsNullOrWhiteSpace(TXBsearchLemma.Text) Then
            FilterLetter = String.Empty
            CurrentPresenter.ChangeLetter(FilterLetter, CreateFilter(True))
        Else
            CurrentPresenter.ChangeLetter(FilterLetter, Nothing)
        End If
    End Sub

    Private Sub LNBreset_Click(sender As Object, e As EventArgs) Handles LNBreset.Click
        TXBsearchLemma.Text = String.Empty
        TXBsearch.Text = String.Empty
        TXBsearchLemmaContent.Text = String.Empty
        DDLsearchType.SelectedIndex = 0
        DDLsearchVisibility.SelectedIndex = 0
        FilterLetter = String.Empty
        ClearCookies(_IdCookies)
        CurrentPresenter.ChangeLetter(FilterLetter, Nothing)
    End Sub

    Private Sub LNBsearchApply_Click(sender As Object, e As EventArgs) Handles LNBsearchApply.Click
        TXBsearch.Text = TXBsearchLemma.Text
        FilterLetter = String.Empty
        CurrentPresenter.ChangeLetter(FilterLetter, CreateFilter(False))
    End Sub

    'Public Function CreateFilter() As GlossaryFilter
    '    If String.IsNullOrWhiteSpace(TXBsearchLemma.Text) Then
    '        TXBsearchLemma.Text = TXBsearch.Text
    '    End If
    '    Dim filter As New GlossaryFilter
    '    filter.LemmaString = TXBsearchLemma.Text
    '    filter.LemmaContentString = TXBsearchLemmaContent.Text
    '    filter.LemmaSearchType = DDLsearchType.SelectedIndex
    '    filter.LemmaVisibilityType = DDLsearchVisibility.SelectedIndex
    '    SaveCookies(filter, _IdCookies)
    '    Return filter
    'End Function

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

#End Region

#Region "Events"

    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        FilterLetter = letter
        CurrentPresenter.ChangeLetter(letter, CreateFilter(False))
    End Sub

    Private Sub RPTlist_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTlist.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim myobj As SimpleLetter = e.Item.DataItem

            Dim ltLetter As Literal = e.Item.FindControl("LTletter")
            If Not IsNothing(ltLetter) Then
                ltLetter.Text = myobj.Letter.ToString().ToUpper()
            End If
            Dim RPTterm As Repeater = e.Item.FindControl("RPTterm")
            If Not IsNothing(RPTterm) Then
                AddHandler RPTterm.ItemDataBound, AddressOf RPTterm_ItemDataBound
                AddHandler RPTterm.ItemCommand, AddressOf RPTterm_ItemCommand
                Dim list As List(Of DTO_TermMap) = ChildList.Where(Function(f) f.FirstLetter = myobj.Letter).OrderBy(Function(f) f.Name).ToList()

                For index As Integer = 0 To list.Count - 1
                    Dim item As DTO_TermMap = list(index)
                    If (index = 0) Then
                        item.CssClass = "first"
                    ElseIf (index = list.Count - 1) Then
                        item.CssClass = "last"
                    End If
                Next

                RPTterm.DataSource = list
                RPTterm.DataBind()
            End If
        End If
    End Sub

    Private Sub RPTterm_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim term As DTO_TermMap = e.Item.DataItem

            Dim HYPterm As HyperLink = e.Item.FindControl("HYPterm")
            If Not IsNothing(HYPterm) Then
                HYPterm.Text = term.Name
                HYPterm.NavigateUrl = ApplicationUrlBase & RootObject.TermView(Glossary.Id, term.Id, IdCommunity, True)
            End If

            Dim LBtermUnpublished As Label = e.Item.FindControl("LBtermUnpublished")
            If Not IsNothing(LBtermUnpublished) Then
                Resource.setLabel(LBtermUnpublished)
                LBtermUnpublished.Visible = Not term.IsPublished
            End If

            Dim LNBviewTerm As LinkButton = e.Item.FindControl("LNBviewTerm")
            If Not IsNothing(LNBviewTerm) Then
                Me.Resource.setLinkButton(LNBviewTerm, True, True, False, False)
                LNBviewTerm.CommandName = "ViewTerm"
                LNBviewTerm.CommandArgument = term.Id
            End If

            Dim LNBvirtualDeleteTerm As LinkButton = e.Item.FindControl("LNBvirtualDeleteTerm")
            If Not IsNothing(LNBvirtualDeleteTerm) Then
                LNBvirtualDeleteTerm.Visible = Glossary.Permission.DeleteTerm
                Resource.setLinkButton(LNBvirtualDeleteTerm, True, True, , True)
                LNBvirtualDeleteTerm.CommandName = termDeleteCommand
                LNBvirtualDeleteTerm.CommandArgument = term.Id
            End If

            Dim HYPeditTerm As HyperLink = e.Item.FindControl("HYPeditTerm")
            If Not IsNothing(HYPeditTerm) Then
                HYPeditTerm.Visible = Glossary.Permission.EditTerm
                HYPeditTerm.NavigateUrl = ApplicationUrlBase & RootObject.TermEdit(term.IdGlossary, term.Id, IdCommunity, 2)
                Resource.setHyperLink(HYPeditTerm, True, True)
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
                        CurrentPresenter.ChangeLetter(FilterLetter, CreateFilter(False))
                    End If
                Case Else

            End Select
        End If
    End Sub

    Public Function GetTileClass(ByVal id As Long) As String
        Dim dto_term As DTO_TermMap = ChildList.FirstOrDefault(Function(f) f.Id = id)
        If dto_term IsNot Nothing Then
            If Not dto_term.IsPublished Then
                Return " unpublished "
            End If
        End If
        Return String.Empty
    End Function

#End Region

#End Region

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
End Class

Public Class SimpleLetter
    Property Letter() As Char
    Property CssClass() As String


End Class