USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateHits]    Script Date: 10/24/2014 10:04:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하
-- Create date: 2014/10/17
-- Description: 해당 게시물의 조회수를 업데이트 합니다.
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdateHits]
	-- Add the parameters for the stored procedure here
	@Id varchar(50)
AS
BEGIN
	UPDATE ARTICLE SET HITS = HITS+1  WHERE ID = @Id
END
