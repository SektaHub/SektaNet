import React from 'react';
import FileUploader from '../components/FileUpload/FileUploader';

const VideoUploader: React.FC = () => {
  const uploadEndpoint = 'https://localhost:7294/api/Reel/upload-multiple';
  const fileFormDataKey = 'videoFiles'; // Key for videos

  return (
    <div>
      <h1>Upload Reels</h1>
      <FileUploader
        uploadEndpoint={uploadEndpoint}
        fileFormDataKey={fileFormDataKey} // Pass the key to FileUploader
      />
    </div>
  );
};

export default VideoUploader;