import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { API_URL } from '../config';

interface Image {
  id: string;
  generatedCaption: string | null;
}

const ImageView: React.FC = () => {
  const { imageId } = useParams<{ imageId?: string }>();
  const [image, setImage] = useState<Image | null>(null);
  const [similarImages, setSimilarImages] = useState<Image[]>([]);

  useEffect(() => {
    // Fetch image data including the caption
    fetch(`${API_URL}/Image/${imageId}/Data`)
      .then(response => response.json())
      .then(data => setImage(data))
      .catch(error => console.error('Error fetching image data:', error));
  }, [imageId]);

  useEffect(() => {
    if (image) {
      // Fetch top 5 conceptually similar images
      fetch(`${API_URL}/Image/GetConceptuallySimmilarImages/${image.id}`)
        .then(response => response.json())
        .then(data => setSimilarImages(data))
        .catch(error => console.error('Error fetching similar images:', error));
    }
  }, [image]);

  if (!image) {
    return <div>Loading...</div>;
  }

  return (
    <div style={{ display: 'flex' }}>
      <div style={{ flex: 1 }}>
        {/* Display main image */}
        <img
          src={`${API_URL}/Image/${image.id}`}
          alt={`Image ${image.id}`}
          style={{ width: '80%', height: 'auto', maxHeight: '100vh' }}
        />
      </div>
      <div style={{ flex: 1, marginLeft: '20px' }}>
        {/* Display main image properties */}
        <h1>Image View</h1>
        <p>{`Caption: ${image.generatedCaption || 'No caption available'}`}</p>
        {/* Add more properties as needed */}
        
        {/* Display top 5 conceptually similar images in a 1x5 grid */}
        <h2>Conceptually Similar Images</h2>
        <div style={{ display: 'flex', flexWrap: 'wrap' }}>
          {similarImages.map(similarImage => (
            <Link key={similarImage.id} to={`/images/${similarImage.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
              <div style={{ border: '1px solid #ccc', margin: '10px', padding: '10px', cursor: 'pointer', width: '100px', height: '100px', overflow: 'hidden' }}>
                {/* Display image thumbnail */}
                <img
                  src={`${API_URL}/Image/${similarImage.id}`}
                  alt={`Image ${similarImage.id}`}
                  style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                />
              </div>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ImageView;
