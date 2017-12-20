Public Class UC_ModuleAttachmentInlineCommands
    Inherits BaseUserControl

#Region "Internal"
    Public Event ItemCommand(ByVal command As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Repository", "Modules", "Repository")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(LNBuploadtomoduleitem, False, True)
            .setLinkButton(LNBuploadtomoduleitemandcommunity, False, True)
            .setLinkButton(LNBlinkfromcommunity, False, True)
            .setLinkButton(LNBaddurltomoduleitem, False, True)
            .setLinkButton(LNBaddurltomoduleitemandcommunity, False, True)

            .setHyperLink(HYPuploadtomoduleitem, False, True)
            .setHyperLink(HYPuploadtomoduleitemandcommunity, False, True)
            .setHyperLink(HYPlinkfromcommunity, False, True)
            .setHyperLink(HYPaddurltomoduleitem, False, True)
            .setHyperLink(HYPaddurltomoduleitemandcommunity, False, True)
        End With
    End Sub
#End Region

    Public Sub InitializeControlForPostback(items As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), Optional dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
        SetInternazionalizzazione()
        InitializeControl(True, items.Count)
        InitializePostBackButtons(items, dAction)
    End Sub
    ''' <summary>
    ''' Load commands for jquery open modal windows, opendialog of hyperlinks = LTopendialogcssclassprefix.text+RepositoryAttachmentUploadActions.item.ToLower()
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="dAction"></param>
    ''' <remarks></remarks>
    Public Sub InitializeControlForJQuery(items As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), Optional dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
        If IsNothing(items) Then
            items = New List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
        End If
        InitializeControl(True, items.Count)
        InitializeControlForJQuery(items.ToDictionary(Function(i) i, Function(i) LTopendialogcssclassprefix.Text & i.ToString), dAction)

    End Sub
    ''' <summary>
    ''' Load commands for jquery open modal windows, for each commands there is a specific cssclass for jquery open modal window
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="dAction"></param>
    ''' <remarks></remarks>
    Public Sub InitializeControlForJQuery(items As Dictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String), Optional dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
        InitializeControl(False, items.Count)
        InitializeJqueryButtons(items, dAction)
    End Sub

    Private Sub InitializeControl(autopostback As Boolean, count As Integer)
        DisplayCommandControls(IIf(autopostback, VIWpostbackCommands.Controls, VIWjqueryCommands.Controls), False)
        If count > 1 Then
            LTddbuttonlist.Text = LTddbuttonlistEnabled.Text
        Else
            LTddbuttonlist.Text = LTddbuttonlistDisabled.Text
        End If
    End Sub
    Private Sub DisplayCommandControls(controls As System.Web.UI.ControlCollection, visible As Boolean)
        For Each oControl As Control In controls
            If TypeOf (oControl) Is HyperLink OrElse TypeOf (oControl) Is LinkButton Then
                oControl.Visible = visible
            End If
        Next
    End Sub

#Region "JQuery items"
    Private Sub InitializeJqueryButtons(items As Dictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String), dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
        MLVcommands.SetActiveView(VIWjqueryCommands)

        If Not IsNothing(items) AndAlso Not items.ContainsKey(dAction) Then
            dAction = items.First.Key
        End If
        For Each item As KeyValuePair(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String) In items
            Dim oHyperlink As HyperLink = VIWjqueryCommands.FindControl("HYP" & item.Key.ToString)
            oHyperlink.Visible = True
            oHyperlink.CssClass = LTddbutton.Text & " " & item.Value
            If item.Key = dAction Then
                oHyperlink.CssClass &= " " & LTddbuttonActive.Text
            End If
        Next
    End Sub
#End Region
#Region "AutoPostback items"
    Private Sub InitializePostBackButtons(items As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
        MLVcommands.SetActiveView(VIWpostbackCommands)
        If IsNothing(items) Then
            items = New List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
        End If

        If Not IsNothing(items) AndAlso Not items.Contains(dAction) Then
            dAction = items.First
        End If

        For Each item As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions In items
            Dim oLinkButton As LinkButton = VIWpostbackCommands.FindControl("LNB" & item.ToString)
            oLinkButton.Visible = True
            If item = dAction Then
                oLinkButton.CssClass &= " " & LTddbuttonActive.Text
            End If
        Next
    End Sub
    Private Sub LNBuploadtomoduleitem_Click(sender As Object, e As System.EventArgs) Handles LNBuploadtomoduleitem.Click
        RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
    End Sub
    Private Sub LNBuploadtomoduleitemandcommunity_Click(sender As Object, e As System.EventArgs) Handles LNBuploadtomoduleitemandcommunity.Click
        RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity)
    End Sub
    Private Sub LNBlinkfromcommunity_Click(sender As Object, e As System.EventArgs) Handles LNBlinkfromcommunity.Click
        RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity)
    End Sub
    Private Sub LNBaddurltomoduleitem_Click(sender As Object, e As System.EventArgs) Handles LNBaddurltomoduleitem.Click
        RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem)
    End Sub
    Private Sub LNBaddurltomoduleitemandcommunity_Click(sender As Object, e As System.EventArgs) Handles LNBaddurltomoduleitemandcommunity.Click
        RaiseEvent ItemCommand(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity)
    End Sub
#End Region

End Class