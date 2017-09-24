# KinectFeaturesExtractionModule
This GitHub project is for creating dataset of various feature extraction strategies.
* For each dataset it creates 5 datasets of various sizes. (5,10,15,20,25)
* It will then evaluate each dataset with a given training set.
* It will produce a spreadsheet with the results.

Must be supplied with a file containing training data and testing data.

## How to run

1. Download the GitHub project into a folder.
2. Download visual studio's 2015 or higher.
3. Place the Training dataset into a known location(e.g. desktop)
4. Open visual studio's
5. Click "File" -> "Open" -> "Project/Solution..." or shortcut "Ctrl + Shift + O"
6. Navigate to the GitHub project and double click on the file "FallDetectionSystemDataProcessor.sln". The file "FallDetectionSystemDataProcessor.sln" should be in the same directory as this README.md file.
7. configure the file program.cs and edit "line 19 - string baseDir = "C:/Users/n/Desktop/";". Change this to the location of the training data.
8. Click the Start button to run the application.
9. This will bring up a console that indicates the program is running.

Once the program has completed it would create new datasets in the debug folder in the GitHub project.
