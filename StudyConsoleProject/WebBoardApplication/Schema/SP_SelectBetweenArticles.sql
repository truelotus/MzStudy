USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectBetweenArticles]    Script Date: 10/17/2014 15:44:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하
-- Create date: 2014/10/17
-- Description:	카운트 기준으로 시작~끝 위치에 존재하는 게시글 데이터를 날짜 오름차순으로 추출
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectBetweenArticles]

	@StartNo int,
	@EndNo int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM ( 
	SELECT ROW_NUMBER() OVER (ORDER BY DATE DESC) AS IDX, * FROM ARTICLE_INFORMATION) ARTICLE 
	WHERE ARTICLE.IDX BETWEEN @StartNo AND @EndNo
END
