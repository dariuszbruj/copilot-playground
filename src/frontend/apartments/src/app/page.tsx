'use server'

import React from 'react';
import { Table } from 'antd';

async function renderTags(address : any): Promise<React.JSX.Element> {
  "use server";

  return (
    <>
        <div>Street: {address.street}</div>
        <div>Building No: {address.buildingNo}</div>
        <div>Flat Number: {address.flatNumber}</div>
        <div>City: {address.city}</div>
        <div>State: {address.state}</div>
        <div>Zip Code: {address.zipCode}</div>
    </>
  );
}

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
      key: 'name'
    },
    {
      title: 'Address',
      dataIndex: 'address',
      key: 'address',
      render: renderTags,
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

  if (!res.ok) {
    // This will activate the closest `error.js` Error Boundary
    throw new Error('Failed to fetch data')
  }

  return res.json()
}

export default HomePage;
