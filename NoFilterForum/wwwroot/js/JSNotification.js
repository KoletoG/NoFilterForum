const connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
let countNots=0;
let firstTime = true;
let notification;
connection.start();
connection.on("ReceiveNotification",()=>{
    countNots++;
    setNotification(countNots);
});

document.addEventListener('DOMContentLoaded',(e)=>{
    notification = document.getElementById('notificationLink');
    fetch('/Notifications/GetNotifications')
    .then(response => response.text())
    .then(data => {
        setNotification(data);
        countNots+=data;
    });
});
function setNotification(count){
    if(count>0){
        if(firstTime)
        {
            notification.classList.remove('text-dark');
            notification.classList.add('text-danger');
            notification.classList.add('fw-bold');
            firstTime = false;
        }
        notification.innerHTML=`Notifications <span class="badge mainText fw-bold nav-link thirdBorder" style="background-color:rgba(var(--bs-danger-rgb),var(--bs-text-opacity));font-size:15px">${count}</span>`;
    }
}