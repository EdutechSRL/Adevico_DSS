Public Class TagRecycleBin
    Inherits TGpageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadForOrganization, True)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
        Master.ServiceNopermission = Resource.getValue("Tags.ServiceTitle.RecycleBin.NoPermission")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Tag.Domain.RootObject.List(True, PreloadForOrganization), IdTagsCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("Tags.ServiceTitle.RecycleBin")
            .setHyperLink(HYPgoTo_TagList, False, True)

        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem() As HyperLink
        Return HYPgoTo_TagList
    End Function
    Protected Friend Overrides Function GetListControl() As UC_TagsList
        Return CTRLtags
    End Function
    Protected Friend Overrides Function GetRecycleUrlItem() As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Function GetAddButton() As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetAddMultipleButton() As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetListControlHeader() As UC_TagsListHeader
        Return CTRLheader
    End Function
#End Region

#Region "internal"
    Private Sub TagRecycleBin_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
#End Region

   
End Class