var options = {
    series: [14, 45, 13, 33,22,5,10,11,14,16,15,40],
    chart: {
    width: 420,
    type: 'donut',
  },
  dataLabels: {
    enabled: false
  },
  responsive: [{
    breakpoint: 480,
    options: {
      chart: {
        width: 380
      },
      legend: {
        position:'bottom'
      }
    }
  }],
  legend: {
    position: 'bottom',
    offsetY: 0,
    height: 'auto',
  },
  labels: ['Not Yet Submitted','Ready to Trade','Awaiting approval','Under Review','Cannot Recreate','Requires Callstack' , 'New Submission','Complaince Issues','Resubmission','Pending Approval','CER approved','CER Failed'],
colors:["#CEF3E3","#A2D7D1","#79BAC3","#57BF88","#71CE87","#00BDA0","#008d96",
"#1b83df","#3A7DB8","#4669B8","#7669B8","#AD9CF0"]
};

  var chart = new ApexCharts(document.querySelector("#dvSTCGeneralChart"), options);
  chart.render();


  var Recoptions = {
    series: [{
    name: "STOCK ABC",
    data: [0,0,0,0,0,0,0,0,0,0,0,0]
  }],
    chart: {
    type: 'area',
    height: 350,
    zoom: {
      enabled: false
    }
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    curve: 'straight'
  },
  
  labels: ['a','b','c','d','e','f','g','h','i','j','k','l'],
  xaxis: {
    type: 'text',
  },
  yaxis: {
    opposite: true
  },
 colors:["#ff0000"]
  };

  var chart = new ApexCharts(document.querySelector("#dvRECFailureChart"), Recoptions);
  chart.render();

  $('.sidebar-link').click(function(e) {
    e.preventDefault();
    var toggleButton = $(this).find('.dropdown-toggler');
    if (toggleButton.next().hasClass('active') ) {
      toggleButton.next().removeClass('active');
      toggleButton.next().slideUp(600);
      toggleButton.parent().removeClass('is-expanded');
    } else {
      toggleButton.parent().parent().find('li .sub-menu').removeClass('active');
      toggleButton.parent().parent().find('li .sub-menu').slideUp(600);
      toggleButton.parent().parent().find('.menu-toggle').parent().removeClass('is-expanded');
      toggleButton.next().toggleClass('active');
      toggleButton.next().slideToggle(600);
      toggleButton.parent().toggleClass('is-expanded');
    }
  });
  var toggleSidebar = function () {
		var w = $(window);
		if ((w.outerWidth() <= 1444) && (w.outerWidth() >= 601)) {
			document.body.classList.add("mini-sidebar");
      if($("#responsive-overlay").hasClass("visible-overlay")){
        $("#responsive-overlay").removeClass("visible-overlay")
        $(".sidebar-container").removeClass("show");
      }
		} else {
			document.body.classList.remove("mini-sidebar");
		}
    if ((w.outerWidth() <= 601)) {
			document.body.classList.add("sidebar-gone");
		} else {
			document.body.classList.remove("sidebar-gone");
		}
	}
	toggleSidebar();
  $(window).resize(toggleSidebar);




  const checkbox = document.getElementById("checkbox")
checkbox.addEventListener("change", () => {
  document.body.classList.toggle("dark")
})
  