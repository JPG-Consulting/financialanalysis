
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
from EdgarDatasets where id =@datasetid;

--select * from EdgarDatasets where id=@datasetid;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--LOG
select 
	--top 20 * 
	*
from [dbo].[Log] 
where 1=1
	AND cast([Date] as date) < cast(SYSDATETIME() as date)
	--and logger <> 'EdgarDatasetTagService'
	--and Level <> 'DEBUG'
	--and level in ('ERROR','FATAL')
	--and level = 'INFO'
	--and message like '%process dim.tsv%'
	--AND Logger = 'EdgarDatasetNumService'
	--and id >= 20489
order by date asc
--order by message 
;

/*
delete from [dbo].[Log] 
where level='DEBUG';
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------
--CHECKS
/*
--sec forms: son 144, mas los 7 duplicados son los 151 que deberian ser
select count(1) from secforms; 

--deberian ser ???
select count(1) from sics;

--form no esta en el listado que informa la sec
select * from secforms where code ='10-Q/A';
select * from secforms where code like '10%';

--check de duplicados
select name,count(1) cant from [dbo].[registrants] group by name having count(1) > 1;
select DimensionH,count(1) cant from [dbo].[EdgarDatasetDimensions]  group by DimensionH having count(1) > 1;

--check de subs sin relacion --> poner constraint FK
select * from [dbo].[EdgarDatasetSubmissions] where EdgarDataset_Id is null;
*/


--------------------------------------------------------------------------------------------------------------------------------------------------------
--tablas individuales
/*
select * from secforms; 
select * from sics;
select * from registrants order by name;

--core data
select * from [dbo].[EdgarDatasetSubmissions];
select * from [dbo].[EdgarDatasetSubmissions] where registrant_id = (select id from registrants where cik = 811222);
select * from [dbo].[EdgarDatasetTags];
select * from [dbo].[EdgarDatasetTags] where tag ='EchoStarXVIMember';
select * from [dbo].[EdgarDatasetTags] where tag+version like 'ARcountry/2013';
select * from [dbo].[EdgarDatasetEdgarDatasetTags];
select * from [dbo].[EdgarDatasetDimensions];

--related data
select * from [dbo].[EdgarDatasetRenderings];
select * from [dbo].[EdgarDatasetPresentations]
select * from [dbo].[EdgarDatasetCalculations];
select * from [dbo].[EdgarDatasetTexts];
select * from [dbo].[EdgarDatasetNumbers];
*/

--------------------------------------------------------------------------------------------------------------------------------------------------------
--delete para reprocesamiento
/*
delete from [dbo].[Log];
delete from [dbo].[EdgarDatasetSubmissions];
delete from [dbo].[EdgarDatasetTags];
delete from [dbo].[EdgarDatasetTagEdgarDatasets];
delete from [dbo].[EdgarDatasetDimensions];
update EdgarDatasets set 
	  [TotalSubmissions] = 0
      ,[ProcessedSubmissions] = 0
      ,[TotalTags] = 0
      ,[ProcessedTags] = 0
      ,[TotalNumbers] = 0
      ,[ProcessedNumbers] = 0
      ,[ProcessedDimensions] = 0
      ,[TotalDimensions] = 0
	where year =2016 and Quarter= 4;

update EdgarDatasets set year =2016, Quarter= 4 where id=201604;

*/

--------------------------------------------------------------------------------------------------------------------------------------------------------
--ejemplo para encontrar linea faltante
--el archivo deberia tener 89 lineas
--la fila que tenga todo en null va a ser la fila no insertada/procesada
/*
SELECT  * 
FROM 
	(select top 89 * from Numbers where n>1) n --linea 1 es el header
	left join EdgarDatasetTexts t on t.LineNumber = n.n 
where t.LineNumber is null;

*/