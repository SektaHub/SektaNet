import React, { useState } from 'react';

const VideoUploader: React.FC = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0] || null;
    setSelectedFile(file);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      alert('Please select a file before uploading.');
      return;
    }

    const formData = new FormData();
    formData.append('videoFile', selectedFile);

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
      <input type="file" accept="video/*" onChange={handleFileChange} />
      <button onClick={handleUpload}>Upload Video</button>
    </div>
  );
};

export default VideoUploader;
