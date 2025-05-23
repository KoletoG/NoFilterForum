let likedPosts = new Set();
let normalColor = "black";
let updatedColor = "red";
function updateLikes(btn)
{
    let btnLike = document.getElementById("postLike");
    let likesCount = document.getElementById("likesCount");
    if(likedPosts.has(btn.id))
    {
        btnLike.style.color=normalColor;
        likedPosts.delete(btn.id);
        likesCount.innerText=parseInt(likesCount.innerText)-1;
    }
    else
    {
        btnLike.style.color=updatedColor;
        likedPosts.add(btnLike.id);
        likesCount.innerText=parseInt(likesCount.innerText)+1;
    }
}