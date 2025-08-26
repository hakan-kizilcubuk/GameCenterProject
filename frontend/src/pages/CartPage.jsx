import { useEffect, useState } from 'react';
import { Container, Typography, List, ListItem, ListItemText, IconButton, Divider, Button } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import { getCart, removeFromCart, clearCart } from '../api/cart';

const USER_ID = 'alice';

export default function CartPage() {
  const [cart, setCart] = useState(null);
  const refresh = () => getCart(USER_ID).then(setCart);

  useEffect(() => { refresh(); }, []);
  if (!cart) return <Container sx={{ py: 5 }}><Typography>Loading…</Typography></Container>;

  const total = `${Number(cart.total.amount||0).toFixed(2)} ${cart.total.currency||'USD'}`;

  return (
    <Container sx={{ py: 5 }}>
      <Typography variant="h4" gutterBottom>Cart</Typography>
      <List dense>
        {cart.items.map(it => (
          <div key={it.id}>
            <ListItem
              secondaryAction={
                <IconButton edge="end" onClick={async()=>{ await removeFromCart(USER_ID, it.id); refresh(); }}>
                  <DeleteIcon />
                </IconButton>
              }
            >
              <ListItemText
                primary={it.title}
                secondary={`${Number(it.price.amount||0).toFixed(2)} ${it.price.currency||'USD'}`}
              />
            </ListItem>
            <Divider component="li" />
          </div>
        ))}
      </List>

      <Typography sx={{ mt: 2, fontWeight: 800 }}>Total: {total}</Typography>
      <Button sx={{ mt: 2 }} variant="contained" color="secondary"
        onClick={async()=>{ /* optionally call purchase endpoint */ }}>
        Checkout (coming soon)
      </Button>
      <Button sx={{ mt: 2, ml: 2 }} onClick={async()=>{ await clearCart(USER_ID); refresh(); }}>
        Clear cart
      </Button>
    </Container>
  );
}
