Imports COL_BusinessLogic_v2
Imports System.Collections.Generic

Public Class PresenterLsMail_Lista
    Inherits GenericPresenter

	Public Sub New(ByVal view As IViewCommon)
		MyBase.view = view
	End Sub

    Private Shadows ReadOnly Property View() As IviewLsMail_Lista
        Get
            View = MyBase.view
        End Get
    End Property

    Public Sub Initialize()
        Me.View.ShowElenco()

        Me.View.HideMessage()
        Me.View.SortDir = "asc"
        Me.BindNomeLista()
        Me.BindLista()

        Me.View.SetInternazionalizzazione()
    End Sub

    Private Sub BindLista()
        Dim oList As List(Of MailingAddress)
        oList = COL_BusinessLogic_v2.MailingManager.GetIndirizziOrdinati(Me.View.IdLista, "PersonaCognome", Me.View.SortDir)
        If oList.Count > 0 Then
            Me.View.SetLista(oList)
            Me.View.ShowElenco()
        Else
            Me.View.ShowNoItem()
        End If
    End Sub
    Private Sub BindNomeLista()
        Dim oLista As New MailingList(Me.View.IdLista)
        oLista.Estrai()
        Me.View.NomeLista = oLista.Nome
    End Sub

    Public Sub SalvaNomeLista()
        Dim oLista As New MailingList(Me.View.IdLista, Me.View.IdProprietario, Me.View.NomeLista)

        If oLista.RenameLista Then
            Me.View.ShowUpdateOk()
        Else
            Me.View.ShowUpdateError()
        End If
    End Sub
    Public Sub RemoveAddress(ByVal oListAddress As List(Of MailingAddress))
        Dim HasNoError As Boolean = True

        For Each oAddress As MailingAddress In oListAddress
            If Not oAddress.Delete() Then
                HasNoError = False
            End If
        Next

        If HasNoError Then
            Me.View.ShowDelOk()
        Else
            Me.View.ShowDelError()
        End If
    End Sub
    Public Sub RemoveAddress(ByVal AddressId As Integer)
        Dim oAddress As New MailingAddress(AddressId)
        If oAddress.Delete Then
            Me.View.ShowDelOk()
        Else
            Me.View.ShowDelError()
        End If
    End Sub
    Public Sub ModifyAddress()
        Dim oAddress As New MailingAddress(Me.View.Ind_Id, Me.View.Ind_PrsnId, Me.View.Ind_Nome, Me.View.Ind_Cognome, Me.View.Ind_Mail, Me.View.Ind_Struttura, Me.View.Ind_Titolo)
        If oAddress.Update Then
            Me.View.ShowUpdateOk()
        Else
            Me.View.ShowUpdateError()
        End If
        Me.BindLista()
    End Sub
    Public Sub DeleteAddress(ByVal IdAddress As Integer)
        Dim oAddress As New MailingAddress()
        oAddress.Id = IdAddress
        If oAddress.Delete Then
            Me.View.ShowDelOk()
        Else
            Me.View.ShowDelError()
        End If
        Me.BindLista()
    End Sub
    Public Sub AddNewAddress()
        Dim oAddress As New MailingAddress(Me.View.Ind_PrsnId, Me.View.Ind_Nome, Me.View.Ind_Cognome, Me.View.Ind_Mail, Me.View.Ind_Struttura, Me.View.Ind_Titolo)
        oAddress.IdLista = Me.View.IdLista

        Dim newId As Integer
        oAddress.SaveNew()
        newId = oAddress.Id
        If newId >= 0 Then
            Me.View.Ind_Id = newId
            Me.View.ShowAddUserOk()
            Me.BindLista()
        ElseIf newId = -1 Then
            Me.View.ShowMessageToPage("Dati già inseriti")
        Else
            Me.View.Ind_Id = 0
            Me.View.ShowAddUserError()
        End If
    End Sub

    Public Sub ReorderList(ByVal SortExpression As String, ByVal SortDirection As String)
        Dim oList As List(Of MailingAddress)
        oList = COL_BusinessLogic_v2.MailingManager.GetIndirizziOrdinati(Me.View.IdLista, SortExpression, SortDirection)
        If oList.Count > 0 Then
            Me.View.SetLista(oList)
            Me.View.ShowElenco()
        Else
            Me.View.ShowNoItem()
        End If
    End Sub

    Public Sub BindModifica(ByVal IdAddress As Integer)
        Dim oAddress As New MailingAddress(IdAddress)
        With oAddress
            .Estrai()
            Me.View.Ind_Id = .Id
            Me.View.Ind_Titolo = .Titolo
            Me.View.Ind_Nome = .PersonaNome
            Me.View.Ind_Cognome = .PersonaCognome
            Me.View.Ind_PrsnId = .PersonaID
            Me.View.Ind_Mail = .PersonaMail
            Me.View.Ind_Struttura = .Struttura
        End With

        Me.View.ShowModifica()

    End Sub
    Public Sub BindNew()
        Me.View.Ind_Id = 0
        Me.View.Ind_PrsnId = 0
        Me.View.Ind_Nome = ""
        Me.View.Ind_Cognome = ""
        Me.View.Ind_Mail = ""
        Me.View.Ind_Struttura = ""
        Me.View.Ind_Titolo = ""
        Me.View.ShowNuovo()
    End Sub

End Class