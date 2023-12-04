import React, { useRef, useEffect, useState } from 'react';
import { useInView } from 'react-intersection-observer';
import "./Reels.css";

export default function Reels() {
  const vidRef = useRef(null);
  const [ref, inView] = useInView({
    threshold: 0.5,
  });
  const [videoUrl, setVideoUrl] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('https://localhost:7294/api/Reel');
        console.log(response); // Add this line to log the response
        if (response.ok) {
          // Set the response directly as the video URL
          setVideoUrl(URL.createObjectURL(await response.blob()));
        } else {
          console.error('Failed to fetch video URL');
        }
      } catch (error) {
        console.error('Error fetching video URL', error);
      }
    };
    
    if (inView && !videoUrl) {
      fetchData();
    }

    if (inView) {
      vidRef.current?.play();
    } else {
      vidRef.current?.pause();
    }
  }, [inView, videoUrl]);

  return (
    <div ref={ref} className='reel-card'>
      {videoUrl && (
        <video className="reel-player" ref={vidRef} src={videoUrl} loop muted={false} controls preload="auto"></video>
      )}
    </div>
  );
}
