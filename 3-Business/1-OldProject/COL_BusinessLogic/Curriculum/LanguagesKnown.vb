Imports COL_DataLayer
Namespace Curriculum_Europeo
    Public Class COL_ConoscenzaLingua

#Region "Private Property"
        Private n_CNLN_id As Integer
        Private n_CNLN_nome As String
        Private n_CNLN_lettura As Integer
        Private n_CNLN_scrittura As Integer
        Private n_CNLN_espressioneOrale As Integer
        Private n_CNLN_CREU_id As Integer
        Private n_CNLN_rendiPubblico As Integer
        Private n_ErroreDB As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_CNLN_id
            End Get
            Set(ByVal Value As Integer)
                n_CNLN_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_CNLN_nome
            End Get
            Set(ByVal Value As String)
                n_CNLN_nome = Value
            End Set
        End Property
        Public Property Lettura() As Integer
            Get
                Lettura = n_CNLN_lettura
            End Get
            Set(ByVal Value As Integer)
                n_CNLN_lettura = Value
            End Set
        End Property
        Public Property Scrittura() As Integer
            Get
                Scrittura = n_CNLN_scrittura
            End Get
            Set(ByVal Value As Integer)
                n_CNLN_scrittura = Value
            End Set
        End Property
        Public Property EspressioneOrale() As Integer
            Get
                EspressioneOrale = n_CNLN_espressioneOrale
            End Get
            Set(ByVal Value As Integer)
                n_CNLN_espressioneOrale = Value
            End Set
        End Property
        Public Property Curriculum_id() As Integer
            Get
                Curriculum_id = n_CNLN_CREU_id
            End Get
            Set(ByVal Value As Integer)
                n_CNLN_CREU_id = Value
            End Set
        End Property
        Public Property RendiPubblico() As Boolean
            Get
                RendiPubblico = (Me.n_CNLN_rendiPubblico = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_CNLN_rendiPubblico = 1
                Else
                    n_CNLN_rendiPubblico = 0
                End If
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_ErroreDB
            End Get
        End Property
#End Region
        Sub New()
            Me.n_ErroreDB = Errori_Db.None
        End Sub

#Region "Metodi Standard"

        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CNLN_id", Me.n_CNLN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_lettura", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_scrittura", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_espressioneOrale", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CNLN_nome = oRequest.GetValueFromParameter(2)
                Me.n_CNLN_lettura = oRequest.GetValueFromParameter(3)
                Me.n_CNLN_scrittura = oRequest.GetValueFromParameter(4)
                Me.n_CNLN_espressioneOrale = oRequest.GetValueFromParameter(5)
                Me.n_CNLN_CREU_id = oRequest.GetValueFromParameter(6)
                Me.n_CNLN_rendiPubblico = oRequest.GetValueFromParameter(7)

                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
                responso = -1
            End Try
        End Sub

        Public Sub EstraiByPersona(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_EstraiByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CNLN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_lettura", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_scrittura", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_espressioneOrale", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(1) <> "" Then
                    Me.n_CNLN_id = oRequest.GetValueFromParameter(1)
                    Me.n_CNLN_nome = oRequest.GetValueFromParameter(2)
                    Me.n_CNLN_lettura = oRequest.GetValueFromParameter(3)
                    Me.n_CNLN_scrittura = oRequest.GetValueFromParameter(4)
                    Me.n_CNLN_espressioneOrale = oRequest.GetValueFromParameter(5)
                    Me.n_CNLN_CREU_id = oRequest.GetValueFromParameter(6)
                    Me.n_CNLN_rendiPubblico = oRequest.GetValueFromParameter(7)
                    Me.n_ErroreDB = Errori_Db.None
                Else
                    Me.n_CNLN_id = -1
                    responso = -1
                    Me.n_ErroreDB = Errori_Db.DBReadExist
                End If

            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
                responso = -1
            End Try
        End Sub

        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CNLN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_nome", Me.n_CNLN_nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_lettura", Me.n_CNLN_lettura, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_scrittura", Me.n_CNLN_scrittura, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_espressioneOrale", Me.n_CNLN_espressioneOrale, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_CREU_id", Me.n_CNLN_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_rendiPubblico", Me.n_CNLN_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CNLN_id = oRequest.GetValueFromParameter(1)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_CNLN_id = -1
                Me.n_ErroreDB = Errori_Db.DBInsert
                responso = -1
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_Modifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CNLN_id", Me.n_CNLN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_nome", Me.n_CNLN_nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_lettura", Me.n_CNLN_lettura, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_scrittura", Me.n_CNLN_scrittura, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_espressioneOrale", Me.n_CNLN_espressioneOrale, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_CREU_id", Me.n_CNLN_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CNLN_rendiPubblico", Me.n_CNLN_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBChange
                responso = -1
            End Try

        End Sub
        Public Function Elimina(ByVal CNLN_id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CNLN_id", CNLN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

#End Region

        Public Function GetNumeroLingue(ByVal CREU_id As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NumLingue As Integer

            With oRequest
                .Command = "sp_ConoscenzaLingua_Getnumero"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CREU_id", CREU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
           
                oParam = objAccesso.GetParameter("@NumLingue", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NumLingue = oRequest.GetValueFromParameter(2)
                Me.n_ErroreDB = Errori_Db.None
            Catch ax As Exception 'record non trovato
                NumLingue = 0
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
            Return NumLingue
        End Function

        Public Function ElencaByPersona(ByVal PRSN_id As Integer, Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ConoscenzaLingua_ElencaByPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

    End Class

End Namespace