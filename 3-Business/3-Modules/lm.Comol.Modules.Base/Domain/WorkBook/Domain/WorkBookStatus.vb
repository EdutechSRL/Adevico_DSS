Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookStatus
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of Integer)

#Region "Private"
        Private _PermissionToEdit As EditingPermission
        Private _AvailableFor As EditingPermission
        Private _Name As String
        Private _Translations As IList(Of WorkBookStatusTraslations)
        Private _IsDefault As Boolean
#End Region

        Public Overridable Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Overridable Property DefaultPermissionToEdit() As EditingPermission
            Get
                Return _PermissionToEdit
            End Get
            Set(ByVal value As EditingPermission)
                _PermissionToEdit = value
            End Set
        End Property
        Public Overridable Property AvailableFor() As EditingPermission
            Get
                Return _AvailableFor
            End Get
            Set(ByVal value As EditingPermission)
                _AvailableFor = value
            End Set
        End Property
        Public Overridable Property Translations() As IList(Of WorkBookStatusTraslations)
            Get
                Return _Translations
            End Get
            Set(ByVal value As IList(Of WorkBookStatusTraslations))
                _Translations = value
            End Set
        End Property
        Public Overridable Property IsDefault() As Boolean
            Get
                Return _IsDefault
            End Get
            Set(ByVal value As Boolean)
                _IsDefault = value
            End Set
        End Property
        Sub New()
            _IsDefault = False
            _PermissionToEdit = EditingPermission.Owner OrElse EditingPermission.Authors OrElse EditingPermission.ModuleManager OrElse EditingPermission.Responsible
            _AvailableFor = EditingPermission.Owner OrElse EditingPermission.Authors OrElse EditingPermission.ModuleManager OrElse EditingPermission.Responsible
        End Sub
        Sub New(ByVal StatusID As Integer)
            MyBase.Id = StatusID
            _IsDefault = False
            _PermissionToEdit = EditingPermission.Owner OrElse EditingPermission.Authors OrElse EditingPermission.ModuleManager OrElse EditingPermission.Responsible
            _AvailableFor = EditingPermission.Owner OrElse EditingPermission.Authors OrElse EditingPermission.ModuleManager OrElse EditingPermission.Responsible
        End Sub
    End Class
End Namespace