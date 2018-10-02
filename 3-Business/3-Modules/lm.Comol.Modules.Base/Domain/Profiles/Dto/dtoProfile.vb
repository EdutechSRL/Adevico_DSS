Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoProfile
        Inherits DomainObject(Of Integer)
#Region "Private Property"
        Private _Login As String
        Private _Name As String
        Private _Password As String
        Private _Surname As String
        Private _Mail As String
        Private _status As String
        Private _statusID As ProfileStatus
        Private _TypeID As Integer
        Private _AuthenticationTypeID As Integer
        Private _TaxCode As String
        Private _isDeletable As Boolean
        Private _AllowSendPassword As Boolean
        Private _AllowChangePassword As Boolean
        Private _TypeName As String
        Private _AuthenticationTypeName As String
        Private _OrganizationID As Integer
        Private _isDefaultOrganization As Boolean
#End Region

#Region "Public Property"
        Public Overridable Property TaxCode() As String
            Get
                Return _TaxCode
            End Get
            Set(ByVal value As String)
                _TaxCode = value
            End Set
        End Property
        Public Overridable Property AuthenticationTypeID() As Integer
            Get
                Return _AuthenticationTypeID
            End Get
            Set(ByVal value As Integer)
                _AuthenticationTypeID = value
            End Set
        End Property
        Public Overridable Property AuthenticationTypeName() As String
            Get
                Return _AuthenticationTypeName
            End Get
            Set(ByVal value As String)
                _AuthenticationTypeName = value
            End Set
        End Property
        Public Overridable Property TypeID() As Integer
            Get
                Return _TypeID
            End Get
            Set(ByVal value As Integer)
                _TypeID = value
            End Set
        End Property
        Public Overridable Property TypeName() As String
            Get
                Return _TypeName
            End Get
            Set(ByVal value As String)
                _TypeName = value
            End Set
        End Property
        Public Overridable Property Login() As String
            Get
                Return _Login
            End Get
            Set(ByVal value As String)
                _Login = value
            End Set
        End Property

        Public Overridable Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Overridable Property Surname() As String
            Get
                Return _Surname
            End Get
            Set(ByVal value As String)
                _Surname = value
            End Set
        End Property
        Public Overridable ReadOnly Property SurnameAndName() As String
            Get
                Return _Surname & " " & _Name
            End Get
        End Property
        Public Overridable Property Mail() As String
            Get
                Return _Mail
            End Get
            Set(ByVal value As String)
                _Mail = value
            End Set
        End Property
        Public Overridable Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property
        Public Overridable Property StatusID() As ProfileStatus
            Get
                Return _statusID
            End Get
            Set(ByVal value As ProfileStatus)
                _statusID = value
            End Set
        End Property
        Public Overridable Property isDeletable() As Boolean
            Get
                Return _isDeletable
            End Get
            Set(ByVal value As Boolean)
                _isDeletable = value
            End Set
        End Property
        Public Overridable Property AllowSendPassword() As Boolean
            Get
                Return _AllowSendPassword
            End Get
            Set(ByVal value As Boolean)
                _AllowSendPassword = value
            End Set
        End Property
        Public Overridable Property isInternalPassword() As Boolean
            Get
                Return _AllowChangePassword
            End Get
            Set(ByVal value As Boolean)
                _AllowChangePassword = value
            End Set
        End Property

        Public Overridable Property OrganizationID() As Integer
            Get
                Return _OrganizationID
            End Get
            Set(ByVal value As Integer)
                _OrganizationID = value
            End Set
        End Property


        Public Overridable Property isDefaultOrganization() As Boolean
            Get
                Return _isDefaultOrganization
            End Get
            Set(ByVal value As Boolean)
                _isDefaultOrganization = value
            End Set
        End Property
#End Region
        Public Sub New()

        End Sub

        Public Sub New(ByVal pId As Integer, ByVal pName As String, ByVal pSurname As String, ByVal pTypeID As Integer, ByVal pLogin As String, ByVal pTaxCode As String, ByVal pMail As String, ByVal oStatus As ProfileStatus, ByVal pAuthenticationTypeID As Integer, ByVal pIsdisabled As Boolean, ByVal pisDefault As Boolean, ByVal pOrganizationID As Integer)
            Me.Name = pName
            Me.isInternalPassword = (pAuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
            Me.AllowSendPassword = (pAuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
            Me.AuthenticationTypeID = pAuthenticationTypeID
            Me.AuthenticationTypeName = ""
            Me.isDeletable = True
            Me.Login = pLogin
            Me.Mail = pMail
            Me.StatusID = oStatus
            Me.Surname = pSurname
            Me.TaxCode = pTaxCode
            Me.TypeID = pTypeID
            Me.TypeName = ""
            Me.Status = ""
            Me.Id = pId
            If pIsdisabled = False Then
                Me.StatusID = ProfileStatus.Active
            Else
                Me.StatusID = ProfileStatus.Disabled
            End If
            isDefaultOrganization = pisDefault
            OrganizationID = pOrganizationID

        End Sub
        Public Sub New(ByVal oPerson As Person, ByVal oStatus As ProfileStatus)
            With oPerson
                Me.Name = .Name
                Me.isInternalPassword = (.AuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
                Me.AllowSendPassword = (.AuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
                Me.AuthenticationTypeID = .AuthenticationTypeID
                Me.AuthenticationTypeName = ""
                Me.isDeletable = True
                Me.Login = .Login
                Me.Mail = .Mail
                Me.StatusID = oStatus
                Me.Surname = .Surname
                Me.TaxCode = .TaxCode
                Me.TypeID = .TypeID
                Me.TypeName = ""
                Me.Status = ""
                Me.Id = oPerson.Id
            End With
        End Sub
        Public Sub New(ByVal oPerson As Person)
            With oPerson
                Me.Name = .Name
                Me.isInternalPassword = (.AuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
                Me.AllowSendPassword = (.AuthenticationTypeID = COL_BusinessLogic_v2.Main.TipoAutenticazione.ComunitaOnLine)
                Me.AuthenticationTypeID = .AuthenticationTypeID
                Me.AuthenticationTypeName = ""
                Me.isDeletable = True
                Me.Login = .Login
                Me.Mail = .Mail
                Me.Surname = .Surname
                Me.TaxCode = .TaxCode
                Me.TypeID = .TypeID
                Me.TypeName = ""
                Me.Status = ""
                Me.Id = oPerson.Id
            End With
        End Sub
        'Private Function HasPermission(ByVal CurrentUserTypeID As Integer, ByVal UserTypeId As Integer) As Boolean
        '    CurrentUserTypeID()

        'End Function
        Public Shared Function Generate(ByVal oDto As dtoProfile, ByVal pStatus As String, ByVal pAuthenticationTypeName As String, ByVal pTypeName As String) As dtoProfile
            oDto.Status = pStatus
            oDto.AuthenticationTypeName = pAuthenticationTypeName
            oDto.TypeName = pTypeName
            Return oDto
        End Function
    End Class
End Namespace