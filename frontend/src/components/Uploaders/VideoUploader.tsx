import React from 'react';
import FileUploader from '../FileUpload/FileUploader';
import { API_URL } from '../../config';

const VideoUploader: React.FC = () => {
  const uploadEndpoint = `${API_URL}/Reel/upload-multiple`;
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