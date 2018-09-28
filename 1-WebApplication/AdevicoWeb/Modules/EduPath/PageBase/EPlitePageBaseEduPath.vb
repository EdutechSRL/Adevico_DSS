Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Coordinamento
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public MustInherit Class EPlitePageBaseEduPath
    Inherits PageBase


#Region "Context"
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property

    Protected _serviceAssignment As ServiceAssignment
    Protected ReadOnly Property ServiceAssignment As ServiceAssignment
        Get
            If IsNothing(_serviceAssignment) Then
                _serviceAssignment = ServiceEP.ServiceAssignments
            End If
            Return _serviceAssignment
        End Get
    End Property
    Protected _ServiceStat As ServiceStat
    Protected ReadOnly Property ServiceStat As ServiceStat
        Get
            If IsNothing(_ServiceStat) Then
                _ServiceStat = ServiceEP.ServiceStat
            End If
            Return _ServiceStat
        End Get
    End Property

    Private _ServiceCertification As lm.Comol.Core.Certifications.Business.CertificationsService
    Protected ReadOnly Property ServiceCF As lm.Comol.Core.Certifications.Business.CertificationsService
        Get
            If IsNothing(_ServiceCertification) Then
                _ServiceCertification = New lm.Comol.Core.Certifications.Business.CertificationsService(PageUtility.CurrentContext)
            End If
            Return _ServiceCertification
        End Get
    End Property
#End Region

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIsMooc() As Boolean
        Get
            Dim isMoocObject As Boolean = False
            Boolean.TryParse(Request.QueryString("isMooc"), isMoocObject)
            Return isMoocObject
        End Get
    End Property

#End Region

#Region "QueryString"
    Public Function GetQuery(key As String, defvalue As String) As String
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As String = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Int32) As Int32
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Int32 = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Int64) As Int64
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Int64 = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Boolean) As Boolean
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Boolean = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
#End Region


#Region "Internal"
    Protected _IsMoocPath As Boolean?
    Protected Property IsMoocPath() As Boolean
        Get
            If _IsMoocPath.HasValue Then
                Return _IsMoocPath.Value
            Else
                Return ViewStateOrDefault("IsMoocPath", False)
            End If
        End Get
        Set(value As Boolean)
            _IsMoocPath = value
            ViewState("IsMoocPath") = value
        End Set
    End Property
    Public Function GetCssFileByType() As String
        Dim isMoocPath = PreloadIsMooc
        If _IsMoocPath.HasValue Then
            isMoocPath = _IsMoocPath
        End If
        If _IsMoocPath Then
            Return "mooc-"
        Else
            Return "mooc-"
        End If
    End Function
#End Region
End Class