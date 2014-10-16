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
		var requestUrl = Request.Url.AbsoluteUri;
		var queryStr = Request.QueryString["read"];
		if (!String.IsNullOrEmpty(queryStr))
			mArticle = GetArticleInfo(Request.QueryString["read"]);
		else
		{
			queryStr = Request.QueryString["update"];

			UpdateArticleDataBase();

		}
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

	public void UpdateArticleDataBase()
	{
		var request = Request;
		string id = request.Params["id"];
		//id 정보를 가지고 디비에 있는지 확인
		if (MsSqlDataBase.HasArticleData(id))
		{
			//있다면 해당 데이터를 업데이트(제목,작성자,내용) 친다.
			
			string title = request.Params["title"];
			string contents = request.Params["contents"];
			string writer = request.Params["writer"];
			string date = DateTime.Now.ToString();
			mArticle = new Article() { Id = id, Title = title, Contents = contents, Writer = writer, Password = null };
			//DB Set
			MsSqlDataBase.UpdateArticleData(mArticle);
		}
		else
		{
			InsertNewArticleDatabase();
		}
	}

	public void InsertNewArticleDatabase()
	{
			var request = Request;
			var id = System.Guid.NewGuid().ToString();
			string title = request.Params["title"];
			string contents = request.Params["contents"];
			string writer = request.Params["writer"];
			string date = DateTime.Now.ToString();
			int no = MsSqlDataBase.GetDataBaseCount() + 1;
			mArticle = new Article() { Id = id, No = no.ToString(), Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };
			//DB Set
			MsSqlDataBase.SetArticleData(mArticle);
	}


	public Article GetArticleInfo(string id)
	{
		var article = new Article();
		var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
		var dataTbl = dataSet.Tables["ARTICLE_INFO"];

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