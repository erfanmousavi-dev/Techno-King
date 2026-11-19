import userIcon from "/icons/user-2.svg";
import PasswordIcon from "/icons/key.svg";
import smsIcon from "/icons/sms.svg";
import eyeIcon from "/icons/eye-slash.svg";
import { useForm } from "react-hook-form";
import { useState } from "react";

interface RegisterInputsProps {
   onSubmit: (data: any) => void | Promise<void>;
}

const RegisterInputs = ({ onSubmit }: RegisterInputsProps) => {
 const {
  register,
  handleSubmit,
  formState: { errors, isSubmitting },
} = useForm();

  const [showPassword, setShowPassword] = useState(false);

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="flex flex-col gap-[1rem] w-full"
    >
      <div>
        <div className="flex gap-1 items-center p-[0.5rem] border border-gray-200 rounded-[0.5rem]">
          <img src={userIcon} alt="User icon" />
          <input
            type="text"
            placeholder="Username"
            className="w-full outline-none border-none text-gray-400"
            {...register("userName", { required: "Username is required" })}
          />
        </div>
        {errors.userName && (
          <span className="text-red-500 text-xs">
            {errors.userName.message as string}
          </span>
        )}
      </div>

      <div>
        <div className="flex gap-1 items-center p-[0.5rem] border border-gray-200 rounded-[0.5rem]">
          <img src={smsIcon} alt="Email icon" />
          <input
            type="text"
            placeholder="E-mail"
            className="w-full outline-none border-none text-gray-400"
            {...register("email", { required: "Email is required" })}
          />
        </div>
        {errors.email && (
          <span className="text-red-500 text-xs">
            {errors.email.message as string}
          </span>
        )}
      </div>

      <div>
        <div className="flex items-center justify-between border border-gray-200 rounded-[0.5rem]">
          <div className="flex gap-1 items-center p-[0.5rem] w-full">
            <img src={PasswordIcon} alt="Password icon" />
            <input
              type={showPassword ? "text" : "password"}
              placeholder="Password"
              className="w-full outline-none border-none text-gray-400"
              {...register("password", { required: "Password is required" })}
            />
          </div>
          <img
            src={eyeIcon}
            alt="Eye icon"
            className="pr-[0.5rem] cursor-pointer"
            onClick={() => setShowPassword((prev) => !prev)}
          />
        </div>
        {errors.password && (
          <span className="text-red-500 text-xs">
            {errors.password.message as string}
          </span>
        )}
      </div>

      <div className="flex gap-2 items-center">
        <input
          type="checkbox"
          className="w-[1rem] h-[1rem] border-[2px] border-gray-500 rounded-[0.3rem]"
          {...register("agreeTerms", { required: "You must agree to the terms" })}
        />
        <div className="text-sm">
          <span className="text-gray-400">I agree to all </span>
          <span className="text-primary-300">Terms & Conditions</span>
        </div>
      </div>
      {errors.agreeTerms && (
        <span className="text-red-500 text-xs">
          {errors.agreeTerms.message as string}
        </span>
      )}

     <button
  type="submit"
  disabled={isSubmitting}
  className="w-full bg-primary-300 text-white py-[0.5rem] rounded-[0.5rem] disabled:opacity-50"
>
  {isSubmitting ? "Creating..." : "Create Account"}
</button>
    </form>
  );
};

export default RegisterInputs;