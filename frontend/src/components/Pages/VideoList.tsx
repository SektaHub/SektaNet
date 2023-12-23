import React, { useState, useEffect } from 'react';

const VideoList: React.FC = () => {
  const [videos, setVideos] = useState<{ id: string; audioTranscription: string | null; duration: number | null }[]>([]);

  useEffect(() => {
    const fetchVideos = async () => {
      try {
        const response = await fetch('https://localhost:7294/api/Reel');
        const data = await response.json();

        if (response.ok) {
          setVideos(data);
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
      <h2>Video List</h2>
      <ul>
        {videos.map((video) => (
          <li key={video.id}>
            <a href={`http://localhost:5173/?videoId=${video.id}`} target="_blank" rel="noopener noreferrer">
              Video {videos.indexOf(video) + 1}
            </a>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default VideoList;
