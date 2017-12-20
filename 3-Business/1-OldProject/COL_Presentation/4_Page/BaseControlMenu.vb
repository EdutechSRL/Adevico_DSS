Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.UCServices
Imports System.Collections.Generic

Public MustInherit Class BaseControlMenu
    Inherits BaseControlSession
    Implements IviewBase

    Private _History As HistoryElement

    Public Sub New()
        MyBase.New()
    End Sub



    Public ReadOnly Property History() As HistoryElement Implements IviewBase.History
        Get
            If Me.PageIstance = False Then
                Try
                    _History = LoadHystoryElement(0, 0)
                Catch ex As Exception
                    _History = Nothing
                End Try
            Else
                PageIstance = True
            End If
            History = _History
        End Get
    End Property

    Private Function LoadHystoryElement(ByVal Indice As Integer, ByVal PadreID As Integer) As HistoryElement
        Dim oElement As HistoryElement = Nothing

        Try
            If Indice <= UBound(Session("ArrComunita"), 2) Then
                Dim ID, RuoloID As Integer
                Dim nome As String = ""
                Dim percorso As String = ""
                Dim isAccessoLibero As Boolean = False
                ID = Session("ArrComunita")(0, Indice)
                nome = Session("ArrComunita")(1, Indice)
                percorso = Session("ArrComunita")(2, Indice)
                RuoloID = Session("ArrComunita")(3, Indice)
                isAccessoLibero = COL_Comunita.GetAccessoLibero(ID)

                If Me.isNonIscrittoComunita And isAccessoLibero Then
                    oElement = New HistoryElement(ID, PadreID, nome, percorso, RuoloID, LoadHystoryElement(Indice + 1, ID), isAccessoLibero)
                ElseIf RuoloID < 0 Then
                    oElement = LoadHystoryElement(Indice + 1, ID)
                Else
                    oElement = New HistoryElement(ID, PadreID, nome, percorso, RuoloID, LoadHystoryElement(Indice + 1, ID), isAccessoLibero)
                End If
            End If
        Catch ex As Exception
            oElement = Nothing
        End Try
        Return oElement
    End Function

    Public Property PostItSistema() As COL_BusinessLogic_v2.COL_PostIt Implements IviewBase.PostItSistema
        Get
            Try
                PostItSistema = DirectCast(Me.Application.Item("oSystemPostIt"), COL_PostIt)
            Catch ex As Exception
                PostItSistema = Nothing
                Me.Application.Item("ShowSystemPostIt") = False
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.COL_PostIt)
            Me.Application.Lock()
            Me.Application.Item("oSystemPostIt") = value
            Me.Application.UnLock()
        End Set
    End Property
    Public Property ShowPostItSistema() As Boolean Implements IviewBase.ShowPostItSistema
        Get
            Try
                ShowPostItSistema = DirectCast(Me.Application.Item("ShowSystemPostIt"), Boolean)
            Catch ex As Exception
                Me.Application.Item("ShowSystemPostIt") = False
                ShowPostItSistema = False
            End Try
        End Get
        Set(ByVal value As Boolean)
            Me.Application.Lock()
            Me.Application.Item("ShowSystemPostIt") = value
            Me.Application.UnLock()
        End Set
    End Property
    Public Property RiepilogoPostIt() As Integer Implements IviewBase.RiepilogoPostIt
        Get
            Try
                RiepilogoPostIt = DirectCast(Session("Popupwin"), Integer)
            Catch ex As Exception
                RiepilogoPostIt = 0
            End Try

        End Get
        Set(ByVal value As Integer)
            Session("Popupwin") = value
        End Set
    End Property

    Public Property UtentiConnessi() As Integer Implements IviewBase.UtentiConnessi
        Get
            Try
                UtentiConnessi = Me.Application.Item("utentiConnessi")
            Catch ex As Exception
                UtentiConnessi = 1
            End Try
        End Get
        Set(ByVal value As Integer)
            Me.Application.Lock()
            Me.Application.Item("utentiConnessi") = value
            Me.Application.UnLock()
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsSessioneScaduta() Then
            Me.SetInternazionalizzazione()
            Me.BindDati()
        End If
    End Sub



    Public Sub CambiaComunitaFromHistory(ByVal ComunitaID As Integer) Implements IviewBase.CambiaComunitaFromHistory
        Dim DestinazioneID As Integer
        DestinazioneID = TrovaComunitaDestinazione(ComunitaID)

        If DestinazioneID <= 0 Then
            Me.GoToPortale()
        ElseIf DestinazioneID > 0 Then

            Session("AdminForChange") = False
            Session("CMNT_path_forAdmin") = ""
            Session("idComunita_forAdmin") = ""
            Session("CMNT_path_forNews") = ""
            Session("CMNT_ID_forNews") = ""

            Me.RetrieveServices(Me.RuoloCorrenteID, ComunitaID)
            Session("ArrComunita") = Me.RemoveFromHystoryElement(0, Session("ArrComunita"), ComunitaID)
            If COL_Comunita.ShowCover(DestinazioneID, Me.UtenteCorrente.Id) Then
                Me.RedirectToUrl("Generici/Cover.aspx")
            Else
                Me.RedirectToUrl(PlainRedirectToDefaultPage(DestinazioneID, Me.UtenteCorrente.Id))  ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
            End If
            '	Response.Redirect("./../Comunita/comunita.aspx") ' se non eseguo il redirect non mi fa vedere il menu aggiornato
        End If
    End Sub
    Private Function TrovaComunitaDestinazione(ByVal ComunitaID As Integer) As Integer
        Dim oIscrizione As New COL_RuoloPersonaComunita
        Dim FoundComunitaID As Integer

        Try
            oIscrizione.Estrai(ComunitaID, Me.UtenteCorrente.Id)
            If oIscrizione.Errore = Errori_Db.None AndAlso oIscrizione.Abilitato AndAlso oIscrizione.Attivato And (oIscrizione.TipoRuolo.Id = Main.TipoRuoloStandard.AccessoNonAutenticato Or oIscrizione.TipoRuolo.Id > 0) Then
                Dim oComunita As New COL_Comunita
                oComunita.Id = ComunitaID
                If (oComunita.isBloccata() = False) Then
                    Session("IdComunita") = ComunitaID
                    If Session("LogonAs") = False Then
                        oIscrizione.UpdateUltimocollegamento()
                    End If
                    Session("IdRuolo") = oIscrizione.TipoRuolo.Id
                    Session("RLPC_ID") = oIscrizione.Id
                    Return ComunitaID
                End If
            ElseIf ComunitaID > 0 Then
                FoundComunitaID = TrovaComunitaDestinazione(Me.History.FindFatherElementByID(ComunitaID).ComunitaID)
            End If
        Catch ex As Exception
            FoundComunitaID = ComunitaID
        End Try

        Return FoundComunitaID
    End Function
    Private Sub RetrieveServices(ByVal RuoloID As Integer, ByVal ComunitaID As Integer)
        Dim oListaServizi As GenericCollection(Of PlainServizioComunita)
        oListaServizi = PlainServizioComunita.ElencaByComunita(RuoloID, ComunitaID)

        If oListaServizi.Count > 0 Then
            Dim ArrPermessi(oListaServizi.Count - 1, 2) As String
            Dim indice As Integer = 0
            For Each oServizio As PlainServizioComunita In oListaServizi
                ArrPermessi(indice, 0) = oServizio.Codice
                ArrPermessi(indice, 1) = oServizio.ID
                ArrPermessi(indice, 2) = oServizio.Permessi
                indice += 1
            Next
            Session("ArrPermessi") = ArrPermessi
        Else
            Session("ArrPermessi") = Nothing
        End If
    End Sub

    Public Sub CambiaComunita(ByVal OrganizzazioneID As Integer, ByVal ComunitaID As Integer, ByVal TipoComunitaID As Integer, ByVal RuoloID As Integer, ByVal iListaServizi As ServiziCorrenti, ByVal iHistory As HistoryElement) Implements IviewBase.CambiaComunita
        Session("TPCM_ID") = TipoComunitaID
        Session("idComunita") = ComunitaID
        Session("IdRuolo") = RuoloID

        Me._History = iHistory
        Me.SetElencoServizi(iListaServizi)


        Dim ArrComunita(,) As String = Nothing
        Dim Indice As Integer = 0

        If Not IsNothing(iHistory) Then
            If iHistory.ComunitaID = ComunitaID Then
                ReDim Preserve ArrComunita(3, Indice)
                ArrComunita(0, Indice) = iHistory.ComunitaID
                ArrComunita(1, Indice) = iHistory.Nome
                ArrComunita(2, Indice) = iHistory.Percorso
                ArrComunita(3, Indice) = iHistory.RuoloID
            ElseIf Not IsNothing(iHistory.SubElement) Then
                ReDim Preserve ArrComunita(3, Indice)
                ArrComunita(0, Indice) = iHistory.ComunitaID
                ArrComunita(1, Indice) = iHistory.Nome
                ArrComunita(2, Indice) = iHistory.Percorso
                ArrComunita(3, Indice) = iHistory.RuoloID
                Me.LoadFromHystoryElement(Indice + 1, ArrComunita, iHistory.SubElement, ComunitaID)
            End If
        End If
        Session("ArrComunita") = ArrComunita
        Dim ArrPermessi(,) As String = Nothing
        Indice = 0
        If iListaServizi.Count > 0 Then
            ReDim Preserve ArrComunita(Indice, 2)
            For Each oElement As ServiceElement In iListaServizi
                ArrPermessi(Indice, 0) = oElement.ServizioCode
                ArrPermessi(Indice, 1) = oElement.ServizioID
                ArrPermessi(Indice, 2) = oElement.ServizioConcreto.PermessiAssociati
            Next
        End If
        Session("ArrPermessi") = ArrPermessi
        Session("ORGN_id") = OrganizzazioneID

    End Sub
    Private Sub LoadFromHystoryElement(ByVal Indice As Integer, ByVal ArrComunita(,) As String, ByVal oElement As HistoryElement, ByVal ComunitaID As Integer)
        If oElement.ComunitaID = ComunitaID Then
            ReDim Preserve ArrComunita(3, Indice)
            ArrComunita(0, Indice) = oElement.ComunitaID
            ArrComunita(1, Indice) = oElement.Nome
            ArrComunita(2, Indice) = oElement.Percorso
            ArrComunita(3, Indice) = oElement.RuoloID
        ElseIf Not IsNothing(oElement.SubElement) Then
            ReDim Preserve ArrComunita(3, Indice)
            ArrComunita(0, Indice) = oElement.ComunitaID
            ArrComunita(1, Indice) = oElement.Nome
            ArrComunita(2, Indice) = oElement.Percorso
            ArrComunita(3, Indice) = oElement.RuoloID
            Me.LoadFromHystoryElement(Indice + 1, ArrComunita, oElement.SubElement, ComunitaID)
        End If
    End Sub
    Private Function RemoveFromHystoryElement(ByVal Indice As Integer, ByVal ArrComunita(,) As String, ByVal ComunitaID As Integer) As String(,)
        Try
            If ArrComunita(0, Indice) = ComunitaID Then
                ReDim Preserve ArrComunita(3, Indice)
                Return ArrComunita
            Else
                Return Me.RemoveFromHystoryElement(Indice + 1, ArrComunita, ComunitaID)
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ViewGetPostBackEventReference(ByVal argomenti As String) As String Implements IviewBase.ViewGetPostBackEventReference
        Return Page.ClientScript.GetPostBackEventReference(Me, argomenti)
    End Function

    Private Function CreateObjectsList(ByVal oType As Integer, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
        Dim oList As New List(Of lm.ActionDataContract.ObjectAction)
        oList.Add(New lm.ActionDataContract.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = PageUtility.CurrentModule.ID})
        Return oList
    End Function

    Public Sub LogOutFunction() 'Optional ByVal UserId As Integer = -1)

        Try
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            If Not IsNothing(oPersona) Then

                'Pulizia allegati mail
                Dim oFile As New FileLayer.COL_File

                Environment.CurrentDirectory = Server.MapPath("./../")
                lm.Comol.Core.File.Delete.Directory(".\Mail\" & oPersona.ID & "\", True)

                'Chiusura Chat
                lm.Comol.Modules.Standard.Chat.IM_SharedFunction.DiscardAllChats(oPersona.ID)
            End If

        Catch ex As Exception

        End Try



    End Sub
End Class

