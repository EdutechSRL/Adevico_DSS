''' <summary>
''' Configurazione validità
''' </summary>
Public Class DelaySubscriptionConfig

    ''' <summary>
    ''' Validità, lette da configurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property Validity As IList(Of dsValidity)

    Public ReadOnly Property Enabled As Boolean
        Get
            Return Not IsNothing(Validity) AndAlso Validity.Any()
        End Get
    End Property

    Public Property AdminTypeIds As List(Of Integer) = New List(Of Integer)

    ''' <summary>
    ''' Recupera l'elenco localizzato di valori ammessi
    ''' </summary>
    ''' <param name="LangCode">Codice lingua internazionalizzazione</param>
    ''' <returns>Lista di keyValuePair con nome e valore periodo</returns>
    Public Function GetValidity(ByVal LangCode As String) As IList(Of KeyValuePair(Of String, Integer))
        If Validity Is Nothing OrElse Not Validity.Any() Then
            Validity = New List(Of dsValidity)()
            Dim defValue As dsValidity = New dsValidity()
            defValue.DefaultName = "Unlimited"
            defValue.Value = -1
            Validity.Add(defValue)
        End If

        Dim kvpValidity As IList(Of KeyValuePair(Of String, Integer)) =
            (From val In Validity
             Select New KeyValuePair(Of String, Integer)(
                 If(val.Names.ContainsKey(LangCode), val.Names(LangCode), val.DefaultName),
                 val.Value)
            ).Distinct().ToList()

        Return kvpValidity
    End Function

    ''' <summary>
    ''' Data una durata, recupera il nome localizzato
    ''' </summary>
    ''' <param name="LangCode"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Function GetValidityName(ByVal LangCode As String, ByVal value As Integer) As String

        If IsNothing(Validity) Then
            Return value.ToString
        End If

        Dim Val As dsValidity = Validity.FirstOrDefault(Function(v) v.Value = value)

        If IsNothing(Val) Then
            Return value.ToString
        End If

        If Val.Names.ContainsKey(LangCode) Then
            Return Val.Names(LangCode)
        End If

        Return Val.DefaultName
    End Function
End Class