from aimlbot import app
from flask import json

# == HELPER METHODS == 
# Tests run from test_runner.py

def test_hello():
    response = app.test_client().get('/')

    assert response.status_code == 200
    assert response.data == b'Hello, World!'

# Method that tests AIML response, can be run with any input from test_runner.
def test_predictResponse(test_data, bot_response):        
    response = app.test_client().post(
        '/request',
        data=json.dumps(test_data),
        content_type='application/json',
    )

    data = response.get_data()

    assert response.status_code == 200
    assert data == bot_response

# Method that tests MongoDB calls, can be run with any input from test_runner.
def test_uploadResult(test_data):

    response = app.test_client().put(
        '/api/results',
        data=json.dumps(test_data),
        content_type='application/json',
    )

    data = json.loads(response.get_data(as_text=True))

    assert response.status_code == 200
    assert data['data'] == "Result sucessfully uploaded."


