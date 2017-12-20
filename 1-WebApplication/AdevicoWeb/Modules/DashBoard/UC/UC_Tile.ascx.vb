Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation

Public Class UC_Tile
    Inherits DBbaseControl

#Region "Internal"
    Private ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(layout As TileLayout, dto As dtoTileDisplay)
        With dto
            Dim isCustom As Boolean = Not String.IsNullOrEmpty(TilesVirtualPath) AndAlso Not String.IsNullOrEmpty(.ImageUrl)

            LTgetTileCssClass.Text = layout.ToString
            If isCustom Then
                LTgetTileCssClass.Text &= " " & LTcssClassCustomTile.Text
            End If

            LTtileTitle.Text = dto.Translation.Title
            IMGtileIcon.Visible = isCustom
            If isCustom Then
                IMGtileIcon.ImageUrl = PageUtility.ApplicationUrlBase & TilesVirtualPath & .ImageUrl
            ElseIf Not String.IsNullOrEmpty(.ImageCssClass) Then
                DVicon.Attributes("class") &= " " & LTcssClassTileIcon.Text & " " & .ImageCssClass
            Else
                DVicon.Attributes("class") &= " " & LTcssClassTileIcon.Text & " " & LTcssClassDefaultItemClass.Text
            End If
            If String.IsNullOrEmpty(dto.CommandUrl) OrElse ((dto.Type = TileType.DashboardUserDefined OrElse dto.Type = TileType.UserDefined) AndAlso String.IsNullOrEmpty(dto.NavigateUrl)) Then
                LTlinkOpen.Visible = False
                LTlinkClose.Visible = False
            ElseIf dto.AutoNavigateUrl OrElse dto.Type = TileType.CombinedTags OrElse dto.Type = TileType.CommunityTag OrElse dto.Type = TileType.CommunityType Then
                LTlinkOpen.Text = String.Format(LTlinkOpen.Text, BaseUrl & dto.CommandUrl, .Translation.Title)
            ElseIf Not String.IsNullOrEmpty(dto.NavigateUrl) Then
                Dim url As String = dto.NavigateUrl
                Dim urlStart As String = ""
                If url.Length > 10 Then
                    urlStart = url.Substring(0, 10)
                Else
                    urlStart = url
                End If
                If Not (urlStart.Contains("://") OrElse urlStart.Contains("mailto:") OrElse urlStart.Contains("news:")) Then
                    Dim queryString As String = ""
                    If (url.Contains("?")) Then
                        url = dto.NavigateUrl.Split("?")(0)
                        queryString = dto.NavigateUrl.Split("?")(1)
                    End If

                    'If PageUtility.BaseUrl = "/" Then

                    'End If
                    'If Not PageUtility.BaseUrl = "/" AndAlso url.StartsWith("/") Then
                    url = (BaseUrl & url).Replace("//", "/")
                    '   End If

                    If lm.Comol.Core.File.Exists.File(Server.MapPath(url)) Then
                        If dto.NavigateUrl.StartsWith("/") Then
                            dto.NavigateUrl = dto.NavigateUrl.Remove(0, 1)
                        End If
                        url = PageUtility.ApplicationUrlBase & dto.NavigateUrl
                    Else
                        url &= queryString
                    End If
                End If
                If dto.ForPreview Then
                    LTlinkOpen.Text = String.Format(LTlinkPreview.Text, url, .Translation.Title)
                Else
                    LTlinkOpen.Text = String.Format(LTlinkOpen.Text, url, .Translation.Title)
                End If
            Else
                LTlinkOpen.Visible = False
                LTlinkClose.Visible = False
            End If
        End With
    End Sub

#Region "Css"
    Public Function GetTileCssClass() As String
        Return LTgetTileCssClass.Text
    End Function
#End Region
#End Region

End Class