import React, { useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import { fetchWithAuth } from "../../api"; // assuming fetchWithAuth is in a file named utils
import {API_URL} from "../../config"
import "./Navbar.css";

const Navbar: React.FC = () => {
  const [currentUser, setCurrentUser] = useState<string | null>(null);

  useEffect(() => {
    const fetchCurrentUser = async () => {
      try {
        const response = await fetchWithAuth(`${API_URL}/identity/current-user`);
        const user = await response.json();
        setCurrentUser(user.username); // Assuming the user object has a 'username' property
      } catch (error) {
        console.error("Error fetching current user:", error);
      }
    };

    fetchCurrentUser();
  }, []);

  return (
    <nav className="sidebar">
      <div className="navContainer">
        <div className="navBarLogo">
          <img className="logo" src="../../../public/hub.png" />
        </div>
        <Button className="homeNavButton" href="/">
          Home
        </Button>
        <Button className="homeNavButton" href="/images">
          Library
        </Button>
        <Button className="homeNavButton" href="/reels">
          Reels
        </Button>
        <Button className="homeNavButton" href="/upload">
          Upload
        </Button>
        {currentUser && <span>Welcome, {currentUser}</span>}
      </div>
    </nav>
  );
};

export default Navbar;
