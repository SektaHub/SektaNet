import React, { useState, useEffect } from 'react';
import Reels from '../components/Reels/Reels';
import './HomePage.css';
import hubLogo from '/hub.png';
import { useInView } from 'react-intersection-observer';
import { API_URL } from '../config';

const HomePage: React.FC = () => {
  const queryParams = new URLSearchParams(window.location.search);
  const initialVideoId = queryParams.get('videoId');

  const [reelList, setReelList] = useState<{ id: number; component: JSX.Element }[]>([]);
  const [loading, setLoading] = useState(false);
  const { ref: loaderRef, inView: loaderInView } = useInView();

  const getRandomVideoIdFromBackend = async () => {
    try {
      const response = await fetch(`${API_URL}/Reel/RandomVideoId`);
      const data = await response.json();

      if (response.ok) {
        return data;
      } else {
        console.error('Error fetching random video ID:', data);
        return null;
      }
    } catch (error: any) {
      console.error('Error fetching random video ID:', error.message);
      return null;
    }
  };

  const loadMoreReels = async () => {
    setLoading(true);
    const randomVideoId = await getRandomVideoIdFromBackend();

    if (randomVideoId !== null) {
      setReelList((prevReels) => [
        ...prevReels,
        { id: prevReels.length, component: <Reels key={prevReels.length} videoId={randomVideoId} /> },
      ]);
    }

    setLoading(false);
  };

  useEffect(() => {
    // If initialVideoId is present, use it for the first video
    if (initialVideoId) {
      setReelList([{ id: 0, component: <Reels key={0} videoId={initialVideoId} /> }]);
    } else {
      // Otherwise, fetch a random videoId for the first video
      loadMoreReels();
    }
  }, []); // Empty dependency array ensures this effect runs only once on mount

  useEffect(() => {
    const fetchData = async () => {
      if (loaderInView && !loading) {
        await loadMoreReels();
      }
    };

    fetchData();
  }, [loaderInView, loading]);

  return (
    <center>
      <div className="ad-cont-left">
        <a href="https://mvr.gov.mk/page/telefonski-imenik" target="_blank" rel="noopener noreferrer">
          <img className="ad" src="/ficoNasilstvo.png" alt="" />
        </a>
      </div>
      <div className="ad-cont-right">
        <img className="ad" src="/JaminEbe.png" alt="" />
      </div>
      <div className="reel-cont">
        {reelList.map((reel) => reel.component)}
        <div ref={loaderRef} style={{ height: '1px' }}></div>
      </div>
    </center>
  );
};

export default HomePage;
