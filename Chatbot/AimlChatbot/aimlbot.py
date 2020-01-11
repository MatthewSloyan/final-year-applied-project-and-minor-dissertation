from flask import Flask, request
import aiml
import os

kernel = aiml.Kernel()
kernel.learn("startup.xml")
kernel.respond("load aiml b")

app = Flask(__name__)

@app.route('/')
def index():
    return "<h1>Welcome!!</h1>"

@app.route('/request', methods=['POST'])
def predictResponse():
    jsonData = request.data

    print(jsonData)

    s = ''

    for i in jsonData:
        s = s + chr(i) 

    s = s.split('=')[1]

    s = s.replace("%20"," ")
    s = s.replace("%3f","")
    print(s)


    response = kernel.respond(s)
    
    print(response)

    return response

if __name__ == "__main__":
    app.run(threaded=True, port=5000)
#host = "192.168.1.7",