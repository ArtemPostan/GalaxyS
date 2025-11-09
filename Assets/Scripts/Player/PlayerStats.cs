public class PlayerStats
{

    public int score = 0;
    public float health = 200;
    public float energy = 100;
    public float damage = 25;
    public float shield = 0;
    public float shieldEnergy = 100;
    public float movementSpeed = 8;
    public float originMovementSpeed = 8;
    public float shootingSpeed;
    public float dropChance = 0.1f;
    public float minRotAngle = -20f;
    public float maxRotAngle = 20f;

    public int currentLevel = 1;
    public int scoreInCurrentLevel = 0;

    public int fireGunLvl = 0;
    public int iceGunLvl = 0;
    public int lightningGunLvl = 0;
    public int shieldLvl = 0;
    public int laserGunLvl = 0;

    public float maxTiltAngle = 45f;

    public int maxHealth = 200;

    public float originShootingSpeed = 0.2f;

    public void Initialize()
    {
        // ”бедитесь, что все стартовые значени€ установлены
        shootingSpeed = originShootingSpeed;
        health = maxHealth;
    }

    public void ResetScore()
    {
        scoreInCurrentLevel = 0;        
    }

}
