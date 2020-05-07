# Final-Year-Applied-Project-And-Minor-Dissertation

**Group members:** Aaron Hannon (G00347352) & Matthew Sloyan (G00348036)

## Demo video and presentation
Below is our demo video and presentation uploaded to YouTube.
[![Watch the demo and presentation video here](https://img.youtube.com/vi/cNwglqfNSNw/maxresdefault.jpg)](https://www.youtube.com/watch?v=cNwglqfNSNw)

This demo video can also be found in the "Demo Video & Presentation" folder on this repository.

## Project overview
This application is a virtual reality training simulation for our client. The function of the application is to reduce training costs while training ticket inspectors on the Luas in Dublin. Currently they have to hire actors and close off a Luas route to train new inspectors and this project helps eliminate that. Once you launch the application on the virtual reality headset you are in a virtual train station. As soon as you hop on a train it disembarks then commencing the training session. The goal is to check everyone's ticket on board the train by conversing with all the non-player characters (NPCs) using your actual voice and they will reply to you using a text-to-speech engine. All the NPCs have different personalities so this is where the conflict resolution aspect of the project comes in. You may come across someone who may be very rude and you must coerce them into giving you their ticket or you may be fortunate to talk to someone who gives you their ticket straight away. Once all the NPCs are checked you may leave the train, check your score and end the simulation. After the purchase of a virtual reality headset there is very little cost involve and the training simulation can be replayed over and over again completely removing the need to hire actors and shutdown a Luas route for an entire day.  

## Main technologies used
* Unity - Main engine of the project.
* VR support for Oculus Quest.
* Flask Server - Handles chatbot and database requests.
* AIML Chatbot - Chatbot brain.
* Keras Neural Network - Chatbot brain (Old version).
* PythonAnywhere: Hosts our flask server.
* MongoDB Database - Stores training data.

More information about the researched technologies and how they work together can be found in the Disseration folder.

## GitHub repository overview
* Chatbot: This folder contains the python flask server along with the AIML files. This handles all AIML and MongoDB database requests. There is also a test Keras chatbot that was used during research. 
* Dissertation: This folder contains all disseration material.
* Presentation: This folder contains the slide material along with a video presentation and full demo.
* Research: This folder contains all the research that was performed prier to development.
* UnityEngine: This folder contains the entire Unity side of the project. This includes all assets scripts etc.

## How to run
1. Download or clone the GitHub repo
2. Add Unity 2019.2.6f1 to your Unity Hub. 
3. Navigate to the "UnityEngine" and add the FinalYearProject folder to your Unity projects in Unity Hub.
4. Launch the project.
5. Once the project is open connect your Oculus Quest to your PC.
6. Navigate to "File" then "Build Settings".
7. Highlight Android under platforms and click "Switch Platform".
8. With your Oculus Quest connected, click "Build and Run".
9. The application will then be build and run on the Oculus.
