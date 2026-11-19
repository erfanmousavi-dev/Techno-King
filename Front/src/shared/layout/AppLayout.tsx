import { Outlet } from "react-router-dom";
import Header from "../components/layout/Header";
import Footer from "../components/layout/Footer";
import { useState } from "react";
import ProductSearch from "../../features/search/components/ProductSearch";
import LoginRegisterForm from "../../features/auth/components/LoginRegisterForm";
import { tokenStorage } from "../../features/auth/api/tokenStorage";
import toast from "react-hot-toast";

function AppLayout() {
  const [isSearchOpen, setIsSearchOpen] = useState(false);
  const [isAuthOpen, setIsAuthOpen] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(
    () => !!tokenStorage.getAccess(),
  );
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);

  const openSearch = () => {
    setIsAuthOpen(false);
    setIsUserMenuOpen(false);
    setIsSearchOpen(true);
  };

  const openAuth = () => {
    setIsSearchOpen(false);
    setIsAuthOpen(true);
  };

  const toggleUserMenu = () => setIsUserMenuOpen((prev) => !prev);

  const handleAuthSuccess = (type: "login" | "register") => {
    setIsLoggedIn(true);
    setIsAuthOpen(false);
    toast.success(
      type === "register"
        ? "Your registration was successful."
        : "You have successfully logged in.",
    );
  };

  const handleLogout = () => {
    tokenStorage.clear();
    setIsLoggedIn(false);
    setIsUserMenuOpen(false);
  };

  return (
    <div className="flex flex-col min-h-screen">
      <Header
        onSearchClick={openSearch}
        onLoginClick={openAuth}
        onUserClick={toggleUserMenu}
        onUserMenuClose={() => setIsUserMenuOpen(false)}
        onLogout={handleLogout}
        isLoggedIn={isLoggedIn}
        isUserMenuOpen={isUserMenuOpen}
        isSearchOpen={isSearchOpen}
      />

      {isSearchOpen && <ProductSearch onClose={() => setIsSearchOpen(false)} />}
      {isAuthOpen && (
        <LoginRegisterForm
          onClose={() => setIsAuthOpen(false)}
          onSuccess={handleAuthSuccess}
        />
      )}
      <main className="grow">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}

export default AppLayout;
