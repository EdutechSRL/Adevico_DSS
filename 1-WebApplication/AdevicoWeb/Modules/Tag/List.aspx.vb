Public Class TagList
    Inherits TGpageBase


#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadForOrganization, False)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
        Master.ServiceNopermission = Resource.getValue("Tags.ServiceTitle.NoPermission")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Tag.Domain.RootObject.List(False, PreloadForOrganization), IdTagsCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("Tags.ServiceTitle")
            .setHyperLink(HYPgoTo_TagRecycleBin, False, True)
            If PreloadForOrganization Then
                .setLinkButtonToValue(LNBaddTag, "Organization", False, True)
                .setLinkButtonToValue(LNBaddMultipleTags, "Organization", False, True)
            Else
                .setLinkButton(LNBaddTag, False, True)
                .setLinkButton(LNBaddMultipleTags, False, True)
            End If
        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem() As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Function GetListControl() As UC_TagsList
        Return CTRLtags
    End Function
    Protected Friend Overrides Function GetRecycleUrlItem() As HyperLink
        Return HYPgoTo_TagRecycleBin
    End Function
    Protected Friend Overrides Function GetAddButton() As LinkButton
        Return LNBaddTag
    End Function
    Protected Friend Overrides Function GetAddMultipleButton() As LinkButton
        Return LNBaddMultipleTags
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

    Private Sub CTRLtags_LanguageChanged(idLanguage As Integer) Handles CTRLtags.LanguageChanged
        CTRLheader.FilterIdLanguage = idLanguage
    End Sub
End Class