Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data
Imports COL_Questionario.RootObject
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public Class DALExternal

    ''' <summary>
    ''' non usare, usare WCF
    ''' </summary>
    ''' <param name="ownerType"></param>
    ''' <param name="ownerId"></param>
    ''' <param name="idPerson"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EduPathCanCreate(ByVal CurrentContext As Global.lm.Comol.Core.DomainModel.iApplicationContext, ByVal ownerType As Integer, ByVal ownerId As Int64, ByVal idPerson As Integer, ByRef questType As Integer, ByRef idRole As Integer) As Boolean
        Dim serviceEP As New Service(CurrentContext)
        Dim oPermissionEP As Global.lm.Comol.Modules.EduPath.Domain.PermissionEP = serviceEP.GetUserPermission_ByActivity(ownerId, idPerson, idRole, False)
        Return oPermissionEP.Create
    End Function
    Public Function EduPathCanEvaluate(ByVal CurrentContext As Global.lm.Comol.Core.DomainModel.iApplicationContext, ByVal ownerType As Integer, ByVal ownerId As Int64, ByVal idPerson As Integer, ByRef questType As Integer, ByRef idRole As Integer) As Boolean
        Dim serviceEP As New Service(CurrentContext)
        Dim oPermissionEP As Global.lm.Comol.Modules.EduPath.Domain.PermissionEP = serviceEP.GetUserPermission_ByActivity(ownerId, idPerson, idRole)
        Return oPermissionEP.Evaluate
    End Function
    'Public Shared Function EduPathCanCompile(ByVal CurrentContext As Global.lm.Comol.Core.DomainModel.iApplicationContext, ByVal ownerType As Integer, ByVal ownerId As Int64, ByVal idPerson As Integer, ByRef questType As Integer, ByRef idRole As Integer) As Boolean


    'Dim serviceEP As New Service(CurrentContext)
    'Dim oPermissionEP As Global.lm.Comol.Modules.EduPath.Domain.PermissionEP = serviceEP.GetUserPermission_ByActivity(ownerId, idPerson, idRole)
    'Return oPermissionEP.Read



    'If questType = Questionario.TipoQuestionario.Questionario OrElse questType = Questionario.TipoQuestionario.Random OrElse questType = Questionario.TipoQuestionario.Autovalutazione Then
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim oUtenti As New List(Of UtenteInvitato)
    '    Dim dbCommand As DbCommand
    '    Dim sql As String = EduPathComposeSQL(ownerType, ownerId, idPerson, questType, 1)
    '    dbCommand = db.GetSqlStringCommand(sql)
    '    If db.ExecuteScalar(dbCommand) > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'Else
    '    Return False
    'End If
    'End Function

    'Private Shared Function EduPathComposeSQL(ByVal ownerType As Integer, ByVal ownerId As Int64, ByVal idPerson As Integer, ByRef questType As Integer, ByRef EPRole As Integer) As String
    '    Dim sql As String
    '    Select Case ownerType
    '        Case OwnerType_enum.EducationalPath
    '            sql = "SELECT   count(*) FROM EP_Assignment WHERE (Role = " & EPRole & ") AND (IdPath = " & ownerId & ") AND (IdPerson = " & idPerson & ")"
    '        Case OwnerType_enum.EduPathUnit
    '            sql = "SELECT   count(*) FROM EP_Assignment WHERE (Role = " & EPRole & ") AND (IdUnit = " & ownerId & ") AND (IdPerson = " & idPerson & ")"
    '        Case OwnerType_enum.EduPathActivity
    '            sql = "SELECT   count(*) FROM EP_Assignment WHERE (Role = " & EPRole & ") AND (IdActivity = " & ownerId & ") AND (IdPerson = " & idPerson & ")"
    '        Case Else
    '            Return False
    '    End Select
    '    Return sql
    'End Function
End Class
