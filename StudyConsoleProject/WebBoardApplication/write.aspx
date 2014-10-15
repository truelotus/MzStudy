<%@ Page Language="C#" AutoEventWireup="true" CodeFile="write.aspx.cs" Inherits="Board_write" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server" method="post" action="read.aspx">
	<div>
		<table>
			<tr>
				Title
				<input type="text" name="title" id="Title" value="<%=mTitle %>"/>
				<br />
				Writer
				<input type="text" name="writer" />
				<br />
				Content
				<TEXTAREA type="text" name="contents" ><%=mContents%></TEXTAREA>
				<br />
				<tr>
		</table>
		<%--<asp:Button ID="Btn_Write" AlternateText="글 등록" />--%>
		<input type="submit" value="글등록" />
		<%--<a id="write" href="<%=GetArticleUrl(mArticleId)%>">글 등록</a>--%>
	</div>
	</form>
</body>
</html>
