// App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './components/Pages/HomePage';
import VideoUploader from './components/Pages/VideoUploader';
import VideoList from './components/Pages/VideoList';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/upload" element={<VideoUploader />} />
        <Route path="/reels" element={<VideoList />} />
      </Routes>
    </Router>
  );
};

export default App;
