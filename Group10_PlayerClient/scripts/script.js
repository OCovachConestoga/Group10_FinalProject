/* FILE: scripts/script.js */
const apiBase = "https://localhost:7190/api"; // Change port if needed

function getUserId() {
  return localStorage.getItem("userId");
}

function logout() {
  localStorage.removeItem("userId");
  window.location.href = "login.html";
}

async function joinGame(gameId) {
  const userId = getUserId();
  if (!userId) return alert("Please log in first.");

  try {
    const res = await fetch(`${apiBase}/games/${gameId}/joinRequest?userId=${userId}`, {
      method: 'POST'
    });

    if (res.ok) {
      alert("Joined game!");
    } else {
      const error = await res.text();
      alert("Failed to join: " + error);
    }
  } catch (err) {
    console.error("Join Game Error:", err);
    alert("Error connecting to server.");
  }

  location.reload();
}

async function leaveGame(gameId) {
  const userId = getUserId();
  if (!userId) return alert("Please log in first.");

  try {
    const res = await fetch(`${apiBase}/games/${gameId}/leaveRequest?userId=${userId}`, {
      method: 'POST'
    });

    if (res.ok) {
      alert("Left game.");
    } else {
      const error = await res.text();
      alert("Failed to leave: " + error);
    }
  } catch (err) {
    console.error("Leave Game Error:", err);
    alert("Error connecting to server.");
  }

  location.reload();
}