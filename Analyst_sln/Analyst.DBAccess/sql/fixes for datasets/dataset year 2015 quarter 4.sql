USE [Analyst_EdgarDatasets_dev]
GO

/*
Issue
the FK to tag table (tag+version) doesn't exist in Tag table
column names
adsh	grp	arc	negative	ptag	pversion	ctag	cversion
original values
0001193125-15-366241	18	1	1	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014
fixed values
0001193125-15-366241	18	1	1	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2015	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2015
*/

declare @submissionid int = (select id from EdgarDatasetSubmissions where ADSH = '0001193125-15-366241')
declare @SequentialNumberForGrouping smallint = 18;
declare @SequentialNumberForArc smallint = 1
declare @Negative bit = 1
declare @ParentTagId int = (select id from EdgarDatasetTags where Tag = 'FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized' and version = 'us-gaap/2015')
declare @ChildTagId int = @ParentTagId
declare @LineNumber int = 601778 --exec [dbo].[GET_MISSING_LINE_NUMBERS] 201504,'EdgarDatasetCalculations',630214 
declare @DatasetId int = 201504

if(not exists(select 1 from EdgarDatasetCalculations where LineNumber = @LineNumber)) 
BEGIN
	begin tran T1;
	INSERT INTO [dbo].[EdgarDatasetCalculations]
			   ([SubmissionId]
			   ,[SequentialNumberForGrouping]
			   ,[SequentialNumberForArc]
			   ,[Negative]
			   ,[LineNumber]
			   ,[DatasetId]
			   ,[ParentTagId]
			   ,[ChildTagId])
		 VALUES
			   (@SubmissionId
			   ,@SequentialNumberForGrouping
			   ,@SequentialNumberForArc
			   ,@Negative
			   ,@LineNumber
			   ,@DatasetId
			   ,@ParentTagId
			   ,@ChildTagId);

	update EdgarDatasets set ProcessedCalculations = ProcessedCalculations + 1 where id = 201504;
	commit tran t1
end
GO