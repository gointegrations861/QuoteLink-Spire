Imports System.Data.Odbc

Public Class NewCustForm
    Public continueAdding = False

    Public Sub New(ByRef spireAPI As SpireAPIContainer)
        Dim defaultTax01 As Int64 = 1
        Dim defaultTax02 As Int64 = 2
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'Sales Taxes
        slsTax1ComboBox.Items.Add("<Default>")
        slsTax2ComboBox.Items.Add("<Default>")
        slsTax1ComboBox.Items.Add("0 - None")
        slsTax2ComboBox.Items.Add("0 - None")

        Dim salesTaxes = spireAPI.GetSalesTaxes()

        For i As Integer = 0 To Integer.Parse(salesTaxes("count")) - 1
            slsTax1ComboBox.Items.Add(salesTaxes("records")(i)("code") & " - " & salesTaxes("records")(i)("name") & " (" & salesTaxes("records")(i)("rate") & "%)")
            slsTax2ComboBox.Items.Add(salesTaxes("records")(i)("code") & " - " & salesTaxes("records")(i)("name") & " (" & salesTaxes("records")(i)("rate") & "%)")
        Next
        slsTax1ComboBox.SelectedIndex = 0
        slsTax2ComboBox.SelectedIndex = 0

        'Currency
        currencySelectComboBox.Items.Add("<Default>")

        Dim currencies = spireAPI.GetCurrencies()

        For i As Integer = 0 To Integer.Parse(currencies("count")) - 1
            currencySelectComboBox.Items.Add(currencies("records")(i)("code") & " - " & currencies("records")(i)("description"))
        Next
        currencySelectComboBox.SelectedIndex = 0

        sellingPriceLevelComboBox.SelectedIndex = My.Settings.SellPriceLevelCombo
    End Sub

    Private Sub newCustCancelButton_Click(sender As Object, e As EventArgs) Handles newCustCancelButton.Click
        continueAdding = False
        Me.Close()
    End Sub

    Private Sub newCustOKButton_Click(sender As Object, e As EventArgs) Handles newCustOKButton.Click
        continueAdding = True
        Me.Close()
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
End Class