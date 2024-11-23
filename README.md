# Face Recognization
This project implements a basic Face Detection and Recognition system using the Emgu.CV library, which is a .NET wrapper for OpenCV. It allows users to capture images from a webcam, save face images with corresponding labels (names), and recognize faces from previously saved images in a simple application.

Key Features:
Face Detection: Uses a Haar Cascade classifier to detect faces in real-time using a webcam feed.
Face Recognition: Trains a face recognition model based on saved images and labels, recognizing faces from the webcam feed.
Face Storage: Allows users to save faces along with labels (names) into a folder and a corresponding text file.
Persistent Database: Saves images and their labels to disk so that faces can be recognized in future sessions.
GUI Integration: Built with Windows Forms, featuring simple controls for capturing and saving faces.
Libraries Used:
Emgu.CV: A .NET wrapper for OpenCV, used for image processing and computer vision tasks.
Windows Forms: For creating the GUI, with components for displaying the webcam feed and controlling the application.

How it Works:
Face Detection: The application uses Haar Cascade Classifiers to detect faces in each webcam frame.
Face Storage: When a face is detected, the user can save the image as a new training sample by entering a name. The image is resized and saved in the /Faces folder.
Face Recognition: During real-time capture, the application uses the EigenObjectRecognizer to recognize faces by comparing them against the stored images and labels.
Files and Folder Structure:
/Faces/: Contains face images and a Faces.txt file with the list of labels.
haarcascade_frontalface_default.xml: The Haar Cascade classifier file for detecting faces.

Features to be Added:
Multiple Face Recognition: Support for recognizing multiple faces in a single frame.
Face Database Management: Add options to update, delete, or modify saved faces.
Improved Performance: Optimize recognition speed for real-time applications.
