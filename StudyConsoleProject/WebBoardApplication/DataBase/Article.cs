﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBoardApplication.DataBase
{
	public class Article
	{
		public string Id { get; set; }
		public int No { get; set; }
		public string Title { get; set; }
		public string Contents { get; set; }
		public string Writer { get; set; }
		public string Date { get; set; }
		public string Password { get; set; }
		public string Hits { get; set; }
	}
}