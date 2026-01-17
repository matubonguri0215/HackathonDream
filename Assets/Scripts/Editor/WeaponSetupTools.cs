using UnityEngine;
using UnityEditor;

public class WeaponSetupTools : EditorWindow
{
    [MenuItem("Tools/HackathonDream/Setup Basic Weapon")]
    public static void SetupBasicWeapon()
    {
        // 1. Create Prefabs Folder
        string prefabFolderPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabFolderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        string weaponFolderPath = prefabFolderPath + "/Weapons";
        if (!AssetDatabase.IsValidFolder(weaponFolderPath))
        {
            AssetDatabase.CreateFolder(prefabFolderPath, "Weapons");
        }

        // 2. Create StandardBullet Prefab
        GameObject bulletGO = new GameObject("StandardBullet_Auto");
        bulletGO.AddComponent<StandardBullet>();
        // Check/Add HitChecker if required by WeaponBulletBase (RequireComponent handles it, but good to ensure)
        if (bulletGO.GetComponent<HitChceker>() == null) bulletGO.AddComponent<HitChceker>();
        
        var col = bulletGO.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.2f;

        var rb = bulletGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Create a simple visual just so it's visible
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(bulletGO.transform);
        visual.AddComponent<SpriteRenderer>(); // User can swap sprite later

        string bulletPath = weaponFolderPath + "/StandardBullet.prefab";
        bulletPath = AssetDatabase.GenerateUniqueAssetPath(bulletPath);
        
        GameObject bulletPrefab = PrefabUtility.SaveAsPrefabAsset(bulletGO, bulletPath);
        DestroyImmediate(bulletGO);
        Debug.Log($"Created Bullet Prefab at: {bulletPath}");

        // 3. Create AssaultRifle Prefab
        GameObject rifleGO = new GameObject("AssaultRifle_Auto");
        var rifleScript = rifleGO.AddComponent<AssaultRifle>();

        GameObject muzzleGO = new GameObject("Muzzle");
        muzzleGO.transform.SetParent(rifleGO.transform);
        muzzleGO.transform.localPosition = new Vector3(0.5f, 0, 0); // Offset slightly

        // Assign References via SerializedObject
        SerializedObject so = new SerializedObject(rifleScript);
        SerializedProperty bulletProp = so.FindProperty("_bulletBase");
        SerializedProperty muzzleProp = so.FindProperty("_InitBulletPos");
        SerializedProperty cooldownProp = so.FindProperty("_coolDownTime");
        SerializedProperty speedProp = so.FindProperty("_moveSpeed");
        SerializedProperty damageProp = so.FindProperty("_weaponDamage");

        if (bulletProp != null) bulletProp.objectReferenceValue = bulletPrefab.GetComponent<StandardBullet>();
        if (muzzleProp != null) muzzleProp.objectReferenceValue = muzzleGO.transform;
        if (cooldownProp != null) cooldownProp.floatValue = 0.1f;
        if (speedProp != null) speedProp.floatValue = 10f;
        if (damageProp != null) damageProp.floatValue = 10f;

        so.ApplyModifiedProperties();

        string riflePath = weaponFolderPath + "/AssaultRifle.prefab";
        riflePath = AssetDatabase.GenerateUniqueAssetPath(riflePath);

        PrefabUtility.SaveAsPrefabAsset(rifleGO, riflePath);
        DestroyImmediate(rifleGO);
        Debug.Log($"Created Weapon Prefab at: {riflePath}");

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/HackathonDream/Setup Shotgun")]
    public static void SetupShotgun()
    {
        string weaponFolderPath = EnsureWeaponFolder();

        // reuse StandardBullet if possible or create new one specific for shotgun
        GameObject bulletPrefab = CreateBulletPrefab<StandardBullet>("ShotgunPellet", weaponFolderPath, 0.15f);

        // Create Shotgun
        GameObject weaponGO = new GameObject("Shotgun_Auto");
        var weaponScript = weaponGO.AddComponent<Shotgun>();

        GameObject muzzleGO = new GameObject("Muzzle");
        muzzleGO.transform.SetParent(weaponGO.transform);
        muzzleGO.transform.localPosition = new Vector3(0.5f, 0, 0);

        SerializedObject so = new SerializedObject(weaponScript);
        SetCommonWeaponProperties(so, bulletPrefab.GetComponent<WeaponBulletBase>(), muzzleGO.transform, 0.8f, 10f, 8f);
        
        // Shotgun specific
        SerializedProperty pelletProp = so.FindProperty("_pelletCount");
        SerializedProperty spreadProp = so.FindProperty("_spreadAngle");
        if (pelletProp != null) pelletProp.intValue = 5;
        if (spreadProp != null) spreadProp.floatValue = 25f;

        so.ApplyModifiedProperties();

        string path = AssetDatabase.GenerateUniqueAssetPath(weaponFolderPath + "/Shotgun.prefab");
        PrefabUtility.SaveAsPrefabAsset(weaponGO, path);
        DestroyImmediate(weaponGO);
        Debug.Log($"Created Shotgun Prefab at: {path}");
        
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/HackathonDream/Setup SniperRifle")]
    public static void SetupSniperRifle()
    {
        string weaponFolderPath = EnsureWeaponFolder();

        // Create PiercingBullet
        GameObject bulletPrefab = CreateBulletPrefab<PiercingBullet>("PiercingBullet", weaponFolderPath, 0.15f);

        // Create SniperRifle
        GameObject weaponGO = new GameObject("SniperRifle_Auto");
        var weaponScript = weaponGO.AddComponent<SniperRifle>();

        GameObject muzzleGO = new GameObject("Muzzle");
        muzzleGO.transform.SetParent(weaponGO.transform);
        muzzleGO.transform.localPosition = new Vector3(1.0f, 0, 0); // Longer barrel

        SerializedObject so = new SerializedObject(weaponScript);
        // High speed, high damage, long cooldown
        SetCommonWeaponProperties(so, bulletPrefab.GetComponent<WeaponBulletBase>(), muzzleGO.transform, 1.5f, 25f, 50f);
        
        so.ApplyModifiedProperties();

        string path = AssetDatabase.GenerateUniqueAssetPath(weaponFolderPath + "/SniperRifle.prefab");
        PrefabUtility.SaveAsPrefabAsset(weaponGO, path);
        DestroyImmediate(weaponGO);
        Debug.Log($"Created SniperRifle Prefab at: {path}");
        
        AssetDatabase.Refresh();
    }

    private static string EnsureWeaponFolder()
    {
        string prefabFolderPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabFolderPath)) AssetDatabase.CreateFolder("Assets", "Prefabs");
        string weaponFolderPath = prefabFolderPath + "/Weapons";
        if (!AssetDatabase.IsValidFolder(weaponFolderPath)) AssetDatabase.CreateFolder(prefabFolderPath, "Weapons");
        return weaponFolderPath;
    }

