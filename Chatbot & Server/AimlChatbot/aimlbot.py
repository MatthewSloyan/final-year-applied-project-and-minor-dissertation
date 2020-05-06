from flask import Flask, jsonify, request, json
from flask_pymongo import PyMongo
import aiml
import os, requests, time
import re
from xml.etree import ElementTree

# import os
# import json
# import time

# Setup connection to MongoDB database
# https://flask-pymongo.readthedocs.io/en/latest/
app = Flask(__name__)
#app.secret_key = os.urandom(24)
#app.config['MONGO_DBNAME'] = 'final-year-project'
#app.config['MONGO_URI'] = 'mongodb://admin:admin1234@ds039457.mlab.com:39457/final-year-project?retryWrites=false'
#app.config['MONGO_CONNECT'] = False
#app.config['JWT_SECRET_KEY'] = 'secret'
mongo = PyMongo(app, uri="mongodb://admin:admin1234@ds039457.mlab.com:39457/final-year-project?retryWrites=false")

# AIML
kernel = aiml.Kernel()
kernel.learn("/home/aaronchannon1/mysite/startup.xml")

@app.route('/')
def index():
    return "<h1>Welcome!!</h1>"

@app.route('/request', methods=['PUT'])
def predictResponse():
     # Get json from request.
    sessionId = request.get_json()['sessionId']
    persona = request.get_json()['persona']
    userInput = request.get_json()['userInput']
    hasTicket = request.get_json()['hasTicket']
    print(sessionId)
    print(persona)
    print(userInput)
    print(hasTicket)
    print(type(hasTicket))
    # Load specific aiml file depending on persona.


    if hasTicket == True:
        kernel.respond("load aiml " + str(persona))
        st= "has ticket"
    else:
        kernel.respond("load aiml " + str(persona)+ " NO TICKET")
        st= "does not have ticket"
    print (st)


    print("DATA:")
    print(kernel.getPredicate("usersName", sessionId))

    #result = re.sub('[^a-zA-Z]', '', userInput)
    result = re.sub(r'([^\s\w]|_)+', '', userInput)

    print("Result:")
    print(result)

    # Predict reponse for specific session using user input.
    response = kernel.respond(result, sessionId)
    print(response)

    return response

@app.route('/api/results', methods=['PUT'])
def uploadResult():
    # Get json from request.
    gameId = request.get_json()['gameId']
    gameScore = request.get_json()['gameScore']
    gameTime = request.get_json()['gameTime']
    npcs = request.get_json()['npcs']

    print(gameId)
    print(gameScore)
    print(npcs)

    # Get collection from database.
    result = mongo.db.test_results

    test = result.find_one({
        '_id': "5e6d381cd9a63706287fc7e1"
    })

    testtwo = {'email': test['gameId'] + ' success'}
    print(testtwo)

    # Write json object to MongoDB database.
    result.insert({
        'gameId': gameId,
        'gameScore': gameScore,
        'gameTime': gameTime,
        'npcs': npcs
    })

    return jsonify(data="Result sucessfully uploaded.")

if __name__ == "__main__":
    app.run(debug=True)
#host = "192.168.1.7",