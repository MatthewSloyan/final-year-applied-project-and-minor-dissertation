# https://keras.io/getting-started/faq/#how-can-i-obtain-reproducible-results-using-keras-during-development
# From our lecturer I found that it was best to seed the random weight generator to get reproducible results.
# As TensorFlow will automatically assign random weights on each run based on probibility.
# To achieve this I found documentation on the Keras website above, which I have adapted.
import numpy as np
import tensorflow as tf
import random as rn

# The below is necessary for starting Numpy and core Python generated random numbers in a well-defined initial state.
np.random.seed(42)
rn.seed(12345)

# Force TensorFlow to use single thread.
# Multiple threads are a potential source of non-reproducible results.
# For further details, see: https://stackoverflow.com/questions/42022950/
session_conf = tf.ConfigProto(intra_op_parallelism_threads=1, inter_op_parallelism_threads=1)

from keras import backend as K

# The below tf.set_random_seed() will make random number generation
# in the TensorFlow backend have a well-defined initial state.
# For further details, see: https://www.tensorflow.org/api_docs/python/tf/set_random_seed
tf.set_random_seed(1234)

sess = tf.Session(graph=tf.get_default_graph(), config=session_conf)
K.set_session(sess)

# Additional Imports
import keras as kr
from keras.models import load_model # To save and load models
import sklearn.preprocessing as pre # For encoding categorical variables.
import json
import pickle
import nltk
from nltk.stem.lancaster import LancasterStemmer
stemmer = LancasterStemmer()

# ==== IMPLEMENTATION =====
# Code adapted from the following tutorial
# https://techwithtim.net/tutorials/ai-chatbot/part-1/

with open("Sources/intents.json") as file:
    data = json.load(file)

try:
    with open("Sources/data.pickle", "rb") as f:
        words, labels, training, output = pickle.load(f)
except:
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

    words = [stemmer.stem(w.lower()) for w in words if w != "?"]
    words = sorted(list(set(words)))

    labels = sorted(labels)

    training = []
    output = []

    out_empty = [0 for _ in range(len(labels))]

    for x, doc in enumerate(docs_x):
        bag = []

        wrds = [stemmer.stem(w.lower()) for w in doc]

        for w in words:
            if w in wrds:
                bag.append(1)
            else:
                bag.append(0)

        output_row = out_empty[:]
        output_row[labels.index(docs_y[x])] = 1

        training.append(bag)
        output.append(output_row)

    training = np.array(training)
    output = np.array(output)

    with open("Sources/data.pickle", "wb") as f:
        pickle.dump((words, labels, training, output), f)

# Set up a neural network model, building it layer by layer sequentially.
model = kr.models.Sequential()

model.add(kr.layers.Dense(units=8, activation='linear', input_dim=len(training[0])))
model.add(kr.layers.Dense(units=10, activation='relu')) 
model.add(kr.layers.Dense(units=len(output[0]), activation='softmax'))
model.compile(optimizer='rmsprop', loss='mse', metrics=['accuracy'])

try:
    model = load_model('Sources/test_model.h5')
except:
    model.fit(training, output, epochs=100, batch_size=8)
    model.save('Sources/test_model.h5')

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
    print("Start talking with the bot (type quit to stop)!")
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

        print(rn.choice(responses))

chat()