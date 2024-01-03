import React, { useState } from 'react';

interface FileUploaderProps {
  onFileSelect: (file: File) => void;
}

const FileUploader: React.FC<FileUploaderProps> = ({ onFileSelect }) => {
  const [selectedFiles, setSelectedFiles] = useState<File[]>([]);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files || [];
    setSelectedFiles(Array.from(files));
  };

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    const files = event.dataTransfer.files || [];
    setSelectedFiles(Array.from(files));
  };

  const handleUpload = async () => {
    if (selectedFiles.length === 0) {
      alert('Please select files before uploading.');
      return;
    }
  
    try {
      const formData = new FormData();
  
      // Append each selected file to the FormData
      selectedFiles.forEach((file) => {
        // Modify this part to match the expected format on the server side
        formData.append('imageFiles', file);
      });
  
      const response = await fetch('https://localhost:7294/api/Image/upload-multiple', {
        method: 'POST',
        body: formData,
      });
  
      if (response.ok) {
        alert('Images uploaded successfully!');
      } else {
        const data = await response.json();
        const responseText = await response.text(); // Add this line
        console.log('Response content:', responseText); // Add this line
        alert(`Error uploading images: ${data.message}`);
      }
    } catch (error: any) {
      console.error('Error uploading images:', error.message);
      alert('An unexpected error occurred while uploading the images.');
      
    } finally {
      setSelectedFiles([]); // Clear selected files after upload
    };
  };

  return (
    <div>
      <div
        style={dropzoneStyle}
        onDrop={handleDrop}
        onDragOver={(e) => e.preventDefault()}
      >
        <input type="file" multiple onChange={handleFileChange} />
        <p>Drag 'n' drop some files here, or click to select files</p>
      </div>
      <div>
        <h2>Selected Files</h2>
        <ul>
          {selectedFiles.map((file, index) => (
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

export default FileUploader;
