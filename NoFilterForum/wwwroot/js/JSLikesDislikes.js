let likedPosts = new Set();
let normalColor = "black";
let updatedColor = "red";
function postLike(btn)
{
    let btnLike = document.getElementById("postLike");
    if(likedPosts.has(btn.id))
    {
        btnLike.style.color=normalColor;
        likedPosts.delete(btn.id);
    }
    else
    {
        btnLike.style.color=updatedColor;
        likedPosts.add(btnLike.id);
    }
}