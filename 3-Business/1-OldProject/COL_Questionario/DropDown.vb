Imports Microsoft.VisualBasic

<Serializable()> Public Class DropDown
    Private _id As Integer
    Private _nome As String
    Private _etichetta As String
    Private _ordinata As Boolean
    Private _tipo As Integer
    Private _idDomanda As Integer
    Private _isMultipla As Boolean
    Private _dropDownItems As New list(Of DropDownItem)

    Public Enum TipoDropDown
        Normale = 0
        Condivisa = 1
        CampoEssay = 2
        CellaMatrice = 3
    End Enum

    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property isMultipla() As Boolean
        Get
            Return _isMultipla
        End Get
        Set(ByVal value As Boolean)
            _isMultipla = value
        End Set
    End Property

    Public Property etichetta() As String
        Get
            Return _etichetta
        End Get
        Set(ByVal value As String)
            _etichetta = value
        End Set
    End Property

    Public Property ordinata() As Boolean
        Get
            Return _ordinata
        End Get
        Set(ByVal value As Boolean)
            _ordinata = value
        End Set
    End Property

    Public Property tipo() As Integer
        Get
            Return _tipo
        End Get
        Set(ByVal value As Integer)
            _tipo = value
        End Set
    End Property

    Public Property idDomanda() As Integer
        Get
            Return _idDomanda
        End Get
        Set(ByVal value As Integer)
            _idDomanda = value
        End Set
    End Property

    Public Property dropDownItems() As list(Of DropDownItem)
        Get
            Return _dropDownItems
        End Get
        Set(ByVal value As list(Of DropDownItem))
            _dropDownItems = value
        End Set
    End Property

End Class
