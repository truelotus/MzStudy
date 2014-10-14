<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read.aspx.cs" Inherits="Board_Read" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<table>
			<tr>
				<td>
					<b >Title</b>
					<p id="title"></p>
				</td>
			</tr>
			<tr>
				<td>
					<b>Writer</b> 
					<p id="writer"></p>
				</td>
			</tr>
			<tr>
				<td>
					<b>Contents</b><p id="contents"></p>
				</td>
			</tr>
		</table>
		<tr>
			<a href="index.aspx">글 목록</a> <a>글 수정</a> <a>글 삭제</a>
		</tr>
	</div>
	</form>
</body>
</html>
