Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class UC_EditSettings
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

#Region "Internal"

    Public Event DeletePrevSetting(ByVal Id As Int64)
    Public Event RecoverPrevSetting(ByVal Id As Int64)
    Public Event DeletePrevSettings(ByVal Ids As IList(Of Int64))

    Public Property TemplateName As String
        Get
            Return Me.TXBname.Text
        End Get
        Set(value As String)
            Me.TXBname.Text = value
        End Set
    End Property
    Public Property CurrentSettings As TemplateVers.Settings
        Get
            Dim Setting As New TemplateVers.Settings()
            With Setting

                'PageSize
                Dim SelSizeId As Integer = 0
                Try
                    SelSizeId = System.Convert.ToInt16(Me.DDLpageFormat.SelectedValue)
                Catch ex As Exception

                End Try

                If (SelSizeId = 0) Then 'Custom
                    .Size = lm.Comol.Core.DomainModel.DocTemplateVers.PageSize.custom
                    .Width = Me.CTRLmeasureWidth.Px
                    .Height = Me.CTRLmeasureWidth.Px

                Else
                    .Size = CType(SelSizeId, TemplateVers.PageSize)
                    Dim PGsize As TemplateVers.Helpers.PageSizeValue = TemplateVers.Helpers.Measure.GetSize(Me.DDLpageFormat.SelectedValue, "px")

                    .Width = PGsize.Width
                    .Height = PGsize.Height
                End If

                'Page Margin

                .MarginBottom = Me.CTRLmarginBottom.Px
                .MarginLeft = Me.CTRLmarginLeft.Px
                .MarginRight = Me.CTRLmarginRight.Px
                .MarginTop = Me.CTRLmarginTop.Px

                .PagePlacingMask = Me.UCplacing.GetMask
                .PagePlacingRange = Me.UCplacing.GetRange

                Select Case RBLbgType.SelectedValue
                    Case 2  'Image
                        '.BackgroundImagePath = HFLimgPath.Value
                        .BackgroundImagePath = Me.UCimage.ImagePath
                        .BackgroundAlpha = 0
                        .BackgroundBlue = 0
                        .BackgroundGreen = 0

                        Select Case Me.DDLimageArrange.SelectedValue
                            Case "-1"
                                .BackGroundImageFormat = lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Center
                            Case "0"
                                .BackGroundImageFormat = lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Tiled
                            Case "1"
                                .BackGroundImageFormat = lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Stretch
                        End Select

                    Case 1  ' Color
                        .BackgroundImagePath = ""
                        'COLOR PICKER VALUE! (SEE SKIN MANAGEMENT!)
                        .BackgroundAlpha = 100

                        Dim Color As System.Drawing.Color = System.Drawing.Color.FromArgb(Me.RDPbackgorundColor.SelectedColor.ToArgb())

                        .BackgroundBlue = Color.B
                        .BackgroundGreen = Color.G
                        .BackgroundRed = Color.R

                    Case Else 'None
                        .BackgroundImagePath = ""
                        .BackgroundAlpha = 0
                        .BackgroundBlue = 0
                        .BackgroundGreen = 0
                End Select

                'PDF Data
                .Title = Me.TXBtitle.Text
                .Subject = Me.TXBsubject.Text
                .Author = Me.TXBauthor.Text
                .Creator = Me.TXBcreator.Text
                .Producer = Me.TXBproducer.Text
                .Keywords = Me.TXBkeywords.Text
            End With

            Return Setting

        End Get

        Set(value As TemplateVers.Settings)

            LBLcode.Text = value.Id.ToString()

            'Page Size
            BindPageFormat(value.Size)

            If value.Size = TemplateVers.PageSize.custom Then
                Me.CTRLmeasureHeight.Px = value.Height
                Me.CTRLmeasureWidth.Px = value.Width
                Me.CTRLmeasureHeight.Enabled = True
                Me.CTRLmeasureWidth.Enabled = True

            Else
                Dim PGsize As TemplateVers.Helpers.PageSizeValue = TemplateVers.Helpers.Measure.GetSize(value.Size, "px")

                Me.CTRLmeasureHeight.Px = PGsize.Height
                Me.CTRLmeasureWidth.Px = PGsize.Width
                Me.CTRLmeasureHeight.Enabled = False
                Me.CTRLmeasureWidth.Enabled = False
            End If

            Me.CTRLmeasureHeight.SetMeasure(UC_Measure.Units.mm)
            Me.CTRLmeasureWidth.SetMeasure(UC_Measure.Units.mm)

            'Page Margin
            Me.CTRLmarginBottom.Px = value.MarginBottom
            Me.CTRLmarginLeft.Px = value.MarginLeft
            Me.CTRLmarginRight.Px = value.MarginRight
            Me.CTRLmarginTop.Px = value.MarginTop

            Me.CTRLmarginBottom.SetMeasure(UC_Measure.Units.mm)
            Me.CTRLmarginLeft.SetMeasure(UC_Measure.Units.mm)
            Me.CTRLmarginRight.SetMeasure(UC_Measure.Units.mm)
            Me.CTRLmarginTop.SetMeasure(UC_Measure.Units.mm)

            'Generic
            Me.UCplacing.InitControl(value.PagePlacingMask, value.PagePlacingRange)

            Me.UCimage.Init(value.BackgroundImagePath, 0, 0, "UC_EditSettings.ascx.vb, 146")

            If Not String.IsNullOrEmpty(value.BackgroundImagePath) Then
                Me.RBLbgType.SelectedValue = 2
                Me.MLVbgType.SetActiveView(Me.Vimage)

            ElseIf (value.BackgroundAlpha > 0) Then
                Me.RBLbgType.SelectedValue = 1
                Me.MLVbgType.SetActiveView(Me.Vcolor)
                Me.RDPbackgorundColor.SelectedColor = Color.FromArgb(value.BackgroundRed, value.BackgroundGreen, value.BackgroundBlue)
            Else
                Me.RBLbgType.SelectedValue = 0
                Me.MLVbgType.SetActiveView(Me.Vnone)
            End If

            'PDF Data
            Me.TXBtitle.Text = value.Title
            Me.TXBsubject.Text = value.Subject
            Me.TXBauthor.Text = value.Author
            Me.TXBcreator.Text = value.Creator
            Me.TXBproducer.Text = value.Producer
            Me.TXBkeywords.Text = value.Keywords

            Me.TXBtitle.Enabled = False
            Me.TXBsubject.Enabled = False
            Me.TXBauthor.Enabled = False
            Me.TXBcreator.Enabled = False
            Me.TXBproducer.Enabled = False
            Me.TXBkeywords.Enabled = False

            Me.PNLsubVersion.Visible = False

            'BackGround
            Select Case value.BackGroundImageFormat
                Case lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Center
                    Me.DDLimageArrange.SelectedValue = "-1"
                Case lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Tiled
                    Me.DDLimageArrange.SelectedValue = "0"
                Case lm.Comol.Core.DomainModel.DocTemplateVers.BackgrounImagePosition.Stretch
                    Me.DDLimageArrange.SelectedValue = "1"
            End Select
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBLmargin_t)
            .setLiteral(LITgenerics_t)

            .setLabel(LBLname_t)
            .setLiteral(LITlayout_t)
            .setLabel(LBLpagesize)
            .setLabel(LBLpageFormat_t)

            '.setCheckBox(CBXhfOnIfirst)
            '.setCheckBox(CBXpageNumber)

            .setLiteral(LITbackground_t)
            .setLabel(LBLtype_t)
            .setRadioButtonList(RBLbgType, "0")
            .setRadioButtonList(RBLbgType, "1")
            .setRadioButtonList(RBLbgType, "2")

            .setDropDownList(DDLimageArrange, "-1")
            .setDropDownList(DDLimageArrange, "0")
            .setDropDownList(DDLimageArrange, "1")

            '.setDropDownList(DDLnumPagePosition, "-1")
            '.setDropDownList(DDLnumPagePosition, "0")

            CTRLmeasureWidth.Label = .getValue("CTRLmeasureWidth.Label")
            CTRLmeasureHeight.Label = .getValue("CTRLmeasureHeight.Label")
            CTRLmarginTop.Label = .getValue("CTRLmarginTop.Label")
            CTRLmarginRight.Label = .getValue("CTRLmarginRight.Label")
            CTRLmarginBottom.Label = .getValue("CTRLmarginBottom.Label")
            CTRLmarginLeft.Label = .getValue("CTRLmarginLeft.Label")

            .setLabel(LBLtitle_t)
            .setLabel(LBLsubject_t)
            .setLabel(LBLauthor_t)
            .setLabel(LBLcreator_t)
            .setLabel(LBLproducer_t)
            .setLabel(LBLkeywords_t)

            .setLiteral(LITrevision_t)
            .setLiteral(LITpdfData)

            .setLabel(LBLhfPosition_t)
            .setLabel(LBLbgimage_t)
            .setLabel(LBLbgImgaArrange_t)

            .setLabel(LBLbgColor_t)

            .setLabel(LBLcode_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Private Sub BindPageFormat(ByVal SelSize As TemplateVers.PageSize)
        Dim Sizes() As Integer = CType([Enum].GetValues(GetType(TemplateVers.PageSize)), Integer())

        Me.DDLpageFormat.Items.Clear()

        For Each SizeId As Integer In Sizes
            Dim liSize As New ListItem()
            liSize.Text = CType(SizeId, TemplateVers.PageSize).ToString().Replace("_L", " " & Me.Resource.getValue("PageFormat.Landscape")).Replace("_", "")

            liSize.Value = SizeId

            Me.DDLpageFormat.Items.Add(liSize)
        Next

        Me.DDLpageFormat.SelectedValue = SelSize

    End Sub
    Public Sub SetTemplteVersion(ByVal TemplateId As Int64, ByVal VersionId As Int64)
        Me.UCimage.TemplateId = TemplateId
        Me.UCimage.VersionId = VersionId
    End Sub
    Public Sub BindPrevVersion(ByVal PrevVersions As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(PrevVersions) AndAlso PrevVersions.Count > 0 Then
            Me.UCprevVersion.BindList(PrevVersions)
            Me.PNLsubVersion.Visible = True
        Else
            Me.PNLsubVersion.Visible = False
        End If
    End Sub
    Private Sub UCprevVersion_DeleteItem(Id As Long) Handles UCprevVersion.DeleteItem
        RaiseEvent DeletePrevSetting(Id)
    End Sub
    Private Sub UCprevVersion_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersion.DeleteItems
        RaiseEvent DeletePrevSettings(Ids)
    End Sub
    Private Sub UCprevVersion_RecoverItem(Id As Long) Handles UCprevVersion.RecoverItem
        RaiseEvent RecoverPrevSetting(Id)
    End Sub
#End Region

#Region "Handler"
    Private Sub DDLpageFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpageFormat.SelectedIndexChanged
        If Me.DDLpageFormat.SelectedValue = TemplateVers.PageSize.custom Then
            Me.CTRLmeasureHeight.Enabled = True
            Me.CTRLmeasureWidth.Enabled = True
        Else

            Dim PGsize As TemplateVers.Helpers.PageSizeValue = TemplateVers.Helpers.Measure.GetSize(Me.DDLpageFormat.SelectedValue, "px")

            Me.CTRLmeasureHeight.Px = PGsize.Height
            Me.CTRLmeasureWidth.Px = PGsize.Width

            Me.CTRLmeasureHeight.Enabled = False
            Me.CTRLmeasureWidth.Enabled = False
        End If
    End Sub
    Private Sub RBLbgType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLbgType.SelectedIndexChanged
        Select Case Me.RBLbgType.SelectedValue

            Case 1
                Me.MLVbgType.SetActiveView(Me.Vcolor)
            Case 2
                Me.MLVbgType.SetActiveView(Me.Vimage)
            Case Else
                Me.MLVbgType.SetActiveView(Me.Vnone)
        End Select
    End Sub
#End Region

End Class