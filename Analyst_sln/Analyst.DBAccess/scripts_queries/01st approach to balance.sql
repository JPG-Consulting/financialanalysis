declare @cik int
set @cik = 1163302


--reports
select * 
from EdgarDatasetRenders 
where SubmissionId = (select SubmissionId	from Registrants r inner join EdgarDatasetSubmissions subs on r.id=subs.RegistrantId where CIK = @cik)
;


/*
select * from SECForms 
where Code in ('10-Q','10-K');--(quartery, annual)
*/

select 
	PreferredLabel,Negating,Value,UnitOfMeasure
	--,'---' [all-->],*
from
(
	select 
		r.Name,r.CIK
		,subs.ADSH
		,forms.Code
		,'---' [ren-->],ren.ShortName
		,'---' [pre-->],pre.Line,pre.FinancialStatement,pre.PreferredLabel,pre.Negating,pre.NumberId,pre.TextId
		,'---' [tags-->],tags.Tag,tags.Custom,tags.Abstract,tags.Datatype,tags.Tlabel,tags.Doc
		,'---' [nums-->],cast(nums.DDate as date) ddate,nums.UnitOfMeasure,nums.Value,nums.FootNote,nums.FootLength,nums.NumberOfDimensions,nums.CoRegistrant,nums.durp,nums.datp,nums.Decimals
	from Registrants r
	inner join EdgarDatasetSubmissions subs on r.id=subs.RegistrantId
	inner join SECForms forms on subs.SECFormId = forms.Id
	inner join EdgarDatasetRenders ren on subs.Id = ren.SubmissionId
	inner join EdgarDatasetPresentations pre on pre.RenderId = ren.Id
	inner join EdgarDatasetTags tags on pre.TagId = tags.Id
	left join EdgarDatasetNumbers nums on pre.NumberId = nums.Id
	where 1=1
		and cik = @cik
		and forms.Code = '10-Q'
		--and ren.Report = 2 --Consolidated Statement Of Operations
		--and ren.Report = 3 --Consolidated Statement Of Comprehensive Income (Loss)
		--and ren.Report = 4--Consolidated Balance Sheet
		and ren.Report = 6--Consolidated Statement Of Cash Flows
	--order by pre.Line
) tabla
order by Line
;