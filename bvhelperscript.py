# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'searchForm.ui'
#
# Created: Tue Feb  5 10:04:45 2013
#      by: pyside-uic 0.2.14 running on PySide 1.1.2
#
# WARNING! All changes made in this file will be lost!
import sys
from datetime import datetime
from ctypes import *

class BVLogonRecord(Structure):
	_fields_ = [("Directory",                           c_char * 261),
				("UserID",                              c_char * 12),
				("Password",                            c_char * 12),
				("LogonDate",                           c_char * 8)]

class BVTelephoneNo(Structure):
	_fields_ = [("TelNoType",                           c_char * 1),
				("TelNo",                               c_char * 30)]

class BVAddress(Structure):
	_fields_ = [("Line1",                               c_char * 45),
				("Line2",                               c_char * 45),
				("Line3",                               c_char * 45),
				("Line4",                               c_char * 45),
				("City",                                c_char * 45),
				("ProvState",                           c_char * 2),
				("PostalZip",                           c_char * 16),
				("CountryCode",                         c_char * 3),
				("Telephone1",                          BVTelephoneNo),
				("Telephone2",                          BVTelephoneNo),
				("Email",                               c_char * 80)]

class BVContactInfo(Structure):
	_fields_ = [("Name",                                c_char * 60),
				("Telephone1",                          BVTelephoneNo),
				("Telephone2",                          BVTelephoneNo),
				("Email",                               c_char * 80)]

class BVCoAddress(Structure):
	_fields_ = [("Address",                             BVAddress),
				("WebPage",                             c_char * 80),
				("Contact1",                            BVContactInfo),
				("Contact2",                            BVContactInfo),
				("Contact3",                            BVContactInfo)]

class BVAddressRecord(Structure):
	_fields_ = [("Name",                                c_char * 60),
				("Company",                             BVCoAddress),
				("OnHold",                              c_char * 1),
				("Territory",                           c_char * 10),
				("TerritoryDescription",                c_char * 80),
				("Salesperson",                         c_char * 10),
				("SalespersonName",                     c_char * 60),
				("ShipViaCode",                         c_char * 10),
				("ShipViaDescription",                  c_char * 60),
				("DefaultWarehouse",                    c_char * 6),
				("RVAccount",                           c_char * 24),
				("SellingNo",                           c_char * 2),
				("SlsTax1Number",                       c_char * 4),
				("SlsTax2Number",                       c_char * 4),
				("SlsTaxNumber",                        c_char * 4 * 2),
				("SlsTaxExempt1",                       c_char * 20),
				("SlsTaxExempt",                        c_char * 20 * 3)]

#CustLastInvoiceDate	Mask   YYYYMMDD
#CustPresentBalance		Mask   ±#############.##
#CustLastYearSales		Mask   ±#############.##
#CustThisYearSales		Mask   ±#############.##
#CustNextYearSales		Mask   ±#############.##
#CustThisYearGP			Mask   ±#############.##
#CustLastYearGP			Mask   ±#############.##
#CustNextYearGP			Mask   ±#############.##
#CustCreditCode			Mask   "0", "1", "2"
#CustCreditLine			Mask   ±#############
#CustDiscountPct		Mask   ±###.##

class BVCustomerRecord(Structure):
	_fields_ = [("CustCode",                            c_char * 20),
				("CustCurrency",                        c_char * 3),
				("CustName",                            c_char * 60),
				("CustLastInvoiceNo",                   c_char * 25),
				("CustLastInvoiceDate",                 c_char * 8),
				("CustPresentBalance",                  c_char * 20),
				("CustLastYearSales",                   c_char * 20),
				("CustThisYearSales",                   c_char * 20),
				("CustNextYearSales",                   c_char * 20),
				("CustThisYearGP",                      c_char * 20),
				("CustLastYearGP",                      c_char * 20),
				("CustNextYearGP",                      c_char * 20),
				("CustCreditCode",                      c_char * 1),
				("CustCreditLine",                      c_char * 20),
				("CustDiscountPct",                     c_char * 10),
				("CustTermCode",                        c_char * 10),
				("CustNotes",                           c_char * 30),
				("CustMisc",                            c_char * 10),
				("CustSpecialCode",                     c_char * 1),
				("CustPriceCode",                       c_char * 1),
				("CustServiceCharge",                   c_char * 1),
				("CustTaxPrompt",                       c_char * 1),
				("CustDefaultShipTo",                   c_char * 20),
				("CustHoldFlag",                        c_char * 1),
				("CustECommerceFlag",                   c_char * 1),
				("CustEShipmentNotify",                 c_char * 1),
				("CustEPastDueNotices",                 c_char * 1),
				("CustEMonthlyStatements",              c_char * 1),
				("CustStatementType",                   c_char * 1),
				("CustInvoiceType",                     c_char * 1),
				("CustPONoRequired",                    c_char * 1),
				("CustARAccount",                       c_char * 24),
				("CustAvgDaysToPay",                    c_char * 20),
				("CustQuotesType",                      c_char * 1),
				("CustOrdConfType",                     c_char * 1),
				("AddressDetails",                      BVAddressRecord)]

