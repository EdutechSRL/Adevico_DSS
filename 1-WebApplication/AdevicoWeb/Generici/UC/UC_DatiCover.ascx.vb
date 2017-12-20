Imports COL_BusinessLogic_v2.Comunita
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.Comol.Entities.Configuration

Partial Public Class UC_DatiCover
	Inherits BaseControlWithLoad

	Private _Cover As COL_Cover
	Private _ServiziDisponibili As List(Of ServicePage)
	Private _ObjectPath As ObjectFilePath


	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property


	Public Property CoverID() As Integer
		Get
			Try
				CoverID = DirectCast(Me.ViewState("CoverID"), Integer)
			Catch ex As Exception
				CoverID = 0
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("CoverID") = value
		End Set
	End Property

	Public Property CoverComunita() As COL_BusinessLogic_v2.COL_Cover
		Get
			CoverComunita = GetCover()
		End Get
		Set(ByVal value As COL_BusinessLogic_v2.COL_Cover)
			_Cover = value
			If IsNothing(value) Then
				Me.CTRLanno.Testo = ""
				Me.CTRLcommento.Testo = ""
				Me.CTRLdidascalia.Testo = ""
				Me.CTRLtitolo.Testo = ""
				Dim oComunita As New COL_Comunita

				oComunita.Id = Me.ComunitaLavoroID
				oComunita.Estrai()
				If oComunita.Errore = Errori_Db.None Then
					Me.CTRLtitolo.Testo = oComunita.Nome
                End If
				SetNoLogo()
				Me.CoverID = 0
			Else
				Bind_DatiCover()
			End If
			Me.DVerroreLogo.Style("display") = "none"
		End Set
	End Property
	Public Property PaginaServizio() As ServicePage
		Get
			Dim oPage As ServicePage

			If Me.DDLpagineDefault.SelectedIndex < 0 Then
				PaginaServizio = Nothing
			Else
				PaginaServizio = New ServicePage(Me.LinguaID, Me.DDLpagineDefault.SelectedValue)
			End If

		End Get
		Set(ByVal value As ServicePage)
			Dim oListitem As ListItem
			oListitem = Me.DDLpagineDefault.Items.FindByValue(value.ID)
			If Not IsNothing(oListitem) Then
				oListitem.Selected = True
			End If
		End Set
	End Property

	Public Sub SetEditImage(ByVal Immagine As String)
		If Immagine = "" Then
			SetNoLogo()
		Else
			Me.Bind_Immagine(Immagine)
		End If
	End Sub
	Public ReadOnly Property PostedFile() As System.Web.HttpPostedFile
		Get
			PostedFile = Me.FILElogo.PostedFile
		End Get
	End Property
	Private ReadOnly Property DestinationPath() As String
		Get
			Dim Path As String = ""
			If IsNothing(_ObjectPath) Then
				_ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Cover)
			End If
			If Me.isModalitaAmministrazione Then
				Path = _ObjectPath.Drive & Me.AmministrazioneComunitaID & "\"
			Else
				Path = _ObjectPath.Drive & Me.ComunitaCorrenteID & "\"
			End If
			Return Path
		End Get
	End Property
	Private ReadOnly Property VirtualDestinationPath() As String
		Get
			Dim Path As String = ""
			If IsNothing(_ObjectPath) Then
				_ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Cover)
			End If
			If Me.isModalitaAmministrazione Then
				Path = _ObjectPath.Virtual & Me.AmministrazioneComunitaID & "/"
			Else
				Path = _ObjectPath.Virtual & Me.ComunitaCorrenteID & "/"
			End If
			Return Path
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub


	Private Sub SetNoLogo()
		Me.IMGlogo.ImageUrl = Me.BaseUrl & "images/nologo.gif"
		Me.IMGlogo.Height = System.Web.UI.WebControls.Unit.Pixel(43)
		Me.IMGlogo.Width = System.Web.UI.WebControls.Unit.Pixel(200)
		Me.BTNeliminaLogo.Enabled = False
	End Sub

	Public Overrides Sub BindDati()
		Me.Bind_PagineDefault()
	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_Cover", "Generici")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setLabel(Me.LBanno)
			.setLabel(Me.LBattivata)
			.setLabel(Me.LBavviso)
			.setLabel(Me.LBcommento)
			.setLabel(Me.LBdidascalia)
			.setLabel(Me.LBerrore)
			.setLabel(Me.LBpaginaDefault)
			.setLabel(Me.LBpaginaDefault_t)
			.setLabel(Me.LBlogo_t)
			.setButton(Me.BTNeliminaLogo)
		End With
	End Sub


