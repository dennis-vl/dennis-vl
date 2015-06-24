using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface
{
  public partial class SiteMaster : MasterPage
  {
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;
    private static DALPortalDataContext dc;

    protected void Page_Init(object sender, EventArgs e)
    {
      // The code below helps to protect against XSRF attacks
      dc = new DALPortalDataContext();
      var requestCookie = Request.Cookies[AntiXsrfTokenKey];
      Guid requestCookieGuidValue;
      if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
      {
        // Use the Anti-XSRF token from the cookie
        _antiXsrfTokenValue = requestCookie.Value;
        Page.ViewStateUserKey = _antiXsrfTokenValue;
      }
      else
      {
        // Generate a new Anti-XSRF token and save to the cookie
        _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
        Page.ViewStateUserKey = _antiXsrfTokenValue;

        var responseCookie = new HttpCookie(AntiXsrfTokenKey)
        {
          HttpOnly = true,
          Value = _antiXsrfTokenValue
        };
        if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
        {
          responseCookie.Secure = true;
        }
        Response.Cookies.Set(responseCookie);
      }

      Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        // Set Anti-XSRF token
        ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
        ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
      }
      else
      {
        // Validate the Anti-XSRF token
        if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
            || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
        {
          throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
        }
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        DropDownListItem dropDownListItem = new DropDownListItem();
        dropDownListItem.Text = "ENG";
        dropDownListItem.Value = "en-US";
        RadDropDownListLanguage.Items.Add(dropDownListItem);

        dropDownListItem = new DropDownListItem();
        dropDownListItem.Text = "FR";
        dropDownListItem.Value = "fr-FR";
        RadDropDownListLanguage.Items.Add(dropDownListItem);

        dropDownListItem = new DropDownListItem();
        dropDownListItem.Text = "NL";
        dropDownListItem.Value = "nl-NL";
        RadDropDownListLanguage.Items.Add(dropDownListItem);

        DropDownListItem selectedDropDownListItem = null;

        if (Session["CurrentLanguage"] != null)
          selectedDropDownListItem = RadDropDownListLanguage.FindItemByValue(Session["CurrentLanguage"].ToString());
        else
          selectedDropDownListItem = RadDropDownListLanguage.FindItemByValue("en-US");

        RadDropDownListLanguage.SelectedIndex = selectedDropDownListItem.Index;

        //HttpCookie httpCookie = Request.Cookies["CurrentLanguage"];
        //if (httpCookie != null && httpCookie.Value != null)
        //  selectedDropDownListItem = RadDropDownListLanguage.FindItemByValue(httpCookie.Value);
        //else
        //  selectedDropDownListItem = RadDropDownListLanguage.FindItemByValue("en-US");

        //RadDropDownListLanguage.SelectedIndex = selectedDropDownListItem.Index;
      }
    }

    protected void LoginStatusMain_LoggingOut(object sender, LoginCancelEventArgs e)
    {
      Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void RadDropDownListLanguage_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
      DropDownListItem selectedDropDownListItem = RadDropDownListLanguage.SelectedItem;
      switch (selectedDropDownListItem.Value)
      {
        case "fr-FR":
          Session["CurrentLanguage"] = "fr-FR";
          break;
        case "nl-NL":
          Session["CurrentLanguage"] = "nl-NL";
          break;
        default:
          Session["CurrentLanguage"] = "en-US";
          break;
      }

      Response.Redirect(Request.RawUrl);
    }

    protected void RadMenuMain_DataBound(object sender, EventArgs e)
    {
      string redirectUrl = "~/Pages/UnAuthorised.aspx";
      RadMenu map = (RadMenu)sender;

      IList<RadMenuItem> siteMapList = map.GetAllItems();
      MembershipUser member = Membership.GetUser();

      string id = member.ProviderUserKey.ToString().ToLower();

      List<userRole> roles = dc.userRoles.Where(c => c.userId.ToString().ToLower().Equals(id)).ToList();
      String currentUrl = HttpContext.Current.Request.Url.AbsolutePath;





      Guid idUser = (Guid)member.ProviderUserKey;

      userSetting setting = dc.userSettings.Where(c => c.userId.Equals(idUser)).Select(c => c).SingleOrDefault();

      int businessPartnerId = dc.contactPersons.Where(c => c.userId.Equals(idUser)).Select(c => c.businessPartnerId).SingleOrDefault();

      if (setting == null)
      {
        var siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/UserAccounts.aspx")).FirstOrDefault();
        if (siteMapNode != null)
        {
          siteMapNode.Remove();
          if (currentUrl.Contains("UserAccounts.aspx"))
          {
            Response.Redirect(redirectUrl);
          }
        }
      }
      else if (setting.siteAdmin == false)
      {
        var siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/UserAccounts.aspx")).FirstOrDefault();
        if (siteMapNode != null)
        {
          siteMapNode.Remove();
          if (currentUrl.Contains("UserAccounts.aspx"))
          {
            Response.Redirect(redirectUrl);
          }
        }

        List<String> DeliveryDetailsRoles = new List<string>() { "CERT_USER", "CERT_VLUSER", "CERT_ACTMNG" };
        if (!roles.Any(c => DeliveryDetailsRoles.Contains(c.roleCode)))
        {
          siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/DeliveryDetails.aspx")).FirstOrDefault();
          if (siteMapNode != null)
          {
            siteMapNode.Remove();
            if (currentUrl.Contains("DeliveryDetails.aspx"))
            {
              Response.Redirect(redirectUrl);
            }
          }
        }
        roleCheck(map);
      }
      //CERT_ACTMNG



    }

    protected void roleCheck(RadMenu map)
    {
      String redirectUrl = "~/Pages/UnAuthorised.aspx";
      String currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

      MembershipUser member = Membership.GetUser();
      String userId = member.ProviderUserKey.ToString().ToLower();

      IList<RadMenuItem> siteMapList = map.GetAllItems();

      List<String> roles = dc.userRoles.Where(c => c.userId.ToString().ToLower().Equals(userId)).Select(c => c.roleCode).ToList();

      if (!roles.Contains("CERT_USER") && !roles.Contains("CERT_VLUSER") && !roles.Contains("CERT_ACTMNG"))
      {
        var siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/DeliveryDetails.aspx")).FirstOrDefault();
        if (siteMapNode != null)
        {
          siteMapNode.Remove();
          if (currentUrl.Contains("DeliveryDetails.aspx"))
          {
            Response.Redirect(redirectUrl);
          }
        }
      }

      if (!roles.Contains("CERT_ACTMNG"))
      {
        var siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/CustomerAccounts.aspx")).FirstOrDefault();
        if (siteMapNode != null)
        {
          siteMapNode.Remove();
          if (currentUrl.Contains("CustomerAccounts.aspx"))
          {
            Response.Redirect(redirectUrl);
          }
        }
      }

      if (!roles.Contains("DOP_USER") && !roles.Contains("DOP_ADMIN"))
      {
        var siteMapNode = siteMapList.Where(c => c.NavigateUrl.EndsWith("/Pages/DOP/DOPDetails.aspx")).FirstOrDefault();
        if (siteMapNode != null)
        {
          siteMapNode.Remove();
          if (currentUrl.Contains("DOPDetails.aspx"))
          {
            Response.Redirect(redirectUrl);
          }
        }
      }
    }
  }
}