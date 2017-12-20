Imports COL_BusinessLogic_v2

Public Interface IviewCalcoloSpazioDisco

	ReadOnly Property Presenter() As PresenterCalcoloSpazioDisco
	ReadOnly Property DrivePath() As String
	WriteOnly Property BarraSezione25() As Integer
	WriteOnly Property BarraSezione50() As Integer
	WriteOnly Property BarraSezione75() As Integer
	WriteOnly Property BarraSezione100() As Integer
	WriteOnly Property BarraSezione150() As Integer
	WriteOnly Property TestoSezioneOver() As String
	Sub SetDisplayInfo(ByVal SizeUsed As Double, ByVal SizeAvailable As Int64)
	Property ComunitaID() As Integer
	Property AreaID() As Integer
	Property oConfigType() As ConfigFileType
	ReadOnly Property MaxSize() As Integer
	ReadOnly Property BarraUnitaBase() As Integer
End Interface