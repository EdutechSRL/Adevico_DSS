Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Data
Imports COL_Questionario.RootObject

Public Class DALDomande

    Public Shared Function readDomandaById(ByVal idDomanda As Integer, ByVal idLingua As Int16, ByVal idQuestionario As Integer) As Domanda
        Dim oDomanda As New Domanda
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        sqlCommand = "sp_Questionario_DomandaByIdELingua_Select"
        dbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, idLingua)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oDomanda.id = isNullInt(sqlReader.Item("DMND_Id"))
                oDomanda.testo = isNullString(sqlReader.Item("DMML_Testo"))
                oDomanda.tipo = isNullInt(sqlReader.Item("DMND_Tipo"))
                oDomanda.idLingua = isNullInt(sqlReader.Item("DMML_IdLingua"))
                oDomanda.idDomandaMultilingua = isNullInt(sqlReader.Item("DMML_Id"))
                oDomanda.testoPrima = isNullString(sqlReader.Item("DMML_TestoPrima"))
                oDomanda.testoDopo = isNullString(sqlReader.Item("DMML_TestoDopo"))
                oDomanda.domandaCount = isNullInt(sqlReader.Item("DMND_Count"))
                oDomanda.suggerimento = isNullString(sqlReader.Item("DMML_Suggerimento"))
                oDomanda.difficolta = isNullInt(sqlReader.Item("LKQD_Difficolta"))
            End While
            sqlReader.Close()
        End Using

        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.Multipla
                oDomanda.domandaMultiplaOpzioni = DALDomande.readDomandaMultiplaOpzioni(oDomanda, db, conn)
            Case Domanda.TipoDomanda.DropDown
                oDomanda.domandaDropDown = DALDomande.readDomandaDropDownBYIDDomanda(oDomanda, db, conn)
            Case Domanda.TipoDomanda.Rating
                oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, conn)
            Case Domanda.TipoDomanda.Meeting
                oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, conn)
            Case Domanda.TipoDomanda.TestoLibero
                oDomanda.opzioniTestoLibero = DALDomande.readDomandaTestoLiberoByID(oDomanda.idDomandaMultilingua, db, conn)
            Case Domanda.TipoDomanda.Numerica
                oDomanda.opzioniNumerica = DALDomande.readDomandaNumericaById(oDomanda.idDomandaMultilingua, db, conn)
        End Select



        Return oDomanda

    End Function
    Public Shared Sub readDomandeByPagina(ByVal oQuestionario As Questionario, ByVal db As Database, ByVal conn As DbConnection)

        For Each pagina As QuestionarioPagina In oQuestionario.pagine
            Dim sqlCommand As String
            Dim dbCommand As DbCommand

            If oQuestionario.idFiglio = 0 Then
                sqlCommand = "sp_Questionario_DomandeByPagina_Select"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = conn
                db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.id)
            Else
                sqlCommand = "sp_Questionario_DomandeByPaginaRandom_Select"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = conn
                db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, oQuestionario.idFiglio)
            End If

            db.AddInParameter(dbCommand, "dallaDomanda", DbType.Int32, pagina.dallaDomanda)
            db.AddInParameter(dbCommand, "allaDomanda", DbType.Int32, pagina.allaDomanda)
            db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuestionario.idLingua)

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim oDomanda As New Domanda

                    oDomanda = readCampiDomanda(oDomanda, sqlReader)

                    oDomanda.numeroPagina = pagina.numeroPagina
                    oDomanda.idPagina = pagina.id

                    'If oQuestionario.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
                    '    Dim nDomandeAssociateQuest As Integer = DALDomande.CountDomandeLibreriaInQuestionari(oDomanda.id, oQuestionario.id)
                    '    If nDomandeAssociateQuest > 0 Then
                    '        oDomanda.isReadOnly = True
                    '    End If
                    'Else
                    '    oDomanda.isReadOnly = False
                    'End If

                    pagina.domande.Add(oDomanda)

                End While
                sqlReader.Close()
            End Using
        Next

    End Sub

    Public Shared Sub readDomandeOldByLibreria(ByVal oQuestionario As Questionario, ByVal db As Database, ByVal conn As DbConnection)

        Dim sqlCommand As String
        Dim dbCommand As DbCommand

        sqlCommand = "sp_Questionario_DomandeOldByLibreria_Select"
        dbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.id)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuestionario.idLingua)

        Dim virtualNumber As Long = 1
        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oDomanda As New Domanda

                oDomanda = readCampiDomanda(oDomanda, sqlReader)

                'oDomanda.testo = oDomanda.testo+"(old version - domanda assco)"
                oDomanda.numeroPagina = oQuestionario.pagine.Count - 1
                oDomanda.idPagina = oQuestionario.pagine(oDomanda.numeroPagina).id
                oDomanda.VirtualNumber = virtualNumber

                oQuestionario.pagine(oDomanda.numeroPagina).domande.Add(oDomanda)
                virtualNumber += 1
            End While
            sqlReader.Close()
        End Using

    End Sub

    Private Shared Function readCampiDomanda(ByVal oDomanda As Domanda, ByVal sqlReader As IDataReader) As Domanda

        oDomanda.id = isNullInt(sqlReader.Item("DMND_Id"))
        oDomanda.numero = isNullInt(sqlReader.Item("LKQD_NumeroDomanda"))
        oDomanda.testo = isNullString(sqlReader.Item("DMML_Testo"))
        oDomanda.tipo = isNullInt(sqlReader.Item("DMND_Tipo"))
        oDomanda.idQuestionario = isNullInt(sqlReader.Item("LKQD_QSTN_Id"))
        oDomanda.peso = isNullInt(sqlReader.Item("LKQD_PesoDomanda"))
        oDomanda.tipoGrafico = isNullInt(sqlReader.Item("LKQD_TPGF_Id"))
        oDomanda.isObbligatoria = isNullBoolean(sqlReader.Item("LKQD_isObbligatorio"))
        oDomanda.isValutabile = isNullBoolean(sqlReader.Item("LKQD_isValutabile"))
        oDomanda.idLingua = isNullInt(sqlReader.Item("DMML_IdLingua"))
        oDomanda.difficolta = isNullInt(sqlReader.Item("LKQD_Difficolta"))
        oDomanda.idDomandaMultilingua = isNullInt(sqlReader.Item("DMML_Id"))
        oDomanda.testoPrima = isNullString(sqlReader.Item("DMML_TestoPrima"))
        oDomanda.testoDopo = isNullString(sqlReader.Item("DMML_TestoDopo"))
        oDomanda.domandaCount = isNullInt(sqlReader.Item("DMND_Count"))
        oDomanda.suggerimento = isNullString(sqlReader.Item("DMML_Suggerimento"))

        Return oDomanda

    End Function

    Public Shared Sub readDomandeByQuestionario(ByVal oQuestionario As Questionario)
        readDomandeByQuestionario(oQuestionario, Nothing, Nothing)
    End Sub

    ' usato per il caricamento dalla libreria di domande
    Public Shared Function readDomandeByQuestionario(ByVal oQuestionario As Questionario, ByVal db As Database, ByVal conn As DbConnection) As Questionario
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
            conn = db.CreateConnection()
        End If

        If oQuestionario.idFiglio = 0 Then
            sqlCommand = "sp_Questionario_DomandeByIDQuestionario_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = conn
            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.id)
        Else
            sqlCommand = "sp_Questionario_DomandeByIDQuestionarioRandom_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = conn
            db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, oQuestionario.idFiglio)

        End If


        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuestionario.idLingua)


        Dim oPagina As New QuestionarioPagina
        oPagina.numeroPagina = 1
        oQuestionario.pagine.Add(oPagina)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oDomanda As New Domanda

                oDomanda = readCampiDomanda(oDomanda, sqlReader)

                oDomanda.numeroPagina = oPagina.numeroPagina
                oQuestionario.pagine(0).domande.Add(oDomanda)
            End While
            sqlReader.Close()
        End Using

        Return oQuestionario
    End Function
    Public Shared Function readIdDomandeAutovalutazione(ByRef idQuestionario As Integer, ByRef difficolta As Domanda.DifficoltaDomanda) As List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        Dim idCollection As New List(Of Integer)
        If difficolta = Domanda.DifficoltaDomanda.Tutte Then
            sqlCommand = "sp_Questionario_DomandeLibrerieByIDQuestionario_select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = conn
        Else
            sqlCommand = "sp_Questionario_DomandeByIDQuestionarioEDifficolta_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = conn
            db.AddInParameter(dbCommand, "difficolta", DbType.Int16, difficolta)
        End If

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                idCollection.Add(isNullInt(sqlReader.Item("LKQD_DMND_Id")))
            End While
            sqlReader.Close()
        End Using

        Return idCollection

    End Function

    Public Shared Function randomizeCollectionOfQuestions(ByRef collectionOfQuestion As List(Of Domanda), Optional ByRef startNumber As Integer = 1) As List(Of Domanda)
        Dim indexCollection As New List(Of Integer)
        Dim newIndexCollection As New List(Of Integer)
        Dim newCollection As New List(Of Domanda)
        Dim counter As Integer
        Dim oDomanda As New Domanda
        For counter = 0 To collectionOfQuestion.Count - 1
            indexCollection.Add(counter)
        Next
        Randomize()
        While indexCollection.Count > 0
            Dim index As Integer = Math.Round(Rnd() * (indexCollection.Count - 1))
            oDomanda = collectionOfQuestion.Item(indexCollection.Item(index))
            oDomanda.numero = newCollection.Count + startNumber
            newCollection.Add(oDomanda)
            indexCollection.RemoveAt(index)
        End While
        Return newCollection
    End Function

    Public Shared Function readDomandaMultiplaOpzioni(ByVal oDomanda As Domanda, ByVal db As Database, ByVal conn As DbConnection) As List(Of DomandaOpzione)
        Dim listaOpzioni As New List(Of DomandaOpzione)
        Try
            Dim sqlCommand As String = "sp_Questionario_DomandaMultiplaOpzioni_Select"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = conn
            db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oDomanda.idDomandaMultilingua)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim oDomandaOpzione As New DomandaOpzione
                    oDomanda.numeroMaxRisposte = isNullInt(sqlReader.Item("DMMT_NumeroMaxRisposte"))
                    oDomandaOpzione.id = isNullInt(sqlReader.Item("DMMO_Id"))
                    oDomandaOpzione.numero = isNullInt(sqlReader.Item("DMMO_NumeroOpzione"))
                    oDomandaOpzione.testo = isNullString(sqlReader.Item("DMMO_Testo"))
                    oDomandaOpzione.peso = isNullDecimal(sqlReader.Item("DMMO_Peso"))
                    oDomandaOpzione.isCorretta = isNullBoolean(sqlReader.Item("DMMO_isCorretta"))
                    oDomandaOpzione.isAltro = isNullBoolean(sqlReader.Item("DMMO_isAltro"))
                    oDomandaOpzione.suggestion = isNullString(sqlReader.Item("DMMO_Suggestion"))
                    listaOpzioni.Add(oDomandaOpzione)
                End While
                sqlReader.Close()
            End Using
            If Not oDomanda.numeroMaxRisposte = 1 Then
                oDomanda.isMultipla = True
            End If
        Catch ex As Exception
            Dim errore As String = ex.Message
        End Try
        Return listaOpzioni
    End Function


    Public Shared Function readDomandaRatingByID(ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As DomandaRating

        Dim sqlCommand As String = "sp_Questionario_DomandaRatingBYIDDomanda_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        Dim oDomandaRating As New DomandaRating
        Dim listaOpzioni As New List(Of DomandaOpzione)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                oDomandaRating.id = isNullInt(sqlReader.Item("DMRT_Id"))
                oDomandaRating.numeroRating = isNullInt(sqlReader.Item("DMRT_Numero"))
                oDomandaRating.mostraND = isNullBoolean(sqlReader.Item("DMRT_MostraND"))
                oDomandaRating.testoND = isNullString(sqlReader.Item("DMRT_TestoND"))
                oDomandaRating.tipoIntestazione = isNullInt(sqlReader.Item("DMRT_TipoIntestazione"))
                Dim oOpzione As New DomandaOpzione
                oOpzione.id = isNullString(sqlReader.Item("DMRO_Id"))
                oOpzione.testo = isNullString(sqlReader.Item("DMRO_TestoMin"))
                oOpzione.testoDopo = isNullString(sqlReader.Item("DMRO_TestoMax"))
                oOpzione.isAltro = isNullBoolean(sqlReader.Item("DMRO_isAltro"))
                oOpzione.numero = isNullInt(sqlReader.Item("DMRO_NumeroOpzione"))
                oOpzione.arrayCBisVisible = isNullString(sqlReader.Item("DMRO_ArrayCBisVisibile"))
                oDomandaRating.opzioniRating.Add(oOpzione)
            End While
            sqlReader.Close()
        End Using

        If oDomandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi Then
            Dim sqlCommand2 As String = "sp_Questionario_DomandaRatingIntestazioni_Select"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
            dbCommand2.Connection = conn
            db.AddInParameter(dbCommand2, "idDomandaRating", DbType.Int32, oDomandaRating.id)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand2)
                While sqlReader.Read()
                    Dim oOpzione As New DomandaOpzione
                    oOpzione.id = isNullInt(sqlReader.Item("DMRI_Id"))
                    oOpzione.numero = isNullInt(sqlReader.Item("DMRI_Indice"))
                    oOpzione.testo = isNullString(sqlReader.Item("DMRI_Testo"))
                    oDomandaRating.intestazioniRating.Add(oOpzione)
                End While
                sqlReader.Close()
            End Using

        ElseIf oDomandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
            For i As Integer = 1 To oDomandaRating.numeroRating
                Dim oOpzione As New DomandaOpzione
                oOpzione.numero = i
                oOpzione.testo = i.ToString()
                oDomandaRating.intestazioniRating.Add(oOpzione)
            Next

        End If

        Return oDomandaRating
    End Function

    Public Shared Function readDomandaTestoLiberoByID(ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of DomandaTestoLibero)

        Dim sqlCommand As String = "sp_Questionario_DomandaTestoLiberoBYIDDomanda_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        Dim oDomande As New List(Of DomandaTestoLibero)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oDomanda As New DomandaTestoLibero
                oDomanda.id = isNullInt(sqlReader.Item("DMTL_Id"))
                oDomanda.idDomanda = isNullInt(sqlReader.Item("DMTL_DMML_Id"))
                oDomanda.etichetta = isNullString(sqlReader.Item("DMTL_Etichetta"))
                oDomanda.isSingleLine = isNullBoolean(sqlReader.Item("DMTL_IsSingleLine"))
                oDomanda.numeroRighe = isNullInt(sqlReader.Item("DMTL_NumeroRighe"))
                oDomanda.numeroColonne = isNullInt(sqlReader.Item("DMTL_NumeroColonne"))
                oDomanda.numero = isNullInt(sqlReader.Item("DMTL_Numero"))
                oDomanda.peso = isNullInt(sqlReader.Item("DMTL_Peso"))
                oDomande.Add(oDomanda)
            End While
            sqlReader.Close()
        End Using

        Return oDomande
    End Function

    Public Shared Function readDomandaNumericaById(ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of DomandaNumerica)

        Dim sqlCommand As String = "sp_Questionario_DomandaNumericaBYIDDomanda_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        Dim oDomande As New List(Of DomandaNumerica)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oDomanda As New DomandaNumerica
                oDomanda.id = isNullInt(sqlReader.Item("DMNM_Id"))
                oDomanda.idDomanda = isNullInt(sqlReader.Item("DMNM_DMML_Id"))
                oDomanda.testoPrima = isNullString(sqlReader.Item("DMNM_testoPrima"))
                oDomanda.testoDopo = isNullString(sqlReader.Item("DMNM_testoDopo"))
                oDomanda.dimensione = isNullInt(sqlReader.Item("DMNM_dimensione"))
                oDomanda.rispostaCorretta = isNullDouble(sqlReader.Item("DMNM_rispostaCorretta"))
                oDomanda.numero = isNullInt(sqlReader.Item("DMNM_Numero"))
                oDomanda.peso = isNullDecimal(sqlReader.Item("DMNM_Peso"))
                oDomande.Add(oDomanda)
            End While
            sqlReader.Close()
        End Using

        Return oDomande
    End Function

    Private Shared Function UpdateDomandeQuestionari(ByRef oDomanda As Domanda, ByVal isReadOnly As Boolean) As Boolean
        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.Multipla
                If oDomanda.domandaMultiplaOpzioni.Count > 0 Then
                    DomandaMultipla_Update(oDomanda, isReadOnly)
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.DropDown
                If oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                    DomandaDropDown_Update(oDomanda, isReadOnly)
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.Rating
                If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                    DomandaRating_Update(oDomanda, isReadOnly)
                Else
                    Return False
                End If

            Case Domanda.TipoDomanda.RatingStars
                DomandaRating_Update(oDomanda, isReadOnly)

            Case Domanda.TipoDomanda.Meeting
                If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                    DomandaRating_Update(oDomanda, isReadOnly)
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.TestoLibero
                DomandaTestoLibero_Update(oDomanda, isReadOnly)
            Case Domanda.TipoDomanda.Numerica
                DomandaNumerica_Update(oDomanda, isReadOnly)
        End Select
    End Function

    Private Shared Function UpdateDomandeLibreria(ByRef oDomanda As Domanda, ByVal isMinorUpdate As Boolean) As Boolean
        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.Multipla
                If oDomanda.domandaMultiplaOpzioni.Count > 0 Then
                    If isMinorUpdate Then ' faccio l'update solo dei campi minori
                        DomandaMultipla_Update(oDomanda, isMinorUpdate)
                    Else
                        ' imposto isOld = true nell'associazione domanda libreria
                        DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                        ' inserisco una nuova domanda associata alla libreria
                        DomandaMultipla_Insert(Domanda_Insert(oDomanda))

                        '' COMMENTATA IL 30/06/2014
                        '' controllo se la domanda è associata solo a quella libreria/questionario
                        'If CountDomandaInQuestionari(oDomanda.id) > 1 Then
                        '    ' imposto isOld = true nell'associazione domanda libreria
                        '    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                        '    'DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                        '    ' inserisco una nuova domanda associata alla libreria
                        '    DomandaMultipla_Insert(Domanda_Insert(oDomanda))
                        'Else
                        '    ' cancello la domanda
                        '    DALDomande.Domanda_Delete(oDomanda.idQuestionario, oDomanda.numero, oDomanda.id)
                        '    ' inserisco una nuova domanda associata alla libreria
                        '    DomandaMultipla_Insert(Domanda_Insert(oDomanda))
                        'End If
                    End If
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.DropDown
                If oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                    If isMinorUpdate Then
                        DomandaDropDown_Update(oDomanda, isMinorUpdate)
                    Else
                        ' cancello l'associazione domanda/libreria
                        DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)

                        'DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                        ' inserisco una nuova domanda associata alla libreria
                        DomandaDropDown_Insert(Domanda_Insert(oDomanda))

                        '' COMMENTATA IL 30/06/2014
                        'If CountDomandaInQuestionari(oDomanda.id) > 1 Then
                        '    ' cancello l'associazione domanda/libreria
                        '    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)

                        '    'DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                        '    ' inserisco una nuova domanda associata alla libreria
                        '    DomandaDropDown_Insert(Domanda_Insert(oDomanda))
                        'Else
                        '    ' cancello la domanda
                        '    DALDomande.Domanda_Delete(oDomanda.idQuestionario, oDomanda.numero, oDomanda.id)
                        '    ' inserisco una nuova domanda associata alla libreria
                        '    DomandaDropDown_Insert(Domanda_Insert(oDomanda))
                        'End If
                    End If
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.Rating
                If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                    DomandaRating_Update(oDomanda, isMinorUpdate)
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.RatingStars
                DomandaRating_Update(oDomanda, isMinorUpdate)

            Case Domanda.TipoDomanda.TestoLibero
                If isMinorUpdate Then ' faccio l'update solo dei campi minori
                    DomandaTestoLibero_Update(oDomanda, isMinorUpdate)
                Else
                    ' cancello l'associazione domanda/libreria
                    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                    ' inserisco una nuova domanda associata alla libreria
                    DomandaTestoLibero_Insert(Domanda_Insert(oDomanda))

                    '' COMMENTATA IL 30/06/2014
                    '' controllo se la domanda è associata solo a quella libreria/questionario
                    'If CountDomandaInQuestionari(oDomanda.id) > 1 Then
                    '    ' cancello l'associazione domanda/libreria
                    '    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)

                    '    'DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                    '    ' inserisco una nuova domanda associata alla libreria
                    '    DomandaTestoLibero_Insert(Domanda_Insert(oDomanda))
                    'Else
                    '    ' cancello la domanda
                    '    DALDomande.Domanda_Delete(oDomanda.idQuestionario, oDomanda.numero, oDomanda.id)
                    '    ' inserisco una nuova domanda associata alla libreria
                    '    DomandaTestoLibero_Insert(Domanda_Insert(oDomanda))
                    'End If
                End If
            Case Domanda.TipoDomanda.Numerica
                If isMinorUpdate Then ' faccio l'update solo dei campi minori
                    DomandaNumerica_Update(oDomanda, isMinorUpdate)
                Else
                    ' cancello l'associazione domanda/libreria
                    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)

                    'DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                    ' inserisco una nuova domanda associata alla libreria
                    DomandaNumerica_Insert(Domanda_Insert(oDomanda))

                    '' COMMENTATA IL 30/06/2014
                    ' controllo se la domanda è associata solo a quella libreria/questionario
                    'If CountDomandaInQuestionari(oDomanda.id) > 1 Then
                    '    cancello(l) 'associazione domanda/libreria
                    '    DomandaQuestionarioLK_Set_isOld(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)

                    '    DomandaQuestionarioLK_Delete(oDomanda.id, oDomanda.numero, oDomanda.idQuestionario)
                    '     inserisco una nuova domanda associata alla libreria
                    '    DomandaNumerica_Insert(Domanda_Insert(oDomanda))
                    'Else
                    '     cancello la domanda
                    '    DALDomande.Domanda_Delete(oDomanda.idQuestionario, oDomanda.numero, oDomanda.id)
                    '     inserisco una nuova domanda associata alla libreria
                    '    DomandaNumerica_Insert(Domanda_Insert(oDomanda))
                    'End If
                End If

        End Select
    End Function

    Public Shared Function Salva(ByRef oDomanda As Domanda, ByVal isMinorUpdate As Boolean, ByVal isLibreria As Boolean) As Boolean
        Dim retVal As Boolean = True
        If oDomanda.id > 0 Then
            oDomanda.isNew = False
            If isLibreria Then
                UpdateDomandeLibreria(oDomanda, isMinorUpdate)
            Else
                UpdateDomandeQuestionari(oDomanda, isMinorUpdate)
            End If
        Else
            oDomanda.isNew = True
            Select Case oDomanda.tipo
                Case Domanda.TipoDomanda.Multipla
                    If oDomanda.domandaMultiplaOpzioni.Count > 0 Then
                        DomandaMultipla_Insert(Domanda_Insert(oDomanda))
                    Else
                        Return False
                    End If
                Case Domanda.TipoDomanda.DropDown
                    If oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                        DomandaDropDown_Insert(Domanda_Insert(oDomanda))
                    Else
                        Return False
                    End If
                Case Domanda.TipoDomanda.Rating
                    If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                        DomandaRating_Insert(Domanda_Insert(oDomanda))
                    Else
                        Return False
                    End If

                Case Domanda.TipoDomanda.RatingStars
                    'If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                    DomandaRatingStars_Insert(Domanda_Insert(oDomanda))

                Case Domanda.TipoDomanda.Meeting
                    If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                        DomandaMeeting_Insert(Domanda_Insert(oDomanda))
                    Else
                        Return False
                    End If
                Case Domanda.TipoDomanda.TestoLibero
                    DomandaTestoLibero_Insert(Domanda_Insert(oDomanda))
                Case Domanda.TipoDomanda.Numerica
                    DomandaNumerica_Insert(Domanda_Insert(oDomanda))
            End Select
        End If

        Return retVal
    End Function

    Public Shared Function SalvaMultilingua(ByRef oDomanda As Domanda, ByVal isChiuso As Boolean) As Boolean
        Dim retVal As Boolean = True
        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.Multipla
                If oDomanda.domandaMultiplaOpzioni.Count > 0 Then
                    DomandaMultipla_Insert(DomandaMultilingua_Insert(oDomanda))
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.DropDown
                If oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                    DomandaDropDown_Insert(DomandaMultilingua_Insert(oDomanda))
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.Rating
                If oDomanda.domandaRating.intestazioniRating.Count > 0 Then
                    DomandaRating_Insert(DomandaMultilingua_Insert(oDomanda))
                Else
                    Return False
                End If
            Case Domanda.TipoDomanda.TestoLibero
                DomandaTestoLibero_Insert(DomandaMultilingua_Insert(oDomanda))
            Case Domanda.TipoDomanda.Numerica
                DomandaNumerica_Insert(DomandaMultilingua_Insert(oDomanda))
        End Select
        Return retVal
    End Function

    Public Shared Function DomandaMultipla_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DomandaMultipla_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "numeroMaxRisposte", DbType.Int32, dom.numeroMaxRisposte)
        db.AddInParameter(dbCommand, "idDomandaMultilingua", DbType.String, dom.idDomandaMultilingua)
        db.AddOutParameter(dbCommand, "idDomandaMultipla", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Dim idDomandaMultipla As Integer = db.GetParameterValue(dbCommand, "idDomandaMultipla")

        DomandaMultiplaOpzioni_Delete(dom, idDomandaMultipla)
        DomandaMultiplaOpzioni_Insert(dom, idDomandaMultipla)

        Return RetVal
    End Function

    Public Shared Function DomandaMultiplaOpzioni_Delete(ByRef dom As Domanda, ByVal idDomandaMultipla As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim oDomOpzione As New DomandaOpzione

        For Each oDomOpzione In dom.domandaMultiplaOpzioni
            Dim sqlCommand As String = "sp_Questionario_DomandaMultiplaOpzioni_Delete"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "idDomandaMultipla", DbType.Int32, idDomandaMultipla)

            RetVal = db.ExecuteNonQuery(dbCommand)
        Next

        Return RetVal
    End Function

    Public Shared Function DomandaMultiplaOpzioni_Insert(ByRef dom As Domanda, ByVal idDomandaMultipla As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim oDomOpzione As New DomandaOpzione

        For Each oDomOpzione In dom.domandaMultiplaOpzioni

            Dim sqlCommand As String = "sp_Questionario_DomandaMultiplaOpzioni_Insert"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "testo", DbType.String, oDomOpzione.testo)
            db.AddInParameter(dbCommand, "numero", DbType.Int32, oDomOpzione.numero)
            db.AddInParameter(dbCommand, "idDomandaMultipla", DbType.Int32, idDomandaMultipla)
            db.AddInParameter(dbCommand, "peso", DbType.Decimal, oDomOpzione.peso)
            db.AddInParameter(dbCommand, "isCorretta", DbType.Boolean, oDomOpzione.isCorretta)
            db.AddInParameter(dbCommand, "isAltro", DbType.Boolean, oDomOpzione.isAltro)
            db.AddInParameter(dbCommand, "suggestion", DbType.String, oDomOpzione.suggestion)

            RetVal = db.ExecuteNonQuery(dbCommand)

        Next

        Return RetVal
    End Function

    Public Shared Function DomandaMultiplaOpzioni_Update(ByRef dom As Domanda, ByVal idDomandaMultipla As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As Integer = 0


        Dim oDomOpzione As New DomandaOpzione

        For Each oDomOpzione In dom.domandaMultiplaOpzioni

            Dim sqlCommand As String = "sp_Questionario_DomandaMultiplaOpzioni_Update"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "testo", DbType.String, oDomOpzione.testo)
            db.AddInParameter(dbCommand, "numero", DbType.Int32, oDomOpzione.numero)
            db.AddInParameter(dbCommand, "idDomandaMultipla", DbType.Int32, idDomandaMultipla)
            db.AddInParameter(dbCommand, "peso", DbType.Decimal, oDomOpzione.peso)
            db.AddInParameter(dbCommand, "isCorretta", DbType.Boolean, oDomOpzione.isCorretta)
            db.AddInParameter(dbCommand, "isAltro", DbType.Boolean, oDomOpzione.isAltro)
            db.AddInParameter(dbCommand, "idDomandaMultiplaOpzioni", DbType.Int32, oDomOpzione.id)
            db.AddInParameter(dbCommand, "suggestion", DbType.String, oDomOpzione.suggestion)

            RetVal = db.ExecuteNonQuery(dbCommand)

        Next

        Return RetVal
    End Function

    Public Shared Function DomandaMultipla_Update(ByRef dom As Domanda, ByRef isChiuso As Boolean) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DomandaMultipla_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomanda", DbType.String, dom.id)
        db.AddInParameter(dbCommand, "idDomandaMultilingua", DbType.String, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand, "idLingua", DbType.String, dom.idLingua)
        db.AddInParameter(dbCommand, "numero", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand, "tipo", DbType.Int32, dom.tipo)
        db.AddInParameter(dbCommand, "peso", DbType.Int32, dom.peso)
        db.AddInParameter(dbCommand, "isValutabile", DbType.Boolean, dom.isValutabile)
        db.AddInParameter(dbCommand, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddInParameter(dbCommand, "difficolta", DbType.Int32, dom.difficolta)
        db.AddInParameter(dbCommand, "numeroMaxRisposte", DbType.Int32, dom.numeroMaxRisposte)
        db.AddInParameter(dbCommand, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))

        db.AddOutParameter(dbCommand, "idDomandaMultipla", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Dim idDomandaMultipla As Integer = db.GetParameterValue(dbCommand, "idDomandaMultipla")


        If isChiuso Then
            DomandaMultiplaOpzioni_Update(dom, idDomandaMultipla)
        Else
            DomandaMultiplaOpzioni_Delete(dom, idDomandaMultipla)
            DomandaMultiplaOpzioni_Insert(dom, idDomandaMultipla)
        End If

        Return RetVal
    End Function

    Public Shared Function DomandaInvertiNumero_Update(ByRef idDomandaUP As Integer, ByVal idDomandaDOWN As Integer, ByVal numeroDomUP As Integer, ByVal numeroDomDOWN As Integer, ByVal idQuestionario As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DomandaInvertiNumero_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomandaUP", DbType.Int32, idDomandaUP)
        db.AddInParameter(dbCommand, "numeroDomandaUP", DbType.Int32, numeroDomUP)
        db.AddInParameter(dbCommand, "idDomandaDOWN", DbType.Int32, idDomandaDOWN)
        db.AddInParameter(dbCommand, "numeroDomandaDOWN", DbType.Int32, numeroDomDOWN)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        RetVal = db.ExecuteNonQuery(dbCommand)


        Return RetVal
    End Function

    Public Shared Function DomandaDropDown_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        dom.domandaDropDown.idDomanda = dom.idDomandaMultilingua
        Dim idDropDown As String = DALDropDown.DropDown_Insert(dom.domandaDropDown)

        Dim sqlCommand2 As String = "sp_Questionario_DomandaDropDown_Insert"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "idDropDown", DbType.Int32, idDropDown)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        Return RetVal
    End Function

    Public Shared Function DomandaDropDown_Update(ByRef dom As Domanda, ByVal isChiuso As Boolean) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DomandaDropDown_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, dom.id)
        db.AddInParameter(dbCommand, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand, "peso", DbType.String, dom.peso)
        db.AddInParameter(dbCommand, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand, "isValutabile", DbType.Boolean, dom.isValutabile)
        db.AddInParameter(dbCommand, "difficolta", DbType.String, dom.difficolta)
        db.AddInParameter(dbCommand, "nome", DbType.String, dom.domandaDropDown.nome)
        db.AddInParameter(dbCommand, "etichetta", DbType.String, dom.domandaDropDown.etichetta)
        db.AddInParameter(dbCommand, "ordinata", DbType.Boolean, dom.domandaDropDown.ordinata)
        db.AddInParameter(dbCommand, "tipo", DbType.Int32, dom.domandaDropDown.tipo)
        db.AddInParameter(dbCommand, "idDropDown", DbType.Int32, dom.domandaDropDown.id)
        db.AddInParameter(dbCommand, "idDomandaMultilingua", DbType.String, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddInParameter(dbCommand, "numero", DbType.Int32, dom.numero)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Dim oItem As New DropDownItem
        If isChiuso Then
            Dim counter As Int16
            For counter = 1 To dom.domandaDropDown.dropDownItems.Count
                Dim sqlCommand2 As String = "sp_Questionario_DropDownItem_Update"
                Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
                db.AddInParameter(dbCommand2, "idDropDown", DbType.Int32, dom.domandaDropDown.id)
                db.AddInParameter(dbCommand2, "testo", DbType.String, dom.domandaDropDown.dropDownItems(counter - 1).testo)
                db.AddInParameter(dbCommand2, "suggestion", DbType.String, dom.domandaDropDown.dropDownItems(counter - 1).suggestion)
                db.AddInParameter(dbCommand2, "indice", DbType.Int32, counter)
                RetVal = db.ExecuteNonQuery(dbCommand2)
            Next
        Else
            Dim sqlCommand3 As String = "sp_Questionario_DropDownItem_Delete"
            Dim dbCommand3 As DbCommand = db.GetStoredProcCommand(sqlCommand3)
            db.AddInParameter(dbCommand3, "idDropDown", DbType.Int32, dom.domandaDropDown.id)
            RetVal = db.ExecuteNonQuery(dbCommand3)

            For Each ddItem As DropDownItem In dom.domandaDropDown.dropDownItems

                Dim sqlCommand2 As String = "sp_Questionario_DropDownItem_Insert"
                Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
                db.AddInParameter(dbCommand2, "idDropDown", DbType.Int32, dom.domandaDropDown.id)
                db.AddInParameter(dbCommand2, "suggestion", DbType.String, ddItem.suggestion)
                db.AddInParameter(dbCommand2, "testo", DbType.String, ddItem.testo)
                db.AddInParameter(dbCommand2, "valore", DbType.Int32, ddItem.numero)
                db.AddInParameter(dbCommand2, "indice", DbType.Int32, ddItem.indice)
                db.AddInParameter(dbCommand2, "peso", DbType.Decimal, ddItem.peso)
                db.AddInParameter(dbCommand2, "isCorretta", DbType.Boolean, ddItem.isCorretta)

                RetVal = db.ExecuteNonQuery(dbCommand2)

            Next

        End If

        Return RetVal
    End Function

    Public Shared Function readDomandaDropDownBYIDDomanda(ByVal oDomanda As Domanda, ByVal db As Database, ByVal conn As DbConnection) As DropDown

        Dim sqlCommand As String = "sp_Questionario_DropDownByIDDomanda_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oDomanda.idDomandaMultilingua)
        Dim odropdown As New DropDown

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                odropdown.id = isNullInt(sqlReader.Item("DMDR_DROP_Id"))
                odropdown.idDomanda = isNullInt(sqlReader.Item("DMDR_DMML_Id"))
                odropdown.nome = isNullString(sqlReader.Item("DROP_Nome"))
                odropdown.etichetta = isNullString(sqlReader.Item("DROP_Label"))
                odropdown.ordinata = isNullBoolean(sqlReader.Item("DROP_Ordinata"))
                odropdown.tipo = isNullInt(sqlReader.Item("DROP_Tipo"))
                odropdown.isMultipla = isNullBoolean(sqlReader.Item("DMDR_isMultipla"))
            End While
            sqlReader.Close()
        End Using
        odropdown.dropDownItems = DALDropDown.readDropDownItems(odropdown)

        Return odropdown
    End Function

    Public Shared Function DomandaRating_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim sqlCommand2 As String = "sp_Questionario_DomandaRating_Insert"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.domandaRating.numeroRating)
        db.AddInParameter(dbCommand2, "mostraND", DbType.Boolean, dom.domandaRating.mostraND)
        db.AddInParameter(dbCommand2, "testoND", DbType.String, dom.domandaRating.testoND)
        db.AddInParameter(dbCommand2, "tipoIntestazione", DbType.Int32, dom.domandaRating.tipoIntestazione)
        db.AddOutParameter(dbCommand2, "idDomandaRating", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        If dom.domandaRating.opzioniRating.Count = 0 Then
            Dim odomandaVuota As New DomandaOpzione
            dom.domandaRating.opzioniRating.Add(odomandaVuota)
        End If

        dom.domandaRating.id = db.GetParameterValue(dbCommand2, "@idDomandaRating")
        For Each oDomIntest As DomandaOpzione In dom.domandaRating.intestazioniRating
            DomandaRatingIntestazioni_Insert(oDomIntest, dom.domandaRating.id)
        Next

        For Each odomOpzione As DomandaOpzione In dom.domandaRating.opzioniRating
            DomandaRatingOpzioni_Insert(odomOpzione, dom.domandaRating.id)
        Next

        Return RetVal
    End Function

    Public Shared Function DomandaRatingStars_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim sqlCommand2 As String = "sp_Questionario_DomandaRating_Insert"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.domandaRating.numeroRating)
        db.AddInParameter(dbCommand2, "mostraND", DbType.Boolean, dom.domandaRating.mostraND)
        db.AddInParameter(dbCommand2, "testoND", DbType.String, dom.domandaRating.testoND)
        db.AddInParameter(dbCommand2, "tipoIntestazione", DbType.Int32, dom.domandaRating.tipoIntestazione)
        db.AddOutParameter(dbCommand2, "idDomandaRating", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        If dom.domandaRating.opzioniRating.Count = 0 Then
            Dim odomandaVuota As New DomandaOpzione
            dom.domandaRating.opzioniRating.Add(odomandaVuota)
        End If

        dom.domandaRating.id = db.GetParameterValue(dbCommand2, "@idDomandaRating")
        For Each oDomIntest As DomandaOpzione In dom.domandaRating.intestazioniRating
            DomandaRatingIntestazioni_Insert(oDomIntest, dom.domandaRating.id)
        Next

        For Each odomOpzione As DomandaOpzione In dom.domandaRating.opzioniRating
            DomandaRatingOpzioni_Insert(odomOpzione, dom.domandaRating.id)
        Next

        Return RetVal
    End Function

    Public Shared Function DomandaMeeting_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim sqlCommand2 As String = "sp_Questionario_DomandaRating_Insert"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.domandaRating.numeroMeeting)
        db.AddInParameter(dbCommand2, "mostraND", DbType.Boolean, dom.domandaRating.mostraND)
        db.AddInParameter(dbCommand2, "testoND", DbType.String, dom.domandaRating.testoND)
        db.AddInParameter(dbCommand2, "tipoIntestazione", DbType.Int32, dom.domandaRating.tipoIntestazione)
        db.AddOutParameter(dbCommand2, "idDomandaRating", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        If dom.domandaRating.opzioniRating.Count = 0 Then
            Dim odomandaVuota As New DomandaOpzione
            dom.domandaRating.opzioniRating.Add(odomandaVuota)
        End If

        dom.domandaRating.id = db.GetParameterValue(dbCommand2, "@idDomandaRating")
        For Each oDomIntest As DomandaOpzione In dom.domandaRating.intestazioniRating
            DomandaRatingIntestazioni_Insert(oDomIntest, dom.domandaRating.id)
        Next

        For Each odomOpzione As DomandaOpzione In dom.domandaRating.opzioniRating
            DomandaRatingOpzioni_Insert(odomOpzione, dom.domandaRating.id)
        Next
        Return RetVal
    End Function

    Public Shared Function DomandaRating_Update(ByRef dom As Domanda, ByVal isChiuso As Boolean) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_DomandaRating_Update"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand2, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand2, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand2, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand2, "peso", DbType.String, dom.peso)
        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.id)
        db.AddInParameter(dbCommand2, "difficolta", DbType.String, dom.difficolta)
        db.AddInParameter(dbCommand2, "idDomandaMultilingua", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "idDomandaRating", DbType.Int32, dom.domandaRating.id)
        If dom.tipo = Domanda.TipoDomanda.Meeting Then
            db.AddInParameter(dbCommand2, "numeroRating", DbType.Int32, dom.domandaRating.numeroMeeting)
        Else
            db.AddInParameter(dbCommand2, "numeroRating", DbType.Int32, dom.domandaRating.numeroRating)
        End If
        db.AddInParameter(dbCommand2, "mostraND", DbType.Boolean, dom.domandaRating.mostraND)
        db.AddInParameter(dbCommand2, "testoND", DbType.String, dom.domandaRating.testoND)
        db.AddInParameter(dbCommand2, "tipoIntestazione", DbType.Int32, dom.domandaRating.tipoIntestazione)
        db.AddInParameter(dbCommand2, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand2, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand2, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand2, "isValutabile", DbType.Boolean, dom.isValutabile)


        RetVal = db.ExecuteNonQuery(dbCommand2)

        If dom.domandaRating.opzioniRating.Count = 0 Then
            Dim odomandaVuota As New DomandaOpzione
            dom.domandaRating.opzioniRating.Add(odomandaVuota)
        End If

        If isChiuso Then
            For Each oDomIntest As DomandaOpzione In dom.domandaRating.intestazioniRating
                DomandaRatingIntestazioni_Update(oDomIntest, dom.domandaRating.id)
            Next
            For Each odomOpzione As DomandaOpzione In dom.domandaRating.opzioniRating
                DomandaRatingOpzioni_Update(odomOpzione, dom.domandaRating.id)
            Next

        Else
            DomandaRatingIntestazioni_Delete(dom, dom.domandaRating.id)
            For Each oDomIntest As DomandaOpzione In dom.domandaRating.intestazioniRating
                DomandaRatingIntestazioni_Insert(oDomIntest, dom.domandaRating.id)
            Next

            DomandaRatingOpzioni_Delete(dom, dom.domandaRating.id)
            For Each odomOpzione As DomandaOpzione In dom.domandaRating.opzioniRating
                DomandaRatingOpzioni_Insert(odomOpzione, dom.domandaRating.id)
            Next

        End If

        Return RetVal
    End Function

    Public Shared Function DomandaRatingOpzioni_Delete(ByRef dom As Domanda, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        'Dim oDomOpzione As New DomandaOpzione

        'For Each oDomOpzione In dom.domandaRating.opzioniRating
        Dim sqlCommand As String = "sp_Questionario_DomandaRatingOpzioni_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idDomandaRating", DbType.Int32, idDomandaRating)

        RetVal = db.ExecuteNonQuery(dbCommand)
        'Next

        Return RetVal
    End Function

    Public Shared Function DomandaRatingOpzioni_Insert(ByRef dom As DomandaOpzione, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand3 As String = "sp_Questionario_DomandaRatingOpzioni_Insert"
        Dim dbCommand3 As DbCommand = db.GetStoredProcCommand(sqlCommand3)

        db.AddInParameter(dbCommand3, "idDomandaRating", DbType.Int32, idDomandaRating)
        db.AddInParameter(dbCommand3, "testoMin", DbType.String, dom.testo)
        db.AddInParameter(dbCommand3, "testoMax", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand3, "isAltro", DbType.Boolean, dom.isAltro)
        db.AddInParameter(dbCommand3, "numeroOpzione", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand3, "arrayCBisVisible", DbType.String, dom.arrayCBisVisible)

        RetVal = db.ExecuteNonQuery(dbCommand3)

        Return RetVal
    End Function

    Public Shared Function DomandaRatingOpzioni_Update(ByRef dom As DomandaOpzione, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand3 As String = "sp_Questionario_DomandaRatingOpzioni_Update"
        Dim dbCommand3 As DbCommand = db.GetStoredProcCommand(sqlCommand3)

        db.AddInParameter(dbCommand3, "idDomandaRating", DbType.Int32, idDomandaRating)
        db.AddInParameter(dbCommand3, "testoMin", DbType.String, dom.testo)
        db.AddInParameter(dbCommand3, "testoMax", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand3, "isAltro", DbType.Boolean, dom.isAltro)
        db.AddInParameter(dbCommand3, "numeroOpzione", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand3, "idOpzione", DbType.Int32, dom.id)
        db.AddInParameter(dbCommand3, "arrayCBisVisible", DbType.String, dom.arrayCBisVisible)

        RetVal = db.ExecuteNonQuery(dbCommand3)

        Return RetVal
    End Function

    Public Shared Function DomandaRatingIntestazioni_Insert(ByRef dom As DomandaOpzione, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand3 As String = "sp_Questionario_DomandaRatingIntestazioni_Insert"
        Dim dbCommand3 As DbCommand = db.GetStoredProcCommand(sqlCommand3)

        db.AddInParameter(dbCommand3, "idDomandaRating", DbType.Int32, idDomandaRating)
        db.AddInParameter(dbCommand3, "indice", DbType.String, dom.numero)
        db.AddInParameter(dbCommand3, "testo", DbType.String, dom.testo)

        RetVal = db.ExecuteNonQuery(dbCommand3)

        Return RetVal
    End Function

    Public Shared Function DomandaRatingIntestazioni_Update(ByRef dom As DomandaOpzione, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand3 As String = "sp_Questionario_DomandaRatingIntestazioni_Update"
        Dim dbCommand3 As DbCommand = db.GetStoredProcCommand(sqlCommand3)

        db.AddInParameter(dbCommand3, "idDomandaRating", DbType.Int32, idDomandaRating)
        db.AddInParameter(dbCommand3, "indice", DbType.String, dom.numero)
        db.AddInParameter(dbCommand3, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand3, "idDomandaRatingIntestazione", DbType.Int32, dom.id)

        RetVal = db.ExecuteNonQuery(dbCommand3)

        Return RetVal
    End Function

    Public Shared Function DomandaRatingIntestazioni_Delete(ByRef dom As Domanda, ByVal idDomandaRating As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        'Dim oDomOpzione As New DomandaOpzione

        'For Each oDomOpzione In dom.domandaRating.opzioniRating
        Dim sqlCommand As String = "sp_Questionario_DomandaRatingIntestazioni_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idDomandaRating", DbType.Int32, idDomandaRating)

        RetVal = db.ExecuteNonQuery(dbCommand)
        'Next

        Return RetVal
    End Function

    Public Shared Function connectQuestionToSurvey(ByRef oQuestion As Domanda, ByRef IdSurvey As Integer, ByRef IdSurveySon As Integer) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String = "sp_Questionario_connectQuestionToSurvey"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, IdSurvey)
            db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, RootObject.setNullInt(IdSurveySon))
            db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oQuestion.id)
            db.AddInParameter(dbCommand, "numeroDomanda", DbType.Int32, oQuestion.numero)
            db.AddInParameter(dbCommand, "peso", DbType.Int32, oQuestion.peso)
            db.AddInParameter(dbCommand, "difficolta", DbType.Int32, oQuestion.difficolta)
            db.AddInParameter(dbCommand, "isObbligatorio", DbType.Boolean, oQuestion.isObbligatoria)
            db.AddInParameter(dbCommand, "isValutabile", DbType.Boolean, oQuestion.isValutabile)
            db.ExecuteNonQuery(dbCommand)

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Function Domanda_Insert(ByRef dom As Domanda) As Domanda

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_Domanda_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand, "tipo", DbType.Int32, dom.tipo)
        db.AddInParameter(dbCommand, "idPagina", DbType.Int32, dom.idPagina)
        db.AddInParameter(dbCommand, "numeroDomanda", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand, "peso", DbType.Int32, dom.peso)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, dom.idLingua)
        db.AddInParameter(dbCommand, "difficolta", DbType.Int32, dom.difficolta)
        db.AddInParameter(dbCommand, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand, "isValutabile", DbType.Boolean, dom.isValutabile)
        db.AddInParameter(dbCommand, "idPersonaCreator", DbType.Int32, dom.idPersonaCreator)
        db.AddInParameter(dbCommand, "dataCreazione", DbType.DateTime, setNullDate(dom.dataCreazione))
        db.AddInParameter(dbCommand, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddOutParameter(dbCommand, "idDomanda", DbType.Int32, 4)
        db.AddOutParameter(dbCommand, "idDomandaMultilingua", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand)

        dom.id = db.GetParameterValue(dbCommand, "idDomanda")
        dom.idDomandaMultilingua = db.GetParameterValue(dbCommand, "idDomandaMultilingua")

        Return dom
    End Function

    Public Shared Function DomandaMultilingua_Insert(ByRef dom As Domanda) As Domanda

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DomandaMultilingua_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomanda", DbType.String, dom.id)
        db.AddInParameter(dbCommand, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, dom.idLingua)
        db.AddOutParameter(dbCommand, "idDomandaMultilingua", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand)

        dom.idDomandaMultilingua = db.GetParameterValue(dbCommand, "idDomandaMultilingua")

        Return dom
    End Function

    Public Shared Function DomandaTestoLibero_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        If dom.opzioniTestoLibero.Count = 0 Then
            Dim odomandaVuota As New DomandaTestoLibero
            dom.opzioniTestoLibero.Add(odomandaVuota)
        End If

        For Each oDomTL As DomandaTestoLibero In dom.opzioniTestoLibero
            Dim sqlCommand2 As String = "sp_Questionario_DomandaTestoLibero_Insert_V2"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

            db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
            db.AddInParameter(dbCommand2, "numeroRighe", DbType.Int32, oDomTL.numeroRighe)
            db.AddInParameter(dbCommand2, "numeroColonne", DbType.Int32, oDomTL.numeroColonne)
            db.AddInParameter(dbCommand2, "etichetta", DbType.String, oDomTL.etichetta)
            db.AddInParameter(dbCommand2, "isSingleLine", DbType.Boolean, oDomTL.isSingleLine)
            db.AddInParameter(dbCommand2, "numero", DbType.String, oDomTL.numero)
            db.AddInParameter(dbCommand2, "peso", DbType.Decimal, oDomTL.peso)

            RetVal = db.ExecuteNonQuery(dbCommand2)
        Next

        Return RetVal
    End Function

    Public Shared Function DomandaTestoLiberoOpzioni_Update(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        If dom.opzioniTestoLibero.Count = 0 Then
            Dim odomandaVuota As New DomandaTestoLibero
            dom.opzioniTestoLibero.Add(odomandaVuota)
        End If


        For Each oDomTL As DomandaTestoLibero In dom.opzioniTestoLibero
            Dim sqlCommand2 As String = "sp_Questionario_DomandaTestoLiberoOpzioni_Update_V2"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

            db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
            db.AddInParameter(dbCommand2, "numeroRighe", DbType.Int32, oDomTL.numeroRighe)
            db.AddInParameter(dbCommand2, "numeroColonne", DbType.Int32, oDomTL.numeroColonne)
            db.AddInParameter(dbCommand2, "etichetta", DbType.String, oDomTL.etichetta)
            db.AddInParameter(dbCommand2, "isSingleLine", DbType.Boolean, oDomTL.etichetta)
            db.AddInParameter(dbCommand2, "numero", DbType.String, oDomTL.numero)
            db.AddInParameter(dbCommand2, "idOpzioneTestoLibero", DbType.String, oDomTL.id)
            db.AddInParameter(dbCommand2, "peso", DbType.Decimal, oDomTL.peso)

            RetVal = db.ExecuteNonQuery(dbCommand2)
        Next

        Return RetVal
    End Function

    Public Shared Function DomandaTestoLibero_Delete(ByVal idDomandaML As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim oDomOpzione As New DomandaOpzione

        Dim sqlCommand As String = "sp_Questionario_DomandaTestoLibero_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idDomandaML", DbType.Int32, idDomandaML)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function DomandaNumerica_Insert(ByRef dom As Domanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        If dom.opzioniNumerica.Count = 0 Then
            Dim odomandaVuota As New DomandaNumerica
            dom.opzioniNumerica.Add(odomandaVuota)
        End If

        For Each oDomNM As DomandaNumerica In dom.opzioniNumerica
            Dim sqlCommand2 As String = "sp_Questionario_DomandaNumerica_Insert"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
            db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
            db.AddInParameter(dbCommand2, "testoPrima", DbType.String, oDomNM.testoPrima)
            db.AddInParameter(dbCommand2, "testoDopo", DbType.String, oDomNM.testoDopo)
            db.AddInParameter(dbCommand2, "dimensione", DbType.Int32, oDomNM.dimensione)
            db.AddInParameter(dbCommand2, "rispostaCorretta", DbType.Double, oDomNM.rispostaCorretta)
            db.AddInParameter(dbCommand2, "numero", DbType.Int32, oDomNM.numero)
            db.AddInParameter(dbCommand2, "peso", DbType.Decimal, oDomNM.peso)

            RetVal = db.ExecuteNonQuery(dbCommand2)
        Next
        Return RetVal
    End Function

    Public Shared Function DomandaTestoLibero_Update(ByRef dom As Domanda, ByVal isChiuso As Boolean) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_DomandaTestoLibero_Update"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand2, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand2, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand2, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand2, "peso", DbType.String, dom.peso)
        db.AddInParameter(dbCommand2, "difficolta", DbType.String, dom.difficolta)
        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.id)
        db.AddInParameter(dbCommand2, "idDomandaMultilingua", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand2, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand2, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand2, "isValutabile", DbType.Boolean, dom.isValutabile)

        RetVal = db.ExecuteNonQuery(dbCommand2)


        If isChiuso Then
            DomandaTestoLiberoOpzioni_Update(dom)
        Else
            DomandaTestoLibero_Delete(dom.idDomandaMultilingua)
            DomandaTestoLibero_Insert(dom)
        End If


        Return RetVal
    End Function

    Public Shared Function DomandaNumerica_Update(ByRef dom As Domanda, ByVal isChiuso As Boolean) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_DomandaNumerica_Update"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "testo", DbType.String, dom.testo)
        db.AddInParameter(dbCommand2, "suggerimento", DbType.String, dom.suggerimento)
        db.AddInParameter(dbCommand2, "testoPrima", DbType.String, dom.testoPrima)
        db.AddInParameter(dbCommand2, "testoDopo", DbType.String, dom.testoDopo)
        db.AddInParameter(dbCommand2, "peso", DbType.String, dom.peso)
        db.AddInParameter(dbCommand2, "difficolta", DbType.String, dom.difficolta)
        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, dom.id)
        db.AddInParameter(dbCommand2, "idDomandaMultilingua", DbType.Int32, dom.idDomandaMultilingua)
        db.AddInParameter(dbCommand2, "idPersonaEditor", DbType.Int32, dom.idPersonaEditor)
        db.AddInParameter(dbCommand2, "dataModifica", DbType.DateTime, setNullDate(dom.dataModifica))
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, dom.idQuestionario)
        db.AddInParameter(dbCommand2, "numero", DbType.Int32, dom.numero)
        db.AddInParameter(dbCommand2, "isObbligatorio", DbType.Boolean, dom.isObbligatoria)
        db.AddInParameter(dbCommand2, "isValutabile", DbType.Boolean, dom.isValutabile)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        If dom.opzioniNumerica.Count = 0 Then
            Dim odomandaVuota As New DomandaNumerica
            dom.opzioniNumerica.Add(odomandaVuota)
        End If

        If isChiuso Then
            For Each oDomNM As DomandaNumerica In dom.opzioniNumerica
                Dim sqlCommand As String = "sp_Questionario_DomandaNumericaOpzioni_Update"
                Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, dom.idDomandaMultilingua)
                db.AddInParameter(dbCommand, "testoPrima", DbType.String, oDomNM.testoPrima)
                db.AddInParameter(dbCommand, "testoDopo", DbType.String, oDomNM.testoDopo)
                db.AddInParameter(dbCommand, "dimensione", DbType.Int32, oDomNM.dimensione)
                db.AddInParameter(dbCommand, "rispostaCorretta", DbType.Double, oDomNM.rispostaCorretta)
                db.AddInParameter(dbCommand, "numero", DbType.Int32, oDomNM.numero)
                db.AddInParameter(dbCommand, "idDomandaNumericaOpzione", DbType.Int32, oDomNM.id)
                db.AddInParameter(dbCommand, "peso", DbType.Decimal, oDomNM.peso)

                db.ExecuteNonQuery(dbCommand)
            Next
        Else
            DomandaNumerica_Delete(dom.idDomandaMultilingua)
            DomandaNumerica_Insert(dom)
        End If


        Return RetVal
    End Function

    Public Shared Function DomandaNumerica_Delete(ByVal idDomandaML As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Dim oDomOpzione As New DomandaOpzione

        Dim sqlCommand As String = "sp_Questionario_DomandaNumerica_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idDomandaML", DbType.Int32, idDomandaML)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function


    Public Shared Function Domanda_Delete(ByVal idQuest As String, ByVal numeroDomanda As String, ByRef idDomanda As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As Integer = 0
        Dim sqlCommand2 As String = "sp_Questionario_Domanda_Delete"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand2, "numeroDomanda", DbType.Int32, numeroDomanda)
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, idQuest)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        Return RetVal
    End Function


    Public Shared Function Domanda_DeleteSimple(ByVal idDomanda As String, ByVal idQuest As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_DomandaDeleteSimple"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, idQuest)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        Return RetVal
    End Function

    Public Shared Function DomandaMultilingua_Delete(ByVal idDomanda As Integer, ByVal idDomandaMultilingua As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_DomandaMultilingua_Delete"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand2, "idDomandaMultilingua", DbType.Int32, idDomandaMultilingua)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        Return RetVal
    End Function

    Public Shared Function CountDomandaInQuestionari(ByVal idDomanda As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_LKQuestionarioDomandaCount"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "IdDomanda", DbType.Int32, idDomanda)

        retVal = db.ExecuteScalar(dbCommand)
        Return retVal

    End Function

    'Public Shared Function DomandaQuestionarioLK_Delete(ByVal idDomanda As Integer, ByVal numeroDomanda As Integer, ByVal idQuestionario As Integer) As String

    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim RetVal As String = ""
    '    Dim sqlCommand2 As String = "sp_Questionario_LKQuestionarioDomanda_Delete"
    '    Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

    '    db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, idDomanda)
    '    db.AddInParameter(dbCommand2, "numeroDomanda", DbType.Int32, numeroDomanda)
    '    db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, idQuestionario)

    '    RetVal = db.ExecuteNonQuery(dbCommand2)

    '    Return RetVal
    'End Function

    Public Shared Function DomandaQuestionarioLK_Set_isOld(ByVal idDomanda As Integer, ByVal numeroDomanda As Integer, ByVal idQuestionario As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand2 As String = "sp_Questionario_LKQuestionarioDomanda_Set_isOld"
        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

        db.AddInParameter(dbCommand2, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand2, "numeroDomanda", DbType.Int32, numeroDomanda)
        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, idQuestionario)

        RetVal = db.ExecuteNonQuery(dbCommand2)

        Return RetVal
    End Function

    Public Shared Function CountDomandeLibreriaInQuestionari(ByVal idDomanda As Integer, ByVal idLibreria As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_CountDomandeLibreriaInQuestionari"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "IdDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "IdLibreria", DbType.Int32, idLibreria)

        retVal = db.ExecuteScalar(dbCommand)
        Return retVal

    End Function



    'Public Shared Function GetQuestionsForStatistics(ByVal items As List(Of QuestionAnswer), ByVal idLingua As Int16) As List(Of Domanda)
    '    Dim results As New List(Of Domanda)
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim conn As DbConnection = db.CreateConnection()

    '    items(0).Answers()
    '    For Each item As LazyAssociatedQuestion In items
    '        Dim question As New Domanda With {.id = item.Id, .idDomandaMultilingua = item.Languages.Where(Function(l) l.IdLanguage.Equals(idLingua)).Select(Function(l) l.Id).FirstOrDefault()}
    '        Select Case question.tipo
    '            Case Domanda.TipoDomanda.Multipla
    '                question.domandaMultiplaOpzioni = DALDomande.readDomandaMultiplaOpzioni(question, db, conn)
    '            Case Domanda.TipoDomanda.DropDown
    '                question.domandaDropDown = DALDomande.readDomandaDropDownBYIDDomanda(question, db, conn)
    '            Case Domanda.TipoDomanda.Rating
    '                question.domandaRating = DALDomande.readDomandaRatingByID(question.idDomandaMultilingua, db, conn)
    '            Case Domanda.TipoDomanda.Meeting
    '                question.domandaRating = DALDomande.readDomandaRatingByID(question.idDomandaMultilingua, db, conn)
    '            Case Domanda.TipoDomanda.TestoLibero
    '                question.opzioniTestoLibero = DALDomande.readDomandaTestoLiberoByID(question.idDomandaMultilingua, db, conn)
    '            Case Domanda.TipoDomanda.Numerica
    '                question.opzioniNumerica = DALDomande.readDomandaNumericaById(question.idDomandaMultilingua, db, conn)
    '        End Select
    '        results.Add(question)
    '    Next
    '    Return results
    'End Function

    'Public Shared Sub readDomandeByIdQuestionarioPadre(ByVal oQuestionario As Questionario, ByVal db As Database, ByVal conn As DbConnection)

    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand

    '    sqlCommand = "sp_Questionario_IdDomandeByQuestionarioPadre_Select"
    '    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '    dbCommand.Connection = conn
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.id)


    '    Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '        While sqlReader.Read()
    '            Dim oDomanda As New Domanda

    '            oDomanda.id = isNullInt(sqlReader.Item("LKQD_DMND_Id"))

    '            oDomanda = readDomandaById(oDomanda, db, conn)

    '            oQuestionario.pagine(0).domande.Add(oDomanda)
    '        End While
    '    End Using

    'End Sub

    'Public Shared Function readDomandaById(ByVal odomanda As Domanda, ByVal db As Database, ByVal conn As DbConnection) As Domanda

    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand

    '    sqlCommand = "sp_Questionario_DomandaByID_Select"
    '    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '    dbCommand.Connection = conn
    '    db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, odomanda.id)


    '    Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '        While sqlReader.Read()

    '            odomanda.testo = isNullString(sqlReader.Item("DMML_Testo"))
    '            odomanda.tipo = isNullInt(sqlReader.Item("DMND_Tipo"))
    '            odomanda.idLingua = isNullInt(sqlReader.Item("DMML_IdLingua"))
    '            odomanda.idDomandaMultilingua = isNullInt(sqlReader.Item("DMML_Id"))
    '            odomanda.testoPrima = isNullString(sqlReader.Item("DMML_TestoPrima"))
    '            odomanda.testoDopo = isNullString(sqlReader.Item("DMML_TestoDopo"))
    '            odomanda.domandaCount = isNullInt(sqlReader.Item("DMND_Count"))
    '            'odomanda = readCampiDomanda(odomanda, sqlReader)

    '        End While
    '    End Using

    '    Return odomanda
    'End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
