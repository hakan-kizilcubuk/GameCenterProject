import { createContext, useContext, useEffect, useState } from 'react';
import * as authApi from '../api/auth';

const AuthCtx = createContext(null);
export const useAuth = () => useContext(AuthCtx);

export default function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    try { return JSON.parse(localStorage.getItem('user')||'null'); } catch { return null; }
  });

  const login = async (email, password) => {
    const u = await authApi.login({ email, password });
    setUser(u);
    return u;
  };
  const register = (payload) => authApi.register(payload);
  const logout = () => { authApi.logout(); setUser(null); };

  return <AuthCtx.Provider value={{ user, login, register, logout }}>
    {children}
  </AuthCtx.Provider>;
}
