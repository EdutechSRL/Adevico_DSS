Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerDashBoard
        Inherits COL_BusinessLogic_v2.ObjectBase
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Remote"
        'Private _ServiceManagement As WSremoteManagement.NotificationManagementSoapClient
        'Private ReadOnly Property ServiceManagement() As WSremoteManagement.NotificationManagementSoapClient
        '    Get
        '        If IsNothing(_ServiceManagement) Then
        '            _ServiceManagement = New WSremoteManagement.NotificationManagementSoapClient
        '        End If
        '        Return _ServiceManagement
        '    End Get
        'End Property
#End Region

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub



    End Class
End Namespace