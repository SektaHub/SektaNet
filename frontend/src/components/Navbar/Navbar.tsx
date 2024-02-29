import React, { useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import { fetchWithAuth } from "../../api"; // assuming fetchWithAuth is in a file named utils
import { handleLogout } from "../../api"; 
import { API_URL } from "../../config";
import "./Navbar.css";

const Navbar: React.FC = () => {
  const [currentUser, setCurrentUser] = useState<string | null>(null);

  useEffect(() => {
    const fetchCurrentUser = async () => {
      try {
        const response = await fetchWithAuth(`${API_URL}/identity/current-user`, { disableRedirect: true });
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
        <Button className="homeNavButton" href="/videos">
          Videos
        </Button>
        <Button className="homeNavButton" href="/files">
          Files
        </Button>
        <Button className="homeNavButton" href="/upload">
          Upload
        </Button>
        {currentUser ? (
          <>
            <span className="currentUserText">Welcome, {currentUser}</span>
            <Button className="homeAccountButton" onClick={handleLogout}>Logout</Button>
          </>
        ) : (
          <Button className="homeAccountButton" href="/login">Login</Button>
        )}
      </div>
    </nav>
  );
};

export default Navbar;