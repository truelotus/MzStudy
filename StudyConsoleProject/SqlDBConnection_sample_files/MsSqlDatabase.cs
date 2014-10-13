using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using GoSPA.DataMiner.Core.Data;
using System.Collections;

namespace GoSPA.DataMiner.Core.Database
{
	internal class MsSqlDatabase : IDatabase
	{
		public SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["gospa_write"].ToString());
		}

		public void TryConnection()
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("SELECT TOP 1 * FROM [ACCOUNTDB].[dbo].[GOSPA_DEAL_INFO]", connection))
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		public decimal InsertApprovedT(ApprovedT item)
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("TEMP_GOSPA_APPR_INFO_INSERT", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				AddParameterFromFieldInfo(item, command);

				connection.Open();

				return (decimal)command.ExecuteScalar();
			}
		}

		public void InsertOrderT(OrderT item)
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("TEMP_GOSPA_DEAL_INFO_INSERT", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				AddParameterFromFieldInfo(item, command);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		private static void AddParameterFromFieldInfo(object item, SqlCommand command)
		{
			foreach (var property in item.GetType().GetProperties())
			{
				var attr = Attribute.GetCustomAttribute(property, typeof(FieldInfoAttribute)) as FieldInfoAttribute;
				if (attr != null)
				{
					if (attr.DbType == SqlDbType.Char
					|| attr.DbType == SqlDbType.NChar
					|| attr.DbType == SqlDbType.VarChar
					|| attr.DbType == SqlDbType.NVarChar)
					{
						command.Parameters.Add("@" + attr.Name, attr.DbType, attr.Size).Value = property.GetValue(item, null);
					}
					else
					{
						command.Parameters.Add("@" + attr.Name, attr.DbType).Value = property.GetValue(item, null);
					}
				}
			}
		}

		public void InsertSomething()
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("sp_something", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.Add("@SomeField1", SqlDbType.VarChar).Value = "SomeValue1";
				command.Parameters.Add("@SomeField2", SqlDbType.Money).Value = "SomeValue2";

				connection.Open();
				command.ExecuteNonQuery();
			}
		}


		public void InsertData(IEnumerable dbDataList)
		{
			foreach (ApprovedT appr in dbDataList)
			{
				var seq = InsertApprovedT(appr);
				foreach (var order in appr.OrderList)
				{
					order.ApprSeqNo = seq;
					InsertOrderT(order);
				}
			}
		}

		public void UpdateData(IEnumerable dbDataList)
		{
			foreach (ApprovedT appr in dbDataList)
			{
				var seq = GetApprovedSeqNo(appr.MagentoApprNO);
				if (seq != null)
				{
					foreach (var order in appr.OrderList)
					{
						UpdateOrderT(order.OrderNo, order.PayType, seq.Value, order.SendDate, order.ShippingFinishDate, order.RemitScheduleDate, order.BuyDecisionDate);
					}
				}
			}
		}

		private void UpdateOrderT(string orderNo, string payType, long apprSeqNo, DateTime? sendDate, DateTime? shippingFinishDate, DateTime? remitScheduleDate, DateTime? buyDecisionDate)
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("TEMP_GOSPA_APPR_INFO_UpdateDates", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.Add("@ITEM_ORDER_NO", SqlDbType.VarChar, 9).Value = orderNo;
				command.Parameters.Add("@PAY_TYPE", SqlDbType.Char, 1).Value = payType;
				command.Parameters.Add("@APPR_SEQNO", SqlDbType.BigInt).Value = apprSeqNo;
				command.Parameters.Add("@SEND_DATE", SqlDbType.DateTime).Value = sendDate;
				command.Parameters.Add("@SHIPPING_FINISH_DATE", SqlDbType.DateTime).Value = shippingFinishDate;
				command.Parameters.Add("@REMIT_SCHEDULED_DATE", SqlDbType.DateTime).Value = remitScheduleDate;
				command.Parameters.Add("@BUY_DECISION_DATE", SqlDbType.DateTime).Value = buyDecisionDate;
				
				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		private long? GetApprovedSeqNo(string magentoApprNo)
		{
			using (var connection = GetConnection())
			using (var command = new SqlCommand("TEMP_GOSPA_APPR_INFO_SelectSeqByMagentoNo", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.Add("@MAGENTO_APPR_NO", SqlDbType.VarChar, 9).Value = magentoApprNo;

				connection.Open();
				
				var result = command.ExecuteScalar();
				if (result == null)
				{
					return null;
				}
				else
				{
					return (long)result;
				}
			}
		}
	}
}
