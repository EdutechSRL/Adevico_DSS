Imports System.Configuration

Namespace Configuration
    ''' <summary>
    ''' Impostazioni avanzate autenticazioni aggiuntive
    ''' Per l'attivazione di specifiche funzionalità all'interno della piattaforma
    ''' </summary>
    <Serializable(), CLSCompliant(True)>
    Public Class AutenticationSetting

        ''' <summary>
        ''' Nome: per selettore utente su pagina iniziale
        ''' </summary>
        Public name As String = ""

        ''' <summary>
        ''' Tipo autenticazione
        ''' </summary>
        Public type As AutenticationSubType = AutenticationSubType.none

        ''' <summary>
        ''' Se il tipo è abilitato in piattaforma
        ''' </summary>
        Public enabled As Boolean = False

        ''' <summary>
        ''' Codice per allineamento con autenticazione di sistema
        ''' </summary>
        Public code As String = ""

        '''' <summary>
        '''' Indirizzo a cui fare la richiesta per l'oAuth
        '''' </summary>
        'Public endpoint As String = ""

        ''' <summary>
        '''  CLIENT_ID per oAuth2
        ''' </summary>
        Public clientId As String = ""
        ''' <summary>
        ''' CLIENT_SECRET per oAuth 2
        ''' </summary>
        Public clientSecret As String = ""
        ''' <summary>
        ''' REQUEST_URL per oAuth 2
        ''' </summary>
        Public requestUrl As String = ""
        ''' <summary>
        ''' SCOPE per oAuth 2
        ''' </summary>
        Public scope As String = ""
        ''' <summary>
        ''' REDIRECT_URI per oAuth 2
        ''' </summary>
        Public redirectUri As String = ""

        ''' <summary>
        ''' Lista di regular expression per verficare le mail autorizzate all'oaut
        ''' </summary>
        ''' <remarks>
        ''' In questo modo è possibile aggiungere sia domini che singole mail
        ''' </remarks>
        Public mailRegExs As IList(Of String) = New List(Of String)

        ''' <summary>
        ''' SE previsto un controllo all'esterno prima dell'iscrizione
        ''' </summary>
        Public verification As SecondaryVerification = SecondaryVerification.none

        ''' <summary>
        ''' Url per la verifica esterna
        ''' </summary>
        Public verificationEndPoint As String = ""
    End Class

    ''' <summary>
    ''' Tipo autenticazione (nel caso ne venissero aggiunti altri in futuro
    ''' </summary>
    Public Enum AutenticationSubType As Integer
        none = 0
        oAuth2 = 1
    End Enum

    Public Enum SecondaryVerification As Integer
        none = 0
        Inaz = 1
    End Enum
End Namespace