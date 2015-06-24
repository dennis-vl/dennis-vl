using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.Membership.OpenAuth;

namespace VanLeeuwen.Projects.WebPortal.UserInterface.Account
{
  public partial class Manage : BasePage
  {
    protected string SuccessMessage
    {
      get;
      private set;
    }

    protected void Page_Load()
    {
      if (!IsPostBack)
      {
        // Render success message
        var message = Request.QueryString["m"];
        if (message != null)
        {
          // Strip the query string from action
          Form.Action = ResolveUrl("~/Account/Manage");

          SuccessMessage =
              message == "ChangePwdSuccess" ? "Your password has been changed."
              : String.Empty;
          successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);

          changePassword.Visible = false;
        }
      }
    }

  }
}
