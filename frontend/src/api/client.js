// Base URL: with Vite proxy use '/api'. If you ever deploy without a proxy,
// set VITE_API_URL in .env and this will use it instead.
const API_BASE = import.meta.env.VITE_API_URL || '/api';

async function request(path, { method = 'GET', body, headers } = {}) {
  const res = await fetch(`${API_BASE}${path}`, {
    method,
    headers: { 'Content-Type': 'application/json', ...(headers || {}) },
    body: body ? JSON.stringify(body) : undefined,
  });

  // Handle non-2xx
  if (!res.ok) {
    let msg = res.statusText;
    try { msg = (await res.text()) || msg; } catch {}
    throw new Error(msg);
  }

  // No content
  if (res.status === 204) return null;

  // JSON result
  return res.json();
}

export const api = {
  get: (path) => request(path),
  post: (path, body) => request(path, { method: 'POST', body }),
  del: (path) => request(path, { method: 'DELETE' }),
  put: (path, body) => request(path, { method: 'PUT', body }),
  patch: (path, body) => request(path, { method: 'PATCH', body }),
};
