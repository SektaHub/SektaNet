import React from 'react';
import FileUploader from '../components/FileUpload/FileUploader';
import { API_URL } from '../config';

const Uploader: React.FC = () => {
  const uploadEndpoint = `${API_URL}/AnyFile/upload-multiple`;
  const fileFormDataKey = 'files'; // Key for videos

  return (
    <div>
      <h1>Upload Images/Reels</h1>
      <FileUploader
        uploadEndpoint={uploadEndpoint}
        fileFormDataKey={fileFormDataKey} // Pass the key to FileUploader
      />
    </div>
  );
};

export default Uploader;