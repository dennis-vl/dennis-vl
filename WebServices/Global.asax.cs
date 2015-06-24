using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace VanLeeuwen.Projects.WebPortal.WebServices
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      Properties.Settings settings = new Properties.Settings();

      ServiceSettings.LogPath = settings.LogPath;
      ServiceSettings.XmlReceiveFolder = settings.XMLReceiveFolder;
      ServiceSettings.XMLInboxRoot = settings.XMLInboxRoot;
			ServiceSettings.XMLOutboxRoot = settings.XMLOutboxRoot;
    }

    protected void Session_Start(object sender, EventArgs e)
    {

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_Error(object sender, EventArgs e)
    {

    }

    protected void Session_End(object sender, EventArgs e)
    {

    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
  }
}