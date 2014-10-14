using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBoardApplication.DataBase;
using System.Data;

public partial class Board_Main : System.Web.UI.Page
{

	public void Page_Load(object sender, EventArgs e)
	{
		var url = Request.Url.AbsoluteUri;

		//string requestUrl = null;


		//if (String.IsNullOrEmpty(requestUrl = Request.QueryString["write"]))
		//{

		//}
		//else if (String.IsNullOrEmpty(requestUrl = Request.QueryString["read"]))
		//{

		//}
		//else if (String.IsNullOrEmpty(requestUrl = Request.QueryString["index"]))
		//{

		//}
	}

	public IEnumerable<Article> GetList()
	{

		var dataSet = MsSqlDataBase.GetData();
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
}