use analyst


declare @cik int
declare @period int

--set @cik = 1163302 --UNITED STATES STEEL CORP
--SET @cik = 1652044 --ALPHABET INC. (Google) --http://financials.morningstar.com/income-statement/is.html?t=0P000002HD&culture=en-US&platform=sal --0001652044-17-000042
set @cik = 875582	--NORTHERN TECHNOLOGIES INTERNATIONAL CORP --https://www.sec.gov/Archives/edgar/data/875582/000117184317004086/f10q_071417p.htm  --https://finance.google.com/finance?q=NASDAQ%3ANTIC&fstype=ii&ei=Z_OIWrH7JYOFmAGtvK-ACA
set @period = 201703

--Forms
--select * from SECForms where Code in ('10-Q','10-K');--(10-Q is quartery, 10-K is annual)

--company
--select 'company---->' tabla,* from Registrants where CIK = @cik

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
	left join EdgarDatasetPresentations pre on ren.Id = pre.RenderId and pre.SubmissionId = sub.Id
where r.CIK = @cik
	and subs.DatasetId = @period
order by ren.Report,pre.Line
*/




--numbers v2
select 
	r.Name,r.CIK,sub.DatasetId,sub.period,sub.ADSH
	,case form.Code
		when '10-Q' then 'Quarterly data'
		when '10-K' then 'Annual data'
		else form.code + ' -- ' + form.Description
	 end as form
	,ren.Report,ren.MenuCategory,ren.ShortName,ren.RenderFileStr
	,pre.ReportNumber,pre.FinancialStatement,pre.Line,pre.PreferredLabel,pre.Negating
	--,pre.ADSH_Tag_Version--raw data
	,tag.Tag
	,tag.LabelText
	--,tags.Documentation --explanaition of tag
	/*
	,cast(
		case 
			when nums.DatavalueEnddate is not null then nums.DatavalueEnddate
			when nums.DatavalueEnddate is null and txt.DatavalueEnddate is not null then nums.DatavalueEnddate
			else null
		 end 
		 as date) as datavalueenddate dved
	,case 
		when num.Value is not null
			then CONCAT(format(nums.Value,'0,000.##'),' ',nums.UnitOfMeasure)
		when num.Value is null and txt.Value is not null 
			then txt.Value
		else null
	end as value
	,nums.Decimals
	*/
	,dim.Segments
	,'nums------>' tabla
	,cast(num.DatavalueEndDate as Date) fecha,format(iif(pre.Negating=1,-1,1) * num.value,'0,000') as value,num.UnitOfMeasure
	,num.CountOfNumberOfQuarters,num.IPRX,num.FootNote,num.FootLength,num.NumberOfDimensions,num.CoRegistrant,num.durp,num.datp,num.Decimals
	,'text------>' tabla
	,txt.*
from EdgarDatasetSubmissions sub
	inner join Registrants r on sub.RegistrantId = r.Id
	inner join SECForms form on sub.SECFormId = form.Id
	left join EdgarDatasetRenders ren on ren.SubmissionId = sub.Id
	left join EdgarDatasetPresentations pre on ren.Id = pre.RenderId and pre.SubmissionId = sub.Id
	left join EdgarDatasetTags tag on tag.Id = pre.TagId
	left join EdgarDatasetNumbers num on num.TagId = tag.Id and num.SubmissionId = sub.Id
	left join EdgarDatasetDimensions dim on num.DimensionId = dim.Id
	left join EdgarDatasetTexts txt on txt.TagId = tag.Id and txt.SubmissionId = sub.Id
where r.CIK = @cik
	and sub.DatasetId = @period
	and (ren.DatasetId = @period or ren.DatasetId is null)
	and (pre.DatasetId = @period or pre.DatasetId is null)
	and (tag.DatasetId = @period or tag.DatasetId is null)
	and (num.DatasetId = @period or num.DatasetId is null)
	and (dim.DatasetId = @period or dim.DatasetId is null)
	and (txt.DatasetId = @period or txt.DatasetId is null)
	--and ren.MenuCategory =  'Cover'
	and (ren.MenuCategory = 'Statements' or ren.MenuCategory = 'S')--depends on the company
	and pre.FinancialStatement = 'IS' --(CP = Cover Page, BS = Balance Sheet, IS = Income Statement, CF = Cash Flow, EQ = Equity, CI = Comprehensive Income, UN = Unclassifiable Statement).
	and num.CountOfNumberOfQuarters = 1 --Three Months Ended
	--and nums.CountOfNumberOfQuarters = 3 --Nine Months Ended
	and dim.Segments is null --if dimension is null, it's the main current statement; if dimension is not null, the it's a detail of the item (for instance: net sales by region or net sales by segment, etc)
order by ren.Report,num.DatavalueEnddate,pre.Line,dim.Segments,num.NumberOfDimensions