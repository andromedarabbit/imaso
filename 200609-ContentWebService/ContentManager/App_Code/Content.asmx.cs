using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Text;
using System.Configuration;
using ContentManager.DocumentTypes;


namespace ContentManager
{
//============================================================
// Content Web Service
//============================================================
///<summary>
///         The Content Web Service exposes public methods for
///         publishing content.  The service illustrates three
///         ways of publishing documents via three web methods:
///         1) Passing XML documents as method arguments 
///            (the PublishXML method)
///         2) Passing encoded binary documents as method arguments 
///            (the Publish method)
///         3) Passing the method request (the SOAP envelope)
///            along with the binary documents as separate 
///            parts of multi part MIME message.  This approach
///            relies on a SOAP extension to process the multi
///            part MIME message.
///            (the PublishMIME method)
///         Documents are published to the 'ContentDirectory'
///         specified in the web.Config file (under AppSettings).
///</summary>
///
///<remarks>
///<author name="Paula Paul"
///        company="Paul Software, Inc.
///        email="Paula@PaulSoftware.com">
///</author>
///
///<changelog>
///  <change     date= "06/28/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking>
///    <item summary=  "created class"></item>  
///    <item summary=  "created PublishXML and Publish methods"></item>
///    <item summary=  "added Document class for Publish method"></item> 
///  </change>
///  <change     date= "06/29/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking>
///    <item summary=  "modified Web.config to include AppSettings for
///                     configuring published documents directory."></item>
///    <item summary=  "moved logic from PublishXML to DocumentXML
///                     class"></item>                    
///  </change>
///  <change     date= "06/30/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking>
///    <item summary=  "added PublishMIME method."></item>
///    <item summary=  "decorated web service and web methods."></item>
///  </change>
///</changelog>
///</remarks>
//============================================================== 
[WebService(Namespace="urn:webservices-paulsoftware-com-content",
          Description="The Content Web Service illustrates three approaches for " +
                      "publishing content via web methods.")]
public class Content : System.Web.Services.WebService
{
    /// <summary>
    /// Content Web Service constructor.
    /// </summary>
    public Content()
    {
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
    }

    //============================================================
    // Content.PublishXML web method
    //============================================================
    ///<summary>
    ///    The PublishXML method saves XML documents to the Content Directory.
    ///</summary>
    ///<param name='contentItems'>The Publish method accepts one argument:
    ///    an array of XmlElements.  Each XmlElement is treated as a separate
    ///    document and is stored in the content directory as a file having a 
    ///    file name specified in the first 'FileName' tag found within the
    ///    document.  If no 'FileName' tag is found, an error message is
    ///    returned.
    ///</param>
    ///<returns>Returns a string describing the results of the request.
    ///    The string will either contain a 'success' message, or a message
    ///    resulting from an exception thrown during processing.
    ///</returns>
    //============================================================
    [SoapDocumentMethodAttribute(Action="PublishXML",
                                 ResponseElementName="PublishXMLResponse")]
    [WebMethod(Description=
               "The PublishXML method accepts an array of 'XmlElements' as an " +  
               "argument.  Each XmlElement can contain complex xml content " +
               "and must contain a <FileName> element.  Documents are " +
               "published in a directory specified in the Web.config file.")]
    public string PublishXML(XmlElement[] contentItems)
    {
      string results = contentItems.Length.ToString() + " documents published!";
      try
      {
        foreach (XmlElement docXML in contentItems)
        {
          // validate the xml argument via the DocumentXML object
          DocumentXML doc = new DocumentXML(docXML); 
          // if no exceptions were thrown, save the xml file
          doc.SaveXML();                              
        }
      }
      catch (Exception e)
      {
        results = "Error: " + e.Message;
      }
      return results;
    }

    //============================================================
    // Content.Publish web method
    //============================================================
    ///<summary>
    ///    The Publish method accepts an array of Documents and saves
    ///    the binary content for each item to the Content directory.
    ///</summary>
    ///<param name='contentItems'>The Publish method accepts one argument:
    ///    an array of Document objects.  The structure of a Document object
    ///    is described in the Document class (Document.cs).
    ///</param>
    ///<returns>Returns a string describing the results of the request.
    ///    The string will either contain a 'success' message, or a message
    ///    resulting from an exception thrown during processing.
    ///</returns>
    //============================================================
    [PublishViaMIME]
    [SoapDocumentMethodAttribute(Action="Publish",
                                 ResponseElementName="PublishResponse")]
    [WebMethod(Description=
               "The Publish method accepts an array of 'document objects' as an " +
               "argument.  Each document consists of a base64 encoded content item " +  
               "plus additional meta data.  Documents are published in a directory " +
               "specified in the Web.config file.")]
    public string Publish(Document[] contentItems)
    {
      string results = contentItems.Length.ToString() + " documents published!";
      try
      {
        foreach (Document doc in contentItems)
        {
          doc.Validate(); // Validate metadata
          doc.Save();     // Ask each content item to save itself to disk
        }
      }
      catch (Exception e)
      {
        results = "Error: " + e.Message;
      }
      return results;
    }
  }
}
