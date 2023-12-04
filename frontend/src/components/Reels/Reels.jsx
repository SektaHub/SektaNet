import React, { useRef, useEffect } from 'react';
import { useInView } from 'react-intersection-observer';
import "./Reels.css";

export default function Reels({ videoId }) { // Assuming you have a unique identifier for each video
  const vidRef = useRef(null);
  const [ref, inView] = useInView({
    threshold: 0.5,
  });

  // Build the video URL dynamically based on the videoId
  const videoUrl = `https://localhost:7294/api/Reel/${videoId}`;

  useEffect(() => {
    if (inView) {
      vidRef.current?.play();
    } else {
      vidRef.current?.pause();
    }
  }, [inView]);

  return (
    <div ref={ref} className='reel-card'>
      <video
        className="reel-player"
        ref={vidRef}
        src={videoUrl}
        loop
        muted={false}
        controls
        preload="metadata"
      ></video>
    </div>
  );
}