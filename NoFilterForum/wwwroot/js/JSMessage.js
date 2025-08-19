const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const deletedMessageText = "Deleted message";

var form = document.getElementById('messageForm');
form.addEventListener('submit',(event)=>{
	event.preventDefault();
});

connection.on("ReceiveMessage",(message,messageId) => {
	showMessages(true,message,messageId,null);
	scrollToMessage(divCol1);
});
connection.on("RemoveMessage",(messageId)=>{
	replaceMessage(messageId);
});
connection.start()
    .then(() => {
        console.log("SignalR Connected.");
        let userRecId = document.getElementById('userRecId').value;
        let lastMessageId = document.getElementById('lastMessageId').value;
        connection.invoke("MarkMessageAsSeen", userRecId, lastMessageId);
    });
function sendMessage(userRecipientId,message,messageId) 
{
    connection.invoke("SendMessage", userRecipientId,message,messageId)
        .catch(err => console.error(err));
}

connection.on("HasSeenMessage",(lastMessageId)=>{
let col = document.getElementById(`colOfLastMessage_${lastMessageId}`)
let seenMessage = document.createElement('h6');
seenMessage.innerText="Seen";
col.appendChild(seenMessage);
});

function removeMessage(userRecipientId,messageId)
{
	connection.invoke("DeleteMessage",userRecipientId,messageId);
}
var mainContainer = document.getElementById('mainContainer');
async function submitMessage(userRecipientId)
{
	let formData = new FormData(form);
	let response = await fetch('/Message/Create',{
		method: 'POST',
		body: formData
		});
	if(!response.ok){
		throw new Error("Error has occured");
	}
	let messageInfo = await response.json();
	showMessages(false,messageInfo.message,messageInfo.messageId,userRecipientId);
	document.getElementById('messageInput').value="";
	sendMessage(userRecipientId,messageInfo.message,messageInfo.messageId);
}
async function deleteMessage(userRecipientId, messageId,form)
{
	let formData = new FormData(form);
	let response = await fetch('/Message/Delete',{
		method: 'POST',
		body: formData
		});
	if(!response.ok){
		throw new Error("Error has occured");
	};
	var btnDelete = document.getElementById(`btn_${messageId}`);
	form.removeChild(btnDelete);
	removeMessage(userRecipientId,messageId);
	replaceMessage(messageId);
}
function replaceMessage(messageId)
{
	var message = document.getElementById(`message_${messageId}`);
	message.innerHTML=deletedMessageText;
}
function showMessages(isFromSignalR,messageText,messageId,userRecipientId)
{
	var date = new Date();	
	var divRow =document.createElement('div');
	divRow.classList.add('row','mb-3');
	var divColSecondary = document.createElement('div');
	divColSecondary.classList.add('col-6','fst-italic','fw-lighter', isFromSignalR ? 'text-start' : 'text-end', 'd-flex' ,isFromSignalR ? 'justify-content-start' : 'justify-content-end' ,'align-items-center' ,'gap-2');
	divColSecondary.innerText = showTime(date);
	divColSecondary.id=`colOfLastMessage_${messageId}`;
	if(!isFromSignalR)
	{
		createForm(divColSecondary,messageId,userRecipientId);
	}
	var divColMain = document.createElement('div');
	divColMain.classList.add('col-6','border','border-2',isFromSignalR ? 'border-primary-subtle':null,isFromSignalR ? 'bg-primary-subtle' :'bg-body-secondary', 'fst-italic', 'text-break','rounded-2');
	var h6message=document.createElement('h6');
	h6message.innerText=messageText;
	h6message.id=`message_${messageId}`;
	divColMain.appendChild(h6message);
	divRow.appendChild(isFromSignalR ? divColMain : divColSecondary);
	divRow.appendChild(isFromSignalR ? divColSecondary : divColMain);
	mainContainer.appendChild(divRow);
	scrollToMessage(divColMain);
}
function createForm(container, messageId,userRecipientId) {
    const form = document.createElement("form");
	form.addEventListener('submit',(e)=>
	{
		e.preventDefault();
		deleteMessage(userRecipientId,messageId,form);
	});
	form.id=`form_${messageId}`;
	const inputMessageId = document.createElement("input");
	inputMessageId.type="hidden";
	inputMessageId.name="MessageId";
	inputMessageId.value=messageId;
	form.appendChild(inputMessageId);
    const button = document.createElement("button");
    button.type = "submit";
	button.id = `btn_${messageId}`
    button.classList.add("delBtn");
    form.appendChild(button);
    container.appendChild(form);
}
function showTime(date){
	
	var hours = date.getHours();
	var minutes = date.getMinutes();
	if(minutes<10){
		return hours + ":0" + minutes;
	}
	else{
		return hours + ":" + minutes;
	}
}
function scrollToMessage(message)
{
	message.scrollIntoView({behavior:"instant"});
}