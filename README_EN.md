# Sistema_Interactivo_UCB

## Description

This project consists of the development of an **educational simulator in virtual reality (VR)** designed for teaching concepts related to **industrial automation**, focusing on **pneumatic systems** controlled by **PLCs**. The simulator aims to improve students' understanding through an immersive environment that allows direct interaction with virtual components such as **pneumatic cylinders** and **valves**.

The project is aimed at **Mechatronics Engineering** students and related fields, facilitating the learning of **symbols**, **connections**, and **programming** through challenges of varying difficulty.

## Technologies Used

- **Unity** (main development engine)
- **XR Interaction Toolkit** (for VR interaction)
- **Meta Quest 2 and Meta Quest 3** (target devices)
- **Blender / Autodesk Fusion 360** (component modeling)
- **C#** (programming language)
- **OpenXR** (cross-platform VR standard)

## Installation

### Prerequisites

- **Unity 2021.3 LTS** or higher with XR support
- **Meta Quest 2 or 3** device
- **Cable or wireless connection** for testing (Air Link or similar)

### Basic Steps
Clone the repository:
```bash
git clone https://github.com/FerPonz98/Sistema_Interactivo_UCB.git
cd Sistema_Interactivo_UCB
```

1️⃣ **Open the project in Unity**  
2️⃣ **Configure the Player Settings for Meta Quest (OpenXR enabled)**  
3️⃣ **Build for Android and test on the headset**

## Usage

Once the application is deployed:

- **Select the game mode:** Free Mode or Time Trial  
- **Solve the challenges** by connecting pneumatic components according to the given questions  
- **The system records** the time and the chosen solution  
- **Levels increase in difficulty** with different types of cylinders, valves, and circuits  

## Simulator Structure

- **Start Panel:** Interface to select game mode  
- **Game Panel:** Immersive scene with interactive prefabs  
- **End Panel:** Summary of the time spent and feedback  
- **Hard Mode:** Includes three possible solutions per question with visual feedback  

## Authors

- **Fernando Ponz** (Lead developer, Mechatronics Engineering)  
- **Universidad Católica Boliviana** (Educational project)

## License

This project is for academic use only and does not have a commercial distribution license in this version.
