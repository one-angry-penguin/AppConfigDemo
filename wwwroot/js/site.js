var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();


connection.on("Notification", function (message) {
    console.log(message);
    document.getElementById("configMessage").innerText(message);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
