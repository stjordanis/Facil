﻿CREATE PROCEDURE [dbo].[ProcWithOutParamsAndRetVal]
  @setOut1 BIT,
  @out1 INT OUTPUT,
  @setOut2 BIT,
  @out2 NVARCHAR(50) = NULL OUTPUT,
  @baseRetVal INT
AS

IF @setOut1 = 1 SET @out1 = 123
IF @setOut2 = 1 SET @out2 = 'test'

RETURN @baseRetVal + COALESCE(@out1, 0)
