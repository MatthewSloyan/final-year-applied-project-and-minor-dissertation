import numpy as np
import tensorflow
import keras as kr
from keras import backend
from keras.models import load_model
import json
import random
import nltk
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

def chat():
    print("Start talking with the bot:")
    while True:
        inp = input("You: ")
        if inp.lower() == "quit":
            break

        results = model.predict([[bag_of_words(inp, words)]])
        results_index = np.argmax(results)
        tag = labels[results_index]

        for tg in data["intents"]:
            if tg['tag'] == tag:
                responses = tg['responses']

        print("Bot: "+random.choice(responses))

chat()