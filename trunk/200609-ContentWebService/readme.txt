Installing and using the ContentManager web service example:
------------------------------------------------------------
Requires:  .NET Beta 2 (SDK, and VS.NET in order to use the project file)

1) unzip all the files (preserving directory structure) into a directory named "C:\ContentWebService"
   (be sure to check that subdirectories called "PublishedContent" and "SOAPResults" are created)

2) using the Internet Services Manager, create a new virtual directory called "ContentManager", and base the virtual directory on the subdirectory at "C:\ContentWebService\ContentManager".  Accept the default web settings.

3) once the virtual directory has been created, right click the "ContentManager" virtual directory in the Internet Services Manager and select "All Tasks -> Configure Server Extensions".  Accept the default settings.

4) build the web service 
   - open the solution file and build it using visual studio or 
   - use the command line: devenv C:\ContentWebService\ContentManager.sln /build debug /projectconfig debug
     (this assumes devenv.exe is in your path) 
      <TODO> NOTE: For some reason this does not work for me until after I open the solution with VS once!!!
      <TODO> include make file with the sample

5) use the SOAP client in "C:\ContentWebService\TheLittleSOAPClient\LittleSOAPClient.vbs" to publish content via the  web service methods PublishXML and Publish.  The client is set up to publish xml documents by default; edit the LittleSOAPClient.vbs file to find instructions for testing the other methods.

6) use the SOAP-via-MIME client in "C:\ContentWebService\TheLittleSOAPClient\LittleSOAPviaMIMEClient.vbs" to call the Publish web method using a SOAP envelope encapsulated within a multi part MIME message.

NOTE:
If you unzip these files to a directory or drive other than the default ("C:\ContentWebService"), you will need to change the following files:
1) LittleSOAPClient.vbs - change the SOAPEnvelopeDirectory variable value
2) web.config (in the ContentManager subdirectory) - change the ContentDirectory key setting in appSettings
