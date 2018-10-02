Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleWorkBookManagement
        Private adapter As ModuleNotificationManagementAdapter
        Private _EditStatus As Boolean
        Private _DeleteStatus As Boolean
        Private _AddStatus As Boolean
        Private _ManagementPermission As Boolean
        Private _Administration As Boolean
        Private _ListStatus As Boolean
        Public Property EditStatus() As Boolean
            Get
                Return _EditStatus
            End Get
            Set(ByVal value As Boolean)
                _EditStatus = value
            End Set
        End Property
        Public Property AddStatus() As Boolean
            Get
                Return _AddStatus
            End Get
            Set(ByVal value As Boolean)
                _AddStatus = value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                Return _ManagementPermission
            End Get
            Set(ByVal value As Boolean)
                _ManagementPermission = value
            End Set
        End Property
        Public Property Administration() As Boolean
            Get
                Return _Administration
            End Get
            Set(ByVal value As Boolean)
                _Administration = value
            End Set
        End Property
        Public Property DeleteStatus() As Boolean
            Get
                Return _DeleteStatus
            End Get
            Set(ByVal value As Boolean)
                _DeleteStatus = value
            End Set
        End Property
        Public Property ListStatus() As Boolean
            Get
                Return _ListStatus
            End Get
            Set(ByVal value As Boolean)
                _ListStatus = value
            End Set
        End Property
        Public Sub New()
            Me._EditStatus = False
            Me._AddStatus = False
            Me._Administration = False
            Me._ManagementPermission = False
        End Sub
        Public Sub New(ByVal s As Services_WorkBook)
            adapter = New ModuleNotificationManagementAdapter(s)
            adapter.Initialize(Me)
        End Sub

        Private Class ModuleNotificationManagementAdapter
            Private _service As Services_WorkBook

            Public Sub New(ByVal s As Services_WorkBook)
                Me._service = s
            End Sub

            Public Sub Initialize(ByVal m As ModuleWorkBookManagement)
                With m
                    .Administration = _service.Admin
                    .EditStatus = _service.Admin
                    .ManagementPermission = _service.Admin
                    .AddStatus = _service.Admin
                    .DeleteStatus = _service.Admin
                    .ListStatus = _service.Admin
                End With
            End Sub
        End Class
    End Class

End Namespace