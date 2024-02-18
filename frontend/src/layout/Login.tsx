import React, {useState, useEffect} from 'react';
import axios from 'axios';
import { API_URL } from '../config';
import CryptoJS from 'crypto-js';

function Login() {
    const [email, setUsername] = useState('');
    const [password, setPassword] = useState('');

    useEffect(() => {
        // Add a request interceptor to include the token in all requests
        const interceptor = axios.interceptors.request.use(config => {
          const token = localStorage.getItem('accessToken');
          if (token) {
            config.headers.Authorization = `Bearer ${token}`;
          }
          return config;
        });
    
        // Clean up the interceptor when the component unmounts
        return () => {
          axios.interceptors.request.eject(interceptor);
        };
      }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
          const response = await axios.post(`${API_URL}/identity/login`, {
            email,
            password
          });
          const { accessToken } = response.data;
        //   const encryptedToken = encryptToken(accessToken);
            localStorage.setItem('accessToken', accessToken);
          alert('Login successful');
          console.log('Login successful:', response.data);
          // You can add further logic here, such as redirecting the user or setting authentication tokens
        } catch (error) {
          alert('Login failed');
          // Handle error, such as showing an error message to the user
        }
      };

    //   const encryptToken = (token) => {
    //     return CryptoJS.AES.encrypt(token, 'secret').toString();
    //   };
    
    //   const decryptToken = (encryptedToken) => {
    //     const bytes = CryptoJS.AES.decrypt(encryptedToken, 'secret');
    //     return bytes.toString(CryptoJS.enc.Utf8);
    //   };

  return (
    <div>

        <div className="wrapper">
            <form action="" onSubmit={handleSubmit}>
                <h1>Login</h1>
                <div className="input">
                    <input type="text" value={email} onChange={(e) => setUsername(e.target.value)} placeholder='Username' required/>
                </div>
                <div className="input">
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder='Password' required/>
                </div>

                <div className="remember">
                    <label htmlFor="">
                        <input type="checkbox" />Remember me </label>
                        <a href="#">Forgot password?</a>
                </div>
                
                <button type='submit'>Login</button>

                <div className="register">
                    <p>Don't have an account? <a href='/register'>Register</a></p>
                </div>
            </form>
        </div>

    </div>
  )
}

export default Login