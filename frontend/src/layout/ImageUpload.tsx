import React from 'react';
import FileUploader from '../components/FileUpload/FileUploader';

const UploadImage: React.FC = () => {
  const handleImageUpload = async (file: File) => {
    const formData = new FormData();
    formData.append('imageFiles', file);

    try {
      const response = await fetch('https://localhost:7294/api/Image/upload', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        alert('Images uploaded successfully!');
      } else {
        const data = await response.json();
        alert(`Error uploading images: ${data.message}`);
      }
    } catch (error : any) {
      console.error('Error uploading images:', error.message);
      alert('An unexpected error occurred while uploading the images.');
    }
  };

  return (
    <div>
      <h1>Upload Images</h1>
      <FileUploader onFileSelect={handleImageUpload} />
    </div>
  );
};

export default UploadImage;
