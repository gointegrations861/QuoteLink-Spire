Imports System.Text.RegularExpressions
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.IO
Imports System.ComponentModel
Imports System.Text
Imports System.Net
Imports Excel = Microsoft.Office.Interop.Excel

'QW Constants

'LineType (Does not compound)
'qwLineTypeProductService = 1
'qwLineTypeComment = 2
'qwLineTypeSubTotal = 4
'qwLineTypeGroupHeader = 8
'qwLineTypeRunningSubTotal = 16 (&H10)
'qwLineTypePercentDiscount = 64 (&H40) 
'qwLineTypePercentCharge = 128 (&H80)

'LineAttribute (Compounds)
'qwLineAttributeNone = 0
'qwLineAttributeExclude = 1
'qwLineAttributeHidePrice = 2
'qwLineAttributeDontPrint = 4
'qwLineAttributeGroupMember = 8
'qwLineAttributeOption = 16 (&H10)
'qwLineAttributeAltIsOverided = 32 (&H20)
'qwLineAttributePrintPicture = 64 (&H40)

'Startup execution path
'1.  Form1_Load             Automatically called when the app is started
'2.  checkLicenseFile       Called in Form1_Load - checks if the license file exists and immediately triggers license check
'3.  LFile1_StatusChanged   Triggered by checkLicenseFile - Checks expiry or sets demo to 30 days on first execution
'4.  loadProfile            Called in Form1_Load - fills in all default values
'5.  connect                Called in loadProfile - calls load_connections then intitializes different components based on connection status
'6.  load_Connections       Called in connect - validates that the connection parameters are valid for Quotewerks and Spire APIs, and Spire ODBC
'7.  setButtonStates        Called in load_Connections - enable/disable funcitonality based on which APIs are valid and connected
'8.  drawSalespeople        Called in connect - sets up the Sales Reps table in the Mappngs tab to allow user to map Quotewerks to Spire salespeople
'9.  drawQwDatabases        Called in connect - sets up the Product Database table in the Inventory Sync tab to select which Quotewerks product database to perform an inventory sync with
'10. loadQuoteStages        Called in connect - sets up the Stage dropdown in the Batch Transfers tab to allow users to filter by Quotewerks stage
'11. loadWarehouses         Called in connect - sets up the Default Warehouse dropdown in the Options tab and Inventory Sync tab
'12. Show                   Called in Form1_Load - setup complete; show the UI

Public Class Form1
    Public DISABLELICENCE = False
    ' this one for license check
    Public CUSTOMIZATION = Custom.Electromate

    Dim status As String

    Dim CustomizationsForm As Customizations = Nothing
    Dim spireAPI As SpireAPIContainer = New SpireAPIContainer()
    Dim QWInstallation As Integer = 1
    Dim QWInstallationStr As String = ""
    Dim loading As Boolean = True
    Private WithEvents LFile1 As SKCLNET.LFile
    Public WithEvents QWApp As QuoteWerks.Application = Nothing
    Public QWBack As Object
    Public slspnMapDict As New Dictionary(Of String, String)
    Public countryMapDict As New Dictionary(Of String, String)
    Public comboBoxDict As New Dictionary(Of String, ComboBox)
    Public vendorDict As New Dictionary(Of String, Tuple(Of Boolean, Boolean))
    Public cancelClose As Boolean = False
    Public stopImport As Boolean = False
    Public key As String = "frodo's mithril vest"

