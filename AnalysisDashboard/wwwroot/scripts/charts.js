function GenerateChart() {

// CHART 1 START ========================================

	var visitorsData1 = {
		"Rasxod1": [{
			
			explodeOnClick: false,
			innerRadius: "50%",
			radius: "90%",
			yValueFormatString: "# ### ##0.## сум",
			startAngle: 90,
			type: "doughnut",
			indexLabel: "#percent%",
			percentFormatString: "#0.##",
			toolTipContent: "{y} (#percent%)",
			dataPoints: [
				{ y: 100000.05, name: "Комиссия банка", indexLabel: "{name}", color: "#33a8c7", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Налоги за период", indexLabel: "{name}", color: "#52e3e1", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Корпоративная карта", indexLabel: "{name}", color: "#a0e426", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Месячная заработная плата", indexLabel: "{name}", color: "#fdf148", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Покупка услуг", indexLabel: "{name}", color: "#ffab00", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Покупка основных средств", indexLabel: "{name}", color: "#f77976", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Финансовые займы", indexLabel: "{name}", color: "#f050ae", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Выплата кредита", indexLabel: "{name}", color: "#d883ff", cursor: "pointer", click: DrillDown1},
				{ y: 100000.05, name: "Коммунальные расходы", indexLabel: "{name}", color: "#9336fd", cursor: "pointer", click: DrillDown1}
			]
		}],
		"Комиссия банка": [{
			color: "#33a8c7",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 }
			]
		}],
		"Налоги за период": [{
			color: "#52e3e1",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 }
			]
		}],
		"Корпоративная карта": [{
			color: "#a0e426",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Бензозаправка", indexLabel: "{y}", y: 500000 },
				{ label:"Газозаправка", indexLabel: "{y}", y: 500000 },
				{ label:"Техника", indexLabel: "{y}", y: 1550000 },
				{ label:"Услуги", indexLabel: "{y}", y: 70000 }
			]
		}],
		"Месячная заработная плата": [{
			color: "#fdf148",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Элшод", indexLabel: "{y}", y: 25000000.15 },
				{ label:"Дилшод", indexLabel: "{y}", y: 50000000.15 },
				{ label:"Шамшод", indexLabel: "{y}", y: 33350000.15 }
			]
		}],
		"Покупка услуг": [{
			color: "#ffab00",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 }
			]
		}],
		"Покупка основных средств": [{
			color: "#f77976",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 },
				{ label:"Разбивка", indexLabel: "{y}", y: 50000.15 }
			]
		}],
		"Финансовые займы": [{
			color: "#f050ae",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Akfa", indexLabel: "{y}", y: 5000015 },
				{ label:"Coca-Cola", indexLabel: "{y}", y: 50000815 },
				{ label:"Artel", indexLabel: "{y}", y: 40000015 },
				{ label:"Pepsi", indexLabel: "{y}", y: 15000015 }
			]
		}],
		"Выплата кредита": [{
			color: "#d883ff",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"01.07.2023", indexLabel: "{y}", y: 1500000 },
				{ label:"01.08.2023", indexLabel: "{y}", y: 1500000 },
				{ label:"01.09.2023", indexLabel: "{y}", y: 1500000 },
				{ label:"01.10.2023", indexLabel: "{y}", y: 1500000 },
				{ label:"01.11.2023", indexLabel: "{y}", y: 1500000 },
				{ label:"01.12.2023", indexLabel: "{y}", y: 1500000 }
			]
		}],
		"Коммунальные расходы": [{
			color: "#9336fd",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Электр", indexLabel: "{y}", y: 50000.15 },
				{ label:"Газ", indexLabel: "{y}", y: 40000.15 },
				{ label:"Вода", indexLabel: "{y}", y: 20000.15 },
				{ label:"Вывоз мусора", indexLabel: "{y}", y: 15000.15 },
				{ label:"Другие", indexLabel: "{y}", y: 30000.15 }
			]
		}]
	};

	var Options1 = {
		// animationEnabled: true,
		// theme: "light2",
		// title: {
		// 	fontFamily: "'Roboto', sans-serif",
		// 	fontWeight: "bold",
		// 	fontSize: 16,
		// 	text: "Расход"
		// },
		title: {
			dockInsidePlotArea: true,
			fontSize: 14,
			fontFamily: "'Roboto', sans-serif",
			fontWeight: "bold",
			horizontalAlign: "center",
			verticalAlign: "center",
			text: "100 000 Сум"
		},
		legend: {
			fontFamily: "'Roboto', sans-serif",
			fontSize: 14
		},
		data: []
	};

	var ChartOptions1 = {
		animationEnabled: true,
		theme: "light1",
		axisX: {
			labelFontColor: "#717171",
			lineColor: "#a2a2a2",
			tickColor: "#a2a2a2"
		},
		axisY: {
			gridThickness: 0,
			includeZero: false,
			labelFontColor: "#717171",
			lineColor: "#a2a2a2",
			tickColor: "#a2a2a2",
			lineThickness: 1
		},
		data: []
	};

	var chart1 = new CanvasJS.Chart("chartContainer1", Options1);
	chart1.options.data = visitorsData1["Rasxod1"];
	chart1.render();

	function DrillDown1(e) {
		chart1 = new CanvasJS.Chart("chartContainer1", ChartOptions1);
		chart1.options.data = visitorsData1[e.dataPoint.name];
		chart1.options.title = { text: e.dataPoint.name }
		chart1.render();
		$("#backButton1").toggleClass("invisible1");
	}

	$("#backButton1").click(function() { 
		$(this).toggleClass("invisible1");
		chart1 = new CanvasJS.Chart("chartContainer1", Options1);
		chart1.options.data = visitorsData1["Rasxod1"];
		chart1.render();
	});

