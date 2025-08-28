import { useState, useEffect } from 'react';
import { Container, Typography, Box, TextField, Button, MenuItem, Select, InputLabel, FormControl } from '@mui/material';
import { useAuth } from '../context/AuthContext';
import { searchGames } from '../api/games';
import { createDiscount } from '../api/discounts';

export default function AdminDashboard() {
  const { user } = useAuth();
  const [discount, setDiscount] = useState({ gameId: '', percentage: '', amount: '', startDate: '', endDate: '' });
  const [message, setMessage] = useState('');
  const [games, setGames] = useState([]);
  useEffect(() => {
    searchGames().then(g => setGames(g.items || g));
  }, []);

  if (!user || user.role !== 'admin') {
    return <Container sx={{ py: 8 }}><Typography>You must be an admin to access this page.</Typography></Container>;
  }

  const handleChange = e => {
    setDiscount({ ...discount, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    setMessage('');
    try {
      await createDiscount({
        gameId: discount.gameId,
        percentage: Number(discount.percentage) / 100,
        amount: discount.amount ? Number(discount.amount) : null,
        startDate: discount.startDate,
        endDate: discount.endDate
      }, user.id);
      setMessage('Discount created successfully!');
      setDiscount({ gameId: '', percentage: '', amount: '', startDate: '', endDate: '' });
    } catch (err) {
      setMessage('Error: ' + (err.message || 'Could not create discount'));
    }
  };

  return (
    <Container maxWidth={false} disableGutters sx={{ minHeight: '100vh', width: '100vw', py: 8, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <Box sx={{ width: 400 }}>
        <Typography variant="h4" gutterBottom>Admin Dashboard</Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ display:'grid', gap:2 }}>
          <FormControl fullWidth>
            <InputLabel>Game</InputLabel>
            <Select name="gameId" value={discount.gameId} label="Game" onChange={handleChange} required>
              {games.map(g => (
                <MenuItem key={g.id} value={g.id}>{g.title}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField name="percentage" label="Discount Percentage (%)" type="number" value={discount.percentage} onChange={handleChange} required />
          <TextField name="amount" label="Discount Amount (optional)" type="number" value={discount.amount} onChange={handleChange} />
          <TextField name="startDate" label="Start Date" type="date" value={discount.startDate} onChange={handleChange} InputLabelProps={{ shrink: true }} required />
          <TextField name="endDate" label="End Date" type="date" value={discount.endDate} onChange={handleChange} InputLabelProps={{ shrink: true }} required />
          <Button variant="contained" type="submit">Create Discount</Button>
          {message && <Typography color={message.startsWith('Error') ? 'error' : 'success.main'}>{message}</Typography>}
        </Box>
      </Box>
    </Container>
  );
}
