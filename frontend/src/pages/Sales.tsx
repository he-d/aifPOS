import React, { useEffect, useState } from 'react';
import {
  Container,
  Paper,
  Typography,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Box,
  CircularProgress,
  Grid,
} from '@mui/material';
import axios from 'axios';

interface Product {
  id: number;
  name: string;
  price: number;
  stock: number;
}

interface SaleItem {
  productId: number;
  quantity: number;
  price: number;
}

interface Sale {
  id: number;
  items: SaleItem[];
  total: number;
  date: string;
}

const Sales: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [sales, setSales] = useState<Sale[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [open, setOpen] = useState(false);
  const [selectedProducts, setSelectedProducts] = useState<SaleItem[]>([]);
  const [cashBalance, setCashBalance] = useState(0);

  const fetchData = async () => {
    try {
      const [productsResponse, salesResponse, balanceResponse] = await Promise.all([
        axios.get('/api/products'),
        axios.get('/api/sales'),
        axios.get('/api/sales/balance'),
      ]);
      setProducts(productsResponse.data);
      setSales(salesResponse.data);
      setCashBalance(balanceResponse.data);
    } catch (err) {
      setError('Failed to load data');
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleOpen = () => {
    setSelectedProducts([]);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setSelectedProducts([]);
  };

  const handleAddProduct = (product: Product) => {
    const existingItem = selectedProducts.find(
      (item) => item.productId === product.id
    );

    if (existingItem) {
      setSelectedProducts(
        selectedProducts.map((item) =>
          item.productId === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        )
      );
    } else {
      setSelectedProducts([
        ...selectedProducts,
        { productId: product.id, quantity: 1, price: product.price },
      ]);
    }
  };

  const handleRemoveProduct = (productId: number) => {
    setSelectedProducts(
      selectedProducts.filter((item) => item.productId !== productId)
    );
  };

  const handleUpdateQuantity = (productId: number, quantity: number) => {
    if (quantity <= 0) {
      handleRemoveProduct(productId);
      return;
    }

    setSelectedProducts(
      selectedProducts.map((item) =>
        item.productId === productId ? { ...item, quantity } : item
      )
    );
  };

  const calculateTotal = () => {
    return selectedProducts.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
  };

  const handleSubmit = async () => {
    try {
      await axios.post('/api/sales', {
        items: selectedProducts,
        total: calculateTotal(),
      });
      handleClose();
      fetchData();
    } catch (err) {
      setError('Failed to create sale');
      console.error('Error creating sale:', err);
    }
  };

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

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Box display="flex" justifyContent="space-between" alignItems="center">
            <Typography variant="h4">Sales</Typography>
            <Box>
              <Typography variant="h6" sx={{ mr: 2 }}>
                Cash Balance: ${cashBalance.toFixed(2)}
              </Typography>
              <Button variant="contained" color="primary" onClick={handleOpen}>
                New Sale
              </Button>
            </Box>
          </Box>
        </Grid>

        {error && (
          <Grid item xs={12}>
            <Typography color="error">{error}</Typography>
          </Grid>
        )}

        <Grid item xs={12}>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Date</TableCell>
                  <TableCell>Items</TableCell>
                  <TableCell align="right">Total</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {sales.map((sale) => (
                  <TableRow key={sale.id}>
                    <TableCell>
                      {new Date(sale.date).toLocaleString()}
                    </TableCell>
                    <TableCell>
                      {sale.items.map((item) => {
                        const product = products.find(
                          (p) => p.id === item.productId
                        );
                        return (
                          <div key={item.productId}>
                            {product?.name} x {item.quantity}
                          </div>
                        );
                      })}
                    </TableCell>
                    <TableCell align="right">
                      ${sale.total.toFixed(2)}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Grid>
      </Grid>

      <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
        <DialogTitle>New Sale</DialogTitle>
        <DialogContent>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <Typography variant="h6">Available Products</Typography>
              <TableContainer>
                <Table size="small">
                  <TableHead>
                    <TableRow>
                      <TableCell>Name</TableCell>
                      <TableCell align="right">Price</TableCell>
                      <TableCell align="right">Stock</TableCell>
                      <TableCell align="right">Action</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {products.map((product) => (
                      <TableRow key={product.id}>
                        <TableCell>{product.name}</TableCell>
                        <TableCell align="right">
                          ${product.price.toFixed(2)}
                        </TableCell>
                        <TableCell align="right">{product.stock}</TableCell>
                        <TableCell align="right">
                          <Button
                            size="small"
                            onClick={() => handleAddProduct(product)}
                          >
                            Add
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </Grid>

            <Grid item xs={12}>
              <Typography variant="h6">Selected Items</Typography>
              <TableContainer>
                <Table size="small">
                  <TableHead>
                    <TableRow>
                      <TableCell>Name</TableCell>
                      <TableCell align="right">Price</TableCell>
                      <TableCell align="right">Quantity</TableCell>
                      <TableCell align="right">Subtotal</TableCell>
                      <TableCell align="right">Action</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {selectedProducts.map((item) => {
                      const product = products.find(
                        (p) => p.id === item.productId
                      );
                      return (
                        <TableRow key={item.productId}>
                          <TableCell>{product?.name}</TableCell>
                          <TableCell align="right">
                            ${item.price.toFixed(2)}
                          </TableCell>
                          <TableCell align="right">
                            <TextField
                              type="number"
                              size="small"
                              value={item.quantity}
                              onChange={(e) =>
                                handleUpdateQuantity(
                                  item.productId,
                                  parseInt(e.target.value)
                                )
                              }
                              inputProps={{ min: 1 }}
                            />
                          </TableCell>
                          <TableCell align="right">
                            ${(item.price * item.quantity).toFixed(2)}
                          </TableCell>
                          <TableCell align="right">
                            <Button
                              size="small"
                              color="error"
                              onClick={() => handleRemoveProduct(item.productId)}
                            >
                              Remove
                            </Button>
                          </TableCell>
                        </TableRow>
                      );
                    })}
                  </TableBody>
                </Table>
              </TableContainer>
            </Grid>

            <Grid item xs={12}>
              <Typography variant="h6" align="right">
                Total: ${calculateTotal().toFixed(2)}
              </Typography>
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button
            onClick={handleSubmit}
            variant="contained"
            color="primary"
            disabled={selectedProducts.length === 0}
          >
            Complete Sale
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default Sales; 