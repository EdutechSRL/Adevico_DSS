Public Class FileSystem
	Inherits ObjectBase


	Sub New()

	End Sub


	'#Region "Metodi"

	'	Public Shared Function FileNameOnly(ByVal strFileName As String) As String
	'		'funzione che passandogli la stringa del percorso ASSOLUTO o RELATIVO di un file restituisce
	'		'il solo nome del file (funzionante sia su IE che su Gecko)

	'		Dim intFileNameLength As Integer
	'		Dim strFileNameOnly As String
	'		If InStr(strFileName, "\") > 0 Or InStr(strFileName, "/") > 0 Then

	'			intFileNameLength = InStr(1, StrReverse(strFileName), "\")
	'			If intFileNameLength = 0 Then
	'				intFileNameLength = InStr(1, StrReverse(strFileName), "/")
	'			End If

	'			strFileNameOnly = Mid(strFileName, (Len(strFileName) - intFileNameLength) + 2)
	'		Else
	'			strFileNameOnly = strFileName
	'		End If

	'		Return strFileNameOnly
	'	End Function

	'	Public Function Upload(ByVal upFile As System.Web.UI.HtmlControls.HtmlInputFile, ByVal savePath As String) As Integer
	'		'Dim strFileName As String
	'		'Dim intFileNameLength As Integer
	'		Dim strFileNameOnly As String
	'		Dim peso As Integer = 0

	'		If upFile.PostedFile.ContentLength = 0 Then
	'			Me.n_errore = Errore_File.zeroByte
	'		Else
	'			If Directory.Exists(savePath) = False Then
	'				Directory.CreateDirectory(savePath)
	'			End If

	'			strFileNameOnly = FileNameOnly(upFile.PostedFile.FileName)

	'			If File.Exists(savePath & strFileNameOnly) Then
	'				Me.n_errore = Errore_File.exsist
	'			Else
	'				upFile.PostedFile.SaveAs(savePath + strFileNameOnly)
	'				If upFile.PostedFile.ContentLength < 1024 Then
	'					Me.n_dimensione = 1	'gli do 1 kb 
	'				Else
	'					Me.n_dimensione = upFile.PostedFile.ContentLength / 1024 'divido per avere la dimensione in kb
	'				End If
	'				Me.n_dataCreazione = Date.Now
	'				Me.n_percorso = upFile.PostedFile.FileName.ToString
	'				Me.n_nome = strFileNameOnly
	'				peso = upFile.PostedFile.ContentLength
	'			End If
	'			Return peso
	'		End If
	'	End Function

	'	Public Shared Function FileExists(ByVal Path As String) As Boolean
	'		If File.Exists(Path) Then
	'			Return True
	'		Else
	'			Return False
	'		End If
	'	End Function

	'	Public Function Upload(ByVal upFile As System.Web.UI.HtmlControls.HtmlInputFile, ByVal savePath As String, ByVal newName As String) As Integer
	'		Dim strFileNameOnly As String
	'		Dim peso As Integer = 0
	'		'se il file che si tenta di uploadare ha lunghezza = a 0 (non esiste)
	'		If upFile.PostedFile.ContentLength = 0 Then
	'			Me.n_errore = Errore_File.zeroByte
	'		Else
	'			Try
	'				strFileNameOnly = newName
	'				upFile.PostedFile.SaveAs(savePath + strFileNameOnly)
	'				Me.n_dimensione = upFile.PostedFile.ContentLength
	'				Me.n_dataCreazione = Date.Now
	'				Me.n_percorso = upFile.PostedFile.FileName.ToString
	'				Me.n_nome = strFileNameOnly
	'				peso = upFile.PostedFile.ContentLength
	'			Catch ex As System.IO.DirectoryNotFoundException
	'				'se la directory di destinazione non esiste...
	'				Me.n_errore = Errore_File.dirNotFound
	'			End Try
	'			Return peso
	'		End If
	'	End Function

	'	Public Function Resize(ByVal imgPath As String, ByVal newFilePath As String, ByVal Width As Integer, ByVal Height As Integer) As Integer
	'		Try
	'			'creo il bitmap dal file indicato
	'			Dim bmpStream As System.Drawing.Image = System.Drawing.Image.FromFile(imgPath)
	'			'creo un nuovo bitmap ridimensionandolo
	'			Dim img As New Bitmap(bmpStream, New Size(Width, Height))
	'			'salvo la nuova immagine come jpeg
	'			img.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
	'			img.Dispose()
	'			bmpStream.Dispose()
	'			'verifico che l'immagine esista
	'			Return 1
	'		Catch ex As System.IO.FileNotFoundException	'file nn trovato
	'			Me.n_errore = Errore_File.fileNotFound
	'			Return -1
	'		Catch 'mancano gli errori es: se nn è un'immagine
	'			Return -2
	'		End Try
	'	End Function
	'	Public Function ResizeProfilo(ByVal imgPath As String, ByVal newFilePath As String) As Integer
	'		Try
	'			'ridimensiona l'immagine proporzionata e aggiunge bande nere se necessario
	'			Dim bmpStream As System.Drawing.Image = System.Drawing.Image.FromFile(imgPath)
	'			'ridimensiono lo stream proporzionato, prima imposto la larghezza 100, 
	'			'se è comunque troppo alta ridimensiono ulteriormente per l'altezza
	'			Dim proporzione As Double
	'			Dim Width, Height As Integer
	'			proporzione = bmpStream.Height / bmpStream.Width
	'			Width = 100
	'			Height = Width * proporzione
	'			If Height > 125 Then 'se comunque è troppo alta ridimensiono de novo
	'				Height = 125
	'				Width = Height / proporzione
	'			End If
	'			'creo l'immagine
	'			Dim img As New Bitmap(bmpStream, New Size(Width, Height))

	'			'cerco le dimensioni per le bande nere che servono
	'			Dim BlackHeight As Integer = (125 - Height) / 2
	'			Dim BlackWidth As Integer = (100 - Width) / 2

	'			'Creiamo una bitmap dalle dimensione di 100x125 che sarà la dimensione della nostra immagine.
	'			'la facciamo nera
	'			Dim bitmap As New Bitmap(100, 125)
	'			Dim g As Graphics = Graphics.FromImage(bitmap)
	'			g.FillRectangle(New SolidBrush(Color.Black), 0, 0, 100, 125)

	'			'sull'immagine nea ci disegnamo sopra la nostra immagine
	'			g.DrawImage(img, BlackWidth, BlackHeight, Width, Height)

	'			'salvo la nuova immagine come jpeg
	'			bitmap.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
	'			bitmap.Dispose()
	'			bmpStream.Dispose()
	'			'verifico che l'immagine esista
	'			Return 1
	'		Catch ex As System.IO.FileNotFoundException	'file nn trovato
	'			Me.n_errore = Errore_File.fileNotFound
	'			Return -1
	'		Catch ex As Exception 'mancano gli errori es: se nn è un'immagine
	'			Return -2
	'		End Try
	'	End Function
	'	Public Function ResizeLogo(ByVal imgPath As String, ByVal newFilePath As String, ByVal Altezza As Integer, ByVal Larghezza As Integer) As Integer
	'		Try
	'			'ridimensiona l'immagine proporzionata e aggiunge bande nere se necessario
	'			Dim bmpStream As System.Drawing.Image = System.Drawing.Image.FromFile(imgPath)
	'			'ridimensiono lo stream proporzionato, prima imposto la larghezza 100, 
	'			'se è comunque troppo alta ridimensiono ulteriormente per l'altezza
	'			Dim proporzione As Double
	'			Dim Width, Height As Integer
	'			proporzione = bmpStream.Height / bmpStream.Width
	'			Width = Larghezza
	'			Height = Width * proporzione
	'			If Height > Altezza Then 'se comunque è troppo alta ridimensiono de novo
	'				Height = Altezza
	'				Width = Height / proporzione
	'			End If
	'			'creo l'immagine
	'			Dim img As New Bitmap(bmpStream, New Size(Width, Height))

	'			'cerco le dimensioni per le bande nere che servono
	'			Dim BlackHeight As Integer = (Altezza - Height) / 2
	'			Dim BlackWidth As Integer = (Larghezza - Width) / 2

	'			'Creiamo una bitmap dalle dimensione di 100x125 che sarà la dimensione della nostra immagine.
	'			'la facciamo nera
	'			Dim bitmap As New Bitmap(Larghezza, Altezza)
	'			Dim g As Graphics = Graphics.FromImage(bitmap)
	'			g.FillRectangle(New SolidBrush(Color.Black), 0, 0, Larghezza, Altezza)

	'			'sull'immagine nea ci disegnamo sopra la nostra immagine
	'			g.DrawImage(img, BlackWidth, BlackHeight, Width, Height)

	'			'salvo la nuova immagine come jpeg
	'			bitmap.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
	'			bitmap.Dispose()
	'			bmpStream.Dispose()
	'			'verifico che l'immagine esista
	'			Return 1
	'		Catch ex As System.IO.FileNotFoundException	'file nn trovato
	'			Me.n_errore = Errore_File.fileNotFound
	'			Return -1
	'		Catch ex As Exception 'mancano gli errori es: se nn è un'immagine
	'			Return -2
	'		End Try
	'	End Function


	'	Public Function elencaFile(ByVal dir As String, ByRef arrFiles As ArrayList) As Integer
	'		'la funzione restituisce oltre che l'elenco dei files tramite il ByRef anke il numero dei files contenuti...
	'		Dim fileList As String()
	'		Dim fileNameOnly As String
	'		Dim result, FileNameLength As Integer
	'		Dim tempArray As New ArrayList
	'		Dim lung As Integer

	'		If System.IO.Directory.Exists(dir) Then
	'			fileList = System.IO.Directory.GetFiles(dir)
	'			lung = fileList.Length 'numero file directory
	'			Dim fileName As String
	'			For Each fileName In fileList
	'				FileNameLength = InStr(1, StrReverse(fileName), "\")
	'				fileNameOnly = Mid(fileName, (Len(fileName) - FileNameLength) + 2)
	'				tempArray.Add(fileNameOnly)
	'			Next
	'			result = lung
	'			arrFiles = tempArray

	'		Else
	'			Me.n_errore = Errore_File.dirNotFound
	'		End If

	'		Return result
	'	End Function

	'	Public Function dimensioneDirectory(ByVal path As String) As Int64
	'		' la funzione restituisce la dimensione dei file presenti in una directory

	'		Try

	'			Dim fileList As String()
	'			Dim dimensione As Int64
	'			Dim dimensioneParziale As Int64

	'			If System.IO.Directory.Exists(path) Then

	'				fileList = System.IO.Directory.GetFiles(path) 'solo i file di quella cartella
	'				Dim fileName As String

	'				For Each fileName In fileList
	'					Dim info As New FileInfo(fileName)
	'					dimensioneParziale = info.Length
	'					dimensioneParziale = dimensioneParziale \ 1024
	'					dimensione = dimensione + dimensioneParziale
	'				Next

	'			Else
	'				Me.n_errore = Errore_File.dirNotFound
	'			End If

	'			Return dimensione
	'		Catch ex As Exception
	'			Return -1
	'		End Try

	'	End Function

	'	Public Function GetDirSizeRecurse(ByVal Path As String) As Int64
	'		'ricava in maniera ricorsiva le dimensioni della directory passata come "path"
	'		Try
	'			Dim dirdata() As System.IO.DirectoryInfo
	'			Dim filedata() As System.IO.FileInfo
	'			Dim file As System.IO.FileInfo
	'			Dim dimension As Int64 = 0

	'			Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(Path)
	'			' Scansione dei file della directory
	'			filedata = dir.GetFiles("*.*")
	'			For Each file In filedata
	'				dimension = dimension + file.Length
	'			Next
	'			' Scansione delle sottodirectory
	'			dirdata = dir.GetDirectories("*")
	'			For Each dir In dirdata
	'				dimension = dimension + GetDirSizeRecurse(dir.FullName)
	'			Next
	'			Return dimension  'ritorna la dimensione in kb
	'		Catch
	'			Return -1
	'		End Try
	'	End Function


	'	Public Function CancDir(ByVal direct As String) As Boolean
	'		Try
	'			System.IO.Directory.Delete(direct, True)
	'			Return True
	'		Catch ex As System.IO.DirectoryNotFoundException
	'			Me.n_errore = Errore_File.dirNotFound
	'		Catch ex As Exception

	'		End Try
	'		Return False
	'	End Function

	'	Public Function CancFile(ByVal file As String) As Boolean
	'		'controlla se il file esisiste e se esiste lo cancella
	'		If System.IO.File.Exists(file) Then
	'			System.IO.File.Delete(file)
	'			Return True
	'		Else
	'			'se non esiste restituisce il valore 404
	'			Me.n_errore = Errore_File.fileNotFound
	'		End If
	'		Return False
	'	End Function

	'	Public Function DirectoryIsEmpty(ByVal percorso As String) As Integer
	'		'se è vuota restituisce 1 se c'è qualcosa restituisce 0
	'		Dim empty As Integer = 0
	'		Try
	'			Dim filelist As String() = System.IO.Directory.GetFiles(percorso)
	'			Dim dirList As String() = System.IO.Directory.GetDirectories(percorso)
	'			If filelist.Length = 0 And dirList.Length = 0 Then
	'				empty = 1
	'			End If
	'		Catch ex As System.IO.DirectoryNotFoundException
	'			'se la directory non esiste
	'			Me.n_errore = Errore_File.dirNotFound
	'		Catch
	'			'se qualcos'altro non va...
	'			empty = -1
	'		End Try
	'		Return empty
	'	End Function

	'	Public Sub DimensioniImg(ByVal percorso As String, ByRef alt As Integer, ByRef larg As Integer)
	'		'funzione che restituisce le dimensioni attuali dell'immagine DA RIGUARDARE
	'		Dim objImg As System.Drawing.Image
	'		Try
	'			objImg = Image.FromFile(percorso)
	'			alt = objImg.Height
	'			larg = objImg.Width

	'		Catch ex As System.IO.FileNotFoundException	'se non c'è il file indicato da questo errore
	'			Me.n_errore = Errore_File.fileNotFound

	'		Catch ex As System.OutOfMemoryException	'a volte se non è un'immagine da questo errore
	'			Me.n_errore = Errore_File.fileMismatchType

	'		Catch ex As System.NullReferenceException 'a volte se non è un'immagine da quest'altro errore
	'			Me.n_errore = Errore_File.fileMismatchType

	'		End Try
	'	End Sub

	'	Public Function elencaFilePercorso(ByVal dir As String, ByRef arrFiles As ArrayList) As Integer
	'		'la funzione restituisce oltre che l'elenco dei files tramite il ByRef anke il numero dei files contenuti con tutto il percorso assoluto...
	'		Dim fileList As String()
	'		Dim result As Integer
	'		Dim tempArray As New ArrayList
	'		Dim lung As Integer
	'		If System.IO.Directory.Exists(dir) Then
	'			fileList = System.IO.Directory.GetFiles(dir)
	'			lung = fileList.Length 'numero file directory
	'			Dim fileName As String
	'			For Each fileName In fileList
	'				tempArray.Add(fileName)
	'			Next
	'			result = lung
	'			arrFiles = tempArray
	'		Else
	'			Me.n_errore = Errore_File.dirNotFound
	'		End If
	'		Return result
	'	End Function

	'	Public Shared Function CreaDirectoryByPath(ByVal Path As String) As Boolean
	'		Dim oElencoPath() As String
	'		Dim NomeDirectory, NomeTemp As String
	'		Try

	'			If System.IO.Directory.Exists(Path) = False Then
	'				Dim i, totale As Integer
	'				oElencoPath = Path.Split("\")

	'				totale = oElencoPath.Length - 1
	'				NomeTemp = oElencoPath(0) & "\"
	'				For i = 1 To totale
	'					NomeDirectory = oElencoPath(i)
	'					If NomeDirectory <> "" Then
	'						NomeTemp = NomeTemp & NomeDirectory & "\"
	'						'   NomeDirectory = Left(Path, InStr(Path, NomeDirectory) + NomeDirectory.Length)
	'						If System.IO.Directory.Exists(NomeTemp) = False Then
	'							Try
	'								System.IO.Directory.CreateDirectory(NomeTemp)
	'							Catch ex As Exception
	'								Return False
	'							End Try

	'						End If
	'					End If
	'				Next
	'			End If
	'		Catch ex As Exception

	'		End Try
	'		Return True
	'	End Function

	'	Public Shared Function DirectoryExist(ByVal Path As String) As Boolean
	'		If Directory.Exists(Path) Then
	'			Return True
	'		Else
	'			Return False
	'		End If
	'	End Function
	'#End Region
End Class