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
	public IEnumerable<Article> mList;

	public int mPageBlockCount = 1;

	public void Page_Load(object sender, EventArgs e)
	{
		//한 페이지 블럭 당 게시글 갯수 10개 기준이다.
		int pageCountValue = 10;

		if (!String.IsNullOrEmpty(Request.QueryString["delete"]))
		{
			DeleteArticle(Request.QueryString["delete"]);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["page"]))
		{
			var pageParam = Convert.ToInt32(Request.QueryString["page"]);
			//보여질 페이지 블럭 총 갯수
			mPageBlockCount = this.GetTotalPageCount(pageParam, pageCountValue);

			//현재 페이지에 들어갈 게시글을 디비에서 조회하여 리턴.
			int end = pageCountValue * pageParam;
			int start = end - (pageCountValue - 1);
			mList = MsSqlDataBaseManager.GetArticleBetweenDataList(start, end);
		}
		else
		{
			//1.목록 첫 진입 시 게시판 메인 접근 시 DB에서 게시글 데이터 조회
			int articleTotalCount = MsSqlDataBaseManager.GetArticleDataCount();

			if (articleTotalCount > 0)
			{
				if (articleTotalCount > 10)
				{
					//초과 시 만들어질 전체 페이지 블럭 총 갯수
					mList = MsSqlDataBaseManager.GetArticleBetweenDataList(1, pageCountValue);
					mPageBlockCount = this.GetTotalPageCount(1, pageCountValue);
				}
			}
		}
	}

	public string GetPageUrl(int pageNum)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/Default.aspx?page={1}", portUrl, pageNum);
	}

	/// <summary>
	/// 게시글 정보를 삭제한다.
	/// </summary>
	/// <param name="id"></param>
	public void DeleteArticle(string id)
	{
		MsSqlDataBaseManager.DeleteArticleData(id);
		Response.Redirect("Default.aspx");
		Response.End();
	}

	/// <summary>
	/// 게시글 읽기 페이지로 이동하는 주소를 보내준다.
	/// </summary>
	/// <param name="article">게시글 정보</param>
	/// <returns>게시글 페이지 주소</returns>
	public string GetArticleUrl(Article article)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?read={1}", portUrl, article.Id);
	}


	public IEnumerable<Article> GetList()
	{
		if (mList != null)
			return mList;

		var dataSet = MsSqlDataBaseManager.GetArticlesData();
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

	/// <summary>
	/// 페이지 블럭 총 갯수 반환한다.
	/// </summary>
	/// <param name="page"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	public int GetTotalPageCount(int page, int count)
	{
		//DB에 저장된 게시글 총 갯수
		var total = MsSqlDataBaseManager.GetArticleDataCount();

		int pageCount = total / count;

		int remain = total % count;
		if (remain > 0)
			pageCount++;

		if (pageCount == 0)
			return 1;

		return pageCount;
	}
}