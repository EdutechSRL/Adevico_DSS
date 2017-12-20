Imports lm.Comol.Core.BaseModules.Web
Imports lm.Comol.Core.BaseModules.Web.Controls
Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class PreviewSkin
    Inherits PageBase
    Implements IViewSkinPreview


#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleSkinPreviewPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleSkinPreviewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSkinPreviewPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadedIdCommunity As Integer Implements IViewSkinPreview.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItem As Long Implements IViewSkinPreview.PreloadedIdItem
        Get
            If IsNumeric(Request.QueryString("idItem")) Then
                Return CLng(Request.QueryString("idItem"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItemType As SkinDisplayType Implements IViewSkinPreview.PreloadedIdItemType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SkinDisplayType).GetByString(Request.QueryString("itemType"), SkinDisplayType.Portal)
        End Get
    End Property
    Private ReadOnly Property PreloadedIdModule As Integer Implements IViewSkinPreview.PreloadedIdModule
        Get
            If IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property

    Protected Property CurrentModule As String Implements IViewSkinPreview.CurrentModule
        Get
            Return ViewStateOrDefault("CurrentModule", "")
        End Get
        Set(value As String)
            ViewState("CurrentModule") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(PreloadedIdModule, PreloadedIdCommunity, PreloadedIdItem, PreloadedIdItemType)
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle.Preview.text")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ToolTip.text")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeSkin(dto As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) Implements IViewSkinPreview.InitializeSkin
        Dim type As SkinDisplayType = PreloadedIdItemType
        Select Case type

            Case SkinDisplayType.Portal, SkinDisplayType.NotAssociated, SkinDisplayType.Module
                dto.Title = Resource.getValue("Preview.SkinDisplayType." & type.ToString)

            Case Else
                dto.Title = String.Format(Resource.getValue("Preview.SkinDisplayType." & type.ToString), dto.Title)
        End Select
        Me.Master.InitializeMasterForPreview(dto)
    End Sub
    Public Sub DisplayContentByService(moduleCode As String) Implements IViewSkinPreview.DisplayContentByService
        Me.DVstandard.Visible = (moduleCode = "")
        Me.DVmodules.Visible = Not (moduleCode = "")
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewSkinPreview.DisplaySessionTimeout

        Me.BindNoPermessi()
    End Sub

#End Region


    
End Class