var connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/NotificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();


connection.on("Notification", function (message) {
    console.log(message);
    document.getElementById("configMessage").innerHTML = message;
});

connection.on("SendMessage", function (message) {
    console.log(message);
    document.getElementById("configMessage").innerHTML = message;
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
