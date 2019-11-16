import numpy as np
import tensorflow
import keras as kr
from keras import backend
from keras.models import load_model
import json
import pickle
import nltk

from nltk.stem.lancaster import LancasterStemmer
stemmer = LancasterStemmer()

# List of Json scenarios files names
intentFiles = ["intents_general.json", "intents_luas.json"]

# Reads in Json file, parses it and writes out to binary file.
# This allows for multiple scenarios to be used and trained.
def processIntents(scenarioNum):
    try:
        filePath = "Intents/" + intentFiles[int(scenarioNum)]

        with open(filePath) as file:
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

        with open("Models/scenario_" + scenarioNum + ".pickle", "wb") as f:
            pickle.dump((words, labels),f)

        # Train model using parsed data.
        trainModel(scenarioNum, training, output)
    except:
        print("Error occured, please try again!")

# Train model using data.
def trainModel(scenarioNum, training, output):

    # BUILDING MODEL
    print(training)
    model = kr.models.Sequential()
    model.add(kr.layers.Dense(units=8, activation='linear', input_dim=len(training[0])))
    model.add(kr.layers.Dense(units=8, activation='relu'))
    model.add(kr.layers.Dense(units=len(output[0]), activation='softmax'))
    model.compile(loss='mse', optimizer='adam', metrics=['accuracy'])

    # TRAINING MODEL
    model.fit(training, output, epochs=100, batch_size=8)
    model.save("Models/model_" + scenarioNum + ".h5")

# User input for our own testing and training of models.
userInput = input("Enter scenario number: ") 
processIntents(userInput)
