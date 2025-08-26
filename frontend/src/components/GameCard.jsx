import { Card, CardActionArea, CardContent, CardMedia, Typography, CardActions, Button, Chip, Stack } from '@mui/material';
import { Link } from 'react-router-dom';
import { addToCart, getCart } from '../api/cart';

const USER_ID = 'alice';

export default function GameCard({ game }) {
  const img = game.imageUrl || `https://picsum.photos/seed/${game.id}/600/800`;
  const amount = Number(game?.price?.amount ?? 0).toFixed(2);
  const currency = game?.price?.currency ?? 'USD';

  return (
    <Card sx={{ height: '100%', transform: 'translateZ(0)', transition: 'all .2s', '&:hover': { transform: 'translateY(-4px)' } }}>
      <CardActionArea component={Link} to={`/games/${game.id}`}>
        <CardMedia component="img" height="200" image={img} alt={game.title} />
        <CardContent>
          <Typography variant="h6" gutterBottom noWrap>{game.title}</Typography>
          <Stack direction="row" spacing={1} sx={{ mb: .5 }}>
            {game.genres?.slice?.(0,2)?.map(g => <Chip key={g.id||g} size="small" label={g.name||g} />)}
          </Stack>
          <Typography sx={{ fontWeight: 800 }}>{amount} {currency}</Typography>
        </CardContent>
      </CardActionArea>
      <CardActions>
        <Button onClick={async()=>{ await addToCart(USER_ID, game.id); await getCart(USER_ID); }} size="small">
          Add to cart
        </Button>
      </CardActions>
    </Card>
  );
}
