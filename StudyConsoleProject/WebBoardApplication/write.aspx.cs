using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;
using System.Data;
using System.Web;

public partial class Board_write : System.Web.UI.Page
{
		public string mUrl;
		private string mArticleId;

		public string mNo; 
		public string mTitle;
		public string mContents;
		public string mWriter;

    protected void Page_Load(object sender, EventArgs e)
    {
			if (String.IsNullOrEmpty(Request.QueryString["update"]))
			{
				Random r = new Random();
				string strRandomNum = r.Next(1, 100).ToString();
				mArticleId = strRandomNum;
				mNo = (MsSqlDataBase.GetDataBaseCount() + 1).ToString();
				if (MsSqlDataBase.HasArticleData(mNo))
				{
					
				}
			}
			else
			{
				SetUpdateArticle(Request.QueryString["update"]);
			}
    }

	/// <summary>
	/// 게시글을 UI에 적용한다.
	/// </summary>
	/// <param name="id"></param>
		public void SetUpdateArticle(string id) 
		{
			var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
			var dataTbl = dataSet.Tables["ARTICLE_INFO"];
			 if (dataSet.Tables.Count > 0)
			 {
				 foreach (DataRow dRow in dataTbl	.Rows)
				 {
					 mNo = dRow["NO"].ToString();
					 mTitle = dRow["TITLE"].ToString();
					 mContents = dRow["CONTENTS"].ToString();
					 mWriter = dRow["WRITER"].ToString();
				 }
			 }
			 else
			 {
				 //DB에서 찾을 수 없음.
			 }
		}
}