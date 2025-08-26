import { api } from './client';

// GET /api/users/{userId}/cart
export function getCart(userId) {
  return api.get(`/users/${encodeURIComponent(userId)}/cart`);
}

// POST /api/users/{userId}/cart/items  { gameId }
export function addToCart(userId, gameId) {
  return api.post(`/users/${encodeURIComponent(userId)}/cart/items`, { gameId });
}

// DELETE /api/users/{userId}/cart/items/{gameId}
export function removeFromCart(userId, gameId) {
  return api.del(`/users/${encodeURIComponent(userId)}/cart/items/${gameId}`);
}

// DELETE /api/users/{userId}/cart
export function clearCart(userId) {
  return api.del(`/users/${encodeURIComponent(userId)}/cart`);
}
