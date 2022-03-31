

$.ajax({
    type: 'GET',
    url: 'Stats/GetMessageInfoFromDatabase?serverid=947357087581765702',
    success: retrieveDataForBarChart,
    error: handleError
});