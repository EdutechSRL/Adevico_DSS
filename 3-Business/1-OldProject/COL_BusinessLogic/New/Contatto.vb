Imports COL_BusinessLogic_v2

Namespace ContattiMail

    Public Class COL_Contatto
        Private _Mail As MailAddress
        Private _Anagrafica As String
        Private _PersonaID As Integer
        Private _Tipo As TipoDestinatarioMail
        Private _RuoloID As Integer
        Private _isByGruppo As Boolean

        Public Property Mail() As MailAddress
            Get
                Mail = _Mail
            End Get
            Set(ByVal value As MailAddress)
                _Mail = value
            End Set
        End Property
        Public Property PersonaID() As Integer
            Get
                PersonaID = _PersonaID
            End Get
            Set(ByVal value As Integer)
                _PersonaID = value
            End Set
        End Property
        Public Property RuoloID() As Integer
            Get
                RuoloID = _RuoloID
            End Get
            Set(ByVal value As Integer)
                _RuoloID = value
            End Set
        End Property
        Public Property TipoDestinatario() As TipoDestinatarioMail
            Get
                TipoDestinatario = _Tipo
            End Get
            Set(ByVal value As TipoDestinatarioMail)
                _Tipo = value
            End Set
        End Property
        Public ReadOnly Property Anagrafica() As String
            Get
                Anagrafica = _Anagrafica
            End Get
        End Property
        Public ReadOnly Property isSelezionatobyGruppo() As Boolean
            Get
                isSelezionatobyGruppo = _isByGruppo
            End Get
        End Property

        Public Sub New()

        End Sub
        Public Sub New(ByVal oPersonaID As Integer, ByVal oAnagrafica As String, ByVal oMail As String, ByVal oRuoloId As Integer, ByVal oTipo As TipoDestinatarioMail, Optional ByVal isByGruppo As Boolean = False)
            Me._Anagrafica = oAnagrafica
            Me._Mail = New MailAddress(oMail, _Anagrafica)
            Me._PersonaID = oPersonaID
            Me._RuoloID = oRuoloId
            Me._Tipo = oTipo
            Me._isByGruppo = isByGruppo
        End Sub
    End Class



    Public Class COL_CollectionContatti
        Inherits Collections.Generic.List(Of COL_Contatto)


        Public Function GetEmailAddresses() As MailAddressCollection
            Dim iResult As New MailAddressCollection
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                iResult.Add(oContatto.Mail)
            Next

            Return iResult
        End Function
        Public Function GetEmailAddressesSingoli() As MailAddressCollection
            Dim iResult As New MailAddressCollection
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                If oContatto.isSelezionatobyGruppo = False Then
                    iResult.Add(oContatto.Mail)
                End If
            Next

            Return iResult
        End Function
        Public Function GetEmailAddressesGruppi() As MailAddressCollection
            Dim iResult As New MailAddressCollection
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                If oContatto.isSelezionatobyGruppo = True Then
                    iResult.Add(oContatto.Mail)
                End If
            Next

            Return iResult
        End Function
        Public Function GetElencoID() As ArrayList
            Dim iResult As New ArrayList
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                iResult.Add(oContatto.PersonaID)
            Next

            Return iResult
        End Function
        Public Function GetElencoIDsingoli() As ArrayList
            Dim iResult As New ArrayList
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                If Not oContatto.isSelezionatobyGruppo Then
                    iResult.Add(oContatto.PersonaID)
                End If
            Next

            Return iResult
        End Function
        Public Function GetElencoIDgruppi() As ArrayList
            Dim iResult As New ArrayList
            Dim oContatto As New COL_Contatto

            For Each oContatto In Me
                If oContatto.isSelezionatobyGruppo Then
                    iResult.Add(oContatto.PersonaID)
                End If
            Next

            Return iResult
        End Function
    End Class

    Public Class CollectionRuoli
        Inherits Collections.Generic.List(Of COL_TipoRuolo)

    End Class

    Public Class CollectionRuoloPersonaComunita
        Inherits Collections.Generic.List(Of TriDirectionalRelation(Of COL_Contatto, COL_TipoRuolo, COL_Comunita))

    End Class

    'Public Class CollectionRuoloPersonaArea
    '    Inherits Collections.Generic.List(Of TriDirectionalRelation(Of COL_Contatto, COL_TipoRuolo)

    'End Class





End Namespace