<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewShipToForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewShipToForm))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.newCustOKButton = New System.Windows.Forms.Button()
        Me.newCustCancelButton = New System.Windows.Forms.Button()
        Me.ex2NumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.sellingPriceLevelComboBox = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ex1NumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.slsTax2ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.slsTax1ComboBox = New System.Windows.Forms.ComboBox()
        Me.ShipToNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ShipToIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.newShipSkipButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label3.Location = New System.Drawing.Point(49, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(189, 39)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Please enter the new Ship-To information"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'newCustOKButton
        '
        Me.newCustOKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.newCustOKButton.Location = New System.Drawing.Point(119, 257)
        Me.newCustOKButton.Name = "newCustOKButton"
        Me.newCustOKButton.Size = New System.Drawing.Size(75, 23)
        Me.newCustOKButton.TabIndex = 9
        Me.newCustOKButton.Text = "OK"
        Me.newCustOKButton.UseVisualStyleBackColor = True
        '
        'newCustCancelButton
        '
        Me.newCustCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.newCustCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.newCustCancelButton.Location = New System.Drawing.Point(200, 257)
        Me.newCustCancelButton.Name = "newCustCancelButton"
        Me.newCustCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.newCustCancelButton.TabIndex = 10
        Me.newCustCancelButton.Text = "Cancel"
        Me.newCustCancelButton.UseVisualStyleBackColor = True
        '
        'ex2NumberTextBox
        '
        Me.ex2NumberTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ex2NumberTextBox.Enabled = False
        Me.ex2NumberTextBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.ex2NumberTextBox.Location = New System.Drawing.Point(201, 191)
        Me.ex2NumberTextBox.MaxLength = 20
        Me.ex2NumberTextBox.Name = "ex2NumberTextBox"
        Me.ex2NumberTextBox.Size = New System.Drawing.Size(74, 22)
        Me.ex2NumberTextBox.TabIndex = 34
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label10.Location = New System.Drawing.Point(122, 194)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 16)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "Exempt No."
        '
        'sellingPriceLevelComboBox
        '
        Me.sellingPriceLevelComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.sellingPriceLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sellingPriceLevelComboBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.sellingPriceLevelComboBox.FormattingEnabled = True
        Me.sellingPriceLevelComboBox.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
        Me.sellingPriceLevelComboBox.Location = New System.Drawing.Point(125, 219)
        Me.sellingPriceLevelComboBox.Name = "sellingPriceLevelComboBox"
        Me.sellingPriceLevelComboBox.Size = New System.Drawing.Size(150, 22)
        Me.sellingPriceLevelComboBox.TabIndex = 33
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label7.Location = New System.Drawing.Point(12, 222)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(102, 16)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Selling Price Level"
        '
        'ex1NumberTextBox
        '
        Me.ex1NumberTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ex1NumberTextBox.Enabled = False
        Me.ex1NumberTextBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.ex1NumberTextBox.Location = New System.Drawing.Point(201, 135)
        Me.ex1NumberTextBox.MaxLength = 20
        Me.ex1NumberTextBox.Name = "ex1NumberTextBox"
        Me.ex1NumberTextBox.Size = New System.Drawing.Size(74, 22)
        Me.ex1NumberTextBox.TabIndex = 26
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label4.Location = New System.Drawing.Point(122, 138)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 16)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Exempt No."
        '
        'slsTax2ComboBox
        '
        Me.slsTax2ComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.slsTax2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.slsTax2ComboBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.slsTax2ComboBox.FormattingEnabled = True
        Me.slsTax2ComboBox.Location = New System.Drawing.Point(125, 163)
        Me.slsTax2ComboBox.Name = "slsTax2ComboBox"
        Me.slsTax2ComboBox.Size = New System.Drawing.Size(150, 22)
        Me.slsTax2ComboBox.TabIndex = 25
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label2.Location = New System.Drawing.Point(12, 166)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Sales Tax 2"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label1.Location = New System.Drawing.Point(12, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 16)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Sales Tax 1"
        '
        'slsTax1ComboBox
        '
        Me.slsTax1ComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.slsTax1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.slsTax1ComboBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.slsTax1ComboBox.FormattingEnabled = True
        Me.slsTax1ComboBox.Location = New System.Drawing.Point(125, 107)
        Me.slsTax1ComboBox.Name = "slsTax1ComboBox"
        Me.slsTax1ComboBox.Size = New System.Drawing.Size(150, 22)
        Me.slsTax1ComboBox.TabIndex = 24
        '
        'ShipToNameTextBox
        '
        Me.ShipToNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ShipToNameTextBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.ShipToNameTextBox.Location = New System.Drawing.Point(125, 79)
        Me.ShipToNameTextBox.MaxLength = 60
        Me.ShipToNameTextBox.Name = "ShipToNameTextBox"
        Me.ShipToNameTextBox.Size = New System.Drawing.Size(150, 22)
        Me.ShipToNameTextBox.TabIndex = 39
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.Label9.Location = New System.Drawing.Point(12, 82)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(93, 16)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Ship-To Name *"
        '
        'ShipToIDTextBox
        '
        Me.ShipToIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ShipToIDTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ShipToIDTextBox.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!)
        Me.ShipToIDTextBox.Location = New System.Drawing.Point(125, 51)
        Me.ShipToIDTextBox.MaxLength = 20
        Me.ShipToIDTextBox.Name = "ShipToIDTextBox"
        Me.ShipToIDTextBox.Size = New System.Drawing.Size(150, 22)
        Me.ShipToIDTextBox.TabIndex = 36
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Tai Le", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 53)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 16)
        Me.Label5.TabIndex = 37
        Me.Label5.Text = "Ship-To ID *"
        '
        'newShipSkipButton
        '
        Me.newShipSkipButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.newShipSkipButton.Location = New System.Drawing.Point(38, 257)
        Me.newShipSkipButton.Name = "newShipSkipButton"
        Me.newShipSkipButton.Size = New System.Drawing.Size(75, 23)
        Me.newShipSkipButton.TabIndex = 40
        Me.newShipSkipButton.Text = "Skip"
        Me.newShipSkipButton.UseVisualStyleBackColor = True
        '
        'NewShipToForm
        '
        Me.AcceptButton = Me.newCustOKButton
        Me.AccessibleDescription = ""
        Me.AccessibleName = ""
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.newCustCancelButton
        Me.ClientSize = New System.Drawing.Size(287, 291)
        Me.Controls.Add(Me.newShipSkipButton)
        Me.Controls.Add(Me.ShipToNameTextBox)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ShipToIDTextBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ex2NumberTextBox)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.sellingPriceLevelComboBox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.ex1NumberTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.slsTax2ComboBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.slsTax1ComboBox)
        Me.Controls.Add(Me.newCustOKButton)
        Me.Controls.Add(Me.newCustCancelButton)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "NewShipToForm"
        Me.Text = "New Ship-To"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents newCustOKButton As System.Windows.Forms.Button
    Friend WithEvents newCustCancelButton As System.Windows.Forms.Button
    Friend WithEvents ex2NumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents sellingPriceLevelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ex1NumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents slsTax2ComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents slsTax1ComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ShipToNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ShipToIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents newShipSkipButton As Button
End Class
