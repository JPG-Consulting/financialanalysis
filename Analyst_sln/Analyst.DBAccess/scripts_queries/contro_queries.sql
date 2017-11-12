
use [Analyst]

--select count(1) from secforms; --son 144, mas los 7 duplicados son los 151 que deberian ser
--select count(1) from sics;

/*
--form no esta en el listado que informa la sec
select * from secforms where code ='10-Q/A';
select * from secforms where code like '10%';
*/

--estado de la ejecucion
select 'registrants' tabla, count(1) cant from registrants
union
select 'submissions' tabla, count(1) cant from [dbo].[EdgarDatasetSubmissions]
union
select 'tags' tabla, count(1) cant from [dbo].[EdgarDatasetTags]
union
select 'tags related' tabla, count(1) cant from [dbo].[EdgarDatasetTagEdgarDatasets] where edgardataset_id=201604
union 
select 'dimensions' tabla, count(1) cant from [dbo].[EdgarDatasetDimensions]
union
select 'dimensions related' tabla, count(1) from [dbo].[EdgarDatasetDimensionEdgarDatasets] where edgardataset_id=201604
UNION
SELECT 'numbers' tabla, count(1) from dbo.EdgarDatasetNumbers where EdgarDataset_Id = 201604
;
;
select * from EdgarDatasets where year =2016 and Quarter= 4;
--select * from [dbo].[EdgarDatasetNumbers] order by linenumber;

/*
--CHECKS

--check de duplicados
select name,count(1) cant from [dbo].[registrants] group by name having count(1) > 1;
select DimensionH,count(1) cant from [dbo].[EdgarDatasetDimensions]  group by DimensionH having count(1) > 1;

--check de subs sin relacion --> poner constraint FK
select * from [dbo].[EdgarDatasetSubmissions] where EdgarDataset_Id is null;
*/

/*
--tablas individuales
select * from secforms; 
select * from sics;
select * from registrants order by name;
select * from [dbo].[EdgarDatasetTags];
select * from [dbo].[EdgarDatasetTags] where tag ='EchoStarXVIMember';
select * from [dbo].[EdgarDatasetTags] where tag+version like 'ARcountry/2013';
select * from [dbo].[EdgarDatasetTagEdgarDatasets];
select * from [dbo].[EdgarDatasetSubmissions];
select * from [dbo].[EdgarDatasetSubmissions] where registrant_id = (select id from registrants where cik = 811222);
select * from [dbo].[EdgarDatasetDimensions];
*/

/*
--delete para reprocesamiento
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
*/

