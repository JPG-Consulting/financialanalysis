use analyst


declare @cik int
declare @period int

--set @cik = 1163302 --UNITED STATES STEEL CORP
SET @cik = 1652044 --ALPHABET INC. (Google) --http://financials.morningstar.com/income-statement/is.html?t=0P000002HD&culture=en-US&platform=sal
set @period = 201701

--company
--select 'company---->' tabla,* from Registrants where CIK = @cik


--Forms
--select * from SECForms where Code in ('10-Q','10-K');--(10-Q is quartery, 10-K is annual)

/*
--submissions
select 'subs--->' tabla,* 
from EdgarDatasetSubmissions subs
inner join Registrants r on subs.RegistrantId = r.Id
where r.CIK = @cik
	and subs.DatasetId = @period
*/

--reports
/*
select 
	subs.Id SubmissionId
	,'reports---->' tabla
	,ren.*
from EdgarDatasetSubmissions subs
	inner join Registrants r on subs.RegistrantId = r.Id
	inner join EdgarDatasetRenders ren on ren.SubmissionId = subs.Id
where r.CIK = @cik
	and subs.DatasetId = @period
order by menucategory
;
*/

--presentations
/*
select 
	r.Name,r.CIK,subs.DatasetId,subs.period
	--,'renders---->' tabla
	,ren.Report,ren.MenuCategory,ren.ShortName,ren.RenderFileStr
	,'presentations ---->' tabla
	,pre.ReportNumber,pre.FinancialStatement,pre.Line,pre.PreferredLabel,pre.Negating,pre.TagId,pre.NumberId,pre.TextId,pre.ADSH_Tag_Version
from EdgarDatasetSubmissions subs
	inner join Registrants r on subs.RegistrantId = r.Id
	left join EdgarDatasetRenders ren on ren.SubmissionId = subs.Id
	left join EdgarDatasetPresentations pre on ren.Id = pre.RenderId
where r.CIK = @cik
	and subs.DatasetId = @period
order by ren.Report,pre.Line
*/



select 
	r.Name,r.CIK,sub.DatasetId,sub.period
	,case form.Code
		when '10-Q' then 'Quarterly data'
		when '10-K' then 'Annual data'
		else form.code + ' -- ' + form.Description
	 end as form
	,ren.Report,ren.MenuCategory,ren.ShortName,ren.RenderFileStr
	,pre.ReportNumber,pre.FinancialStatement,pre.Line,pre.PreferredLabel,pre.Negating
	--,pre.ADSH_Tag_Version--raw data
	,tag.LabelText
	--,tags.Documentation --explanaition of tag
	,cast(
		case 
			when num.DatavalueEnddate is not null then num.DatavalueEnddate
			when num.DatavalueEnddate is null and txt.DatavalueEnddate is not null then num.DatavalueEnddate
			else null
		 end 
		 as date) as datavalueenddate
	,case 
		when num.Value is not null
			then CONCAT(format(num.Value,'0,000.##'),' ',num.UnitOfMeasure)
		when num.Value is null and txt.Value is not null 
			then txt.Value
		else null
	end as value
	,num.Decimals
from EdgarDatasetSubmissions sub
	inner join Registrants r on sub.RegistrantId = r.Id
	inner join SECForms form on sub.SECFormId = form.Id
	left join EdgarDatasetRenders ren on ren.SubmissionId = sub.Id
	left join EdgarDatasetPresentations pre on ren.Id = pre.RenderId
	left join EdgarDatasetTags tag on tag.Id = pre.TagId
	left join EdgarDatasetNumbers num on num.Id = pre.NumberId
	left join EdgarDatasetTexts txt on txt.Id = pre.TextId
where r.CIK = @cik
	and sub.DatasetId = @period
	and ren.MenuCategory = 'Statements' and pre.FinancialStatement = 'IS' --(CP = Cover Page, BS = Balance Sheet, IS = Income Statement, CF = Cash Flow, EQ = Equity, CI = Comprehensive Income, UN = Unclassifiable Statement).
	--and ren.MenuCategory =  'Cover'
order by ren.Report,pre.Line