    Option Explicit
    '========================================================================== 
    ' LittleSOAPClient : possibly the world's smallest SOAP client :-)
    '
    ' Author: Paula Paul, Paul Software, Inc.
    '
    ' Requires: 1) Microsoft.XMLHTTP object, for dispatching the SOAP request
    '              and obtaining the results via HTTP
    '           2) Windows Scripting Host, in order to browse SOAP results via 
    '              Notepad.  WSH is not required to dispatch the SOAP request
    '              and obtain the results.
    '
    ' This script reads a SOAP envelope (an xml file) from disk and posts the
    ' data as a SOAP message to a specified web service / web method.
    '
    ' Usage: Modify WebServiceURL, SOAPEnvelopeDirectory and SOAPResponseFile
    '        settings in the script below if you wish to override the defaults.
    '        Modify the WebServiceURL and WebMethodName variables to call
    '        different web services or methods.
    '========================================================================== 
    Dim SOAPEnvelope
    Dim SOAPEnvelopeDirectory
    Dim SOAPResponseFile
    Dim WebServiceURL
    Dim WebMethodName
    Dim SOAPEnvelopeFile
    Dim requestHTTP
    Dim responseText
    Dim wShell
    
    '========================================================================
    ' Set the "environment variables"
    '========================================================================   
    WebServiceURL = "http://localhost/ContentManager/Content.asmx"
    SOAPEnvelopeDirectory = "C:\ContentWebService\SOAPEnvelopes\"
    SOAPResponseFile = "C:\ContentWebService\SOAPResults\SOAPResults.txt"
    
    '========================================================================
    ' test the three web methods by un-commenting a section below
    '========================================================================
    ' 1. Test the PublishXML method
    WebMethodName = "PublishXML"   
    SOAPEnvelopeFile = SOAPEnvelopeDirectory & "PublishXML.xml"
    
    ' 2. Test the Publish method                                               
    'WebMethodName = "Publish"                                           
    'SOAPEnvelopeFile = SOAPEnvelopeDirectory & "Publish.xml"  
     
    '========================================================================= 
    ' LittleSOAPClient main logic
    '=========================================================================                                                                   
    ' get the SOAP envelope (and XML document) from disk
    SOAPEnvelope = getSOAPEnvelope(SOAPEnvelopeFile)

    ' poor man's debug
    MsgBox("SOAP Envelope:     " & SOAPEnvelope)

    Set requestHTTP = CreateObject("Microsoft.XMLHTTP")

    requestHTTP.open "POST", WebServiceURL, false 
    requestHTTP.setrequestheader "Content-Type", "text/xml"
    requestHTTP.setrequestheader "SOAPAction", WebMethodName
    requestHTTP.Send SoapEnvelope 
    MsgBox("Request sent.  HTTP request status= " & requestHTTP.status)
  
    ' get the SOAP response and save it to disk
    responseText = requestHTTP.responseText
    ' MsgBox responseText
    writeResults SOAPResponseFile, responseText
    
    ' browse the SOAPResults using Notepad
    Set wShell = CreateObject("WScript.Shell")
    wShell.Run "NOTEPAD.EXE " + SOAPResponseFile        

    '=========================================================================
    ' getSOAPEnvelope function (reads the soap envelope xml file from disk)
    '=========================================================================
    Function getSOAPEnvelope(file)
      Dim fso, f
      
      Set fso = CreateObject("Scripting.FileSystemObject")
         
      ' open the envelope file and return a string containing its contents.
      Set f = fso.OpenTextFile(file, 1)
      getSOAPEnvelope =   f.ReadAll
      f.Close
    End Function
  
    '=========================================================================
    ' write results function (writes the responseText to disk)
    '=========================================================================
    Function writeResults(file, results)
      Dim fso, f
      
      Set fso = CreateObject("Scripting.FileSystemObject")
         
      ' create or overwrite the output file
      Set f = fso.CreateTextFile(file, -1)
      ' write the SOAP results to the file
      f.Write results 
      f.Close
    End Function