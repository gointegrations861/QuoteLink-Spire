Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography

Module Module1
    Enum Custom
        Normal = 1
        Electromate = 2
        Primespec = 3
        Norwood = 4
    End Enum

    Enum LicenseState
        DemoActive = 1
        DemoExpire = 2
        LicenseActive = 3
        LicenseApproachingExpire = 4
        LicenseExpire = 5
    End Enum

    Enum LineType
        ProductService = 1
        Comment = 2
        SubTotal = 4
        GroupHeader = 8
        RunningSubTotal = 16
        PercentDiscount = 64
        PercentCharge = 128
    End Enum

    Enum LineAttribute
        None = 0
        Exclude = 1
        HidePrice = 2
        DontPrint = 4
        GroupMember = 8
        OptionAttribute = 16
        AltIsOverided = 32
        PrintPicture = 64
    End Enum

    Public version As String = "3.00n"

    Public qwAppConnected As Boolean = False
    Public qwBackConnected As Boolean = False
    Public dsnConnected As Boolean = False
    Public spireAPIConnected As Boolean = False

    Public defaultQuoteStage As String = ""
    Public defaultWarehouse As String = ""
    Public defaultSyncWarehouse As String = ""

    Public currentProfile As Integer = 0

    Public licState As LicenseState = LicenseState.LicenseExpire

    Public Declare Function ShowWindow Lib "user32" (ByVal winHandle As IntPtr, ByVal nCmdShow As Long) As Long

    Public Function AES_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New RijndaelManaged
        Dim Hash_AES As New MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.ECB
            Dim DESEncrypter As ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
            Return "error"
        End Try
    End Function

    Public Function AES_Decrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New RijndaelManaged
        Dim Hash_AES As New MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.ECB
            Dim DESDecrypter As ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return decrypted
        Catch ex As Exception
            Return "error"
        End Try
    End Function

    'Public Function AlreadyRunning() As Boolean
    '    Dim myProc As Process = Process.GetCurrentProcess
    '    Dim myName As String = myProc.ProcessName

    '    Dim procs() As Process = Process.GetProcessesByName(myName)

    '    If procs.Length = 1 Then Return False

    '    MsgBox("Only one instance allowed", MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information, "Error")
    '    Return True
    'End Function

    'Public Function AESencrypt(ByVal plainText As String, ByVal password As String) As String
    '    Dim legalCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+[]{};':<>?,./\|~`"
    '    Dim random = New Random()
    '    Dim builder = New StringBuilder()

    '    Dim SALT_LENGTH = 32
    '    Dim DERIVATION_ROUNDS = 2
    '    Dim BLOCK_SIZE = 16
    '    Dim KEY_SIZE = 32

    '    Dim ascii As New ASCIIEncoding()

    '    Dim saltString As String = ""
    '    For i As Integer = 0 To KEY_SIZE
    '        Dim ch = legalCharacters(random.Next(0, legalCharacters.Length))
    '        builder.Append(ch)
    '    Next
    '    saltString = builder.ToString()
    '    Dim salt(KEY_SIZE - 1) As Byte
    '    ascii.GetBytes(saltString, 0, KEY_SIZE, salt, 0)

    '    builder.Remove(0, builder.Length())

    '    Dim ivString As String = ""
    '    For i As Integer = 0 To BLOCK_SIZE
    '        Dim ch = legalCharacters(random.Next(0, legalCharacters.Length))
    '        builder.Append(ch)
    '    Next
    '    ivString = builder.ToString()
    '    Dim iv(BLOCK_SIZE - 1) As Byte
    '    ascii.GetBytes(ivString, 0, BLOCK_SIZE, iv, 0)

    '    Dim mySHA256 As SHA256 = SHA256Managed.Create()
    '    Dim hashValue() As Byte = New System.Text.UTF8Encoding().GetBytes(password)

    '    For i As Integer = 0 To DERIVATION_ROUNDS - 1
    '        Dim temp2(hashValue.Length() + salt.Length() - 1) As Byte
    '        hashValue.CopyTo(temp2, 0)
    '        Array.Copy(salt, 0, temp2, hashValue.Length(), salt.Length())
    '        hashValue = mySHA256.ComputeHash(temp2)
    '    Next
    '    Dim derivedKey(KEY_SIZE - 1) As Byte
    '    Array.Copy(hashValue, derivedKey, KEY_SIZE)

    '    Dim AES As New RijndaelManaged
    '    'AES.BlockSize = 128
    '    'AES.KeySize = 256
    '    AES.Key = derivedKey
    '    AES.Mode = CipherMode.CBC
    '    AES.IV = iv

    '    Dim padLength = 16 - (plainText.Length() Mod 16)
    '    plainText = plainText.PadRight(padLength + plainText.Length(), Convert.ToChar(padLength + 48))

    '    Dim data(plainText.Length() - 1) As Byte
    '    ascii.GetBytes(plainText, 0, plainText.Length(), data, 0)

    '    Dim DESEncryptor As ICryptoTransform = AES.CreateEncryptor
    '    Dim encrypted = DESEncryptor.TransformFinalBlock(data, 0, data.Length)
    '    Dim allEncrypted(encrypted.Length() + iv.Length() + salt.Length() - 1) As Byte
    '    Array.Copy(encrypted, 0, allEncrypted, 0, encrypted.Length())
    '    Array.Copy(iv, 0, allEncrypted, encrypted.Length(), iv.Length())
    '    Array.Copy(salt, 0, allEncrypted, encrypted.Length() + iv.Length(), salt.Length())
    '    Return Convert.ToBase64String(allEncrypted)

    'End Function
    'Public Function AESdecrypt(ByVal CipherText As String, ByVal password As String) As String
    '    Dim SALT_LENGTH = 32
    '    Dim DERIVATION_ROUNDS = 2
    '    Dim BLOCK_SIZE = 16
    '    Dim KEY_SIZE = 32

    '    Try
    '        Dim combinedData() As Byte = Convert.FromBase64String(CipherText)

    '        Dim startIv = combinedData.Length() - BLOCK_SIZE - SALT_LENGTH
    '        Dim startSalt = combinedData.Length() - SALT_LENGTH

    '        Dim data(startIv - 1) As Byte
    '        Dim iv(startSalt - startIv - 1) As Byte
    '        Dim salt(SALT_LENGTH - 1) As Byte
    '        Array.Copy(combinedData, 0, data, 0, startIv)
    '        Array.Copy(combinedData, startIv, iv, 0, startSalt - startIv)
    '        Array.Copy(combinedData, startSalt, salt, 0, SALT_LENGTH)
    '        Dim mySHA256 As SHA256 = SHA256Managed.Create()
    '        Dim hashValue() As Byte = New System.Text.ASCIIEncoding().GetBytes(password)

    '        For i As Integer = 0 To DERIVATION_ROUNDS - 1
    '            Dim temp2(hashValue.Length() + salt.Length() - 1) As Byte
    '            Array.Copy(hashValue, 0, temp2, 0, hashValue.Length())
    '            Array.Copy(salt, 0, temp2, hashValue.Length(), salt.Length())
    '            hashValue = mySHA256.ComputeHash(temp2)
    '        Next
    '        Dim derivedKey(KEY_SIZE - 1) As Byte
    '        Array.Copy(hashValue, derivedKey, KEY_SIZE)

    '        Dim AES As New RijndaelManaged
    '        AES.Key = derivedKey
    '        AES.Mode = CipherMode.CBC
    '        AES.IV = iv
    '        AES.Padding = PaddingMode.None

    '        Dim DESDecryptor As ICryptoTransform = AES.CreateDecryptor
    '        'Dim decryptedB() = DESDecryptor.TransformFinalBlock(data, 0, data.Length())
    '        Dim decrypted = System.Text.UTF8Encoding.UTF8.GetString(DESDecryptor.TransformFinalBlock(data, 0, data.Length()))
    '        Dim padLength As Integer = System.Text.Encoding.ASCII.GetBytes(decrypted(decrypted.Length() - 1))(0) - 48
    '        Return decrypted.Substring(0, decrypted.Length() - padLength)
    '    Catch ex As Exception
    '        Return "Invalid License:-:-"
    '    End Try
    'End Function
    'Public Function GetRunningInstance() As Process

    '    'Get the current process and all processes 
    '    'with the same name
    '    Dim current As Process = Process.GetCurrentProcess()
    '    Dim processes() As Process = Process.GetProcessesByName(current.ProcessName)

    '    'Loop through the running processes with the same name
    '    For Each process As Process In processes

    '        'Looking for a process with a different ID and
    '        'the same username
    '        If process.Id <> current.Id And UserInstance.IsSameUser(process.MainWindowHandle) Then
    '            'Return the other process instance.
    '            Return process
    '        End If

    '    Next
    '    Return Nothing
    'End Function

End Module
