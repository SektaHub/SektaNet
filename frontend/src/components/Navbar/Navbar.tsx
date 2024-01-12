import React from "react";
import "./Navbar.css";
import { Button } from "react-bootstrap";

const Navbar: React.FC = () => {
  return (
    <nav className="sidebar">
      {/* <ul className="menu">
        <li>
          <Link to="/">HOME</Link>
        </li>
        <li>
          <Link to="/reels">REELS</Link>
        </li>
        <li>
          <Link to="/images">IMAGES</Link>
        </li>
        <li>
          <Link to="/upload">UPLOAD</Link>
        </li>
      </ul> */}

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
      </div>
    </nav>
  );
};

export default Navbar;