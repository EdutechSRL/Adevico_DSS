Imports System
'Imports System.IO
Imports System.Xml
Imports System.Text
Imports COL_DataLayer
Imports COL_BusinessLogic_v2.CL_persona

Namespace Comunita
    Public Class COL_TreeComunita
        Inherits ObjectBase

        Private Const _MaxIteration = 30000


        Private _CachePath As String = "TREE_communityPath{0}"
        Private _CacheDataset As String = "TREE_CacheDataset{0}"
        Private Enum StringaAbilitato
            abilitato = 1
            bloccato = 0
            inAttesa = -1
            comunitaArchiviata = 2
            comunitaBloccata = 3
        End Enum

#Region "Private Propriety"
        Private n_name As String
        Private n_Errore As FileError
        Private n_Directory As String
#End Region

#Region "Public Propriety"
        Public Enum FileError
            none = 0
            exsist = 1
            'notUploaded = 2
            zeroByte = 3
            dirNotFound = 4
            fileNotFound = 5
        End Enum
        Public Property Nome() As String
            Get
                Nome = n_name
            End Get
            Set(ByVal Value As String)
                n_name = Value
            End Set
        End Property

        Public ReadOnly Property PersonID() As Integer
            Get
                Return CInt(Replace(n_name, ".xml", ""))
            End Get
        End Property

        Public ReadOnly Property Errore() As FileError
            Get
                Errore = n_Errore
            End Get
        End Property
        Public Property Directory() As String
            Get
                Directory = n_Directory
            End Get
            Set(ByVal Value As String)
                n_Directory = Value
            End Set
        End Property
#End Region

        Sub New()
            Me.n_Errore = FileError.none
        End Sub

        Public Function Exist() As Boolean
            Try
                If lm.Comol.Core.File.Exists.Directory(Me.n_Directory) Then
                    If lm.Comol.Core.File.Exists.File(Me.n_Directory & Me.n_name) Then
                        Me.n_Errore = FileError.none
                        Return True
                    Else
                        Me.n_Errore = FileError.fileNotFound
                        Return False
                    End If
                    Return False
                Else
                    lm.Comol.Core.File.Create.Directory(Me.n_Directory)
                    Me.n_Errore = FileError.dirNotFound
                    Return False
                End If
            Catch ex As Exception
                Me.n_Errore = FileError.fileNotFound
                Return False
            End Try
            Return False
        End Function
        Private Function isToUpdate(ByVal Giorni As Integer) As Boolean
            Dim iResponse As Boolean = False
            Try
                Dim oDataCreazione As DateTime
                oDataCreazione = lm.Comol.Core.File.ContentOf.File_LastWrite(Me.n_Directory & Me.n_name)
                oDataCreazione = oDataCreazione.AddDays(Giorni)
                If Not (oDataCreazione < Now) Then
                    Dim oDataset As New DataSet

                    Try
                        oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                        If oDataset.Tables(0).Rows.Count = 0 Then
                            iResponse = True
                        End If
                    Catch ex As Exception
                        iResponse = True
                    End Try
                    Return iResponse
                End If
            Catch ex As Exception
                Return True
            End Try
            Return True
        End Function

