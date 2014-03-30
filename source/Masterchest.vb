Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Text
Imports System.IO
Imports Org.BouncyCastle.Math.EC
Imports System.Security.Cryptography

Public Class mlib
    '/////////////
    '///STRUCTURES
    '/////////////
    Public publickeybytes As Byte()
    Public multisig As Boolean
    Dim isvalidtx As Boolean
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
    Public Class mastercointx_selloffer
        Public txid As String
        Public fromadd As String
        Public type As String
        Public blocktime As Long
        Public blocknum As Integer
        Public valid As Integer
        Public curtype As Integer
        Public minfee As Long
        Public timelimit As Integer
        Public saleamount As Long
        Public offeramount As Long
        Public version As Integer
        Public action As Integer
    End Class
    Public Class mastercointx_spfixed
        Public txid As String
        Public fromadd As String
        Public type As String
        Public blocktime As Long
        Public blocknum As Integer
        Public valid As Integer
        Public ecosystem As Integer
        Public propertytype As Integer
        Public previousid As Integer
        Public category As String
        Public subcategory As String
        Public name As String
        Public url As String
        Public data As String
        Public numberproperties As Long
    End Class
    Public Class mastercointx_spvar
        Public txid As String
        Public fromadd As String
        Public type As String
        Public blocktime As Long
        Public blocknum As Integer
        Public valid As Integer
        Public ecosystem As Integer
        Public propertytype As Integer
        Public previousid As Integer
        Public category As String
        Public subcategory As String
        Public name As String
        Public url As String
        Public data As String
        Public currencydesired As Integer
        Public numberpropertiesperunit As Long
        Public deadline As Long
        Public earlybonus As Integer
        Public percentforissuer As Integer
    End Class
    Public Class mastercointx_acceptoffer
        Public txid As String
        Public toadd As String
        Public fromadd As String
        Public purchaseamount As Long
        Public type As String
        Public blocknum As Integer
        Public blocktime As Long
        Public valid As Integer
        Public curtype As Integer
        Public fee As Long
    End Class
    Public Class mastercointx
        Public txid As String
        Public toadd As String
        Public fromadd As String
        Public value As Long
        Public type As String
        Public blocktime As Long
        Public blocknum As Integer
        Public valid As Integer
        Public curtype As Integer
    End Class
    Public Class mastercointx_btcpayment
        Public txid As String
        Public fromadd As String
        Public vouts As String
        Public type As String
        Public blocktime As Long
        Public blocknum As Integer
        Public valid As Integer
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
    Public cleartextpacket As String

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

    Public Function multisigbarray(ByVal key As String)
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
        Dim retrycount As Integer = 0
        While retrycount < 3
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
                'retry failed connection a few times
                retrycount = retrycount + 1
                If retrycount > 2 Then
                    'exception thrown 
                    MsgBox("Exception thrown in bitcoin rpc call: " & e.Message.ToString)
                End If
            End Try
        End While
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                        MsgBox("Exeption thrown looping outputs : " & e.Message)
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

        'bitcoin payment
        If txtype = "btcpayment" Then
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                'String vouts together
                Dim voutstring As String = ""
                For Each Vout In tx.result.vout
                    For Each address As String In Vout.scriptPubKey.addresses
                        If address <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" And Vout.scriptPubKey.type.ToString.ToLower = "pubkeyhash" Then voutstring = voutstring & "-" & address
                    Next
                Next

                Dim returnobj As New mastercointx_btcpayment
                returnobj.vouts = voutstring
                returnobj.blocktime = tx.result.blocktime
                returnobj.fromadd = txinputadd(txhighvalue)
                returnobj.txid = tx.result.txid
                returnobj.type = "btcpayment"
                returnobj.valid = 0
                Return (returnobj)
            End If
        End If

        'sell offer
        If txtype = "selloffer" Then
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                Dim vouts() As Vout = tx.result.vout.ToArray
                If UBound(vouts) > 0 Then 'we have outputs to work if
                    'loop through and get the data pubkeys
                    Dim outputs As New DataTable
                    outputs.Columns.Add("pubkeys", GetType(String))

                    For i = 0 To UBound(vouts)
                        Try
                            If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                                If asmvars.Count = 5 Then
                                    outputs.Rows.Add(asmvars(2))
                                End If
                                If asmvars.Count = 6 Then
                                    outputs.Rows.Add(asmvars(2))
                                    outputs.Rows.Add(asmvars(3))
                                End If
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown enumerating outputs: " & Trim(vouts(i).scriptPubKey.addresses(0).ToString) & " : " & e.Message)
                        End Try
                    Next
                    isvalidtx = False
                    If outputs.Rows.Count > 0 And outputs.Rows.Count < 3 Then 'we have data to work with
                        '/// multisig
                        '1 remaining output
                        If outputs.Rows.Count = 1 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            isvalidtx = True
                        End If
                        '2 remaining outputs
                        If outputs.Rows.Count = 2 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            'MsgBox(cleartextpacket)
                            isvalidtx = True
                            'otherwise ambiguous
                        End If
                    Else
                        If isvalidtx = False Then  '### fall back to peek and decode
                            'peek and decode routine here
                        End If
                    End If

                    'is tx valid? 
                    If isvalidtx = True Then
                        'decode transaction
                        Dim barray As Byte()
                        barray = multisigbarray(cleartextpacket)
                        Dim versbytes() As Byte = {barray(1), barray(2)}
                        Dim transbytes() As Byte = {barray(3), barray(4)}
                        Dim curidbytes() As Byte = {barray(5), barray(6), barray(7), barray(8)}
                        Dim saleamountbytes() As Byte = {barray(9), barray(10), barray(11), barray(12), barray(13), barray(14), barray(15), barray(16)}
                        Dim offeramountbytes() As Byte = {barray(17), barray(18), barray(19), barray(20), barray(21), barray(22), barray(23), barray(24)}
                        Dim timelimitbyte As Byte = barray(25)
                        Dim minfeebytes() As Byte = {barray(26), barray(27), barray(28), barray(29), barray(30), barray(31), barray(32), barray(33)}
                        Dim actionbyte As Byte = barray(34)

                        'handle endianness
                        If BitConverter.IsLittleEndian = True Then
                            Array.Reverse(versbytes)
                            Array.Reverse(transbytes)
                            Array.Reverse(curidbytes)
                            Array.Reverse(saleamountbytes)
                            Array.Reverse(offeramountbytes)
                            Array.Reverse(minfeebytes)
                        End If

                        If BitConverter.ToUInt16(transbytes, 0) = 20 Then
                            Dim returnobj As New mastercointx_selloffer
                            returnobj.version = BitConverter.ToUInt16(versbytes, 0)
                            returnobj.action = actionbyte
                            returnobj.blocktime = tx.result.blocktime
                            returnobj.fromadd = txinputadd(txhighvalue)
                            returnobj.txid = tx.result.txid
                            returnobj.type = "selloffer"
                            returnobj.curtype = BitConverter.ToUInt32(curidbytes, 0)
                            returnobj.valid = 0
                            returnobj.saleamount = BitConverter.ToUInt64(saleamountbytes, 0)
                            returnobj.offeramount = BitConverter.ToUInt64(offeramountbytes, 0)
                            returnobj.timelimit = timelimitbyte
                            returnobj.minfee = BitConverter.ToUInt64(minfeebytes, 0)
                            Return (returnobj)
                        End If
                    End If
                End If
            End If
        End If

        'accept offer
        If txtype = "acceptoffer" Then
            Dim txinputs As Integer
            Dim txhighvalue As Integer
            Dim txinputadd(1000) As String
            Dim txinputamount(1000) As Double
            Dim toadd As String
            Dim exoamount As Double
            Dim pubkeyhex As String
            Dim inputsum As Double = 0
            Dim outputsum As Double = 0
            Dim fee As Long = 0
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
                        inputsum = inputsum + voutarray(k).value
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                Dim vouts() As Vout = tx.result.vout.ToArray
                If UBound(vouts) > 0 Then 'we have outputs to work if
                    'loop through and get the data pubkeys
                    Dim outputs As New DataTable
                    Dim pubkeys As New DataTable
                    outputs.Columns.Add("address", GetType(String))
                    pubkeys.Columns.Add("pubkey", GetType(String))
                    For i = 0 To UBound(vouts)
                        Try
                            outputsum = outputsum + vouts(i).value
                            If vouts(i).scriptPubKey.type = "pubkeyhash" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                outputs.Rows.Add(Trim(vouts(i).scriptPubKey.addresses(0).ToString))
                            End If
                            If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                                If asmvars.Count = 5 Then
                                    pubkeys.Rows.Add(asmvars(2))
                                End If
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown enumerating outputs: " & Trim(vouts(i).scriptPubKey.addresses(0).ToString) & " : " & e.Message)
                        End Try
                    Next
                    isvalidtx = False
                    Dim refaddress As String = ""
                    Dim outcount As Integer = 0
                    Dim changeout As Integer = -1
                    'strip the last sender output as change (if more than one output sending to self)
                    For i = 0 To outputs.Rows.Count - 1
                        If outputs.Rows(i).Item(0) = txinputadd(txhighvalue).ToString Then changeout = i
                    Next
                    If changeout > -1 Then outputs.Rows(changeout).Delete()
                    'make sure if that if more than one row here the address is the same (reference)
                    For i = 0 To outputs.Rows.Count - 1
                        If refaddress <> outputs.Rows(i).Item(0) Then
                            refaddress = outputs.Rows(i).Item(0)
                            outcount = outcount + 1
                        End If
                    Next
                    If outcount = 1 Then 'there should only be one pubkeyhash output after we've removed exodus and change (sender)
                        If pubkeys.Rows.Count > 0 And pubkeys.Rows.Count < 2 Then 'we have data to work with
                            '/// multisig
                            '1 remaining output
                            If pubkeys.Rows.Count = 1 Then
                                cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, pubkeys.Rows(0).Item(0).ToString.Substring(2, 62))
                                isvalidtx = True
                            End If
                        End If

                        'is tx valid? 
                        If isvalidtx = True Then
                            'decode transaction
                            Dim barray As Byte()
                            barray = multisigbarray(cleartextpacket)
                            Dim transbytes() As Byte = {barray(1), barray(2), barray(3), barray(4)}
                            Dim curidbytes() As Byte = {barray(5), barray(6), barray(7), barray(8)}
                            Dim purchaseamountbytes() As Byte = {barray(9), barray(10), barray(11), barray(12), barray(13), barray(14), barray(15), barray(16)}

                            'handle endianness
                            If BitConverter.IsLittleEndian = True Then
                                Array.Reverse(transbytes)
                                Array.Reverse(curidbytes)
                                Array.Reverse(purchaseamountbytes)
                            End If

                            If BitConverter.ToUInt32(transbytes, 0) = 22 Then
                                Dim returnobj As New mastercointx_acceptoffer

                                returnobj.blocktime = tx.result.blocktime
                                returnobj.fromadd = txinputadd(txhighvalue)
                                returnobj.toadd = refaddress
                                returnobj.txid = tx.result.txid
                                returnobj.type = "acceptoffer"
                                returnobj.curtype = BitConverter.ToUInt32(curidbytes, 0)
                                returnobj.valid = 0
                                fee = (Math.Round((inputsum - outputsum), 8) * 100000000)
                                returnobj.fee = fee
                                returnobj.purchaseamount = BitConverter.ToUInt64(purchaseamountbytes, 0)
                                Return (returnobj)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        'SP var create
        If txtype = "spcreatevar" Then
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                Dim vouts() As Vout = tx.result.vout.ToArray
                If UBound(vouts) > 0 Then 'we have outputs to work if
                    'loop through and get the data pubkeys
                    Dim outputs As New DataTable
                    outputs.Columns.Add("pubkeys", GetType(String))

                    For i = 0 To UBound(vouts)
                        Try
                            If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                                If asmvars.Count = 5 Then
                                    outputs.Rows.Add(asmvars(2))
                                End If
                                If asmvars.Count = 6 Then
                                    outputs.Rows.Add(asmvars(2))
                                    outputs.Rows.Add(asmvars(3))
                                End If
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown enumerating outputs: " & Trim(vouts(i).scriptPubKey.addresses(0).ToString) & " : " & e.Message)
                        End Try
                    Next
                    isvalidtx = False
                    If outputs.Rows.Count > 0 And outputs.Rows.Count < 5 Then 'we have data to work with
                        '/// multisig
                        'compile cleartext message
                        If outputs.Rows.Count = 1 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 2 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 3 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 3, outputs.Rows(2).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 4 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 3, outputs.Rows(2).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 4, outputs.Rows(3).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                    End If

                    'is tx valid? 
                    If isvalidtx = True Then
                        'decode transaction
                        Dim barray As Byte()
                        InputBox("ff", , cleartextpacket)
                        barray = multisigbarray(cleartextpacket)
                        'get string locations
                        Dim null1 As Integer = cleartextpacket.IndexOf("00", 24) / 2
                        Dim null2 As Integer = cleartextpacket.IndexOf("00", (null1 * 2) + 2) / 2
                        Dim null3 As Integer = cleartextpacket.IndexOf("00", (null2 * 2) + 2) / 2
                        Dim null4 As Integer = cleartextpacket.IndexOf("00", (null3 * 2) + 2) / 2
                        Dim null5 As Integer = cleartextpacket.IndexOf("00", (null4 * 2) + 2) / 2
                        MsgBox(null4 & " " & null5)
                        Dim versbytes() As Byte = {barray(1), barray(2)}
                        Dim transbytes() As Byte = {barray(3), barray(4)}
                        Dim ecobyte As Byte = barray(5)
                        Dim proptypebytes() As Byte = {barray(6), barray(7)}
                        Dim prevpropbytes() As Byte = {barray(8), barray(9), barray(10), barray(11)}
                        Dim propcatbuffer As New List(Of Byte)
                        For i = 12 To null1 - 1
                            propcatbuffer.Add(barray(i))
                        Next
                        Dim propcatbytes() As Byte = propcatbuffer.ToArray
                        Dim propsubcatbuffer As New List(Of Byte)
                        For i = null1 + 1 To null2 - 1
                            propsubcatbuffer.Add(barray(i))
                        Next
                        Dim propsubcatbytes() As Byte = propsubcatbuffer.ToArray
                        Dim propnamebuffer As New List(Of Byte)
                        For i = null2 + 1 To null3 - 1
                            propnamebuffer.Add(barray(i))
                        Next
                        Dim propnamebytes() As Byte = propnamebuffer.ToArray
                        Dim propurlbuffer As New List(Of Byte)
                        For i = null3 + 1 To null4 - 1
                            propurlbuffer.Add(barray(i))
                        Next
                        Dim propurlbytes() As Byte = propurlbuffer.ToArray
                        Dim propdatabuffer As New List(Of Byte)
                        For i = null4 + 1 To null5 - 1
                            propdatabuffer.Add(barray(i))
                        Next
                        Dim propdatabytes() As Byte = propdatabuffer.ToArray
                        Dim curdesbytes() As Byte = {barray(null5 + 1), barray(null5 + 2), barray(null5 + 3), barray(null5 + 4)}
                        Dim numpropsbytes() As Byte = {barray(null5 + 5), barray(null5 + 6), barray(null5 + 7), barray(null5 + 8), barray(null5 + 9), barray(null5 + 10), barray(null5 + 11), barray(null5 + 12)}
                        Dim deadlinebytes() As Byte = {barray(null5 + 13), barray(null5 + 14), barray(null5 + 15), barray(null5 + 16), barray(null5 + 17), barray(null5 + 18), barray(null5 + 19), barray(null5 + 20)}
                        Dim earlybird As Byte = barray(null5 + 21)
                        Dim issuerpc As Byte = barray(null5 + 22)
                        'handle endianness
                        If BitConverter.IsLittleEndian = True Then
                            Array.Reverse(versbytes)
                            Array.Reverse(transbytes)
                            Array.Reverse(proptypebytes)
                            Array.Reverse(prevpropbytes)
                            Array.Reverse(numpropsbytes)
                            Array.Reverse(deadlinebytes)
                        End If
                        If BitConverter.ToUInt16(transbytes, 0) = 51 Then
                            Dim returnobj As New mastercointx_spvar
                            returnobj.ecosystem = ecobyte
                            returnobj.propertytype = BitConverter.ToUInt16(proptypebytes, 0)
                            returnobj.previousid = BitConverter.ToUInt32(prevpropbytes, 0)
                            returnobj.category = Encoding.UTF8.GetString(propcatbytes)
                            returnobj.subcategory = Encoding.UTF8.GetString(propsubcatbytes)
                            returnobj.name = Encoding.UTF8.GetString(propnamebytes)
                            returnobj.url = Encoding.UTF8.GetString(propurlbytes)
                            returnobj.data = Encoding.UTF8.GetString(propdatabytes)
                            returnobj.currencydesired = BitConverter.ToUInt32(curdesbytes, 0)
                            returnobj.numberpropertiesperunit = BitConverter.ToUInt64(numpropsbytes, 0)
                            returnobj.deadline = BitConverter.ToUInt64(deadlinebytes, 0)
                            returnobj.earlybonus = earlybird
                            returnobj.percentforissuer = issuerpc
                            returnobj.blocktime = tx.result.blocktime
                            returnobj.fromadd = txinputadd(txhighvalue)
                            returnobj.txid = tx.result.txid
                            returnobj.type = "spcreatevar"
                            Return (returnobj)
                        End If
                    End If
                End If
            End If
        End If

        'SP fixed create
        If txtype = "spcreatefixed" Then
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                Dim vouts() As Vout = tx.result.vout.ToArray
                If UBound(vouts) > 0 Then 'we have outputs to work if
                    'loop through and get the data pubkeys
                    Dim outputs As New DataTable
                    outputs.Columns.Add("pubkeys", GetType(String))

                    For i = 0 To UBound(vouts)
                        Try
                            If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                                If asmvars.Count = 5 Then
                                    outputs.Rows.Add(asmvars(2))
                                End If
                                If asmvars.Count = 6 Then
                                    outputs.Rows.Add(asmvars(2))
                                    outputs.Rows.Add(asmvars(3))
                                End If
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown enumerating outputs: " & Trim(vouts(i).scriptPubKey.addresses(0).ToString) & " : " & e.Message)
                        End Try
                    Next
                    isvalidtx = False
                    If outputs.Rows.Count > 0 And outputs.Rows.Count < 5 Then 'we have data to work with
                        '/// multisig
                        'compile cleartext message
                        If outputs.Rows.Count = 1 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 2 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 3 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 3, outputs.Rows(2).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                        If outputs.Rows.Count = 4 Then
                            cleartextpacket = decryptmastercoinpacket(txinputadd(txhighvalue), 1, outputs.Rows(0).Item(0).ToString.Substring(2, 62))
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 2, outputs.Rows(1).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 3, outputs.Rows(2).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            cleartextpacket = cleartextpacket & decryptmastercoinpacket(txinputadd(txhighvalue), 4, outputs.Rows(3).Item(0).ToString.Substring(2, 62)).Substring(2, 60)
                            isvalidtx = True
                        End If
                    End If

                    'is tx valid? 
                    If isvalidtx = True Then
                        'decode transaction
                        Dim barray As Byte()
                        InputBox("ff", , cleartextpacket)
                        barray = multisigbarray(cleartextpacket)
                        'get string locations
                        Dim null1 As Integer = cleartextpacket.IndexOf("00", 24) / 2
                        Dim null2 As Integer = cleartextpacket.IndexOf("00", (null1 * 2) + 2) / 2
                        Dim null3 As Integer = cleartextpacket.IndexOf("00", (null2 * 2) + 2) / 2
                        Dim null4 As Integer = cleartextpacket.IndexOf("00", (null3 * 2) + 2) / 2
                        Dim null5 As Integer = cleartextpacket.IndexOf("00", (null4 * 2) + 2) / 2

                        Dim versbytes() As Byte = {barray(1), barray(2)}
                        Dim transbytes() As Byte = {barray(3), barray(4)}
                        Dim ecobyte As Byte = barray(5)
                        Dim proptypebytes() As Byte = {barray(6), barray(7)}
                        Dim prevpropbytes() As Byte = {barray(8), barray(9), barray(10), barray(11)}
                        Dim propcatbuffer As New List(Of Byte)
                        For i = 12 To null1 - 1
                            propcatbuffer.Add(barray(i))
                        Next
                        Dim propcatbytes() As Byte = propcatbuffer.ToArray
                        Dim propsubcatbuffer As New List(Of Byte)
                        For i = null1 + 1 To null2 - 1
                            propsubcatbuffer.Add(barray(i))
                        Next
                        Dim propsubcatbytes() As Byte = propsubcatbuffer.ToArray
                        Dim propnamebuffer As New List(Of Byte)
                        For i = null2 + 1 To null3 - 1
                            propnamebuffer.Add(barray(i))
                        Next
                        Dim propnamebytes() As Byte = propnamebuffer.ToArray
                        Dim propurlbuffer As New List(Of Byte)
                        For i = null3 + 1 To null4 - 1
                            propurlbuffer.Add(barray(i))
                        Next
                        Dim propurlbytes() As Byte = propurlbuffer.ToArray
                        Dim propdatabuffer As New List(Of Byte)
                        For i = null4 + 1 To null5 - 1
                            propdatabuffer.Add(barray(i))
                        Next
                        Dim propdatabytes() As Byte = propdatabuffer.ToArray
                        Dim numpropsbytes() As Byte = {barray(null5 + 1), barray(null5 + 2), barray(null5 + 3), barray(null5 + 4), barray(null5 + 5), barray(null5 + 6), barray(null5 + 7), barray(null5 + 8)}
                        'handle endianness
                        If BitConverter.IsLittleEndian = True Then
                            Array.Reverse(versbytes)
                            Array.Reverse(transbytes)
                            Array.Reverse(proptypebytes)
                            Array.Reverse(prevpropbytes)
                            Array.Reverse(numpropsbytes)
                        End If
                        If BitConverter.ToUInt16(transbytes, 0) = 50 Then
                            Dim returnobj As New mastercointx_spfixed
                            returnobj.ecosystem = ecobyte
                            returnobj.propertytype = BitConverter.ToUInt16(proptypebytes, 0)
                            returnobj.previousid = BitConverter.ToUInt32(prevpropbytes, 0)
                            returnobj.category = Encoding.UTF8.GetString(propcatbytes)
                            returnobj.subcategory = Encoding.UTF8.GetString(propsubcatbytes)
                            returnobj.name = Encoding.UTF8.GetString(propnamebytes)
                            returnobj.url = Encoding.UTF8.GetString(propurlbytes)
                            returnobj.data = Encoding.UTF8.GetString(propdatabytes)
                            returnobj.numberproperties = BitConverter.ToUInt64(numpropsbytes, 0)
                            returnobj.blocktime = tx.result.blocktime
                            returnobj.fromadd = txinputadd(txhighvalue)
                            returnobj.txid = tx.result.txid
                            returnobj.type = "spcreatefixed"
                            Return (returnobj)
                        End If
                    End If
                End If
            End If
        End If

        'simple send
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
                        'check vout is standard pubkeyhash, if not abort decoding 
                        If voutarray(k).scriptPubKey.type.ToString.ToLower <> "pubkeyhash" Then
                            Exit Function
                        End If
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
                If UBound(vouts) > 0 Then 'we have outputs to work if
                    'loop through each output and find the exodus address
                    For i = 0 To UBound(vouts)
                        Try
                            If vouts(i).scriptPubKey.addresses(0).ToString = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                exoamount = vouts(i).value 'amount of msc fee
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown detecting fee amounts: " & e.Message)
                        End Try
                    Next
                    'loop through and find the remainder of output addresses
                    Dim outputs As New DataTable
                    outputs.Columns.Add("Address", GetType(String))
                    outputs.Columns.Add("Amount", GetType(Double))
                    outputs.Columns.Add("Seqnum", GetType(Integer))

                    For i = 0 To UBound(vouts)
                        Try
                            If vouts(i).scriptPubKey.type = "multisig" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                multisig = True
                                Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                                pubkeyhex = asmvars(2)
                            End If
                            If vouts(i).scriptPubKey.type = "pubkeyhash" And vouts(i).scriptPubKey.addresses(0).ToString <> "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                                'get address sequence no
                                Dim rowbarray As Byte()
                                rowbarray = ToByteArray(Trim(vouts(i).scriptPubKey.addresses(0).ToString))
                                'add to table
                                outputs.Rows.Add(Trim(vouts(i).scriptPubKey.addresses(0).ToString), vouts(i).value, rowbarray(1))
                            End If
                        Catch e As Exception
                            MsgBox("Exception thrown enumerating outputs: " & Trim(vouts(i).scriptPubKey.addresses(0).ToString) & " : " & e.Message)
                        End Try
                    Next
                    'order the packets
                    outputs.DefaultView.Sort = "Seqnum"
                    Dim output(3) As String
                    Dim outputseq(3) As Integer
                    Dim changeaddress As String = ""
                    Dim dataaddress As String = ""
                    Dim refaddress As String = ""
                    isvalidtx = False
                    If outputs.Rows.Count > 0 And outputs.Rows.Count < 99 Then 'we have data to work with
                        If multisig = True Then
                            '/// multisig - ###FIX 'output() etc###

                            '##BUGFIX REQUIRED
                            '##BUGFIX REQUIRED
                            '##BUGFIX REQUIRED

                            '1 remaining output
                            If outputs.Rows.Count = 1 Then
                                refaddress = outputs.Rows(0).Item("address")
                                isvalidtx = True
                            End If
                            '2 remaining outputs
                            If outputs.Rows.Count = 2 Then
                                If outputs.Rows(0).Item("address") = txinputadd(txhighvalue) Then 'txinputadd(txhighvalue) is the from address
                                    changeaddress = outputs.Rows(0).Item("address")
                                    refaddress = outputs.Rows(1).Item("address")
                                    isvalidtx = True
                                End If
                                If outputs.Rows(1).Item("address") = txinputadd(txhighvalue) Then
                                    changeaddress = outputs.Rows(1).Item("address")
                                    refaddress = outputs.Rows(0).Item("address")
                                    isvalidtx = True
                                End If
                                'otherwise ambiguous
                            End If
                        Else
                            '/// non-multisig
                            'straight to p&d
                            'loop through outputs looking for data address
                            Dim dataaddressfound As Boolean = False

                            For i = 1 To outputs.Rows.Count
                                Dim rowbarray = ToByteArray(Trim(outputs.Rows(i - 1).Item("address")))
                                'test output for simple send bytes
                                If rowbarray(2) = 0 And rowbarray(3) = 0 And rowbarray(4) = 0 And rowbarray(5) = 0 And rowbarray(6) = 0 And rowbarray(7) = 0 And rowbarray(8) = 0 And (rowbarray(9) = 1 Or rowbarray(9) = 2) Then
                                    'check we have not already found the data address
                                    If dataaddressfound = True Then
                                        'more than one data address - drop tx
                                        Exit Function
                                    Else
                                        dataaddressfound = True
                                        dataaddress = outputs.Rows(i - 1).Item("address")
                                    End If
                                End If
                            Next
                            'identify reference address
                            'first test for exactly one output with data seqnum+1
                            Dim dataaddressseqnum As Integer = 9999
                            For i = 1 To outputs.Rows.Count
                                If outputs.Rows(i - 1).Item("address") = dataaddress Then dataaddressseqnum = outputs.Rows(i - 1).Item("seqnum")
                            Next
                            'sanity check - ensure we have a dataaddressseqnum
                            If dataaddressseqnum = 9999 Then Exit Function
                            'calc reference seqnum
                            Dim refseqnum As Integer = dataaddressseqnum + 1
                            'handle 255 to 0 case
                            If refseqnum = 256 Then refseqnum = 0
                            'see if we can find a single output with the reference seqnum
                            Dim refaddressfound As Boolean = False
                            For i = 1 To outputs.Rows.Count
                                If outputs.Rows(i - 1).Item("seqnum") = refseqnum Then
                                    'if we've already found one then this doesn't satisy 'single output with a seqnum +1 of the data address'
                                    If refaddressfound = True Then
                                        'exit without refaddress
                                        refaddressfound = False
                                        refaddress = ""
                                        Exit For
                                    Else
                                        refaddress = outputs.Rows(i - 1).Item("address")
                                        refaddressfound = True
                                    End If
                                End If
                            Next
                            'refaddress will be populated if we have found a single output with data seqnum+1
                            'if it's not populated move to second test (output amounts)
                            If refaddressfound = False And refaddress = "" Then
                                'here we test for a single output with the value the same as the exodus address (excluding data address)
                                For i = 1 To outputs.Rows.Count
                                    If outputs.Rows(i - 1).Item("amount") = exoamount And outputs.Rows(i - 1).Item("address") <> dataaddress Then
                                        'if we've already found one then this doesn't satisfy 'exactly two outputs with the same value as the data address'
                                        If refaddressfound = True Then
                                            'exit without refaddress
                                            refaddressfound = False
                                            refaddress = ""
                                            Exit For
                                        Else
                                            refaddress = outputs.Rows(i - 1).Item("address")
                                            refaddressfound = True
                                        End If
                                    End If
                                Next
                            End If
                            'see if we now have a dataaddress and refaddress, otherwise throw out tx
                            If dataaddress = "" Or refaddress = "" Then
                                Exit Function
                            Else
                                isvalidtx = True
                            End If
                        End If

                        'decode transaction
                        'is tx valid? 
                        If isvalidtx = True Then
                            Dim barray As Byte()
                            'multisig?
                            If multisig = True Then
                                'if still no packet data, abort function
                                If pubkeyhex = "" Then
                                    Return Nothing
                                    Exit Function
                                End If
                                Dim cleartext As String = decryptmastercoinpacket(txinputadd(txhighvalue), 1, pubkeyhex.Substring(2, 62))
                                cleartext = "02" & cleartext
                                barray = multisigbarray(cleartext)
                            Else 'not multisig
                                barray = ToByteArray(Trim(dataaddress))
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
                                returnobj.toadd = refaddress
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
            End If
        End If
    End Function
    Public Function decryptmastercoinpacket(ByVal fromadd As String, ByVal seqnum As Integer, ByVal pubkeyhex As String)
        Try
            Dim shahash As String = fromadd
            For i = 1 To seqnum
                shahash = sha256hash(shahash)
            Next
            Dim cleartext As String
            Dim a As Short
            For a = 1 To 61 Step 2
                Dim byte1 As Byte = Convert.ToByte(Mid(shahash, (a), 2), 16)
                Dim byte2 As Byte = Convert.ToByte(Mid(pubkeyhex, (a), 2), 16)
                cleartext = cleartext & (byte1 Xor byte2).ToString("X2")
            Next
            Return cleartext
        Catch ex As Exception
            MsgBox("ERROR FROM LIBRARY: EXCEPTION IN PACKET DECRYPTION - " & ex.Message)
        End Try
    End Function
    Public Function encryptmastercoinpacket(ByVal fromadd As String, ByVal seqnum As Integer, ByVal pubkeyhex As String)
        Dim shahash As String = fromadd
        Dim obfuscated As String
        For i = 1 To seqnum
            shahash = sha256hash(shahash)
        Next
        Dim a As Short
        For a = 1 To 61 Step 2
            Dim byte1 As Byte = Convert.ToByte(Mid(shahash, (a), 2), 16)
            Dim byte2 As Byte = Convert.ToByte(Mid(pubkeyhex, (a), 2), 16)
            obfuscated = obfuscated & (byte1 Xor byte2).ToString("X2")
        Next
        Return obfuscated
    End Function
    Public Function sha256hash(ByVal text As String)
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(text)
        Dim sha256prov As HashAlgorithm = New SHA256CryptoServiceProvider()
        Dim hashbytes As Byte() = sha256prov.ComputeHash(bytes)
        Dim hash As New StringBuilder
        For Each b As Byte In hashbytes
            hash.AppendFormat("{0:X2}", b)
        Next
        Return hash.ToString()
    End Function
    Public Function validateecdsa(ByVal pubkey As String)
        Dim validpoint As Boolean = False
        'check ecdsa validity
        Try
            Dim barray As Byte() = multisigbarray(pubkey)
            Dim ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1")
            Dim point = ps.Curve.DecodePoint(barray)

            Dim ysquared = point.Y.Multiply(point.Y)
            Dim xcubed = point.X.Multiply(point.X).Multiply(point.X)
            Dim xcurvea = point.X.Multiply(ps.Curve.A)
            Dim final = xcubed.Add(xcurvea).Add(ps.Curve.B)
            If ysquared.Equals(final) Then
                Return True
            Else
                Return False
            End If

        Catch e As Exception
            Return False
        End Try

    End Function

    Public Function getrandombyte()
        Dim s As String = "1234567890ABCDEF"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 2
            Dim idx As Integer = r.Next(0, 16)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString
    End Function
    Public Function encodetx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal toadd As String, ByVal curtype As Integer, ByVal amount As Long)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 35000 'include 0.00011 miner fee
        Dim encodedpubkey, frompubkey As String
        Dim isvalidecdsa As Boolean
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If toadd.Length < 27 Or toadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on to address")
                Exit Function
            End If
            If curtype < 1 Or curtype > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on currency type")
                Exit Function
            End If
            If amount = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on amount")
                Exit Function
            End If

            'calculate encoded public key for tx
            encodedpubkey = "01" 'compressedkey+seqnum
            encodedpubkey = encodedpubkey + "00000000" 'simple send
            encodedpubkey = encodedpubkey + i32tohexlittle(curtype)
            encodedpubkey = encodedpubkey + i64tohexlittle(amount)
            encodedpubkey = encodedpubkey + "0000000000000000000000000000" 'padding

            'obfuscate public key
            encodedpubkey = encryptmastercoinpacket(fromadd, 1, encodedpubkey)

            'build full key
            encodedpubkey = "02" & encodedpubkey & "00" 'last 00 will be rotated immediately

            'validate ECDSA point
            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey = encodedpubkey.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey)
            Loop

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
                MsgBox("Exeption thrown validating key: " & e.Message)
            End Try
            If frompubkey = "" Then
                MsgBox("Error locating public key for from address.")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray
            fromtxamount = 9999999999999
            For i = 0 To UBound(inputs)
                If (inputs(i).amount * 100000000) > (totaltxfee + 6000) And inputs(i).address = fromadd And (inputs(i).amount < fromtxamount) Then
                    fromtxid = inputs(i).txid
                    fromtxvout = inputs(i).vout
                    fromtxamount = inputs(i).amount
                End If
            Next
            If fromtxid = "" Or fromtxvout < 0 Or fromtxamount = 9999999999999 Then
                MsgBox("Insufficient funds for fee at from address.")
                Exit Function
            End If

            'handle change
            changeamount = (fromtxamount * 100000000) - totaltxfee

            'debug
            'MsgBox("DEBUG:" & vbCrLf & "Chosen input TXID is " & fromtxid & vbCrLf & "Chosen input VOUT is " & fromtxvout.ToString & vbCrLf & "Chosen input VALUE is " & (fromtxamount * 100000000).ToString & vbCrLf & "Change amount is " & changeamount.ToString)

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

        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function encodeselltx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal curtype As Integer, ByVal saleamount As Long, ByVal offeramount As Long, ByVal minfee As Long, ByVal timelimit As Integer, ByVal action As Integer)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 35000 'include 0.00011 miner fee
        Dim encodedpubkey, encodedpubkey2, frompubkey As String
        Dim isvalidecdsa As Boolean
        Dim minfeestr, timelimitstr As String
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If curtype < 1 Or curtype > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on currency type")
                Exit Function
            End If
            If saleamount = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on saleamount")
                Exit Function
            End If
            If offeramount = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on offeramount")
                Exit Function
            End If
            If minfee = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on minfee")
                Exit Function
            End If
            If timelimit < 1 Or timelimit > 999 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on timelimit")
                Exit Function
            End If
            If action < 1 Or action > 3 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on action")
                Exit Function
            End If

            'first pubkey
            encodedpubkey = "01" 'compressedkey+seqnum
            encodedpubkey = encodedpubkey + "00010014" 'sell v1
            encodedpubkey = encodedpubkey + i32tohexlittle(curtype)
            encodedpubkey = encodedpubkey + i64tohexlittle(saleamount)
            encodedpubkey = encodedpubkey + i64tohexlittle(offeramount)
            timelimitstr = Conversion.Hex(timelimit)
            If Len(timelimitstr) = 1 Then timelimitstr = "0" & timelimitstr
            encodedpubkey = encodedpubkey + timelimitstr
            minfeestr = i64tohexlittle(minfee)
            encodedpubkey = encodedpubkey + minfeestr.Substring(0, 10)

            'second pubkey
            encodedpubkey2 = "02" 'compressedkey+seqnum
            encodedpubkey2 = encodedpubkey2 + minfeestr.Substring(10, 6)
            encodedpubkey2 = encodedpubkey2 + "0" + action.ToString
            encodedpubkey2 = encodedpubkey2 + "000000000000000000000000000000000000000000000000000000" 'padding

            'obfuscate public keys
            encodedpubkey = encryptmastercoinpacket(fromadd, 1, encodedpubkey)
            encodedpubkey2 = encryptmastercoinpacket(fromadd, 2, encodedpubkey2)

            'build full keys
            encodedpubkey = "02" & encodedpubkey & "00" 'last 00 will be rotated immediately
            encodedpubkey2 = "02" & encodedpubkey2 & "00" 'last 00 will be rotated immediately

            'validate ECDSA points
            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey = encodedpubkey.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey)
            Loop

            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey2 = encodedpubkey2.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey2)
            Loop

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
                MsgBox("Exeption thrown validating key: " & e.Message)
            End Try
            If frompubkey = "" Then
                MsgBox("Error locating public key for from address.")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray
            fromtxamount = 9999999999999
            For i = 0 To UBound(inputs)
                If (inputs(i).amount * 100000000) > (totaltxfee + 6000) And inputs(i).address = fromadd And (inputs(i).amount < fromtxamount) Then
                    fromtxid = inputs(i).txid
                    fromtxvout = inputs(i).vout
                    fromtxamount = inputs(i).amount
                End If
            Next
            If fromtxid = "" Or fromtxvout < 0 Or fromtxamount = 9999999999999 Then
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

            txhex = txhex & "03" 'number of vouts, future: cater for 2 outs (no change) - since we check txin for >totaltxfee there will always be change for now

            'change output
            txhex = txhex & i64tohex(changeamount) 'changeamount value
            txhex = txhex & "19" 'length - 25 bytes
            txhex = txhex & "76a914" & addresstopubkey(fromadd) & "88ac" 'change scriptpubkey
            'exodus output
            txhex = txhex & i64tohex(txfee)
            txhex = txhex & "19"
            txhex = txhex & "76a914946cb2e08075bcbaf157e47bcb67eb2b2339d24288ac" 'exodus scriptpubkey

            'multisig output
            txhex = txhex & i64tohex(txfee * 3)
            txhex = txhex & "69" 'length - ??bytes?? calculate
            txhex = txhex & "51" '???
            txhex = txhex & "21" '???
            txhex = txhex & frompubkey 'first multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey 'second multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey2 'third multisig address
            txhex = txhex & "53ae" '???
            txhex = txhex & "00000000" 'locktime
            txhex = LCase(txhex)
            Return txhex
        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function
    Public Function encodetx50(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal ecosystem As Integer, ByVal proptype As Integer, ByVal previousprop As Long, ByVal category As String, ByVal subcategory As String, ByVal propname As String, ByVal propurl As String, ByVal propdata As String, ByVal props As Long)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 35000 'include 0.00011 miner fee
        Dim encodedpubkey, encodedpubkey2, encodedpubkey3, encodedpubkey4, frompubkey, clearpacket As String
        Dim isvalidecdsa As Boolean
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If ecosystem < 1 Or ecosystem > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on ecosystem")
                Exit Function
            End If
            If IsNothing(proptype) Then proptype = 1
            If IsNothing(previousprop) Then previousprop = 1
            If category = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on category")
                Exit Function
            End If
            If subcategory = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on subcategory")
                Exit Function
            End If
            If propname = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on propname")
                Exit Function
            End If
            If propurl = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on propurl")
                Exit Function
            End If
            If IsNothing(propdata) Then propdata = ""
            If IsNothing(props) Or props = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on props")
                Exit Function
            End If
            
            'cleartext
            clearpacket = ""
            clearpacket = clearpacket + "0000" 'transaction version
            clearpacket = clearpacket + "0032" 'txtype 50 - create fixed number of tokens
            clearpacket = clearpacket + "02" 'ecosystem - hard test msc for now
            clearpacket = clearpacket + "0001" 'property type - indivisible only for now
            clearpacket = clearpacket + i32tohexlittle(previousprop) 'previous property id - 4 byte unsigned 32bit int
            clearpacket = clearpacket + strtohex(category)
            clearpacket = clearpacket + strtohex(subcategory)
            clearpacket = clearpacket + strtohex(propname)
            clearpacket = clearpacket + strtohex(propurl)
            clearpacket = clearpacket + strtohex(propdata)
            clearpacket = clearpacket + i64tohexlittle(props) '8 byte number props 

            'split into packets - find a cleaner way to do this when time allows
            Dim cleartextlength As Integer = Len(clearpacket)
            Dim packetcount As Integer = 0
            Dim clearpacket1, clearpacket2, clearpacket3, clearpacket4 As String
            If cleartextlength < 61 Then 'single packet
                packetcount = 1
                clearpacket1 = "01" & clearpacket
            End If
            If cleartextlength > 60 And cleartextlength < 121 Then 'two packets
                packetcount = 2
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, Len(clearpacket) - 60)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 60)
                    clearpacket2 = clearpacket2 + "0"
                Next
            End If
            If cleartextlength > 121 And cleartextlength < 181 Then 'three packets
                packetcount = 3
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, 60)
                clearpacket3 = "03" & clearpacket.Substring(120, Len(clearpacket) - 120)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 120)
                    clearpacket3 = clearpacket3 + "0"
                Next
            End If
            If cleartextlength > 181 And cleartextlength < 241 Then 'four packets
                packetcount = 4
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, 60)
                clearpacket3 = "03" & clearpacket.Substring(120, 60)
                clearpacket4 = "04" & clearpacket.Substring(180, Len(clearpacket) - 180)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 180)
                    clearpacket4 = clearpacket4 + "0"
                Next
            End If
            'obfuscate public keys, build the full keys and then validate ECDSA points
            encodedpubkey = encryptmastercoinpacket(fromadd, 1, clearpacket1)
            encodedpubkey = "02" & encodedpubkey & "00" 'last 00 will be rotated immediately
            'validate ECDSA points
            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey = encodedpubkey.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey)
            Loop

            If packetcount > 1 Then
                encodedpubkey2 = encryptmastercoinpacket(fromadd, 2, clearpacket2)
                encodedpubkey2 = "02" & encodedpubkey2 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey2 = encodedpubkey2.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey2)
                Loop
            End If
            If packetcount > 2 Then
                encodedpubkey3 = encryptmastercoinpacket(fromadd, 3, clearpacket3)
                encodedpubkey3 = "02" & encodedpubkey3 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey3 = encodedpubkey3.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey3)
                Loop
            End If
            If packetcount > 3 Then
                encodedpubkey4 = encryptmastercoinpacket(fromadd, 4, clearpacket4)
                encodedpubkey4 = "02" & encodedpubkey4 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey4 = encodedpubkey4.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey4)
                Loop
            End If

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
                MsgBox("Exeption thrown validating key: " & e.Message)
            End Try
            If frompubkey = "" Then
                MsgBox("Error locating public key for from address.")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray
            fromtxamount = 9999999999999
            For i = 0 To UBound(inputs)
                If (inputs(i).amount * 100000000) > (totaltxfee + 6000) And inputs(i).address = fromadd And (inputs(i).amount < fromtxamount) Then
                    fromtxid = inputs(i).txid
                    fromtxvout = inputs(i).vout
                    fromtxamount = inputs(i).amount
                End If
            Next
            If fromtxid = "" Or fromtxvout < 0 Or fromtxamount = 9999999999999 Then
                MsgBox("Insufficient funds for fee at from address.")
                Exit Function
            End If

            'raw transaction build
            'handle change
            If packetcount = 2 Then totaltxfee = 35000
            If packetcount = 3 Then totaltxfee = 47000
            If packetcount = 4 Then totaltxfee = 53000
            changeamount = (fromtxamount * 100000000) - totaltxfee

            txhex = "01000000" 'version
            txhex = txhex & "01" 'vin count
            txhex = txhex & txidtohex(fromtxid) 'input txid hex
            txhex = txhex & i32tohex(fromtxvout) 'input vout 00000000
            txhex = txhex & "00" 'scriptsig length
            txhex = txhex & "ffffffff" 'sequence

            If packetcount > 0 And packetcount < 3 Then txhex = txhex & "03" 'number of vouts, future: cater for 2 outs (no change) - since we check txin for >totaltxfee there will always be change for now
            If packetcount > 2 And packetcount < 5 Then txhex = txhex & "04" 'number of vouts, future: cater for 2 outs (no change) - since we check txin for >totaltxfee there will always be change for now

            'change output
            txhex = txhex & i64tohex(changeamount) 'changeamount value
            txhex = txhex & "19" 'length - 25 bytes
            txhex = txhex & "76a914" & addresstopubkey(fromadd) & "88ac" 'change scriptpubkey
            'exodus output
            txhex = txhex & i64tohex(txfee)
            txhex = txhex & "19"
            txhex = txhex & "76a914946cb2e08075bcbaf157e47bcb67eb2b2339d24288ac" 'exodus scriptpubkey
            'first multisig output
            txhex = txhex & i64tohex(txfee * 3)
            txhex = txhex & "69" 'length - ??bytes?? calculate
            txhex = txhex & "51" '???
            txhex = txhex & "21" '???
            txhex = txhex & frompubkey 'first multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey 'second multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey2 'third multisig address - assuming minimum 2 packets for now
            txhex = txhex & "53ae" '???
            If packetcount > 2 And packetcount < 5 Then
                'second multisig output
                If packetcount = 3 Then
                    txhex = txhex & i64tohex(txfee * 2)
                    txhex = txhex & "69" 'length - ??bytes?? calculate
                    txhex = txhex & "51" '???
                    txhex = txhex & "21" '???
                    txhex = txhex & frompubkey 'first multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey3 'second multisig address
                    txhex = txhex & "53ae" '???
                Else
                    txhex = txhex & i64tohex(txfee * 3)
                    txhex = txhex & "69" 'length - ??bytes?? calculate
                    txhex = txhex & "51" '???
                    txhex = txhex & "21" '???
                    txhex = txhex & frompubkey 'first multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey3 'second multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey4 'third multisig address
                    txhex = txhex & "53ae" '???
                End If
            End If
            txhex = txhex & "00000000" 'locktime
            txhex = LCase(txhex)
            Return txhex
        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function
    Public Function encodetx51(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal ecosystem As Integer, ByVal proptype As Integer, ByVal previousprop As Long, ByVal category As String, ByVal subcategory As String, ByVal propname As String, ByVal propurl As String, ByVal propdata As String, ByVal desiredcurrency As Integer, ByVal propsperunit As Long, ByVal expirytime As Long, ByVal earlybird As Integer, ByVal issuerpc As Integer)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 35000 'include 0.00011 miner fee
        Dim encodedpubkey, encodedpubkey2, encodedpubkey3, encodedpubkey4, frompubkey, clearpacket As String
        Dim isvalidecdsa As Boolean
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If ecosystem < 1 Or ecosystem > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on ecosystem")
                Exit Function
            End If
            If IsNothing(proptype) Then proptype = 1
            If IsNothing(previousprop) Then previousprop = 1
            If category = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on category")
                Exit Function
            End If
            If subcategory = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on subcategory")
                Exit Function
            End If
            If propname = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on propname")
                Exit Function
            End If
            If propurl = "" Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on propurl")
                Exit Function
            End If
            If IsNothing(propdata) Then propdata = ""
            If desiredcurrency < 1 Or desiredcurrency > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on desiredcurrency")
                Exit Function
            End If
            If IsNothing(propsperunit) Or propsperunit = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on propsperunit")
                Exit Function
            End If
            If IsNothing(expirytime) Or expirytime = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on expirytime")
                Exit Function
            End If
            If IsNothing(earlybird) Then earlybird = 0
            If earlybird < 0 Or earlybird > 255 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on earlybird")
                Exit Function
            End If
            If IsNothing(issuerpc) Then issuerpc = 0
            If issuerpc < 0 Or issuerpc > 255 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on issuerpc")
                Exit Function
            End If

            Dim issuerstr, earlystr As String

            'cleartext
            clearpacket = ""
            clearpacket = clearpacket + "0000" 'transaction version
            clearpacket = clearpacket + "0033" 'txtype 51 - kickstarter style fundraiser
            clearpacket = clearpacket + "02" 'ecosystem - hard test msc for now
            clearpacket = clearpacket + "0001" 'property type - indivisible only for now
            clearpacket = clearpacket + i32tohexlittle(previousprop) 'previous property id - 4 byte unsigned 32bit int
            clearpacket = clearpacket + strtohex(category)
            clearpacket = clearpacket + strtohex(subcategory)
            clearpacket = clearpacket + strtohex(propname)
            clearpacket = clearpacket + strtohex(propurl)
            clearpacket = clearpacket + strtohex(propdata)
            clearpacket = clearpacket + i32tohexlittle(desiredcurrency) 'desire test MSC
            clearpacket = clearpacket + i64tohexlittle(propsperunit) '8 byte number props per unit invested
            clearpacket = clearpacket + i64tohexlittle(expirytime) 'deadline 64-bits standard unix timestamp, 8 bytes
            earlystr = Conversion.Hex(earlybird)
            If Len(earlystr) = 1 Then earlystr = "0" & earlystr
            clearpacket = clearpacket + earlystr 'early bonus% 8-bit unsigned integer, 1 byte
            issuerstr = Conversion.Hex(issuerpc)
            If Len(issuerstr) = 1 Then issuerstr = "0" & issuerstr
            clearpacket = clearpacket + issuerstr 'shares for issuer. 1 byte percentage again?
            'split into packets - find a cleaner way to do this when time allows
            Dim cleartextlength As Integer = Len(clearpacket)
            Dim packetcount As Integer = 0
            Dim clearpacket1, clearpacket2, clearpacket3, clearpacket4 As String
            If cleartextlength < 61 Then 'single packet
                packetcount = 1
                clearpacket1 = "01" & clearpacket
            End If
            If cleartextlength > 60 And cleartextlength < 121 Then 'two packets
                packetcount = 2
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, Len(clearpacket) - 60)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 60)
                    clearpacket2 = clearpacket2 + "0"
                Next
            End If
            If cleartextlength > 121 And cleartextlength < 181 Then 'three packets
                packetcount = 3
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, 60)
                clearpacket3 = "03" & clearpacket.Substring(120, Len(clearpacket) - 120)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 120)
                    clearpacket3 = clearpacket3 + "0"
                Next
            End If
            If cleartextlength > 181 And cleartextlength < 241 Then 'four packets
                packetcount = 4
                clearpacket1 = "01" & clearpacket.Substring(0, 60)
                clearpacket2 = "02" & clearpacket.Substring(60, 60)
                clearpacket3 = "03" & clearpacket.Substring(120, 60)
                clearpacket4 = "04" & clearpacket.Substring(180, Len(clearpacket) - 180)
                'add padding to last packet
                For i = 0 To (Len(clearpacket) - 180)
                    clearpacket4 = clearpacket4 + "0"
                Next
            End If
            'obfuscate public keys, build the full keys and then validate ECDSA points
            encodedpubkey = encryptmastercoinpacket(fromadd, 1, clearpacket1)
            encodedpubkey = "02" & encodedpubkey & "00" 'last 00 will be rotated immediately
            'validate ECDSA points
            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey = encodedpubkey.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey)
            Loop

            If packetcount > 1 Then
                encodedpubkey2 = encryptmastercoinpacket(fromadd, 2, clearpacket2)
                encodedpubkey2 = "02" & encodedpubkey2 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey2 = encodedpubkey2.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey2)
                Loop
            End If
            If packetcount > 2 Then
                encodedpubkey3 = encryptmastercoinpacket(fromadd, 3, clearpacket3)
                encodedpubkey3 = "02" & encodedpubkey3 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey3 = encodedpubkey3.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey3)
                Loop
            End If
            If packetcount > 3 Then
                encodedpubkey4 = encryptmastercoinpacket(fromadd, 4, clearpacket4)
                encodedpubkey4 = "02" & encodedpubkey4 & "00"
                isvalidecdsa = False
                Do While isvalidecdsa = False
                    Dim rbyte As String = getrandombyte()
                    encodedpubkey4 = encodedpubkey4.Substring(0, 64) & rbyte
                    isvalidecdsa = validateecdsa(encodedpubkey4)
                Loop
            End If

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
                MsgBox("Exeption thrown validating key: " & e.Message)
            End Try
            If frompubkey = "" Then
                MsgBox("Error locating public key for from address.")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray
            fromtxamount = 9999999999999
            For i = 0 To UBound(inputs)
                If (inputs(i).amount * 100000000) > (totaltxfee + 6000) And inputs(i).address = fromadd And (inputs(i).amount < fromtxamount) Then
                    fromtxid = inputs(i).txid
                    fromtxvout = inputs(i).vout
                    fromtxamount = inputs(i).amount
                End If
            Next
            If fromtxid = "" Or fromtxvout < 0 Or fromtxamount = 9999999999999 Then
                MsgBox("Insufficient funds for fee at from address.")
                Exit Function
            End If

            'raw transaction build
            'handle change
            If packetcount = 2 Then totaltxfee = 35000
            If packetcount = 3 Then totaltxfee = 47000
            If packetcount = 4 Then totaltxfee = 53000
            changeamount = (fromtxamount * 100000000) - totaltxfee

            txhex = "01000000" 'version
            txhex = txhex & "01" 'vin count
            txhex = txhex & txidtohex(fromtxid) 'input txid hex
            txhex = txhex & i32tohex(fromtxvout) 'input vout 00000000
            txhex = txhex & "00" 'scriptsig length
            txhex = txhex & "ffffffff" 'sequence

            If packetcount > 0 And packetcount < 3 Then txhex = txhex & "03" 'number of vouts, future: cater for 2 outs (no change) - since we check txin for >totaltxfee there will always be change for now
            If packetcount > 2 And packetcount < 5 Then txhex = txhex & "04" 'number of vouts, future: cater for 2 outs (no change) - since we check txin for >totaltxfee there will always be change for now

            'change output
            txhex = txhex & i64tohex(changeamount) 'changeamount value
            txhex = txhex & "19" 'length - 25 bytes
            txhex = txhex & "76a914" & addresstopubkey(fromadd) & "88ac" 'change scriptpubkey
            'exodus output
            txhex = txhex & i64tohex(txfee)
            txhex = txhex & "19"
            txhex = txhex & "76a914946cb2e08075bcbaf157e47bcb67eb2b2339d24288ac" 'exodus scriptpubkey
            'first multisig output
            txhex = txhex & i64tohex(txfee * 3)
            txhex = txhex & "69" 'length - ??bytes?? calculate
            txhex = txhex & "51" '???
            txhex = txhex & "21" '???
            txhex = txhex & frompubkey 'first multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey 'second multisig address
            txhex = txhex & "21" '???
            txhex = txhex & encodedpubkey2 'third multisig address
            txhex = txhex & "53ae" '???
            If packetcount > 2 And packetcount < 5 Then
                'second multisig output
                If packetcount = 3 Then
                    txhex = txhex & i64tohex(txfee * 2)
                    txhex = txhex & "69" 'length - ??bytes?? calculate
                    txhex = txhex & "51" '???
                    txhex = txhex & "21" '???
                    txhex = txhex & frompubkey 'first multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey3 'second multisig address
                    txhex = txhex & "53ae" '???
                Else
                    txhex = txhex & i64tohex(txfee * 3)
                    txhex = txhex & "69" 'length - ??bytes?? calculate
                    txhex = txhex & "51" '???
                    txhex = txhex & "21" '???
                    txhex = txhex & frompubkey 'first multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey3 'second multisig address
                    txhex = txhex & "21" '???
                    txhex = txhex & encodedpubkey4 'third multisig address
                    txhex = txhex & "53ae" '???
                End If
            End If
            txhex = txhex & "00000000" 'locktime
            txhex = LCase(txhex)
            Return txhex
        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function
    Public Function encodeaccepttx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal toadd As String, ByVal curtype As Integer, ByVal purchaseamount As Long, ByVal mintxfee As Long)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 24000 + mintxfee 'include minimum miner fee specified
        If totaltxfee < 35000 Then totaltxfee = 35000 'sanity check we haven't specified too lower a fee
        Dim encodedpubkey, frompubkey As String
        Dim isvalidecdsa As Boolean
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If toadd.Length < 27 Or toadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on to address")
                Exit Function
            End If
            If curtype < 1 Or curtype > 2 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on currency type")
                Exit Function
            End If
            If purchaseamount = 0 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on purchaseamount")
                Exit Function
            End If

            'calculate encoded public key for tx
            encodedpubkey = "01" 'compressedkey+seqnum
            encodedpubkey = encodedpubkey + "00000016" 'simple send
            encodedpubkey = encodedpubkey + i32tohexlittle(curtype)
            encodedpubkey = encodedpubkey + i64tohexlittle(purchaseamount)
            encodedpubkey = encodedpubkey + "0000000000000000000000000000" 'padding

            'obfuscate public key
            encodedpubkey = encryptmastercoinpacket(fromadd, 1, encodedpubkey)

            'build full key
            encodedpubkey = "02" & encodedpubkey & "00" 'last 00 will be rotated immediately

            'validate ECDSA point
            isvalidecdsa = False
            Do While isvalidecdsa = False
                Dim rbyte As String = getrandombyte()
                encodedpubkey = encodedpubkey.Substring(0, 64) & rbyte
                isvalidecdsa = validateecdsa(encodedpubkey)
            Loop

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
                MsgBox("Exeption thrown validating key: " & e.Message)
            End Try
            If frompubkey = "" Then
                MsgBox("Error locating public key for from address.")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray
            fromtxamount = 9999999999999
            For i = 0 To UBound(inputs)
                If (inputs(i).amount * 100000000) > (totaltxfee + 6000) And inputs(i).address = fromadd And (inputs(i).amount < fromtxamount) Then
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
            txhex = LCase(txhex)
            Return txhex
        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function
    Public Function encodepaytx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal fromadd As String, ByVal toadd As String, ByVal paymentamount As Long)
        Dim txhex, fromtxid As String
        Dim fromtxvout As Integer = -1
        Dim fromtxamount As Double = -1
        Dim changeamount As Long
        Dim txfee As Long = 6000
        Dim totaltxfee As Long = 17000 'include 0.00011 miner fee
        Dim totalout As Long = 0
        Try
            'sanity check input
            If fromadd.Length < 27 Or fromadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on from address")
                Exit Function
            End If
            If toadd.Length < 27 Or toadd.Length > 34 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on to address")
                Exit Function
            End If
            If paymentamount < 5460 Then
                MsgBox("Message from library - aborting transaction build, sanity check failed on paymentamount (under dust)")
                Exit Function
            End If

            'lookup unspent for from address
            Dim listunspent As unspent = JsonConvert.DeserializeObject(Of unspent)(rpccall(bitcoin_con, "listunspent", 2, 1, 999999, 0))
            Dim inputs() As result_unspent = listunspent.result.ToArray

            'loop through inputs until we have enough to cover the total
            Dim inputsum As Long = 0
            Dim inputcount As Short = 0
            Dim inputhex As String = ""
            For i = 0 To UBound(inputs)
                'is the input for the right address?
                If inputs(i).address = fromadd Then
                    'MsgBox(totalout + 6000)
                    'MsgBox(inputs(i).txid)
                    inputhex = inputhex & txidtohex(inputs(i).txid)
                    inputhex = inputhex & i32tohex(inputs(i).vout)
                    inputhex = inputhex & "00" 'scriptsig length
                    inputhex = inputhex & "ffffffff" 'sequence
                    inputsum = inputsum + (inputs(i).amount * 100000000)
                    inputcount = inputcount + 1
                    'MsgBox(inputsum)
                    'sanity check input count is not >24
                    If inputcount > 24 Then
                        MsgBox("ERROR: Input count >24, temporary restriction applied - library currently will not send transactions with over 24 inputs.  Aborting")
                        Exit Function
                    End If
                    'quick fix for avoiding large transactions with small fees - come back & revisit this
                    If inputcount <= 4 Then totaltxfee = 17000
                    If inputcount > 4 Then totaltxfee = 27000
                    If inputcount > 9 Then totaltxfee = 37000
                    If inputcount > 14 Then totaltxfee = 47000
                    If inputcount > 19 Then totaltxfee = 57000
                    'recalculate totalout
                    totalout = totaltxfee + paymentamount
                    'do we have enough yet?
                    If inputsum >= totalout + 6000 Then Exit For
                End If
            Next

            'check inputsum > totalout
            If inputsum < totalout Then
                MsgBox("ERROR: Insufficient funds to cover payment and fees at from address, aborting")
                Exit Function
            End If
            'sanity check inputhex is not empty and inputcount>0
            If inputhex = "" Or inputcount = 0 Then
                MsgBox("ERROR: Inputhex/inputcount sanity check failure, aborting")
                Exit Function
            End If

            'prepend vin count
            inputhex = i16tohex(inputcount) & inputhex

            'handle change
            changeamount = inputsum - totalout

            'build tx hex raw
            txhex = "01000000" 'version
            txhex = txhex & inputhex
            txhex = txhex & "03" 'number of vouts, future: cater for 2 outs (no change) - since we check for >totalout+6000 there will always be change for now
            'change output
            txhex = txhex & i64tohex(changeamount) 'changeamount value
            txhex = txhex & "19" 'length - 25 bytes
            txhex = txhex & "76a914" & addresstopubkey(fromadd) & "88ac" 'change scriptpubkey
            'exodus output
            txhex = txhex & i64tohex(txfee)
            txhex = txhex & "19"
            txhex = txhex & "76a914946cb2e08075bcbaf157e47bcb67eb2b2339d24288ac" 'exodus scriptpubkey
            'reference/destination output
            txhex = txhex & i64tohex(paymentamount)
            txhex = txhex & "19"
            txhex = txhex & "76a914" & addresstopubkey(toadd) & "88ac" 'reference scriptpubkey 

            'finish up
            txhex = txhex & "00000000" 'locktime
            txhex = LCase(txhex)
            Return txhex
        Catch ex As Exception
            MsgBox("LIBRARY ERROR.  Function aborted." & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function getaddresses(ByVal bitcoin_con As bitcoinrpcconnection)
        Dim addresslist As New List(Of btcaddressbal)
        Dim plainaddresslist, myaddresslist As New List(Of String)

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
        'list_received_byaaddress also returns addresses transactions have been sent to (???) - run additional ismine check on each address
        For Each address As String In plainaddresslist
            Dim validate As validate = JsonConvert.DeserializeObject(Of validate)(rpccall(bitcoin_con, "validateaddress", 1, address, 0, 0))
            If validate.result.ismine = True Then
                myaddresslist.Add(address)
            End If
        Next

        'loop through plainaddresslist and get balances to create addresslist object
        For Each address In myaddresslist
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
            MsgBox("Exeption thrown getting block : " & e.Message)
        End Try
    End Function
    Public Function getblocktemplate(ByVal bitcoin_con As bitcoinrpcconnection)
        Try
            Dim blocktemplate As blocktemplate = JsonConvert.DeserializeObject(Of blocktemplate)(rpccall(bitcoin_con, "getblocktemplate", 0, 0, 0, 0))
            Return blocktemplate
        Catch e As Exception
            MsgBox("Exeption thrown getting blocktemplate : " & e.Message)
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
            'MsgBox("Exeption thrown getting block hash : " & e.Message) 'commented due to startup check error verbosity
        End Try
    End Function
    Public Function gettransaction(ByVal bitcoin_con As bitcoinrpcconnection, ByVal txid As String)
        Try
            Dim tx As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, txid, 1, 0))
            Return tx
        Catch e As Exception
            MsgBox("Exeption thrown getting transaction (" & txid & ") : " & e.Message)
        End Try
    End Function
    Public Function ismastercointx(ByVal bitcoin_con As bitcoinrpcconnection, ByVal txid As String)

        Dim tx As txn = JsonConvert.DeserializeObject(Of txn)(rpccall(bitcoin_con, "getrawtransaction", 2, txid, 1, 0))
        Dim vouts() As Vout = tx.result.vout.ToArray
        Dim ismultisig As Boolean = False
        Dim ismsc As Boolean = False
        Dim txtype As String
        Dim pubkeyhex As String = ""
        Try
            For i = 0 To UBound(vouts)
                If Not IsNothing(vouts(i).scriptPubKey.addresses) Then
                    If vouts(i).scriptPubKey.addresses(0).ToString = "1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P" Then
                        ismsc = True
                    End If
                    'see if Class B
                    If vouts(i).scriptPubKey.type = "multisig" Then 'we get away with using the first input to check tx type for now
                        ismultisig = True
                        'ismsc = True 'premature, currently exodus outputs are required for a transaction
                        Dim asmvars As String() = vouts(i).scriptPubKey.asm.ToString.Split(" ")
                        If pubkeyhex = "" Then pubkeyhex = asmvars(2)
                    End If
                End If
            Next

            If ismsc = True Then
                Dim txinputs As Integer
                Dim txhighvalue As Integer
                Dim txinputadd(1000) As String
                Dim txinputamount(1000) As Double
                Dim exoamount As Double
                txinputs = 0
                'calculate input addresses 
                Dim vins() As Vin = tx.result.vin.ToArray
                'get tx type
                If ismultisig = False Then
                    'class A, always simple send/generate
                    Return "simple"
                    Exit Function
                Else
                    'class B, check transaction type
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
                        'if still no packet data, abort function
                        If pubkeyhex = "" Then
                            Return ("None")
                            Exit Function
                        End If

                        'decode transaction - first packet always contains txtype
                        Dim cleartext As String = decryptmastercoinpacket(txinputadd(txhighvalue), 1, pubkeyhex.Substring(2, 62))
                        cleartext = "02" & cleartext
                        Dim barray As Byte()
                        barray = multisigbarray(cleartext)
                        Dim transbytes() As Byte = {barray(4), barray(5)}

                        'handle endianness
                        If BitConverter.IsLittleEndian = True Then
                            Array.Reverse(transbytes)
                        End If
                        Dim transtype As Integer = BitConverter.ToUInt16(transbytes, 0)
                        If transtype = 0 Then
                            Return "simple"
                            Exit Function
                        End If
                        If transtype = 20 Then
                            Return "selloffer"
                            Exit Function
                        End If
                        If transtype = 22 Then
                            Return "acceptoffer"
                            Exit Function
                        End If
                        If transtype = 50 Then
                            Return "spcreatefixed"
                            Exit Function
                        End If
                        If transtype = 51 Then
                            Return "spcreatevar"
                            Exit Function
                        End If
                    End If
                End If
            End If
            Return "None"
        Catch e As Exception
            'MsgBox("Exeption thrown checking if " & txid & " is a Mastercoin transaction: " & e.Message)
            'Allow exception for original multisig (can't decode properly) - come back and trap this properly
            Return "None"
        End Try
    End Function
    Public Function addresstopubkey(ByVal address As String)
        Dim hex() As Byte = ToByteArray(address)
        If Not ((hex Is Nothing) OrElse Not (hex.Length <> 21)) Then
            Dim pubkey As String = bytearraytostring(hex, 1, 20)
            Return pubkey
        End If
    End Function
    Public Function txidtohex(ByVal txid As String)
        Dim hex() As Byte = StringToByteArray(txid)
        Array.Reverse(hex)
        If ((hex IsNot Nothing) And (hex.Length = 32)) Then
            Dim txidhex As String = bytearraytostring(hex, 0, 32)
            Return txidhex
        End If
    End Function
    Public Function strtohex(ByVal str As String)
        Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(str)
        Dim rv As String = ""
        If (bytes IsNot Nothing) Then
            For i = 0 To (Len(str) - 1)
                rv = (rv + (String.Format("{0:X2}", bytes(i))))
            Next
            rv = rv + "00" 'null terminator
            Return rv
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
    Public Function i16tohex(ByVal amount As Byte)
        Dim amounthex As String = String.Format("{0:X2}", amount)
        Return amounthex
    End Function
    Public Function StringToByteArray(ByVal hex As [String]) As Byte()
        Dim NumberChars As Integer = hex.Length
        Dim bytes As Byte() = New Byte(NumberChars \ 2 - 1) {}
        For i As Integer = 0 To NumberChars - 1 Step 2
            bytes(i \ 2) = Convert.ToByte(hex.Substring(i, 2), 16)
        Next
        Return bytes
    End Function
    Public Function strToByteArray(ByVal str As [String]) As Byte()
        Dim NumberChars As Integer = str.Length
        Dim bytes As Byte() = New Byte(NumberChars - 1) {}
        For i As Integer = 0 To NumberChars - 1
            bytes(i) = Convert.ToByte(str.Substring(i, 1), 16)
        Next
        Return bytes
    End Function
End Class
