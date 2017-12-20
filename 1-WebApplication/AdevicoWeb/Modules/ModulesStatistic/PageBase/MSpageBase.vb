Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract

Public MustInherit Class MSpageBase
    Inherits PageBase
    Implements IViewBasePageStatistics


#Region "Implements"
#Region "Preload"
    'Private ReadOnly Property PreloadedFromView() As StatisticView Implements IViewBasePageStatistics.PreloadedFromView
    '    Get
    '        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticView).GetByString(Me.Request.QueryString("From"), StatisticView.MyCommunity)
    '    End Get
    'End Property
#End Region
#Region "View property"
    Protected Friend ReadOnly Property CurrentPageSize() As Integer Implements IViewBasePageStatistics.CurrentPageSize
        Get
            Return 30
        End Get
    End Property
    Protected Friend ReadOnly Property Ascending() As Boolean Implements IViewBasePageStatistics.Ascending
        Get
            If Request.QueryString("Ascending") = "True" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property CurrentUrl() As String Implements IViewBasePageStatistics.CurrentUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query)
            Return url
        End Get
    End Property

   
    Public ReadOnly Property CurrentPage() As Integer Implements IViewBasePageStatistics.CurrentPage
        Get
            If Me.Request.QueryString("Page") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("Page"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PortalName() As String Implements IViewBasePageStatistics.PortalName
        Get
            Return Resource.getValue("PortalName")
        End Get
    End Property
    Protected Friend Property StatisticContext() As UsageContext Implements IViewBasePageStatistics.StatisticContext
        Get
            Dim oContext As New UsageContext With {.CommunityID = -1, .ModuleID = -2, .UserID = -1}
            If TypeOf Me.ViewState("StatisticContext") Is UsageContext Then
                oContext = Me.ViewState("StatisticContext")
            Else
                If IsNumeric(Me.Request.QueryString("IdCommunity")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("IdCommunity")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdCommunity").ToList
                        For Each id As String In stringList
                            oContext.CommunityIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("IdModule")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("IdModule")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdModule").ToList
                        For Each id As String In stringList
                            oContext.ModuleIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try

                End If
                If IsNumeric(Me.Request.QueryString("IdUser")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("IdUser")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdUser").ToList
                        For Each id As String In stringList
                            oContext.UserIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("groupBy")) Then
                    Try
                        oContext.GroupBy = Me.Request.QueryString("groupBy")
                    Catch ex As Exception

                    End Try
                End If
                oContext.CurrentPage = 0
                If IsNumeric(Me.Request.QueryString("Page")) Then
                    Try
                        oContext.CurrentPage = Me.Request.QueryString("Page")
                    Catch ex As Exception

                    End Try
                End If

                If String.IsNullOrEmpty(Me.Request.QueryString("Order")) Then
                    oContext.Order = StatisticOrder.UsageTime
                    oContext.Ascending = False
                Else
                    oContext.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.UsageTime)
                    oContext.Ascending = True
                    If String.IsNullOrEmpty(Me.Request.QueryString("Dir")) Then
                        If oContext.Order = StatisticOrder.AccessNumber OrElse oContext.Order = StatisticOrder.UsageTime Then
                            oContext.Ascending = False
                        End If
                    Else
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "true")
                    End If
                End If
                oContext.SearchBy = ""
                Me.ViewState("StatisticContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As UsageContext)
            Me.ViewState("StatisticContext") = value
        End Set
    End Property

#End Region
#End Region

#Region "Internal"
    'Protected Friend ReadOnly Property AscendingImage() As String
    '    Get
    '        Return Me.BaseUrl & "images/Grid/Ascending.gif"
    '    End Get
    'End Property
    'Protected Friend ReadOnly Property DescendingImage() As String
    '    Get
    '        Return Me.BaseUrl & "images/Grid/Descending.gif"
    '    End Get
    'End Property
    Protected Friend ReadOnly Property AscendingCss() As String
        Get
            Return "icon orderUp"
        End Get
    End Property
    Protected Friend ReadOnly Property DescendingCss() As String
        Get
            Return "icon orderDown"
        End Get
    End Property
#End Region

#Region "Inherits"
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


#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageGlobal", "Statistiche")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendAction(idCommunity As Integer, idModule As Integer, statIdUser As Integer, statIdCommunity As Integer, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
        Dim items As New Dictionary(Of Integer, String)
        items.Add(ModuleStatistics.ObjectType.User, statIdUser)
        items.Add(ModuleStatistics.ObjectType.Community, statIdCommunity)
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, items), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendAction(idCommunity As Integer, idModule As Integer, statIdItem As Integer, type As ModuleStatistics.ObjectType, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, type, statIdItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendAction(idCommunity As Integer, idModule As Integer, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String Implements IViewBasePageStatistics.GetTimeTranslatedString
        Dim oDate As DateTime = New DateTime
        Dim UsageString As String = ""
        oDate = oDate.AddSeconds(oSpan.TotalSeconds)
        With oDate
            If .Year > 0 And oSpan.TotalDays >= 365 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Year"), .Year, .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Month > 0 And oSpan.TotalDays >= 30 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Month"), .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Day > 0 And oSpan.TotalDays >= 1 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Day"), .Day, .Hour, .Minute, .Second)
            ElseIf .Hour > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Hour"), .Hour, .Minute, .Second)
            ElseIf .Minute > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Minute"), .Minute, .Second)
            ElseIf .Second > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Second"), .Second)
            Else
                UsageString = " // "
            End If
        End With
        Return UsageString
    End Function
    Public Function GetSummaryTranslatedString(ByVal summary As dtoSummary, ByVal type As IViewBaseStatistics.SummaryType) As String Implements IViewBasePageStatistics.GetSummaryTranslatedString
        Dim SummaryString As String = Me.Resource.getValue("summary." & type.ToString)
        Select Case type
            Case IViewBaseStatistics.SummaryType.Community
                SummaryString = String.Format(SummaryString, summary.CommunityName)
            Case IViewBaseStatistics.SummaryType.CommunityModules
                SummaryString = String.Format(SummaryString, summary.CommunityName, summary.ModuleName)
            Case IViewBaseStatistics.SummaryType.Modules
                SummaryString = String.Format(SummaryString, summary.ModuleName)
            Case IViewBaseStatistics.SummaryType.Personal
                SummaryString = String.Format(SummaryString, summary.Owner)
            Case IViewBaseStatistics.SummaryType.PersonalCommunity
                SummaryString = String.Format(SummaryString, summary.Owner, summary.CommunityName)
            Case IViewBaseStatistics.SummaryType.PersonalCommunityModules
                SummaryString = String.Format(SummaryString, summary.Owner, summary.CommunityName, summary.ModuleName)
            Case IViewBaseStatistics.SummaryType.PersonalModules
                SummaryString = String.Format(SummaryString, summary.Owner, summary.ModuleName)
            Case IViewBaseStatistics.SummaryType.Portal
        End Select
        Return SummaryString
    End Function
    Private Sub LoadView(url As String) Implements IViewBasePageStatistics.LoadView
        PageUtility.RedirectToUrl(url)
    End Sub
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
#End Region

End Class