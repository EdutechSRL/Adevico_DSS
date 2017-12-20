Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports DTHelpers = lm.Comol.Core.BaseModules.DocTemplate.Helpers

Public Class UC_EditPageElement
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property Item As TemplateVers.PageElement
        Get
            Dim element As TemplateVers.PageElement

            Dim CurViewId As String = Me.MLVtype.GetActiveView().ID

            Select Case CurViewId
                Case Me.VIWimage.ID
                    Dim ImageElement As New TemplateVers.ElementImage

                    'TODO: upload immagine!!!
                    ImageElement.Path = Me.UCimage.ImagePath 'HIDimgPath.Value 'Me.IMGpreview.ImageUrl

                    'Whidt ed Height => convertire in LONG!!!

                    ImageElement.Width = Me.UCimage.Width 'System.Convert.ToInt16(UCimgWidth.Px)
                    ImageElement.Height = Me.UCimage.Height 'System.Convert.ToInt16(UCimgHeight.Px)

                    element = ImageElement
                Case Me.VIWtext.ID
                    Dim TextElement As New TemplateVers.ElementText
                    TextElement.IsHTML = True
                    TextElement.Text = Me.CTRLvisualEditorText.HTML

                    element = TextElement
                Case Else

                    element = New TemplateVers.ElementVoid
            End Select

            Select Case RBLalignment.SelectedValue
                Case -1
                    element.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleLeft
                Case 0
                    element.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter
                Case 1
                    element.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.BottomRight
            End Select



            Return element

        End Get
        Set(value As TemplateVers.PageElement)
            If (IsNothing(value)) Then
                BindNoImage()
                BindNoTxt()

                Me.RBLtype.SelectedValue = "none"
                Me.MLVtype.SetActiveView(Me.VIWnone)
                LBLcode.Text = "-1"
            Else

                LBLcode.Text = value.Id.ToString()

                Select Case value.Alignment
                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.TopLeft
                        Me.RBLalignment.SelectedValue = -1

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.TopCenter
                        Me.RBLalignment.SelectedValue = 0

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.TopRight
                        Me.RBLalignment.SelectedValue = 1

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleLeft
                        Me.RBLalignment.SelectedValue = -1

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter
                        Me.RBLalignment.SelectedValue = 0

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleRight
                        Me.RBLalignment.SelectedValue = 1

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.BottomLeft
                        Me.RBLalignment.SelectedValue = -1

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.BottomCenter
                        Me.RBLalignment.SelectedValue = 0

                    Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.BottomRight
                        Me.RBLalignment.SelectedValue = 1

                End Select

                If TypeOf value Is TemplateVers.ElementText Then
                    BindNoImage()

                    Me.RBLtype.SelectedValue = "txt"
                    Me.MLVtype.SetActiveView(Me.VIWtext)

                    Dim txtEl As TemplateVers.ElementText = TryCast(value, TemplateVers.ElementText)
                    Me.CTRLvisualEditorText.HTML = txtEl.Text


                ElseIf TypeOf value Is TemplateVers.ElementImage Then

                    BindNoTxt()

                    Me.RBLtype.SelectedValue = "img"
                    Me.MLVtype.SetActiveView(Me.VIWimage)

                    Dim imgEl As TemplateVers.ElementImage = TryCast(value, TemplateVers.ElementImage)

                    Me.UCimage.Init(imgEl.Path, imgEl.Width, imgEl.Height, "Uc_EditPageElement.ascx.vb, 114")

                Else
                    BindNoImage()
                    BindNoTxt()
                    Me.RBLtype.SelectedValue = "none"
                    Me.MLVtype.SetActiveView(Me.VIWnone)

                End If

            End If
        End Set
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return Me.CTRLvisualEditorText.EditorClientId
        End Get
    End Property
    Public ReadOnly Property TagOnKeyUpScript As String
        Get
            Return HTMLonKeyUpScript
        End Get
    End Property
    Public Property HTMLonKeyUpScript As String
        Get
            Return ViewStateOrDefault("HTMLonKeyUpScript", "")
        End Get
        Set(value As String)
            Me.ViewState("HTMLonKeyUpScript") = value
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

        BintTagButtons()

        With Me.Resource
            .setLabel(LBLalignment_t)
            .setLabel(LBLtype_t)

            .setLabel(LBLpageTag_t)

            .setRadioButtonList(RBLalignment, "-1")
            .setRadioButtonList(RBLalignment, "0")
            .setRadioButtonList(RBLalignment, "1")

            .setRadioButtonList(RBLtype, "none")
            .setRadioButtonList(RBLtype, "txt")
            .setRadioButtonList(RBLtype, "img")

            .setLabel(LBLcode_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Private Sub BindNoTxt()
        Me.CTRLvisualEditorText.HTML = ""
    End Sub
    Private Sub BindNoImage()
        Me.UCimage.Init()
    End Sub
    Private Sub BintTagButtons()
        Try
            Me.BTNpgtagCurPage.Text = Me.Resource.getValue("PageTag.CurPage")
        Catch ex As Exception
            Me.BTNpgtagCurPage.Text = Me.Resource.getValue("CurrentPage")
        End Try

        Me.BTNpgtagCurPage.CommandArgument = Item.Id.ToString
        Me.BTNpgtagCurPage.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.PageNumberCurrent)


        Try
            Me.BTNpgtagCreateDate.Text = Me.Resource.getValue("PageTag.CreateDate")
        Catch ex As Exception
            Me.BTNpgtagCreateDate.Text = Me.Resource.getValue("Creation Date")
        End Try

        Me.BTNpgtagCreateDate.CommandArgument = Item.Id.ToString
        Me.BTNpgtagCreateDate.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.CreateDate)

        Try
            Me.BTNpgtagCreateTime.Text = Me.Resource.getValue("PageTag.CreateTime")
        Catch ex As Exception
            Me.BTNpgtagCreateTime.Text = Me.Resource.getValue("Creation Time")
        End Try

        Me.BTNpgtagCreateTime.CommandArgument = Item.Id.ToString
        Me.BTNpgtagCreateTime.Attributes.Add("rel", lm.Comol.Core.DomainModel.Helpers.Export.TagReplacer.CreateTime)
    End Sub
    Public Sub SetTemplteVersion(ByVal TemplateId As Int64, ByVal VersionId As Int64)
        Me.UCimage.TemplateId = TemplateId
        Me.UCimage.VersionId = VersionId
    End Sub
#End Region

#Region "Handler"
    Private Sub RBLtype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Select Case RBLtype.SelectedValue
            Case "none"
                Me.MLVtype.SetActiveView(Me.VIWnone)
            Case "txt"
                Me.MLVtype.SetActiveView(Me.VIWtext)

            Case "img"
                Me.MLVtype.SetActiveView(Me.VIWimage)
        End Select
    End Sub
#End Region

End Class