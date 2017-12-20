Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports DTHelpers = lm.Comol.Core.BaseModules.DocTemplate.Helpers

Public Class UC_EditSignature
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event OnDeleteSignature(ByVal SignKey As String)

    Public Property signature As TemplateVers.Signature
        Get
            Dim sign As New TemplateVers.Signature()
            sign.Id = Me.HYDsignId.Value

            'Text
            If (CBXuseText.Checked) Then
                'sign.Text = CTRLvisualEditorNote.HTML
                sign.Text = CTRLvisualEditorText.HTML
                sign.IsHTML = True
            Else
                sign.Text = ""
                sign.IsHTML = False
            End If

            'Image

            sign.HasImage = Not String.IsNullOrEmpty(Me.UCimage.ImagePath)
            If (sign.HasImage) Then
                sign.Path = Me.UCimage.ImagePath
                sign.Width = UCimage.Width
                sign.Height = UCimage.Height

            End If

            'Positioning PDF
            sign.HasPDFPositioning = CBXpdfPositioning.Checked
            If (sign.HasPDFPositioning) Then
                sign.PosLeft = UCimgPosX.Px
                sign.PosBottom = UCimgPosY.Px
            End If

            'Alignment
            Select Case RBLalignment.SelectedValue
                Case -1
                    sign.Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.left
                Case 0
                    sign.Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.center
                Case 1
                    sign.Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.right

            End Select

            'Paging
            sign.PagePlacingMask = Me.UCpagePlacing.GetMask
            sign.PagePlacingRange = Me.UCpagePlacing.GetRange

            Return sign
        End Get
        Private Set(value As TemplateVers.Signature)

            HYDsignId.Value = value.Id
            LITcode.Text = value.Id.ToString()

            'Text
            CBXuseText.Checked = Not String.IsNullOrEmpty(value.Text)

            If (CBXuseText.Checked) Then
                'CTRLvisualEditorNote.HTML = value.Text
                'CTRLvisualEditorNote.Visible = True
                CTRLvisualEditorText.HTML = value.Text
                CTRLvisualEditorText.Visible = True

            Else
                'CTRLvisualEditorNote.Visible = False
                CTRLvisualEditorText.Visible = False

            End If

            'Image
            If value.HasImage Then

                Me.UCimage.Init(value.Path, value.Width, value.Height, "UC_EditSignature.ascx.vb, 73")

            Else
                Me.UCimage.Init("", 0, 0, "UC_EditSignature.ascx.vb, 87")

            End If

            'Positioning PDF
            CBXpdfPositioning.Checked = value.HasPDFPositioning

            If value.HasPDFPositioning Then
                UCimgPosX.Px = value.PosLeft
                UCimgPosY.Px = value.PosBottom
                UCimgPosX.Enabled = True
                UCimgPosY.Enabled = True
            Else
                UCimgPosX.Px = 0
                UCimgPosY.Px = 0
                UCimgPosX.Enabled = False
                UCimgPosY.Enabled = False
            End If

            UCimgPosX.SetMeasure(UC_Measure.Units.mm)
            UCimgPosY.SetMeasure(UC_Measure.Units.mm)

            Select Case value.Position
                Case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.left
                    RBLalignment.SelectedValue = -1
                Case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.center
                    RBLalignment.SelectedValue = 0
                Case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.right
                    RBLalignment.SelectedValue = 1
            End Select

            Me.UCpagePlacing.InitControl(value.PagePlacingMask, value.PagePlacingRange)

        End Set
    End Property
    Public Property CurrentKey As String
        Get
            Return Me.LKBdeletesignature.CommandArgument
        End Get
        Set(value As String)
            Me.LKBdeletesignature.CommandName = "DeleteSignature"
            Me.LKBdeletesignature.CommandArgument = value

        End Set
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return Me.CTRLvisualEditorText.EditorClientId
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

        BintTagButtons()

        With Me.Resource

            .setLinkButton(LKBdeletesignature, True, True, False, False)
            .setRadioButtonList(RBLalignment, -1)
            .setRadioButtonList(RBLalignment, 0)
            .setRadioButtonList(RBLalignment, 1)

            .setCheckBox(CBXuseText)
            .setCheckBox(CBXpdfPositioning)
            .setLabel(LBLsignText_t)
            .setLabel(LBLimage_t)
            .setLabel(LBLalignment_t)

            UCimgPosX.Label = .getValue("UCimgPosX.Label")
            UCimgPosY.Label = .getValue("UCimgPosY.Label")

            .setLabel(LBLhfPositionSign_t)

            .setLiteral(LITcode_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub SetSignature(ByVal Signature As TemplateVers.Signature, ByVal TemplateId As Int64, ByVal VersionId As Int64)
        CTRLvisualEditorText.InitializeControl("SRVDOCT")
        Me.UCimage.TemplateId = TemplateId
        Me.UCimage.VersionId = VersionId

        Me.signature = Signature

        SetInternazionalizzazione()
    End Sub

    Private Sub BintTagButtons()
        Try
            Me.BTNpgtagCurPage.Text = Me.Resource.getValue("PageTag.CurPage")
        Catch ex As Exception
            Me.BTNpgtagCurPage.Text = Me.Resource.getValue("CurrentPage")
        End Try

        Me.BTNpgtagCurPage.CommandArgument = "tag" 'Item.Id.ToString
        Me.BTNpgtagCurPage.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.PageNumberCurrent)


        Try
            Me.BTNpgtagCreateDate.Text = Me.Resource.getValue("PageTag.CreateDate")
        Catch ex As Exception
            Me.BTNpgtagCreateDate.Text = Me.Resource.getValue("Creation Date")
        End Try

        Me.BTNpgtagCreateDate.CommandArgument = "tag" 'Item.Id.ToString
        Me.BTNpgtagCreateDate.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.CreateDate)

        Try
            Me.BTNpgtagCreateTime.Text = Me.Resource.getValue("PageTag.CreateTime")
        Catch ex As Exception
            Me.BTNpgtagCreateTime.Text = Me.Resource.getValue("Creation Time")
        End Try

        Me.BTNpgtagCreateTime.CommandArgument = "tag" 'Item.Id.ToString
        Me.BTNpgtagCreateTime.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.CreateTime)
    End Sub
#End Region

#Region "Handler"
    Private Sub CBXpdfPositioning_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXpdfPositioning.CheckedChanged
        If CBXpdfPositioning.Checked Then
            UCpagePlacing.Enable = True
            UCimgPosX.Enabled = True
            UCimgPosY.Enabled = True
        Else
            UCpagePlacing.Enable = False
            UCimgPosX.Enabled = False
            UCimgPosY.Enabled = False
        End If
    End Sub
    Private Sub CBXuseText_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXuseText.CheckedChanged
        If CBXuseText.Checked Then
            CTRLvisualEditorText.Visible = True
            CTRLvisualEditorText.InitializeControl("SRVDOCT")
            'Me.CTRLvisualEditorNote.Visible = True
        Else
            CTRLvisualEditorText.Visible = False
            'Me.CTRLvisualEditorNote.Visible = False
        End If
    End Sub
    Private Sub LKBdeletesignature_Click(sender As Object, e As System.EventArgs) Handles LKBdeletesignature.Click
        RaiseEvent OnDeleteSignature(CurrentKey)
    End Sub
#End Region

End Class