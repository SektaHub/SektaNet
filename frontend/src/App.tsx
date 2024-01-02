// App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './layout/HomePage';
import VideoUploader from './layout/VideoUploader';
import VideoList from './layout/VideoList';
import ImageList from './layout/ImageList';
import ImageView from './layout/ImageView';
import ImageUploader from './layout/ImageUpload';

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
