using System;
using System.Collections.Generic;
using System.Linq;

public partial class Board_write : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

		public void SetDatabase() 
		{
			string title = String.Format("{0}", Request.Form["title"]);
			string contents = String.Format("{0}", Request.Form["contents"]);
			string writer = "";
			string date = DateTime.Now.ToString();
		}
}