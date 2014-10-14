using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebBoardApplication.DataBase
{
	public static class MsSqlDataBase
	{
		public static SqlConnection GetConnection() 
		{
			return new SqlConnection(getConnectionString());
		}


		public static void TryConnection()
		{
				var connection = GetConnection();
				var command = new SqlCommand("SELECT * FROM ARTICLE_INFO", connection);
				connection.Open();
				command.ExecuteNonQuery();
		}

		//select all
		public static DataSet GetData() 
		{
			TryConnection();

			var command = new SqlCommand("SELECT * FROM ARTICLE_INFO", GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet);
			return dataSet;
		}

		//insert
		public static void SetData(Article article) 
		{
			TryConnection();
			var query = String.Format("INSERT INTO ARTICLE_INFO(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS)VALUE({0},{1},{2},{3},{4},{5},{6},{7})"
				, article.Id,article.No,article.Title,article.Contents,article.Writer,article.Date,article.Password,article.Hits);
			var command = new SqlCommand(query, GetConnection());
			command.ExecuteNonQuery();
		}

		private static string getConnectionString()
		{
			return @"Data Source=localhost;Initial Catalog=pubs;USER ID=EBAYKOREA\mgz730;PASSWORD=Youn820!";
		}

	}
}