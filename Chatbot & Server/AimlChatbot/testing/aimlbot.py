from flask import Flask, jsonify, request, json
from flask_pymongo import PyMongo
import aiml
import re
import env

# == Important ==
# This version of the server is a copy of the main aimlbot.py but merely used for testing purposes using pytest.
# All data is written to a separate test database.

# Setup connection to MongoDB database. Information is stored securily in environment file.
# https://flask-pymongo.readthedocs.io/en/latest/
app = Flask(__name__)
mongo = PyMongo(app, uri="mongodb://" + env.USER + ":" +env.PASSWORD + "@ds039457.mlab.com:39457/" + env.DB + "?retryWrites=false")

# AIML
kernel = aiml.Kernel()
kernel.learn("startup.xml")

@app.route('/')
def index():
    return 'Hello, World!'

@app.route('/request', methods=['PUT'])
def predictResponse():
    # Get json from request.
    sessionId = request.get_json()['sessionId']
    persona = request.get_json()['persona']
    userInput = request.get_json()['userInput']
    hasTicket = request.get_json()['hasTicket']

    # Load specific aiml file depending on persona.
    if hasTicket == True:
        kernel.respond("load aiml " + str(persona))
    else:
        kernel.respond("load aiml " + str(persona)+ " NO TICKET")

    # Remove all extra characters from string.
    result = re.sub(r'([^\s\w]|_)+', '', userInput)

    # Predict reponse for specific session using user input.
    response = kernel.respond(result, sessionId)

    return response

@app.route('/api/results', methods=['PUT'])
def uploadResult():
    # Get collection from database.
    results = mongo.db.test_results

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