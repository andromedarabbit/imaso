<%@ Application Language="C#" %>

<object id="globalBot" runat="server" class="AIMLbot.Bot" scope="application"/>
<object id="userHash" runat="server" class="System.Collections.Hashtable" scope="application" />
<object id="sessionHash" runat="server" class="System.Collections.Hashtable" scope="application" />

<script runat="server">
    
    void Application_Start(object sender, EventArgs e) 
    {
        try
        {
            // Code that runs on application startup
            AIMLWebService.UserSession.Timeout = Convert.ToDouble(ConfigurationSettings.AppSettings["timeout"]);

            string configFilePath = Server.MapPath("");
            configFilePath = System.IO.Path.Combine(configFilePath, @"..\..\Shared\config\Settings.xml");
            configFilePath = System.IO.Path.GetFullPath(configFilePath);

            // globalBot.loadSettings(System.IO.Path.Combine(Server.MapPath(""), System.IO.Path.Combine(@"..\..\Shared\config", "Settings.xml")));
            
            globalBot.loadSettings(configFilePath);
            // globalBot.loadSettings(@"C:\Inetpub\wwwroot\AIMLWebService\config\Settings.xml");
            globalBot.loadAIMLFromFiles();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
