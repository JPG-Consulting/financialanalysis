use analyst

declare @existentes int
declare @total int

select @existentes = count(1) from EdgarDatasetPresentations where datasetid=201701 --2800000

select @total = TotalPresentations from EdgarDatasets where id=201701

select 201701 periodo, @total total,@existentes existentes, @total-@existentes faltantes

 

select @existentes = count(1) from EdgarDatasetPresentations where datasetid=201604 

select @total = TotalPresentations from EdgarDatasets where id=201604

select 201604 periodo, @total total,@existentes existentes, @total-@existentes faltantes


--delete from EdgarDatasetPresentations where datasetid=201604
--update EdgarDatasets set ProcessedPresentations = 0 where id=201604