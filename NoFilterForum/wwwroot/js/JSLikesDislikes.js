let normalColor = "#B58B64";
let updatedColor = "var(--secondary-text-color)";
function updateLike(id)
{
    let btnLike = document.getElementById(`Like_${id}`);
    let likesCount = document.getElementById(`likesCount_${id}`);
    let btnDislike = document.getElementById(`Dislike_${id}`);
    if(btnDislike.style.color==updatedColor)
    {
        btnDislike.style.color=normalColor;
        likesCount.innerText=parseInt(likesCount.innerText)+1;
    }
    if(btnLike.style.color==updatedColor)
    {
        btnLike.style.color=normalColor;
        likesCount.innerText=parseInt(likesCount.innerText)-1;
        likesCount.classList.remove('secondaryText');
        likesCount.classList.add('mainText');
    }
    else
    {
        btnLike.style.color=updatedColor;
        likesCount.innerText=parseInt(likesCount.innerText)+1;
        likesCount.classList.add('secondaryText');
        likesCount.classList.remove('mainText');
    }
}
function likeDislike(id,controller,action){
    let token = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
fetch(`/${controller}/${action}`,{
    method:'POST',
    headers:{
       'RequestVerificationToken' : token,
       'Content-Type': 'application/json'
    },
    body: JSON.stringify({Id:id})
});
if(action=="Like"){
    updateLike(id);
}
else{
    updateDislike(id);
}
}
function updateDislike(id)
{
    let btnLike = document.getElementById(`Like_${id}`);
    let likesCount = document.getElementById(`likesCount_${id}`);
    let btnDislike = document.getElementById(`Dislike_${id}`);
    if(btnLike.style.color==updatedColor)
    {
        btnLike.style.color=normalColor;
        likesCount.innerText=parseInt(likesCount.innerText)-1;
    }
    if(btnDislike.style.color==updatedColor)
    {
        btnDislike.style.color=normalColor;
        likesCount.innerText=parseInt(likesCount.innerText)+1;
        likesCount.classList.remove('secondaryText');
        likesCount.classList.add('mainText');
    }
    else
    {
        btnDislike.style.color=updatedColor;
        likesCount.innerText=parseInt(likesCount.innerText)-1;
        likesCount.classList.add('secondaryText');
        likesCount.classList.remove('mainText');
    }
}
