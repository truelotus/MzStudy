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
	public Article mArticle;

	protected void Page_Load(object sender, EventArgs e)
	{
		//var requestUrl = Request.Url.AbsoluteUri;
		var queryStr = Request.QueryString["read"];

		if (!String.IsNullOrEmpty(queryStr)) 
			mArticle = GetArticleInfo(queryStr);
			
	}

	public string GetDeleteArticleUrl(Article article)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/Default.aspx?delete={1}", portUrl, article.Id);
	}

	public string GetUpdateArticleUrl(Article article)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/write.aspx?update={1}", portUrl, article.Id);
	}
	
	public Article GetArticleInfo(string id)
	{
		var article = new Article();

		MsSqlDataBase.UpdateHits(id);

		var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
		var dataTbl = dataSet.Tables[MsSqlDataBase.DB_TABLE_NAME];

		if (dataSet.Tables.Count > 0)
		{

			foreach (DataRow dRow in dataTbl.Rows)
			{
				article.Id = dRow["ID"].ToString();
				article.No = dRow["NO"].ToString();
				article.Title = dRow["TITLE"].ToString();
				article.Contents = dRow["CONTENTS"].ToString();
				article.Writer = dRow["WRITER"].ToString();
				article.Date = dRow["DATE"].ToString();
				article.Password = dRow["PASSWORD"].ToString();
				article.Hits = dRow["HITS"].ToString();
			}
		}
		return article;
	}

}