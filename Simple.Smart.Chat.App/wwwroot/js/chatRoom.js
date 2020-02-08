// Chat class helper
function ChatRoomMessage(userName, message) {
    this.userName = userName;
    this.message = message;
}

// global properties
var $username = $('#CurrentUser');
var $message = $('#Message');
var $chatSpace = $('#ChatSpace');
var messagesQueue = new Array();

// Event listeners
$('#sendMessgeBtn').on('click', function (e) {
    //e.preventDefault();
    console.log('Form submitted!!!');
    //return false;
});

// Helper functions
function clearMessageField() {
    messagesQueue.push($message.val());
    $message.val('');
}

function sendMessage() {
    var text = messagesQueue.shift() || "";
    if (text.trim() === "") return;

    var message = new ChatRoomMessage($username.val(), text);
    console.log('Message sent: ', message);
    sendMessageToHub(message);
}

function addMessageToChat(sentMessage) {
    console.log('Message to add to chat: ', sentMessage);
    var fromCurrentUser =
        sentMessage.userName === $username.val();
    var textAlign = fromCurrentUser ? 'text-right' : 'text-left';
    var currDate = getCurrDate();

    let $textMsg = $('<p></p>')
        .attr('class', textAlign)
        .html(sentMessage.message);

    let dateSent = '<span class="date-sent">' + currDate + '</span>';

    let $from = $('<p></p>')
        .attr('class', 'msg-from ' + textAlign)
        .html(sentMessage.userName + ' - ' + dateSent);

    var $msgContainer = $('<div></div>')
        .attr('class', fromCurrentUser
            ? "container-fluid msg-holder bg-primary m-1 p-3" : "container-fluid msg-holder bg-secondary m-1 p-3")
        .append($from)
        .append($textMsg);

    var $colSize = $('<div></div>')
        .attr('class', fromCurrentUser ? "col-11 offset-1" : "col-11")
        .append($msgContainer);

    var $row = $('<div class="row"></div>')
        .append($colSize);

    $chatSpace.append($row);
    $chatSpace.scrollTop($chatSpace.prop("scrollHeight"));
}

function getCurrDate() {
    var currentdate = new Date();

    return (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
}