import Link from "next/link";
import React from "react";
import "./NavBar.css";

interface Props {
  isLoggedIn: boolean;
}
const NavBar = ({ isLoggedIn }: Props) => {
  console.log(isLoggedIn, "isLoggedIn");
  const navBarItems = [
    {
      title: "Home",
      link: "/dashboard",
      active: false,
    },
    {
      title: "Library",
      link: "/library",
      active: isLoggedIn,
    },
    {
      title: "Files",
      link: "/files",
      active: isLoggedIn,
    },
    {
      title: "Upload",
      link: "/upload",
      active: isLoggedIn,
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
