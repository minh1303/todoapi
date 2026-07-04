import { useAuth } from "../AuthContext";
import { useState } from "react";


export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const {error, login} = useAuth();
function handleEmailChange(event: React.ChangeEvent<HTMLInputElement>) {
    setEmail(event.target.value);
  }

  function handlePasswordChange(event: React.ChangeEvent<HTMLInputElement>) {
    setPassword(event.target.value);
  }
  async function handleSubmit(e : React.MouseEvent<HTMLButtonElement>) {
    e.preventDefault();
    await login(email, password);
  }
  return (
    <div className="login-container text-center flex flex-col items-center justify-center h-screen gap-4 ">
      <div className="login-form flex flex-col gap-4 border border-black-300 p-8 rounded shadow-md justify-end items-end">
        <span className="text-lg font-medium">
          Email{" "}
          <input
            className="border border-black-300 rounded py-1 px-1"
            type="email"
            name="email"
            id="email"
            value={email}
            onChange={handleEmailChange}
          />
        </span>
        <span className="text-lg font-medium">
          Password{" "}
          <input
            className="border border-black-300 rounded py-1 px-1"
            type="password"
            name="password"
            id="password"
            value={password}
            onChange={handlePasswordChange}
          />
        </span>
        {error && <p className="text-red-500">{error}</p>}
        <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={handleSubmit}>
          Login
        </button >
      </div>
    </div>
  );
}
