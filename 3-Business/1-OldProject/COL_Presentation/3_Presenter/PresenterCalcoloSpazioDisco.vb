
Public Class PresenterCalcoloSpazioDisco
	Inherits ObjectBase
	Protected _view As IviewCalcoloSpazioDisco

	Public Sub New(ByVal view As IviewCalcoloSpazioDisco)
		_view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewCalcoloSpazioDisco
		Get
			View = _view
		End Get
	End Property

	Public Sub Init()
		Dim SpazioOccupato As Int64 = Me.CalcoloSpazio
		Dim Percentuale As Double

		Dim MaxSize As Int64
		If Me.View.MaxSize < 0 Then
			MaxSize = 10 * 1024 * 1024
		ElseIf Me.View.MaxSize = 1 Then
			MaxSize = Me.View.MaxSize * 1024
		Else
			MaxSize = Me.View.MaxSize * 1024 * 1024
		End If

		If Me.View.MaxSize > 0 Then
			Percentuale = ((SpazioOccupato * 100) / MaxSize)
			If Percentuale < 1 Then
				Percentuale = 1
			End If
		Else
			Percentuale = 20
		End If

		Me.View.TestoSezioneOver = ""
		Me.View.BarraSezione25 = 0
		Me.View.BarraSezione50 = 0
		Me.View.BarraSezione75 = 0
		Me.View.BarraSezione100 = 0
		Me.View.BarraSezione150 = 0

		If Percentuale <= 25 Then
			Me.View.BarraSezione25 = (Percentuale * Me.View.BarraUnitaBase) / 25
		ElseIf Percentuale <= 50 Then
			Me.View.BarraSezione25 = Me.View.BarraUnitaBase
			Me.View.BarraSezione50 = (Percentuale * Me.View.BarraUnitaBase) / 50
		ElseIf Percentuale <= 75 Then
			Me.View.BarraSezione25 = Me.View.BarraUnitaBase
			Me.View.BarraSezione50 = Me.View.BarraUnitaBase
			Me.View.BarraSezione75 = (Percentuale * Me.View.BarraUnitaBase) / 75
		ElseIf Percentuale <= 100 Then
			Me.View.BarraSezione25 = Me.View.BarraUnitaBase
			Me.View.BarraSezione50 = Me.View.BarraUnitaBase
			Me.View.BarraSezione75 = Me.View.BarraUnitaBase
			Me.View.BarraSezione100 = (Percentuale * Me.View.BarraUnitaBase) / 100
		Else
			Me.View.BarraSezione25 = Me.View.BarraUnitaBase
			Me.View.BarraSezione50 = Me.View.BarraUnitaBase
			Me.View.BarraSezione75 = Me.View.BarraUnitaBase
			Me.View.BarraSezione100 = Me.View.BarraUnitaBase
			Me.View.BarraSezione150 = Me.View.BarraUnitaBase
			Me.View.TestoSezioneOver = String.Format("{0:F2}", Percentuale) & "%"
		End If
		If Me.View.MaxSize = 1 Then
			Me.View.SetDisplayInfo(SpazioOccupato / (1024 * 1024), MaxSize / 1024)
		Else
			Me.View.SetDisplayInfo(SpazioOccupato / (1024 * 1024), MaxSize / (1024 * 1024))
		End If
	End Sub

	Private Function CalcoloSpazio() As Int64
		Dim totale As Int64
		If Me.View.oConfigType = ConfigFileType.File Then
			Dim cacheKey As String = CachePolicy.SpazioFileComunita(Me.View.ComunitaID)

			If ObjectBase.Cache(cacheKey) Is Nothing Then
                totale = lm.Comol.Core.File.ContentOf.Directory_Size(Me.View.DrivePath(), True)
				ObjectBase.Cache.Insert(cacheKey, totale, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				totale = CType(ObjectBase.Cache(cacheKey), Int64)
			End If
			Return totale
		Else
            Return lm.Comol.Core.File.ContentOf.Directory_Size(Me.View.DrivePath(), True)
		End If
	End Function

    'Public Function GetDirSize(ByVal Path As System.String) As Int64
    '	Try
    '		Dim oDirectoryList() As System.IO.DirectoryInfo
    '		Dim oFileList() As System.IO.FileInfo
    '		Dim oFile As System.IO.FileInfo
    '		Dim DirSize As Int64 = 0


    '		Dim oDirectory As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(Path)
    '		' Scansione dei file della directory
    '		oFileList = oDirectory.GetFiles("*.*")
    '		For Each oFile In oFileList
    '			DirSize += oFile.Length
    '		Next
    '		' Scansione delle sottodirectory
    '		oDirectoryList = oDirectory.GetDirectories("*")
    '		For Each oDirectory In oDirectoryList
    '			DirSize += GetDirSize(oDirectory.FullName)
    '		Next
    '		Return DirSize
    '	Catch
    '		Return -1
    '	End Try
    'End Function

	'Private Function CalculateFreeSpace(ByVal RemotePah As String) As Int64
	'	Try
	'		Dim ServerName As String = "\\"
	'		If RemotePah = "" Then
	'			ServerName &= "."
	'		Else
	'			ServerName &= RemotePah
	'		End If
	'		ServerName &= "\root\cimv2"

	'		'Connection credentials to the remote computer -
	'		' not needed if the logged in account has access

	'		Dim oConnectionOptions As System.Management.ConnectionOptions = New System.Management.ConnectionOptions
	'		Dim oManagementScope As New System.Management.ManagementScope(ServerName, oConnectionOptions)
	'		Dim oQuery As System.Management.ObjectQuery = New ObjectQuery("select FreeSpace,Size,Name from Win32_LogicalDisk where DriveType=3")

	'		Dim oSearcher As ManagementObjectSearcher = New ManagementObjectSearcher(oManagementScope, oQuery)

	'		Dim oReturnCollection As ManagementObjectCollection
	'		oReturnCollection = oSearcher.Get()

	'		Dim strFreespace, strTotalspace As String
	'		Dim D_Freespace, D_Totalspace As Double

	'		For Each oObject As ManagementObject In oReturnCollection
	'			strFreespace = oObject("FreeSpace").ToString()
	'			strTotalspace = oObject("Size").ToString()
	'			D_Freespace = D_Freespace + System.Convert.ToDouble(strFreespace)
	'			D_Totalspace = D_Totalspace + System.Convert.ToDouble(strTotalspace)

	'		Next
	'	Catch ex As Exception

	'	End Try

	'End Function


End Class