import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

interface Image {
  id: string;
  // Add other image properties as needed
}

const ImageList: React.FC = () => {
  const [images, setImages] = useState<Image[]>([]);

  useEffect(() => {
    // Fetch images from your API or backend
    fetch('https://localhost:7294/api/Image').then(response => response.json()).then(data => setImages(data));
  }, []);

  return (
    <div>
      <h1>Image List</h1>
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        {images.map(image => (
          <Link key={image.id} to={`/images/${image.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
            <div style={{ border: '1px solid #ccc', margin: '10px', padding: '10px', cursor: 'pointer', width: '200px', height: '250px', overflow: 'hidden' }}>
              {/* Display image properties or thumbnail */}
              <img
                src={`https://localhost:7294/api/Image/${image.id}`}
                alt={`Image ${image.id}`}
                style={{ width: '100%', height: '100%', objectFit: 'cover' }}
              />
              <p>{/* Display other image properties */}</p>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
};

export default ImageList;
