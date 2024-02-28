// App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './layout/HomePage';
import VideoUploader from './components/Uploaders/VideoUploader';
import ReelList from './layout/ReelList';
import VideoList from './layout/VideoList';
import ImageList from './layout/ImageList';
import ImageView from './layout/ImageView';
import ImageUploader from './components/Uploaders/ImageUploader';
import Layout from './layout/Layout';
import Uploader from './layout/Uploader';
import Login from './layout/Login';
import Register from './layout/Register';
import FileList from './layout/FileList';

const App: React.FC = () => {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path='/login' element={<Login />}></Route>
          <Route path='/register' element={<Register />}></Route>
          <Route path="/uploadReel" element={<VideoUploader />} />
          <Route path="/reels" element={<ReelList />} />
          <Route path="/videos" element={<VideoList />} />
          <Route path="/images" element={<ImageList />} />
          <Route path="/files" element={<FileList />} />
          <Route path="/images/:imageId" element={<ImageView />} /> 
          <Route path="/uploadImage" element={<ImageUploader />} />
          <Route path="/upload" element={<Uploader />} />
        </Routes>
      </Layout>
    </Router>
  );
};

export default App;
