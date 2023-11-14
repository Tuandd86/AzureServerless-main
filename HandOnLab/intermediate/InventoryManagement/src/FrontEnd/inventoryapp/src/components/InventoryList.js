import React, { useEffect, useState } from 'react';
import axios from 'axios';

const InventoryList = () => {
    const [items, setItems] = useState([]);

    useEffect(() => {
        axios.get('/api/GetInventoryItems') // Modify with correct Azure Function URL
            .then(response => {
                setItems(response.data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    return (
        <div>
            <h2>Inventory List</h2>
            {items.map(item => (
                <div key={item.id}>{item.name}</div>
            ))}
        </div>
    );
};

export default InventoryList;
