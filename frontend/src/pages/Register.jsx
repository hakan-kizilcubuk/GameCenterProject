import { useState } from 'react';
import { Container, Box, TextField, Button, Typography, Alert } from '@mui/material';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

export default function Register() {
  const { register } = useAuth();
  const nav = useNavigate();
  const [form, setForm] = useState({ email:'', displayName:'', password:'' });
  const [err, setErr] = useState('');

  const submit = async (e) => {
    e.preventDefault();
    setErr('');
    try { await register(form); nav('/login'); }
    catch (e) { setErr(e.message || 'Register failed'); }
  };

  return (
    <Container sx={{ py: 8, maxWidth: 520 }}>
      <Typography variant="h4" gutterBottom>Create your account</Typography>
      {err && <Alert severity="error" sx={{ mb: 2 }}>{err}</Alert>}
      <Box component="form" onSubmit={submit} sx={{ display:'grid', gap:2 }}>
        <TextField label="Email" value={form.email} onChange={e=>setForm({...form, email:e.target.value})} required />
        <TextField label="Display name" value={form.displayName} onChange={e=>setForm({...form, displayName:e.target.value})} />
        <TextField label="Password" type="password" value={form.password} onChange={e=>setForm({...form, password:e.target.value})} required />
        <Button variant="contained" type="submit">Sign up</Button>
      </Box>
    </Container>
  );
}
