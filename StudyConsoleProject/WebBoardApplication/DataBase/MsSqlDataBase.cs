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

		/// <summary>
		/// DB에 저장된
		/// </summary>
		/// <returns></returns>
		public static DataSet GetArticlesData()
		{
			TryConnection();

			var command = new SqlCommand("SELECT * FROM ARTICLE_INFO", GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet);
			return dataSet;
		}

		/// <summary>
		/// DB에 데이터를 추가한다.
		/// </summary>
		/// <param name="article"></param>
		public static void SetArticleData(Article article)
		{
			if (article == null)
				return;

			TryConnection();

			var query = String.Format("INSERT INTO ARTICLE_INFO(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) VALUE({0},{1},{2},{3},{4},{5},{6},{7})"
				, article.Id, article.No, article.Title, article.Contents, article.Writer, article.Date, article.Password, article.Hits);
			var command = new SqlCommand(query, GetConnection());
			command.ExecuteNonQuery();
		}

		/// <summary>
		///  DB에서 ID를 조회하여 데이터를 반환한다.
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public static DataSet GetSelectedArticleData(string id)
		{
			if (String.IsNullOrEmpty(id))
				return null;

			TryConnection();

			var query = String.Format(" SELECT * FROM ARTICLE_INFO WHERE NO = {0}", id);
			var command = new SqlCommand(query, GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet);

			return dataSet;
		}

		/// <summary>
		/// DB에서 ID를 조회하여 데이터를 삭제한다.
		/// </summary>
		/// <param name="article"></param>
		public static void DeleteArticleData(Article article)
		{
			if (article == null)
				return;

			TryConnection();

			var query = String.Format("DELETE FROM ARTICLE_INFO WHERE ID = {0}", article.Id);
			var command = new SqlCommand(query, GetConnection());
			command.ExecuteNonQuery();
		}


		private static string getConnectionString()
		{
			return @"Data Source=localhost; Database=Article; USER ID=sa; PASSWORD=Youn820!;";
		}

	}
}