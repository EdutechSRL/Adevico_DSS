Imports COL_BusinessLogic_v2
Imports System.Collections.Generic

Public Class PresenterLsMail
    Inherits GenericPresenter

	Public Sub New(ByVal view As IViewCommon)
		MyBase.view = view
	End Sub
    Private Shadows ReadOnly Property View() As IviewLsMail
        Get
            View = MyBase.view
        End Get
    End Property

    Public Sub Init()
        Me.Bind()
        Me.View.Show(IviewLsMail.Panel.InvioMail)
    End Sub
    ''' <summary>
    ''' Aggiorna la pagina ed i dati in essa contenuta
    ''' </summary>
    ''' <remarks>
    ''' Viene aggiornato il mittente, le liste di distribuzione e caricati gli allegati
    ''' </remarks>
    Private Sub Bind()
        With Me.View.UtenteCorrente
            Me.View.Mittente = .Cognome & " " & .Nome & " " & .Mail
        End With
        Me.BindListe()
        Me.View.LoadAttachments(, True, )
    End Sub

    ''' <summary>
    ''' Invia l'oggetto mail presente nella vista
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendMail()
		'Dim oResourceConfig As New ResourceManager
		'oResourceConfig = 'GetResourceConfig(Me.View.LinguaCode)

        Dim oMail As COL_E_Mail = Me.View.oMail
        Try
            If oMail.IndirizziTO.Count = 0 Then
                Me.View.ShowNoDestinatari(True)
                Exit Sub
            Else
                Me.View.ShowNoDestinatari(False)
            End If
        Catch ex As Exception

        End Try
        
        Try

            If Me.View.HasPermessi Then
				oMail.InviaMailHTML()
            Else
                Me.View.Show(IviewLsMail.Panel.NoPermessi)
                Exit Sub
            End If

            If oMail.Errore = Errori_Db.None Then
                Me.View.Show(IviewLsMail.Panel.MessageOK)
            Else
                Me.View.Show(IviewLsMail.Panel.MessageError)
            End If

        Catch ex As Exception
            Me.View.Show(IviewLsMail.Panel.MessageError)
        End Try
    End Sub

    ''' <summary>
    ''' Effettua il caricamento dell'elenco delle liste
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BindListe()
        Dim oLista As List(Of MailingList)
        oLista = MailingManager.GetListeOrdinate(Me.View.UtenteCorrente.Id, "Nome", "asc")
        If oLista.Count > 0 Then
            Me.View.BindListe(oLista)
        Else
            Me.View.BindListe(Nothing)
        End If
    End Sub
End Class
