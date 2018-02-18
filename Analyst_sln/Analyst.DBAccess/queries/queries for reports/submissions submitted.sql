--select count(1) from EdgarDatasetSubmissions --26558

select sub.DatasetId,cast(sub.period as date) periodo, r.Name,forms.Code,forms.Description
from EdgarDatasetSubmissions sub
inner join Registrants r on sub.RegistrantId = r.Id
inner join SECForms forms on sub.SECFormId = forms.Id
--where forms.Code not in ('10-Q','10-K','10-K/A','10-Q/A')
