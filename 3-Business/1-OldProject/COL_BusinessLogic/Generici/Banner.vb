Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita

Public Class COL_Banner

    Private n_errore As Errori_Db

    Public Function HasUserRead(ByVal PersonaID As Integer, ByVal QuestionarioID As Integer) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        'Dim Totale As Integer = 0

        With oRequest
            .Command = "sp_Questionario_CountUserQuestion"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@QSTN_Id", QuestionarioID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)


            oParam = objAccesso.GetAdvancedParameter("@Totale", 0, ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)


            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)

            If oRequest.GetValueFromParameter(3) > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False


        Return False
    End Function

    Public Sub SetUser(ByVal PersonaID As Integer, ByVal QuestionarioID As Integer)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        '@PRSN_Id int,
        '@QSTN_Id int

        With oRequest
            .Command = "sp_Questionario_InsertUser" '###
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@QSTN_Id", QuestionarioID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_errore = Errori_Db.None
        Catch ex As Exception
            Me.n_errore = Errori_Db.DBInsert
            iResponse = -1
        End Try

    End Sub

End Class
