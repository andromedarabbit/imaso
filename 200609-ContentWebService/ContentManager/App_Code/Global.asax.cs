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
    public static string debuginfo;
    protected void Application_Start(Object sender, EventArgs e)
    {
    
    }
    
    protected void Session_Start(Object sender, EventArgs e)
    {
    
    }
    
    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
      // write the incoming request to disk if desired
      // Request.SaveAs("C:\\_request.txt",true);
    }
    
    protected void Application_EndRequest(Object sender, EventArgs e)
    {
      // write the response data to disk if desired (NOTE this file must exist!)
      // Response.WriteFile("C:\\_response.txt");
    }
    
    protected void Session_End(Object sender, EventArgs e)
    {
    
    }
    
    protected void Application_End(Object sender, EventArgs e)
    {
    
    }
  }
}

