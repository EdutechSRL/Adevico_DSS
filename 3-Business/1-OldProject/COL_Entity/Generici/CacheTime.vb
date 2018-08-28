Public Class CacheTime
	Public Shared Function Scadenza2minuti() As TimeSpan
		Return New TimeSpan(0, 2, 0)
	End Function
	Public Shared Function Scadenza20minuti() As TimeSpan
		Return New TimeSpan(0, 20, 0)
	End Function
	Public Shared Function Scadenza30minuti() As TimeSpan
		Return New TimeSpan(0, 30, 0)
	End Function
	Public Shared Function Scadenza60minuti() As TimeSpan
		Return New TimeSpan(1, 0, 0)
	End Function
	Public Shared Function Scadenza12Ore() As TimeSpan
		Return New TimeSpan(12, 0, 0)
	End Function
	Public Shared Function ScadenzaGiornaliera() As TimeSpan
		Return New TimeSpan(1, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaSettimanale() As TimeSpan
		Return New TimeSpan(7, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaQuindicinale() As TimeSpan
		Return New TimeSpan(15, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaMensile() As TimeSpan
		Return New TimeSpan(30, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaBimestrale() As TimeSpan
		Return New TimeSpan(60, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaTrimestrale() As TimeSpan
		Return New TimeSpan(90, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaSemestrale() As TimeSpan
		Return New TimeSpan(180, 0, 0, 0)
	End Function
	Public Shared Function ScadenzaAnnuale() As TimeSpan
		Return New TimeSpan(365, 0, 0, 0)
	End Function

End Class