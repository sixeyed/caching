using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sixeyed.Caching.Tests.Stubs.Website
{
    public partial class Page2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            timestampLabel.Text = "Server timestamp: " + DateTime.Now;
        }
    }
}