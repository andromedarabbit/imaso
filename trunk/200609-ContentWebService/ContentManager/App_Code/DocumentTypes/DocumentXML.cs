using System;
using System.Xml;
using System.Configuration;

namespace ContentManager.DocumentTypes
{
//============================================================
// DocumentXML Class
//============================================================
///<summary>
///         The DocumentXML class validates XmlElement arguments
///         passed to PublishXML and saves the xml content to disk.
///                  
///         The xml content will be saved to the content directory
///         when the DocumentXML.Save method is invoked.
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
///  <change     date= "06/29/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking> 
///    <item summary=  "created class and SaveXML method."></item> 
///  </change>
///  <change     date= "07/01/2001"   name="Paula Paul">
///    <tracking build=""             issueIDs=""</tracking>
///    <item summary=  "Put validation logic into the constructor."></item>
///  </change>
///</changelog>
///</remarks>
//============================================================== 
	public class DocumentXML
	{
    private string FileName;    // member variable for the xml file name
    private XmlElement Content; // member variable for the content to be saved to disk
    
    //============================================================
    // DocumentXML constructor
    //============================================================
    ///<summary>
    ///    The DocumentXML constructor verifies that the xmlElement
    ///    passed as an argument contains a non-empty 'FileName' tag
    ///    and stores the contents of that element in the FileName
    ///    member variable of the class.  It also stores a copy of the
    ///    XmlElement argument in the 'Content' member variable.
    ///</summary>
    ///<param name='doc'>The SaveXML method takes one argument, of type
    ///    XmlElement.  The argument contains xml content to be saved to disk.
    ///</param>
    //============================================================
    public DocumentXML(XmlElement doc)
    {
      // use the XmlTextReader to read the XmlElement argument to the constructor
      XmlTextReader reader = new 
        XmlTextReader(doc.OuterXml.ToString(),System.Xml.XmlNodeType.Element,null);
      // Position the reader to the file name by reading to the <FileName> tag
      while(reader.Read())
      {
        if ("FileName" == reader.Name) break;
      }
      // if a <FileName> element was not found, the reader will be at EOF
      if (reader.EOF)
      {
        throw new Exception("The required 'FileName' element was not found");
      }
      // if a <FileName> element was found, read the file name and save the filename
      // and the raw xml to member variables
      else
      {
        FileName = reader.ReadElementString("FileName");
        // check to see that we have a non zero length file name 
        // (additionally, this function could check to see if the file already exists)
        if (FileName.Trim().Length > 0)
        {
          Content = doc;
        }
        else
        {
          throw new Exception("The required 'FileName' element was empty.");
        }
        reader.Close();
      }
    }

    //============================================================
    // DocumentXML.SaveXML
    //============================================================
    ///<summary>
    ///    The DocumentXML.SaveXML writes the xml content in the
    ///    'Content' member variable to disk, as a file with a name
    ///    specified in the 'FileName' member variable.  The 'Content'
    ///    and 'FileName' member variables are set by the DocumentXML
    ///    constructor.
    ///</summary>
    ///<param name='void'>The SaveXML method does not take arguments.
    ///</param>
    ///<returns>Returns void.  Exceptions will be caught
    ///         by the invoking web method (PublishXML).
    ///</returns>
    //============================================================
    public void SaveXML()
    {
      // create the output file and write the xml content to the file
      // NOTE: this will overwrite any existing file
      XmlTextWriter writer = 
        new XmlTextWriter(ConfigurationSettings.AppSettings.Get("ContentDirectory") 
                          + FileName, System.Text.Encoding.Default);
      writer.WriteRaw(Content.OuterXml.ToString());
      // Close the writer and underlying file.     
      writer.Close(); 
    }
	}
}
