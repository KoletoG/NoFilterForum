function markSeenPost(postId)
{
    let wasSeen = localStorage.getItem(`${postId}_seen`);
    if(wasSeen == "1")
    {
        let date = document.getElementById(`${postId}_date`);
        date.innerHTML+=" / seen";
    }
}