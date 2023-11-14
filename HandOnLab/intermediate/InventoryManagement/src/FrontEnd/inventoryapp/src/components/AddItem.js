import React, { useState } from 'react';
import axios from 'axios';

const AddItem = () => {
    const [itemName, setItemName] = useState('');

    const handleSubmit = (event) => {
        event.preventDefault();
        axios.post('/api/CreateInventoryItem', { name: itemName })
            .then(response => {
                console.log('Item added:', response.data);
                setItemName('');
            })
            .catch(error => console.error('Error adding item:', error));
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Add Item</h2>
            <input
                type="text"
                value={itemName}
                onChange={e => setItemName(e.target.value)}
                placeholder="Item name"
            />
            <button type="submit">Add</button>
        </form>
    );
};

export default AddItem;
