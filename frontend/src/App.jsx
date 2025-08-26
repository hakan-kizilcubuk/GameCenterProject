import { BrowserRouter, Routes, Route, Link, Navigate } from 'react-router-dom';
import { createTheme, ThemeProvider, CssBaseline, AppBar, Toolbar, Button, Container, Box, Snackbar, Alert } from '@mui/material';
import { useEffect, useState } from 'react';
import Catalog from './components/Catalog';
import { login, logout } from './api/auth';

const theme = createTheme({ palette: { mode: 'light', primary: { main: '#7e57c2' } } });

function useAuth() {
  const [user, setUser] = useState(() => JSON.parse(localStorage.getItem('user') || 'null'));
  const signIn = async (email, password) => setUser(await login({ email, password }));
  const signOut = () => { logout(); setUser(null); };
  return { user, signIn, signOut };
}

function Protected({ children }) {
  const hasToken = !!localStorage.getItem('token');
  return hasToken ? children : <Navigate to="/" replace />;
}

export default function App() {
  const auth = useAuth();
  const [msg, setMsg] = useState('');

  useEffect(() => {
    if (auth.user) setMsg(`Welcome, ${auth.user.displayName || auth.user.email}!`);
  }, [auth.user]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <AppBar position="static">
          <Toolbar>
            <Box sx={{ flexGrow: 1 }}>
              <Button color="inherit" component={Link} to="/">Catalog</Button>
              <Button color="inherit" component={Link} to="/cart">Cart</Button>
              <Button color="inherit" component={Link} to="/library">Library</Button>
            </Box>
            {auth.user ? (
              <Button color="inherit" onClick={auth.signOut}>Logout</Button>
            ) : (
              <LoginButtons onLogin={auth.signIn} />
            )}
          </Toolbar>
        </AppBar>

        <Container sx={{ py: 3 }}>
          <Routes>
            <Route path="/" element={<Catalog />} />
            <Route path="/cart" element={<Protected><CartPage /></Protected>} />
            <Route path="/library" element={<Protected><LibraryPage /></Protected>} />
          </Routes>
        </Container>

        <Snackbar open={!!msg} autoHideDuration={2500} onClose={() => setMsg('')}>
          <Alert severity="success" variant="filled">{msg}</Alert>
        </Snackbar>
      </BrowserRouter>
    </ThemeProvider>
  );
}

/* --- small components --- */
function LoginButtons({ onLogin }) {
  const [open, setOpen] = useState(false);
  return (
    <>
      <Button color="inherit" onClick={() => setOpen(true)}>Login</Button>
      {open && <LoginDialog onClose={() => setOpen(false)} onLogin={onLogin} />}
    </>
  );
}

import { Dialog, DialogTitle, DialogContent, TextField, DialogActions } from '@mui/material';
function LoginDialog({ onClose, onLogin }) {
  const [email, setEmail] = useState('test@example.com');
  const [password, setPassword] = useState('Password1!');
  const [err, setErr] = useState('');

  return (
    <Dialog open onClose={onClose}>
      <DialogTitle>Sign in</DialogTitle>
      <DialogContent sx={{ pt: 2, display: 'grid', gap: 2, minWidth: 360 }}>
        <TextField label="Email" value={email} onChange={e => setEmail(e.target.value)} fullWidth />
        <TextField label="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} fullWidth />
        {err && <Alert severity="error">{err}</Alert>}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button variant="contained" onClick={async () => {
          try { await onLogin(email, password); onClose(); } catch (e) { setErr(String(e.message || e)); }
        }}>Login</Button>
      </DialogActions>
    </Dialog>
  );
}

/* placeholders – keep your existing implementations or wire from earlier */
import CartPage from './pages/CartPage';
import LibraryPage from './pages/LibraryPage';
