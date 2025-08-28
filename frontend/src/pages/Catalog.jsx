import { useEffect, useState } from 'react';
import { Box, Container, Grid, TextField, Typography } from '@mui/material';
import { searchGames } from '../api/games';
import GameCard from '../components/GameCard';

export default function Catalog() {
  const [q, setQ] = useState('');
  const [games, setGames] = useState([]);

  useEffect(() => {
    let alive = true;
    searchGames(q, 1, 24).then(d => alive && setGames(Array.isArray(d) ? d : []));
    return () => { alive = false; };
  }, [q]);

  return (
    <>
      <Box sx={{
        minHeight: '100vh',
        width: '100vw',
        py: 8,
        background: 'radial-gradient(1200px 400px at 20% -10%, rgba(126,87,194,.35), transparent), radial-gradient(900px 300px at 80% -20%, rgba(38,198,218,.25), transparent)'
      }}>
        <Container maxWidth={false} disableGutters>
          <Typography variant="h3" gutterBottom>Discover your next game</Typography>
          <Typography color="text.secondary" sx={{ mb: 3 }}>
            Curated catalog with instant purchase and your own library.
          </Typography>
          <TextField placeholder="Search games…" value={q} onChange={e=>setQ(e.target.value)} fullWidth />
        </Container>
        <Container maxWidth={false} disableGutters sx={{ py: 4 }}>
          <Grid container spacing={2}>
            {games.map(g => (
              <Grid key={g.id} item xs={12} sm={6} md={4} lg={3}>
                <GameCard game={g} />
              </Grid>
            ))}
          </Grid>
        </Container>
      </Box>
    </>
  );
}
