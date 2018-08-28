Namespace Configuration

    <Serializable(), CLSCompliant(True)> Public Class WebConferencingSettings
        ''' <summary>
        ''' Se può essere effettuata la registrazione
        ''' </summary>
        ''' <remarks></remarks>
        Public CanRecord As Boolean
        ''' <summary>
        ''' Il tempo di validità di una registrazione, al termine dei quali la registrazione sarà cancellata.
        ''' </summary>
        ''' <remarks>0 = no limit</remarks>
        Public RecordExpDay As Integer

        ''' <summary>
        ''' Se le statistiche sono attive
        ''' </summary>
        ''' <remarks></remarks>
        Public CanTrace As Boolean
        ''' <summary>
        ''' Il tempo di validità di eventuali log, al termine dei quali i log saranno cancellati.
        ''' </summary>
        ''' <remarks>0 = no limits</remarks>
        Public TraceExpDay As Integer

        ''' <summary>
        ''' SE è previsto l'utilizzo del database interno a Comol
        ''' </summary>
        ''' <remarks>Probabilmente verrà eliminato</remarks>
        Public UseDataBase As Boolean

        ''' <summary>
        ''' Se è necessario impostare un proxy. In produzione non ha molto senso (IP pubblici), ma in sviluppo diventa necessario per le chiamate esterne.
        ''' </summary>
        ''' <remarks></remarks>
        Public UseProxy As Boolean
        ''' <summary>
        ''' Eventuale URL del proxy
        ''' </summary>
        ''' <remarks>
        ''' 1. non sono al momento previste o gestite le credenziali relative
        ''' 2. SE VUOTO, UseProxy sarà impostato a false
        ''' </remarks>
        Public ProxyUrl As String

        ''' <summary>
        ''' Il sistema correntemente utilizzato
        ''' </summary>
        ''' <remarks>
        ''' none - il sistema di videoconferenza è DISATTIVATO
        ''' Per altri valori è NECESSARIO impostare i relativi valori. In caso contrario, System sarà messo a "none" e la WebConferencing DISATTIVATA!
        ''' </remarks>
        Public System As WBsystem

        ''' <summary>
        ''' Settings propri di eWorks
        ''' </summary>
        ''' <remarks>
        ''' Controllati solamente se System = eWorks.
        ''' In questo caso, se vuoti, System sarà impostato a "none"
        ''' </remarks>
        Public eWSettings As eWorksSettings
        ''' <summary>
        ''' Settings propri di OpenMeeting
        ''' </summary>
        ''' <remarks>
        ''' Controllati solamente se System = OpenMeeting.
        ''' In questo caso, se vuoti, System sarà impostato a "none"
        ''' </remarks>
        Public OMSettings As OpenMeetingSettings


        ''' <summary>
        ''' Indica se il nome contiene l'ID della stanza
        ''' </summary>
        ''' <remarks></remarks>
        Public NameWithId As Boolean = False

        ''' <summary>
        ''' Tutti i parametri per eWorks
        ''' </summary>
        ''' <remarks></remarks>
        Public Class eWorksSettings
            Public BaseUrl As String
            Public MainUserId As String
            Public MainUserPwd As String
            Public MaxUrlChar As Integer
            ''' <summary>
            ''' Usato solo dalla versione 7.0 in poi.
            ''' Valori considerati: "7.0"
            ''' </summary>
            ''' <remarks></remarks>
            Public Version As String
        End Class

        ''' <summary>
        ''' Tutti i parametri per OpenMeeting
        ''' </summary>
        ''' <remarks></remarks>
        Public Class OpenMeetingSettings
            Public BaseUrl As String
            Public MainUserLogin As String
            Public MainUserPwd As String
        End Class

        ''' <summary>
        ''' Sistemi disponibili
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum WBsystem
            none
            eWorks
            OpenMeeting
        End Enum

        Public Sub New()
            System = WBsystem.none
        End Sub

    End Class

End Namespace