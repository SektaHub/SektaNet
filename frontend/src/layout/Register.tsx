import {useState} from 'react';
import axios from 'axios';
import { API_URL } from '../config';

function Register() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async (e : any) => {
        e.preventDefault();
        try{
            const response = await axios.post(`${API_URL}/identity/register`, {
                email, password
            });
            alert('Registration successful');
            console.log('Registration successful:', response.data);
        } catch(error){
            alert('Registration failed');
            console.error('Registration failed:', error);
        }
    }
  return (
    <div>

        <div className="wrapper">
            <form action="" onSubmit={handleRegister}>
                <h1>Register</h1>
                <div className="input">
                    <input type="email" name="" value={email} onChange={(e) => setEmail(e.target.value)} placeholder='Email' required/>
                </div>
                <div className="input">
                    <input type="password" name="" value={password} onChange={(e) => setPassword(e.target.value)} placeholder='Password' required/>
                </div>
                
                <button type='submit'>Sign up</button>

                <div className="register">
                    <p>Already have an account? <a href='/login'>Login</a></p>
                </div>
            </form>
        </div>

    </div>
  )
}

export default Register