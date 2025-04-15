import requests
import urllib3

# Disable SSL warning
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
# Set up session to store cookies
session = requests.Session()

# Defining the URLs that will be reuired for the testing.
LOGIN_URL = "https://localhost:7190/api/auth/login"
GAME_POST_URL = "https://localhost:7190/api/games/NewPost"
GET_GAMES_URL = "https://localhost:7190/api/games"

# Match the controller's expected input model and make sure this email and password already exist in the Database!!!
login_payload = {
    "email": "henry21@gmail.com",
    "password": "Henry21",
    "userName": "henry21@gmail.com"
}

def test_post_game():
    print("\n Testing POST /api/games/NewPost")

    # Create a sample data for a game (with the same session) to add to the list of available games
    game = {
        "location": "Waterloo, Ontario",
        "date": "2025-04-17T15:00:00",
        "maxPlayers": 4,
        "organizer": "Testing if It Works",
        "isFull": False
    }
    # Here it gets sent as a POST and creates a new game in the db.
    response = session.post(GAME_POST_URL, json=game, verify=False)

    print(f"POST Status: {response.status_code}")
    try:
        result = response.json()
        print("Created Game:", result)
        return result["gameId"]
    except:
        print("Raw Response:", response.text)
        return None

def test_get_games():
    print("\n Testing GET /api/games")

    # Sends a GET response to get the games in the database.
    response = session.get(GET_GAMES_URL, verify=False)
    print(f"Status Code: {response.status_code}")
    try:
        games = response.json()
        for g in games:
            print(f"- Game #{g['gameId']} at {g['location']} on {g['date']}")
        return games
    except:
        print("Raw Response:", response.text)
        return []

def test_edit_game(game_id):
    print(f"\n Testing PUT /api/games/{game_id}")
    # Create the data that needs to get sent for editing the game.
    updated_game = {
        "gameId": game_id,
        "location": "China",
        "date": "2025-04-18T16:30:00",
        "maxPlayers": 2,
        "organizer": "LC Sign Tony",
        "isFull": False
    }
    # Sends a PUT response to edit the game information.
    response = session.put(f"https://localhost:7190/api/games/{game_id}", json=updated_game, verify=False)
    print(f"Status Code: {response.status_code}")
    print("Response:", response.text)

# Main Code for the testing script.

# Logging in using a raw User model with the help of sessions.
login_response = session.post(LOGIN_URL, json=login_payload, verify=False, allow_redirects=False)

# Printstatements for Debugging.
print("Login Status:", login_response.status_code)
print("Set-Cookie Header:", login_response.headers.get("Set-Cookie"))
print("Session Cookies:", session.cookies.get_dict())

if login_response.status_code == 200:

    # Create a new game
    new_game_id = test_post_game()
    
    # Get the list of games
    all_games = test_get_games()
    
   
    # Updating the first game that is available in the list.
    if all_games:
        specific_game_id = all_games[0]['gameId']
        print(f"\nUpdating game with id: {specific_game_id}")
        test_edit_game(specific_game_id)
    else:
        print("No games available to update")
else:
    print("Login failed")
    print(login_response.text)