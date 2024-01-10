import React from 'react';
// import 'bootstrap/dist/css/bootstrap.css';
import VideoUploader from '../components/Uploaders/VideoUploader';
import ImageUpload from '../components/Uploaders/ImageUploader';
import "../Mantastrap.css";

const Uploader: React.FC = () => {

  return (
    <div className='uploader-cont'>
          <div className='margina-xl'>
              <ImageUpload />
          </div>
          <div className='margina-xl'>
              <VideoUploader />
          </div>
    </div>
        
  );
};

export default Uploader;