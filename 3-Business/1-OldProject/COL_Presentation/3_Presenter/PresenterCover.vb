Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer


Public Class PresenterCover
	Inherits GenericPresenter
	Private _Cover As COL_Cover

	
	Private ReadOnly Property Cover() As COL_Cover
		Get
			Dim oCover As COL_Cover
			If Me.View.isModalitaAmministrazione Then
				oCover = COL_Cover.GetFromComunita(Me.View.ComunitaCorrenteID)
			Else
				oCover = COL_Cover.GetFromComunita(Me.View.ComunitaCorrenteID)
			End If
			Cover = oCover
		End Get
	End Property
	Public Sub New(ByVal view As IviewGeneric)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewCover
		Get
			View = MyBase.view
		End Get
	End Property

	Public Sub Init()
		Me.View.CoverComunita = Me.Cover
		Try
			If Me.View.TipoPersonaID = Main.TipoPersonaStandard.Guest Then
				Me.View.SkipCover = False
			ElseIf Me.View.isModalitaAmministrazione Then
				Me.View.SkipCover = COL_RuoloPersonaComunita.isSkipCover(Me.View.AmministrazioneComunitaID, Me.View.UtenteCorrente.Id)
				Me.View.VisibilitaSkip = False
			Else
				Me.View.SkipCover = COL_RuoloPersonaComunita.isSkipCover(Me.View.ComunitaCorrenteID, Me.View.UtenteCorrente.Id)
			End If
		Catch ex As Exception

		End Try

	End Sub

	Public Sub Edit()
		Me.View.ShowManagement(Me.Cover)
	End Sub

	Public Sub Save(Optional ByVal ShowCover As Boolean = False)
        Dim oCover As New COL_Cover
        Dim isNewCover As Boolean = False
        Dim HasError As Boolean = False
		oCover = Me.View.CoverComunita
		If oCover.Id > 0 Then
			oCover.Modifica()
        Else
            isNewCover = True
            oCover.Aggiungi()
		End If
		If oCover.ErroreDB = Errori_Db.None Then
			Dim oPage As ServicePage = Me.View.PaginaServizio
			If Not IsNothing(oPage) Then
				If oPage.ID > 0 Then
					oCover.SetDefaultPage(oPage)
				End If
			End If
			Dim Immagine As String = Me.View.SaveImageFile(oCover.Immagine)

			If Immagine = oCover.Immagine And ShowCover Then
                Me.View.ShowCover(oCover)
            ElseIf Immagine <> "" Then
                Dim OldImage As String = ""
                OldImage = oCover.Immagine
                oCover.UpDateImmagine(Me.View.DestinationPath, Immagine)
                If oCover.ErroreDB = Errori_Db.None Then
                    If ShowCover Then
                        Me.View.ShowCover(oCover)
                    Else
                        Me.View.ShowErrorEditing(IviewCover.CoverError.None)
                    End If
                    'If OldImage <> "" Then
                    '	Me.View.DeleteImageFile(OldImage)
                    'End If
                Else
                    Me.View.ShowErrorEditing(IviewCover.CoverError.ImageNotChanged)
                End If
			End If
        ElseIf oCover.Id > 0 Then
            HasError = True
            Me.View.ShowErrorEditing(IviewCover.CoverError.NotChanged)
		Else
			Me.View.ShowErrorEditing(IviewCover.CoverError.ImageNotUploaded)
        End If
        If HasError = False Then
            If isNewCover Then
                Me.View.SendAddNotification(oCover.Id)
            Else
                Me.View.SendEditNotification(oCover.Id)
            End If
        End If
	End Sub
	Public Sub Show()
		Me.View.CoverComunita = Me.Cover
	End Sub
	Public Function GetDefaultPage() As String
		Dim ComunitaID As Integer
		Dim oPage As ServicePage
		If Me.View.isModalitaAmministrazione Then
			ComunitaID = Me.View.AmministrazioneComunitaID
		Else
			ComunitaID = Me.View.ComunitaCorrenteID
		End If

		oPage = COL_Comunita.LazyGetCurrentDefaultPage(ComunitaID)

		If IsNothing(oPage) Then
			Return Me.View.PaginaDefault
		Else
			Dim Redirigi As Boolean = False
			'Redirigi = CanRedirectToDefaultPage(oPage.Servizio.Code, ComunitaID, Me.View.UtenteCorrente.Id)
			If Redirigi Then
				Return oPage.Url
			Else
				Return Me.View.PaginaDefault
			End If
		End If
	End Function

	Public Sub SetSkipCover(ByVal Skip As Boolean)
		Dim ComunitaID As Integer

		If Me.View.isModalitaAmministrazione Then
			ComunitaID = Me.View.AmministrazioneComunitaID
		Else
			ComunitaID = Me.View.ComunitaCorrenteID
		End If
		If Me.View.TipoPersonaID <> Main.TipoPersonaStandard.Guest Then
			COL_RuoloPersonaComunita.skipCover(ComunitaID, Me.View.UtenteCorrente.Id, Skip)
		End If
	End Sub
End Class