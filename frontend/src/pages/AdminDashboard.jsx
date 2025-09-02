import { useState, useEffect } from 'react';
import { Container, Typography, Box, TextField, Button, MenuItem, Select, InputLabel, FormControl } from '@mui/material';
import { useAuth } from '../context/AuthContext';
import { searchGames } from '../api/games';
import { createDiscount } from '../api/discounts';
import { addEdition } from '../api/editions';

export default function AdminDashboard() {
  const { user } = useAuth();
  const [discount, setDiscount] = useState({ gameId: '', percentage: '', amount: '', startDate: '', endDate: '' });
  const [edition, setEdition] = useState({ gameId: '', name: '', description: '', amount: '', currency: 'USD' });
  const [message, setMessage] = useState('');
  const [editionMsg, setEditionMsg] = useState('');
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
  const handleEditionChange = e => {
    setEdition({ ...edition, [e.target.name]: e.target.value });
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

  // Add Edition
  const handleEditionSubmit = async e => {
    e.preventDefault();
    setEditionMsg('');
    try {
      await addEdition(
        edition.gameId,
        edition.name,
        edition.description,
        Number(edition.amount),
        edition.currency
      );
      setEditionMsg('Edition added successfully!');
      setEdition({ gameId: '', name: '', description: '', amount: '', currency: 'USD' });
    } catch (err) {
      setEditionMsg('Error: ' + (err.message || 'Could not add edition'));
    }
  };

  return (
    <Container maxWidth={false} disableGutters sx={{ minHeight: '100vh', width: '100vw', py: 8, display: 'flex', alignItems: 'flex-start', justifyContent: 'center', gap: 6 }}>
      <Box sx={{ width: 400 }}>
        <Typography variant="h4" gutterBottom>Admin Dashboard</Typography>
  <Box component="form" onSubmit={handleSubmit} sx={{ display:'grid', gap:2, mb: 6 }}>
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

        {/* Add Edition Form */}
  <Box component="form" onSubmit={handleEditionSubmit} sx={{ display:'grid', gap:2 }}>
          <Typography variant="h6" gutterBottom>Add Game Edition</Typography>
          <FormControl fullWidth>
            <InputLabel>Game</InputLabel>
            <Select name="gameId" value={edition.gameId} label="Game" onChange={handleEditionChange} required>
              {games.map(g => (
                <MenuItem key={g.id} value={g.id}>{g.title}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField name="name" label="Edition Name" value={edition.name} onChange={handleEditionChange} required />
          <TextField name="description" label="Edition Description" value={edition.description} onChange={handleEditionChange} required />
          <TextField name="amount" label="Edition Price" type="number" value={edition.amount} onChange={handleEditionChange} required />
          <TextField name="currency" label="Currency" value={edition.currency} onChange={handleEditionChange} required />
          <Button variant="contained" type="submit">Add Edition</Button>
          {editionMsg && <Typography color={editionMsg.startsWith('Error') ? 'error' : 'success.main'}>{editionMsg}</Typography>}
        </Box>
      </Box>
    </Container>
  );
}
