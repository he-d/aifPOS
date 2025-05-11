import React, { useEffect, useState } from 'react';
import {
  Container,
  Grid,
  Paper,
  Typography,
  Box,
  CircularProgress,
} from '@mui/material';
import axios from 'axios';

interface DashboardStats {
  totalSales: number;
  cashBalance: number;
  productCount: number;
}

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [salesResponse, balanceResponse, productsResponse] = await Promise.all([
          axios.get('/api/sales/total'),
          axios.get('/api/sales/balance'),
          axios.get('/api/products/count'),
        ]);

        setStats({
          totalSales: salesResponse.data,
          cashBalance: balanceResponse.data,
          productCount: productsResponse.data,
        });
      } catch (err) {
        setError('Failed to load dashboard statistics');
        console.error('Error loading dashboard:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchStats();
  }, []);

  if (loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="80vh"
      >
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Container>
        <Typography color="error" variant="h6">
          {error}
        </Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Grid container spacing={3}>
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 240,
            }}
          >
            <Typography component="h2" variant="h6" color="primary" gutterBottom>
              Total Sales
            </Typography>
            <Typography component="p" variant="h4">
              ${stats?.totalSales.toFixed(2)}
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 240,
            }}
          >
            <Typography component="h2" variant="h6" color="primary" gutterBottom>
              Cash Balance
            </Typography>
            <Typography component="p" variant="h4">
              ${stats?.cashBalance.toFixed(2)}
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 240,
            }}
          >
            <Typography component="h2" variant="h6" color="primary" gutterBottom>
              Total Products
            </Typography>
            <Typography component="p" variant="h4">
              {stats?.productCount}
            </Typography>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default Dashboard; 