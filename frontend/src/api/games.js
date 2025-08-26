import { api } from './client';
export function searchGames(q='', page=1, pageSize=20) {
  const qs = new URLSearchParams({ q, page, pageSize });
  return api.get(`/games?${qs}`);
}
export const getGameById = (id) => api.get(`/games/${id}`);
