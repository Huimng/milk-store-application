const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ChatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(function () {
    console.log("SignalR Connected.");

    connection.on("Loading", function () {
        console.log("System accounts updated via SignalR.");
        location.reload(); // Reload the page on update
    });
}).catch(function (err) {
    return console.error(err.toString());
});
