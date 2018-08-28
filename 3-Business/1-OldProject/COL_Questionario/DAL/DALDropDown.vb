Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Data
Imports COL_Questionario.RootObject

Public Class DALDropDown

    Public Shared Function DropDown_Insert(ByRef drop As DropDown) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_DropDown_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "nome", DbType.String, drop.nome)
        db.AddInParameter(dbCommand, "etichetta", DbType.String, drop.etichetta)
        db.AddInParameter(dbCommand, "ordinata", DbType.Boolean, drop.ordinata)
        db.AddInParameter(dbCommand, "tipo", DbType.Int32, drop.tipo)
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, drop.idDomanda)
        db.AddOutParameter(dbCommand, "idDropDown", DbType.Int32, 4)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Dim idDropDown As Integer = db.GetParameterValue(dbCommand, "idDropDown")


        Dim oItem As New DropDownItem

        For Each oItem In drop.dropDownItems
            Dim sqlCommand2 As String = "sp_Questionario_DropDownItem_Insert"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

            db.AddInParameter(dbCommand2, "idDropDown", DbType.Int32, idDropDown)
            db.AddInParameter(dbCommand2, "testo", DbType.String, oItem.testo)
            db.AddInParameter(dbCommand2, "suggestion", DbType.String, oItem.suggestion)
            db.AddInParameter(dbCommand2, "valore", DbType.String, oItem.numero)
            db.AddInParameter(dbCommand2, "indice", DbType.String, oItem.indice)
            db.AddInParameter(dbCommand2, "peso", DbType.Decimal, oItem.peso)
            db.AddInParameter(dbCommand2, "isCorretta", DbType.Boolean, oItem.isCorretta)
            RetVal = db.ExecuteNonQuery(dbCommand2)

        Next

        RetVal = idDropDown

        Return RetVal
    End Function

    Public Shared Function readDropDownItems(ByVal oDropDown As DropDown) As list(Of DropDownItem)
        ' leggo gli items della drop down

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_DropDownItemByIDDropDown_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDropDown", DbType.Int32, oDropDown.id)
        db.AddInParameter(dbCommand, "ordina", DbType.Boolean, oDropDown.ordinata)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim listItem As New list(Of DropDownItem)

        While sqlReader.Read()
            Dim oDropDownItem As New DropDownItem
            oDropDownItem.id = isNullInt(sqlReader.Item("DRIT_Id"))
            oDropDownItem.idDropDown = isNullInt(sqlReader.Item("DRIT_DROP_Id"))
            oDropDownItem.indice = isNullInt(sqlReader.Item("DRIT_Indice"))
            oDropDownItem.testo = isNullString(sqlReader.Item("DRIT_Testo"))
            oDropDownItem.suggestion = isNullString(sqlReader.Item("DRIT_Suggestion"))
            oDropDownItem.numero = isNullInt(sqlReader.Item("DRIT_Valore"))
            oDropDownItem.peso = isNullDecimal(sqlReader.Item("DRIT_Peso"))
            oDropDownItem.isCorretta = isNullBoolean(sqlReader.Item("DRIT_isCorretta"))

            listItem.Add(oDropDownItem)
        End While
        sqlReader.Close()
        Return listItem
    End Function

End Class
