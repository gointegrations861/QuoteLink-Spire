Imports System.Data.Odbc

Public Class NewShipToForm

    Public continueAdding = False
    Public skipShipTo = False

    Public Sub New(ByRef spireAPI As SpireAPIContainer, Optional ByVal tax1No As Integer = -1, Optional ByVal tax2No As Integer = -1)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        slsTax1ComboBox.Items.Add("<Default>")
        slsTax2ComboBox.Items.Add("<Default>")
        slsTax1ComboBox.Items.Add("0 - None")
        slsTax2ComboBox.Items.Add("0 - None")

        'Dim conn As OdbcConnection
        'Dim comm As OdbcCommand
        'Dim dr As OdbcDataReader
        'Dim connectionString As String
        'Dim sql As String
        'connectionString = "DSN=" & dsn & ";SERVER=" & server & ";"
        'conn = New OdbcConnection(connectionString)
        'conn.Open()

        'sql = "SELECT tax_no, name, rate FROM public.sales_taxes"
        'comm = New OdbcCommand(sql, conn)
        'dr = comm.ExecuteReader()
        'While (dr.Read())
        '    slsTax1ComboBox.Items.Add(dr.GetInt16(0).ToString & " - " & dr.GetString(1).Trim & " (" & dr.GetDecimal(2).ToString & "%)")
        '    slsTax2ComboBox.Items.Add(dr.GetInt16(0).ToString & " - " & dr.GetString(1).Trim & " (" & dr.GetDecimal(2).ToString & "%)")
        'End While
        'dr.Close()
        'comm.Dispose()
        'conn.Close()
        'conn.Dispose()
        Dim salesTaxes = spireAPI.GetSalesTaxes()
        Dim tax1Index = 0
        Dim tax2Index = 0
        For i As Integer = 0 To Integer.Parse(salesTaxes("count")) - 1
            slsTax1ComboBox.Items.Add(salesTaxes("records")(i)("code") & " - " & salesTaxes("records")(i)("name") & " (" & salesTaxes("records")(i)("rate") & "%)")
            If salesTaxes("records")(i)("code") = tax1No Then
                tax1Index = slsTax1ComboBox.Items.Count
            End If
            slsTax2ComboBox.Items.Add(salesTaxes("records")(i)("code") & " - " & salesTaxes("records")(i)("name") & " (" & salesTaxes("records")(i)("rate") & "%)")
            If salesTaxes("records")(i)("code") = tax2No Then
                tax2Index = slsTax2ComboBox.Items.Count
            End If
        Next
        slsTax1ComboBox.SelectedIndex = tax1Index
        slsTax2ComboBox.SelectedIndex = tax2Index
    End Sub

    Private Sub newCustCancelButton_Click(sender As Object, e As EventArgs) Handles newCustCancelButton.Click
        continueAdding = False
        Me.Close()
    End Sub

    Private Sub newCustOKButton_Click(sender As Object, e As EventArgs) Handles newCustOKButton.Click
        continueAdding = True
        Me.Close()
        'If slsTax1ComboBox.SelectedIndex = -1 Or slsTax2ComboBox.SelectedIndex = -1 Then
        '    Dim ret = MsgBox("Empty Sales tax. Do you wish to continue?", MsgBoxStyle.YesNoCancel, "No Sales Tax")
        '    If ret = MsgBoxResult.Yes Then
        '        continueAdding = True
        '        Me.Close()
        '    ElseIf MsgBoxResult.No Then
        '        continueAdding = False
        '    Else
        '        continueAdding = False
        '        Me.Close()
        '    End If
        'Else
        '    continueAdding = True
        '    Me.Close()
        'End If
    End Sub

    Private Sub slsTax1ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles slsTax1ComboBox.SelectedIndexChanged
        If slsTax1ComboBox.SelectedIndex = 0 Or slsTax1ComboBox.SelectedIndex = 1 Then
            ex1NumberTextBox.Enabled = False
        Else
            ex1NumberTextBox.Enabled = True
        End If
    End Sub

    Private Sub slsTax2ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles slsTax2ComboBox.SelectedIndexChanged
        If slsTax2ComboBox.SelectedIndex = 0 Or slsTax2ComboBox.SelectedIndex = 1 Then
            ex2NumberTextBox.Enabled = False
        Else
            ex2NumberTextBox.Enabled = True
        End If
    End Sub

    Private Sub newShipSkipButton_Click(sender As Object, e As EventArgs) Handles newShipSkipButton.Click
        skipShipTo = True
        continueAdding = True
        Me.Close()
    End Sub
End Class