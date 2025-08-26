const API = import.meta.env.VITE_API_URL || '/api';
const authHeaders = () => {
  const t = localStorage.getItem('token');
  return t ? { Authorization: `Bearer ${t}` } : {};
};
async function request(path, { method='GET', body, headers } = {}) {
  const res = await fetch(`${API}${path}`, {
    method,
    headers: { 'Content-Type':'application/json', ...authHeaders(), ...(headers||{}) },
    body: body ? JSON.stringify(body) : undefined
  });
  if (!res.ok) throw new Error((await res.text()) || res.statusText);
  return res.status === 204 ? null : res.json();
}
export const api = {
  get: (p) => request(p),
  post: (p,b) => request(p, { method:'POST', body:b }),
  del: (p) => request(p, { method:'DELETE' })
};
