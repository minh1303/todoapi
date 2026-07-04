import { createContext, useContext, useState } from "react";
import type { ReactNode } from 'react';


export interface AuthContextType {
  token: string | null; 
  setToken: (token: string | null) => void;
  loading: boolean;
  error : string | null;
  login: (email: string, password: string) => Promise<void>;
}
interface AuthProviderProps {
  children: ReactNode; // This means "any React components inside"
}


const AuthContext = createContext<AuthContextType | undefined>(undefined);

const apiUrl = import.meta.env.VITE_API_URL;

export function AuthProvider({ children }: AuthProviderProps) {
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  async function login(email: string, password: string) {
    setLoading(true);
    setError(null); 
    const response = await fetch(`${apiUrl}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      setError("Login failed");
      setLoading(false);
      return;
    }

    const data = await response.json();
    console.log("Login successful, received token:", data.token);
    setToken(data.token);
    setLoading(false);  
    localStorage.setItem("token", data.token); // Store the token in localStorage
  }
  const value = {
    token,
    setToken,
    loading,
    error,
    login
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};