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
} from '@mui/material';
import axios from 'axios';

interface Product {
  id: number;
  name: string;
  price: number;
  stock: number;
}

const Products: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [open, setOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    price: '',
    stock: '',
  });

  const fetchProducts = async () => {
    try {
      const response = await axios.get('/api/products');
      setProducts(response.data);
    } catch (err) {
      setError('Failed to load products');
      console.error('Error loading products:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const handleOpen = (product?: Product) => {
    if (product) {
      setEditingProduct(product);
      setFormData({
        name: product.name,
        price: product.price.toString(),
        stock: product.stock.toString(),
      });
    } else {
      setEditingProduct(null);
      setFormData({ name: '', price: '', stock: '' });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingProduct(null);
    setFormData({ name: '', price: '', stock: '' });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingProduct) {
        await axios.put(`/api/products/${editingProduct.id}`, {
          ...formData,
          price: parseFloat(formData.price),
          stock: parseInt(formData.stock),
        });
      } else {
        await axios.post('/api/products', {
          ...formData,
          price: parseFloat(formData.price),
          stock: parseInt(formData.stock),
        });
      }
      handleClose();
      fetchProducts();
    } catch (err) {
      setError('Failed to save product');
      console.error('Error saving product:', err);
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      try {
        await axios.delete(`/api/products/${id}`);
        fetchProducts();
      } catch (err) {
        setError('Failed to delete product');
        console.error('Error deleting product:', err);
      }
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
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4">Products</Typography>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Add Product
        </Button>
      </Box>

      {error && (
        <Typography color="error" sx={{ mb: 2 }}>
          {error}
        </Typography>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell align="right">Price</TableCell>
              <TableCell align="right">Stock</TableCell>
              <TableCell align="right">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {products.map((product) => (
              <TableRow key={product.id}>
                <TableCell>{product.name}</TableCell>
                <TableCell align="right">${product.price.toFixed(2)}</TableCell>
                <TableCell align="right">{product.stock}</TableCell>
                <TableCell align="right">
                  <Button
                    color="primary"
                    onClick={() => handleOpen(product)}
                    sx={{ mr: 1 }}
                  >
                    Edit
                  </Button>
                  <Button
                    color="error"
                    onClick={() => handleDelete(product.id)}
                  >
                    Delete
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>
          {editingProduct ? 'Edit Product' : 'Add New Product'}
        </DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              label="Name"
              fullWidth
              required
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
            />
            <TextField
              margin="dense"
              label="Price"
              type="number"
              fullWidth
              required
              value={formData.price}
              onChange={(e) =>
                setFormData({ ...formData, price: e.target.value })
              }
            />
            <TextField
              margin="dense"
              label="Stock"
              type="number"
              fullWidth
              required
              value={formData.stock}
              onChange={(e) =>
                setFormData({ ...formData, stock: e.target.value })
              }
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" color="primary">
              {editingProduct ? 'Save' : 'Add'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </Container>
  );
};

export default Products; 