class BVFullAddressRecord(Structure):
	_fields_ = [("Code",								c_char * 20),
				("ID",									c_char * 20),
				("AddressDetails",						BVAddressRecord)]

class BVQuantityBreaks(Structure):
	_fields_ = [("InvBreakQuantity",					c_char * 11),
				("InvBreakPrice",						c_char * 17)]

#Units					Mask ±##########.#####
#Amount					Mask ±#############.##

class BVYearSales(Structure):
	_fields_ = [("Units",								c_char * 17),
				("Amount",								c_char * 17)]


class BVSalesByPeriod(Structure):
	_fields_ = [("LastYearSales",						BVYearSales * 13),
				("ThisYearSales",						BVYearSales * 13),
				("NextYearSales",						BVYearSales * 13)]

#InvCurrentCost			Mask ±##########.#####
#InvAverageCost			Mask ±##########.#####
#InvSellingPrice		Mask ±##########.#####
#InvPromoStartDate		Mask YYYYMMDD
#InvPromoEndDate		Mask YYYYMMDD
#InvPromoSellingPrice	Mask ±##########.#####
#InvOnHandQty			Mask ±##########.#####
#InvReorderPointQty		Mask ±##########.#####
#InvCommittedQty		Mask ±##########.#####
#InvBackOrderQty		Mask ±##########.#####
#InvOnOrderQty			Mask ±##########.#####
#InvMisc2				Mask ±##########.#####
#InvWeight				Mask ±######.#####

class BVInventoryRecord(Structure):
	_fields_ = [("InvWhse",								c_char * 6),
				("InvPartCode",							c_char * 34),
				("InvDescription",						c_char * 80),
				("InvProductCode",						c_char * 10),
				("InvLocation",							c_char * 20),
				("InvOnHoldFlag",						c_char * 1),
				("InvCurrentCost",						c_char * 17),
				("InvAverageCost",						c_char * 17),
				("InvSellingPrice1",					c_char * 17),
				("InvSellingPrice",						c_char * 17 * 19),
				("InvPromoStartDate",					c_char * 8),
				("InvPromoEndDate",						c_char * 8),
				("InvPromoFlag",						c_char * 1),
				("InvPromoMandatoryFlag",				c_char * 1),
				("InvPromoSellingPrice",				c_char * 17),
				("InvQuantityBreak",					BVQuantityBreaks * 9),
				("InvTaxFlag1",							c_char * 1),
				("InvTaxFlag2",							c_char * 1),
				("InvTaxFlag",							c_char * 1 * 2),
				("InvMeasure",							c_char * 10),
				("InvCurrentPONo",						c_char * 10),
				("InvPODueDate",						c_char * 8),
				("InvMinBuyQty",						c_char * 11),
				("InvDiscountableFlag",					c_char * 1),
				("InvSerializedFlag",					c_char * 1),
				("InvSalesAccount",						c_char * 4),
				("InvOnHandQty",						c_char * 17),
				("InvReorderPointQty",					c_char * 17),
				("InvCommittedQty",						c_char * 17),
				("InvBackOrderQty",						c_char * 17),
				("InvOnOrderQty",						c_char * 17),
				("InvAltPartWarehouse",					c_char * 6),
				("InvAltPartCode",						c_char * 34),
				("InvMisc1",							c_char * 30),
				("InvMisc2",							c_char * 17),
				("InvSalesByPeriod",					BVSalesByPeriod),
				("InvType",								c_char * 1),
				("InvImageFileName",					c_char * 261),
				("InvWeight",							c_char * 13),
				("InvECommerceFlag",					c_char * 1),
				("InvUPCCode",							c_char * 40),
				("InvPreferredVendor",					c_char * 20),
				("InvXtdDesc",							c_char * 5000),
				("InvUomDesc",							c_char * 80),
				("InvAllowBackOrders",					c_char * 1),
				("InvAllowReturns",						c_char * 1)]


