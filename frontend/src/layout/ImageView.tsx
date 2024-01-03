import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

interface Image {
  id: string;
  generatedCaption: string | null; // Add other image properties as needed
}

const ImageView: React.FC = () => {
  const { imageId } = useParams<{ imageId?: string }>();
  const [image, setImage] = useState<Image | null>(null);

  useEffect(() => {
    // Fetch image data including the caption
    fetch(`https://localhost:7294/api/Image/${imageId}/Data`)
      .then(response => response.json())
      .then(data => setImage(data))
      .catch(error => console.error('Error fetching image data:', error));
  }, [imageId]);

  if (!image) {
    return <div>Loading...</div>;
  }

  return (
    <div style={{ display: 'flex' }}>
      <div style={{ flex: 1 }}>
        {/* Display image */}
        <img
          src={`https://localhost:7294/api/Image/${image.id}`}
          alt={`Image ${image.id}`}
          style={{ width: '100%', height: 'auto', maxHeight: '100vh' }}
        />
      </div>
      <div style={{ flex: 1, marginLeft: '20px' }}>
        {/* Display other image properties */}
        <h1>Image View</h1>
        <p>{`Caption: ${image.generatedCaption || 'No caption available'}`}</p>
        {/* Add more properties as needed */}
      </div>
    </div>
  );
};

export default ImageView;
