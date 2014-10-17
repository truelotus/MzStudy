USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectAllArticles]    Script Date: 10/17/2014 17:30:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하
-- Create date: 2014/10/13
-- Description:	Database에 있는 모든 게시글을 가져옵니다.
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectAllArticles]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM ARTICLE_INFORMATION ORDER BY DATE DESC
END
