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
	public Article mArticle = new Article();

	public Comment mComment = new Comment();

	protected void Page_Load(object sender, EventArgs e)
	{
		var queryStr = Request.QueryString["read"];

		if (!String.IsNullOrEmpty(queryStr))
		{
			mArticle = GetArticleInfo(queryStr);
			GetUpdateArticleUrl(mArticle);
			return;
		}

		if (!String.IsNullOrEmpty(Request.QueryString["update"]))
		{
			mArticle = GetArticleInfo(queryStr);
		}

		if (!String.IsNullOrEmpty(Request.QueryString["updateCom"]))
		{
			//댓글 수정 요청 시 사용자가 수정을 할 수 있도록 댓글 작성 란에 정보를 보여준다.
			
			mComment = GetCommentInfo(Request.QueryString["updateCom"]);
			mArticle = GetArticleInfo(mComment.Article_Id);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["deleteCom"]))
		{
			//삭제다.
			var commentData = GetCommentInfo(Request.QueryString["deleteCom"].ToString());
			mArticle = GetArticleInfo(commentData.Article_Id);
			MsSqlDataBase.DeleteArticleCommentData(Request.QueryString["deleteCom"]);
			
			RedirectReadPage(commentData.Article_Id);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["getComment"]))
		{
			//수정 페이지에서 작성자가 수정 완료를 눌렀을 경우.
			var commentId = Request.QueryString["getComment"].ToString();
			UpdateArticleComment(commentId);
		}
		else
		{
			if (String.IsNullOrEmpty(Request.Params["id"]))
			{
				mComment = new Comment();
			}
			else
			{
				if (Request.Params["id"].Contains("commentId"))
				{
					mComment = new Comment();
				}
				else
				{
					//댓글 새로 작성 요청 왔다!
					mComment = new Comment()
					{
						Article_Id = Request.Params["id"].ToString(),
						Id = System.Guid.NewGuid().ToString(),
						Writer = Request.Params["writer"],
						Contents = Request.Params["contents"],
						Date = DateTime.Now.ToString(),
						No = (MsSqlDataBase.GetCommentDataCount(Request.Params["id"].ToString()) + 1).ToString(),
						Password = ""
					};
					var isSave = MsSqlDataBase.SetArticleComment(mComment);
					if (isSave)
					{
						RedirectReadPage(mComment.Article_Id);
					}
				}
			}

		}

	}

	public string GetReadPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?read={1}", portUrl, id);
	}

	/// <summary>
	/// 읽기페이지로 댓글 삭제 요청 한다.
	/// </summary>
	/// <param name="id"></param>
	public string GetDeleteCommentPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?deleteCom={1}", portUrl, id);
	}

	/// <summary>
	/// 읽기페이지로 댓글 수정 요청 한다.
	/// </summary>
	/// <param name="id"></param>
	public string GetUpdateCommentPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?updateCom={1}", portUrl, id);
	}

	/// <summary>
	/// 읽기페이지로 새 요청 날린다.
	/// </summary>
	/// <param name="id"></param>
	private void RedirectReadPage(string id)
	{
		Response.Redirect(String.Format("read.aspx?read={0}", id));
		Response.End();
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

	public void UpdateArticleComment(string commentId)
	{
		var comId = Request.QueryString["updateCom"];
		var commentData = GetCommentInfo(comId);

		string articleId = commentData.Article_Id;
		string id = commentData.Id;
		string no = commentData.No;
		string contents = commentData.Contents;
		string writer = commentData.Writer;
		string date = DateTime.Now.ToString();

		Comment comment = null;
		if (String.IsNullOrEmpty(id))
		{
			//Article 생성
			id = System.Guid.NewGuid().ToString();
			date = DateTime.Now.ToString();
			no = (MsSqlDataBase.GetCommentDataCount(articleId) + 1).ToString();
			comment = new Comment() { Article_Id = articleId, Id = id, No = no, Contents = contents, Writer = writer, Date = date, Password = null };

			MsSqlDataBase.SetArticleComment(comment);
		}
		else
		{
			//Article 수정
			comment = new Comment() { Article_Id = articleId, Id = id, No = no, Contents = contents, Writer = writer, Date = date, Password = null };
			MsSqlDataBase.UpdateCommentData(comment);
		}


		RedirectReadPage(articleId);
	}

	public Article GetArticleInfo(string id)
	{
		var article = new Article();
		var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
		var dataTbl = dataSet.Tables[MsSqlDataBase.DATA_TABLE_ARTICLE_INFORMATION];

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


	public Comment GetCommentInfo(string id)
	{
		var comment = new Comment();
		var dataSet = MsSqlDataBase.GetSelectedCommentData(id);
		var dataTbl = dataSet.Tables[MsSqlDataBase.DATA_TABLE_ARTICLE_COMMENT];

		if (dataSet.Tables.Count > 0)
		{
			foreach (DataRow dRow in dataTbl.Rows)
			{
				comment.Article_Id = dRow["ARTICLE_ID"].ToString();
				comment.Id = dRow["ID"].ToString();
				comment.No = dRow["NO"].ToString();
				comment.Contents = dRow["CONTENTS"].ToString();
				comment.Writer = dRow["WRITER"].ToString();
				comment.Date = dRow["DATE"].ToString();
				comment.Password = dRow["PASSWORD"].ToString();
			}
		}
		return comment;
	}



	public IEnumerable<Comment> GetComments(string articleId)
	{
		if (String.IsNullOrEmpty(articleId))
			return null;

		var dataSet = MsSqlDataBase.GetArticleComments(articleId);
		var list = new List<Comment>();

		if (dataSet.Tables.Count > 0)
		{
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				var item = new Comment();
				item.Article_Id = row["ARTICLE_ID"].ToString();
				item.Id = row["ID"].ToString();
				item.No = row["NO"].ToString();
				item.Contents = row["CONTENTS"].ToString();
				item.Writer = row["WRITER"].ToString();
				item.Date = row["DATE"].ToString();
				item.Password = row["PASSWORD"].ToString();

				list.Add(item);
			}
		}
		return list;
	}

}