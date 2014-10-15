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
		public Article mArticle;
    protected void Page_Load(object sender, EventArgs e)
    {
			var requestUrl = Request.Url.AbsoluteUri;

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

		public void SetDatabase(HttpRequest request)
		{

			Random r = new Random();
			string strRandomNum = r.Next(1, 100).ToString();
			var id = strRandomNum; 

			string title = request.Params["title"];
			string contents = request.Params["contents"];
			string writer = request.Params["writer"];
			string date = DateTime.Now.ToString();
			int no = MsSqlDataBase.GetDataBaseCount() + 1;
			mArticle = new Article() { Id = id, No = no, Title = title, Contents = contents, Writer = writer, Date = date, Password = null, Hits = "0" };
			//DB Set
			MsSqlDataBase.SetArticleData(mArticle);
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
					article.No = (int)dRow["NO"];
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