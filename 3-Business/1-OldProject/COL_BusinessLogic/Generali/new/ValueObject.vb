'public abstract class ValueObject<T> : IEquatable<T>
'    where T : ValueObject<T>
'{
'    public override bool Equals(object obj)
'    {
'        if (obj == null)
'            return false;

'        T other = obj as T;

'        return Equals(other);
'    }

'    public override int GetHashCode()
'    {
'        IEnumerable<FieldInfo> fields = GetFields();

'        int startValue = 17;
'        int multiplier = 59;

'        int hashCode = startValue;

'        foreach (FieldInfo field in fields)
'        {
'            object value = field.GetValue(this);

'            if (value != null)
'                hashCode = hashCode * multiplier + value.GetHashCode();
'        }

'        return hashCode;
'    }

'    public virtual bool Equals(T other)
'    {
'        if (other == null)
'            return false;

'        Type t = GetType();
'        Type otherType = other.GetType();

'        if (t != otherType)
'            return false;

'        FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

'        foreach (FieldInfo field in fields)
'        {
'            object value1 = field.GetValue(other);
'            object value2 = field.GetValue(this);

'            if (value1 == null)
'            {
'                if (value2 != null)
'                    return false;
'            }
'            else if (! value1.Equals(value2))
'                return false;
'        }

'        return true;
'    }

'    private IEnumerable<FieldInfo> GetFields()
'    {
'        Type t = GetType();

'        List<FieldInfo> fields = new List<FieldInfo>();

'        while (t != typeof(object))
'        {
'            fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

'            t = t.BaseType;
'        }

'        return fields;
'    }

'    public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
'    {
'        return x.Equals(y);
'    }

'    public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
'    {
'        return ! (x == y);
'    }
'}


Imports System.Reflection
Imports System.Collections
Imports System.Collections.Generic

<Serializable()>
Public MustInherit Class ValueObject(Of T As ValueObject(Of T))
    Implements IEquatable(Of T)


    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If

        Dim other As T = TryCast(obj, T)

        Return Equals(other)
    End Function

    Public Overridable Overloads Function Equals(ByVal other As T) As Boolean Implements System.IEquatable(Of T).Equals
        If other Is Nothing Then
            Return False
        End If

        Dim t As Type = Me.GetType()
        Dim otherType As Type = other.GetType()

        If t IsNot otherType Then
            Return False
        End If

        Dim fields As FieldInfo() = t.GetFields(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.[Public])

        For Each field As FieldInfo In fields
            Dim value1 As Object = field.GetValue(other)
            Dim value2 As Object = field.GetValue(Me)

            If value1 Is Nothing Then
                If value2 IsNot Nothing Then
                    Return False
                End If
            ElseIf Not value1.Equals(value2) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function GetFields() As IEnumerable(Of FieldInfo)
        Dim t As Type = Me.GetType()

        Dim fields As New List(Of FieldInfo)()

        While t IsNot GetType(Object)
            fields.AddRange(t.GetFields(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.[Public]))

            t = t.BaseType
        End While

        Return fields
    End Function

    Public Shared Operator =(ByVal x As ValueObject(Of T), ByVal y As ValueObject(Of T)) As Boolean
        Return x.Equals(y)
    End Operator

    Public Shared Operator <>(ByVal x As ValueObject(Of T), ByVal y As ValueObject(Of T)) As Boolean
        Return Not (x = y)
    End Operator

End Class