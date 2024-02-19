import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { API_URL } from '../config';
import { fetchWithAuth } from '../api';

interface Image {
  id: string;
  // Add other image properties as needed
}

const ImageList: React.FC = () => {
  const [images, setImages] = useState<Image[]>([]);
  const [searchCaption, setSearchCaption] = useState<string>('');
  const [currentPage, setCurrentPage] = useState<number>(1); // Pagination state
  const itemsPerPage = 14; // Items per page, could be configurable

  useEffect(() => {
    // Fetch images from your API or backend based on the search caption
    let url = `${API_URL}/Image`;

    const queryParams = [`page=${currentPage}`, `pageSize=${itemsPerPage}`];
    if (searchCaption) {
      queryParams.push(`caption=${encodeURIComponent(searchCaption)}`);
    }

    const fullUrl = `${url}?${queryParams.join('&')}`;

    fetchWithAuth(fullUrl)
      .then(response => response.json())
      .then(data => setImages(data));
  }, [searchCaption, currentPage]);

  // Basic pagination controls
  const goToNextPage = () => {
    setCurrentPage(c => c + 1);
  };

  const goToPreviousPage = () => {
    setCurrentPage(c => c > 1 ? c - 1 : 1);
  };

  return (
    <div>
      <h1>Image List</h1>
      <div style={{ marginBottom: '10px', position: 'relative' }}>
        <input
          type="text"
          placeholder="Search by caption"
          value={searchCaption}
          onChange={(e) => setSearchCaption(e.target.value)}
          style={{
            padding: '10px',
            fontSize: '16px',
            width: '30%',
            boxSizing: 'border-box',
            borderRadius: '5px',
            border: '1px solid #ccc',
            marginBottom: '10px',
          }}
        />
        {searchCaption && (
          <span
            style={{
              position: 'absolute',
              top: '50%',
              right: '10px',
              transform: 'translateY(-50%)',
              cursor: 'pointer',
            }}
            onClick={() => setSearchCaption('')}
          >
            &times;
          </span>
        )}
      </div>
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        {images.map(image => (
          <Link key={image.id} to={`/images/${image.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
            <div style={{ border: '1px solid #ccc', margin: '10px', padding: '10px', cursor: 'pointer', width: '200px', height: '250px', overflow: 'hidden' }}>
              <img
                src={`${API_URL}/Image/${image.id}/Content`}
                alt={`Image ${image.id}`}
                style={{ width: '100%', height: '100%', objectFit: 'cover' }}
              />
              <p>{/* Display other image properties */}</p>
            </div>
          </Link>
        ))}
      </div>
      <div style={{ display: 'flex', justifyContent: 'center', marginTop: '20px' }}>
        {/* Pagination Controls */}
        <button onClick={goToPreviousPage} disabled={currentPage === 1}>
          Previous
        </button>
        <span style={{ margin: '0 10px' }}>
          Page {currentPage}
        </span>
        <button onClick={goToNextPage}>
          Next
        </button>
      </div>
    </div>
  );
};

export default ImageList;