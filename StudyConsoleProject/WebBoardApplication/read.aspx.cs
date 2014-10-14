using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBoardApplication.DataBase;
using System.Data;

public partial class Board_Read : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
			var requestUrl = Request.Url.AbsoluteUri;
			if (!String.IsNullOrEmpty(Request.QueryString["read"]))
			{
				var article = GetArticleInfo(Request.QueryString["read"]);
			
				//Request.Form["title"] = article.Title;
				//Request.Form["contents"] = article.Contents;
				//Request.Form["writer"] = article.Writer;
			}
    }

		public Article GetArticleInfo(string id) 
		{
			var article = new Article();
			var dataSet = MsSqlDataBase.GetSelectedArticleData(id);

			if (dataSet.Tables.Count > 0)
			{

				foreach (DataRow row in dataSet.Tables[0].Rows)
				{
					article.Id = row["ID"].ToString();
					article.No = row["NO"].ToString();
					article.Title = row["TITLE"].ToString();
					article.Contents = row["CONTENTS"].ToString();
					article.Writer = row["WRITER"].ToString();
					article.Date = row["DATE"].ToString();
					article.Password = row["PASSWORD"].ToString();
					article.Hits = row["HITS"].ToString();
				}
			}

			return article;
		}
}