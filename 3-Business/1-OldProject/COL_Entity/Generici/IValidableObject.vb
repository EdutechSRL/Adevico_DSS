Imports System
Imports System.Collections.Generic
Imports System.Text
Imports ManagedDesigns.ApplicationBlocks.Validation


Interface IValidableObject
	ReadOnly Property IsValid() As Boolean
	Function Validate() As IList(Of ValidationError)
End Interface