# loadDLL = WinDLL("C:\Program Files (x86)\Sage BusinessVision SDK\BV7api.dll")


# BVLogonWithDir = loadDLL['BVLogonWithDir']
# BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
# BVLogonWithDir.restype = c_int
# BVAddCustomer = loadDLL['BVAddCustomer']
# BVAddCustomer.argtypes = [POINTER(BVCustomerRecord)]
# BVAddCustomer.restype = c_int

# bvdll = WinDLL('C:\Program Files (x86)\Sage BusinessVision SDK\BV7api.dll')
# bvdll.BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
# bvdll.BVLogonWithDir.restype = c_short
# bvdll.BVAddCustomer.argtypes = [POINTER(BVCustomerRecord)]
# bvdll.BVAddCustomer.restype = c_int

def logonProcess(loc,user,password,logondate):
	bvdll = WinDLL('BV7api.dll')
	bvdll.BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
	bvdll.BVLogonWithDir.restype = c_int
	#LogonRecord = BVLogonRecord(loc,user,password,datetime.now().strftime("%Y%m%d"))
	LogonRecord = BVLogonRecord(loc,user,password,logondate)

	logonRet = bvdll.BVLogonWithDir(byref(LogonRecord))

	sys.exit(logonRet)

def createCustomer(loc,user,password,logondate,custno,custname,addrname,addr1,addr2,addr3,city,provstate,countrycode,postal,phone,email,slstax1,slstax2,exno,currency,selllevel,shipvia,shipviadesc):
	bvdll = WinDLL('BV7api.dll')
	bvdll.BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
	bvdll.BVLogonWithDir.restype = c_int
	
	#LogonRecord = BVLogonRecord(loc,user,password,datetime.now().strftime("%Y%m%d"))
	LogonRecord = BVLogonRecord(loc,user,password,logondate)
	
	logonRet = bvdll.BVLogonWithDir(byref(LogonRecord))
	
	if logonRet != 0:
		print "couldnt logon: " + str(logonRet)
		sys.exit(logonRet)
	else:
		bvdll.BVAddCustomer.argtypes = [POINTER(BVCustomerRecord)]
		bvdll.BVAddCustomer.restype = c_int
		
		CustomerRecord = BVCustomerRecord()
		CustomerRecord.CustCode = custno
		CustomerRecord.CustName = custname
		CustomerRecord.CustCurrency = currency
		CustomerRecord.AddressDetails.Name = addrname
		CustomerRecord.AddressDetails.SlsTax1Number = slstax1
		CustomerRecord.AddressDetails.SlsTax2Number = slstax2
		CustomerRecord.AddressDetails.SlsTaxExempt1 = exno
		CustomerRecord.AddressDetails.SellingNo = selllevel
		CustomerRecord.AddressDetails.ShipViaCode = shipvia
		CustomerRecord.AddressDetails.ShipViaDescription = shipviadesc
		CustomerRecord.AddressDetails.Company.Address.Email = email
		CustomerRecord.AddressDetails.Company.Address.Line1 = addr1
		CustomerRecord.AddressDetails.Company.Address.Line2 = addr2
		CustomerRecord.AddressDetails.Company.Address.Line3 = addr3
		CustomerRecord.AddressDetails.Company.Address.City = city
		CustomerRecord.AddressDetails.Company.Address.ProvState = provstate
		CustomerRecord.AddressDetails.Company.Address.CountryCode = countrycode
		CustomerRecord.AddressDetails.Company.Address.PostalZip = postal
		CustomerRecord.AddressDetails.Company.Address.Telephone1.TelNo = phone
		CustomerRecord.AddressDetails.Company.Address.Email = email
		
		addRet = bvdll.BVAddCustomer(byref(CustomerRecord))
		print addRet
		sys.exit(addRet)

