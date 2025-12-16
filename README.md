# ⛏️ Minecraft Crafting System: Backend & UI Challenge

![Unity](https://img.shields.io/badge/Unity-2025%2B-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow?style=for-the-badge)
---

## 🎯 Project Goal

The primary objective is to replicate the logic and interface of the inventory, stacking, and crafting mechanics from *Minecraft*. This challenge focuses on **system architecture** and **decoupling data from presentation** using the **Model-View-Controller (MVC)** pattern.

### ⚙️ Technologies & Concepts Applied
* **Engine:** Unity (2026.6000.2.14f1 / URP 2D)
* **Language:** C#
* **Data Structure:** ScriptableObjects (for Item Database & Recipe Database)
* **Architecture:** Model-View-Controller (MVC)
* **Version Control:** GitFlow (Atomic Commits)

---

## 🧠 System Architecture Overview

The system is divided into three main, decoupled components:

### 1. The Model (Data)
* **`ItemData.cs` (ScriptableObject):** Defines the static properties of any item (e.g., ID, `maxStackSize`, icon). This acts as the project's centralized database.
* **`InventorySlot.cs`:** A serializable C# class representing a single container space (the item reference + the `stackSize` count).
* **✨ `CraftingRecipe.cs` (ScriptableObject):** Defines the requirements (`RequiredItem`) and the result (`resultItem`, `resultAmount`) of a recipe. This acts as the crafting database.

### 2. The Controller (Logic)
* **`InventoryManager.cs`:** The central brain. It handles core mechanics like `AddItem()`, stacking, and empty slot searching.
    * **Synchronization:** Contains the method that calls `InventoryUI.UpdateUI()` after every successful change in the logic.
* **✨ `CraftingManager.cs`:** The crafting controller. Its responsibility is to search within `availableRecipes` and execute the **resource verification** logic (`CanCraft`).

### 3. The View (UI - Completed)
* **✨ `InventoryUI.cs`:** The Visual Controller. Dynamically generates the 36 `Slot_Prefab` instances and associates them with their respective logical `InventorySlot` objects.
* **✨ `SlotUI.cs`:** The individual View. Responsible **only** for drawing the Sprite and text based on the assigned `InventorySlot` content, and managing icon visibility if the slot is empty.

---
<img width="1593" height="891" alt="image" src="https://github.com/user-attachments/assets/4383fb0c-8ab0-473c-ab33-e4c4d9e9a3f8" />

---

## 📈 DevLog & Progress Summary

Here are the key advancements achieved to date:

### ⚙️ DEVLOG: Day 2 (UI Implementation & Synchronization)

| Feature | Status | Detail |
| :--- | :--- | :--- |
| **UI Generation** | **COMPLETED** | `InventoryUI.cs` dynamically generates 36 slots based on `Slot_Prefab` using `Grid Layout Group`. |
| **Slot View** | **COMPLETED** | `SlotUI.cs` implemented to read `itemIcon` and `stackSize` from the Model. |
| **MVC Synchronization** | **COMPLETED** | The `UpdateUI()` method was implemented and called from `InventoryManager` after every `AddItem`, ensuring the UI instantly reflects the Model. |
| **Troubleshooting** | Resolved | Multiple `NullReferenceException` errors caused by missing UI reference assignments within the `Slot_Prefab` were fixed. |

### ⚙️ DEVLOG: Day 3 (Crafting Data & Verification)

| Feature | Status | Detail |
| :--- | :--- | :--- |
| **Recipe Structure** | **COMPLETED** | Implementation of `CraftingRecipe.cs` (SO) and `RequiredItem.cs` to formally define crafting requirements. |
| **Crafting Controller** | **IMPLEMENTED** | `CraftingManager.cs` added to the system to handle crafting logic. |
| **Verification Logic** | **IMPLEMENTED** | The `CanCraft(recipe)` function was implemented, which uses `CountItem()` to sweep the entire inventory and verify if the total resources are sufficient for the recipe (Shapeless Crafting). |
| **Next Step** | Pending | Implement the `CraftingGridManager` and the Crafting Area UI (2x2) to separate input items from the main inventory. |

---

### 🏷️ DevLog Tags
`#GameplayProgramming` `#UI` `#Synchronization` `#MVC` `#ScriptableObjects` `#CraftingSystem`
