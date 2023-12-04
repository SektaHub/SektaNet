import React,{useRef, useEffect} from 'react'
import { useInView } from 'react-intersection-observer';
import reel1 from '../../../public/saat.mp4';
import reel2 from '../../../public/tristlav.mp4';
import reel3 from '../../../public/KimJong.mp4';
import reel4 from '../../../public/tateDum.mp4';
import reel5 from '../../../public/tateJail.mp4';
import reel6 from '../../../public/tateWorkout.mp4';
import reel7 from '../../../public/TateDepre.mp4';
import "./Reels.css"; 

const reels = [
  reel1, reel2, reel3, reel4, reel5, reel6, reel7
];

export default function Reels() {
  const vidRef = useRef(null);
  const [ref, inView] = useInView({
    threshold: 0.5,
  });

  useEffect(() => {
    if (inView) {
      vidRef.current?.play();
    } else {
      vidRef.current?.pause();
    }
  }, [inView]);

  const getRandomReel = () => {
      const randomIndex = Math.floor(Math.random() * reels.length);
      return reels[randomIndex];
  };

  const randomReel = getRandomReel();

  return (
    <div ref={ref} className='reel-card'>
        <video className="reel-player" ref={vidRef} src={randomReel} loop muted={false} controls></video>
    </div>
  ); 
}
