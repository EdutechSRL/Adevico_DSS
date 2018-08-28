Imports Comol.Entity.File
Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class QuizSettings

#Region "Private properties"
		Private _SessionTimeout As Integer
		Private _QuestionForPage As Integer
		Private _MaxSessionAliveTick As Integer
		Private _AutoSave As Integer
		Private _OvertimeSave As Integer
		Private _ItemForDropDown As Integer
		Private _popUpHeight As String
		Private _popUpWidth As String
		Private _RowItemsForPage As Int64
        Private _MaxDouble As Double
        Private _DefaultGroupName As String
        Private _DefaultScalaValutazione As Integer
		Private _Urls As New List(Of URLelement)
#End Region

#Region "Public properties"
		Public Property SessionTimeout() As Integer
			Get
				Return _SessionTimeout
			End Get
			Set(ByVal value As Integer)
				_SessionTimeout = value
			End Set
		End Property
		Public Property QuestionForPage() As Integer
			Get
				Return _QuestionForPage
			End Get
			Set(ByVal value As Integer)
				_QuestionForPage = value
			End Set
		End Property
        Public Property DefaultScalaValutazione() As Integer
            Get
                Return _DefaultScalaValutazione
            End Get
            Set(ByVal value As Integer)
                _DefaultScalaValutazione = value
            End Set
        End Property
        Public Property MaxSessionAliveTick() As Integer
            Get
                Return _MaxSessionAliveTick
            End Get
            Set(ByVal value As Integer)
                _MaxSessionAliveTick = value
            End Set
        End Property
        Public Property AutoSave() As Integer
            Get
                Return _AutoSave
            End Get
            Set(ByVal value As Integer)
                _AutoSave = value
            End Set
        End Property
        Public Property OvertimeSave() As Integer
            Get
                Return _OvertimeSave
            End Get
            Set(ByVal value As Integer)
                _OvertimeSave = value
            End Set
        End Property
        Public Property ItemForDropDown() As Integer
            Get
                Return _ItemForDropDown
            End Get
            Set(ByVal value As Integer)
                _ItemForDropDown = value
            End Set
        End Property
        Public Property PopUpHeight() As String
            Get
                Return _popUpHeight
            End Get
            Set(ByVal value As String)
                _popUpHeight = value
            End Set
        End Property
        Public Property PopUpWidth() As String
            Get
                Return _popUpWidth
            End Get
            Set(ByVal value As String)
                _popUpWidth = value
            End Set
        End Property
        Public Property RowItemsForPage() As Int64
            Get
                Return _RowItemsForPage
            End Get
            Set(ByVal value As Int64)
                _RowItemsForPage = value
            End Set
        End Property
        Public Property MaxDoubleSize() As Double
            Get
                Return _MaxDouble
            End Get
            Set(ByVal value As Double)
                _MaxDouble = value
            End Set
        End Property
        Public Property DefaultGroupName() As String
            Get
                Return _DefaultGroupName
            End Get
            Set(ByVal value As String)
                _DefaultGroupName = value
            End Set
        End Property
        Public Property Urls() As List(Of URLelement)
            Get
                Return _Urls
            End Get
            Set(ByVal value As List(Of URLelement))
                _Urls = value
            End Set
        End Property
#End Region

        Sub New()

        End Sub
        Public Function GetUrl(ByVal Name As String) As URLelement
            If Me._Urls.Count = 0 Then
                Return Nothing
            Else
                Return Me._Urls.Find(New GenericPredicate(Of URLelement, String)(Name, AddressOf URLelement.FindByName))
            End If
        End Function
    End Class
End Namespace