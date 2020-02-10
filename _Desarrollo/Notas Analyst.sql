use Analyst_Edgar_dev

select * from EdgarDatasets where TotalSubmissions > 0 order by id

----select * from MasterIndexes where year = 2016 and quarter = 1
--select count(*) from IndexEntries where MasterIndexId = 93

select id,date,level,logger,Exception,Message 
from Log 
where 1=1 
--and logger <> 'EdgarDatasetCalculationService' -- dataset 201801 y 201802
--and Level <> 'INFO' --only errors
order by date 

--delete from log









