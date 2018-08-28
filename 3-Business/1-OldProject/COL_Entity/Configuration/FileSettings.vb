Imports Comol.Entity.File
Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class FileSettings

		Public Enum ConfigType
			File = 0
            VideoCast = 2
			Scorm = 3
			Cover = 4
			Mail = 5
            Profilo = 7
			Wiki = 8
			FileTesi = 9
            Questionari = 11
            UserCertifications = 12
            Noticeboard = 13
            Tile = 14
            Repository = -1
            Glossary = 15
            FileThumbnail = 16
		End Enum

#Region "Private properties"
		Private _Path As New Hashtable
#End Region

#Region "Public properties"
		Public ReadOnly Property Repository() As ConfigurationPath
			Get
				Repository = DirectCast(_Path.Item(ConfigType.Repository), ConfigurationPath)
			End Get
		End Property
		Public ReadOnly Property Materiale() As ConfigurationPath
			Get
				Materiale = DirectCast(_Path.Item(ConfigType.File), ConfigurationPath)
			End Get
        End Property
        Public ReadOnly Property FileThumbnail() As ConfigurationPath
            Get
                Return DirectCast(_Path.Item(ConfigType.FileThumbnail), ConfigurationPath)
            End Get
        End Property
        Public ReadOnly Property VideoCast() As ConfigurationPath
            Get
                VideoCast = DirectCast(_Path.Item(ConfigType.VideoCast), ConfigurationPath)
            End Get
        End Property
		Public ReadOnly Property Scorm() As ConfigurationPath
			Get
				Scorm = DirectCast(_Path.Item(ConfigType.Scorm), ConfigurationPath)
			End Get
		End Property
		Public ReadOnly Property Cover() As ConfigurationPath
			Get
				Cover = DirectCast(_Path.Item(ConfigType.Cover), ConfigurationPath)
			End Get
		End Property
		Public ReadOnly Property Mail() As ConfigurationPath
			Get
				Mail = DirectCast(_Path.Item(ConfigType.Mail), ConfigurationPath)
			End Get
		End Property
        'Public ReadOnly Property Profile() As ConfigurationPath
        '    Get
        '        Profile = DirectCast(_Path.Item(ConfigType.Profilo), ConfigurationPath)
        '    End Get
        'End Property
		Public ReadOnly Property Wiki() As ConfigurationPath
			Get
				Wiki = DirectCast(_Path.Item(ConfigType.Wiki), ConfigurationPath)
			End Get
		End Property
		Public ReadOnly Property FileTesi() As ConfigurationPath
			Get
				FileTesi = DirectCast(_Path.Item(ConfigType.FileTesi), ConfigurationPath)
			End Get
        End Property
        Public ReadOnly Property UserCertifications() As ConfigurationPath
            Get
                Dim item As ConfigurationPath = GetByCode(ConfigType.UserCertifications)
                If IsNothing(item) Then
                    Return New ConfigurationPath() With {.isInvalid = True}
                Else
                    Return item
                End If
            End Get
        End Property

        Public ReadOnly Property Noticeboard() As ConfigurationPath
            Get
                Dim item As ConfigurationPath = GetByCode(ConfigType.Noticeboard)
                If IsNothing(item) Then
                    Return New ConfigurationPath() With {.isInvalid = True}
                Else
                    Return item
                End If
            End Get
        End Property
        Public ReadOnly Property Glossary() As ConfigurationPath
            Get
                Dim item As ConfigurationPath = GetByCode(ConfigType.Glossary)
                If IsNothing(item) Then
                    Return New ConfigurationPath() With {.isInvalid = True}
                Else
                    Return item
                End If
            End Get
        End Property
        Public ReadOnly Property ProfilesHome() As ConfigurationPath
            Get
                Dim item As ConfigurationPath = GetByCode(ConfigType.Profilo)
                If IsNothing(item) Then
                    Return New ConfigurationPath() With {.isInvalid = True}
                Else
                    Return item
                End If
            End Get
        End Property
        Public ReadOnly Property Tiles() As ConfigurationPath
            Get
                Dim item As ConfigurationPath = GetByCode(ConfigType.Tile)
                If IsNothing(item) Then
                    Return New ConfigurationPath() With {.isInvalid = True}
                Else
                    Return item
                End If
            End Get
        End Property
        'Public ReadOnly Property Profile() As ConfigurationPath
        '    Get
        '        Profile = DirectCast(_Path.Item(ConfigType.Profilo), ConfigurationPath)
        '    End Get
        'End Property
#End Region

		Public Function GetByCode(ByVal oConfig As ConfigType) As ConfigurationPath
			Return DirectCast(_Path.Item(oConfig), ConfigurationPath)
        End Function

		Public Sub AddSettings(ByVal oConfigurationPath As ConfigurationPath, ByVal oConfigType As ConfigType)
			_Path.Add(oConfigType, oConfigurationPath)
		End Sub
        'Public Sub CopySettings(ByVal oConfigurationPath As ConfigurationPath, ByVal key As ConfigType)
        '    If _Path.ContainsKey(key) Then
        '        _Path(key) = oConfigurationPath
        '    Else
        '        _Path.Add(key, oConfigurationPath)
        '    End If

        'End Sub
	End Class
End Namespace