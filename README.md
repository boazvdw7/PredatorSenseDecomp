# PredatorSenseDecomp (WIP)

**This project is for the PH-315-51 only**

This repository contains a reverse-engineered version of Acer's PredatorSense application using [dnSpy](https://github.com/dnSpy/dnSpy) because Acer 
decided to stop updating the software and it's broken on Windows 11. This project aims to resolve the bugs introduced with Windows 11 and add QoL features.

## Disclaimer

This project is intended strictly for **educational and research purposes**.

---

## Implemented Features
- **Fan Curve Editor:** Modify fan curves (located in advanced settings)
- **Minimize to System Tray**

## WIP Features
- [x] **Fan Curve Editor:** Modify fan curves (located in advanced settings)
- [x] **Minimize to System Tray**
- [x] **Installer:** Create an installer for easy deployment
- [ ] **Custom Fan Profiles:** Create and manage custom fan profiles
- [ ] **Individual Fan Control:** Control each fan independently

## Current state

- Both PredatorSense and TsDotNetLib are 100% decompiled and can be build
- Some debug symbols are not translated yet
- Some code has been temporarily disabled which might break some functions

---

## Overview

The solution includes:

- `TsDotNetLib/` – Decompiled core library (originally `TsDotNetLib.dll`)
- `PredatorSense/` – Decompiled main application referencing the above library

Both components are restored as buildable Visual Studio projects.  
References and paths have been adjusted to allow building from source.

---

## How to Build

1. Clone the repository
2. Open `PredatorSense.sln` in Visual Studio
3. Build the solution (ensure `TsDotNetLib` is correctly referenced)

---

## Tools Used

- [dnSpy](https://github.com/dnSpy/dnSpy): for .NET assembly decompilation
- Visual Studio 2022

---

## Credits

- [dnSpy by 0xd4d](https://github.com/dnSpy/dnSpy) – indispensable tool for reverse engineering and debugging .NET binaries.

