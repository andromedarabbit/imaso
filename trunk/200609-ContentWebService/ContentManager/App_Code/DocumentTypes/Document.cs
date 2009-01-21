using System;
using System.Xml;
using System.IO;
using System.Configuration;

namespace ContentManager.DocumentTypes
{
//============================================================
// Document Class
//============================================================
///<summary>
///         The Document class is the data type of arguments passed
///         to the Publish web service.  Publish accepts a Document
///         array, iterates over each Document item in the array, and
///         invokes each Document item's Save method to save the binary
///         document to disk.
///         Each Document item consists of meta data properties: Title,
///         Author, Abstract and File Name, plus a string containing 
///         the base64 encoded binary content that will be saved to the
///         content directory when the Document.Save method is invoked.
///         The content directory is specified in the Web.config file,
///         as an AppSetting.
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
///    <item summary=  "created Save method"></item> 
///  </change>
///  <change     date= "06/29/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking>
///    <item summary=  "changed Save method to use Web.Config file
///                     for content directory."></item>
///  </change>
///</changelog>
///</remarks>
//============================================================== 
  public class Document
  {
    //============================================================
    // Document constructor (empty)
    //============================================================
    public Document()
    {
    }
    // Document meta data that could be stored in a database or index
    // (document meta data is only provided as an example and is not processed)
    public string Author;
    public string Title;
    public string Abstract;
    public string[] Categories;

    // the file name (including application extension) used to save the binary file
    // in the target Content Directory
    public string FileName;

    // the base64 encoded binary data
    public string Data;

    //============================================================
    // Document.Validate method
    //============================================================
    ///<summary>
    ///    The Document.Validate method enforces any business
    ///    logic for required metadata or content.
    ///</summary>
    ///<param name='void'>The Validate method does not take arguments.
    ///</param>
    ///<returns>Returns void.  Exceptions may be thrown for invalid
    ///    metadata values (e.g. blank FileName).  Exceptions are
    ///    processed in the calling web method.
    ///</returns>
    //============================================================
    public void Validate()
    {
      // Validate the metadata (easily extended for more complex rules!)
      if (FileName.Trim().Length == 0)
      {
        throw new Exception("FileName must be non-blank.");
      }
    }

    //============================================================
    // Document.Save method
    //============================================================
    ///<summary>
    ///    The Document.Save method decodes the base64 encoded data
    ///    in the 'Data' property of the Document and saves the 
    ///    decoded data in the Content Directory, as a file with the
    ///    file name specified in the Document's FileName property.
    ///    This method could be enhanced to also store the other 
    ///    document meta data in a database or other index.
    ///</summary>
    ///<param name='void'>The Save method does not take arguments.
    ///</param>
    ///<returns>Returns void.  Exceptions may be thrown for 'file exists'
    ///         or invalid base64 data.  These Exceptions will be caught
    ///         by the invoking web method (Publish).
    ///</returns>
    //============================================================
    public void Save()
    {
      int base64len = 0;
      byte[] base64 = new byte[1000];

      // create a string Xml fragment called <Content></Content> and place the 
      // base64 encoded data in the fragment
      string dataElement = "<Content>" + Data + "</Content>";

      // use the XmlTextReader to read the fragment, and use its ReadBase64 method
      // to decode the data
      XmlTextReader reader = 
        new XmlTextReader(dataElement,System.Xml.XmlNodeType.Element,null);

      // Position the reader to the base64 data by reading to the <Content> tag
      while(reader.Read())
      {
        if ("Content" == reader.Name) break;
      }
      // we are positioned at the Base64 encoded content now, so:
      // 1) create the output file 
      //    (NOTE: if you pass zero length content, a zero length file is created)
      FileStream fs = 
        new FileStream(ConfigurationSettings.AppSettings.Get("ContentDirectory") 
                       + this.FileName, FileMode.CreateNew, FileAccess.Write);
      // 2) Create a binary writer, to write the decoded binary data to the file
      BinaryWriter w = new BinaryWriter(fs);  
      // 3) read the first chunk of the content
      base64len = reader.ReadBase64(base64,0,50);
      while (0 != base64len)
      {
        w.Write(base64,0,base64len);
        base64len = reader.ReadBase64(base64,0,50);
      }
      // Close the Reader
      reader.Close();
      // Close the writer and underlying file.     
      w.Close(); 
    }
	}
}
