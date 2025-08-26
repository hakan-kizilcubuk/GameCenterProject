import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: { main: '#7e57c2' },    // purple
    secondary: { main: '#26c6da' },  // teal
    background: { default: '#0f0f12', paper: '#17171c' },
    text: { primary: '#f2f3f5', secondary: '#b7bac1' }
  },
  shape: { borderRadius: 12 },
  components: {
    MuiCard: { styleOverrides: { root: { background: '#1c1f27' } } },
    MuiAppBar: { styleOverrides: { root: { background: 'linear-gradient(90deg,#6a4fb3,#26c6da)' } } }
  },
  typography: {
    fontFamily: '"Inter", system-ui, -apple-system, Segoe UI, Roboto, "Helvetica Neue", Arial',
    h3: { fontWeight: 800 },
    h6: { fontWeight: 700 }
  }
});

export default theme;
