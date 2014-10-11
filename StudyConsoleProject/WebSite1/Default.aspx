<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Web local file explorer</title>
</head>
<h1>
	<%=mCurrentDirectoryPath%>
</h1>
<a href="<%=mParentDirectoryPath%>">[Go to parent directory..]</a>
<body>
	<form id="form1" runat="server">
	<hr />
	<table id="t_list">
		<!--아이템 넣어 주는 부분 처음만 위 세항목 출력-->
		<%
		int i = 0;
		var list = GetAllFiles(mCurrentDirectoryPath);
	if (list == null)
  {
		%>
		<td>
		<b>"디렉토리에 접근 할 수 없습니다."</b></td>
		<%
	return;
  }
	foreach (var item in list)
  {
	  if (item != null)
	  {
		  if (i == 0)
		  {
		%>
		<tr>
			<td>
				<b>Name</b>
			</td>
			<td>
				<b>Size</b>
			</td>
			<td>
				<b>DateModified</b>
			</td>
		</tr>
		<%
	  }
		%>
		<tr>
			<!--item image-->
			<td>
				<img src='<%=GetFolderIcon()%>' />
			</td>
			<!--item path-->
			<td>
				<b /><a href="<%=GetUrl(item) %>">
					<%=GetShortName(item)%></a>
			</td>
			<!--item size-->
			<td>
				<b />
				<%=GetSize(item) %>
			</td>
			<!--item modifieddate-->
			<td>
				<b />
				<%=GetModifiedDate(item) %>
			</td>
		</tr>
		<%
	  i++;
	} %>
		<%
  }
		%>
	</table>
	</form>
</body>
</html>
