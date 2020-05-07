# Final-Year-Applied-Project-And-Minor-Dissertation

**Group members:** Aaron Hannon (G00347352) & Matthew Sloyan (G00348036)

## Demo video and presentation
Below is our demo and presentation video available on YouTube. Please click on this video image to view the video. This demo video can also be found in the "Demo Video & Presentation" folder on this repository.

[![Watch the demo and presentation video here](https://img.youtube.com/vi/cNwglqfNSNw/maxresdefault.jpg)](https://www.youtube.com/watch?v=cNwglqfNSNw)

## Project overview
This application is a virtual reality training simulation for our client. The function of the application is to reduce training costs while training ticket inspectors on the Luas in Dublin. Currently they must hire actors and close off a Luas route to train new inspectors and this project helps eliminate that. Once you launch the application on the virtual reality headset you are in a virtual train station. As soon as you hop on a train it disembarks then commencing the training session. The goal is to check everyone's ticket on board the train by conversing with all the non-player characters (NPCs) using your actual voice and they will reply to you using a text-to-speech engine (Azure). All the NPCs have different personalities so this is where the conflict resolution aspect of the project comes in. You may come across someone who may be very rude and you must coerce them into giving you their ticket or you may be fortunate to talk to someone who gives you their ticket straight away. The NPCs also have the chance to have no ticket, which is determined at random. If the NPC has no ticket the player must deal with the situation accordingly. The interactions work using an AIML chatbot that is hosted on PythonAnywhere and is connected using a flask server. Requests are made on each interaction to this chatbot and a response is predicted based on the diction we have created. Once all the NPCs are checked you may leave the train, check your score, and end the simulation. Training information such as conversation logs and session data are then uploaded to MongoDB once completed. After the purchase of a virtual reality headset there is very little cost involve and the training simulation can be replayed over and over again completely removing the need to hire actors and shutdown a Luas route for an entire day.  

## Main technologies used
* Unity - Main engine of the project.
* Virtual Reality (VR) - Support for Oculus Quest.
* Azure Speech Services - Speech to Text and Text to Speech used for NPC interactions.
* Flask Server - Handles chatbot and database requests.
* AIML Chatbot - Chatbot brain.
* Keras Neural Network - Chatbot brain (Old version).
* PythonAnywhere - Hosts our flask server.
* MongoDB Database - Stores training data.

More information about the researched technologies and how they work together can be found in the "Disseration" folder.

## GitHub repository overview
* Chatbot & Server: This folder contains the python flask server along with the AIML files. This handles all AIML and MongoDB database requests. There is also a test Keras chatbot that was used during research. 
* Demo Video & Presentation: This folder contains the slide material along with a video presentation and full demo.
* Dissertation: This folder contains all disseration material and final PDF file.
* Research: This folder contains all the research that was performed prier to development. All researched technologies can be found in the "Technology Review" chapter of the Dissertation.
* UnityEngine: This folder contains the entire Unity side of the project. This includes all assets scripts etc.

## How to run on Oculus Quest VR headset
1. Download or clone the GitHub repository using `git clone https://github.com/MatthewSloyan/final-year-applied-project-and-minor-dissertation.git`.
2. Add Unity 2019.2.6f1 to Unity Hub. 
3. Navigate to the "UnityEngine" and add the FinalYearProject folder to your Unity projects in Unity Hub.
4. Launch the project.
5. Once the project is open connect your Oculus Quest to your PC using the cable provided.
6. Navigate to "File" then "Build Settings".
7. Highlight Android under platforms and click "Switch Platform", this may take a few minutes.
8. With your Oculus Quest connected, click "Build and Run".
9. The application will then be built and will run on the Oculus Quest.
