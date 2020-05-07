import tests

# MongoDB test runners.
def test_run_update_old_result():
    test_data = {
        "_id": {
            "$oid": "5e8f79608b5f8f7375cfd675"
        },
        "gameId": 9279,
        "gameScore": 0,
        "gameTime": "Time:\n0 Mins 5 Sec",
        "npcs": [
            {
                "sessionId": 6591,
                "complete": True,
                "score": 1,
                "voiceName": "de-DE-KatjaNeural",
                "conversations": [
                    "Do you have a ticket?",
                    "Here you go!"
                ]
            }
        ]
    }

    tests.test_uploadResult(test_data)

def test_run_upload_new_result():
    test_data = {
        "gameId": 9279,
        "gameScore": 0,
        "gameTime": "Time:\n0 Mins 5 Sec",
        "npcs": [
            {
                "sessionId": 6591,
                "complete": True,
                "score": 1,
                "voiceName": "de-DE-KatjaNeural",
                "conversations": [
                    "Do you have a ticket?",
                    "Here you go!"
                ]
            }
        ]
    }

    tests.test_uploadResult(test_data)

# Generic home page test.
def test_home_page():
    tests.test_hello()

# AIML test runners.
def test_aiml_positive_ticket():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'HELLO', 'hasTicket': True}
    tests.test_predictResponse(test_data, "Hello Pleased to meet you!")

def test_aiml_positive_no_ticket():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'WHAT IS THE TIME', 'hasTicket': False}
    tests.test_predictResponse(test_data, "I'm very sorry, I don't have the time.")

def test_aiml_rude_ticket():
    test_data = {'sessionId': 1234, 'persona': 0, 'userInput': 'HELLO', 'hasTicket': True}
    tests.test_predictResponse(test_data, "What do you want? Can't you see I am busy?")

def test_aiml_rude_no_ticket():
    test_data = {'sessionId': 1234, 'persona': 0, 'userInput': 'HOW ARE YOU', 'hasTicket': False}
    tests.test_predictResponse(test_data, "I'd be better if you left me alone")

def test_aiml_neutral_ticket():
    test_data = {'sessionId': 1234, 'persona': 1, 'userInput': 'HOW ARE YOU', 'hasTicket': True}
    tests.test_predictResponse(test_data, "I'm okay")

def test_aiml_neutral_no_ticket():
    test_data = {'sessionId': 1234, 'persona': 1, 'userInput': 'YOU NEED A TICKET', 'hasTicket': False}
    tests.test_predictResponse(test_data, "But I don't have one.")