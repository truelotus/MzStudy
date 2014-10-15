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

		public const string DB_TABLE_NAME = "ARTICLE_INFO";


		public static SqlConnection GetConnection()
		{
			return new SqlConnection(getConnectionString());
		}


		/// <summary>
		/// DB에 저장된 게시글 데이터를 전부 반환한다.
		/// </summary>
		/// <returns></returns>
		public static DataSet GetArticlesData()
		{
			var command = new SqlCommand("SELECT * FROM " + DB_TABLE_NAME, GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet);
			return dataSet;
		}

		/// <summary>
		/// ARTICLE_INFO의 데이터 갯수 반환한다
		/// </summary>
		/// <returns></returns>
		public static int GetDataBaseCount()
		{
			var connection = GetConnection();
			var command = new SqlCommand("SELECT COUNT(*) FROM " + DB_TABLE_NAME, connection);
			connection.Open();
			var num = (int) command.ExecuteScalar();
			connection.Close();
			return num;
		}

		/// <summary>
		/// DB에 데이터를 추가한다.
		/// </summary>
		/// <param name="article"></param>
		public static void SetArticleData(Article article)
		{
			if (article == null)
				return;

			var query = String.Format("INSERT INTO " + DB_TABLE_NAME + "(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7})"
				, article.Id, article.No, article.Title, article.Contents, article.Writer, article.Date, article.Password, article.Hits);
			var connection = GetConnection();
			var command = new SqlCommand(query, connection);
			connection.Open();
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

			var query = String.Format("SELECT * FROM " + DB_TABLE_NAME + " WHERE NO = {0}", id);
			var command = new SqlCommand(query, GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, "ARTICLE_INFO");

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

			var query = String.Format("DELETE FROM " + DB_TABLE_NAME + " WHERE ID = {0}", article.Id);
			var command = new SqlCommand(query, GetConnection());
			command.ExecuteNonQuery();
		}


		private static string getConnectionString()
		{
			return @"Data Source=localhost; Database=Article; USER ID=sa; PASSWORD=Youn820!;";
		}

	}
}