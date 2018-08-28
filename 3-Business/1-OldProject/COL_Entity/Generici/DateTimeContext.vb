<Serializable(), CLSCompliant(True)> Public Class DateTimeContext
#Region "Private property"
	Private _Date As DateTime
	Private _Day As Integer
	Private _Month As Integer
	Private _Year As Integer
	Private _Hour As Integer
	Private _Minutes As Integer
	Private _Seconds As Integer
#End Region

#Region "Public property"
	Public Property Value() As DateTime
		Get
			Return _Date
		End Get
		Set(ByVal value As DateTime)
			_Date = value
		End Set
	End Property
	Public Property Day() As Integer
		Get
			Return _Day
		End Get
		Set(ByVal value As Integer)
			_Day = value
		End Set
	End Property
	Public Property Month() As Integer
		Get
			Return _Month
		End Get
		Set(ByVal value As Integer)
			_Month = value
		End Set
	End Property
	Public Property Year() As Integer
		Get
			Return _Year
		End Get
		Set(ByVal value As Integer)
			_Year = value
		End Set
	End Property
	Public Property Hour() As Integer
		Get
			Return _Hour
		End Get
		Set(ByVal value As Integer)
			_Hour = value
		End Set
	End Property
	Public Property Minutes() As Integer
		Get
			Return _Minutes
		End Get
		Set(ByVal value As Integer)
			_Minutes = value
		End Set
	End Property
	Public Property Seconds() As Integer
		Get
			Return _Seconds
		End Get
		Set(ByVal value As Integer)
			_Seconds = value
		End Set
	End Property
#End Region

	Public Sub New()
		Me._Day = -1
		Me._Hour = -1
		Me._Minutes = -1
		Me._Month = -1
		Me._Year = -1
		Me._Seconds = -1
	End Sub

	Public Shared Function CreateByDate(ByVal oDate As DateTime) As DateTimeContext
		Dim oDateTimeContext As New DateTimeContext
		oDateTimeContext.Day = oDate.Day
		oDateTimeContext.Month = oDate.Month
		oDateTimeContext.Year = oDate.Year
		oDateTimeContext.Value = oDate


		Return oDateTimeContext
	End Function
	Public Shared Function CreateByDateTime(ByVal oDate As DateTime) As DateTimeContext
		Dim oDateTimeContext As New DateTimeContext
		oDateTimeContext.Day = oDate.Day
		oDateTimeContext.Month = oDate.Month
		oDateTimeContext.Year = oDate.Year
		oDateTimeContext.Hour = oDate.Hour
		oDateTimeContext.Minutes = oDate.Minute
		oDateTimeContext.Seconds = oDate.Second
		oDateTimeContext.Value = oDate

		Return oDateTimeContext
	End Function
End Class