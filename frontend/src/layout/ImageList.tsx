import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { API_URL } from '../config';
import { fetchWithAuth } from '../api';

interface Image {
  id: string;
  // Add other image properties as needed
}

interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
}

const ImageList: React.FC = () => {
  const [images, setImages] = useState<Image[]>([]);
  const [searchCaption, setSearchCaption] = useState<string>('');
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(0);
  const [totalItems, setTotalItems] = useState<number>(0);
  const itemsPerPage = 14; // Items per page, could be configurable

  useEffect(() => {
    let url = `${API_URL}/Image/PaginatedSemantic`;

    const queryParams = [`page=${currentPage}`, `pageSize=${itemsPerPage}`];
    if (searchCaption) {
      queryParams.push(`captionSearch=${encodeURIComponent(searchCaption)}`);
    }

    fetchWithAuth(`${url}?${queryParams.join('&')}`)
      .then(response => response.json())
      .then((data: PaginatedResponse<Image>) => {
        setImages(data.items);
        setTotalItems(data.totalCount);
        setTotalPages(Math.ceil(data.totalCount / itemsPerPage));
      });
  }, [searchCaption, currentPage]);

  const goToFirstPage = () => setCurrentPage(1);
  const goToLastPage = () => setCurrentPage(totalPages);
  const goToNextPage = () => setCurrentPage(c => c + 1);
  const goToPreviousPage = () => setCurrentPage(c => Math.max(1, c - 1));

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
            border: '1px solid #ccc',
            borderRadius: '5px',
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
      {images ? (
        images.map(image => (
          <Link key={image.id} to={`/images/${image.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
            <div style={{ border: '1px solid #ccc', margin: '10px', padding: '10px', cursor: 'pointer', width: '200px', height: '250px', overflow: 'hidden' }}>
              <img
                src={`${API_URL}/Image/${image.id}/Content`}
                alt={`Image ${image.id}`}
                style={{ width: '100%', height: '100%', objectFit: 'cover' }}
              />
              {/* You can display more properties here */}
            </div>
          </Link>
        ))
      ) : (
        <p>Loading...</p>
      )}
    </div>
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: '20px' }}>
        <button onClick={goToFirstPage} disabled={currentPage === 1}>First</button>
        <button onClick={goToPreviousPage} disabled={currentPage === 1}>Previous</button>
        <span style={{ margin: '0 10px' }}>{`Page ${currentPage} of ${totalPages} | Total Items: ${totalItems}`}</span>
        <button onClick={goToNextPage} disabled={currentPage === totalPages}>Next</button>
        <button onClick={goToLastPage} disabled={currentPage === totalPages}>Last</button>
      </div>
    </div>
  );
};

export default ImageList;