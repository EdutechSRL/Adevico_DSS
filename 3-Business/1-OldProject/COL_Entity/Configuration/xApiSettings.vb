Imports System.Configuration

Namespace Configuration
    ''' <summary>
    ''' Impostazioni avanzate xApiVideo
    ''' Per l'attivazione di specifiche funzionalità all'interno della piattaforma
    ''' </summary>
    <Serializable(), CLSCompliant(True)>
    Public Class xApiSettings
        ''' <summary>
        ''' SE xApi sono attivate o meno: inutile.
        ''' </summary>
        Public Enabled As Boolean = False

        ''' <summary>
        ''' Base Url player xApi
        ''' </summary>
        Public basexApiUrl As String = ""

        ''' <summary>
        ''' Parametri url: standardizzati per xApi video ed xApiStoryline
        ''' </summary>
        Public UrlParameters As String = ""

        ''' <summary>
        ''' Action xApi per video
        ''' </summary>
        Public xApiVideoAction As String = ""
        ''' <summary>
        ''' Indica se il sistema permette di selezionare video xApi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HasVideo As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(xApiVideoAction)
            End Get
        End Property

        ''' <summary>
        ''' Action xApi Storyline
        ''' </summary>
        Public xApiStoryLineAction As String = ""
        ''' <summary>
        ''' Indica se il sistema permette di selezionare video xApi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HasStoryLine As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(xApiStoryLineAction)
            End Get
        End Property

    End Class

End Namespace
