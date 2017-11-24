CREATE PROCEDURE SP_GET_CALCULATIONS_KEYS
	@datsetid int
AS
BEGIN
	select 
		s.ADSH+CONVERT(varchar(1),c.[SequentialNumberForGrouping])+convert(varchar(1),c.[SequentialNumberForArc]) [key]
		,c.Id
	from EdgarDatasetCalculations c inner join EdgarDatasetSubmissions s on c.SubmissionId = s.Id
	where c.DatasetId = @datsetid and s.DatasetId = @datsetid;

END
--------------------------------------------
