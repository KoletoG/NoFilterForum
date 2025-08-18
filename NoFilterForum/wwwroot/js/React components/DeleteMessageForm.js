import React from "react";

function DeleteMessageForm({ messageId, userId }) {
  return (
    <form onSubmit={(event) =>
        {
        event.preventDefault();
        deleteMessage(userId,messageId)
        }}>
      <button type="submit" className="btn btn-danger btn-sm">
        Delete
      </button>
    </form>
  );
}

export default DeleteMessageForm;