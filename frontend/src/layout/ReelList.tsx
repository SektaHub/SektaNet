import React, { useState, useEffect } from 'react';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { API_URL } from '../config';
import { BASE_URL } from '../config';
import { fetchWithAuth } from '../api';

const ReelList: React.FC = () => {
  const [videos, setVideos] = useState<{
    id: string;
    thumbnail: string;
    audioTranscription: string | null;
    duration: number | null;
  }[]>([]);

  const fetchThumbnail = async (videoId: string) => {
    try {
      const response = await fetchWithAuth(`${API_URL}/Reel/${videoId}/thumbnail`);
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
      const response = await fetchWithAuth(`${API_URL}/Reel/${videoId}`, {
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
        const response = await fetchWithAuth(`${API_URL}/Reel`);
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
          <Col key={video.id} xs={12} sm={6} md={4} lg={3}>
            <Card>
              <div style={{ width: '200px', height: '150px' }}>
                <Card.Img style={{ objectFit: 'cover', width: '100%', height: '100%' }} variant="top" src={video.thumbnail} alt={`Thumbnail for Video ${videos.indexOf(video) + 1}`} />
              </div>
              <Card.Body>
                <Card.Title style={{ color: 'white' }}>Video {videos.indexOf(video) + 1}</Card.Title>
                <Card.Text style={{ color: 'white' }}>ID: {video.id}</Card.Text>
                <Card.Text style={{ color: 'white'}}>Duration: {video.duration} seconds</Card.Text>
                <Card.Text style={{ color: 'white' , maxWidth: '26vh'}}>Transcription: {video.audioTranscription}</Card.Text>
                <Button variant="primary" href={`${BASE_URL}/?videoId=${video.id}`} target="_blank" rel="noopener noreferrer">
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

export default ReelList;
