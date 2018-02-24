--todo: agregar linenumber en tag y dimension

SELECT  'missing line: ' + str(n),* 
FROM 
	(select top 557638 * from Numbers where n>1) n
	left join EdgarDatasetTags t on t.LineNumber = n.n 
where t.LineNumber is null

;

