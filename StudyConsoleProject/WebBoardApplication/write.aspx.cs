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
				//새 게시물을 등록한다.
				InsertNewArticleDatabase();
			}

			if (String.IsNullOrEmpty(Request.QueryString["update"]))
			{
				if (mArticle == null)
					mArticle = new Article();
			}
			else
			{
				//ID를 가지고 게시 글 업데이트한다.
				SetUpdateArticle(Request.QueryString["update"]);
			}
		}
	}


	public void InsertNewArticleDatabase()
	{
		var request = Request;
		
		string id = request.Params["id"];
		string no = request.Params["no"];
		string title = request.Params["title"];
		string contents = request.Params["contents"];
		string writer = request.Params["writer"];
		string hits = request.Params["hits"];
		string date = DateTime.Now.ToString();

		Article article = null;
		if (String.IsNullOrEmpty(id))
		{
			//Article 생성
			id = System.Guid.NewGuid().ToString();
			date = DateTime.Now.ToString();
			no = (MsSqlDataBase.GetArticleDataCount() + 1).ToString();
			article = new Article() { Id = id, No = no, Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };

			MsSqlDataBase.SetArticleData(article);
		}
		else
		{
			//Article 수정
			article = new Article() { Id = id, No = no, Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = hits };
			MsSqlDataBase.UpdateArticleData(article);		
		}

		RedirectReadPage(article.Id);
	}

	/// <summary>
	/// 읽기페이지로 새요청 날린다.
	/// </summary>
	/// <param name="id"></param>
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
		var dataTbl = dataSet.Tables[MsSqlDataBase.DATA_TABLE_ARTICLE_INFORMATION];
		if (dataSet.Tables.Count > 0)
		{
			foreach (DataRow dRow in dataTbl.Rows)
			{
				mArticle.No = dRow["NO"].ToString();
				mArticle.Title = dRow["TITLE"].ToString();
				mArticle.Contents = dRow["CONTENTS"].ToString();
				mArticle.Writer = dRow["WRITER"].ToString();
				mArticle.Hits = dRow["HITS"].ToString();
			}
		}
	}
}