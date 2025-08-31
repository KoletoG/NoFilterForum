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
    window.location.href = `/Reply/Index?postId=${postId}`;
}