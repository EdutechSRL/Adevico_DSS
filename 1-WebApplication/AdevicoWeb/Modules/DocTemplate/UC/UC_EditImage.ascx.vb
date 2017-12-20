Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports DTHelpers = lm.Comol.Core.BaseModules.DocTemplate.Helpers

Public Class UC_EditImage
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property TemplateId As Integer
        Get
            Dim TmplId As Int64 = 0
            Try
                TmplId = Me.ViewState("CurTmplId")
            Catch ex As Exception
            End Try

            Return TmplId
        End Get
        Set(value As Integer)
            Me.ViewState("CurTmplId") = value
        End Set
    End Property
    Public Property VersionId As Integer
        Get
            Dim VrsId As Int64 = 0
            Try
                VrsId = Me.ViewState("CurVrsId")
            Catch ex As Exception
            End Try
            Return VrsId
        End Get
        Set(value As Integer)
            Me.ViewState("CurVrsId") = value
        End Set
    End Property
    Public ReadOnly Property ImagePath As String
        Get
            Return Me.HIDimgPath.Value
        End Get
    End Property
    Public ReadOnly Property Width As Single
        Get
            Return System.Convert.ToInt16(UCimgWidth.Px)
        End Get
    End Property
    Public ReadOnly Property Height As Single
        Get
            Return System.Convert.ToInt16(UCimgHeight.Px)
        End Get
    End Property
    'Public Property ShowMeasure As Boolean
    '    Get
    '        Return Me.UCimgHeight.Visible
    '    End Get
    '    Set(value As Boolean)
    '        Me.UCimgHeight.Visible = value
    '        Me.UCimgWidth.Visible = value
    '    End Set
    'End Property
    Public ReadOnly Property TemplateBaseUrl As String
        Get
            Return Me.FullBaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    Public ReadOnly Property TemplateTmpBaseUrl As String
        Get
            Dim Url As String = MyBase.SystemSettings.DocTemplateSettings.BaseUrl & "/" & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder & "/"

            Return Me.FullBaseUrl & Url.Replace("\", "/").Replace("\\", "/") '
        End Get
    End Property
    Public ReadOnly Property FullBaseUrl() As String
        Get
            Return Me.Request.Url.AbsoluteUri.Replace( _
             Me.Request.Url.PathAndQuery, "") + Me.BaseUrl
        End Get
    End Property
    Public ReadOnly Property TemplateTmpFolder As String
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & "\" & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder & "\").Replace("/", "\").Replace("\\", "\")
        End Get
    End Property

    Public Property ShowMeasure As Boolean
        Get
            Return PNLimgSize.Visible
        End Get
        Set(value As Boolean)
            PNLimgSize.Visible = value
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
        UCimgWidth.Label = Me.Resource.getValue("UCimgWidth.Label")
        UCimgHeight.Label = Me.Resource.getValue("UCimgHeight.Label")
        Me.Resource.setLinkButton(LKBaddImageFile, True, True, False, False)
        Me.Resource.setLabel(LBaddNewImage_t)
    End Sub
#End Region

#Region "Internal"

#Region "Init"

    ''' <summary>
    ''' Inizializza il controllo.
    ''' </summary>
    ''' <param name="ImagePath">Nome immagine: Guid.ext</param>
    ''' <param name="Width">Larghezza in pixel.</param>
    ''' <param name="Height">Altezza in pixel.</param>
    ''' <remarks>Impostare ID TEMPLATE ed ID VERSION PRIMA di usare questo metodo!</remarks>
    Public Sub Init(ByVal ImagePath As String, ByVal Width As Short, ByVal Height As Short, ByVal TestSource As String)

        If (Me.TemplateId <= 0 OrElse Me.VersionId <= 0) Then
            Throw New ArgumentException("Tamplete and Version ID cannot be null or <= 0. Set it befor run this sub! Source " & TestSource)
        End If

        Me.HIDimgPath.Value = ImagePath

        If (ImagePath.StartsWith("#")) Then
            ImagePath = ImagePath.Remove(0, 1)
            Me.IMGpreview.ImageUrl = lm.Comol.Core.DomainModel.DocTemplateVers.Business.ImageHelper.GetImageUrl(ImagePath, TemplateTmpBaseUrl, Me.TemplateId, Me.VersionId)
        Else
            Me.IMGpreview.ImageUrl = TemplateVers.Business.ImageHelper.GetImageUrl(ImagePath, TemplateBaseUrl, TemplateId, VersionId)
        End If

        Me.IMGpreview.CssClass = "OldImage"


        Me.UCimgWidth.Px = Width
        Me.UCimgWidth.SetMeasure(UC_Measure.Units.px)

        If (Width > 0) Then
            Me.IMGpreview.Width = Width
        ElseIf Not IsNothing(Me.IMGpreview.Attributes("width")) Then
            Me.IMGpreview.Attributes().Remove("width")
        End If

        If (Height > 0) Then
            Me.IMGpreview.Height = Height
        ElseIf Not IsNothing(Me.IMGpreview.Attributes("height")) Then
            Me.IMGpreview.Attributes().Remove("height")
        End If

        Me.UCimgHeight.Px = Height
        Me.UCimgHeight.SetMeasure(UC_Measure.Units.px)

        Me.SetInternazionalizzazione()

    End Sub
    ''' <summary>
    ''' Inizializza il controllo.
    ''' </summary>
    ''' <param name="TemplateId">L'ID del relativo Template.</param>
    ''' <param name="VersionId">L'ID della relativa versione</param>
    ''' <param name="ImagePath">Nome immagine: Guid.ext</param>
    ''' <param name="Width">Larghezza in pixel.</param>
    ''' <param name="Height">Altezza in pixel.</param>
    ''' <remarks></remarks>
    Public Sub Init(ByVal TemplateId As Int64, ByVal VersionId As Int64, ByVal ImagePath As String, ByVal Width As Short, ByVal Height As Short)
        Me.TemplateId = TemplateId
        Me.VersionId = VersionId

        Me.HIDimgPath.Value = ImagePath

        Me.IMGpreview.ImageUrl = TemplateVers.Business.ImageHelper.GetImageUrl(ImagePath, TemplateBaseUrl, TemplateId, VersionId)
        Me.IMGpreview.CssClass = "OldImage"

        If (Width > 0) Then
            Me.IMGpreview.Width = Width
        ElseIf Not IsNothing(Me.IMGpreview.Attributes("width")) Then
            Me.IMGpreview.Attributes().Remove("width")
        End If
        Me.UCimgWidth.Px = Width
        Me.UCimgWidth.SetMeasure(UC_Measure.Units.px)


        If (Height > 0) Then
            Me.IMGpreview.Height = Height
        ElseIf Not IsNothing(Me.IMGpreview.Attributes("height")) Then
            Me.IMGpreview.Attributes().Remove("height")
        End If

        Me.UCimgHeight.Px = Height
        Me.UCimgHeight.SetMeasure(UC_Measure.Units.px)
    End Sub
    Public Sub Init()
        Me.HIDimgPath.Value = ImagePath
        Me.IMGpreview.ImageUrl = ""
        Me.IMGpreview.CssClass = "NoImage"
        Me.UCimgWidth.Px = 0
        Me.UCimgWidth.SetMeasure(UC_Measure.Units.px)
        Me.UCimgHeight.Px = 0
        Me.UCimgHeight.SetMeasure(UC_Measure.Units.px)
    End Sub
#End Region

#End Region

#Region "Handler"
    Private Sub LKBaddImageFile_Click(sender As Object, e As System.EventArgs) Handles LKBaddImageFile.Click
        Dim fileName As String = ""
        If (Not String.IsNullOrEmpty(Me.FUPimageFile.FileName)) Then

            Dim PrevFile As String = Me.HIDimgPath.Value
            If Not String.IsNullOrEmpty(PrevFile) AndAlso PrevFile.StartsWith("#") Then

                DTHelpers.DocTempalteFileHelper.DeleteFile(TemplateVers.Business.ImageHelper.GetImagePath(PrevFile.Replace("#", ""), Me.TemplateTmpFolder, Me.TemplateId, Me.VersionId))
            End If

            fileName = DTHelpers.DocTempalteFileHelper.UploadFile(Me.FUPimageFile, TemplateVers.Business.ImageHelper.GetImagePath("", Me.TemplateTmpFolder, Me.TemplateId, Me.VersionId))

            If (fileName.StartsWith("ErrorCode.")) Then
                Me.HIDimgPath.Value = ""
            Else

                Me.HIDimgPath.Value = "#" & fileName
                Me.IMGpreview.ImageUrl = lm.Comol.Core.DomainModel.DocTemplateVers.Business.ImageHelper.GetImageUrl(fileName, TemplateTmpBaseUrl, Me.TemplateId, Me.VersionId)

                Me.IMGpreview.CssClass = "NewImage"

                If (Me.UCimgWidth.Px > 0) Then
                    Me.IMGpreview.Width = Me.UCimgWidth.Px
                ElseIf Not IsNothing(Me.IMGpreview.Attributes("width")) Then
                    Me.IMGpreview.Attributes().Remove("width")
                    Me.UCimgWidth.Px = 0

                End If

                If (Me.UCimgHeight.Px > 0) Then
                    Me.IMGpreview.Height = Me.UCimgHeight.Px
                ElseIf Not IsNothing(Me.IMGpreview.Attributes("height")) Then
                    Me.IMGpreview.Attributes().Remove("height")
                    Me.UCimgHeight.Px = 0

                End If

                Me.IMGpreview.Visible = True

                Me.UCimgHeight.SetMeasure(True, UC_Measure.Units.px)
                Me.UCimgWidth.SetMeasure(True, UC_Measure.Units.px)
            End If
        End If
    End Sub
#End Region

End Class