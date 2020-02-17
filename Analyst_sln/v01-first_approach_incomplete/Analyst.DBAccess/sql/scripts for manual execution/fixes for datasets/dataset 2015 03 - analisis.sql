/*
Datasetid 201503 -- cal.tsv -- line[601530]: 0001193125-15-276703 17 1 1 FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized us-gaap/2014 FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized us-gaap/2014
Datasetid 201503 -- cal.tsv -- line[601530]: Key FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized|us-gaap/2014 is not present in the Tags dictionary, line number: 601530

adsh					grp	arc	negative	ptag																			pversion		ctag																			cversion
0001193125-15-276703	17	1	1			FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014	FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized	us-gaap/2014
*/

declare @DatasetId int = 201503
declare @TotalCalculations int

select * from EdgarDatasets where id= @DatasetId
select @TotalCalculations= TotalCalculations from EdgarDatasets where id= @DatasetId
select @TotalCalculations TotalCalculations

select * 
from EdgarDatasetTags
where 1=1
	and DatasetId = @DatasetId
	and Tag = 'FinancialGuaranteeInsuranceContractsFutureExpectedPremiumRevenueToBeRecognized'


select LineNumber,'----',* 
from EdgarDatasetCalculations
where 1=1
	and DatasetId = @DatasetId
	and LineNumber >= 601528
	and LineNumber <= 601532

--exec [dbo].[GET_MISSING_LINE_NUMBERS] @DatasetId,'EdgarDatasetCalculations', @TotalCalculations --601530