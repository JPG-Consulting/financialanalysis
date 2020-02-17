--delete from EdgarDatasetTexts
--update EdgarDatasets set ProcessedTexts =0 where id=201604;

--select * from EdgarDatasets where id=201604;
--select * from EdgarDatasetTexts order by LineNumber;


SELECT  'missing line: ' + str(n),* 
FROM 
	(select top 89 * from Numbers where n>1) n
	left join EdgarDatasetTexts t on t.LineNumber = n.n 
where t.LineNumber is null;
--falta linea 66


/*
adsh	tag	version	ddate	qtrs	iprx	lang	dcml	durp	datp	dimh	dimn	coreg	escaped	srclen	txtlen	footnote	footlen	context	value
...
linea 65
0001163302-16-000148	SharebasedCompensationArrangementBySharebasedPaymentAwardFairValueAssumptionsExpectedTerm1	us-gaap/2016	20150930	3	0	en-US	32767	0.019179106	0.0	0x8fc1a9f1d4eaa7702123f37031d313ae	1		0	3	3		0	FD2015Q3YTD_us-gaap_AwardTypeAxis_us-gaap_EmployeeStockOptionMember	P5Y
linea 66
0001163302-16-000148	SharebasedCompensationArrangementBySharebasedPaymentAwardFairValueAssumptionsExpectedTerm1	us-gaap/2016	20160930	3	0	en-US	32767	0.008219957	0.0	0x8fc1a9f1d4eaa7702123f37031d313ae	1		0	3	3		0	FD2016Q3YTD_us-gaap_AwardTypeAxis_us-gaap_EmployeeStockOptionMember	P5Y
*/

--tag
--select * from EdgarDatasetTags where tag='SharebasedCompensationArrangementBySharebasedPaymentAwardFairValueAssumptionsExpectedTerm1' and Version='us-gaap/2016';
--432898

--dimension
--select * from EdgarDatasetDimensions where DimensionH = '0x8fc1a9f1d4eaa7702123f37031d313ae';
--562

select *
from [dbo].[EdgarDatasetTexts] 
where 
	[Dataset_Id] = 201604 --@DataSetId 
	AND [Dimension_Id] = 562 --@Dimension_Id
	AND [Submission_Id] =1 --@Submission_Id
	AND [Tag_Id] =432898--@Tag_Id

/*
--THE COMPLETE HAS TO BE THE KEY, IT IS:
5.    TXT is a data set that contains the plain (no HTML) text of each non-numeric XBRL fact.  These fields comprise a unique compound key:
1)    adsh - EDGAR accession number
2)    tag – tag used by the filer
3)    version – if a standard tag, the taxonomy of origin, otherwise equal to adsh
4)    ddate - period end date
5)    qtrs - duration in number of quarters
6)    dimh - dimension hash value
7)    iprx - a sequential integer used to distinguish otherwise identical facts
*/
