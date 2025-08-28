
import { useEffect, useState } from 'react';
import { Container, Typography, List, ListItem, ListItemText, IconButton, Divider, Button, Avatar, ListItemAvatar } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import { getCart, removeFromCart, clearCart } from '../api/cart';
import { useAuth } from '../context/AuthContext';

export default function CartPage() {
  const { user } = useAuth();
  const [cart, setCart] = useState(null);
  const userId = user?.id;
  const refresh = () => userId && getCart(userId).then(setCart);

  useEffect(() => { refresh(); }, [userId]);
  if (!userId) return <Container sx={{ py: 5 }}><Typography>Please log in to view your cart.</Typography></Container>;
  if (!cart) return <Container sx={{ py: 5 }}><Typography>Loading…</Typography></Container>;

  const total = `${Number(cart.total.amount||0).toFixed(2)} ${cart.total.currency||'USD'}`;

  return (
    <Container maxWidth={false} disableGutters sx={{ minHeight: '100vh', width: '100vw', py: 5 }}>
      <Typography variant="h4" gutterBottom>Cart</Typography>
      <List dense>
        {cart.items.map(it => (
          <div key={it.id}>
            <ListItem
              secondaryAction={
                <IconButton edge="end" onClick={async()=>{ if(userId){ await removeFromCart(userId, it.id); refresh(); } }}>
                  <DeleteIcon />
                </IconButton>
              }
            >
              <ListItemAvatar>
                <Avatar src={it.imageUrl || `https://picsum.photos/seed/${it.id}/64/64`} alt={it.title} />
              </ListItemAvatar>
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
      <Button sx={{ mt: 2, ml: 2 }} onClick={async()=>{
        if(userId){
          await clearCart(userId);
          // Always fetch the latest cart after clearing (API returns 204)
          await getCart(userId).then(setCart);
        }
      }}>
        Clear cart
      </Button>
    </Container>
  );
}
