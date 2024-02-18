import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL } from '../config';

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    useEffect(() => {
        const interceptor = axios.interceptors.request.use(config => {
            const token = localStorage.getItem('accessToken');
            if(token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        });

        return () => {
            axios.interceptors.request.eject(interceptor);
        };
    }, []);

    const handleSubmit = async (e : any) => {
        e.preventDefault();
        try {
            const response = await axios.post(`${API_URL}/identity/login`, { email, password });
            const { accessToken } = response.data;
            localStorage.setItem('accessToken', accessToken);
            alert('Login successful');
            // Redirect user or update app state
        } catch (error) {
            // You can customize this part to handle different types of errors differently
            alert('Login failed');
        }
    };

    const handleLogout = () => {
      localStorage.removeItem('accessToken'); // Clear access token
      window.location.href = '/login'; // Redirect to login page
  };

    return (
      <div className="wrapper">
          <form onSubmit={handleSubmit}>
              <h1>Login</h1>
              <div className="input">
                  <input type="text" value={email} onChange={(e) => setEmail(e.target.value)} placeholder='Username' required />
              </div>
              <div className="input">
                  <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder='Password' required />
              </div>

              <div className="remember">
                  <label>
                      <input type="checkbox" />Remember me
                  </label>
                  <a href="#">Forgot password?</a>
              </div>

              <button type='submit'>Login</button>

              <div className="register">
                  <p>Don't have an account? <a href='/register'>Register</a></p>
              </div>
          </form>

          <button onClick={handleLogout}>Logout</button> {/* Logout button */}
      </div>
    );
}

export default Login;
