# ⛏️ Minecraft Crafting System: Backend & UI Challenge

![Unity](https://img.shields.io/badge/Unity-2025%2B-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow?style=for-the-badge)

---

## 🎯 Project Goal

The primary objective is to replicate the logic and interface of the inventory, stacking, and crafting mechanics from *Minecraft*. This challenge focuses on **system architecture**, **clean code principles (SOLID)**, and **decoupling data from presentation** using the **Model-View-Controller (MVC)** pattern.

### ⚙️ Technologies & Concepts Applied
* **Engine:** Unity (2026.6000.2.14f1 / URP 2D)
* **Language:** C#
* **Data Structure:** ScriptableObjects (Item & Recipe Databases)
* **Architecture:** Model-View-Controller (MVC) & Singleton Pattern
* **Design Principles:** Single Responsibility Principle (SRP) & Open/Closed Principle (OCP)
* **Version Control:** GitFlow (Atomic Commits)

---

## 🧠 System Architecture Overview

The system is strictly decoupled into data, logic, and presentation layers:

### 1. The Model (Data)
* **`ItemData.cs` (ScriptableObject):** Defines static properties (ID, `maxStackSize`, icon). Acts as the item database.
* **`InventorySlot.cs`:** A serializable C# class representing a container space (Item reference + Count).
* **✨ `CraftingRecipe.cs` (ScriptableObject):** Upgraded to support two crafting modes:
    * **Shapeless:** List of ingredients (`shapelessIngredients`) where order is irrelevant.
    * **Shaped:** A 3x3 Grid definition (`shapedGrid`) representing the required pattern.
* **`RequiredItem.cs`:** Serializable helper class for defining recipe ingredients.

### 2. The Controller (Logic)
* **`InventoryManager.cs`:** The central brain for storage. Handles `AddItem()`, stacking logic, and overflow checks.
* **✨ `CraftingManager.cs`:** The crafting brain. Features:
    * **Hybrid Validation:** Automatically detects if a recipe is Shapeless or Shaped.
    * **Relative Pattern Matching:** Uses a "Bounding Box" algorithm to detect shaped recipes regardless of their position in the grid (e.g., a 2x1 pattern works in any column).
    * **Consumption Logic:** Safely removes materials from the grid upon crafting.
* **✨ `CraftingGridManager.cs`:** Responsible solely for holding references to the 9 input slots and providing them to the Manager.
* **✨ `DragManager.cs` (Singleton):** Manages the global state of the "hand" (cursor), holding item data while dragging between slots.

### 3. Interaction & Input (The "Glue")
* **✨ `SlotInteraction.cs`:** Handles Pointer events (Click, Drag, Drop). Implements standard inventory behavior (Left Click to take all, Right Click to split stack).
* **✨ `OutputSlotInteraction.cs`:** A specialized subclass (Polymorphism/OCP) that overrides interaction logic specifically for the crafting result slot (e.g., prohibits placing items, handles crafting consumption on pickup).

### 4. The View (UI)
* **`InventoryUI.cs`:** Dynamically generates the slot grid and syncs visual state with data.
* **`SlotUI.cs`:** Renders the Sprite and Quantity text.
* **✨ `DragVisual.cs`:** Follows the mouse cursor, rendering the item currently being dragged.

---

## 📸 Visual Progress

https://github.com/user-attachments/assets/b1f22839-c276-460b-817f-6ad8c7c9e5ed

---

## 📈 Feature Progress Summary

### ✅ Inventory Fundamentals
- [x] **Data Structure:** Separated `ItemData` vs `InventorySlot`.
- [x] **Stacking Logic:** Items automatically stack up to their `maxStackSize`.
- [x] **Overflow Handling:** Returns `false` if inventory is full.

### ✅ Drag & Drop System
- [x] **Visual Feedback:** Item icon follows the mouse cursor.
- [x] **Swapping:** Items swap places if dropped on occupied slots.
- [x] **Splitting:** Right-click splits stacks in half.
- [x] **Stack Accumulation:** Dropping same-type items merges their stacks.

### ✅ Crafting System (Advanced)
- [x] **3x3 Grid:** Fully functional crafting table UI.
- [x] **Shapeless Crafting:** Works for unordered recipes (e.g., 1 Log -> 4 Planks).
- [x] **Shaped Crafting:** Works for specific patterns (e.g., Pickaxe).
- [x] **Relative Positioning:** Patterns are detected anywhere in the grid (e.g., a 2-vertical stick recipe works in the left, center, or right column).
- [x] **Output Slot Logic:** - Preview appears automatically.
    - Taking the item consumes ingredients.
    - Support for "Spam-Clicking" to craft multiple times rapidly.
    - "Ghost Item" prevention (Diamond doesn't appear on empty tables).

---

## 🚀 Next Steps (Roadmap)
- [ ] **Shift-Click Shortcuts:** Move items instantly between Inventory and Hotbar/Grid.
- [ ] **Tooltips:** Hover information for items.
