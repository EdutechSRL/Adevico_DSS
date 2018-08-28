Namespace Configuration.Components
	<Serializable(), CLSCompliant(True)> Public Class TemplateTag

#Region "Private properties"
		Private _Name As String
		Private _Tag As String
		Private _Proprieta As String
        Private _isMandatory As Boolean
        Private _isInvalid As Boolean
        Private _Fase As Integer
#End Region

#Region "Public properties"
        Public ReadOnly Property Name() As String
            Get
                Name = _Name
            End Get
        End Property

        Public ReadOnly Property Tag() As String
            Get
                Tag = _Tag
            End Get
        End Property

        Public ReadOnly Property Proprieta() As String
            Get
                Proprieta = _Proprieta
            End Get
        End Property

        Public ReadOnly Property isMandatory() As Boolean
            Get
                isMandatory = _isMandatory
            End Get
        End Property

        Public ReadOnly Property isInvalid() As Boolean
            Get
                isInvalid = _isInvalid
            End Get
        End Property
        Public ReadOnly Property Fase() As Integer
            Get
                Fase = _Fase
            End Get
        End Property
#End Region

        Public Sub New(ByVal oNome As String, ByVal oTagValue As String, ByVal oTagProperty As String, ByVal oIsObligatory As Boolean, ByVal oFase As Integer)
            _Name = oNome
            _Tag = oTagValue
            _Proprieta = oTagProperty
            _isMandatory = oIsObligatory
            _Fase = oFase
            If _Tag = "" Then
                Me._isInvalid = True
            Else
                Me._isInvalid = False
            End If
        End Sub

		Public Shared Function FindByName(ByVal item As TemplateTag, ByVal iName As String) As Boolean
			Return item.Name = iName
		End Function

	End Class
End Namespace