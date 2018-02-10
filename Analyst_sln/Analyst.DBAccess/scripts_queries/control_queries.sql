
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

from EdgarDatasets where id in(201701,201604);

--------------------------------------------------------------------------------------------------------------------------------------------------------
--Para revisar performance
--https://docs.microsoft.com/en-us/sql/relational-databases/performance/monitoring-performance-by-using-the-query-store

--------------------------------------------------------------------------------------------------------------------------------------------------------
--LOG
select 
	*
from [dbo].[Log] 
where 1=1
	--AND cast([Date] as date) >= cast(SYSDATETIME() as date)
	--and Level <> 'DEBUG'
	--and level in ('ERROR','FATAL')
	--and level = 'INFO'
	--and message like '%process dim.tsv%'
	--and Logger = 'EdgarDatasetDimensionService'
	--and Logger = 'EdgarDatasetTagService'
	--and logger <> 'EdgarDatasetTagService'
	--AND Logger = 'EdgarDatasetNumService'
	--and Logger = 'EdgarDatasetTextService'
	--and id >= ?
order by date asc
--order by Logger,date asc
--order by Logger,message,date asc
--order by message,date asc
;

--select distinct exception from Log;

/*
delete from [dbo].[Log] 
where id <= 2221370
where level='DEBUG';
*/

--check missing
/*
declare @total int
select @total=TotalTags from EdgarDatasets where id=201604
exec GET_MISSING_LINE_NUMBERS 201604,'EdgarDatasetTags',@total
*/
--------------------------------------------------------------------------------------------------------------------------------------------------------
--CHECKS
/*
--sec forms: son 144, mas los 7 duplicados son los 151 que deberian ser
select count(1) from secforms; 

--deberian ser ???
select count(1) from sics;


--check de duplicados
select name,count(1) cant from [dbo].[registrants] group by name having count(1) > 1;
select DimensionH,count(1) cant from [dbo].[EdgarDatasetDimensions]  group by DimensionH having count(1) > 1;

*/


--------------------------------------------------------------------------------------------------------------------------------------------------------

