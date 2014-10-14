using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;

public partial class Board_write : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


		public void SetDatabase() 
		{
			string title = Request.Form["title"];
			string contents = Request.Form["contents"];

			Random r = new Random();

			string writer = "User" + r.Next(1, 100).ToString();
			string date = DateTime.Now.ToString();
			Article article = new Article() { Id = r.Next(1, 100).ToString(), No = "1", Title = title, Contents = contents, Writer = writer, Date = date, Password = null };

			MsSqlDataBase.SetArticleData(article);
		}


}