#Region "Inserimento, Modifica Run-Time"
        Public Function Insert(ByVal oComunita As COL_Comunita, ByVal Path As String, ByVal nomi As String, ByVal oRuoloPersonaComunita As COL_RuoloPersonaComunita) As Boolean
            Dim Responsabile, Proprietario As String
            Dim nomiArray() As String

            nomiArray = nomi.Split(",")
            If IsNothing(nomiArray(0)) Then
                Responsabile = ""
            Else
                Responsabile = nomiArray(0)
            End If
            If IsNothing(nomiArray(1)) Then
                Proprietario = ""
            Else
                Proprietario = nomiArray(1)
            End If
            Me.Insert(oComunita, Path, Responsabile, Proprietario, oRuoloPersonaComunita)
        End Function
        Public Function Insert(ByVal oComunita As COL_Comunita, ByVal Path As String, ByVal Responsabile As String, ByVal Proprietario As String, ByVal oRuoloPersonaComunita As COL_RuoloPersonaComunita) As Boolean
            Dim oDataset As New DataSet
            Dim oResponsabile As New COL_Persona


            Try
                oResponsabile = oComunita.GetResponsabile
                If oResponsabile.Errore <> Errori_Db.None Then
                    oResponsabile.ID = 0
                End If
                If Me.Exist() = False Then
                    ' Inizializzo il dataset 
                    Me.RigeneraFileXML()
                End If
                Dim cacheKey As String = String.Format(_CachePath, "_" & Me.PersonID.ToString)
                ObjectBase.PurgeCacheItems(cacheKey)

                Dim CMNT_ID, i, totale As Integer
                CMNT_ID = oComunita.Id

                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)

                If oDataset.Tables.Count = 0 Then
                    oDataset = Me.CreaAlberoIscrizione(oRuoloPersonaComunita.Persona.ID, COL_Persona.GetLinguaID(oRuoloPersonaComunita.Persona.ID))
                    oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    Exit Function
                Else
                    If oDataset.Tables(0).Rows.Count = 0 Then
                        oDataset = Me.CreaAlberoIscrizione(oRuoloPersonaComunita.Persona.ID, COL_Persona.GetLinguaID(oRuoloPersonaComunita.Persona.ID))
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                        Exit Function
                    End If
                    'Me.RigeneraFileXML()
                    'oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                End If

                If CMNT_ID > 0 And oDataset.Tables.Count > 0 Then
                    Dim oData As DataView
                    Dim oRow As DataRow
                    oData = oDataset.Tables(0).DefaultView

                    oData.RowFilter = "CMNT_ID =" & CMNT_ID & " AND ALCM_Path='" & Path & "'"
                    totale = oData.Count

                    If oDataset.Tables(0).Columns.Contains("CMNT_Archiviata") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_Archiviata", System.Type.GetType("System.Boolean"))
                    End If

                    If oDataset.Tables(0).Columns.Contains("CMNT_Bloccata") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_Bloccata", System.Type.GetType("System.Boolean"))
                    End If

                    If oDataset.Tables(0).Columns.Contains("CMNT_PRFS_ID") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_PRFS_ID", System.Type.GetType("System.Int32"))
                    End If

                    If oDataset.Tables(0).Columns.Contains("CMNT_AccessoLibero") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_AccessoLibero", System.Type.GetType("System.Boolean"))
                    End If
                    If oDataset.Tables(0).Columns.Contains("CMNT_AccessoCopisteria") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_AccessoCopisteria", System.Type.GetType("System.Boolean"))
                    End If


                    If totale > 0 Then

                        oData.RowFilter = "CMNT_ID =" & CMNT_ID
                        totale = oData.Count
                        For i = 0 To totale - 1
                            oRow = oData.Item(i).Row

                            oRow.Item("CMNT_ID") = oComunita.Id
                            oRow.Item("CMNT_nome") = oComunita.Nome
                            'forse va tolta ?
                            oRow.Item("CMNT_PRSN_Id") = oComunita.CreatoreID

                            oRow.Item("ALCM_ResponsabileID") = oResponsabile.ID
                            'escludo le cose che hanno in comune ma DIVERSE
                            '   oRow.Item("ALCM_Path") = Path
                            '  oRow.Item("ALCM_PadreID") = oComunita.IdPadre
                            '  oRow.Item("CMNT_ORGN_id") = oComunita.Organizzazione.Id
                            oRow.Item("CMNT_isChiusa") = oComunita.IsChiusa
                            oRow.Item("CMNT_Responsabile") = Responsabile
                            oRow.Item("AnagraficaCreatore") = Proprietario
                            oRow.Item("CMNT_MaxIscritti") = oComunita.MaxIscritti
                            ' oRow.Item("CMNT_isIscritto") = True

                            oRow.Item("CMNT_TPCM_id") = oComunita.TipoComunita.ID
                            oRow.Item("TPCM_Descrizione") = oComunita.TipoComunita.Descrizione
                            oRow.Item("TPCM_Icona") = oComunita.TipoComunita.Icona


                            oRow.Item("RLPC_TPRL_id") = oRuoloPersonaComunita.TipoRuolo.Id
                            oRow.Item("RLPC_attivato") = oRuoloPersonaComunita.Attivato
                            oRow.Item("RLPC_abilitato") = oRuoloPersonaComunita.Abilitato
                            oRow.Item("TPRL_Nome") = oRuoloPersonaComunita.TipoRuolo.Nome
                            oRow.Item("CMNT_Archiviata") = oComunita.Archiviata
                            oRow.Item("CMNT_Bloccata") = oComunita.Bloccata


                            ' INSERITO FEBBRAIO 2007
                            '     oRow.Item("CMNT_PRFS_ID") = oComunita.GetProfiloServizioID
                            oRow.Item("CMNT_AccessoLibero") = oComunita.HasAccessoLibero
                            oRow.Item("CMNT_AccessoCopisteria") = oComunita.HasAccessoCopisteria
                            ' FINE INSERITO FEBBRAIO 2007
                          

                            If IsDate(oRuoloPersonaComunita.UltimoCollegamento) Then
                                If Not Equals(New Date, oRuoloPersonaComunita.UltimoCollegamento) Then
                                    oRow.Item("RLPC_UltimoCollegamento") = oRuoloPersonaComunita.UltimoCollegamento
                                End If
                            End If
                        Next

                    Else
                        Dim Percorso() As String

                        Dim PadreVirtualeID As Integer
                        Dim ALCM_RealPath As String = ""

                        If oComunita.IdPadre = 0 Then
                            PadreVirtualeID = 0
                            ALCM_RealPath = "." & oComunita.Id & "."
                        Else
                            Percorso = Path.Split(".")
                            Try
                                PadreVirtualeID = Percorso(UBound(Percorso) - 2)
                            Catch ex As Exception
                                PadreVirtualeID = 0
                            End Try
                            Try
                                If ALCM_RealPath <> "." & oComunita.Id & "." Then
                                    ALCM_RealPath = Replace(Path, "." & oComunita.Id & ".", ".")
                                End If
                            Catch ex As Exception

                            End Try
                        End If

                        oRow = oDataset.Tables(0).NewRow
                        If oRuoloPersonaComunita.isResponsabile Then
                            oRow.Item("ALCM_ResponsabileID") = oRuoloPersonaComunita.Persona.ID
                        Else
                            oRow.Item("ALCM_ResponsabileID") = oResponsabile.ID
                        End If

                        oRow.Item("ALCM_HasFigli") = False
                        oRow.Item("NoDelete") = 1
                        oRow.Item("CMNT_dataCreazione") = oComunita.DataCreazione
                        oRow.Item("CMNT_dataInizioIscrizione") = oComunita.DataInizioIscrizione
                        oRow.Item("CMNT_dataFineIscrizione") = oComunita.DataFineIscrizione
                        oRow.Item("ALCM_PadreID") = PadreVirtualeID
                        oRow.Item("ALCM_PadreVirtuale_ID") = PadreVirtualeID

                        oRow.Item("ALCM_isChiusa") = oComunita.IsChiusa
                        If PadreVirtualeID = oComunita.IdPadre Then
                            oRow.Item("ALCM_isDiretto") = True
                        Else
                            oRow.Item("ALCM_isDiretto") = False
                        End If
                        oRow.Item("CMNT_Iscritti") = 0

                        Dim oDataViewPAdre As New DataView
                        oDataViewPAdre = oDataset.Tables(0).DefaultView
                        Try
                            oDataViewPAdre.RowFilter = "ALCM_PAth='" & ALCM_RealPath & "'"
                            If oDataViewPAdre.Count > 0 Then
                                oRow.Item("CMNT_nomePadre") = oDataViewPAdre.Item(0).Item("CMNT_nome")
                                oRow.Item("ALCM_PercorsoDiretto") = oDataViewPAdre.Item(0).Item("ALCM_PercorsoDiretto")
                                oRow.Item("ALCM_isChiusaForPadre") = False
                            Else
                                oRow.Item("CMNT_nomePadre") = ""
                                oRow.Item("ALCM_PercorsoDiretto") = True
                                oRow.Item("ALCM_isChiusaForPadre") = False
                            End If

                        Catch ex As Exception

                        End Try
                        oRow.Item("CMNT_ID") = oComunita.Id
                        oRow.Item("CMNT_PRSN_Id") = oComunita.CreatoreID
                        oRow.Item("CMNT_nome") = oComunita.Nome
                        oRow.Item("ALCM_Path") = Path
                        oRow.Item("ALCM_PadreID") = oComunita.IdPadre
                        oRow.Item("CMNT_ORGN_id") = oComunita.Organizzazione.Id
                        oRow.Item("CMNT_isChiusa") = oComunita.IsChiusa
                        oRow.Item("CMNT_MaxIscritti") = oComunita.MaxIscritti
                        oRow.Item("CMNT_Responsabile") = Responsabile
                        oRow.Item("AnagraficaCreatore") = Proprietario
                        oRow.Item("ALCM_Livello") = oComunita.Livello

                        oRow.Item("ALCM_RealPath") = ALCM_RealPath
                        'oRow.Item("CMNT_IsComunita") = True
                        'If Path <> "" Then
                        '    oRow.Item("lvl") = Path.Split(".").Length - 3
                        'Else
                        '    oRow.Item("lvl") = 0
                        'End If
                        '  oRow.Item("CMNT_isIscritto") = True

                        oRow.Item("CMNT_TPCM_id") = oComunita.TipoComunita.ID
                        oRow.Item("TPCM_Descrizione") = oComunita.TipoComunita.Descrizione
                        oRow.Item("TPCM_Icona") = oComunita.TipoComunita.Icona


                        oRow.Item("RLPC_TPRL_id") = oRuoloPersonaComunita.TipoRuolo.Id
                        oRow.Item("RLPC_attivato") = oRuoloPersonaComunita.Attivato
                        oRow.Item("RLPC_abilitato") = oRuoloPersonaComunita.Abilitato
                        oRow.Item("TPRL_Nome") = oRuoloPersonaComunita.TipoRuolo.Nome
                        oRow.Item("RLPC_UltimoCollegamento") = oRuoloPersonaComunita.UltimoCollegamento

                        oRow.Item("CMNT_CanSubscribe") = oComunita.CanSubscribe
                        oRow.Item("CMNT_CanUnsubscribe") = oComunita.CanUnsubscribe
                        oRow.Item("CMNT_MaxIscrittiOverList") = oComunita.OverMaxIscritti
                        oRow.Item("CMNT_Archiviata") = oComunita.Archiviata
                        oRow.Item("CMNT_Bloccata") = oComunita.Bloccata


                        ' INSERITO FEBBRAIO 2007
                        '     oRow.Item("CMNT_PRFS_ID") = oComunita.GetProfiloServizioID
                        oRow.Item("CMNT_AccessoLibero") = oComunita.HasAccessoLibero
                        oRow.Item("CMNT_AccessoCopisteria") = oComunita.HasAccessoCopisteria
                        ' FINE INSERITO FEBBRAIO 2007

                        oDataset.Tables(0).Rows.Add(oRow)

                        oData.RowFilter = "CMNT_ID =" & oComunita.IdPadre & " AND ALCM_PATh<>'" & ALCM_RealPath & "'"
                        If oData.Count > 0 Then
                            totale = oData.Count - 1
                            For i = 0 To totale
                                Dim oNewRow As DataRow
                                oNewRow = oDataset.Tables(0).NewRow
                                oNewRow.ItemArray = oRow.ItemArray

                                oNewRow.Item("ALCM_RealPath") = oData.Item(i).Row("ALCM_Path")
                                oNewRow.Item("ALCM_Path") = oData.Item(i).Row("ALCM_Path") & oComunita.Id & "."
                                oNewRow.Item("ALCM_PercorsoDiretto") = oData.Item(i).Row("ALCM_PercorsoDiretto")
                                oDataset.Tables(0).Rows.Add(oNewRow)
                            Next

                        End If

                    End If
                    oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                End If
            Catch ex As Exception

            End Try

        End Function

        Public Function ClonaIscrizione(ByVal ComunitaID As Integer, ByVal PadreID As Integer, ByVal Path As String) As Boolean
            Dim oDataset As New DataSet

            Try
                If Me.Exist() = False Then
                    Me.RigeneraFileXML()
                End If
                Dim cacheKey As String = String.Format(_CachePath, "_" & Me.PersonID.ToString)
                ObjectBase.PurgeCacheItems(cacheKey)

                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                    Dim oDataView As DataView

                    oDataView = oDataset.Tables(0).DefaultView

                    oDataView.RowFilter = "CMNT_ID =" & ComunitaID & " AND ALCM_Path<>'" & Path & "'"
                    If oDataset.Tables(0).Columns.Contains("CMNT_Archiviata") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_Archiviata", System.Type.GetType("System.Boolean"))
                    End If

                    If oDataset.Tables(0).Columns.Contains("CMNT_Bloccata") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_Bloccata", System.Type.GetType("System.Boolean"))
                    End If
                    If oDataset.Tables(0).Columns.Contains("CMNT_PRFS_ID") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_PRFS_ID", System.Type.GetType("System.Int32"))
                    End If

                    If oDataset.Tables(0).Columns.Contains("CMNT_AccessoLibero") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_AccessoLibero", System.Type.GetType("System.Boolean"))
                    End If
                    If oDataset.Tables(0).Columns.Contains("CMNT_AccessoCopisteria") = False Then
                        oDataset.Tables(0).Columns.Add("CMNT_AccessoCopisteria", System.Type.GetType("System.Boolean"))
                    End If

                    If oDataView.Count > 0 Then
                        Dim oNewRowComunita As DataRow
                        oNewRowComunita = oDataset.Tables(0).NewRow
                        oNewRowComunita.Item("ALCM_ResponsabileID") = oDataView.Item(0).Item("ALCM_ResponsabileID")
                        oNewRowComunita.Item("ALCM_HasFigli") = oDataView.Item(0).Item("ALCM_HasFigli")
                        oNewRowComunita.Item("NoDelete") = oDataView.Item(0).Item("NoDelete")
                        oNewRowComunita.Item("CMNT_dataCreazione") = oDataView.Item(0).Item("CMNT_dataCreazione")
                        oNewRowComunita.Item("CMNT_dataInizioIscrizione") = oDataView.Item(0).Item("CMNT_dataInizioIscrizione")
                        oNewRowComunita.Item("CMNT_dataFineIscrizione") = oDataView.Item(0).Item("CMNT_dataFineIscrizione")
                        oNewRowComunita.Item("ALCM_PadreID") = oDataView.Item(0).Item("ALCM_PadreID")
                        oNewRowComunita.Item("ALCM_PadreVirtuale_ID") = PadreID
                        oNewRowComunita.Item("ALCM_isChiusa") = oDataView.Item(0).Item("ALCM_isChiusa")
                        If PadreID = oNewRowComunita.Item("ALCM_PadreID") Then
                            oNewRowComunita.Item("ALCM_isDiretto") = True
                        Else
                            oNewRowComunita.Item("ALCM_isDiretto") = False
                        End If
                        oNewRowComunita.Item("CMNT_Iscritti") = oDataView.Item(0).Item("CMNT_Iscritti")

                        oNewRowComunita.Item("CMNT_nomePadre") = oDataView.Item(0).Item("CMNT_nomePadre")
                        oNewRowComunita.Item("ALCM_PercorsoDiretto") = False
                        oNewRowComunita.Item("ALCM_isChiusaForPadre") = False
                        oNewRowComunita.Item("CMNT_ID") = ComunitaID
                        oNewRowComunita.Item("CMNT_PRSN_Id") = oDataView.Item(0).Item("CMNT_PRSN_Id")
                        oNewRowComunita.Item("ALCM_Path") = Path
                        oNewRowComunita.Item("ALCM_PadreID") = oDataView.Item(0).Item("ALCM_PadreID")
                        oNewRowComunita.Item("CMNT_ORGN_id") = oDataView.Item(0).Item("CMNT_ORGN_id")
                        oNewRowComunita.Item("CMNT_isChiusa") = oDataView.Item(0).Item("CMNT_isChiusa")
                        oNewRowComunita.Item("CMNT_MaxIscritti") = oDataView.Item(0).Item("CMNT_MaxIscritti")
                        oNewRowComunita.Item("CMNT_Responsabile") = oDataView.Item(0).Item("CMNT_Responsabile")
                        oNewRowComunita.Item("AnagraficaCreatore") = oDataView.Item(0).Item("AnagraficaCreatore")
                        oNewRowComunita.Item("ALCM_Livello") = oDataView.Item(0).Item("ALCM_Livello")

                        Dim ALCM_RealPath As String
                        ALCM_RealPath = Path
                        ALCM_RealPath = Replace(Path, "." & PadreID & ".", ".")
                        oNewRowComunita.Item("ALCM_RealPath") = ALCM_RealPath
                        oNewRowComunita.Item("CMNT_TPCM_id") = oDataView.Item(0).Item("CMNT_TPCM_id")
                        oNewRowComunita.Item("TPCM_Descrizione") = oDataView.Item(0).Item("TPCM_Descrizione")
                        oNewRowComunita.Item("TPCM_Icona") = oDataView.Item(0).Item("TPCM_Icona")
                        oNewRowComunita.Item("RLPC_TPRL_id") = oDataView.Item(0).Item("RLPC_TPRL_id")
                        oNewRowComunita.Item("RLPC_attivato") = oDataView.Item(0).Item("RLPC_attivato")
                        oNewRowComunita.Item("RLPC_abilitato") = oDataView.Item(0).Item("RLPC_abilitato")
                        oNewRowComunita.Item("TPRL_Nome") = oDataView.Item(0).Item("TPRL_Nome")
                        oNewRowComunita.Item("RLPC_UltimoCollegamento") = oDataView.Item(0).Item("RLPC_UltimoCollegamento")
                        oNewRowComunita.Item("CMNT_CanSubscribe") = oDataView.Item(0).Item("CMNT_CanSubscribe")
                        oNewRowComunita.Item("CMNT_CanUnsubscribe") = oDataView.Item(0).Item("CMNT_CanUnsubscribe")
                        oNewRowComunita.Item("CMNT_MaxIscrittiOverList") = oDataView.Item(0).Item("CMNT_MaxIscrittiOverList")
                        oNewRowComunita.Item("CMNT_Archiviata") = oDataView.Item(0).Item("CMNT_Archiviata")
                        oNewRowComunita.Item("CMNT_Bloccata") = oDataView.Item(0).Item("CMNT_Bloccata")
                        oNewRowComunita.Item("PRDO_descrizione") = oDataView.Item(0).Item("PRDO_descrizione")
                        oNewRowComunita.Item("PRDO_id") = oDataView.Item(0).Item("PRDO_id")
                        oNewRowComunita.Item("CMNT_anno") = oDataView.Item(0).Item("CMNT_anno")
                        oNewRowComunita.Item("CMNT_AnnoAccademico") = oDataView.Item(0).Item("CMNT_AnnoAccademico")
                        oNewRowComunita.Item("CRSO_codice") = oDataView.Item(0).Item("CRSO_codice")

                        oNewRowComunita.Item("TPCS_ID") = oDataView.Item(0).Item("TPCS_ID")
                        oNewRowComunita.Item("TPCS_nome") = oDataView.Item(0).Item("TPCS_nome")

                        ' INSERITO FEBBRAIO 2007
                        Try
                            '  oNewRowComunita.Item("CMNT_PRFS_ID") = oDataView.Item(0).Item("CMNT_PRFS_ID")
                        Catch ex As Exception

                        End Try
                        Try
                            oNewRowComunita.Item("CMNT_AccessoLibero") = oDataView.Item(0).Item("CMNT_AccessoLibero")
                        Catch ex As Exception

                        End Try
                        Try
                            oNewRowComunita.Item("CMNT_AccessoLibero") = oDataView.Item(0).Item("CMNT_AccessoLibero")
                        Catch ex As Exception

                        End Try
                        ' FINE INSERITO FEBBRAIO 2007

                        oDataset.Tables(0).Rows.Add(oNewRowComunita)
                        oDataView.RowFilter = "CMNT_ID =" & PadreID & " AND ALCM_PATh<>'" & Path & "'"
                        If oDataView.Count > 0 Then
                            Dim i, totale As Integer
                            totale = oDataView.Count - 1
                            For i = 0 To totale
                                Dim oNewRow As DataRow
                                oNewRow = oDataset.Tables(0).NewRow
                                oNewRow.ItemArray = oNewRowComunita.ItemArray

                                oNewRow.Item("ALCM_RealPath") = oDataView.Item(i).Row("ALCM_Path")
                                oNewRow.Item("ALCM_Path") = oDataView.Item(i).Row("ALCM_Path") & ComunitaID & "."
                                oNewRow.Item("ALCM_PercorsoDiretto") = oDataView.Item(i).Row("ALCM_PercorsoDiretto")
                                oDataset.Tables(0).Rows.Add(oNewRow)
                            Next
                        End If

                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                        Return True
                    End If
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Function Update(ByVal oComunita As COL_Comunita, ByVal Path As String, ByVal nomi As String, ByVal oRuoloPersonaComunita As COL_RuoloPersonaComunita) As Boolean
            Dim Responsabile, Proprietario As String
            Dim nomiArray() As String

            nomiArray = nomi.Split(",")
            If IsNothing(nomiArray(0)) Then
                Responsabile = ""
            Else
                Responsabile = nomiArray(0)
            End If
            If IsNothing(nomiArray(1)) Then
                Proprietario = ""
            Else
                Proprietario = nomiArray(1)
            End If
            Me.Insert(oComunita, Path, Responsabile, Proprietario, oRuoloPersonaComunita)
        End Function
        Public Function CambiaResponsabile(ByVal ComunitaID As Integer, ByVal isResponsabile As Boolean) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() Then
                    Dim i, totale As Integer

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                        Dim oData As DataView
                        Dim oRow As DataRow
                        oData = oDataset.Tables(0).DefaultView

                        oData.RowFilter = "CMNT_ID =" & ComunitaID
                        totale = oData.Count
                        If totale > 0 Then
                            oData.RowFilter = "CMNT_ID =" & ComunitaID
                            totale = oData.Count
                            For i = 0 To totale - 1
                                oRow = oData.Item(i).Row

                                oRow.Item("CMNT_Responsabile") = isResponsabile
                                '   oRow.Item("CMNT_isIscritto") = True
                            Next
                        End If
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    End If
                End If
            Catch ex As Exception

            End Try
        End Function
        Public Function CambiaAbilitazione(ByVal ComunitaID As Integer, ByVal isAbilitato As Boolean) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() Then
                    Dim i, totale As Integer

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                        Dim oData As DataView
                        Dim oRow As DataRow
                        oData = oDataset.Tables(0).DefaultView

                        oData.RowFilter = "CMNT_ID=" & ComunitaID
                        totale = oData.Count
                        If totale > 0 Then
                            For i = 0 To totale - 1
                                oRow = oData.Item(i).Row
                                '  oRow.Item("CMNT_isIscritto") = True
                                oRow.Item("RLPC_abilitato") = isAbilitato
                                If isAbilitato Then
                                    oRow.Item("RLPC_Attivato") = isAbilitato
                                End If
                            Next
                            oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
        End Function
        Public Function CambiaIsBloccata(ByVal ComunitaID As Integer, ByVal Bloccata As Boolean) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() Then
                    Dim i, totale As Integer

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                        Dim oData As DataView
                        Dim oRow As DataRow
                        oData = oDataset.Tables(0).DefaultView

                        oData.RowFilter = "CMNT_ID =" & ComunitaID
                        totale = oData.Count
                        If totale > 0 Then
                            oData.RowFilter = "CMNT_ID =" & ComunitaID
                            totale = oData.Count
                            For i = 0 To totale - 1
                                oRow = oData.Item(i).Row
                                oRow.Item("CMNT_Bloccata") = Bloccata
                            Next
                        End If
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    End If
                End If
            Catch ex As Exception

            End Try
        End Function
        Public Function Delete(ByVal ComunitaID As Integer, ByVal Path As String) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() = True Then
                    Dim i, totale As Integer
                    Dim oData As DataView

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    oData = oDataset.Tables(0).DefaultView

                    'Elimino tutti i record figli della comunità che sto per cancellare
                    oData.RowFilter = "ALCM_RealPath like '" & Path & "%'"

                    totale = oData.Count - 1
                    If oData.Count > 0 Then
                        For i = totale To 0 Step -1
                            oData.Delete(i)
                        Next
                    End If

                    'elimino ora le eventuali comunita con stesso id, 
                    'ma su livello superiore o su altro ramo dell'albero
                    oData.RowFilter = "CMNT_ID =" & ComunitaID
                    totale = oData.Count - 1

                    While oData.Count > 0
                        If oData.Count > 0 Then
                            oData.RowFilter = "ALCM_Path like '" & oData.Item(0).Item("ALCM_Path") & "%'"
                            For i = oData.Count - 1 To 0 Step -1
                                oData.Delete(i)
                            Next
                            oData.RowFilter = "CMNT_ID =" & ComunitaID
                        End If
                    End While

                End If

                oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
            Catch ex As Exception

            End Try
            Return False
        End Function
