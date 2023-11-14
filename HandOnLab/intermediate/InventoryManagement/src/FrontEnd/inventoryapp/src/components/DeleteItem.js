import React, { useState } from 'react';
import axios from 'axios';

const DeleteItem = () => {
    const [itemId, setItemId] = useState('');

    const handleSubmit = (event) => {
        event.preventDefault();
        axios.delete(`/api/DeleteInventoryItem/${itemId}`)
            .then(response => {
                console.log('Item deleted:', response.data);
                setItemId('');
            })
            .catch(error => console.error('Error deleting item:', error));
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Delete Item</h2>
            <input
                type="text"
                value={itemId}
                onChange={e => setItemId(e.target.value)}
                placeholder="Item ID to Delete"
            />
            <button type="submit">Delete</button>
        </form>
    );
};

export default DeleteItem;
