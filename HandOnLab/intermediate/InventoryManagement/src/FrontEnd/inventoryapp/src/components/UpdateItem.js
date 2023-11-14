import React, { useState } from 'react';
import axios from 'axios';

const UpdateItem = () => {
    const [itemId, setItemId] = useState('');
    const [itemName, setItemName] = useState('');

    const handleSubmit = (event) => {
        event.preventDefault();
        axios.put(`/api/UpdateInventoryItem`, { id: itemId, name: itemName })
            .then(response => {
                console.log('Item updated:', response.data);
                setItemId('');
                setItemName('');
            })
            .catch(error => console.error('Error updating item:', error));
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Update Item</h2>
            <input
                type="text"
                value={itemId}
                onChange={e => setItemId(e.target.value)}
                placeholder="Item ID"
            />
            <input
                type="text"
                value={itemName}
                onChange={e => setItemName(e.target.value)}
                placeholder="New Item Name"
            />
            <button type="submit">Update</button>
        </form>
    );
};

export default UpdateItem;
