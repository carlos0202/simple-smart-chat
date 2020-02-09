var connection = new signalR.HubConnectionBuilder()
    .withUrl('/Home/Index')
    .build();

// addMessageToChat from chatRoom.js
connection.on('receiveMessage', addMessageToChat);

connection.start()
    .catch(error => {
        console.warn(error.message);
        $('#sendMessgeBtn').prop('disabled', true);
    });

connection.closedCallbacks.push(function () {
    console.info('client disconnected from server...');
    $('#sendMessgeBtn').prop('disabled', true);
});

connection.reconnectedCallbacks.push(function () {
    console.info('client reconnected to server...');
    $('#sendMessgeBtn').prop('disabled', false);
});

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}