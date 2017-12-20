Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation

Public Class PathSummary
    Inherits PageBaseEduPath

    Dim i As List(Of dtoUserPaths)

    Dim _pathcount As Int32
    Public Property PathCount As Int32
        Get
            Return _pathcount
        End Get
        Set(value As Int32)
            _pathcount = value
        End Set
    End Property

    Public ReadOnly Property UniquePath As Boolean
        Get
            Return PathCount = 1
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

    Private Sub PathSummary_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property


    Public Property orole As Int32
        Get
            Return ViewStateOrDefault("selectedRole", 0)
        End Get
        Set(value As Int32)
            ViewState("selectedRole") = value
        End Set
    End Property

    Public Property name As String
        Get
            Return ViewStateOrDefault("username", "")
        End Get
        Set(value As String)
            ViewState("username") = value
        End Set
    End Property

    Public ReadOnly Property CmntId As Int32
        Get
            If (String.IsNullOrEmpty(Request.QueryString("ComId"))) Then
                Return ComunitaCorrenteID
            Else
                Dim _id As Int32 = Integer.Parse(Request.QueryString("ComId"))
                Return _id
            End If

        End Get
    End Property

    Public Overrides Sub BindDati()
        MLVpathsummary.SetActiveView(VIWpathsummary)
        'i = New List(Of dtoUserPaths)

        'Dim o As New dtoUserPaths()
        'o.Person = New Person() With {.Name = "Name", .Surname = "User", .Id = 23}
        'o.NotStarted = 1
        'o.Started = 2
        'o.Completed = 3
        'o.Id = 1

        'i.Add(o)

        PathCount = ServiceEP.CountActivePaths(CmntId)

        Dim c As New COL_Comunita(CmntId)
        Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage)

        LBserviceCommunity.Text = c.EstraiNomeBylingua(LinguaID)

        Dim list As IList(Of dtoUserPaths)
        If Not Page.IsPostBack Then


            Dim l As New List(Of Comol.Entity.Role)

            l = ServiceEP.GetActiveRoles(CmntId, LinguaID)
            l.Add(New Comol.Entity.Role() With {.ID = 0, .Name = Me.Resource.getValue("NONE")})

            l = l.OrderBy(Function(x) x.Name).ToList()



            DDLroleFilter.DataSource = l
            DDLroleFilter.DataValueField = "ID"
            DDLroleFilter.DataTextField = "Name"
            DDLroleFilter.DataBind()

            Dim p As New PagerBase(20, 0)



            Try
                If (Not String.IsNullOrEmpty(Request.QueryString("RId"))) Then
                    orole = Int32.Parse(Request.QueryString("RId"))
                    DDLroleFilter.SelectedValue = orole
                End If
            Catch ex As Exception

            End Try
            Try
                If (Not String.IsNullOrEmpty(Request.QueryString("Search"))) Then
                    name = Server.UrlDecode(Request.QueryString("Search"))
                    TXBuserFilter.Text = name
                End If
            Catch ex As Exception

            End Try

            'p.PageSize = 61
            'p.FirstPage = 1
            p.Count = Me.ServiceStat.CountUserPathsCount(CmntId, orole, name) - 1
            p.initialize()

            Try
                If (Not String.IsNullOrEmpty(Request.QueryString("Page"))) Then
                    p.PageIndex = Int32.Parse(Request.QueryString("Page"))
                End If
            Catch ex As Exception

            End Try
            Pager = p
        Else
            Pager.Count = Me.ServiceStat.CountUserPathsCount(CmntId, orole, name) - 1
            'Pager.initialize()
        End If

        list = Me.ServiceStat.GetUserPathsCount(CmntId, orole, name, Me.Pager.PageIndex, Me.Pager.PageSize, CHBshowall.Checked)

        If UniquePath Then
            DIVinfo.Visible = True
            'LBpathName.Text = list.First().Paths.First().PathName
            Try
                LBpathName.Text = list.SelectMany(Function(cuser) cuser.Paths).First().PathName

            Catch ex As Exception

            End Try
        Else
            DIVinfo.Visible = False
        End If

        RPTusers.DataSource = list
        RPTusers.DataBind()
    End Sub

    Public Overrides Sub BindNoPermessi()
        MLVpathsummary.SetActiveView(VIWerror)
    End Sub

    Public Sub SetQueryString(key As String, value As String)
        Dim nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString())
        nameValues.Set(key, value)
        Dim url As String = Request.Url.AbsolutePath
        Dim updatedQueryString As String = "?" + nameValues.ToString()
        Response.Redirect(url + updatedQueryString)
    End Sub

    Private Property Pager As PagerBase
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridTop.Pager = value
            Me.PGgridBottom.Pager = value
            'Me.DVpagerTop.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property

    Public Overrides Function HasPermessi() As Boolean
        'Error.NotPermission
        'Dim result As Boolean = MyBase.Permission.Admin

        Dim result As Boolean = MyBase.PermissionOtherCommunity(CmntId).Admin

        LBerror.Text = Me.Resource.getValue("Error.NotPermission")

        Return result
    End Function

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        'Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
        BindDati()
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        'Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
        BindDati()
    End Sub

    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Stat", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(BTNupdateFilter)
            .setLabel(LBcommunityRoleTitle)
            .setLabel(LBuserfilterTitle)
            .setLabel(LBservice)
            .setLiteral(LTpageBottom)
            .setLiteral(LTpageTop)
            .setLabel(LBlegend)
            .setLabel(LBpathNameTitle)
            .setHyperLink(Me.HYPlistEduPath, False, True)
            .setCheckBox(CHBshowall)

        End With

        Aclearit.InnerText = Me.Resource.getValue("Aclearit.text")

        LBnotstarted.ToolTip = Me.Resource.getValue("LBnotstarted.tooltip")
        LBstarted.ToolTip = Me.Resource.getValue("LBstarted.tooltip")
        LBcompleted.ToolTip = Me.Resource.getValue("LBcompleted.tooltip")

        LBnotstartedlabel.Text = Me.Resource.getValue("LBnotstarted.tooltip")
        LBstartedlabel.Text = Me.Resource.getValue("LBstarted.tooltip")
        LBcompletedlabel.Text = Me.Resource.getValue("LBcompleted.tooltip")
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

    Private Sub SetLbValueWithMinValueFormat(ByRef oLb As Label, ByRef value As Int16, ByRef minValue As Int16, isAutoEp As Boolean)
        If isAutoEp Then
            oLb.Text = ServiceEP.GetTime(value) & " (" & ServiceEP.GetTime(minValue) & ")"
        Else
            oLb.Text = value & " (" & minValue & ")"
        End If

        'If (value >= minValue) Then
        '    oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
        'ElseIf (value > 0 AndAlso minValue > 0) Then
        '    oLb.CssClass = oLb.CssClass & "Ep_ItemYellow"
        'Else
        '    oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
        'End If
    End Sub


    Private Sub RPTusers_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTusers.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dto As dtoUserPaths = CType(e.Item.DataItem, dtoUserPaths)

            Dim olabel As Label
            olabel = e.Item.FindControl("LBusername")
            olabel.Text = dto.Person.SurnameAndName

            olabel = e.Item.FindControl("LBnotstarted")
            olabel.Text = dto.NotStarted
            olabel.ToolTip = Me.Resource.getValue("LBnotstarted.tooltip")

            olabel = e.Item.FindControl("LBstarted")
            olabel.Text = dto.Started
            olabel.ToolTip = Me.Resource.getValue("LBstarted.tooltip")

            olabel = e.Item.FindControl("LBcompleted")
            olabel.Text = dto.Completed
            olabel.ToolTip = Me.Resource.getValue("LBcompleted.tooltip")

            Dim hyp As HyperLink
            hyp = e.Item.FindControl("HYPinfo")
            Me.Resource.setHyperLink(hyp, False, True)
            hyp.NavigateUrl = Me.BaseUrl & RootObject.UserPathSummary(dto.Person.Id, CmntId, Pager.PageIndex, Server.UrlEncode(orole), name)

            If UniquePath Then


                If (dto.Paths.Count > 0) Then
                    Dim dtoUnique As dtoUserPathInfo = dto.Paths.FirstOrDefault()

                    Dim olabelUnique As Label
                    'olabel = e.Item.FindControl("LBpathname")
                    'olabel.Text = dtoUnique.PathName

                    olabel = e.Item.FindControl("LBstartdate")
                    olabel.Text = dtoUnique.StartDate

                    olabel = e.Item.FindControl("LBenddate")
                    olabel.Text = dtoUnique.EndDate


                    olabel = e.Item.FindControl("LBdeadline")
                    olabel.Text = ""

                    If (Not dtoUnique.Ps Is Nothing) Then
                        If (dtoUnique.Ps.Path.FloatingDeadlines) Then
                            olabel.Text = "--- floating ---"
                        Else
                            If (dtoUnique.Ps.Path.EndDate.HasValue) Then
                                olabel.Text = dtoUnique.Ps.Path.EndDate.Value.ToShortDateString() + " " + dtoUnique.Ps.Path.EndDate.Value.ToShortTimeString()
                                If dtoUnique.Ps.Path.EndDateOverflow.HasValue And dtoUnique.Ps.Path.EndDate.Value <> dtoUnique.Ps.Path.EndDateOverflow.Value Then
                                    olabel.Text += " *"
                                End If
                            Else
                                olabel.Text = Me.Resource.getValue("NONE")
                            End If
                        End If

                    End If

                    ' LBdeadline()

                    olabel = e.Item.FindControl("LBcompletion")
                    If (Not dtoUnique.Ps Is Nothing) Then


                        SetLbValueWithMinValueFormat(olabel, dtoUnique.Completion, dtoUnique.Ps.Path.MinCompletion, isAutoEp(dtoUnique.Ps.Path.EPType))
                    Else
                        olabel.Text = "---"

                    End If
                    Dim oHyp As HyperLink
                    oHyp = e.Item.FindControl("HYPstats")
                    oHyp.Visible = True
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.Text = ""
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsManage(dtoUnique.IdPath, Me.CurrentCommunityID, dtoUnique.IdPerson, ItemType.Path, 0, DateTime.Now, CHBshowall.Checked, "PathSummary")
                    'Me.GetDetailUrl(dtoItem.UserId)
                Else
                    hyp = e.Item.FindControl("HYPinfo")
                    hyp.Visible = False

                End If

            Else

                If (dto.Paths.Count = 0) Then
                    hyp = e.Item.FindControl("HYPinfo")
                    hyp.Visible = False
                End If

                Dim r As Repeater = e.Item.FindControl("RPTpaths")
                AddHandler r.ItemDataBound, AddressOf RPTpaths_ItemDataBound
                r.DataSource = dto.Paths
                r.DataBind()
            End If




        Else
            If (e.Item.ItemType = ListItemType.Header) Then
                Dim olabel As Label
                olabel = e.Item.FindControl("LBnameheader")
                olabel.Text = Me.Resource.getValue("LBnameheader.text")

                If (UniquePath) Then
                    olabel.Text = Me.Resource.getValue("LBuserheader.text")
                End If

                olabel = e.Item.FindControl("LBstatusheader")
                olabel.Text = Me.Resource.getValue("LBstatusheader.text")

                olabel = e.Item.FindControl("LBcompletionheader")
                olabel.Text = Me.Resource.getValue("LBcompletionheader.text")

                olabel = e.Item.FindControl("LBstartdateheader")
                olabel.Text = Me.Resource.getValue("LBstartdateheader.text")

                olabel = e.Item.FindControl("LBenddateheader")
                olabel.Text = Me.Resource.getValue("LBenddateheader.text")

                olabel = e.Item.FindControl("LBdeadlineheader")
                olabel.Text = Me.Resource.getValue("LBdeadlineheader.text")

                olabel = e.Item.FindControl("LBactionheader")
                olabel.Text = Me.Resource.getValue("LBactionheader.text")

            End If
        End If
    End Sub

    Protected Function isAutoEp(PathType As EPType)
        Return ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Auto)
    End Function
    Protected Function isTimeEp(PathType As EPType)
        Return ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Time)
    End Function
    Protected Function isMarkEp(PathType As EPType)
        Return ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Time)
    End Function



    Private Sub RPTpaths_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dto As dtoUserPathInfo = CType(e.Item.DataItem, dtoUserPathInfo)

            Dim olabel As Label
            olabel = e.Item.FindControl("LBpathname")
            olabel.Text = dto.PathName

            olabel = e.Item.FindControl("LBstartdate")
            olabel.Text = dto.StartDate

            olabel = e.Item.FindControl("LBenddate")
            olabel.Text = dto.EndDate


            olabel = e.Item.FindControl("LBdeadline")
            olabel.Text = ""

            If (Not dto.Ps Is Nothing) Then
                If (dto.Ps.Path.FloatingDeadlines) Then
                    olabel.Text = "--- floating ---"
                Else
                    If (dto.Ps.Path.EndDate.HasValue) Then
                        olabel.Text = dto.Ps.Path.EndDate.Value.ToShortDateString() + " " + dto.Ps.Path.EndDate.Value.ToShortTimeString()
                        If dto.Ps.Path.EndDateOverflow.HasValue And dto.Ps.Path.EndDate.Value <> dto.Ps.Path.EndDateOverflow.Value Then
                            olabel.Text += " *"
                        End If
                    Else
                        olabel.Text = Me.Resource.getValue("NONE")
                    End If
                End If

            End If

            ' LBdeadline()

            olabel = e.Item.FindControl("LBcompletion")
            If (Not dto.Ps Is Nothing) Then


                SetLbValueWithMinValueFormat(olabel, dto.Completion, dto.Ps.Path.MinCompletion, isAutoEp(dto.Ps.Path.EPType))
            Else
                olabel.Text = "---"

            End If
            Dim oHyp As HyperLink
            oHyp = e.Item.FindControl("HYPstats")
            Me.Resource.setHyperLink(oHyp, False, True)
            oHyp.Text = ""
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsManage(dto.IdPath, Me.CurrentCommunityID, dto.IdPerson, ItemType.Path, 0, DateTime.Now, False, "PathSummary")
            'Me.GetDetailUrl(dtoItem.UserId)

            oHyp = e.Item.FindControl("HYPcertificates")
            Me.Resource.setHyperLink(oHyp, False, True)
            oHyp.Text = ""
            oHyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationUser(dto.IdCommunity, dto.IdPath, dto.IdPerson)

            oHyp.Visible = ServiceEP.PathHasSubActivityType(dto.IdPath, SubActivityType.Certificate) AndAlso Not IsNothing(dto.Ps) AndAlso dto.Ps.Status > StatusStatistic.Started

        End If
    End Sub

    Public Function Zero(value As Int32) As String
        If (value = 0) Then
            Return "nopath"
        Else
            Return ""
        End If
    End Function

    Public Function ExpandIfOnlyOne(value As Int32) As String
        If (value < 2) Then
            If (UniquePath) Then
                Return "expanded unique"
            Else
                Return "expanded"
            End If
        Else
            Return ""
        End If
    End Function

    Public Function ExpandMe(value As Int32) As String
        If (Request.QueryString("UserId") = value.ToString()) Then
            Return "expanded"
        Else
            Return ""
        End If
    End Function

    Public Function Status(s As String) As String

        Select Case s
            Case "notstarted"
                Return "gray"
            Case "started"
                Return "yellow"
            Case "completed"
                Return "green"

            Case Else
                Return ""
        End Select

    End Function

    Public Function StatusTitle(s As String) As String

        Select Case s
            Case "notstarted"
                Return Me.Resource.getValue("EduPathTranslations.NotStarted")
            Case "started"
                Return Me.Resource.getValue("EduPathTranslations.Started")
            Case "completed"
                Return Me.Resource.getValue("EduPathTranslations.Completed")

            Case Else
                Return ""
        End Select

    End Function

    Private Sub DDLroleFilter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLroleFilter.SelectedIndexChanged
        orole = DDLroleFilter.SelectedItem.Value
    End Sub

    Private Sub BTNupdateFilter_Click(sender As Object, e As System.EventArgs) Handles BTNupdateFilter.Click
        Pager.GoFirst()
        orole = DDLroleFilter.SelectedItem.Value
        name = TXBuserFilter.Text
        BindDati()
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As ModuleStatus)

    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As ModuleStatus)

    End Sub
    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return False
        End Get
    End Property
End Class