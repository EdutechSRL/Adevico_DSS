Public Class HistoryElement
	Private _ComunitaID As String
	Private _Nome As String
	Private _Percorso As String
	Private _RuoloID As Integer
	Private _PadreID As Integer
	Private _AccessoAnonimo As Boolean
	Private _SubElement As HistoryElement

	Public Property ComunitaID() As Integer
		Get
			ComunitaID = _ComunitaID
		End Get
		Set(ByVal value As Integer)
			_ComunitaID = value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Nome = _Nome
		End Get
		Set(ByVal value As String)
			_Nome = value
		End Set
	End Property
	Public Property Percorso() As String
		Get
			Percorso = _Percorso
		End Get
		Set(ByVal value As String)
			_Percorso = value
		End Set
	End Property
	Public Property RuoloID() As Integer
		Get
			RuoloID = _RuoloID
		End Get
		Set(ByVal value As Integer)
			_RuoloID = value
		End Set
	End Property
	Public Property ComunitaPadreID() As Integer
		Get
			ComunitaPadreID = _PadreID
		End Get
		Set(ByVal value As Integer)
			_PadreID = value
		End Set
	End Property
	Public Property AccessoAnonimo() As Boolean
		Get
			AccessoAnonimo = _AccessoAnonimo
		End Get
		Set(ByVal value As Boolean)
			_AccessoAnonimo = value
		End Set
	End Property
	Public Property SubElement() As HistoryElement
		Get
			SubElement = _SubElement
		End Get
		Set(ByVal value As HistoryElement)
			_SubElement = value
		End Set
	End Property

	Public Sub New()
		_Nome = String.Empty
		_Percorso = String.Empty
		_RuoloID = Main.TipoRuoloStandard.AccessoNonAutenticato
		_SubElement = Nothing
		_PadreID = 0
		_AccessoAnonimo = False
	End Sub


	Public Sub New(ByVal ID As Integer, ByVal PadreID As Integer, ByVal Nome As String, ByVal Percorso As String, ByVal RuoloID As Integer, ByVal oElement As HistoryElement, ByVal AccessoAnonimo As Boolean)
		_ComunitaID = ID
		_Nome = Nome
		_Percorso = String.Empty
		_RuoloID = RuoloID
		_SubElement = oElement
		_PadreID = PadreID
		_AccessoAnonimo = AccessoAnonimo
	End Sub

	Public Function ElementsCount() As Integer
		Dim iTotale As Integer = 0
		Dim oElement As HistoryElement

		oElement = Me
		While Not IsNothing(oElement)
			iTotale += 1
			oElement = oElement.SubElement
		End While
		Return iTotale
	End Function

	Public Function GetSubElement(ByVal index As Integer) As HistoryElement
		Dim iTotale As Integer = 0
		Dim oElement As HistoryElement

		oElement = Me
		While Not IsNothing(oElement)
			If index = iTotale Then
				Return oElement
			End If
			iTotale += 1
			oElement = oElement.SubElement
		End While
		Return Nothing
	End Function
	Public Function GetElementByComunitaID(ByVal ComunitaID As Integer) As HistoryElement
		Dim oElement As HistoryElement

		oElement = Me
		While Not IsNothing(oElement)
			If oElement.ComunitaID = ComunitaID Then
				Return oElement
			End If
			oElement = oElement.SubElement
		End While
		Return Nothing
	End Function
	Public Function FindFatherElementByID(ByVal ComunitaID As Integer) As HistoryElement
		Dim oElement As HistoryElement

		oElement = Me
		While Not IsNothing(oElement)
			If oElement._ComunitaID = ComunitaID Then
				Return GetElementByComunitaID(oElement._PadreID)
			End If
			oElement = oElement.SubElement
		End While
		Return Nothing
	End Function
	Public Function ClearByComunitaID(ByVal ComunitaID As Integer) As HistoryElement
		If Me.ComunitaID = ComunitaID Then
			Return Nothing
		Else
			Dim oElement As HistoryElement = Me.SubElement
			While Not IsNothing(oElement.SubElement)
				If oElement.SubElement.ComunitaID = ComunitaID Then
					oElement.SubElement = Nothing
					Return Me
				End If
				oElement = oElement.SubElement
			End While
			Return Me
		End If
		Return Me
	End Function
	Public Function GetLastElement() As HistoryElement
		Return LoadSubElement(Me)
	End Function
	Public Function GetFirstElement() As HistoryElement
		Dim oElement As New HistoryElement
		With oElement
			.AccessoAnonimo = Me.AccessoAnonimo
			.ComunitaID = Me.ComunitaID
			.ComunitaPadreID = Me.ComunitaPadreID
			.Nome = Me.Nome
			.Percorso = Me.Percorso
			.RuoloID = Me.RuoloID
			.SubElement = Nothing
		End With

		Return oElement
	End Function

	Private Function LoadSubElement(ByVal oElement As HistoryElement) As HistoryElement
		If IsNothing(oElement.SubElement) Then
			Return oElement
		Else
			Return LoadSubElement(oElement.SubElement)
		End If
	End Function
End Class