#Region "Visualizzazione Cover"
	Private Sub Bind_PagineDefault()
		Dim oLista As List(Of ServicePage)
		Me.DDLpagineDefault.Items.Clear()
		Try
			Dim Index As Integer = 0


			oLista = COL_Comunita.LazyListOfDefaultPage(Me.ComunitaLavoroID, Me.LinguaID, "Nome")

			For Each oPage As ServicePage In oLista
				If oPage.Servizio.isAbilitato = False Or oPage.Servizio.isAttivato = False Then
					oLista(Index).Nome = oLista(Index).Nome & " (" & Me.Resource.getValue("disattivato") & ")"
				End If
				Index += 1
			Next
			If Index > 0 Then
				Me.DDLpagineDefault.DataSource = oLista
			Else
				oLista = New List(Of ServicePage)
				oLista.Add(New ServicePage(Me.LinguaID, -1, Me.Resource.getValue("Nondefinito"), ComunitaLavoroID))
			End If

		Catch ex As Exception
			oLista = New List(Of ServicePage)
			oLista.Add(New ServicePage(Me.LinguaID, -1, Me.Resource.getValue("Nondefinito"), ComunitaLavoroID))
		End Try

		Me.DDLpagineDefault.DataTextField = "Nome"
		Me.DDLpagineDefault.DataValueField = "ID"
		Me.DDLpagineDefault.DataBind()
		Me.Resource.setDropDownList(Me.DDLpagineDefault, -1)
	End Sub

	Private Sub Bind_DatiCover()
		Me.CoverID = Me._Cover.Id
		Me.CBXattivata.Checked = Me._Cover.isAttivata
		Try
			Dim oPage As ServicePage
			If Me.isModalitaAmministrazione Then
				oPage = COL_Comunita.LazyGetCurrentDefaultPage(Me.AmministrazioneComunitaID)
			Else
				oPage = COL_Comunita.LazyGetCurrentDefaultPage(Me.ComunitaCorrenteID)
			End If
			If Not IsNothing(oPage) Then
				Me.DDLpagineDefault.SelectedValue = oPage.ID
			End If
		Catch ex As Exception

		End Try
		Me.Bind_CoverTitolo()
		Me.Bind_AnnoAccademico()
		Me.Bind_Commenti()
		Me.Bind_Immagine(Me._Cover.Immagine)
	End Sub

	Private Sub Bind_CoverTitolo()
		With Me._Cover
			Me.CTRLtitolo.Testo = .Titolo
			Me.CTRLtitolo.FontFace = .FontFace1
			Me.CTRLtitolo.FontSize = .FontSize1
			Me.CTRLtitolo.FontColor = .FontColor1

			Me.CTRLtitolo.isBold = .Bold1
			Me.CTRLtitolo.isItalic = .Italic1
		End With
	End Sub
	Private Sub Bind_AnnoAccademico()
		With Me._Cover
			Me.CTRLanno.Testo = .Anno
			Me.CTRLanno.FontFace = .FontFace2
			Me.CTRLanno.FontSize = .FontSize2
			Me.CTRLanno.FontColor = .FontColor2

			Me.CTRLanno.isBold = .Bold2
			Me.CTRLanno.isItalic = .Italic2
		End With
	End Sub
	Private Sub Bind_Commenti()
		With Me._Cover
			Me.CTRLcommento.Testo = .Commenti
			Me.CTRLcommento.FontFace = .FontFace3
			Me.CTRLcommento.FontSize = .FontSize3
			Me.CTRLcommento.FontColor = .FontColor3

			Me.CTRLcommento.isBold = .Bold3
			Me.CTRLcommento.isItalic = .Italic3
		End With

	End Sub
	Private Sub Bind_Immagine(ByVal Immagine As String)
		Me.IMGlogo.ImageUrl = Me.VirtualDestinationPath & Immagine

        'Dim oFile As New FileLayer.COL_File
        Dim altezza, larghezza As Integer
        lm.Comol.Core.File.ContentOf.ImageSize(Me.DestinationPath & Immagine, altezza, larghezza)

		If altezza > 300 Then
			Me.IMGlogo.Height = System.Web.UI.WebControls.Unit.Pixel(300)
			'osservo se così ridimensionata è ancora più larga di 700 se si taglio!
			Dim proporzione As Double = altezza / 300
			Dim larghezzaNew As Double = larghezza / proporzione
			If larghezzaNew > 700 Then
				Me.IMGlogo.Width = System.Web.UI.WebControls.Unit.Pixel(700)
			Else
				Me.IMGlogo.Width = System.Web.UI.WebControls.Unit.Pixel(larghezzaNew)
			End If
		Else
			Me.IMGlogo.Height = System.Web.UI.WebControls.Unit.Pixel(altezza)
			If larghezza > 700 Then
				Me.IMGlogo.Width = System.Web.UI.WebControls.Unit.Pixel(700)
				'osservo se così ridimensionata è ancora più alta di 300 se si taglio!
				Dim proporzione As Double = larghezza / 700
				Dim altezzaNew As Double = altezzaNew / proporzione
				If altezzaNew > 300 Then
					Me.IMGlogo.Height = System.Web.UI.WebControls.Unit.Pixel(300)
				Else
					Me.IMGlogo.Height = System.Web.UI.WebControls.Unit.Pixel(altezzaNew)
				End If
			Else
				Me.IMGlogo.Width = System.Web.UI.WebControls.Unit.Pixel(larghezza)
			End If
		End If
		Me.BTNeliminaLogo.Enabled = True
	End Sub


