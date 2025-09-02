import { api } from './client';

export function addEdition(gameId, name, description, amount, currency) {
  return api.post(`/games/${gameId}/editions`, {
    name,
    description,
    price: { amount, currency }
  });
}
