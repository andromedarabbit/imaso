using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;

namespace ContentManager
{
  //============================================================
  // PublishViaMIME Soap Extension
  //============================================================
  ///<summary>
  ///     The PublishViaMIME class is a SOAP extension that allows
  ///     the 'Publish' web method to be invoked either via a standard
  ///     SOAP envelope (posted to the web service via HTTP), or via
  ///     a SOAP envelope that is the root of a multi part MIME message
  ///     (also posted to the web service via HTTP)
  ///     
  ///     The SOAP extension determines the message type by examining
  ///     the Content-Type header of the message.
  ///     If the Content-Type is "Multipart/Related", the SOAP envelope
  ///     is extracted (by finding the message part referenced by the
  ///     "start=" value in the Content-Type header), and all content
  ///     references within the SOAP message (href:cid="xxxx") are 
  ///     resolved.
  ///     
  ///     The resulting SOAP message with resolved content references
  ///     is then substituted for the original unserialized request
  ///     during the ProcessMessage.BeforeDeserialize stage, the
  ///     Content-Type is reset to text/xml, and normal web method
  ///     processing occurs.
  ///     
  ///     The 'Publish' web method is decorated with the attribute:
  ///     [PublishViaMIME]
  ///     to enable this SOAP extension.
  ///     
  ///     This is a very simple (and not robust) implementation of one
  ///     approach described in "SOAP Messages with Attachments":
  ///     http://msdn.microsoft.com/library/en-us/dnsoapsp/html/soapattachspec.asp
  ///</summary>
  ///
  ///<remarks>
  ///<author name="Paula Paul"
  ///        company="Paul Software, Inc."
  ///        email="Paula@PaulSoftware.com"> 
  ///</author>
  ///
  ///<changelog>
  ///  <change     date= "07/05/2001"   name="Paula Paul">
  ///    <tracking build=""             issueIDs=""></tracking>
  ///    <item summary=  "created class"></item>  
  ///    <item summary=  "created Content-Type handling logic"></item>
  ///  </change>
  ///  <change     date= "07/11/2001"   name="Paula Paul">
  ///    <tracking build=""             issueIDs=""></tracking>
  ///    <item summary=  "first pass with document substitution"></item>                
  ///  </change>
  ///  <change     date= "07/16/2001"   name="Paula Paul">
  ///    <tracking build=""             issueIDs=""></tracking>
  ///    <item summary=  "finished comments and code cleanup"></item>
  ///  </change>
  ///</changelog>
  ///</remarks>
  //============================================================== 
  public class PublishViaMIME : SoapExtension 
  {

    Stream oldStream;      // references to input and output streams
    Stream newStream;
    TextReader reader;     // reader and writer are used to copy streams
    TextWriter writer;
    string MIMEBoundary = "";
    string startContentID = "";
    string SOAPenvelope = "";
   
    //============================================================
    // Override: GetInitializer (1 of 2)
    //============================================================
    ///<summary>
    /// we don't need any information on initialization - GetInitializer
    /// and Initialize don't do anything... (but they must be present)
    ///</summary>
    //============================================================
    public override object GetInitializer(LogicalMethodInfo methodInfo, 
                                          SoapExtensionAttribute attribute) 
    {
      return null;
    }

    //============================================================
    // Required : GetInitializer (2 of 2)
    //============================================================
    ///<summary>
    /// we don't need any information on initialization - GetInitializer
    /// and Initialize don't do anything... (but they must be present)
    ///</summary>
    //============================================================
    public override object GetInitializer(System.Type type) 
    {
      return null;
    }

    //============================================================
    // Override: Initialize
    //============================================================
    ///<summary>
    /// we don't need any information on initialization - GetInitializer
    /// and Initialize don't do anything... (but they must be present)
    ///</summary>
    //============================================================
    public override void Initialize(object initializer) 
    {
    }

