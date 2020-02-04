from flask import Flask, request
import aiml
# import os
# import json
# import time

kernel = aiml.Kernel()
kernel.learn("/home/aaronchannon1/mysite/startup.xml")
kernel.respond("load aiml b")

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

if __name__ == "__main__":
    app.run(threaded=True, port=5000)
#host = "192.168.1.7",