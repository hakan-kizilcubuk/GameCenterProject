import { api } from './client';
const uid = (id)=>encodeURIComponent(id);
export const getCart = (userId)=> api.get(`/users/${uid(userId)}/cart`);
export const addToCart = (userId, gameId)=> api.post(`/users/${uid(userId)}/cart/items`, { gameId });
export const removeFromCart = (userId, gameId)=> api.del(`/users/${uid(userId)}/cart/items/${gameId}`);
export const clearCart = (userId)=> api.del(`/users/${uid(userId)}/cart`);
