Imports COL_BusinessLogic_v2
Imports System.Collections.Generic

Public Class PresenterLsMail_Gestione
    Inherits GenericPresenter

	Public Sub New(ByVal view As IViewCommon)
		MyBase.view = view
	End Sub
    Private Shadows ReadOnly Property View() As IviewLsMail_Gestione
        Get
            View = MyBase.view
        End Get
    End Property
    Public Sub Initialize()
        Me.View.OrderDir = "asc"
        Me.BindLista()
    End Sub

    ''' <summary>
    ''' Carica i dati dell'elenco
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindLista()
        Dim oElencoListe As List(Of MailingList)
        Try
            oElencoListe = MailingManager.GetListeOrdinate(Me.View.UtenteCorrente.Id, "Nome", Me.View.OrderDir)
            If oElencoListe.Count > 0 Then
                Me.View.ShowLista()
                Me.View.SetLista(oElencoListe)
            Else
                Me.View.ShowNoData()
            End If
        Catch ex As Exception
            Me.View.ShowNoData()
        End Try

        '"desc"

    End Sub

    ''' <summary>
    ''' Aggiorna i dati della lista
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshLista()
        Me.BindLista()
    End Sub

    ''' <summary>
    ''' Aggiunge una nuova lista
    ''' </summary>
    ''' <remarks></remarks>
    Public Function AddNewLista() As Integer
        Dim oLista As New MailingList(Me.View.UtenteCorrente.Id, Me.View.NewName)
        If Not oLista.Nome = "" Then
            If oLista.AddLista() Then
                Me.View.ShowInsertOk()
                Me.BindLista()
            Else
                Me.View.ShowErrorInsert()
            End If
        End If
        Me.BindLista()
        Return oLista.Id
    End Function

    ''' <summary>
    ''' Elimina la lista
    ''' </summary>
    ''' <param name="IdLista">Id della lista da eliminare</param>
    ''' <remarks></remarks>
    Public Sub DeleteLista(ByVal IdLista As Integer)
        Dim oLista As New MailingList()
        oLista.Id = IdLista
        If oLista.RemoveList() Then
            Me.View.ShowEliminaOk()
        Else
            Me.View.ShowErrorElimina()
        End If
        Me.BindLista()
    End Sub

    ''' <summary>
    ''' Aggiorna la lista riordinandone i record
    ''' </summary>
    ''' <param name="SortExpression"></param>
    ''' <remarks></remarks>
    Public Sub ReorderList(ByVal SortExpression As String)
        If Me.View.OrderDir = "asc" Then
            Me.View.OrderDir = "desc"
        Else
            Me.View.OrderDir = "asc"
        End If

        Dim oElencoListe As List(Of MailingList)
        oElencoListe = MailingManager.GetListeOrdinate(Me.View.UtenteCorrente.Id, SortExpression, Me.View.OrderDir)
        If oElencoListe.Count > 0 Then
            Me.View.ShowLista()
            Me.View.SetLista(oElencoListe)
        Else
            Me.View.ShowNoData()
        End If
    End Sub

End Class