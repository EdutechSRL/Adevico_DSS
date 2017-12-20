Imports lm.Comol.Core.DomainModel.Repository

Public Class UC_ModuleAttachmentJqueryHeaderCommands
    Inherits FRbaseControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    ''' <summary>
    ''' Load commands for jquery open modal windows, opendialog of hyperlinks = LTopendialogcssclassprefix.text+RepositoryAttachmentUploadActions.item.ToLower()
    ''' </summary>
    ''' <param name="actions"></param>
    ''' <param name="actionsTitle"></param>
    ''' <remarks></remarks>
    Public Sub InitializeControlForJQuery(actions As List(Of RepositoryAttachmentUploadActions), actionsTitle As Dictionary(Of RepositoryAttachmentUploadActions, String), Optional tags As List(Of String) = Nothing)
        If IsNothing(actions) Then
            actions = New List(Of RepositoryAttachmentUploadActions)
        End If
        If IsNothing(actionsTitle) Then
            actionsTitle = New Dictionary(Of RepositoryAttachmentUploadActions, String)
        End If
        InitializeControlForJQuery(actions.ToDictionary(Function(i) i, Function(i) CTRLscripts.OpendialogCssClassPrefix & i.ToString), actionsTitle, tags)
    End Sub
    ''' <summary>
    ''' Load commands for jquery open modal windows, for each commands there is a specific cssclass for jquery open modal window
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="actionsTitle"></param>
    ''' <remarks></remarks>
    Public Sub InitializeControlForJQuery(items As Dictionary(Of RepositoryAttachmentUploadActions, String), actionsTitle As Dictionary(Of RepositoryAttachmentUploadActions, String), Optional tags As List(Of String) = Nothing)
        CTRLheader.InitializeHeader(tags)
        LTdialogTitle.Text = ""
        For Each item As KeyValuePair(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String) In items
            Dim script As String = CTRLscripts.GetBaseScript
            Dim oLiteral As Literal = FindControl("LT" & item.Key.ToString)
            Dim title As String = ""
            If actionsTitle.ContainsKey(item.Key) Then
                title = actionsTitle(item.Key)
            End If

            LTdialogTitle.Text &= Replace(Replace(LTdialogTitleTemplate.Text, "#type#", item.Key.ToString), "#value#", Replace(title, "'", "\'")) & vbCrLf

            If Not IsNothing(oLiteral) Then
                oLiteral.Visible = True


                If item.Key = RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity OrElse item.Key = RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity Then
                    script = script.Replace(CTRLscripts.GetPlaceholderAutoOpen, CTRLscripts.GetAutoOpenScript)
                Else
                    script = script.Replace(CTRLscripts.GetPlaceholderAutoOpen, "")
                End If
                script = script.Replace(CTRLscripts.GetPlaceholderTitle, title)
                script = script.Replace(CTRLscripts.GetPlaceholderDialogIdentifyer, CTRLscripts.GetDivDialogCssClassPrefix & item.Key.ToString)
                script = script.Replace(CTRLscripts.GetPlaceholderOpendialog, item.Value)
                script = script.Replace(CTRLscripts.GetPlaceholderCloseScripts, LTcloseScripts.Text)
                Dim oSizesLiteral As Literal = FindControl("LT" & item.Key.ToString & "Window")

                Dim sizes As New List(Of String)
                If Not IsNothing(oSizesLiteral) Then
                    sizes = oSizesLiteral.Text.Split(",").Where(Function(s) Not String.IsNullOrEmpty(s)).ToList().Where(Function(s) IsNumeric(s)).ToList
                End If
                If sizes.Count < 4 Then
                    sizes = CTRLscripts.GetWwindowDefaultSizes.Split(",").Where(Function(s) Not String.IsNullOrEmpty(s)).ToList().Where(Function(s) IsNumeric(s)).ToList
                End If
                If sizes.Count = 4 Then
                    script = script.Replace(CTRLscripts.GetPlaceholderWidth, sizes(0))
                    script = script.Replace(CTRLscripts.GetPlaceholderHeight, sizes(1))
                    script = script.Replace(CTRLscripts.GetPlaceholderMinWidth, sizes(2))
                    script = script.Replace(CTRLscripts.GetPlaceholderMinHeight, sizes(3))
                End If

                oLiteral.Text = script
            Else
                script = script
            End If
        Next
    End Sub

#End Region

  
  
End Class