# AGENTS.md

Welcome to the `HackathonDream` project. This document serves as a guide for AI agents and developers to understand the project architecture, conventions, and status.

## Project Overview
- **Name**: HackathonDream
- **Engine**: Unity (C#)
- **Genre**: 3D Shooter / Action (Inferred)
- **Platform**: Windows (Dev environment)

## Directory Structure & Key Files
The core source code is located in `Assets/Scripts/`.

### Core Framework
- **`MonoSingleton.cs`**: Base class for Singleton managers.
- **`InputManager.cs`**: Handles player input.
- **`MainLoopEntry.cs`**: Entry point for the main game loop.
- **`MyDebugLogger.cs`**: Custom logging wrapper.
- **`ReactiveProperty.cs`**: Implementation of reactive variables for state management.
- **`ObjectPool.cs`**: Generic object pooling system.

### Entities & Combat (`Assets/Scripts/Entities/`)
- **`EntityStatus.cs`**: Manages entity stats (HP, etc.).
- **`WeaponBase.cs`**: Base class for all weapons.
- **`WeaponBulletBase.cs`**: Base class for bullets/projectiles.
- **`BulletObjectPool.cs`**: Specialized pool for bullet management.
- **`EntityUtil.cs`**: Contains utility interfaces like `IDamageable`.

### Gameplay Modules
- **`Player/`**: Player controller and specific logic.
- **`Enemy/`**: Enemy AI and behavior.
- **`UI/`**: User Interface logic.
- **`Sound/`**: Audio management.

## Architecture Guidelines

### 1. Object Management
- **Pooling**: Use `ObjectPool` or `BulletObjectPool` for frequently instantiated objects (bullets, effects) to avoid GC pressure.
- **Singletons**: Use `MonoSingleton<T>` for global managers.

### 2. Event System
- Use C# `Action` or `Action<T>` for loose coupling between components (e.g., `OnHit` events in bullets).
- **Critical**: Always check for null before invoking events (`OnEvent?.Invoke()`).

### 3. Data Binding
- Use `ReactiveProperty<T>` to expose changing values (like HP) to UI or other systems without direct dependency.

## Development Rules
- **Formatting**: Standard C# coding conventions. PascalCase for public members, camelCase for private.
- **Safety**: Guard against `MissingReferenceException` and `NullReferenceException`, especially when dealing with objects that might be destroyed/pooled.
- **Files**: Do not modify existing `.meta` files manually.

## Recent Context (System State)
- **Bullet System**: Recently underwent debugging for `MissingReferenceException` and destruction logic. Ensure `BulletObjectPool` is correctly utilized.
- **Git**: Authentication issues were resolved; use proper credentials.

## Instructions for Agents
1. **Read-First**: Before modifying complex logic, read specific class files (`WeaponBase`, `EntityStatus`) to understand existing inheritance and patterns.
2. **Context**: refer to this file to understand where new scripts should be placed.
