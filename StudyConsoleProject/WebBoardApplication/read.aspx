<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read.aspx.cs" Inherits="Board_Read" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
	<script src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
	<script>
	
		$(document).ready(function () {
			$(".addCommentCommand").click(function () {
				$("#commentTable").append
		("<tr><td><b>Writer </b>작성자</td><td><b>Date :</b>날짜</td></tr><tr><td><b>Contents </b>음화화<p><a>댓글 수정 </a><a>댓글 삭제</a></p></td></tr>"); });});
	</script>
	
</head>
<body>
	<form id="article_form" runat="server" method="get" action="Default.aspx">
	<table>
		<tr>
			<td>
				<b>Title</b>
				<p id="title">
					<%=mArticle.Title %></p>
			</td>
		</tr>
		<tr>
			<td>
				<b>Writer</b>
				<p id="writer">
					<%=mArticle.Writer %></p>
			</td>
		</tr>
		<tr>
			<td>
				<b>Contents</b><p id="contents">
					<%=mArticle.Contents %></p>
			</td>
		</tr>
	</table>
	<tr>
		<a href="Default.aspx">글 목록</a> 
		<a href="<%=GetUpdateArticleUrl(mArticle)%>">글 수정</a>
		<a href="<%=GetDeleteArticleUrl(mArticle.Id)%>">글 삭제</a>
	</tr>
	</form>

	<!--댓글 테이블 (댓글 작성자&작성날짜&내용 테이블, 댓글 등록,수정,삭제 버튼)-->
	<form id="comment_form" method="get" action="read.aspx">
	<input type="text" name="id" id="articleId" value="<%=mArticle.Id%>" style="visibility: hidden" />
	<input type="text" name="commentNo" id="commentNo" value="<%=mComment.No%>" style="visibility: hidden" />
	<input type="text" name="commentId" id="commentId" value="<%=mComment.Article_Id%>"
		style="visibility: hidden" />
	<div>
		Writer
		<input type="text" name="writer" id="comment_writer_textbox" value="<%=mComment.Writer%>" />
		Content
		<textarea type="text" name="contents" id="comment_contents_textbox"><%=mComment.Contents%></textarea>
		<input type="submit" value="댓글 작성" />
	</div>
	</form>
	<button class="addCommentCommand">jquery로 작성하기</button>
	<br />
	<table id="commentTable">
		<% var list = GetComments(mArticle.Id);
	 if (list != null && list.Count() > 0)
	{%>
		<%foreach (var item in list)
		{%>
			<tr>
				<td>
					<b>Writer </b>
					<%=item.Writer%>
				</td>
				<td>
					<b>Date :</b>
					<%=item.Date%>
				</td>
			</tr>
			<tr>
				<td>
					<b>Contents </b>
					<%=item.Contents%>
					<p>
						<a href="<%=GetUpdateCommentPageUrl(item.Id)%>">댓글 수정 </a>
						<a href="<%=GetDeleteCommentPageUrl(item.Id)%>">댓글 삭제</a>
					</p>
				</td>
			</tr>
		<% } %>
	<%} %>
	</table>
	
</body>
</html>
