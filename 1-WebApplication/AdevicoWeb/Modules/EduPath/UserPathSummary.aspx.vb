Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation

Public Class UserPathSummary
    Inherits EPpageBaseEduPath

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        MLVpathsummary.SetActiveView(VIWpathsummary)

        If Not UserIdquery Then
            Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(CmntId, EpViewModeType.View, PreloadIsMooc)
            Me.HYPlistEduPath.Visible = True
            Me.HYPback.Visible = False
        Else
            Try
                Dim page As Int32 = Int32.Parse(Request.QueryString("Page"))
                Dim role As Int32 = Int32.Parse(Request.QueryString("RId"))
                Dim search As String = Request.QueryString("Search")

                If (From = "list") Then
                    Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(CmntId, EpViewModeType.View, PreloadIsMooc)
                Else
                    Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.PathSummary(CmntId, Me.UserId, 0, page, role, search, PreloadIsMooc)
                End If
            Catch ex As Exception
                If (From = "list") Then
                    Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(CmntId, EpViewModeType.View, PreloadIsMooc)
                Else
                    Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.PathSummary(CmntId, Me.UserId, 0, PreloadIsMooc)
                End If
            End Try
            Me.HYPlistEduPath.Visible = False
            Me.HYPback.Visible = True
        End If

        Dim list As IList(Of dtoUserPaths) = New List(Of dtoUserPaths)
        If Not Page.IsPostBack Then
            Dim l As New List(Of Comol.Entity.Role)

            l = ServiceEP.GetActiveRoles(CmntId, LinguaID)
            l.Add(New Comol.Entity.Role() With {.ID = 0, .Name = Me.Resource.getValue("NONE")})
            l = l.OrderBy(Function(x) x.Name).ToList()
        End If

        Dim dtou As dtoUserPaths = Me.ServiceStat.GetSelectedUserPathsCount(PreloadIsMooc, CmntId, UserId)
        If (Not dtou Is Nothing) Then
            list.Add(dtou)
        Else
            ShowMessageToPage(Me.Resource.getValue("Error.Url"))
        End If

        RPTusers.DataSource = list
        RPTusers.DataBind()
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpathsummary.SetActiveView(VIWerror)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim result As Boolean = False
        If (UserId = UtenteCorrente.ID) Then
            result = True
        Else
            result = MyBase.PermissionOtherCommunity(CmntId).Admin
        End If
        LBerror.Text = Me.Resource.getValue("Error.NotPermission")

        Return result
    End Function
    Public Overrides Sub SetCultureSettings()
        If PreloadIsMooc Then
            MyBase.SetCulture("pg_MoocStatistics", "EduPath")
        Else
            MyBase.SetCulture("pg_Stat", "EduPath")
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBservice)
            .setHyperLink(Me.HYPlistEduPath, False, True)
            .setHyperLink(Me.HYPback, False, True)
            .setLabel(LBcommunitynameTitle)
            .setLabel(LBstatusTitle)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Function Zero(value As Int32) As String
        If (value = 0) Then
            Return "nopath"
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
#End Region

  


    Public ReadOnly Property UserIdquery As Boolean
        Get
            Return Not String.IsNullOrEmpty(Request.QueryString("UserId"))
        End Get
    End Property

    Public ReadOnly Property UserIdqueryCurrent As Boolean
        Get
            If (UserIdquery) Then
                Return UserId = UtenteCorrente.ID
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property UserId As Int32
        Get
            If (String.IsNullOrEmpty(Request.QueryString("UserId"))) Then
                Return UtenteCorrente.ID
            Else
                Dim _id As Int32 = Integer.Parse(Request.QueryString("UserId"))
                Return _id
            End If

        End Get
    End Property


    Public ReadOnly Property From As String
        Get
            If (String.IsNullOrEmpty(Request.QueryString("From"))) Then
                Return ""
            Else
                Return Request.QueryString("From")
            End If

        End Get
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

  


    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

   

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        LBerror.Text = errorMessage
        MLVpathsummary.SetActiveView(VIWerror)
    End Sub

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

  

    Private Sub UserPathSummary_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

    Private Sub RPTusers_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTusers.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dto As dtoUserPaths = CType(e.Item.DataItem, dtoUserPaths)

            Dim olabel As Label
            'olabel = e.Item.FindControl("LBusername")
            'olabel.Text = dto.Person.SurnameAndName

            If ComunitaCorrenteID <> CmntId Then
                DIVothercommunity.Visible = True
                Dim c As New COL_Comunita(CmntId)
                LBcommunityname.Text = c.EstraiNomeBylingua(LinguaID)
            Else
                DIVothercommunity.Visible = False
            End If

            LBserviceUserName.Text = dto.Person.SurnameAndName

            olabel = LBnotstarted 'e.Item.FindControl("LBnotstarted")
            olabel.Text = dto.NotStarted
            olabel.ToolTip = Me.Resource.getValue("LBnotstarted.tooltip")
            LBnotstartedlabel.Text = olabel.ToolTip

            olabel = LBstarted  'e.Item.FindControl("LBstarted")
            olabel.Text = dto.Started
            olabel.ToolTip = Me.Resource.getValue("LBstarted.tooltip")
            LBstartedlabel.Text = olabel.ToolTip

            olabel = LBcompleted  'e.Item.FindControl("LBcompleted")
            olabel.Text = dto.Completed
            olabel.ToolTip = Me.Resource.getValue("LBcompleted.tooltip")
            LBcompletedlabel.Text = olabel.ToolTip


            Dim r As Repeater = e.Item.FindControl("RPTpaths")
            AddHandler r.ItemDataBound, AddressOf RPTpaths_ItemDataBound
            r.DataSource = dto.Paths
            r.DataBind()


        Else
            If (e.Item.ItemType = ListItemType.Header) Then
                Dim olabel As Label
                olabel = e.Item.FindControl("LBpathheader")
                olabel.Text = Me.Resource.getValue("LBpathheader.text")

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
            'Dim permissions As PermissionEP = ServiceEP.GetCurrentUserPermission_ByPath(dto.IdPath)
            If dto.PathInfo.Status = lm.Comol.Modules.EduPath.Domain.Status.Locked AndAlso Not dto.CanManage Then
                oHyp.Visible = False
            ElseIf dto.CanManage Then
                oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsManage(dto.IdPath, Me.CurrentCommunityID, dto.IdPerson, ItemType.Path, 0, DateTime.Now, False, "PathSummary", PreloadIsMooc)
            ElseIf dto.CanStat Then
                oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsView(dto.IdPath, Me.CurrentCommunityID, DateTime.Now, False, PreloadIsMooc)
            Else
                oHyp.Visible = False
            End If
            'If permissions.ViewUserStat Then

            'ElseIf permissions.ViewOwnStat Then

            'Else

            'End If



            'Me.GetDetailUrl(dtoItem.UserId)

            oHyp = e.Item.FindControl("HYPcertificates")
            Me.Resource.setHyperLink(oHyp, False, True)
            oHyp.Text = ""
            oHyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationUser(Me.CurrentCommunityID, dto.IdPath, dto.IdPerson, dto.IsMooc)

            oHyp.Visible = ServiceEP.PathHasSubActivityType(dto.IdPath, SubActivityType.Certificate) AndAlso Not IsNothing(dto.Ps) AndAlso dto.Ps.Status > StatusStatistic.Browsed

        End If
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