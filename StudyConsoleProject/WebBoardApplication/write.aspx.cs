using System;
using System.Collections.Generic;
using System.Linq;
using WebBoardApplication.DataBase;
using System.Data;

public partial class Board_write : System.Web.UI.Page
{
		public string mUrl;
		private string mArticleId;

		public string mTitle;
		public string mContents;

    protected void Page_Load(object sender, EventArgs e)
    {
			if (String.IsNullOrEmpty(Request.QueryString["update"]))
			{
				Random r = new Random();
				string strRandomNum = r.Next(1, 100).ToString();
				mArticleId = strRandomNum;
			}
			else
			{
				SetUpdateArticle(Request.QueryString["update"]);
			}
			
    }

		public void SetUpdateArticle(string id) 
		{
			var dataSet = MsSqlDataBase.GetSelectedArticleData(id);
			var dataTbl = dataSet.Tables["ARTICLE_INFO"];

			 if (dataSet.Tables.Count > 0)
			 {
				 foreach (DataRow dRow in dataTbl.Rows)
				 {
					 
					 mTitle = dRow["TITLE"].ToString();
					 mContents = dRow["CONTENTS"].ToString();

				 }
			 }
		}

}