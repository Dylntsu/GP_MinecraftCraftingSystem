# ⛏️ Minecraft Crafting System: Backend & UI Challenge

![Unity](https://img.shields.io/badge/Unity-2022%2B-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow?style=for-the-badge)
---

## 🎯 Project Goal

The primary objective is to replicate the logic and interface of the inventory, stacking, and crafting mechanics from *Minecraft*. This challenge focuses on **system architecture** and **decoupling data from presentation** using the **Model-View-Controller (MVC)** pattern.

### ⚙️ Technologies & Concepts Applied
* **Engine:** Unity (2026.6000.2.14f1 / URP 2D)
* **Language:** C#
* **Data Structure:** ScriptableObjects (for Item Database)
* **Architecture:** Model-View-Controller (MVC)
* **Version Control:** GitFlow (Atomic Commits)

---

## 🧠 System Architecture Overview

The system is split into three main, decoupled components:

### 1. The Model (Data)
* **`ItemData.cs` (ScriptableObject):** Defines the static properties of any item (e.g., ID, `maxStackSize`, icon). This acts as the project's centralized database.
* **`InventorySlot.cs`:** A serializable C# class representing a single container space (the item reference + the `stackSize` count).

### 2. The Controller (Logic)
* **`InventoryManager.cs`:** The central brain. It handles the core mechanics:
    * Initializes the `InventorySlot[]` array.
    * Manages the complex `AddItem()` logic (prioritizing existing stacks before looking for an empty slot).

### 3. The View (UI - *In Progress*)
* **`SlotUI.cs` / `InventoryUI.cs`:** These scripts (coming in Week 2) will be responsible **only** for drawing the contents stored in the `InventorySlot` array and passing player input back to the Controller.
