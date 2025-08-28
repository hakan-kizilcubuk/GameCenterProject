import { api } from './client';
const uid = (id)=>encodeURIComponent(id);
export const createDiscount = (discount, adminUserId) =>
  api.post(`/admin/discounts?adminUserId=${uid(adminUserId)}`, discount);
