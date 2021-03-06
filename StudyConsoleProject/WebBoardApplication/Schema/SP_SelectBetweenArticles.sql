USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectBetweenArticles]    Script Date: 10/24/2014 09:59:54 ******/
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
	--OVER()의 ORDER BY를 기준으로 정렬을 시킨 다음, IDX를 만들어 차례대로 번호를 부여해준다.
	-- 번호는 IDX라는 컬럼에 넣고 IDX를 가지고 시작과 끝 번호를 가진 데이터를 선택한다.
	SELECT * FROM ( 
	SELECT ROW_NUMBER() OVER (ORDER BY DATE DESC) AS IDX, * FROM ARTICLE) ARTICLE 
	WHERE ARTICLE.IDX BETWEEN @StartNo AND @EndNo
END
