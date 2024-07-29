"use client";
import { Inter } from "next/font/google";
import "./globals.css";
import NavBar from "./_lib/NavBar/NavBar";

const inter = Inter({ subsets: ["latin"] });

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const isLoggedIn = localStorage.getItem("token") === null;
  console.log(isLoggedIn);
  return (
    <html lang="en">
      <body className={inter.className}>
        <div className="bodyWrapper">
          <div className="navBarWrapper">
            <NavBar isLoggedIn={isLoggedIn} />
          </div>
          <div className="content">{children}</div>
        </div>
      </body>
    </html>
  );
}
