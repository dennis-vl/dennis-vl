using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace VanLeeuwen.Projects.WebPortal.UserInterface
{
  public class BasePage : Page
  {
    //Check if cookie exists, then change our website culture to the desired language
    //else set our website culture to the default language (EN) here and 
    //create the cookie with this value

    protected override void InitializeCulture()
    {
      CultureInfo Cul = null;

      if (Session["CurrentLanguage"] != null)
        Cul = CultureInfo.CreateSpecificCulture(Session["CurrentLanguage"].ToString());
      else
        Cul = CultureInfo.CreateSpecificCulture("en-US");

      System.Threading.Thread.CurrentThread.CurrentUICulture = Cul;
      System.Threading.Thread.CurrentThread.CurrentCulture = Cul;
      
      //string lang = string.Empty;
      //HttpCookie cookie = Request.Cookies["CurrentLanguage"];

      //if (cookie != null && cookie.Value != null)
      //{
      //  lang = cookie.Value;
      //  CultureInfo Cul = CultureInfo.CreateSpecificCulture(lang);

      //  System.Threading.Thread.CurrentThread.CurrentUICulture = Cul;
      //  System.Threading.Thread.CurrentThread.CurrentCulture = Cul;
      //}
      //else
      //{
      //  if (string.IsNullOrEmpty(lang)) lang = "en-US";
      //  CultureInfo Cul = CultureInfo.CreateSpecificCulture(lang);

      //  System.Threading.Thread.CurrentThread.CurrentUICulture = Cul;
      //  System.Threading.Thread.CurrentThread.CurrentCulture = Cul;

      //  HttpCookie cookie_new = new HttpCookie("CurrentLanguage");
      //  cookie_new.Value = lang;
      //  Response.SetCookie(cookie_new);
      //}

      base.InitializeCulture();
    }
  }
}
