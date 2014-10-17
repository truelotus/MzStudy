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
	public int mBlockCount = 1;
	public void Page_Load(object sender, EventArgs e)
	{
		//한페이지의 게시글 갯수 기준
		int pageCountValue = 10;
		//현재 페이지 TODO: 기본값은 1이고 요청시 변경되어야함.
		int page = 1;

		if (!String.IsNullOrEmpty(Request.QueryString["delete"]))
		{
			DeleteArticle(Request.QueryString["delete"]);
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["page"]))
		{
			//만들어질 전체 페이지 블럭 총 갯수
			mBlockCount = this.GetTotalPageCount(page, pageCountValue);

			//현재 페이지에 들어갈 게시글을 디비에서 조회하여 리턴.
			int end = pageCountValue * page;
			int start = end - (pageCountValue - 1);
			mList = MsSqlDataBase.GetArticleBetweenDataList(start, end);
		}
		else
		{
			//1.첫 진입 시 게시판 메인 접근 시DB에서 게시글 데이터 조회
			if (MsSqlDataBase.GetDataBaseCount() > 0)
			{
				mList = MsSqlDataBase.GetArticleBetweenDataList(1, pageCountValue);
				mBlockCount = this.GetTotalPageCount(1, pageCountValue);
			}
		}
	}


	public void DeleteArticle(string id)
	{
		MsSqlDataBase.DeleteArticleData(id);
	}

	public string GetArticleUrl(Article article)
	{
		var portUrl = Request.Url.Host + ":" + Request.Url.Port;
		return String.Format("http://{0}/read.aspx?read={1}", portUrl, article.Id);
	}


	public IEnumerable<Article> GetList()
	{
		if (mList != null)
			return mList;

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

	/// <summary>
	/// 총 페이지 갯수 반환한다.
	/// </summary>
	/// <param name="page"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	public int GetTotalPageCount(int page, int count)
	{
		//게시글 총 갯수
		var total = MsSqlDataBase.GetDataBaseCount();
		//페이지 갯수
		int pageCount = total / count;
		int remain = total % count;
		if (remain > 0)
			pageCount++;

		if (pageCount == 0)
			return 1;

		return pageCount;
	}
}