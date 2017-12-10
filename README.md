Falta:
	Backend
		* Solo quedaria procesar el pre.tsv para que procese un dataset completo
		* crear backend con angular:
			- que en la misma pagina pueda ver
				estado de datasets --> debe tener un timer
				sics
				forms
				registrants
			- login con token
		* que si el archivo no esta avise, no son tantos como para automatizar la descarga
		* que haga unzip y cree la carpeta correspondiente
		
	Frontend
		1) armar los estados financieros historicos
			Estado de resultados
			Flujo de fondos
		2) Bajar precios del dia
		3) si esta 1 y 2 puedo: calcular P/E (historico)
		4) homogeneizar lo mas posibles dichos estados financieros, empezar con version tentativa:
			Ventas - CMV = Gs Brutos
			GS Brutos - GsComer - GsAdmin - OtrosGastos = EBITDA
			EBITDA - Amortizaciones = EBIT
			EBIT - Intereses = EBT
			EBT - Taxes = RdoNeto
		5) Pensar que otros multiplos puedo calcular

		10) Used frameworks:
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
			

