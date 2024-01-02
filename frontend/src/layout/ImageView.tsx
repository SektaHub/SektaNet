import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

interface Image {
  id: string;
  // Add other image properties as needed
}

const ImageView: React.FC = () => {
  const { imageId } = useParams<{ imageId?: string }>();
  const [image, setImage] = useState<Image | null>(null);

  useEffect(() => {
    // Fetch the specific image by ID from your API or backend
    if (imageId) {
      fetch(`https://localhost:7294/api/Image/${imageId}`)
        .then(response => response.blob())
        .then(blob => {
          const objectURL = URL.createObjectURL(blob);
          setImage({ id: imageId }); // Set other image properties if needed
          const img = new Image();
          img.src = objectURL;
          img.onload = () => {
            // Clean up the object URL after the image has loaded
            URL.revokeObjectURL(objectURL);
          };
        })
        .catch(error => console.error('Error fetching image:', error));
    }
  }, [imageId]);

  if (!image) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Image View</h1>
      <div>
        {/* Display image */}
        <img src={`https://localhost:7294/api/Image/${image.id}`} alt={`Image ${image.id}`} style={{ maxWidth: '100%' }} />
        <p>{/* Display other image properties */}</p>
      </div>
    </div>
  );
};

export default ImageView;
