using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBoardApplication.DataBase
{
	public class Comment
	{
		public string Article_Id { get; set; }
		public string Id { get; set; }
		public string No { get; set; }
		public string Contents { get; set; }
		public string Writer { get; set; }
		public string Date { get; set; }
		public string Password { get; set; }
	}
}