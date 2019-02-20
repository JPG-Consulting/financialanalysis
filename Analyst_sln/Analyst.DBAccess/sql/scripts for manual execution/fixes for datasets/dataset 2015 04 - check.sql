select * from EdgarDatasets where Id in (201504)
select * from EdgarDatasetCalculations where DatasetId = 201504 and LineNumber > 601775 and LineNumber <601780 order by LineNumber
select 1 from EdgarDatasetCalculations where LineNumber = 601778 and DatasetId = 201504