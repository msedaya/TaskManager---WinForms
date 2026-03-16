# Task Manager
A personal task manager desktop application built with C# and WinForms.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6?style=flat-square&logo=windows)
![Language](https://img.shields.io/badge/language-C%23-239120?style=flat-square&logo=csharp)

---

## Features

- Add, edit, and delete tasks
- Mark tasks as complete
- Priority levels — High, Medium, Low
- Filter tasks by status
- Due date tracking with overdue detection
- Color-coded priority strips and status badges
- Status bar with live task counts

---

## Project Structure

```
TaskManager/
├── Models/
│   └── TaskItem.cs          # Task data model, enums
├── Services/
│   └── TaskService.cs       # Business logic, CRUD operations
├── Helpers/
│   └── UIFactory.cs         # Design system, control factory
├── Forms/
│   ├── MainForm.cs          # Main window logic and events
│   ├── MainForm.Design.cs   # Main window layout
│   ├── TaskDialog.cs        # Add/Edit dialog logic
│   └── TaskDialog.Design.cs # Add/Edit dialog layout
└── Program.cs               # Entry point
```

---

## Architecture

```
UI (Forms)  →  Service  →  Models
                 ↑
         TasksChanged event
```

- **Models** — plain data classes, no logic
- **Service** — all business logic, fires events on data change
- **Forms** — UI only, delegates everything to the service
- **UIFactory** — design system, all controls created here

---

## Patterns Used

| Pattern | Where |
|---|---|
| Service layer | `TaskService.cs` |
| Observer (events) | `TasksChanged` event |
| Factory | `UIFactory.cs` |
| Partial class | `MainForm.cs` / `MainForm.Design.cs` |
| Owner-draw | `ListBox_DrawItem` in `MainForm.cs` |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Windows OS (WinForms is Windows only)
- VSCode with C# Dev Kit extension

### Run the project

```bash
git clone https://github.com/YOURUSERNAME/TaskManager.git
cd TaskManager/TaskManager
dotnet run
```

---

## Key Concepts Learned

- Code-first WinForms — no drag and drop designer
- Partial classes to separate UI layout from logic
- `DockStyle` and `FlowLayoutPanel` for layout
- `TableLayoutPanel` for grid-based forms
- Owner-draw `ListBox` for custom row rendering
- `SplitContainer` for resizable panels
- Event-driven programming with `EventHandler`
- LINQ for filtering and querying collections
- `IReadOnlyList` to protect internal state
- `Guid` for unique identifiers
- Nullable types `DateTime?` and `TaskItem?`
- Pattern matching with `switch` expressions

---

## What's Next

This is **Project 1** of a 5-project C# desktop development course.

| Project | Topic |
|---|---|
| **1 — Task Manager** | WinForms basics, UIFactory, Service layer |
| 2 — Expense Tracker | JSON persistence, Repository pattern |
| 3 — Inventory Manager | SQLite, Entity Framework, LINQ |
| 4 — Budget Dashboard | WPF, MVVM pattern, data binding |
| 5 — News Reader | REST APIs, async/await, dependency injection |