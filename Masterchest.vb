Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Text
Imports System.IO
Imports Org.BouncyCastle.Math.EC

Public Class mlib
    '/////////////
    '///STRUCTURES
    '/////////////
    Public publickeybytes As Byte()
    Public multisig As Boolean
    Public Class bitcoinrpcconnection
        Public bitcoinrpcserver As String
        Public bitcoinrpcport As Integer
        Public bitcoinrpcuser As String
        Public bitcoinrpcpassword As String
    End Class
    Public Class result_validate
        Public isvalid As Boolean
        Public address As String
        Public ismine As Boolean
        Public isscript As Boolean
        Public pubkey As String
        Public iscompressed As Boolean
        Public account As String
    End Class
    Public Class validate
        Public result As result_validate
        Public err As Object
        Public id As String
    End Class
    Public Class blockhash
        Public id As String
        Public err As Object
        Public result As String
    End Class
    Public Class mastercointx
        Public txid As String
        Public toadd As String
        Public fromadd As String
        Public value As Long
        Public type As String
        Public blocktime As Long
        Public valid As Integer
        Public curtype As Integer
    End Class
    Public Class result_block
        Public hash As String
        Public confirmations As Integer
        Public size As Integer
        Public height As Integer
        Public version As Integer
        Public merkleroot As String
        Public tx As List(Of String)
        Public time As Long
        Public nonce As Long
        Public bits As String
        Public difficulty As Double
        Public previousblockhash As String
        Public nextblockhash As String
    End Class
    Public Class Block
        Public result As result_block
        Public err As Object
        Public id As String
    End Class
    Public Class bttx
        Public data As String
        Public hash As String
        Public depends
        Public fee As Long
        Public sigops As Integer
    End Class
    Public Class result_blocktemplate
        Public version As Integer
        Public previousblockhash As String
        Public transactions As List(Of bttx)
        Public coinbaseasux
        Public coinbasevalue As Long
        Public target As String
        Public mintime As Long
        Public mutable
        Public noncerange As String
        Public sigoplimit As Integer
        Public sizelimit As Long
        Public curtime As Long
        Public bits As String
        Public height As Integer
    End Class
    Public Class blocktemplate
        Public result As result_blocktemplate
        Public err As Object
        Public id As String
    End Class
    Public Class ScriptSig
        Public asm As String
        Public hex As String
    End Class
    Public Class Vin
        Public txid As String
        Public vout As Integer
        Public scriptSig As ScriptSig
        Public sequence As Object
    End Class
    Public Class ScriptPubKey
        Public asm As String
        Public hex As String
        Public reqSigs As Integer
        Public type As String
        Public addresses As List(Of String)
    End Class
    Public Class Vout
        Public value As Double
        Public n As Integer
        Public scriptPubKey As ScriptPubKey
    End Class
    Public Class result_txn
        Public hex As String
        Public txid As String
        Public version As Integer
        Public locktime As Integer
        Public vin As List(Of Vin)
        Public vout As List(Of Vout)
        Public blockhash As String
        Public confirmations As Integer
        Public time As Integer
        Public blocktime As Integer
    End Class
    Public Class txn
        Public result As result_txn
        Public err As Object
        Public id As String
    End Class
    Public Class result_unspent
        Public txid As String
        Public vout As Integer
        Public address As String
        Public account As String
        Public scriptpubkey As String
        Public amount As Double
        Public confirmations As Integer
    End Class
    Public Class unspent
        Public result As List(Of result_unspent)
        Public err As Object
        Public id As String
    End Class
    Public Class isvalid
        Public result As result_isvalid
        Public err As Object
        Public id As String
    End Class
    Public Class result_isvalid
        Public isvalid As String
    End Class
    Public Class blockcount
        Public result As String
        Public err As Object
        Public id As String
    End Class
    Public Class result_blockcount
        Public result As Int64
    End Class
    Public Class rcvaddress
        Public result As List(Of result_rcvaddress)
        Public err As Object
        Public id As String
    End Class
    Public Class result_rcvaddress
        Public address As String
        Public account As String
        Public amount As Double
        Public confirmations As Integer
    End Class
    Public Class btcaddressbal
        Public address As String
        Public amount As Double
        Public uamount As Double
    End Class

    '////////////
    '///FUNCTIONS
    '////////////
    Public Function ToByteArray(ByVal base58 As String) As Byte()
        Dim bi2 As New Org.BouncyCastle.Math.BigInteger("0")
        Dim b58 As String = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"

        For Each c As Char In base58
            If b58.IndexOf(c) <> -1 Then
                bi2 = bi2.Multiply(New Org.BouncyCastle.Math.BigInteger("58"))
                bi2 = bi2.Add(New Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()))
            Else
                Return Nothing
            End If
        Next

        Dim bb As Byte() = bi2.ToByteArrayUnsigned()

        ' interpret leading '1's as leading zero bytes
        For Each c As Char In base58
            If c <> "1"c Then
                Exit For
            End If
            Dim bbb As Byte() = New Byte(bb.Length) {}
            Array.Copy(bb, 0, bbb, 1, bb.Length)
            bb = bbb
        Next
        Return bb
    End Function

    Public Function FromByteArray(ByVal ba As Byte()) As String
        Dim addrremain As New Org.BouncyCastle.Math.BigInteger(1, ba)

        Dim big0 As New Org.BouncyCastle.Math.BigInteger("0")
        Dim big58 As New Org.BouncyCastle.Math.BigInteger("58")

        Dim b58 As String = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"

        Dim rv As String = ""

        While addrremain.CompareTo(big0) > 0
            Dim d As Integer = Convert.ToInt32(addrremain.[Mod](big58).ToString())
            addrremain = addrremain.Divide(big58)
            rv = b58.Substring(d, 1) & rv
        End While

        ' handle leading zeroes
        For Each b As Byte In ba
            If b <> 0 Then
                Exit For
            End If

            rv = "1" & rv
        Next
        Return rv
    End Function

    Public Function multisigbarray(key As String)
        Dim parts(key.Length \ 2 - 1) As String
        Dim larray As New List(Of Byte)
        For x As Integer = 0 To key.Length - 1 Step 2
            parts(x \ 2) = key.Substring(x, 2)
            larray.Add(Byte.Parse(parts(x \ 2), System.Globalization.NumberStyles.HexNumber))
        Next
        Dim barray As Byte() = larray.ToArray
        Return barray
    End Function

    Public Function rpccall(ByVal bitcoin_con, ByVal method, ByVal param0, ByVal param1, ByVal param2, ByVal param3)
        Try
            Dim webRequest1 As HttpWebRequest = WebRequest.Create("http://" & bitcoin_con.bitcoinrpcserver.ToString & ":" & bitcoin_con.bitcoinrpcport.ToString)
            webRequest1.Credentials = New NetworkCredential(bitcoin_con.bitcoinrpcuser.ToString, bitcoin_con.bitcoinrpcpassword.ToString)
            webRequest1.ContentType = "application/json-rpc"
            webRequest1.Method = "POST"
            Dim joe As New JObject()
            joe.Add(New JProperty("jsonrpc", "1.0"))
            joe.Add(New JProperty("id", "1"))
            joe.Add(New JProperty("method", method))
            Dim props As New JArray()
            'add appropriate number of params (param0 is parameter count)
            If param0 = 0 Then
                joe.Add(New JProperty("params", New JArray()))
            End If
            If param0 = 1 Then
                props.Add(param1)
                joe.Add(New JProperty("params", props))
            End If
            If param0 = 2 Then
                props.Add(param1)
                props.Add(param2)
                joe.Add(New JProperty("params", props))
            End If
            If param0 = 3 Then
                props.Add(param1)
                props.Add(param2)
                props.Add(param3)
                joe.Add(New JProperty("params", props))
            End If
            '// serialize json for the request

            Dim s As String = JsonConvert.SerializeObject(joe)
            Dim bytearray As Byte() = Encoding.UTF8.GetBytes(s)
            webRequest1.ContentLength = bytearray.Length
            Dim datastream As Stream = webRequest1.GetRequestStream()
            datastream.Write(bytearray, 0, bytearray.Length)
            datastream.Close()

            Dim webResponse1 As WebResponse = webRequest1.GetResponse()
            Dim returnstream As Stream = webResponse1.GetResponseStream()
            Dim rreader As StreamReader = New StreamReader(returnstream, Encoding.UTF8)
            Dim responsestring As String = rreader.ReadToEnd()
            Return (responsestring)
        Catch e As Exception
            'exception thrown 
            MsgBox("Exception thrown: " & e.Message.ToString)
        End Try
    End Function

    Public Function getmastercointransaction(ByVal bitcoin_con As bitcoinrpcconnection, ByVal txid As String, ByVal txtype As String)
        Dim tx As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, txid, 1, 0))
        'was it a purchase to consider as generate?
        If txtype = "generate" Then
            Dim txinputs As Integer
            Dim txhighvalue As Integer
            Dim txinputadd(1000) As String
            Dim txinputamount(1000) As Double
            Dim exoamount As Double
            txinputs = 0
            'calculate input addresses 
            Dim vins() As Vin = tx.result.vin.ToArray
            'get inputs
            For i = 0 To UBound(vins)
                'loop through each vin getting txid
                Dim vinresults As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, vins(i).txid.ToString, 1, 0))
                Dim voutnum As Integer = vins(i).vout
                'loop through vinresults until voutnum is located and grab address
                Dim voutarray() As Vout = vinresults.result.vout.ToArray
                For k = 0 To UBound(voutarray)
                    If voutarray(k).n = voutnum Then
                        'check we haven't seen this input address before
                        If txinputadd.Contains(voutarray(k).scriptPubKey.addresses(0).ToString) Then
                            'get location of address and increase amount
                            For p = 0 To txinputs
                                If txinputadd(p) = voutarray(k).scriptPubKey.addresses(0).ToString Then
                                    txinputamount(p) = txinputamount(p) + voutarray(k).value
                                    If txinputamount(p) > txinputamount(txhighvalue) Then txhighvalue = p
                                End If
                            Next
                        Else
                            txinputs = txinputs + 1
                            txinputamount(txinputs) = voutarray(k).value
                            If txinputamount(txinputs) > txinputamount(txhighvalue) Then txhighvalue = txinputs
                            txinputadd(txinputs) = voutarray(k).scriptPubKey.addresses(0).ToString
                        End If
                    End If
                Next
            Next
            If txinputs > 0 Then
                'calculate MSC purchased
                Dim vouts() As Vout = tx.result.vout.ToArray
                'loop through each output and get the value and address
                For i = 0 To UBound(vouts)
                    Try
                        If vouts(i).scriptPubKey.addresses(0).ToString = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                            exoamount = vouts(i).value
                        End If
                    Catch e As Exception
                        MsgBox("Exeption thrown : " & e.Message)
                    End Try
                Next

                Dim dif As Long = 1377993600 - tx.result.blocktime
                Dim bonus As Double = 0.1 * (dif / 604800)
                If bonus < 0 Then bonus = 0 'avoid negative bonus
                Dim totalmscexo As Double = exoamount * 100 * (1 + bonus) * 100000000
                Dim returnobj As New mastercointx
                returnobj.blocktime = tx.result.blocktime
                returnobj.fromadd = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P"
                returnobj.toadd = txinputadd(txhighvalue)
                returnobj.txid = tx.result.txid
                returnobj.type = "generate"
                returnobj.value = totalmscexo
                returnobj.valid = 1
                returnobj.curtype = 0
                Return returnobj
            End If
        End If

        'see if mastercoin transaction and if so decode
        If txtype = "send" Then
            Dim txinputs As Integer
            Dim txhighvalue As Integer
            Dim txinputadd(1000) As String
            Dim txinputamount(1000) As Double
            Dim exoamount As Double
            Dim pubkeyhex As String
            txinputs = 0
            'calculate input addresses 
            Dim vins() As Vin = tx.result.vin.ToArray
            'get inputs
            For i = 0 To UBound(vins)
                'loop through each vin getting txid
                Dim vinresults As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, vins(i).txid.ToString, 1, 0))
                Dim voutnum As Integer = vins(i).vout
                'loop through vinresults until voutnum is located and grab address
                Dim voutarray() As Vout = vinresults.result.vout.ToArray
                For k = 0 To UBound(voutarray)
                    If voutarray(k).n = voutnum Then
                        'check we haven't seen this input address before
                        If txinputadd.Contains(voutarray(k).scriptPubKey.addresses(0).ToString) Then
                            'get location of address and increase amount
                            For p = 0 To txinputs
                                If txinputadd(p) = voutarray(k).scriptPubKey.addresses(0).ToString Then
                                    txinputamount(p) = txinputamount(p) + voutarray(k).value
                                    If txinputamount(p) > txinputamount(txhighvalue) Then txhighvalue = p
                                End If
                            Next
                        Else
                            txinputs = txinputs + 1
                            txinputamount(txinputs) = voutarray(k).value
                            If txinputamount(txinputs) > txinputamount(txhighvalue) Then txhighvalue = txinputs
                            txinputadd(txinputs) = voutarray(k).scriptPubKey.addresses(0).ToString
                        End If
                    End If
                Next
            Next
            If txinputs > 0 Then
                multisig = False
                Dim vouts() As Vout = tx.result.vout.ToArray
                'loop through each output and find the exodus address
                For i = 0 To UBound(vouts)
                    Try
                        If vouts(i).scriptPubKey.addresses(0).ToString = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                            exoamount = vouts(i).value 'amount of msc fee
                        End If
                    Catch e As Exception
                        MsgBox("Exception thrown : " & e.Message)
                    End Try
                Next
                'loop through and find the remainder of output addresses
                Dim outputs As New DataTable
                outputs.Columns.Add("Address", GetType(String))
                outputs.Columns.Add("Amount", GetType(Long))
                outputs.Columns.Add("Seqnum", GetType(Integer))

                For i = 0 To UBound(vouts)
                    Try
                        If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                            multisig = True
                            Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                            pubkeyhex = asmvars(2)
                        End If
                        If vouts(i).scriptPubKey.type = "pubkeyhash" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then 'found data or destination address with matching transaction value
                            'get address sequence no
                            Dim rowbarray As Byte()
                            rowbarray = ToByteArray(Trim(vouts(i).scriptPubKey.addresses(0).ToString))
                            'add to table
                            outputs.Rows.Add(Trim(vouts(i).scriptPubKey.addresses(0).ToString), vouts(i).value, rowbarray(1))

                        End If
                    Catch e As Exception
                        MsgBox("Exception thrown : " & e.Message)
                    End Try
                Next
                'order the packets
                outputs.DefaultView.Sort = "Seqnum"
                Dim isvalidtx As Boolean
                Dim output(3) As String
                Dim outputseq(3) As Integer
                Dim txdataaddress As String
                Dim txrefaddress As String
                Dim txchangeaddress As String
                isvalidtx = False

                Dim outid As Integer = 1
                For Each row In outputs.DefaultView
                    outputseq(outid) = row("Seqnum")
                    output(outid) = row("Address")
                    outid = outid + 1
                Next

                If multisig = True Then
                    '/// multisig
                    '1 remaining output
                    If outputs.Rows.Count = 1 Then
                        txrefaddress = outputs.Rows(0).Item(0)
                        isvalidtx = True
                    End If
                    '2 remaining outputs
                    If outputs.Rows.Count = 2 Then
                        If output(1) = txinputadd(txhighvalue) Then
                            txchangeaddress = output(1)
                            txrefaddress = output(2)
                            isvalidtx = True
                        End If
                        If output(2) = txinputadd(txhighvalue) Then
                            txchangeaddress = output(2)
                            txrefaddress = output(1)
                            isvalidtx = True
                        End If
                        'otherwise ambiguous
                    End If
                Else
                    '/// non-multisig
                    '2 remaining outputs
                    If outputs.Rows.Count = 2 Then
                        If outputseq(2) - outputseq(1) = 1 Then
                            txdataaddress = output(1)
                            txrefaddress = output(2)
                            isvalidtx = True
                        End If
                        'handle 255
                        If (outputseq(2) = 255 And outputseq(1) = 0) Then
                            txdataaddress = output(2)
                            txrefaddress = output(1)
                            isvalidtx = True
                        End If
                    End If
                    '3 remaining outputs
                    If outputs.Rows.Count = 3 Then
                        If outputseq(2) - outputseq(1) = 1 And outputseq(3) - outputseq(2) <> 1 Then
                            txdataaddress = output(1)
                            txrefaddress = output(2)
                            txchangeaddress = output(3)
                            isvalidtx = True
                        End If
                        If outputseq(3) - outputseq(2) = 1 And outputseq(2) - outputseq(1) <> 1 Then
                            txdataaddress = output(2)
                            txrefaddress = output(3)
                            txchangeaddress = output(1)
                            isvalidtx = True
                        End If
                        'handle 255
                        If (outputseq(3) = 255 And outputseq(1) = 0) And (outputseq(2) - outputseq(1) <> 1 And outputseq(3) - outputseq(2) <> 1) Then
                            txdataaddress = output(3)
                            txrefaddress = output(1)
                            txchangeaddress = output(2)
                            isvalidtx = True
                        End If
                    End If
                End If

                'is tx valid? 
                If isvalidtx = True Then

                    'decode transaction
                    Dim barray As Byte()
                    'multisig?
                    If multisig = True Then
                        barray = multisigbarray(pubkeyhex)
                    Else 'not multisig
                        barray = ToByteArray(Trim(txdataaddress))
                    End If
                    Dim transbytes() As Byte = {barray(2), barray(3), barray(4), barray(5)}
                    Dim curidbytes() As Byte = {barray(6), barray(7), barray(8), barray(9)}
                    Dim amountbytes() As Byte = {barray(10), barray(11), barray(12), barray(13), barray(14), barray(15), barray(16), barray(17)}
                    'handle endianness
                    If BitConverter.IsLittleEndian = True Then
                        Array.Reverse(transbytes)
                        Array.Reverse(curidbytes)
                        Array.Reverse(amountbytes)
                    End If

                    If BitConverter.ToUInt32(transbytes, 0) = 0 Then
                        Dim returnobj As New mastercointx
                        returnobj.blocktime = tx.result.blocktime
                        returnobj.fromadd = txinputadd(txhighvalue)
                        returnobj.toadd = txrefaddress
                        returnobj.txid = tx.result.txid
                        returnobj.type = "simple"
                        returnobj.curtype = BitConverter.ToUInt32(curidbytes, 0)
                        returnobj.valid = 0
                        returnobj.value = BitConverter.ToUInt64(amountbytes, 0)
                        Return returnobj
                    End If
                End If
            End If
        End If
    End Function
    Public Function encodetx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal toadd As String, ByVal curtype As Integer, ByVal amount As Long)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 50000 'include 0.0002 miner fee
        Dim encodedpubkey, frompubkey As String

        'calculate encoded public key for tx
        encodedpubkey = "0201" 'compressedkey+seqnum
        encodedpubkey = encodedpubkey + "00000000" 'simple send
        encodedpubkey = encodedpubkey + i32tohexlittle(curtype)
        encodedpubkey = encodedpubkey + i64tohexlittle(amount)
        encodedpubkey = encodedpubkey + "000000000000000000000000000000" 'padding

        'get public key for from address
        Try
            Dim validate As validate = JsonConvert.DeserializeObject(Of validate)(rpccall(bitcoin_con, "validateaddress", 1, fromadd, 0, 0))
            frompubkey = validate.result.pubkey
            If validate.result.iscompressed = False Then
                'compress public key
                frompubkey = frompubkey.Substring(2, 128)
                If Val(Right(frompubkey, 1)) Mod 2 Then
                    frompubkey = "03" & Left(frompubkey, 64)
                Else
                    frompubkey = "02" & Left(frompubkey, 64)
                End If
            End If
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
        If frompubkey = "" Then
            MsgBox("Error locating public key for from address.")
            Exit Function
        End If

        'lookup unspent for from address
        Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
        Dim inputs() As result_unspent = listunspent.result.ToArray
        For i = 0 To UBound(inputs)
            If (inputs(i).amount * 100000000) > totaltxfee And inputs(i).address = fromadd Then
                fromtxid = inputs(i).txid
                fromtxvout = inputs(i).vout
                fromtxamount = inputs(i).amount
            End If
        Next
        If fromtxid = "" Or fromtxvout < 0 Or fromtxamount < 0 Then
            MsgBox("Insufficient funds for fee at from address.")
            Exit Function
        End If

        'handle change
        changeamount = (fromtxamount * 100000000) - totaltxfee

        'build tx hex raw
        txhex = "01000000" 'version
        txhex = txhex & "01" 'vin count
        txhex = txhex & txidtohex(fromtxid) 'input txid hex
        txhex = txhex & i32tohex(fromtxvout) 'input vout 00000000
        txhex = txhex & "00" 'scriptsig length
        txhex = txhex & "ffffffff" 'sequence

        txhex = txhex & "04" 'number of vouts, future: cater for 3 outs (no change) - since we check txin for >totaltxfee there will always be change for now

        'change output
        txhex = txhex & i64tohex(changeamount) 'changeamount value
        txhex = txhex & "19" 'length - 25 bytes
        txhex = txhex & "76a914" & addresstopubkey(fromadd) & "88ac" 'change scriptpubkey
        'exodus output
        txhex = txhex & i64tohex(txfee)
        txhex = txhex & "19"
        txhex = txhex & "76a914946cb2e08075bcbaf157e47bcb67eb2b2339d24288ac" 'exodus scriptpubkey
        'reference/destination output
        txhex = txhex & i64tohex(txfee)
        txhex = txhex & "19"
        txhex = txhex & "76a914" & addresstopubkey(toadd) & "88ac" 'data scriptpubkey 

        'multisig output
        txhex = txhex & i64tohex(txfee * 2)
        txhex = txhex & "47" 'length - ??bytes?? calculate
        txhex = txhex & "51" '???
        txhex = txhex & "21" '???
        txhex = txhex & frompubkey 'first multisig address
        txhex = txhex & "21" '???
        txhex = txhex & encodedpubkey 'second multisig address
        txhex = txhex & "52ae" '???
        txhex = txhex & "00000000" 'locktime

        Return txhex
    End Function

    Public Function getaddresses(ByVal bitcoin_con As bitcoinrpcconnection)
        Dim addresslist As New List(Of btcaddressbal)
        Dim plainaddresslist As New List(Of String)

        'first use listreceivedbyaddress 0 true to get 'all addresses'
        Dim addresses As rcvaddress = JsonConvert.DeserializeObject(Of rcvaddress)(rpccall(bitcoin_con, "listreceivedbyaddress", 2, 0, True, 0))
        For Each result In addresses.result
            plainaddresslist.Add(result.address.ToString)
        Next
        'since documentation is wrong and this does not list all addresses (eg change), use listunspent to gather up any addresses not returned by listreceivedbyaddress
        Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 0, 999999, 0))
        For Each result In listunspent.result
            If Not plainaddresslist.Contains(result.address.ToString) Then 'avoid duplicates
                plainaddresslist.Add(result.address.ToString)
            End If
        Next
        'loop through plainaddresslist and get balances to create addresslist object
        For Each address In plainaddresslist
            Dim addressbal As Double = 0
            Dim uaddressbal As Double = 0
            For Each result In listunspent.result
                If result.address.ToString = address.ToString Then
                    If result.confirmations = 0 Then
                        uaddressbal = uaddressbal + result.amount
                    Else
                        addressbal = addressbal + result.amount
                    End If
                End If
            Next
            Dim addressobj As New btcaddressbal
            addressobj.address = address.ToString
            addressobj.amount = addressbal
            addressobj.uamount = uaddressbal
            addresslist.Add(addressobj)
        Next

        Return addresslist
    End Function
    Public Function getblock(ByVal bitcoin_con As bitcoinrpcconnection, ByVal blockhash As String)
        Try
            Dim block As Block = JsonConvert.DeserializeObject(Of Block)(rpccall(bitcoin_con, "getblock", 1, blockhash, 1, 0))
            Return block
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
    End Function
    Public Function getblocktemplate(ByVal bitcoin_con As bitcoinrpcconnection)
        Try
            Dim blocktemplate As blocktemplate = JsonConvert.DeserializeObject(Of blocktemplate)(rpccall(bitcoin_con, "getblocktemplate", 0, 0, 0, 0))
            Return blocktemplate
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
    End Function
    Public Function getblockcount(ByVal bitcoin_con As bitcoinrpcconnection)
        Dim blockcount As blockcount = JsonConvert.DeserializeObject(Of blockcount)(rpccall(bitcoin_con, "getblockcount", 0, 0, 1, 0))
        Return blockcount
    End Function
    Public Function getblockhash(ByVal bitcoin_con As bitcoinrpcconnection, ByVal block As Integer)
        Try
            Dim blockhash As blockhash = JsonConvert.DeserializeObject(Of blockhash)(rpccall(bitcoin_con, "getblockhash", 1, block, 1, 0))
            Return blockhash
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
    End Function
    Public Function gettransaction(ByVal bitcoin_con As bitcoinrpcconnection, ByVal txid As String)
        Try
            Dim tx As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, txid, 1, 0))
            Return tx
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
    End Function
    Public Function ismastercointx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal txid As String)
        Try
            Dim tx As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, txid, 1, 0))
            Dim vouts() As Vout = tx.result.vout.ToArray
            For i = 0 To UBound(vouts)
                If Not IsNothing(vouts(i).scriptPubKey.addresses) Then
                    If vouts(i).scriptPubKey.addresses(0).ToString = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                        Return True
                        Exit Function
                    End If
                End If
            Next
            Return False
        Catch e As Exception
            MsgBox("Exeption thrown : " & e.Message)
        End Try
    End Function
    Public Function addresstopubkey(address As String)
        Dim hex() As Byte = ToByteArray(address)
        If Not ((hex Is Nothing) OrElse Not (hex.Length <> 21)) Then
            Dim pubkey As String = bytearraytostring(hex, 1, 20)
            Return pubkey
        End If
    End Function
    Public Function txidtohex(txid As String)
        Dim hex() As Byte = StringToByteArray(txid)
        Array.Reverse(hex)
        If ((hex IsNot Nothing) And (hex.Length = 32)) Then
            Dim txidhex As String = bytearraytostring(hex, 0, 32)
            Return txidhex
        End If
    End Function
    Public Function bytearraytostring(ByVal ba() As Byte, ByVal offset As Integer, ByVal count As Integer)
        Dim rv As String = ""
        Dim usedcount As Integer = 0
        Dim i As Integer = offset
        Do While (usedcount < count)
            rv = (rv + (String.Format("{0:X2}", ba(i))))
            i = i + 1
            usedcount = usedcount + 1
        Loop
        rv = LCase(rv)
        Return rv
    End Function
    Public Function i64tohex(ByVal amount As Long)
        Dim amountbytes() As Byte = BitConverter.GetBytes(amount)
        Dim amounthex As String = bytearraytostring(amountbytes, 0, 8)
        Return amounthex
    End Function
    Public Function i32tohex(ByVal amount As Integer)
        Dim amountbytes() As Byte = BitConverter.GetBytes(amount)
        Dim amounthex As String = bytearraytostring(amountbytes, 0, 4)
        Return amounthex
    End Function
    Public Function i64tohexlittle(ByVal amount As Long)
        Dim amountbytes() As Byte = BitConverter.GetBytes(amount)
        Array.Reverse(amountbytes)
        Dim amounthex As String = bytearraytostring(amountbytes, 0, 8)
        Return amounthex
    End Function
    Public Function i32tohexlittle(ByVal amount As Integer)
        Dim amountbytes() As Byte = BitConverter.GetBytes(amount)
        Array.Reverse(amountbytes)
        Dim amounthex As String = bytearraytostring(amountbytes, 0, 4)
        Return amounthex
    End Function
    Public Function StringToByteArray(hex As [String]) As Byte()
        Dim NumberChars As Integer = hex.Length
        Dim bytes As Byte() = New Byte(NumberChars \ 2 - 1) {}
        For i As Integer = 0 To NumberChars - 1 Step 2
            bytes(i \ 2) = Convert.ToByte(hex.Substring(i, 2), 16)
        Next
        Return bytes
    End Function

End Class
