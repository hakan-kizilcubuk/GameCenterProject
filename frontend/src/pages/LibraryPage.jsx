import { useEffect, useState } from 'react';
import { Container, Typography, Chip, Stack } from '@mui/material';
import { getLibrary, purchaseCart } from '../api/library';
const USER_ID = 'alice';

export default function LibraryPage() {
  const [lib, setLib] = useState(null);
  const refresh = () => getLibrary(USER_ID).then(setLib);

  useEffect(() => { refresh(); }, []);
  if (!lib) return <Container sx={{ py: 5 }}><Typography>Loading…</Typography></Container>;

  return (
    <Container sx={{ py: 5 }}>
      <Typography variant="h4" gutterBottom>My Library</Typography>
      <Stack direction="row" spacing={1} useFlexGap flexWrap="wrap">
        {lib.games.map(g => <Chip key={g.id} label={g.title} />)}
      </Stack>
      <Typography sx={{ mt: 3 }}>
        Want to move your cart items to library?
        <a href="#" onClick={async(e)=>{ e.preventDefault(); await purchaseCart(USER_ID); refresh(); }}> Purchase cart</a>.
      </Typography>
    </Container>
  );
}
