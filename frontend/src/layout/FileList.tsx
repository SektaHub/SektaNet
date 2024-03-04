import React, { useState, useEffect } from 'react';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { API_URL } from '../config';
//import { BASE_URL } from '../config'; // Import the generic file icon image

const FileList: React.FC = () => {
  const [files, setFiles] = useState<{
    id: string;
    name: string;
  }[]>([]);

  const handleDeleteClick = async (fileId: string) => {
    try {
      const response = await fetch(`${API_URL}/GenericFile/${fileId}`, {
        method: 'DELETE',
      });

      if (response.ok) {
        // Update the files list after deletion
        const updatedFiles = files.filter((file) => file.id !== fileId);
        setFiles(updatedFiles);
      } else {
        console.error(`Error deleting file ${fileId}: ${response.statusText}`);
      }
    } catch (error: any) {
      console.error(`Error deleting file ${fileId}: ${error.message}`);
    }
  };

  useEffect(() => {
    const fetchFiles = async () => {
      try {
        const response = await fetch(`${API_URL}/GenericFile`); // Ensure correct endpoint
        const data = await response.json();

        if (response.ok) {
          setFiles(data);
        } else {
          console.error('Error fetching files:', data);
        }
      } catch (error: any) {
        console.error('Error fetching files:', error.message);
      }
    };

    fetchFiles();
  }, []);

  return (
    <div>
      <h2 style={{ color: 'white' }}>Files</h2>
      <Row xs={1} md={3} className="g-4">
        {files.map((file) => (
          <Col key={file.id} xs={12} sm={6} md={4} lg={3}>
            <Card>
              <div style={{ width: '200px', height: '150px' }}>
                <img style={{ objectFit: 'cover', width: '100%', height: '100%' }} src={"/fajl.png"} alt={`Generic File Icon for File ${files.indexOf(file) + 1}`} />
              </div>
              <Card.Body>
                <Card.Title style={{ color: 'white' }}>{file.name}</Card.Title>
                <Button variant="danger" onClick={() => handleDeleteClick(file.id)}>
                  Delete
                </Button>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default FileList;
