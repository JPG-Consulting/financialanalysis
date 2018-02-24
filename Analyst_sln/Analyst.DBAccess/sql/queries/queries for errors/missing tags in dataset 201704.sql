/*
Lines
	Linea 470159
	0000913760-17-000119	3	3	1	CashAndSecuritiesSegregatedUnderOtherRegulations	us-gaap/2017	CashAndSecuritiesSegregatedUnderOtherRegulations	us-gaap/2017
	Linea 470160
	0000913760-17-000119	3	4	1	CashAndSecuritiesSegregatedUnderOtherRegulations	us-gaap/2017	CashAndSecuritiesSegregatedUnderOtherRegulations	us-gaap/2017
	Linea 639693
	0000913760-17-000119	3	1	1	CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation	0000913760-17-000119	CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation	0000913760-17-000119
	Linea 639694
	0000913760-17-000119	3	2	1	CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation	0000913760-17-000119	CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation	0000913760-17-000119

Errors
	Analyst.Domain.Edgar.Exceptions.EdgarLineException: Error in file cal.tsv, line 470159 ---> 
	System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.     
	at Analyst.Services.EdgarDatasetServices.EdgarDatasetCalculationService.Parse(IAnalystRepository repository, List`1 fieldNames, List`1 fields, Int32 lineNumber) 
	in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetCalculationService.cs:line 69     
	at Analyst.Services.EdgarDatasetServices.EdgarDatasetBaseService`1.ProcessRange(String fileName, EdgarTaskState state, Tuple`2 range, String[] allLines, String header, ConcurrentBag`1 missing, ConcurrentDictionary`2 failedLines) 
	in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetBaseService.cs:line 279     
	--- End of inner exception stack trace ---

	Analyst.Domain.Edgar.Exceptions.EdgarLineException: Error in file cal.tsv, line 470160 ---> System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.     at Analyst.Services.EdgarDatasetServices.EdgarDatasetCalculationService.Parse(IAnalystRepository repository, List`1 fieldNames, List`1 fields, Int32 lineNumber) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetCalculationService.cs:line 69     at Analyst.Services.EdgarDatasetServices.EdgarDatasetBaseService`1.ProcessRange(String fileName, EdgarTaskState state, Tuple`2 range, String[] allLines, String header, ConcurrentBag`1 missing, ConcurrentDictionary`2 failedLines) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetBaseService.cs:line 279     --- End of inner exception stack trace ---

	Analyst.Domain.Edgar.Exceptions.EdgarLineException: Error in file cal.tsv, line 639693 ---> System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.     at Analyst.Services.EdgarDatasetServices.EdgarDatasetCalculationService.Parse(IAnalystRepository repository, List`1 fieldNames, List`1 fields, Int32 lineNumber) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetCalculationService.cs:line 69     at Analyst.Services.EdgarDatasetServices.EdgarDatasetBaseService`1.ProcessRange(String fileName, EdgarTaskState state, Tuple`2 range, String[] allLines, String header, ConcurrentBag`1 missing, ConcurrentDictionary`2 failedLines) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetBaseService.cs:line 279     --- End of inner exception stack trace ---

	Analyst.Domain.Edgar.Exceptions.EdgarLineException: Error in file cal.tsv, line 639694 ---> System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.     at Analyst.Services.EdgarDatasetServices.EdgarDatasetCalculationService.Parse(IAnalystRepository repository, List`1 fieldNames, List`1 fields, Int32 lineNumber) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetCalculationService.cs:line 69     at Analyst.Services.EdgarDatasetServices.EdgarDatasetBaseService`1.ProcessRange(String fileName, EdgarTaskState state, Tuple`2 range, String[] allLines, String header, ConcurrentBag`1 missing, ConcurrentDictionary`2 failedLines) in D:\Importante\Desarrollo\Sourcecode\FinanceAnalysis.Net\Analyst_sln\Analyst.Services\EdgarDatasetServices\EdgarDatasetBaseService.cs:line 279     --- End of inner exception stack trace ---
*/

select * from EdgarDatasetTags where Tag = 'CashAndSecuritiesSegregatedUnderOtherRegulations'
select * from EdgarDatasetTags where Tag = 'CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation'


/*
INSERT INTO [dbo].[EdgarDatasetTags]
           ([Tag]
           ,[Version]
           ,[Custom]
           ,[Abstract]
           ,[Datatype]
           ,[LabelText]
           ,[Documentation]
           ,[LineNumber]
           ,[DatasetId])
     VALUES
           ('CashAndSecuritiesSegregatedUnderOtherRegulations'
           ,'us-gaap/2017'
           ,0
           ,-1
           ,null
           ,null
           ,null
           ,532470
           ,201704)


INSERT INTO [dbo].[EdgarDatasetTags]
           ([Tag]
           ,[Version]
           ,[Custom]
           ,[Abstract]
           ,[Datatype]
           ,[LabelText]
           ,[Documentation]
           ,[LineNumber]
           ,[DatasetId])
     VALUES
           ('CashAndSecuritiesSegregatedUnderCommodityExchangeActRegulationExcessFundsInSegregation'
           ,'0000913760-17-000119'
           ,1
           ,-1
           ,null
           ,null
           ,null
           ,532471
           ,201704)

update EdgarDatasets set ProcessedTags = 532470, TotalTags=532470 where Id=201704

*/

select count(1) from EdgarDatasetTags where DatasetId = 201704 --532470


exec [dbo].[GET_MISSING_LINE_NUMBERS] 201704,'EdgarDatasetTags',532470 --null


select max(LineNumber) from EdgarDatasetTags  --532471