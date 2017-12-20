Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBpageBaseSearch
    Inherits DBpageBaseDashboardLoader
    Implements IViewBaseSearch

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadSearch As DisplaySearchItems Implements IViewBaseSearch.PreloadSearch
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplaySearchItems).GetByString(Request.QueryString("s"), DisplaySearchItems.Simple)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadSearchText As String Implements IViewBaseSearch.PreloadSearchText
        Get
            Return Request.QueryString("t")
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdcommunityType As Int32 Implements IViewBaseSearch.PreloadIdCommunityType
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idType")) AndAlso IsNumeric(Request.QueryString("idType")) Then
                Return CInt(Request.QueryString("idType"))
            Else
                Return -1
            End If
        End Get
    End Property

#End Region

#End Region


#Region "Internal"
    Public Function GetBaseUrl() As String
        Return PageUtility.ApplicationUrlBase
    End Function
#End Region

#Region "Implements"
  
    Protected Friend MustOverride Sub EnableFullWidth(value As Boolean) Implements IViewBaseSearch.EnableFullWidth
    Private Sub LoadDashboard(url As String) Implements IViewDefaultDashboardLoader.LoadDashboard
        PageUtility.RedirectToUrl(url)
    End Sub
    Protected Friend Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewBaseSearch.GetSubmittedFilters
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
        If Not Page.IsPostBack Then
            filter.IdOrganization = -1
            filter.IdcommunityType = -1
        End If

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdcommunityType = CInt(Request.Form(item.ToString))
                        Else
                            .IdcommunityType = PreloadIdcommunityType
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.coursetime
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdCourseTime = CInt(Request.Form(item.ToString))
                        Else
                            .IdCourseTime = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.degreetype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdDegreeType = CInt(Request.Form(item.ToString))
                        Else
                            .IdDegreeType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdOrganization = CInt(Request.Form(item.ToString))
                        Else
                            .IdOrganization = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdResponsible = CInt(Request.Form(item.ToString))
                        Else
                            .IdResponsible = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Communities.CommunityStatus.None
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Year = CInt(Request.Form(item.ToString))
                        Else
                            .Year = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag
                        .IdTags = New List(Of Long)
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            For Each idTag As String In Request.Form(item.ToString).Split(",")
                                .IdTags.Add(CLng(idTag))
                            Next
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.name
                        .SearchBy = lm.Comol.Core.BaseModules.CommunityManagement.SearchCommunitiesBy.Contains
                        .Value = Request.Form(item.ToString)
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters
                        .StartWith = ""
                        If Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            Dim charInt As Integer = CInt(Request.Form(item.ToString))
                            Select Case charInt
                                Case -1
                                    .StartWith = ""
                                Case -9
                                    .StartWith = "#"
                                Case Else
                                    .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                            End Select
                        End If
                End Select
            Next
        End With

        Return filter
    End Function
#End Region

End Class