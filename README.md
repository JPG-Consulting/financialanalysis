Objective
	As investor, I want to see estimation of stock valuations

	
	
1. Milestones	
	1. [ ] Data sources
		1. [x] Edgar Datasets
		2. [ ] Current files
			1. [ ] Edgar Indexes
			2. [ ] Download current submissions on demand (documental DB)
		3. [ ] Prices
	2. [ ] Show raw balances
		1. [ ] Estimate standard Income statement
		2. [ ] Historical Income statment
		3. [ ] Historical cash flow
		4. Homogeneizar lo mas posibles dichos estados financieros, empezar con version tentativa:
			Ventas - CMV = Gs Brutos
			GS Brutos - GsComer - GsAdmin - OtrosGastos = EBITDA
			EBITDA - Amortizaciones = EBIT
			EBIT - Intereses = EBT
			EBT - Taxes = RdoNeto
	3. [ ] FCFF
		1. [ ] Project cash flows
			1. [ ] Sales
			2. Lot of accounting work
			3. ...
		2. [ ] Estimate WACC
			1. [ ] Estimate E/D proportion (assumption: historical proportion will continue)
			2. [ ] Estimate tax rate
			3. [ ] Estimate kd
				1. [ ] Long term dept
				2. [ ] Interests
			4. [ ] Estimate ke
				1. [ ] rf --> 10y Bond
				2. [ ] rm-rf = 5.5% (Damodaran)
				3. [ ] Prices
	4. [ ] Multiple
		1. [ ] With Prices and earnings, P/E can be estimated
		2. [ ] What other multiple?

2. Pending tasks:
	1. unit test
	2. Backend
		1. Login with token
		2. que si el archivo no esta avise, no son tantos como para automatizar la descarga
		3. que haga unzip y cree la carpeta correspondiente
		
	3. Frontend
		1. Pending tasks?

		2. Used frameworks:
			Angular 
				version 1.4.9

			Bootstrap 
				version 4.0.0-beta
				It is used for styling.

			Angular simple pagination
				version 1.0.5
				https://www.npmjs.com/package/angular-simple-pagination
				It's under MIT License (https://spdx.org/licenses/MIT.html)
				It is used to reuse standard behaviors.

			Jasmine 
				version 2.0
				It is used for unit testing
			
3. Links
	* Markdowns
	https://guides.github.com/features/mastering-markdown/
	
	* Edgar Financial Statement and Notes Data Sets
	https://www.sec.gov/dera/data/financial-statement-and-notes-data-set.html
	
