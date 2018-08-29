Imports COL_DataLayer

Namespace CL_permessi
	Public Class RuoloServizio
		Inherits ObjectBase

#Region "Public Property"
		Private _Ruolo As New COL_TipoRuolo
		Private _Permessi As New List(Of Permessi)
		Private _Permessi_Definiti As String
		Private _Permessi_Default As String
		Private _Permessi_Profilo As String
		Private _isAssociato As Boolean
		Private _isFromProfilo As Boolean
		Private _isDefault As Boolean
#End Region

#Region "Public Property"
		Public Property Ruolo() As COL_TipoRuolo
			Get
				Ruolo = _Ruolo
			End Get
			Set(ByVal Value As COL_TipoRuolo)
				_Ruolo = Value
			End Set
		End Property
		Public Property PermessiDisponibili() As List(Of Permessi)
			Get
				PermessiDisponibili = _Permessi
			End Get
			Set(ByVal Value As List(Of Permessi))
				_Permessi = Value
			End Set
		End Property
		Public Property IsAssociato() As Boolean
			Get
				Return _isAssociato
			End Get
			Set(ByVal value As Boolean)
				_isAssociato = value
			End Set
		End Property
		Public Property IsFromProfilo() As Boolean
			Get
				Return _isFromProfilo
			End Get
			Set(ByVal value As Boolean)
				_isFromProfilo = value
			End Set
		End Property
		Public Property IsDefault() As Boolean
			Get
				Return _isDefault
			End Get
			Set(ByVal value As Boolean)
				_isDefault = value
			End Set
		End Property
		Public Property Permessi_Default() As String
			Get
				Return _Permessi_Default
			End Get
			Set(ByVal value As String)
				_Permessi_Default = value
			End Set
		End Property
		Public Property Permessi_Definiti() As String
			Get
				Return _Permessi_Definiti
			End Get
			Set(ByVal value As String)
				_Permessi_Definiti = Value
			End Set
		End Property
		Public Property Permessi_Profilo() As String
			Get
				Return _Permessi_Profilo
			End Get
			Set(ByVal value As String)
				_Permessi_Profilo = Value
			End Set
		End Property

		Public ReadOnly Property PermessiAssociati(ByVal Filter As Show) As List(Of Permessi)
			Get
				Dim StringaPermessi As String = ""
				Dim oLista As New List(Of Permessi)

				Select Case Filter
					Case Show.ValueDefault
						StringaPermessi = Me._Permessi_Default
					Case Show.ValueDefiniti
						StringaPermessi = Me._Permessi_Definiti
					Case Show.ValueProfilo
						StringaPermessi = Me._Permessi_Profilo
				End Select
				Dim index As Integer = StringaPermessi.IndexOf("1", 0)
				While index >= 0
					Dim oPermesso As Permessi
					oPermesso = Me._Permessi.Find(New GenericPredicate(Of Permessi, Integer)(index, AddressOf Permessi.FindByPosizione))
					If Not IsNothing(oPermesso) Then
						oLista.Add(oPermesso)
					End If

					index = StringaPermessi.IndexOf("1", index + 1)
				End While

				Return oLista
			End Get
		End Property
		Public Enum Show
			ValueProfilo
			ValueDefault
			ValueDefiniti
		End Enum
#End Region

#Region "Metodi New"
		Sub New()
		End Sub
		Sub New(ByVal oRuolo As COL_TipoRuolo, ByVal iPermessi As List(Of Permessi))
			Me._Ruolo = oRuolo
			Me._Permessi = iPermessi
		End Sub
		Sub New(ByVal oRuolo As COL_TipoRuolo, ByVal iPermessi As List(Of Permessi), ByVal PermDefiniti As String, ByVal PermDefault As String, ByVal PermProfilo As String, ByVal isAssociato As Boolean, ByVal isDefault As Boolean, ByVal isFromProfilo As Boolean)
			Me._Ruolo = oRuolo
			Me._Permessi = iPermessi
			Me.Permessi_Definiti = PermDefiniti
			Me._Permessi_Default = PermDefault
			Me._Permessi_Profilo = PermProfilo
			Me._isAssociato = IsAssociato
			Me._isDefault = isDefault
			Me._isFromProfilo = isFromProfilo
		End Sub
#End Region

		'Public Shared Function List(ByVal ServizioID As Integer, ByVal LinguaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of RuoliServizio)
		'	Dim oLista As New List(Of RuoliServizio)
		'	Dim cacheKey As String = CachePolicy.RuoliServizio(ServizioID, LinguaID)

		'	If sortDirection <> String.Empty Then
		'		sortDirection = sortDirection.ToLower
		'	End If

		'	If ObjectBase.Cache(cacheKey) Is Nothing Then
		'		oLista = PermessiServizio.RetrieveListFromDB()
		'		ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaMensile)
		'	Else
		'		oLista = CType(ObjectBase.Cache(cacheKey), List(Of PermessiServizio))
		'	End If


		'	If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
		'		oLista.Sort(New GenericComparer(Of PermessiServizio)(sortExpression))
		'	End If

		'	If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
		'		oLista.Reverse()
		'	End If
		'	Return oLista
		'End Function
		'Private Shared Function RetrieveListFromDB() As List(Of PermessiServizio)
		'	Dim oLista As New List(Of PermessiServizio)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	Dim oDatareader As IDataReader

		'	With oRequest
		'		.Command = "sp_Provincia_Elenca"
		'		.CommandType = CommandType.StoredProcedure

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With

		'	Try
		'		oDatareader = objAccesso.GetdataReader(oRequest)
		'		While oDatareader.Read
		'			Try
		'				oLista.Add(New PermessiServizio(oDatareader("PRVN_Id"), GenericValidator.ValString(oDatareader("PRVN_nome"), ""), GenericValidator.ValString(oDatareader("PRVN_sigla"), "")))
		'			Catch ex As Exception

		'			End Try
		'		End While
		'		oDatareader.Close()
		'	Catch ex As Exception
		'	Finally
		'		If Not IsNothing(oDatareader) Then
		'			If oDatareader.IsClosed = False Then
		'				oDatareader.Close()
		'			End If
		'		End If
		'	End Try
		'	Return oLista
		'End Function

		Public Shared Function FindByRole(ByVal item As RuoloServizio, ByVal argument As Integer) As Boolean
			Return IIf(IsNothing(item.Ruolo), False, item.Ruolo.Id = argument)
		End Function

       
	End Class

End Namespace