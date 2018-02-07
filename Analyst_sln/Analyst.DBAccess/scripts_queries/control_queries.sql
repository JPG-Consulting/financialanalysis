
use [Analyst]
declare @datasetid int;
set @datasetid = 201701;

--estado de la ejecucion

select id,
	cast(iif(totalsubmissions != 0, cast(processedsubmissions as float)/cast(totalsubmissions as float) *100,0) as nvarchar) + '%' subs
	,cast(iif(TotalTags != 0, cast(ProcessedTags as float)/cast(TotalTags as float)*100,0) as nvarchar) + '%' tags
	,cast(iif(TotalDimensions != 0 ,cast(ProcessedDimensions as float)/cast(TotalDimensions as float) *100,0)as nvarchar) + '%' dims
	,cast(iif(totalrenders != 0 ,cast(processedrenders as float)/cast(totalrenders as float) *100,0)as nvarchar) + '%' ren
	,cast(iif(totalpresentations != 0,cast(processedpresentations as float)/cast(totalpresentations as float) *100,0)as nvarchar) + '%' pre
	,cast(iif(totalnumbers != 0 ,cast(processednumbers as float)/cast(totalnumbers as float) *100,0)as nvarchar) + '%' nums
	,cast(iif(TotalCalculations != 0 ,cast(ProcessedCalculations as float)/cast(TotalCalculations as float) *100,0)as nvarchar) + '%' calcs
	,cast(iif(TotalTexts != 0 ,cast(ProcessedTexts as float)/cast(TotalTexts as float) *100,0)as nvarchar) + '%' texts
from EdgarDatasets where id in(201701,201604);

--select * from EdgarDatasets where id=@datasetid;

--select count(1) pre from EdgarDatasetPresentations;
--------------------------------------------------------------------------------------------------------------------------------------------------------
--Para revisar performance
--https://docs.microsoft.com/en-us/sql/relational-databases/performance/monitoring-performance-by-using-the-query-store

--------------------------------------------------------------------------------------------------------------------------------------------------------
--LOG
select 
	--top 20 * 
	*
from [dbo].[Log] 
where 1=1
	--AND cast([Date] as date) >= cast(SYSDATETIME() as date)
	--and logger <> 'EdgarDatasetTagService'
	--and Level <> 'DEBUG'
	--and level in ('ERROR','FATAL')
	--and level = 'INFO'
	--and message like '%process dim.tsv%'
	--and Logger= 'EdgarDatasetDimensionService'
	--and Logger= 'EdgarDatasetTagService'
	--AND Logger = 'EdgarDatasetNumService'
	--and Logger = 'EdgarDatasetTextService'
	--and id >= 20559
order by date asc
--order by message,date asc
;

--select distinct exception from Log;

/*
delete from [dbo].[Log] 
where id <= 580
where level='DEBUG';
*/

--check missing
--exec GET_MISSING_LINE_NUMBERS 201701,'EdgarDatasetNumbers',7502895

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

