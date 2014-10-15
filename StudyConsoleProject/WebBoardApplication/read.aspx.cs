﻿using System;
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
		//
		if (!String.IsNullOrEmpty(Request.QueryString["read"]))
		{
			mArticle = GetArticleInfo(Request.QueryString["read"]);
		}
		else
		{
			SetDatabase(Request);
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

	public void SetDatabase(HttpRequest request)
	{
		//id 정보를 가지고 디비에 있는지 확인
		string num = request.Params["no"];
		if (MsSqlDataBase.HasArticleData(num))
		{
			//있다면 해당 데이터를 업데이트(제목,작성자,내용) 친다.
			string title = request.Params["title"];
			string contents = request.Params["contents"];
			string writer = request.Params["writer"];
			string date = DateTime.Now.ToString();
			mArticle = new Article() { No = num, Title = title, Contents = contents, Writer = writer, Password = null };
			//DB Set
			MsSqlDataBase.UpdateArticleData(mArticle);
		}
		else
		{
			//없다면 일반 데이터 추가이다.
			Random r = new Random();
			string strRandomNum = r.Next(1, 100).ToString();
			var id = strRandomNum;

			string title = request.Params["title"];
			string contents = request.Params["contents"];
			string writer = request.Params["writer"];
			string date = DateTime.Now.ToString();
			int no = MsSqlDataBase.GetDataBaseCount() + 1;
			mArticle = new Article() { Id = id, No = no.ToString(), Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };
			//DB Set
			MsSqlDataBase.SetArticleData(mArticle);
		}
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