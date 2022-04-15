$(document).ready(function () {
    let serverId = $("#ServerId").attr('value');
    console.log("server id= " + serverId)
    $.ajax({
        type: 'GET',
        url: '../Account/GetVoiceChannelInfoFromDatabase?serverid=' + serverId,
        success: retrieveVoiceInfoForServer,
        error: handleError
    });

})



function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

function retrieveVoiceInfoForServer(data) {

    barGraphHourlyVoiceActivity(data);
};

function barGraphHourlyVoiceActivity(data) {
    $("#usersVoiceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    for (var a = 0; a < xValues.length; a++)
    {
        
        for (var i = 0; i < data.length; i++) {
            if (data[i].hour < 12)
                {
                var current_time = data[i].hour + "am";
            }
            else {
                var current_time = data[i].hour - 12 + "pm";
            }
            if (xValues[a] == current_time)
            yValues[a] = data[i].avgMemberCount;
        }
    }
    console.log(xValues);
    console.log(yValues);



    new Chart("usersVoiceHourlyAllTimeChart", {
        type: "bar",

        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: "Blue",
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
                    text: "Active Voice Times",
                    padding: 10,
                    color: 'black',
                    font: {
                        size: 25
                    }
                },
            },
            scales: {
                y: {
                    title: {
                        display: true,
                        text: 'Active Users',
                        padding: 10,
                        color: 'black',
                        font: {
                            size: 25
                        }
                    },
                    ticks: {
                        beginAtZero: false,
                        precision: 0,
                        color: 'black',
                        font: {
                            size: 20
                        }
                    }

                },
                x: {
                    ticks: {
                        precision: 0,
                        color: 'Black',
                        font: {
                            size: 16,
                            family: 'Helvetica'
                        }
                    }
                }
            },

        }




    })


};