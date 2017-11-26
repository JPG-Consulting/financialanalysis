CREATE PROCEDURE GET_MISSING_LINE_NUMBERS
	@datasetid int
	,@table varchar(10)
AS
BEGIN

	declare @total int;

	select @total = totalnumbers from EdgarDatasets where id=@datasetid and @table = 'num';
	--repito el select para otra tabla
	
	SELECT  
		number linenumber
	FROM 
		(
			select *
			from Numbers 
			where number>1 and number <= @total
		) n
		left join EdgarDatasetNumbers t on t.LineNumber = n.number
	where 1=1
		and t.LineNumber is null
		and @table = 'num'
	/*
	union
	... lo mismo para otra tabla
	*/
	;
END