using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;

public partial class Board_write : System.Web.UI.Page
{
		public string mUrl;
		private string mArticleId;

    protected void Page_Load(object sender, EventArgs e)
    {
			Random r = new Random();
			string strRandomNum = r.Next(1, 100).ToString();
			mArticleId = strRandomNum; 
    }

}