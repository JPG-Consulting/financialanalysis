# Objective

As investor, I want to see estimation of stock valuations

	
	
1. Milestones	
	* [ ] Data sources
		* [x] Edgar Datasets
		* [ ] Current files
			* [ ] Edgar Indexes
			* [ ] Download current submissions on demand (documental DB)
		* [ ] Prices
	* [ ] Show raw balances
		* [ ] Estimate standard Income statement
		* [ ] Historical Income statment
		* [ ] Historical cash flow
		* [ ] Homogeneizar lo mas posibles dichos estados financieros, empezar con version tentativa:
			* Ventas - CMV = Gs Brutos
			* GS Brutos - GsComer - GsAdmin - OtrosGastos = EBITDA
			* EBITDA - Amortizaciones = EBIT
			* EBIT - Intereses = EBT
			* EBT - Taxes = RdoNeto
	* [ ] FCFF
		* [ ] Project cash flows
			* [ ] Sales
			* [ ] Lot of accounting work
			* [ ] ...
		* [ ] Estimate WACC
			* [ ] Estimate E/D proportion (assumption: historical proportion will continue)
			* [ ] Estimate tax rate
			* [ ] Estimate kd
				* [ ] Long term dept
				* [ ] Interests
			* [ ] Estimate ke
				* [ ] rf --> 10y Bond
				* [ ] rm-rf = 5.5% (Damodaran)
				* [ ] Prices
	* [ ] Multiple
		* [ ] With Prices and earnings, P/E can be estimated
		* [ ] What other multiple?

2. Pending tasks:
	1. unit test
	2. Backend
		1. Login with token
		2. que si el archivo no esta avise, no son tantos como para automatizar la descarga
		3. que haga unzip y cree la carpeta correspondiente
		
	3. Frontend
		1. Pending tasks?

		2. Used frameworks:
			* Angular 
				* version 1.4.9

			* Bootstrap 
				* version 4.0.0-beta
				* It is used for styling.

			* Angular simple pagination
				* version 1.0.5
				* https://www.npmjs.com/package/angular-simple-pagination
				* It's under MIT License (https://spdx.org/licenses/MIT.html)
				* It is used to reuse standard behaviors.

			* Jasmine 
				* version 2.0
				* It is used for unit testing
			
3. Links
	* Markdowns: 
	
	https://guides.github.com/features/mastering-markdown/
	
	* Edgar Financial Statement and Notes Data Sets
	
	https://www.sec.gov/dera/data/financial-statement-and-notes-data-set.html
	
