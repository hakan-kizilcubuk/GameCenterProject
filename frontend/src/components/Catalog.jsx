import { useEffect, useState } from 'react';
import { searchGames } from '../api/games';
import { addToCart, getCart } from '../api/cart';
import { Grid, Card, CardContent, CardActions, Typography, Button, TextField } from '@mui/material';

const USER_ID = 'alice';

export default function Catalog() {
  const [games, setGames] = useState([]);
  const [q, setQ] = useState('');

  useEffect(() => {
    let alive = true;
    searchGames(q).then(d => alive && setGames(d));
    return () => { alive = false; };
  }, [q]);

  return (
    <>
      <TextField label="Search" value={q} onChange={e => setQ(e.target.value)} fullWidth sx={{ mb: 2 }} />
      <Grid container spacing={2}>
        {games.map(g => (
          <Grid key={g.id} item xs={12} sm={6} md={4} lg={3}>
            <Card variant="outlined">
              <CardContent>
                <Typography variant="h6" gutterBottom>{g.title}</Typography>
                <Typography color="text.secondary">{g.releaseDate}</Typography>
                <Typography sx={{ mt: 1.5, fontWeight: 700 }}>
                  {g.price.amount.toFixed(2)} {g.price.currency}
                </Typography>
              </CardContent>
              <CardActions>
                <Button size="small" onClick={async () => {
                  await addToCart(USER_ID, g.id);
                  await getCart(USER_ID);
                }}>Add to cart</Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </>
  );
}
