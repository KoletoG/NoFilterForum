const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();

connection.on("ReceiveNotification",()=>{
let notification = document.getElementById('notificationLink');
notification.classList.remove('text-dark');
notification.classList.add('text-danger');
});

