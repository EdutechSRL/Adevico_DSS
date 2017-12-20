Imports COL_BusinessLogic_v2.UCServices

Public Interface IviewCover
	Inherits IviewBase

	Enum CoverError
		None = 0
		ImageNotUploaded
		ImageNotDeleted
		ImageNotChanged
		ImageNotResized
		NotChanged
		NotAdded
	End Enum

	ReadOnly Property Servizio() As Services_Cover
	ReadOnly Property PaginaDefault() As String
	Property CoverComunita() As COL_Cover
	Property SkipCover() As Boolean
	ReadOnly Property DestinationPath() As String
	Property VisibilitaSkip() As Boolean
	ReadOnly Property PaginaServizio() As ServicePage

	Sub ShowManagement(ByVal oCover As COL_Cover)
	Sub ShowCover(ByVal oCover As COL_Cover)
	'Sub SaveCover(ByVal oCover As COL_Cover)
	Sub ShowErrorEditing(ByVal Errore As CoverError)
	Function SaveImageFile(Optional ByVal ImmagineCorrente As String = "") As String
	Sub DeleteImageFile(ByVal Immagine As String)
    Sub SetEditImage(ByVal Immagine As String)
    Sub SendEditNotification(ByVal CoverID As Integer)
    Sub SendAddNotification(ByVal CoverID As Integer)
End Interface