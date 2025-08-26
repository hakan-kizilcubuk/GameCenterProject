import { AppBar, Toolbar, Button, Box, IconButton, Menu, MenuItem, Avatar } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useAuth } from '../context/AuthContext';

export default function NavBar() {
  const { user, logout } = useAuth();
  const [anchor, setAnchor] = useState(null);
  const nav = useNavigate();

  return (
    <AppBar position="sticky" elevation={6}>
      <Toolbar sx={{ gap: 2 }}>
        <Box sx={{ fontWeight: 900, letterSpacing: 2 }}>GAMECENTER</Box>

        <Box sx={{ ml: 2, display: 'flex', gap: 1 }}>
          <Button component={Link} to="/" color="inherit">Catalog</Button>
          <Button component={Link} to="/cart" color="inherit">Cart</Button>
          <Button component={Link} to="/library" color="inherit">Library</Button>
        </Box>

        <Box sx={{ flexGrow: 1 }} />
        {user ? (
          <>
            <IconButton color="inherit" onClick={(e)=>setAnchor(e.currentTarget)}>
              <Avatar sx={{ width: 32, height: 32 }}>{(user.displayName||user.email||'?')[0]?.toUpperCase()}</Avatar>
            </IconButton>
            <Menu open={!!anchor} anchorEl={anchor} onClose={()=>setAnchor(null)}>
              <MenuItem onClick={()=>{ setAnchor(null); nav('/'); }}>Home</MenuItem>
              <MenuItem onClick={()=>{ logout(); setAnchor(null); nav('/login'); }}>Logout</MenuItem>
            </Menu>
          </>
        ) : (
          <>
            <Button component={Link} to="/login" variant="outlined" color="inherit">Login</Button>
            <Button component={Link} to="/register" variant="contained" color="secondary">Sign up</Button>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
}
