declare @datasetid int;
declare @table varchar(10);

set @datasetid = 201701;
set @table ='num';

-------------------------------------------------------------------------------------------------------------------------------------
--DUPLICATE ANALYSYS



-------------------------------------------------------------------------------------------------------------------------------------
--MISSING ANALYSYS

--status in dataset
--select id,totalnumbers,processednumbers,(totalnumbers -processednumbers) totalmissing  from EdgarDatasets where id=@datasetid;

--quantity in table 
--select count(1) quantity_in_table_num from EdgarDatasetNumbers where DatasetId = @datasetid;

/*
--missing lines
--Datasetid 201701 -- num.tsv -- range: 5939781 to 6252401 -- BEGIN
SELECT  
	--'missing line: ' + str(number) missing,* 
	number
	--,LineNumber
	--,id
FROM 
	(select top 7502895 * from Numbers where number>1) n
	left join EdgarDatasetNumbers t on t.LineNumber = n.number
where 1=1
	and t.LineNumber is null
	--and n.number <= 7502895

order by number
;
*/

--exec GET_MISSING_LINE_NUMBERS @datasetid,@table;

-------------------------------------------------------------------------------------------------------------------------------------
--SPECIFIC CASE OF MISSING LINE
/*
adsh	tag	version	ddate	qtrs	uom	dimh	iprx	value	footnote	footlen	dimn	coreg	durp	datp	dcml
17667:
0001564590-17-001812	OperatingIncomeLoss	us-gaap/2016	20150331	1	USD	0x00000000	0	140600000.0000	Includes $32.6 million of impairment losses in the fourth quarter of 2015. See Note 6  Property, Plant and Equipment and Intangible Assets. | Includes a provisional goodwill impairment of $290.0 million in the fourth quarter of 2015. See Note 7  Goodwill.	114	0		0.024658024	0.0	-5
17668:
0001564590-17-001812	OperatingIncomeLoss	us-gaap/2016	20150630	1	USD	0x00000000	0	114800000.0000		0	0		0.013698995	0.0	-5
*/
declare @DDate datetime
declare @CountOfNumberOfQuarters int
declare @UnitOfMeasure nvarchar(20)
declare @IPRX smallint
declare @Value float
declare @FootNote nvarchar(512)
declare @FootLength smallint
declare @NumberOfDimensions smallint
declare @CoRegistrant nvarchar(256)
declare @durp real
declare @datp real
declare @Decimals int
declare @Dimension_Id int
declare @Submission_Id int
declare @Tag_Id int
declare @LineNumber int
declare @EdgarDataset_Id int


--linenumber 17667
set @EdgarDataset_Id = @datasetid
set @Submission_Id = 4954;
set @Tag_Id = 389947;
set @DDate = '20150331';
set @CountOfNumberOfQuarters =1;
set @UnitOfMeasure = 'USD';
set @Dimension_Id = 1;
set @IPRX =0 ;

select *
from [dbo].[EdgarDatasetNumbers] 
where [DatasetId]=@EdgarDataset_Id
	and [SubmissionId]= @Submission_Id
	and [TagId]=@Tag_Id
	and cast([DDate] as date)=cast(@DDate as date)
	and [CountOfNumberOfQuarters]=@CountOfNumberOfQuarters
	and [UnitOfMeasure]=@UnitOfMeasure
	and [DimensionId] = @Dimension_Id
	and [IPRX] =@IPRX
	;

--linenumber 17668
set @EdgarDataset_Id = @datasetid
set @Submission_Id = 4954;
set @Tag_Id = 389947;
set @DDate = '20150630';
set @CountOfNumberOfQuarters =1;
set @UnitOfMeasure = 'USD';
set @Dimension_Id = 1;
set @IPRX = 0;

select *
from [dbo].[EdgarDatasetNumbers] 
where [DatasetId]=@EdgarDataset_Id
	and [SubmissionId]= @Submission_Id
	and [TagId]=@Tag_Id
	and cast([DDate] as date)=cast(@DDate as date)
	and [CountOfNumberOfQuarters]=@CountOfNumberOfQuarters
	and [UnitOfMeasure]=@UnitOfMeasure
	and [DimensionId] = @Dimension_Id
	and [IPRX] =@IPRX
	;

--supuestas lineas duplicadas
select * from EdgarDatasetNumbers where datasetid=201701 and LineNumber in (17667,17668) ORDER BY LineNumber;

select * from EdgarDatasetSubmissions where id=4954;
--ADSH: 0001564590-17-001812
--0001564590-17-001812	1379661	TARGA RESOURCES PARTNERS LP	4922	US	TX	HOUSTON	77002	1000 LOUISIANA	SUITE 4300	(713)584-100	US	TX	HOUSTON	77002	1000 LOUISIANA	SUITE 4300	US	DE	651295427			4-NON	1	1231	10-K	20161231	2016	FY	20170221	2017-02-17 19:08:00.0	0	1	ngls-20161231.xml	1					

select * from Registrants where id = 4538;
--CIK: 1379661

--revisando los archivos, la unica diferencia entre ambas lineas es el valor de footlen

-------------------------------------------------------------------------------------------------------------------------------------
/*
4.    NUM is a data set of all numeric XBRL facts presented on the primary financial statements. These fields comprise a unique compound key:
1)    adsh - EDGAR accession number
2)    tag - tag used by the filer
3)    version – if a standard tag, the taxonomy of origin, otherwise equal to adsh.
4)    ddate - period end date
5)    qtrs - duration in number of quarters
6)    uom - unit of measure
7)    dimh - 16-byte dimensional qualifier
8)    iprx - a sequential integer used to distinguish otherwise identical facts
*/
