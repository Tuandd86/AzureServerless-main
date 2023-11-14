import React from 'react';
import InventoryList from './components/InventoryList';
import AddItem from './components/AddItem';
import UpdateItem from './components/UpdateItem';
import DeleteItem from './components/DeleteItem';

function App() {
  return (
    <div>
      <h1>Inventory Management System</h1>
      <AddItem />
      <UpdateItem />
      <DeleteItem />
      <InventoryList />
    </div>
  );
}

export default App;
