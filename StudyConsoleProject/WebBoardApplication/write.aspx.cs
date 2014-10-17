using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;
using System.Data;
using System.Web;

public partial class Board_write : System.Web.UI.Page
{
	public Article mArticle;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request != null)
		{
			if (!String.IsNullOrEmpty(Request.Params["title"]))
			{
				//새 게시물을 등록
				mArticle = InsertNewArticleDatabase();
			}
		}

		if (String.IsNullOrEmpty(Request.QueryString["update"]))
		{
			if (mArticle == null)
				mArticle = new Article();
		}
		else
		{
			//게시 글 업데이트.
			SetUpdateArticle(Request.QueryString["update"].Split('=')[0]);
		}
	}

	public Article InsertNewArticleDatabase()
	{
		var request = Request;
		Article article = null;
		string id = request.Params["id"];
		string no = request.Params["no"];
		string title = request.Params["title"];
		string contents = request.Params["contents"];
		string writer = request.Params["writer"];
		string date = DateTime.Now.ToString();

		if (String.IsNullOrEmpty(id))
		{
			//생성
			id = System.Guid.NewGuid().ToString();
			date = DateTime.Now.ToString();
			article = new Article() { Id = id, No = no, Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };

			MsSqlDataBase.SetArticleData(article);
			RedirectReadPage(article.Id);
		}
		else
		{
			//수정
			article = new Article() { Id = id, No = no, Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };
			MsSqlDataBase.UpdateArticleData(article);
			RedirectReadPage(article.Id);
		}

		return article;
	}

	private void RedirectReadPage(string id)
	{
		Response.Redirect(String.Format("read.aspx?read={0}", id));
		Response.End();
	}

	/// <summary>
	/// 게시글 정보를 UI에 뿌린다.
	/// </summary>
	/// <param name="id"></param>
	public void SetUpdateArticle(string id)
	{
		mArticle = new Article();
		mArticle.Id = id;
		var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
		var dataTbl = dataSet.Tables["ARTICLE_INFO"];
		if (dataSet.Tables.Count > 0)
		{
			foreach (DataRow dRow in dataTbl.Rows)
			{
				mArticle.No = dRow["NO"].ToString();
				mArticle.Title = dRow["TITLE"].ToString();
				mArticle.Contents = dRow["CONTENTS"].ToString();
				mArticle.Writer = dRow["WRITER"].ToString();
			}
		}
	}

	public string GetArticleUrl(string id)
	{
		return String.Format("read.aspx?update={0}", id);
	}
}