// CHART 1 END ========================================

// CHART 2 START ========================================

	var visitorsData2 = {
		"Prixod2": [{
			
			explodeOnClick: false,
			innerRadius: "50%",
			radius: "90%",
			yValueFormatString: "# ### ##0.## сум",
			startAngle: 90,
			type: "doughnut",
			indexLabel: "#percent%",
			percentFormatString: "#0.##",
			toolTipContent: "{y} (#percent%)",
			dataPoints: [
				{ y: 100000.05, name: "Терминал UzCard", indexLabel: "{name}", color: "#f94144"},
				{ y: 100000.05, name: "Терминал Humo", indexLabel: "{name}", color: "#f3722c"},
				{ y: 100000.05, name: "Наличные (пополнение)", indexLabel: "{name}", color: "#f8961e"},
				{ y: 100000.05, name: "Поступления через платежные приложения", indexLabel: "{name}", color: "#f9844a", cursor: "pointer", click: DrillDown2},
				{ y: 100000.05, name: "Переводы на счет (от партнеров или источников)", indexLabel: "{name}", color: "#f9c74f", cursor: "pointer", click: DrillDown2},
				{ y: 100000.05, name: "Кэшбеки", indexLabel: "{name}", color: "#90be6d"},
				{ y: 100000.05, name: "Проценты с депозитов", indexLabel: "{name}", color: "#43aa8b"},
				{ y: 100000.05, name: "Возврат финзаймов", indexLabel: "{name}", color: "#4d908e"},
				{ y: 100000.05, name: "Прочие поступления", indexLabel: "{name}", color: "#577590"}
			]
		}],
		"Поступления через платежные приложения": [{
			color: "#f9844a",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"Click", indexLabel: "{y}", y: 123450.15 },
				{ label:"Payme", indexLabel: "{y}", y: 12400.23 },
				{ label:"Uzum", indexLabel: "{y}", y: 107100.03 },
				{ label:"Paylov", indexLabel: "{y}", y: 92400.23 }
			]
		}],
		"Переводы на счет (от партнеров или источников)": [{
			color: "#f9c74f",
			type: "column",
			yValueFormatString: "# ### ##0.## сум",
			dataPoints: [
				{ label:"01.01.2023", indexLabel: "{y}", y: 45015000 },
				{ label:"02.02.2023", indexLabel: "{y}", y: 35005000 },
				{ label:"03.03.2023", indexLabel: "{y}", y: 38005000 },
				{ label:"04.04.2023", indexLabel: "{y}", y: 39005000 },
				{ label:"05.05.2023", indexLabel: "{y}", y: 37005000 },
				{ label:"06.06.2023", indexLabel: "{y}", y: 40023000 }
			]
		}]
	};

	var Options2 = {
		// animationEnabled: true,
		// title: {
		// 	fontFamily: "'Roboto', sans-serif",
		// 	fontWeight: "bold",
		// 	fontSize: 16,
		// 	text: "Прибыль"
		// },
		title: {
			dockInsidePlotArea: true,
			fontSize: 14,
			fontFamily: "'Roboto', sans-serif",
			fontWeight: "bold",
			horizontalAlign: "center",
			verticalAlign: "center",
			text: "50 000 000 Сум"
		},
		legend: {
			fontFamily: "'Roboto', sans-serif",
			fontSize: 14
		},
		data: []
	};

	var ChartOptions2 = {
		animationEnabled: true,
		theme: "light1",
		axisX: {
			labelFontColor: "#717171",
			lineColor: "#a2a2a2",
			tickColor: "#a2a2a2"
		},
		axisY: {
			gridThickness: 0,
			includeZero: false,
			labelFontColor: "#717171",
			lineColor: "#a2a2a2",
			tickColor: "#a2a2a2",
			lineThickness: 1
		},
		data: []
	};

	var chart2 = new CanvasJS.Chart("chartContainer2", Options2);
	chart2.options.data = visitorsData2["Prixod2"];
	chart2.render();

	function DrillDown2(e) {
		chart2 = new CanvasJS.Chart("chartContainer2", ChartOptions2);
		chart2.options.data = visitorsData2[e.dataPoint.name];
		chart2.options.title = { text: e.dataPoint.name }
		chart2.render();
		$("#backButton2").toggleClass("invisible2");
	}

	$("#backButton2").click(function() { 
		$(this).toggleClass("invisible2");
		chart2 = new CanvasJS.Chart("chartContainer2", Options2);
		chart2.options.data = visitorsData2["Prixod2"];
		chart2.render();
	});

