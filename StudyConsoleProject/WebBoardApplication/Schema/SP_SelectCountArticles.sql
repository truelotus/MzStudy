USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectCountArticles]    Script Date: 10/17/2014 12:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectCountArticles]
	-- Add the parameters for the stored procedure here
	@TableName varchar(50),
	@Count int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(@Count) FROM ARTICLE_INFORMATION
END
