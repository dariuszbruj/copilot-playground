'use server'

import React from 'react';
import ApartmentsTableClientComponent from './_components/client/ApartmentsTableClientComponent';

async function HomePage() : Promise<React.JSX.Element> {
  const apartments : Apartment[] = await getData();
  
  return (
    <>
      <ApartmentsTableClientComponent apartments={apartments} />
    </>
  );
};

async function getData() : Promise<Apartment[]> {
    
    try {

        const res = await fetch('http://localhost:5000/apartments')

        if (!res.ok) {
            // This will activate the closest `error.js` Error Boundary
            throw new Error('Failed to fetch data')
        }

        return res.json()
    } 
    catch (error) 
    {
        return [];
    }
}

export default HomePage;
