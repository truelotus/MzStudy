USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectAllArticles]    Script Date: 10/24/2014 08:55:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하	
-- Create date: 2014/10/17
-- Description: 작성된 게시글 전체 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectAllArticles]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM ARTICLE ORDER BY DATE DESC
END
