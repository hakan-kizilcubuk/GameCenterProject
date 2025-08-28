import { useState } from 'react';
import { Container, Box, TextField, Button, Typography, Alert } from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const nav = useNavigate();
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [err, setErr] = useState('');

  const submit = async (e) => {
    e.preventDefault();
    setErr('');
    try { await login(email, password); nav('/'); }
    catch (e) { setErr(e.message || 'Login failed'); }
  };

  return (
  <Container maxWidth={false} disableGutters sx={{ minHeight: '100vh', width: '100vw', py: 8, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <Typography variant="h4" gutterBottom>Welcome back</Typography>
      {err && <Alert severity="error" sx={{ mb: 2 }}>{err}</Alert>}
      <Box component="form" onSubmit={submit} sx={{ display:'grid', gap:2 }}>
        <TextField label="Email" value={email} onChange={e=>setEmail(e.target.value)} required />
        <TextField label="Password" type="password" value={password} onChange={e=>setPassword(e.target.value)} required />
        <Button variant="contained" type="submit">Login</Button>
        <Typography color="text.secondary">No account? <Link to="/register">Sign up</Link></Typography>
      </Box>
    </Container>
  );
}
