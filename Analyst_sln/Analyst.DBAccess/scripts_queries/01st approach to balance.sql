select r.cik,r.name,r.sic_id,forms.Code,subs.Period,tags.tag,tags.[Version],dims.Segments,nums.*
from Registrants r
inner join EdgarDatasetSubmissions subs on r.id=subs.Registrant_Id
inner join SECForms forms on subs.Form_Id = forms.Id
inner join EdgarDatasetNumbers nums on nums.Submission_Id=subs.Id
inner join EdgarDatasetTags tags on tags.id = nums.tag_id
inner join EdgarDatasetDimensions dims on dims.id = nums.Dimension_Id
where cik = 1163302
;

