SpaceZ

Summary
This will start the backend server to serve the data from the database.
SpaceZ industries is in the business of launching rockets and satellites in space for their customers. Every SpaceZ space-craft launch has two parts a launch vehicle or the "Rocket" and the payload which could be a satellite. SpaceZ has a Deep Space Network (DSN) facility containing a Mission-Control system and communication system from which they launch and communicate with their spacecraft. 

SpaceZ wants you to design a software system to run their operations. This software system can be classified as follows:

1) DSN Software Component Features:
	1. Able to show dashboard for
		1. All current active spacecrafts.
		2. All spacecrafts waiting to be launched.
	2. Able to select a specific active spacecraft and look at its data.
	3. Able to send command to a specific spacecraft.
	4. Able to launch a new spacecraft.
	
2) Launch-Vehicle Software Component Features:
	1. Able to receive and process commands from DSN.	
	2. Able to send real-time telemetry of itself back to DSN.
	
3) Payload/Satellite Software Component Features:
	1. Able to receive and process commands from DSN.	
	2. Able to send real-time telemetry of itself back to DSN.
	3. Able to send its Data back to DSN.


Technology 
This project was created with the C sharp for backend and XAML for the frontend and SQL Server was used for the database.

Installation Guide

1. Clone this project into a directory on your system using any of the 2 options provided below:
2. via SSH: > git clone git@github.com:dhruv1462/SpaceZ.git
3. via HTTPS: > git clone https://github.com/dhruv1462/SpaceZ.git
4. Open the solution file in the Visual studio. 
5. This will load all your files in project.
6. Change the connection string for SQL server and SQL database and also change the path of the process in the CommunicationSystem.cs 
7. Now clean the project and build it.
8. Run the Project
