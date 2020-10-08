Public Class MappingClass
    Private itemType As String
    Private dbName As String
    Private disName As String

    Property Type() As String
        Get
            Return itemType
        End Get
        Set(ByVal value As String)
            itemType = value
        End Set
    End Property
    Property DatabaseName() As String
        Get
            Return dbName
        End Get
        Set(ByVal value As String)
            dbName = value
        End Set
    End Property
    Property DisplayName() As String
        Get
            Return disName
        End Get
        Set(ByVal value As String)
            disName = value
        End Set
    End Property
End Class
