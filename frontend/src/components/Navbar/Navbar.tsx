import React from 'react';
import { Link } from 'react-router-dom';
import "./Navbar.css"

const Navbar: React.FC = () => {
  return (
    <nav className="sidebar">
      <ul className='menu'>
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
      </ul>
    </nav>
  );
};

export default Navbar;
