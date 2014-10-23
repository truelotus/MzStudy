using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebBoardApplication.DataBase
{
	public static class MsSqlDataBaseManager
	{
		/// <summary>
		/// 게시글 테이블
		/// </summary>
		public static string DATA_TABLE_ARTICLE_INFORMATION = "ARTICLE_INFORMATION";

		/// <summary>
		/// 댓글 테이블
		/// </summary>
		public static string DATA_TABLE_ARTICLE_COMMENT = "ARTICLE_COMMENT";

		/// <summary>
		/// web.config의 ConnectionString을 가져와 데이터베이스 서버와 연결 합니다.
		/// </summary>
		/// <returns></returns>
		public static SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["aticle"].ToString());
		}

		/// <summary>
		/// DB에 저장되어 있는 Article 데이터를 전부 반환합니다.
		/// Table : ARTICLE_INFORMATION
		/// </summary>
		/// <returns>Article 데이터</returns>
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
		///Table : ARTICLE_INFORMATION
		/// </summary>
		/// <returns></returns>
		public static int GetArticleDataCount()
		{
			using (var connection = GetConnection())
			using (var cmd = new SqlCommand("SP_SelectCountArticles", connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				connection.Open();
				var num = (int)cmd.ExecuteScalar();
				return num;
			}
		}

		/// <summary>
		/// 해당 Article 조회수를 업데이트 합니다.
		/// Table : ARTICLE_INFORMATION
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
			}
		}

		/// <summary>
		/// DB에 Article 데이터를 추가합니다.
		/// Table : ARTICLE_INFORMATION
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
		///  Table : ARTICLE_INFORMATION
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
				adapter.Fill(dataSet, DATA_TABLE_ARTICLE_INFORMATION);

				return dataSet;
			}
		}

		/// <summary>
		/// Article ID를 가진 데이터의 존재여부를 확인합니다.
		/// Table : ARTICLE_INFORMATION
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
				adapter.Fill(dataSet, DATA_TABLE_ARTICLE_INFORMATION);
				var dataTbl = dataSet.Tables[DATA_TABLE_ARTICLE_INFORMATION];

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
		/// DB에서 ID를 조회하여 해당 Article데이터와 댓글데이터를 삭제합니다.
		/// Table : ARTICLE_INFORMATION & ARTICLE_COMMENT
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
		/// Table : ARTICLE_INFORMATION
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
		/// Table : ARTICLE_INFORMATION
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
				adapter.Fill(dataSet, DATA_TABLE_ARTICLE_INFORMATION);
				var dataTbl = dataSet.Tables[DATA_TABLE_ARTICLE_INFORMATION];

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

		/// <summary>
		/// 게시글 Id를 가진 Comment db 정보 조회 후 반환합니다.
		/// Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DataSet GetArticleComments(string id) 
		{
			using (var cmd = new SqlCommand("SP_SelectArticleAllComment", GetConnection()))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Article_id", SqlDbType.VarChar).Value = id;
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				return dataSet;
			}
		}
		/// <summary>
		/// 댓글 정보를 데이터베이스에 입력합니다.
		/// Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="articleComment"></param>
		/// <returns></returns>
		public static bool SetArticleComment(Comment articleComment)
		{
			if (articleComment == null)
				return false;

			try
			{
				using (var connection = GetConnection())
				using (var cmd = new SqlCommand("SP_InsertArticleComment", connection))
				{
					connection.Open();
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add("@Article_id", SqlDbType.VarChar).Value = articleComment.Article_Id;

					cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = articleComment.Id;

					if (articleComment.Contents == null)
						cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = articleComment.Contents;

					if (articleComment.Writer == null)
						cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = articleComment.Writer;

					cmd.Parameters.Add("@Date", SqlDbType.VarChar).Value = articleComment.Date;

					if (articleComment.Password == null)
						cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = articleComment.Password;

					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// DB에서 Comment ID를 조회하여 해당 Comment 데이터를 삭제합니다.
		/// Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="article"></param>
		public static void DeleteArticleCommentData(string id)
		{
			using (var connect = GetConnection())
			using (var cmd = new SqlCommand("SP_DeleteArticleComment", connect))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
				connect.Open();
				cmd.ExecuteNonQuery();
				connect.Close();
			}
		}

		/// <summary>
		/// DB에서 Comment ID를 조회하여 해당하는 Comment 데이터를 반환합니다.
		/// Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="id">Comment ID</param>
		/// <returns>Comment DataSet</returns>
		public static DataSet GetSelectedCommentData(string id)
		{
			if (String.IsNullOrEmpty(id))
				return null;
			using (var cmd = new SqlCommand("SP_SelectArticleComment", GetConnection()))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet, DATA_TABLE_ARTICLE_COMMENT);

				return dataSet;
			}
		}

		/// <summary>
		/// 댓글 정보를 수정합니다.
		/// Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="comment">수정한 댓글 정보</param>
		public static void UpdateCommentData(Comment comment)
		{
			//아이디 조회

				using (var connect = GetConnection())
				using (var cmd = new SqlCommand("SP_UpdateArticleComment", connect))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = comment.Id;

					if (comment.Contents == null)
						cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Contents", SqlDbType.VarChar).Value = comment.Contents;

					if (comment.Writer == null)
						cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Writer", SqlDbType.VarChar).Value = comment.Writer;
					//null을 허용한 컬럼.
					if (comment.Password == null)
						cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value;
					else
						cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = comment.Password;

					connect.Open();
					cmd.ExecuteNonQuery();
					connect.Close();
				}
			
				return;
		}

		/// <summary>
		///  게시글 ID를 가진 댓글 데이터의 총 갯수를 반환합니다.
		///  Table : ARTICLE_COMMENT
		/// </summary>
		/// <param name="articleId">게시글 ID</param>
		/// <returns>게시글 ID를 가진 댓글 데이터의 총 갯수</returns>
		public static int GetCommentDataCount(string articleId)
		{
			if (String.IsNullOrEmpty(articleId))
				return 0;

			using (var connection = GetConnection())
			using (var cmd = new SqlCommand("SP_SelectCountComments", connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@Article_id", SqlDbType.VarChar).Value = articleId;
				connection.Open();
				var num = (int)cmd.ExecuteScalar();
				return num;
			}
		}
	}
}