Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.Comol.Core.FileRepository.Domain.ScormSettings
Public Class UC_ScormSettings
    Inherits FRbaseControl

#Region "Internal"
    Public Property ReadOnlyMode As Boolean
        Get
            Return ViewStateOrDefault("ReadOnlyMode", False)
        End Get
        Set(value As Boolean)
            ViewState("ReadOnlyMode") = value
            HYPscormActivitySettings.Enabled = Not value
            HYPscormOriginalSettings.Enabled = Not value
            HYPscormPackageSettings.Enabled = Not value
        End Set
    End Property
    Public ReadOnly Property CssClass As String
        Get
            If (ReadOnlyMode) Then
                Return " readonly"
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#Region "Internal"
    Public Sub InitializeControl(uniqueId As String, filename As String, settings As dtoScormPackageSettings, allowEditing As Boolean, viewMode As Boolean)
        ReadOnlyMode = allowEditing
        InitializeTranslations(viewMode)
        RenderTree(uniqueId, filename, settings)
        RBLevaluationMode.SelectedValue = CInt(settings.EvaluationType)
        'RBLevaluationMode.Enabled = Not ReadOnlyMode
        Select Case settings.EvaluationType
            Case EvaluationType.CustomForActivities
                HYPscormActivitySettings.CssClass &= " active"
                HYPscormPackageSettings.CssClass = Replace(HYPscormPackageSettings.CssClass, " active", "")
                HYPscormOriginalSettings.CssClass = Replace(HYPscormOriginalSettings.CssClass, " active", "")
            Case EvaluationType.CustomForPackage
                HYPscormPackageSettings.CssClass &= " active"
                HYPscormActivitySettings.CssClass = Replace(HYPscormActivitySettings.CssClass, " active", "")
                HYPscormOriginalSettings.CssClass = Replace(HYPscormOriginalSettings.CssClass, " active", "")
                CTRLsettings.InitializeControl(settings, allowEditing)
            Case EvaluationType.FromScormEvaluation
                HYPscormOriginalSettings.CssClass &= " active"
                HYPscormActivitySettings.CssClass = Replace(HYPscormActivitySettings.CssClass, " active", "")
                HYPscormPackageSettings.CssClass = Replace(HYPscormPackageSettings.CssClass, " active", "")
        End Select
        RPTactivities.DataSource = settings.Activities
        RPTactivities.DataBind()
    End Sub
    Private Sub InitializeTranslations(viewMode As Boolean)
        With Resource
            If viewMode Then
                LBdescriptionScormSettings.Text = Resource.getValue("LBdescriptionScormSettings.viewMode")
            Else
                .setLabel(LBdescriptionScormSettings)
            End If
            .setLiteral(LTscormSettingsBoxTitle)
            .setLabel(LBscormSettingsBoxModified)
            .setLiteral(LTscormSettingsBoxDescription)
            .setHyperLink(HYPscormOriginalSettings, False, True)
            .setHyperLink(HYPscormPackageSettings, False, True)
            .setHyperLink(HYPscormActivitySettings, False, True)
            .setLabel(LBscormPackageSettingsBoxModified)
            .setLiteral(LTscormPackageSettingsBoxTitle)
            .setLiteral(LTscormMessage)
            .setHyperLink(HYPseeAllScormActivities, False, True)
        End With
    End Sub
    Public Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
        Dim template As String = LTtemplateFile.Text
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                template = Replace(template, "#ico#", LTitemFolderCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                template = Replace(template, "#ico#", LTitemUrlCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                template = Replace(template, "#ico#", LTitemMultimediaCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                template = Replace(template, "#ico#", LTitemScormPackageCssClass.Text)
            Case Else
                If Not String.IsNullOrWhiteSpace(fileExtension) Then
                    fileExtension = fileExtension.ToLower
                End If
                If fileExtension.StartsWith(".") Then
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
                Else
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        template = Replace(template, "#name#", fullname)
        Return template
    End Function
    Public Function GetPackageEvaluationType() As EvaluationType
        Return DirectCast(CInt(RBLevaluationMode.SelectedValue), EvaluationType)
    End Function
    Public Function GetSettings() As List(Of dtoScormItemEvaluationSettings)
        Dim settings As New List(Of dtoScormItemEvaluationSettings)
        Select Case CInt(RBLevaluationMode.SelectedValue)
            Case CInt(EvaluationType.CustomForPackage)
                settings.Add(CTRLsettings.GetSettings)
            Case CInt(EvaluationType.CustomForActivities)
                For Each row As RepeaterItem In RPTactivities.Items
                    Dim oControl As UC_ScormSettingsItem = row.FindControl("CTRLitemSettings")
                    settings.Add(oControl.GetSettings)
                Next
        End Select
        Return settings
    End Function

#Region "Render Tree"
    Private Sub RenderTree(versionIdentifier As String, filename As String, settings As dtoScormPackageSettings)
        Dim render As String = LTtreeRoot.Text
        render = Replace(render, "#uniqueIdVersion#", versionIdentifier)
        render = Replace(render, "#name#", settings.Name)
        render = Replace(render, "#dataid#", settings.DataId)
        render = Replace(render, "#datachildren#", settings.DataChildren)
        LTcookieTemplate.Text = Replace(LTcookieTemplate.Text, "#uniqueIdVersion#", versionIdentifier)

        If settings.Children.Any Then
            render = Replace(render, "#childrennodes#", RenderTree(settings.Children))
        Else
            render = Replace(render, "#childrennodes#", "")
        End If
        LTrenderTree.Text = render
    End Sub
    Private Function RenderTree(organizations As List(Of dtoScormOrganizationSettings)) As String
        Dim render As String = ""
        Dim nodeRender As String = ""
        For Each node As dtoScormOrganizationSettings In organizations
            nodeRender = LTtreeFolderNode.Text
            nodeRender = Replace(nodeRender, "#name#", node.Name)
            nodeRender = Replace(nodeRender, "#dataid#", node.DataId)
            nodeRender = Replace(nodeRender, "#datachildren#", node.DataChildren)

            If node.Children.Any() Then
                nodeRender = Replace(nodeRender, "#childrennodes#", RenderTree(node.Children))
            Else
                nodeRender = Replace(nodeRender, "#childrennodes#", "")
            End If
            render &= nodeRender

        Next
        Return Replace(LTtreeChildrenNodes.Text, "#childrennodes#", render)
    End Function
    Private Function RenderTree(folders As List(Of dtoScormOrganizationFolder)) As String
        Dim render As String = ""
        Dim nodeRender As String = ""
        For Each folder As dtoScormOrganizationFolder In folders
            nodeRender = LTtreeFolderNode.Text
            nodeRender = Replace(nodeRender, "#name#", folder.Name)
            nodeRender = Replace(nodeRender, "#dataid#", folder.DataId)
            nodeRender = Replace(nodeRender, "#datachildren#", folder.DataChildren)

            If folder.Children.Any() Then
                nodeRender = Replace(nodeRender, "#childrennodes#", RenderTree(folder.Children))
            Else
                nodeRender = Replace(nodeRender, "#childrennodes#", "")
            End If
            render &= nodeRender
        Next
        Return Replace(LTtreeChildrenNodes.Text, "#childrennodes#", render)
    End Function
#End Region
    Private Sub RPTactivities_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTactivities.ItemDataBound
        Dim item As dtoScormActivitySettings = DirectCast(e.Item.DataItem, dtoScormActivitySettings)
        Dim oLiteral As Literal = e.Item.FindControl("LTactivityBoxTitle")
        oLiteral.Text = item.OrganizationName & " - " & item.Name
        Dim oLabel As Label = e.Item.FindControl("LBscormActivitySettingsBoxModified")
        Resource.setLabel(oLabel)

        Dim oControl As UC_ScormSettingsItem = e.Item.FindControl("CTRLitemSettings")
        oControl.InitializeControl(item, ReadOnlyMode)
    End Sub

#End Region
End Class