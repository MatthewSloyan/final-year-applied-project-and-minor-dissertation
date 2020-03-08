from flask import Flask, jsonify, request, json
from flask_pymongo import PyMongo
from flask_cors import CORS
import aiml
app = Flask(__name__)

# MONGODB
app.config['MONGO_DBNAME'] = 'final-year-project'
app.config['MONGO_URI'] = 'mongodb://admin:admin1234@ds039457.mlab.com:39457/final-year-project?retryWrites=false'

mongo = PyMongo(app)
CORS(app)

# AIML
kernel = aiml.Kernel()
kernel.learn("/home/aaronchannon1/mysite/startup.xml")

app = Flask(__name__)

@app.route('/')
def index():
    return "<h1>Welcome!!</h1>"

@app.route('/request', methods=['POST'])
def predictResponse():
    jsonData = request.data

    #sessionData = kernel.getSessionData(sessionId)

    print(jsonData)
    #data = json.loads(jsonData)
    s = ''

    for i in jsonData:
        s = s + chr(i)

    sentenceString = s.split('&')[0]
    sent = sentenceString.split('=')[1]

    sessionString = s.split('&')[1]
    sessionId = sessionString.split('=')[1]

    personaString = s.split('&')[2]
    persona = personaString.split('=')[1]

    kernel.respond("load aiml "+persona)

    print("DATA:")
    print(kernel.getPredicate("usersName", sessionId))

    print(sent)
    print(sessionId)
    print(persona)

    sent = sent.replace("%20"," ")
    sent = sent.replace("%3f","")

    #response = kernel.respond(sent,sessionId)
    response = kernel.respond(sent,sessionId)

    print(response)

    return response

@app.route('/api/results', methods=['POST'])
def uploadResult():
    # Get json from request and collection mongo.
    jsonData = request.data
    results = mongo.db.results

    print(jsonData)

    return jsonify(data="Result sucessfully uploaded.")

if __name__ == "__main__":
    app.run(threaded=True, port=5000)
#host = "192.168.1.7",