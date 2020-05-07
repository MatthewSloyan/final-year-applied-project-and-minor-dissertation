# This is an old version of our chatbot, developed intiailly using a Keras machine learning neural network.
# This script is the flask server that would host our chatbot and make predictions on the trained neural network.
# We later decided to develop our chatbot using AIML which can be seen in a separate folder.
# However this implementation worked very well and was a great learning experience.

from flask import Flask, json, jsonify, render_template, request
import numpy as np
import pickle
import random
import tensorflow
import keras as kr
from keras.models import load_model
model_general = load_model('Models/model_0.h5')
model_luas = load_model('Models/model_1.h5')

import nltk
from nltk.stem.lancaster import LancasterStemmer
stemmer = LancasterStemmer()

# List of Json scenarios files names
intentFiles = ["intents_general.json", "intents_luas.json"]

def bag_of_words(s, words):
    bag = [0 for _ in range(len(words))]

    s_words = nltk.word_tokenize(s)
    s_words = [stemmer.stem(word.lower()) for word in s_words]

    for se in s_words:
        for i, w in enumerate(words):
            if w == se:
                bag[i] = 1
            
    return np.array(bag)

def chat(sentence, scenarioNum):
    with open("Intents/" + intentFiles[int(scenarioNum)]) as file:
        data = json.load(file)
    
    with open("Models/scenario_" + str(scenarioNum) + ".pickle","rb") as f:
        words, labels = pickle.load(f)

    if scenarioNum == 0:
        results = model_general.predict([[bag_of_words(sentence, words)]])
    else:
        results = model_luas.predict([[bag_of_words(sentence, words)]])

    results_index = np.argmax(results)
    tag = labels[results_index]

    for tg in data["intents"]:
        if tg['tag'] == tag:
            responses = tg['responses']

    return random.choice(responses)

#send string to server...
#server gets string and predicts through chat
#predict writes to file and file returned

app = Flask(__name__)

@app.route('/', methods=['POST'])
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

    response = chat(str(s), 0)
    
    return response

if __name__ == "__main__":
    app.run(debug = False, threaded = False)