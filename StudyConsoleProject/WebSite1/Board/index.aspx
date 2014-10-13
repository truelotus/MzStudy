<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Board_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>게시판</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<h1>List</h1>
		<table>
		<tr>
		<!--게시판 테이블 헤더 태그-->
		<th>No</th>
		<th>Title</th>
		<th>Writer</th>
		<th>Date</th>
		<th>Hits</th>
		</tr>
		<tr>
		<!--게시판 테이블 데이터 태그-->
		<td>1</td>
		<td>나는 제목</td>
		<td>나는 작성자</td>
		<td>오늘</td>
		<td>4982340</td>
		</tr></table>
		<a href="write.aspx">글 작성</a>
    </div>
    </form>
</body>
</html>
