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

		if (!String.IsNullOrEmpty(Request.QueryString["reqCommentUpdate"]))
		{
			queryStr = Request.QueryString["reqCommentUpdate"].ToString();
			//댓글 수정 요청 시 사용자가 수정을 할 수 있도록 댓글 작성 란에 정보를 보여준다.
			mComment = GetCommentInfo(queryStr);
			mArticle = GetArticleInfo(mComment.Article_Id);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["reqCommentDelete"]))
		{
			//댓글 삭제 요청.
			queryStr = Request.QueryString["reqCommentDelete"].ToString();
			var commentData = GetCommentInfo(queryStr);
			mArticle = GetArticleInfo(commentData.Article_Id);
			MsSqlDataBaseManager.DeleteArticleCommentData(queryStr);

			RedirectReadPage(commentData.Article_Id);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["getComment"]))
		{
			//수정 페이지에서 작성자가 수정 완료를 눌렀을 경우. 업데이트한다.
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
				//댓글 새로 작성 요청 왔다!
				mComment = new Comment()
				{
					Article_Id = Request.Params["id"].ToString(),
					Id = System.Guid.NewGuid().ToString(),
					Writer = Request.Params["writer"],
					Contents = Request.Params["contents"],
					Date = DateTime.Now.ToString(),
					No = (MsSqlDataBaseManager.GetCommentDataCount(Request.Params["id"].ToString()) + 1).ToString(),
					Password = ""
				};
				var isSave = MsSqlDataBaseManager.SetArticleComment(mComment);
				if (isSave)
					RedirectReadPage(mComment.Article_Id);
			}
		}
	}


	public string GetReadPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?read={1}", portUrl, id);
	}

	/// <summary>
	/// 댓글 삭제 요청(read.aspx) 한다.
	/// </summary>
	/// <param name="id"></param>
	public string GetDeleteCommentPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?reqCommentDelete={1}", portUrl, id);
	}

	/// <summary>
	/// 댓글 수정 요청(read.aspx) 한다.
	/// </summary>
	/// <param name="id"></param>
	public string GetUpdateCommentPageUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?reqCommentUpdate={1}", portUrl, id);
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

	/// <summary>
	/// 게시글 삭제 요청 url을 반환한다.
	/// </summary>
	/// <param name="id">article id</param>
	/// <returns></returns>
	public string GetDeleteArticleUrl(string id)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/Default.aspx?delete={1}", portUrl, id);
	}

	/// <summary>
	/// 게시글 업데이트 요청 url을 반환한다.
	/// </summary>
	/// <param name="article">article</param>
	/// <returns></returns>
	public string GetUpdateArticleUrl(Article article)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/write.aspx?update={1}", portUrl, article.Id);
	}

	/// <summary>
	/// 댓글 데이터를 수정 한다.
	/// </summary>
	/// <param name="commentId">댓글 id</param>
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
			no = (MsSqlDataBaseManager.GetCommentDataCount(articleId) + 1).ToString();
			comment = new Comment() { Article_Id = articleId, Id = id, No = no, Contents = contents, Writer = writer, Date = date, Password = null };

			MsSqlDataBaseManager.SetArticleComment(comment);
		}
		else
		{
			//Article 수정
			comment = new Comment() { Article_Id = articleId, Id = id, No = no, Contents = contents, Writer = writer, Date = date, Password = null };
			MsSqlDataBaseManager.UpdateCommentData(comment);
		}

		RedirectReadPage(articleId);
	}

	/// <summary>
	/// 게시글 데이터를 수정 한다.
	/// </summary>
	/// <param name="id">게시글 id</param>
	/// <returns></returns>
	public Article GetArticleInfo(string id)
	{
		var article = new Article();
		var dataSet = MsSqlDataBaseManager.GetSelectedArticleData(id);
		var dataTbl = dataSet.Tables[MsSqlDataBaseManager.DATA_TABLE_ARTICLE_INFORMATION];

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

	/// <summary>
	/// DB에서 댓글 정보를 가져온다
	/// </summary>
	/// <param name="id">댓글 id</param>
	/// <returns></returns>
	public Comment GetCommentInfo(string id)
	{
		var comment = new Comment();
		var dataSet = MsSqlDataBaseManager.GetSelectedCommentData(id);
		var dataTbl = dataSet.Tables[MsSqlDataBaseManager.DATA_TABLE_ARTICLE_COMMENT];

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


	/// <summary>
	/// 게시글에 등록되어 있는 댓글 리스트를 반환합니다.
	/// </summary>
	/// <param name="articleId">게시글 ID</param>
	/// <returns></returns>
	public IEnumerable<Comment> GetComments(string articleId)
	{
		if (String.IsNullOrEmpty(articleId))
			return null;

		var dataSet = MsSqlDataBaseManager.GetArticleComments(articleId);
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