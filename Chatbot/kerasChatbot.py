from flask import Flask, json, jsonify, render_template, request
import numpy as np
import tensorflow
import socket
import os
import sys
import keras as kr
from keras import backend
from keras.models import load_model
import json
import random
import nltk

import win32com.client as wincl
#from gtts import gTTS
import os
#import pyttsx3

from nltk.stem.lancaster import LancasterStemmer
stemmer = LancasterStemmer()



with open("intents.json") as file:
    data = json.load(file)

#PRE-PROCESSING
words = []
labels = []
docs_x = []
docs_y = []

for intent in data["intents"]:
    for pattern in intent["patterns"]:
        wrds = nltk.word_tokenize(pattern)
        words.extend(wrds)
        docs_x.append(wrds)
        docs_y.append(intent["tag"])

    if intent["tag"] not in labels:
        labels.append(intent["tag"])

words = [stemmer.stem(w.lower()) for w in words if w not in "?"]
words = sorted(list(set(words)))

labels = sorted(labels)

training = []
output = []
out_empty = [0 for _ in range(len(labels))]

for x, doc in enumerate(docs_x):
    bag = []

    wrds = [stemmer.stem(w) for w in doc]
    for w in words:
        if w in wrds:
            bag.append(1)
        else:
            bag.append(0)

    output_row =  out_empty[:]
    output_row[labels.index(docs_y[x])] = 1

    training.append(bag)
    output.append(output_row)

training = np.array(training)
output = np.array(output)

# BUILDING MODEL
model = kr.models.Sequential()
model.add(kr.layers.Dense(units=8, activation='linear', input_dim=len(training[0])))
model.add(kr.layers.Dense(units=8, activation='relu'))
model.add(kr.layers.Dense(units=len(output[0]), activation='softmax'))
model.compile(loss='mse', optimizer='adam', metrics=['accuracy'])

# TRAINING MODEL
try:
    print("LOADING.....")
    model = load_model('model.h5')
except:
    print("LOADING FAILED....")
    model.fit(training, output, epochs=1000, batch_size=8)
    model.save('model.h5')


def bag_of_words(s, words):
    bag = [0 for _ in range(len(words))]

    s_words = nltk.word_tokenize(s)
    s_words = [stemmer.stem(word.lower()) for word in s_words]

    for se in s_words:
        for i, w in enumerate(words):
            if w == se:
                bag[i] = 1
            
    return np.array(bag)

def chat(sentence):

    results = model.predict([[bag_of_words(sentence, words)]])
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
def getImage():
    jsonData = request.data

    # get the base64 binary out of the json data so it can be decoded.
    # base64 data follows this format "data:image/png;base64,iVBORw0KGgoA..."
    # so strip out from base64, onwards.

    s = ''

    for i in jsonData:
        s = s + chr(i) 

    s = s.split('=')[1]

    s = s.replace("%20"," ")
    print(s)

    response = chat(str(s))
    
    # WORKS
    # speak = wincl.Dispatch("SAPI.SpVoice")
    # speak.Speak(response)

    # WORKS
    # tts = gTTS(text=response, lang='en')
    # tts.save("good.mp3")
    
    # DOESNT WORK    
    # engine = pyttsx3.init("dummy")
    # engine.say('Sally sells seashells by the seashore.')
    # engine.say('The quick brown fox jumped over the lazy dog.')
    # engine.runAndWait()
    
    return response

if __name__ == "__main__":
    app.run(debug = False, threaded = False)
