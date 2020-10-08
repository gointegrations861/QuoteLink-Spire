Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Web

Public Class SpireAPIContainer
    Private url As Uri
    Private username As String
    Private password As String
    Private auth As String

    Sub New()
        username = ""
        password = ""
        auth = ""
    End Sub

    Public Function Init(ByVal newurl As String, ByVal newusername As String, ByVal newpassword As String) As Boolean
        If newurl.Length > 0 And newusername.Length > 0 And newpassword.Length > 0 Then
            Try
                Dim newauth = "Basic " & Convert.ToBase64String(Encoding.Default.GetBytes(newusername & ":" & newpassword))
                Dim request = CType(WebRequest.Create(newurl), HttpWebRequest)
                request.Headers.Set("Authorization", newauth)
                Using response = CType(request.GetResponse(), HttpWebResponse)
                    'Dim sr = New StreamReader(response.GetResponseStream)
                    If response.StatusCode = HttpStatusCode.OK Then
                        url = New Uri(newurl)
                        username = newusername
                        password = newpassword
                        auth = newauth
                        Return True
                    End If
                End Using
            Catch ex As Exception
            End Try
        End If

        Return False
    End Function

    Public Function FindCustomer(ByVal CustomerID As String)
        If CustomerID.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "customers/?filter=" & HttpUtility.UrlEncode(String.Format("{{""customerNo"":""{0}""}}", CustomerID))))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse()
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function GetCustomer(ByVal CustomerID As String)
        If CustomerID.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "customers/" & CustomerID))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse()
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function AddCustomer(ByRef custObj As Object) As String
        Dim jsonstring = Helpers.Json.Encode(custObj)
        Dim requeststring = "_content_type=" & HttpUtility.UrlEncode("application/json") & "&_content=" & HttpUtility.UrlEncode(jsonstring)
        Dim requestbytearray = Encoding.UTF8.GetBytes(requeststring)
        Dim request = WebRequest.Create(New Uri(url, "customers/"))
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = requestbytearray.Length
        request.Headers.Add("Authorization", auth)
        Using datastream = request.GetRequestStream()
            datastream.Write(requestbytearray, 0, requestbytearray.Length)
        End Using

        Try
            Using response = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.Created Then
                    Dim location = response.GetResponseHeader("location").Split(New Char() {"/"c})
                    Return location(location.Length - 1)
                Else
                    MessageBox.Show("Error Adding Customer, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    My.Application.Log.WriteEntry("Error Adding Customer", TraceEventType.Critical)
                    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
                    Return Nothing
                End If
            End Using
        Catch ex As WebException
            Using response = CType(ex.Response(), HttpWebResponse)
                Using datastream = response.GetResponseStream()
                    Using reader = New StreamReader(datastream)
                        Dim message = reader.ReadToEnd()
                        MessageBox.Show("Error creating customer: " + message.Trim(""""), "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        My.Application.Log.WriteEntry("Error creating customer: " + message.Trim(""""), TraceEventType.Critical)
                    End Using
                End Using
            End Using
            My.Application.Log.WriteException(ex)
            My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            Return Nothing
        End Try
    End Function

    Public Function FindItem(ByVal whse As String, ByVal partno As String)
        If whse.Length > 0 And partno.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "inventory/items/?filter=" & HttpUtility.UrlEncode(String.Format("{{""whse"":""{0}"",""partNo"":""{1}""}}", whse, partno))))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse()
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim returnString = sr.ReadToEnd
                returnString = returnString.Replace("""allowBackOrders"":true,", "").Replace("""allowBackOrders"":false,", "")
                Dim jsonobj = Helpers.Json.Decode(returnString)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function GetItem(ByVal ItemID As String)
        If ItemID.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "inventory/items/" & ItemID))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function AddItem(ByRef itemObj As Object) As String
        Dim jsonstring = Helpers.Json.Encode(itemObj)
        My.Application.Log.WriteEntry("Adding Item", TraceEventType.Information)
        My.Application.Log.WriteEntry(jsonstring, TraceEventType.Information)
        Dim requeststring = "_content_type=" & HttpUtility.UrlEncode("application/json") & "&_content=" & HttpUtility.UrlEncode(jsonstring)
        Dim requestbytearray = Encoding.UTF8.GetBytes(requeststring)
        Dim request = WebRequest.Create(New Uri(url, "inventory/items/"))
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = requestbytearray.Length
        request.Headers.Add("Authorization", auth)
        Using datastream = request.GetRequestStream()
            datastream.Write(requestbytearray, 0, requestbytearray.Length)
        End Using

        Try
            Using response = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.Created Then
                    Dim location = response.GetResponseHeader("location").Split(New Char() {"/"c})
                    Return location(location.Length - 1)
                Else
                    MessageBox.Show("Error Adding Item, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    My.Application.Log.WriteEntry("Error Adding Item", TraceEventType.Critical)
                    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
                    Return Nothing
                End If
            End Using
        Catch ex As WebException
            Using response = CType(ex.Response(), HttpWebResponse)
                Using datastream = response.GetResponseStream()
                    Using reader = New StreamReader(datastream)
                        Dim message = reader.ReadToEnd()
                        MessageBox.Show("Error creating item: " + message.Trim(""""), "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        My.Application.Log.WriteEntry("Error creating item: " + message.Trim(""""), TraceEventType.Critical)
                    End Using
                End Using
            End Using
            My.Application.Log.WriteException(ex)
            My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            Return Nothing
        End Try
    End Function

    Public Function UpdateItem(ByVal itemID As String, ByRef itemObj As Object)
        Dim jsonstring = Helpers.Json.Encode(itemObj)
        Try
            Dim requeststring = "_content_type=" & HttpUtility.UrlEncode("application/json") & "&_content=" & HttpUtility.UrlEncode(jsonstring) & "&_method=PUT"
            Dim requestbytearray = Encoding.UTF8.GetBytes(requeststring)
            Dim request = WebRequest.Create(New Uri(url, "inventory/items/" & itemID))
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = requestbytearray.Length
            request.Headers.Add("Authorization", auth)
            Using datastream = request.GetRequestStream()
                datastream.Write(requestbytearray, 0, requestbytearray.Length)
            End Using


            Using response = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    Return True
                Else
                    MessageBox.Show("Error Updating Item Price, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    My.Application.Log.WriteEntry("Error Updating Item Price - " & itemID, TraceEventType.Critical)
                    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error Updating Item Price, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            My.Application.Log.WriteEntry("Error Updating Item Price - " & itemID, TraceEventType.Critical)
            My.Application.Log.WriteException(ex)
            My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            Return Nothing
        End Try
    End Function

    Public Function GetAllItems(ByVal whse As String, ByVal start As Integer, ByVal limit As Integer)
        Dim request = WebRequest.Create(New Uri(url, String.Format("inventory/items/?limit={0}&start={1}&sort=partNo&filter={{""whse"":""{2}""}}", limit.ToString(), start.ToString(), whse)))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream())
            Dim returnString = sr.ReadToEnd
            returnString = returnString.Replace("""allowBackOrders"":true,", "").Replace("""allowBackOrders"":false,", "")
            Dim jsonobj = Helpers.Json.Decode(returnString)
            Return jsonobj
        End Using

    End Function

    Public Function GetSalespeople()
        Dim request = WebRequest.Create(New Uri(url, "salespeople/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
            Return jsonobj
        End Using
    End Function

    Public Function FindWhse(ByVal whse As String)
        If whse.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "inventory/warehouses/?filter=" & HttpUtility.UrlEncode(String.Format("{{""code"":""{0}""}}", whse))))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse()
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function GetWarehouses()
        Dim request = WebRequest.Create(New Uri(url, "inventory/warehouses/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function GetSalesTaxes()
        Dim request = WebRequest.Create(New Uri(url, "sales_taxes/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function GetCurrencies()
        Dim request = WebRequest.Create(New Uri(url, "currencies/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function GetShipTo(ByVal shipid As String, ByVal custid As String)
        Dim request = WebRequest.Create(New Uri(url, "addresses/?filter=" & HttpUtility.UrlEncode(String.Format("{{""recordType"":""CUST"",""shipId"":""{0}"",""linkNo"":""{1}""}}", shipid, custid))))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function AddShipTo(ByVal custid As String, ByRef shipObj As Object) As String
        Dim jsonstring = Helpers.Json.Encode(shipObj)
        Dim requeststring = "_content_type=" & HttpUtility.UrlEncode("application/json") & "&_content=" & HttpUtility.UrlEncode(jsonstring) & "&_method=PUT"
        Dim requestbytearray = Encoding.UTF8.GetBytes(requeststring)
        Dim request = WebRequest.Create(New Uri(url, "customers/" & custid))
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = requestbytearray.Length
        request.Headers.Add("Authorization", auth)
        Dim datastream = request.GetRequestStream()
        datastream.Write(requestbytearray, 0, requestbytearray.Length)
        datastream.Close()

        Try
            Using response = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    'Doesn't return status 201 like other modules
                    Return "OK"
                Else
                    MessageBox.Show("Error Adding A Ship-To, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    My.Application.Log.WriteEntry("Error Adding Ship-to", TraceEventType.Critical)
                    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
                    Return Nothing
                End If
            End Using
            'If response.StatusCode = HttpStatusCode.Created Then
            '    Dim location = response.GetResponseHeader("location").Split("/")
            '    Return location(location.Length - 1)
            'Else
            '    MessageBox.Show("Error Adding A Ship-To, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    My.Application.Log.WriteEntry("Error Adding Ship-to", TraceEventType.Critical)
            '    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            '    Return Nothing
            'End If
        Catch ex As Exception
            MessageBox.Show("Error Adding A Ship-To, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            My.Application.Log.WriteEntry("Error Adding Ship-to", TraceEventType.Critical)
            My.Application.Log.WriteException(ex)
            My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            Return Nothing
        End Try
    End Function

    Public Function AddOrder(ByRef custObj As Object) As String
        Dim jsonstring = Helpers.Json.Encode(custObj)
        'MessageBox.Show(jsonstring)
        My.Application.Log.WriteEntry("Adding Order", TraceEventType.Information)
        My.Application.Log.WriteEntry(jsonstring, TraceEventType.Information)
        Dim requeststring = "_content_type=" & HttpUtility.UrlEncode("application/json") & "&_content=" & HttpUtility.UrlEncode(jsonstring)
        Dim requestbytearray = Encoding.UTF8.GetBytes(requeststring)
        Dim request = WebRequest.Create(New Uri(url, "sales/orders/"))
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = requestbytearray.Length
        request.Headers.Add("Authorization", auth)
        Using datastream = request.GetRequestStream()
            datastream.Write(requestbytearray, 0, requestbytearray.Length)
        End Using
        Try
            Using response = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.Created Then
                    Dim location = response.GetResponseHeader("location").Split(New Char() {"/"c})
                    Return location(location.Length - 1)
                Else
                    MessageBox.Show("Error Adding The Order, Data Has Been Logged", "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    My.Application.Log.WriteEntry("Error Adding Order", TraceEventType.Critical)
                    My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
                    Return Nothing
                End If
            End Using
        Catch ex As WebException
            Using response = CType(ex.Response(), HttpWebResponse)
                Using datastream = response.GetResponseStream()
                    Using reader = New StreamReader(datastream)
                        Dim message = reader.ReadToEnd()
                        MessageBox.Show("Error during transfer: " + message.Trim(""""), "Spire API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        My.Application.Log.WriteEntry("Error Adding Order: " + message.Trim(""""), TraceEventType.Critical)
                    End Using
                End Using
            End Using
            My.Application.Log.WriteException(ex)
            My.Application.Log.WriteEntry(jsonstring, TraceEventType.Critical)
            Return Nothing
        End Try
    End Function

    Public Function GetOrder(ByVal orderid As String)
        If orderid.Length > 0 Then
            Dim request = WebRequest.Create(New Uri(url, "sales/orders/" & orderid))
            request.Headers.Add("Authorization", auth)

            Using response = request.GetResponse
                Dim sr = New StreamReader(response.GetResponseStream())
                Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)
                Return jsonobj
            End Using
        End If

        Return Nothing
    End Function

    Public Function GetTerms()
        Dim request = WebRequest.Create(New Uri(url, "payment_terms/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function GetShipVia()
        Dim request = WebRequest.Create(New Uri(url, "shipping_methods/?limit=100"))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            Return jsonobj
        End Using
    End Function

    Public Function CheckProductCode(ByVal code As String) As Boolean
        Dim request = WebRequest.Create(New Uri(url, String.Format("inventory/product_codes/?filter={{""code"":""{0}""}}", code)))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            If jsonobj("count") = 1 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Public Function CheckVendor(ByVal code As String) As Boolean
        Dim request = WebRequest.Create(New Uri(url, String.Format("vendors/?filter={{""code"":""{0}""}}", code)))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            If jsonobj("count") = 1 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Public Function CheckTerritory(ByVal code As String, ByRef terrdescription As String) As Boolean
        Dim request = WebRequest.Create(New Uri(url, String.Format("territories/?filter={{""code"":""{0}""}}", code)))
        request.Headers.Add("Authorization", auth)

        Using response = request.GetResponse
            Dim sr = New StreamReader(response.GetResponseStream)
            Dim jsonobj = Helpers.Json.Decode(sr.ReadToEnd)

            If jsonobj("count") = 1 Then
                terrdescription = jsonobj("records")(0)("name")
                Return True
            Else
                Return False
            End If
        End Using
    End Function
End Class
