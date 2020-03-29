USE [Analyst_Edgar_dev]
GO

/*
Issue
the FK to tag table (tag+version) doesn't exist in Tag table
column names

original values
adsh					grp	arc	negative	ptag																			pversion		ctag																			cversion
0001193125-15-276703	17	1	1			FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014

fixed values
adsh					grp	arc	negative	ptag																			pversion		ctag																			cversion
0001193125-15-276703	17	1	1			FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2015	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2015

*/
declare @DatasetId int = 201503

declare @submissionid int = (select id from EdgarDatasetSubmissions where ADSH = '0001193125-15-276703')
declare @SequentialNumberForGrouping smallint = 17;
declare @SequentialNumberForArc smallint = 1
declare @Negative bit = 1
declare @ParentTagId int = (select id from EdgarDatasetTags where Tag = 'FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized' and version = 'us-gaap/2015' and DatasetId = @DatasetId)
declare @ChildTagId int = @ParentTagId
declare @LineNumber int = 601530 


if(not exists(select 1 from EdgarDatasetCalculations where LineNumber = @LineNumber and DatasetId = @DatasetId)) 
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

	update EdgarDatasets 
	set ProcessedCalculations = (select count(1) from EdgarDatasetCalculations where DatasetId = @DatasetId) 
	where id = @DatasetId;
	commit tran t1
end
GO