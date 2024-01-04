import React, { useState } from 'react';

interface FileUploaderProps {
  uploadEndpoint: string;
  fileFormDataKey: string; // New prop to specify the FormData key
}

const FileUploader: React.FC<FileUploaderProps> = ({ uploadEndpoint, fileFormDataKey }) => {
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
      selectedFiles.forEach((file) => {
        formData.append(fileFormDataKey, file); // Use the passed in key for form data
      });

      const response = await fetch(uploadEndpoint, {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        let errorMessage = 'Error uploading files.';
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
          try {
            const data = await response.json();
            errorMessage += ` ${data.message}`; // Assuming the error message is in the `message` property
          } catch (err) {
            errorMessage += ' Could not parse JSON error message.';
          }
        } else {
          try {
            const responseText = await response.text();
            errorMessage += ` Response: ${responseText}`;
          } catch (err) {
            errorMessage += ' Could not retrieve the error message.';
          }
        }
        alert(errorMessage);
      } else {
        alert('Files uploaded successfully!');
        setSelectedFiles([]); // Clear selected files after upload
      }
    } catch (error: any) {
      console.error('Error uploading files:', error);
      alert('An unexpected error occurred while uploading the files.');
    }
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