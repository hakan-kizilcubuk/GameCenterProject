import { api } from './client';

// GET /api/users/{userId}/library
export function getLibrary(userId) {
  return api.get(`/users/${encodeURIComponent(userId)}/library`);
}

// POST /api/users/{userId}/library/purchase
export function purchaseCart(userId) {
  return api.post(`/users/${encodeURIComponent(userId)}/library/purchase`);
}
