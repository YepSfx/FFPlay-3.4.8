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

## Build Requirements (Windows)
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



---
If you have any questions, please e-mail me @ cj.github.proj@gmail.com
