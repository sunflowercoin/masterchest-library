Initial commit of the Masterchest Library.

This is a .NET DLL that can be referenced in a project and used to perform Mastercoin functions.

This readme will be updated as time allows, however a rough run down of the useful functions would be:

Firstly import Masterchest:

	Imports Masterchest.Masterchest

Create a new instance of the Masterchest lib:
	
	Dim mlib As New Masterchest.mlib

Create a bitcoind/qt connection:

	Dim bitcoin_con as bitcoinrpcconnection
	bitcoinrpcserver = "127.0.0.1"
        bitcoinrpcport = 8322
        bitcoinrpcuser = "rpcuser"
	bitcoinrpcpassword = "rpcpassword"

Retrieve a block hash:

	Dim hash As blockhash = mlib.getblockhash(bitcoin_con, blocknumber)

Retrieve a block:

	Dim gotblock As Block = mlib.getblock(bitcoin_con, blockhash)

Retrieve the block currently being mined:

	Dim result as blocktemplate = mlib.getblocktemplate(bitcoin_con)

Create an array of transaction IDs in a block:

	Dim txarray() As String = gotblock.result.tx.ToArray

Loop through transactions in a block:

	For Each tx As String In gotblock.result.tx
            'do something
        Next

Retrieve a transaction:

	Dim txn As txn = mlib.gettransaction(bitcoin_con, transactionid)
	MsgBox(txn.result.time)

Check if a transaction looks like it cointains a Mastercoin transaction:

	Dim ismastercoin As Boolean
        ismastercoin = mlib.ismastercointx(bitcoin_con, transactionid)
        MsgBox(ismastercoin)

Decode a Mastercoin transaction (original or multisig):

	Dim mtx As txn = mlib.gettransaction(bitcoin_con, transactionid)

        'before Exodus cutoff so check for generate 
        Dim generatemastercointxinfo As mastercointx
        If mtx.result.blocktime < 1377993875 Then generatemastercointxinfo = mlib.getmastercointransaction(bitcoin_con, transactionid, "generate")

        'check for simple send
        Dim sendmastercointxinfo As mastercointx = mlib.getmastercointransaction(bitcoin_con, transactionid, "send")

        If generatemastercointxinfo IsNot Nothing Then
            MsgBox("From: " & generatemastercointxinfo.fromadd)
            MsgBox("To: " & generatemastercointxinfo.toadd)
            MsgBox("Type: " & generatemastercointxinfo.type)
            MsgBox("Currency: " & generatemastercointxinfo.curtype) '0 means both 1 & 2 for now
            MsgBox("Amount: " & generatemastercointxinfo.value)
        End If

        If sendmastercointxinfo IsNot Nothing Then
            MsgBox("From: " & sendmastercointxinfo.fromadd)
            MsgBox("To: " & sendmastercointxinfo.toadd)
            MsgBox("Type: " & sendmastercointxinfo.type)
            MsgBox("Currency: " & sendmastercointxinfo.curtype)
            MsgBox("Amount: " & sendmastercointxinfo.value)
        End If

Encode a Mastercoin transaction (multisig only):

	Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
	'send rawtx through signing and broadcast
	
