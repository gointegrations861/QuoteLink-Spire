﻿Public Class Customizations
    Private Sub Customizations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        orderDataGridView.Rows.Add(14)
        orderDataGridView.Rows.Item(0).Cells.Item(0).Value = "CustomText09"
        orderDataGridView.Rows.Item(0).Cells.Item(1).Value = "Customer No"
        orderDataGridView.Rows.Item(1).Cells.Item(0).Value = "ShipToContact"
        orderDataGridView.Rows.Item(1).Cells.Item(1).Value = "Order Contact"
        orderDataGridView.Rows.Item(2).Cells.Item(0).Value = "ShipToEmail"
        orderDataGridView.Rows.Item(2).Cells.Item(1).Value = "Order Email"
        orderDataGridView.Rows.Item(3).Cells.Item(0).Value = "ShipToPhone + ShipToPhoneExt"
        orderDataGridView.Rows.Item(3).Cells.Item(1).Value = "Order Phone"
        orderDataGridView.Rows.Item(4).Cells.Item(0).Value = "ShipToFax + ShipToFaxExt"
        orderDataGridView.Rows.Item(4).Cells.Item(1).Value = "Order Fax"
        orderDataGridView.Rows.Item(5).Cells.Item(0).Value = "Quote Document Number"
        orderDataGridView.Rows.Item(5).Cells.Item(1).Value = "QW_EISQ (UDF)"
        orderDataGridView.Rows.Item(6).Cells.Item(0).Value = "<current time>"
        orderDataGridView.Rows.Item(6).Cells.Item(1).Value = "QW_Convert (UDF)"
        orderDataGridView.Rows.Item(7).Cells.Item(0).Value = """TRUE"""
        orderDataGridView.Rows.Item(7).Cells.Item(1).Value = "QW_NewCust (UDF)"
        orderDataGridView.Rows.Item(8).Cells.Item(0).Value = "PurchasingNotes"
        orderDataGridView.Rows.Item(8).Cells.Item(1).Value = "QW_PONotes (UDF)"
        orderDataGridView.Rows.Item(9).Cells.Item(0).Value = "InternalNotes"
        orderDataGridView.Rows.Item(9).Cells.Item(1).Value = "QW_IntNote (UDF)"
        orderDataGridView.Rows.Item(10).Cells.Item(0).Value = "ShipToContact"
        orderDataGridView.Rows.Item(10).Cells.Item(1).Value = "QW_SoldTo (UDF)"
        orderDataGridView.Rows.Item(11).Cells.Item(0).Value = "ShipToEmail"
        orderDataGridView.Rows.Item(11).Cells.Item(1).Value = "QW_STEmail (UDF)"
        orderDataGridView.Rows.Item(12).Cells.Item(0).Value = "ShipToPhone + ShipToPhoneExt"
        orderDataGridView.Rows.Item(12).Cells.Item(1).Value = "QW_STPhone (UDF)"
        orderDataGridView.Rows.Item(13).Cells.Item(0).Value = "CustomText01"
        orderDataGridView.Rows.Item(13).Cells.Item(1).Value = "qw_lead_time (UDF)"

        orderItemDataGridView.Rows.Add(12)
        orderItemDataGridView.Rows.Item(0).Cells.Item(0).Value = "AlternateUnitPrice"
        orderItemDataGridView.Rows.Item(0).Cells.Item(1).Value = "Unit Price"
        orderItemDataGridView.Rows.Item(1).Cells.Item(0).Value = "ManufacturerPartNumber"
        orderItemDataGridView.Rows.Item(1).Cells.Item(1).Value = "Description"
        orderItemDataGridView.Rows.Item(2).Cells.Item(0).Value = "Description"
        orderItemDataGridView.Rows.Item(2).Cells.Item(1).Value = "Extended Description"
        orderItemDataGridView.Rows.Item(3).Cells.Item(0).Value = "Category"
        orderItemDataGridView.Rows.Item(3).Cells.Item(1).Value = "Product Code"
        orderItemDataGridView.Rows.Item(4).Cells.Item(0).Value = "CustomText07"
        orderItemDataGridView.Rows.Item(4).Cells.Item(1).Value = "HS Code"
        orderItemDataGridView.Rows.Item(5).Cells.Item(0).Value = "CustomText08"
        orderItemDataGridView.Rows.Item(5).Cells.Item(1).Value = "Primary Vendor"
        orderItemDataGridView.Rows.Item(6).Cells.Item(0).Value = "CustomText09"
        orderItemDataGridView.Rows.Item(6).Cells.Item(1).Value = "Country of Origin"
        orderItemDataGridView.Rows.Item(7).Cells.Item(0).Value = "CustomText01"
        'orderItemDataGridView.Rows.Item(7).Cells.Item(1).Value = "Quoted Lead Time (Comment in order details)"
        orderItemDataGridView.Rows.Item(7).Cells.Item(1).Value = "QW_CT01 (UDF)"
        orderItemDataGridView.Rows.Item(8).Cells.Item(0).Value = "<no mapping>"
        orderItemDataGridView.Rows.Item(8).Cells.Item(1).Value = "'Customer Request Date: See Above' (Comment in order details)"
        orderItemDataGridView.Rows.Item(9).Cells.Item(0).Value = "CustomText02"
        orderItemDataGridView.Rows.Item(9).Cells.Item(1).Value = "QW_CT02 (UDF)"
        orderItemDataGridView.Rows.Item(10).Cells.Item(0).Value = "VendorPartNumber"
        orderItemDataGridView.Rows.Item(10).Cells.Item(1).Value = "QW_VendorP (UDF)"
        orderItemDataGridView.Rows.Item(11).Cells.Item(0).Value = "AlternateUnitPrice"
        orderItemDataGridView.Rows.Item(11).Cells.Item(1).Value = "qw_lincost (UDF)"

        customerDataGridView.Rows.Add(2)
        customerDataGridView.Rows.Item(0).Cells.Item(0).Value = "Postal Code"
        customerDataGridView.Rows.Item(0).Cells.Item(1).Value = "Territory (Via Spreadsheet)"
        customerDataGridView.Rows.Item(1).Cells.Item(0).Value = "CustomText11"
        customerDataGridView.Rows.Item(1).Cells.Item(1).Value = "zoho_account_id (Internal database field)"

        writebackDataGridView.Rows.Add(1)
        writebackDataGridView.Rows.Item(0).Cells.Item(0).Value = "Order Number"
        writebackDataGridView.Rows.Item(0).Cells.Item(1).Value = "CustomText07 (Order)"
    End Sub

End Class