#Region "Initialization"
    'Function to allow unsecure https connection for spire api
    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
    Private Sub QWApp_AfterOpenDocument() Handles QWApp.AfterOpenDocument
        Dim docID As String = QWApp.DocFunctions.GetDocumentHeaderValue("ID")
    End Sub

    Public Sub QWApp_BeforeDeleteDocument(ByVal iSource As Short, ByVal lDocID As Integer, ByRef bCancel As Boolean) Handles QWApp.BeforeDeleteDocument
    End Sub

    Public Sub QWApp_AfterConvertDocument(ByRef iResult As Integer) Handles QWApp.AfterConvertDocument
        If Not (qwBackConnected And dsnConnected) Then
            Return
        End If
        'Error in the conversion
        If iResult <> 0 Then
            Return
        End If
        Dim docID As String = QWApp.DocFunctions.GetDocumentHeaderValue("ID")
        Dim docNames As String = QWApp.DocFunctions.GetDocumentHeaderValue("DocName")
        Dim docType As String = QWApp.DocFunctions.GetDocumentHeaderValue("DocType")
        Dim labeltext As String
        labeltext = "Document converted to an " & docType & ": " & docNames & Environment.NewLine & "Click to transfer to Spire"

        NotifyIcon1.Tag = docID
        NotifyIcon1.BalloonTipTitle = "Document Converted"
        NotifyIcon1.BalloonTipText = labeltext
        NotifyIcon1.ShowBalloonTip(3)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Hide()
        ShowInTaskbar = False
        'Override https validation to allow unsecure connections for spire api
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

        '############################### ELECTROMATE CUSTOMIZATION ####################################
        If CUSTOMIZATION = Custom.Electromate Then
            terrSpreadsheetGroupBox.Visible = True
            CustomizationsToolStripMenuItem.Visible = True
            introductionNotesCheckBox.Visible = True
            purchasingNotesCheckBox.Visible = True
            closingNotesCheckBox.Visible = True
            internalNotesCheckBox.Visible = True
        Else
            TabControl1.TabPages.RemoveAt(5)
        End If
        '##############################################################################################

        'Dim runningInstance As Process = GetRunningInstance()
        'If runningInstance IsNot Nothing Then
        '     'There is another instance of this process; 
        '     'show it instead
        '     ShowWindow(runningInstance.MainWindowHandle, 1)
        'End If

        'Set parameters for licensing
        LFile1 = New SKCLNET.LFile
        '18131
        LFile1.CPAlgorithm = 65536
        LFile1.CPAlgorithmDrive = "0"
        LFile1.CPTolerance = 20
        LFile1.DateFormat = "M/d/yyyy"
        LFile1.EZTrial = False
        LFile1.LFPassword = "password"
        LFile1.Location = New System.Drawing.Point(400, 192)
        LFile1.Name = "LFile1"
        LFile1.SemPath = "C:\"
        LFile1.SemPrefix = "sema"
        LFile1.Size = New System.Drawing.Size(32, 32)
        LFile1.TabIndex = 1
        LFile1.Text = "LFile1"
        LFile1.UseEZTrigger = False
        LFile1.UseLastUsedTime = True

        'LFile1.LFName = "sample.ini"
        'LFile1.CPAlgorithm = 65536
        'LFile1.CPAlgorithmDrive = "0"
        'LFile1.CPTolerance = 20
        'LFile1.DateFormat = "M/d/yyyy"
        'LFile1.EZTrial = False
        'LFile1.LFOpenFlags = SKLFOPENFLAGS.CREATE_NORMAL
        'LFile1.LFPassword = "{b84f5d53-4518-4fe4-8b44-313e424a10ce}"
        'LFile1.LFType = SKLFTYPES.FILE
        'LFile1.Location = New System.Drawing.Point(400, 207)
        'LFile1.Name = "LFile1"
        'LFile1.SemPath = "C:\"
        'LFile1.SemPrefix = "sema"
        'LFile1.SemType = SKSEMTYPES.SEMFILE
        'LFile1.Size = New System.Drawing.Size(32, 32)
        'LFile1.TabIndex = 1
        'LFile1.Text = "LFile1"
        'LFile1.UseEZTrigger = False
        'LFile1.UseLastUsedTime = True
        'LFile1.TCSeed = 400 'Default
        'LFile1.TCRegKey2Seed = 123 'Default

        'LFile1 = New SKCLNET.LFile
        'LFile1.CPAlgorithm = 65536
        'LFile1.CPTolerance = 20
        'LFile1.LFOpenFlags = SKLFOPENFLAGS.CREATE_NORMAL
        'LFile1.EZTrial = True
        'LFile1.UseEZTrigger = True
        'LFile1.LFPassword = "{b84f5d53-4518-4fe4-8b44-313e424a10ce}"
        'LFile1.DateFormat = "dd/MM/yyyy"
        'LFile1.SemPath = "C:\"
        'LFile1.SemPrefix = "sema"
        'LFile1.SemType = SKSEMTYPES.SEMFILE

        'If the license file exists, set the path to trigger the license check
        checkLicenseFile()

        NotifyIcon1.Icon = My.Resources.disconnected
        NotifyIcon1.Text = "QuoteWerks and Spire not connected"

        'Check the last used profile and update the profile names
        My.Settings.SettingsKey = "Global"
        My.Settings.Reload()
        Dim profileID = My.Settings.DefaultProfile
        Profile1ToolStripMenuItem.Text = My.Settings.Profile1Name
        Profile2ToolStripMenuItem.Text = My.Settings.Profile2Name

        'Load the last used profile
        loadProfile(profileID)

        'Make the app visible
        Show()
        ShowInTaskbar = True

        loading = False
    End Sub

    Private Sub load_Connections()
        dsnStatusLabel.Text = "Not Connected"
        qwDatabase.Text = "Not Connected"
        spireAPIStatusLabel.Text = "Not Connected"
        qwAppConnected = False
        qwBackConnected = False
        dsnConnected = False
        spireAPIConnected = False
        qwStatusLabel.Text = ""

        'QUOTEWERKS FRONTEND
        'Connects to a running instance of quotewerks on the same computer, used for trigger functions, inventory sync
        Try
            QWApp = GetObject(, "QuoteWerks.Application")
            If Not (QWApp Is Nothing) Then
                qwAppConnected = True
                'MessageBox.Show("test")
            End If
        Catch ex As Exception
            If ex.Message.Contains("ActiveX") Then
                qwStatusLabel.Text = "Unable to connect to an open instance of QuoteWerks."
                'qwStatusLabel.Text = ex.Message
            Else
                qwStatusLabel.Text = ex.Message
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
        'QUOTEWERKS BACKEND
        'Connects to the database using containers and queries
        Try
            QWBack = CreateObject("QuoteWerksBackend.Installations")
            If Not (QWBack Is Nothing) Then
                qwInstancesComboBox.Items.Clear()
                For i As Integer = 1 To QWBack.count
                    qwInstancesComboBox.Items.Insert(0, QWBack.Item(i).Path)
                    If QWBack.Item(i).Path = QWInstallationStr Then
                        QWInstallation = i
                    End If
                Next
                qwInstancesComboBox.Items.Insert(0, "<None>")
                qwInstancesComboBox.SelectedIndex = QWBack.count - QWInstallation + 1
                If QWInstallation = 0 Then
                    qwStatusLabel.Text &= If(qwAppConnected, "Please choose an installation to connect", "")
                ElseIf QWBack.count >= QWInstallation Then
                    QWBack.Item(QWInstallation).Database.RequestDeveloperAccess("QWTEST")
                    Dim status = QWBack.Item(QWInstallation).Database.DeveloperAccessAuthorizationStatus("QWTEST")
                    If status = 2 Then
                        qwBackConnected = True
                        qwStatusLabel.Text &= If(qwAppConnected, "Connected", "")
                        qwDatabase.Text = QWBack.Item(QWInstallation).Path
                    End If
                End If
                'For i As Integer = 1 To QWBack.Count
                '    QWBack.Item(i).Database.RequestDeveloperAccess("QWTEST")
                '    Dim status = QWBack.Item(i).Database.DeveloperAccessAuthorizationStatus("QWTEST")

                '    If status = 2 Then
                '        qwBackConnected = True
                '        QWInstallation = i
                '        qwStatusLabel.Text &= If(qwAppConnected, "Connected", "")
                '        qwDatabase.Text = QWBack.Item(i).Path
                '        Exit For
                '    End If
                'Next
                If Not qwBackConnected Then
                    MessageBox.Show("Couldn't get developer access authorization", "Backend Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    qwStatusLabel.Text &= If(qwAppConnected, "", Environment.NewLine)
                    qwStatusLabel.Text &= "Couldn't get developer access authorization"
                End If
            Else
                MessageBox.Show("QuoteWerksBackend.Installations returned Nothing", "Backend Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                qwStatusLabel.Text &= If(qwAppConnected, "", Environment.NewLine)
                qwStatusLabel.Text &= "Could not connect to the backend installation."
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Frontend Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            qwStatusLabel.Text &= If(qwAppConnected, "", Environment.NewLine)
            qwStatusLabel.Text &= "Could not connect to the backend installation."
        End Try

        'Spire DSN
        If DSNTextBox.Text.Length > 0 Then
            Dim connectionString As String
            connectionString = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"
            Dim conn As OdbcConnection
            conn = New OdbcConnection(connectionString)
            Try
                conn.Open()
                dsnStatusLabel.Text = "Connected"
                dsnConnected = True
            Catch ex As Exception
                MessageBox.Show(ex.Message, "DSN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                conn.Close()
                conn.Dispose()
            End Try
        End If

        'Spire API
        If SpireURLTextBox.Text.Trim.Length > 0 Then
            Dim ret = spireAPI.Init(SpireURLTextBox.Text, SpireUsernameTextBox.Text, SpirePasswordTextBox.Text)
            If ret Then
                spireAPIConnected = True
                spireAPIStatusLabel.Text = "Connected"
            End If
        End If

        setButtonStates()

        If qwAppConnected And qwBackConnected And dsnConnected And spireAPIConnected Then
            NotifyIcon1.Icon = My.Resources.connected
            NotifyIcon1.Text = "QuoteWerks and Spire Connected"
        Else
            NotifyIcon1.Icon = My.Resources.disconnected
            Dim statusString As String = ""
            If Not qwAppConnected Or Not qwBackConnected Then
                statusString = "QuoteWerks"
                If Not dsnConnected Or Not spireAPIConnected Then
                    statusString &= " and Spire"
                End If
            Else
                statusString = "Spire"
            End If
            statusString &= " not connected"
            NotifyIcon1.Text = statusString
        End If
    End Sub

    Public Sub setButtonStates()
        If spireAPIConnected And qwBackConnected And licState <> LicenseState.DemoExpire And licState <> LicenseState.LicenseExpire Then
            batchTransferButton.Enabled = True
        Else
            batchTransferButton.Enabled = False
        End If

        If spireAPIConnected And qwBackConnected And qwAppConnected And licState <> LicenseState.DemoExpire And licState <> LicenseState.LicenseExpire Then
            transferOrderButton.Enabled = True
        Else
            transferOrderButton.Enabled = False
        End If

        If spireAPIConnected And qwAppConnected And licState <> LicenseState.DemoExpire And licState <> LicenseState.LicenseExpire Then
            InvSyncButton.Enabled = True
        Else
            InvSyncButton.Enabled = False
        End If

        If spireAPIConnected And qwBackConnected Then
            refreshSlspnButton.Enabled = True
        Else
            refreshSlspnButton.Enabled = False
        End If

        If qwBackConnected Then
            batchLookupButton.Enabled = True
        Else
            batchLookupButton.Enabled = False
        End If
    End Sub

    Public Sub drawQwDatabases()
        databaseTable.Visible = False
        databaseTable.Controls.Clear()
        databaseTable.RowStyles.Clear()
        databaseTable.RowCount = 1
        Dim dbCount = QWApp.ProductDatabases.Count
        For i = 1 To dbCount
            Dim rb = New RadioButton()
            rb.AutoSize = True
            rb.Location = New System.Drawing.Point(6, 19 + ((i - 1) * 23))
            rb.Name = "RadioButton" & i
            rb.Size = New System.Drawing.Size(90, 17)
            rb.TabIndex = (i - 1)
            rb.TabStop = True
            rb.Text = QWApp.ProductDatabases.Item(i).Name & " ( " & QWApp.ProductDatabases.Item(i).DataSource & " ) "
            rb.UseVisualStyleBackColor = True
            databaseTable.Controls.Add(rb, 0, i)
        Next
        databaseTable.Visible = True
    End Sub

    Public Sub loadQuoteStages(defaultValue As String)
        filterStageComboBox.Items.Clear()
        With QWBack.Item(QWInstallation)
            Dim iError = .Database.OpenDB("LOOKUP", "QWTEST")

            With .Database.Recordset
                If filterTypeComboBox.SelectedIndex = 0 Then
                    .QueryEx("SELECT LookupMemoValue FROM LookupItems where LookupName = 'TB_DOCSTATUS_QUOTE'")
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        Do
                            filterStageComboBox.Items.Add(.GetFieldValue("LookupMemoValue"))
                            .MoveNext()
                        Loop While Not .EOF
                    End If
                ElseIf filterTypeComboBox.SelectedIndex = 1 Then
                    .QueryEx("SELECT LookupMemoValue FROM LookupItems where LookupName = 'TB_DOCSTATUS_ORDER'")
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        Do
                            filterStageComboBox.Items.Add(.GetFieldValue("LookupMemoValue"))
                            .MoveNext()
                        Loop While Not .EOF
                    End If
                Else
                    .QueryEx("SELECT LookupMemoValue FROM LookupItems where LookupName = 'TB_DOCSTATUS_INVOICE'")
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        Do
                            filterStageComboBox.Items.Add(.GetFieldValue("LookupMemoValue"))
                            .MoveNext()
                        Loop While Not .EOF
                    End If
                End If
            End With
        End With

        Dim index = filterStageComboBox.FindString(defaultValue)
        If index = -1 Then
            index = filterStageComboBox.FindString("Closed")
            If index = -1 Then
                filterStageComboBox.SelectedIndex = 0
            Else
                filterStageComboBox.SelectedIndex = index
            End If
        Else
            filterStageComboBox.SelectedIndex = index
        End If

    End Sub

    Public Sub loadWarehouses(defaultValue As String, defaultSyncValue As String)
        defaultWarehouseComboBox.Items.Clear()
        defaultSyncWarehouseComboBox.Items.Clear()

        Dim whses = spireAPI.GetWarehouses()

        For i As Integer = 0 To Integer.Parse(whses("count")) - 1
            defaultWarehouseComboBox.Items.Add(whses("records")(i)("code"))
            defaultSyncWarehouseComboBox.Items.Add(whses("records")(i)("code"))
        Next

        'Dim conn As OdbcConnection
        'Dim comm As OdbcCommand
        'Dim dr As OdbcDataReader
        'Dim connectionString As String
        'Dim sql As String
        'connectionString = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"
        'sql = "SELECT whse FROM public.inventory_warehouses"
        'conn = New OdbcConnection(connectionString)
        'conn.Open()
        'comm = New OdbcCommand(sql, conn)
        'dr = comm.ExecuteReader()
        'While (dr.Read())
        '    defaultWarehouseComboBox.Items.Add(dr.GetValue(0).ToString())
        '    defaultSyncWarehouseComboBox.Items.Add(dr.GetValue(0).ToString())
        'End While
        'conn.Close()
        'dr.Close()
        'comm.Dispose()
        'conn.Dispose()

        Dim index = defaultWarehouseComboBox.FindString(defaultValue)
        If index = -1 Then
            defaultWarehouseComboBox.SelectedIndex = 0
        Else
            defaultWarehouseComboBox.SelectedIndex = index
        End If

        index = defaultSyncWarehouseComboBox.FindString(defaultSyncValue)
        If index = -1 Then
            defaultSyncWarehouseComboBox.SelectedIndex = 0
        Else
            defaultSyncWarehouseComboBox.SelectedIndex = index
        End If
    End Sub

    Public Sub loadCountries()
        countryMapDict.Clear()
        Dim conn As OdbcConnection
        Dim comm As OdbcCommand
        Dim dr As OdbcDataReader
        Dim sql As String
        Dim connectionString As String = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"
        conn = New OdbcConnection(connectionString)
        conn.Open()

        sql = "select code, lower(name) from countries"
        comm = New OdbcCommand(sql, conn)
        dr = comm.ExecuteReader()
        While dr.Read()
            countryMapDict.Add(dr.GetString(1).Trim(), dr.GetString(0).Trim())
        End While
        dr.Close()
        comm.Dispose()
        conn.Close()
        conn.Dispose()
    End Sub

    Private Sub drawSalespeople()
        slspnMapTable.Visible = False
        slspnMapTable.Controls.Clear()
        slspnMapTable.RowStyles.Clear()
        slspnMapTable.RowCount = 1

        comboBoxDict.Clear()

        Dim label As Label
        Dim comboBox As ComboBox
        Dim sageSalesRepsList As New List(Of String)

        Dim slsppl = spireAPI.GetSalespeople()

        For i As Integer = 0 To Integer.Parse(slsppl("count")) - 1
            sageSalesRepsList.Add(slsppl("records")(i)("name"))
        Next
        'Dim conn As OdbcConnection
        'Dim comm As OdbcCommand
        'Dim dr As OdbcDataReader
        'Dim connectionString As String
        'Dim sql As String
        'connectionString = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"
        'sql = "SELECT name FROM public.salespeople"
        'conn = New OdbcConnection(connectionString)
        'conn.Open()
        'comm = New OdbcCommand(sql, conn)
        'dr = comm.ExecuteReader()
        'While (dr.Read())
        '    sageSalesRepsList.Add(dr.GetValue(0).ToString())
        'End While
        'conn.Close()
        'dr.Close()
        'comm.Dispose()
        'conn.Dispose()

        If qwBackConnected Then
            For i As Integer = 0 To QWBack.Item(QWInstallation).Security.Users.Count - 1
                label = New Label()
                label.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
                label.Text = QWBack.Item(QWInstallation).Security.Users.Item(i + 1).UserName
                slspnMapTable.Controls.Add(label, 0, i)

                comboBox = New ComboBox()
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                comboBox.Items.Add("--")
                For Each repName In sageSalesRepsList
                    comboBox.Items.Add(repName)
                Next
                If slspnMapDict.ContainsKey(label.Text) Then
                    Dim index = comboBox.FindString(slspnMapDict(label.Text))
                    If index = -1 Then
                        comboBox.SelectedIndex = 0
                    Else
                        comboBox.SelectedIndex = index
                    End If
                Else
                    comboBox.SelectedIndex = 0
                End If
                AddHandler comboBox.SelectedIndexChanged, AddressOf defaultsStateChanged
                comboBoxDict.Add(label.Text(), comboBox)
                slspnMapTable.Controls.Add(comboBox, 1, i)
            Next
        End If
        slspnMapTable.Visible = True
    End Sub

    Public Sub connect()
        saveDefaults()
        load_Connections()

        If qwBackConnected And spireAPIConnected Then
            drawSalespeople()
        End If
        If qwAppConnected Then
            drawQwDatabases()
        End If
        If qwBackConnected Then
            Try
                loadQuoteStages(defaultQuoteStage)
                'readTranslationFile()
                'applyTranslations()
                'ResetMappings()
            Catch ex As Exception
            End Try
        End If
        If spireAPIConnected Then
            loadWarehouses(defaultWarehouse, defaultSyncWarehouse)
        End If

        If CUSTOMIZATION = Custom.Electromate Then
            drawVendors()
            If dsnConnected Then
                loadCountries()
            End If
        ElseIf CUSTOMIZATION = Custom.Norwood Then
            If dsnConnected Then
                loadCountries()
            End If
        End If

        batchOrderDataGridView.Rows.Clear()
    End Sub

    Public Sub drawVendors()
        'Custom for electromate
        DataGridView1.Rows.Clear()
        For Each kvp As KeyValuePair(Of String, Tuple(Of Boolean, Boolean)) In vendorDict
            Dim index = DataGridView1.Rows.Add()

            DataGridView1.Rows(index).Cells(0).Value = kvp.Key
            DataGridView1.Rows(index).Cells(1).Value = kvp.Value.Item1
            DataGridView1.Rows(index).Cells(2).Value = kvp.Value.Item2

        Next
    End Sub

#End Region

#Region "Event Handlers"
    Private Sub Form1_FormClosing(sender As Object, e As CancelEventArgs) Handles MyBase.FormClosing
        NotifyIcon1.Visible = False
        NotifyIcon1.Dispose()
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        ConnectButton.Enabled = False
        loading = True
        connect()
        loading = False
        ConnectButton.Enabled = True
    End Sub

    Private Sub LogonDateTextBox_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

        '97 - 122 = Ascii codes for simple letters
        '65 - 90  = Ascii codes for capital letters
        '48 - 57  = Ascii codes for numbers

        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub defaultsStateChanged(sender As Object, e As EventArgs) Handles updatedItemComboBox.SelectedIndexChanged, transactionComboBox.SelectedIndexChanged, newCustomerComboBox.SelectedIndexChanged, itemMapComboBox.SelectedIndexChanged, descriptionCheckBox.CheckStateChanged, filterStageComboBox.SelectedIndexChanged, ServerTextBox.Leave, DSNTextBox.Leave, syncPricingCheckBox.CheckStateChanged, syncCostCheckBox.CheckStateChanged, defaultSyncWarehouseComboBox.SelectedIndexChanged, defaultWarehouseComboBox.SelectedIndexChanged, dbUsernameTextBox.Leave, dbServerTextBox.Leave, dbPasswordTextBox.Leave, dbNameTextBox.Leave, defaultSelPriceLevelComboBox.SelectedIndexChanged, bundledItemComboBox.SelectedIndexChanged, newItemComboBox.SelectedIndexChanged, sequentialCustCheckBox.CheckStateChanged, purchasingNotesCheckBox.CheckStateChanged, introductionNotesCheckBox.CheckStateChanged, internalNotesCheckBox.CheckStateChanged, closingNotesCheckBox.CheckStateChanged, suppressSlspnWarningCheckBox.CheckStateChanged, dateMapComboBox.SelectedIndexChanged, SpireUsernameTextBox.Leave, SpireURLTextBox.Leave, SpirePasswordTextBox.Leave, defaultSyncSellPriceLevelComboBox.SelectedIndexChanged, terrWorksheetTextBox.Leave, terrSpreadsheetTextBox.Leave, territoryMapComboBox.SelectedIndexChanged, customerNoMapComboBox.SelectedIndexChanged
        saveDefaults()
    End Sub
    Private Sub refreshSlspnButton_Click(sender As Object, e As EventArgs) Handles refreshSlspnButton.Click
        drawSalespeople()
    End Sub

    Private Sub UserNameTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            connect()
        End If
    End Sub

    Private Sub batchOrderDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles batchOrderDataGridView.SelectionChanged
        batchOrderDataGridView.ClearSelection()
    End Sub

    Private Sub terrSpreadsheetButton_Click(sender As Object, e As EventArgs) Handles terrSpreadsheetButton.Click
        Dim strFileName As String
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "Excel (*.xlsx)|*.xlsx"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            terrSpreadsheetTextBox.Text = strFileName
        End If
        saveDefaults()
    End Sub

    Private Sub InvSyncStopButton_Click(sender As Object, e As EventArgs) Handles InvSyncStopButton.Click
        stopImport = True
    End Sub

    Private Sub SQLServerCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SQLServerCheckBox.CheckedChanged
        If SQLServerCheckBox.Checked Then
            dbServerTextBox.Enabled = True
            dbNameTextBox.Enabled = True
            dbUsernameTextBox.Enabled = True
            dbPasswordTextBox.Enabled = True
        Else
            dbServerTextBox.Enabled = False
            dbNameTextBox.Enabled = False
            dbUsernameTextBox.Enabled = False
            dbPasswordTextBox.Enabled = False
        End If
        saveDefaults()
    End Sub

    Private Sub quoteTypeChanged(sender As Object, e As EventArgs) Handles filterTypeComboBox.SelectedIndexChanged
        If loading Then
            Return
        End If
        loadQuoteStages(My.Settings.FilterStageCombo)
        saveDefaults()
    End Sub

    Private Sub selectAllCheckBox_CheckStateChanged(sender As Object, e As EventArgs) Handles selectAllCheckBox.CheckStateChanged
        For Each row As DataGridViewRow In batchOrderDataGridView.Rows
            row.Cells("CheckBoxColumn").Value = selectAllCheckBox.Checked
        Next
    End Sub

    Private Sub transferOrderButton_Click(sender As Object, e As EventArgs) Handles transferOrderButton.Click
        Dim docID As String = QWApp.DocFunctions.GetDocumentHeaderValue("ID")
        transferOrderSpire(docID)
    End Sub

    Private Sub batchLookupButton_Click(sender As Object, e As EventArgs) Handles batchLookupButton.Click
        'Search quotewerks for documents which fit the search criteria selected by the user
        'Make sure the date format is something we can understand easily
        System.Globalization.CultureInfo.CurrentCulture.ClearCachedData()
        If System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern <> "MM/dd/yyyy" And System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern <> "M/d/yyyy" Then
            MessageBox.Show("Make sure the system Short Date format is set to MM/dd/yyyy or M/d/yyyy for the transfer to operate correctly." &
                            " It is currently set to " & System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern & ".", "Wrong Date Format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Process.Start("INTL.CPL")
            Return
        End If

        batchOrderDataGridView.Rows.Clear()

        Dim count = 0
        Dim fromDate = fromDateTimePicker.Value.ToShortDateString
        Dim toDate = toDateTimePicker.Value.ToShortDateString
        Dim customerNoMap As String = customerNoMapComboBox.SelectedItem

        Dim typeFilter As String
        If filterTypeComboBox.SelectedIndex = 2 Then
            typeFilter = "INVOICE"
        ElseIf filterTypeComboBox.SelectedIndex = 1 Then
            typeFilter = "ORDER"
        Else
            typeFilter = "QUOTE"
        End If
        With QWBack.Item(QWInstallation)
            Dim iError = .Database.OpenDB("DOCS", "QWTEST")

            With .Database.Recordset
                .QueryEx("SELECT ID, DocDate, DocNo, DocName, CustomNumber01, " & customerNoMap & " FROM DocumentHeaders WHERE DocType = '" & sqlEscape(typeFilter) & "' AND DocStatus = '" & sqlEscape(filterStageComboBox.SelectedItem.ToString()) & "' AND DocDate <= #" & toDate & "# AND DocDate >= #" & fromDate & "#")
                If .RecordCount > 0 Then
                    'First try Access format query
                    .MoveFirst()
                    If Not .EOF Then
                        Do
                            batchOrderDataGridView.Rows.Add(False, .GetFieldValue("DocNo").ToString().Trim(), .GetFieldValue("DocDate").ToString().Trim(), .GetFieldValue("DocName").ToString().Trim(), .GetFieldValue(customerNoMap).ToString().Trim())
                            batchOrderDataGridView.Rows.Item(batchOrderDataGridView.Rows.Count - 1).Tag = .GetFieldValue("ID").ToString()

                            If .GetFieldValue("CustomNumber01").ToString = "1" Then
                                batchOrderDataGridView.Rows.Item(batchOrderDataGridView.Rows.Count - 1).Cells("CheckBoxColumn").Style.BackColor = Color.PaleGreen
                            End If

                            count += 1
                            .MoveNext()
                        Loop While Not .EOF
                    End If
                    setButtonStates()
                    selectAllCheckBox.Enabled = True
                Else
                    'If nothing is returned by the previous query, try a SQL format query
                    .QueryEx("SELECT ID, DocDate, DocNo, DocName, CustomNumber01, " & customerNoMap & " " &
                             "FROM DocumentHeaders " &
                             "WHERE DocType = '" & sqlEscape(typeFilter) & "' " &
                             "AND DocStatus = '" & sqlEscape(filterStageComboBox.SelectedItem.ToString()) & "' " &
                             "AND CONVERT(datetime,DocDate,101) <= CONVERT(datetime,'" & toDate & "',101) AND CONVERT(datetime,DocDate,101) >= CONVERT(datetime,'" & fromDate & "',101)")
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        If Not .EOF Then
                            Do
                                batchOrderDataGridView.Rows.Add(False, .GetFieldValue("DocNo").ToString().Trim(), .GetFieldValue("DocDate").ToString().Trim(), .GetFieldValue("DocName").ToString().Trim(), .GetFieldValue(customerNoMap).ToString().Trim())
                                batchOrderDataGridView.Rows.Item(batchOrderDataGridView.Rows.Count - 1).Tag = .GetFieldValue("ID").ToString

                                If .GetFieldValue("CustomNumber01").ToString().Trim() = "1" Then
                                    batchOrderDataGridView.Rows.Item(batchOrderDataGridView.Rows.Count - 1).Cells("CheckBoxColumn").Style.BackColor = Color.PaleGreen
                                End If

                                count += 1
                                .MoveNext()
                            Loop While Not .EOF
                        End If
                        setButtonStates()
                        selectAllCheckBox.Enabled = True
                    Else
                        batchOrderDataGridView.Rows.Add(False, "--", "--", "No Results")
                        batchOrderDataGridView.Rows(0).Cells("CheckBoxColumn").ReadOnly = True
                        batchTransferButton.Enabled = False
                        selectAllCheckBox.Enabled = False
                    End If
                End If
            End With
            .Database.CloseDB()
        End With
        If count > 1 Then
            batchOrderDataGridView.Sort(batchOrderDataGridView.Columns(2), ListSortDirection.Descending)
        End If
    End Sub

    Private Sub batchTransferButton_Click(sender As Object, e As EventArgs) Handles batchTransferButton.Click
        'Loop through all rows in the batch transfers tab, if the row is checked, transfer the quote to spire
        Dim whseObj = spireAPI.FindWhse(defaultWarehouseComboBox.SelectedItem.ToString)
        If whseObj("count") <> "1" Then
            MessageBox.Show("The entered default warehouse does not exist. No quotes transferred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        batchLookupButton.Enabled = False
        Dim successCount As Integer = 0
        For Each row As DataGridViewRow In batchOrderDataGridView.Rows
            If row.Cells("CheckBoxColumn").Value = True Then
                Dim success = transferOrderSpire(row.Tag, False)
                If success Then
                    row.Cells("CheckBoxColumn").Value = False
                    row.Cells("CheckBoxColumn").Style.BackColor = Color.PaleGreen
                    successCount += 1
                Else
                    row.Cells("CheckBoxColumn").Style.BackColor = Color.Red
                End If
            End If
        Next
        If successCount <> 0 Then
            MessageBox.Show("Successfully transferred " & successCount & " quotes.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("No quotes transferred.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        batchLookupButton.Enabled = True
    End Sub

    Private Sub CustomizationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CustomizationsToolStripMenuItem.Click
        CustomizationsForm = New Customizations()
        CustomizationsForm.electromateGroupBox.Visible = True
        CustomizationsForm.ShowDialog()
    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If loading Then
            Return
        End If
        Dim vendorName = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString().Trim()
        If e.ColumnIndex = 0 Then
            If vendorDict.ContainsKey(vendorName) Then
                MessageBox.Show("Vendor already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                DataGridView1.Rows.RemoveAt(e.RowIndex)
                Return
            End If
            vendorDict.Add(vendorName, Tuple.Create(False, False))
            For i = 0 To DataGridView1.Rows.Count - 2
                If Not vendorDict.ContainsKey(DataGridView1.Rows(i).Cells(0).Value.ToString().Trim()) Then
                    vendorDict.Remove(DataGridView1.Rows(i).Cells(0).Value.ToString().Trim())
                End If
            Next
            saveDefaults()
        End If
    End Sub

    Private Sub DataGridView1_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim vendorName = e.Row.Cells(0).Value.ToString().Trim()
        If vendorDict.ContainsKey(vendorName) Then
            vendorDict.Remove(vendorName)
        End If

        saveDefaults()
    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        If DataGridView1.CurrentCell.ColumnIndex = 0 Then
            Dim prodCode As TextBox = e.Control
            prodCode.CharacterCasing = CharacterCasing.Upper
        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        DataGridView1.EndEdit()
        Dim vendorName = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString().Trim()
        If e.ColumnIndex = 1 Then
            Dim lotNumbered As Boolean = If(DataGridView1.Rows(e.RowIndex).Cells(1).Value Is Nothing, False, DataGridView1.Rows(e.RowIndex).Cells(1).Value)
            vendorDict(vendorName) = Tuple.Create(lotNumbered, False)
            If lotNumbered Then
                DataGridView1.Rows(e.RowIndex).Cells(2).Value = False
            End If
        ElseIf e.ColumnIndex = 2 Then
            Dim serialized As Boolean = If(DataGridView1.Rows(e.RowIndex).Cells(2).Value Is Nothing, False, DataGridView1.Rows(e.RowIndex).Cells(2).Value)
            vendorDict(vendorName) = Tuple.Create(False, serialized)
            If serialized Then
                DataGridView1.Rows(e.RowIndex).Cells(1).Value = False
            End If
        End If
        saveDefaults()
    End Sub

#End Region

#Region "Licencing"

    Sub setLicense(ByRef state As Integer)
        licState = state
        Select Case state
            Case LicenseState.DemoActive
                demoLabel.Visible = True
                ActivateToolStripMenuItem.Enabled = True
            Case LicenseState.DemoExpire
                demoLabel.Visible = True
                ActivateToolStripMenuItem.Enabled = True
            Case LicenseState.LicenseApproachingExpire
                demoLabel.Visible = False
                ActivateToolStripMenuItem.Enabled = False
            Case LicenseState.LicenseActive
                demoLabel.Visible = False
                ActivateToolStripMenuItem.Enabled = False
            Case LicenseState.LicenseExpire
                demoLabel.Visible = False
                ActivateToolStripMenuItem.Enabled = True
        End Select
        setButtonStates()
    End Sub

    Public Sub ConvertToDemo(ByVal days As Long)
        MessageBox.Show("Demo set to " & days & " days from today", "Success!")

        ' We don't want StatusChanged to fire because we are already
        ' in this subroutine
        LFile1.Enabled = False

        ' Set to Demo mode
        LFile1.ExpireMode = "D"

        ' Set Demo Expiration to X days from today
        LFile1.ExpireDateHard = DateTime.Today.AddDays(days)

        ' Put things back to normal.
        ' Force the StatusChanged event to fire
        LFile1.Enabled = True
        LFile1.ForceStatusChanged()
    End Sub

    Private Sub LFile1_StatusChanged(ByVal startup As Boolean) Handles LFile1.StatusChanged
        'If recursive Then
        '    If LFile1.IsExpired Or LFile1.DaysLeft = 0 Then
        '        Application.Exit()
        '    Else
        '        MessageBox.Show("Activation Successful.", "Success")
        '    End If
        'ElseIf LFile1.DaysLeft <= 30 And LFile1.DaysLeft > 0 Then
        '    MessageBox.Show("You have " & LFile1.DaysLeft.ToString & " days left in the demo.", "Demo")
        'ElseIf LFile1.IsExpired Or LFile1.DaysLeft = 0 Then
        '    recursive = True
        '    MessageBox.Show("QuoteLink has Expired. Please activate.", "Expired")
        '    Dim ret = LFile1.ShowTriggerDlg(0, 1, "Activate|User Code:|Computer ID:|License ID:|Password:", 0, 0)
        '    If ret = 0 Then
        '        Application.Exit()
        '    ElseIf LFile1.IsExpired = False Then
        '        ActivateToolStripMenuItem.Text = "Activated"
        '        ActivateToolStripMenuItem.Enabled = False
        '    End If
        'Else
        '    ActivateToolStripMenuItem.Text = "Activated"
        '    ActivateToolStripMenuItem.Enabled = False
        'End If
        Dim lblPaymentDate As String
        ' Let's see if this computer is authorized
        'we can just set the flags to 0 since we are using Enhanced Algorithms (CPAlgorithm = 65536)
        If LFile1.CPCheck(0) = 1 Then
            ' passed copy protection test - let's see if we are in Payment mode
            If LFile1.IsExpired Then
                ' This computer Payment is past due
                MessageBox.Show("QuoteLink has expired.  Please call for assistance.", "Application Violation")
                setLicense(LicenseState.LicenseExpire)
            Else
                ' So far, so good.  Let's make sure they didn't turn the clock back
                If LFile1.IsClockTurnedBack Then
                    MessageBox.Show("Your clock has been turned back.  Please correct and re-run application", "Application Date Error")
                    setLicense(LicenseState.LicenseExpire)
                Else
                    ' Turn on all menu options since everything is okay
                    'MenuOptionsRetail()
                    If LFile1.DaysLeft < 30 Then
                        MessageBox.Show("QuoteLink will expire in " & LFile1.DaysLeft & " day(s).")
                        setLicense(LicenseState.LicenseApproachingExpire)
                    Else
                        setLicense(LicenseState.LicenseActive)
                    End If
                End If
            End If
        Else
            ' License File is okay, but this computer is not authorized
            ' see if this is in demo mode
            If LFile1.ExpireDateHard = "12/31/2050" Then
                ' This is not a demo but the copy protection failed the test
                ' convert this copy back to a 30-day demo
                ConvertToDemo(30)
            Else
                ' turn on our demo indicator
                lblPaymentDate = "Demo expires:  " & LFile1.ExpireDateHard

                If LFile1.IsExpired Then
                    ' This demo has expired
                    MessageBox.Show("This demo has expired.  Please call for assistance.", "Application Violation")
                    lblPaymentDate = "Demo has expired!"
                    setLicense(LicenseState.DemoExpire)
                Else
                    ' So far, so good.  Let's make sure they didn't turn the clock back
                    If LFile1.IsClockTurnedBack Then
                        MessageBox.Show("Your clock has been turned back.  Please correct and re-run application", "Application Date Error")
                        setLicense(LicenseState.DemoExpire)
                    Else
                        ' Turn on all menu options since everything is okay
                        ' You could limit the demo by turning off a menu option if you want
                        'MenuOptionsDemo()
                        MessageBox.Show("Demo expires:  " & LFile1.ExpireDateHard)
                        setLicense(LicenseState.DemoActive)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub LFile1_Trigger(ByVal event_num As Integer, ByVal event_data As Integer) Handles LFile1.Trigger
        Select Case event_num
            Case 2
                ' Set Demo Expiration to 30 days from today
                LFile1.CPAdd(4, 0)
                'MessageBox.Show(event_data.ToString)
                ConvertToDemo(event_data)
            Case 3
                ' This code should de-authorize this computer
                LFile1.CPDelete(0)
                MsgBox("Application de-activation complete", 0, "Success!")
            Case 4
                ' if the user turned the date forward by mistake, ran you program and
                ' then set the clock back to the correct time, they will not be allowed
                ' in because pp_valdate() will fail.  They will call you and you will give
                ' them code 6 to force the last used date/time fields to be set to the
                ' current (and correct) date and time.
                LFile1.ResetLastUsedInfo()
                MsgBox("Last-used info has been reset.", 0, "Success!")
            Case 5
                ' Set Demo Expiration to X days from today, where X is the
                ' Trigger Code Additional Number (regkey2)
                ConvertToDemo(event_data)
            Case 7
                LFile1.SetUserNumber(1, event_data)
            Case Else
                ' Invalid code was entered
                MsgBox("Invalid Code Entered!", 0, "Invalid Code")
        End Select
    End Sub

    Private Sub checkLicenseFile()
        If DISABLELICENCE Then
            MessageBox.Show("This is a demo version")
            licState = LicenseState.LicenseActive
            Return
        End If
        Try
            Dim m_licensePath As String = ""
            Dim m_appPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            m_licensePath = m_appPath & "\ql.ini"
            MessageBox.Show(m_licensePath)
            If File.Exists(m_licensePath) Then
                'if a license file is present in the application's directory, use that file
                LFile1.LFName = m_licensePath
            Else
                MessageBox.Show("Unable to find license file.", "Error")
                LFile1.Enabled = False
            End If

        Catch ex As Exception
            MessageBox.Show("Unable to load license file.", "Error")
            LFile1.Enabled = False
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

#End Region

#Region "Inventory Sync"

    Private Sub InvSyncButton_Click(sender As Object, e As EventArgs) Handles InvSyncButton.Click
        stopImport = False

        If Not qwAppConnected Then
            MessageBox.Show("QuoteWerks must be running to synchronize products.", "QuoteWerks Not Running", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim rButton As RadioButton = databaseTable.Controls.OfType(Of RadioButton)().Where(Function(r) r.Checked = True).FirstOrDefault()
        If rButton Is Nothing Then
            MessageBox.Show("Please select a Product Database to synchronize.", "No Database Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim rx As Regex = New Regex("\( .* \)$")
        Dim match As Match = rx.Match(rButton.Text.Trim)
        Dim productDatabase = match.Value.Substring(2, match.Value.Length - 4)

        If Not testDatabaseConn(productDatabase) Then
            Return
        End If

        Dim ret = MessageBox.Show("Are you sure you wish to synchronize inventory from Spire to QuoteWerks?", "Proceed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        If ret <> DialogResult.Yes Then
            Return
        End If

        InvSyncButton.Enabled = False
        InvSyncStopButton.Enabled = True

        'Dim bvSubDesc As String
        'Dim bvInvOrLabour As Int32 = 2
        'Dim bvSubCode As String
        'Dim bvSequence As Integer
        'Dim qwDatabase As String = ""
        Dim itemMap As String = itemMapComboBox.SelectedItem

        Dim dneArray As New List(Of String)
        Dim dneBundleArray As New List(Of String)
        Dim priceArray As New List(Of String)
        Dim costArray As New List(Of String)
        Dim quantityArray As New List(Of String)
        Dim cnnOLEDB As New OleDbConnection
        Dim cmdOLEDB As New OleDbCommand
        Dim strConnectionString As String
        If SQLServerCheckBox.Checked Then
            strConnectionString = "Provider=SQLOLEDB;Data Source=" & dbServerTextBox.Text & ";Initial Catalog=" & dbNameTextBox.Text & ";User ID=" & dbUsernameTextBox.Text & ";Password=" & dbPasswordTextBox.Text & ";"
        Else
            strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & productDatabase
        End If

        cnnOLEDB.ConnectionString = strConnectionString
        cnnOLEDB.Open()
        cmdOLEDB.Connection = cnnOLEDB

        Dim spireCode As String
        Dim spireQuantity As Decimal = 0
        Dim spirePrice As Decimal = 0
        Dim spireCost As Decimal = 0
        Dim spireDesc As String
        Dim spireType As String
        Dim spireUOM As String
        Dim spireProductCode As String
        Dim spirePrefVendor As String

        Dim totalCount As Integer = 0
        Dim start As Integer = 0
        Dim limit As Integer = 100
        Dim recordCount As Integer = 0

        Dim count As Integer = 1
        'If item exists update quantity, cost, price
        Do
            Dim records = spireAPI.GetAllItems(defaultSyncWarehouseComboBox.SelectedItem.ToString, start, limit)

            For i As Integer = 0 To Integer.Parse(records("records").Length) - 1
                If count Mod 5 = 0 Then
                    tempcount.Text = "Checking: " & count.ToString()
                    Application.DoEvents()
                End If
                spireCode = records("records")(i)("partNo")
                spireType = records("records")(i)("type")
                Decimal.TryParse(records("records")(i)("availableQty"), spireQuantity)
                Decimal.TryParse(records("records")(i)("pricing")("sellPrice")(defaultSyncSellPriceLevelComboBox.SelectedIndex), spirePrice)
                Decimal.TryParse(records("records")(i)("costCurrent"), spireCost)

                If SQLServerCheckBox.Checked Then
                    cmdOLEDB.CommandText = "SELECT Price, Availability, Cost, Description, Manufacturer FROM dbo." & productDatabase & "_Products WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                Else
                    cmdOLEDB.CommandText = "SELECT Price, Availability, Cost, Description, Manufacturer FROM Products WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                End If

                Dim myReader As OleDbDataReader = cmdOLEDB.ExecuteReader(CommandBehavior.Default)

                If myReader.Read() Then
                    Dim qwPrice = myReader.GetDouble(0)
                    Dim qwQuantity = myReader.GetInt32(1)
                    Dim qwCost = myReader.GetDouble(2)
                    Dim qwDesc = myReader.GetString(3)
                    Dim qwManufacturer = myReader.GetString(4)
                    myReader.Close()
                    myReader.Dispose()
                    'Update Quantity
                    If Math.Floor(spireQuantity) <> qwQuantity Then
                        If SQLServerCheckBox.Checked Then
                            cmdOLEDB.CommandText = "UPDATE dbo." & productDatabase & "_Products SET Availability = " & spireQuantity & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                        Else
                            cmdOLEDB.CommandText = "UPDATE Products SET Availability = " & spireQuantity & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                        End If
                        cmdOLEDB.ExecuteNonQuery()
                        quantityArray.Add(spireCode)
                    End If
                    'Update Price
                    If syncPricingCheckBox.Checked Then
                        If spirePrice <> qwPrice Then
                            If SQLServerCheckBox.Checked Then
                                cmdOLEDB.CommandText = "UPDATE dbo." & productDatabase & "_Products SET Price = " & spirePrice & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                            Else
                                cmdOLEDB.CommandText = "UPDATE Products SET Price = " & spirePrice & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                            End If
                            cmdOLEDB.ExecuteNonQuery()
                            priceArray.Add(spireCode)
                        End If
                    End If
                    'Update Cost
                    If syncCostCheckBox.Checked Then
                        If spireCost <> qwCost Then
                            If SQLServerCheckBox.Checked Then
                                cmdOLEDB.CommandText = "UPDATE dbo." & productDatabase & "_Products SET Cost = " & spireCost & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                            Else
                                cmdOLEDB.CommandText = "UPDATE Products SET Cost = " & spireCost & " WHERE " & itemMap & " = '" & sqlEscape(spireCode) & "'"
                            End If
                            cmdOLEDB.ExecuteNonQuery()
                            costArray.Add(spireCode)
                        End If
                    End If
                    'Used for a one-time mass update
                    'If CUSTOMIZATION = Custom.Electromate Then
                    '    If Not qwManufacturer.ToLower().Contains("maxon") Then
                    '        Dim api_updateitem = New Dictionary(Of String, Object)

                    '        api_updateitem.Add("extendedDescription", qwDesc.Replace(Environment.NewLine, vbLf))
                    '        spireAPI.UpdateItem(records("records")(i)("id"), api_updateitem)
                    '    End If
                    'End If
                Else
                    If spireType = "K" Or spireType = "M" Then
                        dneBundleArray.Add(records("records")(i)("id"))
                    Else
                        dneArray.Add(records("records")(i)("id"))
                    End If
                    myReader.Close()
                    myReader.Dispose()
                End If
                count += 1

            Next

            Integer.TryParse(records("count"), totalCount)
            recordCount = records("records").Length
            start += limit
        Loop While totalCount <> (start - limit + recordCount)

        tempcount.Text = "Updating: " & count.ToString

        'If item doesn't exist add it to QW
        If dneArray.Count <> 0 Then
            Dim comm As OdbcCommand
            Dim dr As OdbcDataReader
            Dim sql As String
            Dim errCodes As String = ""
            If SQLServerCheckBox.Checked Then
                cmdOLEDB.CommandText = "SELECT MAX(ID) FROM dbo." & productDatabase & "_Products"
            Else
                cmdOLEDB.CommandText = "SELECT MAX(ID) FROM Products"
            End If
            Dim test = cmdOLEDB.ExecuteScalar()
            Dim nextProductNum As Integer
            If test Is Nothing Then
                nextProductNum = 1
            Else
                nextProductNum = Integer.Parse(test) + 1
            End If

            Dim connectionString As String = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"
            Using conn = New OdbcConnection(connectionString)
                conn.Open()

                'sql = String.Format("update inventory set udf_data = udf_data || '""QW_VendorP""=>""{1}""{2}{3}' :: hstore where id = {0}", _
                '                    itemID, _
                '                    .GetFieldValue("VendorPartNumber").ToString(), _
                '                    If(estore.Trim().Length <> 0, ",""QW_CT01""=>""" & estore & """", ""), _
                '                    If(customnumber.Trim().Length <> 0, ",""QW_CT02""=>""" & customnumber & """", ""))

                count = 1
                For Each spireItem In dneArray
                    If count Mod 5 = 0 Then
                        Application.DoEvents()
                        tempcount.Text = "Adding: " & count.ToString
                    End If
                    If stopImport Then
                        MessageBox.Show("Import Stopped.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        InvSyncButton.Enabled = True
                        InvSyncStopButton.Enabled = False
                        cnnOLEDB.Close()
                        cnnOLEDB.Dispose()
                        Return
                    End If

                    Dim itemObj = spireAPI.GetItem(spireItem)

                    spireCode = itemObj("partNo")
                    Decimal.TryParse(itemObj("pricing")(itemObj("sellMeasureCode"))("sellPrices")(defaultSyncSellPriceLevelComboBox.SelectedIndex), spirePrice)
                    Decimal.TryParse(itemObj("currentCost"), spireCost)
                    Decimal.TryParse(itemObj("availableQty"), spireQuantity)
                    spireDesc = If(itemObj("description") Is Nothing, "", itemObj("description"))
                    spireUOM = If(itemObj("sellMeasureCode") Is Nothing, "", itemObj("sellMeasureCode"))
                    spireProductCode = If(itemObj("productCode") Is Nothing, "", itemObj("productCode"))
                    Try
                        spirePrefVendor = itemObj("primaryVendor")("name")
                    Catch ex As Exception
                        spirePrefVendor = ""
                    End Try

                    'bvList = bvPrice
                    If CUSTOMIZATION = Custom.Electromate Then
                        sql = String.Format("select udf_data -> 'QW_VendorP' as vendorpartnumber, udf_data -> 'QW_CT01' as estore, udf_data -> 'QW_CT02' as customnumber from inventory where id = {0}", _
                                            spireItem)
                        comm = New OdbcCommand(sql, conn)
                        dr = comm.ExecuteReader()
                        dr.Read()
                        Dim VendorPartNumber = If(dr.IsDBNull(0), "", dr.GetString(0))
                        Dim eStoreNumber = If(dr.IsDBNull(1), "", dr.GetString(1))
                        Dim CustomNumber = If(dr.IsDBNull(2), "", dr.GetString(2))
                        dr.Close()
                        Dim extendedDescription = itemObj("extendedDescription")
                        If SQLServerCheckBox.Checked Then
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO dbo." & productDatabase & "_Products(" & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer, Created, LastModified, ManufacturerPartNumber, VendorPartNumber, CustomText01, CustomText02) " & _
                                    "VALUES('{0}','{1}',{2},{3},{4},'{5}','{6}',1,1,{7},'B','{8}',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'{9}','{10}','{11}','{12}')", _
                                    sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), If(extendedDescription Is Nothing, "", sqlEscape(itemObj("extendedDescription"))), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))), sqlEscape(spireDesc.Substring(0, Math.Min(spireDesc.Length, 40))), sqlEscape(VendorPartNumber.Substring(0, Math.Min(VendorPartNumber.Length, 40))), eStoreNumber, CustomNumber)
                        Else
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO Products(ID, " & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer, ManufacturerPartNumber, VendorPartNumber, CustomText01, CustomText02) " & _
                                    "VALUES({0},'{1}','{2}',{3},{4},{5},'{6}','{7}',1,1,{8},'B','{9}','{10}','{11}','{12}','{13}')", _
                                    nextProductNum, sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), If(extendedDescription Is Nothing, "", sqlEscape(itemObj("extendedDescription"))), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))), sqlEscape(spireDesc.Substring(0, Math.Min(spireDesc.Length, 40))), sqlEscape(VendorPartNumber.Substring(0, Math.Min(VendorPartNumber.Length, 40))), eStoreNumber, CustomNumber)
                        End If
                        comm.Dispose()
                    ElseIf CUSTOMIZATION = Custom.Primespec Then
                        Dim extendedDescription = If(itemObj("extendedDescription") Is Nothing, "", sqlEscape(itemObj("extendedDescription")))
                        If SQLServerCheckBox.Checked Then
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO dbo." & productDatabase & "_Products(" & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer, Created, LastModified, CustomMemo01) " & _
                                    "VALUES('{0}','{1}',{2},{3},{4},'{5}','{6}',1,1,{7},'B','{8}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP,'{9}')", _
                                    sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), sqlEscape(spireDesc), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))), extendedDescription)
                        Else
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO Products(ID, " & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer, CustomMemo01) " & _
                                    "VALUES({0},'{1}','{2}',{3},{4},{5},'{6}','{7}',1,1,{8},'B','{9}','{10}')", _
                                    nextProductNum, sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), sqlEscape(spireDesc), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))), extendedDescription)
                        End If
                    Else
                        If SQLServerCheckBox.Checked Then
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO dbo." & productDatabase & "_Products(" & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer, Created, LastModified) " & _
                                    "VALUES('{0}','{1}',{2},{3},{4},'{5}','{6}',1,1,{7},'B','{8}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)", _
                                    sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), sqlEscape(spireDesc), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))))
                        Else
                            cmdOLEDB.CommandText = String.Format( _
                                    "INSERT INTO Products(ID, " & itemMap & ", Description, Price, Cost, List, Category, UnitOfPricing, UnitOfPricingFactor, UnitOfMeasureFactor, Availability, TaxCode, Manufacturer) " & _
                                    "VALUES({0},'{1}','{2}',{3},{4},{5},'{6}','{7}',1,1,{8},'B','{9}')", _
                                    nextProductNum, sqlEscape(spireCode.Substring(0, Math.Min(spireCode.Length, 40))), sqlEscape(spireDesc), spirePrice, spireCost, spirePrice, sqlEscape(spireProductCode.Substring(0, Math.Min(spireProductCode.Length, 30))), sqlEscape(spireUOM.Substring(0, Math.Min(spireUOM.Length, 12))), spireQuantity, sqlEscape(spirePrefVendor.Substring(0, Math.Min(spirePrefVendor.Length, 40))))
                        End If
                    End If
                    Try
                        cmdOLEDB.ExecuteNonQuery()
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        errCodes &= spireCode & ", "
                    End Try

                    nextProductNum += 1
                    count += 1
                Next
            End Using

            tempcount.Text = "Adding: " & count.ToString

            If errCodes.Length <> 0 Then
                MessageBox.Show("Unable to add the following products: " & errCodes.Substring(0, errCodes.Length - 2), "Errror", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        'If dneBundleArray.Count <> 0 Then
        '    Dim BundleID As Integer
        '    With QWBack.Item(QWInstallation)
        '        Dim iError = .Database.OpenDB("RELATION", "QWTEST")

        '        With .Database.Recordset
        '            Dim errCodes As String = ""

        '            comm1 = New OdbcCommand(sql, conn)
        '            comm1.CommandTimeout = 600
        '            count = 1

        '            ' Invoke the Match method.
        '            Dim m As Match = Regex.Match(rButton.Text.Trim, _
        '                             "(.+) \( .* \)$", _
        '                             RegexOptions.IgnoreCase)

        '            ' If successful, write the group.
        '            If (m.Success) Then
        '                qwDatabase = m.Groups(1).Value
        '            End If
        '            For Each bvCode In dneBundleArray
        '                If stopImport Then
        '                    MessageBox.Show("Import Stopped.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '                    InvSyncButton.Enabled = True
        '                    InvSyncStopButton.Enabled = False
        '                    comm1.Dispose()
        '                    conn.Close()
        '                    conn.Dispose()
        '                    Return
        '                End If
        '                If count Mod 5 = 0 Then
        '                    Application.DoEvents()
        '                    tempcount.Text = "Adding: " & count.ToString
        '                End If
        '                MessageBox.Show("adding " & bvCode)
        '                sql = _
        '                    "SELECT sequence, bom.description, bom_items.description, bom_items.qty, bom_items.part_no " & _
        '                    "FROM public.bom " & _
        '                    "JOIN public.bom_items ON bom.id = bom_items.bom_id " & _
        '                    "WHERE bom.whse = '" & sqlEscape(defaultSyncWarehouseComboBox.SelectedItem.ToString) & "' AND bom.part_no = '" & sqlEscape(bvCode) & "' " & _
        '                    "ORDER BY sequence"

        '                comm1.CommandText = sql
        '                dr1 = comm1.ExecuteReader()
        '                Dim addHeader = True
        '                While dr1.Read()

        '                    MessageBox.Show("adding " & bvCode)
        '                    Try
        '                        bvSequence = dr1.GetInt32(0)
        '                    Catch ex As Exception
        '                        bvSequence = 0
        '                    End Try
        '                    Try
        '                        bvDesc = dr1.GetString(1).Trim
        '                    Catch ex As Exception
        '                        bvDesc = ""
        '                    End Try
        '                    Try
        '                        bvSubDesc = dr1.GetString(2).Trim
        '                    Catch ex As Exception
        '                        bvSubDesc = ""
        '                    End Try
        '                    Try
        '                        bvQuantity = dr1.GetDecimal(3)
        '                    Catch ex As Exception
        '                        bvQuantity = 0
        '                    End Try
        '                    Try
        '                        bvSubCode = dr1.GetString(4)
        '                    Catch ex As Exception
        '                        bvSubCode = ""
        '                    End Try
        '                    bvList = bvPrice
        '                    If addHeader Then
        '                        addHeader = False
        '                        .AddNew("BundleHeaders")
        '                        .SetFieldValue("Name", bvDesc)
        '                        .SetFieldValue("Description", bvDesc)
        '                        .SetFieldValue("Type", 1)
        '                        .SetFieldValue("PartNumber", sqlEscape(bvCode.Substring(0, Math.Min(bvCode.Length, 40))))
        '                        .Update()
        '                        BundleID = Integer.Parse(.GetFieldValue("ID"))
        '                    End If
        '                    .AddNew("BundleItems")
        '                    .SetFieldValue("BundleID", BundleID)
        '                    .SetFieldValue("BundleItemFlags", 0)
        '                    .SetFieldValue("LineItemAttributes", 0)
        '                    .SetFieldValue("LineItemType", 1)
        '                    .SetFieldValue("PartNumber", bvSubCode)
        '                    .SetFieldValue("Description", bvSubDesc)
        '                    .SetFieldValue("Pieces", bvQuantity)
        '                    .SetFieldValue("SourceDatabase", qwDatabase)
        '                    .SetFieldValue("SortOrder", bvSequence)
        '                    .Update()
        '                    count += 1
        '                End While

        '                dr1.Close()
        '            Next

        '            tempcount.Text = "Adding: " & count.ToString

        '            comm1.Dispose()

        '            If errCodes.Length <> 0 Then
        '                MessageBox.Show("Unable to add the products codes " & errCodes.Substring(0, errCodes.Length - 2), "Errror", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            End If
        '        End With
        '    End With
        'End If

        cnnOLEDB.Close()
        cnnOLEDB.Dispose()

        Dim MsgString As String
        If dneArray.Count = 0 And dneBundleArray.Count = 0 And quantityArray.Count = 0 And priceArray.Count = 0 Then
            MsgString = "No changes to synchronize"
        Else
            If dneArray.Count = 0 Then
                MsgString = "No new items added"
            Else
                MsgString = "Added " & dneArray.Count & " new item"
                If dneArray.Count > 1 Then
                    MsgString += "s"
                End If
            End If

            'If dneBundleArray.Count > 0 Then
            '    MsgString += ", added " & dneBundleArray.Count & " new bundle"
            '    If dneArray.Count > 1 Then
            '        MsgString += "s"
            '    End If
            'End If

            If quantityArray.Count = 0 And priceArray.Count = 0 Then
                MsgString += "."
            Else
                If quantityArray.Count <> 0 Then
                    MsgString += ", updated " & quantityArray.Count & " quantit"
                    If quantityArray.Count > 1 Then
                        MsgString += "ies"
                    Else
                        MsgString += "y"
                    End If
                End If
                If priceArray.Count <> 0 Then
                    MsgString += ", updated " & priceArray.Count & " price"
                    If priceArray.Count > 1 Then
                        MsgString += "s"
                    End If
                End If
                If costArray.Count <> 0 Then
                    MsgString += ", updated " & costArray.Count & " cost"
                    If costArray.Count > 1 Then
                        MsgString += "s"
                    End If
                End If
            End If
        End If

        MessageBox.Show(MsgString, "Synchronization Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        InvSyncButton.Enabled = True
        InvSyncStopButton.Enabled = False

    End Sub

    Private Function testDatabaseConn(Optional ByVal prodDB As String = "") As Boolean
        If SQLServerCheckBox.Checked And (dbServerTextBox.Text.Trim.Length = 0 Or dbNameTextBox.Text.Trim.Length = 0 Or dbUsernameTextBox.Text.Trim.Length = 0) Then
            MessageBox.Show("Please enter the SQL Server information to connect.", "Connection Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        Try
            Dim cnnOLEDB As New OleDbConnection

            Dim strConnectionString As String
            If SQLServerCheckBox.Checked Then
                strConnectionString = "Provider=SQLOLEDB;Data Source=" & dbServerTextBox.Text & ";Initial Catalog=" & dbNameTextBox.Text & ";User ID=" & dbUsernameTextBox.Text & ";Password=" & dbPasswordTextBox.Text & ";"
            Else
                strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & prodDB
            End If
            cnnOLEDB.ConnectionString = strConnectionString
            cnnOLEDB.Open()
            cnnOLEDB.Close()
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

#End Region

#Region "Order Transfer"

    Private Function getCategory(ByVal code As String, ByVal vendor As String) As String
        'ELECTROMATE FUNCTION
        Dim strConnectionString As String = Nothing
        Dim TableName = ""
        For i = 1 To QWApp.ProductDatabases.Count
            If vendor = QWApp.ProductDatabases.Item(i).Name Then
                If SQLServerCheckBox.Checked Then
                    TableName = QWApp.ProductDatabases.Item(i).DbProductsTable
                    strConnectionString = "Provider=SQLOLEDB;Data Source=" & dbServerTextBox.Text & ";Initial Catalog=" & dbNameTextBox.Text & ";User ID=" & dbUsernameTextBox.Text & ";Password=" & dbPasswordTextBox.Text & ";"
                Else
                    strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & QWApp.ProductDatabases.Item(i).DataSource
                End If
                Exit For
            End If
        Next
        If strConnectionString Is Nothing Then
            Return ""
        End If

        Dim itemMap As String = itemMapComboBox.SelectedItem
        Dim Category = ""
        Using cnnOLEDB As New OleDbConnection
            cnnOLEDB.ConnectionString = strConnectionString
            cnnOLEDB.Open()

            Using cmdOLEDB As New OleDbCommand
                cmdOLEDB.Connection = cnnOLEDB
                If SQLServerCheckBox.Checked Then
                    cmdOLEDB.CommandText = "SELECT Category FROM dbo." & TableName & " WHERE " & itemMap & " = '" & sqlEscape(code) & "'"
                Else
                    cmdOLEDB.CommandText = "SELECT Category FROM Products WHERE " & itemMap & " = '" & sqlEscape(code) & "'"
                End If
                Using myReader As OleDbDataReader = cmdOLEDB.ExecuteReader(CommandBehavior.Default)
                    If myReader.Read() Then
                        Category = myReader.GetString(0).Trim()
                    End If
                End Using
            End Using
        End Using
        Return Category
    End Function

    Private Function getTerritory(ByVal postalcode As String, ByRef terrdescription As String) As String
        'ELECTROMATE FUNCTION
        Dim xlApp As Excel.Application = Nothing
        Dim xlWorkBook As Excel.Workbook = Nothing
        Dim xlWorkSheet As Excel.Worksheet = Nothing
        Dim Territory As String = Nothing

        Try
            xlApp = CreateObject("Excel.Application")
            xlWorkBook = xlApp.Workbooks.Open(terrSpreadsheetTextBox.Text, [ReadOnly]:=True)
            xlWorkSheet = xlWorkBook.Worksheets(terrWorksheetTextBox.Text)

            'loop through each row
            For X As Integer = 1 To xlWorkSheet.Range("A1048576").End(Excel.XlDirection.xlUp).Row Step 1
                'check if the cell value matches the search string.
                If xlWorkSheet.Cells(X, 1).value = postalcode Then
                    Territory = xlWorkSheet.Cells(X, 2).value.ToString().Trim()
                    Exit For
                End If
            Next
        Catch ex As Exception

        Finally
            If xlWorkBook IsNot Nothing Then
                xlWorkBook.Close(False)
            End If
            If xlApp IsNot Nothing Then
                xlApp.Quit()
            End If
            releaseObject(xlApp)
            releaseObject(xlWorkBook)
            releaseObject(xlWorkSheet)
        End Try

        If Territory Is Nothing Then
            Return ""
        End If

        If spireAPI.CheckTerritory(Territory, terrdescription) Then
            Return Territory
        Else
            Return ""
        End If

        Return Nothing
    End Function

    Private Function transferOrderSpire(docID As String, Optional showSuccess As Boolean = True) As Boolean
        'Main function to create sales order in Spire
        'Step 1: Check if customer exists
        'Step 2: Prompt user to ask if new customer should be generated
        'Step 3: Add new customer
        'Step 3.1: Query to get bill-to and ship-to details
        'Step 3.2: Lookup the 3 character country code used by Spire
        'Step 3.3: Ask user for new customer details specific to Spire, validate the data before continuing
        'Step 3.4: New Customer; Add Ship-to details
        'Step 3.4.1: Only continue if the ship-to details are different that bill-to details
        'Step 3.4.2: Ask user for new ship-to details specific to Spire, validate the data before continuing
        'Step 3.5: Insert customer
        'Step 3.6: Existing Customer; Add Ship-to details
        'Step 3.6.1: Only continue if the ship-to details are different that bill-to details
        'Step 3.6.2: Ask user for new ship-to details specific to Spire, validate the data before continuing
        'Step 4: Set customer ID
        'Step 5: Set order status
        'Step 6: Set FOB
        'Step 7: Set PO number
        'Step 8: Set freight
        'Step 9: Set terms
        'Step 10: Set order date
        'Step 11: Set required date
        'Step 12: Set ship-to ID
        'Step 13: Set salesperson
        'Step 14: Set territory
        'Step 15: Add items - loop through each item
        'Step 15.1: Bundles - reset flag and bundle values, check when the bundle ends
        'Step 15.2: Add comment line
        'Step 15.3: Bundles - get bundle header values
        'Step 15.4: Check if warehouse exists
        'Step 15.5: Check if item exists
        'Step 15.6: Prompt user to ask if new item should be generated, use as non-stocked, or abort
        'Step 15.7: Add new item
        'Step 15.7.1: Validate part number is not too long, everything else will be truncated
        'Step 15.7.2: Set warehouse
        'Step 15.7.3: Set part number
        'Step 15.7.4: Set current cost
        'Step 15.7.5: Set description
        'Step 15.7.6: Read default UOM from database
        'Step 15.7.7: Set pricing
        'Step 15.7.8: Insert item
        'Step 15.8: Item exists; get item values
        'Step 15.9: Check if item price in quotewerks is different from spire; prompt user to choose price to use
        'Step 15.10: Bundles - combine discounts for bundles
        'Step 15.11: Add non-stocked item to the order
        'Step 15.11.1: Set warehouse
        'Step 15.11.2: Set part number
        'Step 15.11.3: Set description
        'Step 15.11.4: Set order quantity
        'Step 15.11.5: Set line discount
        'Step 15.11.6: Set unit price
        'Step 15.12: Add regular item to the order
        'Step 15.12.1: Set item id
        'Step 15.12.2: Set order quantity
        'Step 15.12.3: Set line discount
        'Step 15.12.4: Set unit price
        'Step 16: Insert order
        Dim bundleDiscount As Integer = 0
        Dim itemMap As String = itemMapComboBox.SelectedItem
        Dim customerNoMap As String = customerNoMapComboBox.SelectedItem
        Dim territoryMap As String = territoryMapComboBox.SelectedItem

        Dim sql As String
        Dim rowsAffected As Integer

        With QWBack.Item(QWInstallation)
            Dim iError = .Database.OpenDB("DOCS", "QWTEST")

            With .Database.Recordset

                Dim companyName As String = Nothing

                Dim OrdNo As String = ""
                Dim ShipTo As String = ""
                Dim ShipToName As String = ""
                Dim custNo As String = ""
                Dim custName As String = ""
                Dim ordDate = "NULL"
                Dim ReqDate = "NULL"
                Dim DocumentNo As String = ""
                Dim FOB As String = ""
                Dim PONo As String = ""
                Dim ShipVia As String = ""
                Dim Freight As Decimal = 0.0
                Dim SlspnNo As String = ""
                Dim SlspnName As String = ""
                Dim TermsDesc As String = ""
                Dim IntroductionNotes As String = ""
                Dim PurchasingNotes As String = ""
                Dim ClosingNotes As String = ""
                Dim InternalNotes As String = ""
                Dim AltCurrency As String = ""
                Dim NON_STOCKED As Boolean = False

                'Electromate
                Dim customVariable01 As String = ""
                Dim customVariable02 As String = ""
                Dim customVariable03 As String = ""
                Dim customVariable04 As String = ""
                Dim customVariable05 As String = ""
                Dim customVariable06 As String = ""
                Dim customVariable07 As String = ""
                Dim costsToUpdate As List(Of Tuple(Of Int32, Decimal)) = New List(Of Tuple(Of Int32, Decimal))

                Dim custObj As Object

                Dim connectionString As String = "DSN=" & DSNTextBox.Text & ";SERVER=" & ServerTextBox.Text & ";"

                'Step 1
                Dim lookupError = False
                Dim customerExists As Boolean = False
                .QueryEx("SELECT DocNo," & customerNoMap & "  FROM DocumentHeaders where ID = " & sqlEscape(docID))
                If .RecordCount > 0 Then
                    .MoveFirst()
                    If Not .EOF Then
                        DocumentNo = .GetFieldValue("DocNo").ToString().Trim()
                        companyName = .GetFieldValue(customerNoMap).ToString().Trim().ToUpper()
                        If companyName.Length() = 0 Then
                            customerExists = False
                        Else

                            custObj = spireAPI.FindCustomer(companyName)
                            If custObj("count") = "1" Then
                                customerExists = True
                                custObj = spireAPI.GetCustomer(custObj("records")(0)("id"))
                            End If
                        End If
                    Else
                        lookupError = True
                    End If
                Else
                    lookupError = True
                End If

                If lookupError Then
                    MessageBox.Show("Error locating document. It may have been changed, please click Lookup to search again.", "Document Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If

                Dim customerAdded As Boolean = False

                'Step 2
                'If not take action based on the option chosen on options tab (Actumatically generate or ask user)
                Dim addCustomer As Boolean = False
                If Not customerExists Then
                    If newCustomerComboBox.SelectedIndex = 1 Then
                        addCustomer = True
                    Else
                        Dim ret = MessageBox.Show("Do you wish to add this customer to Spire?", "Customer Does Not Exist", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                        If ret = DialogResult.Yes Then
                            addCustomer = True
                        ElseIf ret = DialogResult.Cancel Or ret = DialogResult.No Then
                            Return False
                        End If
                    End If
                End If

                If CUSTOMIZATION = Custom.Electromate And customerExists Then
                    .QueryEx("SELECT AlternateCurrency," & customerNoMap & "  FROM DocumentHeaders where ID = " & sqlEscape(docID))
                    Dim altcur() = .GetFieldValue("AlternateCurrency").ToString().Split(Chr(4))

                    Dim enteredCustNo = .GetFieldValue(customerNoMap).ToString().Trim()

                    If altcur.Count = 3 And enteredCustNo.Length > 0 Then
                        If altcur(0) = "USD" And enteredCustNo.Substring(0, 1) <> "9" Then
                            MessageBox.Show("The customer number entered in the Custom tab must begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False
                        ElseIf altcur(0) = "CAD" And enteredCustNo.Substring(0, 1) = "9" Then
                            MessageBox.Show("The customer number entered in the Custom tab must NOT begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False
                        End If
                    End If
                End If

                'New Customer/Ship-To form
                Dim tax1No As Integer = -1
                Dim tax2No As Integer = -1
                Dim ex1No As String
                Dim ex2No As String
                Dim sellPriceLevel As Integer = 1
                Dim shipTax1No As Integer = -1
                Dim shipTax2No As Integer = -1
                Dim shipEx1No As String
                Dim shipEx2No As String
                Dim shipSellPriceLevel As Integer = 1
                Dim currency As String = ""
                Dim newCustNo As String
                Dim newCustName As String
                Dim newShipToID As String

                'Step 3
                If addCustomer Then
                    Dim validCustomerName = False

                    Dim newForm = New NewCustForm(spireAPI)
                    newForm.Text = newForm.Text & " - " & DocumentNo

                    If sequentialCustCheckBox.Checked Then
                        newForm.CustNoTextBox.Text = "<Sequential>"
                        newForm.CustNoTextBox.Enabled = False
                    End If

                    Dim nextCustNo As String = ""

                    'Step 3.1: Query to get bill-to and ship-to details
                    .QueryEx("SELECT " &
                             If(CUSTOMIZATION = Custom.Electromate, "CustomText11, AlternateCurrency, ", "") &
                             If(CUSTOMIZATION = Custom.Primespec, "CustomText02, ", "") &
                             "BillToCompany," &
                             "BillToContact," &
                             "BillToAddress1," &
                             "BillToAddress2," &
                             "BillToAddress3," &
                             "BillToCity," &
                             "BillToState," &
                             "BillToCountry," &
                             "BillToPostalCode," &
                             "BillToEmail," &
                             "BillToFax," &
                             "BillToFaxExt," &
                             "BillToPhone," &
                             "BillToPhoneExt, " &
                             "ShipToCompany," &
                             "ShipToContact," &
                             "ShipToAddress1," &
                             "ShipToAddress2," &
                             "ShipToAddress3," &
                             "ShipToCity," &
                             "ShipToState," &
                             "ShipToCountry," &
                             "ShipToPostalCode," &
                             "ShipToPhone," &
                             "ShipToPhoneExt," &
                             "ShipToFax," &
                             "ShipToFaxExt," &
                             "ShipToEmail, " &
                             "SalesRep, " &
                             customerNoMap & " " &
                             "FROM DocumentHeaders WHERE ID = " & sqlEscape(docID))
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        If Not .EOF Then
                            If CUSTOMIZATION = Custom.Electromate Then
                                If .GetFieldValue("CustomText11").ToString().Trim().Length = 0 Then
                                    MessageBox.Show("Zoho Account ID is empty for this customer. Please update it before continuing.", "Missing Zoho Account ID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Return False
                                End If
                            End If

                            Dim Company As String = .GetFieldValue("BillToCompany").ToString().Trim()
                            Dim Contact As String = .GetFieldValue("BillToContact").ToString().Trim()
                            If Company.Length > 0 Then
                                custName = Company
                            ElseIf Contact.Length > 0 Then
                                custName = Contact
                            Else
                                MessageBox.Show("Bill-to Company or Contact fields must be filled in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return False
                            End If
                            custName = custName.Substring(0, Math.Min(custName.Length, 60))
                            Dim Addr1 As String = .GetFieldValue("BillToAddress1").ToString().Trim()
                            Dim Addr2 As String = .GetFieldValue("BillToAddress2").ToString().Trim()
                            Dim Addr3 As String = .GetFieldValue("BillToAddress3").ToString().Trim()
                            Dim City As String = .GetFieldValue("BillToCity").ToString().Trim()
                            Dim ProvState As String = .GetFieldValue("BillToState").ToString().Trim()
                            Dim Country As String = .GetFieldValue("BillToCountry").ToString().Trim()
                            Dim Postal As String = .GetFieldValue("BillToPostalCode").ToString().Trim()
                            Dim Email As String = .GetFieldValue("BillToEmail").ToString().Trim()
                            Dim Fax As String = .GetFieldValue("BillToFax").ToString().Trim() & .GetFieldValue("BillToFaxExt").ToString().Trim()
                            Dim Phone As String = .GetFieldValue("BillToPhone").ToString().Trim() & .GetFieldValue("BillToPhoneExt").ToString().Trim()

                            Fax = Regex.Replace(Fax, "[^0-9]", "")
                            Fax = Fax.Substring(0, Math.Min(30, Fax.Length))
                            Phone = Regex.Replace(Phone, "[^0-9]", "")
                            Phone = Phone.Substring(0, Math.Min(30, Phone.Length))

                            If Fax.Length > 0 AndAlso Fax.Substring(0, 1) = "1" Then
                                Fax = Fax.Substring(1)
                            End If
                            If Phone.Length > 0 AndAlso Phone.Substring(0, 1) = "1" Then
                                Phone = Phone.Substring(1)
                            End If

                            ProvState = ProvState.Substring(0, Math.Min(ProvState.Length, 2))

                            'Dim sanitizedPhone = Regex.Replace(customVariable03, "[^0-9]", "")
                            'sanitizedPhone = sanitizedPhone.Substring(0, Math.Min(10, sanitizedPhone.Length))

                            'Step 3.2: Lookup the 3 character country code used by Spire
                            If Country.Length > 0 Then
                                If Country = "USA" Then
                                    Country = "United States"
                                End If
                                If countryMapDict.ContainsKey(Country.ToLower()) Then
                                    Country = countryMapDict(Country.ToLower())
                                Else
                                    Country = ""
                                End If
                            End If

                            Dim enteredCustNo = .GetFieldValue(customerNoMap).ToString().Trim()

                            If CUSTOMIZATION = Custom.Electromate Then
                                Dim altcur() = .GetFieldValue("AlternateCurrency").ToString().Split(Chr(4))

                                If altcur.Count = 3 And enteredCustNo.Length > 0 Then
                                    If altcur(0) = "USD" And enteredCustNo.Substring(0, 1) <> "9" Then
                                        MessageBox.Show("The customer number entered in the Custom tab must begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Return False
                                    ElseIf altcur(0) = "CAD" And enteredCustNo.Substring(0, 1) = "9" Then
                                        MessageBox.Show("The customer number entered in the Custom tab must NOT begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Return False
                                    End If
                                End If
                            End If

                            newForm.CustNameTextBox.Text = custName
                            newForm.CustNoTextBox.Text = enteredCustNo

                            'Step 3.3: Ask user for new customer details specific to Spire, validate the data before continuing
                            Do
                                newForm.ShowDialog()
                                If newForm.continueAdding = False Then
                                    Return False
                                Else
                                    If sequentialCustCheckBox.Checked Then
                                        validCustomerName = True
                                    Else
                                        newCustNo = newForm.CustNoTextBox.Text
                                        newCustName = newForm.CustNameTextBox.Text
                                        If newCustNo.Trim().Length = 0 Then
                                            MessageBox.Show("Please enter a customer number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        ElseIf newCustName.Trim().Length = 0 Then
                                            MessageBox.Show("Please enter a customer name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Else
                                            Dim check_custObj = spireAPI.FindCustomer(newCustNo.Trim().ToUpper())
                                            If check_custObj("count") = "1" Then
                                                MessageBox.Show("The entered customer number already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            Else
                                                If CUSTOMIZATION = Custom.Electromate Then
                                                    Dim altcur() = .GetFieldValue("AlternateCurrency").ToString().Split(Chr(4))

                                                    If altcur.Count = 3 And altcur(0) = "USD" And newCustNo.Trim().Substring(0, 1) <> "9" Then
                                                        MessageBox.Show("The customer number must begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    ElseIf altcur.Count = 3 And altcur(0) = "CAD" And newCustNo.Trim().Substring(0, 1) = "9" Then
                                                        MessageBox.Show("The customer number must NOT begin with a 9.", "Customer Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    Else
                                                        validCustomerName = True
                                                        custNo = newCustNo.Trim().ToUpper()
                                                    End If
                                                Else
                                                    validCustomerName = True
                                                    custNo = newCustNo.Trim().ToUpper()
                                                    custName = newCustName.Trim()
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Loop Until validCustomerName = True

                            If newForm.slsTax1ComboBox.SelectedIndex > 0 Then
                                Integer.TryParse(newForm.slsTax1ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), tax1No)
                            End If
                            If newForm.slsTax2ComboBox.SelectedIndex > 0 Then
                                Integer.TryParse(newForm.slsTax2ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), tax2No)
                            End If
                            ex1No = newForm.ex1NumberTextBox.Text.Trim()
                            ex2No = newForm.ex2NumberTextBox.Text.Trim()
                            sellPriceLevel = newForm.sellingPriceLevelComboBox.SelectedIndex + 1

                            If newForm.currencySelectComboBox.SelectedIndex <> 0 Then
                                currency = newForm.currencySelectComboBox.SelectedItem.ToString.Substring(0, 3)
                            End If

                            Dim api_addcustomer = New Dictionary(Of String, Object)
                            If Not sequentialCustCheckBox.Checked Then
                                api_addcustomer.Add("customerNo", custNo)
                            End If
                            If currency.Length > 0 Then
                                api_addcustomer.Add("currency", currency)
                            End If
                            api_addcustomer.Add("name", custName)

                            api_addcustomer.Add("address", New Dictionary(Of String, Object))
                            api_addcustomer("address").Add("streetAddress", Addr1 & vbLf & Addr2 & vbLf & Addr3 & vbLf)
                            api_addcustomer("address").Add("city", City)
                            api_addcustomer("address").Add("postalCode", Postal)
                            api_addcustomer("address").Add("provState", ProvState)
                            api_addcustomer("address").Add("country", Country)
                            api_addcustomer("address").Add("name", custName)
                            api_addcustomer("address").Add("email", Email)
                            'api_addcustomer("address").Add("website", custName)
                            api_addcustomer("address").Add("phone", New Dictionary(Of String, Object))
                            api_addcustomer("address")("phone").Add("number", Phone)
                            api_addcustomer("address")("phone").Add("format", 1)
                            api_addcustomer("address").Add("fax", New Dictionary(Of String, Object))
                            api_addcustomer("address")("fax").Add("number", Fax)
                            api_addcustomer("address")("fax").Add("format", 1)
                            api_addcustomer("address").Add("sellLevel", sellPriceLevel)

                            If tax1No > 0 Or tax2No > 0 Then
                                api_addcustomer("address").Add("salesTaxes", New List(Of Dictionary(Of String, Object)))
                                api_addcustomer("address")("salesTaxes").Add(New Dictionary(Of String, Object))
                                api_addcustomer("address")("salesTaxes").Add(New Dictionary(Of String, Object))
                                api_addcustomer("address")("salesTaxes").Add(New Dictionary(Of String, Object))
                                api_addcustomer("address")("salesTaxes").Add(New Dictionary(Of String, Object))
                                If tax1No > 0 Then
                                    api_addcustomer("address")("salesTaxes")(0).Add("code", tax1No)
                                    api_addcustomer("address")("salesTaxes")(0).Add("exempt", ex1No)
                                End If
                                If tax2No > 0 Then
                                    api_addcustomer("address")("salesTaxes")(1).Add("code", tax2No)
                                    api_addcustomer("address")("salesTaxes")(1).Add("exempt", ex2No)
                                End If
                            End If

                            If CUSTOMIZATION = Custom.Electromate Then
                                If Postal.Length > 3 Then
                                    Dim TerrDescription As String = ""
                                    Dim Territory = getTerritory(Postal.Substring(0, 3), TerrDescription)

                                    If Territory.Length > 0 Then
                                        api_addcustomer("address").Add("territory", New Dictionary(Of String, Object))
                                        api_addcustomer("address")("territory").Add("code", Territory)
                                        api_addcustomer("address")("territory").Add("description", TerrDescription)
                                    End If
                                End If
                            ElseIf CUSTOMIZATION = Custom.Norwood Then
                                Dim TerrCode = ""
                                If Country = "CAN" Then
                                    TerrCode = "CAN"
                                ElseIf Country = "USA" Then
                                    TerrCode = "USA"
                                Else
                                    TerrCode = ""
                                End If
                                If TerrCode.Length > 0 Then
                                    Dim TerrDescription = ""
                                    spireAPI.CheckTerritory(TerrCode, TerrDescription)
                                    api_addcustomer("address").Add("territory", New Dictionary(Of String, Object))
                                    api_addcustomer("address")("territory").Add("code", TerrCode)
                                    api_addcustomer("address")("territory").Add("description", TerrDescription)
                                End If

                                'SALES PERSON
                                'Step 13: Set salesperson
                                Dim qwSalesRep = .GetFieldValue("SalesRep").ToString().Trim()
                                If comboBoxDict.ContainsKey(qwSalesRep) Then
                                    Dim sageSalesRep = comboBoxDict(qwSalesRep).SelectedItem.ToString
                                    If sageSalesRep <> "--" Then
                                        Dim salespeopleObj = spireAPI.GetSalespeople()

                                        For i As Integer = 0 To Integer.Parse(salespeopleObj("count")) - 1
                                            If salespeopleObj("records")(i)("name") = sageSalesRep Then
                                                api_addcustomer("address").Add("salesperson", New Dictionary(Of String, Object))
                                                api_addcustomer("address")("salesperson").Add("code", salespeopleObj("records")(i)("code"))
                                                api_addcustomer("address")("salesperson").Add("name", salespeopleObj("records")(i)("name"))
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If

                            End If

                            'Step 3.4: Add Ship-to details
                            Dim shipCompany As String = .GetFieldValue("ShipToCompany").ToString().Trim()
                            Dim shipContact As String = .GetFieldValue("ShipToContact").ToString().Trim()
                            If shipCompany.Length > 0 Then
                                ShipTo = shipCompany
                                ShipToName = shipCompany
                            ElseIf shipContact.Length > 0 Then
                                ShipTo = shipContact
                                ShipToName = shipContact
                            End If
                            If ShipTo.Length > 0 Then
                                ShipTo = ShipTo.ToUpper().Substring(0, Math.Min(ShipTo.Length, 20))
                                ShipToName = ShipToName.Substring(0, Math.Min(ShipToName.Length, 60))
                                Dim shipAddr1 As String = .GetFieldValue("ShipToAddress1").ToString().Trim()
                                Dim shipAddr2 As String = .GetFieldValue("ShipToAddress2").ToString().Trim()
                                Dim shipAddr3 As String = .GetFieldValue("ShipToAddress3").ToString().Trim()
                                Dim shipCity As String = .GetFieldValue("ShipToCity").ToString().Trim()
                                Dim shipProvState As String = .GetFieldValue("ShipToState").ToString().Trim()
                                Dim shipCountry As String = .GetFieldValue("ShipToCountry").ToString().Trim()
                                Dim shipPostal As String = .GetFieldValue("ShipToPostalCode").ToString().Trim()

                                shipProvState = shipProvState.Substring(0, Math.Min(shipProvState.Length, 2))

                                If shipCountry.Length > 0 Then
                                    If shipCountry = "USA" Then
                                        shipCountry = "United States"
                                    End If
                                    If countryMapDict.ContainsKey(shipCountry.ToLower()) Then
                                        shipCountry = countryMapDict(shipCountry.ToLower())
                                    Else
                                        shipCountry = ""
                                    End If
                                End If

                                'Step 3.4.1: Only continue if the ship-to details are different that bill-to details
                                If shipAddr1 <> Addr1 Or shipAddr2 <> Addr2 Or shipAddr3 <> Addr3 Or shipCity <> City Or shipProvState <> ProvState Or shipCountry <> Country Or shipPostal <> Postal Then
                                    Dim shipPhone As String = .GetFieldValue("ShipToPhone").Trim & .GetFieldValue("ShipToPhoneExt").ToString().Trim()
                                    Dim shipFax As String = .GetFieldValue("ShipToFax").ToString().Trim() & .GetFieldValue("ShipToFaxExt").ToString().Trim()
                                    Dim shipEmail As String = .GetFieldValue("ShipToEmail").ToString().Trim()

                                    shipFax = Regex.Replace(shipFax, "[^0-9]", "")
                                    shipFax = shipFax.Substring(0, Math.Min(30, shipFax.Length))
                                    shipPhone = Regex.Replace(shipPhone, "[^0-9]", "")
                                    shipPhone = shipPhone.Substring(0, Math.Min(30, shipPhone.Length))

                                    If shipFax.Length > 0 AndAlso shipFax.Substring(0, 1) = "1" Then
                                        shipFax = shipFax.Substring(1)
                                    End If
                                    If shipPhone.Length > 0 AndAlso shipPhone.Substring(0, 1) = "1" Then
                                        shipPhone = shipPhone.Substring(1)
                                    End If

                                    Dim newShipForm = New NewShipToForm(spireAPI)
                                    newShipForm.ShipToIDTextBox.Text = ShipTo
                                    newShipForm.ShipToNameTextBox.Text = ShipToName
                                    newShipForm.slsTax1ComboBox.SelectedIndex = newForm.slsTax1ComboBox.SelectedIndex
                                    newShipForm.slsTax2ComboBox.SelectedIndex = newForm.slsTax2ComboBox.SelectedIndex
                                    newShipForm.sellingPriceLevelComboBox.SelectedIndex = newForm.sellingPriceLevelComboBox.SelectedIndex

                                    'Step 3.4.2: Ask user for new ship-to details specific to Spire, validate the data before continuing
                                    Dim validShipToID = False
                                    Do
                                        newShipForm.ShowDialog()
                                        If newShipForm.continueAdding = False Then
                                            Return False
                                        ElseIf newShipForm.skipShipTo = True Then
                                            validShipToID = True
                                            ShipTo = ""
                                        Else
                                            newShipToID = newShipForm.ShipToIDTextBox.Text
                                            If newShipToID.Trim().Length = 0 Then
                                                MessageBox.Show("Please enter a ship-to ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            Else
                                                validShipToID = True
                                                ShipTo = newShipToID
                                                ShipToName = newShipForm.ShipToNameTextBox.Text.Trim()
                                            End If
                                        End If
                                    Loop Until validShipToID = True

                                    'Only add the ship-to if the Skip button wasn't clicked
                                    If newShipForm.skipShipTo = False Then
                                        If newShipForm.slsTax1ComboBox.SelectedIndex > 0 Then
                                            Integer.TryParse(newShipForm.slsTax1ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), shipTax1No)
                                        End If
                                        If newShipForm.slsTax2ComboBox.SelectedIndex > 0 Then
                                            Integer.TryParse(newShipForm.slsTax2ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), shipTax2No)
                                        End If
                                        shipEx1No = newShipForm.ex1NumberTextBox.Text.Trim()
                                        shipEx2No = newShipForm.ex2NumberTextBox.Text.Trim()
                                        shipSellPriceLevel = newShipForm.sellingPriceLevelComboBox.SelectedIndex + 1

                                        'Dim sanitizedPhone = Regex.Replace(customVariable03, "[^0-9]", "")
                                        'sanitizedPhone = sanitizedPhone.Substring(0, Math.Min(10, sanitizedPhone.Length))

                                        api_addcustomer.Add("shippingAddresses", New List(Of Dictionary(Of String, Object)))
                                        api_addcustomer("shippingAddresses").Add(New Dictionary(Of String, Object))
                                        api_addcustomer("shippingAddresses")(0).Add("shipId", ShipTo)
                                        api_addcustomer("shippingAddresses")(0).Add("name", ShipToName)
                                        api_addcustomer("shippingAddresses")(0).Add("streetAddress", shipAddr1 & vbLf & shipAddr2 & vbLf & shipAddr3 & vbLf)
                                        api_addcustomer("shippingAddresses")(0).Add("city", shipCity)
                                        api_addcustomer("shippingAddresses")(0).Add("postalCode", shipPostal)
                                        api_addcustomer("shippingAddresses")(0).Add("provState", shipProvState)
                                        api_addcustomer("shippingAddresses")(0).Add("country", shipCountry)
                                        api_addcustomer("shippingAddresses")(0).Add("email", shipEmail)
                                        api_addcustomer("shippingAddresses")(0).Add("phone", New Dictionary(Of String, Object))
                                        api_addcustomer("shippingAddresses")(0)("phone").Add("number", shipPhone)
                                        api_addcustomer("shippingAddresses")(0)("phone").Add("format", 1)
                                        api_addcustomer("shippingAddresses")(0).Add("fax", New Dictionary(Of String, Object))
                                        api_addcustomer("shippingAddresses")(0)("fax").Add("number", shipFax)
                                        api_addcustomer("shippingAddresses")(0)("fax").Add("format", 1)
                                        api_addcustomer("shippingAddresses")(0).Add("sellLevel", shipSellPriceLevel)

                                        If CUSTOMIZATION = Custom.Electromate Then
                                            If shipPostal.Length > 3 Then
                                                Dim TerrDescription As String = ""
                                                Dim Territory = getTerritory(shipPostal.Substring(0, 3), TerrDescription)
                                                If Territory.Length > 0 Then
                                                    api_addcustomer("shippingAddresses")(0).Add("territory", New Dictionary(Of String, Object))
                                                    api_addcustomer("shippingAddresses")(0)("territory").Add("code", Territory)
                                                    api_addcustomer("shippingAddresses")(0)("territory").Add("description", TerrDescription)
                                                End If
                                            End If
                                        ElseIf CUSTOMIZATION = Custom.Norwood Then
                                            Dim TerrCode = ""
                                            If shipCountry = "Canada" Then
                                                TerrCode = "CAN"
                                            ElseIf shipCountry = "USA" Then
                                                TerrCode = "USA"
                                            Else
                                                TerrCode = ""
                                            End If
                                            If TerrCode.Length > 0 Then
                                                Dim TerrDescription = ""
                                                spireAPI.CheckTerritory(TerrCode, TerrDescription)
                                                api_addcustomer("shippingAddresses")(0).Add("territory", New Dictionary(Of String, Object))
                                                api_addcustomer("shippingAddresses")(0)("territory").Add("code", TerrCode)
                                                api_addcustomer("shippingAddresses")(0)("territory").Add("description", TerrDescription)
                                            End If

                                            'SALES PERSON
                                            Dim qwSalesRep = .GetFieldValue("SalesRep").ToString().Trim()
                                            If comboBoxDict.ContainsKey(qwSalesRep) Then
                                                Dim sageSalesRep = comboBoxDict(qwSalesRep).SelectedItem.ToString
                                                If sageSalesRep <> "--" Then
                                                    Dim salespeopleObj = spireAPI.GetSalespeople()

                                                    For i As Integer = 0 To Integer.Parse(salespeopleObj("count")) - 1
                                                        If salespeopleObj("records")(i)("name") = sageSalesRep Then
                                                            api_addcustomer("shippingAddresses")(0).Add("salesperson", New Dictionary(Of String, Object))
                                                            api_addcustomer("shippingAddresses")(0)("salesperson").Add("code", salespeopleObj("records")(i)("code"))
                                                            api_addcustomer("shippingAddresses")(0)("salesperson").Add("name", salespeopleObj("records")(i)("name"))
                                                            Exit For
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        End If

                                        If shipTax1No > 0 Or shipTax2No > 0 Then
                                            api_addcustomer("shippingAddresses")(0).Add("salesTaxes", New List(Of Dictionary(Of String, Object)))
                                            api_addcustomer("shippingAddresses")(0)("salesTaxes").Add(New Dictionary(Of String, Object))
                                            api_addcustomer("shippingAddresses")(0)("salesTaxes").Add(New Dictionary(Of String, Object))
                                            api_addcustomer("shippingAddresses")(0)("salesTaxes").Add(New Dictionary(Of String, Object))
                                            api_addcustomer("shippingAddresses")(0)("salesTaxes").Add(New Dictionary(Of String, Object))
                                            If shipTax1No > 0 Then
                                                api_addcustomer("shippingAddresses")(0)("salesTaxes")(0).Add("code", shipTax1No)
                                                api_addcustomer("shippingAddresses")(0)("salesTaxes")(0).Add("exempt", shipEx1No)
                                            End If
                                            If shipTax2No > 0 Then
                                                api_addcustomer("shippingAddresses")(0)("salesTaxes")(1).Add("code", shipTax2No)
                                                api_addcustomer("shippingAddresses")(0)("salesTaxes")(1).Add("exempt", shipEx2No)
                                            End If
                                        End If
                                    End If
                                Else
                                    ShipTo = ""
                                End If
                            End If

                            'Step 3.5: Insert customer
                            status = spireAPI.AddCustomer(api_addcustomer)

                            If status Is Nothing Then
                                MessageBox.Show("Some Error")
                                Return False
                            Else
                                custObj = spireAPI.GetCustomer(status)

                                If CUSTOMIZATION = Custom.Electromate Then
                                    If (custObj("id").ToString().Length = 0) Then
                                        Return False
                                    End If
                                    Dim customerID As Integer
                                    If (Not Integer.TryParse(custObj("id").ToString(), customerID)) Then
                                        MessageBox.Show("Customer ID is not a number.")
                                        Return False
                                    End If

                                    Using conn = New OdbcConnection(connectionString)
                                        conn.Open()

                                        sql = String.Format("update customers set zoho_account_id = '{0}' where id = {1}",
                                                            .GetFieldValue("CustomText11").ToString(),
                                                            customerID)
                                        Using comm = New OdbcCommand(sql, conn)
                                            Try
                                                rowsAffected = comm.ExecuteNonQuery()
                                            Catch ex As Exception
                                                MessageBox.Show("Error updating Zoho Account ID", "Spire Error")
                                                My.Application.Log.WriteEntry("Error updating Zoho Account ID", TraceEventType.Critical)
                                                My.Application.Log.WriteException(ex)
                                                My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                                comm.Dispose()
                                                conn.Close()
                                                Return False
                                            End Try
                                        End Using

                                        sql = String.Format("update addresses set udf_data = udf_data || '{{""label"":""{0}""}}' where link_table = 'CUST' and link_no = '{1}' and addr_type = 'B'",
                                                            "SOMETHNG",
                                                            custObj("customerNo"))
                                        Using comm = New OdbcCommand(sql, conn)
                                            Try
                                                rowsAffected = comm.ExecuteNonQuery()
                                            Catch ex As Exception
                                                MessageBox.Show("Error updating customer level", "Spire Error")
                                                My.Application.Log.WriteEntry("Error updating Zoho Account ID", TraceEventType.Critical)
                                                My.Application.Log.WriteException(ex)
                                                My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                                comm.Dispose()
                                                conn.Close()
                                                Return False
                                            End Try
                                        End Using
                                    End Using
                                ElseIf CUSTOMIZATION = Custom.Primespec Then
                                    If (custObj("customerNo").ToString().Trim().Length = 0) Then
                                        Return False
                                    End If
                                    Dim customerID As String = custObj("customerNo").ToString().Trim()

                                    Using conn = New OdbcConnection(connectionString)
                                        conn.Open()

                                        sql = String.Format("update customers set zoho_account_id = '{0}' where cust_no = '{1}'",
                                                            .GetFieldValue("CustomText02").ToString(),
                                                            customerID)
                                        Using comm = New OdbcCommand(sql, conn)
                                            Try
                                                rowsAffected = comm.ExecuteNonQuery()
                                            Catch ex As Exception
                                                MessageBox.Show("Error updating Zoho Account ID", "Spire Error")
                                                My.Application.Log.WriteEntry("Error updating Zoho Account ID", TraceEventType.Critical)
                                                My.Application.Log.WriteException(ex)
                                                My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                                comm.Dispose()
                                                conn.Close()
                                                Return False
                                            End Try
                                        End Using
                                    End Using
                                End If
                            End If
                        End If
                    End If

                    .Query("DocumentHeaders", , "ID = " & docID)
                    If .RecordCount > 0 Then
                        .MoveFirst()
                        .SetFieldValue(customerNoMap, custNo)
                        .Update()
                    End If
                Else
                    'Step 3.6: Existing Customer; Add Ship-to details
                    'Make sure ship-to exists
                    .QueryEx("SELECT ShipToCompany," &
                                 "ShipToContact, " &
                                 "ShipToAddress1," &
                                 "ShipToAddress2," &
                                 "ShipToAddress3," &
                                 "ShipToCity," &
                                 "ShipToState," &
                                 "ShipToCountry," &
                                 "ShipToPostalCode," &
                                 "ShipToPhone," &
                                 "ShipToPhoneExt," &
                                 "ShipToFax," &
                                 "ShipToFaxExt," &
                                 "ShipToEmail, " &
                                 "SalesRep " &
                             "FROM DocumentHeaders " &
                             "WHERE ID = " & sqlEscape(docID))
                    Dim shipCompany As String = .GetFieldValue("ShipToCompany").ToString().Trim()
                    Dim shipContact As String = .GetFieldValue("ShipToContact").ToString().Trim()

                    If shipCompany.Length > 0 Or shipContact.Length > 0 Then
                        Dim shipAddr1 As String = .GetFieldValue("ShipToAddress1").ToString().Trim()
                        Dim shipAddr2 As String = .GetFieldValue("ShipToAddress2").ToString().Trim()
                        Dim shipAddr3 As String = .GetFieldValue("ShipToAddress3").ToString().Trim()
                        Dim shipCity As String = .GetFieldValue("ShipToCity").ToString().Trim()
                        Dim shipProvState As String = .GetFieldValue("ShipToState").ToString().Trim()
                        Dim shipCountry As String = .GetFieldValue("ShipToCountry").ToString().Trim()
                        Dim shipPostal As String = .GetFieldValue("ShipToPostalCode").ToString().Trim()

                        shipProvState = shipProvState.Substring(0, Math.Min(shipProvState.Length, 2))

                        If shipCountry.Length > 0 Then
                            If shipCountry = "USA" Then
                                shipCountry = "United States"
                            End If
                            If countryMapDict.ContainsKey(shipCountry.ToLower()) Then
                                shipCountry = countryMapDict(shipCountry.ToLower())
                            Else
                                shipCountry = ""
                            End If
                        End If

                        Dim spireStreet1Line = If(custObj("address")("streetAddress") Is Nothing, "", custObj("address")("streetAddress").ToString().Replace(vbLf, ","))
                        If spireStreet1Line.Length > 40 Then
                            spireStreet1Line = spireStreet1Line.Substring(0, 40)
                        End If

                        'Step 3.6.1: Only continue if the ship-to details are different that bill-to details
                        If shipAddr1 <> spireStreet1Line _
                            Or shipCity <> If(custObj("address")("city") Is Nothing, "", custObj("address")("city").ToString()) _
                            Or shipProvState <> If(custObj("address")("provState") Is Nothing, "", custObj("address")("provState").ToString()) _
                            Or shipCountry <> If(custObj("address")("country") Is Nothing, "", custObj("address")("country").ToString()) _
                            Or shipPostal <> If(custObj("address")("postalCode") Is Nothing, "", custObj("address")("postalCode").ToString()) Then

                            Dim foundShip = False
                            For Each shippAddress In custObj("shippingAddresses")
                                spireStreet1Line = If(shippAddress("streetAddress") Is Nothing, "", shippAddress("streetAddress").ToString().Replace(vbLf, ","))
                                If spireStreet1Line.Length > 40 Then
                                    spireStreet1Line = spireStreet1Line.Substring(0, 40)
                                End If
                                If shipAddr1 = spireStreet1Line _
                                    And shipCity = If(shippAddress("city") Is Nothing, "", shippAddress("city").ToString()) _
                                    And shipProvState = If(shippAddress("provState") Is Nothing, "", shippAddress("provState").ToString()) _
                                    And shipCountry = If(shippAddress("country") Is Nothing, "", shippAddress("country").ToString()) _
                                    And shipPostal = If(shippAddress("postalCode") Is Nothing, "", shippAddress("postalCode").ToString()) Then

                                    foundShip = True
                                    ShipTo = shippAddress("shipId").ToString()
                                    Exit For
                                End If
                            Next

                            If Not foundShip Then
                                Dim shipPhone As String = .GetFieldValue("ShipToPhone").ToString().Trim() & .GetFieldValue("ShipToPhoneExt").ToString().Trim()
                                Dim shipFax As String = .GetFieldValue("ShipToFax").ToString().Trim() & .GetFieldValue("ShipToFaxExt").ToString().Trim()
                                Dim shipEmail As String = .GetFieldValue("ShipToEmail").ToString().Trim()

                                shipFax = Regex.Replace(shipFax, "[^0-9]", "")
                                shipFax = shipFax.Substring(0, Math.Min(30, shipFax.Length))
                                shipPhone = Regex.Replace(shipPhone, "[^0-9]", "")
                                shipPhone = shipPhone.Substring(0, Math.Min(30, shipPhone.Length))

                                If shipPhone.Length > 0 AndAlso shipPhone.Substring(0, 1) = "1" Then
                                    shipPhone = shipPhone.Substring(1)
                                End If
                                If shipFax.Length > 0 AndAlso shipFax.Substring(0, 1) = "1" Then
                                    shipFax = shipFax.Substring(1)
                                End If

                                Dim custSellLevel = 1
                                Integer.TryParse(custObj("address")("sellLevel"), custSellLevel)
                                Dim newShipForm = New NewShipToForm(spireAPI, custObj("address")("salesTaxes")(0)("code"), custObj("address")("salesTaxes")(1)("code"))
                                newShipForm.ShipToIDTextBox.Text = ShipTo
                                newShipForm.ShipToNameTextBox.Text = ShipToName
                                newShipForm.slsTax1ComboBox.SelectedIndex = 0
                                newShipForm.slsTax2ComboBox.SelectedIndex = 0
                                newShipForm.sellingPriceLevelComboBox.SelectedIndex = custSellLevel - 1

                                'Step 3.6.2: Ask user for new ship-to details specific to Spire, validate the data before continuing
                                Dim validShipToID = False
                                Do
                                    newShipForm.ShowDialog()
                                    If newShipForm.continueAdding = False Then
                                        Return False
                                    ElseIf newShipForm.skipShipTo = True Then
                                        validShipToID = True
                                        ShipTo = ""
                                    Else
                                        newShipToID = newShipForm.ShipToIDTextBox.Text
                                        If newShipToID.Trim().Length = 0 Then
                                            MessageBox.Show("Please enter a ship-to ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Else
                                            Dim check_shipToObj = spireAPI.GetShipTo(newShipToID.Trim(), custObj("customerNo"))
                                            If check_shipToObj("count") = "1" Then
                                                MessageBox.Show("The entered ship-to ID already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            Else
                                                validShipToID = True
                                                ShipTo = newShipToID
                                                ShipToName = newShipForm.ShipToNameTextBox.Text.Trim()
                                            End If
                                        End If
                                    End If
                                Loop Until validShipToID = True

                                'Only add the ship-to if the Skip button wasn't clicked
                                If newShipForm.skipShipTo = False Then
                                    If newShipForm.slsTax1ComboBox.SelectedIndex > 0 Then
                                        Integer.TryParse(newShipForm.slsTax1ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), shipTax1No)
                                    End If
                                    If newShipForm.slsTax2ComboBox.SelectedIndex > 0 Then
                                        Integer.TryParse(newShipForm.slsTax2ComboBox.SelectedItem.ToString().Split(New Char() {"-"c}, 2)(0).Trim(), shipTax2No)
                                    End If
                                    shipEx1No = newShipForm.ex1NumberTextBox.Text.Trim
                                    shipEx2No = newShipForm.ex2NumberTextBox.Text.Trim
                                    shipSellPriceLevel = newShipForm.sellingPriceLevelComboBox.SelectedIndex + 1

                                    Dim api_addshipto = New Dictionary(Of String, Object)
                                    api_addshipto.Add("shippingAddresses", New List(Of Dictionary(Of String, Object)))

                                    For i As Integer = 0 To custObj("shippingAddresses").Length - 1
                                        api_addshipto("shippingAddresses").Add(New Dictionary(Of String, Object))
                                        api_addshipto("shippingAddresses")(0).Add("id", custObj("shippingAddresses")(i)("id"))
                                    Next
                                    Dim index = api_addshipto("shippingAddresses").Count
                                    api_addshipto("shippingAddresses").Add(New Dictionary(Of String, Object))
                                    api_addshipto("shippingAddresses")(index).Add("shipId", ShipTo)
                                    api_addshipto("shippingAddresses")(index).Add("name", ShipToName)
                                    api_addshipto("shippingAddresses")(index).Add("streetAddress", shipAddr1 & Environment.NewLine & shipAddr2 & Environment.NewLine & shipAddr3 & Environment.NewLine)
                                    api_addshipto("shippingAddresses")(index).Add("city", shipCity)
                                    api_addshipto("shippingAddresses")(index).Add("postalCode", shipPostal)
                                    api_addshipto("shippingAddresses")(index).Add("provState", shipProvState)
                                    api_addshipto("shippingAddresses")(index).Add("country", shipCountry)
                                    api_addshipto("shippingAddresses")(index).Add("email", shipEmail)
                                    api_addshipto("shippingAddresses")(index).Add("phone", New Dictionary(Of String, Object))
                                    api_addshipto("shippingAddresses")(index)("phone").Add("number", shipPhone)
                                    api_addshipto("shippingAddresses")(index)("phone").Add("format", 1)
                                    api_addshipto("shippingAddresses")(index).Add("fax", New Dictionary(Of String, Object))
                                    api_addshipto("shippingAddresses")(index)("fax").Add("number", shipFax)
                                    api_addshipto("shippingAddresses")(index)("fax").Add("format", 1)
                                    api_addshipto("shippingAddresses")(index).Add("sellLevel", shipSellPriceLevel)

                                    If CUSTOMIZATION = Custom.Electromate Then
                                        If shipPostal.Length > 3 Then
                                            Dim TerrDescription As String = ""
                                            Dim Territory = getTerritory(shipPostal.Substring(0, 3), TerrDescription)
                                            If Territory.Length > 0 Then
                                                api_addshipto("shippingAddresses")(index).Add("territory", New Dictionary(Of String, Object))
                                                api_addshipto("shippingAddresses")(index)("territory").Add("code", Territory)
                                                api_addshipto("shippingAddresses")(index)("territory").Add("description", TerrDescription)
                                            End If
                                        End If
                                    ElseIf CUSTOMIZATION = Custom.Norwood Then
                                        Dim TerrCode = ""
                                        If shipCountry = "Canada" Then
                                            TerrCode = "CAN"
                                        ElseIf shipCountry = "USA" Then
                                            TerrCode = "USA"
                                        Else
                                            TerrCode = ""
                                        End If
                                        If TerrCode.Length > 0 Then
                                            Dim TerrDescription = ""
                                            spireAPI.CheckTerritory(TerrCode, TerrDescription)
                                            api_addshipto("shippingAddresses")(index).Add("territory", New Dictionary(Of String, Object))
                                            api_addshipto("shippingAddresses")(index)("territory").Add("code", TerrCode)
                                            api_addshipto("shippingAddresses")(index)("territory").Add("description", TerrDescription)
                                        End If

                                        'SALES PERSON
                                        Dim qwSalesRep = .GetFieldValue("SalesRep").ToString().Trim()
                                        If comboBoxDict.ContainsKey(qwSalesRep) Then
                                            Dim sageSalesRep = comboBoxDict(qwSalesRep).SelectedItem.ToString
                                            If sageSalesRep <> "--" Then
                                                Dim salespeopleObj = spireAPI.GetSalespeople()

                                                For i As Integer = 0 To Integer.Parse(salespeopleObj("count")) - 1
                                                    If salespeopleObj("records")(i)("name") = sageSalesRep Then
                                                        api_addshipto("shippingAddresses")(index).Add("salesperson", New Dictionary(Of String, Object))
                                                        api_addshipto("shippingAddresses")(index)("salesperson").Add("code", salespeopleObj("records")(i)("code"))
                                                        api_addshipto("shippingAddresses")(index)("salesperson").Add("name", salespeopleObj("records")(i)("name"))
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If

                                    If shipTax1No > 0 Or shipTax2No > 0 Then
                                        api_addshipto("shippingAddresses")(index).Add("salesTaxes", New List(Of Dictionary(Of String, Object)))
                                        api_addshipto("shippingAddresses")(index)("salesTaxes").Add(New Dictionary(Of String, Object))
                                        api_addshipto("shippingAddresses")(index)("salesTaxes").Add(New Dictionary(Of String, Object))
                                        api_addshipto("shippingAddresses")(index)("salesTaxes").Add(New Dictionary(Of String, Object))
                                        api_addshipto("shippingAddresses")(index)("salesTaxes").Add(New Dictionary(Of String, Object))
                                        If shipTax1No > 0 Then
                                            api_addshipto("shippingAddresses")(index)("salesTaxes")(0).Add("code", shipTax1No)
                                            api_addshipto("shippingAddresses")(index)("salesTaxes")(0).Add("exempt", shipEx1No)
                                        End If
                                        If shipTax2No > 0 Then
                                            api_addshipto("shippingAddresses")(index)("salesTaxes")(1).Add("code", shipTax2No)
                                            api_addshipto("shippingAddresses")(index)("salesTaxes")(1).Add("exempt", shipEx2No)
                                        End If
                                    End If

                                    status = spireAPI.AddShipTo(custObj("id"), api_addshipto)
                                    If status Is Nothing Then
                                        Return False
                                    End If
                                End If
                            End If
                        Else
                            ShipTo = ""
                        End If
                    End If
                End If

                'GENERAL ORDER INFORMATION

                Dim api_addorder = New Dictionary(Of String, Object)

                'Step 4: Set customer ID
                api_addorder.Add("customer", New Dictionary(Of String, Object))
                api_addorder("customer").Add("id", custObj("id"))

                'Step 5: Set order status
                If transactionComboBox.SelectedItem.ToString = "Booking Order" Then
                    api_addorder.Add("type", "B")
                ElseIf transactionComboBox.SelectedItem.ToString = "Standing Order" Then
                    api_addorder.Add("type", "S")
                ElseIf transactionComboBox.SelectedItem.ToString = "Quote" Then
                    api_addorder.Add("type", "Q")
                ElseIf transactionComboBox.SelectedItem.ToString = "RMA" Then
                    api_addorder.Add("type", "R")
                ElseIf transactionComboBox.SelectedItem.ToString = "Working Order" Then
                    api_addorder.Add("type", "W")
                Else
                    api_addorder.Add("type", "O")
                End If

                .QueryEx("SELECT " & If(CUSTOMIZATION = Custom.Electromate,
                                        "CustomText01, " &
                                        "ShipToContact, " &
                                        "ShipToPhone, " &
                                        "ShipToPhoneExt, " &
                                        "ShipToFax, " &
                                        "ShipToFaxExt, " &
                                        "ShipToEmail, ", "") &
                             If(territoryMapComboBox.SelectedIndex > 0, territoryMap & ", ", "") &
                            "DocDate, " &
                            "DocDueDate, " &
                            "FOB, " &
                            "SoldToPONumber, " &
                            "ShipVia, " &
                            "ShippingAmount, " &
                            "SalesRep, " &
                            "IntroductionNotes, " &
                            "PurchasingNotes, " &
                            "ClosingNotes, " &
                            "InternalNotes, " &
                            "ShipToCompany, " &
                            "Terms " &
                            "FROM DocumentHeaders where ID = " & sqlEscape(docID))
                If .RecordCount > 0 Then
                    .MoveFirst()
                    If Not .EOF Then
                        FOB = .GetFieldValue("FOB").ToString().Trim()
                        PONo = .GetFieldValue("SoldToPONumber").ToString().Trim()
                        ShipVia = .GetFieldValue("ShipVia").ToString().Trim()
                        TermsDesc = .GetFieldValue("Terms").ToString().Trim()
                        IntroductionNotes = .GetFieldValue("IntroductionNotes").ToString().Trim().Replace(vbCr, "  ").Replace(vbLf, "  ")
                        PurchasingNotes = .GetFieldValue("PurchasingNotes").ToString().Trim().Replace(vbCr, "  ").Replace(vbLf, "  ")
                        ClosingNotes = .GetFieldValue("ClosingNotes").ToString().Trim().Replace(vbCr, "  ").Replace(vbLf, "  ")
                        InternalNotes = .GetFieldValue("InternalNotes").ToString().Trim().Replace(vbCr, "  ").Replace(vbLf, "  ")

                        If CUSTOMIZATION = Custom.Electromate Then
                            customVariable01 = .GetFieldValue("CustomText01").ToString().Trim()
                            customVariable02 = .GetFieldValue("ShipToContact").ToString().Trim()
                            customVariable03 = .GetFieldValue("ShipToPhone").ToString().Trim() & .GetFieldValue("ShipToPhoneExt").ToString().Trim()
                            customVariable04 = .GetFieldValue("ShipToEmail").ToString().Trim()

                            Dim customPhone = Regex.Replace(customVariable03, "[^0-9]", "")
                            customPhone = customPhone.Substring(0, Math.Min(30, customPhone.Length))
                            If customPhone.Length > 0 AndAlso customPhone.Substring(0, 1) = "1" Then
                                customPhone = customPhone.Substring(1)
                            End If

                            Dim customFax = .GetFieldValue("ShipToFax").ToString().Trim() & .GetFieldValue("ShipToFaxExt").ToString().Trim()
                            customFax = Regex.Replace(customFax, "[^0-9]", "")
                            customFax = customFax.Substring(0, Math.Min(30, customFax.Length))
                            If customFax.Length > 0 AndAlso customFax.Substring(0, 1) = "1" Then
                                customFax = customFax.Substring(1)
                            End If

                            api_addorder.Add("contact", New Dictionary(Of String, Object))
                            api_addorder("contact").Add("name", customVariable02)
                            api_addorder("contact").Add("email", customVariable04)
                            api_addorder("contact").Add("phone", New Dictionary(Of String, Object))
                            api_addorder("contact")("phone").Add("number", customPhone)
                            api_addorder("contact")("phone").Add("format", 1)
                            api_addorder("contact").Add("fax", New Dictionary(Of String, Object))
                            api_addorder("contact")("fax").Add("number", customFax)
                            api_addorder("contact")("fax").Add("format", 1)
                        End If

                        'Step 6: Set FOB
                        api_addorder.Add("fob", FOB)
                        'Step 7: Set PO number
                        api_addorder.Add("customerPO", PONo)

                        'Step 8: Set freight
                        Decimal.TryParse(.GetFieldValue("ShippingAmount").ToString().Trim(), Freight)
                        api_addorder.Add("freight", Freight)

                        'TODO: SHIP VIA - ADD TO ADDRESS

                        'If ShipVia.Length > 0 Then
                        '    Dim shipviaObj = spireAPI.GetShipVia()
                        '    For i As Integer = 0 To Integer.Parse(shipviaObj("count")) - 1
                        '        If shipviaObj("records")(i)("description") = ShipVia Then
                        '            api_addorder.Add("shipCode", shipviaObj("records")(i)("code"))
                        '            api_addorder.Add("shipDescription", shipviaObj("records")(i)("description"))
                        '            Exit For
                        '        End If
                        '    Next
                        'End If

                        'TERMS
                        'Step 9: Set terms
                        'Check if customer has a terms code
                        If custObj("paymentTerms") Is Nothing And TermsDesc.Length > 0 Then
                            Dim termsObj = spireAPI.GetTerms()
                            For i As Integer = 0 To Integer.Parse(termsObj("count")) - 1
                                If termsObj("records")(i)("description").ToString().ToLower() = TermsDesc.ToLower() Then
                                    api_addorder.Add("termsCode", termsObj("records")(i)("code"))
                                    api_addorder.Add("termsText", termsObj("records")(i)("description"))
                                    Exit For
                                End If
                            Next
                        End If

                        'ORDER DATE
                        'Step 10: Set order date
                        If dateMapComboBox.SelectedIndex = 0 Then
                            Dim unformattedDate = .GetFieldValue("DocDate").ToString.Trim
                            If unformattedDate.Length > 0 Then
                                Dim dateList() As String = unformattedDate.Split("/")
                                If dateList.Length = 3 Then
                                    api_addorder.Add("orderDate", dateList(2) & "-" & dateList(0) & "-" & dateList(1))
                                End If
                            End If
                        Else
                            api_addorder.Add("orderDate", DateTime.Today.ToString("yyyy-MM-dd"))
                        End If

                        'REQUIRED DATE
                        'Step 11: Set required date
                        Dim unformattedDueDate = .GetFieldValue("DocDueDate").ToString.Trim
                        If unformattedDueDate.Length > 0 Then
                            Dim dateList() As String = unformattedDueDate.Split("/")
                            If dateList.Length = 3 Then
                                api_addorder.Add("requiredDate", dateList(2) & "-" & dateList(0) & "-" & dateList(1))
                            End If
                        End If

                        'SHIP-TO
                        'Step 12: Set ship-to ID
                        api_addorder.Add("shippingAddress", New Dictionary(Of String, Object))
                        If ShipTo <> "" Then
                            api_addorder("shippingAddress").Add("shipId", ShipTo)
                        End If

                        'SALES PERSON
                        'Step 13: Set salesperson
                        Dim qwSalesRep = .GetFieldValue("SalesRep").ToString().Trim()
                        If comboBoxDict.ContainsKey(qwSalesRep) Then
                            Dim sageSalesRep = comboBoxDict(qwSalesRep).SelectedItem.ToString
                            If sageSalesRep = "--" Then
                                If Not suppressSlspnWarningCheckBox.Checked Then
                                    Dim ret = MessageBox.Show("No sales rep. chosen for " & qwSalesRep & ". Leaving this entry blank in Spire", "Empty Sales Rep", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                                    If ret <> DialogResult.OK Then
                                        Return False
                                    End If
                                End If
                            Else
                                Dim salespeopleObj = spireAPI.GetSalespeople()

                                For i As Integer = 0 To Integer.Parse(salespeopleObj("count")) - 1
                                    If salespeopleObj("records")(i)("name") = sageSalesRep Then
                                        api_addorder("shippingAddress").Add("salesperson", New Dictionary(Of String, Object))
                                        api_addorder("shippingAddress")("salesperson").Add("code", salespeopleObj("records")(i)("code"))
                                        api_addorder("shippingAddress")("salesperson").Add("name", salespeopleObj("records")(i)("name"))
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        'TERRITORY
                        'Step 14: Step territory
                        If (territoryMapComboBox.SelectedIndex > 0) Then
                            Dim terr = .GetFieldValue(territoryMap).ToString().Trim()

                            If (terr.Length > 0) Then
                                Dim temp As String = ""
                                If spireAPI.CheckTerritory(terr, temp) Then
                                    api_addorder("shippingAddress").Add("territory", New Dictionary(Of String, Object))
                                    api_addorder("shippingAddress")("territory").Add("code", terr)
                                Else
                                    MessageBox.Show("The Territory entered in QuoteWerks does not exist.", "Invalid Territory", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return False
                                End If
                            End If
                        End If
                    End If
                Else
                    MessageBox.Show("Error finding header info in QuoteWerks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If

                Dim unitPriceField = "UnitPrice"
                'Step 15: Add items - loop through each item
                .QueryEx("SELECT " & If(CUSTOMIZATION = Custom.Electromate,
                                        "DocumentItems.AlternateUnitPrice, " &
                                        "DocumentItems.Vendor, " &
                                        "DocumentItems.CustomText01, " &
                                        "DocumentItems.CustomText02, " &
                                        "DocumentItems.CustomText07, " &
                                        "DocumentItems.CustomText08, " &
                                        "DocumentItems.CustomText09, " &
                                        "DocumentItems.CustomNumber01, " &
                                        "DocumentItems.CustomNumber02, ", "") &
                            "DocumentItems.ManufacturerPartNumber, " &
                            "DocumentItems.VendorPartNumber, " &
                            "DocumentItems.InternalPartNumber, " &
                            "DocumentItems.QtyBase, " &
                            "DocumentItems.UnitOfMeasure, " &
                            "DocumentItems.Description, " &
                            "DocumentItems.PriceModifier, " &
                            "DocumentItems." & unitPriceField & ", " &
                            "DocumentItems.UnitCost, " &
                            "DocumentItems.ItemType, " &
                            "DocumentItems.LineType, " &
                            "DocumentItems.LineAttributes " &
                        "FROM DocumentHeaders LEFT JOIN DocumentItems ON DocumentHeaders.ID = DocumentItems.DocID " &
                        "WHERE DocumentHeaders.ID = " & sqlEscape(docID) & " " &
                        "ORDER BY DocumentItems.ID")

                '**********************ADD PRODUCTS***********************
                api_addorder.Add("items", New List(Of Dictionary(Of String, Object)))
                Dim recordCount As Integer = 0
                Dim isBundleItem As Boolean = False
                Dim bundleQuantity As Decimal = 1.0
                If .RecordCount > 0 Then
                    .MoveFirst()
                    Do
                        Dim recordLineType As String = .GetFieldValue("LineType")
                        Dim recordLineAttribute As Integer = Integer.Parse(.GetFieldValue("LineAttributes"))
                        If recordLineAttribute And LineAttribute.Exclude Then
                            .MoveNext()
                            Continue Do
                        End If

                        'Step 15.1: Bundles - reset flag and bundle values, check when the bundle ends
                        If recordLineAttribute And LineAttribute.GroupMember Then
                            'BUNDLE ITEM
                            isBundleItem = True
                        Else
                            isBundleItem = False
                            bundleDiscount = 0
                            bundleQuantity = 1
                        End If

                        If recordLineType <> LineType.SubTotal And recordLineType <> LineType.RunningSubTotal And recordLineType <> LineType.PercentDiscount Then
                            If recordLineType = LineType.Comment Then
                                'COMMENT LINE
                                'Step 15.2: Add comment line
                                api_addorder("items").Add(New Dictionary(Of String, Object))
                                api_addorder("items")(recordCount).Add("comment", .GetFieldValue("Description").ToString().Trim())
                                recordCount += 1
                            ElseIf recordLineType = LineType.GroupHeader And bundledItemComboBox.SelectedIndex = 1 Then
                                'BUNDLE HEADER
                                'Step 15.3: Bundles - get bundle header values
                                Dim bundleDiscountCode As String = .GetFieldValue("PriceModifier")
                                If bundleDiscountCode.Length() <> 0 AndAlso bundleDiscountCode(0) = "D" Then
                                    bundleDiscount = Double.Parse(bundleDiscountCode.Substring(1))
                                Else
                                    bundleDiscount = 0
                                End If
                                Decimal.TryParse(.GetFieldValue("QtyBase").ToString().Trim(), bundleQuantity)

                                isBundleItem = False
                            Else
                                'REGULAR ITEM
                                If isBundleItem And bundledItemComboBox.SelectedIndex = 0 Then
                                    'Only use top line item of a bundle
                                    .MoveNext()
                                    Continue Do
                                End If

                                Dim Code As String = .GetFieldValue(itemMap).ToString().Trim().ToUpper()
                                If Code.Length = 0 Then
                                    'Skip the empty lines
                                    .MoveNext()
                                    Continue Do
                                End If
                                Dim quitTransfer = False
                                Dim Whse As String = defaultWarehouseComboBox.SelectedItem.ToString()
                                Dim FullDesc As String = .GetFieldValue("Description").ToString().Trim()
                                Dim FullDesc1Line As String = FullDesc.Replace(Environment.NewLine, " ")
                                Dim Desc As String
                                If FullDesc1Line.Length > 80 Then
                                    Desc = FullDesc1Line.Substring(0, Math.Min(FullDesc1Line.Length, 77)) & "..."
                                Else
                                    Desc = FullDesc1Line
                                End If

                                Dim ProdCode = ""
                                Dim OrderedQty As Decimal = 0.0
                                Dim UnitPrice As Decimal = 0.0
                                Dim qwUnitPrice As Decimal = 0.0
                                Dim spireUnitPrice As Decimal = 0.0
                                Dim UnitCost As Decimal = 0.0
                                Dim qwUnitCost As Decimal = 0.0
                                Dim spireUnitCost As Decimal = 0.0
                                Dim DiscCode = .GetFieldValue("PriceModifier").ToString().Trim()
                                Dim LineDisc As Decimal = 0.0

                                If CUSTOMIZATION = Custom.Electromate Then
                                    Decimal.TryParse(.GetFieldValue("AlternateUnitPrice").ToString().Trim(), qwUnitPrice)
                                Else
                                    Decimal.TryParse(.GetFieldValue("UnitPrice").ToString().Trim(), qwUnitPrice)
                                End If

                                Decimal.TryParse(.GetFieldValue("QtyBase").ToString().Trim(), OrderedQty)

                                Decimal.TryParse(.GetFieldValue("UnitCost").ToString().Trim(), qwUnitCost)

                                OrderedQty = OrderedQty * bundleQuantity

                                'Step 15.4: Check if warehouse exists
                                Dim whseObj = spireAPI.FindWhse(Whse)
                                If whseObj("count") <> "1" Then
                                    MessageBox.Show("The entered warehouse does not exist.", "Invalid Warehouse", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return False
                                End If

                                'Step 15.5: Check if item exists
                                Dim itemObj = spireAPI.FindItem(Whse, Code)
                                If itemObj("count") <> "1" Then
                                    'Item does not exist
                                    If recordLineType = LineType.GroupHeader Then
                                        'Item is a bundle header
                                        MessageBox.Show("This bundle " & Code & " does not exist in Spire. Please add it there before continuing.", "Bundle Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return False
                                    End If

                                    'Step 15.6: Prompt user to ask if new item should be generated, use as non-stocked, or abort
                                    Dim addItem = False
                                    If newItemComboBox.SelectedIndex = 0 Then
                                        Dim newItemBox = New NewItemPrompt
                                        newItemBox.Text = newItemBox.Text & " - " & DocumentNo
                                        newItemBox.newItemLabel.Text = "The item '" & Code & "' does not exist. Do you wish to add it now?"
                                        newItemBox.ShowDialog()

                                        If newItemBox.selection = 0 Then
                                            quitTransfer = True
                                        ElseIf newItemBox.selection = 1 Then
                                            addItem = True
                                        Else
                                            NON_STOCKED = True
                                        End If
                                    ElseIf newItemComboBox.SelectedIndex = 1 Then
                                        addItem = True
                                    Else
                                        NON_STOCKED = True
                                    End If

                                    'Step 15.7: Add item
                                    If addItem Then
                                        'Add new item
                                        spireUnitPrice = qwUnitPrice
                                        spireUnitCost = qwUnitCost

                                        'Step 15.7.1: Validate part number is not too long, everything else will be truncated
                                        If Code.Length > 34 Then
                                            Dim ret = MessageBox.Show("Part number '" & Code & "' is greater than the maximum 34 characters allowed by Spire.", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            Return False
                                        End If

                                        Dim api_additem = New Dictionary(Of String, Object)
                                        'Step 15.7.2: Set warehouse
                                        api_additem.Add("whse", Whse)
                                        'Step 15.7.3: Set part number
                                        api_additem.Add("partNo", Code)
                                        'Step 15.7.4: Set current cost
                                        api_additem.Add("currentCost", qwUnitCost)

                                        Dim s As String = ""
                                        If .GetFieldValue("CustomText07").ToString().Trim() <> "" Then
                                            s = .GetFieldValue("CustomText07").ToString().Trim()
                                            Dim numberOfChrechters As Integer = s.Length
                                            If numberOfChrechters > 27 Then
                                                If s.Contains("\") Then
                                                    Dim index = s.IndexOf("\")
                                                    s = s.Substring(0, index)
                                                Else
                                                    s = s.Substring(0, 26)
                                                End If
                                            End If
                                        End If

                                        If CUSTOMIZATION = Custom.Electromate Then
                                                api_additem.Add("description", .GetFieldValue("ManufacturerPartNumber").ToString().Trim())
                                                api_additem.Add("extendedDescription", FullDesc.Replace(Environment.NewLine, vbLf))
                                                api_additem.Add("harmonizedCode", s)
                                                Dim countryOfOrigin = .GetFieldValue("CustomText09").ToString().Trim()
                                                If countryMapDict.ContainsKey(countryOfOrigin.ToLower()) Then
                                                    api_additem.Add("manufactureCountry", countryMapDict(countryOfOrigin.ToLower()))
                                                End If

                                                'Dim category = getCategory(Code, .GetFieldValue("Vendor").ToString().Trim())
                                                'Hardcoding it for now since it doesn't seem possible to properly determine which product database the item is from
                                                Dim category = getCategory(Code, "Electromate")
                                                If category.Length > 0 AndAlso spireAPI.CheckProductCode(category) Then
                                                    api_additem.Add("productCode", category)
                                                End If

                                                Dim vendor = .GetFieldValue("CustomText08").ToString().Trim().ToUpper()
                                                If vendor.Length > 0 AndAlso spireAPI.CheckVendor(vendor) Then
                                                    api_additem.Add("primaryVendor", New Dictionary(Of String, String))
                                                    api_additem("primaryVendor").Add("vendorNo", vendor)
                                                    If vendorDict.ContainsKey(vendor) Then
                                                        If vendorDict(vendor).Item1 Then
                                                            api_additem.Add("lotNumbered", True)
                                                        ElseIf vendorDict(vendor).Item2 Then
                                                            api_additem.Add("serialized", True)
                                                        End If
                                                    End If
                                                End If
                                            Else
                                                'Step 15.7.5: Set description
                                                api_additem.Add("description", FullDesc1Line)
                                            End If

                                            'Step 15.7.6: Read default UOM from database
                                            Dim defaultUOM As String = Nothing
                                            Using conn = New OdbcConnection(connectionString)
                                                conn.Open()

                                                sql = "select txt_data from system_settings where key = 'spire.inventory.default_stock_uom'"

                                                Using comm = New OdbcCommand(sql, conn)
                                                    Try
                                                        defaultUOM = comm.ExecuteScalar().ToString().Trim()
                                                    Catch ex As Exception
                                                    End Try
                                                End Using
                                            End Using

                                            If defaultUOM.Length = 0 Or defaultUOM Is Nothing Then
                                                defaultUOM = "EA"
                                            End If

                                            'Step 15.7.7: Set pricing
                                            api_additem.Add("pricing", New Dictionary(Of String, Object))
                                            api_additem("pricing").Add(defaultUOM, New Dictionary(Of String, Object))
                                            api_additem("pricing")(defaultUOM).Add("sellPrices", New List(Of Decimal))
                                            api_additem("pricing")(defaultUOM)("sellPrices").Add(spireUnitPrice)

                                            'Step 15.7.8: Insert item
                                            status = spireAPI.AddItem(api_additem)
                                            If status Is Nothing Then
                                                quitTransfer = True
                                            Else

                                                'Commenting this since consecutive requests seem to crash for electromate
                                                itemObj = spireAPI.GetItem(status)

                                                'Add Custom Fields
                                                If CUSTOMIZATION = Custom.Electromate Then
                                                    Dim itemID = status.ToString()

                                                    Dim estore = .GetFieldValue("CustomText01").ToString().Trim()
                                                    Dim customnumber = .GetFieldValue("CustomText02").ToString().Trim()
                                                    Dim vendorpart = .GetFieldValue("VendorPartNumber").ToString().Trim()

                                                    Using conn = New OdbcConnection(connectionString)
                                                        conn.Open()

                                                        sql = String.Format("update inventory set udf_data = udf_data || '{{""QW_VendorP"":""{1}""{2}{3}}}' where id = {0}",
                                                                            itemID,
                                                                            vendorpart,
                                                                            If(estore.Length <> 0, ",""QW_CT01"":""" & estore.Replace("'", "''") & """", ""),
                                                                            If(customnumber.Length <> 0, ",""QW_CT02"":""" & customnumber.Replace("'", "''") & """", ""))
                                                        Using comm = New OdbcCommand(sql, conn)
                                                            Try
                                                                rowsAffected = comm.ExecuteNonQuery()
                                                            Catch ex As Exception
                                                                MessageBox.Show("Error updating custom fields for new item", "Spire Error")
                                                                My.Application.Log.WriteEntry("Error Writing Item UDFs", TraceEventType.Critical)
                                                                My.Application.Log.WriteException(ex)
                                                                My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                                                comm.Dispose()
                                                                conn.Close()
                                                                Return False
                                                            End Try
                                                        End Using
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    Else
                                    'Step 15.8: Item exists; get item values
                                    itemObj = spireAPI.GetItem(itemObj("records")(0)("id"))
                                    Dim custSellLevel As Integer = 1
                                    Integer.TryParse(custObj("address")("sellLevel"), custSellLevel)
                                    spireUnitPrice = itemObj("pricing")(itemObj("sellMeasureCode"))("sellPrices")(custSellLevel - 1)
                                    spireUnitCost = itemObj("currentCost")
                                    If CUSTOMIZATION = Custom.Electromate Then
                                        If (Not Regex.Match(.GetFieldValue("ManufacturerPartNumber").ToString().Trim(), "\.\.\.$").Success) And (itemObj("description") <> .GetFieldValue("ManufacturerPartNumber").ToString().Trim()) Then
                                            MessageBox.Show("Item description does not match with Spire, aborting transfer", "Inconsistency", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                            quitTransfer = True
                                        ElseIf itemObj("extendedDescription") <> FullDesc.Replace(Environment.NewLine, vbLf) Then
                                            Dim api_updateitem = New Dictionary(Of String, Object)
                                            api_updateitem.Add("extendedDescription", FullDesc.Replace(Environment.NewLine, vbLf))
                                            spireAPI.UpdateItem(itemObj("id"), api_updateitem)
                                        End If
                                    End If
                                End If

                                If quitTransfer Then
                                    Return False
                                End If

                                'Step 15.9: Check if item price in quotewerks is different from spire; prompt user to choose price to use
                                If spireUnitPrice <> qwUnitPrice Then
                                    If NON_STOCKED Or updatedItemComboBox.SelectedIndex = 2 Then
                                        spireUnitPrice = qwUnitPrice
                                    ElseIf updatedItemComboBox.SelectedIndex = 0 Then
                                        Dim ret = MessageBox.Show("The price for item '" & Code & "' is listed as $" & spireUnitPrice.ToString() & " in Spire and $" & qwUnitPrice & " in QuoteWerks. Would you like to use the new QuoteWerks Price?" & vbLf & vbLf & "Press No to use Spire price.", "Update Price", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                                        If ret = DialogResult.Yes Then
                                            'Yes Clicked
                                            spireUnitPrice = qwUnitPrice
                                        ElseIf ret <> DialogResult.No Then
                                            'Didn't click Yes or No
                                            Return False
                                        End If
                                    End If
                                End If

                                If CUSTOMIZATION = Custom.Electromate Then
                                    'Dim quotewerksCost = qwUnitCost
                                    If spireUnitCost <> qwUnitCost Then
                                        If NON_STOCKED Or updatedItemComboBox.SelectedIndex = 2 Then
                                            spireUnitCost = qwUnitCost
                                        ElseIf updatedItemComboBox.SelectedIndex = 0 Then
                                            Dim ret = MessageBox.Show("The cost for item '" & Code & "' is listed as $" & spireUnitCost.ToString() & " in Spire and $" & qwUnitCost & " in QuoteWerks. Would you like to use the new QuoteWerks Cost?" & vbLf & vbLf & "Press No to use Spire cost.", "Update Cost", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                                            If ret = DialogResult.Yes Then
                                                'Yes Clicked
                                                spireUnitCost = qwUnitCost
                                                'Dim api_updateitem = New Dictionary(Of String, Object)
                                                'api_updateitem.Add("currentCost", spireUnitCost)
                                                'spireAPI.UpdateItem(itemObj("id"), api_updateitem)
                                            ElseIf ret <> DialogResult.No Then
                                                'Didn't click Yes or No
                                                Return False
                                            End If
                                        End If
                                    End If
                                    costsToUpdate.Add(New Tuple(Of Int32, Decimal)(recordCount + 1, spireUnitCost))
                                End If

                                'Step 15.10: Bundles - combine discounts for bundles
                                Dim discountCode As String = DiscCode
                                If discountCode.Length() <> 0 AndAlso discountCode(0) = "D" Then
                                    Dim originalDiscount As Decimal = 0.0
                                    Decimal.TryParse(discountCode.Substring(1), originalDiscount)
                                    If isBundleItem And bundleDiscount <> 0 Then
                                        'Combine discount percentages: (x+y) - x*y
                                        LineDisc = ((originalDiscount / 100 + bundleDiscount / 100) - (originalDiscount / 100 * bundleDiscount / 100)) * 100
                                    Else
                                        LineDisc = originalDiscount
                                    End If
                                End If

                                api_addorder("items").Add(New Dictionary(Of String, Object))
                                If NON_STOCKED Then
                                    'Step 15.11: Add non-stocked item to the order
                                    api_addorder("items")(recordCount).Add("inventory", New Dictionary(Of String, Object))
                                    'Step 15.11.1: Set warehouse
                                    api_addorder("items")(recordCount)("inventory").Add("whse", Whse)
                                    'Step 15.11.2: Set part number
                                    api_addorder("items")(recordCount)("inventory").Add("partNo", Code)
                                    If CUSTOMIZATION = Custom.Electromate Then
                                        api_addorder("items")(recordCount).Add("description", .GetFieldValue("ManufacturerPartNumber").ToString().Trim())
                                        Dim vendor = .GetFieldValue("CustomText08").ToString().Trim()
                                        If vendor.Length > 0 AndAlso spireAPI.CheckVendor(vendor) Then
                                            api_addorder("items")(recordCount).Add("vendor", vendor)
                                        End If
                                    Else
                                        'Step 15.11.3: Set description
                                        api_addorder("items")(recordCount).Add("description", Desc)
                                    End If
                                    'Step 15.11.4: Set order quantity
                                    api_addorder("items")(recordCount).Add("orderQty", OrderedQty.ToString())
                                    'Step 15.11.5: Set line discount
                                    api_addorder("items")(recordCount).Add("lineDiscountPct", LineDisc.ToString())
                                    'Step 15.11.6: Set unit price
                                    api_addorder("items")(recordCount).Add("unitPrice", spireUnitPrice.ToString())
                                Else
                                    'Step 15.12: Add regular item to the order
                                    api_addorder("items")(recordCount).Add("inventory", New Dictionary(Of String, Object))
                                    'Step 15.12.1: Set item id
                                    api_addorder("items")(recordCount)("inventory").Add("id", itemObj("id"))
                                    'Step 15.12.2: Set order quantity
                                    api_addorder("items")(recordCount).Add("orderQty", OrderedQty.ToString())
                                    'Step 15.12.3: Set line discount
                                    api_addorder("items")(recordCount).Add("lineDiscountPct", LineDisc.ToString())

                                    If spireUnitPrice = qwUnitPrice Then
                                        'Step 15.12.4: Set unit price
                                        api_addorder("items")(recordCount).Add("unitPrice", spireUnitPrice.ToString())
                                    End If
                                End If

                                recordCount += 1
                            End If
                        End If
                        .MoveNext()
                    Loop While Not .EOF
                    If CUSTOMIZATION = Custom.Electromate Then
                        If customVariable01.Length > 0 Then
                            api_addorder("items").Add(New Dictionary(Of String, Object))
                            api_addorder("items")(recordCount).Add("comment", "")
                            'api_addorder("items").Add(New Dictionary(Of String, Object))
                            'api_addorder("items")(recordCount + 1).Add("comment", "Quoted Lead Time: " & customVariable01)
                            api_addorder("items").Add(New Dictionary(Of String, Object))
                            api_addorder("items")(recordCount + 1).Add("comment", "Customer Request Date: See Above")
                        End If
                    End If

                Else
                    MessageBox.Show("Error finding quote item info in QuoteWerks", "Error")
                    Return False
                End If

                'Step 16: Insert order
                status = spireAPI.AddOrder(api_addorder)

                If status Is Nothing Then
                    Return False
                Else
                    'NOTES

                    If introductionNotesCheckBox.Checked And IntroductionNotes.Length <> 0 Then
                    End If
                    If purchasingNotesCheckBox.Checked And PurchasingNotes.Length <> 0 Then
                    End If
                    If closingNotesCheckBox.Checked And ClosingNotes.Length <> 0 Then
                    End If
                    If internalNotesCheckBox.Checked And InternalNotes.Length <> 0 Then
                    End If

                    'Add Custom Fields
                    If CUSTOMIZATION = Custom.Electromate Then
                        Dim orderID = status.ToString()

                        Dim orderobj = spireAPI.GetOrder(orderID)

                        .Query("DocumentHeaders", , "ID = " & docID)
                        If .RecordCount > 0 Then
                            .MoveFirst()
                            .SetFieldValue("CustomText07", orderobj("orderNo"))
                            .Update()
                        End If

                        Using conn = New OdbcConnection(connectionString)
                            conn.Open()

                            Dim sanitizedPhone = Regex.Replace(customVariable03, "[^0-9]", "")
                            sanitizedPhone = sanitizedPhone.Substring(0, Math.Min(10, sanitizedPhone.Length))

                            sql = String.Format("update sales_orders set udf_data = udf_data || '{{""QW_EISQ"":""{1}"",""QW_Convert"":""{2}""{3}{4}{5}{6}{7}{8}{9}{10}}}' where id = {0}",
                                                orderID,
                                                DocumentNo,
                                                DateTime.Now.ToString("HHmm"),
                                                If(addCustomer, ",""QW_NewCust"":""TRUE""", ""),
                                                If(purchasingNotesCheckBox.Checked And PurchasingNotes.Length <> 0, ",""QW_PONotes"":""" & PurchasingNotes.Replace("'", "''") & """", ""),
                                                If(internalNotesCheckBox.Checked And InternalNotes.Length <> 0, ",""QW_IntNote"":""" & InternalNotes.Replace("'", "''") & """", ""),
                                                If(customVariable02.Trim().Length <> 0, ",""QW_SoldTo"":""" & customVariable02.Trim().Replace("'", "''") & """", ""),
                                                If(sanitizedPhone.Length <> 0, ",""QW_STPhone"":""" & sanitizedPhone & """", ""),
                                                If(customVariable04.Trim().Length <> 0, ",""QW_STEmail"":""" & customVariable04.Trim().Replace("'", "''") & """", ""),
                                                If(ShipVia.Trim().Length <> 0, ",""QW_ShipVia"":""" & ShipVia.Trim().Replace("'", "''") & """", ""),
                                                If(customVariable01.Trim().Length <> 0, ",""qw_lead_time"":""" & customVariable01.Trim().Replace("'", "''") & """", ""))

                            Using comm = New OdbcCommand(sql, conn)
                                Try
                                    rowsAffected = comm.ExecuteNonQuery()
                                Catch ex As Exception
                                    MessageBox.Show("Error updating custom fields for sales order", "Spire Error")
                                    My.Application.Log.WriteEntry("Error Writing Sales Order UDFs", TraceEventType.Critical)
                                    My.Application.Log.WriteException(ex)
                                    My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                    comm.Dispose()
                                    conn.Close()
                                    Return False
                                End Try
                            End Using

                            For Each k As Tuple(Of Int32, Decimal) In costsToUpdate
                                sql = String.Format("update sales_order_items set current_cost={2}, average_cost={2}, user_cost=true, udf_data = udf_data || '{{""qw_lincost"":""{2}""}}' where order_no='{0}' and sequence={1}", orderobj("orderNo"), k.Item1, k.Item2)
                                Using comm = New OdbcCommand(sql, conn)
                                    Try
                                        rowsAffected = comm.ExecuteNonQuery()
                                    Catch ex As Exception
                                        MessageBox.Show("Error updating order costs", "Spire Error")
                                        My.Application.Log.WriteEntry("Error updating order costs", TraceEventType.Critical)
                                        My.Application.Log.WriteException(ex)
                                        My.Application.Log.WriteEntry(sql, TraceEventType.Critical)
                                        comm.Dispose()
                                        conn.Close()
                                        Return False
                                    End Try
                                End Using
                            Next
                        End Using
                    End If
                End If
            End With
            .Database.CloseDB()
        End With

        If CUSTOMIZATION = Custom.Electromate Then
        Else
            updateTransferred(docID)
        End If
        Return True
    End Function

    Private Sub updateTransferred(docID As String)
        'Set the value of CustomNumber01 to 1 in quotewerks, which is used by the batchLookupButton_Click function to show a green background for transferred quotes
        With QWBack.Item(QWInstallation)
            Dim iError = .Database.OpenDB("DOCS", "QWTEST")

            With .Database.Recordset
                .Query("DocumentHeaders", , "ID = " & docID)
                If .RecordCount > 0 Then
                    Dim i As String = "1"
                    .MoveFirst()
                    .SetFieldValue("CustomNumber01", i)
                    .Update()
                End If
            End With
            .Database.CloseDB()
        End With
    End Sub

#End Region

#Region "Toolstrip, Notify, Profiles"

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles NotifyIcon1.DoubleClick
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub ContextMenuStrip1_Closing(ByVal sender As Object, ByVal e As ToolStripDropDownClosingEventArgs) Handles ContextMenuStrip1.Closing
        If cancelClose Then
            e.Cancel = True
            cancelClose = False
        End If
    End Sub

    Private Sub ToolStripMenuItem2_MouseDown(sender As Object, e As MouseEventArgs) Handles ToolStripMenuItem2.MouseDown
        cancelClose = True
    End Sub

    Private Sub ActivateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivateToolStripMenuItem.Click
        If LFile1.ShowTriggerDlg(Me.Handle.ToInt32, 0, "Activate|User Code:|Computer ID:|License ID:|Password:", 0, 0) = 1 Then
            MessageBox.Show("Activation Successful", "")
        End If
    End Sub

    Private Sub ProfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Profile1ToolStripMenuItem.Click, Profile2ToolStripMenuItem.Click, Profile3ToolStripMenuItem.Click
        Dim item = CType(sender, ToolStripMenuItem)
        loadProfile(Integer.Parse(item.Name.Substring(7, 1)), True)
    End Sub

    Private Sub ManageProfilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageProfilesToolStripMenuItem.Click
        Dim pm As ProfileManager = New ProfileManager
        pm.ShowDialog()
        Profile1ToolStripMenuItem.Text = My.Settings.Profile1Name
        Profile2ToolStripMenuItem.Text = My.Settings.Profile2Name
        Profile3ToolStripMenuItem.Text = My.Settings.Profile3Name
        My.Settings.SettingsKey = "Profile" & currentProfile.ToString
        My.Settings.Reload()

        If currentProfile = 1 Then
            Me.Text = "QuoteLink - " & Profile1ToolStripMenuItem.Text
        ElseIf currentProfile = 2 Then
            Me.Text = "QuoteLink - " & Profile2ToolStripMenuItem.Text
        Else
            Me.Text = "QuoteLink - " & Profile3ToolStripMenuItem.Text
        End If
    End Sub

    Private Sub NotifyIcon1_BalloonTipClicked(sender As Object, e As EventArgs) Handles NotifyIcon1.BalloonTipClicked
        If Not (qwBackConnected And dsnConnected) Then
            Return
        End If
        Dim ret = MessageBox.Show("Are you sure you wish to transfer the converted document to Spire?", "Transfer Order", MessageBoxButtons.YesNo)
        If ret <> DialogResult.Yes Then
            Return
        End If
        Dim ni = CType(sender, NotifyIcon)
        transferOrderSpire(ni.Tag)
    End Sub

    Public Sub loadProfile(ByVal profileID As Integer, Optional ByVal saveprofile As Boolean = False)
        If currentProfile = profileID Then
            Return
        End If
        currentProfile = profileID
        If currentProfile = 1 Then
            Profile1ToolStripMenuItem.Checked = True
            Profile2ToolStripMenuItem.Checked = False
            Profile3ToolStripMenuItem.Checked = False
            Me.Text = "QuoteLink - " & Profile1ToolStripMenuItem.Text
        ElseIf currentProfile = 2 Then
            Profile1ToolStripMenuItem.Checked = False
            Profile2ToolStripMenuItem.Checked = True
            Profile3ToolStripMenuItem.Checked = False
            Me.Text = "QuoteLink - " & Profile2ToolStripMenuItem.Text
        Else
            Profile1ToolStripMenuItem.Checked = False
            Profile2ToolStripMenuItem.Checked = False
            Profile3ToolStripMenuItem.Checked = True
            Me.Text = "QuoteLink - " & Profile3ToolStripMenuItem.Text
        End If
        loading = True
        My.Settings.SettingsKey = "Profile" & currentProfile.ToString
        My.Settings.Reload()

        Dim slspnData = My.Settings.SlspnStringText
        DSNTextBox.Text = My.Settings.DSNNameText
        ServerTextBox.Text = My.Settings.DSNServerText
        descriptionCheckBox.Checked = My.Settings.DescriptionCheck
        syncPricingCheckBox.Checked = My.Settings.SyncPriceCheck
        syncCostCheckBox.Checked = My.Settings.SyncCostCheck
        newCustomerComboBox.SelectedIndex = My.Settings.NewCustomerCombo
        updatedItemComboBox.SelectedIndex = My.Settings.UpdateItemCombo
        itemMapComboBox.SelectedIndex = My.Settings.ItemNumberMapCombo
        dateMapComboBox.SelectedIndex = My.Settings.DateMapCombo
        transactionComboBox.SelectedIndex = My.Settings.TransTypeCombo
        newItemComboBox.SelectedIndex = My.Settings.NewItemCombo
        filterTypeComboBox.SelectedIndex = My.Settings.FilterTypeCombo
        defaultQuoteStage = My.Settings.FilterStageCombo
        defaultSyncWarehouse = My.Settings.InvSyncWarehouseCombo
        defaultWarehouse = My.Settings.DefaultWarehouseCombo
        SQLServerCheckBox.Checked = My.Settings.UseSQLBackendCheck
        dbServerTextBox.Text = My.Settings.SQLServerText
        dbNameTextBox.Text = My.Settings.SQLDatabaseText
        dbUsernameTextBox.Text = My.Settings.SQLUsernameText
        dbPasswordTextBox.Text = If(My.Settings.SQLPasswordText.Length = 0, "", AES_Decrypt(My.Settings.SQLPasswordText, key))
        defaultSelPriceLevelComboBox.SelectedIndex = My.Settings.SellPriceLevelCombo
        bundledItemComboBox.SelectedIndex = My.Settings.BOMComboBox
        QWInstallationStr = My.Settings.QWInstallation
        sequentialCustCheckBox.Checked = My.Settings.SequentialCustCheck
        suppressSlspnWarningCheckBox.Checked = My.Settings.SuppressSlspnWarningCheck
        introductionNotesCheckBox.Checked = My.Settings.IntroductionNotesCheck
        closingNotesCheckBox.Checked = My.Settings.ClosingNotesCheck
        purchasingNotesCheckBox.Checked = My.Settings.PurchasingNotesCheck
        internalNotesCheckBox.Checked = My.Settings.InternalNotesCheck
        SpireURLTextBox.Text = My.Settings.SpireURLText
        SpireUsernameTextBox.Text = My.Settings.SpireUsernameText
        SpirePasswordTextBox.Text = My.Settings.SpirePasswordText
        defaultSyncSellPriceLevelComboBox.SelectedIndex = My.Settings.DefaultSyncSellPriceLevelCombo
        customerNoMapComboBox.SelectedIndex = My.Settings.CustomerNoMapCombo
        territoryMapComboBox.SelectedIndex = My.Settings.TerritoryMapCombo

        If CUSTOMIZATION = Custom.Electromate Then
            terrSpreadsheetTextBox.Text = My.Settings.TerrSpreadsheetLocation
            terrWorksheetTextBox.Text = My.Settings.TerrWorksheet
        End If

        If Not SQLServerCheckBox.Checked Then
            dbServerTextBox.Enabled = False
            dbNameTextBox.Enabled = False
            dbUsernameTextBox.Enabled = False
            dbPasswordTextBox.Enabled = False
        End If

        If saveprofile Then
            My.Settings.SettingsKey = "Global"
            My.Settings.Reload()
            My.Settings.DefaultProfile = currentProfile
            My.Settings.Save()
            My.Settings.SettingsKey = "Profile" & currentProfile.ToString
            My.Settings.Reload()
        End If

        slspnMapDict.Clear()
        Dim slspnList() As String = slspnData.Split("^")
        If slspnList.Length() >= 2 Then
            For i As Integer = 0 To slspnList.Length() - 1 Step 2
                slspnMapDict.Add(slspnList(i), slspnList(i + 1))
            Next
        End If

        If CUSTOMIZATION = Custom.Electromate Then
            Dim vendorData = My.Settings.VendorStringText
            vendorDict.Clear()
            Dim vendorList() As String = vendorData.Split("^")
            If vendorList.Length() >= 3 Then
                For i As Integer = 0 To vendorList.Length() - 1 Step 3
                    vendorDict.Add(vendorList(i), Tuple.Create(Boolean.Parse(vendorList(i + 1)), Boolean.Parse(vendorList(i + 2))))
                Next
            End If
        End If

        connect()

        loading = False
    End Sub

    Private Sub saveDefaults()
        If loading Then
            Return
        End If

        Dim slspnString As String = String.Join("^", comboBoxDict.Select(Function(kvp) String.Format("{0}^{1}", kvp.Key, kvp.Value.SelectedItem.ToString())).ToArray())

        My.Settings.DSNNameText = DSNTextBox.Text.Trim
        My.Settings.DSNServerText = ServerTextBox.Text.Trim
        My.Settings.DescriptionCheck = descriptionCheckBox.Checked
        My.Settings.SyncPriceCheck = syncPricingCheckBox.Checked
        My.Settings.SyncCostCheck = syncCostCheckBox.Checked
        My.Settings.NewCustomerCombo = If(newCustomerComboBox.SelectedIndex = -1, 0, newCustomerComboBox.SelectedIndex)
        My.Settings.UpdateItemCombo = If(updatedItemComboBox.SelectedIndex = -1, 0, updatedItemComboBox.SelectedIndex)
        My.Settings.ItemNumberMapCombo = If(itemMapComboBox.SelectedIndex = -1, 0, itemMapComboBox.SelectedIndex)
        My.Settings.DateMapCombo = If(dateMapComboBox.SelectedIndex = -1, 0, dateMapComboBox.SelectedIndex)
        My.Settings.TransTypeCombo = If(transactionComboBox.SelectedIndex = -1, 0, transactionComboBox.SelectedIndex)
        My.Settings.NewItemCombo = If(newItemComboBox.SelectedIndex = -1, 0, newItemComboBox.SelectedIndex)
        My.Settings.FilterTypeCombo = If(filterTypeComboBox.SelectedIndex = -1, 0, filterTypeComboBox.SelectedIndex)
        My.Settings.FilterStageCombo = If(filterStageComboBox.SelectedIndex = -1, "", filterStageComboBox.SelectedItem.ToString)
        My.Settings.UseSQLBackendCheck = SQLServerCheckBox.Checked
        My.Settings.SQLServerText = dbServerTextBox.Text.Trim
        My.Settings.SQLDatabaseText = dbNameTextBox.Text.Trim
        My.Settings.SQLUsernameText = dbUsernameTextBox.Text.Trim
        My.Settings.SQLPasswordText = AES_Encrypt(dbPasswordTextBox.Text.Trim, key)
        My.Settings.SellPriceLevelCombo = defaultSelPriceLevelComboBox.SelectedIndex
        My.Settings.BOMComboBox = bundledItemComboBox.SelectedIndex
        My.Settings.QWInstallation = qwInstancesComboBox.SelectedItem.ToString
        My.Settings.SequentialCustCheck = sequentialCustCheckBox.Checked
        My.Settings.SuppressSlspnWarningCheck = suppressSlspnWarningCheckBox.Checked
        My.Settings.IntroductionNotesCheck = introductionNotesCheckBox.Checked
        My.Settings.ClosingNotesCheck = closingNotesCheckBox.Checked
        My.Settings.PurchasingNotesCheck = purchasingNotesCheckBox.Checked
        My.Settings.InternalNotesCheck = internalNotesCheckBox.Checked
        My.Settings.SpireURLText = SpireURLTextBox.Text.Trim
        My.Settings.SpireUsernameText = SpireUsernameTextBox.Text.Trim
        My.Settings.SpirePasswordText = SpirePasswordTextBox.Text.Trim
        My.Settings.DefaultSyncSellPriceLevelCombo = defaultSyncSellPriceLevelComboBox.SelectedIndex
        My.Settings.CustomerNoMapCombo = customerNoMapComboBox.SelectedIndex
        My.Settings.TerritoryMapCombo = territoryMapComboBox.SelectedIndex

        If CUSTOMIZATION = Custom.Electromate Then
            My.Settings.TerrSpreadsheetLocation = terrSpreadsheetTextBox.Text
            My.Settings.TerrWorksheet = terrWorksheetTextBox.Text

            Dim vendorString As String = String.Join("^", vendorDict.Select(Function(kvp) String.Format("{0}^{1}^{2}", kvp.Key, kvp.Value.Item1, kvp.Value.Item2)))
            My.Settings.VendorStringText = vendorString
        End If

        If dsnConnected Then
            My.Settings.SlspnStringText = slspnString
            My.Settings.DefaultWarehouseCombo = If(defaultWarehouseComboBox.SelectedIndex = -1, "", defaultWarehouseComboBox.SelectedItem.ToString)
            My.Settings.InvSyncWarehouseCombo = If(defaultSyncWarehouseComboBox.SelectedIndex = -1, "", defaultSyncWarehouseComboBox.SelectedItem.ToString)
        End If

        My.Settings.Save()

        QWInstallationStr = qwInstancesComboBox.SelectedItem.ToString

        defaultQuoteStage = If(filterStageComboBox.SelectedIndex = -1, "", filterStageComboBox.SelectedItem.ToString())

        slspnMapDict.Clear()
        Dim slspnList() As String = slspnString.Split("^")
        If slspnList.Length() >= 2 Then
            For i As Integer = 0 To slspnList.Length() - 1 Step 2
                slspnMapDict.Add(slspnList(i), slspnList(i + 1))
            Next
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim aboutdialog = New AboutForm
        aboutdialog.versionTextBox.Text = version
        aboutdialog.ShowDialog()
        aboutdialog.Dispose()
    End Sub

    Private Sub ToolStripMenuExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click, ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

#End Region

#Region "Helpers"

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Function sqlEscape(ByVal str) As String
        Return str.Replace("'", "''")
    End Function

    Private Function shellEscape(ByVal str) As String
        Return str.Replace("""", """""")
    End Function

#End Region

#Region "Test Buttons"

    Private Sub TestButton_Click(sender As Object, e As EventArgs) Handles TestButton.Click
        MessageBox.Show("No sales rep. chosen for " & "John Wick" & ". Leaving this entry blank in Spire", "Empty Sales Rep", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
        Return
        With QWBack.Item(QWInstallation)
            Dim iError = .Database.OpenDB("DOCS", "QWTEST")

            With .Database.Recordset
                .QueryEx("SELECT DocNo, AlternateCurrency FROM DocumentHeaders where DocNo = 'EISO32086'")
                If .RecordCount > 0 Then
                    .MoveFirst()
                    If Not .EOF Then
                        Dim altcur() = .GetFieldValue("AlternateCurrency").ToString().Split(Chr(4))

                        If altcur.Count = 3 Then
                            If altcur(0) = "USD" Then
                                MessageBox.Show("USD")
                            ElseIf altcur(0) = "CAD" Then
                                MessageBox.Show("CAD")
                            End If
                        End If
                    Else
                        MessageBox.Show("ERROR")
                    End If
                Else
                    MessageBox.Show("No Results")
                End If
            End With

        End With

        Return
    End Sub

    Private Sub TestSQL_Button_Click(sender As Object, e As EventArgs) Handles TestSQL_Button.Click
        Dim rButton As RadioButton = databaseTable.Controls.OfType(Of RadioButton)().Where(Function(r) r.Checked = True).FirstOrDefault()
        If rButton Is Nothing Then
            MessageBox.Show("Please select a Product Database to synchronize.", "No Database Selected")
            InvSyncButton.Enabled = True
            Return
        End If

        Dim rx As Regex = New Regex("\( .* \)")
        Dim match As Match = rx.Match(rButton.Text)
        Dim productDatabase = match.Value.Substring(2, match.Value.Length - 4)

        If testDatabaseConn(productDatabase) Then
            MessageBox.Show("Success")
        End If
    End Sub

    Private Sub qwInstancesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles qwInstancesComboBox.SelectedIndexChanged

    End Sub

    Private Sub GroupBox6_Enter(sender As Object, e As EventArgs) Handles GroupBox6.Enter

    End Sub

#End Region

End Class
