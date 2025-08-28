import { useEffect, useState } from 'react';
import { Container, Typography, Chip, Stack, Avatar } from '@mui/material';
import { getLibrary, purchaseCart } from '../api/library';
import { getCart } from '../api/cart';
import { useAuth } from '../context/AuthContext';

export default function LibraryPage() {
  const { user } = useAuth();
  const [lib, setLib] = useState(null);
  const userId = user?.id;
  // Optionally, allow parent to pass a cart refresh function
  const [_, setCart] = useState(); // dummy state to force cart refresh
  const refresh = () => userId && getLibrary(userId).then(setLib);

  useEffect(() => { refresh(); }, [userId]);
  if (!userId) return <Container sx={{ py: 5 }}><Typography>Please log in to view your library.</Typography></Container>;
  if (!lib) return <Container sx={{ py: 5 }}><Typography>Loading…</Typography></Container>;

  return (
    <Container maxWidth={false} disableGutters sx={{ minHeight: '100vh', width: '100vw', py: 5 }}>
      <Typography variant="h4" gutterBottom>My Library</Typography>
      <Stack direction="row" spacing={1} useFlexGap flexWrap="wrap">
        {lib.games.map(g => (
          <Chip
            key={g.id}
            label={g.title}
            avatar={<Avatar src={g.imageUrl || `https://picsum.photos/seed/${g.id}/64/64`} alt={g.title} />}
            sx={{ minWidth: 120, maxWidth: 200, fontWeight: 500 }}
          />
        ))}
      </Stack>
      <Typography sx={{ mt: 3 }}>
        Want to move your cart items to library?
        <a href="#" onClick={async(e)=>{
          e.preventDefault();
          await purchaseCart(userId);
          refresh();
          // Also refresh the cart so price/items disappear
          await getCart(userId).then(setCart);
        }}> Purchase cart</a>.
      </Typography>
    </Container>
  );
}
