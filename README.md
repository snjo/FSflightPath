FSflightPath
============
Copyright Andreas Aakvik Gogstad

A KSP mod addon. Create flight path and have targets follow them.


v1.2
-Save and load paths to files.
-Alternate models can be defined for the paths by editing the path file for now.

v1.1
-Added looping path functionality.
When you end a path, the time to return to the first point using your current velocity is calculated. Will look OK if it the end doesn't overshoot the start.
-Changed gui toggle hotkey to alt+F6 (Or Shift+F6 fo Linux people)

Take care when loading a path to not occupy its starting position with your current vessel. Cause you will blow up.
The recording and manual play back is now the "work path". When a work path plane ends its run, it will fall out of the sky rather than be deleted, even if Go off rails at end" is marked.
Paths loaded from external files will play automatically, and the plane will either loop, be removed or fall out of the sky (go off rails) when the path ends.

Menu explanation

Record - Starts or continues recording to the work path.
Stop - Ends the recording. Also calculates the time to return the start for looping paths.
Clear - Clears then nodes, and adds the first node at the current location. Best used while recording is active.
Add node - For precise work like recording a stand still, or small ground movements, add nodes at the start and end of the stop to make it look better.

Loop path - If ON, the plane will return to the start of the path when it reaches the end, using the end speed.
Off rails at end - If the path isn't a loop, the plane will fall to the ground after the path ends. Otherwise it will be deleted. (Unless it's a work path playback)

Play - Plays the work path
OffRails - Makes the work path plane fall down

Save - saves the work path using the file name in the text box
Load - Opens up the path loading dialog

Alt+F6 - show/hide the menu

Current limitations:
-It will not use your craft as the dummy, but the model in the models folder, but it can be replaced. (You could make a new plane model, or a truck, whatever)
-No sound on the dummy plane.

How it works:
When recording, nodes will be created storing the vessels current position, rotation and velocity. Nodes will only be created if there has been sufficient change to those values, so you will create lots of nodes during turns and other maneuvers, and very few when flying in a straight path.
To precisely record a small taxiing maneuver or a stop,you can add a node manually.
By default, no nodes will be created if you have moved less than 2 m. The precision settings will be controllable later on.

Source code

License: You can re-use the code as you want, as long as you give attribution in the forum post and readme file. You can rename and recompile stuff, but not repack the plugin as is in your own mod without permission.
Any models, sounds, textures or other assets are not for re-use without permission.
