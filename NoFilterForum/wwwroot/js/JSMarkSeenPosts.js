function markSeenPost(postId)
{
    let wasSeen = localStorage.getItem(`${postId}_seen`);
    if(wasSeen == "1")
    {
        let post = document.getElementById(`${postId}_title`);
        post.classList.add('secondaryText');
        post.style='font-style:italic';
        post.classList.remove('mainText');
    }
}