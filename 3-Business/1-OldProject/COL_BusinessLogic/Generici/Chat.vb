Imports COL_DataLayer
Imports COL_BusinessLogic_v2.CL_permessi


Public Class Chat

    Public Shared Function GetComunitaByUtente(ByVal Utente_ID As Integer, Optional ByVal Perm1 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm2 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm3 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm4 As MyServices.PermissionType = MyServices.PermissionType.None) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Persona_EstraiCMNT_SRV" '!!! <-- Da mettere a posto quando tutto funziona...
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Utente_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@SRVZ_Code", Services_CHAT.Codex, ParameterDirection.Input, SqlDbType.VarChar, True, 15)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@permesso1", CType(Perm1, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@permesso2", CType(Perm2, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@permesso3", CType(Perm3, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@permesso4", CType(Perm4, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            'La riga sotto e' momentaneamente superflua
            oParam = objAccesso.GetAdvancedParameter("@forAND", 0, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception

        End Try
        Return oDataSet
    End Function
End Class
