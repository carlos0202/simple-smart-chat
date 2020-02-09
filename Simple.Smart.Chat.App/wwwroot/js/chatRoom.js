// Chat class helper
function ChatRoomMessage(userName, message) {
    this.userName = userName;
    this.message = message;
}

// global properties
const $username = $('#CurrentUser');
const $message = $('#Message');
const $chatSpace = $('#ChatSpace');
const messagesQueue = new Array();


// Helper functions
function clearMessageField() {
    messagesQueue.push($message.val());
    $message.val('');
}

function sendMessage() {
    var text = messagesQueue.shift() || "";
    if (text.trim() === "") return;

    let message = new ChatRoomMessage($username.val(), text);

    sendMessageToHub(message);
}

function addMessageToChat(sentMessage) {
    console.log(`Message to add to chat: ${sentMessage}`);
    var fromCurrentUser =
        sentMessage.userName === $username.val();
    var textAlign = fromCurrentUser ? 'text-right' : 'text-left';
    var currDate = getCurrDate();
    var stappleContainerClasses = "container-fluid msg-holder m-1 p-1 border border";
    let dateSent = `<span class="date-sent">${currDate}</span>`;
    let msg = `
        <div class="row">
            <div class="${fromCurrentUser ? "col-11 offset-1" : "col-11"}">
                <div class="${stappleContainerClasses}-${fromCurrentUser ? "primary" : "secondary"}">
                    <p class="msg-from ${textAlign}">
                        ${sentMessage.userName} - ${dateSent}
                    </p>
                    <p class="${textAlign}">${sentMessage.message}</p>
                </div>
            </div>
        </div>
    `;

    $chatSpace.append(msg);
    $chatSpace.scrollTop($chatSpace.prop("scrollHeight"));
}

function getCurrDate() {
    let currentdate = new Date();

    let am = currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    let dateStr = `
        ${currentdate.getMonth() + 1}/${currentdate.getDate()}/${currentdate.getFullYear()} ${am}
    `;

    return dateStr.trim();
}