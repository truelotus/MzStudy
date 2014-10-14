﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Board_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>게시판</title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h1>
			List</h1>
		<table>
			<tr>
				<!--게시판 테이블 헤더 태그-->
				<th>
					No
				</th>
				<th>
					Title
				</th>
				<th>
					Writer
				</th>
				<th>
					Date
				</th>
				<th>
					Hits
				</th>
			</tr>
			<tr>
				<!--게시판 테이블 데이터 태그-->
				<% var list = GetList();
	   foreach (WebBoardApplication.DataBase.Article item in list)
	 {
		 if (item != null)
	 {%>
				<tr>
					<td>
						<%=item.No%>
					</td>
					<td>
						<a href=<%=GetArticleUrl(item)%>><%=item.Title%></a>
					</td>
					<td>
						<%=item.Writer%>
					</td>
					<td>
						<%=item.Date%>
					</td>
					<td>
						<%=item.Hits%>
					</td>
				</tr>
				<% }

	 }%>
			</tr>
		</table>
		<a href="write.aspx">글 작성</a>
	</div>
	</form>
</body>
</html>