    //============================================================
    // Override: ChainStream
    //============================================================
    ///<summary>
    ///  ChainStream allows us to save a reference to the Stream representing
    ///  the SOAP request or SOAP response so we can manipulate the request
    ///  (or response) stream.
    ///  We need to save the reference to the stream during ChainStream,
    ///  because the stream is not writable during subsequent calls to
    ///  ProcessMessage.
    /// 
    /// The flow of control is:
    /// 1) ChainStream 
    ///      provides access to the incoming serialized stream buffer
    /// 2) ProcessMessage.BeforeDeserialize 
    ///      replace the original MIME input with the SOAP message extracted
    ///      from the multi part message, with resolved content references
    /// 3) ProcessMessage.AfterDeserialize
    ///      not used
    /// 4) Web Method is executed
    /// 5) ChainStream
    ///      provides access to the outgoing serialized stream buffer
    /// 6) ProcessMessage.BeforeSerialize
    ///      not used
    /// 7) ProcessMessage.AfterSerialize
    ///      not used
    ///</summary>
    //============================================================
    public override Stream ChainStream( Stream stream )
    {
      //save a reference to the input stream and the return stream as local 
      //member variables.  The return stream will be modified in the 
      //ProcessMessage.BeforeDeserialize stage to replace the original input
      //stream with a modified SOAP message stream.
      switch (stream.GetType().ToString())
      {
        case "System.Web.HttpInputStream": // we want to modify the input
          oldStream = stream;
          newStream = new MemoryStream();
          return newStream;
                                           // and avoid messing with the output
        case "System.Web.Services.Protocols.SoapExtensionStream": 
          return stream;
        default:
          throw new Exception ("Unknown Stream type in ChainStream!");
      }
    }

    //============================================================
    // Override: ProcessMessage
    //============================================================
    ///<summary>
    ///  ProcessMessage gets control at for stages during the 
    ///  processing of a SOAP request:
    ///  1) BeforeDeserialize: as the serialized request is received
    ///     via HTTP - before is is deserialized for invocation of the
    ///     web method
    ///  2) AfterDeserialize - after the request is serialized but
    ///     before the web method is invoked
    ///  3) BeforeSerialize - before the web method results are 
    ///     serialized for return to the caller via HTTP
    ///  4) AfterSerialize - after the web method results are serialized
    ///     for return to the caller via HTTP
    ///     
    ///  For PublishViaMIME, we are only interested in the BeforeDeserialize
    ///  stage.  We'll examine the incoming stream; if it is a multi part
    ///  MIME message we'll extract the SOAP envelope from the message,
    ///  resolve the references to other MIME parts (content), and replace
    ///  the request stream with the resolved SOAP envelope.
    ///</summary>
    //============================================================
    public override void ProcessMessage(SoapMessage message) 
    {
      switch (message.Stage) 
      {
        // Intercept the request before it is deserialized. Handle the
        // case where the SOAP envelope is part of a multi part MIME message.
        // Otherwise, let the requst pass through normal serialization.
        case SoapMessageStage.BeforeDeserialize:     
          if (isMultiPartMIME(message.ContentType))  // handle multi part MIME 
          {
            //extract the SOAP envelope from the MIME Message
            SOAPenvelope = getSOAPEnvelope(oldStream);
            //extract the encoded content from the MIME message and
            // insert the encoded content into the SOAP message
            resolveContentRefs(oldStream); 
            //replace the original (MIME) stream with the SOAP message
            replaceMessageStream(newStream);
            //make sure we reset the request's content type, since it
            //is no longer in "Multipart/Related" form
            message.ContentType = "text/xml";
          }
          else //if not a multi part message, pass the original stream through
          {
            reader = new StreamReader(oldStream);
            writer = new StreamWriter(newStream);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
          }
          newStream.Position = 0;
          break;

        case SoapMessageStage.AfterDeserialize:
          break;

        case SoapMessageStage.BeforeSerialize:
          break;

        case SoapMessageStage.AfterSerialize:
          break;

        default:
          throw new Exception("invalid stage in ProcessMessage!");
      }
    }

