const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let countNots=0;
connection.start();
connection.on("ReceiveNotification",()=>{
    countNots++;
    setNotification(countNots);
});

document.addEventListener('DOMContentLoaded',(e)=>{
    fetch('/Notifications/GetNotifications')
    .then(response => response.text())
    .then(data => {
        setNotification(data);
        countNots+=data;
    });
});
function setNotification(count){
    if(count>0){
let notification = document.getElementById('notificationLink');
notification.classList.remove('text-dark');
notification.classList.add('text-danger');
notification.innerText=`Notifications(${count})`;
    }
}