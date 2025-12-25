import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';


function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-left">Word Chain Game</div>
      <div className="navbar-right">
        <Link to="/" className="nav-btn">Home</Link>
        <Link to="/game" className="nav-btn">Game</Link>
      </div>
    </nav>
  );
}

export default Navbar;