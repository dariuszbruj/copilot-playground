'use client'

import React from 'react';
import { Table } from 'antd';

function ApartmentsTableClientComponent({ apartments }: { apartments: Apartment[] }) {
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
            render: renderAddress,
        },
    ];

    return (
        <div>
            <h1>Apartment Listings</h1>
            <Table columns={columns} dataSource={apartments} rowKey="id" />
        </div>
    );
};

function renderAddress(address : Address): React.JSX.Element {
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

export default ApartmentsTableClientComponent;
