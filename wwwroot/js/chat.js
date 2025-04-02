"use strict";




var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").withAutomaticReconnect([0, 2000, 10000, 30000]).configureLogging(signalR.LogLevel.Information).build()

console.log(connection)
document.getElementById("sendButton").disabled = true
console.log(document.getElementById("sendButton"))

var box = document.querySelectorAll("#userBox")
var senderId;
var recId;
var latestElem;
var userId;
box.forEach(elem => {
	elem.addEventListener("click", (e) => {
		//connection.invoke("GetMessages",)
		document.getElementById("messageBox").hidden = false
		recId = elem.querySelector("#receiverId").value
		latestElem = elem.querySelector("#latestMessage")
		console.log("latestElem")
		userId = document.getElementById("userId").value;
		senderId = document.getElementById("userId").value;
		connection.invoke("GetMessages", recId, senderId).catch(err => {
			return console.log(err)
		})
		e.preventDefault();
	})
})

var messageBox = document.getElementById("message");


document.getElementById("sendButton").addEventListener("click", function (event) {
	message = messageBox
	if (message.value.length != 0) {
		console.log(receiverId)
		connection.invoke("SendMessage", message.value, senderId.toString(), recId.toString()).catch(function (err) {
			return console.error(err.toString());
		});
		window.scrollTo({
			top: document.body.scrollHeight,
			behavior: 'smooth'
		});
		event.preventDefault();
		message.value = ""
	}
	});


connection.on("ReceiveAllMessage", function (message) {
	var json = JSON.parse(message)
	const messageContainer = document.getElementById("messagesList");
	console.log(json)
	messageContainer.innerHTML = ""
	json.forEach(e => {
		var style = userId === e.senderId.toString() ? "align-self-start bg-success" : "align-self-end bg-white"
		var styleText = userId === e.senderId.toString() ? "text-white " : "text-dark"
		messageContainer.innerHTML += `
        <div class=" p-1 ${style} my-1 mx-3 rounded shadow message-item">
            <div class="options">
                <a href="#"><i class="fas fa-angle-down text-muted px-2"></i></a>
            </div>
            <div class="d-flex flex-row ${styleText} ">
                <div class="body m-1  mr-2">${e.message}</div>
                <div class="time ml-auto small   text-right flex-shrink-0 align-self-end text-muted" style="width:75px;">
                    9:00 AM

                </div>
            </div>
        </div>`
	})
	

	messageBox.value = ""
	console.log(messageContainer)
})
connection.on("ReceiveMessage", function (message) {
	console.log(latestElem)
	var message = JSON.parse(message)
	console.log(message)
	const messageContainer = document.getElementById("messagesList");
	latestElem.textContent = message.message.toString()
	messageContainer.innerHTML += `
        <div class="align-self-start bg-success p-1 my-1  mx-3 rounded shadow message-item">
            <div class="options">
                <a href="#"><i class="fas fa-angle-down text-muted px-2"></i></a>
            </div>
            <div class="d-flex flex-row text-white">
                <div class="body m-1  mr-2">${message.message}</div>
                <div class="time ml-auto small text-right flex-shrink-0 align-self-end text-muted" style="width:75px;">
                    9:00 AM

                </div>
            </div>
        </div>`

	

})

connection.start().then(function () {
	console.log("SignalR connection established.");
	document.getElementById("sendButton").disabled = false;
	console.log("disabled")
}).catch(function (err) {
	return console.error(err.toString())
})

connection.onclose(error => {
	console.assert(connection.state ===
		signalR.HubConnectionState.Disconnected);
	document.getElementById("messageInput").disabled = true;
	const li = document.createElement("li");
	li.textContent = `Connection closed due to error "${error}". Try
refreshing this page to restart the connection.`;
	document.getElementById("messageList").appendChild(li);
});
document.getElementById("messageBox").hidden = true

connection.onreconnected(connectionId => {
	console.assert(connection.state ===
		signalR.HubConnectionState.Connected);
	document.getElementById("messageInput").disabled = false;
	const li = document.createElement("li");
	li.textContent = `Connection reestablished. Connected with connectionId
"${connectionId}".`;
	document.getElementById("messageList").appendChild(li);
});

connection.onreconnecting(error => {
	console.assert(connection.state ===
		signalR.HubConnectionState.Reconnecting);
	document.getElementById("messageInput").disabled = true;
	const li = document.createElement("li");
	li.textContent = `Connection lost due to error "${error}".
Reconnecting.`;
	document.getElementById("messageList").appendChild(li);
});

var lockResolver;
if (navigator && navigator.locks && navigator.locks.request) {
	const promise = new Promise((res) => {
		lockResolver = res;
	});
	navigator.locks.request('unique_lock_name', { mode: "shared" }, () => {
		return promise;
	});
}