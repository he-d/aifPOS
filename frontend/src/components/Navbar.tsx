import React from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  IconButton,
} from '@mui/material';
import { ShoppingCart, Person, ExitToApp } from '@mui/icons-material';
import { useAuth } from '../hooks/useAuth';

const Navbar: React.FC = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          ClubPOS
        </Typography>
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button
            color="inherit"
            component={RouterLink}
            to="/"
            startIcon={<ShoppingCart />}
          >
            Sales
          </Button>
          <Button
            color="inherit"
            component={RouterLink}
            to="/products"
            startIcon={<Person />}
          >
            Products
          </Button>
          <IconButton color="inherit" onClick={handleLogout}>
            <ExitToApp />
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar; 