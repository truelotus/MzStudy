using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBoardApplication.DataBase;
using System.Data;
using System.IO;

public partial class Board_Main : System.Web.UI.Page
{

	public void Page_Load(object sender, EventArgs e)
	{
		var requestUrl = Request.Url.AbsoluteUri;
	}

	public string GetArticleUrl(Article article) 
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?read={1}", portUrl, article.Id);
	}

	public IEnumerable<Article> GetList()
	{

		var dataSet = MsSqlDataBase.GetArticlesData();
		var list = new List<Article>();

		if (dataSet.Tables.Count > 0)
		{
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				var item = new Article();
				item.Id = row["ID"].ToString();
				item.No = row["NO"].ToString();
				item.Title = row["TITLE"].ToString();
				item.Contents = row["CONTENTS"].ToString();
				item.Writer = row["WRITER"].ToString();
				item.Date = row["DATE"].ToString();
				item.Password = row["PASSWORD"].ToString();
				item.Hits = row["HITS"].ToString();

				list.Add(item);
			}
		}
		return list;
	}
}