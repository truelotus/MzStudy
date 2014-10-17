USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteArticle]    Script Date: 10/17/2014 17:27:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하
-- Create date: 2014/10/13
-- Description:	해당 ID를 가진 게시글을 삭제 합니다.
-- =============================================
ALTER PROCEDURE [dbo].[Sp_DeleteArticle]
	-- Add the parameters for the stored procedure here
	@Id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM ARTICLE_INFORMATION WHERE ID = @Id
END
