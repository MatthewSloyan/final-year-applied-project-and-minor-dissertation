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
def test_aiml_polite():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'HELLO'}
    tests.test_predictResponse(test_data, "Hello Pleased to meet you!")

def test_aiml_neutral():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'DO YOU HAVE A TICKET'}
    tests.test_predictResponse(test_data, "Here it is=1")

def test_aiml_rude():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'TICKET PLEASE'}
    tests.test_predictResponse(test_data, "Here you go!=2")

def test_aiml_end():
    test_data = {'sessionId': 1234, 'persona': 2, 'userInput': 'THANK YOU'}
    tests.test_predictResponse(test_data, "You're Welcome!=2")