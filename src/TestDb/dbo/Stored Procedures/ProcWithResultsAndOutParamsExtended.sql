﻿CREATE PROCEDURE [dbo].[ProcWithResultsAndOutParamsExtended]
  @setOut1 BIT,
  @out1 INT OUTPUT,
  @setOut2 BIT,
  @out2 NVARCHAR(50) = NULL OUTPUT
AS

-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

IF @setOut1 = 1 SET @out1 = 123
IF @setOut2 = 1 SET @out2 = 'test'

SELECT Foo = 1, Bar = 2
