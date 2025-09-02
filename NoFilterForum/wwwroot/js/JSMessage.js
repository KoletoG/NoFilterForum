const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const deletedMessageText = "Deleted message";
let latestSeenMessageId="";
let latestAddedMessageId="";
let latestAddedMessageByOtherUserId = "";
var form = document.getElementById('messageForm');
let chatId="";
let userRecId = "";
let userSendId = "";
form.addEventListener('submit',(event)=>{
	event.preventDefault();
});
function setLatestAddedMessageId(latestMessageIdValue){
	latestAddedMessageId = latestMessageIdValue;
}
function setChatId(chatIdValue){
	chatId = chatIdValue;
}
function setUserRecipientId(userRecIdValue){
	userRecId = userRecIdValue;
}
function setUserSendId(userSendIdValue){
	userSendId = userSendIdValue;
}
function setLastOtherUserMessage(messageValue){
	latestAddedMessageByOtherUserId = messageValue;
}
connection.on("ReceiveMessage",(message,messageId) => {
	showMessages(true,message,messageId,null);
	connection.invoke("FeedbackOfSeen",userRecId);
	scrollToMessage(divCol1);
});
connection.on("RemoveMessage",(messageId)=>{
	replaceMessage(messageId);
});
connection.on("WasSeen",()=>{
	if(latestSeenMessageId!="")
	{
		document.getElementById(`h6OfSeenMessage_${latestSeenMessageId}`).remove(); // delete previously "Seen" message
	}
	addSeenMessage(latestAddedMessageId); // Add the new "seen" message in our current chat
	latestSeenMessageId = latestAddedMessageId;
	fetch('/Chat/UpdateLastMessage',{
		method: 'POST',
		headers:{'Content-Type':'application/json'},
		body:JSON.stringify({ChatId:chatId,MessageId:latestSeenMessageId,UserId:userRecId})
		}); // update the last seen message by the other user which in this case is latestAddedMessageId or latestSeenMessageId
});
function addSeenMessage(messageId)
{
	let col = document.getElementById(`colOfLastMessage_${messageId}`)
	let seenMessage = document.createElement('h6');
	seenMessage.id = `h6OfSeenMessage_${messageId}`;
	seenMessage.innerText="Seen";
	col.appendChild(seenMessage);
}
function startAll(){
fetch(`/Chat/GetLastMessage?chatId=${chatId}`) // When going to the chat, set "seen" to what message has been last seen
  .then(response => response.text())
  .then(data => {
	latestSeenMessageId = data; // Last message seen by the other user
	if(latestSeenMessageId != "")
		{
			addSeenMessage(latestSeenMessageId); // Set the seen message in current user chat
		}
	});

connection.start().then(()=>{
connection.invoke("FeedbackOfSeen",userRecId); // Send the other user's a response that we have seen his last message
fetch('/Chat/UpdateLastMessage',{
		method: 'POST',
		headers:{'Content-Type':'application/json'},
		body:JSON.stringify({ChatId:chatId,MessageId:latestAddedMessageByOtherUserId,UserId:userSendId})
		});
});
}
function sendMessage(userRecipientId,message,messageId) 
{
	latestAddedMessageId = messageId; // Set the last added message from ourselves
    connection.invoke("SendMessage", userRecipientId,message,messageId)
        .catch(err => console.error(err));
	connection.invoke("ShowMessageIndex",message,userRecipientId,userSendId);
}
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
	let afToken = document.querySelector('meta[name="csrf-token"]').getAttribute('content');;
	let formData = new FormData(form);
	let response = await fetch('/Message/Delete',{
		method: 'POST',
		headers:{
			'RequestVerificationToken':afToken
		},
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
	divColSecondary.classList.add('col-5','fst-italic','fw-lighter', isFromSignalR ? 'text-start' : 'text-end', 'd-flex' ,isFromSignalR ? 'justify-content-start' : 'justify-content-end' ,'align-items-center' ,'gap-2');
	divColSecondary.innerText = showTime(date);
	divColSecondary.id=`colOfLastMessage_${messageId}`;
	if(!isFromSignalR)
	{
		createForm(divColSecondary,messageId,userRecipientId);
	}
	divRow.addEventListener("mouseover",()=>{
		showButtonDelete(messageId);
	});
	divRow.addEventListener("mouseleave",()=>{
		hideButtonDelete(messageId);
	})
	var divColMain = document.createElement('div');
	divColMain.classList.add('col-7','border','border-2',isFromSignalR ? 'border-primary-subtle':null,isFromSignalR ? 'bg-primary-subtle' :'bg-body-secondary', 'text-break','rounded-2');
	var h6message=document.createElement('h6');
	h6message.innerText=messageText;
	h6message.id=`message_${messageId}`;
	divColMain.appendChild(h6message);
	divRow.appendChild(isFromSignalR ? divColMain : divColSecondary);
	divRow.appendChild(isFromSignalR ? divColSecondary : divColMain);
	mainContainer.appendChild(divRow);
	scrollToMessage(divColMain);
}
function showButtonDelete(messageId)
{
	var btn = document.getElementById(`btn_${messageId}`);
	if(btn !=null){
		btn.hidden=false;
	}
}
function hideButtonDelete(messageId)
{var btn = document.getElementById(`btn_${messageId}`);
	if(btn !=null){
		btn.hidden=true;
	}
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
	const inputChatId = document.createElement("input");
	inputChatId.type="hidden";
	inputChatId.name="ChatId";
	inputChatId.value=chatId;
	form.appendChild(inputMessageId);
	form.appendChild(inputChatId);
    const button = document.createElement("button");
    button.type = "submit";
	button.id = `btn_${messageId}`
    button.classList.add("delBtn");
	button.hidden = true;
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