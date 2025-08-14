const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage",(userId,message) => {
	alert(userId + " : " + message);
});
connection.start();
function sendMessage() {
        connection.invoke("SendMessage", "User1", "Hello SignalR")
                  .catch(err => console.error(err));
    }
var form = document.getElementById('messageForm');
form.addEventListener('submit',(event)=>{
	event.preventDefault();
});
var mainContainer = document.getElementById('mainContainer');
async function submitMessage()
{
	sendMessage();

	let messageText;
	let formData = new FormData(form);
	let response = await fetch('/Message/Create',{
		method: 'POST',
		body: formData
		});
	if(!response.ok){
		throw new Error("Error has occured");
	}
	messageText = await response.text();
	var divRow =document.createElement('div');
	divRow.classList.add('row','mb-3');
	var divCol1 = document.createElement('div');
	divCol1.classList.add('col-7');
	var divCol2 = document.createElement('div');
	divCol2.classList.add('col-5','border','border-2', 'bg-body-secondary', 'fst-italic', 'text-break','rounded-2');
	var h6message=document.createElement('h6');
	h6message.innerText=messageText;
	divCol2.appendChild(h6message);
	divRow.appendChild(divCol1);
	divRow.appendChild(divCol2);
	mainContainer.appendChild(divRow);
}