    //============================================================
    // isMultiPartMIME
    //============================================================
    ///<summary>
    ///    Given the Content-Type string from the header of the request,
    ///    determine if the request is in multi part MIME format.
    ///    If so, save the pertinent information from the Content-Type
    ///    string in member variables:
    ///      MIMEBoundary = the string that separates one content item
    ///                     from another
    ///      startContentID = the Content-ID of the 'root' content item
    ///                     (the root content item must be the SOAP message)
    ///                     
    ///    Sample Content-Type header string:
    ///    Multipart/Related; boundary=MIME_boundary; 
    ///               type=text/xml; start=..soapenvelope.xml.. 
    ///</summary>
    ///<param name='contentType'>The Content-Type header of the message.
    ///</param>
    ///<returns>(boolean) true if the Content-Type header indicates this is
    ///    a "Multipart/Related" (MIME) format message.  False otherwise.
    ///</returns>
    //============================================================
    private bool isMultiPartMIME(string contentType)
    {
      bool results = false;
      String[] contentTypeItems = contentType.Split(new Char[]{';'});
      foreach (String item in contentTypeItems)
      {
        String trimItem = item.Trim();
        if (trimItem.ToUpper() == "MULTIPART/RELATED")
        {
          results = true;
        } 
        if (trimItem.IndexOf("boundary=") >= 0) //MIMEBoundary member variable
        {
          MIMEBoundary = 
            trimItem.Substring(trimItem.IndexOf("boundary=") + 9).Trim();
        }
        if (trimItem.IndexOf("start=") >= 0)   //startContentID member variable
        {
          startContentID = ("Content-ID: " + 
            trimItem.Substring(trimItem.IndexOf("start=") + 6)).Trim();
        }
      }
      return results;
    }

    //============================================================
    // getSOAPEnvelope
    //============================================================
    ///<summary>
    ///    Given the HTTP request stream in multipart/related format,
    ///    extract the root content item, which contains the SOAP envelope
    ///    (with unresolved content references).   
    ///</summary>
    ///<param name='input'>The request stream.
    ///</param>
    ///<returns>(string) A string containing the SOAP envelope text, including
    ///    any unresolved content references to other parts of the multi part
    ///    message.
    ///</returns>
    //============================================================
    private string getSOAPEnvelope(Stream input)
    {
      string envelope = "";
      input.Position = 0;
      StreamReader sr = new StreamReader(input, System.Text.Encoding.Default, true);

      // Read through the (non null) lines of the incoming message
      // looking for the ContentID header equal to the start content id
      // the start content id identifies the soap envelope content item
      // NOTE - this logic assumes that the Content-ID header occurs right
      // before the encoded content.
      for (String Line = sr.ReadLine(); Line != null; Line = sr.ReadLine()) 
      {
        if (Line.IndexOf(startContentID) >= 0) //found the SOAP envelope
        {
          bool foundEndOfEnvelope = false;
          String envLine = sr.ReadLine();
          while (envLine != null && !foundEndOfEnvelope)
          {
            envelope = envelope + envLine;
            if (envLine.IndexOf("</soap:Envelope>") >= 0)
            {
              foundEndOfEnvelope = true;
            }
            envLine = sr.ReadLine();
          }
        }
      }
      input.Position = 0;
      return envelope;
    }

    //============================================================
    // resolveContentRefs
    //============================================================
    ///<summary>
    ///    Given the HTTP request stream in multipart/related format,
    ///    locate all the content items (other than the root
    ///    SOAP envelope), and replace content references in the SOAP
    ///    envelope with the matching content items.
    ///    NOTE - this logic assumes that a Content-ID header
    ///    immediately precedes the encoded content!
    ///</summary>
    ///<param name='MIMEstream'>The request stream.  Also depends on the 
    ///    SOAPEnvelope member variable, which is populated at this point.
    ///</param>
    ///<returns>(void) 
    ///    Thows exceptions for content items without corresponding
    ///    references in the SOAP envelope, and for references in the SOAP
    ///    envelope without corresponding content items in the stream.
    ///</returns>
    //============================================================
    private void resolveContentRefs(Stream MIMEstream)
    {
      string contID;
      string content;

      MIMEstream.Position = 0;
      StreamReader sr = 
        new StreamReader(MIMEstream, System.Text.Encoding.Default, true);

      // Read through the request stream, looking for the Content-ID headers
      // ignore the content of the root item (the soap envelope content)
      for (String Line = sr.ReadLine(); (Line != null) ; Line = sr.ReadLine()) 
      {
        // process every Content-ID *other* than the start Content-ID
        if ((Line.IndexOf("Content-ID: ") >= 0) && 
            (Line.IndexOf(startContentID) < 0))
        {
          //save the Content-ID value, and the base64 encoded content
          contID = Line.Substring(Line.IndexOf("Content-ID: ") + 12).Trim();
          //remove the starting and ending "<" and ">"
          if (contID.StartsWith("<")) contID = contID.Substring(1);
          if (contID.EndsWith(">")) contID = contID.Substring(0,contID.Length - 1);
          //get the encoded content immediately following the Content-ID header
          //(until the next MIME Boundary is reached)
          content = "";
          Line = sr.ReadLine();
          while ((Line != null) && (Line.IndexOf(MIMEBoundary) < 0))
          {
            content = content + Line;
            Line = sr.ReadLine();
          }
          insertContent(contID, content);
        }
      }
      // now that we've processed all the MIME 'attachments', look through
      // the envelope and make sure there are no remaining unresolved href's
      if (SOAPenvelope.IndexOf("href=\"cid:")>=0)
      {
        throw new Exception("Envelope contains unresolved content references");
      } 
      MIMEstream.Position = 0;  //reset the stream position
    }

