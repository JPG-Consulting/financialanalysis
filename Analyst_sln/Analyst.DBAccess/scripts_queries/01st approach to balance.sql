declare @cik int
set @cik = 1163302

select 
	r.cik,r.name,r.sic_id,forms.Code,subs.Period,
	tags.tag,tags.[Version],
	dims.Segments,
	nums.*
from Registrants r
inner join EdgarDatasetSubmissions subs on r.id=subs.Registrant_Id
inner join SECForms forms on subs.Form_Id = forms.Id
inner join EdgarDatasetNumbers nums on nums.Submission_Id=subs.Id
inner join EdgarDatasetTags tags on tags.id = nums.tag_id
inner join EdgarDatasetDimensions dims on dims.id = nums.Dimension_Id
where 1=1
	and cik = @cik
	and forms.Code = '10-Q'
;

select ren.*
from Registrants r
inner join EdgarDatasetSubmissions subs on r.id=subs.Registrant_Id
inner join EdgarDatasetRenderings ren on subs.Id = ren.Submission_Id
where cik = @cik
;

select ren.*
from Registrants r
inner join EdgarDatasetSubmissions subs on r.id=subs.Registrant_Id
inner join EdgarDatasetRenderings ren on subs.Id = ren.Submission_Id
where cik = @cik
;



select * from EdgarDatasetPresentations;
