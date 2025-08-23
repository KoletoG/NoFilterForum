const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("MessageIndexReceive",(userRecId,message)=>{
var mainText = document.getElementById(`message_${userRecId}`);
mainText.innerText=message;
mainText.classList.remove('text-black-50');
mainText.classList.add('fw-bold', 'text-dark');
let date = new Date();
var dateText = document.getElementById(`messageDate_${userRecId}`);
dateText.innerText=showTime(date);
dateText.classList.remove('text-black-50');
dateText.classList.add('fw-bold','text-dark');
});
document.addEventListener('DOMContentLoaded',(e)=>{
    
connection.start();
});
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