import React from "react";
import type { Metadata } from "next";
import StyledComponentsRegistry from "./AntdRegistry";
//import "./globals.css";

export const metadata: Metadata = {
    title: "Apartments App",
    description: "An app to manage apartments",
};

interface RootLayoutProps {
    children: React.ReactNode;
}

function RootLayout({ children }: RootLayoutProps) {
    return (
        <html lang="en">
        <body>
        <StyledComponentsRegistry>{children}</StyledComponentsRegistry>
        </body>
        </html>
    );
}

export default RootLayout;
