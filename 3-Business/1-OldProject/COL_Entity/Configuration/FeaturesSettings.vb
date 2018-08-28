Imports System.Configuration

Namespace Configuration
    ''' <summary>
    ''' Impostazioni avanzate.
    ''' Per l'attivazione di specifiche funzionalità all'interno della piattaforma
    ''' </summary>
    <Serializable(), CLSCompliant(True)>
    Public Class FeaturesSettings
        ''' <summary>
        ''' Attiva la valutazione avanzata nei bandi
        ''' </summary>
        Public CallAdvanceEvaluation As Boolean = False

        ''' <summary>
        ''' Attiva sotto-attività di tipo multilingua
        ''' </summary>
        Public EdupathMultilanguage As Boolean = False
        ''' <summary>
        ''' Mostra il "codice" di un percorso formativo
        ''' </summary>
        Public EdupathShowCode As Boolean = False
        ''' <summary>
        ''' Utente: mostra settore e mansione di un utente
        ''' </summary>
        Public UsersShowMansion As Boolean = False

        ''' <summary>
        ''' Configurazione scadenza accesso comunità
        ''' </summary>
        Public DelaySubconf As DelaySubscriptionConfig = New DelaySubscriptionConfig()

        ''' <summary>
        ''' Configurazione xApiVideo
        ''' </summary>
        Public xApiSettings As xApiSettings = New xApiSettings()


        Public Autentications As IList(Of AutenticationSetting) = New List(Of AutenticationSetting)()

        Public Sub New()
            CallAdvanceEvaluation = False

            EdupathMultilanguage = False
            EdupathShowCode = False

            UsersShowMansion = False
            DelaySubconf = New DelaySubscriptionConfig()
            xApiSettings = New xApiSettings()

            Autentications = New List(Of AutenticationSetting)()
        End Sub


    End Class

End Namespace