    private static GameObject CreateBulletPrefab<T>(string name, string folderPath, float radius) where T : Component
    {
        GameObject bulletGO = new GameObject(name);
        bulletGO.AddComponent<T>();
        if (bulletGO.GetComponent<HitChceker>() == null) bulletGO.AddComponent<HitChceker>();
        
        var col = bulletGO.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = radius;

        var rb = bulletGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(bulletGO.transform);
        visual.AddComponent<SpriteRenderer>();

        string path = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{name}.prefab");
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(bulletGO, path);
        DestroyImmediate(bulletGO);
        return prefab;
    }

    private static void SetCommonWeaponProperties(SerializedObject so, WeaponBulletBase bullet, Transform muzzle, float coolDown, float speed, float damage)
    {
        SerializedProperty bulletProp = so.FindProperty("_bulletBase");
        SerializedProperty muzzleProp = so.FindProperty("_InitBulletPos");
        SerializedProperty cooldownProp = so.FindProperty("_coolDownTime");
        SerializedProperty speedProp = so.FindProperty("_moveSpeed");
        SerializedProperty damageProp = so.FindProperty("_weaponDamage");

        if (bulletProp != null) bulletProp.objectReferenceValue = bullet;
        if (muzzleProp != null) muzzleProp.objectReferenceValue = muzzle;
        if (cooldownProp != null) cooldownProp.floatValue = coolDown;
        if (speedProp != null) speedProp.floatValue = speed;
        if (damageProp != null) damageProp.floatValue = damage;
    }
}
