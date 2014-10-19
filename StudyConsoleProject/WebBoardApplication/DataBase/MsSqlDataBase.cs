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
		public static string DB_TABLE_NAME = "ARTICLE_INFORMATION";


		public static SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["aticle_write"].ToString());
		}


		/// <summary>
		/// DB에 저장되어 있는 Article 데이터를 전부 반환합니다.
		/// </summary>
		/// <returns></returns>
		public static DataSet GetArticlesData()
		{
			using (var cmd = new SqlCommand("SP_SelectAllArticles", GetConnection()))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				return dataSet;
			}
		}

		/// <summary>
		///DB 데이터 갯수 반환합니다.
		/// </summary>
		/// <returns></returns>
		public static int GetDataBaseCount()
		{
			using (var connection = GetConnection())
			using (var cmd = new SqlCommand("SP_SelectCountArticles", connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				connection.Open();
				var num = (int)cmd.ExecuteScalar();
				connection.Close();
				return num;
			}
		}

		/// <summary>
		/// 해당 Article 조회수를 업데이트 합니다.
		/// </summary>
		/// <param name="id"></param>
		public static void UpdateHits(string id)
		{
			using (var connection = GetConnection())
			using (var cmd = new SqlCommand("SP_UpdateHits", connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
				connection.Open();
				cmd.ExecuteNonQuery();
				connection.Close();
			}
		}

		/// <summary>
		/// DB에 Article 데이터를 추가합니다.
		/// </summary>
		/// <param name="article"></param>
		public static bool SetArticleData(Article article)
		{
			if (article == null)
				return false;

			try
			{
				using (var connection = GetConnection())
				using (var cmd = new SqlCommand("SP_InsertNewArticle", connection))
				{
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
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		///  DB에서 ID를 조회하여 Article 데이터를 반환합니다.
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public static DataSet GetSelectedArticleData(string id)
		{
			if (String.IsNullOrEmpty(id))
				return null;
			using (var cmd = new SqlCommand("SP_SelectArticle", GetConnection()))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet, DB_TABLE_NAME);

				return dataSet;
			}
		}

		/// <summary>
		/// Article ID를 가진 데이터의 존재여부를 확인합니다.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool HasArticleData(string id)
		{
			if (String.IsNullOrEmpty(id))
				return false;

			using (var cmd = new SqlCommand("SP_SelectArticle", GetConnection()))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
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
			}
			return false;
		}

		/// <summary>
		/// DB에서 ID를 조회하여 해당 Article데이터를 삭제합니다.
		/// </summary>
		/// <param name="article"></param>
		public static void DeleteArticleData(string id)
		{
			using (var connect = GetConnection())
			using (var cmd = new SqlCommand("Sp_DeleteArticle", connect))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
				connect.Open();
				cmd.ExecuteNonQuery();
				connect.Close();	
			}
		}

		/// <summary>
		/// Article ID를 가지고 DB Update합니다.
		/// </summary>
		/// <param name="article"></param>
		public static void UpdateArticleData(Article article)
		{
			//아이디 조회
			if (HasArticleData(article.Id))
			{
				using (var connect = GetConnection())
				using (var cmd = new SqlCommand("SP_UpdateArticle", connect))
				{
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
			}
			else
				return;
		}

		/// <summary>
		/// 시작과 끝 범위 사이에 있는 해당하는 게시글 데이터를 날짜 오름차순으로 조회 합니다.
		/// </summary>
		/// <param name="start">조회 시작 번호</param>
		/// <param name="end">조회 끝 번호</param>
		/// <returns></returns>
		public static IEnumerable<Article> GetArticleBetweenDataList(int start, int end)
		{
			using (var connect = GetConnection())
			using (var cmd = new SqlCommand("SP_SelectBetweenArticles", connect))
			{
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
}