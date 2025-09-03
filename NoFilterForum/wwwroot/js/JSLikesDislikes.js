let normalColor = "black";
let updatedColor = "#236A96";
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
    }
    else
    {
        btnLike.style.color=updatedColor;
        likesCount.innerText=parseInt(likesCount.innerText)+1;
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
    }
    else
    {
        btnDislike.style.color=updatedColor;
        likesCount.innerText=parseInt(likesCount.innerText)-1;
    }
}
