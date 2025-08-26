import { api } from './client';
const uid = (id)=>encodeURIComponent(id);
export const getLibrary = (userId)=> api.get(`/users/${uid(userId)}/library`);
export const purchaseCart = (userId)=> api.post(`/users/${uid(userId)}/library/purchase`);
