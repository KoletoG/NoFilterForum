const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

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
connection.start();
function sendMessage(userId,message,messageId) 
{
    connection.invoke("SendMessage", userId,message,messageId)
        .catch(err => console.error(err));
}

function removeMessage(userId,messageId)
{
	connection.invoke("DeleteMessage",userId,messageId);
}
var mainContainer = document.getElementById('mainContainer');
async function submitMessage(userId,userMe)
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
	showMessages(false,messageInfo.message,messageInfo.messageId,userId);
	document.getElementById('messageInput').value="";
	sendMessage(userId,messageInfo.message,messageInfo.messageId);
}
async function deleteMessage(user2Id, messageId,form)
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
	removeMessage(user2Id,messageId);
	replaceMessage(messageId);
}
function replaceMessage(messageId)
{
	var message = document.getElementById(`message_${messageId}`);
	message.innerHTML="Deleted message";
}
function showMessages(isFromSignalR,messageText,messageId,user2Id)
{
	var date = new Date();	
	var divRow =document.createElement('div');
	divRow.classList.add('row','mb-3');
	var divColSecondary = document.createElement('div');
	divColSecondary.classList.add('col-6','fst-italic','fw-lighter', isFromSignalR ? 'text-start' : 'text-end');
	divColSecondary.innerText = showTime(date);
	if(!isFromSignalR)
	{
		createForm(divColSecondary,messageId,user2Id);
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
function createForm(container, messageId,user2Id) {
    const form = document.createElement("form");
	form.addEventListener('submit',(e)=>{
		e.preventDefault();
		deleteMessage(user2Id,messageId,form);
	})
	form.id=`form_${messageId}`;
	const inputMessageId = document.createElement("input");
	inputMessageId.type="hidden";
	inputMessageId.name="MessageId";
	inputMessageId.value=messageId;
	form.appendChild(inputMessageId);
    const button = document.createElement("button");
    button.type = "submit";
    button.textContent = "Delete";
	button.id = `btn_${messageId}`
    button.classList.add("btn", "btn-danger", "btn-sm");
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