// CHART 2 END ========================================

// CHART 3 START ========================================
	 


// CHART 3 END ========================================

}

var debitBarList = new Array();
var creditBarList = new Array();

var debitList = new Array();
var creditList = new Array();

function addDebitBar(info) {
	debitBarList.push({ y: info.amount, name: info.name, indexLabel: "{name}", color: info.color, cursor: "pointer", code: info.code });
}
function addBarCredit(info) {
	creditBarList.push({ y: info.amount, name: info.name, indexLabel: "{name}", color: info.color, cursor: "pointer", code: info.code });
}

function addDebit(info) {
	debitList.push({ x: new Date(info.year, info.month - 1, info.day), y: info.amount }); 
}
function addCredit(info) {
	creditList.push({ x: new Date(info.year, info.month - 1, info.day), y: info.amount });
}

function cleanDebitBarList() {
	while (debitBarList.length > 0) {
		debitBarList.pop();
	}
}
function cleanCreditBarList() {
	while (creditBarList.length > 0) {
		creditBarList.pop();
	}
}

function cleanDebitList() {
	while (debitList.length > 0) {
		debitList.pop();
	}
}
function cleanCreditList() {
	while (creditList.length > 0) {
		creditList.pop();
	}
}

var DotNetHelper;
function setObjReference(dotNetHelper) {
	DotNetHelper = dotNetHelper;
};

