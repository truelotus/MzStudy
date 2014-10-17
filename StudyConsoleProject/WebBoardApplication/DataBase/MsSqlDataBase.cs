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

		public const string DB_TABLE_NAME = "ARTICLE_INFORMATION";


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
		///DB 데이터 갯수 반환한다
		/// </summary>
		/// <returns></returns>
		public static int GetDataBaseCount()
		{
			var connection = GetConnection();
			var command = new SqlCommand("SELECT COUNT(*) FROM " + DB_TABLE_NAME, connection);
			connection.Open();
			var num = (int)command.ExecuteScalar();
			connection.Close();
			return num;
		}

		/// <summary>
		/// 해당 게시물의 조회수를 업데이트 합니다.
		/// </summary>
		/// <param name="id"></param>
		public static void UpdateHits(string id) 
		{
			var connection = GetConnection();
			var cmd = new SqlCommand("SP_UpdateHits", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
		}

		/// <summary>
		/// DB에 데이터를 추가한다.
		/// </summary>
		/// <param name="article"></param>
		public static bool SetArticleData(Article article)
		{
			if (article == null)
				return false;

			//일반 Sql query문
			//var query = String.Format("INSERT INTO " + DB_TABLE_NAME + "(ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7})"
			//  , article.Id, article.No, article.Title, article.Contents, article.Writer, article.Date, article.Password, article.Hits);
			try
			{
				var connection = GetConnection();

				var cmd = new SqlCommand("SP_InsertNewArticle", connection);
				connection.Open();
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = article.Id;

				cmd.Parameters.Add("@No", SqlDbType.Int).Value = Convert.ToInt32(article.No);

				if (article.Title == null)
					cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = article.Title;

				if (article.Contents == null)
					cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = article.Contents;

				if (article.Writer == null)
					cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = article.Writer;

				cmd.Parameters.Add("@Date", SqlDbType.VarChar).Value = article.Date;

				if (article.Password == null)
					cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = article.Password;

				cmd.Parameters.Add("@Hits", SqlDbType.Int).Value = Convert.ToInt32(article.Hits);

				cmd.ExecuteNonQuery();

				connection.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
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
			cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, "ARTICLE_INFO");

			return dataSet;
		}

		public static bool HasArticleData(string id)
		{
			if (String.IsNullOrEmpty(id))
				return false;

			var query = String.Format("SELECT * FROM "+DB_TABLE_NAME+" WHERE ID = '{0}'", id);
			var cmd = new SqlCommand(query, GetConnection());
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, DB_TABLE_NAME);
			var dataTbl = dataSet.Tables[DB_TABLE_NAME];

			if (dataSet.Tables.Count > 0)
			{
				foreach (DataRow dRow in dataTbl.Rows)
				{
					if (dRow["ID"] == null)
						return false;
					else
						return true;
				}
			}

			return false;
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
			cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
			connect.Open();
			cmd.ExecuteNonQuery();
			connect.Close();
		}

		/// <summary>
		/// Article의 No 정보를 가지고 DB Update한다.
		/// </summary>
		/// <param name="article"></param>
		public static void UpdateArticleData(Article article)
		{
			//아이디 조회
			if (HasArticleData(article.Id))
			{
				var connect = GetConnection();
				var cmd = new SqlCommand("SP_UpdateArticle", connect);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = article.Id;
				if (article.Title == null)
					cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = article.Title;

				if (article.Contents == null)
					cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = DBNull.Value;
				else
					cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = article.Contents;

				if (article.Writer == null)
					cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = DBNull.Value;
				else
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
			else
				return;
		}

		/// <summary>
		/// index 기준으로 시작~끝 위치에 존재하는 게시글 데이터를 날짜 오름차순으로 추출
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static IEnumerable<Article> GetArticleBetweenDataList(int start, int end)
		{
			var connect = GetConnection();
			var cmd = new SqlCommand("SP_SelectBetweenArticles", connect);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@StartNo", SqlDbType.Int).Value = start;
			cmd.Parameters.Add("@EndNo", SqlDbType.Int).Value = end;

			connect.Open();
			cmd.ExecuteNonQuery();
			connect.Close();

			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, DB_TABLE_NAME);
			var dataTbl = dataSet.Tables[DB_TABLE_NAME];

			var articleList = new List<Article>();
			if (dataSet.Tables.Count > 0)
			{

				foreach (DataRow dRow in dataTbl.Rows)
				{
					var article = new Article();
					article.Id = dRow["ID"].ToString();
					article.No = dRow["NO"].ToString();
					article.Title = dRow["TITLE"].ToString();
					article.Contents = dRow["CONTENTS"].ToString();
					article.Writer = dRow["WRITER"].ToString();
					article.Date = dRow["DATE"].ToString();
					article.Password = dRow["PASSWORD"].ToString();
					article.Hits = dRow["HITS"].ToString();
					articleList.Add(article);
				}
			}
			else
				return null;

			return articleList;
		}
	}
}