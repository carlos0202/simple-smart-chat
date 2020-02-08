var connection = new signalR.HubConnectionBuilder()
    .withUrl('/Home/Index')
    .build();

// addMessageToChat from chatRoom.js
connection.on('receiveMessage', addMessageToChat);

connection.start()
    .catch(error => {
        console.error(error.message);
    });

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}