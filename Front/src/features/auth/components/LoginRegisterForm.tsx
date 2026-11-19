import facebookIcon from "/icons/facebook2.svg";
import googleIcon from "/icons/google.svg";
import closeIcon from "/icons/close-circle.svg";
import { useEffect, useState } from "react";
import LoginInputs from "./LoginInputs";
import RegisterInputs from "./RegisterInputs";
import { registerUser } from "../api/registere";
import { loginUser } from "../api/login";
import { tokenStorage } from "../api/tokenStorage";
import toast from "react-hot-toast";

interface LoginRegisterFormProps {
  onClose: () => void;
  onSuccess: (type: "login" | "register") => void;
}

const LoginRegisterForm = ({ onClose, onSuccess }: LoginRegisterFormProps) => {
  const [isRegistered, setIsRegistered] = useState(true);

  useEffect(() => {
    const prevOverflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    return () => {
      document.body.style.overflow = prevOverflow;
    };
  }, []);

  const handleLoginSubmit = async (data: any) => {
    try {
      const result = await loginUser({
        email: data.email,
        password: data.password,
        rememberMe: data.keepLoggedIn,
      });
      tokenStorage.save(result);
      onSuccess("login");
      console.log("Success:", result);
    } catch (error) {
      console.error("Login error:", error);
       toast.error("Invalid email or password.")
    }
  };

  const handleRegisterSubmit = async (data: any) => {
    try {
      const result = await registerUser({
        userName: data.userName,
        email: data.email,
        password: data.password,
      });
      tokenStorage.save(result.data);
      onSuccess("register");
      console.log("Success:", result);
    } catch (error) {
      console.error("Register error:", error);
      toast.error("Registration failed. Please try again.");
    }
  };

  return (
    <div
      className="fixed inset-0 z-50 flex items-start justify-center overflow-y-auto bg-black/40 px-4 py-8 md:items-center"
      onClick={onClose}
    >
      <div
        className="relative w-full max-w-[30rem] rounded-[0.5rem] bg-white py-[2rem]"
        onClick={(e) => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
      >
        <button
          type="button"
          onClick={onClose}
          className="absolute right-4 top-4"
          aria-label="Close"
        >
          <img src={closeIcon} alt="Close icon" />
        </button>

        <div className="mx-auto flex flex-col items-center">
          <div className="flex w-[65%] justify-between border-b-[2px] border-gray-200">
            <div
              onClick={() => setIsRegistered(true)}
              className={`mb-[-2px] w-[45%] cursor-pointer text-center text-gray-400 ${
                isRegistered ? "border-b-[2px] border-primary-300" : ""
              }`}
            >
              Login
            </div>
            <div
              onClick={() => setIsRegistered(false)}
              className={`mb-[-2px] w-[45%] cursor-pointer text-center text-gray-400 ${
                !isRegistered ? "border-b-[2px] border-primary-300" : ""
              }`}
            >
              Create Account
            </div>
          </div>

          <h3 className="mb-[1rem] mt-[1.4rem] text-[1.5rem] font-semibold">
            {isRegistered ? "Log in to Tech Heim" : "Create your account"}
          </h3>

          <div className="flex w-[65%] flex-col gap-[1rem]">
            {isRegistered ? (
              <LoginInputs onSubmit={handleLoginSubmit} />
            ) : (
              <RegisterInputs onSubmit={handleRegisterSubmit} />
            )}
          </div>

          <div className="flex w-[65%] items-center justify-center">
            <div className="grow border border-gray-200"></div>
            <div className="my-[1rem] px-1">
              {isRegistered ? "Or Log In with" : "Or Sign Up with"}
            </div>
            <div className="grow border border-gray-200"></div>
          </div>

          <div className="flex w-[65%] items-center justify-between text-primary-300">
            <button className="flex w-[45%] items-center justify-center gap-1 rounded-[0.5rem] border-[2px] border-primary-300 py-[0.4rem]">
              <img src={googleIcon} alt="Google icon" />
              <span>Google</span>
            </button>
            <button className="flex w-[45%] items-center justify-center gap-1 rounded-[0.5rem] border-[2px] border-primary-300 py-[0.4rem]">
              <img src={facebookIcon} alt="Facebook icon" />
              <span>Facebook</span>
            </button>
          </div>

          <div className="mt-[1rem] flex gap-2 pt-[0.4rem]">
            <span className="text-gray-400">
              {!isRegistered
                ? "Already have an account ?"
                : "Don't have an account ?"}
            </span>
            <span
              className="cursor-pointer text-primary-300"
              onClick={() => setIsRegistered(!isRegistered)}
            >
              {!isRegistered ? "sign in" : "sign up"}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginRegisterForm;
