function  updateSeenState(e,postId)
{
    e.preventDefault();
 fetch('/Post/UpdateSeenState',{
    method:'POST',
    headers:{
        'Content-Type':'application/json'
    },
    body:JSON.stringify(postId)
    });
    localStorage.setItem(`${postId}_seen`,"1"); // set to 1 as it is a string and to not waste unnecessary space in localStorage
    window.location.href = `/Reply/Index?postId=${postId}`;
}

