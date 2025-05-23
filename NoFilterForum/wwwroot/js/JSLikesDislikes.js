let normalColor = "black";
let updatedColor = "red";
function updatePostLikes()
{
    let btnLike = document.getElementById("postLike");
    let likesCount = document.getElementById("likesPostCount");
    let btnDislike = document.getElementById("postDislike");
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
function updatePostDislikes()
{
    let btnLike = document.getElementById("postLike");
    let likesCount = document.getElementById("likesPostCount");
    let btnDislike = document.getElementById("postDislike");
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
function updateLikes(id)
{
    let btnLike = document.getElementById(`like_${id}`);
    let likesCount = document.getElementById(`likesCount_${id}`);
    let btnDislike = document.getElementById(`dislike_${id}`);
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
function updateDislikes(id)
{
    let btnLike = document.getElementById(`like_${id}`);
    let likesCount = document.getElementById(`likesCount_${id}`);
    let btnDislike = document.getElementById(`dislike_${id}`);
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