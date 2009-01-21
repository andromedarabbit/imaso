using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace ContentManager 
{
  /// <summary>
  /// Summary description for Global.
  /// </summary>
  public class Global : System.Web.HttpApplication
  {
    ///<summary>
    ///    Application_Start: empty
    ///</summary>
    protected void Application_Start(Object sender, EventArgs e)
    {
    }
 
    ///<summary>
    ///    Session_Start: empty
    ///</summary>
    protected void Session_Start(Object sender, EventArgs e)
    {
    
    }
  
    ///<summary>
    ///    Application_BeginRequest: uncomment lines as needed to view 
    ///    request data (saved to file)
    ///</summary>
    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
      // write the incoming request to disk if desired
      // Request.SaveAs("C:\\_request.txt",true);
    }
    
      
    ///<summary>
    ///    Application_EndRequest: uncomment lines as needed to view 
    ///    response data (saved to file)
    ///</summary>
    protected void Application_EndRequest(Object sender, EventArgs e)
    {
      // write the response data to disk if desired (NOTE this file must exist!)
      // Response.WriteFile("C:\\_response.txt");
    }
    
    ///<summary>
    ///    Session_End: empty
    ///</summary>
    protected void Session_End(Object sender, EventArgs e)
    {
    }

    ///<summary>
    ///    Application_End: empty
    ///</summary>
    protected void Application_End(Object sender, EventArgs e)
    {
    }
  }
}

