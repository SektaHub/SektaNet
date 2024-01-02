import React, { useState } from 'react';
import { useDropzone } from 'react-dropzone';

const UploadImage: React.FC = () => {
  const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);

  const onDrop = (acceptedFiles: File[]) => {
    // Handle the dropped files
    setUploadedFiles([...uploadedFiles, ...acceptedFiles]);
  };

  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  const handleUpload = async () => {
    // Check if there are files to upload
    if (uploadedFiles.length === 0) {
      return;
    }
  
    const formData = new FormData();
    uploadedFiles.forEach((file, index) => {
      formData.append('imageFiles', file, file.name);
    });
  
    try {
      const response = await fetch('https://localhost:7294/api/Image/upload-multiple', {
        method: 'POST',
        body: formData,
      });
  
      if (response.ok) {
        // Handle successful upload (e.g., show a success message)
        console.log('Files uploaded successfully');
      } else {
        // Handle upload failure (e.g., show an error message)
        console.error('Upload failed');
      }
    } catch (error) {
      console.error('Error uploading files:', error);
    }
  };
  
  

  return (
    <div>
      <h1>Upload Images</h1>
      <div {...getRootProps()} style={dropzoneStyle}>
        <input {...getInputProps()} />
        {isDragActive ? <p>Drop the files here ...</p> : <p>Drag 'n' drop some files here, or click to select files</p>}
      </div>
      <div>
        <h2>Uploaded Files</h2>
        <ul>
          {uploadedFiles.map((file, index) => (
            <li key={index}>{file.name}</li>
          ))}
        </ul>
      </div>
      <button onClick={handleUpload}>Upload</button>
    </div>
  );
};

const dropzoneStyle: React.CSSProperties = {
  border: '2px dashed #cccccc',
  borderRadius: '4px',
  padding: '20px',
  textAlign: 'center',
  cursor: 'pointer',
};

export default UploadImage;
