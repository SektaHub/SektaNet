import React from 'react';
import FileUploader from '../components/FileUpload/FileUploader';

const ImageUpload: React.FC = () => {
  const uploadEndpoint = 'https://localhost:7294/api/Image/upload-multiple';
  const fileFormDataKey = 'imageFiles'; // Key for images

  return (
    <div>
      <h1>Upload Images</h1>
      <FileUploader
        uploadEndpoint={uploadEndpoint}
        fileFormDataKey={fileFormDataKey} // Pass the key to FileUploader
      />
    </div>
  );
};

export default ImageUpload;