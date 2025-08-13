var form = document.getElementById('messageForm');
form.addEventListener('submit',(event)=>{
	event.preventDefault();
});
function submitMessage()
{
	let formData = new FormData(form);
	fetch('/Message/Create',{
		method: 'POST',
		body: formData
		}).then(response=>{
		if(!response.ok)
		{
			throw new Error("Error has occcured");
		}
		return response.text();
		}).then(data=>{
		var label = document.createElement('label');
		label.textContent=data;
		document.appendChild(label);
		});
}