def createShipTo(loc,user,password,logondate,custno,addrname,addr1,addr2,addr3,city,provstate,countrycode,postal,phone,fax,email,slstax1,slstax2,exno,selllevel, shipvia, shipviadesc):
	bvdll = WinDLL('BV7api.dll')
	bvdll.BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
	bvdll.BVLogonWithDir.restype = c_int
	
	#LogonRecord = BVLogonRecord(loc,user,password,datetime.now().strftime("%Y%m%d"))
	LogonRecord = BVLogonRecord(loc,user,password,logondate)

	logonRet = bvdll.BVLogonWithDir(byref(LogonRecord))
	
	if logonRet != 0:
		print "couldnt logon: " + str(logonRet)
		sys.exit(logonRet)
	else:
		bvdll.BVAddShipToAddress.argtypes = [POINTER(BVFullAddressRecord)]
		bvdll.BVAddShipToAddress.restype = c_int
		
		ShipTo = BVFullAddressRecord()
		ShipTo.Code = custno
		ShipTo.ID = addrname
		ShipTo.AddressDetails.Name = addrname
		ShipTo.AddressDetails.SlsTax1Number = slstax1
		ShipTo.AddressDetails.SlsTax2Number = slstax2
		ShipTo.AddressDetails.SlsTaxExempt1 = exno
		ShipTo.AddressDetails.SellingNo = selllevel
		ShipTo.AddressDetails.ShipViaCode = shipvia
		ShipTo.AddressDetails.ShipViaDescription = shipviadesc
		ShipTo.AddressDetails.Company.Address.Line1 = addr1
		ShipTo.AddressDetails.Company.Address.Line2 = addr2
		ShipTo.AddressDetails.Company.Address.Line3 = addr3
		ShipTo.AddressDetails.Company.Address.City = city
		ShipTo.AddressDetails.Company.Address.ProvState = provstate
		ShipTo.AddressDetails.Company.Address.CountryCode = countrycode
		ShipTo.AddressDetails.Company.Address.PostalZip = postal
		ShipTo.AddressDetails.Company.Address.Telephone1.TelNo = phone
		ShipTo.AddressDetails.Company.Address.Email = email
		
		addRet = bvdll.BVAddShipToAddress(byref(ShipTo))
		print addRet
		sys.exit(addRet)

def createItem(loc,user,password,logondate,whse,partno,desc,xdesc,price,cost):
	bvdll = WinDLL('BV7api.dll')
	bvdll.BVLogonWithDir.argtypes = [POINTER(BVLogonRecord)]
	bvdll.BVLogonWithDir.restype = c_int
	
	#LogonRecord = BVLogonRecord(loc,user,password,datetime.now().strftime("%Y%m%d"))
	LogonRecord = BVLogonRecord(loc,user,password,logondate)

	logonRet = bvdll.BVLogonWithDir(byref(LogonRecord))
	
	if logonRet != 0:
		print "couldnt logon: " + str(logonRet)
		sys.exit(logonRet)
	else:
		bvdll.BVAddInventoryPart.argtypes = [POINTER(BVInventoryRecord)]
		bvdll.BVAddInventoryPart.restype = c_int
		
		NewItem = BVInventoryRecord()
		NewItem.InvWhse = whse
		NewItem.InvPartCode = partno
		NewItem.InvDescription = desc
		if len(xdesc) > 0:
			NewItem.InvXtdDesc = xdesc.replace("<cr^>", "\n")
		NewItem.InvSellingPrice1 = '+' + '{0:.4f}'.format(float(price)).zfill(16)
		NewItem.InvCurrentCost = '+' + '{0:.4f}'.format(float(cost)).zfill(16)
		NewItem.InvAllowBackOrders = '1'
		NewItem.InvAllowReturns = '1'
		NewItem.InvTaxFlag1 = '1'
		NewItem.InvTaxFlag2 = '1'
		
		addRet = bvdll.BVAddInventoryPart(byref(NewItem))
		print addRet
		sys.exit(addRet)

def main():
	if len(sys.argv) > 1:
		command = sys.argv[1]
		print command
		if command == "customer":
			createCustomer(sys.argv[2],sys.argv[3],sys.argv[4],sys.argv[5],sys.argv[6],sys.argv[7],sys.argv[8],sys.argv[9],sys.argv[10],sys.argv[11],sys.argv[12],sys.argv[13],sys.argv[14],sys.argv[15],sys.argv[16],sys.argv[17],sys.argv[18],sys.argv[19],sys.argv[20],sys.argv[21],sys.argv[22],sys.argv[23],sys.argv[24])
		elif command == "shipto":
			createShipTo(sys.argv[2],sys.argv[3],sys.argv[4],sys.argv[5],sys.argv[6],sys.argv[7],sys.argv[8],sys.argv[9],sys.argv[10],sys.argv[11],sys.argv[12],sys.argv[13],sys.argv[14],sys.argv[15],sys.argv[16],sys.argv[17],sys.argv[18],sys.argv[19],sys.argv[20],sys.argv[21],sys.argv[22],sys.argv[23])
		elif command == "item":
			createItem(sys.argv[2],sys.argv[3],sys.argv[4],sys.argv[5],sys.argv[6],sys.argv[7],sys.argv[8],sys.argv[9],sys.argv[10],sys.argv[11])

if __name__ == '__main__':
	main()