# 🃏 Card Game Project

A mobile and PC compatible card game prototype developed in Unity. This project showcases card management, sorting algorithms, responsive input handling, and smooth drag-and-drop interactions. Tested on Android for performance and stability.

## Features

### Card Management & Sorting
- Multiple sorting strategies: One-Two-Three, Seven-Seven-Seven, and Smart Sorting.
- Object pooling to reduce memory allocations.
- Optimized in-place sorting logic to minimize garbage collection.

### Input Handling (PC & Mobile)
- Unified input system supporting both mouse and touch.
- Cooldown mechanism to prevent rapid spam clicks.
- Drag-and-drop interaction with visual feedback.

### Gameplay Experience
- Only valid cards are draggable from hand.
- Bezier curve–based card placement for smooth layout.
- Input is temporarily disabled during animations for better UX.

### UI & Scene Management
- Simple and clean scene transitions.
- Safe area support for various screen sizes and aspect ratios.

### Performance Optimizations
- Object pooling for all cards.
- Parallel animations using `UniTask`.
- Avoided unnecessary list allocations.

### Unit Testing
- Sorting logic is thoroughly tested with `NUnit`.
- Edge cases and multiple hand configurations are validated.

### Technologies Used
- **Unity (URP)**
- **C#**
- **Zenject**
- **Odin Inspector**
- **UniTask**
- **DOTween**
- **Unity Input System**
- **NUnit**

## Setup
1.	Open the project in Unity 2022.3 LTS or later.
2.	Make sure Git LFS is installed to fetch all assets properly.
3.	All required packages (UniTask, DOTween, Zenject) are included via UPM or embedded in the project.

## APK
https://drive.google.com/file/d/1sqFFYBU9dz0AV4ZxuUrjPG11b2I9UlOG/view?usp=sharing

## Gameplay
https://drive.google.com/file/d/1zfnzIGvaGkJzwb0rEbxGO4GhumH658kg/view?usp=drive_link
