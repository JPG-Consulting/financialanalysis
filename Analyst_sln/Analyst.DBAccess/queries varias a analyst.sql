
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
;

--check de duplicados
--select name,count(1) cant from registrants group by name having count(1) > 1;

--tablas individuales
/*
select * from secforms; 
select * from sics;
select * from [dbo].[EdgarDatasetTags] where tag+version like 'ARcountry/2013';
select * from registrants;
select * from [dbo].[EdgarDatasetSubmissions];
select * from [dbo].[EdgarDatasetSubmissions] where registrant_id = (select id from registrants where cik = 811222);
*/

/*
--delete para reprocesamiento
delete from [dbo].[EdgarDatasetSubmissions];
delete from registrants;
delete from [dbo].[EdgarDatasetTags];
*/

