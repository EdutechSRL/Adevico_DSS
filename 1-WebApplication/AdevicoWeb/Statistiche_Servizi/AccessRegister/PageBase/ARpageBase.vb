Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract

Public MustInherit Class ARpageBase
    Inherits PageBase
    Implements iViewBaseAccessResult


#Region "Implements"
    Protected Friend ReadOnly Property AscendingCss() As String Implements iViewBaseAccessResult.AscendingCss
        Get
            Return "icon orderUp"
        End Get
    End Property
    Protected Friend ReadOnly Property DescendingCss() As String Implements iViewBaseAccessResult.DescendingCss
        Get
            Return "icon orderDown"
        End Get
    End Property
    
#End Region

#Region "Internal"
   
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
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

    '#Region "Implements"
    '    Private Sub SendAction(idCommunity As Integer, idModule As Integer, statIdUser As Integer, statIdCommunity As Integer, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
    '        Dim items As New Dictionary(Of Integer, String)
    '        items.Add(ModuleStatistics.ObjectType.User, statIdUser)
    '        items.Add(ModuleStatistics.ObjectType.Community, statIdCommunity)
    '        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, items), InteractionType.UserWithLearningObject)
    '    End Sub
    '    Private Sub SendAction(idCommunity As Integer, idModule As Integer, statIdItem As Integer, type As ModuleStatistics.ObjectType, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
    '        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, type, statIdItem.ToString), InteractionType.UserWithLearningObject)
    '    End Sub
    '    Private Sub SendAction(idCommunity As Integer, idModule As Integer, action As ModuleStatistics.ActionType) Implements IViewBasePageStatistics.SendAction
    '        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    '    End Sub

    '    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String Implements IViewBasePageStatistics.GetTimeTranslatedString
    '        Dim oDate As DateTime = New DateTime
    '        Dim UsageString As String = ""
    '        oDate = oDate.AddSeconds(oSpan.TotalSeconds)
    '        With oDate
    '            If .Year > 0 And oSpan.TotalDays >= 365 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Year"), .Year, .Month, .Day, .Hour, .Minute, .Second)
    '            ElseIf .Month > 0 And oSpan.TotalDays >= 30 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Month"), .Month, .Day, .Hour, .Minute, .Second)
    '            ElseIf .Day > 0 And oSpan.TotalDays >= 1 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Day"), .Day, .Hour, .Minute, .Second)
    '            ElseIf .Hour > 0 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Hour"), .Hour, .Minute, .Second)
    '            ElseIf .Minute > 0 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Minute"), .Minute, .Second)
    '            ElseIf .Second > 0 Then
    '                UsageString = String.Format(Me.Resource.getValue("UsageTime.Second"), .Second)
    '            Else
    '                UsageString = " // "
    '            End If
    '        End With
    '        Return UsageString
    '    End Function
    '    Public Function GetSummaryTranslatedString(ByVal summary As dtoSummary, ByVal type As IViewBaseStatistics.SummaryType) As String Implements IViewBasePageStatistics.GetSummaryTranslatedString
    '        Dim SummaryString As String = Me.Resource.getValue("summary." & type.ToString)
    '        Select Case type
    '            Case IViewBaseStatistics.SummaryType.Community
    '                SummaryString = String.Format(SummaryString, summary.CommunityName)
    '            Case IViewBaseStatistics.SummaryType.CommunityModules
    '                SummaryString = String.Format(SummaryString, summary.CommunityName, summary.ModuleName)
    '            Case IViewBaseStatistics.SummaryType.Modules
    '                SummaryString = String.Format(SummaryString, summary.ModuleName)
    '            Case IViewBaseStatistics.SummaryType.Personal
    '                SummaryString = String.Format(SummaryString, summary.Owner)
    '            Case IViewBaseStatistics.SummaryType.PersonalCommunity
    '                SummaryString = String.Format(SummaryString, summary.Owner, summary.CommunityName)
    '            Case IViewBaseStatistics.SummaryType.PersonalCommunityModules
    '                SummaryString = String.Format(SummaryString, summary.Owner, summary.CommunityName, summary.ModuleName)
    '            Case IViewBaseStatistics.SummaryType.PersonalModules
    '                SummaryString = String.Format(SummaryString, summary.Owner, summary.ModuleName)
    '            Case IViewBaseStatistics.SummaryType.Portal
    '        End Select
    '        Return SummaryString
    '    End Function
    '    Private Sub LoadView(url As String) Implements IViewBasePageStatistics.LoadView
    '        PageUtility.RedirectToUrl(url)
    '    End Sub
    '    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer)
    '        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
    '        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
    '        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

    '        dto.DestinationUrl = destinationUrl

    '        If idCommunity > 0 Then
    '            dto.IdCommunity = idCommunity
    '        End If

    '        webPost.Redirect(dto)
    '    End Sub
    '#End Region

End Class