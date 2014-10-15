using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebBoardApplication.DataBase
{
	public static class MsSqlDataBase
	{

		public const string DB_TABLE_NAME = "ARTICLE_INFO";


		public static SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["aticle_write"].ToString());
		}


		/// <summary>
		/// DB에 저장된 게시글 데이터를 전부 반환한다.
		/// </summary>
		/// <returns></returns>
		public static DataSet GetArticlesData()
		{
			var command = new SqlCommand("SP_SelectAllArticles", GetConnection());
			command.CommandType = CommandType.StoredProcedure;
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

			//일반 Sql query문
			//var query = String.Format("INSERT INTO " + DB_TABLE_NAME + "(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7})"
			//  , article.Id, article.No, article.Title, article.Contents, article.Writer, article.Date, article.Password, article.Hits);

			var connection = GetConnection();

			var cmd = new SqlCommand("SP_InsertNewArticle", connection);
			connection.Open();
			cmd.CommandType = CommandType.StoredProcedure;
			
			cmd.Parameters.Add("@Id", SqlDbType.Int, 10).Value = Convert.ToInt32(article.Id);
			cmd.Parameters.Add("@No", SqlDbType.Int).Value = Convert.ToInt32(article.No);
			cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = article.Title;
			cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = article.Contents;
			cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = article.Writer;
			cmd.Parameters.Add("@Date", SqlDbType.VarChar).Value = article.Date;

			//null을 허용한 컬럼.
			if (article.Password==null)
				cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value;
			else
				cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = article.Password;

			cmd.Parameters.Add("@Hits", SqlDbType.Int).Value = Convert.ToInt32(article.Hits);

			cmd.ExecuteNonQuery();

			connection.Close();
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

			//var query = String.Format("SELECT * FROM " + DB_TABLE_NAME + " WHERE ID = {0}", id);

			var cmd = new SqlCommand("SP_SelectArticle", GetConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(id);
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, "ARTICLE_INFO");

			return dataSet;
		}

		/// <summary>
		/// DB에서 ID를 조회하여 데이터를 삭제한다.
		/// </summary>
		/// <param name="article"></param>
		public static void DeleteArticleData(string id)
		{

			//var query = String.Format("DELETE FROM " + DB_TABLE_NAME + " WHERE ID = {0}", id);
			var connect = GetConnection();
			var cmd = new SqlCommand("Sp_DeleteArticle", connect);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(id);
			connect.Open();
			cmd.ExecuteNonQuery();
			connect.Close();
		}

		public static void UpdateArticleData(Article article) 
		{
			//아이디 조회
			var dataSet = GetSelectedArticleData(article.Id);
			var dataTbl = dataSet.Tables["ARTICLE_INFO"];

			if (dataSet.Tables.Count > 0)
			{
				//var query = String.Format("UPDATE INTO " + DB_TABLE_NAME + "(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7})"
				//, article.Id, article.No, article.Title, article.Contents, article.Writer, article.Date, article.Password, article.Hits);

				var connect = GetConnection();
				var cmd = new SqlCommand("SP_UpdateArticle2", connect);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = article.Title;
				cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = article.Contents;
				cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = article.Writer;
				//null을 허용한 컬럼.
				if (article.Password == null)
					cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = article.Password;

				connect.Open();

				cmd.ExecuteNonQuery();

				connect.Close();
			}
		}
	}
}