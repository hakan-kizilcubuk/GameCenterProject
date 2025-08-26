import { api } from './client';

// GET /api/games?q=&page=&pageSize=
export function searchGames(q = '', page = 1, pageSize = 20) {
  const qs = new URLSearchParams();
  if (q) qs.set('q', q);
  qs.set('page', String(page));
  qs.set('pageSize', String(pageSize));
  return api.get(`/games?${qs.toString()}`);
}

// GET /api/games/{id}
export function getGameById(id) {
  return api.get(`/games/${id}`);
}
