// App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './components/Pages/HomePage';
import VideoUploader from './components/Pages/VideoUploader';
import VideoList from './components/Pages/VideoList';
import ImageList from './components/Pages/ImageList';
import ImageView from './components/Pages/ImageView';
import ImageUploader from './components/Pages/ImageUploader';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/uploadReel" element={<VideoUploader />} />
        <Route path="/reels" element={<VideoList />} />
        <Route path="/images" element={<ImageList />} />
        <Route path="/images/:imageId" element={<ImageView />} /> 
        <Route path="/uploadImage" element={<ImageUploader />} />
      </Routes>
    </Router>
  );
};

export default App;
