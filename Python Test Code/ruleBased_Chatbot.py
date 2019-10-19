# Code adapted from the following tutorial
# https://towardsdatascience.com/build-your-first-chatbot-using-python-nltk-5d07b027e727

# This simple chatbot is a rule based bot. It works using pairs of inputs and possible outputs
# If input is equal to one of these pairs then it will output the results.
# (.*) = anything can be input as long as the rest of the string matches.
# E.g For r"(.*) (town|city) ?" you could put in Where is New York city? or where is Castlebar town?
# As long as town? and city? are input with a sentence before then it will be valid.

# If the input doesn't match one of the possibilities then it will return with a default reply,
# so it wouldn't work well for inputs with speech to text. It would however suit if all the user inputs where predetermined (E.g Selectable from a list).

# From this I don't feel it will be very useful in our project as it's very simple but it was a good learning experience to see how it works.

from nltk.chat.util import Chat, reflections

pairs = [
    [
        r"my name is (.*)",
        ["Hello %1, How are you today ?",]
    ],
     [
        r"what is your name ?",
        ["My name is Chatty and I'm a chatbot ?",]
    ],
    [
        r"how are you ?",
        ["I'm doing good\nHow about You ?",]
    ],
    [
        r"sorry (.*)",
        ["Its alright","Its OK, never mind",]
    ],
    [
        r"i'm (.*) doing good",
        ["Nice to hear that","Alright :)",]
    ],
    [
        r"hi|hey|hello",
        ["Hello", "Hey there",]
    ],
    [
        r"(.*) age?",
        ["I'm a computer program dude\nSeriously you are asking me this?",]
        
    ],
    [
        r"what (.*) want ?",
        ["Make me an offer I can't refuse",]
        
    ],
    [
        r"(.*) (town|city) ?",
        ['Mayo, Ireland',]
    ],
    [
        r"how is weather in (.*)?",
        ["Weather in %1 is awesome like always","Too hot man here in %1","Too cold man here in %1","Never even heard about %1"]
    ],
    [
        r"i work in (.*)?",
        ["%1 is an Amazing company, I have heard about it. But they are in huge loss these days.",]
    ],
    [
        r"(.*)raining in (.*)",
        ["No rain since last week here in %2","Damn its raining too much here in %2"]
    ],
    [
        r"(.*) (sports|game) ?",
        ["I'm a very big fan of Football",]
    ],
    [
        r"who (.*) sportsperson ?",
        ["Salah","Ronaldo","Roony"]
    ],
    [
        r"who (.*) (moviestar|actor)?",
        ["Brad Pitt"]
    ],
    [
        r"quit",
        ["Bye take care. See you soon :) ","It was nice talking to you. See you soon :)"]
    ],
]

# Method to run chatbot using NLTK chat and reflections.
def chatty():
    print("Hi, I'm Chatty and I chat alot ;)\nStart a conversation or type quit to exit ") # default message at the start
    chat = Chat(pairs, reflections)
    chat.converse()

if __name__ == "__main__":
    chatty()