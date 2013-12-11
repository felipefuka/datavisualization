datavisualization
=================

IMPORTANT! HOW TO EXECUTE!
Please extract "Data Visualization" directory using WinRar and then 
run "MetricSPlat_Kinect.exe". 

In order to view and debug the program you will need Kinect for Windows
SDK (1.5 or above) for Microsoft Visual C# 2010 which can be found at the
following link: 

http://www.microsoft.com/en-us/kinectforwindowsdev/Downloads.aspx


Motivation: 
Create and evaluate with new forms of visualizing and interacting
with large multidimensional data sets.


Description: 
This application is an implementation of Fastmap, a new indexation 
algorithm, that is used to visualize large multidimensional data sets
(Cancer.txt, in this example). This application also has implemented
natural interaction via gestures in order to rotate, zoom, and move the
3D visualization.

For more information regarding the Fastmapping algorithm and its 
application in data visualization:
https://www.google.com.br/url?sa=t&rct=j&q=&esrc=s&source=web&cd=1&sqi=2&ved=0CDIQFjAA&url=http%3A%2F%2Fwww.cis.temple.edu%2F~vasilis%2FCourses%2FCIS595%2FPapers%2FPres-FastMap.doc&ei=HPQrUaiMNJHm9gT8-YGICg&usg=AFQjCNERtPJhn7o6WJ0Gb4ySIdPvuuQiyQ&sig2=uIOXE0MpGDaVV26syzccVQ&bvm=bv.42768644,d.dmQ


Controls:
Kinect
Move your palms while in front of the Kinect/PC device and your natural 
gestures will interact with the 3D visualization. 

Mouse
Moving your mouse over the 3D visualization will interact with it. There
are also key modifiers:
Press and hold Control: limits mouse movements to interacting only with the Z axis
Press and hold Alt: limits mouse movements to interacting only with the Y axis
Press and hold Shift: adjusts zoom
clicking and dragging: Move 3D visualization

Buttons
Rotate Z -: rotates the 3D visualization in the Z axis clockwise.
Rotate Z +: rotates the 3D visualization in the Z axis counter clockwise.
Rotate Y -: rotates the 3D visualization in the Y axis clockwise.
Rotate Y +: rotates the 3D visualization in the Y axis counter clockwise.
Zoom -: zooms out.
Zoom +: zooms in.


Responsibilities:
I picked up this project as a substitute for an on going research
project at the University of Sao Paulo (in Sao Carlos, Sao Paulo, 
Brazil).The original code had been written in C++ and we needed a 
version in C# in order to integrate motion-sensing. During this 
project I was responsible for adapting the available C++ code to 
C#, implementing 3D graphics in OpenGL as well as aiding in the kinect 
intergration. 


Results:
By having participated in this project I obtained advanced knowledge
in indexation algorithms and advanced data structures such as Octrees.
Furthermore, I refined my skills in OpenGl, and gained a larger
understanding of the similarities and differences between C++ and C#.
Lastly, I taught myself how to integrate Kinect to a C# application 
through the use online tutorials and determination.



