import React, { useState, useEffect } from 'react';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

const VideoList: React.FC = () => {
  const [videos, setVideos] = useState<{
    id: string;
    thumbnail: string;
    audioTranscription: string | null;
    duration: number | null;
  }[]>([]);

  const fetchThumbnail = async (videoId: string) => {
    try {
      const response = await fetch(`https://localhost:7294/api/Reel/${videoId}/thumbnail`);
      if (response.ok) {
        const blob = await response.blob();
        const thumbnailUrl = URL.createObjectURL(blob);
        return thumbnailUrl;
      } else {
        console.error(`Error fetching thumbnail for video ${videoId}: ${response.statusText}`);
        return ''; // Return an empty string or a default thumbnail URL
      }
    } catch (error: any) {
      console.error(`Error fetching thumbnail for video ${videoId}: ${error.message}`);
      return ''; // Return an empty string or a default thumbnail URL
    }
  };

  const handleDeleteClick = async (videoId: string) => {
    try {
      const response = await fetch(`https://localhost:7294/api/Reel/${videoId}`, {
        method: 'DELETE',
      });

      if (response.ok) {
        // Update the videos list after deletion
        const updatedVideos = videos.filter((video) => video.id !== videoId);
        setVideos(updatedVideos);
      } else {
        console.error(`Error deleting video ${videoId}: ${response.statusText}`);
      }
    } catch (error: any) {
      console.error(`Error deleting video ${videoId}: ${error.message}`);
    }
  };

  useEffect(() => {
    const fetchVideos = async () => {
      try {
        const response = await fetch('https://localhost:7294/api/Reel');
        const data = await response.json();

        if (response.ok) {
          const videosWithThumbnails = await Promise.all(
            data.map(async (video: any) => {
              const thumbnail = await fetchThumbnail(video.id);
              return { ...video, thumbnail };
            })
          );

          setVideos(videosWithThumbnails);
        } else {
          console.error('Error fetching videos:', data);
        }
      } catch (error: any) {
        console.error('Error fetching videos:', error.message);
      }
    };

    fetchVideos();
  }, []);

  return (
    <div>
      <h2 style={{ color: 'white' }}>Video List</h2>
      <Row xs={1} md={3} className="g-4">
        {videos.map((video) => (
          <Col key={video.id}>
            <Card>
              <Card.Img variant="top" src={video.thumbnail} alt={`Thumbnail for Video ${videos.indexOf(video) + 1}`} />
              <Card.Body>
                <Card.Title style={{ color: 'white' }}>Video {videos.indexOf(video) + 1}</Card.Title>
                <Card.Text style={{ color: 'white' }}>ID: {video.id}</Card.Text>
                <Card.Text style={{ color: 'white' }}>Duration: {video.duration} seconds</Card.Text>
                <Button variant="primary" href={`http://localhost:5173/?videoId=${video.id}`} target="_blank" rel="noopener noreferrer">
                  Watch Video
                </Button>
                <Button variant="danger" onClick={() => handleDeleteClick(video.id)}>
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

export default VideoList;