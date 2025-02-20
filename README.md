# NITHmouseController
_An application for hands-free mouse control and emulation using a webcam (and NITHsensors)_

<br>
<p align="center">
  <img src="https://raw.githubusercontent.com/LIMUNIMI/NITHmouseController/main/NithLogo_White_Trace.png" alt="NITH logo." width="150px"/>
</p>
<br>

<p align="center">
  <img src="https://raw.githubusercontent.com/LIMUNIMI/NITHmouseController/refs/heads/main/Readme_Images/GUI.png" width="75%" />
</p>


## Overview
NITHmouseController is an application which provides a series of tools and experimental methods for hands-free mouse control through NITH sensors. Thus it is suitable for users with motor disabilities such as quadriplegia.
It provides a way for moving the mouse as well as pressing the buttons, using:
- Head rotation (for cursor movement)
- Eye blinking or mouth aperture (for button clicking)

## Requirements and installation
### Sensors
NITHmouseController **requires** some sensors able to detect the cursor movements.

You can use a normal laptop webcam in order to do it all, but we _strongly recommend_ using a dedicated sensor for head tracking to achieve more precision, such as the [NITHheadTracker](https://neeqstock.notion.site/NITHheadTracker-BNO055-eda9cb4d752c45869abd85d06a1d7e5d), which you can easily build by yourself by following the instructions provided in the link.

For webcam control, which is required either way, you can download and use our [NITHwebcamWrapper](https://neeqstock.notion.site/NITHwebcamWrapper-ee58e40a3d1349c8a43caf9fd577be10). Please follow all the instructions provided in the link for downloading and using it. It will require to download and install Python, as indicated.

### Computer and software requirements
At the present moment, NITHmouseController is **only compatible with Windows** operating system.
Apart from that, you can use it on any machine, provided you have a webcam.

### Download and run
In order to download and use NITHmouseController, you can [download the latest release](https://github.com/LIMUNIMI/NITHmouseController/releases), or clone the entire repository. In either case, you should **run NITHmouseController.exe**. If you cloned the repo, you will find it in the _bin/release_ folder.

## Usage

To start using the application, please follow these steps:

1. **Run the Application**  
    Launch **NITHmouseController.exe**. Make sure **NITHwebcamWrapper** is running in the background.

2. **Authorize Connection**  
    When prompted by a Windows popup, **grant authorization** to allow NITHmouseController to listen for connections. Once authorized, the application will automatically connect and receive data from NITHwebcamWrapper.

3. **USB Head Tracker**  
    If you are using a USB head tracker compatible with NITH (e.g. _NITHheadTracker_):
    - Click **Scan** to detect the device through the USB ports.
    - Upon recognition, the head tracker will automatically connect to the application.

4. **Calibration**  
    Follow these steps to calibrate your system:
    - Click **Center** to calibrate the head tracker while keeping your head facing the screen.
    - With your eyes and mouth at their full aperture, click **Open** within the calibration controls.
    - Then, close both your eyes and mouth and click **Closed**.  
    This process allows the application to understand the full range of your movements.

5. **Adjust Settings**  
    - **Cursor Sensitivity**: Use the **Sensitivity** control to adjust how fast the cursor moves.
    - **Movement Smoothing**: Adjust the **Movement filter** control to smooth out cursor movements.

### Interaction Methods
**Head rotation** along the **pitch** and **yaw** axes controls the movement of the mouse cursor.
The application supports two distinct methods for simulating mouse clicks:
- **Blink Clicking**:  
    A left eye blink triggers a left mouse button click. You can also hold the click by keeping your eye closed.
- **Mouth Clicking**:  
    - Move your head using the **roll axis** to select either the left or right mouse button.  
    - Return your head to the center once the button is selected.
    - Opening your mouth past a certain threshold will trigger a click for the selected button.  
      You can maintain the click by keeping your mouth open.

### GUI
The graphical user interface of the application contains 3 **console output screens**, in black.
- The leftmost outputs raw data from the USB NITH head tracker (or any compatible sensor correctly connected through USB);
- The central one outputs raw data from the NITHwebcamWrapper (or any compatible sensor correctly connected through UDP);
- The rightmost outputs general debug messages.

On the bottom left corner, there's a **head position indicator** which provides feedback on head rotation. The white dot reflects the movement on pitch/yaw axes, the green dot reflects the movement on the pitch/roll axes.

## Contribute
You are absolutely free and welcome to contribute to the development of this program!
You can clone and fork this repo as you wish. Please raise an issue for any question.

The application is written in C# using _Microsoft Visual Studio_. We recommend using the same IDE: you can clone this repo and open the _.sln_ solution file.
In order to compile the source code, you need to clone and place the repositories of the following libraries next to NITHmouseController's root folder.
- [NITHlibrary](https://github.com/LIMUNIMI/NITHlibrary), which is the main library for interfacing with NITH sensors and wrappers
- [NITHemulation](https://github.com/LIMUNIMI/NITHemulation), which provides emulation capabilities for standard I/O devices (mouse and keyboard)

## License
This software is licensed through the GNU GPL-v3.0 Free Software license.



