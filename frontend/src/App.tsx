import { useState, useEffect } from 'react';
import hubLogo from '/hub.png';
import './App.css';
import Reels from "./components/Reels/Reels.jsx";
import { useInView } from 'react-intersection-observer';

function App() {

  const [reelList, setReelList] = useState([{ id: 0, component: <Reels key={0} videoId={getRandomVideoId()} /> }]);
  const [loading, setLoading] = useState(false);
  const { ref: loaderRef, inView: loaderInView } = useInView();

  function getRandomVideoId() {
    return Math.floor(Math.random() * 20 + 1);
  }

  const loadMoreReels = () => {
    setLoading(true);
    setReelList((prevReels) => [
      ...prevReels,
      { id: prevReels.length, component: <Reels key={prevReels.length} videoId={getRandomVideoId()} /> },
    ]);
    setLoading(false);
  };

  useEffect(() => {
    if (loaderInView && !loading) {
      loadMoreReels();
    }
  }, [loaderInView, loading]);

  return (
    <center>
      <div className='logo-cont'>
        <a href="https://www.instagram.com/sektakecdvanaesipol/" target="_blank">
          <img
            src={hubLogo}
            className="logo"
            alt="Hub logo"
          />
        </a>
      </div>
      <div className='ad-cont-left'>
        <a href='https://mvr.gov.mk/page/telefonski-imenik' target='_blank'>
          <img className='ad' src="../public/ficoNasilstvo.png" alt="" />
        </a>
      </div>
      <div className='ad-cont-right'>
        <img className='ad' src="../public/JaminEbe.png" alt="" />
      </div>
      <p className='reel'>Reels</p>
      <div className='reel-cont'>
        {reelList.map(reel => reel.component)}
        <div ref={loaderRef} style={{ height: '1px' }}></div>
      </div>
    </center>
  );
}

export default App;
