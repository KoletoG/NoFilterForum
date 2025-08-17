const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage",(message) => {
	showMessages(true,message);
	scrollToMessage(divCol1);
});
connection.start();
function sendMessage(userId,message) {
        connection.invoke("SendMessage", userId,message)
                  .catch(err => console.error(err));
    }
var form = document.getElementById('messageForm');
form.addEventListener('submit',(event)=>{
	event.preventDefault();
});

var mainContainer = document.getElementById('mainContainer');
async function submitMessage(userId)
{
	let formData = new FormData(form);
	let response = await fetch('/Message/Create',{
		method: 'POST',
		body: formData
		});
	if(!response.ok){
		throw new Error("Error has occured");
	}
	let messageText = await response.text();
	showMessages(false,messageText);
	document.getElementById('messageInput').value="";
	sendMessage(userId,messageText);
}
function showMessages(isFromSignalR,messageText)
{
	var date = new Date();	
	var divRow =document.createElement('div');
	divRow.classList.add('row','mb-3');
	var divColSecondary = document.createElement('div');
	divColSecondary.classList.add('col-6','fst-italic','fw-lighter', isFromSignalR ? 'text-start' : 'text-end');
	divColSecondary.innerText = showTime(date);
	var divColMain = document.createElement('div');
	divColMain.classList.add('col-6','border','border-2',isFromSignalR ? 'border-primary-subtle':null,isFromSignalR ? 'bg-primary-subtle' :'bg-body-secondary', 'fst-italic', 'text-break','rounded-2');
	var h6message=document.createElement('h6');
	h6message.innerText=messageText;
	divColMain.appendChild(h6message);
	divRow.appendChild(isFromSignalR ? divColMain : divColSecondary);
	divRow.appendChild(isFromSignalR ? divColSecondary : divColMain);
	mainContainer.appendChild(divRow);
	scrollToMessage(divColMain);
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