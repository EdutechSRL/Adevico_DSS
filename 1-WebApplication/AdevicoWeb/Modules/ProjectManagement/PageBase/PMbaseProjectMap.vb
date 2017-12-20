Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMbaseProjectMap
    Inherits PMpageBaseEdit
    Implements IViewBaseProjectMap

#Region "Implements Property"
    Protected ReadOnly Property UnknownUser As String Implements IViewBaseProjectMap.UnknownUser
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
    Protected ReadOnly Property CurrentShortDatePattern As String Implements IViewBaseProjectMap.CurrentShortDatePattern
        Get
            Return LoaderCultureInfo.DateTimeFormat.ShortDatePattern
        End Get
    End Property
#End Region

#Region "Implements"
    Protected MustOverride Sub SetDashboardUrl(url As String, dashboard As PageListType) Implements IViewBaseProjectMap.SetDashboardUrl
    Protected MustOverride Sub SetEditProjectUrl(url As String) Implements IViewBaseProjectMap.SetEditProjectUrl
    Protected MustOverride Sub SetProjectsUrl(url As String) Implements IViewBaseProjectMap.SetProjectsUrl
    Protected MustOverride Sub DisplaySessionTimeout() Implements IViewBaseProjectMap.DisplaySessionTimeout
    Protected MustOverride Sub DisplayUnknownProject() Implements IViewBaseProjectMap.DisplayUnknownProject
    Protected MustOverride Sub LoadProjectDateInfo(project As dtoProject, allowEdit As Boolean) Implements IViewBaseProjectMap.LoadProjectDateInfo

    Protected Overloads Sub RedirectToUrl(url As String) Implements IViewBaseProjectMap.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

#Region "Internal"
    Protected ReadOnly Property CurrentDatePickerShortDatePattern As String
        Get
            Dim pattern As String = CurrentShortDatePattern.ToLower

            Return pattern.Replace("mm", "m").Replace("yy", "y").Replace("dd", "d")
        End Get
    End Property
    Protected ReadOnly Property InternalDialogTitleTranslation() As String
        Get
            Return Resource.getValue("InternalDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property ExternalDialogTitleTranslation() As String
        Get
            Return Resource.getValue("ExternalDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property RemoveResourceDialogTitleTranslation() As String
        Get
            Return Resource.getValue("RemoveResourceDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property ProjectResourcesDialogTitleTranslation() As String
        Get
            Return Resource.getValue("ProjectResourcesDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property EditActivityDialogTitleTranslation() As String
        Get
            Return Resource.getValue("EditActivityDialogTitleTranslation")
        End Get
    End Property
    Protected Property LoaderCultureInfo As System.Globalization.CultureInfo
        Get
            Return ViewStateOrDefault("LoaderCultureInfo", Resource.CultureInfo)
        End Get
        Set(value As System.Globalization.CultureInfo)
            ViewState("LoaderCultureInfo") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Protected Function GetCssStatuslight(completeness As Integer) As String
        If completeness = 100 Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function
    Protected Function GetCssStatuslight(completeness As Integer, isCompleted As Boolean) As String
        If completeness = 100 AndAlso isCompleted Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function

#Region "Activities"

#Region "CssClass"
    Protected Function GetRowCssClass(activity As dtoMapActivity) As String
        Dim cssClass As String = ""
        If activity.IdParent > 0 Then
            cssClass = " child-of-" & activity.IdParent.ToString
        End If
        cssClass &= IIf(activity.Status.HasFlag(FieldStatus.error), " " & FieldStatus.error.ToString(), "")
        Return cssClass
    End Function
    Protected Function GetCellCssClass(status As FieldStatus) As String
        Return IIf(status.HasFlag(FieldStatus.error) OrElse status.HasFlag(FieldStatus.errorfatherlinked) OrElse status.HasFlag(FieldStatus.errorsummarylinked), " " & FieldStatus.error.ToString(), " " & status.ToString)
    End Function
    Protected Function GetDeadlineCssClass(isAfterDeadline As Boolean) As String
        Return IIf(isAfterDeadline, " " & FieldStatus.error.ToString(), "")
    End Function
    Protected Function GetResourcesCssClass(item As dtoMapActivity) As String
        Dim cssClass As String = IIf(item.Resources.Status.HasFlag(FieldStatus.error), " " & FieldStatus.error.ToString(), " " & item.Resources.Status.ToString)

        If Not item.IsSummary AndAlso (IsNothing(item.Resources.GetValue) OrElse (Not IsNothing(item.Resources.GetValue) AndAlso Not item.Resources.GetValue().Any())) Then
            cssClass &= " noresource"
        End If

        Return cssClass
    End Function
#End Region

#End Region
    Protected Function GetDateFromString(ByVal inputDate As String, ByVal defaultDateTime As DateTime?) As DateTime
        Try
            Dim dItems As String() = inputDate.Split("/")
            If dItems.Count = 3 Then
                If dItems(0).Count = 1 Then
                    dItems(0) = "0" + dItems(0)
                End If
                If dItems(1).Count = 1 Then
                    dItems(1) = "0" + dItems(1)
                End If
            End If
            Return DateTime.ParseExact(String.Join("/", dItems), CurrentShortDatePattern, LoaderCultureInfo.CurrentCulture.InvariantCulture)
        Catch ex As Exception
            Return defaultDateTime
        End Try
        Return defaultDateTime
    End Function
#End Region

End Class