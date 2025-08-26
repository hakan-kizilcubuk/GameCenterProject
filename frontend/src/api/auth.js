import { api } from './client';

export async function register({ email, password, displayName }) {
  return api.post('/auth/register', { email, password, displayName });
}

export async function login({ email, password }) {
  const data = await api.post('/auth/login', { email, password });
  localStorage.setItem('token', data.token);
  localStorage.setItem('user', JSON.stringify(data.user));
  return data.user;
}

export function logout() {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
}

export async function me() {
  return api.get('/auth/me');
}
