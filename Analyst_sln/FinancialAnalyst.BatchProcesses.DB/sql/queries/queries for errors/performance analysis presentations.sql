
/*
--execution took two days to load a half of records
--delete from [EdgarDatasetPresentations]
--(2930426 row(s) affected)

select id
	,cast(iif(totalpresentations != 0,cast(processedpresentations as float)/cast(totalpresentations as float) *100,0)as nvarchar) + '%' pre
	,totalpresentations
	,processedpresentations --2930426
from EdgarDatasets
where id=201701;

--update EdgarDatasets set processedpresentations = 0 where id=201701
*/
---------------------------------------------------------------------------------------------------------

select * from EdgarDatasetPresentations;