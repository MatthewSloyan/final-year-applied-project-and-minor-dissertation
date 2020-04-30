from flask import Flask, jsonify, request, json
from flask_pymongo import PyMongo
import aiml

# Setup connection to MongoDB database
# https://flask-pymongo.readthedocs.io/en/latest/
app = Flask(__name__)
app.config['MONGO_DBNAME'] = 'final-year-project'
app.config['MONGO_URI'] = 'mongodb://admin:admin1234@ds039457.mlab.com:39457/final-year-project?retryWrites=false'
mongo = PyMongo(app)

# AIML
kernel = aiml.Kernel()
kernel.learn("startup.xml")

@app.route('/')
def index():
    return 'Hello, World!'

@app.route('/request', methods=['POST'])
def predictResponse():
    # Get json from request.
    sessionId = request.get_json()['sessionId']
    persona = request.get_json()['persona']
    userInput = request.get_json()['userInput']
    print(sessionId)
    print(persona)
    print(userInput)

    # Load specific aiml file depending on persona.
    kernel.respond("load aiml " + str(persona))

    print("DATA:")
    print(kernel.getPredicate("usersName", sessionId))

    # Predict reponse for specific session using user input.
    response = kernel.respond(userInput, sessionId)
    print(response)

    return response

@app.route('/api/results', methods=['PUT'])
def uploadResult():
    # Get collection from database.
    results = mongo.db.results

    # Get json from request.
    gameId = request.get_json()['gameId']
    gameScore = request.get_json()['gameScore']
    gameTime = request.get_json()['gameTime']
    npcs = request.get_json()['npcs']

    # Write json object to MongoDB database.
    results.insert_one({
        'gameId': gameId,
        'gameScore': gameScore,
        'gameTime': gameTime,
        'npcs': npcs
    })

    return jsonify(data="Result sucessfully uploaded.")

if __name__ == "__main__":
    app.run(threaded=True, port=5000)