    //============================================================
    // insertContent
    //============================================================
    ///<summary>
    ///    Given the contentID and encoded content from an item in
    ///    the multi part request stream, update the SOAPEnvelope.
    ///    Insert the encoded content at the point of the matching
    ///    reference attribute (href="cid:...") in the SOAP envelope.  
    ///    Reference attribute example:
    ///      <Data href="cid:MIMEtesttext.txt@paulsoftware.com"></Data>
    ///</summary>
    ///<param name='contentID'>The content ID of the item from the request 
    ///    stream.
    ///</param>
    ///<param name='content'>The encoded content from the request stream.
    ///</param>
    ///<returns>(void) 
    ///    Thows exceptions for content items without corresponding
    ///    references in the SOAP envelope.
    ///</returns>
    //============================================================
    private void insertContent(string contentID, string content)
    {
      string newSOAPEnvelope;
      // find the start of the href, and the length of the reference attribute
      int refStart = SOAPenvelope.IndexOf("href=\"cid:" + contentID);
      int refLength = contentID.Length + 11; // length includes close quote
      // if a corresponding reference was found, replace it with the content
      if (refStart >=0)
      { 
        // remove the href attribute (so we know we've resolved the reference)
        newSOAPEnvelope = SOAPenvelope.Substring(0,refStart) + 
                          SOAPenvelope.Substring(refStart + refLength);
        // insert data after the element closure (">") that contained the href
        int dataStart = newSOAPEnvelope.IndexOf(">",refStart) + 1;
        SOAPenvelope = newSOAPEnvelope.Substring(0,dataStart) + content +
                       newSOAPEnvelope.Substring(dataStart);
      }
      else
      {
        throw new Exception("Extraneous content: a reference to Content ID= " + 
                             contentID + " was not found in the SOAP request.");
      }
    }

    //============================================================
    // replaceMessageStream
    //============================================================
    ///<summary>
    ///    replaces a stream with a new stream containing the text found
    ///    in the SOAPEnvelope member variable.
    ///</summary>
    ///<param name='target'>The target stream.
    ///</param>
    ///<returns>(void) 
    ///</returns>
    //============================================================
    private void replaceMessageStream(Stream target)
    {
      target.Position = 0;
      TextWriter writer = new StreamWriter(target);
      // write the SOAPenvelope member variable contents to the target stream
      writer.WriteLine(SOAPenvelope);
      writer.Flush();
      target.Flush();
      // tidy up the target stream
      target.SetLength(target.Position);
      target.Position = 0;
    }
  }

  //============================================================
  // PublishViaMIMEAttribute Class
  //============================================================
  ///<summary>
  ///    Defines a SoapExtensionAttribute for our SOAP Extension so that
  ///    any Web Service method can be decorated as follows -
  ///        [PublishViaMIMEAttribute]
  ///    to enable this SOAP Extension.
  ///</summary>
  //============================================================
  [AttributeUsage(AttributeTargets.Method)]
  public class PublishViaMIMEAttribute : SoapExtensionAttribute 
  {
    private int priority;

    ///<summary>
    ///    Soap Extension Attribute ExtensionType property (override)
    ///</summary>
    public override Type ExtensionType 
    {
      get { return typeof(PublishViaMIME); }
    }

    ///<summary>
    ///    Soap Extension Attribute Priority property (override)
    ///</summary>
    public override int Priority 
    {
      get { return priority; }
      set { priority = value; }
    }
  }
}
