    Option Explicit
    '========================================================================== 
    ' LittleSOAPviaMIMEClient : SOAP request via a multi part MIME message
    '
    ' Author: Paula Paul, Paul Software, Inc.
    '
    ' Requires: 1) Microsoft.XMLHTTP object, for dispatching the SOAP request
    '              and obtaining the results via HTTP
    '           2) Windows Scripting Host, in order to browse SOAP results via 
    '              Notepad.  WSH is not required to dispatch the SOAP request
    '              and obtain the results.
    '
    ' This script reads a multi part MIME message (a text file) from disk and
    ' posts the data to a specified web service / web method.
    '
    ' Usage: Modify WebServiceURL, SOAPEnvelopeDirectory and SOAPResponseFile
    '        settings in the script below if you wish to override the defaults.
    '        Modify the WebServiceURL and WebMethodName variables to call
    '        different web services or methods.
    '========================================================================== 
    Dim SOAPEnvelope
    Dim SOAPEnvelopeDirectory
    Dim SOAPResponseFile
    Dim MIMEMessageFile
    Dim WebServiceURL
    Dim WebMethodName
    Dim requestHTTP
    Dim responseText
    Dim wShell
    
    '=========================================================================
    ' Set the "environment variables" 
    '=========================================================================  
    WebServiceURL = "http://localhost/ContentManager/Content.asmx"
    SOAPEnvelopeDirectory = "C:\ContentWebService\SOAPEnvelopes\"
    SOAPResponseFile = "C:\ContentWebService\SOAPResults\SOAPResults.txt"
    
    '========================================================================
    ' test the Publish method via a multi part MIME message
    '========================================================================
    'Test the Publish method via a multi part MIME message                                             
    WebMethodName = "Publish"                                           
    MIMEMessageFile = SOAPEnvelopeDirectory & "MIMEMessage.txt"  
     
    '========================================================================== 
    ' LittleSOAPviaMIMEClient main logic
    '==========================================================================                                                                   
    ' get the multi part MIME message from disk
    SOAPEnvelope = getSOAPEnvelope(MIMEMessageFile)

    ' poor man's debug
    MsgBox("MIME message:     " & SOAPEnvelope)

    Set requestHTTP = CreateObject("Microsoft.XMLHTTP")

    requestHTTP.open "POST", WebServiceURL, false 
    ' Set the Content-Type header to indicate we are sending a "Multipart/Related"
    ' (MIME) message, and include the start content ID
    requestHTTP.setrequestheader "Content-Type", "Multipart/Related; boundary=MIME_boundary; type=text/xml; start=<soapenvelope.xml@paulsoftware.com>"
    requestHTTP.setrequestheader "Content-Description", "A multi part MIME message with a SOAP envelope."
    requestHTTP.setrequestheader "SOAPAction", WebMethodName
    requestHTTP.Send SoapEnvelope 
    MsgBox("Request sent.  HTTP request status= " & requestHTTP.status)
  
    ' get the SOAP response and save it to disk
    responseText = requestHTTP.responseText
    writeResults SOAPResponseFile, responseText
    
    ' browse the SOAPResults using Notepad
    Set wShell = CreateObject("WScript.Shell")
    wShell.Run "NOTEPAD.EXE " + SOAPResponseFile 

    '==========================================================================
    ' getSOAPEnvelope function (reads the soap envelope xml file from disk)
    '==========================================================================
    Function getSOAPEnvelope(file)
      Dim fso, f
      
      Set fso = CreateObject("Scripting.FileSystemObject")
         
      ' open the soap envelope file and return a string containing its contents.
      Set f = fso.OpenTextFile(file, 1)
      getSOAPEnvelope =   f.ReadAll
      f.Close
    End Function

    '==========================================================================
    ' write results function (writes the responseText to disk)
    '==========================================================================
    Function writeResults(file, results)
      Dim fso, f
      
      Set fso = CreateObject("Scripting.FileSystemObject")
         
     ' create or overwrite the output file
      Set f = fso.CreateTextFile(file, -1)
      ' write the SOAP results to the file
      f.Write results 
      f.Close
    End Function