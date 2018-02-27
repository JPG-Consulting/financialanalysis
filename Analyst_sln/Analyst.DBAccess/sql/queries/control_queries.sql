
use [Analyst]
declare @datasetid int;
set @datasetid = 201701;

--estado de la ejecucion

select id,
	cast(iif(totalsubmissions != 0, cast(processedsubmissions as float)/cast(totalsubmissions as float) *100,0) as nvarchar) + '% (pending: ' + cast((totalsubmissions-processedsubmissions) as nvarchar) +')' subs
	,cast(iif(TotalTags != 0, cast(ProcessedTags as float)/cast(TotalTags as float)*100,0) as nvarchar) + '% (' + cast((TotalTags-ProcessedTags) as nvarchar) +')'  tags
	,cast(iif(TotalDimensions != 0 ,cast(ProcessedDimensions as float)/cast(TotalDimensions as float) *100,0)as nvarchar) + '% (pending: ' + cast((TotalDimensions-ProcessedDimensions) as nvarchar) +')'  dims
	,cast(iif(totalnumbers != 0 ,cast(processednumbers as float)/cast(totalnumbers as float) *100,0)as nvarchar) + '% (pending: ' + cast((totalnumbers-processednumbers) as nvarchar) +')' nums
	,cast(iif(TotalCalculations != 0 ,cast(ProcessedCalculations as float)/cast(TotalCalculations as float) *100,0)as nvarchar) + '% (pending: ' + cast((TotalCalculations-ProcessedCalculations) as nvarchar) +')'  calcs
	,cast(iif(TotalTexts != 0 ,cast(ProcessedTexts as float)/cast(TotalTexts as float) *100,0)as nvarchar) + '% (pending: ' + cast((TotalTexts-ProcessedTexts) as nvarchar) +')' texts
	,cast(iif(totalrenders != 0 ,cast(processedrenders as float)/cast(totalrenders as float) *100,0)as nvarchar) + '% (pending: ' + cast((totalrenders-processedrenders) as nvarchar) +')' ren
	,cast(iif(totalpresentations != 0,cast(processedpresentations as float)/cast(totalpresentations as float) *100,0)as nvarchar) + '% (pending: ' + cast((totalpresentations-processedpresentations) as nvarchar) +')' pre

from EdgarDatasets where id in(201601,201602,201603,201604,201701,201702,201703,201704);

--------------------------------------------------------------------------------------------------------------------------------------------------------
--Para revisar performance
--https://docs.microsoft.com/en-us/sql/relational-databases/performance/monitoring-performance-by-using-the-query-store

--------------------------------------------------------------------------------------------------------------------------------------------------------
--LOG
select 
	*
from [dbo].[Log] 
where 1=1
	--and (Logger = 'Analyst.Services.EdgarDatasetServices.EdgarDatasetService' or Exception is not null)
--order by date asc
order by Logger, date asc
;


/*
--clear log
delete from [dbo].[Log] 
*/

/*
--clear presentations
update EdgarDatasets set ProcessedPresentations = 0 where id=201701
delete from EdgarDatasetPresentations where datasetid=201701

--clear numbers
update EdgarDatasets set ProcessedNumbers = 0 where id=201704
delete from EdgarDatasetNumbers where datasetid=201704

--clear text
update EdgarDatasets set ProcessedTexts = 0 where id=201601
delete from EdgarDatasetTexts where datasetid=201601
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------
--CHECKS

declare @total int
declare @processed int
declare @indb int

USE [Analyst]
GO
/*
SELECT [Id]
      ,[TotalSubmissions],[ProcessedSubmissions]
      ,[TotalTags],[ProcessedTags]
      ,[TotalNumbers],[ProcessedNumbers]
      ,[ProcessedDimensions],[TotalDimensions]
      ,[ProcessedRenders],[TotalRenders]
      ,[ProcessedPresentations],[TotalPresentations]
      ,[ProcessedCalculations],[TotalCalculations]
      ,[ProcessedTexts],[TotalTexts]
	FROM [dbo].[EdgarDatasets]
	where TotalSubmissions > 0
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------

