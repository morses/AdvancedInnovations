// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.
$(".change").on("click", function () {
    if ($("body").hasClass("dark")) {
        $("body").removeClass("dark");
        $(".change").text("OFF");
    } else {
        $("body").addClass("dark");
        $(".change").text("ON");
    }
});

/*JQuery for graphs on home page and bar and pie graphs on home page*/
$.ajax({
    type: 'GET',
    url: 'Home/GetServerDataFromDb',
    success: retrieveDataForBarChart,
    error: handleError
});

$.ajax({
    type: 'GET',
    url: 'Api/GetPresenceDataFromDb',
    success: retrieveDataForPieChart,
    error: handleError
});

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

function retrieveDataForBarChart(data) {
    gdata = data;
    barGraphTopUsersPerServer(data.userPerServer);
};

function retrieveDataForPieChart(data) {
    gdata = data;
    pieGraphTopUsersPerGam(data.userPerGame);
};

function barGraphTopUsersPerServer(data) {
    $("#barChart").empty();
    var count = 0;
    var xValues = [];
    var yValues = [];
    for (var i = 0; i < data.length; ++i) {
        xValues.push(data[i].name);
        yValues.push(data[i].approximateMemberCount);

        if (count == 3) {
            break;
        }
        count += 1;
    };

    var barColors = ["lightblue", "rgb(243, 128, 128)", "rgb(139, 236, 97)", "rgb(255, 214, 70)", "rgb(144, 12, 63)"];

    new Chart("barChart", {
        type: "bar",
        label: 'Scatter Dataset',
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues,
            }]
        },

        options: {
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: "Users per Servers",
                    padding: 10,
                    color: 'black',
                    font: {
                        size: 25
                    }
                },
                
            },
            scales: {
                y: {

                    ticks: {
                        beginAtZero: false,
                        precision: 0,
                        color: 'black',
                        font: {
                        size: 20
                    }
                    }
                }
            },

        }










    })
};

function pieGraphTopUsersPerGam(data) {
    $("#pieChart").empty();

    var countForX = 0;
    var countForY = 0;
    var xValues = [];
    var yValues = [];;

    for (let value of Object.keys(data)) {
        xValues.push(value)
        if (countForX == 3) {
            break;
        }
        countForX += 1;
    }
    for (let value of Object.values(data)) {
        yValues.push(value)
        if (countForY == 3) {
            break;
        }
        countForY += 1;
    }

    var barColors = ["lightblue", "rgb(243, 128, 128)", "rgb(139, 236, 97)", "rgb(255, 214, 70)", "rgb(144, 12, 63)"];

    new Chart("pieChart", {
        type: "doughnut",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            plugins: {
            legend: {
                labels: {
                    // This more specific font property overrides the global property
                    color: 'black',
                    font: {
                        size: 20
                    }
                }
            },
            title: {
                display: true,
                text: "Games played",
                color: 'black',
                font: {
                    size: 20
                }

            }
        }
        }
    });
};