
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

from EdgarDatasets where id in(201604,201701,201702);

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
	--and id >= 2221700
--order by date asc
order by Logger,date asc
--order by Logger,message,date asc
--order by message,date asc
;


/*
--clear log
delete from [dbo].[Log] 
where id <= 2221370
where level='DEBUG';
*/

/*
--clear presentations
update EdgarDatasets set ProcessedPresentations = 0 where id=201701
delete from EdgarDatasetPresentations where datasetid=201701
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------
--CHECKS

/*
--check missing presentations
declare @existentes int
declare @total int
declare @dataset int = 201701
select @existentes = count(1) from EdgarDatasetPresentations where datasetid=@dataset
select @total = TotalPresentations from EdgarDatasets where id=@dataset
select @dataset periodo, @total total,@existentes existentes, @total-@existentes faltantes
*/


/*
--check missing tags
declare @total int
select @total=TotalTags from EdgarDatasets where id=201604
exec GET_MISSING_LINE_NUMBERS 201604,'EdgarDatasetTags',@total
*/

/*
--sec forms: should be 144, plus 7 duplicated, it is the 151
select count(1) from secforms; 

--duplicated check in dimensions
select name,count(1) cant from [dbo].[registrants] group by name having count(1) > 1;
select DimensionH,count(1) cant from [dbo].[EdgarDatasetDimensions]  group by DimensionH having count(1) > 1;
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------

