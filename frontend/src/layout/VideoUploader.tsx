import React from 'react';
import FileUploader from '../components/FileUpload/FileUploader';

const VideoUploader: React.FC = () => {
  const handleVideoUpload = async (file: File) => {
    const formData = new FormData();
    formData.append('videoFile', file);

    try {
      const response = await fetch('https://localhost:7294/api/Reel/upload', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        alert('Video uploaded successfully!');
      } else {
        const data = await response.json();
        alert(`Error uploading video: ${data.message}`);
      }
    } catch (error : any) {
      console.error('Error uploading video:', error.message);
      alert('An unexpected error occurred while uploading the video.');
    }
  };

  return (
    <div>
      <h1>Upload Video</h1>
      <FileUploader onFileSelect={handleVideoUpload} />
    </div>
  );
};

export default VideoUploader;