function Chart_1(sumDebit) {
	 
	var visitorsData1 = {
		"Rasxod1": [{ 
			click: DrillDown1,
			explodeOnClick: false,
			innerRadius: "50%",
			radius: "90%",
			yValueFormatString: "# ### ##0.## сум",
			startAngle: 90,
			type: "doughnut",
			indexLabel: "#percent%",
			percentFormatString: "#0.##",
			toolTipContent: "{y} (#percent%)",
			dataPoints: debitBarList
		}] 
	};
	 
	var Options1 = {
		animationEnabled: true, 
		title: {
			dockInsidePlotArea: true,
			fontSize: 14,
			fontFamily: "'Roboto', sans-serif",
			fontWeight: "bold",
			horizontalAlign: "center",
			verticalAlign: "center",
			text: sumDebit
		},
		legend: {
			fontFamily: "'Roboto', sans-serif",
			fontSize: 14
		},
		data: []
	};
	  
	var chart1 = new CanvasJS.Chart("chartContainer1", Options1);
	chart1.options.data = visitorsData1["Rasxod1"];
	chart1.render();
	 
	function DrillDown1(e) { 
		DotNetHelper.invokeMethodAsync('ClickBarChart1', e.dataPoint.code); 
	} 
}

function Chart_2(sumDebit) {
	var visitorsData2 = {
		"Prixod2": [{
			click: DrillDown2,
			explodeOnClick: false,
			innerRadius: "50%",
			radius: "90%",
			yValueFormatString: "# ### ##0.## сум",
			startAngle: 90,
			type: "doughnut",
			indexLabel: "#percent%",
			percentFormatString: "#0.##",
			toolTipContent: "{y} (#percent%)",
			dataPoints: creditBarList
		}]
	};

	var Options2 = { 
		animationEnabled: true, 
		title: {
			dockInsidePlotArea: true,
			fontSize: 14,
			fontFamily: "'Roboto', sans-serif",
			fontWeight: "bold",
			horizontalAlign: "center",
			verticalAlign: "center",
			text: sumDebit
		},
		legend: {
			fontFamily: "'Roboto', sans-serif",
			fontSize: 14
		},
		data: []
	};
	 
	var chart2 = new CanvasJS.Chart("chartContainer2", Options2);
	chart2.options.data = visitorsData2["Prixod2"];
	chart2.render();

	function DrillDown2(e) {
		DotNetHelper.invokeMethodAsync('ClickBarChart2', e.dataPoint.code); 
	} 
}

function Chart_3() {
	var chart3 = new CanvasJS.Chart("chartContainer3", {
		animationEnabled: true,
		theme: "light2", 
		axisX: {
			valueFormatString: "DD MM",
			crosshair: {
				enabled: true,
				snapToDataPoint: true
			}
		},
		axisY: {
			fontFamily: "'Roboto', sans-serif",
			fontWeight: "normal",
			fontSize: 16,
			title: "Сумма",
			includeZero: true,

			crosshair: {
				enabled: true
			}
		},
		toolTip: {
			shared: true
		},
		legend: {
			cursor: "pointer",
			verticalAlign: "bottom",
			horizontalAlign: "left",
			dockInsidePlotArea: true,
			itemclick: toogleDataSeries
		},
		data: [{
			type: "spline",
			showInLegend: true,
			name: "Приход",
			// markerType: "square",
			xValueFormatString: "DD MMM, YYYY",
			yValueFormatString: "# ### ##0.## сум",
			color: "#00ca21",
			dataPoints: debitList
		},
		{
			type: "spline",
			showInLegend: true,
			name: "Расход",
			color: "#F08080",
			yValueFormatString: "# ### ##0.## сум",
			// lineDashType: "dash",
			dataPoints: creditList
		}]
	});
	 
	chart3.render();

	function toogleDataSeries(e) {
		if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
			e.dataSeries.visible = false;
		} else {
			e.dataSeries.visible = true;
		}
		chart3.render();
	}
}

function refreshChartPage() { 
	DotNetHelper.invokeMethodAsync('RefreshChartPage');
}