<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read.aspx.cs" Inherits="Board_Read" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
	<script src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
	<script type="text/javascript" >

		

		//	1.해당 버튼 onclick 이벤트에 연결 했을 때 스트립트
		//	function addCommentCommand() {
		//	$("#commentTable").append("<tr><td><b>Writer </b>" + $("#comment_writer_textbox").val() + "</td><td><b>Date :</b>날짜</td></tr><tr><td><b>Contents </b>"
		//	+ $("#comment_contents_textbox").val() + "<p><a>댓글 수정 </a><a>댓글 삭제</a></p></td></tr>");
		//	}

		//	2. 한번에 나열한 스크립트
		//	$(document).ready(function () {
		//		$("#addCommentCommand").click(function () {
		//			$("#commentTable").append("<tr><td><b>Writer </b>" + $("#comment_writer_textbox").val() + "</td><td><b>Date :</b>날짜</td></tr><tr><td><b>Contents </b>"
		//			+ $("#comment_contents_textbox").val() + "<p><a>댓글 수정 </a><a>댓글 삭제</a></p></td></tr>");
		//		});
		//	});


		function addCommentClick(id) {
			$.ajax({
				type: "POST",
				url: "./read.aspx/ReturnBoard",
				data: "{write: '" + $("#comment_writer_textbox").val() + "', content: '" 
				+ $("#comment_contents_textbox").val() + "', id: '" + "<%=System.Guid.NewGuid().ToString()%>" + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				async: true,
				success: function (data) {
					addCommentElement(data.d);
				},
				error: function (err) {
					alert(err);
				}
			});
		}


		//테이블에 댓글 추가 동작
		function addCommentElement(id) {
		var str = id.split(';');
		$("#commentTable").prepend("<tr><td><b>Writer </b>" +
		str[0]
		+ "</td><td><b>Date :</b><%=GetTodayDateString()%></td></tr><tr><td><b>Contents </b>"
		+ str[1]
		+ "<p><a href='<%=GetUpdateCommentPageUrl()%>" + str[2] + "'>댓글 수정</a><a href='<%=GetDeleteCommentPageUrl()%>" + str[2] + "'>댓글 삭제</a></p></td></tr>");
		//<%SetComment(str[2],str[0],str[1]); %>
		
		}
		
		//function ready() { $("#addCommentCommand").click(addComment("")); }
		//$(document).ready(ready);
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
		<button class="addCommentCommand" id="addCommentCommand" onclick="addCommentClick()" >jquery로 작성하기</button>
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
						<a href="<%=GetUpdateCommentUrl(item.Id)%>">댓글 수정 </a>
						<a href="<%=GetDeleteCommentUrl(item.Id)%>">댓글 삭제</a>
					</p>
				</td>
			</tr>
		<% } %>
	<%} %>
	</table>

</body>

</html>
