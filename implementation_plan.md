# Implementation Plan: Additional Weapons (Shotgun & Sniper Rifle)

This plan outlines the implementation of two new weapon types to expand the game's arsenal.

## 1. Goal
Implement **Shotgun** (Multi-projectile spread) and **Sniper Rifle** (High-power, piercing) to demonstrate weapon variety and system flexibility.

## 2. Proposed Changes

### A. Core System Updates (`WeaponBulletBase.cs`)
*   **Modification**: Change `HandleOnHit` from `private` to `protected virtual`.
    *   *Reason*: To allow the Sniper Rifle's bullet (`PiercingBullet`) to override the default "destroy on hit" behavior, enabling it to penetrate enemies.

### B. New Weapon: Shotgun
*   **Script**: `Assets/Scripts/Entities/Shotgun.cs`
*   **Logic**:
    *   Inherits from `WeaponBase`.
    *   **Override `DoShot`**:
        *   Fires multiple `StandardBullet` instances (e.g., 5 pellets) at once.
        *   Calculates a spread angle (e.g., -15 to +15 degrees) for each pellet.
*   **Assets**:
    *   Uses existing `StandardBullet` prefab.

### C. New Weapon: Sniper Rifle
*   **Script**: `Assets/Scripts/Entities/SniperRifle.cs`
*   **Logic**:
    *   Inherits from `WeaponBase`.
    *   **Override `DoShot`**:
        *   Fires a single `PiercingBullet`.
        *   High projectile speed, long cooldown.
*   **New Bullet Class**: `Assets/Scripts/Entities/PiercingBullet.cs`
    *   Inherits from `WeaponBulletBase`.
    *   **Override `HandleOnHit`**:
        *   Inflicts damage.
        *   **Does NOT** call `Kill()` immediately.
        *   (Optional) Implements a "max pierce count" or destroys only on wall collision.
    *   **Override `OnLifeTimeEnd`**:
        *   Ensure it cleans up properly (returns to pool).

## 3. Implementation Steps

1.  **Refactor**: Update `WeaponBulletBase.cs` to expose `HandleOnHit`.
2.  **Class Creation**:
    *   Create `Shotgun.cs`.
    *   Create `PiercingBullet.cs`.
    *   Create `SniperRifle.cs`.
3.  **Setup**:
    *   Use the `BulletObjectPool` for the new `PiercingBullet`.
    *   (Optional) Update `WeaponSetupTools` to support these new types.

## 4. Verification
*   **Shotgun**: Verify multiple bullets fire in a cone.
*   **Sniper**: Verify bullet passes through the first target and hits the second.
