import React, { useState, useEffect } from 'react';
import wordList from 'word-list-json';
import './game.css';

function Game() {
  const [inputWord, setInputWord] = useState("");
  const [currentWord, setCurrentWord] = useState("");
  const [usedWords, setUsedWords] = useState([]);
  const [message, setMessage] = useState("");
  const [timeLeft, setTimeLeft] = useState(15);
  const [gameOver, setGameOver] = useState(false);

  const [totalLetters, setTotalLetters] = useState(0);
  const [elapsedTime, setElapsedTime] = useState(0); 
  const [longestWord, setLongestWord] = useState("");
  const [startTime, setStartTime] = useState(null);
  const [isNewHighScore, setIsNewHighScore] = useState(false);

  useEffect(() => {
    let timer;
    if (!gameOver && currentWord) {
      if (!startTime) {
        setStartTime(Date.now()); 
      }
      timer = setInterval(() => {
        setTimeLeft((prev) => {
          if (prev === 1) {
            clearInterval(timer);
            setGameOver(true);
          }
          return prev - 1;
        });

        setElapsedTime((prevTime) => prevTime + 1);
      }, 1000);
    } else {
      clearInterval(timer);
    }
    return () => clearInterval(timer);
  }, [currentWord, gameOver]);

  useEffect(() => {
    if (gameOver) {
      const bestScore = JSON.parse(localStorage.getItem("bestScore")) || 0;
      if (totalLetters > bestScore) {
        localStorage.setItem("bestScore", totalLetters);
        setIsNewHighScore(true);
      }
    }
  }, [gameOver]);

  const handleInputChange = (e) => {
    setInputWord(e.target.value.toLowerCase());
  };

  const handleSubmit = () => {
    const trimmedWord = inputWord.trim().toLowerCase();
    if (trimmedWord === "") return;

    let newMessage = "";
    let newMessageClass = "";

    if (!wordList.includes(trimmedWord)) {
      newMessage = `❌ '${trimmedWord.toUpperCase()}' is not a valid word.`;
      newMessageClass = "incorrect-message";
    } else if (usedWords.includes(trimmedWord)) {
      newMessage = "⚠ This word has already been used!";
      newMessageClass = "used-message";
    } else if (currentWord && trimmedWord[0] !== currentWord.slice(-1)) {
      newMessage = `❌ Must start with '${currentWord.slice(-1).toUpperCase()}'!`;
      newMessageClass = "incorrect-message";
    } else {
      newMessage = "✅ Correct!";
      newMessageClass = "correct-message";

      setUsedWords([...usedWords, trimmedWord]);
      setCurrentWord(trimmedWord);

      if (trimmedWord.length > longestWord.length) {
        setLongestWord(trimmedWord);
      }

      setTotalLetters((prev) => prev + trimmedWord.length);
      setTimeLeft(15);
    }

    setMessage(newMessage);
    setInputWord("");
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      handleSubmit();
    }
  };

  const handleRestart = () => {
    setInputWord("");
    setCurrentWord("");
    setUsedWords([]);
    setMessage("");
    setTimeLeft(15);
    setGameOver(false);
    setTotalLetters(0);
    setElapsedTime(0);
    setLongestWord("");
    setStartTime(Date.now());
    setIsNewHighScore(false);
  };

  return (
    <div className="game">
      <h1>Word Chain Game</h1>

      {gameOver ? (
        <div className="score-summary">
          <h2>{isNewHighScore ? " CONGRATULATIONS! NEW HIGH SCORE" : "GAME OVER"}</h2>
          <div className="game-over-gif">
            {isNewHighScore ? (
              <img 
                src="/gif/highscore.gif" 
                alt="Congratulations" 
                className="game-over-image" 
              />
            ) : (
              <img 
                src="/gif/game-over-game.gif" 
                alt="Game Over" 
                className="game-over-image" 
              />
            )}
          </div>
          <div className="summary-grid">
            <div className="summary-card">
              <p>Word count</p>
              <strong>{usedWords.length}</strong>
            </div>
            <div className="summary-card">
              <p>Score</p>
              <strong>{totalLetters}</strong>
            </div>
            <div className="summary-card">
              <p>Total time</p>
              <strong>{(elapsedTime).toFixed(2)}s</strong>
            </div>
            <div className="summary-card">
              <p>Your Longest word</p>
              <strong>{longestWord}</strong>
            </div>
            <div className="summary-card">
              <p>Best Score</p>
              <strong>{Math.max(totalLetters, JSON.parse(localStorage.getItem("bestScore")) || 0)}</strong>
            </div>
          </div>
          <button onClick={handleRestart}>Restart</button>
        </div>
      ) : (
        <>
          <div>
            <div className="score-board">
              <div className="score-item">
                <p>Word Count</p>
                <strong>{usedWords.length}</strong>
              </div>
              <div className="score-item">
                <p>Time Left</p>
                <strong>{timeLeft}</strong>
              </div>
              <div className="score-item">
                <p>Score</p>
                <strong>{totalLetters}</strong>
              </div>
            </div>

            <h2>Current Word: {currentWord || "—"}</h2>
            {currentWord && (
              <p className="next-letter-message">
                Next word must start with <strong>'{currentWord.slice(-1).toUpperCase()}'</strong>
              </p>
            )}

            <input
              type="text"
              value={inputWord}
              onChange={handleInputChange}
              onKeyDown={handleKeyPress}
              autoFocus
            />

            <p className={ 
              message.startsWith("✅") ? "message correct-message" :
              message.startsWith("⚠") ? "message used-message" :
              message.startsWith("❌") ? "message incorrect-message" :
              "message"
            }>
              {message}
            </p>
          </div>

          <div className="word-list">
            <ul>
              {usedWords.map((word, index) => (
                <li key={index}>{word}</li>
              ))}
            </ul>
          </div>
        </>
      )}
    </div>
  );
}

export default Game;
