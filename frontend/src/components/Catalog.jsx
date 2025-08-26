import { useEffect, useState } from 'react';
import { searchGames, addToCart, getCart } from '../api';

const USER_ID = 'alice';

export default function Catalog() {
  const [games, setGames] = useState([]);
  const [q, setQ] = useState('');

  useEffect(() => {
    let mounted = true;
    searchGames(q).then(data => mounted && setGames(data));
    return () => { mounted = false; };
  }, [q]);

  return (
    <div>
      <input value={q} onChange={e => setQ(e.target.value)} placeholder="search..." />
      <ul>
        {games.map(g => (
          <li key={g.id}>
            {g.title} — {g.price.amount.toFixed(2)} {g.price.currency}
            <button onClick={async () => {
              await addToCart(USER_ID, g.id);
              const cart = await getCart(USER_ID);
              console.log('cart:', cart);
              alert(`Added ${g.title}`);
            }}>Add</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
