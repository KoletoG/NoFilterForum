function markSeenPost(postId)
{
    let wasSeen = localStorage.getItem(`${postId}_seen`);
    if(wasSeen == "1")
    {
        let title = document.getElementById(`${postId}_title`);
        let body = document.getElementById(`${postId}_body`);
        title.classList.add('secondaryText');
        title.style='font-style:italic';
        title.classList.remove('mainText');
        body.classList.add('fst-italic');
        body.classList.remove('mainText');
        body.classList.add('secondaryText');
    }
}