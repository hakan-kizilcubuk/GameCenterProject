import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Box, Container, Typography, Stack, Chip, Button } from '@mui/material';
import { getGameById } from '../api/games';
import { addToCart, getCart } from '../api/cart';

const USER_ID = 'alice';

export default function GameDetails() {
  const { id } = useParams();
  const [game, setGame] = useState(null);

  useEffect(() => { getGameById(id).then(setGame); }, [id]);
  if (!game) return <Container sx={{ py: 5 }}><Typography>Loading…</Typography></Container>;

  const img = game.imageUrl || `https://picsum.photos/seed/${game.id}/1200/800`;
  const amount = Number(game?.price?.amount ?? 0).toFixed(2);
  const currency = game?.price?.currency ?? 'USD';

  return (
    <Box sx={{ py: 6 }}>
      <Container>
        <img src={img} alt={game.title} style={{ width: '100%', borderRadius: 16, boxShadow: '0 16px 40px rgba(0,0,0,.35)' }} />
        <Typography variant="h3" sx={{ mt: 3 }}>{game.title}</Typography>
        <Stack direction="row" spacing={1} sx={{ my: 1 }}>
          {game.tags?.map(t => <Chip key={t.id||t} label={t.name||t} size="small" />)}
        </Stack>
        <Typography color="text.secondary" sx={{ my: 2 }}>{game.description}</Typography>
        <Typography variant="h5" sx={{ fontWeight: 900, mb: 2 }}>{amount} {currency}</Typography>
        <Button variant="contained" onClick={async()=>{ await addToCart(USER_ID, id); await getCart(USER_ID); }}>
          Add to cart
        </Button>
      </Container>
    </Box>
  );
}
