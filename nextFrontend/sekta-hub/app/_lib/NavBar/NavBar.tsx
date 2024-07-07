import Link from "next/link";
import React from "react";
import "./NavBar.css";
const NavBar = () => {
  const navBarItems = [
    {
      title: "Home",
      link: "/dashboard",
      active: false,
    },
    {
      title: "Library",
      link: "/library",
      active: false,
    },
    {
      title: "Reels",
      link: "/reels",
      active: false,
    },
    {
      title: "Videos",
      link: "/videos",
      active: false,
    },
    {
      title: "Files",
      link: "/files",
      active: false,
    },
    {
      title: "Upload",
      link: "/upload",
      active: false,
    },
    {
      title: "Login",
      link: "/login",
      active: false,
    },
  ];
  return (
    <div>
      <div className="navBarContainer">
        <div className="logo">Ikona</div>
        <div className="navContainer">
          {navBarItems.map((item) => (
            <Link
              className="navItems"
              key={item.title}
              href={item.link}
              hidden={item.active}
            >
              {item.title}
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};

export default NavBar;
