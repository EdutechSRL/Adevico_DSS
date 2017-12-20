Public Class BaseNotificationUtility
    Protected _Utility As OLDpageUtility

    Public Function ServiceLoaderPage(ByVal CommunityID As Integer, ByVal DestinationUrl As String) As String
        Return _Utility.EncryptedUrl("Notification/ServiceLoaderPage.aspx", "CommunityID=" & CommunityID.ToString & "&DestinationUrl=" & _Utility.GetUrlEncoded(DestinationUrl), SecretKeyUtil.EncType.Altro)
        'Return _Utility.BaseUrl & "Notification/ServiceLoaderPage.aspx?CommunityID={0}&PageToLoad={1}"
    End Function
    Public Function ServiceLoaderPage(ByVal CommunityID As Integer, ByVal DestinationUrl As String, ByVal FromUrl As String) As String
        Return _Utility.EncryptedUrl("Notification/ServiceLoaderPage.aspx", "CommunityID=" & CommunityID.ToString & "&DestinationUrl=" & _Utility.GetUrlEncoded(DestinationUrl) & "&FromUrl=" & _Utility.GetUrlEncoded(FromUrl), SecretKeyUtil.EncType.Altro)
        'Return _Utility.BaseUrl & "Notification/ServiceLoaderPage.aspx?CommunityID={0}&PageToLoad={1}"
    End Function


    Sub New(ByVal oUtility As OLDpageUtility)
        _Utility = oUtility
    End Sub
End Class