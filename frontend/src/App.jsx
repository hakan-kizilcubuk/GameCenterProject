import { ThemeProvider, CssBaseline, Box } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import theme from './theme';
import AuthProvider from './context/AuthContext';
import ProtectedRoute from './routes/ProtectedRoute';

import NavBar from './components/NavBar';
import Catalog from './pages/Catalog';
import GameDetails from './pages/GameDetails';
import CartPage from './pages/CartPage';
import LibraryPage from './pages/LibraryPage';
import Login from './pages/Login';
import Register from './pages/Register';
import AdminDashboard from './pages/AdminDashboard';

export default function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <BrowserRouter>
          <NavBar />
          <Box sx={{ minHeight: '100vh' }}>
            <Routes>
              <Route path="/" element={<Catalog />} />
              <Route path="/games/:id" element={<GameDetails />} />
              <Route path="/cart" element={<ProtectedRoute><CartPage /></ProtectedRoute>} />
              <Route path="/library" element={<ProtectedRoute><LibraryPage /></ProtectedRoute>} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/admin" element={<ProtectedRoute><AdminDashboard /></ProtectedRoute>} />
            </Routes>
          </Box>
        </BrowserRouter>
      </AuthProvider>
    </ThemeProvider>
  );
}
