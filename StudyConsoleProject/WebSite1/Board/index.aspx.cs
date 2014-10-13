using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Board_Main : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
			var url = Request.Url.AbsoluteUri;

			string requestUrl = null;


			if (String.IsNullOrEmpty(requestUrl = Request.QueryString["write"]))
			{

			}
			else if (String.IsNullOrEmpty(requestUrl = Request.QueryString["read"]))
			{

			}
			else if (String.IsNullOrEmpty(requestUrl = Request.QueryString["index"]))
			{

			}

			
			
    }

		public void Write(string url) 
		{

		}
}