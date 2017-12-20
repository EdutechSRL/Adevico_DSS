'Public Class AP_BaseAuthenticationPages
'    Inherits PageBase
'    '  Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewBaseAuthenticationPages

'    '#Region "Implements"

'    '#Region "Preload"
'    '    Protected Friend ReadOnly Property PreloadIdCommunity As Long Implements IViewBaseAuthenticationPages.PreloadedIdSkin
'    '        Get
'    '            If IsNumeric(Me.Request.QueryString("idSkin")) Then
'    '                Return CInt(Me.Request.QueryString("idSkin"))
'    '            Else
'    '                Return -1
'    '            End If
'    '        End Get
'    '    End Property

'    '#End Region

'    '#End Region


'#Region "Inherits"
'    Public Overrides ReadOnly Property AlwaysBind As Boolean
'        Get
'            Return False
'        End Get
'    End Property

'    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
'        Get
'            Return False
'        End Get
'    End Property
'#End Region

'#Region "Inherits"
'    Public Overrides Function HasPermessi() As Boolean
'        Return True
'    End Function
'    Public Overrides Sub RegistraAccessoPagina()

'    End Sub
'    Public Overrides Sub SetCultureSettings()
'        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
'    End Sub
'    Public Overrides Sub ShowMessageToPage(errorMessage As String)

'    End Sub

'#End Region

'#Region "Implements"
'    'Public Function GetSkinInfoFromCookie() As Long Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewBaseAuthenticationPages.GetSkinInfoFromCookie

'    'End Function

'    'Public ReadOnly Property PreloadedIdSkin As Long Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewBaseAuthenticationPages.PreloadedIdSkin
'    '    Get

'    '    End Get
'    'End Property

'    'Public Sub WriteSkinInfoToCookie(idSkin As Long) Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewBaseAuthenticationPages.WriteSkinInfoToCookie

'    'End Sub
'#End Region

'End Class
