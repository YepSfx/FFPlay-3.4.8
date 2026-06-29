# FFPlay-3.4.8

This repository provides a multimedia playback library and sample demos to demonstrate its usage.

---

## Library
**FFPlayLib** is a library for playing multimedia file formats supported by **FFPlay** in **FFmpeg 3.4.8**.

---

## Demos
- **Delphi / Lazarus**
- **C++ MFC**
- **C# (WinForms and WPF)**
- **Python (Windows and Linux)**
---

## Usage
To use **FFPlayLib**, ensure the following libraries are available in your system path:
- **SDL2**
- **FFmpeg (3.4.x)**

---

## Build Instructions (Windows)
To build the library for **Windows OS**:
1. Install **MSYS2** on the build machine.
2. Build the library in the **MSYS2 environment (MinGW32)**.

---

## Build Requirements (Linux - Ubuntu)
- **GCC version 14**
- **SDL2** library must be built on the build machine.
- **ffmpeg 3.4.x** (https://ffmpeg.org/download.html) must be built on the build machine.

---

## Build Scripts
Instead of using a Makefile, two scripts are provided for building:
- `./build-win` – Build script for **Windows** systems.
- `./build-ubuntu` – Build script for **Ubuntu** systems (tested on **Ubuntu 22.04 LTS**).

---

### Notes
- Ensure all dependencies are correctly installed and available in your system path before building or running demos.
- For detailed instructions on installing MSYS2 and building SDL2/FFmpeg, refer to their official documentation.

## Screenshots

![Ubuntu screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/ubuntu.png "Lazarus FFPlayer in Ubuntu")
![DELPHI screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/win-delphi.png "DELPHI FFPlayer in Windows")
![Lazarus screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/win-laz.png "Lazarus FFPlayer in Windows")
![MFC(C++) screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/win-mfc(cpp).png "MFC(C++) FFPlayer in Windows")
![Winforms (C# .Net) screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/win-winforms.png "C# Winforms FFPlayer in Windows")
![WPF (C# .Net) screenshot](https://raw.githubusercontent.com/YepSfx/FFPlay-3.4.8/main/screenshots/win-wpf.png "C# WPF FFPlayer in Windows")


---
If you have any questions, please e-mail me @ cj.github.proj@gmail.com
