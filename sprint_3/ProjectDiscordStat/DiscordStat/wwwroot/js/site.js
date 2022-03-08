// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.

//$(".change").on("click", function () {
//    if ($("body").hasClass("dark")) {
//        $("body").removeClass("dark");

//    } else {
//        $("body").addClass("dark");

//    }
//});

/*setInterval(function () { $("body").addClass("dark") });*/

/*JQuery for graphs on home page and bar and pie graphs on home page*/
$.ajax({
    type: 'GET',
    url: 'Home/GetDataAsynchronousParallel',
    success: retrieveDataForBarChart,
    error: handleError
});

$.ajax({
    type: 'GET',
    url: 'Api/GetDataAsynchronousParallel',
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
    var barColors = ["red", "green", "blue", "orange", "brown"];

    new Chart("barChart", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues,
            }]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: "Top 5: Users per Servers",
            },
            xValues: {
                title: {
                    text: 'Servers',
                    font: {
                        family: 'Courier New, monospace',
                        size: 18,
                        color: 'white'
                    }
                },
            },
            yValues: {
                title: {
                    text: 'Users',
                    font: {
                        family: 'Courier New, monospace',
                        size: 18,
                        color: 'white'
                    }
                }
            }

        }
    })
};

function pieGraphTopUsersPerGam(data) {
    $("#pieChart").empty();

    var countForX = 0;
    var countForY = 0;
    var  xValues = [];
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

    var barColors = ["red", "green", "blue", "orange", "brown"];

    new Chart("pieChart", {
        type: "pie",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: "Users Playing top 5 games"
            }

        }
    });
};