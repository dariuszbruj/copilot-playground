import React from 'react';
import { Table } from 'antd';

async function HomePage() {
  const apartments = await getData()

  const columns = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
    },
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
    },
  ];

  return (
    <div>
      <h1>Apartment Listings</h1>
      <Table columns={columns} dataSource={apartments} rowKey="id" />
    </div>
  );
};

async function getData() {
  const res = await fetch('http://localhost:5000/apartments')
  // The return value is *not* serialized
  // You can return Date, Map, Set, etc.

  if (!res.ok) {
    // This will activate the closest `error.js` Error Boundary
    throw new Error('Failed to fetch data')
  }

  return res.json()
}


export default HomePage;
