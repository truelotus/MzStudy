<%@ Page Language="C#" AutoEventWireup="true" CodeFile="write.aspx.cs" Inherits="Board_write" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" style="text-align:left" runat="server" method="post" action="write.aspx">
	<div>
		<input type="text" style="visibility: hidden" name="id" id="Id" value="<%=mArticle.Id%>" />
		<input type="text" style="visibility: hidden" name="no" id="No" value="<%=mArticle.No%>" />
		<input type="text" style="visibility: hidden" name="date" id="Date" value="<%=mArticle.Date%>" />
		<table>
			<tr>
			<div>
				Title
				<input type="text" name="title" id="Title" value="<%=mArticle.Title%>" />
				Writer
				<input type="text" name="writer" value="<%=mArticle.Writer%>" />
				<br />
				Content
				<br />
				<textarea type="text" style="width: 450px; name="contents" ><%=mArticle.Contents%></textarea>
				<br />
				</div>
			<tr>
		</table>
		<%--<asp:Button ID="Btn_Write" AlternateText="글 등록" />--%>
		<input type="submit" value="글등록" />
		<%--<a id="write" href="<%=GetArticleUrl(mArticleId)%>">글 등록</a>--%>
	</div>
	</form>
</body>
</html>
