const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();
connection.on("ReceiveNotification",()=>{
let notification = document.getElementById('notificationLink');
notification.classList.remove('text-dark');
notification.classList.add('text-danger');
});

document.addEventListener('DOMContentLoaded',(e)=>{
    fetch('/Notifications/GetNotifications')
    .then(response => response.text())
    .then(data => {
        setNotification(data);
    });
});
function setNotification(count){
    if(count>0){
let notification = document.getElementById('notificationLink');
notification.classList.remove('text-dark');
notification.classList.add('text-danger');
    }
}