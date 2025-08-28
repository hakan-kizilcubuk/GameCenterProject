import { createTheme } from '@mui/material/styles';

export const getTheme = (mode = 'light') => createTheme({
  palette: {
    mode,
    primary: { main: '#6c37c8ff' },    
    secondary: { main: '#05aac0ff' },  
    background: mode === 'dark'
      ? { default: '#0f0f12', paper: '#17171c' }
      : { default: '#f2f3f5', paper: '#fff' },
    text: mode === 'dark'
      ? { primary: '#f2f3f5', secondary: '#b7bac1' }
      : { primary: '#222', secondary: '#555' }
  },
  shape: { borderRadius: 12 },
  components: {
    MuiCard: { styleOverrides: { root: { background: mode === 'dark' ? '#1c1f27' : '#fff' } } },
    MuiAppBar: { styleOverrides: { root: { background: 'linear-gradient(90deg,#6a4fb3,#26c6da)' } } }
  },
  typography: {
    fontFamily: '"Inter", system-ui, -apple-system, Segoe UI, Roboto, "Helvetica Neue", Arial',
    h3: { fontWeight: 800 },
    h6: { fontWeight: 700 }
  }
});
