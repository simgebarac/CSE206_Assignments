
import React from 'react';
import { useNavigate } from 'react-router-dom';

function Home() {
  const navigate = useNavigate();
  return (
    <div className="home-container">
      <h1>Word Chain Game</h1>
      <div className="home-gif">
        <img
          src="/gif/start.gif"
          alt=""
          className="start-gif"
        />
      </div>
      <button onClick={() => navigate('/game')}>Start Game</button>
    </div>
  );
}

export default Home;