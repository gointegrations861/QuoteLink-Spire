<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewCustForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewCustForm))
        Me.newCustCancelButton = New System.Windows.Forms.Button()
        Me.newCustOKButton = New System.Windows.Forms.Button()
        Me.CustNoTextBox = New System.Windows.Forms.TextBox()
        Me.ex1NumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.slsTax2ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.slsTax1ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.currencySelectComboBox = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.sellingPriceLevelComboBox = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CustNameTextBox = New System.Windows.Forms.TextBox()
        Me.ex2NumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'newCustCancelButton
        '
        Me.newCustCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.newCustCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.newCustCancelButton.Location = New System.Drawing.Point(200, 312)
        Me.newCustCancelButton.Name = "newCustCancelButton"
        Me.newCustCancelButton.Size = New System.Drawing.Size(75, 25)
        Me.newCustCancelButton.TabIndex = 8
        Me.newCustCancelButton.Text = "Cancel"
        Me.newCustCancelButton.UseVisualStyleBackColor = True
        '
        'newCustOKButton
        '
        Me.newCustOKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.newCustOKButton.Location = New System.Drawing.Point(119, 312)
        Me.newCustOKButton.Name = "newCustOKButton"
        Me.newCustOKButton.Size = New System.Drawing.Size(75, 25)
        Me.newCustOKButton.TabIndex = 7
        Me.newCustOKButton.Text = "OK"
        Me.newCustOKButton.UseVisualStyleBackColor = True
        '
        'CustNoTextBox
        '
        Me.CustNoTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustNoTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CustNoTextBox.Location = New System.Drawing.Point(125, 51)
        Me.CustNoTextBox.MaxLength = 20
        Me.CustNoTextBox.Name = "CustNoTextBox"
        Me.CustNoTextBox.Size = New System.Drawing.Size(150, 22)
        Me.CustNoTextBox.TabIndex = 3
        '
        'ex1NumberTextBox
        '
        Me.ex1NumberTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ex1NumberTextBox.Enabled = False
        Me.ex1NumberTextBox.Location = New System.Drawing.Point(201, 135)
        Me.ex1NumberTextBox.MaxLength = 20
        Me.ex1NumberTextBox.Name = "ex1NumberTextBox"
        Me.ex1NumberTextBox.Size = New System.Drawing.Size(74, 22)
        Me.ex1NumberTextBox.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(122, 138)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 16)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Exempt No."
        '
        'slsTax2ComboBox
        '
        Me.slsTax2ComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.slsTax2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.slsTax2ComboBox.FormattingEnabled = True
        Me.slsTax2ComboBox.Location = New System.Drawing.Point(125, 163)
        Me.slsTax2ComboBox.Name = "slsTax2ComboBox"
        Me.slsTax2ComboBox.Size = New System.Drawing.Size(150, 22)
        Me.slsTax2ComboBox.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 166)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Sales Tax 2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 16)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Sales Tax 1"
        '
        'slsTax1ComboBox
        '
        Me.slsTax1ComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.slsTax1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.slsTax1ComboBox.FormattingEnabled = True
        Me.slsTax1ComboBox.Location = New System.Drawing.Point(125, 107)
        Me.slsTax1ComboBox.Name = "slsTax1ComboBox"
        Me.slsTax1ComboBox.Size = New System.Drawing.Size(150, 22)
        Me.slsTax1ComboBox.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(49, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(189, 39)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Please enter the new Customer information"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 16)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Customer Number *"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 222)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 16)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Default Currency"
        '
        'currencySelectComboBox
        '
        Me.currencySelectComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.currencySelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.currencySelectComboBox.FormattingEnabled = True
        Me.currencySelectComboBox.Location = New System.Drawing.Point(125, 219)
        Me.currencySelectComboBox.Name = "currencySelectComboBox"
        Me.currencySelectComboBox.Size = New System.Drawing.Size(150, 22)
        Me.currencySelectComboBox.TabIndex = 16
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 250)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(102, 16)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Selling Price Level"
        '
        'sellingPriceLevelComboBox
        '
        Me.sellingPriceLevelComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sellingPriceLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sellingPriceLevelComboBox.FormattingEnabled = True
        Me.sellingPriceLevelComboBox.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
        Me.sellingPriceLevelComboBox.Location = New System.Drawing.Point(125, 247)
        Me.sellingPriceLevelComboBox.Name = "sellingPriceLevelComboBox"
        Me.sellingPriceLevelComboBox.Size = New System.Drawing.Size(150, 22)
        Me.sellingPriceLevelComboBox.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 280)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 16)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "* Required"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 82)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 16)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Customer Name *"
        '
        'CustNameTextBox
        '
        Me.CustNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustNameTextBox.Location = New System.Drawing.Point(125, 79)
        Me.CustNameTextBox.MaxLength = 60
        Me.CustNameTextBox.Name = "CustNameTextBox"
        Me.CustNameTextBox.Size = New System.Drawing.Size(150, 22)
        Me.CustNameTextBox.TabIndex = 21
        '
        'ex2NumberTextBox
        '
        Me.ex2NumberTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ex2NumberTextBox.Enabled = False
        Me.ex2NumberTextBox.Location = New System.Drawing.Point(201, 191)
        Me.ex2NumberTextBox.MaxLength = 20
        Me.ex2NumberTextBox.Name = "ex2NumberTextBox"
        Me.ex2NumberTextBox.Size = New System.Drawing.Size(74, 22)
        Me.ex2NumberTextBox.TabIndex = 22
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(122, 194)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 16)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "Exempt No."
        '
        'NewCustForm
        '
        Me.AcceptButton = Me.newCustOKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.newCustCancelButton
        Me.ClientSize = New System.Drawing.Size(287, 347)
        Me.Controls.Add(Me.ex2NumberTextBox)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.CustNameTextBox)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.CustNoTextBox)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.sellingPriceLevelComboBox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.currencySelectComboBox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ex1NumberTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.slsTax2ComboBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.slsTax1ComboBox)
        Me.Controls.Add(Me.newCustOKButton)
        Me.Controls.Add(Me.newCustCancelButton)
        Me.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "NewCustForm"
        Me.Text = "New Customer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents newCustCancelButton As System.Windows.Forms.Button
    Friend WithEvents newCustOKButton As System.Windows.Forms.Button
    Friend WithEvents CustNoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ex1NumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents slsTax2ComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents slsTax1ComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents currencySelectComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents sellingPriceLevelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CustNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ex2NumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