#End Region

	Private Function GetCover() As COL_Cover
		If Me.CoverID = 0 Then
			Me._Cover = New COL_Cover
			With Me._Cover
				If Me.isModalitaAmministrazione Then
					.CMNT_ID = Me.AmministrazioneComunitaID
				Else
					.CMNT_ID = Me.ComunitaCorrenteID
				End If
				.IDautore = Me.UtenteCorrente.Id
				.DataCreazione = Now
			End With
		Else
			Me._Cover = New COL_Cover
			Me._Cover.Id = Me.CoverID
			Me._Cover.Estrai()
			Me._Cover.DataModifica = Now
		End If

		With Me._Cover
			.Titolo = Me.CTRLtitolo.Testo
			.FontFace1 = Me.CTRLtitolo.FontFace
			.FontSize1 = Me.CTRLtitolo.FontSize
			.FontColor1 = Me.CTRLtitolo.FontColor
			.Bold1 = Me.CTRLtitolo.isBold
			.Italic1 = Me.CTRLtitolo.isItalic

			.Anno = Me.CTRLanno.Testo
			.FontFace2 = Me.CTRLanno.FontFace
			.FontSize2 = Me.CTRLanno.FontSize
			.FontColor2 = Me.CTRLanno.FontColor
			.Bold2 = Me.CTRLanno.isBold
			.Italic2 = Me.CTRLanno.isItalic

			.Commenti = Me.CTRLcommento.Testo
			.FontFace3 = Me.CTRLcommento.FontFace
			.FontSize3 = Me.CTRLcommento.FontSize
			.FontColor3 = Me.CTRLcommento.FontColor
			.Bold3 = Me.CTRLcommento.isBold
			.Italic3 = Me.CTRLcommento.isItalic
			.Didascalia = Me.CTRLdidascalia.Testo
			.isAttivata = Me.CBXattivata.Checked
		End With
		Return Me._Cover
	End Function

	Public Sub ShowErrorEditing(ByVal ShowMessage As Boolean, ByVal Message As String)
		If ShowMessage Then
			Me.LBerrore.Text = Message
			Me.DVerroreLogo.Style("display") = "block"
		Else
			Me.DVerroreLogo.Style("display") = "none"
		End If
	End Sub

End Class