#End Region

#Region "Nuovi Metodi"
        Public Function FindCommunityPath(ByVal PersonID As Integer) As List(Of CommunityPath)
            Dim iResponse As New List(Of CommunityPath)

            'Dim cacheKey As String = String.Format(_CachePath, "_" & PersonID.ToString)
            'If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
            Dim oDocument As XDocument
            Dim oDataset As New DataSet
            Dim filtro As String = " RLPC_TPRL_id <> -2 AND RLPC_TPRL_id <> -3 "

            Try
                oDocument = XDocument.Load(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                Dim oQuery = oDocument.Root.Elements("Table").ToList()
                Dim i, totale As Integer
                Dim ElencoComunitaID As String = ""
                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If filtro <> "" Then
                    Dim oDatasetTemp As New DataSet
                    Dim oData As DataView

                    oDatasetTemp = Me.RigeneraDataset()
                    oData = oDataset.Tables(0).DefaultView
                    oData.RowFilter = filtro
                    totale = oData.Count - 1
                    If oData.Count > 0 Then
                        For i = 0 To totale
                            oDatasetTemp.Tables(0).ImportRow(oData.Item(i).Row)
                        Next
                    End If
                    oDataset = oDatasetTemp
                End If

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                While oDataview.Count > 0
                    Dim ComunitaID As Integer

                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()
            Catch ex As Exception
                oDataset = Nothing
            End Try
            iResponse = New List(Of CommunityPath)
            If Not (oDataset Is Nothing OrElse oDataset.Tables.Count = 0 OrElse oDataset.Tables(0).Rows.Count = 0) Then
                For Each oRow As DataRow In oDataset.Tables(0).Rows
                    iResponse.Add(New CommunityPath() With {.ID = oRow.Item("CMNT_ID"), .Path = oRow.Item("ALCM_Path")})
                Next
            End If
            'COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, iResponse, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza20minuti)
            'Else
            'iResponse = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of CommunityPath))
            'End If
            Return iResponse
        End Function
        Private Function FindDataset(ByVal oPersona As COL_Persona, ByVal ComunitaCorrenteID As Integer, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal ComunitaPadreId As Integer = -1, Optional ByVal Path As String = "", Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal oFiltroLettera As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal EsclusoPath As Boolean = False, Optional ByVal showNomePadre As Boolean = False) As DataSet
            Dim oDataset As New DataSet
            Dim filtro As String

            Try

                If TipoComunitaID >= 0 Then
                    filtro = "CMNT_TPCM_id=" & TipoComunitaID
                Else
                    filtro = "1=1 "
                End If


                If oFiltroRicerca = FiltroComunita.creataPrima Or oFiltroRicerca = FiltroComunita.creataDopo Or oFiltroRicerca = FiltroComunita.dataFineIscrizionePrima Or oFiltroRicerca = FiltroComunita.dataIscrizioneDopo Then
                    Dim stringaData As String = ""
                    Dim oData As DateTime
                    If valore <> "" Then

                        If IsDate(valore) Then
                            oData = CDate(valore)
                            If oFiltroRicerca = FiltroComunita.creataPrima Then
                                stringaData = " CMNT_dataCreazione <= #" & FormatDateTime(oData) & "# "
                            ElseIf oFiltroRicerca = FiltroComunita.creataDopo Then
                                stringaData = " CMNT_dataCreazione >= #" & FormatDateTime(oData) & "# "
                            ElseIf oFiltroRicerca = FiltroComunita.dataIscrizioneDopo Then
                                stringaData = " CMNT_dataInizioIscrizione >= #" & FormatDateTime(oData) & "# "
                                'stringaData = " Year(CMNT_dataInizioIscrizione) >= " & Year(oData) & " and Month(CMNT_dataInizioIscrizione)>=" & Month(oData) & " and Day(CMNT_dataInizioIscrizione) >= " & Day(oData)
                            ElseIf oFiltroRicerca = FiltroComunita.dataFineIscrizionePrima Then
                                stringaData = " CMNT_dataFineIscrizione >= #" & FormatDateTime(oData) & "# "
                                'stringaData = " Year(CMNT_dataFineIscrizione) <= " & Year(oData) & " and Month(CMNT_dataFineIscrizione)>=" & Month(oData) & " and Day(CMNT_dataFineIscrizione) >= " & Day(oData)
                            End If
                            If stringaData <> "" Then
                                If filtro <> "" Then
                                    filtro = filtro & " AND " & stringaData
                                Else
                                    filtro = stringaData
                                End If

                            End If
                        End If
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.contiene Then
                    If valore <> "" Then
                        filtro = filtro & " AND CMNT_nome like '%" & valore & "%' "
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.nome Then
                    If valore <> "" Then
                        filtro = filtro & " AND CMNT_nome like '" & valore & "%' "
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.IDresponsabile And valore <> "" And valore <> "-1" Then
                    filtro = filtro & " AND ALCM_ResponsabileID=" & valore & " "
                End If
                If oFiltroLettera = FiltroComunita.altro Then
                    filtro = filtro & " AND CMNT_nome  NOT like '[A-Z]%' "
                ElseIf oFiltroLettera = FiltroComunita.tutti Then
                    filtro = filtro
                Else
                    filtro = filtro & " AND (CMNT_nome like '" & oFiltroLettera.ToString & "%' or CMNT_nome like '" & oFiltroLettera.ToString.ToUpper & "%')"
                End If

                If FacoltaID > 0 Then
                    Dim oOrganizzazione As New COL_Organizzazione
                    Dim CMNT_ID As Integer
                    oOrganizzazione.Id = FacoltaID
                    Try
                        CMNT_ID = oOrganizzazione.RitornaComunitaOrganizzazione()
                        If filtro = "" Then
                            filtro = "ALCM_Path like '." & CMNT_ID & ".%'"
                        Else
                            filtro = filtro & " AND ALCM_Path like '." & CMNT_ID & ".%'"
                        End If
                    Catch ex As Exception

                    End Try
                End If
               


                If ComunitaPadreId > -1 Then
                    If filtro = "" Then
                        filtro = "ALCM_PadreVirtuale_ID=" & ComunitaPadreId
                    Else
                        filtro = filtro & " AND ALCM_PadreVirtuale_ID=" & ComunitaPadreId
                    End If
                End If
                If Path <> "" Then
                    If filtro = "" Then
                        filtro = "ALCM_Path like '" & Path & "%'"
                    Else
                        filtro = filtro & " AND ALCM_Path like '" & Path & "%'"
                    End If
                End If
                If EsclusoPath And Path <> "" Then
                    If filtro = "" Then
                        filtro = "ALCM_Path <> '" & Path & "'"
                    Else
                        filtro = filtro & " AND ALCM_Path <> '" & Path & "'"
                    End If
                End If
                Select Case FiltroStato
                    Case Main.FiltroStatoComunita.Archiviata
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Archiviata=true "
                        Else
                            filtro = "CMNT_Archiviata=true "
                        End If
                    Case Main.FiltroStatoComunita.Attiva
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Archiviata=false and CMNT_Bloccata=false "
                        Else
                            filtro = "CMNT_Archiviata=false and CMNT_Bloccata=false "
                        End If
                    Case Main.FiltroStatoComunita.Bloccata
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Bloccata=true "
                        Else
                            filtro = "CMNT_Bloccata=true "
                        End If
                End Select

                If filtro = "" Then
                    filtro = " RLPC_TPRL_id <> -2 AND RLPC_TPRL_id <> -3 "
                Else
                    filtro = filtro & " AND RLPC_TPRL_id <> -2 AND RLPC_TPRL_id <> -3 "
                End If

                Dim i, totale As Integer
                Dim ElencoComunitaID As String = ""
                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If filtro <> "" Then
                    Dim oDatasetTemp As New DataSet
                    Dim oData As DataView

                    oDatasetTemp = Me.RigeneraDataset()
                    oData = oDataset.Tables(0).DefaultView
                    oData.RowFilter = filtro
                    totale = oData.Count - 1
                    If oData.Count > 0 Then
                        For i = 0 To totale
                            oDatasetTemp.Tables(0).ImportRow(oData.Item(i).Row)
                        Next
                    End If
                    oDataset = oDatasetTemp
                End If

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                While oDataview.Count > 0
                    Dim ComunitaID As Integer

                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()



                totale = oDataset.Tables(0).Rows.Count - 1

                oDataset.Tables(0).Columns.Add(New DataColumn("Entra"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                oDataset.Tables(0).Columns.Add(New DataColumn("RLPC_UltimoCollegamentoStringa"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                oDataset.Tables(0).Columns.Add(New DataColumn("ORGN_Diretta"))

                'DISATTIVATO GENNAIO 2010
                'Dim oDataviewNews As New DataView
                'If totale >= 0 Then
                '    Dim oDatasetNews As New DataSet
                '    Try
                '        If ComunitaCorrenteID < 0 Then
                '            oDatasetNews = oPersona.HasNewsForComunita(False, 0, FiltroStato)
                '        Else
                '            oDatasetNews = oPersona.HasNewsForComunita(True, ComunitaCorrenteID, FiltroStato)
                '        End If
                '        oDataviewNews = oDatasetNews.Tables(0).DefaultView
                '    Catch ex As Exception
                '        oDatasetNews = New DataSet
                '        oDataviewNews = Nothing
                '    End Try
                'End If
                Dim img As String
                For i = 0 To totale
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)
                    ' mostro una diversa icona in base all'attivazione o all'abilitazione
                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If
                    If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                        oRow.Item("Periodo") = "&nbsp;"
                    Else
                        oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                    End If


                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                            oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                            oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("RLPC_UltimoCollegamento")) Then
                        If Not Equals(New Date, oRow.Item("RLPC_UltimoCollegamento")) Then
                            Dim oData As DateTime
                            oData = FormatDateTime(oRow.Item("RLPC_UltimoCollegamento"), DateFormat.GeneralDate)
                            oRow.Item("RLPC_UltimoCollegamento") = oData
                            oRow.Item("RLPC_UltimoCollegamentoStringa") = oData.ToString("dd/MM/yy HH:mm")
                        Else
                            oRow.Item("RLPC_UltimoCollegamentoStringa") = oResource.getValue("iscrizione")
                        End If
                    Else
                        oRow.Item("RLPC_UltimoCollegamentoStringa") = oResource.getValue("iscrizione")
                    End If

                    If IsDBNull(oRow.Item("TPCM_icona")) Then
                        oRow.Item("TPCM_icona") = ""
                    Else
                        img = oRow.Item("TPCM_icona")
                        img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If showNomePadre Then
                        If IsDBNull(oRow.Item("CMNT_NomePadre")) Then
                            oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                        Else
                            If oRow.Item("CMNT_NomePadre") = "" Then
                                oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                                oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                            Else
                                oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_nomePadre") & "</span>&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                                oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & "&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                            End If
                        End If
                    Else
                        oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                        oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                    End If

                    oRow.Item("HasNews") = False
                    ' COMMENTATO GENNAIO 2010
                    'Try
                    '    If IsNothing(oDataviewNews) Then
                    '        oRow.Item("HasNews") = False
                    '    Else
                    '        oDataviewNews.RowFilter = "CMNT_ID =" & oRow.Item("CMNT_ID")
                    '        If oDataviewNews.Count <= 0 Then
                    '            oRow.Item("HasNews") = False
                    '        Else
                    '            oRow.Item("HasNews") = True
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    oRow.Item("HasNews") = False
                    'End Try
                Next


            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function
        Public Function RicercaComunita(ByVal oPersona As COL_Persona, ByVal ComunitaCorrenteID As Integer, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal ComunitaPadreId As Integer = -1, Optional ByVal Path As String = "", Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal oFiltroLettera As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal EsclusoPath As Boolean = False, Optional ByVal showNomePadre As Boolean = False) As DataSet
            Dim oDataset As New DataSet
            Dim filtro As String

            Try

                If TipoComunitaID >= 0 Then
                    filtro = "CMNT_TPCM_id=" & TipoComunitaID
                Else
                    filtro = "1=1 "
                End If


                If oFiltroRicerca = FiltroComunita.creataPrima Or oFiltroRicerca = FiltroComunita.creataDopo Or oFiltroRicerca = FiltroComunita.dataFineIscrizionePrima Or oFiltroRicerca = FiltroComunita.dataIscrizioneDopo Then
                    Dim stringaData As String = ""
                    Dim oData As DateTime
                    If valore <> "" Then

                        If IsDate(valore) Then
                            oData = CDate(valore)
                            If oFiltroRicerca = FiltroComunita.creataPrima Then
                                stringaData = " CMNT_dataCreazione <= #" & FormatDateTime(oData) & "# "
                            ElseIf oFiltroRicerca = FiltroComunita.creataDopo Then
                                stringaData = " CMNT_dataCreazione >= #" & FormatDateTime(oData) & "# "
                            ElseIf oFiltroRicerca = FiltroComunita.dataIscrizioneDopo Then
                                stringaData = " CMNT_dataInizioIscrizione >= #" & FormatDateTime(oData) & "# "
                                'stringaData = " Year(CMNT_dataInizioIscrizione) >= " & Year(oData) & " and Month(CMNT_dataInizioIscrizione)>=" & Month(oData) & " and Day(CMNT_dataInizioIscrizione) >= " & Day(oData)
                            ElseIf oFiltroRicerca = FiltroComunita.dataFineIscrizionePrima Then
                                stringaData = " CMNT_dataFineIscrizione >= #" & FormatDateTime(oData) & "# "
                                'stringaData = " Year(CMNT_dataFineIscrizione) <= " & Year(oData) & " and Month(CMNT_dataFineIscrizione)>=" & Month(oData) & " and Day(CMNT_dataFineIscrizione) >= " & Day(oData)
                            End If
                            If stringaData <> "" Then
                                If filtro <> "" Then
                                    filtro = filtro & " AND " & stringaData
                                Else
                                    filtro = stringaData
                                End If

                            End If
                        End If
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.contiene Then
                    If valore <> "" Then
                        filtro = filtro & " AND CMNT_nome like '%" & valore & "%' "
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.nome Then
                    If valore <> "" Then
                        filtro = filtro & " AND CMNT_nome like '" & valore & "%' "
                    End If
                ElseIf oFiltroRicerca = FiltroComunita.IDresponsabile And valore <> "" And valore <> "-1" Then
                    filtro = filtro & " AND ALCM_ResponsabileID=" & valore & " "
                End If
                If oFiltroLettera = FiltroComunita.altro Then
                    filtro = filtro & " AND CMNT_nome  NOT like '[A-Z]%' "
                ElseIf oFiltroLettera = FiltroComunita.tutti Then
                    filtro = filtro
                Else
                    filtro = filtro & " AND (CMNT_nome like '" & oFiltroLettera.ToString & "%' or CMNT_nome like '" & oFiltroLettera.ToString.ToUpper & "%')"
                End If

                If FacoltaID > 0 Then
                    Dim oOrganizzazione As New COL_Organizzazione
                    Dim CMNT_ID As Integer
                    oOrganizzazione.Id = FacoltaID
                    Try
                        CMNT_ID = oOrganizzazione.RitornaComunitaOrganizzazione()
                        If filtro = "" Then
                            filtro = "ALCM_Path like '." & CMNT_ID & ".%'"
                        Else
                            filtro = filtro & " AND ALCM_Path like '." & CMNT_ID & ".%'"
                        End If
                    Catch ex As Exception

                    End Try
                End If
              


                If ComunitaPadreId > -1 Then
                    If filtro = "" Then
                        filtro = "ALCM_PadreVirtuale_ID=" & ComunitaPadreId
                    Else
                        filtro = filtro & " AND ALCM_PadreVirtuale_ID=" & ComunitaPadreId
                    End If
                End If
                If Path <> "" Then
                    If filtro = "" Then
                        filtro = "ALCM_Path like '" & Path & "%'"
                    Else
                        filtro = filtro & " AND ALCM_Path like '" & Path & "%'"
                    End If
                End If
                If EsclusoPath And Path <> "" Then
                    If filtro = "" Then
                        filtro = "ALCM_Path <> '" & Path & "'"
                    Else
                        filtro = filtro & " AND ALCM_Path <> '" & Path & "'"
                    End If
                End If
                Select Case FiltroStato
                    Case Main.FiltroStatoComunita.Archiviata
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Archiviata=true "
                        Else
                            filtro = "CMNT_Archiviata=true "
                        End If
                    Case Main.FiltroStatoComunita.Attiva
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Archiviata=false and CMNT_Bloccata=false "
                        Else
                            filtro = "CMNT_Archiviata=false and CMNT_Bloccata=false "
                        End If
                    Case Main.FiltroStatoComunita.Bloccata
                        If filtro <> "" Then
                            filtro = filtro & " AND CMNT_Bloccata=true "
                        Else
                            filtro = "CMNT_Bloccata=true "
                        End If
                End Select

                If filtro = "" Then
                    filtro = " RLPC_TPRL_id <> -2 AND RLPC_TPRL_id <> -3 "
                Else
                    filtro = filtro & " AND RLPC_TPRL_id <> -2 AND RLPC_TPRL_id <> -3 "
                End If

                Dim i, totale As Integer
                Dim ElencoComunitaID As String = ""
                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If filtro <> "" Then
                    Dim oDatasetTemp As New DataSet
                    Dim oData As DataView

                    oDatasetTemp = Me.RigeneraDataset()
                    oData = oDataset.Tables(0).DefaultView
                    oData.RowFilter = filtro
                    totale = oData.Count - 1
                    If oData.Count > 0 Then
                        For i = 0 To totale
                            oDatasetTemp.Tables(0).ImportRow(oData.Item(i).Row)
                        Next
                    End If
                    oDataset = oDatasetTemp
                End If

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                While oDataview.Count > 0
                    Dim ComunitaID As Integer

                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()



                totale = oDataset.Tables(0).Rows.Count - 1

                oDataset.Tables(0).Columns.Add(New DataColumn("Entra"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                oDataset.Tables(0).Columns.Add(New DataColumn("RLPC_UltimoCollegamentoStringa"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                oDataset.Tables(0).Columns.Add(New DataColumn("ORGN_Diretta"))

                'DISATTIVATO 2010
                'Dim oDataviewNews As New DataView
                'If totale >= 0 Then
                '    Dim oDatasetNews As New DataSet
                '    Try
                '        If ComunitaCorrenteID < 0 Then
                '            oDatasetNews = oPersona.HasNewsForComunita(False, 0, FiltroStato)
                '        Else
                '            oDatasetNews = oPersona.HasNewsForComunita(True, ComunitaCorrenteID, FiltroStato)
                '        End If
                '        oDataviewNews = oDatasetNews.Tables(0).DefaultView
                '    Catch ex As Exception
                '        oDatasetNews = New DataSet
                '        oDataviewNews = Nothing
                '    End Try
                'End If
                Dim img As String
                For i = 0 To totale
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)
                    ' mostro una diversa icona in base all'attivazione o all'abilitazione
                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If
                    If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                        oRow.Item("Periodo") = "&nbsp;"
                    Else
                        oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                    End If


                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                            oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                            oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("RLPC_UltimoCollegamento")) Then
                        If Not Equals(New Date, oRow.Item("RLPC_UltimoCollegamento")) Then
                            Dim oData As DateTime
                            oData = FormatDateTime(oRow.Item("RLPC_UltimoCollegamento"), DateFormat.GeneralDate)
                            oRow.Item("RLPC_UltimoCollegamento") = oData
                            oRow.Item("RLPC_UltimoCollegamentoStringa") = oData.ToString("dd/MM/yy HH:mm")
                        Else
                            oRow.Item("RLPC_UltimoCollegamentoStringa") = oResource.getValue("iscrizione")
                        End If
                    Else
                        oRow.Item("RLPC_UltimoCollegamentoStringa") = oResource.getValue("iscrizione")
                    End If

                    If IsDBNull(oRow.Item("TPCM_icona")) Then
                        oRow.Item("TPCM_icona") = ""
                    Else
                        img = oRow.Item("TPCM_icona")
                        img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If showNomePadre Then
                        If IsDBNull(oRow.Item("CMNT_NomePadre")) Then
                            oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                        Else
                            If oRow.Item("CMNT_NomePadre") = "" Then
                                oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                                oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                            Else
                                oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_nomePadre") & "</span>&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                                oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & "&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                            End If
                        End If
                    Else
                        oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                        oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                    End If


                    oRow.Item("HasNews") = False
                    ' COMMENTATO GENNAIO 2010
                    'Try
                    '    If IsNothing(oDataviewNews) Then
                    '        oRow.Item("HasNews") = False
                    '    Else
                    '        oDataviewNews.RowFilter = "CMNT_ID =" & oRow.Item("CMNT_ID")
                    '        If oDataviewNews.Count <= 0 Then
                    '            oRow.Item("HasNews") = False
                    '        Else
                    '            oRow.Item("HasNews") = True
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    oRow.Item("HasNews") = False
                    'End Try
                Next


            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function
        Public Function RicercaComunitaAlbero(ByVal oPersona As COL_Persona, ByVal ComunitaCorrenteID As Integer, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal ComunitaPadreId As Integer = -1, Optional ByVal Path As String = "", Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal EsclusoPath As Boolean = False, Optional ByVal oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero) As DataSet
            Dim oDataset As New DataSet
            Dim totale, i, Livello, ComunitaID As Integer

            Try
                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)

                If Not oDataset.Tables(0).Columns.Contains("CMNT_Esteso") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_EstesoNoSpan") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Proprieta") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Alternative") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Iscritti") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCreazioneText") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCreazioneText"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCessazioneText") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCessazioneText"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("HasNews"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("NoDelete") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("NoDelete"))
                End If

                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","
                Dim img As String = ""
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True
                totale = oDataset.Tables(0).Rows.Count

                If FacoltaID > 0 Then
                    Dim oOrganizzazione As New COL_Organizzazione
                    Dim oComunitaFacoltaID As Integer

                    Try
                        oOrganizzazione.Id = FacoltaID
                        oComunitaFacoltaID = oOrganizzazione.RitornaComunitaOrganizzazione()
                        oDataview.RowFilter = "ALCM_Path not like '." & oComunitaFacoltaID & ".%'"

                        While oDataview.Count > 0
                            oDataview.Item(0).Delete()
                        End While
                    Catch ex As Exception

                    End Try
                    oDataview.RowFilter = ""
                End If

                oDataview.Sort = "ALCM_Livello DESC"
                Try
                    Livello = oDataview.Item(0).Item("ALCM_Livello")
                    oDataview.RowFilter = "NoDelete<>0 and ALCM_Livello=" & Livello
                Catch ex As Exception
                    Livello = -1
                End Try

                oDataview.Sort = "ALCM_Path"
                While Livello >= 0
                    While oDataview.Count > 0
                        Dim ComunitaTipoID As Integer
                        Dim ComunitaPercorso, PercorsoPadre As String
                        Dim isDelete As Boolean = False
                        Dim isChiusa As Boolean = False

                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
                        ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
                        PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
                        isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")

                        If IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
                            oDataview.Delete(0)
                            isDelete = True
                        Else
                            If oDataview.Item(0).Row.Item("RLPC_TPRL_id") < 0 And Not (oDataview.Item(0).Row.Item("CMNT_AccessoLibero") = 1 And oDataview.Item(0).Row.Item("RLPC_TPRL_id") = Main.TipoRuoloStandard.AccessoNonAutenticato) Then
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If

                        If TipoComunitaID <> -1 And Not isDelete Then
                            If TipoComunitaID <> ComunitaTipoID Then
                                oDataview.Delete(0)
                                isDelete = True
                            Else
                               
                            End If
                        End If
                        If Not isDelete Then
                            Select Case oFiltroRicerca
                                Case Main.FiltroComunita.contiene
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) < 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.IDresponsabile
                                    If IsNumeric(valore) And valore <> -1 Then
                                        If oDataview.Item(0).Row.Item("ALCM_ResponsabileID") <> valore Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.nome
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) <> 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.creataDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.creataPrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataFineIscrizionePrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataInizioIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataIscrizioneDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataFineIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                            End Select
                        End If

                        If Not isDelete Then
                            Select Case FiltroStato
                                Case Main.FiltroStatoComunita.Archiviata
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") <> True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Attiva
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") = True Or oDataview.Item(0).Row.Item("CMNT_Bloccata") = True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Bloccata
                                    If oDataview.Item(0).Row.Item("CMNT_Bloccata") = False Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                            End Select
                        End If

                        If Not isDelete Then
                            Dim Filtro As String
                            Dim j As Integer

                            Filtro = oDataview.RowFilter
                            oDataview.RowFilter = "NoDelete <> 0 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
                            For j = oDataview.Count - 1 To 0 Step -1
                                oDataview.Item(j).Item("NoDelete") = 0
                            Next
                            oDataview.RowFilter = Filtro
                        End If
                    End While
                    Livello -= 1
                    oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND NoDelete <>0"
                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()

                If oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo Then
                    Me.GeneraNodiOrganizzativi(oDataset, oResource)
                End If

                totale = oDataset.Tables(0).Rows.Count
                'DISATTIVATO 2010
                'Dim oDatasetNews As DataSet
                'Dim oDataviewNews As New DataView
                'If totale > 0 Then
                '    Try
                '        oDatasetNews = oPersona.HasNewsForComunita(False, 0, FiltroStato)
                '        oDataviewNews = oDatasetNews.Tables(0).DefaultView
                '    Catch ex As Exception
                '        oDatasetNews = New DataSet
                '        oDataviewNews = Nothing
                '    End Try
                'End If

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    ComunitaID = oRow.Item("CMNT_ID")
                    If oRow.Item("CMNT_ID") > 0 Then
                        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                            oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            If oRow.Item("CMNT_Responsabile") = "" Then
                                oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                                oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                            Else
                                oRow.Item("AnagraficaResponsabile") = "&nbsp;(" & oRow.Item("CMNT_Responsabile") & ") &nbsp;"
                            End If
                        End If
                    Else
                        oRow.Item("AnagraficaResponsabile") = ""
                    End If

                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                    If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                        img = oRow.Item("TPCM_icona")
                        'img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If
                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                            oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                            oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If
                    Try
                        Dim numIscritti, maxIscritti As Integer
                        maxIscritti = oRow.Item("CMNT_MaxIscritti")
                        numIscritti = oRow.Item("CMNT_Iscritti")
                        Try
                            oRow.Item("Iscritti") = numIscritti
                        Catch ex As Exception
                            oRow.Item("Iscritti") = 0
                            numIscritti = 0
                        End Try

                        If (maxIscritti <= 0) Then
                            oRow.Item("CMNT_Iscritti") = 0
                        Else
                            If numIscritti > maxIscritti Then
                                oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                            ElseIf numIscritti = maxIscritti Then
                                oRow.Item("CMNT_Iscritti") = -1
                                oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                            Else
                                oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                            End If
                        End If
                    Catch ex As Exception

                    End Try


                    oRow.Item("HasNews") = False
                    ' COMMENTATO GENNAIO 2010
                    'Try
                    '    If IsNothing(oDataviewNews) Then
                    '        oRow.Item("HasNews") = False
                    '    Else
                    '        oDataviewNews.RowFilter = "CMNT_ID =" & oRow.Item("CMNT_ID")
                    '        If oDataviewNews.Count <= 0 Then
                    '            oRow.Item("HasNews") = False
                    '        Else
                    '            oRow.Item("HasNews") = True
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    oRow.Item("HasNews") = False
                    'End Try
                    Try
                        If oRow.Item("ALCM_Path") = oRow.Item("ALCM_RealPath") Then
                            oRow.Item("ALCM_RealPath") = ""
                        End If
                    Catch ex As Exception

                    End Try
                Next
                oDataset.AcceptChanges()
            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function
        Public Function RicercaComunitaAlberoByServizio(ByVal ServizioCodice As String, ByVal oPersona As COL_Persona, ByVal ComunitaCorrenteID As Integer, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal ComunitaPadreId As Integer = -1, Optional ByVal Path As String = "", Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal EsclusoPath As Boolean = False, Optional ByVal oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero, Optional ByVal DatiRistretti As Boolean = True, Optional ByVal LivelloMassimo As Integer = -1) As DataSet
            Dim oDataset As New DataSet
            Dim oDatasetPermessi As New DataSet

            Try
                Dim oDataviewPermessi As DataView

                Try
                    oDatasetPermessi = COL_Comunita.ElencaListaSemplice_ForServizio(oPersona.Lingua.ID, ServizioCodice, oPersona.ID, FiltroStato)
                Catch ex As Exception

                End Try


                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)

                If Not DatiRistretti Then
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Esteso") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_EstesoNoSpan") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("Iscritti") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
                    End If

                    If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCreazioneText") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCreazioneText"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCessazioneText") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCessazioneText"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("HasNews"))
                    End If
                End If
                If Not oDataset.Tables(0).Columns.Contains("LKSC_Permessi") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("LKSC_Permessi"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Proprieta") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Alternative") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("NoDelete") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("NoDelete"))
                End If
                Dim totale, i, Livello As Integer
                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","
                Dim img As String = ""
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True
                totale = oDataset.Tables(0).Rows.Count

                If FacoltaID > 0 Then
                    Dim oOrganizzazione As New COL_Organizzazione
                    Dim oComunitaFacoltaID As Integer

                    Try
                        oOrganizzazione.Id = FacoltaID
                        oComunitaFacoltaID = oOrganizzazione.RitornaComunitaOrganizzazione()
                        oDataview.RowFilter = "ALCM_Path not like '." & oComunitaFacoltaID & ".%'"

                    Catch ex As Exception

                    End Try
                End If

                If LivelloMassimo >= 0 Then
                    If oDataview.RowFilter = "" Then
                        oDataview.RowFilter = "ALCM_Livello>" & LivelloMassimo & " "
                    Else
                        oDataview.RowFilter &= " or ALCM_Livello>" & LivelloMassimo & " "
                    End If
                End If
                If oDataview.RowFilter <> "" Then
                    Try
                        While oDataview.Count > 0
                            oDataview.Item(0).Delete()
                        End While
                    Catch ex As Exception

                    End Try
                    oDataview.RowFilter = ""
                End If


                Me.AggiornaForServizio(oPersona.ID, oDataview, oDatasetPermessi, ServizioCodice, LivelloMassimo)

                Try
                    oDataviewPermessi = oDatasetPermessi.Tables(0).DefaultView
                Catch ex As Exception
                    oDataviewPermessi = Nothing
                End Try

                oDataview.RowFilter = ""
                oDataview.Sort = "ALCM_Livello DESC"
                Try
                    Livello = oDataview.Item(0).Item("ALCM_Livello")
                    oDataview.RowFilter = "NoDelete<>0 and ALCM_Livello=" & Livello
                Catch ex As Exception
                    Livello = -1
                End Try

                oDataview.Sort = "ALCM_Path"
                While Livello >= 0
                    While oDataview.Count > 0
                        Dim oRow As DataRow
                        Dim ComunitaID, ComunitaTipoID As Integer
                        Dim ComunitaPercorso, PercorsoPadre As String
                        Dim isDelete As Boolean = False
                        Dim isChiusa As Boolean = False

                        oRow = oDataview.Item(0).Row
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
                        ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
                        PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
                        isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")

                        If IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
                            oDataview.Delete(0)
                            isDelete = True
                        Else
                            If oDataview.Item(0).Row.Item("RLPC_TPRL_id") < 0 And Not (oRow.Item("CMNT_AccessoLibero") = 1 And oDataview.Item(0).Row.Item("RLPC_TPRL_id") = Main.TipoRuoloStandard.AccessoNonAutenticato) Then
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If

                        If TipoComunitaID <> -1 And Not isDelete Then
                            If TipoComunitaID <> ComunitaTipoID Then
                                oDataview.Delete(0)
                                isDelete = True
                            Else
                               
                            End If
                        End If
                        If Not isDelete Then
                            Select Case oFiltroRicerca
                                Case Main.FiltroComunita.contiene
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) < 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.IDresponsabile
                                    If IsNumeric(valore) And valore <> -1 Then
                                        If oDataview.Item(0).Row.Item("ALCM_ResponsabileID") <> valore Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.nome
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) <> 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.creataDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.creataPrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataFineIscrizionePrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataInizioIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataIscrizioneDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataFineIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                            End Select
                        End If

                        If Not isDelete Then
                            Select Case FiltroStato
                                Case Main.FiltroStatoComunita.Archiviata
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") <> True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Attiva
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") = True Or oDataview.Item(0).Row.Item("CMNT_Bloccata") = True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Bloccata
                                    If oDataview.Item(0).Row.Item("CMNT_Bloccata") = False Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                            End Select
                        End If

                        If Not isDelete Then
                            oDataviewPermessi.RowFilter = "CMNT_ID=" & oDataview.Item(0).Row.Item("CMNT_ID")
                            If oDataviewPermessi.Count > 0 Then
                                If ServizioCodice = Services_File.Codex Then
                                    Dim oServizio As New Services_File
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_Mail.Codex Then
                                    Dim oServizio As New Services_Mail
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.SendMail) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_PostIt.Codex Then
                                    Dim oServizio As New Services_PostIt
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.GestioneServizio Or oServizio.ViewPostIt Or oServizio.CreatePostIt) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_RaccoltaLink.Codex Then
                                    Dim oServizio As New Services_RaccoltaLink
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.ExportLink Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_AmministraComunita.Codex Then
                                    Dim oServizio As New Services_AmministraComunita
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.CreateComunity Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                End If
                            Else
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If
                        If Not isDelete Then
                            Dim Filtro As String
                            Dim j As Integer

                            Filtro = oDataview.RowFilter
                            oDataview.RowFilter = "NoDelete <> 0 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
                            For j = oDataview.Count - 1 To 0 Step -1
                                oDataview.Item(j).Item("NoDelete") = 0
                            Next
                            oDataview.RowFilter = Filtro
                        End If
                    End While
                    Livello -= 1
                    oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND NoDelete <>0"
                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()

                If oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo Then
                    Me.GeneraNodiOrganizzativi(oDataset, oResource)
                End If

                totale = oDataset.Tables(0).Rows.Count
                'DISATTIVATO 2010
                'Dim oDatasetNews As DataSet
                'Dim oDataviewNews As New DataView
                'If totale > 0 Then
                '    Try
                '        oDatasetNews = oPersona.HasNewsForComunita(False, 0, FiltroStato)
                '        oDataviewNews = oDatasetNews.Tables(0).DefaultView
                '    Catch ex As Exception
                '        oDatasetNews = New DataSet
                '        oDataviewNews = Nothing
                '    End Try
                'End If

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("CMNT_ID") > 0 Then
                        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                            oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            If oRow.Item("CMNT_Responsabile") = "" Then
                                oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                                oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                            Else
                                oRow.Item("AnagraficaResponsabile") = "&nbsp;(" & oRow.Item("CMNT_Responsabile") & ") &nbsp;"
                            End If
                        End If
                    Else
                        oRow.Item("AnagraficaResponsabile") = ""
                    End If

                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                    If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                        img = oRow.Item("TPCM_icona")
                        'img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If Not DatiRistretti Then
                        If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                            If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                                oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                            End If
                        End If
                        If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                            If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                                oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                            End If
                        End If

                        Try
                            Dim numIscritti, maxIscritti As Integer
                            maxIscritti = oRow.Item("CMNT_MaxIscritti")
                            numIscritti = oRow.Item("CMNT_Iscritti")
                            Try
                                oRow.Item("Iscritti") = numIscritti
                            Catch ex As Exception
                                oRow.Item("Iscritti") = 0
                                numIscritti = 0
                            End Try

                            If (maxIscritti <= 0) Then
                                oRow.Item("CMNT_Iscritti") = 0
                            Else
                                If numIscritti > maxIscritti Then
                                    oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                    oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                                ElseIf numIscritti = maxIscritti Then
                                    oRow.Item("CMNT_Iscritti") = -1
                                    oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                                Else
                                    oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                    oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                                End If
                            End If
                        Catch ex As Exception

                        End Try

                        oRow.Item("HasNews") = False
                        ' COMMENTATO GENNAIO 2010
                        'Try
                        '    If IsNothing(oDataviewNews) Then
                        '        oRow.Item("HasNews") = False
                        '    Else
                        '        oDataviewNews.RowFilter = "CMNT_ID =" & oRow.Item("CMNT_ID")
                        '        If oDataviewNews.Count <= 0 Then
                        '            oRow.Item("HasNews") = False
                        '        Else
                        '            oRow.Item("HasNews") = True
                        '        End If
                        '    End If
                        'Catch ex As Exception
                        '    oRow.Item("HasNews") = False
                        'End Try
                    End If

                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If

                    Try
                        If oRow.Item("ALCM_Path") = oRow.Item("ALCM_RealPath") Then
                            oRow.Item("ALCM_RealPath") = ""
                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        oDataviewPermessi.RowFilter = "CMNT_ID=" & oRow.Item("CMNT_ID")
                        If oDataviewPermessi.Count > 0 Then
                            oRow.Item("LKSC_Permessi") = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                        Else
                            oRow.Item("LKSC_Permessi") = "00000000000000000000000000000000"
                        End If
                    Catch ex As Exception

                    End Try

                Next
                oDataset.AcceptChanges()
            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function
        Public Function RicercaComunitaAlberoRidottoByServizio(ByVal ServizioCodice As String, ByVal oPersona As COL_Persona, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal LivelloMassimo As Integer = -1, Optional ByVal oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero, Optional ByVal odatasetEscluse As DataSet = Nothing) As DataSet
            Dim oDataset As New DataSet
            Dim oDatasetPermessi As New DataSet

            Try
                Dim oDataviewPermessi As DataView

                Try
                    oDatasetPermessi = COL_Comunita.ElencaListaSemplice_ForServizio(oPersona.Lingua.ID, ServizioCodice, oPersona.ID, FiltroStato)
                Catch ex As Exception

                End Try

                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If Not oDataset.Tables(0).Columns.Contains("LKSC_Permessi") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("LKSC_Permessi"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Proprieta") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Alternative") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("NoDelete") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("NoDelete"))
                End If
                Dim totale, i, Livello As Integer
                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","
                Dim img As String = ""
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True
                totale = oDataset.Tables(0).Rows.Count

                If IsNothing(odatasetEscluse) = False Then
                    Dim Filtro As String = ""
                    Try
                        totale = odatasetEscluse.Tables(0).Rows.Count - 1
                        For i = 0 To totale
                            Dim oRow As DataRow
                            oRow = odatasetEscluse.Tables(0).Rows(i)
                            If i = 0 Then
                                Filtro = "ALCM_Path like '%." & oRow.Item("CMNT_ID") & ".%'"
                            Else
                                Filtro &= " OR ALCM_Path like '%." & oRow.Item("CMNT_ID") & ".%'"
                            End If
                        Next
                    Catch ex As Exception

                    End Try

                    If Filtro <> "" Then
                        oDataview.RowFilter = Filtro
                        While oDataview.Count > 0
                            oDataview.Item(0).Delete()
                        End While
                        oDataview.RowFilter = ""
                    End If

                End If
                If FacoltaID > 0 Then
                    Dim oOrganizzazione As New COL_Organizzazione
                    Dim oComunitaFacoltaID As Integer

                    Try
                        oOrganizzazione.Id = FacoltaID
                        oComunitaFacoltaID = oOrganizzazione.RitornaComunitaOrganizzazione()
                        oDataview.RowFilter = "ALCM_Path not like '." & oComunitaFacoltaID & ".%'"

                    Catch ex As Exception

                    End Try
                End If

                If LivelloMassimo >= 0 Then
                    If oDataview.RowFilter = "" Then
                        oDataview.RowFilter = "ALCM_Livello>" & LivelloMassimo & " "
                    Else
                        oDataview.RowFilter &= " or ALCM_Livello>" & LivelloMassimo & " "
                    End If
                End If
                If oDataview.RowFilter <> "" Then
                    Try
                        While oDataview.Count > 0
                            oDataview.Item(0).Delete()
                        End While
                    Catch ex As Exception

                    End Try
                    oDataview.RowFilter = ""
                End If


                Me.AggiornaForServizio(oPersona.ID, oDataview, oDatasetPermessi, ServizioCodice, LivelloMassimo)

                Try
                    oDataviewPermessi = oDatasetPermessi.Tables(0).DefaultView
                Catch ex As Exception
                    oDataviewPermessi = Nothing
                End Try

                oDataview.RowFilter = ""
                oDataview.Sort = "ALCM_Livello DESC"
                Try
                    Livello = oDataview.Item(0).Item("ALCM_Livello")
                    oDataview.RowFilter = "NoDelete<>0 and ALCM_Livello=" & Livello
                Catch ex As Exception
                    Livello = -1
                End Try

                oDataview.Sort = "ALCM_Path"
                While Livello >= 0
                    While oDataview.Count > 0
                        Dim ComunitaID, ComunitaTipoID As Integer
                        Dim ComunitaPercorso, PercorsoPadre As String
                        Dim isDelete As Boolean = False
                        Dim isChiusa As Boolean = False

                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
                        ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
                        PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
                        isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")

                        If IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
                            oDataview.Delete(0)
                            isDelete = True
                        Else
                            If oDataview.Item(0).Row.Item("RLPC_TPRL_id") < 0 And Not (oDataview.Item(0).Row.Item("CMNT_AccessoLibero") = 1 And oDataview.Item(0).Row.Item("RLPC_TPRL_id") = Main.TipoRuoloStandard.AccessoNonAutenticato) Then
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If

                        If TipoComunitaID <> -1 And Not isDelete Then
                            If TipoComunitaID <> ComunitaTipoID Then
                                oDataview.Delete(0)
                                isDelete = True
                            Else
                               
                            End If
                        End If
                        If Not isDelete Then
                            Select Case oFiltroRicerca
                                Case Main.FiltroComunita.contiene
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) < 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.IDresponsabile
                                    If IsNumeric(valore) And valore <> -1 Then
                                        If oDataview.Item(0).Row.Item("ALCM_ResponsabileID") <> valore Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.nome
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) <> 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.creataDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.creataPrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataFineIscrizionePrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataInizioIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataIscrizioneDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataFineIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                            End Select
                        End If

                        If Not isDelete Then
                            Select Case FiltroStato
                                Case Main.FiltroStatoComunita.Archiviata
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") <> True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Attiva
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") = True Or oDataview.Item(0).Row.Item("CMNT_Bloccata") = True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Bloccata
                                    If oDataview.Item(0).Row.Item("CMNT_Bloccata") = False Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                            End Select
                        End If

                        If Not isDelete Then
                            oDataviewPermessi.RowFilter = "CMNT_ID=" & oDataview.Item(0).Row.Item("CMNT_ID")
                            If oDataviewPermessi.Count > 0 Then
                                If ServizioCodice = Services_File.Codex Then
                                    Dim oServizio As New Services_File
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_Mail.Codex Then
                                    Dim oServizio As New Services_Mail
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.SendMail) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_PostIt.Codex Then
                                    Dim oServizio As New Services_PostIt
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.GestioneServizio Or oServizio.ViewPostIt Or oServizio.CreatePostIt) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_RaccoltaLink.Codex Then
                                    Dim oServizio As New Services_RaccoltaLink
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.ExportLink Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                ElseIf ServizioCodice = Services_AmministraComunita.Codex Then
                                    Dim oServizio As New Services_AmministraComunita
                                    oServizio.PermessiAssociati = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                                    If Not (oServizio.Admin Or oServizio.CreateComunity Or oServizio.Moderate) Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                End If
                            Else
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If
                        If Not isDelete Then
                            Dim Filtro As String
                            Dim j As Integer

                            Filtro = oDataview.RowFilter
                            oDataview.RowFilter = "NoDelete <> 0 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
                            For j = oDataview.Count - 1 To 0 Step -1
                                oDataview.Item(j).Item("NoDelete") = 0
                            Next
                            oDataview.RowFilter = Filtro
                        End If
                    End While
                    Livello -= 1
                    oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND NoDelete <>0"
                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()

                If oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo Then
                    Me.GeneraNodiOrganizzativi(oDataset, oResource)
                End If

                totale = oDataset.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("CMNT_ID") > 0 Then
                        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                            oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            If oRow.Item("CMNT_Responsabile") = "" Then
                                oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                                oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                            Else
                                oRow.Item("AnagraficaResponsabile") = "&nbsp;(" & oRow.Item("CMNT_Responsabile") & ") &nbsp;"
                            End If
                        End If
                    Else
                        oRow.Item("AnagraficaResponsabile") = ""
                    End If

                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                    If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                        img = oRow.Item("TPCM_icona")
                        'img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If

                    Try
                        If oRow.Item("ALCM_Path") = oRow.Item("ALCM_RealPath") Then
                            oRow.Item("ALCM_RealPath") = ""
                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        oDataviewPermessi.RowFilter = "CMNT_ID=" & oRow.Item("CMNT_ID")
                        If oDataviewPermessi.Count > 0 Then
                            oRow.Item("LKSC_Permessi") = oDataviewPermessi.Item(0).Item("LKSC_Permessi")
                        Else
                            oRow.Item("LKSC_Permessi") = "00000000000000000000000000000000"
                        End If
                    Catch ex As Exception

                    End Try

                Next
                oDataset.AcceptChanges()
            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function

        Public Function AlberoGeneraleForFiltri(ByVal LinguaID As Integer, ByVal ComunitaCorrenteID As Integer, ByVal oResource As ResourceManager, ByVal ImageBaseDir As String, ByVal TipoComunitaID As Integer, Optional ByVal FacoltaID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal TipoCorsoDilaureaID As Integer = -1, Optional ByVal ComunitaPadreId As Integer = -1, Optional ByVal Path As String = "", Optional ByVal oFiltroRicerca As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal EsclusoPath As Boolean = False, Optional ByVal oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero, Optional ByVal DatiRistretti As Boolean = True) As DataSet
            Dim oDataset As New DataSet

            Try
                oDataset = COL_Comunita.RicercaComunitaAlberoForManagement(LinguaID, FacoltaID, Path, ComunitaCorrenteID, , oFiltroRicerca, , valore, TipoComunitaID, TipoCorsoDilaureaID, , Anno, PeriodoID, FiltroStato, Main.FiltroRicercaComunitaByIscrizione.forAdmin, False, False)

                If Not DatiRistretti Then
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Esteso") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_EstesoNoSpan") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("Iscritti") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
                    End If

                    If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCreazioneText") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCreazioneText"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCessazioneText") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCessazioneText"))
                    End If
                    If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
                        oDataset.Tables(0).Columns.Add(New DataColumn("HasNews"))
                    End If
                End If
                If Not oDataset.Tables(0).Columns.Contains("LKSC_Permessi") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("LKSC_Permessi"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Proprieta") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Alternative") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
                End If

                If Not oDataset.Tables(0).Columns.Contains("NoDelete") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("NoDelete"))
                End If
                Dim totale, i, Livello As Integer
                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","
                Dim img As String = ""
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True
                totale = oDataset.Tables(0).Rows.Count


                oDataview.Sort = "ALCM_Livello DESC"
                Try
                    Livello = oDataview.Item(0).Item("ALCM_Livello")
                    oDataview.RowFilter = "(NoDelete=0 or NoDelete= null)  and ALCM_Livello=" & Livello
                Catch ex As Exception
                    Livello = -1
                End Try

                oDataview.Sort = "ALCM_Path"
                While Livello >= 0
                    While oDataview.Count > 0
                        Dim ComunitaID, ComunitaTipoID As Integer
                        Dim ComunitaPercorso, PercorsoPadre As String
                        Dim isDelete As Boolean = False
                        Dim isChiusa As Boolean = False

                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
                        ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
                        PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
                        isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")

                        If TipoComunitaID <> -1 And Not isDelete Then
                            If TipoComunitaID <> ComunitaTipoID Then
                                oDataview.Delete(0)
                                isDelete = True
                            Else
                                
                            End If
                        End If
                        If Not isDelete Then
                            Select Case oFiltroRicerca
                                Case Main.FiltroComunita.contiene
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) < 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.IDresponsabile
                                    If IsNumeric(valore) And valore <> -1 Then
                                        If oDataview.Item(0).Row.Item("ALCM_ResponsabileID") <> valore Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.nome
                                    If valore <> "" Then
                                        If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) <> 1 Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    End If
                                Case Main.FiltroComunita.creataDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.creataPrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataFineIscrizionePrima
                                    Try
                                        If valore > oDataview.Item(0).Row.Item("CMNT_dataInizioIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Case Main.FiltroComunita.dataIscrizioneDopo
                                    Try
                                        If valore < oDataview.Item(0).Row.Item("CMNT_dataFineIscrizione") Then
                                            oDataview.Delete(0)
                                            isDelete = True
                                        End If
                                    Catch ex As Exception

                                    End Try
                            End Select
                        End If

                        If Not isDelete Then
                            Select Case FiltroStato
                                Case Main.FiltroStatoComunita.Archiviata
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") <> True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Attiva
                                    If oDataview.Item(0).Row.Item("CMNT_Archiviata") = True Or oDataview.Item(0).Row.Item("CMNT_Bloccata") = True Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                                Case Main.FiltroStatoComunita.Bloccata
                                    If oDataview.Item(0).Row.Item("CMNT_Bloccata") = False Then
                                        oDataview.Delete(0)
                                        isDelete = True
                                    End If
                            End Select
                        End If
                        If Not isDelete Then
                            Dim Filtro As String
                            Dim j As Integer

                            Filtro = oDataview.RowFilter
                            oDataview.RowFilter = "(NoDelete=0 or NoDelete= null)  and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
                            For j = oDataview.Count - 1 To 0 Step -1
                                oDataview.Item(j).Item("NoDelete") = 1
                            Next
                            oDataview.RowFilter = Filtro
                        End If
                    End While
                    Livello -= 1
                    oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND (NoDelete=0 or NoDelete= null) "
                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()

                If oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo Then
                    Me.GeneraNodiOrganizzativi(oDataset, oResource)
                End If

                totale = oDataset.Tables(0).Rows.Count

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("CMNT_ID") > 0 Then
                        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                            oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            If oRow.Item("CMNT_Responsabile") = "" Then
                                oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                                oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                            Else
                                oRow.Item("AnagraficaResponsabile") = "&nbsp;(" & oRow.Item("CMNT_Responsabile") & ") &nbsp;"
                            End If
                        End If
                    Else
                        oRow.Item("AnagraficaResponsabile") = ""
                    End If

                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                    If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                        img = oRow.Item("TPCM_icona")
                        'img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If Not DatiRistretti Then
                        If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                            If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                                oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                            End If
                        End If
                        If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                            If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                                oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                            End If
                        End If

                        Try
                            Dim numIscritti, maxIscritti As Integer
                            maxIscritti = oRow.Item("CMNT_MaxIscritti")
                            numIscritti = oRow.Item("CMNT_Iscritti")
                            Try
                                oRow.Item("Iscritti") = numIscritti
                            Catch ex As Exception
                                oRow.Item("Iscritti") = 0
                                numIscritti = 0
                            End Try

                            If (maxIscritti <= 0) Then
                                oRow.Item("CMNT_Iscritti") = 0
                            Else
                                If numIscritti > maxIscritti Then
                                    oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                    oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                                    oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                                ElseIf numIscritti = maxIscritti Then
                                    oRow.Item("CMNT_Iscritti") = -1
                                    oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                                Else
                                    oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                    oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                        oRow.Item("HasNews") = False
                    End If

                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If

                    Try
                        If oRow.Item("ALCM_Path") = oRow.Item("ALCM_RealPath") Then
                            oRow.Item("ALCM_RealPath") = ""
                        End If
                    Catch ex As Exception

                    End Try
                Next
                oDataset.AcceptChanges()
            Catch ex As Exception
                oDataset = Me.RigeneraDataset()
            End Try
            Return oDataset
        End Function

        Private Sub AggiornaForServizio(ByVal PersonaID As Integer, ByRef oDataview As DataView, ByRef oDatasetPermessi As DataSet, ByVal ServizioCodice As String, ByVal LivelloMassimo As Integer)
            Dim PercorsoTemporaneo, PercorsiEsclusi, Permessi As String
            Dim i, totale As Integer

            'Aggiunto x blocco!
            Dim j As Integer = 0

            Try
                oDataview.RowFilter = "RLPC_TPRL_id=" & Main.TipoRuoloStandard.AdminComunità
                PercorsiEsclusi = ""
                While oDataview.Count > 0

                    'Aggiunto x blocco!
                    Dim k As Integer = 0

                    'Aggiunto x blocco!
                    j += 1
                    PercorsoTemporaneo = oDataview.Item(0).Item("ALCM_PATH")
                    oDataview.RowFilter = "(RLPC_TPRL_id< 0 or RLPC_TPRL_ID = null) and ALCM_Path like '" & PercorsoTemporaneo & "%'"

                    totale = oDataview.Count
                    While oDataview.Count > 0

                        k += 1

                        '   Dim oServizio As COL_BusinessLogic_v2.CL_permessi.COL_Servizio
                        Permessi = COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, oDataview.Item(i).Item("CMNT_ID"), ServizioCodice)


                        Dim oRowPermessi As DataRow
                        oRowPermessi = oDatasetPermessi.Tables(0).NewRow()
                        oRowPermessi.Item("CMNT_ID") = oDataview.Item(i).Item("CMNT_ID")
                        oRowPermessi.Item("LKSC_Permessi") = Permessi

                        oDataview.Item(i).Item("RLPC_TPRL_ID") = Main.TipoRuoloStandard.AdminComunità
                        oDataview.Item(i).Item("RLPC_Attivato") = True
                        oDataview.Item(i).Item("RLPC_Abilitato") = True

                        'Aggiunto x blocco!
                        If (k >= _MaxIteration) Then
                            Throw New IndexOutOfRangeException("100% thread lock")
                        End If
                    End While

                    If InStr(PercorsiEsclusi, "ALCM_Path not like '" & PercorsoTemporaneo & "%'") < 1 Then
                        PercorsiEsclusi &= " and ALCM_Path not like '" & PercorsoTemporaneo & "%'"
                    End If
                    oDataview.RowFilter = "RLPC_TPRL_id=" & Main.TipoRuoloStandard.AdminComunità & PercorsiEsclusi

                    'Aggiunto x blocco!
                    If (j >= _MaxIteration) Then
                        Throw New IndexOutOfRangeException("100% thread lock")
                    End If

                End While
            Catch ex As Exception

            End Try

        End Sub
        Private Sub GeneraNodiOrganizzativi(ByVal oDataset As DataSet, ByVal oResource As ResourceManager)
            Try
                Dim VirtualeID As Integer = -100
                Me.GeneraNodiAltreComunita(oDataset, VirtualeID)
                VirtualeID -= 1

            Catch ex As Exception

            End Try
        End Sub
        Private Sub GeneraNodiAltreComunita(ByVal oDataset As DataSet, ByRef VirtualeID As Integer)
            Try
                Try
                    Dim oDataview As DataView
                    Dim Filtro As String
                    oDataview = oDataset.Tables(0).DefaultView

                    oDataview.RowFilter = "ALCM_Livello=1 AND ALCM_RealPath not like '%-%' AND CMNT_ID > 0"
                    If oDataview.Count > 0 Then
                        Dim ALCM_RealPath, ALCM_Path As String
                        VirtualeID -= 1

                        Filtro = oDataview.RowFilter
                        While oDataview.Count > 0
                            ALCM_RealPath = oDataview.Item(0).Item("ALCM_RealPath")
                            ALCM_Path = ALCM_RealPath & VirtualeID & "."

                            Me.GeneraNodiAltriTipiComunita(oDataset, oDataview, VirtualeID)
                            oDataview.RowFilter = "ALCM_Livello=1 AND ALCM_RealPath not like '%-%'  AND CMNT_ID > 0"
                            If oDataview.Count > 0 Then
                                VirtualeID -= 1
                            End If
                        End While
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End Sub

        Private Sub GeneraNodiAltriTipiComunita(ByVal oDataset As DataSet, ByVal oDataview As DataView, ByRef VirtualeID As Integer)
            Try
                While oDataview.Count > 0
                    Dim oRow As DataRow
                    Dim oRowTesi As DataRow
                    oRow = oDataset.Tables(0).NewRow
                    oRow.Item("ALCM_RealPath") = oDataview.Item(0).Item("ALCM_RealPath")
                    oRow.Item("ALCM_Path") = oDataview.Item(0).Item("ALCM_RealPath") & VirtualeID & "."
                    oRow.Item("CMNT_ID") = VirtualeID
                    oRow.Item("CMNT_Nome") = oDataview.Item(0).Item("TPCM_Descrizione")
                    oRow.Item("ALCM_PadreVirtuale_ID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
                    oRow.Item("ALCM_PadreID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
                    oRow.Item("ALCM_PercorsoDiretto") = True
                    oRow.Item("CMNT_IsChiusa") = False
                    oRow.Item("ALCM_isChiusaForPadre") = False
                    oRow.Item("CMNT_TPCM_ID") = oDataview.Item(0).Item("CMNT_TPCM_ID")
                    oDataset.Tables(0).Rows.Add(oRow)

                    For Each oRowTesi In oDataset.Tables(0).Select("ALCM_Livello=1 AND CMNT_TPCM_ID=" & oDataview.Item(0).Item("CMNT_TPCM_ID") & " AND ALCM_RealPath='" & oRow.Item("ALCM_RealPath") & "'")
                        oRowTesi.Item("ALCM_RealPath") = oRow.Item("ALCM_Path")
                    Next
                    oDataview.RowFilter &= " AND ALCM_RealPath='" & oRow.Item("ALCM_RealPath") & "' AND ALCM_RealPath<>'" & oRow.Item("ALCM_Path") & "' AND CMNT_TPCM_ID<>" & oRow.Item("CMNT_TPCM_ID")
                    If oDataview.Count > 0 Then
                        VirtualeID -= 1
                    End If
                End While

            Catch ex As Exception

            End Try
        End Sub
        Public Function CambiaAttivazione(ByVal ComunitaID As Integer, ByVal isAttivato As Boolean, ByVal oResource As ResourceManager) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() Then
                    Dim i, totale As Integer

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                        Dim oData As DataView
                        Dim oRow As DataRow
                        oData = oDataset.Tables(0).DefaultView

                        oData.RowFilter = "CMNT_ID =" & ComunitaID
                        totale = oData.Count
                        If totale > 0 Then
                            oData.RowFilter = "CMNT_ID =" & ComunitaID
                            totale = oData.Count
                            For i = 0 To totale - 1
                                oRow = oData.Item(i).Row
                                ' oRow.Item("CMNT_isIscritto") = True
                                oRow.Item("RLPC_attivato") = isAttivato

                                If CBool(oRow.Item("RLPC_abilitato")) = False Then
                                    If isAttivato Then
                                        'oRow.Item("Proprieta") = "./../images/ico/lumgiallo.gif"
                                        'If Not IsNothing(oResource) Then
                                        '    oRow.Item("Alternative") = oResource.getValue("abilitato." & Me.StringaAbilitato.inAttesa)
                                        'Else
                                        '    oRow.Item("Alternative") = "Accesso in attesa di conferma"
                                        'End If
                                        oRow.Item("RLPC_abilitato") = isAttivato
                                        oRow.Item("Proprieta") = "./../images/ico/lumverde.gif"
                                        If Not IsNothing(oResource) Then
                                            oRow.Item("Alternative") = oResource.getValue("abilitato." & StringaAbilitato.abilitato)
                                        Else
                                            oRow.Item("Alternative") = "Accesso  consentito"
                                        End If
                                    Else
                                        oRow.Item("Proprieta") = "./../images/ico/lumrosso.gif"
                                        If Not IsNothing(oResource) Then
                                            oRow.Item("Alternative") = oResource.getValue("abilitato." & StringaAbilitato.bloccato)
                                        Else
                                            oRow.Item("Alternative") = "Accesso non consentito"
                                        End If
                                    End If
                                Else
                                    oRow.Item("Proprieta") = "./../images/ico/lumverde.gif"
                                    If Not IsNothing(oResource) Then
                                        oRow.Item("Alternative") = oResource.getValue("abilitato." & StringaAbilitato.abilitato)
                                    Else
                                        oRow.Item("Alternative") = "Accesso  consentito"
                                    End If
                                End If
                            Next
                        End If
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    End If
                End If
            Catch ex As Exception

            End Try
        End Function
        Public Function CambiaMaxIscritti(ByVal ComunitaID As Integer, ByVal MaxIscritti As Integer) As Boolean
            Dim oDataset As New DataSet

            Try

                If Me.Exist() Then
                    Dim i, totale As Integer

                    oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                    If ComunitaID > 0 And oDataset.Tables.Count > 0 Then
                        Dim oData As DataView
                        Dim oRow As DataRow
                        oData = oDataset.Tables(0).DefaultView

                        oData.RowFilter = "CMNT_ID =" & ComunitaID
                        totale = oData.Count
                        If totale > 0 Then
                            oData.RowFilter = "CMNT_ID =" & ComunitaID
                            totale = oData.Count
                            For i = 0 To totale - 1
                                oRow = oData.Item(i).Row
                                oRow.Item("CMNT_MaxIscritti") = MaxIscritti
                            Next
                        End If
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    End If
                End If
            Catch ex As Exception

            End Try
        End Function
        Public Sub AggiornaInfo(ByVal PersonaId As Integer, ByVal LinguaID As Integer, Optional ByVal DayToReloadProfile As Integer = 4, Optional ByVal RigeneraByConfig As Boolean = False)
            Dim totale As Integer
            Dim Rigenera As Boolean = True
            Dim oDataset As New DataSet

            Try
                If Me.Exist() = False Then
                    Me.RigeneraDataset()
                ElseIf RigeneraByConfig Then
                    Rigenera = Me.isToUpdate(DayToReloadProfile)
                End If

                If Rigenera Then
                    oDataset = New DataSet
                    oDataset = Me.CreaAlberoIscrizione(PersonaId, LinguaID, Main.FiltroStatoComunita.Tutte)
                    totale = oDataset.Tables(0).Rows.Count

                    If totale > 0 Then
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    Else
                        Me.RigeneraFileXML()
                    End If
                End If
            Catch ex As Exception
                Me.RigeneraFileXML()
            End Try
        End Sub
        Public Sub AggiornaInfoDefaultUserLanguage(ByVal PersonaId As Integer, Optional ByVal oResourceConfig As ResourceManager = Nothing, Optional ByVal RigeneraByConfig As Boolean = False)

            Dim Rigenera As Boolean = True
            Try
                If Me.Exist() = False Then
                    Me.RigeneraDataset()
                ElseIf Not IsNothing(oResourceConfig) Then
                    If RigeneraByConfig Then
                        Dim Giorni As Integer
                        Try
                            Giorni = oResourceConfig.getValue("aggiornamentoXML")
                        Catch ex As Exception
                            Giorni = 4
                        End Try
                        Rigenera = Me.isToUpdate(Giorni)
                    End If

                End If
                If Rigenera Then
                    Dim totale As Integer
                    Dim oDataset As New DataSet
                    oDataset = Me.CreaAlberoIscrizione(PersonaId, -1, Main.FiltroStatoComunita.Tutte)
                    totale = oDataset.Tables(0).Rows.Count

                    If totale > 0 Then
                        oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
                    Else
                        Me.RigeneraFileXML()
                    End If
                End If
            Catch ex As Exception
                Me.RigeneraFileXML()
            End Try
        End Sub

        Private Function CreaAlberoIscrizione(ByVal PersonaId As Integer, ByVal LinguaID As Integer, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oDataset As New DataSet

            Try
                Dim totale As Integer
                oDataset = COL_Comunita.GeneraAlberoComunitaForAccesso(LinguaID, -1, , -1, PersonaId, , , , , , , , , FiltroStato)
                totale = oDataset.Tables(0).Rows.Count

                If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("HasNews", System.Type.GetType("System.Boolean")))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Proprieta") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta", System.Type.GetType("System.String")))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Alternative") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Alternative", System.Type.GetType("System.String")))
                End If

                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","
                Dim Livello As Integer
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True

                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_PAth"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)

                Dim myRow As DataRow

                For Each myRow In oDataset.Relations("NodeRelation").ParentTable().Rows
                    If myRow.GetParentRows(oDataset.Relations("NodeRelation")).Length = 0 And myRow.Item("ALCM_Livello") <> 0 Then
                        Dim pp As String

                        pp = myRow.Item("ALCM_Path")
                        pp = myRow.Item("ALCM_RealPath")
                        myRow.Delete()
                    End If
                Next myRow
                oDataset.Relations.Remove("NodeRelation")

                oDataview.Sort = "ALCM_Livello DESC"
                Try
                    Livello = oDataview.Item(0).Item("ALCM_Livello")

                    oDataview.RowFilter = "NoDelete<>1 and ALCM_Livello=" & Livello
                Catch ex As Exception
                    Livello = -1
                End Try
                oDataview.Sort = "ALCM_Path"
                While Livello >= 0
                    While oDataview.Count > 0
                        Dim ComunitaID, ComunitaTipoID As Integer
                        Dim ComunitaPercorso, PercorsoPadre As String
                        Dim isDelete As Boolean = False
                        Dim isChiusa As Boolean = False

                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
                        ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
                        PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
                        isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")

                        If IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
                            oDataview.Delete(0)
                            isDelete = True
                        Else
                            If oDataview.Item(0).Row.Item("RLPC_TPRL_id") < 0 And Not (oDataview.Item(0).Row.Item("CMNT_AccessoLibero") = 1 And oDataview.Item(0).Row.Item("RLPC_TPRL_id") = Main.TipoRuoloStandard.AccessoNonAutenticato) Then
                                oDataview.Delete(0)
                                isDelete = True
                            End If
                        End If

                        If Not isDelete Then
                            Dim Filtro As String
                            Dim j As Integer

                            Filtro = oDataview.RowFilter
                            oDataview.RowFilter = "NoDelete <> 1 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
                            For j = oDataview.Count - 1 To 0 Step -1
                                '        prova = oDataview.Item(j).Item("CMNT_ID")
                                oDataview.Item(j).Item("NoDelete") = 1
                            Next
                            oDataview.RowFilter = Filtro
                        End If
                    End While
                    Livello -= 1
                    oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND NoDelete <> 1"
                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()

                totale = oDataset.Tables(0).Rows.Count
            Catch ex As Exception

            End Try
            Return oDataset
        End Function

        Private Function RigeneraDataset() As DataSet
            Dim oDataset As New DataSet

            Try
                oDataset.Tables.Add("comunita")
                oDataset.Tables(0).Columns.Add("ALCM_Livello", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_ID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_isChiusa", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("CMNT_nome", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_TPCM_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_Responsabile", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("AnagraficaCreatore", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_nomePadre", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_Iscritti", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("TPCM_Icona", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("TPCM_Descrizione", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_dataCreazione", System.Type.GetType("System.DateTime"))
                oDataset.Tables(0).Columns.Add("CMNT_dataCessazione", System.Type.GetType("System.DateTime"))
                oDataset.Tables(0).Columns.Add("CMNT_dataInizioIscrizione", System.Type.GetType("System.DateTime"))
                oDataset.Tables(0).Columns.Add("CMNT_dataFineIscrizione", System.Type.GetType("System.DateTime"))
                oDataset.Tables(0).Columns.Add("CMNT_CanSubscribe", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_MaxIscrittiOverList", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_CanUnsubscribe", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_CloneEsternoID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_Bloccata", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("CMNT_Archiviata", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("PRDO_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("PRDO_descrizione", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_Anno", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_AnnoAccademico", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CRSO_codice", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("ALCM_PadreID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ALCM_PadreVirtuale_ID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ALCM_isChiusa", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("ALCM_isChiusaForPadre", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("ALCM_isDiretto", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("ALCM_HasFigli", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("ALCM_PercorsoDiretto", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("ALCM_Path", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("ALCM_RealPath", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_MaxIscritti", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("TPCS_nome", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("RLPC_attivato", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("RLPC_abilitato", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("RLPC_UltimoCollegamento", System.Type.GetType("System.DateTime"))
                oDataset.Tables(0).Columns.Add("TPRL_Nome", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_PRSN_ID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("TPCS_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("NoDelete", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_ORGN_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("RLPC_TPRL_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("ALCM_ResponsabileID", System.Type.GetType("System.Int32"))
                'MANCANO !!!!!!!!!!!!!
                '  oDataset.Tables(0).Columns.Add("lvl", System.Type.GetType("System.Int32"))
                ' oDataset.Tables(0).Columns.Add("CMNT_isIscritto", System.Type.GetType("System.Boolean"))
                ' oDataset.Tables(0).Columns.Add("CMNT_IsComunita", System.Type.GetType("System.Boolean"))
                ' oDataset.Tables(0).Columns.Add("CMNT_CRDS_id", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("HasNews", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("Proprieta", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("Alternative", System.Type.GetType("System.String"))
                oDataset.Tables(0).Columns.Add("CMNT_PRFS_ID", System.Type.GetType("System.Int32"))
                oDataset.Tables(0).Columns.Add("CMNT_AccessoLibero", System.Type.GetType("System.Boolean"))
                oDataset.Tables(0).Columns.Add("CMNT_AccessoCopisteria", System.Type.GetType("System.Boolean"))
            Catch ex As Exception

            End Try
            Return oDataset
        End Function

        Private Sub RigeneraFileXML()
            Dim oDataset As New DataSet

            Try
                oDataset = Me.RigeneraDataset
                oDataset.WriteXml(Me.n_Directory & Me.n_name, XmlWriteMode.WriteSchema)
            Catch ex As Exception

            End Try
        End Sub

        Public Function GetComunitaByIdFromXML(ByVal ComunitaID As Integer, ByVal isAttivato As Boolean, ByVal isAbilitato As Boolean) As DataSet
            Dim oDataset As New DataSet
            Dim filtro As String = ""

            Try
                If ComunitaID >= 0 Then
                    filtro = "CMNT_ID=" & ComunitaID
                End If

                If filtro = "" Then
                    filtro = " RLPC_attivato = " & isAttivato
                Else
                    filtro = filtro & " AND RLPC_attivato = " & isAttivato
                End If

                If filtro = "" Then
                    filtro = " RLPC_abilitato = " & isAbilitato
                Else
                    filtro = filtro & " AND RLPC_abilitato = " & isAbilitato
                End If

                oDataset.ReadXml(Me.n_Directory & Me.n_name, XmlReadMode.Auto)
                If filtro <> "" Then
                    Dim oDatasetTemp As New DataSet
                    Dim oData As DataView
                    Dim i, totale As Integer
                    oData = oDataset.Tables(0).DefaultView
                    oData.RowFilter = filtro

                    oDatasetTemp = RigeneraDataset()
                    totale = oData.Count - 1
                    If oData.Count > 0 Then
                        For i = 0 To totale
                            oDatasetTemp.Tables(0).ImportRow(oData.Item(i).Row)
                        Next
                    End If
                    oDataset = oDatasetTemp

                End If
            Catch ex As Exception
                oDataset = RigeneraDataset()
            End Try
            Return oDataset
        End Function
#End Region

        Public Shared Function GetAnniAccademici(Optional ByVal ORGN_ID As Integer = -1, Optional ByVal PRSN_ID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oDataset As New DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TreeComunita_GetAnniAccademici"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)

            Catch ex As Exception

            End Try
            Return oDataset
        End Function
    End Class

    Public Class CommunityPath
        Public ID As Integer
        Public Path As String
        Public Sub New()

        End Sub
    End Class
End Namespace