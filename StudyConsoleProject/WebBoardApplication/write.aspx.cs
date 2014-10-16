using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;
using System.Data;
using System.Web;

public partial class Board_write : System.Web.UI.Page
{
		public string mUrl;

		public string mArticleId;
		public string mNo; 
		public string mTitle;
		public string mContents;
		public string mWriter;

    protected void Page_Load(object sender, EventArgs e)
    {
			var queryStr = Request.QueryString["update"];
			if (String.IsNullOrEmpty(queryStr))
			{
				mArticleId = System.Guid.NewGuid().ToString();
				mNo = (MsSqlDataBase.GetDataBaseCount() + 1).ToString();
			}
			else
			{
				SetUpdateArticle(queryStr.Split('=')[0]);
			}
    }

	/// <summary>
	/// 게시글을 UI에 적용한다.
	/// </summary>
	/// <param name="id"></param>
		public void SetUpdateArticle(string id) 
		{
			mArticleId = id;
			var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
			var dataTbl = dataSet.Tables["ARTICLE_INFO"];
			 if (dataSet.Tables.Count > 0)
			 {
				 foreach (DataRow dRow in dataTbl.Rows)
				 {
					 mNo = dRow["NO"].ToString();
					 mTitle = dRow["TITLE"].ToString();
					 mContents = dRow["CONTENTS"].ToString();
					 mWriter = dRow["WRITER"].ToString();
				 }
			 }
		}

		public string GetArticleUrl(string id)
		{
			return String.Format("read.aspx?update={0}", id);
		}
}